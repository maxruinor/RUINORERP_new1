
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:48
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    public partial class tb_FM_PaymentRecordDetail : BaseEntity, ICloneable
    { 
        #region 扩展属性
        
        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        public virtual List<tb_FM_ReceivablePayable> tb_FM_ReceivablePayables { get; set; }
       
        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        public virtual List<tb_FM_PreReceivedPayment> tb_FM_PreReceivedPayments { get; set; }


        #endregion

 
    }
}

