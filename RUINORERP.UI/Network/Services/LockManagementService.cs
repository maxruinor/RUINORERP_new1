using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network;
using RUINORERP.PacketSpec.Models.Lock;
using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Responses;
using MySqlX.XDevAPI;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 锁定管理服务类，提供单据锁定、解锁、检查锁状态和请求解锁等功能
    /// 实现IDisposable接口以支持资源的释放
    /// </summary>
    public class LockManagementService : IDisposable
    {
        /// <summary>
        /// 客户端通信服务，用于发送锁定相关的命令到服务器
        /// </summary>
        private readonly ClientCommunicationService _clientCommunicationService;

        /// <summary>
        /// 日志记录器，用于记录服务操作的日志信息
        /// </summary>
        private readonly ILogger<LockManagementService> _logger;

        /// <summary>
        /// 表示对象是否已被释放
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 初始化LockManagementService类的新实例
        /// </summary>
        /// <param name="clientCommunicationService">客户端通信服务实例</param>
        /// <param name="logger">日志记录器实例，可选</param>
        public LockManagementService(
            ClientCommunicationService clientCommunicationService,
            ILogger<LockManagementService> logger = null)
        {
            _clientCommunicationService = clientCommunicationService ?? throw new ArgumentNullException(nameof(clientCommunicationService));
            _logger = logger;

            _logger?.LogDebug("LockManagementService 初始化完成");
        }

        /// <summary>
        /// 析构函数，确保在对象被垃圾回收时释放资源
        /// </summary>
        ~LockManagementService()
        {
            // 不要在析构函数中调用虚方法，因为对象的派生类可能已经被垃圾回收器终结
            Dispose(false);
        }

        /// <summary>
        /// 锁定单据
        /// </summary>
        /// <param name="lockRequest">锁定请求信息</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>锁定操作的响应结果</returns>
        /// <exception cref="ArgumentNullException">当lockRequest为null时抛出</exception>
        public async Task<LockResponse> LockAsync(LockRequest lockRequest, CancellationToken ct = default)
        {
            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            try
            {
                // 发送锁定命令到服务器
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(LockCommands.Lock, lockRequest, ct);
                if (response.IsSuccess)
                {
                    _logger?.LogDebug("单据锁定成功 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                }
                else
                {
                    _logger?.LogWarning("单据锁定失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                        lockRequest.LockInfo.BillID, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送锁定单据请求时发生异常 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                throw;
            }
        }
        /// <summary>
        /// 锁定单据（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>锁定操作的响应结果</returns>
        public async Task<LockResponse> LockAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建锁定请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                };

                lockRequest.LockInfo.SetLockKey();

                _logger?.LogDebug("开始锁定单据 - 单据ID: {BillId}, 菜单ID: {MenuId}, 用户ID: {UserId}, 用户名称: {UserName}",
                    billId, menuId, currentUserId, currentUserName);

                // 调用现有方法执行锁定操作
                return await LockAsync(lockRequest, ct);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送锁定单据请求时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }
        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="lockRequest">解锁请求信息</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>解锁操作的响应结果</returns>
        /// <exception cref="ArgumentNullException">当lockRequest为null时抛出</exception>
        public async Task<LockResponse> UnlockBillAsync(LockRequest lockRequest, CancellationToken ct = default)
        {
            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            _logger?.LogDebug("开始解锁单据 - 单据ID: {BillId}, 解锁类型: {UnlockType}, 用户ID: {UserId}",
                lockRequest.LockInfo.BillID, lockRequest.UnlockType, lockRequest.LockedUserName);

            try
            {
                // 发送解锁命令到服务器
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(LockCommands.RequestUnlock
                    , lockRequest, ct);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("单据解锁成功 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                }
                else
                {
                    _logger?.LogWarning("单据解锁失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                        lockRequest.LockInfo.BillID, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送解锁单据请求时发生异常 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                throw;
            }
        }

        /// <summary>
        /// 解锁单据（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>解锁操作的响应结果</returns>
        public async Task<LockResponse> UnlockBillAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    UnlockType = UnlockType.Normal,
                    LockedUserName = currentUserName,
                };
                lockRequest.LockInfo.SetLockKey();

                _logger?.LogDebug("开始解锁单据 - 单据ID: {BillId}, 菜单ID: {MenuId}, 用户ID: {UserId}, 用户名称: {UserName}",
                    billId, menuId, currentUserId, currentUserName);

                // 调用现有方法执行解锁操作
                return await UnlockBillAsync(lockRequest, ct);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送解锁单据请求时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 检查单据锁定状态（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>锁状态信息</returns>
        public async Task<LockResponse> CheckLockStatusAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建检查锁状态请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                };
                lockRequest.LockInfo.SetLockKey();

                _logger?.LogDebug("开始检查单据锁定状态 - 单据ID: {BillId}, 菜单ID: {MenuId}, 用户ID: {UserId}, 用户名称: {UserName}",
                    billId, menuId, currentUserId, currentUserName);

                // 调用现有方法执行检查锁状态操作
                var result = await CheckLockStatusAsync(lockRequest, ct);
                
                _logger?.LogDebug("检查单据锁定状态完成 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查单据锁定状态时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 管理员强制解锁单据
        /// </summary>
        /// <param name="lockRequest">锁定请求信息</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>解锁操作的响应结果</returns>
        /// <exception cref="ArgumentNullException">当lockRequest为null时抛出</exception>
        public async Task<LockResponse> ForceUnlockBillAsync(LockRequest lockRequest, CancellationToken ct = default)
        {
            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            // 设置解锁类型为强制解锁
            lockRequest.UnlockType = UnlockType.Force;
            
            _logger?.LogWarning("管理员开始强制解锁单据 - 单据ID: {BillId}, 管理员: {AdminName}",
                lockRequest.LockInfo.BillID, lockRequest.LockedUserName);

            try
            {
                // 发送强制解锁命令到服务器
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(LockCommands.RequestUnlock,
                    lockRequest, ct);

                if (response.IsSuccess)
                {
                    _logger?.LogWarning("管理员强制解锁单据成功 - 单据ID: {BillId}, 管理员: {AdminName}",
                        lockRequest.LockInfo.BillID, lockRequest.LockedUserName);
                }
                else
                {
                    _logger?.LogError("管理员强制解锁单据失败 - 单据ID: {BillId}, 管理员: {AdminName}, 错误信息: {ErrorMessage}",
                        lockRequest.LockInfo.BillID, lockRequest.LockedUserName, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送强制解锁单据请求时发生异常 - 单据ID: {BillId}, 管理员: {AdminName}",
                    lockRequest.LockInfo.BillID, lockRequest.LockedUserName);
                throw;
            }
        }

        /// <summary>
        /// 管理员强制解锁单据（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>解锁操作的响应结果</returns>
        public async Task<LockResponse> ForceUnlockBillAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前管理员信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建强制解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    UnlockType = UnlockType.Force,
                    LockedUserName = currentUserName,
                };
                lockRequest.LockInfo.SetLockKey();

                _logger?.LogWarning("管理员开始强制解锁单据 - 单据ID: {BillId}, 菜单ID: {MenuId}, 管理员ID: {AdminId}, 管理员名称: {AdminName}",
                    billId, menuId, currentUserId, currentUserName);

                // 调用强制解锁方法
                return await ForceUnlockBillAsync(lockRequest, ct);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送强制解锁单据请求时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 检查单据锁定状态（重载方法）
        /// </summary>
        /// <param name="lockRequest">锁定请求信息</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>锁状态信息</returns>
        public async Task<LockResponse> CheckLockStatusAsync(LockRequest lockRequest, CancellationToken ct = default)
        {
            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            try
            {
                // 确保锁键已设置
                if (string.IsNullOrEmpty(lockRequest.LockInfo.LockKey))
                {
                    lockRequest.LockInfo.SetLockKey();
                }

                // 发送检查锁状态命令到服务器
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(LockCommands.CheckLockStatus, lockRequest, ct);
 
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查单据锁定状态时发生异常 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                throw;
            }
        }

       

        /// <summary>
        /// 请求解锁（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="RequestUserName">锁定者用户名</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>请求解锁操作的响应结果</returns>
        public async Task<LockResponse> RequestUnlockAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建请求解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserName = currentUserName,
                    //LockedUserName = RequestUserName,
                };
                lockRequest.LockInfo.SetLockKey();
 

                // 创建带参数的请求解锁方法
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.RequestUnlock, lockRequest, ct);
                
                if (response.IsSuccess)
                {
                    _logger?.LogDebug("请求解锁发送成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                }
                else
                {
                    _logger?.LogWarning("请求解锁发送失败 - 单据ID: {BillId}, 菜单ID: {MenuId}, 错误信息: {ErrorMessage}",
                        billId, menuId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送请求解锁时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 拒绝解锁（重载方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="requesterUserName">请求者用户名</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>拒绝解锁操作的响应结果</returns>
        public async Task<LockResponse> RefuseUnlockAsync(long billId, long menuId, string requesterUserName, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建拒绝解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserName = requesterUserName,
                    LockedUserName = currentUserName,
                };
                lockRequest.LockInfo.SetLockKey();

                _logger?.LogDebug("开始拒绝解锁单据 - 单据ID: {BillId}, 菜单ID: {MenuId}, 请求者: {RequesterUserName}, 拒绝者: {LockedUserName}",
                    billId, menuId, requesterUserName, currentUserName);

                // 调用现有RefuseUnlockAsync方法执行拒绝解锁操作
                var response = await RefuseUnlockAsync(lockRequest, ct);
                
                if (response.IsSuccess)
                {
                    _logger?.LogDebug("拒绝解锁操作成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                }
                else
                {
                    _logger?.LogWarning("拒绝解锁操作失败 - 单据ID: {BillId}, 菜单ID: {MenuId}, 错误信息: {ErrorMessage}",
                        billId, menuId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送拒绝解锁时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 拒绝解锁
        /// </summary>
        /// <param name="lockRequest">请求解锁的信息</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>请求解锁操作的响应结果</returns>
        /// <exception cref="ArgumentNullException">当lockRequest为null时抛出</exception>
        public async Task<LockResponse> RefuseUnlockAsync(LockRequest lockRequest, CancellationToken ct = default)
        {


            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            _logger?.LogDebug("开始发送解锁请求 - 单据ID: {BillId}, 请求者: {RequesterUserName}, 锁定者: {LockedUserName}",
                lockRequest.LockInfo.BillID, lockRequest.RequesterUserName, lockRequest.LockedUserName);

            try
            {
                // 发送请求解锁命令到服务器
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(LockCommands.RefuseUnlock
                    , lockRequest, ct);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("请求解锁发送成功 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                }
                else
                {
                    _logger?.LogWarning("请求解锁发送失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                        lockRequest.LockInfo.BillID, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送请求解锁时发生异常 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                throw;
            }
        }

        /// <summary>
        /// 处理解锁请求
        /// </summary>
        /// <param name="lockRequest">请求解锁的信息</param>
        /// <returns>处理解锁请求的响应结果</returns>
        public Task<LockResponse> HandleUnlockRequestAsync(LockRequest lockRequest)
        {
            // 这里实现处理其他用户发送的解锁请求的逻辑
            // 通常会显示通知给当前锁定者，并根据用户的选择执行相应操作
            throw new NotImplementedException("HandleUnlockRequestAsync 方法尚未实现");
        }

        /// <summary>
        /// 释放由LockManagementService占用的非托管资源，以及可选的托管资源
        /// </summary>
        /// <param name="disposing">如果为true，则释放托管资源和非托管资源；如果为false，则仅释放非托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    _logger?.LogDebug("LockManagementService 释放托管资源");
                    // 在这里添加需要释放的托管资源
                }

                // 释放非托管资源
                _logger?.LogDebug("LockManagementService 释放非托管资源");

                _disposed = true;
            }
        }

        /// <summary>
        /// 释放由LockManagementService使用的所有资源
        /// </summary>
        /// <summary>
        /// 刷新单据锁定状态
        /// 该方法通过向服务器发送请求来更新锁定的过期时间，确保当前用户能够持续编辑单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>刷新操作的响应结果</returns>
        public async Task<LockResponse> RefreshLockAsync(long billId, long menuId, CancellationToken ct = default)
        {
            try
            {
                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.UserId = currentUserId;
                lockInfo.UserName = currentUserName;

                // 创建刷新锁定请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RefreshMode = true, // 标记为刷新模式
                };
                lockRequest.LockInfo.SetLockKey();

                _logger?.LogDebug("开始刷新单据锁定 - 单据ID: {BillId}, 菜单ID: {MenuId}, 用户ID: {UserId}, 用户名称: {UserName}",
                    billId, menuId, currentUserId, currentUserName);

                // 使用CheckLockStatus命令来刷新锁定
                // 注：在服务器端实现中，CheckLockStatus命令可能会自动刷新当前用户持有的锁
                var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.CheckLockStatus, lockRequest, ct);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("单据锁定刷新成功 - 单据ID: {BillId}", billId);
                }
                else
                {
                    _logger?.LogWarning("单据锁定刷新失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                        billId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "刷新单据锁定时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // 通知垃圾回收器不再为这个对象调用析构函数
            GC.SuppressFinalize(this);
        }
    }
}
