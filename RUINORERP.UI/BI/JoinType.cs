// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：SQL连接类型枚举，用于行级权限规则编辑窗体
// ********************************************************

using System;
using System.ComponentModel;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// SQL连接类型枚举
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// 内连接（INNER JOIN）
        /// </summary>
        [Description("内连接")]
        Inner = 0,

        /// <summary>
        /// 左连接（LEFT JOIN）
        /// </summary>
        [Description("左连接")]
        Left = 1,

        /// <summary>
        /// 右连接（RIGHT JOIN）
        /// </summary>
        [Description("右连接")]
        Right = 2,

        /// <summary>
        /// 全连接（FULL JOIN）
        /// </summary>
        [Description("全连接")]
        Full = 3,

        /// <summary>
        /// 不需要联表
        /// </summary>
        [Description("不需要联表")]
        None = 99
    }

    /// <summary>
    /// JoinType扩展方法
    /// </summary>
    public static class JoinTypeExtensions
    {
        /// <summary>
        /// 获取JoinType对应的SQL JOIN语句
        /// </summary>
        /// <param name="joinType">连接类型</param>
        /// <returns>SQL JOIN语句</returns>
        public static string ToSqlString(this JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.Inner:
                    return "INNER JOIN";
                case JoinType.Left:
                    return "LEFT JOIN";
                case JoinType.Right:
                    return "RIGHT JOIN";
                case JoinType.Full:
                    return "FULL JOIN";
                case JoinType.None:
                default:
                    return "";
            }
        }

        /// <summary>
        /// 从字符串解析JoinType
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <returns>JoinType枚举值</returns>
        public static JoinType ParseFrom(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return JoinType.None;
            }

            value = value.ToUpper().Replace(" ", "");
            
            switch (value)
            {
                case "INNERJOIN":
                case "INNER":
                    return JoinType.Inner;
                case "LEFTJOIN":
                case "LEFT":
                    return JoinType.Left;
                case "RIGHTJOIN":
                case "RIGHT":
                    return JoinType.Right;
                case "FULLJOIN":
                case "FULL":
                    return JoinType.Full;
                default:
                    return JoinType.None;
            }
        }

        /// <summary>
        /// 获取JoinType的描述
        /// </summary>
        /// <param name="joinType">连接类型</param>
        /// <returns>描述文本</returns>
        public static string GetDescription(this JoinType joinType)
        {
            var field = joinType.GetType().GetField(joinType.ToString());
            if (field == null)
            {
                return joinType.ToString();
            }

            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? joinType.ToString();
        }
    }
}
