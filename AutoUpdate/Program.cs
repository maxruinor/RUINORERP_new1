﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoUpdate
{

    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        //使用实例：
        /*
         强制无介面更新  还需要优化 2022-6-23
         * 
         
         
         
                AutoUpdate.FrmUpdate fu = new AutoUpdate.FrmUpdate();
                if (fu.CheckHasUpdates())
                {
                        fu.UpdateAndDownLoadFile();

                        fu.ApplyApp();//应用copy
                }
        fu.SatrtEntryPointExe();s
                //else
                //{
                //        System.Windows.Forms.Application.Run(new frmMain());
                //}
         
         
         
         
         
         =========================
         * 有介面更新
               AutoUpdate.FrmUpdate fu = new AutoUpdate.FrmUpdate();
                if (fu.CheckHasUpdates())
                {
                    Process.Start("AutoUpdate.exe");
                    // System.Windows.Forms.Application.Run(new AutoUpdate.FrmUpdate());
                }
                else
                {
                    System.Windows.Forms.Application.Run(new frmMain());
                }
         
         
         
         */
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmUpdate frm = new FrmUpdate();
            Application.Run(frm);
            return frm.mainResult;
        }

      
    }
}