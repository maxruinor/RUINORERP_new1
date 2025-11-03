using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息显示组件基类
    /// 提供统一的消息显示和处理功能
    /// </summary>
    public abstract class BaseMessagePrompt : KryptonForm
    {
        /// <summary>
        /// 消息数据
        /// </summary>
        public MessageData MessageData { get; set; }
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger<BaseMessagePrompt> Logger;
        
        /// <summary>
        /// 消息管理器
        /// </summary>
        protected readonly EnhancedMessageManager MessageManager;
        
        /// <summary>
        /// 菜单权限助手
        /// </summary>
        protected MenuPowerHelper MenuPowerHelper;
        
        /// <summary>
        /// 初始化组件
        /// 子类必须实现此方法来初始化UI组件
        /// </summary>
        protected abstract void InitializeComponents();
        
        /// <summary>
        /// 更新消息显示
        /// 子类必须实现此方法来根据消息数据更新UI
        /// </summary>
        protected abstract void UpdateMessageDisplay();
        
        /// <summary>
        /// 显示消息
        /// 通用的消息显示方法
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public virtual void ShowMessage(MessageData messageData)
        {
            try
            {
                MessageData = messageData;
                
                // 更新消息为已读状态
                if (!messageData.IsRead)
                {
                    messageData.MarkAsRead();
                }
                
                UpdateMessageDisplay();
                
                // 设置窗口位置和显示
                PositionForm();
                ShowMessageForm();
                
                Logger.LogDebug($"显示消息: {messageData.Title}, 类型: {messageData.MessageType}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "显示消息时发生错误");
            }
        }
        
        /// <summary>
        /// 设置发送者文本
        /// </summary>
        /// <param name="text">发送者名称</param>
        public abstract void SetSenderText(string text);
        
        /// <summary>
        /// 设置主题文本
        /// </summary>
        /// <param name="text">消息主题</param>
        public abstract void SetSubjectText(string text);
        
        /// <summary>
        /// 定位窗体
        /// 默认定位到屏幕右下角
        /// 子类可以重写此方法自定义位置
        /// </summary>
        protected virtual void PositionForm()
        {
            this.StartPosition = FormStartPosition.Manual;
            
            // 设置窗体的初始位置为屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );
        }
        
        /// <summary>
        /// 显示消息窗体
        /// 子类可以重写此方法自定义显示逻辑
        /// </summary>
        protected virtual void ShowMessageForm()
        {
            // 如果已经显示，则闪烁窗口获取用户注意
            if (this.Visible)
            {
                this.Focus();
                this.FlashWindow();
            }
            else
            {
                this.Show();
                this.BringToFront();
            }
        }
        
        /// <summary>
        /// 闪烁窗口
        /// 获取用户注意力
        /// </summary>
        protected void FlashWindow()
        {
            // 实现窗口闪烁逻辑
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
        }
        
        /// <summary>
        /// 处理确认操作
        /// </summary>
        /// <param name="status">确认状态</param>
        protected virtual async Task HandleConfirmationAsync(ConfirmStatus status)
        {
            try
            {
                if (MessageData != null)
                {
                    MessageData.ConfirmStatus = status;
                    MessageData.ConfirmTime = DateTime.Now;
                    
                    // 使用消息管理器标记消息状态变更
                    if (MessageManager != null)
                    {
                        // 这里只是更新内存中的消息状态，实际的数据持久化应该在MessageManager中处理
                        await Task.Run(() =>
                        {
                            // 标记为已读
                            MessageManager.MarkAsRead(MessageData.Id);
                            Logger.LogDebug($"已更新消息状态: {MessageData.Id}, 确认状态: {status}");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理消息确认时发生错误");
            }
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseMessagePrompt()
        {
            // 获取服务实例
            MenuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            Logger = Startup.GetFromFac<ILogger<BaseMessagePrompt>>();
            MessageManager = Startup.GetFromFac<EnhancedMessageManager>();
            
            // 初始化组件
            InitializeComponents();
        }
        
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageManager">消息管理器</param>
        protected BaseMessagePrompt(MessageData messageData, ILogger<BaseMessagePrompt> logger = null, EnhancedMessageManager messageManager = null)
            : this()
        {
            MessageData = messageData;
            Logger = logger ?? Logger;
            MessageManager = messageManager ?? MessageManager;
            
            // 如果提供了消息数据，则更新显示
            if (messageData != null)
            {
                UpdateMessageDisplay();
            }
        }
    }
}