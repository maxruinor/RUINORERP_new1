﻿using System;
using System.Windows.Forms;
using WinLib.ScrollBarEx.Win32.Const;
using WinLib.ScrollBarEx.Win32;
using WinLib.ScrollBarEx.Win32.Struct;
using System.Drawing;

namespace WinLib.ScrollBarEx
{
    /* 作者：Starts_2000
     * 日期：2010-07-30
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    internal abstract class MaskControlBase : 
        NativeWindow, IDisposable
    {
        #region Filedes

        private CreateParams _createParams;
        private bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">被覆盖控件的句柄。</param>
        protected MaskControlBase(IntPtr hWnd)
            : base()
        {
            CreateParamsInternal(hWnd);
        }

        protected MaskControlBase(
            IntPtr hWnd, Rectangle rect)
        {
            CreateParamsInternal(hWnd, rect);
        }

        ~MaskControlBase()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取窗口句柄是否已经创建。
        /// </summary>
        protected bool IsHandleCreated
        {
            get { return base.Handle != IntPtr.Zero; }
        }

        protected virtual CreateParams CreateParams
        {
            get
            {
                return _createParams;
            }
        }

        /// <summary>
        /// 创建窗口的句柄。
        /// </summary>
        internal protected void OnCreateHandle()
        {
            base.CreateHandle(CreateParams);
            SetZorder();
        }

        protected virtual void OnPaint(IntPtr hWnd)
        {
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            try
            {
                switch (m.Msg)
                {
                    case WM.WM_NCPAINT:
                    case WM.WM_PAINT:
                        OnPaint(m.HWnd);
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">被覆盖控件句柄。</param>
        internal void CreateParamsInternal(IntPtr hWnd)
        {
            IntPtr hParent = WinLib.ScrollBarEx.Win32.NativeMethods.GetParent(hWnd);
            RECT rect = new RECT();
            Point point = new Point();

            WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowRect(hWnd, ref rect);
            point.X = rect.Left;
            point.Y = rect.Top;
            WinLib.ScrollBarEx.Win32.NativeMethods.ScreenToClient(hParent, ref point);

            int dwStyle =
                SS.SS_OWNERDRAW |
                WS.WS_CHILD |
                WS.WS_CLIPSIBLINGS |
                WS.WS_OVERLAPPED | WS.WS_VISIBLE;
            int dwExStyle = WS_EX.WS_EX_TOPMOST | WS_EX.WS_EX_TOOLWINDOW;
            _createParams = new CreateParams();

            _createParams.Parent = hParent;
            _createParams.ClassName = ClassName.STATIC;
            _createParams.Caption = null;
            _createParams.Style = dwStyle;
            _createParams.ExStyle = dwExStyle;
            _createParams.X = point.X;
            _createParams.Y = point.Y;
            _createParams.Width = rect.Right - rect.Left;
            _createParams.Height = rect.Bottom - rect.Top;
        }

        internal void CreateParamsInternal(IntPtr hWnd, Rectangle rect)
        {
            IntPtr hParent = WinLib.ScrollBarEx.Win32.NativeMethods.GetParent(hWnd);

            int dwStyle =
                SS.SS_OWNERDRAW |
                WS.WS_CHILD |
                WS.WS_CLIPSIBLINGS |
                WS.WS_OVERLAPPED | WS.WS_VISIBLE;
            int dwExStyle = WS_EX.WS_EX_TOPMOST | WS_EX.WS_EX_TOOLWINDOW;
            _createParams = new CreateParams();

            _createParams.Parent = hParent;
            _createParams.ClassName = ClassName.STATIC;
            _createParams.Caption = null;
            _createParams.Style = dwStyle;
            _createParams.ExStyle = dwExStyle;
            _createParams.X = rect.X;
            _createParams.Y = rect.Y;
            _createParams.Width = rect.Width;
            _createParams.Height = rect.Height;
        }

        internal void DestroyHandleInternal()
        {
            if (IsHandleCreated)
            {
                base.DestroyHandle();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">被覆盖控件句柄。</param>
        internal void CheckBounds(IntPtr hWnd)
        {
            RECT controlRect = new RECT();
            RECT maskRect = new RECT();

            WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowRect(base.Handle, ref maskRect);
            WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowRect(hWnd, ref controlRect);

            uint uFlag =
                SWP.SWP_NOACTIVATE |
                SWP.SWP_NOOWNERZORDER |
                SWP.SWP_NOZORDER;

            if (!WinLib.ScrollBarEx.Win32.NativeMethods.EqualRect(ref controlRect, ref maskRect))
            {
                Point point = new Point(controlRect.Left, controlRect.Top);
                IntPtr hParent = WinLib.ScrollBarEx.Win32.NativeMethods.GetParent(base.Handle);
                WinLib.ScrollBarEx.Win32.NativeMethods.ScreenToClient(hParent, ref point);

                WinLib.ScrollBarEx.Win32.NativeMethods.SetWindowPos(
                    base.Handle,
                    IntPtr.Zero,
                    point.X,
                    point.Y,
                    controlRect.Right - controlRect.Left,
                    controlRect.Bottom - controlRect.Top,
                    uFlag);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">被覆盖控件句柄。</param>
        internal void SetVisibale(IntPtr hWnd)
        {
            bool bVisible = (WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowLong(
                hWnd,
                GWL.GWL_STYLE) & WS.WS_VISIBLE) == WS.WS_VISIBLE;
            SetVisibale(bVisible);
        }

        internal void SetVisibale(bool visibale)
        {
            if (visibale)
            {
                WinLib.ScrollBarEx.Win32.NativeMethods.ShowWindow(base.Handle, SW.SW_NORMAL);
            }
            else
            {
                WinLib.ScrollBarEx.Win32.NativeMethods.ShowWindow(base.Handle, SW.SW_HIDE);
            }
        }

        private void SetZorder()
        {
            uint uFlags =
                SWP.SWP_NOMOVE |
                SWP.SWP_NOSIZE |
                SWP.SWP_NOACTIVATE |
                SWP.SWP_NOOWNERZORDER;

            WinLib.ScrollBarEx.Win32.NativeMethods.SetWindowPos(
                base.Handle,
                HWND.HWND_TOP,
                0,
                0,
                0,
                0,
                uFlags);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _createParams = null;
                }

                DestroyHandleInternal();
            }
            _disposed = true;
        }

        #endregion
    }
}
