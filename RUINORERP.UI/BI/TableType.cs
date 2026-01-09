// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：表类型枚举，用于行级权限规则编辑窗体
// ********************************************************

using System;
using System.ComponentModel;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 表类型枚举
    /// 用于分类不同的数据表，便于在界面中展示
    /// </summary>
    public enum TableType
    {
        /// <summary>
        /// 基础数据表（如产品、仓库等）
        /// </summary>
        [Description("基础数据")]
        Base = 0,

        /// <summary>
        /// 业务数据表（如订单、入库单等）
        /// </summary>
        [Description("业务数据")]
        Business = 1,

        /// <summary>
        /// 视图
        /// </summary>
        [Description("视图")]
        View = 2,

        /// <summary>
        /// 系统表
        /// </summary>
        [Description("系统表")]
        System = 3,

        /// <summary>
        /// 临时表
        /// </summary>
        [Description("临时表")]
        Temporary = 4,

        /// <summary>
        /// 未分类
        /// </summary>
        [Description("未分类")]
        Unknown = 99
    }

    /// <summary>
    /// 表信息类，用于存储表的详细信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体类型全限定名
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 表类型
        /// </summary>
        public TableType TableType { get; set; }

        /// <summary>
        /// 是否可缓存
        /// </summary>
        public bool IsCacheable { get; set; }

        /// <summary>
        /// 模块名称（可选）
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 显示文本（用于下拉框显示）
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return TableName;
                }
                return $"{Description} ({TableName})";
            }
        }
    }
}
