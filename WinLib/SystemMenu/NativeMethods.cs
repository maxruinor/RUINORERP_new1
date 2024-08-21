using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinLib.SystemMenu
{
    internal class NativeMethods
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public static readonly IntPtr TRUE = new IntPtr(1);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool InsertMenu(
            IntPtr hMenu,
            int wPosition,
            int wFlags,
            int wIDNewItem,
            string lpNewItem);

        [DllImport("user32.dll",SetLastError = true)]
        public static extern bool AppendMenu(
            IntPtr hMenu, int wFlags, int wIDNewItem, string lpNewItem);
    }
}
