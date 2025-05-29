using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using Krypton.Workspace;
using Krypton.Navigator;
using HLH.Lib.Security;
using RUINORERP.UI.Report;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("角色个性化编辑", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.个性化设置)]
    public partial class UCUserPersonalizedEdit : UserControl
    {
        public UCUserPersonalizedEdit()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseTheForm(this);
        }


        tb_UserPersonalized Personalized = null;

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbPrinterList.SelectedItem != null)
            {
                Personalized.PrinterName = cmbPrinterList.SelectedItem.ToString();
            }
            Personalized.UseUserOwnPrinter = chkSelectPrinter.Checked;

            var ctr = MainForm.Instance.AppContext.GetRequiredService<tb_UserPersonalizedController<tb_UserPersonalized>>();
            ReturnResults<tb_UserPersonalized> rs = await ctr.SaveOrUpdate(Personalized);
            if (rs.Succeeded)
            {
                MessageBox.Show("修改成功！");
                CloseTheForm(this);
            }
            else
            {
                MessageBox.Show("修改失败，请联系管理员。");
                CloseTheForm(this);
            }


        }

        private void UCUserPersonalizedEdit_Load(object sender, EventArgs e)
        {
            Personalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            DataBindingHelper.BindData4CheckBox<tb_UserPersonalized>(Personalized, t => t.UseUserOwnPrinter, chkSelectPrinter, false);
            DataBindingHelper.BindData4CheckBox<tb_UserPersonalized>(Personalized, t => t.SelectTemplatePrint, chkSelectTemplatePrint, false);

            cmbPrinterList.Items.Clear();
            var printers = LocalPrinter.GetLocalPrinters();
            foreach (var item in printers)
            {
                cmbPrinterList.Items.Add(item);
            }
            if (Personalized.UseUserOwnPrinter.HasValue && Personalized.UseUserOwnPrinter.Value)
            {
                cmbPrinterList.SelectedIndex = cmbPrinterList.FindString(Personalized.PrinterName);
            }
        }

        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }
        }
    }
}
