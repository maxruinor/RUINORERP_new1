using RUINORERP.UI.Network.Services;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 业务服务类使用示例
    /// 展示如何使用登录服务、缓存同步服务和消息提示服务
    /// </summary>
    public class ServiceUsageExample
    {
        private readonly UserLoginService _loginService;
        private readonly CacheSyncService _cacheSyncService;
        private readonly MessageNotificationService _messageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceUsageExample(
            UserLoginService loginService,
            CacheSyncService cacheSyncService,
            MessageNotificationService messageService)
        {
            _loginService = loginService;
            _cacheSyncService = cacheSyncService;
            _messageService = messageService;

            // 订阅消息事件
            _messageService.PopupMessageReceived += OnPopupMessageReceived;
            _messageService.SystemNotificationReceived += OnSystemNotificationReceived;
            _messageService.BroadcastMessageReceived += OnBroadcastMessageReceived;
        }

        /// <summary>
        /// 处理弹窗消息
        /// </summary>
        private void OnPopupMessageReceived(string sender, string content)
        {
            Console.WriteLine($"[弹窗消息] 来自 {sender}: {content}");
        }

        /// <summary>
        /// 处理系统通知
        /// </summary>
        private void OnSystemNotificationReceived(string title, string content)
        {
            Console.WriteLine($"[系统通知] {title}: {content}");
        }

        /// <summary>
        /// 处理广播消息
        /// </summary>
        private void OnBroadcastMessageReceived(string sender, string content)
        {
            Console.WriteLine($"[广播消息] 来自 {sender}: {content}");
        }

        /// <summary>
        /// 演示完整的业务流程
        /// </summary>
        public async Task DemonstrateBusinessFlowAsync()
        {
            Console.WriteLine("=== 开始演示业务流程 ===");

            // 1. 用户登录
            Console.WriteLine("1. 执行用户登录...");
            var loginResponse = await _loginService.LoginAsync("testuser", "password123");
            if (loginResponse.IsSuccess())
            {
                Console.WriteLine($"   登录成功，会话ID: {loginResponse.Data.SessionId}");
                
                // 2. 缓存数据同步
                Console.WriteLine("2. 执行缓存数据同步...");
                var syncResponse = await _cacheSyncService.FullSyncAsync();
                if (syncResponse.IsSuccess())
                {
                    Console.WriteLine("   缓存数据同步成功");
                }
                else
                {
                    Console.WriteLine($"   缓存数据同步失败: {syncResponse.Message}");
                }

                // 3. 请求特定缓存数据
                Console.WriteLine("3. 请求用户配置缓存...");
                var cacheResponse = await _cacheSyncService.RequestCacheDataAsync("user_config");
                if (cacheResponse.IsSuccess())
                {
                    Console.WriteLine("   缓存数据获取成功");
                }
                else
                {
                    Console.WriteLine($"   缓存数据获取失败: {cacheResponse.Message}");
                }

                // 4. 发送消息
                Console.WriteLine("4. 发送测试消息...");
                var messageResponse = await _messageService.SendPopupMessageAsync(
                    "这是一条测试消息", "admin");
                if (messageResponse.IsSuccess())
                {
                    Console.WriteLine("   消息发送成功");
                }
                else
                {
                    Console.WriteLine($"   消息发送失败: {messageResponse.Message}");
                }

                // 5. 发送系统通知
                Console.WriteLine("5. 发送系统通知...");
                var notificationResponse = await _messageService.SendSystemNotificationAsync(
                    "系统维护通知", "系统将在今晚12点进行维护");
                if (notificationResponse.IsSuccess())
                {
                    Console.WriteLine("   系统通知发送成功");
                }
                else
                {
                    Console.WriteLine($"   系统通知发送失败: {notificationResponse.Message}");
                }
            }
            else
            {
                Console.WriteLine($"   登录失败: {loginResponse.Message}");
            }

            Console.WriteLine("=== 业务流程演示结束 ===");
        }

        /// <summary>
        /// 演示缓存操作
        /// </summary>
        public async Task DemonstrateCacheOperationsAsync()
        {
            Console.WriteLine("=== 开始演示缓存操作 ===");

            // 同步特定数据
            Console.WriteLine("1. 同步用户权限数据...");
            var syncResult = await _cacheSyncService.SyncCacheDataAsync(
                "user_permissions", new { UserId = "123", Permissions = new[] { "read", "write" } });
            Console.WriteLine(syncResult.IsSuccess() ? "   数据同步成功" : $"   数据同步失败: {syncResult.Message}");

            // 使缓存失效
            Console.WriteLine("2. 使用户配置缓存失效...");
            var invalidateResult = await _cacheSyncService.InvalidateCacheAsync("user_config");
            Console.WriteLine(invalidateResult.IsSuccess() ? "   缓存失效成功" : $"   缓存失效失败: {invalidateResult.Message}");

            // 查询同步状态
            Console.WriteLine("3. 查询数据同步状态...");
            var statusResponse = await _cacheSyncService.GetSyncStatusAsync();
            if (statusResponse.IsSuccess())
            {
                Console.WriteLine("   同步状态查询成功");
            }
            else
            {
                Console.WriteLine($"   同步状态查询失败: {statusResponse.Message}");
            }

            Console.WriteLine("=== 缓存操作演示结束 ===");
        }

        /// <summary>
        /// 演示消息操作
        /// </summary>
        public async Task DemonstrateMessageOperationsAsync()
        {
            Console.WriteLine("=== 开始演示消息操作 ===");

            // 发送广播消息
            Console.WriteLine("1. 发送广播消息...");
            var broadcastResult = await _messageService.BroadcastMessageAsync("系统广播消息");
            Console.WriteLine(broadcastResult.IsSuccess() ? "   广播消息发送成功" : $"   广播消息发送失败: {broadcastResult.Message}");

            // 发送用户消息
            Console.WriteLine("2. 发送用户消息...");
            var userMessageResult = await _messageService.SendMessageToUserAsync(
                "这是一条用户消息", "targetUser123");
            Console.WriteLine(userMessageResult.IsSuccess() ? "   用户消息发送成功" : $"   用户消息发送失败: {userMessageResult.Message}");

            Console.WriteLine("=== 消息操作演示结束 ===");
        }
    }
}