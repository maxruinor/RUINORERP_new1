using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF
{
    public class WorkflowContext
    {
        public OrderDetails OrderDetails { get; set; }
        // 其他可能需要的属性
    }

    public class OrderDetails
    {
        public decimal Amount { get; set; }
        public string Submitter { get; set; }
        public string Approver { get; set; }
        // 其他订单详情
    }
}
