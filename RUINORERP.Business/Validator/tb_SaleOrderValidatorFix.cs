// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:24:06
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using Castle.Core.Resource;
using System.Linq;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 箱规表验证类
    /// </summary>
    public partial class tb_SaleOrderValidator : BaseValidatorGeneric<tb_SaleOrder>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.IsFromPlatform)
             .Custom((value, context) =>
             {
                 var Order = context.InstanceToValidate as tb_SaleOrder;
                 if (Order != null)
                 {
                     //根据配置判断平台单是不是必须勾选
                     //实际情况是 保存时可能不清楚交期，保存后截图发给供应商后才知道。这时提交才要求
                     if (ValidatorConfig.IsFromPlatform)
                     {
                         //只是默认值。不能强制选择
                         //if (Order.IsFromPlatform)
                         //{
                         //    context.AddFailure("平台单：必须选择。");
                         //}
                     }
                 }
             });

            RuleFor(x => x.tb_SaleOrderDetails).NotNull().WithMessage("销售明细:不能为空。");
            RuleFor(x => x.tb_SaleOrderDetails).Must(list => list.Count > 0).WithMessage("销售明细不能为空。");

            RuleFor(x => x.TotalAmount).GreaterThan(0).When(x => x.tb_SaleOrderDetails.Any(s => s.Gift == false)).WithMessage("总金额：明细中有非赠品产品时，总金额要大于零。");//可非全赠品时 总金额要大于0.订单。
            RuleFor(x => x.TotalQty).GreaterThan(0).WithMessage("总数量：要大于零。");
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0).WithMessage("总金额：要大于零。");
            RuleFor(x => x.TotalAmount).Equal(x => x.tb_SaleOrderDetails.Sum(c => (c.TransactionPrice) * c.Quantity) + x.FreightIncome).WithMessage("总金额，成交小计：要等于成交价*数量，包含运费。");
            RuleFor(x => x.TotalCost).Equal(x => x.tb_SaleOrderDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity)).WithMessage($"总成本，成本小计：要等于（成本价+定制成本）*数量。");

            // 完善平台单和平台订单号的关联验证，只提示一次
            RuleFor(x => x)
                .Custom((saleOrder, context) =>
                {
                    bool hasPlatformOrderNo = !string.IsNullOrEmpty(saleOrder.PlatformOrderNo);
                    bool isFromPlatform = saleOrder.IsFromPlatform;

                    // 如果勾选了平台单，平台订单号不能为空
                    if (isFromPlatform && !hasPlatformOrderNo)
                    {
                        context.AddFailure("PlatformOrderNo", "平台单时，平台订单号不能为空。");
                    }
                    // 如果填写了平台订单号，必须勾选平台单
                    else if (hasPlatformOrderNo && !isFromPlatform)
                    {
                        context.AddFailure("IsFromPlatform", "平台订单号不为空时，【平台单】必需勾选。");
                    }
                });

            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款方式:有付款的情况下，付款方式不能为空。");
            RuleFor(x => x.Notes).MinimumLength(5).When(c => c.IsCustomizedOrder == true).WithMessage("备注:定制单时下，备注内容长度必须超过5。");
            RuleFor(x => x.Account_id).GreaterThan(0).When(c => c.Account_id.HasValue).WithMessage("付款账号:请选择正确的付款账号。");

            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("汇率:必须大于零。");

            //RuleFor(tb_SaleOrder => tb_SaleOrder.ProjectGroup_ID).NotNull().WithMessage("项目组:不能为空。");

            RuleFor(x => x.ProjectGroup_ID)
             .Custom((value, context) =>
             {
                 var Order = context.InstanceToValidate as tb_SaleOrder;
                 if (Order != null)
                 {
                     //根据配置判断
                     if (ValidatorConfig.NeedInputProjectGroup)
                     {
                         if (Order.ProjectGroup_ID == null)
                         {
                             context.AddFailure("项目组:不能为空。");
                         }
                     }
                 }
             });

            RuleFor(x => x.PayStatus)
             .Custom((value, context) =>
      {
          var Order = context.InstanceToValidate as tb_SaleOrder;
          if (Order != null)
          {
              //根据配置判断平台单是不是必须勾选
              //实际情况是 保存时可能不清楚交期，保存后截图发给供应商后才知道。这时提交才要求
              if (ValidatorConfig.IsFromPlatform)
              {
                  //只是默认值。不能强制选择
                  //if (Order.IsFromPlatform)
                  //{
                  //    context.AddFailure("平台单：必须选择。");
                  //}
              }
          }
      });
        }
    }
}