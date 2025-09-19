using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 广播结果 - 使用统一的ApiResponse模式
    /// </summary>
    public class BroadcastResult : ApiResponse<BroadcastResultData>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BroadcastResult() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public BroadcastResult(bool success, string message, BroadcastResultData data = null, int code = 200) 
            : base(success, message, data, code) { }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static BroadcastResult CreateSuccess(int totalTargets, int successfulTargets, int failedTargets, string message = "广播操作成功")
        {
            return new BroadcastResult(true, message, new BroadcastResultData
            {
                TotalTargets = totalTargets,
                SuccessfulTargets = successfulTargets,
                FailedTargets = failedTargets
            }, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static new BroadcastResult Failure(string message, int code = 500)
        {
            return new BroadcastResult(false, message, null, code);
        }
    }

    /// <summary>
    /// 广播结果数据
    /// </summary>
    public class BroadcastResultData
    {
        /// <summary>
        /// 总目标数
        /// </summary>
        public int TotalTargets { get; set; }

        /// <summary>
        /// 成功目标数
        /// </summary>
        public int SuccessfulTargets { get; set; }

        /// <summary>
        /// 失败目标数
        /// </summary>
        public int FailedTargets { get; set; }
    }
}
