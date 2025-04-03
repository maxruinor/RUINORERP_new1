using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core
{




    public enum SeriesType { Line, Column, Pie, Area }
    public enum LineType
    {
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

    public enum MetricUnit
    {
        笔,
        元,
        个,
        人,
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
        None = 0,
        Daily = 1,      // 按日
        Weekly,     // 按周
        Monthly,    // 按月
        Quarterly,  // 按季度
        Yearly,     // 按年
        Custom,      // 自定义
    }



    public enum TimeSelectType
    {
        [Description("最近7天")]
        Last7Days = 1,

        [Description("最近15天")]
        Last15Days,

        [Description("最近30天")]
        Last30Days,

        [Description("本周")]
        CurrentWeek,

        [Description("上周")]
        LastWeek,

        [Description("本月")]
        CurrentMonth,

        [Description("上个月")]
        LastMonth,

        [Description("本季度")]
        CurrentQuarter,

        [Description("上季度")]
        LastQuarter,

        [Description("本年")]
        CurrentYear,

        [Description("最近半年")]
        Last6Months,

        [Description("最近一年")]
        Last12Months,

        [Description("自定义")]
        Custom
    }


    public enum DimensionType
    {
        String,
        DateTime,
        Numeric,
        Boolean
    }


}

