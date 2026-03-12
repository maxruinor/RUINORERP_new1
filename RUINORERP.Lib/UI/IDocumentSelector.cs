using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RUINORERP.Lib.UI
{
    /// <summary>
    /// 单据选择器接口
    /// 用于业务层调用 UI 层的选择窗体，避免循环依赖
    /// </summary>
    /// <typeparam name="T">单据类型</typeparam>
    public interface IDocumentSelector<T> where T : class
    {
        /// <summary>
        /// 选中的单据列表
        /// </summary>
        List<T> SelectedItems { get; }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        bool AllowMultiSelect { get; set; }

        /// <summary>
        /// 窗体标题
        /// </summary>
        string SelectorTitle { get; set; }

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        string ConfirmButtonText { get; set; }

        /// <summary>
        /// 配置列映射
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="columnTitle">列标题</param>
        void ConfigureColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string columnTitle = "");

        /// <summary>
        /// 配置求和列
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式</param>
        void ConfigureSummaryColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression);

        /// <summary>
        /// 初始化选择器
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="title">窗体标题</param>
        void InitializeSelector(List<T> dataSource, string title = "请选择");

        /// <summary>
        /// 设置默认选中的项目
        /// </summary>
        /// <param name="items">要默认选中的项目列表</param>
        void SetDefaultSelectedItems(List<T> items);

        /// <summary>
        /// 显示选择窗体
        /// </summary>
        /// <returns>用户是否确认选择</returns>
        bool ShowDialog();
    }

    /// <summary>
    /// 单据选择器工厂接口
    /// </summary>
    public interface IDocumentSelectorFactory
    {
        /// <summary>
        /// 创建单据选择器
        /// </summary>
        /// <typeparam name="T">单据类型</typeparam>
        /// <returns>单据选择器实例</returns>
        IDocumentSelector<T> CreateSelector<T>() where T : class;
    }
}
