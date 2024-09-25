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

using RUINORERP.Model.Base;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("员工管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.行政资料)]
    public partial class UCEmployeeList : BaseForm.BaseListGeneric<tb_Employee>
    {
        public UCEmployeeList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCEmployeeEdit);

            List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
            kvlist.Add(new KeyValuePair<object, string>(true, "男"));
            kvlist.Add(new KeyValuePair<object, string>(false, "女"));
            Expression<Func<tb_Employee, bool?>> expr;
            expr = (p) => p.Gender;// == name;
            var mb = expr.GetMemberInfo();
            string colName = mb.Name;
            base.ColNameDataDictionary.TryAdd(colName, kvlist);


            List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr1;
            expr1 = (p) => p.Is_available;// == name;
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr2;
            expr2 = (p) => p.Is_enabled;// == name;
            string colName1 = expr1.GetMemberInfo().Name;
            string colName2 = expr2.GetMemberInfo().Name;
            base.ColNameDataDictionary.TryAdd(colName1, kvlist1);
            base.ColNameDataDictionary.TryAdd(colName2, kvlist1);

        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Employee).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            QueryConditionFilter.FilterLimitExpressions.Clear();
        }

    }
}
