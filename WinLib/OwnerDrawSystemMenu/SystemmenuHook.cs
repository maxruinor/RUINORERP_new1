using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class OwnerDrawSystemMenuHook : IDisposable
    {
        #region Fileds

        private IntPtr _hMenu;
        private OwnerFormNativeWindow _formNativeWindow;
        private HookMenuNativeWindow _menuNativeWindow;
        private SystemMenuRenderer _renderer;

        private static WindowsHook _windowsHook;
        private static int _osVersion;
        private static WindowsType _windowsType;
        private const string MenuRealFormClassName = "#32768";

        #endregion

        #region Constructors

        static OwnerDrawSystemMenuHook()
        {
            _osVersion = Environment.OSVersion.Version.Major;
            _windowsType = SystemInformationHelper.GetWindowsVersionType();
        }

        public OwnerDrawSystemMenuHook()
        {
            _windowsHook = new WindowsHook(
                ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes.WH_CALLWNDPROC);
            _windowsHook.HookMessage += new ExOwnerDrawSystemMenu.NativeMethods.HookProc(CallWndProcHook);
        }

        #endregion

        #region Properties

        public SystemMenuRenderer Renderer
        {
            get 
            {
                if (_renderer == null)
                {
                    _renderer = new SystemMenuProfessionalRenderer();
                }
                return _renderer;
            }
            set
            {
                _renderer = value;
            }
        }

        internal bool IsUseOwnerDraw
        {
            get { 
                return _windowsType != WindowsType.Windows95 &&
                    _windowsType != WindowsType.WindowsNT4; }
        }

        internal bool IsMenuHandler
        {
            get { return _hMenu != ExOwnerDrawSystemMenu.NativeMethods.FALSE; }
        }

        #endregion

        #region HookProc Methods

        private IntPtr CallWndProcHook(
            int code, IntPtr wparam, IntPtr lparam)
        {
            if (code == (int)ExOwnerDrawSystemMenu.NativeMethods.HookCodes.HC_ACTION)
            {
                ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT cwp =
                    (ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT)Marshal.PtrToStructure(
                    lparam, 
                    typeof(ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT));

                switch (cwp.message)
                {
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_CREATE:
                        WmCreate(ref cwp);
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_INITMENUPOPUP:
                        WmInitMenupopup(ref cwp);
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_UNINITMENUPOPUP:
                        WmUnitMenupopup(ref cwp);
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_DESTROY:
                        WmDestroy(ref cwp);
                        break;
                }
            }

            return ExOwnerDrawSystemMenu.NativeMethods.FALSE;
        }

        #endregion

        #region Windows Message Methods

        private void WmDestroy(ref ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT cwp)
        {
            if (_menuNativeWindow != null)
            {
                _menuNativeWindow.Dispose();
                _menuNativeWindow = null;
            }
        }

        private void WmCreate(ref ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT cwp)
        {
            StringBuilder builder = new StringBuilder(0x40);
            ExOwnerDrawSystemMenu.NativeMethods.GetClassName(
                cwp.hWnd, builder, 0x40);
            string text1 = builder.ToString();
            if (string.Compare(text1, MenuRealFormClassName, false) == 0)
            {
                _menuNativeWindow = new HookMenuNativeWindow(this, cwp.hWnd);
            }
        }

        private void WmInitMenupopup(ref ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT cwp)
        {
            if (ExOwnerDrawSystemMenu.NativeMethods.HIWORD(cwp.lParam) != 1)
            {
                return;
            }

            _hMenu = cwp.wParam;
            IntPtr hMenu = cwp.wParam;
            ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO itemInfo =
                    new ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO();
            int itemID;
            bool success;
            int fMask = (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_STATE;

            fMask |= _osVersion > 4 ?
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_FTYPE :
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_TYPE;

            itemInfo.cbSize = Marshal.SizeOf(itemInfo);
            itemInfo.fMask = fMask;

            int itemCount = ExOwnerDrawSystemMenu.NativeMethods.GetMenuItemCount(hMenu);
            for (int item = 0; item < itemCount; item++)
            {
                success = ExOwnerDrawSystemMenu.NativeMethods.GetMenuItemInfo(
                    hMenu,
                    item,
                    true,
                    ref itemInfo);
                itemID = ExOwnerDrawSystemMenu.NativeMethods.GetMenuItemID(hMenu, item);
                int uFlags = (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_BYPOSITION |
                    (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_OWNERDRAW;
                if ((itemInfo.fType & (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_SEPARATOR) == 0)
                {
                    uFlags |= itemInfo.fState;
                }
                else
                {
                    uFlags |= (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_SEPARATOR;
                }

                ExOwnerDrawSystemMenu.NativeMethods.ModifyMenu(
                    hMenu,
                    item,
                    uFlags,
                    itemID,
                    item.ToString());
            }
            _formNativeWindow = new OwnerFormNativeWindow(this, cwp.hWnd);
        }

        private void WmUnitMenupopup(ref ExOwnerDrawSystemMenu.NativeMethods.CWPSTRUCT cwp)
        {
            if (_formNativeWindow != null)
            {
                _formNativeWindow.Dispose();
                _formNativeWindow = null;
            }
            _hMenu = ExOwnerDrawSystemMenu.NativeMethods.FALSE;
        }

        private IntPtr WmMeasureItem(ref Message m)
        {
            ExOwnerDrawSystemMenu.NativeMethods.MEASUREITEMSTRUCT lParam =
                (ExOwnerDrawSystemMenu.NativeMethods.MEASUREITEMSTRUCT)Marshal.PtrToStructure(
                m.LParam,
                typeof(ExOwnerDrawSystemMenu.NativeMethods.MEASUREITEMSTRUCT));
            if (lParam.CtlType != (int)ExOwnerDrawSystemMenu.NativeMethods.ODT.ODT_MENU)
            {
                return ExOwnerDrawSystemMenu.NativeMethods.FALSE;
            }

            ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO itemInfo =
                new ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO();
            StringBuilder menuText;
            int menuItemHeight;
            int minMenuItemWidth = 0;
            int fMask = (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_STATE;

            fMask |= _osVersion > 4 ?
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_FTYPE :
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_TYPE;

            itemInfo.cbSize = Marshal.SizeOf(itemInfo);
            itemInfo.fMask = fMask;

            ExOwnerDrawSystemMenu.NativeMethods.GetMenuItemInfo(
                _hMenu,
                lParam.itemID,
                false,
                ref itemInfo);

            int state = itemInfo.fState;
            int type = itemInfo.fType;

            if ((type & (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_SEPARATOR) != 0)
            {
                menuText = new StringBuilder("-");
                menuItemHeight = 3;
            }
            else
            {
                menuText = new StringBuilder(256);
                ExOwnerDrawSystemMenu.NativeMethods.GetMenuString(
                    _hMenu,
                    lParam.itemID,
                    menuText,
                    256,
                    (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_BYCOMMAND);
                menuItemHeight = SystemInformation.MenuHeight;
            }


            IntPtr dc = ExOwnerDrawSystemMenu.NativeMethods.GetDC(IntPtr.Zero);
            Graphics graphics = Graphics.FromHdcInternal(dc);

            MeasureItemExEventArgs e = new MeasureItemExEventArgs(
                    graphics,
                    menuText.ToString(),
                    SystemFonts.MenuFont,
                    menuItemHeight);
            try
            {
                Renderer.CalcSystemMenuItem(e);

                minMenuItemWidth = TextRenderer.MeasureText(
                    graphics,
                    "关闭(&C)     Alt + F4",
                    SystemFonts.MenuFont).Width + 
                    SystemInformation.MenuCheckSize.Width;
            }
            finally
            {
                graphics.Dispose();
            }
            ExOwnerDrawSystemMenu.NativeMethods.ReleaseDC(IntPtr.Zero, dc);
            lParam.itemHeight = e.ItemHeight;

            if (e.ItemWidth < minMenuItemWidth)
            {
                lParam.itemWidth = minMenuItemWidth;
            }
            else
            {
                lParam.itemWidth = e.ItemWidth;
            }
            Marshal.StructureToPtr(lParam, m.LParam, false);
            return ExOwnerDrawSystemMenu.NativeMethods.TRUE;
        }

        private IntPtr WmDrawItem(ref Message m)
        {
            ExOwnerDrawSystemMenu.NativeMethods.DRAWITEMSTRUCT lParam =
                (ExOwnerDrawSystemMenu.NativeMethods.DRAWITEMSTRUCT)Marshal.PtrToStructure(
                m.LParam,
                typeof(ExOwnerDrawSystemMenu.NativeMethods.DRAWITEMSTRUCT));

            if (lParam.hwndItem != _hMenu)
            {
                return ExOwnerDrawSystemMenu.NativeMethods.FALSE;
            }

            ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO itemInfo =
                new ExOwnerDrawSystemMenu.NativeMethods.MENUITEMINFO();
            StringBuilder menuText;

            itemInfo.cbSize = Marshal.SizeOf(itemInfo);
            itemInfo.fMask =
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_STATE |
                (int)ExOwnerDrawSystemMenu.NativeMethods.MIIM.MIIM_FTYPE;

            ExOwnerDrawSystemMenu.NativeMethods.GetMenuItemInfo(
                _hMenu,
                lParam.itemID,
                false,
                ref itemInfo);

            int state = itemInfo.fState;
            int type = itemInfo.fType;
            bool _isSeparator = false;
            if ((type & (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_SEPARATOR) != 0)
            {
                _isSeparator = true;
                menuText = new StringBuilder(1);
            }
            else
            {
                menuText = new StringBuilder(256);
                ExOwnerDrawSystemMenu.NativeMethods.GetMenuString(
                    _hMenu,
                    lParam.itemID,
                    menuText,
                    256,
                    (int)ExOwnerDrawSystemMenu.NativeMethods.MF.MF_BYCOMMAND);
            }

            using (Graphics graphics = Graphics.FromHdc(lParam.hDC))
            {
                Renderer.DrawSystemMenuItem(new DrawItemExEventArgs(
                    graphics,
                    menuText.ToString(),
                    SystemInformation.MenuFont,
                    lParam.rcItem.Rect,
                    lParam.itemID,
                    (DrawItemState)lParam.itemState,
                    _isSeparator));
            }

            return ExOwnerDrawSystemMenu.NativeMethods.TRUE;
        }

        private void WmNcPaint(ref Message m)
        {
            IntPtr hDc = ExOwnerDrawSystemMenu.NativeMethods.GetWindowDC(m.HWnd);
            try
            {
                using (Graphics g = Graphics.FromHdc(hDc))
                {
                    DrawNc(g, m.HWnd);
                }
            }
            finally
            {
                ExOwnerDrawSystemMenu.NativeMethods.ReleaseDC(m.HWnd, hDc);
            }
        }

        private void WmPrint(ref Message m)
        {
            try
            {
                using (Graphics g = Graphics.FromHdcInternal(m.WParam))
                {
                    DrawNc(g, m.HWnd);
                }
            }
            finally
            {
                ExOwnerDrawSystemMenu.NativeMethods.ReleaseDC(m.HWnd, m.WParam);
            }
        }

        private void WmWindowPosChanging(ref Message m)
        {
            ExOwnerDrawSystemMenu.NativeMethods.WINDOWPOS windowPos =
                (ExOwnerDrawSystemMenu.NativeMethods.WINDOWPOS)m.GetLParam(
                typeof(ExOwnerDrawSystemMenu.NativeMethods.WINDOWPOS));
            //if ((windowPos.flags & (int)ExOwnerDrawSystemMenu.NativeMethods.SWP.SWP_NOMOVE) == 0)
            //{
            //    windowPos.cx += 24;
            //}

            if ((windowPos.flags & (int)ExOwnerDrawSystemMenu.NativeMethods.SWP.SWP_NOSIZE) == 0)
            {
                windowPos.cx += Renderer.MenuBarWidth;
                Marshal.StructureToPtr(windowPos, m.LParam, false);
            }
        }

        private void WmNcCalcSize(ref Message m)
        {
            if (m.WParam == ExOwnerDrawSystemMenu.NativeMethods.FALSE)
            {
                ExOwnerDrawSystemMenu.NativeMethods.RECT rect = (ExOwnerDrawSystemMenu.NativeMethods.RECT)m.GetLParam(
                    typeof(ExOwnerDrawSystemMenu.NativeMethods.RECT));
                rect.Left += 24;
                Marshal.StructureToPtr(
                    rect,
                    m.LParam,
                    false);
            }
            else
            {
                ExOwnerDrawSystemMenu.NativeMethods.NCCALCSIZE_PARAMS lParam =
                    (ExOwnerDrawSystemMenu.NativeMethods.NCCALCSIZE_PARAMS)m.GetLParam(
                    typeof(ExOwnerDrawSystemMenu.NativeMethods.NCCALCSIZE_PARAMS));
                lParam.rectProposed.Left += 24;
                Marshal.StructureToPtr(
                    lParam,
                    m.LParam,
                    false);
            }
        }

        #endregion

        #region Helper Methods

        private void DrawNc(Graphics g, IntPtr hWnd)
        {
            Rectangle rect = Rectangle.Round(g.VisibleClipBounds);
            Rectangle clientRect = GetClientRect(hWnd, 24);

            g.ExcludeClip(clientRect);
            Renderer.DrawSystemMenuNC(
                new SystemMenuNCRenderEventArgs(g, rect));
        }

        private Rectangle GetClientRect(IntPtr hWnd, int leftMargin)
        {
            ExOwnerDrawSystemMenu.NativeMethods.RECT lpRect = new ExOwnerDrawSystemMenu.NativeMethods.RECT();
            int style = ExOwnerDrawSystemMenu.NativeMethods.GetWindowLong(
                hWnd, (int)ExOwnerDrawSystemMenu.NativeMethods.GWL.GWL_STYLE);
            int styleEx = ExOwnerDrawSystemMenu.NativeMethods.GetWindowLong(
                hWnd, (int)ExOwnerDrawSystemMenu.NativeMethods.GWL.GWL_EXSTYLE);
            ExOwnerDrawSystemMenu.NativeMethods.AdjustWindowRectEx(
                ref lpRect,
                style,
                false,
                styleEx);
            int left = -lpRect.Left;
            int right = -lpRect.Top;
            ExOwnerDrawSystemMenu.NativeMethods.GetClientRect(
                hWnd,
                ref lpRect);
            lpRect.Left += (leftMargin + left);
            lpRect.Right += (leftMargin + left);
            lpRect.Top += right;
            lpRect.Bottom += right;

            return lpRect.Rect;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (_windowsHook != null)
            {
                _windowsHook.Dispose();
            }

            if (_formNativeWindow != null)
            {
                _formNativeWindow.Dispose();
            }

            if (_menuNativeWindow != null)
            {
                _menuNativeWindow.Dispose();
            }

            _hMenu = IntPtr.Zero;
            _renderer = null;

        }

        #endregion

        #region OwnerFormNativeWindow Class

        private class OwnerFormNativeWindow : NativeWindow, IDisposable
        {
            private OwnerDrawSystemMenuHook _owner;

            public OwnerFormNativeWindow(
                OwnerDrawSystemMenuHook owner, IntPtr hForm)
                : base()
            {
                _owner = owner;
                base.AssignHandle(hForm);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_MEASUREITEM:
                        if (_owner.WmMeasureItem(ref m) == ExOwnerDrawSystemMenu.NativeMethods.TRUE)
                        {
                            m.Result = ExOwnerDrawSystemMenu.NativeMethods.TRUE;
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_DRAWITEM:
                        if (_owner.WmDrawItem(ref m) == ExOwnerDrawSystemMenu.NativeMethods.TRUE)
                        {
                            m.Result = ExOwnerDrawSystemMenu.NativeMethods.TRUE;
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }

            #region IDisposable 成员

            public void Dispose()
            {
                base.ReleaseHandle();
                _owner = null;
            }

            #endregion
        }

        #endregion

        #region HookMenuNativeWindow Class

        private class HookMenuNativeWindow : NativeWindow, IDisposable
        {
            private OwnerDrawSystemMenuHook _owner;
            private int _lastSelectedIndex = -1;

            public HookMenuNativeWindow(
                OwnerDrawSystemMenuHook owner, IntPtr hMenu)
                : base()
            {
                _owner = owner;
                base.AssignHandle(hMenu);
            }

            protected override void WndProc(ref Message m)
            {
                if (!_owner.IsMenuHandler)
                {
                    base.WndProc(ref m);
                    return;
                }

                switch (m.Msg)
                {
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_NCPAINT:
                        _owner.WmNcPaint(ref m);
                        m.Result = ExOwnerDrawSystemMenu.NativeMethods.FALSE;
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_PRINT:
                        base.WndProc(ref m);
                        if (_owner.IsUseOwnerDraw)
                        {
                            _owner.WmPrint(ref m);
                        }
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_NCCALCSIZE:
                        base.WndProc(ref m);
                        _owner.WmNcCalcSize(ref m);
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_WINDOWPOSCHANGING:
                        base.WndProc(ref m);
                        _owner.WmWindowPosChanging(ref m);
                        break;
                    case (int)ExOwnerDrawSystemMenu.NativeMethods.WindowsMessage.WM_ERASEBKGND:
                        m.Result = ExOwnerDrawSystemMenu.NativeMethods.TRUE;
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }

            #region IDisposable 成员

            public void Dispose()
            {
                base.ReleaseHandle();
                _owner = null;
            }

            #endregion
        }

        #endregion
    }
}
