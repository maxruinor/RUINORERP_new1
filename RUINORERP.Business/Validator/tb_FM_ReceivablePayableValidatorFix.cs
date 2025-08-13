
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/23/2025 14:28:32
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Global.EnumExt;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 应收应付表验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentValidator:AbstractValidator<tb_FM_PriceAdjustment>*/
    public partial class tb_FM_ReceivablePayableValidator : BaseValidatorGeneric<tb_FM_ReceivablePayable>
    {
        public override void Initialize()
        {
            //审核时才需验证，所以状态为提交保存时可以忽略
            RuleFor(x => x.PayeeInfoID).NotNull().When(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款 && c.ARAPStatus > (int)ARAPStatus.待审核).WithMessage("收款信息:付款时，对方的收款账号等信息不能为空。");
        }
    }

}

