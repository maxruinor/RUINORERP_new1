
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/08/2024 01:29:55
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960验证类
    /// </summary>
    /*public partial class tb_ManufacturingOrderValidator:AbstractValidator<tb_ManufacturingOrder>*/
    public partial class tb_ManufacturingOrderValidator : BaseValidatorGeneric<tb_ManufacturingOrder>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            // 如果选择了外发，则必须填写外发加工商
            RuleFor(x => x.CustomerVendor_ID_Out).NotNull().When(x => x.IsOutSourced == true).WithMessage("选择外发时，必须要选择外发的工厂。");
            RuleFor(x => x.CustomerVendor_ID_Out).GreaterThan(0).When(x => x.IsOutSourced == true).WithMessage("选择外发时，必须要选择外发的工厂。");


            RuleFor(x => x.PreStartDate)
          .Custom((value, context) =>
          {
              var validEntity = context.InstanceToValidate as tb_ManufacturingOrder;
             // if (validEntity != null && validEntity.DataStatus == (int)DataStatus.新建)
             // {
                  //实际情况是 保存时可能不清楚交期。这时提交才要求
                  if (ValidatorConfig.CurrentValue.预开工日期必填)
                  {
                      RuleFor(x => x.PreStartDate).NotNull().WithMessage("预开工日期：必须填写。");
                      RuleFor(x => x.PreStartDate).NotEmpty().When(c => c.PreStartDate.HasValue).WithMessage("预开工日期：必须填写。");
                  }
             // }
          });


            RuleFor(x => x.PreEndDate)
            .Custom((value, context) =>
            {
              var validEntity = context.InstanceToValidate as tb_ManufacturingOrder;
                // if (validEntity != null && validEntity.DataStatus == (int)DataStatus.新建)
                // {
                //实际情况是 保存时可能不清楚交期。这时提交才要求
              if (ValidatorConfig.CurrentValue.预完工日期必填)
              {
                  RuleFor(x => x.PreEndDate).NotNull().WithMessage("预完工日期：必须填写。");
                  RuleFor(x => x.PreEndDate).NotEmpty().When(c => c.PreStartDate.HasValue).WithMessage("预完工日期：必须填写。");
              }
                // }
            });




        }
    }

}

