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

using System.Threading;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Core;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("公司设置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCCompanyList : BaseForm.BaseListGeneric<tb_Company>
    {
        public UCCompanyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCompanyEdit);
        }


    

        protected override async Task Add()
        {
            if (ListDataSoure.Count == 0)
            {
                await base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                if (MainForm.Instance.AppContext.CanUsefunctionModules.Contains(GlobalFunctionModule.多公司经营功能))
                {
                    if (MessageBox.Show("您确定多公司模式运营吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        base.Add();
                        base.toolStripButtonModify.Enabled = false;
                    }
                }
                else
                {
                    //您没有使用多公司模式运营的权限。请购买后使用
                    MessageBox.Show("您没有使用多公司模式运营的权限。请购买后使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    base.toolStripButtonModify.Enabled = false;
                    base.toolStripButtonAdd.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 要保留一条记录
        /// </summary>
        /// <returns></returns>
        protected override Task<bool> Delete()
        {
            if (ListDataSoure.Count == 1)
            {
                return Task.FromResult(false);
            }
            else
            {
                return base.Delete();
            }
        }

    }
}
