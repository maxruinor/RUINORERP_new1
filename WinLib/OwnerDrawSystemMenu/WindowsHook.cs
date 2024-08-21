using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    internal class WindowsHook : IDisposable
    {
        #region Fields

        private bool _hookInitialized = false;
        private IntPtr _hookHandle = IntPtr.Zero;
        private GCHandle _procedureHandle;
        private ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes _hookType;
        private HookScope _hookScope = HookScope.Local;

        #endregion

        #region Constructors

        public WindowsHook(ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes type)
        {
            InternalConstructor(type, HookScope.Local);
        }

        public WindowsHook(
            ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes type,
            HookScope scope)
        {
            InternalConstructor(type, scope);
        }

        private void InternalConstructor(
            ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes type, 
            HookScope scope)
        {
            _hookScope = scope;
            _hookType = type;
            if (_hookScope == HookScope.Local)
            {
                if (_hookType == ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes.WH_JOURNALRECORD ||
                    _hookType == ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes.WH_JOURNALPLAYBACK ||
                    _hookType == ExOwnerDrawSystemMenu.NativeMethods.WindowsHookCodes.WH_SYSMSGFILTER)
                {
                    throw new Exception("Hook: " + _hookType.ToString() +
                        " cannot have local scope.");
                }
            }

            StartHook();
        }

        ~WindowsHook()
        {
            Dispose(false);
        }

        #endregion

        #region Event

        public event ExOwnerDrawSystemMenu.NativeMethods.HookProc HookMessage;

        #endregion

        #region Virtual Methods

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_hookInitialized)
                {
                    EndHook();
                }
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion

        #region Private Methods

        private void StartHook()
        {
            ExOwnerDrawSystemMenu.NativeMethods.HookProc hookProc = 
                new ExOwnerDrawSystemMenu.NativeMethods.HookProc(OnHookMessage);
            _procedureHandle = GCHandle.Alloc(hookProc);

            IntPtr hThisDLLInstance = IntPtr.Zero;
            int threadID = 0;
            if (_hookScope == HookScope.Global)
            {
                hThisDLLInstance = Marshal.GetHINSTANCE(GetType().Module);
                Debug.Assert(hThisDLLInstance != IntPtr.Zero);
            }
            else
            {
                threadID = ExOwnerDrawSystemMenu.NativeMethods.GetCurrentThreadId();
            }

            _hookHandle = ExOwnerDrawSystemMenu.NativeMethods.SetWindowsHookEx(
                _hookType, hookProc, hThisDLLInstance, threadID);
            if (_hookHandle == IntPtr.Zero)
            {
                throw new SecurityException("Failed to create Windows Hook");
            }

            _hookInitialized = true;
        }

        private void EndHook()
        {	
            ExOwnerDrawSystemMenu.NativeMethods.UnhookWindowsHookEx(_hookHandle);
            _procedureHandle.Free();
            _hookHandle = IntPtr.Zero;
        }

        private IntPtr OnHookMessage(int code, IntPtr wparam, IntPtr lparam)
        {
            if (HookMessage != null)
            {
                if (HookMessage(code, wparam, lparam) == ExOwnerDrawSystemMenu.NativeMethods.TRUE)
                {
                    return ExOwnerDrawSystemMenu.NativeMethods.TRUE;
                }
            }

            return ExOwnerDrawSystemMenu.NativeMethods.CallNextHookEx(_hookHandle, code, wparam, lparam);
        }

        #endregion
    }
}
