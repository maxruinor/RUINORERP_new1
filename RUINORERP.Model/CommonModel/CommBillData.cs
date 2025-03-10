using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 通过单据工厂统一输出公共数据
    /// </summary>
    [Serializable]
    public class CommBillData
    {
        private long billID;
        private string billNo;
        private string bizName;
        private BizType _bizType;
        private Type bizEntityType;
        public long BillID { get => billID; set => billID = value; }
        public string BillNo { get => billNo; set => billNo = value; }

        public string BizName { get => bizName; set => bizName = value; }
        public Type BizEntityType { get => bizEntityType; set => bizEntityType = value; }
        public BizType BizType { get => _bizType; set => _bizType = value; }
    }
}
