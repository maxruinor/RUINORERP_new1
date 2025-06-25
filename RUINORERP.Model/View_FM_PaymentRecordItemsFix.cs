
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/18/2024 22:55:50
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{

    public partial class View_FM_PaymentRecordItems : BaseViewEntity
    {

        //手动添加一个属性来定义到 关联的对象 ？
        public override void InitRelatedTableTypes()
        {
            base.SetRelatedTableTypes<tb_FM_PaymentRecord>();
            base.SetRelatedTableTypes<tb_FM_PaymentRecordDetail>();
            base.SetRelatedTableTypes<tb_Currency>();
            base.SetRelatedTableTypes<tb_CustomerVendor>();
            base.SetRelatedTableTypes<tb_Employee>();
            base.SetRelatedTableTypes<tb_FM_PayeeInfo>();
            base.SetRelatedTableTypes<tb_PaymentMethod>();
            base.SetRelatedTableTypes<tb_FM_Account>();
        }

    }
}

