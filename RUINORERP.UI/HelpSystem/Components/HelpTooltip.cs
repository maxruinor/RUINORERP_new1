using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// 智能帮助提示气泡
    /// 用于显示简短的帮助提示信息
    /// 具有自动隐藏、智能定位、可配置超时等功能
    /// </summary>
    public class HelpTooltip : Form
    {
        #region 私有字段

        /// <summary>
        /// 显示提示文本的标签控件
        /// </summary>
        private Label _label;

        /// <summary>
        /// 自动隐藏定时器
        /// </summary>
        private System.Timers.Timer _autoHideTimer;

        /// <summary>
        /// 默认超时时间(毫秒)
        /// </summary>
        private const int DEFAULT_TIMEOUT = 5000;

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 目标控件,用于计算提示位置
        /// </summary>
        private Control _targetControl;

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取或设置超时时间(毫秒)
        /// 0表示不自动隐藏
        /// </summary>
        public int Timeout { get; set; } = DEFAULT_TIMEOUT;

        /// <summary>
        /// 获取或设置提示文本
        /// </summary>
        public string Text
        {
            get => _label?.Text ?? string.Empty;
            set
            {
                if (_label != null)
                {
                    _label.Text = value;
                    AdjustSize();
                }
            }
        }

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        public Color TooltipBackColor { get; set; } = Color.FromArgb(255, 255, 240);

        /// <summary>
        /// 获取或设置文本颜色
        /// </summary>
        public Color TextColor { get; set; } = Color.FromArgb(50, 50, 50);

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public Font TooltipFont { get; set; } = new Font("Microsoft YaHei", 9F);

        /// <summary>
        /// 获取或设置提示的最大宽度
        /// </summary>
        public int MaxWidth { get; set; } = 300;

        /// <summary>
        /// 获取或设置是否显示阴影效果
        /// </summary>
        public bool ShowShadow { get; set; } = true;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// 初始化帮助提示气泡组件
        /// </summary>
        public HelpTooltip()
        {
            InitializeComponent();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            // 设置窗体属性
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = TooltipBackColor;
            this.Padding = new Padding(10);
            
            // 启用双缓冲,减少闪烁
            this.DoubleBuffered = true;

            // 创建标签控件
            _label = new Label
            {
                Dock = DockStyle.Fill,
                Font = TooltipFont,
                ForeColor = TextColor,
                Text = "",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 添加标签到窗体
            this.Controls.Add(_label);

            // 创建自动隐藏定时器
            _autoHideTimer = new System.Timers.Timer(DEFAULT_TIMEOUT);
            _autoHideTimer.AutoReset = false;
            _autoHideTimer.Elapsed += AutoHideTimer_Elapsed;

            // 设置窗体大小
            this.Size = new Size(MaxWidth, 80);
        }

        #endregion

        #region 公共方法 - 显示和隐藏

        /// <summary>
        /// 显示帮助提示
        /// </summary>
        /// <param name="text">提示文本</param>
        /// <param name="targetControl">目标控件</param>
        /// <param name="position">提示位置(相对于目标控件)</param>
        public void Show(string text, Control targetControl, TooltipPosition position = TooltipPosition.Right)
        {
            if (targetControl == null || string.IsNullOrEmpty(text))
            {
                return;
            }

            // 保存目标控件引用
            _targetControl = targetControl;

            // 设置提示文本
            _label.Text = text;

            // 调整大小
            AdjustSize();

            // 计算显示位置
            Point screenLocation = CalculatePosition(targetControl, position);

            // 确保位置在屏幕范围内
            screenLocation = EnsureInScreenBounds(screenLocation, targetControl);

            // 设置窗体位置
            this.Location = screenLocation;

            // 显示窗体
            base.Show();

            // 重置并启动自动隐藏定时器
            if (Timeout > 0)
            {
                _autoHideTimer.Stop();
                _autoHideTimer.Interval = Timeout;
                _autoHideTimer.Start();
            }
        }

        /// <summary>
        /// 在指定位置显示帮助提示
        /// </summary>
        /// <param name="text">提示文本</param>
        /// <param name="screenLocation">屏幕坐标位置</param>
        public void ShowAt(string text, Point screenLocation)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // 设置提示文本
            _label.Text = text;

            // 调整大小
            AdjustSize();

            // 确保位置在屏幕范围内
            screenLocation = EnsureInScreenBounds(screenLocation, null);

            // 设置窗体位置
            this.Location = screenLocation;

            // 显示窗体
            base.Show();

            // 重置并启动自动隐藏定时器
            if (Timeout > 0)
            {
                _autoHideTimer.Stop();
                _autoHideTimer.Interval = Timeout;
                _autoHideTimer.Start();
            }
        }

        /// <summary>
        /// 隐藏提示
        /// </summary>
        public new void Hide()
        {
            _autoHideTimer.Stop();
            base.Hide();
        }

        /// <summary>
        /// 延迟隐藏提示
        /// </summary>
        /// <param name="delay">延迟时间(毫秒)</param>
        public void HideDelayed(int delay)
        {
            _autoHideTimer.Stop();
            _autoHideTimer.Interval = delay;
            _autoHideTimer.Start();
        }

        #endregion

        #region 私有方法 - 位置计算

        /// <summary>
        /// 计算提示的显示位置
        /// </summary>
        /// <param name="targetControl">目标控件</param>
        /// <param name="position">提示位置</param>
        /// <returns>屏幕坐标</returns>
        private Point CalculatePosition(Control targetControl, TooltipPosition position)
        {
            // 获取目标控件的屏幕位置
            Rectangle targetBounds = targetControl.RectangleToScreen(targetControl.ClientRectangle);

            switch (position)
            {
                case TooltipPosition.Top:
                    return new Point(
                        targetBounds.X + (targetBounds.Width - this.Width) / 2,
                        targetBounds.Y - this.Height - 5);

                case TooltipPosition.Bottom:
                    return new Point(
                        targetBounds.X + (targetBounds.Width - this.Width) / 2,
                        targetBounds.Bottom + 5);

                case TooltipPosition.Left:
                    return new Point(
                        targetBounds.X - this.Width - 5,
                        targetBounds.Y + (targetBounds.Height - this.Height) / 2);

                case TooltipPosition.Right:
                default:
                    return new Point(
                        targetBounds.Right + 5,
                        targetBounds.Y);
            }
        }

        /// <summary>
        /// 确保位置在屏幕范围内
        /// </summary>
        /// <param name="location">原始位置</param>
        /// <param name="targetControl">目标控件(可选)</param>
        /// <returns>调整后的位置</returns>
        private Point EnsureInScreenBounds(Point location, Control targetControl)
        {
            // 获取屏幕工作区(排除任务栏)
            Screen screen = targetControl != null 
                ? Screen.FromControl(targetControl) 
                : Screen.PrimaryScreen;

            Rectangle workingArea = screen.WorkingArea;

            // 检查右边界
            if (location.X + this.Width > workingArea.Right)
            {
                location.X = workingArea.Right - this.Width;
            }

            // 检查左边界
            if (location.X < workingArea.Left)
            {
                location.X = workingArea.Left;
            }

            // 检查下边界
            if (location.Y + this.Height > workingArea.Bottom)
            {
                location.Y = workingArea.Bottom - this.Height;
            }

            // 检查上边界
            if (location.Y < workingArea.Top)
            {
                location.Y = workingArea.Top;
            }

            return location;
        }

        /// <summary>
        /// 调整窗体大小以适应文本内容
        /// </summary>
        private void AdjustSize()
        {
            try
            {
                using (Graphics g = this.CreateGraphics())
                {
                    // 测量文本大小
                    SizeF textSize = g.MeasureString(
                        _label.Text, 
                        _label.Font, 
                        MaxWidth - 20);

                    // 计算新的窗体大小(加上边距)
                    int newWidth = (int)Math.Min(textSize.Width + 20, MaxWidth);
                    int newHeight = (int)(textSize.Height + 20);

                    // 确保最小尺寸
                    newWidth = Math.Max(newWidth, 200);
                    newHeight = Math.Max(newHeight, 60);

                    // 设置新的窗体大小
                    this.Size = new Size(newWidth, newHeight);
                }
            }
            catch
            {
                // 测量失败,使用默认大小
                this.Size = new Size(MaxWidth, 80);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 自动隐藏定时器事件
        /// </summary>
        private void AutoHideTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // 在UI线程中隐藏
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => base.Hide()));
                }
                else
                {
                    base.Hide();
                }
            }
            catch
            {
                // 窗体可能已关闭,忽略异常
            }
        }

        /// <summary>
        /// 重写OnPaint方法,绘制边框和阴影效果
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 绘制边框
            using (Pen borderPen = new Pen(Color.FromArgb(200, 200, 180), 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
            }

            // 绘制阴影效果
            if (ShowShadow)
            {
                using (Pen shadowPen = new Pen(Color.FromArgb(30, 30, 30), 1))
                {
                    // 右侧阴影
                    e.Graphics.DrawLine(shadowPen, this.Width - 1, 1, this.Width - 1, this.Height - 1);
                    // 底部阴影
                    e.Graphics.DrawLine(shadowPen, 1, this.Height - 1, this.Width - 1, this.Height - 1);
                }
            }
        }

        /// <summary>
        /// 重写OnMouseDown方法,点击提示时自动隐藏
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Hide();
        }

        /// <summary>
        /// 重写OnKeyDown方法,按下ESC键时隐藏
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                Hide();
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    _autoHideTimer?.Stop();
                    _autoHideTimer?.Dispose();
                    _label?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 提示位置枚举
    /// 定义提示相对于目标控件的位置
    /// </summary>
    public enum TooltipPosition
    {
        /// <summary>
        /// 在目标控件上方显示
        /// </summary>
        Top,

        /// <summary>
        /// 在目标控件下方显示
        /// </summary>
        Bottom,

        /// <summary>
        /// 在目标控件左侧显示
        /// </summary>
        Left,

        /// <summary>
        /// 在目标控件右侧显示
        /// </summary>
        Right
    }
}
