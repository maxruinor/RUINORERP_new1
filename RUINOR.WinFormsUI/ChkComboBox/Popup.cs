using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using VS = System.Windows.Forms.VisualStyles;

/*
<li>Base class for custom tooltips.</li>
<li>Office-2007-like tooltip class.</li>
*/
namespace RUINOR.WinFormsUI.ChkComboBox
{
    /// <summary>
    /// CodeProject.com "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp".
    /// Represents a pop-up window.
    /// </summary>
    [CLSCompliant(true), ToolboxItem(false)]
    public partial class Popup : ToolStripDropDown
    {
        #region " Fields & Properties "

        private Control content;
        /// <summary>
        /// Gets the content of the pop-up.
        /// </summary>
        public Control Content
        {
            get { return content; }
        }

        private bool fade;
        /// <summary>
        /// Gets a value indicating whether the <see cref="PopupControl.Popup"/> uses the fade effect.
        /// </summary>
        /// <value><c>true</c> if pop-up uses the fade effect; otherwise, <c>false</c>.</value>
        /// <remarks>To use the fade effect, the FocusOnOpen property also has to be set to <c>true</c>.</remarks>
        public bool UseFadeEffect
        {
            get { return fade; }
            set
            {
                if (fade == value) return;
                fade = value;
            }
        }

        private bool focusOnOpen = true;
        /// <summary>
        /// Gets or sets a value indicating whether to focus the content after the pop-up has been opened.
        /// </summary>
        /// <value><c>true</c> if the content should be focused after the pop-up has been opened; otherwise, <c>false</c>.</value>
        /// <remarks>If the FocusOnOpen property is set to <c>false</c>, then pop-up cannot use the fade effect.</remarks>
        public bool FocusOnOpen
        {
            get { return focusOnOpen; }
            set { focusOnOpen = value; }
        }

        private bool acceptAlt = true;
        /// <summary>
        /// Gets or sets a value indicating whether presing the alt key should close the pop-up.
        /// </summary>
        /// <value><c>true</c> if presing the alt key does not close the pop-up; otherwise, <c>false</c>.</value>
        public bool AcceptAlt
        {
            get { return acceptAlt; }
            set { acceptAlt = value; }
        }

        private Popup ownerPopup;
        private Popup childPopup;

        private bool _resizable;
        private bool resizable;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PopupControl.Popup" /> is resizable.
        /// </summary>
        /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
        public bool Resizable
        {
            get { return resizable && _resizable; }
            set { resizable = value; }
        }

        private ToolStripControlHost host;

