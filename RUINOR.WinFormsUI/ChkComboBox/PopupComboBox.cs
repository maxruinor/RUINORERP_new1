using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Threading;

namespace RUINOR.WinFormsUI.ChkComboBox
{
    /// <summary>
    /// CodeProject.com "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp".
    /// Represents a Windows combo box control with a custom popup control attached.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Displays an editable text box with a drop-down list of permitted values.")]
    public partial class PopupComboBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl.PopupComboBox" /> class.
        /// </summary>
        public PopupComboBox()
        {
            InitializeComponent();
            base.DropDownHeight = base.DropDownWidth = 1;
            base.IntegralHeight = false;
        }

        /// <summary>
        /// The pop-up wrapper for the dropDownControl. 
        /// Made PROTECTED instead of PRIVATE so descendent classes can set its Resizable property.
        /// Note however the pop-up properties must be set after the dropDownControl is assigned, since this 
        /// popup wrapper is recreated when the dropDownControl is assigned.
        /// </summary>
        protected Popup dropDown;

        private Control dropDownControl;
        /// <summary>
        /// Gets or sets the drop down control.
        /// </summary>
        /// <value>The drop down control.</value>
        public Control DropDownControl
        {
            get
            {
                return dropDownControl;
            }
            set
            {
                if (dropDownControl == value)
                    return;
                dropDownControl = value;
                dropDown = new Popup(value);
            }
        }

        // 创建下拉控件
        private void CreateDropDown()
        {
            if (DropDownControl != null && !DropDownControl.IsDisposed)
            {
                // 创建新的Popup实例，并设置适当的属性
                dropDown = new Popup(DropDownControl);
                
                // 设置下拉控件的样式和行为
                dropDown.UseFadeEffect = false; // 禁用淡入淡出效果，提高响应速度
                dropDown.FocusOnOpen = true;    // 打开时聚焦，提升用户体验
                
                // 确保下拉控件的尺寸合理
                dropDown.Width = this.Width;
                dropDown.Height = Math.Min(300, this.Height * 5); // 合理的默认高度
            }
        }
        
        /// <summary>
        /// Shows the drop down.
        /// 优化下拉显示逻辑，提高响应速度和可靠性
        /// </summary>
        public void ShowDropDown()
        {
            // 快速路径：如果下拉已经可见，则不执行任何操作
            if (dropDown != null && dropDown.Visible)
            {
                return;
            }
            
            // 验证下拉控件是否已创建，如果未创建则立即创建
            if (dropDown == null && DropDownControl != null)
            {
                CreateDropDown();
            }
            
            // 安全检查 - 确保控件已创建且未被释放
            if (dropDown == null || !IsHandleCreated || IsDisposed)
            {
                return;
            }
            
            try
            {
                // 更新下拉控件位置并立即显示
                // 设置下拉控件的尺寸，确保其与父控件宽度匹配
                dropDown.Width = this.Width;
                
                // 如果有预设的高度，则使用预设高度
                if (dropDown.Height < 100) // 确保高度合理，避免过小
                {
                    dropDown.Height = 200; // 设置一个合理的默认高度
                }
                
                // 直接显示下拉控件，不添加额外延迟
                dropDown.Show(this);
            }
            catch (Exception ex)
            {
                // 在实际应用中，可能需要记录异常日志
                System.Diagnostics.Debug.WriteLine($"显示下拉失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 在后台线程中执行操作，并在UI线程中执行回调
        /// 用于处理耗时操作，避免阻塞UI线程
        /// </summary>
        /// <param name="operation">要在后台执行的操作</param>
        /// <param name="callback">操作完成后在UI线程执行的回调</param>
        protected void ExecuteAsyncOperation(Func<Task> operation, Action callback = null)
        {
            // 使用Task在后台线程执行操作
            Task.Run(async () =>
            {
                try
                {
                    await operation();
                }
                catch (Exception ex)
                {
                    // 在实际应用中，可能需要记录异常日志
                    System.Diagnostics.Debug.WriteLine($"异步操作出错: {ex.Message}");
                }
                finally
                {
                    // 操作完成后，如果有回调，则在UI线程执行
                    if (callback != null && IsHandleCreated)
                    {
                        BeginInvoke(callback);
                    }
                }
            });
        }

        /// <summary>
        /// Hides the drop down.
        /// </summary>
        public void HideDropDown()
        {
            if (dropDown != null)
            {
                dropDown.Hide();
            }
        }

        // 上次处理的消息记录，用于消息去重和节流
        private Message _lastProcessedMessage;
        private long _lastMessageTimeTicks;
        private const long MESSAGE_THROTTLE_TICKS = 10 * 10000; // 10毫秒的节流时间（以ticks为单位）

        /// <summary>
        /// Processes Windows messages.
        /// 优化消息处理逻辑，确保下拉按钮点击能正常响应
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            // 获取当前时间的ticks
            long currentTicks = DateTime.Now.Ticks;
            
            // 处理下拉按钮点击相关的消息
            if (m.Msg == (NativeMethods.WM_REFLECT + NativeMethods.WM_COMMAND))
            {
                int notificationCode = NativeMethods.HIWORD(m.WParam);
                if (notificationCode == NativeMethods.CBN_DROPDOWN)
                {
                    // 确保dropDown不为null
                    if (dropDown != null)
                    {
                        // 优化：使用毫秒差值计算，避免创建TimeSpan对象
                        long lastClosedTicks = dropDown.LastClosedTimeStamp.Ticks;
                        // 10,000 ticks = 1 millisecond
                        // 减少延迟时间，从500ms改为100ms，提高响应速度
                        if ((currentTicks - lastClosedTicks) > 100 * 10000)
                        {
                            ShowDropDown();
                        }
                    }
                    // 设置结果为0
                    m.Result = IntPtr.Zero;
                    // 更新消息记录
                    UpdateMessageTracking(m, currentTicks);
                    return; // 处理完下拉消息后直接返回
                }
            }
            // 只对特定的可能重复的消息类型应用去重和节流，而不是所有消息
            // 避免影响用户交互的正常消息
            bool shouldApplyThrottling = false;
            
            // 更新消息记录
            UpdateMessageTracking(m, currentTicks);
            
            // 对于其他消息，调用基类处理
            base.WndProc(ref m);
        }
        
        /// <summary>
        /// 更新消息跟踪信息
        /// </summary>
        /// <param name="m">当前处理的消息</param>
        /// <param name="ticks">当前时间的ticks</param>
        private void UpdateMessageTracking(Message m, long ticks)
        {
            // 保存当前消息信息
            _lastProcessedMessage = m;
            _lastMessageTimeTicks = ticks;
        }

        #region " Unused Properties "

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownWidth
        {
            get { return base.DropDownWidth; }
            set { base.DropDownWidth = value; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set
            {
                dropDown.Height = value;
                base.DropDownHeight = value;
            }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { base.IntegralHeight = value; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        #endregion
    }
}
