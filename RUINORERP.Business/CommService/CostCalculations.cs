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
                   */
        /// <summary>
        /// TODO成本这块还需要完善
        /// </summary>
        /// <param name="_appContext"></param>
        /// <param name="inv"></param>
        /// <param name="currentCostPrice"></param>
        public static void CostCalculation(ApplicationContext _appContext, tb_Inventory inv,decimal currentCostPrice)
        {
            Global.库存成本计算方式 m = (Global.库存成本计算方式)_appContext.SysConfig.CostCalculationMethod;

            switch (m)
            {
                case 库存成本计算方式.先进先出法:
                    inv.CostFIFO = currentCostPrice;
                    break;
                case 库存成本计算方式.后进先出法:
                    break;
                case 库存成本计算方式.加权平均法:
                    //inv.Inv_Cost
                    break;
                case 库存成本计算方式.移动平均法:

                    break;
                default:
                    break;
            }



        }
    }
}
