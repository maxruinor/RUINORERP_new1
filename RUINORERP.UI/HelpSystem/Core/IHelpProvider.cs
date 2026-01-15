using System;
using System.Collections.Generic;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助提供者接口
    /// 定义帮助内容提供者的标准契约
    /// 支持本地文件系统和在线API两种实现方式
    /// 通过依赖注入可以灵活切换不同的帮助提供者
    /// </summary>
    public interface IHelpProvider : IDisposable
    {
        #region 获取帮助内容

        /// <summary>
        /// 获取帮助内容
        /// 根据帮助上下文信息定位并返回相应的帮助内容
        /// </summary>
        /// <param name="context">帮助上下文,包含帮助级别、实体类型、字段名等信息</param>
        /// <returns>帮助内容(HTML或Markdown格式),未找到则返回null</returns>
        string GetHelpContent(HelpContext context);

        #endregion

        #region 帮助搜索

        /// <summary>
        /// 搜索帮助内容
        /// 根据关键词在帮助内容中搜索匹配的结果
        /// 支持模糊搜索和相关性排序
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="context">当前帮助上下文,用于优先级排序(可选)</param>
        /// <returns>搜索结果列表,按相关度从高到低排序</returns>
        List<HelpSearchResult> Search(string keyword, HelpContext context = null);

        #endregion

        #region 帮助存在性检查

        /// <summary>
        /// 检查指定上下文的帮助是否存在
        /// 用于在显示帮助前快速判断是否有相关帮助内容
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助存在返回true,否则返回false</returns>
        bool HelpExists(HelpContext context);

        #endregion

        #region 索引管理

        /// <summary>
        /// 重新加载帮助索引
        /// 当帮助文件内容更新时调用此方法重新建立索引
        /// </summary>
        void ReloadIndex();

        #endregion

        #region 元数据

        /// <summary>
        /// 获取帮助提供者名称
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// 获取帮助内容根路径
        /// </summary>
        string HelpContentRootPath { get; }

        /// <summary>
        /// 获取帮助总数
        /// </summary>
        int HelpCount { get; }

        #endregion
    }
}
