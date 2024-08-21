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
using System.Reflection;
using RUINORERP.Model;

using RUINORERP.UI.Common;
using RUINORERP.Business;

namespace RUINORERP.UI.SS
{
    //没有使用
    [FormMark(typeof(MenuInit), "菜单初始化", true)]
    public partial class MenuInit : UserControl
    {
        public MenuInit()
        {
            InitializeComponent();
        }

        tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        private void MenuInit_LoadAsync(object sender, EventArgs e)
        {
            //加载当前程序集的业务窗体
            List<MenuAttrAssemblyInfo> list =UIHelper.RegisterForm();
            dataGridView1.DataSource = list;
            
            List<tb_MenuInfo> listAA = new List<tb_MenuInfo>();
            listAA = mc.Query();
        }


        private void kryptonDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows != null && this.dataGridView1.SelectedRows.Count > 0)
            {
                //this.cmbAoubtForm.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                //lblfrm.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            }
        }
        //将集合绑定到树


    }
}
