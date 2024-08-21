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

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("库位管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.仓库资料)]
    public partial class UCLocationList : BaseForm.BaseListGeneric<tb_Location>
    {
        public UCLocationList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCLocationEdit);

        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Location).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            //清空过滤条件，因为这个基本数据 需要显示出来 
            QueryConditionFilter.FilterLimitExpressions = new List<System.Linq.Expressions.LambdaExpression>();
        }

    }
}
