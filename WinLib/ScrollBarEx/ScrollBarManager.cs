﻿using System;
using WinLib.ScrollBarEx.Win32;
using WinLib.ScrollBarEx.Win32.Struct;
using WinLib.ScrollBarEx.Win32.Const;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace WinLib.ScrollBarEx
{
    /* 作者：Starts_2000
     * 日期：2010-07-30
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    internal class ScrollBarManager : NativeWindow, IDisposable
    {
        #region Fields

        private bool _bPainting;
        private ScrollBar _owner;
        private ScrollBarMaskControl _maskControl;
        private ScrollBarHistTest _lastMouseDownHistTest;
        private bool _disposed;

        #endregion

        #region Constructors

        internal ScrollBarManager(ScrollBar owner)
            : base()
        {
            _owner = owner;
            CreateHandle();
        }

        ~ScrollBarManager()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        private IntPtr OwnerHWnd
        {
            get { return _owner.Handle; }
        }

        private Orientation Direction
        {
            get
            {
                if (_owner is HScrollBar)
                {
                    return Orientation.Horizontal;
                }

                return Orientation.Vertical;
            }
        }

        private int ArrowCx
        {
            get { return SystemInformation.HorizontalScrollBarArrowWidth; }
        }

        private int ArrowCy
        {
            get { return SystemInformation.VerticalScrollBarArrowHeight; }
        }

        #endregion

        #region WndProc Method

        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM.WM_PAINT:
                        if (!_bPainting)
                        {
                            PAINTSTRUCT ps = new PAINTSTRUCT();

                            _bPainting = true;
                            WinLib.ScrollBarEx.Win32.NativeMethods.BeginPaint(m.HWnd, ref ps);
                            DrawScrollBar(m.HWnd, _maskControl.Handle);
                            WinLib.ScrollBarEx.Win32.NativeMethods.ValidateRect(m.HWnd, ref ps.rcPaint);
                            WinLib.ScrollBarEx.Win32.NativeMethods.EndPaint(m.HWnd, ref ps);
                            _bPainting = false;

                            m.Result = Result.TRUE;
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;
                    case SBM.SBM_SETSCROLLINFO:
                        DrawScrollBar(m.HWnd, _maskControl.Handle, true, false);
                        base.WndProc(ref m);
                        break;
                    case WM.WM_STYLECHANGED:
                        DrawScrollBar(m.HWnd, _maskControl.Handle, false, true);
                        base.WndProc(ref m);
                        break;
                    case WM.WM_LBUTTONDOWN:
                        _lastMouseDownHistTest = ScrollBarHitTest(m.HWnd);
                        DrawScrollBar(m.HWnd, _maskControl.Handle);
                        base.WndProc(ref m);
                        break;
                    case WM.WM_LBUTTONUP:
                    case WM.WM_MOUSEMOVE:
                        DrawScrollBar(m.HWnd, _maskControl.Handle);
                        base.WndProc(ref m);
                        break;
                    case WM.WM_MOUSELEAVE:
                        DrawScrollBar(m.HWnd, _maskControl.Handle);
                        base.WndProc(ref m);
                        break;
                    case WM.WM_WINDOWPOSCHANGED:
                        WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(
                            m.LParam, typeof(WINDOWPOS));
                        bool hide = (pos.flags & SWP.SWP_HIDEWINDOW) != 0;
                        bool show = (pos.flags & SWP.SWP_SHOWWINDOW) != 0;
                        if (hide)
                        {
                            _maskControl.SetVisibale(false);
                        }
                        else if (show)
                        {
                            _maskControl.SetVisibale(true);
                        }
                        _maskControl.CheckBounds(m.HWnd);
                        base.WndProc(ref m);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Draw Methods

        private void DrawScrollBar(
            IntPtr scrollBarHWnd, IntPtr maskHWnd)
        {
            DrawScrollBar(scrollBarHWnd, maskHWnd, false, false);
        }

        private void DrawScrollBar(
            IntPtr scrollBarHWnd, IntPtr maskHWnd, 
            bool sbm, bool styleChanged)
        {
            Rectangle bounds;
            Rectangle trackRect;
            Rectangle topLeftArrowRect;
            Rectangle bottomRightArrowRect;
            Rectangle thumbRect;

            ControlState topLeftArrowState;
            ControlState bottomRightArrowState;
            ControlState thumbState;

            Orientation direction = Direction;
            bool bHorizontal = direction == Orientation.Horizontal;
            ScrollBarHistTest histTest;

            CalculateRect(scrollBarHWnd, bHorizontal, out bounds, out trackRect,
                out topLeftArrowRect, out bottomRightArrowRect, out thumbRect);
            GetState(scrollBarHWnd, bHorizontal, out histTest, out topLeftArrowState,
                out bottomRightArrowState, out thumbState);

            if (sbm)
            {
                if (histTest == ScrollBarHistTest.None)
                {
                    thumbState = ControlState.Pressed;
                }
                else if(_lastMouseDownHistTest == ScrollBarHistTest.Track)
                {
                    thumbState = ControlState.Normal;
                }
            }

            if (styleChanged)
            {
                thumbState = ControlState.Normal;
            }

            DrawScrollBar(maskHWnd, bounds, trackRect, topLeftArrowRect, bottomRightArrowRect,
                thumbRect, topLeftArrowState, bottomRightArrowState, thumbState, direction);
        }

        private void DrawScrollBar(
           ControlState topLeftArrowState,
           ControlState bottomRightArrowState,
           ControlState thumbState)
        {
            Rectangle bounds;
            Rectangle trackRect;
            Rectangle topLeftArrowRect;
            Rectangle bottomRightArrowRect;
            Rectangle thumbRect;

            Orientation direction = Direction;
            bool bHorizontal = direction == Orientation.Horizontal;

            CalculateRect(OwnerHWnd, bHorizontal, out bounds, out trackRect,
                out topLeftArrowRect, out bottomRightArrowRect, out thumbRect);
            DrawScrollBar(_maskControl.Handle, bounds, trackRect, topLeftArrowRect,
                bottomRightArrowRect, thumbRect, topLeftArrowState, 
                bottomRightArrowState, thumbState, direction);
        }

        private void DrawScrollBar(
            IntPtr maskHWnd,
            Rectangle bounds,
            Rectangle trackRect,
            Rectangle topLeftArrowRect,
            Rectangle bottomRightArrowRect,
            Rectangle thumbRect,
            ControlState topLeftArrowState,
            ControlState bottomRightArrowState,
            ControlState thumbState,
            Orientation direction)
        {
            bool bHorizontal = direction == Orientation.Horizontal;
            ArrowDirection arrowDirection;
            bool bEnabled = _owner.Enabled;
            IScrollBarPaint paint = _owner as IScrollBarPaint;

            if (paint == null)
            {
                return;
            }

            ImageDc tempDc = new ImageDc(bounds.Width, bounds.Height);
            IntPtr hdc = WinLib.ScrollBarEx.Win32.NativeMethods.GetDC(maskHWnd);
            try
            {
                using (Graphics g = Graphics.FromHdc(tempDc.Hdc))
                {
                    using (PaintScrollBarTrackEventArgs te =
                        new PaintScrollBarTrackEventArgs(
                        g,
                        trackRect,
                        direction,
                        bEnabled))
                    {
                        paint.OnPaintScrollBarTrack(te);
                    }

                    arrowDirection = bHorizontal ? 
                        ArrowDirection.Left : ArrowDirection.Up;

                    using (PaintScrollBarArrowEventArgs te =
                        new PaintScrollBarArrowEventArgs(
                        g,
                        topLeftArrowRect,
                        topLeftArrowState,
                        arrowDirection,
                        direction,
                        bEnabled))
                    {
                        paint.OnPaintScrollBarArrow(te);
                    }

                    arrowDirection = bHorizontal ?
                        ArrowDirection.Right : ArrowDirection.Down;

                    using (PaintScrollBarArrowEventArgs te =
                        new PaintScrollBarArrowEventArgs(
                        g,
                        bottomRightArrowRect,
                        bottomRightArrowState,
                        arrowDirection,
                        direction,
                        bEnabled))
                    {
                        paint.OnPaintScrollBarArrow(te);
                    }

                    using (PaintScrollBarThumbEventArgs te =
                        new PaintScrollBarThumbEventArgs(
                        g,
                        thumbRect,
                        thumbState,
                        direction,
                        bEnabled))
                    {
                        paint.OnPaintScrollBarThumb(te);
                    }
                }

                WinLib.ScrollBarEx.Win32.NativeMethods.BitBlt(
                    hdc,
                    0,
                    0,
                    bounds.Width,
                    bounds.Height,
                    tempDc.Hdc,
                    0,
                    0,
                    TernaryRasterOperations.SRCCOPY);
            }
            finally
            {
                WinLib.ScrollBarEx.Win32.NativeMethods.ReleaseDC(maskHWnd, hdc);
                tempDc.Dispose();
            }
        }

        private void CalculateRect(
            IntPtr scrollBarHWnd,
            bool bHorizontal,
            out Rectangle bounds,
            out Rectangle trackRect,
            out Rectangle topLeftArrowRect,
            out Rectangle bottomRightArrowRect,
            out Rectangle thumbRect)
        {
            RECT rect = new RECT();
            WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowRect(scrollBarHWnd, ref rect);
            WinLib.ScrollBarEx.Win32.NativeMethods.OffsetRect(ref rect, -rect.Left, -rect.Top);

            int arrowWidth = bHorizontal ? ArrowCx : ArrowCy;

            bounds = rect.Rect;
            Point point = GetScrollBarThumb(
                bounds, bHorizontal, arrowWidth);

            trackRect = bounds;
            if (bHorizontal)
            {
                topLeftArrowRect = new Rectangle(
                    0, 0, arrowWidth, bounds.Height);
                bottomRightArrowRect = new Rectangle(
                    bounds.Width - arrowWidth, 0, arrowWidth, bounds.Height);
                if (_owner.RightToLeft == RightToLeft.Yes)
                {
                    thumbRect = new Rectangle(
                        point.Y,
                        0,
                        point.X - point.Y,
                        bounds.Height);
                }
                else
                {
                    thumbRect = new Rectangle(
                        point.X,
                        0,
                        point.Y - point.X,
                        bounds.Height);
                }
            }
            else
            {
                topLeftArrowRect = new Rectangle(
                    0, 0, bounds.Width, arrowWidth);
                bottomRightArrowRect = new Rectangle(
                    0, bounds.Height - arrowWidth, bounds.Width, arrowWidth);
                thumbRect = new Rectangle(
                    0, point.X, bounds.Width, point.Y - point.X);
            }
        }

        private void GetState(
            IntPtr scrollBarHWnd,
            bool bHorizontal,
            out ScrollBarHistTest histTest,
            out ControlState topLeftArrowState,
            out ControlState bottomRightArrowState,
            out ControlState thumbState)
        {
            histTest = ScrollBarHitTest(scrollBarHWnd);
            bool bLButtonDown = Helper.LeftKeyPressed();
            bool bEnabled = _owner.Enabled;

            topLeftArrowState = ControlState.Normal;
            bottomRightArrowState = ControlState.Normal;
            thumbState = ControlState.Normal;

            switch (histTest)
            {
                case ScrollBarHistTest.LeftArrow:
                case ScrollBarHistTest.TopArrow:
                    if (bEnabled)
                    {
                        topLeftArrowState = bLButtonDown ?
                            ControlState.Pressed : ControlState.Hover;
                    }
                    break;
                case ScrollBarHistTest.RightArrow:
                case ScrollBarHistTest.BottomArrow:
                    if (bEnabled)
                    {
                        bottomRightArrowState = bLButtonDown ?
                            ControlState.Pressed : ControlState.Hover;
                    }
                    break;
                case ScrollBarHistTest.Thumb:
                    if (bEnabled)
                    {
                        thumbState = bLButtonDown ?
                            ControlState.Pressed : ControlState.Hover;
                    }
                    break;
            }
        }

        #endregion

        #region Helper Methods

        protected void CreateHandle()
        {
            base.AssignHandle(OwnerHWnd);
            _maskControl = new ScrollBarMaskControl(this);
            _maskControl.OnCreateHandle();
        }

        internal void ReleaseHandleInternal()
        {
            if (base.Handle != IntPtr.Zero)
            {
                base.ReleaseHandle();
            }
        }

        private SCROLLBARINFO GetScrollBarInfo(IntPtr hWnd)
        {
            SCROLLBARINFO sbi = new SCROLLBARINFO();
            sbi.cbSize = Marshal.SizeOf(sbi);
            WinLib.ScrollBarEx.Win32.NativeMethods.SendMessage(
                hWnd,
                SBM.SBM_GETSCROLLBARINFO,
                0,
                ref sbi);
            return sbi;
        }

        private SCROLLBARINFO GetScrollBarInfo(IntPtr hWnd, uint objid)
        {
            SCROLLBARINFO sbi = new SCROLLBARINFO();
            sbi.cbSize = Marshal.SizeOf(sbi);

            WinLib.ScrollBarEx.Win32.NativeMethods.GetScrollBarInfo(hWnd, objid, ref sbi);
            return sbi;
        }

        private Point GetScrollBarThumb()
        {
            bool bHorizontal = Direction == Orientation.Horizontal;
            int arrowWidth = bHorizontal ? ArrowCx : ArrowCy;
            return GetScrollBarThumb(
                _owner.ClientRectangle,
                bHorizontal,
                arrowWidth);
        }

        private Point GetScrollBarThumb(
            Rectangle rect, bool bHorizontal, int arrowWidth)
        {
            ScrollBar scrollBar = _owner;
            int width;
            Point point = new Point();

            if (bHorizontal)
            {
                width = rect.Width - arrowWidth * 2;
            }
            else
            {
                width = rect.Height - arrowWidth * 2;
            }

            int value = scrollBar.Maximum - scrollBar.Minimum - scrollBar.LargeChange + 1;
            float thumbWidth = (float)width / ((float)value / scrollBar.LargeChange + 1);

            if (thumbWidth < 8)
            {
                thumbWidth = 8f;
            }

            if (value != 0)
            {
                int curValue = scrollBar.Value - scrollBar.Minimum;
                if (curValue > value)
                {
                    curValue = value;
                }
                point.X = (int)(curValue * ((float)(width - thumbWidth) / value));
            }
            point.X += arrowWidth;
            point.Y = point.X + (int)Math.Ceiling(thumbWidth);

            if (bHorizontal && scrollBar.RightToLeft == RightToLeft.Yes)
            {
                point.X = scrollBar.Width - point.X;
                point.Y = scrollBar.Width - point.Y;
            }

            return point;
        }

        private ScrollBarHistTest ScrollBarHitTest(IntPtr hWnd)
        {
            Point point = new Point();
            RECT rect = new RECT();
            Point thumbPoint = GetScrollBarThumb();

            int arrowCx = ArrowCx;
            int arrowCy = ArrowCy;

            WinLib.ScrollBarEx.Win32.NativeMethods.GetWindowRect(hWnd, ref rect);
            WinLib.ScrollBarEx.Win32.NativeMethods.OffsetRect(ref rect, -rect.Left, -rect.Top);

            RECT tp = rect;
            WinLib.ScrollBarEx.Win32.NativeMethods.GetCursorPos(ref point);
            WinLib.ScrollBarEx.Win32.NativeMethods.ScreenToClient(hWnd, ref point);

            if (Direction == Orientation.Horizontal)
            {
                if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref rect, point))
                {
                    // left arrow
                    tp.Right = arrowCx;
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.LeftArrow;
                    }
                    // right arrow
                    tp.Left = rect.Right - arrowCx;
                    tp.Right = rect.Right;
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.RightArrow;
                    }

                    // button
                    if (_owner.RightToLeft == RightToLeft.Yes)
                    {
                        tp.Left = point.Y;
                        tp.Right = point.X;
                    }
                    else
                    {
                        tp.Left = thumbPoint.X;
                        tp.Right = thumbPoint.Y;
                    }
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.Thumb;
                    }
                    // track
                    return ScrollBarHistTest.Track;
                }
            }
            else
            {
                if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref rect, point))
                {
                    // top arrow
                    tp.Bottom = arrowCy;
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.TopArrow;
                    }
                    // bottom arrow
                    tp.Top = rect.Bottom - arrowCy;
                    tp.Bottom = rect.Bottom;
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.BottomArrow;
                    }
                    // button
                    tp.Top = thumbPoint.X;
                    tp.Bottom = thumbPoint.Y;
                    if (WinLib.ScrollBarEx.Win32.NativeMethods.PtInRect(ref tp, point))
                    {
                        return ScrollBarHistTest.Thumb;
                    }
                    // track
                    return ScrollBarHistTest.Track;
                }
            }
            return ScrollBarHistTest.None;
        }

        private void InvalidateWindow(bool messaged)
        {
            InvalidateWindow(OwnerHWnd, messaged);
        }

        private void InvalidateWindow(IntPtr hWnd, bool messaged)
        {
            if (messaged)
            {
                WinLib.ScrollBarEx.Win32.NativeMethods.RedrawWindow(
                    hWnd,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    RDW.RDW_INTERNALPAINT);
            }
            else
            {
                WinLib.ScrollBarEx.Win32.NativeMethods.RedrawWindow(
                    hWnd,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    RDW.RDW_INVALIDATE | RDW.RDW_UPDATENOW);
            }
        }

        #endregion

        #region ScrollBarNativeWindow

        private class ScrollBarMaskControl : 
            MaskControlBase
        {
            private ScrollBarManager _owner;

            public ScrollBarMaskControl(ScrollBarManager owner)
                : base(owner.OwnerHWnd)
            {
                _owner = owner;
            }

            protected override void OnPaint(IntPtr hWnd)
            {
                _owner.DrawScrollBar(_owner.OwnerHWnd, hWnd);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _owner = null;
                }
                base.Dispose(disposing);
            }
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
                    if (_maskControl != null)
                    {
                        _maskControl.Dispose();
                        _maskControl = null;
                    }
                    _owner = null;
                }

                ReleaseHandleInternal();
            }
            _disposed = true;
        }

        #endregion
    }
}
