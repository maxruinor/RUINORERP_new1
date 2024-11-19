
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:01
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 包装规格表验证类
    /// </summary>
    /*public partial class tb_PackingValidator:AbstractValidator<tb_Packing>*/
    public partial class tb_PackingValidator : BaseValidatorGeneric<tb_Packing>
    {
        public override void Initialize()
        {
            // 确保三个字段中至少有一个有值
            RuleFor(x => x.ProdBaseID)
                .NotNull().WithMessage("产品:下拉选择值不正确。")
                .When(x => !x.ProdDetailID.HasValue && !x.BundleID.HasValue);

            RuleFor(x => x.ProdDetailID)
                .NotNull().WithMessage("产品详情:下拉选择值不正确。")
                .When(x => !x.ProdBaseID.HasValue && !x.BundleID.HasValue);

            RuleFor(x => x.BundleID)
                .NotNull().WithMessage("套装组合:下拉选择值不正确。")
                .When(x => !x.ProdBaseID.HasValue && !x.ProdDetailID.HasValue);

        }
    }
 

}

