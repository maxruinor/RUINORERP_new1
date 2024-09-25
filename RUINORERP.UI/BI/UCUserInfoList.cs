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
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("用户信息", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCUserInfoList : BaseForm.BaseListGeneric<tb_UserInfo>
    {
        public UCUserInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCUserInfoEdit);
            /* 添加了这个。反面 checkbox 没有勾出来。
            List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
            kvlist.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist.Add(new KeyValuePair<object, string>(false, "否"));
            System.Linq.Expressions.Expression<Func<tb_UserInfo, bool?>> expr1;
            expr1 = (p) => p.is_available;// == name;
            System.Linq.Expressions.Expression<Func<tb_UserInfo, bool?>> expr2;
            expr2 = (p) => p.is_enabled;// == name;
            string colName1 = expr1.GetMemberInfo().Name;
            string colName2 = expr2.GetMemberInfo().Name;
            base.ColNameDataDictionary.TryAdd(colName1, kvlist);
            base.ColNameDataDictionary.TryAdd(colName2, kvlist);
            */
        }



    }
}
