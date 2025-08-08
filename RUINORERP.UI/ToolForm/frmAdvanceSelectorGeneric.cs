using RUINORERP.Common.Extensions;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Filters;

namespace RUINORERP.UI.ToolForm
{
    public partial class frmAdvanceSelector<T> : Krypton.Toolkit.KryptonForm
    {
        // 选中的单据列表
        public List<T> SelectedItems { get; private set; } = new List<T>();


        // 是否允许多选
        public bool AllowMultiSelect { get; set; } = false;
        // 窗体标题
        public string SelectorTitle
        {
            get => this.Text;
            set => this.Text = value;
        }

        // 确认按钮文本
        public string ConfirmButtonText
        {
            get => btnOk.Text;
            set => btnOk.Text = value;
        }

        // 列配置：属性名 -> 列标题
        private Dictionary<string, string> _columnMappings = new Dictionary<string, string>();
        // 用于表达式树配置的列映射
        private readonly Dictionary<string, string> _expressionColumnMappings = new Dictionary<string, string>();



        #region 求指定列的和
        /// <summary>
        /// 保存要总计的列
        /// </summary>
        public List<string> SummaryCols { get; set; } = new List<string>();


        /// <summary>
        /// 使用表达式树配置列映射
        /// </summary>
        public void ConfigureSummaryColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyName = propertyExpression.GetMemberInfo().Name;
            if (!SummaryCols.Contains(propertyName))
            {
                SummaryCols.Add(propertyName);
            }
        }
        #endregion