        private Size minSize;
        /// <summary>
        /// Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MinimumSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        private Size maxSize;
        /// <summary>
        /// Gets or sets the size that is the upper limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MaximumSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        /// <summary>
        /// Gets parameters of a new window.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Windows.Forms.CreateParams" /> used when creating a new window.</returns>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
                return cp;
            }
        }

        #endregion

        #region " Constructors "

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl.Popup"/> class.
        /// </summary>
        /// <param name="content">The content of the pop-up.</param>
        /// <remarks>
        /// Pop-up will be disposed immediately after disposion of the content control.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="content" /> is <code>null</code>.</exception>
        public Popup(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            this.content = content;
            this.fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
            this._resizable = true;
            InitializeComponent();
            AutoSize = false;
            DoubleBuffered = true;
            ResizeRedraw = true;
            host = new ToolStripControlHost(content);
            Padding = Margin = host.Padding = host.Margin = Padding.Empty;
            MinimumSize = content.MinimumSize;
            content.MinimumSize = content.Size;
            MaximumSize = content.MaximumSize;
            content.MaximumSize = content.Size;
            Size = content.Size;
            content.Location = Point.Empty;
            Items.Add(host);
            content.Disposed += delegate(object sender, EventArgs e)
            {
                content = null;
                Dispose(true);
            };
            content.RegionChanged += delegate(object sender, EventArgs e)
            {
                UpdateRegion();
            };
            content.Paint += delegate(object sender, PaintEventArgs e)
            {
                PaintSizeGrip(e);
            };
            UpdateRegion();
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Processes a dialog box key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (acceptAlt && ((keyData & Keys.Alt) == Keys.Alt)) return false;
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Updates the pop-up region.
        /// </summary>
        protected void UpdateRegion()
        {
            if (this.Region != null)
            {
                this.Region.Dispose();
                this.Region = null;
            }
            if (content.Region != null)
            {
                this.Region = content.Region.Clone();
            }
        }

        /// <summary>
        /// Shows pop-up window below the specified control.
        /// </summary>
        /// <param name="control">The control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below the specified control, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            Show(control, control.ClientRectangle);
        }

        /// <summary>
        /// Shows pop-up window below the specified area of specified control.
        /// </summary>
        /// <param name="control">The control used to compute screen location of specified area.</param>
        /// <param name="area">The area of control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below specified area, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control, Rectangle area)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            resizableTop = resizableRight = false;
            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            Rectangle screen = Screen.FromControl(control).WorkingArea;
            if (location.X + Size.Width > (screen.Left + screen.Width))
            {
                resizableRight = true;
                location.X = (screen.Left + screen.Width) - Size.Width;
            }
            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                resizableTop = true;
                location.Y -= Size.Height + area.Height;
            }
            location = control.PointToClient(location);
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        private const int frames = 1;
        private const int totalduration = 0; // ML : 2007-11-05 : was 100 but caused a flicker.
        private const int frameduration = totalduration / frames;
        /// <summary>
        /// Adjusts the size of the owner <see cref="T:System.Windows.Forms.ToolStrip" /> to accommodate the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> if the owner <see cref="T:System.Windows.Forms.ToolStrip" /> is currently displayed, or clears and resets active <see cref="T:System.Windows.Forms.ToolStripDropDown" /> child controls of the <see cref="T:System.Windows.Forms.ToolStrip" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> is not currently displayed.
        /// </summary>
        /// <param name="visible">true if the owner <see cref="T:System.Windows.Forms.ToolStrip" /> is currently displayed; otherwise, false.</param>
        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && fade && focusOnOpen) Opacity = 0;
            base.SetVisibleCore(visible);
            if (!visible || !fade || !focusOnOpen) return;
            for (int i = 1; i <= frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(frameduration);
                }
                Opacity = opacity * (double)i / (double)frames;
            }
            Opacity = opacity;
        }

        private bool resizableTop;
        private bool resizableRight;

        private void SetOwnerItem(Control control)
        {
            if (control == null)
            {
                return;
            }
            if (control is Popup)
            {
                Popup popupControl = control as Popup;
                ownerPopup = popupControl;
                ownerPopup.childPopup = this;
                OwnerItem = popupControl.Items[0];
                return;
            }
            if (control.Parent != null)
            {
                SetOwnerItem(control.Parent);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            content.MinimumSize = Size;
            content.MaximumSize = Size;
            content.Size = Size;
            content.Location = Point.Empty;
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opening" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnOpening(CancelEventArgs e)
        {
            if (content.IsDisposed || content.Disposing)
            {
                e.Cancel = true;
                return;
            }
            UpdateRegion();
            base.OnOpening(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opened" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnOpened(EventArgs e)
        {
            if (ownerPopup != null)
            {
                ownerPopup._resizable = false;
            }
            if (focusOnOpen)
            {
                content.Focus();
            }
            base.OnOpened(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            if (ownerPopup != null)
            {
                ownerPopup._resizable = true;
            }
            base.OnClosed(e);
        }

        public DateTime LastClosedTimeStamp = DateTime.Now;

        protected override void OnVisibleChanged(EventArgs e)
        {
            // 只有在从可见变为不可见时才更新时间戳
            if (Visible == false && !IsDisposed)
            {
                // 优化：减少DateTime.Now的调用频率，只在真正需要时更新
                LastClosedTimeStamp = DateTime.Now;
            }
            
            // 确保基类方法被调用
            base.OnVisibleChanged(e);
        }

        #endregion

        #region " Resizing Support "

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            // 快速路径：只处理必要的消息类型
            switch (m.Msg)
            {
                case NativeMethods.WM_NCACTIVATE:
                    // 只在需要时处理NCACTIVATE消息
                    if (m.WParam != IntPtr.Zero && childPopup != null && childPopup.Visible)
                    {
                        childPopup.Hide();
                        return;
                    }
                    break;
                
                case NativeMethods.WM_NCHITTEST:
                case NativeMethods.WM_GETMINMAXINFO:
                    // 只有在可调整大小且不是大量消息处理时才进行调整大小处理
                    if (Resizable)
                    {
                        if (InternalProcessResizing(ref m, false))
                        {
                            return;
                        }
                    }
                    break;
                
                // 其他消息直接传递给基类，避免不必要的处理
                default:
                    // 对于高频消息，可以添加额外的优化
                    break;
            }
            
            // 调用基类处理
            base.WndProc(ref m);
        }

        /// <summary>
        /// Processes the resizing messages.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessResizing(ref Message m)
        {
            return InternalProcessResizing(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool InternalProcessResizing(ref Message m, bool contentControl)
        {
            // 现在WM_NCACTIVATE消息处理已移至WndProc的快速路径中
            
            // 快速检查是否可调整大小
            if (!Resizable)
            {
                return false;
            }
            
            // 只处理特定的调整大小相关消息
            if (m.Msg == NativeMethods.WM_NCHITTEST)
            {
                // 对于NCHITTEST消息，只在鼠标移动或点击时才处理
                // 减少不必要的计算
                return OnNcHitTest(ref m, contentControl);
            }
            else if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
            {
                // MINMAXINFO消息处理
                return OnGetMinMaxInfo(ref m);
            }
            
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            NativeMethods.MINMAXINFO minmax = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
            minmax.maxTrackSize = this.MaximumSize;
            minmax.minTrackSize = this.MinimumSize;
            Marshal.StructureToPtr(minmax, m.LParam, false);
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            // 快速提取坐标，避免不必要的对象创建
            int x = NativeMethods.LOWORD(m.LParam);
            int y = NativeMethods.HIWORD(m.LParam);
            
            // 直接使用屏幕坐标进行快速范围检查，避免PointToClient转换
            // 只在需要更精确检查时才进行转换
            Rectangle bounds = contentControl ? content.Bounds : Bounds;
            if (x < bounds.Left || x > bounds.Right || y < bounds.Top || y > bounds.Bottom)
            {
                return false; // 快速排除屏幕外的情况
            }
            
            // 只在需要时进行PointToClient转换
            Point clientLocation = PointToClient(new Point(x, y));
            
            // 避免每次都创建GripBounds对象，使用简单的边界检查
            Rectangle clientRect = contentControl ? content.ClientRectangle : ClientRectangle;
            int borderWidth = 4; // 调整大小边界的宽度
            
            IntPtr transparent = new IntPtr(NativeMethods.HTTRANSPARENT);
            
            // 优化：先检查四个角落
            if (resizableTop && y <= clientRect.Top + borderWidth)
            {
                if (resizableRight && x <= clientRect.Left + borderWidth)
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPLEFT;
                    return true;
                }
                if (!resizableRight && x >= clientRect.Right - borderWidth)
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPRIGHT;
                    return true;
                }
                // 顶部边缘
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOP;
                return true;
            }
            else if (!resizableTop && y >= clientRect.Bottom - borderWidth)
            {
                if (resizableRight && x <= clientRect.Left + borderWidth)
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMLEFT;
                    return true;
                }
                if (!resizableRight && x >= clientRect.Right - borderWidth)
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return true;
                }
                // 底部边缘
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOM;
                return true;
            }
            
            // 左右边缘检查
            if (resizableRight && x <= clientRect.Left + borderWidth)
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTLEFT;
                return true;
            }
            if (!resizableRight && x >= clientRect.Right - borderWidth)
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTRIGHT;
                return true;
            }
            
            return false;
        }

        private VS.VisualStyleRenderer sizeGripRenderer;
        /// <summary>
        /// Paints the size grip.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
        public void PaintSizeGrip(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null || !resizable)
            {
                return;
            }
            Size clientSize = content.ClientSize;
            if (Application.RenderWithVisualStyles)
            {
                if (this.sizeGripRenderer == null)
                {
                    this.sizeGripRenderer = new VS.VisualStyleRenderer(VS.VisualStyleElement.Status.Gripper.Normal);
                }
                this.sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10));
            }
            else
            {
                ControlPaint.DrawSizeGrip(e.Graphics, content.BackColor, clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10);
            }
        }

        #endregion
    }
}
