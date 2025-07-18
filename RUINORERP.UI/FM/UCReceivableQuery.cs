﻿using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("应收款单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收款管理, BizType.应收款单)]
    [SharedIdRequired]
    public partial class UCReceivableQuery : UCReceivablePayableQuery, ISharedIdentification
    {
        public UCReceivableQuery()
        {
            InitializeComponent(); 
            base.PaymentType = Global.EnumExt.ReceivePaymentType.收款;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;

    }
}
