
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:32
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
    /// 入库单 非生产领料/退料验证类
    /// </summary>
    /*public partial class tb_StockInValidator:AbstractValidator<tb_StockIn>*/
    public partial class tb_StockInValidator : BaseValidatorGeneric<tb_StockIn>
    {

        public override void Initialize()
        {
            RuleFor(tb_StockIn => tb_StockIn.Type_ID).Must(CheckForeignKeyValue).WithMessage("入库类型:下拉选择值不正确。");
        }

    }

}

