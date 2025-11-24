using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 工作流设计器操作枚举
    /// </summary>
    public enum WFDesignerEnum
    {
 
        新建,        // 新建
        保存,        // 保存
        打开,        // 打开
        属性,        // 属性
        剪切,        // 剪切
        复制,        // 复制
        粘贴,        // 粘贴
        撤销,        // 撤销
        重做,        // 重做
        指针,        // 指针
        绘图,        // 绘图
        连接,        // 连接
        移动连接点,  // 移动连接点
        绘制矩形,    // 绘制矩形
        绘制椭圆,    // 绘制椭圆
        文本工具,    // 文本工具
        置于底层,    // 置于底层
        置于顶层,    // 置于顶层
        向上,        // 向
        向下,        // 向下
        组合,        // 组合
        取消分组,    // 取消分组
    }

    /// <summary>
    /// 流程导航节点类型枚举
    /// </summary>
    public enum ProcessNavigationNodeType
    {
        开始节点,
        结束节点,
        审批节点,
        会签节点,
        或签节点,
        条件节点,
        通知节点,
        提交节点,
        流程导航节点
    }

    /// <summary>
    /// 流程导航图模式枚举
    /// </summary>
    public enum ProcessNavigationMode
    {
        设计模式,
        预览模式
    }

    /// <summary>
    /// ERP业务模块枚举
    /// </summary>
    public enum ERPBusinessModule
    {
        未分类 = 0,
        采购管理 = 1,
        销售管理 = 2,
        库存管理 = 3,
        生产管理 = 4,
        财务管理 = 5,
        客户关系管理 = 6,
        人力资源管理 = 7,
        质量管理 = 8,
        报表分析 = 9,
        系统管理 = 10
    }

    /// <summary>
    /// 流程导航图级别枚举
    /// </summary>
    public enum ProcessNavigationLevel
    {
        总图 = 0,      // 系统总览图
        模块图 = 1,    // 模块级别图
        业务图 = 2     // 具体业务流程图
    }

    /// <summary>
    /// 流程导航节点业务类型枚举
    /// </summary>
    public enum ProcessNavigationNodeBusinessType
    {
        通用节点 = 0,
        菜单节点 = 1,      // 关联到具体菜单
        模块节点 = 2,      // 关联到业务模块
        流程节点 = 3,      // 关联到子流程
        外部系统节点 = 4,  // 关联到外部系统
        数据源节点 = 5     // 关联到数据源
    }
}

