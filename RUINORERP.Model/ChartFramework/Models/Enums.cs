namespace RUINORERP.Model.ChartFramework.Models
{
    /// <summary>
    /// 图表类型枚举
    /// </summary>
    public enum ChartType
    {
        /// <summary>
        /// 折线图
        /// </summary>
        Line,

        /// <summary>
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

    /// <summary>
    /// 系列类型枚举
    /// </summary>
    public enum SeriesType { Line, Column, Pie, Area }

    /// <summary>
    /// 线型枚举
    /// </summary>
    public enum LineType
    {
        Solid,      // 实线
        Dashed,     // 虚线
        Dotted      // 点线
    }

    /// <summary>
    /// 值类型枚举
    /// </summary>
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

    /// <summary>
    /// 时间选择类型枚举
    /// </summary>
    public enum TimeSelectType
    {
        CurrentWeek = 1,
        LastWeek,
        CurrentMonth,
        LastMonth,
        CurrentQuarter,
        LastQuarter,
        CurrentYear,
        Last7Days,
        Last15Days,
        Last30Days,
        Last6Months,
        Last12Months,
        Custom
    }
}
