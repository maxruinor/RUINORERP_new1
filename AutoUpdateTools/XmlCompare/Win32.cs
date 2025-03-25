using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools.XmlCompare
{
    public static class Win32
    {
        public const int WM_VSCROLL = 0x115;
        public const int WM_HSCROLL = 0x114;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_VERT = 1;
        public const int SB_HORZ = 0;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);


        public const int EM_GETSCROLLPOS = 0x4DD;
        public const int EM_SETSCROLLPOS = 0x4DE;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref POINT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    }
}
