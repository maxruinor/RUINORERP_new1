
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:13
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
    /// 产品拆分单验证类
    /// </summary>
    /*public partial class tb_ProdSplitValidator:AbstractValidator<tb_ProdSplit>*/
    public partial class tb_ProdSplitValidator : BaseValidatorGeneric<tb_ProdSplit>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.SplitParentQty).GreaterThan(0).WithMessage("分割单的母件数量：必须大于零。");
            RuleFor(x => x.SplitChildTotalQty).GreaterThan(0).WithMessage("子件总数量：必须大于零。");
        }
    }

}

