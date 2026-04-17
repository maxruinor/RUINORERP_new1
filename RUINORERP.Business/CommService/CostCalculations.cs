using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 成本计算1
    /// </summary>
    public class CostCalculations
    {
        /*
                   直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                  平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                  先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                  后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                  数据来源可以是多种多样的，例如：
                  采购价格：从供应商处购买产品或物品时的价格。
                  生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                  市场价格：参考市场上类似产品或物品的价格。
          */

        /// <summary>
        /// 移动加权平均成本计算，要以未税成本为基准
        /// 当前方法中不能对tb_Inventory inv的数量进行操作，因为这里只是价格计算，数量的变动要在调用方处理
        /// </summary>
        /// <param name="_appContext"></param>
        /// <param name="inv"></param>
        /// <param name="currentQty"></param>
        /// <param name="UntaxedUnitPrice"></param>
        /// <param name="UntaxedShippCost"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public static void CostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty,
            decimal UntaxedUnitPrice, decimal UntaxedShippCost = 0)
        {
            // 前置校验：当前操作数量不能为0（避免无意义操作）
            if (currentQty == 0) throw new ArgumentException("当前操作数量不能为0");

            // 获取系统配置
            bool allowNegativeInventory = _appContext.SysConfig.CheckNegativeInventory;
            //新成本单价 = （原库存成本×原数量 + (采购不含税单价×采购数量 + 不含税运费)）/(原数量 + 采购数量 )
            // 先算成本再赋值数量（保持原有逻辑顺序）
            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;

            // 计算新库存数量
            int newQty = inv.Quantity + currentQty;
            // 负库存检查（如果不允许负库存）
            if (!allowNegativeInventory && newQty < 0)
            {
                throw new InvalidOperationException($"成本计算时，库存数量不能为负（操作后数量：{newQty}）");
            }

            // 保存原始库存信息，用于计算成本变化
            decimal originalCost = inv.Inv_Cost;
            int originalQty = inv.Quantity;

            // 执行成本计算
            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                {
                    // ✅ 先进先出法(FIFO): 入库时更新FIFO成本,出库时按最早批次成本计算
                    // 注意: 完整的FIFO需要批次管理表(tb_InventoryBatch),这里仅记录最新入库成本
                    // 实际出库时应从批次表中按入库时间顺序扣减
                    inv.CostFIFO = UntaxedUnitPrice;
                    
                    // FIFO的成本计算: 如果有现有库存,保留原有FIFO成本;如果是新库存或库存为0,使用新成本
                    if (inv.Quantity > 0)
                    {
                        // 有库存时,FIFO成本保持不变(因为新入库的在后面,不影响已存在的库存成本)
                        // CostFIFO只记录最新入库单价,用于后续新批次的参考
                        // Inv_Cost保持原有的加权平均值(简化处理)
                        decimal originalAmount = inv.Inv_Cost * inv.Quantity;
                        decimal newAmount = UntaxedUnitPrice * currentQty + UntaxedShippCost;
                        decimal totalAmount = originalAmount + newAmount;
                        int totalQty = inv.Quantity + currentQty;
                        
                        if (totalQty > 0)
                        {
                            inv.Inv_Cost = totalAmount / totalQty;
                        }
                    }
                    else
                    {
                        // 库存为0时,直接使用新成本
                        inv.Inv_Cost = UntaxedUnitPrice;
                    }
                    break;
                }

                case 库存成本计算方式.月加权平均:
                {
                    // ✅ 月加权平均法: 每月重新计算一次平均成本
                    // 公式: 月初结存金额 + 本月入库金额) / (月初结存数量 + 本月入库数量)
                    // 注意: 实际实现需要在月初锁定上月成本,这里简化为持续累加
                    
                    // 如果当前是月初第一次入库,应该重置月度累计值
                    // TODO: 需要添加月度累计字段(tb_Inventory.MonthlyBeginQty, MonthlyBeginAmount)
                    // 目前简化处理: 使用移动加权的方式近似
                    
                    if (inv.Quantity > 0)
                    {
                        // 有库存时,累加计算
                        decimal originalAmount = inv.Inv_Cost * inv.Quantity;
                        decimal newAmount = UntaxedUnitPrice * currentQty + UntaxedShippCost;
                        decimal totalAmount = originalAmount + newAmount;
                        int totalQty = inv.Quantity + currentQty;
                        
                        if (totalQty > 0)
                        {
                            inv.CostMonthlyWA = totalAmount / totalQty;
                            inv.Inv_Cost = inv.CostMonthlyWA;
                        }
                    }
                    else
                    {
                        // 库存为0时,直接使用新成本
                        inv.CostMonthlyWA = UntaxedUnitPrice;
                        inv.Inv_Cost = inv.CostMonthlyWA;
                    }
                    break;
                }

                case 库存成本计算方式.移动加权平均法:
                {
                    //仅在移动加权平均法中考虑运费成本
                    //其他方法忽略运费（符合标准会计实践）
                    // 计算移动加权平均时的关键分母
                    decimal totalQty = newQty;

                    // 分母异常处理（避免除零或负数导致的不合理计算）
                    if (totalQty == 0)
                    {
                        // 数量归零时保留 前后成本选最大的，保守防御性设置成本。利润可少。不真实亏。
                        //出库金额ForeignTotalAmount和 预付金额prePayments[i].ForeignBalanceAmount 比较
                        var SelectCost = Math.Max(inv.Inv_Cost, UntaxedUnitPrice);
                        //因为特殊情况，前面系统中出现了负数。如果入库数量与原来的和为0，则取最新成本为结果。
                        inv.Inv_Cost = SelectCost;
                        return;
                    }
                    else
                    {
                        // 核心计算公式：
                        // 新成本 = (原库存金额 + 新入库金额 + 运费) / 新库存总量
                        decimal originalAmount = inv.Inv_Cost * inv.Quantity;
                        decimal newAmount = UntaxedUnitPrice * currentQty;
                        decimal totalAmount = originalAmount + newAmount + UntaxedShippCost;

                        inv.CostMovingWA = totalAmount / totalQty;
                        inv.Inv_Cost = inv.CostMovingWA;

                    }

                    break;
                }

                case 库存成本计算方式.实际成本法:
                {
                    // ✅ 实际成本法(个别计价法): 每批货物单独核算成本
                    // 适用于贵重物品、定制化产品等可以单独识别的场景
                    // 需要批次管理支持,这里简化为记录最新实际成本
                    
                    // 实际成本法下,每次入库都记录该批次的实际成本
                    // 出库时需要指定具体批次(通过批次ID)
                    inv.Inv_AdvCost = UntaxedUnitPrice;
                    
                    // 简化处理: 如果有库存,计算加权平均;如果没有,直接使用新成本
                    if (inv.Quantity > 0)
                    {
                        decimal originalAmount = inv.Inv_Cost * inv.Quantity;
                        decimal newAmount = UntaxedUnitPrice * currentQty + UntaxedShippCost;
                        decimal totalAmount = originalAmount + newAmount;
                        int totalQty = inv.Quantity + currentQty;
                        
                        if (totalQty > 0)
                        {
                            inv.Inv_Cost = totalAmount / totalQty;
                        }
                    }
                    else
                    {
                        inv.Inv_Cost = UntaxedUnitPrice;
                    }
                    break;
                }

                default:
                    throw new NotImplementedException($"未实现的成本计算方式：{m}");
            }
            // 成本负数检查（所有方法通用）
            if (inv.Inv_Cost < 0)
            {
                throw new InvalidOperationException($"计算得到负库存成本（成本值：{inv.Inv_Cost}）");
            }

            // 注意：这里不更新库存数量，数量的变动要在调用方处理
            // inv.Quantity = newQty; // 已移除，避免重复计算数量
        }


        /// <summary>
        /// 采购价格调整专用：仅修正移动加权平均成本，不改动库存数量。
        /// </summary>
        /// <param name="_appContext"></param>
        /// <param name="inv">要调整的库存记录</param>
        /// <param name="costDiff">不含税的成本差异额（可正可负，含运费）</param>
        public static void AdjustCostOnly(ApplicationContext _appContext,
                                          tb_Inventory inv,
                                          decimal costDiff)
        {
            // 只允许移动加权平均法通过本接口调成本，其余方法直接返回
            var m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;
            if (m != 库存成本计算方式.移动加权平均法)
                return;

            // 当前库存数量
            int onHandQty = inv.Quantity;

            // 极端保护：库存数量为 0 时不允许再调成本
            if (onHandQty == 0)
                throw new InvalidOperationException("库存数量为 0 时不允许再调整成本");

            // 把差异额直接加到总库存金额上
            decimal originalAmount = inv.Inv_Cost * onHandQty;
            decimal newTotalAmount = originalAmount + costDiff;

            // 重新计算单位成本
            inv.CostMovingWA = newTotalAmount / onHandQty;
            inv.Inv_Cost = inv.CostMovingWA;

            // 保护：不允许出现负成本
            if (inv.Inv_Cost < 0)
                throw new InvalidOperationException($"调整后出现负库存成本：{inv.Inv_Cost}");
        }


        public static void AntiCostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty, decimal currentCostPrice)
        {
            // 前置校验：当前操作数量不能为0（反审核无意义操作）
            if (currentQty == 0) throw new ArgumentException("反审核操作成本计算时，当前数量不能为0");

            // 获取系统配置
            bool allowNegativeInventory = _appContext.SysConfig.CheckNegativeInventory;
            // 先算成本再赋值数量（保持原有逻辑顺序）
            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;

            // 计算新库存数量
            int newQty = inv.Quantity - currentQty;

            // 负库存检查（如果不允许负库存）
            if (!allowNegativeInventory && newQty < 0)
            {
                throw new InvalidOperationException($"库存数量不能为负（操作后数量：{newQty}）");
            }

            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                {
                    // ✅ FIFO反审核: 移除最近入库的批次,恢复之前的FIFO成本
                    // 注意: 完整实现需要从批次表中删除对应批次
                    // 这里简化处理: 重新计算剩余库存的加权平均成本
                    
                    if (newQty > 0)
                    {
                        // 有剩余库存时,反算成本
                        decimal currentAmount = inv.Inv_Cost * inv.Quantity;
                        decimal removeAmount = currentCostPrice * currentQty;
                        decimal remainingAmount = currentAmount - removeAmount;
                        
                        if (remainingAmount < 0)
                        {
                            // 防止出现负金额(可能是成本价不一致导致)
                            // TODO: 添加日志记录: FIFO反审核后金额为负
                            remainingAmount = 0;
                        }
                        
                        inv.CostFIFO = remainingAmount / newQty;
                        inv.Inv_Cost = inv.CostFIFO;
                    }
                    else
                    {
                        // 库存归零,保留最后成本作为参考
                        inv.CostFIFO = currentCostPrice;
                        inv.Inv_Cost = currentCostPrice;
                    }
                    break;
                }

                case 库存成本计算方式.月加权平均:
                {
                    // ✅ 月加权平均反审核: 从月度累计中扣减
                    // 注意: 完整实现需要维护月度累计字段
                    // 这里简化为移动加权的反向计算
                    
                    if (newQty > 0)
                    {
                        decimal currentAmount = inv.Inv_Cost * inv.Quantity;
                        decimal removeAmount = currentCostPrice * currentQty;
                        decimal remainingAmount = currentAmount - removeAmount;
                        
                        if (remainingAmount < 0)
                        {
                            // TODO: 添加日志记录: 月加权平均反审核后金额为负
                            remainingAmount = 0;
                        }
                        
                        inv.CostMonthlyWA = remainingAmount / newQty;
                        inv.Inv_Cost = inv.CostMonthlyWA;
                    }
                    else
                    {
                        // 库存归零,保留最后成本
                        inv.CostMonthlyWA = currentCostPrice;
                        inv.Inv_Cost = inv.CostMonthlyWA;
                    }
                    break;
                }

                case 库存成本计算方式.移动加权平均法:
                {
                    // 计算反审核后的分母（原数量 - 当前操作数量）
                    decimal denominator = newQty;

                    // 分母异常处理
                    if (denominator == 0)
                    {
                        if (currentCostPrice > 0)
                        {
                            inv.Inv_Cost = currentCostPrice;
                        }
                        else
                        {
                            // 当分母为0且无手工指定成本时，保留原始成本更合理
                            // 但需要明确提示可能的风险
                            throw new InvalidOperationException("【移动加权平均计算时】反审核时总数量为0且未提供有效单价，是否有引用入库单？\r\n" +
                                "系统无法确定新成本。审核失败！");
                        }
                        return;
                    }
                    else
                    {
                        // 核心反算公式：
                        // 原始库存金额 = (当前库存金额 - 反审核货物金额)
                        decimal currentAmount = inv.Inv_Cost * inv.Quantity;
                        decimal removeAmount = currentCostPrice * currentQty;
                        decimal originalAmount = currentAmount - removeAmount;
                        
                        // 防止出现负金额
                        if (originalAmount < 0)
                        {
                            // TODO: 添加日志记录: 移动加权平均反审核后金额为负
                            originalAmount = 0;
                        }

                        inv.CostMovingWA = originalAmount / denominator;
                        inv.Inv_Cost = inv.CostMovingWA;
                    }
                    break;
                }

                case 库存成本计算方式.实际成本法:
                {
                    // ✅ 实际成本法反审核: 移除指定批次的成本
                    // 注意: 完整实现需要通过批次ID定位具体批次
                    // 这里简化为加权平均的反向计算
                    
                    if (newQty > 0)
                    {
                        decimal currentAmount = inv.Inv_Cost * inv.Quantity;
                        decimal removeAmount = currentCostPrice * currentQty;
                        decimal remainingAmount = currentAmount - removeAmount;
                        
                        if (remainingAmount < 0)
                        {
                            // TODO: 添加日志记录: 实际成本法反审核后金额为负
                            remainingAmount = 0;
                        }
                        
                        inv.Inv_AdvCost = remainingAmount / newQty;
                        inv.Inv_Cost = inv.Inv_AdvCost;
                    }
                    else
                    {
                        // 库存归零,保留最后成本
                        inv.Inv_AdvCost = currentCostPrice;
                        inv.Inv_Cost = inv.Inv_AdvCost;
                    }
                    break;
                }

                default:
                    throw new NotImplementedException($"未实现的成本计算方式反审核逻辑：{m}");
            }

            // 强化成本负数控制（反审核后成本不能为负）
            if (inv.Inv_Cost < 0)
            {
                throw new InvalidOperationException($"反审核计算得到负库存成本（成本值：{inv.Inv_Cost}），请检查操作数量或原成本价是否合理");
            }

            // 注意：这里不更新库存数量，数量的变动要在调用方处理
            // inv.Quantity = newQty; // 已移除，避免重复计算数量
        }


      
    }
}
