using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
  
        /// <summary>
        /// 图表交互事件参数
        /// </summary>
        public class ChartInteractionEventArgs : EventArgs
        {
            /// <summary>
            /// 交互的数据点
            /// </summary>
            public DataPoint? DataPoint { get; }

            /// <summary>
            /// 交互的系列
            /// </summary>
            public DataSeries? Series { get; }

            /// <summary>
            /// 交互类型（点击、悬停等）
            /// </summary>
            public InteractionType InteractionType { get; }

            /// <summary>
            /// 鼠标位置（如适用）
            /// </summary>
            public PointF? MousePosition { get; }

            public ChartInteractionEventArgs(
                DataPoint? dataPoint,
                DataSeries? series,
                InteractionType interactionType,
                PointF? mousePosition = null)
            {
                DataPoint = dataPoint;
                Series = series;
                InteractionType = interactionType;
                MousePosition = mousePosition;
            }
        }

        /// <summary>
        /// 交互类型枚举
        /// </summary>
        public enum InteractionType
        {
            Click,
            Hover,
            RightClick,
            DoubleClick,
            SelectionChanged
        }
    }
 
