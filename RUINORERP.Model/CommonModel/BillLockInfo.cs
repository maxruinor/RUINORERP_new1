using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    ///  单据锁定信息
    /// </summary>
    public class BillLockInfo
    {
        public string LockedName { get; set; }
        // 发送时间
        public string LockedTime { get; set; }

        public long LockedUserID { get; set; }

        /// <summary>
        /// 可用，生效
        /// </summary>
        public bool Available { get; set; } = true;
        // 消息内容
        public string Content { get; set; }

        public int BizType { get; set; }


        public int BizTypeText { get; set; }

        public long BillID { get; set; }


        public string BillNo { get; set; }
        /// <summary>
        /// josn格式
        /// </summary>
        public string BillData { get; set; }


        //如果有值，则表示要按照这个值来处理。别人无法处理。即流程处理。
        // 下次处理者
        public string NextProcessor { get; set; }

        // 构造函数
        public BillLockInfo()
        {
            LockedTime = DateTime.Now.ToString();
        }

         
    }

}

