# Token认证机制重构与重连优化方案

## 问题分析

### 1. 当前系统存在的问题

1. **重连后认证状态丢失**：当服务器重启后，客户端成功重连，但没有自动恢复认证状态，导致用户需要重新登录。

2. **Token刷新机制存在缺陷**：
   - SilentTokenRefresher中调用RefreshTokenAsync时传入了空字符串作为accessToken
   - Token验证和刷新逻辑分散在多个服务中，职责不够清晰

3. **Token管理代码冗余复杂**：
   - Token相关操作分散在UserLoginService、TokenRefreshService、SilentTokenRefresher等多个类中
   - 缺少统一的Token生命周期管理

4. **缺少重连后的认证恢复机制**：
   - ClientCommunicationService的重连逻辑中没有包含认证状态恢复步骤
   - 心跳包中没有包含用户认证信息

## 解决方案

### 1. 第一次登录认证时用户信息保持

- 登录成功后，将用户信息和Token保存在本地TokenStorage中
- 心跳包中添加用户会话信息，确保服务器能够识别已认证的用户
- 服务器端在心跳处理时验证用户会话有效性

### 2. 短时间掉线重连后的Token刷新与认证

- 在ClientCommunicationService中实现重连成功后的认证恢复机制
- 重连成功后自动检查本地Token是否有效
- 如果Token有效，向服务器发送Token验证请求恢复认证状态
- 如果Token即将过期，自动触发Token刷新

### 3. Token认证与刷新机制重构

- 简化Token刷新流程，移除冗余代码
- 统一Token管理，集中处理Token的存储、验证和刷新
- 改进SilentTokenRefresher，确保正确传递accessToken
- 添加统一的Token过期处理策略

## 实施计划

### 第一阶段：Token管理核心优化

1. 改进TokenRefreshService，修复Token刷新逻辑
2. 优化SilentTokenRefresher，确保正确传入accessToken
3. 加强TokenManager的功能，提供更完整的Token生命周期管理

### 第二阶段：重连认证恢复机制实现

1. 在ClientCommunicationService中添加重连后认证恢复逻辑
2. 修改TryReconnectAsync方法，添加认证状态恢复步骤
3. 在心跳包中添加用户认证信息

### 第三阶段：服务器端配合修改

1. 改进LoginCommandHandler，支持基于Token的快速重认证
2. 优化HeartbeatCommandHandler，验证心跳中的会话信息

## 详细实现代码

### 1. TokenRefreshService改进

```csharp
public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
{
    // 从TokenManager获取当前Token
    var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
    if (currentToken == null || string.IsNullOrEmpty(currentToken.AccessToken))
    {
        throw new InvalidOperationException("没有可用的Token进行刷新");
    }
    
    try
    {
        // 创建刷新Token请求
        SimpleRequest request = SimpleRequest.CreateString(currentToken.AccessToken);

        // 发送请求并获取响应
        var response = await _communicationService.SendCommandAsync(AuthenticationCommands.RefreshToken, request, ct);

        // 从响应中提取Token信息
        if (response?.ExecutionContext?.Token != null)
        {
            // 自动更新存储的Token
            await _tokenManager.TokenStorage.SetTokenAsync(response.ExecutionContext.Token);
            return response.ExecutionContext.Token;
        }

        return null;
    }
    catch (Exception ex)
    {
        throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
    }
}
```

### 2. SilentTokenRefresher优化

```csharp
private async Task<bool> RefreshTokenInternalAsync(CancellationToken cancellationToken)
{
    await _refreshLock.WaitAsync(cancellationToken);
    try
    {
        TokenInfo tokenInfo = null;
        int retryCount = 0;
        Exception lastException = null;

        while (retryCount < MAX_RETRY_COUNT)
        {
            try
            {
                // 调用Token刷新服务，不再需要传入accessToken
                tokenInfo = await _tokenRefreshService.RefreshTokenAsync(cancellationToken);
                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
                {
                    OnRefreshSucceeded(tokenInfo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                retryCount++;
                if (retryCount < MAX_RETRY_COUNT)
                {
                    await Task.Delay(RETRY_DELAY_MS, cancellationToken);
                }
            }
        }

        if (lastException != null)
        {
            OnRefreshFailed(lastException);
        }
        return false;
    }
    finally
    {
        _refreshLock.Release();
    }
}
```

### 3. ClientCommunicationService重连认证恢复

