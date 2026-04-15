using RUINORERP.Model.UI;
using RUINORERP.UI.ToolForm;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RUINORERP.UI.ToolForm
{
    /// <summary>
    /// 单据选择器适配器
    /// 将 frmAdvanceSelector 适配为 IDocumentSelector 接口
    /// </summary>
    /// <typeparam name="T">单据类型</typeparam>
    public class DocumentSelectorAdapter<T> : IDocumentSelector<T> where T : class
    {
        private readonly frmAdvanceSelector<T> _selector;

        public DocumentSelectorAdapter()
        {
            _selector = new frmAdvanceSelector<T>();
        }

        /// <summary>
        /// 选中的单据列表
        /// </summary>
        public List<T> SelectedItems
        {
            get => _selector.SelectedItems;
        }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiSelect
        {
            get => _selector.AllowMultiSelect;
            set => _selector.AllowMultiSelect = value;
        }

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string SelectorTitle
        {
            get => _selector.SelectorTitle;
            set => _selector.SelectorTitle = value;
        }

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        public string ConfirmButtonText
        {
            get => _selector.ConfirmButtonText;
            set => _selector.ConfirmButtonText = value;
        }

        /// <summary>
        /// 配置列映射
        /// </summary>
        public void ConfigureColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string columnTitle = "")
        {
            _selector.ConfigureColumn(propertyExpression, columnTitle);
        }

        /// <summary>
        /// 配置求和列
        /// </summary>
        public void ConfigureSummaryColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            _selector.ConfigureSummaryColumn(propertyExpression);
        }

        /// <summary>
        /// 初始化选择器
        /// </summary>
        public void InitializeSelector(List<T> dataSource, string title = "请选择")
        {
            _selector.InitializeSelector(dataSource, title);
        }

        /// <summary>
        /// 设置默认选中的项目
        /// </summary>
        public void SetDefaultSelectedItems(List<T> items)
        {
            _selector.SetDefaultSelectedItems(items);
        }

        /// <summary>
        /// 显示选择窗体
        /// </summary>
        public bool ShowDialog()
        {
            return _selector.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }
    }

    /// <summary>
    /// 单据选择器工厂
    /// </summary>
    public class DocumentSelectorFactory : IDocumentSelectorFactory
    {
        /// <summary>
        /// 创建单据选择器
        /// </summary>
        public IDocumentSelector<T> CreateSelector<T>() where T : class
        {
            return new DocumentSelectorAdapter<T>();
        }
    }
}
