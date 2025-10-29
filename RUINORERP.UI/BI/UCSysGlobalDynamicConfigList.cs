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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;
using RUINORERP.Global;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.SuperSocketClient;



namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("动态参数列表", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.动态参数)]
    public partial class UCSysGlobalDynamicConfigList : BaseForm.BaseListGeneric<tb_SysGlobalDynamicConfig>
    {
        public UCSysGlobalDynamicConfigList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCSysGlobalDynamicConfigEdit);
            //超级管理员才可以删除
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                toolStripButtonDelete.Visible = false;
            }
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_SysGlobalDynamicConfig, int?>> exp;
            exp = (p) => p.ValueType;
            base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetEnumDescription(typeof(ConfigValueType)));
        }

        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SysGlobalDynamicConfig).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}


        tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig> pctr = Startup.GetFromFac<tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig>>();
        protected async override Task<bool> Delete()
        {
            //动态参数不能轻易删除。需要提示，谨慎操作
            bool rs = false;
            if (MessageBox.Show("动态参数不能轻易删除。你确认要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                List<tb_SysGlobalDynamicConfig> list = new List<tb_SysGlobalDynamicConfig>();
                //如果是选择了多行。则批量删除
                foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
                {
                    list.Add(dr.DataBoundItem as tb_SysGlobalDynamicConfig);
                }
                 rs = await pctr.DeleteAsync(list.Select(c => c.ConfigID).ToArray());
                if (rs)
                {
                    Query();
                }
            }
            return rs;
        }
     

    }
}
