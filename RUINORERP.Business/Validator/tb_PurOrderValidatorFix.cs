
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
using RUINORERP.Business.Config;
using RUINORERP.Model.Context;

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
            RuleFor(x => x.tb_PurOrderDetails).NotNull().WithMessage("订单明细:不能为空。");
            RuleFor(x => x.tb_PurOrderDetails).Must(list => list.Count > 0).WithMessage("订单明细不能为空。");

            RuleFor(x => x.TotalAmount).Equal(x => x.tb_PurOrderDetails.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity) + x.ShipCost).WithMessage("总金额：要等于成交价*数量，包含运费。");

            RuleFor(customer => customer.PreDeliveryDate)
           .Custom((value, context) =>
           {
               var purOrder = context.InstanceToValidate as tb_PurOrder;
               if (purOrder != null)
               {

                   //根据配置判断预交日期是不是必须填写
                   //实际情况是 保存时可能不清楚交期，保存后截图发给供应商后才知道。这时提交才要求
                   if (ValidatorConfig.预交日期必填)
                   {
                       if (purOrder.PreDeliveryDate == null || purOrder.PreDeliveryDate.HasValue)
                       {
                           context.AddFailure("预交日期：必须填写。");
                       }
                       RuleFor(x => x.PreDeliveryDate).NotNull().WithMessage("预交日期：必须填写。");
                       RuleFor(x => x.PreDeliveryDate).NotEmpty().When(c => c.PreDeliveryDate.HasValue).WithMessage("预交日期：必须填写。");
                   }
                   //string propertyName = context.PropertyName;
                   //if (!BeUniqueName(propertyName, value))
                   //{
                   //    context.AddFailure("客户名称不能重复。");
                   //}
               }
           });


            //RuleFor(tb_PurOrder => tb_PurOrder.cu).NotNull().WithMessage("付款状态:不能为空。");
            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).WithMessage("付款方式:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款方式:有付款的情况下，付款方式不能为空。");


            RuleFor(x => x.PayStatus)
             .Custom((value, context) =>
             {
                 var purOrder = context.InstanceToValidate as tb_PurOrder;
                 if (purOrder != null)
                 {
                     if (purOrder.PayStatus == (int)PayStatus.全额预付 || purOrder.PayStatus == (int)PayStatus.部分预付)
                     {
                         if (purOrder.PayeeInfoID == null)
                         {
                             context.AddFailure($"收款信息：{(PayStatus)purOrder.PayStatus}款时，请填写对方的收款账号。");
                         }
                         else if (purOrder.PayeeInfoID.Value <= 0)
                         {
                             context.AddFailure($"收款信息：{(PayStatus)purOrder.PayStatus}款时，下拉选择值不正确。");
                         }
                     }
                     else if (purOrder.PayStatus == (int)PayStatus.未付款)
                     {
                         //未付款时
                         if (purOrder.PayeeInfoID <= 0)
                         {
                             purOrder.PayeeInfoID = null;
                         }
                     }
                 }
             });


            // 这里添加额外的初始化代码
            RuleFor(x => x.PreDeliveryDate).GreaterThan(x => x.PurDate)
                .When(c => c.PreDeliveryDate.HasValue)
                .WithMessage("预交日期：必须大于采购日期。");

        }
    }





}

