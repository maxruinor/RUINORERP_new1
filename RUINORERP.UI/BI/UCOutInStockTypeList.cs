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
using System.Collections.Concurrent;
using System.Linq.Expressions;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo( "出入库类型", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.仓库资料)]
    public partial class UCOutInStockTypeList : BaseForm.BaseListGeneric<tb_OutInStockType>
    {
        public UCOutInStockTypeList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCOutInStockTypeEdit);
            List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
            kvlist.Add(new KeyValuePair<object, string>(false, "出库"));
            kvlist.Add(new KeyValuePair<object, string>(true, "入库"));

            //Expression<Func<Person, bool>> expr;
            //expr = new SimplifyExpression().Visit(expr);  
            //string nameToFind = "Smith";
            //Expression<Func<tb_OutInStockType, bool?>> colexp = new Expression<Func<tb_OutInStockType, bool?>>();
            //这样暂时是为了检测字段变化，这样编译时就会检查到
            Expression<Func<tb_OutInStockType, bool?>> expr;
            expr = (p) => p.OutIn;// == name;
            var mb = expr.GetMemberInfo();
            string colName = mb.Name;
            base.ColNameDataDictionary.TryAdd(colName, kvlist);
        }

        

 
    }
}
