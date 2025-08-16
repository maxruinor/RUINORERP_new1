
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:25
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。验证类
    /// </summary>
    /*public partial class tb_BOM_SValidator:AbstractValidator<tb_BOM_S>*/
    public partial class tb_BOM_SDetailValidator : BaseValidatorGeneric<tb_BOM_SDetail>
    {
        public override void Initialize()
        {
            RuleFor(x => x.UsedQty).GreaterThan(0).WithMessage("配方明细，用量：要大于零。");
            RuleFor(x => x.UnitCost).GreaterThan(0).WithMessage("配方明细，单位成本：要大于零。");
        }
    }
}

