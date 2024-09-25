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

using RUINORERP.Business.Processor;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("字段管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCFieldInfoList : BaseForm.BaseListGeneric<tb_FieldInfo>
    {

        public UCFieldInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFieldInfoEdit);
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FieldInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }


        //因为tb_P4FieldInfo中引用了字段表中的信息，所以要使用导航删除。但是一定要细心

        tb_FieldInfoController<tb_FieldInfo> childctr = Startup.GetFromFac<tb_FieldInfoController<tb_FieldInfo>>();
        protected async override void Delete()
        {
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_FieldInfo buttonInfo = dr.DataBoundItem as tb_FieldInfo;
                bool rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_FieldInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{buttonInfo.FieldText},{buttonInfo.FieldName}删除成功。");
                }
            }
            Query();
        }

    }
}
