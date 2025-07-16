
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:43:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Common.Helper;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 维修物料明细表验证类
    /// </summary>
    /*public partial class tb_AS_RepairOrderMaterialDetailValidator:AbstractValidator<tb_AS_RepairOrderMaterialDetail>*/
    public partial class tb_AS_RepairOrderMaterialDetailValidator : BaseValidatorGeneric<tb_AS_RepairOrderMaterialDetail>
    {
        public override void Initialize()
        {
            RuleFor(x => x.SubtotalTransAmount).GreaterThan(0).When(x => x.Gift == false).WithMessage("单价：明细中有非赠品产品时，要大于零。");
            RuleFor(x => x.ShouldSendQty).GreaterThan(0).WithMessage("数量：要大于零。");
            RuleFor(x => x.SubtotalCost).GreaterThanOrEqualTo(0).WithMessage("总金额：要大于零。");
            RuleFor(x => x.SubtotalTransAmount).Equal(c => (c.UnitPrice) * c.ShouldSendQty).WithMessage("总金额，成交小计：要等于成交价*数量。");

        }

    }
}

