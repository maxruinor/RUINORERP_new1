using Microsoft.Extensions.DependencyInjection;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息服务使用示例
    /// 展示如何使用MessageService和SimplifiedMessageService
    /// </summary>
    public class MessageServiceUsageExample
    {
        private readonly MessageService _messageService;
        private readonly SimplifiedMessageService _simplifiedMessageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public MessageServiceUsageExample(IServiceProvider serviceProvider)
        {
            // 从依赖注入容器获取消息服务
            _messageService = serviceProvider.GetService<MessageService>();
            _simplifiedMessageService = serviceProvider.GetService<SimplifiedMessageService>();

            // 订阅消息事件
            SubscribeToMessages();
        }

        /// <summary>
        /// 订阅消息事件
        /// </summary>
        private void SubscribeToMessages()
        {
            if (_messageService != null)
            {
                // 订阅各种消息事件
                _messageService.PopupMessageReceived += OnPopupMessageReceived;
                _messageService.UserMessageReceived += OnUserMessageReceived;
                _messageService.DepartmentMessageReceived += OnDepartmentMessageReceived;
                _messageService.BroadcastMessageReceived += OnBroadcastMessageReceived;
                _messageService.SystemNotificationReceived += OnSystemNotificationReceived;
            }

            if (_simplifiedMessageService != null)
            {
                // 订阅简化版消息服务的事件
                _simplifiedMessageService.PopupMessageReceived += OnPopupMessageReceived;
                _simplifiedMessageService.UserMessageReceived += OnUserMessageReceived;
                _simplifiedMessageService.DepartmentMessageReceived += OnDepartmentMessageReceived;
                _simplifiedMessageService.BroadcastMessageReceived += OnBroadcastMessageReceived;
                _simplifiedMessageService.SystemNotificationReceived += OnSystemNotificationReceived;
            }
        }

        /// <summary>
        /// 处理弹窗消息
        /// </summary>
        private void OnPopupMessageReceived(MessageReceivedEventArgs args)
        {
            // 处理接收到的弹窗消息
            Console.WriteLine($"接收到弹窗消息: {args.Data}");
        }

        /// <summary>
        /// 处理用户消息
        /// </summary>
        private void OnUserMessageReceived(MessageReceivedEventArgs args)
        {
            // 处理接收到的用户消息
            Console.WriteLine($"接收到用户消息: {args.Data}");
        }

        /// <summary>
        /// 处理部门消息
        /// </summary>
        private void OnDepartmentMessageReceived(MessageReceivedEventArgs args)
        {
            // 处理接收到的部门消息
            Console.WriteLine($"接收到部门消息: {args.Data}");
        }

        /// <summary>
        /// 处理广播消息
        /// </summary>
        private void OnBroadcastMessageReceived(MessageReceivedEventArgs args)
        {
            // 处理接收到的广播消息
            Console.WriteLine($"接收到广播消息: {args.Data}");
        }

        /// <summary>
        /// 处理系统通知
        /// </summary>
        private void OnSystemNotificationReceived(MessageReceivedEventArgs args)
        {
            // 处理接收到的系统通知
            Console.WriteLine($"接收到系统通知: {args.Data}");
        }

        /// <summary>
        /// 使用完整版消息服务发送各种类型的消息
        /// </summary>
        public async Task UseFullMessageServiceAsync()
        {
            if (_messageService == null)
            {
                Console.WriteLine("消息服务未初始化");
                return;
            }

            try
            {
                // 发送弹窗消息给指定用户
                var popupResponse = await _messageService.SendPopupMessageAsync(
                    "user123", 
                    "这是一条测试弹窗消息", 
                    "测试标题");

                if (popupResponse.IsSuccess)
                {
                    Console.WriteLine("弹窗消息发送成功");
                }
                else
                {
                    Console.WriteLine($"弹窗消息发送失败: {popupResponse.ErrorMessage}");
                }

                // 发送用户消息
                var userResponse = await _messageService.SendMessageToUserAsync(
                    "user456", 
                    "这是一条测试用户消息");

                if (userResponse.IsSuccess)
                {
                    Console.WriteLine("用户消息发送成功");
                }
                else
                {
                    Console.WriteLine($"用户消息发送失败: {userResponse.ErrorMessage}");
                }

                // 发送部门消息
                var departmentResponse = await _messageService.SendMessageToDepartmentAsync(
                    "dept789", 
                    "这是一条测试部门消息");

                if (departmentResponse.IsSuccess)
                {
                    Console.WriteLine("部门消息发送成功");
                }
                else
                {
                    Console.WriteLine($"部门消息发送失败: {departmentResponse.ErrorMessage}");
                }

                // 广播消息
                var broadcastResponse = await _messageService.BroadcastMessageAsync(
                    "这是一条测试广播消息");

                if (broadcastResponse.IsSuccess)
                {
                    Console.WriteLine("广播消息发送成功");
                }
                else
                {
                    Console.WriteLine($"广播消息发送失败: {broadcastResponse.ErrorMessage}");
                }

                // 发送系统通知
                var notificationResponse = await _messageService.SendSystemNotificationAsync(
                    "这是一条测试系统通知");

                if (notificationResponse.IsSuccess)
                {
                    Console.WriteLine("系统通知发送成功");
                }
                else
                {
                    Console.WriteLine($"系统通知发送失败: {notificationResponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送消息时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 使用简化版消息服务发送消息
        /// </summary>
        public async Task UseSimplifiedMessageServiceAsync()
        {
            if (_simplifiedMessageService == null)
            {
                Console.WriteLine("简化版消息服务未初始化");
                return;
            }

            try
            {
                // 使用简化方法发送文本消息给用户
                var result1 = await _simplifiedMessageService.SendTextMessageToUserAsync(
                    "user123", 
                    "这是一条简化的文本消息");

                Console.WriteLine($"发送文本消息结果: {result1}");

                // 使用简化方法发送弹窗消息
                var result2 = await _simplifiedMessageService.SendPopupMessageToUserAsync(
                    "user456", 
                    "这是一条简化的弹窗消息", 
                    "简化标题");

                Console.WriteLine($"发送弹窗消息结果: {result2}");

                // 使用简化方法发送部门消息
                var result3 = await _simplifiedMessageService.SendMessageToDepartmentAsync(
                    "dept789", 
                    "这是一条简化的部门消息");

                Console.WriteLine($"发送部门消息结果: {result3}");

                // 使用简化方法广播消息
                var result4 = await _simplifiedMessageService.BroadcastMessageAsync(
                    "这是一条简化的广播消息");

                Console.WriteLine($"广播消息结果: {result4}");

                // 使用简化方法发送系统通知
                var result5 = await _simplifiedMessageService.SendSystemNotificationAsync(
                    "这是一条简化的系统通知");

                Console.WriteLine($"发送系统通知结果: {result5}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送简化消息时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 演示消息服务的完整使用流程
        /// </summary>
        public async Task DemonstrateMessageServiceUsageAsync()
        {
            Console.WriteLine("开始演示消息服务使用...");

            // 使用完整版消息服务
            Console.WriteLine("\n--- 使用完整版消息服务 ---");
            await UseFullMessageServiceAsync();

            // 使用简化版消息服务
            Console.WriteLine("\n--- 使用简化版消息服务 ---");
            await UseSimplifiedMessageServiceAsync();

            Console.WriteLine("\n消息服务使用演示完成");
        }
    }
}