using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NetTaste;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerService;
using RUINORERP.Server.ServerSession;
using RUINORERP.Services;
using RUINORERP.WF.BizOperation.Condition;
using SharpYaml.Tokens;
using StackExchange.Redis;
using SuperSocket.Server;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 统一用户服务实现 - 整合SuperSocket和Network版本的用户服务功能
    /// 提供用户认证、管理、缓存同步、消息推送等完整功能
    /// 暂时注释
    /// </summary>
    public class UnifiedUserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICacheService _cacheService;
        private readonly ISecurityService _securityService;
        private readonly ILogger<UnifiedUserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionManager _sessionManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository">用户仓储接口</param>
        /// <param name="roleRepository">角色仓储接口</param>
        /// <param name="cacheService">缓存服务接口</param>
        /// <param name="securityService">安全服务接口</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="configuration">配置接口</param>
        /// <param name="sessionManager">会话管理接口</param>
        public UnifiedUserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICacheService cacheService,
            ISecurityService securityService,
            ILogger<UnifiedUserService> logger,
            IConfiguration configuration,
            ISessionManager sessionManager)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        }

        #region 用户认证和管理功能 (来自Network版本)

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>认证结果</returns>
        public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
        {
            try
            {
                _logger.LogDebug($"Attempting authentication for user: {username}");

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Username and password are required"
                    };
                }

                var user = await GetUserByUsernameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning($"Authentication failed: User not found - {username}");
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning($"Authentication failed: User account is inactive - {username}");
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "User account is inactive"
                    };
                }

                if (user.IsLocked)
                {
                    _logger.LogWarning($"Authentication failed: User account is locked - {username}");
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "User account is locked"
                    };
                }

                if (!VerifyPassword(password, user.PasswordHash, user.Salt))
                {
                    await HandleFailedLoginAttemptAsync(user);
                    _logger.LogWarning($"Authentication failed: Invalid password - {username}");
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                await HandleSuccessfulLoginAsync(user);

                var token = await GenerateAuthTokenAsync(user);
                var userRoles = await GetUserRolesAsync(user.Id);
                var permissions = await GetUserPermissionsAsync(user.Id);

                _logger.LogInformation($"User authenticated successfully: {username}");

                return new AuthenticationResult
                {
                    Success = true,
                    Message = "Authentication successful",
                    User = user,
                    Token = token,
                    Roles = userRoles,
                    Permissions = permissions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during authentication for user: {username}");
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "Authentication failed due to system error"
                };
            }
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        public async Task<User> GetUserByIdAsync(string userId)
        {
            var cacheKey = $"user:id:{userId}";
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(15));
            }

            return user;
        }

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户信息</returns>
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var cacheKey = $"user:username:{username.ToLower()}";
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(15));
            }

            return user;
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="request">创建用户请求</param>
        /// <returns>创建的用户信息</returns>
        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation($"Creating new user: {request.Username}");

                await ValidateCreateUserRequestAsync(request);

                var salt = GenerateSalt();
                var passwordHash = HashPassword(request.Password, salt);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    IsActive = true,
                    IsLocked = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,
                    FailedLoginAttempts = 0,
                    MustChangePassword = request.MustChangePassword
                };

                var createdUser = await _userRepository.CreateAsync(user);

                if (request.RoleIds != null && request.RoleIds.Any())
                {
                    await AssignRolesToUserAsync(createdUser.Id, request.RoleIds);
                }

                await InvalidateUserCacheAsync(createdUser.Username);

                _logger.LogInformation($"User created successfully: {request.Username}");
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user: {request?.Username}");
                throw;
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="request">更新用户请求</param>
        /// <returns>更新后的用户信息</returns>
        public async Task<User> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            try
            {
                _logger.LogInformation($"Updating user: {userId}");

                var existingUser = await _userRepository.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    throw new ArgumentException($"User not found: {userId}");
                }

                // 更新用户信息
                existingUser.Email = request.Email ?? existingUser.Email;
                existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
                existingUser.LastName = request.LastName ?? existingUser.LastName;
                existingUser.IsActive = request.IsActive ?? existingUser.IsActive;
                existingUser.IsLocked = request.IsLocked ?? existingUser.IsLocked;
                existingUser.ModifiedDate = DateTime.UtcNow;
                existingUser.ModifiedBy = request.ModifiedBy;

                var updatedUser = await _userRepository.UpdateAsync(existingUser);

                // 更新角色分配
                if (request.RoleIds != null)
                {
                    await UpdateUserRolesAsync(userId, request.RoleIds);
                }

                await InvalidateUserCacheAsync(updatedUser.Username);

                _logger.LogInformation($"User updated successfully: {userId}");
                return updatedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>删除是否成功</returns>
        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation($"Deleting user: {userId}");

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for deletion: {userId}");
                    return false;
                }

                // 检查是否为系统管理员用户
                if (user.IsSystemAdmin)
                {
                    _logger.LogWarning($"Cannot delete system admin user: {userId}");
                    return false;
                }

                // 删除用户角色关联
                await RemoveAllRolesFromUserAsync(userId);

                // 删除用户
                var result = await _userRepository.DeleteAsync(userId);

                if (result)
                {
                    await InvalidateUserCacheAsync(user.Username);
                    _logger.LogInformation($"User deleted successfully: {userId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>用户分页列表</returns>
        public async Task<PagedResult<User>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var cacheKey = $"users:all:{pageNumber}:{pageSize}";
                var cachedResult = await _cacheService.GetAsync<PagedResult<User>>(cacheKey);

                if (cachedResult != null)
                {
                    return cachedResult;
                }

                var result = await _userRepository.GetAllAsync(pageNumber, pageSize);

                if (result != null)
                {
                    await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="searchTerm">搜索关键词</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>搜索结果</returns>
        public async Task<PagedResult<User>> SearchUsersAsync(string searchTerm, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllUsersAsync(pageNumber, pageSize);
                }

                var cacheKey = $"users:search:{searchTerm}:{pageNumber}:{pageSize}";
                var cachedResult = await _cacheService.GetAsync<PagedResult<User>>(cacheKey);

                if (cachedResult != null)
                {
                    return cachedResult;
                }

                var result = await _userRepository.SearchAsync(searchTerm, pageNumber, pageSize);

                if (result != null)
                {
                    await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(2));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching users with term: {searchTerm}");
                throw;
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="resetBy">重置操作人</param>
        /// <returns>重置是否成功</returns>
        public async Task<bool> ResetPasswordAsync(string userId, string newPassword, string resetBy)
        {
            try
            {
                _logger.LogInformation($"Resetting password for user: {userId}");

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"User not found: {userId}");
                }

                var salt = GenerateSalt();
                var passwordHash = HashPassword(newPassword, salt);

                user.PasswordHash = passwordHash;
                user.Salt = salt;
                user.MustChangePassword = true;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = resetBy;

                var result = await _userRepository.UpdateAsync(user);

                if (result != null)
                {
                    await InvalidateUserCacheAsync(user.Username);
                    _logger.LogInformation($"Password reset successfully for user: {userId}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting password for user: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 锁定用户账户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="lockedBy">锁定操作人</param>
        /// <param name="reason">锁定原因</param>
        /// <returns>锁定是否成功</returns>
        public async Task<bool> LockUserAsync(string userId, string lockedBy, string reason = null)
        {
            try
            {
                _logger.LogInformation($"Locking user account: {userId}");

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"User not found: {userId}");
                }

                user.IsLocked = true;
                user.LockReason = reason;
                user.LockedDate = DateTime.UtcNow;
                user.LockedBy = lockedBy;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = lockedBy;

                var result = await _userRepository.UpdateAsync(user);

                if (result != null)
                {
                    await InvalidateUserCacheAsync(user.Username);
                    _logger.LogInformation($"User account locked successfully: {userId}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error locking user account: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 解锁用户账户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="unlockedBy">解锁操作人</param>
        /// <returns>解锁是否成功</returns>
        public async Task<bool> UnlockUserAsync(string userId, string unlockedBy)
        {
            try
            {
                _logger.LogInformation($"Unlocking user account: {userId}");

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"User not found: {userId}");
                }

                user.IsLocked = false;
                user.LockReason = null;
                user.LockedDate = null;
                user.LockedBy = null;
                user.FailedLoginAttempts = 0;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = unlockedBy;

                var result = await _userRepository.UpdateAsync(user);

                if (result != null)
                {
                    await InvalidateUserCacheAsync(user.Username);
                    _logger.LogInformation($"User account unlocked successfully: {userId}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error unlocking user account: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>角色列表</returns>
        public async Task<List<Role>> GetUserRolesAsync(string userId)
        {
            try
            {
                var cacheKey = $"user:roles:{userId}";
                var cachedRoles = await _cacheService.GetAsync<List<Role>>(cacheKey);

                if (cachedRoles != null)
                {
                    return cachedRoles;
                }

                var roles = await _roleRepository.GetRolesByUserIdAsync(userId);

                if (roles != null)
                {
                    await _cacheService.SetAsync(cacheKey, roles, TimeSpan.FromMinutes(30));
                }

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting roles for user: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限列表</returns>
        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            try
            {
                var cacheKey = $"user:permissions:{userId}";
                var cachedPermissions = await _cacheService.GetAsync<List<string>>(cacheKey);

                if (cachedPermissions != null)
                {
                    return cachedPermissions;
                }

                var roles = await GetUserRolesAsync(userId);
                var permissions = new List<string>();

                foreach (var role in roles)
                {
                    if (role.Permissions != null)
                    {
                        permissions.AddRange(role.Permissions);
                    }
                }

                // 去重
                permissions = permissions.Distinct().ToList();

                await _cacheService.SetAsync(cacheKey, permissions, TimeSpan.FromMinutes(30));

                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting permissions for user: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 验证用户是否拥有指定权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permission">权限标识</param>
        /// <returns>是否拥有权限</returns>
        public async Task<bool> HasPermissionAsync(string userId, string permission)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(permission))
                {
                    return false;
                }

                var permissions = await GetUserPermissionsAsync(userId);
                return permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking permission for user: {userId}, permission: {permission}");
                return false;
            }
        }

        /// <summary>
        /// 验证用户是否拥有指定角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否拥有角色</returns>
        public async Task<bool> HasRoleAsync(string userId, string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
                {
                    return false;
                }

                var roles = await GetUserRolesAsync(userId);
                return roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking role for user: {userId}, role: {roleName}");
                return false;
            }
        }

        #endregion

        #region 缓存管理和消息推送功能 (来自SuperSocket版本)

        /// <summary>
        /// 发送缓存数据列表到客户端
        /// </summary>
        /// <param name="playerSession">客户端会话</param>
        /// <param name="tableName">表名</param>
        public async Task SendCacheDataListAsync(SessionforBiz playerSession, string tableName)
        {
            try
            {
                if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    // 发送缓存数据
                    var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (cacheList == null)
                    {
                        // 启动时服务器都没有加载缓存，则不发送
                        BizCacheHelper.Instance.SetDictDataSource(tableName, true);
                        await Task.Delay(500);
                        cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    }

                    // 上面查询可能还是没有立即加载成功
                    if (cacheList == null)
                    {
                        return;
                    }

                    if (cacheList is JArray)
                    {
                        // 暂时认为服务器的都是泛型形式保存的
                    }

                    if (TypeHelper.IsGenericList(cacheList.GetType()))
                    {
                        var lastList = ((IEnumerable<dynamic>)cacheList).ToList();
                        if (lastList != null)
                        {
                            int pageSize = 100; // 每页100行
                            for (int i = 0; i < lastList.Count; i += pageSize)
                            {
                                // 计算当前页的结束索引，确保不会超出数组界限
                                int endIndex = Math.Min(i + pageSize, lastList.Count);

                                // 获取当前页的JArray片段
                                object page = lastList.Skip(i).Take(endIndex - i).ToArray();

                                // 处理当前页
                                await SendCacheDataListAsync(playerSession, tableName, page);

                                // 如果当前页是最后一页，可能不足200行，需要特殊处理
                                if (endIndex == lastList.Count)
                                {
                                    // 处理最后一页的逻辑，如果需要的话
                                    // 发送完成！
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintMsg($"{tableName}最后一页发送完成,总行数:{endIndex}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
        }

        /// <summary>
        /// 发送缓存数据列表到客户端（分页版本）
        /// </summary>
        /// <param name="playerSession">客户端会话</param>
        /// <param name="tableName">表名</param>
        /// <param name="list">数据列表</param>
        private async Task SendCacheDataListAsync(SessionforBiz playerSession, string tableName, object list)
        {
            try
            {
                string json = JsonConvert.SerializeObject(list,
                      new JsonSerializerSettings
                      {
                          Converters = new List<JsonConverter> { new CustomCollectionJsonConverter() },
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                          Formatting = Formatting.None
                      });
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(tableName);
                tx.PushString(json);
                await playerSession.AddSendDataAsync((byte)ServerCommand.发送缓存数据列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据:" + ex.Message);
            }
        }

        /// <summary>
        /// 接收更新缓存指令
        /// </summary>
        /// <param name="userSession">用户会话</param>
        /// <param name="gd">原始数据</param>
        public async Task ReceiveUpdateCacheCommandAsync(SessionforBiz userSession, OriginalData gd)
        {
            try
            {
                int index = 0;
                string time = ByteDataAnalysis.GetString(gd.Two, ref index);
                string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                
                // 更新服务器的缓存
                JObject obj = JObject.Parse(json);
                MyCacheManager.Instance.UpdateEntityList(tableName, obj);
                
                // 再转发给其他客户端
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(time);
                tx.PushString(tableName);
                tx.PushString(json);

                foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                {
                    // 排除更新者自己
                    if (item.Key == userSession.SessionID)
                    {
                        continue;
                    }
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    await sessionforBiz.AddSendDataAsync((byte)ServerCommand.转发更新缓存, null, tx.toByte());
                    
                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintMsg($"转发更新的缓存{tableName}给：" + item.Value.User.姓名);
                    }
                }

                // 如果是产品表有变化，还需要更新产品视图的缓存
                if (tableName == nameof(tb_Prod))
                {
                    var prod = obj.ToObject<tb_Prod>();
                    await BroadcastProdCacheDataAsync(userSession, prod);
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收更新缓存指令:" + ex.Message);
            }
        }

        /// <summary>
        /// 广播产品缓存数据
        /// </summary>
        /// <param name="userSession">用户会话</param>
        /// <param name="prod">产品信息</param>
        private async Task BroadcastProdCacheDataAsync(SessionforBiz userSession, tb_Prod prod)
        {
            try
            {
                View_ProdDetail viewProdDetail = new View_ProdDetail();
                viewProdDetail = await Program.AppContextData.Db.CopyNew().Queryable<View_ProdDetail>()
                    .SingleAsync(p => p.ProdBaseID == prod.ProdBaseID);
                MyCacheManager.Instance.UpdateEntityList<View_ProdDetail>(viewProdDetail);
                
                // 发送缓存数据
                string json = JsonConvert.SerializeObject(viewProdDetail,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

                string tableName = nameof(View_ProdDetail);
                ByteBuff tx = new ByteBuff(200);
                string sendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendTime);
                tx.PushString(tableName);
                tx.PushString(json);

                foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                {
                    // 排除更新者自己
                    if (item.Key == userSession.SessionID)
                    {
                        continue;
                    }
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    await sessionforBiz.AddSendDataAsync((byte)ServerCommand.转发更新缓存, null, tx.toByte());

                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintMsg($"转发更新缓存{tableName}给：" + item.Value.User.姓名);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BroadcastProdCacheDataAsync: {ex.Message}");
            }
        }

        /// <summary>
        /// 接收用户登录指令
        /// </summary>
        /// <param name="userSession">用户会话</param>
        /// <param name="gd">原始数据</param>
        /// <returns>用户信息</returns>
        public async Task<tb_UserInfo> ReceiveUserLoginCommandAsync(SessionforBiz userSession, OriginalData gd)
        {
            tb_UserInfo user = null;
            try
            {
                int index = 0;
                string loginTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                var userName = ByteDataAnalysis.GetString(gd.Two, ref index);
                var password = ByteDataAnalysis.GetString(gd.Two, ref index);

                string msg = string.Empty;

                user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                    .Where(u => u.UserName == userName && u.Password == password)
                    .Includes(x => x.tb_employee)
                    .Includes(x => x.tb_User_Roles)
                    .SingleAsync();
                
                if (user != null)
                {
                    // 登录成功
                    userSession.User.用户名 = user.UserName;
                    if (user.tb_employee != null)
                    {
                        userSession.User.姓名 = user.tb_employee.Employee_Name;
                        userSession.User.Employee_ID = user.Employee_ID.Value;
                    }
                    
                    // 登录时间
                    userSession.User.登陆时间 = DateTime.Now;
                    userSession.User.UserID = user.User_ID;
                    userSession.User.超级用户 = user.IsSuperUser;
                    userSession.User.在线状态 = true;
                    userSession.User.授权状态 = true;
                    
                    // 通知客户端
                    await SendPromptMessageToClientAsync(userSession, "用户登录成功！");
                    
                    // 记录登录日志
                    await LogUserLoginAsync(user.User_ID, userSession.RemoteEndPoint?.ToString(), true, "登录成功");
                    
                    return user;
                }
                else
                {
                    // 登录失败
                    await SendPromptMessageToClientAsync(userSession, "用户名或密码错误！");
                    
                    // 记录登录失败日志
                    await LogUserLoginAsync(null, userSession.RemoteEndPoint?.ToString(), false, "用户名或密码错误");
                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收用户登录指令:" + ex.Message);
                await SendPromptMessageToClientAsync(userSession, "系统错误，请联系管理员！");
                return null;
            }
        }

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="reason">下线原因</param>
        /// <param name="forceBy">强制下线操作人</param>
        /// <returns>是否成功</returns>
        public async Task<bool> ForceUserOfflineAsync(string userId, string reason, string forceBy)
        {
            try
            {
                _logger.LogInformation($"Forcing user offline: {userId}, reason: {reason}");

                // 查找用户的会话
                var session = frmMain.Instance.sessionListBiz.Values
                    .Cast<SessionforBiz>()
                    .FirstOrDefault(s => s.User.UserID == userId);

                if (session != null)
                {
                    // 发送强制下线消息
                    await SendForceOfflineMessageAsync(session, reason, forceBy);

                    // 关闭会话
                    await session.CloseAsync(CloseReason.WindowsShutDown);

                    // 记录日志
                    await LogForceOfflineAsync(userId, reason, forceBy);

                    _logger.LogInformation($"User forced offline successfully: {userId}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"User session not found for force offline: {userId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error forcing user offline: {userId}");
                return false;
            }
        }

        /// <summary>
        /// 发送强制下线消息
        /// </summary>
        /// <param name="session">用户会话</param>
        /// <param name="reason">下线原因</param>
        /// <param name="forceBy">操作人</param>
        private async Task SendForceOfflineMessageAsync(SessionforBiz session, string reason, string forceBy)
        {
            try
            {
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(reason);
                tx.PushString(forceBy);
                tx.PushString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                await session.AddSendDataAsync((byte)ServerCommand.强制用户下线, null, tx.toByte());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending force offline message to user: {session.User?.UserID}");
            }
        }

        /// <summary>
        /// 发送提示消息到客户端
        /// </summary>
        /// <param name="session">用户会话</param>
        /// <param name="message">消息内容</param>
        public async Task SendPromptMessageToClientAsync(SessionforBiz session, string message)
        {
            try
            {
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(message);
                await session.AddSendDataAsync((byte)ServerCommand.发送提示消息, null, tx.toByte());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending prompt message to client: {session.User?.UserID}");
            }
        }

        /// <summary>
        /// 发送在线用户列表
        /// </summary>
        /// <param name="session">目标会话</param>
        public async Task SendOnlineUserListAsync(SessionforBiz session)
        {
            try
            {
                var onlineUsers = frmMain.Instance.sessionListBiz.Values
                    .Cast<SessionforBiz>()
                    .Where(s => s.User.在线状态)
                    .Select(s => new
                    {
                        UserID = s.User.UserID,
                        UserName = s.User.用户名,
                        RealName = s.User.姓名,
                        LoginTime = s.User.登陆时间,
                        IPAddress = s.RemoteEndPoint?.ToString()
                    })
                    .ToList();

                string json = JsonConvert.SerializeObject(onlineUsers,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                ByteBuff tx = new ByteBuff(200);
                tx.PushString(json);
                await session.AddSendDataAsync((byte)ServerCommand.发送在线用户列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending online user list");
            }
        }

        /// <summary>
        /// 推送版本更新消息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="updateUrl">更新地址</param>
        /// <param name="description">更新描述</param>
        public async Task PushVersionUpdateAsync(string version, string updateUrl, string description)
        {
            try
            {
                _logger.LogInformation($"Pushing version update: {version}");

                ByteBuff tx = new ByteBuff(200);
                tx.PushString(version);
                tx.PushString(updateUrl);
                tx.PushString(description);

                foreach (var session in frmMain.Instance.sessionListBiz.Values.Cast<SessionforBiz>())
                {
                    await session.AddSendDataAsync((byte)ServerCommand.推送版本更新, null, tx.toByte());
                }

                _logger.LogInformation($"Version update pushed successfully: {version}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error pushing version update: {version}");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <param name="storedHash">存储的哈希值</param>
        /// <param name="salt">盐值</param>
        /// <returns>是否验证通过</returns>
        private bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hashedPassword = HashPassword(password, salt);
            return hashedPassword == storedHash;
        }

        /// <summary>
        /// 哈希密码
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <param name="salt">盐值</param>
        /// <returns>哈希后的密码</returns>
        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// 生成盐值
        /// </summary>
        /// <returns>盐值字符串</returns>
        private string GenerateSalt()
        {
            var randomBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// 生成认证令牌
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>认证令牌</returns>
        private async Task<string> GenerateAuthTokenAsync(User user)
        {
            return await _securityService.GenerateJwtTokenAsync(user);
        }

        /// <summary>
        /// 处理登录失败尝试
        /// </summary>
        /// <param name="user">用户信息</param>
        private async Task HandleFailedLoginAttemptAsync(User user)
        {
            user.FailedLoginAttempts++;

            // 如果失败次数达到阈值，锁定账户
            var maxAttempts = _configuration.GetValue<int>("Security:MaxLoginAttempts", 5);
            if (user.FailedLoginAttempts >= maxAttempts)
            {
                user.IsLocked = true;
                user.LockedDate = DateTime.UtcNow;
                user.LockReason = "Too many failed login attempts";
            }

            await _userRepository.UpdateAsync(user);
            await InvalidateUserCacheAsync(user.Username);
        }

        /// <summary>
        /// 处理成功登录
        /// </summary>
        /// <param name="user">用户信息</param>
        private async Task HandleSuccessfulLoginAsync(User user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            user.FailedLoginAttempts = 0;
            user.IsLocked = false;
            user.LockedDate = null;
            user.LockReason = null;

            await _userRepository.UpdateAsync(user);
            await InvalidateUserCacheAsync(user.Username);
        }

        /// <summary>
        /// 验证创建用户请求
        /// </summary>
        /// <param name="request">创建用户请求</param>
        private async Task ValidateCreateUserRequestAsync(CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ArgumentException("Username is required");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Password is required");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required");
            }

            // 检查用户名是否已存在
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new ArgumentException($"Username '{request.Username}' already exists");
            }

            // 检查邮箱是否已存在
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new ArgumentException($"Email '{request.Email}' already exists");
            }
        }

        /// <summary>
        /// 分配角色给用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        private async Task AssignRolesToUserAsync(string userId, IEnumerable<string> roleIds)
        {
            await _userRepository.AssignRolesAsync(userId, roleIds);
            await InvalidateUserRolesCacheAsync(userId);
        }

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        private async Task UpdateUserRolesAsync(string userId, IEnumerable<string> roleIds)
        {
            await _userRepository.UpdateUserRolesAsync(userId, roleIds);
            await InvalidateUserRolesCacheAsync(userId);
        }

        /// <summary>
        /// 移除用户的所有角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        private async Task RemoveAllRolesFromUserAsync(string userId)
        {
            await _userRepository.RemoveAllRolesAsync(userId);
            await InvalidateUserRolesCacheAsync(userId);
        }

        /// <summary>
        /// 使用户缓存失效
        /// </summary>
        /// <param name="username">用户名</param>
        private async Task InvalidateUserCacheAsync(string username)
        {
            var cacheKeys = new[]
            {
                $"user:username:{username.ToLower()}",
                $"users:all:*",
                $"users:search:*"
            };

            await _cacheService.RemoveByPatternAsync(cacheKeys);
        }

        /// <summary>
        /// 使用户角色缓存失效
        /// </summary>
        /// <param name="userId">用户ID</param>
        private async Task InvalidateUserRolesCacheAsync(string userId)
        {
            var cacheKey = $"user:roles:{userId}";
            await _cacheService.RemoveAsync(cacheKey);
        }

        /// <summary>
        /// 记录用户登录日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="success">是否成功</param>
        /// <param name="message">日志消息</param>
        private async Task LogUserLoginAsync(string userId, string ipAddress, bool success, string message)
        {
            try
            {
                var loginLog = new UserLoginLog
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    LoginTime = DateTime.UtcNow,
                    IpAddress = ipAddress,
                    Success = success,
                    Message = message,
                    UserAgent = "SuperSocket Client"
                };

                await _userRepository.LogLoginAsync(loginLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging user login for user: {userId}");
            }
        }

        /// <summary>
        /// 记录强制下线日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="reason">下线原因</param>
        /// <param name="forceBy">操作人</param>
        private async Task LogForceOfflineAsync(string userId, string reason, string forceBy)
        {
            try
            {
                var log = new UserForceOfflineLog
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ForceTime = DateTime.UtcNow,
                    Reason = reason,
                    ForceBy = forceBy
                };

                await _userRepository.LogForceOfflineAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging force offline for user: {userId}");
            }
        }

        #endregion
    }

    /// <summary>
    /// 认证结果
    /// </summary>
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public List<Role> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }

    /// <summary>
    /// 创建用户请求
    /// </summary>
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool MustChangePassword { get; set; }
        public string CreatedBy { get; set; }
        public List<string> RoleIds { get; set; }
    }

    /// <summary>
    /// 更新用户请求
    /// </summary>
    public class UpdateUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public string ModifiedBy { get; set; }
        public List<string> RoleIds { get; set; }
    }

    /// <summary>
    /// 用户登录日志
    /// </summary>
    public class UserLoginLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserAgent { get; set; }
    }

    /// <summary>
    /// 用户强制下线日志
    /// </summary>
    public class UserForceOfflineLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime ForceTime { get; set; }
        public string Reason { get; set; }
        public string ForceBy { get; set; }
    }
}