using System;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助级别枚举
    /// 定义帮助系统的四级帮助层次结构
    /// </summary>
    public enum HelpLevel
    {
        /// <summary>
        /// 字段级别 - 最细粒度的帮助
        /// 用于描述单个字段/属性的详细说明
        /// 例如: 客户ID、订单日期、数量等字段
        /// </summary>
        Field = 0,

        /// <summary>
        /// 控件级别 - 控件使用说明
        /// 用于描述单个控件的功能和使用方法
        /// 例如: 保存按钮、查询按钮、客户下拉框等
        /// </summary>
        Control = 1,

        /// <summary>
        /// 窗体级别 - 整个窗体的帮助
        /// 用于描述整个窗体的功能、操作流程和注意事项
        /// 例如: 销售订单管理窗体、采购订单管理窗体等
        /// </summary>
        Form = 2,

        /// <summary>
        /// 模块级别 - 业务模块的介绍
        /// 用于描述整个业务模块的概述、相关功能和业务流程
        /// 例如: 销售管理、采购管理、库存管理等模块
        /// </summary>
        Module = 3
    }
}
