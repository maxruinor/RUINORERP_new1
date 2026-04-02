
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/23/2025 14:28:32
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.Validator;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 应收应付表验证类 - 扩展验证逻辑
    /// 支持验证层值转换：将UI传递的-1值自动转换为null
    /// 根据收付类型动态显示"收款账户"或"付款账户"
    /// </summary>
    public partial class tb_FM_ReceivablePayableValidator : BaseValidatorGeneric<tb_FM_ReceivablePayable>
    {
        public override void Initialize()
        {
            // ========================================
            // 公司账户字段验证 - 使用新的智能验证规则
            // 功能说明：
            // 1. 自动将UI传递的-1或0值转换为null
            // 2. 根据收付类型动态显示"收款账户"或"付款账户"
            // 3. 红字单据（负数金额）时允许账户为空
            // ========================================

            // 应用值转换 - 将-1或0转换为null
            this.ApplyPropertyTransformation(x => x.Account_id, ValueTransformConfig.Default);

            // 添加外键值验证，根据收付类型动态显示字段名称
            RuleFor(x => x.Account_id)
                .Must((instance, value) =>
                {
                    // 红字单据（负数金额）时允许账户为空
                    if (instance.TotalLocalPayableAmount <= 0)
                    {
                        return true;
                    }
                    // 如果值为null，验证失败（应收应付单账户必填）
                    if (!value.HasValue)
                    {
                        return false;
                    }
                    // 检查是否为有效的非零非负一值
                    return value.Value != 0 && value.Value != -1;
                })
                .WithMessage(instance =>
                {
                    // 根据收付类型动态返回字段名称
                    string fieldName = GetAccountFieldName(instance);
                    return $"{fieldName}:下拉选择值不正确。";
                })
                .When(x => x.TotalLocalPayableAmount > 0);

            // 非空验证
            RuleFor(x => x.Account_id)
                .NotNull()
                .WithMessage(instance =>
                {
                    string fieldName = GetAccountFieldName(instance);
                    return $"{fieldName}:不能为空。";
                })
                .When(x => x.TotalLocalPayableAmount > 0 && x.Account_id.HasValue);

            // ========================================
            // 收款信息字段验证
            // 根据收付类型动态显示"收款信息"或"付款信息"
            // ========================================

            // 应用值转换 - 将-1或0转换为null
            this.ApplyPropertyTransformation(x => x.PayeeInfoID, ValueTransformConfig.Default);

            RuleFor(customer => customer.PayeeInfoID)
                .Custom((value, context) =>
                {
                    var receivablePayable = context.InstanceToValidate as tb_FM_ReceivablePayable;
                    if (receivablePayable == null) return;

                    // 获取字段显示名称
                    string accountFieldName = GetAccountFieldName(receivablePayable);
                    string payeeFieldName = GetPayeeFieldName(receivablePayable);

                    // 根据配置判断收款账户是否必须填写
                    if (ValidatorConfig.收付款账户必填)
                    {
                        // 只在付款单时验证收款信息，收款单时不验证
                        if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {
                            if (receivablePayable.PayeeInfoID == null || !receivablePayable.PayeeInfoID.HasValue)
                            {
                                if (receivablePayable.TotalLocalPayableAmount > 0)
                                {
                                    context.AddFailure($"{payeeFieldName}必填：必须填写。");
                                }
                                else
                                {
                                    context.AddFailure($"付款红字时，{payeeFieldName}必填：必须填写。");
                                }
                            }
                        }
                    }
                    else
                    {
                        // 审核时才需验证，所以状态为提交保存时可以忽略
                        if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款
                            && receivablePayable.ARAPStatus > (int)ARAPStatus.待审核
                            && receivablePayable.TotalLocalPayableAmount > 0
                            && (receivablePayable.PayeeInfoID == null || !receivablePayable.PayeeInfoID.HasValue))
                        {
                            context.AddFailure($"{payeeFieldName}:付款时，对方的{payeeFieldName}等信息不能为空。");
                        }
                    }
                });

            // 收款信息下拉值验证
            RuleFor(x => x.PayeeInfoID)
                .Must((instance, value) =>
                {
                    if (!value.HasValue) return true;
                    return value.Value != 0 && value.Value != -1;
                })
                .WithMessage(instance =>
                {
                    string payeeFieldName = GetPayeeFieldName(instance);
                    return $"{payeeFieldName}:下拉选择值不正确。";
                })
                .When(x => x.TotalLocalPayableAmount > 0);

            // 收款账号长度验证
            RuleFor(x => x.PayeeAccountNo)
                .MaximumMixedLength(100)
                .WithMessage(instance =>
                {
                    string payeeFieldName = GetPayeeFieldName(instance);
                    return $"{payeeFieldName}账号:不能超过最大长度,100。";
                })
                .When(x => x.TotalLocalPayableAmount > 0);
        }

        /// <summary>
        /// 根据收付类型获取账户字段的显示名称
        /// </summary>
        /// <param name="entity">应收应付单实体</param>
        /// <returns>字段显示名称：收款账户 或 付款账户</returns>
        private string GetAccountFieldName(tb_FM_ReceivablePayable entity)
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

        /// <summary>
        /// 根据收付类型获取收款信息字段的显示名称
        /// </summary>
        /// <param name="entity">应收应付单实体</param>
        /// <returns>字段显示名称：收款信息 或 付款信息</returns>
        private string GetPayeeFieldName(tb_FM_ReceivablePayable entity)
        {
            if (entity == null)
            {
                return "收款信息";
            }

            // 根据ReceivePaymentType判断是收款还是付款
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                return "收款信息";
            }
            else if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
            {
                return "付款信息";
            }
            else
            {
                // 默认返回通用名称
                return "收款信息";
            }
        }
    }

}
