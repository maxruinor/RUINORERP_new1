using RUINOR.WinFormsUI.Demo.ChkComboBoxDemo;
using RUINOR.WinFormsUI.Demo.CustomPictureBoxDemo;
using RUINOR.WinFormsUI.Demo.TreeGridView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmChkComboBoxDemo());
            return;
            Application.Run(new frmMain());
            Application.Run(new RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumnsDemo.Form1());
            Application.Run(new RUINOR.WinFormsUI.Demo.TreeGridView.Form1());
            
            Application.Run(new TryTreeListView.TryTreeListView());
        }
    }
}