        /// <summary>
        /// 使用表达式树配置列映射
        /// </summary>
        /// <param name="columnTitle">如果有值，则按指定。没有，则按实体的字段描述</param>
        public void ConfigureColumn<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string columnTitle = "")
        {
            var propertyName = propertyExpression.GetMemberInfo().Name;
            _expressionColumnMappings[propertyName] = columnTitle;
        }

        // 数据源
        private List<T> _dataSource;

        public frmAdvanceSelector()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;

        }

        /// <summary>
        /// 初始化选择器
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="columnMappings">列配置（属性名 -> 列标题）</param>
        /// <param name="title">窗体标题</param>
        public void InitializeSelector(List<T> dataSource, Dictionary<string, string> columnMappings, string title = "请选择")
        {
            #region 初始化
            DisplayTextResolver = new GridViewDisplayTextResolver(typeof(T));
            dgvItems.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvItems.XmlFileName = this.Name + typeof(T).Name + "frmAdvanceSelector";

            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dgvItems.FieldNameList.TryRemove(item, out kv);
            }
            dgvItems.BizInvisibleCols = InvisibleCols;

            DisplayTextResolver.Initialize(dgvItems);

            #endregion

            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _columnMappings = columnMappings ?? GenerateDefaultColumns();

            SelectorTitle = title;
            BindData();
            ConfigureDataGridView();

        }

        /// <summary>
        /// 初始化选择器
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="columnMappings">列配置（属性名 -> 列标题）</param>
        /// <param name="title">窗体标题</param>
        public void InitializeSelector(List<T> dataSource, string title = "请选择")
        {
            #region 初始化
            DisplayTextResolver = new GridViewDisplayTextResolver(typeof(T));
            dgvItems.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvItems.XmlFileName = title + this.Name + typeof(T).Name + "frmAdvanceSelector";
            dgvItems.NeedSaveColumnsXml = true;
            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dgvItems.FieldNameList.TryRemove(item, out kv);
            }

            // 合并表达式配置和直接配置的列映射
            _columnMappings = _expressionColumnMappings.Any()
                ? new Dictionary<string, string>(_expressionColumnMappings)
                : GenerateDefaultColumns();
            ReplaceColumnHeaders();

            dgvItems.BizInvisibleCols = InvisibleCols;

            DisplayTextResolver.Initialize(dgvItems);
            #endregion

            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            SelectorTitle = title;
            BindData();
            ConfigureDataGridView();
        }

        // 完善列头替换功能的方法
        private void ReplaceColumnHeaders()
        {
            // 存储处理后的字段列表
            var updatedFieldNames = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();

            foreach (var item in _columnMappings)
            {
                // 查找对应的字段信息
                var existingKv = dgvItems.FieldNameList
                    .FirstOrDefault(c => c.Key == item.Key);

                // 如果找到对应字段
                if (existingKv.Key != null)
                {
                    // 提取原有的布尔值
                    bool originalBoolValue = existingKv.Value.Value;

                    // 确定最终使用的列标题
                    // 如果映射中有新标题则使用新标题，否则保留原有标题
                    string columnHeader = !string.IsNullOrEmpty(item.Value)
                        ? item.Value
                        : existingKv.Value.Key;

                    // 更新字段信息
                    updatedFieldNames[item.Key] = new KeyValuePair<string, bool>(columnHeader, originalBoolValue);
                }
            }

            // 处理那些在_columnMappings中没有对应配置的字段，保留其原有配置
            foreach (var existingField in dgvItems.FieldNameList)
            {
                if (!updatedFieldNames.ContainsKey(existingField.Key))
                {
                    updatedFieldNames[existingField.Key] = existingField.Value;
                }
            }

            // 将更新后的字段列表写回数据源
            dgvItems.FieldNameList = updatedFieldNames;
        }


        /// <summary>
        /// 配置DataGridView
        /// </summary>
        private void ConfigureDataGridView()
        {
            dgvItems.SelectionMode = AllowMultiSelect
                ? DataGridViewSelectionMode.FullRowSelect
                : DataGridViewSelectionMode.FullRowSelect;

            dgvItems.MultiSelect = AllowMultiSelect;
            dgvItems.UseSelectedColumn = AllowMultiSelect;
            dgvItems.RowHeadersVisible = true;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            //dgvItems.AutoGenerateColumns = false;
            //dgvItems.Columns.Clear();

            // 优化：只清除除了"Selected"之外的列
            //var columnsToRemove = dgvItems.Columns.Cast<DataGridViewColumn>()
            //    .Where(c => c.Name != "Selected")
            //    .ToList();

            //foreach (var column in columnsToRemove)
            //{
            //    foreach (var mapping in _columnMappings)
            //    {
            //        if (column.Name == mapping.Key)
            //        {
            //            column.Visible = true;
            //        }
            //        else
            //        {
            //            column.Visible = false;
            //            var colDisplay = dgvItems.ColumnDisplays.FirstOrDefault(c => c.ColName == column.Name);
            //            colDisplay.Disable = false;
            //        }
            //    }
            //}

            // 优化：隐藏除_columnMappings指定外的所有列，并禁用它们
            var columnsToProcess = dgvItems.Columns.Cast<DataGridViewColumn>().ToList();

            foreach (var column in columnsToProcess)
            {
                if (_columnMappings.ContainsKey(column.Name))
                {
                    // 显示_columnMappings中指定的列
                    column.Visible = true;
                }
                else if (column.Name == "Selected")
                {
                    // 默认隐藏但是可以启用
                    column.Visible = false;
                    var colDisplay = dgvItems.ColumnDisplays.FirstOrDefault(c => c.ColName == column.Name);
                    if (colDisplay != null)
                    {
                        colDisplay.Disable = false;
                    }
                }
                else
                {
                    // 隐藏并禁用其他列
                    column.Visible = false;

                    // 禁用列显示设置
                    var colDisplay = dgvItems.ColumnDisplays.FirstOrDefault(c => c.ColName == column.Name);
                    if (colDisplay != null)
                    {
                        colDisplay.Disable = true; // 注意这里改为true，表示禁用
                    }
                }
            }

            // 添加选择列（如果允许多选）
            //if (AllowMultiSelect && !dgvItems.Columns.Contains("Selected"))
            //{
            //    dgvItems.RowHeadersWidth = 50;
            //    dgvItems.RowHeadersVisible = true;
            //}

            // 添加数据列
            //foreach (var mapping in _columnMappings)
            //{
            //    dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            //    {
            //        HeaderText = mapping.Value,
            //        DataPropertyName = mapping.Key,
            //        Name = mapping.Key,
            //        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //    });
            //}
        }


        /// <summary>
        /// 保存不可见的列
        /// 系统设置为不可用，或程序中控制了不可见的列
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            if (_dataSource == null) return;

            var bindingList = new BindingList<T>(_dataSource);
            var bindingSource = new BindingSource(bindingList, null);
            dgvItems.DataSource = bindingSource;
            //// 自动调整列宽
            //foreach (DataGridViewColumn column in dgvItems.Columns)
            //{
            //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //}
            if (SummaryCols.Count > 0)
            {
                dgvItems.IsShowSumRow = true;
                dgvItems.SumColumns = SummaryCols.ToArray();
            }
        }

        /// <summary>
        /// 生成默认列配置（使用属性名作为标题）
        /// </summary>
        private Dictionary<string, string> GenerateDefaultColumns()
        {
            var columns = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                // 跳过复杂类型
                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    continue;

                columns.Add(prop.Name, prop.Name);
            }
            return columns;
        }

        /// <summary>
        /// 设置自定义格式化器
        /// </summary>
        public void SetColumnFormatter(string propertyName, Func<object, string> formatter)
        {
            foreach (DataGridViewColumn column in dgvItems.Columns)
            {
                if (column.DataPropertyName == propertyName)
                {
                    column.DefaultCellStyle.FormatProvider = new CustomFormatter(formatter);
                    break;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 结束当前单元格编辑状态
            dgvItems.EndEdit();

            // 收集被勾选的行
            var checkedRows = new List<T>();
            if (dgvItems.MultiSelect)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    // 获取"Selected"列的CheckBox状态
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null && Convert.ToBoolean(checkBoxCell.Value) == true)
                    {
                        checkedRows.Add((T)row.DataBoundItem);
                    }
                }
                SelectedItems = checkedRows;
            }

            //优先多选模式
            if (SelectedItems.Count == 0 && !dgvItems.MultiSelect)
            {
                SelectedItems = dgvItems.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Select(row => (T)row.DataBoundItem)
                    .ToList();
            }
            // 检查是否有勾选的行
            if (SelectedItems.Count == 0)
            {
                MessageBox.Show("请至少勾选一项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 设置选中项并关闭对话框

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dgvItems.DataSource = new BindingSource(new BindingList<T>(_dataSource), null);
                return;
            }

            var filtered = _dataSource.Where(item =>
                _columnMappings.Keys.Any(prop =>
                {
                    var value = item.GetType().GetProperty(prop)?.GetValue(item)?.ToString();
                    return value?.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                })
            ).ToList();

            dgvItems.DataSource = new BindingSource(new BindingList<T>(filtered), null);
        }

        private void dgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOk.PerformClick();
            }
        }

        public GridViewDisplayTextResolver DisplayTextResolver;
        private void frmAdvanceSelector_Load(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// 自定义格式化器
    /// </summary>
    public class CustomFormatter : ICustomFormatter, IFormatProvider
    {
        private readonly Func<object, string> _formatter;

        public CustomFormatter(Func<object, string> formatter)
        {
            _formatter = formatter;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return _formatter(arg);
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }


}
