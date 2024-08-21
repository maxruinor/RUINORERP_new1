using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation
{
    public class BaseBizData
    {
        public BizType BizType;

        /// <summary>
        /// 业务单号
        /// </summary>
        public long BizID;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID;
    }
}
