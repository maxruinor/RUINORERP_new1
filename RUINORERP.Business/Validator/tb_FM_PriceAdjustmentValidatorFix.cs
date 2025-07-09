
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

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 价格调整单验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentValidator:AbstractValidator<tb_FM_PriceAdjustment>*/
    public partial class tb_FM_PriceAdjustmentValidator : BaseValidatorGeneric<tb_FM_PriceAdjustment>
    {
        public override void Initialize()
        {
            RuleFor(x => x.AdjustReason).NotNull().WithMessage("调整原因:不能为空。");
            RuleFor(x => x.AdjustReason).MinimumLength(2).WithMessage("调整原因:要大于2个字。");
        }
    }

}

