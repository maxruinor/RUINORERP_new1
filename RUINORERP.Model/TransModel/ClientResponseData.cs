using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{

    /// <summary>
    /// 客户端对服务器提醒的响应
    /// </summary>
    public class ClientResponseData
    {
        /// <summary>
        /// 所有业务的唯一主键
        /// </summary>
        public long BizPrimaryKey { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.Cancel;
        /// <summary>
        /// 提醒间隔，默认20秒
        /// </summary>
        public double RemindInterval { get; set; } = 20;
    
        public string ResponseId { get; set; }
        public string RequestId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string ClientId { get; set; }
        public string AdditionalData { get; set; }

        // 可以添加通用方法，例如格式化时间戳
        public string FormatTimestamp()
        {
            return Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }

}
