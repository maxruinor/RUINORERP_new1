using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UserCenter
{
    /// <summary>
    /// 中控台
    /// </summary>
    public class CentralConsole
    {
        public List<待办事项> Operations { get; set; }
        public List<数据概览> Todos { get; set; }

        public CentralConsole()
        {
            Operations = new List<待办事项>();
            Todos = new List<数据概览>();
        }

        public void AddOperation(待办事项 operation)
        {
            Operations.Add(operation);
        }

        public void AddTodo(数据概览 todo)
        {
            Todos.Add(todo);
        }
    }


    public enum 待办事项
    {
        采购_采购订单,
        采购_退款退货处理,
        销售_销售订单,
        销售_退款退货处理,
        销售_销售出库单,

        生产_计划单,
        生产_制令单,
        仓库_采购入库单,
        仓库_盘点单,
        仓库_缴库单,
        仓库_退料单,
        仓库_领料单,
        仓库_分割组合,
        其他_入库单,
        其他_出库单,

        财务_费用报销单,
        返工退库,//指加工模块
        请购单,
        借出单,
        归还单,
        损溢费用单,

        预收款单,
        预付款单,
        应收款单,
        应付款单,
        收款对账单,
        付款对账单,
        收款单,
        付款单,
        

    }

    public enum 常用操作
    {
        订单查询,
        发货管理,
        退款管理,
        客户管理,
        库存管理,
        生产管理,
        采购管理,
        数据分析,
    }

    public enum 数据概览
    {
        销售单元 = 1,
        销售情况概览 = 2,
        采购单元 = 3,
        采购金额 = 4,
        库存单元 = 5,
        生产单元 = 6,
        /// <summary>
        /// 显示其他出库入情况，盘点，销售退回
        /// </summary>
        财务单元 = 8,
        订单销售额 = 9,
        出库销售额 = 10,
        毛利润,
        净利润,
        订单量,

        库存周转率,
        生产效率,
        采购成本,
        库存情况
    }
    /// <summary>
    /// 用于展示的灵活部件
    /// </summary>
    public enum BuildingBlocksParts
    {
        销售模块,
        采购模块,
        库存模块,
        财务模块,
        客户CRM,
    }

}
