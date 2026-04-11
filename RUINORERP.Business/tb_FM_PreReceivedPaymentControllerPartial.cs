
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:33:16
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using AutoMapper;
using OfficeOpenXml.Export.ToDataTable;
using RUINORERP.Business.CommService;
using System.Windows.Forms;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentController<T> : BaseController<T> where T : class
    {
        //protected async Task SettlePrePayment()
        //{
        //    var targetStatus = IsFullSettlement ?
        //        PrePaymentStatus.全额核销 :
        //        PrePaymentStatus.处理中;

        //    bool result = await Submit(targetStatus);
        //    if (result)
        //    {
        //        // 核销成功后处理
        //    }
        //}
        /// <summary>
        /// 客户取消订单时，如果有订单，如果财务没有在他对应的收付单里审核前是可以反审的。否则只能通过红字机制处理。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;

            try
            {
                //只有生效状态的才允许反审，其它不能也不需要，有可能可删除。也可能只能红字
                // 获取当前状态
                var statusProperty = typeof(PrePaymentStatus).Name;
                var currentStatus = (PrePaymentStatus)Enum.ToObject(
                    typeof(PrePaymentStatus),
                    entity.GetPropertyValue(statusProperty)
                );
                var ValidateValue = StateManager.ValidateBusinessStatusTransitionAsync(currentStatus, PrePaymentStatus.已生效 as Enum);
                if (!ValidateValue.IsSuccess)
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以反审";
                    return rmrs;
                }


                var paymentRecordController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //注意，反审 将对应的预付生成的收款单，只有收款单没有审核前，可以删除
                //不能直接删除上级。要让对应的人员自己删除。不然不清楚。逻辑也不对。只能通过判断
                var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            // .Where(c => (c.PaymentStatus == (int)PaymentStatus.草稿 || c.PaymentStatus == (int)PaymentStatus.待审核))
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.PreRPID))
                              .ToListAsync();
                if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
                {

                    if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.待审核)
                        && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.未审核)))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【待审核】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，请联系上级财务删除后才能反审。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }


                    //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                    if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付)
                        && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【已支付】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，不能反审预款单,请联系上级财务，或作退回处理。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        foreach (var item in PaymentRecordlist)
                        {
                            //删除对应的由预收款生成的收款单
                            await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                                .Include(c => c.tb_FM_PaymentRecordDetails)
                                .ExecuteCommandAsync();
                        }

                    }

                }

                entity.PrePaymentStatus = (int)PrePaymentStatus.待审核;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                {
                    it.PrePaymentStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;

                return rmrs;
            }
            catch (Exception ex)
            {
                // 检测是否为死锁异常
                bool isDeadlock = IsDeadlockException(ex);
                
                if (isDeadlock)
                {
                    _logger.LogWarning($"检测到死锁 - 预收款单号: {entity?.PreRPNO}, 异常消息: {ex.Message}");
                    
                    // 记录死锁相关信息
                    TransactionMetrics.RecordDeadlock(
                        "tb_FM_PreReceivedPayment", 
                        "AntiApproval", 
                        TimeSpan.FromSeconds(0), 
                        ex.Message,
                        entity?.PreRPNO);
                }
                
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }



        /// <summary>
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时
        /// 预收款单本身是「收款」的一种业务类型，销售订单审核时已经生成了预收款单 ，通过 BizType 标记其业务属性为预收款。
        /// 这里审核生成收款单
        /// tb_FM_PaymentSettlement 不需要立即生成，但需在后续触发核销时生成（抵扣时生成）。
        /// 销售订单审核时，则自动审核掉预收款单
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;
            try
            {
                if (entity.PrePaymentStatus != (int)PrePaymentStatus.草稿 && entity.PrePaymentStatus != (int)PrePaymentStatus.待审核)
                {
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = $"预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单{entity.PreRPNO}，状态为【{((PrePaymentStatus)entity.PrePaymentStatus).ToString()}】\r\n请确认状态为【草稿】或【待审核】才可以审核。";

                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.Debug(rmrs.ErrorMsg);
                    }

                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    if (!entity.PayeeInfoID.HasValue)
                    {
                        rmrs.ErrorMsg = $"{entity.PreRPNO}付款时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }


                var records = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecordDetail>()
                    .Includes(c => c.tb_fm_paymentrecord)
                    .Where(c => c.SourceBizType == (int)BizType.预收款单 && c.SourceBilllId == entity.PreRPID)
                    .Where(c => c.tb_fm_paymentrecord.ApprovalStatus == (int)ApprovalStatus.审核通过
                    && c.tb_fm_paymentrecord.ApprovalResults == true
                    && c.tb_fm_paymentrecord.PaymentStatus == (int)PaymentStatus.已支付)
                    .ToListAsync();
                if (records.Count > 0)
                {
                    //一个预收款单可以生成两份收款单，仅仅是在退款要冲销时。即收款金额要为负数
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = $" 预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单{entity.PreRPNO},已生成收款单,系统不支持重复生成收付款。";
                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.Debug(rmrs.ErrorMsg);
                    }
                    return rmrs;
                }


                // 查询相同收款方向、相同来源单据的所有待审核预收付款单
                // 注意：这里查询的是数据库中已存在的单据，不包括当前正在审核的entity
                List<tb_FM_PreReceivedPayment> existingPendingPayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                    .Where(c => c.ReceivePaymentType == entity.ReceivePaymentType)
                    .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待审核)
                    .Where(c => c.SourceBillId == entity.SourceBillId)
                    // 排除当前单据本身，避免重复计算
                    .WhereIF(entity.PreRPID > 0, c => c.PreRPID != entity.PreRPID)
                    .ToListAsync();

                // 将当前正在审核的单据添加到列表中，确保验证时包含当前单据的金额
                // 这样可以在验证时检查累计金额是否超过订单总额
                existingPendingPayments.Add(entity);

                //验证 如果相同收款方向 下，相同业务类型下的相同来源单号，比方一个销售订单 多次收款，一个采购订单 多次付款时
                //则要计算累计收款金额，如果累计金额大于等于收款金额，则不能再收款。如果超过收款金额，则进一步提示才能继续收款。
                decimal TotalOrderAmount = 0;
                if (entity.SourceBizType.HasValue)
                {
                    if (entity.SourceBizType.Value == (int)BizType.销售订单)
                    {
                        var saleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                        .Where(c => c.SOrder_ID == entity.SourceBillId)
                        .SingleAsync();
                        if (saleOrder != null)
                        {
                            TotalOrderAmount = saleOrder.TotalAmount;
                        }
                    }
                    if (entity.SourceBizType.Value == (int)BizType.采购订单)
                    {
                        var purOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                        .Where(c => c.PurOrder_ID == entity.SourceBillId)
                        .SingleAsync();
                        if (purOrder != null)
                        {
                            TotalOrderAmount = purOrder.TotalAmount;
                        }
                    }
                }

                // 调用验证方法，检查预收付款单的合法性
                if (!ValidatePaymentDetails(existingPendingPayments, entity, TotalOrderAmount, rmrs))
                {
                    rmrs.ErrorMsg = "验证预收付款明细的合法性不通过!审核失败。";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }


                entity.PrePaymentStatus = (int)PrePaymentStatus.已生效;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                entity.ApprovalResults = true;


                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                BusinessHelper.Instance.ApproverEntity(entity);


                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                {
                    it.PrePaymentStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;


                return rmrs;
            }
            catch (Exception ex)
            {
                // 检测是否为死锁异常
                bool isDeadlock = IsDeadlockException(ex);
                
                if (isDeadlock)
                {
                    _logger.LogWarning($"检测到死锁 - 预收款单号: {entity?.PreRPNO}, 异常消息: {ex.Message}");
                    
                    // 记录死锁相关信息
                    TransactionMetrics.RecordDeadlock(
                        "tb_FM_PreReceivedPayment", 
                        "Approval", 
                        TimeSpan.FromSeconds(0), 
                        ex.Message,
                        entity?.PreRPNO);
                }
                
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }


        /// <summary>
        /// 验证预收付款明细的合法性
        /// 检查同一业务来源下的预收付款单是否存在重复、超额等问题
        /// </summary>
        /// <param name="prePaymentLists">待审核的预收付款单列表（包含当前单据）</param>
        /// <param name="currentPrePayment">当前正在操作的预收付款单</param>
        /// <param name="totalOrderAmount">来源订单的总金额</param>
        /// <param name="returnResults">返回结果对象，用于传递错误信息</param>
        /// <returns>验证是否通过</returns>
        public static bool ValidatePaymentDetails(List<tb_FM_PreReceivedPayment> prePaymentLists, tb_FM_PreReceivedPayment currentPrePayment, decimal totalOrderAmount, ReturnResults<T> returnResults = null)
        {
            // 如果列表为空，直接通过验证
            if (prePaymentLists.Count == 0)
            {
                return true;
            }
            
            // 获取收付类型（收款或付款），用于提示信息
            var PaymentType = (ReceivePaymentType)prePaymentLists[0].ReceivePaymentType;

            // 按来源业务类型分组（如销售订单、采购订单等）
            var groupedByBizType = prePaymentLists
                .GroupBy(d => d.SourceBizType)
                .ToList();

            foreach (var bizTypeGroup in groupedByBizType)
            {
                // 在每个业务类型内，按来源单号进一步分组
                var groupedByBillNo = bizTypeGroup
                    .GroupBy(d => d.SourceBillNo)
                    .ToList();

                foreach (var billNoGroup in groupedByBillNo)
                {
                    var items = billNoGroup.ToList();

                    // 如果只有一个预收付款单，无需验证重复性，直接跳过
                    if (items.Count == 1)
                        continue;

                    // 如果有两个或以上预收付款单，需要进行详细验证
                    if (items.Count >= 2)
                    {
                        // 计算该来源单号下所有预收付款单的本币金额总和
                        decimal totalLocalAmount = items.Sum(i => i.LocalPrepaidAmount);
                        // 计算外币金额总和
                        decimal totalForeignAmount = items.Sum(i => i.ForeignPrepaidAmount);

                        // 获取金额计算容差阈值（用于处理浮点数精度问题）
                        decimal tolerance = 0.0001m; // 默认容差
                        try
                        {
                            // 从授权控制器获取系统配置的金额计算容差
                            var authorizeController = RUINORERP.Model.Context.ApplicationContext.Current?.GetRequiredService<IAuthorizeController>();
                            if (authorizeController != null)
                            {
                                tolerance = authorizeController.GetAmountCalculationTolerance();
                            }
                        }
                        catch
                        {
                            // 如果获取失败，使用默认容差
                        }

                        // 检查是否为对冲情况（正负金额相抵，总和接近0）
                        // 例如：一笔收款和一笔退款，金额相等方向相反
                        if (Math.Abs(totalLocalAmount) <= tolerance && Math.Abs(totalForeignAmount) <= tolerance)
                            continue; // 对冲情况合法，跳过验证

                        // 检查累计预收付款金额是否超过订单总金额
                        if (totalLocalAmount > totalOrderAmount)
                        {
                            // 超额情况：弹出确认对话框，用户确认后允许继续
                            if (MessageBox.Show($"预{PaymentType}单总金额{totalLocalAmount}(包含当前金额{currentPrePayment.LocalPrepaidAmount})，超过了订单总金额{totalOrderAmount}，确定要超额预{PaymentType}吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                continue; // 用户确认超额，跳过验证
                            }
                            else
                            {
                                // 用户取消超额操作，构建错误信息并返回失败
                                StringBuilder errorBuilder1 = new StringBuilder();
                                errorBuilder1.AppendLine($"操作已取消：预{PaymentType}金额超额");
                                errorBuilder1.AppendLine($"业务类型：{(BizType)bizTypeGroup.Key}");
                                errorBuilder1.AppendLine($"来源单号：{billNoGroup.Key}");
                                errorBuilder1.AppendLine();
                                errorBuilder1.AppendLine($"累计预{PaymentType}金额：{totalLocalAmount}");
                                errorBuilder1.AppendLine($"订单总金额：{totalOrderAmount}");
                                errorBuilder1.AppendLine($"超额金额：{totalLocalAmount - totalOrderAmount}");
                                errorBuilder1.AppendLine();
                                errorBuilder1.AppendLine("建议：");
                                errorBuilder1.AppendLine("1. 检查是否有重复的预收付款单");
                                errorBuilder1.AppendLine("2. 调整预收付款金额，使其不超过订单总额");
                                errorBuilder1.AppendLine("3. 如确需超额，请与财务部门确认");

                                if (returnResults != null)
                                {
                                    returnResults.ErrorMsg = errorBuilder1.ToString();
                                }
                                return false;
                            }
                        }
                        else
                        {
                            // 累计金额未超过订单总额的情况
                            
                            // 特殊情况：两笔款项正好等于订单总额（如首款+尾款）
                            if (totalLocalAmount == totalOrderAmount)
                            {
                                continue; // 正好等于订单总额，合法，跳过验证
                            }

                            // 检查是否存在重复金额的预收付款单
                            // 如果当前单据金额与列表中已有单据金额相同，可能存在重复支付风险
                            if (prePaymentLists.Any(a => a.PreRPID != currentPrePayment.PreRPID && a.LocalPrepaidAmount == currentPrePayment.LocalPrepaidAmount))
                            {
                                // 弹出警告对话框，用户确认后允许继续
                                if (MessageBox.Show($"当前的预{PaymentType}单总金额{totalLocalAmount}与对应来源业务的已支付笔数中的金额相同，请确认不是重复支付，确定要预{PaymentType}吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    continue; // 用户确认非重复，跳过验证
                                }
                                else
                                {
                                    // 用户取消操作，构建错误信息并返回失败
                                    StringBuilder errorBuilder2 = new StringBuilder();
                                    errorBuilder2.AppendLine($"操作已取消：可能存在重复预{PaymentType}");
                                    errorBuilder2.AppendLine($"业务类型：{(BizType)bizTypeGroup.Key}");
                                    errorBuilder2.AppendLine($"来源单号：{billNoGroup.Key}");
                                    errorBuilder2.AppendLine();
                                    errorBuilder2.AppendLine($"当前预{PaymentType}金额：{currentPrePayment.LocalPrepaidAmount}");
                                    errorBuilder2.AppendLine($"累计预{PaymentType}金额：{totalLocalAmount}");
                                    errorBuilder2.AppendLine();
                                    
                                    // 列出金额相同的单据
                                    var duplicateItems = prePaymentLists.Where(a => a.PreRPID != currentPrePayment.PreRPID && a.LocalPrepaidAmount == currentPrePayment.LocalPrepaidAmount).ToList();
                                    errorBuilder2.AppendLine($"发现 {duplicateItems.Count} 笔相同金额的预{PaymentType}单：");
                                    int itemIndex = 1;
                                    foreach (var item in duplicateItems)
                                    {
                                        errorBuilder2.AppendLine($"{itemIndex}. 单据编号: {item.PreRPNO}, 金额: {item.LocalPrepaidAmount}, 创建时间: {item.Created_at}");
                                        itemIndex++;
                                    }
                                    errorBuilder2.AppendLine();
                                    errorBuilder2.AppendLine("建议：");
                                    errorBuilder2.AppendLine("1. 检查上述单据是否为重复录入");
                                    errorBuilder2.AppendLine("2. 如确认为不同批次的预收付款，请修改金额或添加备注区分");
                                    errorBuilder2.AppendLine("3. 联系财务人员确认处理方式");

                                    if (returnResults != null)
                                    {
                                        returnResults.ErrorMsg = errorBuilder2.ToString();
                                    }
                                    return false;
                                }
                            }
                        }
                    }

                    // 构建详细的错误消息，说明重复预收付款单的问题
                    StringBuilder errorBuilder = new StringBuilder();
                    errorBuilder.AppendLine($"错误：不能存在相同业务来源的重复预{PaymentType}单数据");
                    errorBuilder.AppendLine($"业务类型：{(BizType)bizTypeGroup.Key}");
                    errorBuilder.AppendLine($"来源单号：{billNoGroup.Key}");
                    errorBuilder.AppendLine();
                    errorBuilder.AppendLine($"重复预{PaymentType}单详情：");

                    int index = 1;
                    // 列出所有重复单据的详细信息，便于用户排查
                    foreach (var item in items)
                    {
                        errorBuilder.AppendLine($"{index}. 单据编号: {item.PreRPNO}");
                        errorBuilder.AppendLine($"   金额: {item.LocalPrepaidAmount}");
                        errorBuilder.AppendLine($"   交易方向: {(ReceivePaymentType)(item.ReceivePaymentType)}");
                        errorBuilder.AppendLine($"   创建时间: {item.Created_at}");
                        errorBuilder.AppendLine($"   创建用户: {item.Created_by}");
                        errorBuilder.AppendLine();
                        index++;
                    }

                    errorBuilder.AppendLine("可能原因：");
                    errorBuilder.AppendLine($"1. 生成了重复的预{PaymentType}单");
                    errorBuilder.AppendLine("2. 导入数据时发生重复");
                    errorBuilder.AppendLine("3. 系统操作错误导致重复记录");
                    errorBuilder.AppendLine();
                    errorBuilder.AppendLine("解决建议：");
                    errorBuilder.AppendLine("1. 检查并删除重复的预收款单");
                    errorBuilder.AppendLine("2. 确保每张业务单据只对应一张预收款单");
                    errorBuilder.AppendLine("3. 如需多次预收款，请确保业务来源信息不同（如分批次、分项目等）");

                    // 设置错误信息并返回验证失败
                    if (returnResults != null)
                    {
                        returnResults.ErrorMsg = errorBuilder.ToString();
                    }
                    return false; // 验证失败，存在不合法的重复数据
                }
            }

            // 所有验证通过
            return true;
        }


        /// <summary>
        /// 预收款单逻辑删除
        /// 删除前检测并级联处理关联的收款单
        /// </summary>
        /// <param name="ObjectEntity">预收款单实体</param>
        /// <returns>删除是否成功</returns>
        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PreReceivedPayment ObjectEntity)
        {
            // 先检测并处理关联的收款单
            var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                .Includes(a => a.tb_FM_PaymentRecordDetails)
                .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == ObjectEntity.PreRPID))
                .ToListAsync();

            if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
            {
                // 检查是否存在已审核的收款单，不能直接删除
                if (PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过))
                {
                    var approvedRecord = PaymentRecordlist.FirstOrDefault(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过);
                    throw new Exception($"预收款单{ObjectEntity.PreRPNO}已关联已审核的收款单{approvedRecord?.PaymentNo}，请先处理收款单后再删除");
                }

                // 删除未审核的收款单（草稿或待审核状态）
                foreach (var item in PaymentRecordlist)
                {
                    await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                        .Include(c => c.tb_FM_PaymentRecordDetails)
                        .ExecuteCommandAsync();
                }
            }

            // 执行预收款单的逻辑删除
            int count = await _unitOfWorkManage.GetDbClient()
                .Deleteable<tb_FM_PreReceivedPayment>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 通过销售订单生成预收款单1
        /// </summary>
        /// <param name="entity"></param>
        ///<param name="PrepaidAmount">手动再次预付款时的金额，</param>
        /// <returns></returns>
        public async Task<tb_FM_PreReceivedPayment> BuildPreReceivedPaymentAsync(tb_SaleOrder entity, decimal PrepaidAmount = 0)
        {

            // 外币相关处理 正确是 外币时一定要有汇率
            decimal exchangeRate = 1; // 获取销售订单的汇率
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                exchangeRate = 1; // 获取销售订单的汇率
                                  // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                  // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
            }

            #region 生成预收款



            tb_FM_PreReceivedPayment payable = new tb_FM_PreReceivedPayment();
            payable = mapper.Map<tb_FM_PreReceivedPayment>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.IsAvailable = true;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            if (entity.tb_projectgroup != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            payable.PreRPNO = await bizCodeService.GenerateBizBillNoAsync(BizType.预收款单);
            payable.SourceBizType = (int)BizType.销售订单;
            payable.SourceBillNo = entity.SOrderNo;
            payable.SourceBillId = entity.SOrder_ID;
            payable.Currency_ID = entity.Currency_ID;
            payable.PrePayDate = entity.SaleDate;
            payable.ExchangeRate = exchangeRate;

            payable.LocalPrepaidAmountInWords = string.Empty;
            payable.Account_id = entity.Account_id;

            if (PrepaidAmount == 0)
            {        //如果是外币时，则由外币算出本币
                if (entity.PayStatus == (int)PayStatus.全额预付)
                {
                    //外币时 全部付款，则外币金额=本币金额/汇率 在UI中显示出来。
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignTotalAmount;
                        //payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    //本币时
                    payable.LocalPrepaidAmount = entity.TotalAmount;

                }
                else            //来自于订金
                if (entity.PayStatus == (int)PayStatus.部分预付)
                {
                    //外币时
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignDeposit;
                        // payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    else
                    {
                        payable.LocalPrepaidAmount = entity.Deposit;
                    }
                }
            }
            else
            {
                payable.LocalPrepaidAmount = PrepaidAmount;
            }

            payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToUpperAmount();
            payable.IsAvailable = true;//默认可用
            payable.PrePaymentReason = $"销售订单{entity.SOrderNo}的预收款。";
            if (!string.IsNullOrEmpty(entity.PlatformOrderNo) && entity.PlatformOrderNo.Trim().Length > 3)
            {
                payable.PrePaymentReason += $"平台单号：{entity.PlatformOrderNo}";
            }
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.PrePaymentStatus = (int)PrePaymentStatus.待审核;
            payable.LocalBalanceAmount = 0;
            payable.ForeignBalanceAmount = 0;

            #endregion
            return payable;
        }


        /// <summary>
        /// 通过销售订单生成预收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="SaveToDb"></param>
        /// <returns></returns>
        public async Task<tb_FM_PreReceivedPayment> BuildPreReceivedPaymentAsync(tb_PurOrder entity, decimal PrepaidAmount = 0)
        {

            // 外币相关处理 正确是 外币时一定要有汇率
            decimal exchangeRate = 1; // 获取销售订单的汇率
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                exchangeRate = 1; // 获取销售订单的汇率
                                  // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                  // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
            }

            #region 生成预付款

            tb_FM_PreReceivedPayment payable = new tb_FM_PreReceivedPayment();
            payable = mapper.Map<tb_FM_PreReceivedPayment>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.IsAvailable = true;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.PrePayDate = System.DateTime.Now;
            if (entity.tb_projectgroup != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            //采购就是付款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;

            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            payable.PreRPNO = await bizCodeService.GenerateBizBillNoAsync(BizType.预付款单);
            payable.SourceBizType = (int)BizType.采购订单;
            payable.SourceBillNo = entity.PurOrderNo;
            payable.SourceBillId = entity.PurOrder_ID;
            payable.Currency_ID = entity.Currency_ID;
            payable.ExchangeRate = exchangeRate;
            payable.LocalPrepaidAmountInWords = string.Empty;
            if (PrepaidAmount == 0)
            {

                //payable.Account_id = entity.Account_id;//付款账户信息 在采购订单时 不用填写。由财务决定 
                //如果是外币时，则由外币算出本币
                if (entity.PayStatus == (int)PayStatus.全额预付)
                {
                    //外币时 全部付款，则外币金额=本币金额/汇率 在UI中显示出来。
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignTotalAmount;
                        //payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    //本币时
                    payable.LocalPrepaidAmount = entity.TotalAmount;

                }
                else            //来自于订金
                if (entity.PayStatus == (int)PayStatus.部分预付)
                {
                    //外币时
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignDeposit;
                        // payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    else
                    {
                        payable.LocalPrepaidAmount = entity.Deposit;
                    }
                }
            }
            else
            {
                payable.LocalPrepaidAmount = PrepaidAmount;
            }


            //payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToString("C");
            payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToUpperAmount();
            payable.IsAvailable = true;//默认可用
            payable.PrePaymentReason = $"采购订单{entity.PurOrderNo}的预付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.PrePaymentStatus = (int)PrePaymentStatus.待审核;
            payable.LocalBalanceAmount = 0;
            payable.ForeignBalanceAmount = 0;

            #endregion
            return payable;
        }


        ///// <summary>
        ///// 要生成收付单 没完成
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns></returns>
        //public async virtual Task<bool> BatchApproval(List<tb_FM_PreReceivedPayment> entitys, ApprovalEntity approvalEntity)
        //{
        //    try
        //    {
        //        // 开启事务，保证数据一致性
        //        _unitOfWorkManage.BeginTran();
        //        if (!approvalEntity.ApprovalResults)
        //        {
        //            if (entitys == null)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            foreach (var entity in entitys)
        //            {
        //                //这部分是否能提出到上一级公共部分？
        //                entity.PrePaymentStatus = (int)PrePaymentStatus.已生效;
        //                entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
        //                //后面已经修改为
        //                entity.ApprovalResults = approvalEntity.ApprovalResults;
        //                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
        //                BusinessHelper.Instance.ApproverEntity(entity);
        //                //只更新指定列
        //                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
        //                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
        //            }
        //        }
        //        // 注意信息的完整性
        //        _unitOfWorkManage.CommitTran();

        //        //_logger.Info(approvalEntity.bizName + "审核事务成功");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
        //        _unitOfWorkManage.RollbackTran();
        //        _logger.Error(approvalEntity.bizName + "事务回滚");
        //        return false;
        //    }

        //}


        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PreReceivedPayment> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PreReceivedPayment>()
                .Where(m => m.PreRPID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();


            foreach (var item in list)
            {
                if (item.SourceBizType.HasValue && item.SourceBizType.Value == (int)BizType.采购订单)
                {
                    item.tb_purorder = await _appContext.Db.CopyNew().Queryable<tb_PurOrder>()
                        .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.PurOrder_ID == item.SourceBillId).FirstAsync();
                }

                if (item.SourceBizType.HasValue && item.SourceBizType.Value == (int)BizType.销售订单)
                {
                    item.tb_saleorder = await _appContext.Db.CopyNew().Queryable<tb_SaleOrder>()
                        .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.SOrder_ID == item.SourceBillId).FirstAsync();
                }
            }

            return list as List<T>;
        }

        /// <summary>
        /// 批量结案  预收付款单标记结案，数据状态为9
        /// 功能仅修改单据状态，不涉及其他业务逻辑处理
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> entitys)
        {
            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                _unitOfWorkManage.BeginTran();
                for (int m = 0; m < entitys.Count; m++)
                {
                    tb_FM_PreReceivedPayment entity = entitys[m] as tb_FM_PreReceivedPayment;
                    // 判断当前状态是否可以结案
                    var currentStatus = (PrePaymentStatus)entity.PrePaymentStatus;
                    var validateResult = StateManager.ValidateBusinessStatusTransitionAsync(currentStatus, PrePaymentStatus.结案);
                    if (!validateResult.IsSuccess)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以结案";
                        rs.Succeeded = false;
                        return rs;
                    }
                    // 修改状态为结案
                    entity.PrePaymentStatus = (int)PrePaymentStatus.结案;
                    entity.Remark += "【手动结案】";
                    // 只更新指定列
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                    {
                        it.PrePaymentStatus,
                        it.Remark
                    }).ExecuteCommandAsync();
                }
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "预收付款单结案失败");
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }
        }

        /// <summary>
        /// 批量反结案  预收付款单标记反结案
        /// 根据实际的核销和退款情况，恢复到对应的非终态状态：
        /// - 核销金额=0 且 退款金额=0 → 待核销（未开始使用）
        /// - 核销金额>0 或 退款金额>0 → 处理中（已部分使用）
        /// 注意：不会恢复到终态（全额核销、全额退款、混合结清），因为那些是自然结束的终态
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> AntiBatchCloseCaseAsync(List<T> entitys)
        {
            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                _unitOfWorkManage.BeginTran();
                
                for (int m = 0; m < entitys.Count; m++)
                {
                    tb_FM_PreReceivedPayment entity = entitys[m] as tb_FM_PreReceivedPayment;
                    
                    // 判断当前状态是否可以反结案
                    var currentStatus = (PrePaymentStatus)entity.PrePaymentStatus;
                    if (currentStatus != PrePaymentStatus.结案)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"只有状态为【结案】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单才可以反结案，当前状态为【{currentStatus.ToString()}】";
                        rs.Succeeded = false;
                        return rs;
                    }
                    
                    // 根据实际的核销和退款情况，计算应该恢复到的状态
                    // 反结案后只能是非终态：待核销或处理中
                    PrePaymentStatus targetStatus;
                    
                    // 判断逻辑：
                    // 1. 如果有任何核销或退款记录 → 处理中
                    // 2. 如果没有任何核销和退款 → 待核销
                    if ((entity.LocalPaidAmount > 0 || entity.ForeignPaidAmount > 0) ||  // 有核销记录
                        (entity.LocalRefundAmount > 0 || entity.ForeignRefundAmount > 0))  // 有退款记录
                    {
                        targetStatus = PrePaymentStatus.处理中;
                    }
                    else
                    {
                        targetStatus = PrePaymentStatus.待核销;
                    }
                    
                    // 验证状态转换是否合法
                    var validateResult = StateManager.ValidateBusinessStatusTransitionAsync(currentStatus, targetStatus);
                    if (!validateResult.IsSuccess)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以反结案到【{targetStatus.ToString()}】状态";
                        rs.Succeeded = false;
                        return rs;
                    }
                    
                    // 修改状态为目标状态
                    entity.PrePaymentStatus = (int)targetStatus;
                    entity.Remark += "【手动反结案】";
                    
                    // 只更新指定列
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                    {
                        it.PrePaymentStatus,
                        it.Remark
                    }).ExecuteCommandAsync();
                }
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "预收付款单反结案失败");
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }
        }
        
        /// <summary>
        /// 检测是否为死锁异常
        /// </summary>
        private bool IsDeadlockException(Exception ex)
        {
            if (ex == null) return false;
            
            string message = ex.Message.ToLower();
            return message.Contains("deadlock") || 
                   message.Contains("1205") ||  // MySQL/SQL Server 死锁错误码
                   message.Contains("1092") ||  // MySQL kill query 错误
                   message.Contains("lock") ||
                   message.Contains("timeout") ||
                   message.Contains("was deadlocked");
        }
    }
}



