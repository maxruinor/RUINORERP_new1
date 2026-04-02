
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：2026/04/02
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;
using RUINORERP.Business.Validator;
using RUINORERP.Global.EnumExt;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单验证类 - 扩展验证逻辑
    /// 支持验证层值转换：将UI传递的-1值自动转换为null
    /// 根据收付类型动态显示"收款账户"或"付款账户"
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentValidator : BaseValidatorGeneric<tb_FM_PreReceivedPayment>
    {
        /// <summary>
        /// 初始化自定义验证规则
        /// 在模板生成的验证规则基础上，添加智能验证逻辑
        /// </summary>
        public override void Initialize()
        {
            // ========================================
            // 公司账户字段验证 - 使用新的智能验证规则
            // 功能说明：
            // 1. 自动将UI传递的-1或0值转换为null
            // 2. 根据全局配置【预收付款单账户必填】决定是否允许为空
            // 3. 默认值为false，表示预收付款单的账户可以为空
            // 4. 根据ReceivePaymentType动态显示"收款账户"或"付款账户"
            // ========================================
            
            // 应用值转换 - 将-1或0转换为null
            this.ApplyPropertyTransformation(x => x.Account_id, ValueTransformConfig.Default);
            
            // 添加外键值验证，根据收付类型动态显示字段名称
            RuleFor(x => x.Account_id)
                .Must((instance, value) =>
                {
                    // 如果值为null，根据是否必填决定验证结果
                    bool isRequired = ValidatorConfig?.预收付款单账户必填 ?? false;
                    if (!value.HasValue)
                    {
                        return !isRequired;
                    }
                    // 检查是否为有效的非零非负一值
                    return value.Value != 0 && value.Value != -1;
                })
                .WithMessage(instance => 
                {
                    // 根据收付类型动态返回字段名称
                    string fieldName = GetAccountFieldName(instance);
                    return $"{fieldName}:下拉选择值不正确。";
                });

            // 如果必填，添加非空验证
            bool isAccountRequired = ValidatorConfig?.预收付款单账户必填 ?? false;
            if (isAccountRequired)
            {
                RuleFor(x => x.Account_id)
                    .NotNull()
                    .WithMessage(instance =>
                    {
                        string fieldName = GetAccountFieldName(instance);
                        return $"{fieldName}:不能为空。";
                    });
            }

            // 收款信息字段 - 同样使用智能验证规则
            this.RuleForNullableForeignKey(
                x => x.PayeeInfoID,
                "收款信息",
                false,
                ValidatorConfig);

            // 经办人字段 - 使用智能验证规则
            this.RuleForNullableForeignKey(
                x => x.Employee_ID,
                "经办人",
                false,
                ValidatorConfig);

            // 部门字段 - 使用智能验证规则
            this.RuleForNullableForeignKey(
                x => x.DepartmentID,
                "部门",
                false,
                ValidatorConfig);

            // 项目组字段 - 使用智能验证规则
            this.RuleForNullableForeignKey(
                x => x.ProjectGroup_ID,
                "项目组",
                false,
                ValidatorConfig);

            // 付款方式字段 - 使用智能验证规则
            this.RuleForNullableForeignKey(
                x => x.Paytype_ID,
                "付款方式",
                false,
                ValidatorConfig);
        }

        /// <summary>
        /// 根据收付类型获取账户字段的显示名称
        /// </summary>
        /// <param name="entity">预收付款单实体</param>
        /// <returns>字段显示名称：收款账户 或 付款账户</returns>
        private string GetAccountFieldName(tb_FM_PreReceivedPayment entity)
        {
            if (entity == null)
            {
                return "公司账户";
            }

            // 根据ReceivePaymentType判断是收款还是付款
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                return "收款账户";
            }
            else if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
            {
                return "付款账户";
            }
            else
            {
                // 默认返回通用名称
                return "公司账户";
            }
        }
    }
}
