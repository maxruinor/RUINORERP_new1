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

    [MenuAttrAssemblyInfo("按钮管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCButtonInfoList : BaseForm.BaseListGeneric<tb_ButtonInfo>
    {
        public UCButtonInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCButtonInfoEdit);
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ButtonInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }


        //因为tb_P4Button中引用了按钮表中的信息，所以要使用导航删除。但是一定要细心

        tb_ButtonInfoController<tb_ButtonInfo> childctr = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
        protected async override void Delete()
        {
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_ButtonInfo buttonInfo = dr.DataBoundItem as tb_ButtonInfo;
                bool rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_ButtonInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{buttonInfo.BtnText},{buttonInfo.BtnName}删除成功。");
                }
            }
            Query();
        }


    }
}
