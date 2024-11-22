
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:22
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据验证类
    /// </summary>
    /*public partial class tb_PurOrderValidator:AbstractValidator<tb_PurOrder>*/
    public partial class tb_PurOrderValidator : BaseValidatorGeneric<tb_PurOrder>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.PreDeliveryDate).GreaterThan(x => x.PurDate)
                .When(c => c.PreDeliveryDate.HasValue)
                .WithMessage("预交日期：必须大于采购日期。");

        }
    }





}

