
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
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
    /// 系统全局动态配置表 行转列验证类
    /// </summary>
    /*public partial class tb_SysGlobalDynamicConfigValidator:AbstractValidator<tb_SysGlobalDynamicConfig>*/
    public partial class tb_SysGlobalDynamicConfigValidator : BaseValidatorGeneric<tb_SysGlobalDynamicConfig>
    {

        //配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public tb_SysGlobalDynamicConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config)
        {

            ValidatorConfig = config;

            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.ConfigKey).MaximumMixedLength(255).WithMessage("配置项:不能超过最大长度,255.");
            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.ConfigKey).NotEmpty().WithMessage("配置项:不能为空。");

            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.ConfigValue).NotEmpty().WithMessage("配置值:不能为空。");

            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.Description).MaximumMixedLength(200).WithMessage("配置描述:不能超过最大长度,200.");
            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.Description).NotEmpty().WithMessage("配置描述:不能为空。");

            //***** 
            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.ValueType).NotNull().WithMessage("配置项的值类型:不能为空。");

            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.ConfigType).MaximumMixedLength(100).WithMessage("配置类型:不能超过最大长度,100.");

            //有默认值


            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


            RuleFor(tb_SysGlobalDynamicConfig => tb_SysGlobalDynamicConfig.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

            Initialize();
        }








        private bool CheckForeignKeyValue(long ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }

        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;

        }
    }

}

