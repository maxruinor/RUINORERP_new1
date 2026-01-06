﻿using System;
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



using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.ATechnologyStack;
namespace RUINORERP.UI.FM
{
    /// <summary>
    /// //付款记录表
    /// </summary>
    [MenuAttrAssemblyInfo("付款单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.付款管理, BizType.付款单)]
    [SharedIdRequired]
    public partial class UCFMPaymentRecordQuery : UCPaymentRecordQuery, ISharedIdentification
    {
        public UCFMPaymentRecordQuery()
        {
            InitializeComponent();
            base.PaymentType = Global.EnumExt.ReceivePaymentType.付款;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;

        public override void BuildInvisibleCols()
        {
            base.ChildInvisibleCols.Add(c => c.SourceBilllId);
            base.MasterInvisibleCols.Add(c => c.SourceBillNos);
            base.BuildInvisibleCols();
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }
    }
}
