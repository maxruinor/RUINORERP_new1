using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.UI
{
    /// <summary>
    /// 连接线样式枚举
    /// </summary>
    public enum ConnectionLineStyle
    {
        /// <summary>
        /// 直线连接
        /// </summary>
        Straight,

        /// <summary>
        /// 直角连接线
        /// </summary>
        Orthogonal,

        /// <summary>
        /// 贝塞尔曲线
        /// </summary>
        Bezier,

        /// <summary>
        /// 平滑曲线
        /// </summary>
        Curved,

        /// <summary>
        /// 圆角连接线
        /// </summary>
        Rounded,

        /// <summary>
        /// 肘形连接线
        /// </summary>
        Elbow,

        /// <summary>
        /// 弧形连接线
        /// </summary>
        Arc
    }
}