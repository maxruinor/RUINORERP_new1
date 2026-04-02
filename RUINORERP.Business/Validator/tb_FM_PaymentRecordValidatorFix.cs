
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 16:10:18
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using System.Linq;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Business.Validator;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 付款单验证器 - 扩展验证逻辑
    /// 支持验证层值转换：将UI传递的-1值自动转换为null
    /// 根据收付类型动态显示"收款账户"或"付款账户"
    /// </summary>
    public partial class tb_FM_PaymentRecordValidator : BaseValidatorGeneric<tb_FM_PaymentRecord>
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
            // 2. 根据全局配置【收付款单账户必填】决定是否允许为空
            // 3. 默认值为true，表示收付款单的账户必须填写
            // 4. 根据ReceivePaymentType动态显示"收款账户"或"付款账户"
            // ========================================
            
            // 应用值转换 - 将-1或0转换为null
            this.ApplyPropertyTransformation(x => x.Account_id, ValueTransformConfig.Default);
            
            // 添加外键值验证，根据收付类型动态显示字段名称
            RuleFor(x => x.Account_id)
                .Must((instance, value) =>
                {
                    // 如果值为null，根据是否必填决定验证结果
                    bool isRequired = ValidatorConfig?.收付款单账户必填 ?? true;
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
            bool isAccountRequired = ValidatorConfig?.收付款单账户必填 ?? true;
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

            // 付款方式字段 - 同样使用智能验证规则
            this.RuleForPaymentRecordAccountField(
                x => x.Paytype_ID,
                "付款方式",
                ValidatorConfig);

            // 收款信息字段 - 使用智能验证规则（可选）
            this.RuleForNullableForeignKey(
                x => x.PayeeInfoID,
                "收款信息",
                false,
                ValidatorConfig);

            // 经办人字段 - 使用智能验证规则（可选）
            this.RuleForNullableForeignKey(
                x => x.Employee_ID,
                "经办人",
                false,
                ValidatorConfig);

            // 往来单位字段 - 使用智能验证规则（可选）
            this.RuleForNullableForeignKey(
                x => x.CustomerVendor_ID,
                "往来单位",
                false,
                ValidatorConfig);

            // 验证收付款单总金额不能为零
            // 红蓝单对冲核销功能仅限于对账单余额模式,不适用于普通收付款单
            RuleFor(x => x.TotalLocalPayableAmount)
                .NotEqual(0)
                .WithMessage("收付款单总金额不能为零。");

          
            // 验证付款单关联报销单时,付款金额必须与报销金额一致
            RuleFor(x => x)
                .Must(CheckExpenseClaimPaymentAmount)
                .When(x => x.tb_FM_PaymentRecordDetails != null && x.tb_FM_PaymentRecordDetails.Any(d => d.SourceBizType == (int)BizType.费用报销单))
                .WithMessage("付款单对报销单付款时,付款金额必须与报销金额一致。");
        }

        /// <summary>
        /// 根据收付类型获取账户字段的显示名称
        /// </summary>
        /// <param name="entity">收付款单实体</param>
        /// <returns>字段显示名称：收款账户 或 付款账户</returns>
        private string GetAccountFieldName(tb_FM_PaymentRecord entity)
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
        /// 验证付款单对报销单付款时，付款金额是否与报销金额一致
        /// </summary>
        /// <param name="paymentRecord">付款记录</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        private bool CheckExpenseClaimPaymentAmount(tb_FM_PaymentRecord paymentRecord)
        {
            // 如果没有明细数据，跳过验证
            if (paymentRecord.tb_FM_PaymentRecordDetails == null || !paymentRecord.tb_FM_PaymentRecordDetails.Any())
            {
                return true;
            }

            // 检查是否有关联报销单的明细
            var expenseClaimDetails = paymentRecord.tb_FM_PaymentRecordDetails
                .Where(d => d.SourceBizType == (int)BizType.费用报销单)
                .ToList();

            // 如果没有关联报销单的明细，跳过验证
            if (!expenseClaimDetails.Any())
            {
                return true;
            }

            try
            {
                // 获取数据库上下文
                var db = _appContext?.Db;
                if (db == null)
                {
                    return true; // 无法获取数据库上下文时，跳过验证
                }

                // 对每张报销单进行验证
                foreach (var detailGroup in expenseClaimDetails.GroupBy(d => d.SourceBilllId))
                {
                    long claimId = detailGroup.Key;
                    decimal totalPaymentAmount = detailGroup.Sum(d => d.LocalAmount);

                    // 查询报销单信息
                    var expenseClaim = db.Queryable<tb_FM_ExpenseClaim>()
                        .Where(c => c.ClaimMainID == claimId)
                        .First();

                    if (expenseClaim == null)
                    {
                        continue; // 报销单不存在，跳过
                    }

                    // 比较付款金额与报销金额
                    if (totalPaymentAmount != expenseClaim.ClaimAmount)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // 验证过程出错时记录日志，但不阻止保存
                System.Diagnostics.Debug.WriteLine($"验证报销单付款金额时出错: {ex.Message}");
                return true;
            }
        }
    }

}
