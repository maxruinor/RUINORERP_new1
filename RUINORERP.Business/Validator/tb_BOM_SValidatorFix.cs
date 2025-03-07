
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:25
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。验证类
    /// </summary>
    /*public partial class tb_BOM_SValidator:AbstractValidator<tb_BOM_S>*/
    public partial class tb_BOM_SValidator : BaseValidatorGeneric<tb_BOM_S>
    {
        public override void Initialize()
        {
            List<string> errorList = new List<string>();

            RuleFor(tb_BOM_S => tb_BOM_S.BOM_Name).MinimumLength(2).WithMessage("配方名称:长度要大于2。");

            RuleFor(x => x.TotalMaterialCost).GreaterThan(0).WithMessage("总物料费用:要大于零。");
            RuleFor(x => x.TotalMaterialQty).GreaterThan(0).WithMessage("用料总量:要大于零。");
            RuleFor(x => x.OutputQty).GreaterThan(0).WithMessage("产出量:要大于零。");

            RuleFor(x => x.PeopleQty).PrecisionScale(15, 5, true).WithMessage("人数:小数位不能超过5。");
            RuleFor(x => x.WorkingHour).PrecisionScale(15, 5, true).WithMessage("工时:小数位不能超过5。");
            RuleFor(x => x.MachineHour).PrecisionScale(15, 5, true).WithMessage("机时:小数位不能超过5。");
            RuleFor(x => x.DailyQty).PrecisionScale(18, 0, true).WithMessage("日产量:小数位不能超过0。");

            RuleFor(x => x.OutApportionedCost).PrecisionScale(19, 4, true).WithMessage("外发分摊费用:小数位不能超过4。");
            RuleFor(x => x.TotalOutManuCost).PrecisionScale(19, 4, true).WithMessage("外发费用:小数位不能超过4。");
            RuleFor(x => x.OutProductionAllCosts).PrecisionScale(19, 4, true).WithMessage("外发总成本:小数位不能超过4。");

            RuleFor(x => x.SelfApportionedCost).PrecisionScale(19, 4, true).WithMessage("自制分摊费用:小数位不能超过4。");
            RuleFor(x => x.TotalSelfManuCost).PrecisionScale(19, 4, true).WithMessage("自制费用:小数位不能超过4。");
            RuleFor(x => x.SelfProductionAllCosts).PrecisionScale(19, 4, true).WithMessage("自产总成本:小数位不能超过4。");

            RuleFor(x => x.TotalOutManuCost).
            Custom((contact, context) =>
            {
                var bom = context.InstanceToValidate as tb_BOM_S; // 假设你的实体类名为Customer
                bool isout = bom.TotalOutManuCost > 0;
                bool isselt = bom.TotalSelfManuCost > 0;
                if (!isout && !isselt)
                {
                    if (!errorList.Contains("委托外发费用、自行制造费用、至少填写一个。"))
                    {
                        context.AddFailure("委托外发费用、自行制造费用、至少填写一个。");
                    }
                   
                }
            });


            RuleFor(x => x.TotalSelfManuCost).
            Custom((contact, context) =>
            {
                var bom = context.InstanceToValidate as tb_BOM_S; // 假设你的实体类名为Customer
                bool isout = bom.TotalOutManuCost > 0;
                bool isselt = bom.TotalSelfManuCost > 0;
                if (!isout && !isselt)
                {
                    if (!errorList.Contains("委托外发费用、自行制造费用、至少填写一个。"))
                    {
                        context.AddFailure("委托外发费用、自行制造费用、至少填写一个。");
                    }
                }
            });



            RuleFor(x => x.OutProductionAllCosts).
          Custom((contact, context) =>
          {
              var bom = context.InstanceToValidate as tb_BOM_S; // 假设你的实体类名为Customer
              bool isout = bom.OutProductionAllCosts > 0;
              bool isselt = bom.SelfProductionAllCosts > 0;
              if (!isout && !isselt)
              {
                  if (!errorList.Contains("委托外发、自产制造的总费用至少填写一个。"))
                  {
                      context.AddFailure("委托外发、自产制造的总费用至少填写一个。");
                  }
              }
          });


            RuleFor(x => x.SelfProductionAllCosts).
            Custom((contact, context) =>
            {
                var bom = context.InstanceToValidate as tb_BOM_S; // 假设你的实体类名为Customer
                bool isout = bom.OutProductionAllCosts > 0;
                bool isselt = bom.SelfProductionAllCosts > 0;
                if (!isout && !isselt)
                {
                    if (!errorList.Contains("委托外发、自产制造的总费用至少填写一个。"))
                    {
                        context.AddFailure("委托外发、自产制造的总费用至少填写一个。");
                    }
                }
            });


        }
    }
}

