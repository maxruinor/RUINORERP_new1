
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
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Global;
using System.Linq;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据验证类
    /// </summary>
    /*public partial class tb_PurEntryReValidator:AbstractValidator<tb_PurEntry>*/
    public partial class tb_PurEntryReValidator : BaseValidatorGeneric<tb_PurEntryRe>
    {

        //  /// <summary>
        //  /// 可配置性全局参数
        //  /// </summary>
        //public readonly IOptionsMonitor<SystemGlobalconfig> Globalconfig;
        //public tb_PurEntryReValidator(IOptionsMonitor<SystemGlobalconfig> _Globalconfig)
        //{
        //    Globalconfig = _Globalconfig;
        //}


        public override void Initialize()
        {
            RuleFor(x => x.TotalAmount).GreaterThan(0).When(x => x.tb_PurEntryReDetails.Any(c => c.IsGift == null || (c.IsGift.HasValue && !c.IsGift.Value))).WithMessage("总金额：要大于零。");
            RuleFor(x => x.TotalAmount).Equal(x => x.tb_PurEntryReDetails.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity)).WithMessage("总金额：要等于成交价*数量。");

            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款方式:有付款的情况下，付款方式不能为空。");
        }
    }





}

