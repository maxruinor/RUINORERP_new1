using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using RUINORERP.Global;
using Krypton.Workspace;
using Krypton.Navigator;
using HLH.Lib.Security;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("密码修改", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.个性化设置)]
    public partial class UCPwdModify : UserControl
    {
        public UCPwdModify()
        {
            InitializeComponent();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {

            if (userInfo.Password != EncryptionHelper.AesEncryptByHashKey(txtOldPwd.Text, userInfo.UserName))
            {
                MessageBox.Show("旧密码输入不正确，请重试！");
            }
            else
            {
                if (txtNewPwd.Text.Trim().Length == 0)
                {
                    MessageBox.Show("新密码不能为空，请重试！");
                }
                else if (txtNewPwd.Text != txtNewPwdConfirm.Text)
                {
                    MessageBox.Show("新密码两次输入不一致，请重试！");
                }
                else
                {

                    string newpwd = EncryptionHelper.AesEncryptByHashKey(txtNewPwd.Text, userInfo.UserName);
                    userInfo.Password = newpwd;
                    tb_UserInfoController<tb_UserInfo> ctrUser = MainForm.Instance.AppContext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();
                    bool rs = await ctrUser.UpdateAsync(userInfo);
                    if (rs)
                    {
                        MessageBox.Show("修改成功！下次登录生效。");
                        CloseTheForm(this);
                    }
                    else
                    {
                        MessageBox.Show("修改失败，请联系管理员。");
                        CloseTheForm(this);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseTheForm(this);
        }

        tb_UserInfo userInfo = null;
        private void UCPwdModify_Load(object sender, EventArgs e)
        {
            userInfo = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
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
