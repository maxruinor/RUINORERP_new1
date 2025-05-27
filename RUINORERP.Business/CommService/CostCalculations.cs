using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 成本计算
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
                 
        /// <summary>
        /// TODO成本这块还需要完善
        /// </summary>
        /// <param name="_appContext"></param>
        /// <param name="inv"></param>
        /// <param name="currentCostPrice"></param>
        public static void CostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty, decimal currentCostPrice)
        {
            //注意！！！！！！！！
            //要先算成本再赋值数量

            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;
            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                    inv.CostFIFO = currentCostPrice;
                    inv.Inv_Cost = inv.CostFIFO;
                    break;
                case 库存成本计算方式.月加权平均:
                    inv.CostMonthlyWA = currentCostPrice;
                    inv.Inv_Cost = inv.CostMonthlyWA;
                    break;
                case 库存成本计算方式.移动加权平均法:
                    inv.CostMovingWA = (currentCostPrice * currentQty + inv.Inv_Cost * inv.Quantity) / (currentQty + inv.Quantity);
                    inv.Inv_Cost = inv.CostMovingWA;
                    break;
                case 库存成本计算方式.实际成本法:
                    inv.Inv_AdvCost = currentCostPrice;
                    inv.Inv_Cost = inv.Inv_AdvCost;
                    break;

                default:
                    break;
            }

            if (inv.Inv_Cost < 0)
            {
                throw new Exception("库存成本不能小于0");
            }
        }
          */

        public static void CostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty, decimal UntaxedUnitPrice, decimal UntaxedShippCost = 0)
        {

            //新成本单价 = （原库存成本×原数量 + (采购不含税单价×采购数量 + 不含税运费)）/(原数量 + 采购数量 )
            // 先算成本再赋值数量（保持原有逻辑顺序）
            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;

            // 前置校验：当前操作数量不能为0（避免无意义操作）
            if (currentQty == 0)
            {
                throw new ArgumentException("当前操作数量不能为0");
            }
            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                    // 先进先出法不允许负库存（符合常规业务逻辑）
                    if (inv.Quantity < 0)
                    {
                        throw new InvalidOperationException("先进先出法下不允许现有库存为负数");
                    }
                    inv.CostFIFO = UntaxedUnitPrice;
                    inv.Inv_Cost = inv.CostFIFO;
                    break;

                case 库存成本计算方式.月加权平均:
                    // 月加权平均法要求现有库存不能为负（否则无法正确计算月度平均）
                    if (inv.Quantity < 0)
                    {
                        throw new InvalidOperationException("月加权平均法下不允许现有库存为负数");
                    }
                    inv.CostMonthlyWA = UntaxedUnitPrice;
                    inv.Inv_Cost = inv.CostMonthlyWA;
                    break;

                case 库存成本计算方式.移动加权平均法:
                    // 计算移动加权平均时的关键分母
                    decimal totalQty = currentQty + inv.Quantity;

                    // 分母异常处理（避免除零或负数导致的不合理计算）
                    if (totalQty == 0)
                    {
                        throw new DivideByZeroException("移动加权平均计算时总数量不能为0");
                    }
                    if (totalQty < 0)
                    {
                        // 当总数量为负时，检查是否因负库存导致
                        if (inv.Quantity < 0 && Math.Abs(inv.Quantity) > currentQty)
                        {
                            throw new InvalidOperationException("移动加权平均计算时，现有负库存数量超过当前操作数量，无法正确计算成本");
                        }
                        // 其他情况（如currentQty为负且绝对值过大）
                        throw new InvalidOperationException($"移动加权平均计算时总数量不能为负（当前总数量：{totalQty}）");
                    }

                    // 正常计算逻辑
                    inv.CostMovingWA = (UntaxedUnitPrice * currentQty + (inv.Inv_Cost * inv.Quantity + UntaxedShippCost)) / totalQty;
                    inv.Inv_Cost = inv.CostMovingWA;
                    break;

                case 库存成本计算方式.实际成本法:
                    // 实际成本法要求严格按批次成本记录，不允许负库存
                    if (inv.Quantity < 0)
                    {
                        throw new InvalidOperationException("实际成本法下不允许现有库存为负数");
                    }
                    inv.Inv_AdvCost = UntaxedUnitPrice;
                    inv.Inv_Cost = inv.Inv_AdvCost;
                    break;

                default:
                    throw new NotImplementedException($"未实现的成本计算方式：{m}");
            }

            // 强化成本负数控制（原逻辑补充）
            if (inv.Inv_Cost < 0)
            {
                throw new InvalidOperationException($"计算得到负库存成本（成本值：{inv.Inv_Cost}），请检查操作数量或成本价是否合理");
            }
        }
        public static void AntiCostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty, decimal currentCostPrice)
        {
            // 先算成本再赋值数量（保持原有逻辑顺序）
            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;

            // 前置校验：当前操作数量不能为0（反审核无意义操作）
            if (currentQty == 0)
            {
                throw new ArgumentException("反审核操作的当前数量不能为0");
            }

            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                    // 反审核后库存数量 = 原数量 - 当前操作数量（假设currentQty为原操作数量）
                    int newQuantity = inv.Quantity - currentQty;
                    if (newQuantity < 0)
                    {
                        throw new InvalidOperationException($"先进先出法反审核后库存数量不能为负（当前计算结果：{newQuantity}）");
                    }
                    inv.CostFIFO = currentCostPrice;
                    inv.Inv_Cost = inv.CostFIFO;
                    break;

                case 库存成本计算方式.月加权平均:
                    int monthlyNewQty = inv.Quantity - currentQty;
                    if (monthlyNewQty < 0)
                    {
                        throw new InvalidOperationException($"月加权平均法反审核后库存数量不能为负（当前计算结果：{monthlyNewQty}）");
                    }
                    inv.CostMonthlyWA = currentCostPrice;
                    inv.Inv_Cost = inv.CostMonthlyWA;
                    break;

                case 库存成本计算方式.移动加权平均法:
                    // 计算反审核后的分母（原数量 - 当前操作数量）
                    decimal denominator = inv.Quantity - currentQty;

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
                            throw new InvalidOperationException("移动加权平均反审核时总数量为0且未提供有效手工成本，无法确定新成本");
                        }
                        return;
                    }
                    if (denominator < 0)
                    {
                        // 检查是否因反审核数量过大导致负库存
                        if (currentQty > inv.Quantity)
                        {
                            throw new InvalidOperationException($"移动加权平均反审核后库存数量为负（原数量：{inv.Quantity}, 操作数量：{currentQty}）");
                        }
                        throw new InvalidOperationException($"移动加权平均反审核分母不能为负（当前分母：{denominator}）");
                    }

                    // 执行移动加权平均反计算
                    inv.CostMovingWA = (inv.CostMovingWA * inv.Quantity - currentCostPrice * currentQty) / denominator;
                    inv.Inv_Cost = inv.CostMovingWA;
                    break;

                case 库存成本计算方式.实际成本法:
                    int actualNewQty = inv.Quantity - currentQty;
                    if (actualNewQty < 0)
                    {
                        throw new InvalidOperationException($"实际成本法反审核后库存数量不能为负（当前计算结果：{actualNewQty}）");
                    }
                    inv.Inv_AdvCost = currentCostPrice;
                    inv.Inv_Cost = inv.Inv_AdvCost;
                    break;

                default:
                    throw new NotImplementedException($"未实现的成本计算方式反审核逻辑：{m}");
            }

            // 强化成本负数控制（反审核后成本不能为负）
            if (inv.Inv_Cost < 0)
            {
                throw new InvalidOperationException($"反审核计算得到负库存成本（成本值：{inv.Inv_Cost}），请检查操作数量或原成本价是否合理");
            }
        }


        ///// <summary>
        ///// 反成本计算，反审核时使用
        ///// </summary>
        ///// <param name="_appContext"></param>
        ///// <param name="inv"></param>
        ///// <param name="currentQty"></param>
        ///// <param name="currentCostPrice"></param>
        //public static void AntiCostCalculation(ApplicationContext _appContext, tb_Inventory inv, int currentQty, decimal currentCostPrice)
        //{
        //    //注意！！！！！！！！
        //    //要先算成本再赋值数量
        //    Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;
        //    switch (m)
        //    {
        //        case 库存成本计算方式.先进先出法:
        //            inv.CostFIFO = currentCostPrice;
        //            inv.Inv_Cost = inv.CostFIFO;
        //            break;
        //        case 库存成本计算方式.月加权平均:
        //            inv.CostMonthlyWA = currentCostPrice;
        //            inv.Inv_Cost = inv.CostMonthlyWA;
        //            break;
        //        case 库存成本计算方式.移动加权平均法:
        //            //防止除0错误
        //            if ((inv.Quantity - currentQty) == 0)
        //            {
        //                //如果没有引用入库明细传入成本。则要手工指定。如果为零则是没有手工指定。将默认保留原始成本
        //                if (currentCostPrice > 0)
        //                {
        //                    inv.Inv_Cost = currentCostPrice;
        //                }

        //                return;
        //            }
        //            inv.CostMovingWA = (inv.CostMovingWA * inv.Quantity - currentCostPrice * currentQty) / (inv.Quantity - currentQty);
        //            inv.Inv_Cost = inv.CostMovingWA;
        //            break;
        //        case 库存成本计算方式.实际成本法:
        //            inv.Inv_AdvCost = currentCostPrice;
        //            inv.Inv_Cost = inv.Inv_AdvCost;
        //            break;

        //        default:
        //            break;
        //    }



        //}
    }
}