```csharp
private async Task<bool> TryReconnectAsync()
{
    if (!_networkConfig.AutoReconnect || _disposed || string.IsNullOrEmpty(_serverAddress))
        return false;

    _logger.Debug("开始尝试重连服务器...");

    for (int attempt = 0; attempt < _networkConfig.MaxReconnectAttempts; attempt++)
    {
        if (_disposed)
            break;

        _logger.Debug($"重连尝试 {attempt + 1}/{_networkConfig.MaxReconnectAttempts}");

        try
        {
            // 使用连接锁确保不会并发重连
            await _connectionLock.WaitAsync();
            try
            {
                // 检查是否已经连接
                if (_isConnected)
                {
                    _logger.Debug("检测到已连接，取消重连尝试");
                    return true;
                }

                if (await _socketClient.ConnectAsync(_serverAddress, _serverPort, CancellationToken.None).ConfigureAwait(false))
                {
                    lock (_syncLock)
                    {
                        _heartbeatFailureCount = 0;
                        // 使用统一的连接状态更新方法
                        UpdateConnectionState(true);
                    }

                    _logger.Debug("服务器重连成功");
                    
                    // 重连成功后尝试恢复认证状态
                    await RestoreAuthenticationStateAsync();
                    
                    return true;
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "重连失败");
            _eventManager.OnErrorOccurred(new Exception($"重连尝试 {attempt + 1} 失败: {ex.Message}", ex));
        }

        // 等待重连延迟
        if (attempt < _networkConfig.MaxReconnectAttempts - 1)
        {
            _logger.Debug($"等待 {_networkConfig.ReconnectDelay.TotalSeconds} 秒后进行下一次重连");
            await Task.Delay(_networkConfig.ReconnectDelay, CancellationToken.None).ConfigureAwait(false);
        }
    }

    _logger.LogError("达到最大重连尝试次数，重连失败");
    _eventManager.OnErrorOccurred(new Exception("重连服务器失败: 达到最大尝试次数"));

    // 触发重连失败事件，通知UI层进行注销锁定处理
    _eventManager.OnReconnectFailed();

    return false;
}

/// <summary>
/// 重连成功后恢复认证状态
/// </summary>
private async Task RestoreAuthenticationStateAsync()
{
    try
    {
        // 获取TokenManager实例（需要通过依赖注入或其他方式获取）
        var tokenManager = _serviceProvider.GetService<TokenManager>();
        if (tokenManager == null)
        {
            _logger.LogWarning("无法获取TokenManager实例，跳过认证状态恢复");
            return;
        }

        // 检查是否有保存的Token
        var tokenInfo = await tokenManager.TokenStorage.GetTokenAsync();
        if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
        {
            _logger.Debug("检测到已保存的Token，尝试恢复认证状态");
            
            // 验证Token是否仍然有效
            var validateRequest = SimpleRequest.CreateString(tokenInfo.AccessToken);
            var response = await SendCommandAsync(AuthenticationCommands.ValidateToken, validateRequest, CancellationToken.None);
            
            if (response != null && response.IsSuccess)
            {
                _logger.Debug("Token验证成功，认证状态已恢复");
                
                // 恢复会话ID
                if (!string.IsNullOrEmpty(MainForm.Instance?.AppContext?.SessionId))
                {
                    _logger.Debug("会话ID已恢复");
                }
                
                // 启动心跳
                await StartHeartbeatAfterLoginAsync(CancellationToken.None);
                return;
            }
            else if (IsTokenExpired(tokenInfo))
            {
                _logger.Debug("Token已过期，尝试刷新Token");
                
                // 尝试刷新Token
                var refreshService = _serviceProvider.GetService<ITokenRefreshService>();
                if (refreshService != null)
                {
                    try
                    {
                        var newToken = await refreshService.RefreshTokenAsync(CancellationToken.None);
                        if (newToken != null)
                        {
                            _logger.Debug("Token刷新成功，认证状态已恢复");
                            await StartHeartbeatAfterLoginAsync(CancellationToken.None);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Token刷新失败");
                    }
                }
            }
        }
        
        _logger.Debug("没有有效的Token或Token验证失败，无法自动恢复认证状态");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "恢复认证状态时发生异常");
    }
}

/// <summary>
/// 检查Token是否即将过期
/// </summary>
private bool IsTokenExpired(TokenInfo tokenInfo)
{
    // 提前5分钟视为过期，以便有足够时间刷新
    return tokenInfo.ExpiresAt <= DateTime.UtcNow.AddMinutes(5);
}
```

### 4. 心跳包中添加用户认证信息

```csharp
private async Task StartHeartbeatAfterLoginAsync(CancellationToken cancellationToken)
{
    try
    {
        if (_heartbeatManager == null)
        {
            _heartbeatManager = new HeartbeatManager(_communicationService, _networkConfig.HeartbeatInterval, _networkConfig.MaxHeartbeatFailures);
            _heartbeatManager.HeartbeatFailed += OnHeartbeatFailed;
        }
        
        // 启动心跳，并传入用户会话信息
        await _heartbeatManager.StartAsync(cancellationToken);
        _logger.Debug("心跳已启动");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "启动心跳失败");
        throw;
    }
}
```

## 代码优化建议

### 1. Token管理优化

- 将Token管理相关功能集中到TokenManager中，减少代码重复
- 实现Token自动刷新机制，在Token即将过期时主动刷新
- 添加Token过期事件通知，方便应用程序做出相应处理

### 2. 认证流程优化

- 实现基于Token的无状态认证，提高系统可扩展性
- 优化重连后的认证恢复流程，减少不必要的网络请求
- 添加认证状态变更事件，方便UI层响应认证状态变化

### 3. 错误处理优化

- 统一认证相关错误处理，提供更友好的错误提示
- 实现细粒度的错误日志记录，便于问题排查
- 添加错误重试策略，提高系统稳定性

## 总结

通过以上重构和优化，可以有效解决重连后认证状态丢失的问题，同时简化Token管理相关代码，提高系统的可维护性和用户体验。重构后的系统将能够在服务器短暂离线后，自动重连并恢复认证状态，无需用户重新登录。