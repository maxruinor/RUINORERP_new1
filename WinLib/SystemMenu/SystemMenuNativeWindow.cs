using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-08-30
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */
    public class SystemMenuNativeWindow : NativeWindow, IDisposable
    {
        private Form _owner;
        private IntPtr _hMenu;
        private Dictionary<int, EventHandler> _menuClickEventList;

        public SystemMenuNativeWindow(Form owner)
        {
            _owner = owner;
            base.AssignHandle(owner.Handle);
            GetSystemMenu();
        }

        protected Dictionary<int, EventHandler> MenuClickEventList
        {
            get
            {
                if (_menuClickEventList == null)
                {
                    _menuClickEventList = new Dictionary<int, EventHandler>(10);
                }
                return _menuClickEventList;
            }
        }

        public bool InsertMenu(
            int position, 
            int id, 
            string text,
            EventHandler menuClickEvent)
        {
            return InsertMenu(
                position,
                id,
                MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_STRING,
                text,
                menuClickEvent);
        }

        public bool InertSeparator(int position)
        {
            return InsertMenu(
                position,
                0,
                MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_SEPARATOR,
                "",
                null);
        }

        public bool InsertMenu(
            int position, 
            int id, 
            MenuItemFlag flag, 
            string text,
            EventHandler menuClickEvent)
        {
            if ((flag & MenuItemFlag.MF_SEPARATOR) != MenuItemFlag.MF_SEPARATOR &&
                !ValidateID(id))
            {
                throw new ArgumentOutOfRangeException(
                    "id",
                    string.Format(
                    "菜单ID只能在{0}-{1}之间取值。", 0, 0xF000));
            }

            bool sucess = WinLib.SystemMenu.NativeMethods.InsertMenu(
               _hMenu,
               position,
               (int)flag,
               id,
               text);
            if (sucess && menuClickEvent != null)
            {
                MenuClickEventList.Add(id, menuClickEvent);
            }
            return sucess;
        }

        public bool AppendMenu(
            int id, 
            string text, 
            EventHandler menuClickEvent)
        {
            return AppendMenu(
                id,
                MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_STRING,
                text,
                menuClickEvent);
        }

        public bool AppendSeparator()
        {
            return AppendMenu(
                0,
                MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_SEPARATOR,
                "",
                null);
        }

        public bool AppendMenu(
            int id, 
            MenuItemFlag flag, 
            string text, 
            EventHandler menuClickEvent)
        {
            if ((flag & MenuItemFlag.MF_SEPARATOR) != MenuItemFlag.MF_SEPARATOR &&
                !ValidateID(id))
            {
                throw new ArgumentOutOfRangeException(
                    "id",
                    string.Format(
                    "菜单ID只能在{0}-{1}之间取值。", 0, 0xF000));
            }

            bool sucess = WinLib.SystemMenu.NativeMethods.AppendMenu(
                _hMenu,
                (int)flag,
                id,
                text);
            if (sucess && menuClickEvent != null)
            {
                MenuClickEventList.Add(id, menuClickEvent);
            }
            return sucess;
        }

        public void Revert()
        {
            WinLib.SystemMenu.NativeMethods.GetSystemMenu(base.Handle, true);
            Dispose();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinLib.SystemMenu.NativeMethods.WM_SYSCOMMAND:
                    OnWmSysCommand(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void GetSystemMenu()
        {
            _hMenu = WinLib.SystemMenu.NativeMethods.GetSystemMenu(base.Handle, false);
            if (_hMenu == IntPtr.Zero)
            {
                throw new Win32Exception("获取系统菜单失败。");
            }
        }

        private bool ValidateID(int id)
        {
            return id < 0xF000 && id > 0;
        }

        private void OnWmSysCommand(ref Message m)
        {
            int id = m.WParam.ToInt32();

            EventHandler handler;
            if (MenuClickEventList.TryGetValue(id, out handler))
            {
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
                m.Result = WinLib.SystemMenu.NativeMethods.TRUE;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            base.ReleaseHandle();
            _owner = null;
            _hMenu = IntPtr.Zero;
            if (_menuClickEventList != null)
            {
                _menuClickEventList.Clear();
                _menuClickEventList = null;
            }
        }

        #endregion
    }
}
