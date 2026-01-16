
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:10
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 成品入库单 要进一步完善验证类
    /// </summary>
    /*public partial class tb_FinishedGoodsInvValidator:AbstractValidator<tb_FinishedGoodsInv>*/
    public partial class tb_FinishedGoodsInvValidator : BaseValidatorGeneric<tb_FinishedGoodsInv>
    {
        public override void Initialize()
        {
            
        }

    }
}

