
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
            //比方采购退货 的应付款单的红字，则是负数时，不用提供对应的对方的收款账号等信息

            RuleFor(x => x.Account_id).Must(CheckForeignKeyValueCanNull).When(x => x.TotalLocalPayableAmount > 0).WithMessage("公司账户:下拉选择值不正确。");
            RuleFor(tb_FM_ReceivablePayable => tb_FM_ReceivablePayable.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

            RuleFor(tb_FM_ReceivablePayable => tb_FM_ReceivablePayable.PayeeInfoID).Must(CheckForeignKeyValueCanNull).When(x => x.TotalLocalPayableAmount > 0).WithMessage("收款信息:下拉选择值不正确。");
            RuleFor(tb_FM_ReceivablePayable => tb_FM_ReceivablePayable.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

            RuleFor(tb_FM_ReceivablePayable => tb_FM_ReceivablePayable.PayeeAccountNo).MaximumMixedLength(100).When(x => x.TotalLocalPayableAmount > 0).WithMessage("收款账号:不能超过最大长度,100.");

            //审核时才需验证，所以状态为提交保存时可以忽略
            RuleFor(x => x.PayeeInfoID).NotNull().When(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款 && c.ARAPStatus > (int)ARAPStatus.待审核).When(x => x.TotalLocalPayableAmount > 0).WithMessage("收款信息:付款时，对方的收款账号等信息不能为空。");
        }
    }

}

