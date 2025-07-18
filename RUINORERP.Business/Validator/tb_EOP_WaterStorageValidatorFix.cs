
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/18/2025 10:33:40
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
    /// 蓄水登记表验证类
    /// </summary>
    /*public partial class tb_EOP_WaterStorageValidator:AbstractValidator<tb_EOP_WaterStorage>*/
    public partial class tb_EOP_WaterStorageValidator : BaseValidatorGeneric<tb_EOP_WaterStorage>
    {

        public override void Initialize()
        {
            RuleFor(t => t.PlatformOrderNo).MinimumLength(3).WithMessage("平台单:不能小于长度3");
        }

    }

}

