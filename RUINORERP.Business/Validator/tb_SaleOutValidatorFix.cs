
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
using System.Linq;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{

    public partial class tb_SaleOutValidator : BaseValidatorGeneric<tb_SaleOut>
    {
    
          

             public override void Initialize()
        {
            // 这里添加额外的初始化代码
            // 这里添加额外的初始化代码
            RuleFor(x => x.TotalQty).GreaterThan(0).WithMessage("总数量：要大于零。");

            RuleFor(x => x.tb_SaleOutDetails).NotNull().WithMessage("明细:不能为空。");
            RuleFor(x => x.tb_SaleOutDetails).Must(list => list.Count > 0).WithMessage("明细不能为空。");

            RuleFor(x => x.IsFromPlatform)
             .Custom((value, context) =>
             {
                 var Order = context.InstanceToValidate as tb_SaleOut;
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

            RuleFor(x => x.tb_SaleOutDetails).Must(list => list.Count > 0).WithMessage("销售明细不能为空。");

            RuleFor(x => x.TotalAmount).GreaterThan(0).When(x => x.tb_SaleOutDetails.Any(s => s.Gift == false)).WithMessage("总金额：明细中有非赠品产品时，总金额要大于零。");//可非全赠品时 总金额要大于0.订单。
           
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0).WithMessage("总金额：要大于零。");
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(x => x.tb_SaleOutDetails.Sum(c => (c.TransactionPrice) * c.Quantity) + x.FreightIncome - x.TotalCommissionAmount).WithMessage("总金额：等于成交价*数量+运费-佣金");
            //成本包含了运费成本，也要大于等于成本小计
            RuleFor(x => x.TotalCost).Equal(x => x.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity) + x.FreightCost).WithMessage("总成本：等于（成本+加定制成本）*数量。包含运费成本");

            RuleFor(x => x.PlatformOrderNo).NotEmpty().When(c => c.IsFromPlatform).WithMessage("平台单时，平台订单号不能为空。");
            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款方式:有付款的情况下，付款方式不能为空。");
            RuleFor(x => x.Notes).MinimumLength(5).When(c => c.IsCustomizedOrder == true).WithMessage("备注:定制单时下，备注内容长度必须超过5。");
            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("汇率:必须大于零。");


            RuleFor(x => x.PayStatus)
      .Custom((value, context) =>
      {
          var Order = context.InstanceToValidate as tb_SaleOut;
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

