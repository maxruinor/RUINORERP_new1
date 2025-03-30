using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core
{
     



    public enum SeriesType { Line, Column, Pie, Area }
    public enum LineType {
        Solid,      // 实线
        Dashed,     // 虚线
        Dotted      // 点线
    }
    public enum ChartType 
    { 
        /// <summary
        /// 折线图
        /// </summary>
        Line,


        /// <summary
        /// 柱状图
        /// </summary>
        Column,

        /// <summary>
        /// 饼图
        /// </summary>
        Pie,

        Area,
         
        Curve,      // 曲线图
    
        Bar,        // 条形图
      
        Donut       // 环形图
    }
    public enum MetricType
    {
        Count, //个数
        Sum, //求和
        Avg, //平均
        Max, //最大
        Min //最小
    }


 

 

    public enum ValueType
    {
        Absolute,    // 绝对值
        Percentage,  // 百分比
        Currency,   // 货币值
        Scientific, // 科学计数
        Days,
        Hours,

    }

    /// <summary>
    /// 时间范围类型枚举
    /// 按周 应该就是最近7天
    /// </summary>
    public enum TimeRangeType
    {
        Daily,      // 按日
        Weekly,     // 按周
        Monthly,    // 按月
        YearlyMonthly,     // 按年月
        Quarterly,  // 按季度
        Yearly,     // 按年
        Custom,      // 自定义
        None
    }



    public enum DimensionType
    {
        String,
        DateTime,
        Numeric,
        Boolean
    }


}

