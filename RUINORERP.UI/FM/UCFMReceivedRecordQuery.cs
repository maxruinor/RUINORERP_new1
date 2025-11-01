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

using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;


using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.ATechnologyStack;
namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 收款记录表
    /// </summary>
    [MenuAttrAssemblyInfo("收款单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收款管理, BizType.收款单)]
    [SharedIdRequired]
    public partial class UCFMReceivedRecordQuery : UCPaymentRecordQuery, ISharedIdentification
    {
        public UCFMReceivedRecordQuery()
        {
            InitializeComponent();
            base.PaymentType = Global.EnumExt.ReceivePaymentType.收款;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;
        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
            base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            base.ChildInvisibleCols.Add(c => c.SourceBilllId);
            base.BuildInvisibleCols();
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }
        
    }
}
