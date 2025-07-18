
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2025 19:17:36
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using SharpYaml.Tokens;
using System.Runtime.Remoting.Contexts;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 单位换算表验证类
    /// </summary>
    public partial class tb_Unit_ConversionValidator : BaseValidatorGeneric<tb_Unit_Conversion>
    {
        public override void Initialize()
        {
            //来源和目标不能相同
            RuleFor(x => x.Source_unit_id)
             .Custom((value, context) =>
             {
                 var entity = context.InstanceToValidate as tb_Unit_Conversion;

                 // 确保实体不为null  并且是新增时才判断
                 if (entity != null && entity.UnitConversion_ID == 0)
                 {
                     string propertyName = context.PropertyPath;

                     if (entity.Source_unit_id == entity.Target_unit_id)
                     {
                         context.AddFailure("来源单位和目标单位不能相同。");
                     }

                 }
             });
        }
    }

}

