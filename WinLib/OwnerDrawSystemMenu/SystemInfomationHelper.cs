using System;
using System.Collections.Generic;
using System.Text;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public static class SystemInformationHelper
    {
        public static WindowsType GetWindowsVersionType()
        {
            OperatingSystem system = Environment.OSVersion;
            WindowsType type = WindowsType.None;
            switch (system.Platform)
            {
                case PlatformID.Win32NT:
                    switch (system.Version.Major)
                    {
                        case 3:
                            break;
                        case 4:
                            type = WindowsType.WindowsNT4;
                            break;
                        case 5:
                            switch (system.Version.Minor)
                            {
                                case 0: // w2k
                                    type = WindowsType.Windows2K;
                                    break;
                                case 1: // xp
                                    type = WindowsType.WindowsXP;
                                    break;

                                default: // > xp
                                    type = WindowsType.WindowsXPP;
                                    break;
                            }
                            break;
                    }
                    break;
                case PlatformID.Win32Windows:
                    switch (system.Version.Minor)
                    {
                        case 0:
                            type = WindowsType.Windows95;
                            break;
                        case 10:
                            type = WindowsType.Windows98;
                            break;
                        case 90:
                            type = WindowsType.WindowsME;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return type;
        }
    }
}
