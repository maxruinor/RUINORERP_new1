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
using TransInstruction;
using System.Threading;
using RUINORERP.Global;
using SourceGrid2.Win32;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;
using RUINORERP.Common.Extensions;
using System.Reflection;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.Business.CommService;
using RUINORERP.UI.CommonUI;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("编号规则设置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCsysBillNoRuleList : BaseForm.BaseListGeneric<tb_sys_BillNoRule>
    {
        public UCsysBillNoRuleList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCsysBillNoRuleEdit);
            ////固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            //System.Linq.Expressions.Expression<Func<DateFormat, int?>> exp;
            //exp = (p) => p.data;
            //base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(DateFormat)));

            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.DateFormat, typeof(DateFormat));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.ResetMode, typeof(ResetMode));
        }

        public override async Task<List<tb_sys_BillNoRule>> Save()
        {
            List<tb_sys_BillNoRule> list = new List<tb_sys_BillNoRule>();
            list = await base.Save();
            MainForm.Instance.AppContext.BillNoRules = list;
            return list.ToList();
        }
    }
}
