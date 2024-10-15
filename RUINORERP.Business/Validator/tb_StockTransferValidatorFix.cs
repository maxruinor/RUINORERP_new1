
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:37
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
    /// 调拨单-两个仓库之间的库存转移验证类
    /// </summary>
    /*public partial class tb_StockTransferValidator:AbstractValidator<tb_StockTransfer>*/
    public partial class tb_StockTransferValidator : BaseValidatorGeneric<tb_StockTransfer>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(tb_StockTransfer => tb_StockTransfer.Location_ID_from).Must(CheckForeignKeyValue).WithMessage("调出仓库:下拉选择值不正确。");
            RuleFor(tb_StockTransfer => tb_StockTransfer.Location_ID_to).Must(CheckForeignKeyValue).WithMessage("调入仓库:下拉选择值不正确。");
        }

    }

}

