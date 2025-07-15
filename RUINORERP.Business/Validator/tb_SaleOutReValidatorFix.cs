
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/08/2023 18:58:40
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
namespace RUINORERP.Business
{
    /// <summary>
    /// 销售出库退回单验证类
    /// </summary>
    public partial class tb_SaleOutReValidator : BaseValidatorGeneric<tb_SaleOutRe>
    {
        public override void Initialize()
        {
            RuleFor(x => x.tb_SaleOutReDetails).NotNull().WithMessage("明细:不能为空。");
            RuleFor(x => x.tb_SaleOutReDetails).Must(list => list.Count > 0).WithMessage("明细不能为空。");
            RuleFor(tb_SaleOutRe => tb_SaleOutRe.CustomerVendor_ID).NotNull().WithMessage("退货客户:不能为空。");
            RuleFor(tb_SaleOutRe => tb_SaleOutRe.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("退货客户:下拉选择值不正确。");
            RuleFor(tb_SaleOutRe => tb_SaleOutRe.ReturnReason).MinimumLength(5).WithMessage("退货原因:不能小于长度,5.");
        }

    }
}

