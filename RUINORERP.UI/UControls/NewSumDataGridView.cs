using FastReport.DevComponents.DotNetBar.Controls;
using HLH.Lib.List;
using Krypton.Toolkit;
using Newtonsoft.Json;
using NPOI.Util;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using static RUINORERP.UI.UControls.ContextMenuController;


namespace RUINORERP.UI.UControls
{


    /// <summary>
    /// 2023-8-19 在这个项目中完善 最新
    /// 2022-7-28 best new
    /// 这个控制组里面的子控件 需要一个标记2020
    /// 修改一下 让他不合计时 就是普通的,最原生的滚动条
    /// 2020-09-10 添加了 批量修改列的值的方法。（没有保存到数据库）
    /// 2020-9-10 添加修改后。保存到数据库的方法。应该是在数据源的框架内，才有效）
    /// 2021-7-9 添加一个字段 Variable storage parameters VarStoragePara
    /// 添加排序数据源如果不是的话。
    /// 添加列的隐藏功能？
    /// 2023-11-13添加列头可为checkbox 但是原一才生效，
    /// 后使用绑定实体基类中有Selected列的方法来控制  
    /// kryptonDataGridView1.MultiSelect = MultipleChoices;
    /// kryptonDataGridView1.UseSelectedColumn = MultipleChoices;
    /// 2023-11-25 优化列显示控制：this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
    /// 2025 添加了很多功能，如多选模式，批量编辑列
    /// </summary>
    [Serializable]
    public class NewSumDataGridView : KryptonDataGridView
    {
        /// <summary>
        /// 保存不可见的列 ,业务性的。由程序硬编码控制了
        /// 系统设置为不可用，或程序中控制了不可见的列
        /// add by  watson2025-6-11
        /// </summary>
        public HashSet<string> BizInvisibleCols { get; set; } = new HashSet<string>();


        private bool _CustomRowNo = false;

        /// <summary>
        /// 自定义行号,如果为真。则不会在这个类里处理添加行号的工作。得手动实现
        /// </summary>
        [Browsable(true)]
        [Description("自定义行号")]
        public bool CustomRowNo
        {
            get
            {
                return _CustomRowNo;
            }
            set
            {
                _CustomRowNo = value;

            }
        }

        public event EventHandler 删除选中行;
        private bool _是否使用内置右键功能 = true;





        [Browsable(true)]
        [Description("是否使用内置右键功能")]
        public bool Use是否使用内置右键功能
        {
            get
            {
                return _是否使用内置右键功能;
            }
            set
            {
                _是否使用内置右键功能 = value;

            }
        }

        private bool _UseCustomColumnDisplay = true;


        [Browsable(true)]
        [Description("是否使用内置自定义列显示功能")]
        public bool UseCustomColumnDisplay
        {
            get
            {
                return _UseCustomColumnDisplay;
            }
            set
            {
                _UseCustomColumnDisplay = value;
                if (_UseCustomColumnDisplay)
                {

                }
            }
        }

        private bool _UseSelectedColumn = false;
        [Browsable(true)]
        [Description("是否使用内置多选功能，基于数据源中有Selected列")]
        public bool UseSelectedColumn
        {
            get { return _UseSelectedColumn; }
            set
            {
                if (_UseSelectedColumn != value)
                {
                    _UseSelectedColumn = value;
                    SetSelectedColumn(value);
                    // 触发列显示状态更新
                    BindColumnStyle();
                }
            }
        }


        #region 实现右键多选菜单

        private bool _headerMenuShown = false; // 标记是否显示行头菜单

        //保存实际业务右键菜单
        ContextMenuStrip HolderMenu = new ContextMenuStrip();
        ContextMenuStrip headerMenu = new ContextMenuStrip();
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);
            if (e.Button == MouseButtons.Right)
            {
                if (HolderMenu.Items.Count == 0)
                {
                    HolderMenu = this.ContextMenuStrip;
                }

                //处理左上角行头右键点击
                if (e.RowIndex == -1 && e.ColumnIndex == -1 && e.Button == MouseButtons.Right)
                {
                    var headerMenu = BuildHeaderMenu(GridViewExtension.MULTI_SELECT_MODE, GridViewExtension.FILTER_BY_COLUMN);
                    this.ContextMenuStrip = headerMenu;

                    ShowHeaderContextMenu(headerMenu, e.Location);
                    _headerMenuShown = true;

                }
                //列头
                else if (e.RowIndex == -1 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Right && UseBatchEditColumn)
                {
                    _currentEditingColumn = this.Columns[e.ColumnIndex];
                    var headerMenu = BuildHeaderMenu(GridViewExtension.BATCH_EDIT_COLUMN);
                    this.ContextMenuStrip = headerMenu;
                    ShowHeaderContextMenu(headerMenu, e.Location);
                    _headerMenuShown = true;
                }
                else
                {
                    _currentEditingColumn = null;
                    this.ContextMenuStrip = HolderMenu;
                    _headerMenuShown = false;
                }
            }

        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Right)
            {
                if (!_headerMenuShown && HolderMenu.Items.Count > 0)
                {
                    if (true)
                    {
                        this.ContextMenuStrip = HolderMenu;
                    }
                }

            }
        }


        //后面优化吧。把菜单仓库过程重构 传入 菜单名称和委托事件
        private ContextMenuStrip BuildHeaderMenu(params string[] args)
        {

            //如果这里想优化是不是放到缓存中
            //if (headerMenu.Items.Count > 0)
            //{
            //    return headerMenu;
            //}


            foreach (var item in args)
            {
                if (item == GridViewExtension.MULTI_SELECT_MODE && !headerMenu.Items.ContainsKey(GridViewExtension.MULTI_SELECT_MODE))
                {
                    #region
                    // 创建带复选框的菜单项
                    ToolStripMenuItem multiSelectItem = new ToolStripMenuItem(GridViewExtension.MULTI_SELECT_MODE)
                    {
                        Name = GridViewExtension.MULTI_SELECT_MODE,
                        CheckOnClick = true,
                        Checked = this.UseSelectedColumn
                    };

                    multiSelectItem.Click += (sender, e) =>
                    {
                        this.UseSelectedColumn = ((ToolStripMenuItem)sender).Checked;
                        MultiSelect = UseSelectedColumn;
                        //((ToolStripMenuItem)sender).Checked = this.UseSelectedColumn;
                    };

                    // 添加其他行头相关菜单项...
                    headerMenu.Items.Add(multiSelectItem);
                    #endregion
                }

                if (item == GridViewExtension.FILTER_BY_COLUMN && !headerMenu.Items.ContainsKey(GridViewExtension.FILTER_BY_COLUMN))
                {
                    #region
                    // 创建带复选框的菜单项
                    ToolStripMenuItem FilterItem = new ToolStripMenuItem(GridViewExtension.FILTER_BY_COLUMN)
                    {
                        Name = GridViewExtension.FILTER_BY_COLUMN,
                        CheckOnClick = true,
                        //Text = "⚙️",
                        Checked = this.EnableFiltering
                    };

                    FilterItem.Click += (sender, e) =>
                    {
                        this.EnableFiltering = ((ToolStripMenuItem)sender).Checked;
                        //((ToolStripMenuItem)sender).Checked = this.UseSelectedColumn;
                    };

                    // 添加其他行头相关菜单项...
                    headerMenu.Items.Add(FilterItem);
                    #endregion
                }


                if (item == GridViewExtension.BATCH_EDIT_COLUMN && !headerMenu.Items.ContainsKey(GridViewExtension.BATCH_EDIT_COLUMN))
                {
                    if (UseBatchEditColumn)
                    {
                        // 新增"批量编辑列值"菜单项
                        ToolStripMenuItem batchEditItem = new ToolStripMenuItem(GridViewExtension.BATCH_EDIT_COLUMN);
                        batchEditItem.Name = GridViewExtension.BATCH_EDIT_COLUMN;
                        batchEditItem.Click += BatchEditItem_Click;
                        headerMenu.Items.Add(batchEditItem);
                    }

                }
            }


            return headerMenu;
        }





        private void ShowHeaderContextMenu(ContextMenuStrip headerMenu, Point screenPos)
        {

            //// 显示菜单前隐藏默认菜单
            //this.ContextMenuStrip?.Hide();

            //// 显示自定义菜单
            //headerMenu.Show(screenPos);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // 如果显示了行头菜单，则阻止默认右键菜单
            if (!_headerMenuShown)
            {
                base.OnMouseUp(e);
            }
            _headerMenuShown = false; // 重置标记
        }
        private bool _headerMenuActive;

        protected override void OnMouseLeave(EventArgs e)
        {
            _headerMenuActive = false;
            base.OnMouseLeave(e);
        }


        #endregion

        #region 实现批量编辑列的值的功能
        private bool _UseBatchEditColumn = false;
        [Browsable(true)]
        [Description("是否批量编辑列的值")]
        public bool UseBatchEditColumn
        {
            get { return _UseBatchEditColumn; }
            set
            {
                if (_UseBatchEditColumn != value)
                {
                    _UseBatchEditColumn = value;
                }
            }
        }

        // 在NewSumDataGridView类中添加以下成员变量
        private DataGridViewColumn _currentEditingColumn = null;
        private Form _batchEditForm = null;


        // 处理批量编辑列值菜单项点击事件
        private void BatchEditItem_Click(object sender, EventArgs e)
        {
            if (_currentEditingColumn == null)
                return;

            // 获取列的数据类型
            ColumnDataType dataType = GetColumnDataType(_currentEditingColumn);

            // 创建并显示批量编辑表单
            ShowBatchEditForm(dataType);
        }

        // 确定列的数据类型
        private ColumnDataType GetColumnDataType(DataGridViewColumn column)
        {
            if (column == null || column.DataPropertyName == string.Empty)
                return ColumnDataType.Other;

            // 如果是DataTable数据源
            if (this.DataSource is DataTable)
            {
                DataTable dt = this.DataSource as DataTable;
                if (dt.Columns.Contains(column.DataPropertyName))
                {
                    Type columnType = dt.Columns[column.DataPropertyName].DataType;

                    if (columnType == typeof(string))
                        return ColumnDataType.String;
                    else if (columnType == typeof(int) || columnType == typeof(long) ||
                             columnType == typeof(short) || columnType == typeof(byte))
                        return ColumnDataType.Integer;
                    else if (columnType == typeof(decimal) || columnType == typeof(double) ||
                             columnType == typeof(float))
                        return ColumnDataType.Decimal;
                    else if (columnType == typeof(bool))
                        return ColumnDataType.Boolean;
                    else if (columnType == typeof(DateTime))
                        return ColumnDataType.DateTime;
                }
            }
            // 如果是对象集合数据源
            else if (this.Rows.Count > 0 && this.Rows[0].DataBoundItem != null)
            {
                object item = this.Rows[0].DataBoundItem;
                Type itemType = item.GetType();
                PropertyInfo property = itemType.GetProperty(column.DataPropertyName);

                if (property != null)
                {
                    Type propertyType = property.PropertyType;

                    if (propertyType == typeof(string))
                        return ColumnDataType.String;
                    else if (propertyType == typeof(int) || propertyType == typeof(long) ||
                             propertyType == typeof(short) || propertyType == typeof(byte))
                        return ColumnDataType.Integer;
                    else if (propertyType == typeof(decimal) || propertyType == typeof(double) ||
                             propertyType == typeof(float))
                        return ColumnDataType.Decimal;
                    else if (propertyType == typeof(bool))
                        return ColumnDataType.Boolean;
                    else if (propertyType == typeof(DateTime))
                        return ColumnDataType.DateTime;
                }
            }

            return ColumnDataType.Other;
        }

        // 显示批量编辑表单
        private void ShowBatchEditForm(ColumnDataType dataType)
        {
            if (_batchEditForm != null && !_batchEditForm.IsDisposed)
                _batchEditForm.Dispose();

            _batchEditForm = new Form();
            _batchEditForm.Text = $"批量编辑列: {_currentEditingColumn.HeaderText}";
            _batchEditForm.Size = new Size(300, 180);
            _batchEditForm.StartPosition = FormStartPosition.CenterScreen;
            _batchEditForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            _batchEditForm.MaximizeBox = false;
            _batchEditForm.MinimizeBox = false;

            // 创建标签
            Label lblValue = new Label();
            lblValue.Text = "新值:";
            lblValue.Location = new Point(20, 30);
            lblValue.Size = new Size(50, 20);
            _batchEditForm.Controls.Add(lblValue);

            // 根据数据类型创建不同的输入控件
            Control inputControl = null;
            switch (dataType)
            {
                case ColumnDataType.String:
                    TextBox txtValue = new TextBox();
                    txtValue.Location = new Point(80, 30);
                    txtValue.Size = new Size(180, 25);
                    inputControl = txtValue;
                    break;

                case ColumnDataType.Integer:
                    NumericUpDown numValue = new NumericUpDown();
                    numValue.Location = new Point(80, 30);
                    numValue.Size = new Size(180, 25);
                    numValue.Minimum = int.MinValue;
                    numValue.Maximum = int.MaxValue;
                    inputControl = numValue;
                    break;

                case ColumnDataType.Decimal:
                    NumericUpDown decValue = new NumericUpDown();
                    decValue.Location = new Point(80, 30);
                    decValue.Size = new Size(180, 25);
                    decValue.Minimum = decimal.MinValue;
                    decValue.Maximum = decimal.MaxValue;
                    decValue.DecimalPlaces = 2;
                    inputControl = decValue;
                    break;

                case ColumnDataType.Boolean:
                    ComboBox boolValue = new ComboBox();
                    boolValue.Location = new Point(80, 30);
                    boolValue.Size = new Size(180, 25);
                    boolValue.Items.Add("是");
                    boolValue.Items.Add("否");
                    boolValue.SelectedIndex = 0;
                    inputControl = boolValue;
                    break;

                case ColumnDataType.DateTime:
                    DateTimePicker dtValue = new DateTimePicker();
                    dtValue.Location = new Point(80, 30);
                    dtValue.Size = new Size(180, 25);
                    inputControl = dtValue;
                    break;

                default:
                    TextBox txtOtherValue = new TextBox();
                    txtOtherValue.Location = new Point(80, 30);
                    txtOtherValue.Size = new Size(180, 25);
                    inputControl = txtOtherValue;
                    break;
            }

            if (inputControl != null)
            {
                _batchEditForm.Controls.Add(inputControl);

                // 创建"确定"按钮
                Button btnOK = new Button();
                btnOK.Text = "确定";
                btnOK.Location = new Point(80, 100);
                btnOK.Size = new Size(75, 30);
                btnOK.Click += (sender, e) =>
                {
                    ApplyBatchEditValue(inputControl, dataType);
                    _batchEditForm.Close();
                };
                _batchEditForm.Controls.Add(btnOK);

                // 创建"取消"按钮
                Button btnCancel = new Button();
                btnCancel.Text = "取消";
                btnCancel.Location = new Point(175, 100);
                btnCancel.Size = new Size(75, 30);
                btnCancel.Click += (sender, e) =>
                {
                    _batchEditForm.Close();
                };
                _batchEditForm.Controls.Add(btnCancel);
            }

            _batchEditForm.ShowDialog();
        }

        // 应用批量编辑的值到所有行
        private void ApplyBatchEditValue(Control inputControl, ColumnDataType dataType)
        {
            if (_currentEditingColumn == null || inputControl == null)
                return;

            object newValue = null;

            // 根据数据类型获取输入值
            switch (dataType)
            {
                case ColumnDataType.String:
                    newValue = ((TextBox)inputControl).Text;
                    break;

                case ColumnDataType.Integer:
                    newValue = Convert.ToInt32(((NumericUpDown)inputControl).Value);
                    break;

                case ColumnDataType.Decimal:
                    newValue = ((NumericUpDown)inputControl).Value;
                    break;

                case ColumnDataType.Boolean:
                    newValue = ((ComboBox)inputControl).SelectedIndex == 0;
                    break;

                case ColumnDataType.DateTime:
                    newValue = ((DateTimePicker)inputControl).Value;
                    break;

                default:
                    newValue = ((TextBox)inputControl).Text;
                    break;
            }

            // 开始批量更新
            this.SuspendLayout();
            try
            {   // 使用事务处理提高性能
                this.BeginEdit(true);
                // 更新所有行的值
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    if (!this.Rows[i].IsNewRow)
                    {
                        this.Rows[i].Cells[_currentEditingColumn.Index].Value = newValue;

                        // 如果是数据绑定的对象，也更新对象属性值
                        if (this.Rows[i].DataBoundItem != null)
                        {
                            object item = this.Rows[i].DataBoundItem;
                            Type itemType = item.GetType();
                            PropertyInfo property = itemType.GetProperty(_currentEditingColumn.DataPropertyName);

                            if (property != null && property.CanWrite)
                            {
                                // 转换值类型以匹配属性类型
                                object convertedValue = Convert.ChangeType(newValue, property.PropertyType);
                                property.SetValue(item, convertedValue, null);
                            }
                        }
                    }
                }

                this.EndEdit();
                // 标记数据已修改
                dgvEdit = true;

                MessageBox.Show($"成功更新{this.Rows.Count}行数据。", "批量编辑",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新数据时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }


        #endregion

        private void SetSelectedColumn(bool _UseSelectedColumns)
        {
            if (this.Columns["Selected"] == null)
            {
                return;
            }


            // 创建一个新的 DataGridViewCheckBoxColumn
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "Selected";
            checkBoxColumn.ValueType = typeof(Boolean);
            checkBoxColumn.DataPropertyName = "Selected";



            checkBoxColumn.HeaderText = "选择";
            checkBoxColumn.Visible = _UseSelectedColumns;

            //if (_UseSelectedColumns)
            //{
            //    var colSelected = this.Columns["Selected"];
            //    colSelected = new DataGridViewCheckBoxColumn(true);
            //    colSelected.Name = "Selected";
            //    colSelected.DataPropertyName="Selected";
            //}
            //else
            //{
            //    this.Columns["Selected"].ReadOnly = false;
            //}
            if (_UseSelectedColumns)
            {
                checkBoxColumn.ReadOnly = false;
                this.ReadOnly = false;
                this.MultiSelect = true;
            }
            else
            {
                checkBoxColumn.ReadOnly = true;
                this.MultiSelect = false;
            }

            checkBoxColumn.Width = 45;
            checkBoxColumn.DisplayIndex = 0;


            DataGridViewCellStyle dgc = new DataGridViewCellStyle();
            dgc.Alignment = DataGridViewContentAlignment.MiddleCenter;
            checkBoxColumn.DefaultCellStyle = dgc;

            //this.Columns["Selected"].Frozen = true;
            // 找到对应于 Selected 列的 DataGridViewColumn
            DataGridViewColumn selectedColumn = this.Columns["Selected"];
            // 将 CheckBoxColumn 替换为原来的列
            this.Columns.Remove(selectedColumn);
            this.Columns.Add(checkBoxColumn);
            checkBoxColumn.HeaderCell.ContextMenuStrip = CreateSelectedAllContextMenuStrip();
            //打开了多选开关才显示
            if (_UseSelectedColumns)
            {
                if (!FieldNameList.ContainsKey("Selected"))
                {
                    FieldNameList.TryAdd("Selected", new KeyValuePair<string, bool>("选择", _UseSelectedColumns));
                }
                else
                {
                    KeyValuePair<string, bool> kvselected = new KeyValuePair<string, bool>();
                    FieldNameList.TryGetValue("Selected", out kvselected);
                    kvselected = new KeyValuePair<string, bool>("选择", _UseSelectedColumns);
                }


            }

        }

        /// <summary>
        ///  
        /// </summary>
        [Browsable(true), Category("Appearance")]
        public string summaryDescription = "2020-08最新 带有合计列功能;";

        /// <summary>
        /// 
        /// </summary>

        [Browsable(true)]
        [Category("Z_ByWatson"), Description("描述！2022.")]
        public string SummaryDescription
        {
            get { return summaryDescription; }
            set { summaryDescription = value; }
        }



        ////BindingSource 加入???

        //private object _VarStoragePara = new object();

        ///// <summary>
        ///// 变量参数存储，传值时使用
        ///// </summary>
        //[Browsable(false)]
        //public object VarStoragePara
        //{
        //    get { return _VarStoragePara; }
        //    set { _VarStoragePara = value; }
        //}


        private bool _isShowSumRow = false;             //是否显示合计行
        private string _sumCellFormat = "N2";           //合计单元格格式化字符串
        private int _sumRowHeight = 30;                 //合计行高
        private DataGridView _dgvSumRow = null;         //合计行
        private VScrollBar _vScrollBar = null;          //垂直滚动条
        private HScrollBar _hScrollBar = null;          //水平滚动条
        private bool _initSourceGriding = false;        //指示是否正在进行初始grid
        private DockStyle _dock;                        //Dock
        private int _dgvSourceMaxHeight = 0;           //dgvSource最大高度
        private int _dgvSourceMaxWidth = 0;             //dgvSource最大宽度


        //public delegate void ClickEvent(string menuText, object obj);
        // public delegate void EventHandler(object sender, EventArgs e);
        //public static Dictionary<ContextMenuController, EventHandler> ClickActionList = new Dictionary<ContextMenuController, EventHandler>();


        private Panel _panel = new Panel();

        /// <summary>
        /// 自定义列的
        /// </summary>
        private CustomizeGrid customizeGrid;

        public void SetUseCustomColumnDisplay(bool _UseCustomColumnDisplay)
        {
            customizeGrid.UseCustomColumnDisplay = _UseCustomColumnDisplay;
            UseCustomColumnDisplay = _UseCustomColumnDisplay;
            AllowUserToOrderColumns = UseCustomColumnDisplay;
        }
        /// <summary>
        /// 因为暂时事件无法通过属性中的数据传输，先用名称再从这里搜索来匹配
        /// </summary>
        public List<EventHandler> ContextClickList = new List<EventHandler>();


        /// <summary>
        /// 获取当前是否处于设计器模式
        /// </summary>
        /// <remarks>
        /// 在程序初始化时获取一次比较准确，若需要时获取可能由于布局嵌套导致获取不正确，如GridControl-GridView组合。
        /// </remarks>
        /// <returns>是否为设计器模式</returns>
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) == null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }

        #region 添加分页功能
        //添加分页面板容器
        private KryptonPanel _paginationPanel = new KryptonPanel();
        private bool _enablePagination = false;

        /// <summary>
        /// 分页信息
        /// </summary>
        public class PaginationInfo
        {
            public int PageIndex { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public long TotalCount { get; set; }
            public int TotalPages => TotalCount > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
            public int StartRecord => (PageIndex - 1) * PageSize + 1;
            public int EndRecord => Math.Min(PageIndex * PageSize, (int)TotalCount);
        }

        private PaginationInfo _paginationInfo = new PaginationInfo();

        /// <summary>
        /// 启用分页功能
        /// </summary>
        [Browsable(true)]
        [Description("启用分页功能")]
        public bool EnablePagination
        {
            get => _enablePagination;
            set
            {
                _enablePagination = value;
                UpdatePaginationPanelVisibility();
                if (value)
                {
                    InitializePaginationPanel();
                }
            }
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        [Browsable(false)]
        public PaginationInfo Pagination => _paginationInfo;

        /// <summary>
        /// 页面大小选项
        /// </summary>
        private int[] _pageSizeOptions = new int[] { 10, 20, 50, 100 };

        /// <summary>
        /// 分页变更事件
        /// </summary>
        public event EventHandler<PaginationInfo> PaginationChanged;

        #endregion


        #region 添加筛选功能
        //添加筛选行容器
        private KryptonPanel _filterPanel = new KryptonPanel();

        private Dictionary<string, KryptonComboBox> _filterTypeBoxes = new Dictionary<string, KryptonComboBox>();
        private Dictionary<string, KryptonTextBox> _filterValueBoxes = new Dictionary<string, KryptonTextBox>();

        //  private Dictionary<string, FilterType> _filterTypes = new Dictionary<string, FilterType>();


        //动态创建筛选文本框
        private void CreateFilterControls()
        {
            if (!_enableFiltering) return;

            if (_filterValueBoxes.Count == this.Columns.GetColumnCount(DataGridViewElementStates.Visible))
            {
                return;
            }

            _filterPanel.Controls.Clear();
            _filterTypeBoxes.Clear();
            _filterValueBoxes.Clear();
            int currentX = this.RowHeadersWidth; // 从行头右侧开始
            int padding = 0; // 控件间间距

            foreach (ColDisplayController col in this.ColumnDisplays.OrderBy(c => c.ColDisplayIndex).ToList())
            {
                if (!col.Visible || col.ColWidth < 50) continue; // 跳过不可见或太窄的列

                // 1. 创建筛选类型下拉框
                KryptonComboBox cmbType = new KryptonComboBox
                {
                    Top = 0,
                    Width = 35,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Tag = col.ColName
                };

                cmbType.DisplayMember = "Text";
                cmbType.ValueMember = "Value";
                cmbType.DataSource = Enum.GetValues(typeof(FilterType))
                 .Cast<FilterType>()
                 .Select(ft => new { Text = GetFilterSymbol(ft), Value = ft })
                 .ToList();

                // 在每个TextBox旁添加ComboBox
                // cmbType.Items.AddRange(Enum.GetNames(typeof(FilterType)));
                //cmbType.SelectedIndex = 0;
                cmbType.SelectedIndexChanged += FilterType_Changed;

                // 2. 创建筛选值文本框
                KryptonTextBox txtValue = new KryptonTextBox
                {
                    Width = col.ColWidth - cmbType.Width - padding,
                    Tag = col.ColName,

                    //PlaceholderText = $"筛选 {col.HeaderText}",
                    Anchor = AnchorStyles.Left | AnchorStyles.Right
                };

                ToolTipValues toolTip = new ToolTipValues(null);
                toolTip.Description = $"筛选 {col.ColDisplayText}";
                toolTip.Heading = "";
                toolTip.EnableToolTips = true;
                txtValue.ToolTipValues = toolTip;
                txtValue.TextChanged += FilterValue_Changed;

                // 3. 设置位置
                cmbType.Location = new Point(currentX, 3);
                txtValue.Location = new Point(currentX + cmbType.Width + padding, 3);

                // 4. 添加到面板
                _filterPanel.Controls.Add(cmbType);
                _filterPanel.Controls.Add(txtValue);

                // 5. 保存引用
                _filterTypeBoxes.Add(col.ColName, cmbType);
                _filterValueBoxes.Add(col.ColName, txtValue);

                // 6. 移动到下一列位置
                currentX += col.ColWidth + padding;
            }
        }

        // 列位置变化时更新筛选控件位置
        private void UpdateFilterControlsPosition()
        {
            if (!_enableFiltering) return;

            int currentX = this.RowHeadersWidth;
            int padding = 0;

            foreach (DataGridViewColumn col in this.Columns)
            {
                if (!col.Visible || !_filterTypeBoxes.ContainsKey(col.Name)) continue;

                var cmbType = _filterTypeBoxes[col.Name];
                var txtValue = _filterValueBoxes[col.Name];

                cmbType.Location = new Point(currentX, 3);
                txtValue.Location = new Point(currentX + cmbType.Width + padding, 3);
                txtValue.Width = col.Width - cmbType.Width - padding;

                currentX += col.Width + padding;
            }
        }
        private void ApplyFilters()
        {
            if (this.DataSource == null) return;

            try
            {
                // 构建过滤条件
                var filterConditions = new List<string>();

                foreach (var colName in _filterValueBoxes.Keys)
                {
                    string filterValue = _filterValueBoxes[colName].Text.Trim();
                    if (string.IsNullOrEmpty(filterValue)) continue;

                    var filterTypeBox = _filterTypeBoxes[colName];
                    if (!Enum.TryParse(filterTypeBox.SelectedItem?.ToString(), out FilterType filterType))
                    {
                        filterType = FilterType.Contains;
                    }
                    // 如果是外键字段，获取显示的名称进行过滤
                    //if (colName == "DepartmentID")
                    //{
                    //    var departmentName = GetDepartmentNameByFilterValue(filterValue);
                    //    if (!string.IsNullOrEmpty(departmentName))
                    //    {
                    //        filterValue = departmentName;
                    //    }
                    //}

                    string condition = GetFilterCondition(colName, filterValue, filterType);
                    if (!string.IsNullOrEmpty(condition))
                    {
                        filterConditions.Add(condition);
                    }
                }

                // 应用过滤
                string finalFilter = string.Join(" AND ", filterConditions);

                if (this.DataSource is DataTable dt)
                {
                    dt.DefaultView.RowFilter = finalFilter;
                }
                else if (this.DataSource is DataView dv)
                {
                    dv.RowFilter = finalFilter;
                }
                else if (this.DataSource is BindingSource bs)
                {
                    if (bs.DataSource is DataTable bsDt)
                    {
                        bsDt.DefaultView.RowFilter = finalFilter;
                    }
                    else if (bs.DataSource is DataView bsDv)
                    {
                        bsDv.RowFilter = finalFilter;
                    }
                    else
                    {
                        bs.Filter = finalFilter;
                        //bs.RemoveFilter
                    }
                }
                else
                {


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"筛选出错: {ex.Message}");
            }
        }

        private string GetFilterCondition_old(string columnName, string value, FilterType filterType)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            // 防止SQL注入式攻击
            string safeValue = value.Replace("'", "''");

            switch (filterType)
            {
                case FilterType.Contains:
                    return $"[{columnName}] LIKE '%{safeValue}%'";
                case FilterType.StartsWith:
                    return $"[{columnName}] LIKE '{safeValue}%'";
                case FilterType.Equals:
                    return $"[{columnName}] = '{safeValue}'";
                case FilterType.NotEqual:
                    return $"[{columnName}] <> '{safeValue}'";
                default:
                    return $"[{columnName}] LIKE '%{safeValue}%'";
            }
        }


        private string GetFilterCondition(string columnName, string value, FilterType filterType)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            // 防止 SQL 注入式攻击
            string safeValue = value.Replace("'", "''");

            // 处理外键字段：如果列名是外键（如 DepartmentID），尝试获取对应的显示名称进行过滤
            //if (columnName == "DepartmentID")
            //{
            //    var departmentName = GetDepartmentNameByFilterValue(safeValue);
            //    if (!string.IsNullOrEmpty(departmentName))
            //    {
            //        safeValue = departmentName;
            //    }
            //}

            switch (filterType)
            {
                case FilterType.Contains:
                    return $"{columnName} LIKE '%{safeValue}%'";

                case FilterType.StartsWith:
                    return $"[{columnName}] LIKE '{safeValue}%'";

                case FilterType.Equals:
                    // 尝试解析为数字或日期
                    if (long.TryParse(safeValue, out long numericValue))
                    {
                        return $"[{columnName}] = {numericValue}";
                    }
                    else if (DateTime.TryParse(safeValue, out DateTime dateValue))
                    {
                        // 日期格式需要使用 # 包裹
                        return $"[{columnName}] = #{dateValue.ToShortDateString()}#";
                    }
                    else
                    {
                        return $"[{columnName}] = '{safeValue}'";
                    }

                case FilterType.NotEqual:
                    // 尝试解析为数字或日期
                    if (long.TryParse(safeValue, out long numericValueNot))
                    {
                        return $"[{columnName}] <> {numericValueNot}";
                    }
                    else if (DateTime.TryParse(safeValue, out DateTime dateValueNot))
                    {
                        return $"[{columnName}] <> #{dateValueNot.ToShortDateString()}#";
                    }
                    else
                    {
                        return $"[{columnName}] <> '{safeValue}'";
                    }

                default:
                    return $"[{columnName}] LIKE '%{safeValue}%'";
            }
        }

        // 筛选值变化处理（带防抖）
        //添加防抖逻辑避免频繁过滤：
        private System.Threading.Timer _filterTimer;
        // 修改 FilterType 的显示逻辑
        private string GetFilterSymbol(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return "%";
                case FilterType.StartsWith:
                    return "^";
                case FilterType.Equals:
                    return "=";
                case FilterType.NotEqual:
                    return "!=";
                default:
                    return "%";
            }
        }

        private void FilterValue_Changed(object sender, EventArgs e)
        {
            _filterTimer?.Dispose();
            _filterTimer = new System.Threading.Timer(_ =>
            {
                this.Invoke(new Action(ApplyFilters));
            }, null, 500, Timeout.Infinite);
        }

        private void FilterType_Changed(object sender, EventArgs e)
        {
            ApplyFilters(); // 筛选类型变化立即应用
        }




        private bool _enableFiltering = false;

        [Browsable(true)]
        [Category("行为")]
        [Description("是否启用顶部筛选功能")]
        public bool EnableFiltering
        {
            get { return _enableFiltering; }
            set
            {
                if (_enableFiltering != value)
                {
                    _enableFiltering = value;
                    UpdateFilterPanelVisibility();
                    if (_enableFiltering && this.DataSource != null)
                    {

                        CreateFilterControls();


                        // 示例：切换过滤面板的可见性
                        _filterPanel.Visible = _enableFiltering;

                        // 调整 DataGridView 的位置或大小，避免遮挡
                        if (_filterPanel.Visible)
                        {
                            this.Top = _filterPanel.Height;
                            this.Height = this.Height - _filterPanel.Height;
                        }
                        else
                        {
                            this.Top = 0;
                            this.Height = this.Height;
                            this.Dock = DockStyle.Fill;
                        }

                    }
                }
            }
        }
        private void UpdateFilterPanelVisibility()
        {
            _filterPanel.Visible = _enableFiltering;
            if (_enableFiltering)
            {
                // 调整数据区域位置
                this.Top = _filterPanel.Height;
                this.Height = Parent.Height - _filterPanel.Height;
            }
            else
            {
                this.Top = 0;
                this.Height = Parent.Height;
            }
        }

        #endregion



        /// <summary>
        /// 初始化
        /// </summary>
        //[Designer(typeof(MyDesigner))]
        public NewSumDataGridView()
        {

            // 启用双缓冲
            this.DoubleBuffered = true;
            //// 在构造函数中添加
            ///这个设置会在每次行数变化时触发全量行头宽度计算，当数据量超过1000行时会产生严重的性能问题
            //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.TopLeftHeaderCell.Style.BackColor = Color.LightGray;
            this.TopLeftHeaderCell.ToolTipText = "点击右键:设置多选模式菜单";

            base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            InitializeEvents();

            //所以行号不需要特别处理，调用者也不需要实现，除非有特殊要求
            //this.CellPainting += NewSumDataGridView_CellPainting;
            this.ReadOnly = false;
            InitializeDefaultStyles();
            //不占用原始的属性
            // this.Tag = "SUMDG";

            _panel.Dock = DockStyle.Fill;
            //     _panel.BackColor = Color.FromArgb(255, 192, 192);
            _panel.BorderStyle = BorderStyle.FixedSingle;
            _panel.Size = new Size(800, 500);

            //自定义列
            customizeGrid = new CustomizeGrid();
            customizeGrid.InitializeDefaultColumnCustomizeGrid += CustomizeGrid_InitializeDefaultColumnCustomizeGrid;
            customizeGrid.UseCustomColumnDisplay = UseCustomColumnDisplay;
            AllowUserToOrderColumns = UseCustomColumnDisplay;
            customizeGrid.targetDataGridView = this;


            //运行时，直接判断属性是否设置。如果没有就提示
            //或者在数据变动时提示
            //合并 设置右键菜单 只执行一次
            if (!setContextMenu)
            {
                //每个Gridview会初始化一个默认菜单
                this.ContextMenuStrip = GetContextMenu();
            }

            this.ColumnWidthChanged -= DataGridView_ColumnWidthChanged;
            this.ColumnWidthChanged += DataGridView_ColumnWidthChanged;


            this.CurrentCellChanged += dataGridView1_CurrentCellChanged;
            this.SelectionChanged += dataGridView1_SelectionChanged;

            // 智能过滤初始化
            InitializeFilterPanel();

            // 分页功能初始化
            InitializePaginationPanel();

        }

        private void CustomizeGrid_InitializeDefaultColumnCustomizeGrid(List<ColDisplayController> ColumnDisplays)
        {
            //初始化列，有两种 一种是数据库中的。一种是xml文件的
            BindColumnStyle();
        }

        private void InitializeFilterPanel()
        {
            #region  智能过滤

            // 筛选面板初始化
            _filterPanel = new KryptonPanel
            {
                Dock = DockStyle.Top,
                Height = 25,
                BackColor = Color.WhiteSmoke,
                Visible = _enableFiltering,

            };


            this.Controls.Add(_filterPanel);

            // 将过滤面板置于底层
            _filterPanel.SendToBack();


            // 订阅布局变化事件
            this.ColumnWidthChanged += (s, e) => UpdateFilterControlsPosition();
            this.ColumnDisplayIndexChanged += (s, e) => UpdateFilterControlsPosition();
            this.Scroll += (s, e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                    UpdateFilterControlsPosition();
            };

            #endregion
        }

        private void InitializeEvents()
        {
            this.ColumnWidthChanged += new DataGridViewColumnEventHandler(this_ColumnWidthChanged);
            this.DataSourceChanged += new EventHandler(this_DataSourceChanged);
            this.RowHeadersWidthChanged += new EventHandler(this_RowHeadersWidthChanged);
            this.MouseWheel += new MouseEventHandler(dgvSource_MouseWheel);
            this.CellEndEdit += NewSumDataGridView_CellEndEdit;
            this.DataError += NewSumDataGridView_DataError;
            this.CellValueChanged += NewSumDataGridView_CellValueChanged;
            this.CurrentCellChanged += NewSumDataGridView_CurrentCellChanged;
            this.CurrentCellDirtyStateChanged += NewSumDataGridView_CurrentCellDirtyStateChanged;


            this.DataBindingComplete -= NewSumDataGridView_DataBindingComplete;
            this.DataBindingComplete += NewSumDataGridView_DataBindingComplete;

            this.ColumnDisplayIndexChanged -= NewSumDataGridView_ColumnDisplayIndexChanged;
            this.ColumnDisplayIndexChanged += NewSumDataGridView_ColumnDisplayIndexChanged;


            this.MouseDown += NewSumDataGridView_MouseDown;
            this.CellClick += NewSumDataGridView_CellClick;
            this.CellStateChanged += dataGridView1_CellStateChanged;
        }

        private void InitializeDefaultStyles()
        {
            DataGridViewCellStyle c = new DataGridViewCellStyle();
            c.BackColor = Color.Yellow;
            this.AlternatingRowsDefaultCellStyle = c;
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.GridColor = Color.SkyBlue;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            this.DefaultCellStyle.SelectionBackColor = Color.MistyRose;
            this.DefaultCellStyle.SelectionForeColor = Color.Blue;
            this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Gold;
            this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }

        // 处理CurrentCellChanged事件，以更新当前单元格的背景色
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {

            if (this.CurrentCell != null)
            {
                // 设置当前单元格的选中背景色为蓝色
                this.CurrentCell.Style.SelectionBackColor = Color.LightBlue;
                //将当前单元各对应的行的其它单元格的背景色设置恢复一下
                foreach (DataGridViewCell cell in this.SelectedCells)
                {
                    if (cell != this.CurrentCell)
                    {
                        cell.Style.SelectionBackColor = Color.MistyRose; ;
                    }
                }
            }

        }

        // 处理SelectionChanged事件，以更新其他选中单元格的背景色
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //以更新当前单元格的背景色 dataGridView1_CurrentCellChanged已经实现
            return;
            if (this.SelectedRows.Count > 200)
            {
                return;
            }

            // 仅更新必要元素
            //foreach (DataGridViewRow row in this.Rows)
            //{
            //    row.Selected = row.Index == this.CurrentCell?.RowIndex;
            //}

            foreach (DataGridViewCell cell in this.SelectedCells)
            {
                if (cell != this.CurrentCell)
                {
                    // 设置其他选中单元格的背景色为默认颜色
                    //cell.Style.SelectionBackColor = this.DefaultCellStyle.BackColor;
                    cell.Style.SelectionBackColor = Color.MistyRose;

                }
            }
        }

        private void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            //保存在内存中
            #region 列有变化就保存到内存，关闭时保存到数据库设置中

            ColDisplayController columnDisplay = this.ColumnDisplays.FirstOrDefault(c => c.ColName == e.Column.Name);
            if (columnDisplay != null)
            {
                columnDisplay.ColWidth = e.Column.Width;
            }

            #endregion
        }

        private void NewSumDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void NewSumDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.MultiSelect == false)
            {
                if (e.RowIndex == -1 && e.ColumnIndex == -1)
                {
                    this.MultiSelect = true;
                    // 遍历所有行，设置选中状态
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        this.Rows[i].Selected = true;
                    }
                    //this.MultiSelect = false;
                }

            }
            else
            {
                if (e.RowIndex == -1 && e.ColumnIndex == -1)
                {
                    // 遍历所有行，设置选中状态
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        this.Rows[i].Selected = true;
                    }
                }
            }

        }


        private void NewSumDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            // 判断鼠标按下事件发生在 DataGridView 上
            if (e.Button == MouseButtons.Left)
            {
                // 将鼠标点击位置转换为相对于 DataGridView 左上角的坐标
                Point point = new Point(e.X + HorizontalScrollingOffset, e.Y);

                // 调用 HitTest 方法获取鼠标点击位置所对应的单元格信息
                var hitInfo = HitTest(point.X, point.Y);

                // 根据返回结果判断鼠标点击位置所处于的区域（单元格、表头等）
                switch (hitInfo.Type)
                {
                    case DataGridViewHitTestType.Cell:
                        // 鼠标点击位置在单元格内部

                        int rowIndex = hitInfo.RowIndex;   // 获取行索引
                        int columnIndex = hitInfo.ColumnIndex;   // 获取列索引

                        // 输出选中单元格的值
                        //System.Diagnostics.Debug.WriteLine("Selected Cell Value: " + columnIndex, rowIndex].Value);
                        break;
                    case DataGridViewHitTestType.ColumnHeader:
                        DisplayIndexChangedFlag = true;
                        break;
                    default:
                        // 其他情况，比如鼠标点击位置不在任何单元格内部
                        break;
                }
            }
        }

        //标识只有点击标题才修改顺序
        private bool DisplayIndexChangedFlag = false;

        private void NewSumDataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            /*
            var g = (DataGridView)sender;
            var property = typeof(DataGridViewColumn).GetProperty("DisplayIndexHasChanged",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (g.Columns.Cast<DataGridViewColumn>().Any(x => (bool)property.GetValue(x)))
                return;
            else
            {
                System.Diagnostics.Debug.WriteLine("Changed");
            }
          */
            //System.Diagnostics.Debug.WriteLine("{0} 的位置改变到 {1} ", e.Column.Name, e.Column.DisplayIndex);
            if (DisplayIndexChangedFlag)
            {
                //保存显示顺序
                ColumnDisplays = customizeGrid.SaveDisplayIndex(ColumnDisplays);
                DisplayIndexChangedFlag = false;
            }

        }



        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            // 关闭保存列样式，注意要先保存列样式，再调用父类的Dispose()
            if (UseCustomColumnDisplay)
            {
                //因为使用了开源的UI框架 krypton 在关闭时还会执行ColumnDisplayIndexChanged，将顺序打乱了。所以这里有一些问题
                //所以通过添加一个属性和一个方法来判断，并且拖放时立即保存到List中。保存时就不从dg中取显示顺序了
                SaveColumnStyle();
            }


            base.Dispose(disposing);
        }

        /// <summary>
        /// 保存列控制信息的列表 ，这个值设计时不生成
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();

        private bool _NeedSaveColumnsXml = true;

        /// <summary>
        /// 是否保存列自定义设置
        /// </summary>
        public bool NeedSaveColumnsXml
        {
            get { return _NeedSaveColumnsXml; }
            set
            {
                _NeedSaveColumnsXml = value;
                customizeGrid.NeedSaveColumnsXml = value;
            }

        }


        public void SaveColumnStyle()
        {
            foreach (DataGridViewColumn dc in Columns)
            {
                ColDisplayController cdc = ColumnDisplays.Where(s => s.ColName == dc.Name).FirstOrDefault();
                if (cdc != null)
                {
                    cdc.ColDisplayText = dc.HeaderText;
                    //因为使用了开源的UI框架 krypton 在关闭时还会执行ColumnDisplayIndexChanged，将顺序打乱了。所以这里有一些问题
                    //所以通过添加一个属性和一个方法来判断，并且拖放时立即保存到List中。保存时就不从dg中取显示顺序了
                    //cdc.ColDisplayIndex = dc.DisplayIndex; 这个特别处理
                    cdc.ColWidth = dc.Width;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                }
            }

            var cols = from ColDisplayController col in ColumnDisplays
                       orderby col.ColDisplayIndex
                       select col;

            customizeGrid.NeedSaveColumnsXml = NeedSaveColumnsXml;
            customizeGrid.SaveColumnsList(cols.ToList());
        }

        /// <summary>
        /// 绑定列样式
        /// </summary>
        public void BindColumnStyle(List<ColDisplayController> ColumnDisplays = null)
        {
            if (ColumnDisplays == null)
            {
                ColumnDisplays = this.ColumnDisplays;
            }

            if (this.Columns == null || this.Columns.Count == 0) return;
            this.SuspendLayout();
            try
            {
                // 处理特殊列
                var selectedColumn = Columns["Selected"];
                if (selectedColumn != null)
                {
                    selectedColumn.Visible = UseSelectedColumn;
                    selectedColumn.ReadOnly = !UseSelectedColumn;
                }
                ColumnDisplays.Where(c => c.Disable).ToList().ForEach(f => f.ColDisplayIndex = 1000);
                ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == "Selected").FirstOrDefault();
                if (cdc != null)
                {
                    cdc.Disable = !UseSelectedColumn;
                    cdc.Visible = UseSelectedColumn;
                    if (UseSelectedColumn)
                    {
                        this.ReadOnly = false;
                        Columns[cdc.ColName].ReadOnly = false;
                    }
                    else
                    {
                        Columns[cdc.ColName].ReadOnly = true;
                    }
                }
                else
                {
                    if (UseSelectedColumn)
                    {
                        ColumnDisplays.Add(new ColDisplayController()
                        {
                            ColName = "Selected",
                            ColDisplayIndex = 0,
                            ColDisplayText = "选择",
                            ColWidth = 50,
                            Disable = !UseSelectedColumn,
                            Visible = UseSelectedColumn
                        });
                    }
                }
                //加载列样式
                foreach (ColDisplayController displayController in ColumnDisplays)
                {
                    if (!Columns.Contains(displayController.ColName)) continue;

                    if (Columns.Contains(displayController.ColName))
                    {

                        #region 设置列

                        Columns[displayController.ColName].HeaderText = displayController.ColDisplayText;
                        if (displayController.ColDisplayIndex < ColumnCount)
                        {
                            Columns[displayController.ColName].DisplayIndex = displayController.ColDisplayIndex;
                        }
                        Columns[displayController.ColName].Width = displayController.ColWidth;

                        Columns[displayController.ColName].Visible = displayController.Visible && !displayController.Disable;

                        if (displayController.ColName == "Selected")
                        {
                            //为选择列时
                            Columns[displayController.ColName].HeaderText = "选择";
                            Columns[displayController.ColName].Visible = displayController.Visible;
                            if (UseSelectedColumn)
                            {
                                Columns[displayController.ColName].Visible = true;
                                Columns[displayController.ColName].DisplayIndex = 0;
                            }
                            else
                            {
                                Columns[displayController.ColName].Visible = false;
                            }
                        }

                        if (displayController.Disable)
                        {
                            Columns[displayController.ColName].Visible = false;
                        }

                        //最后处理特别情况，如果整个dg只读为假，则checkbox可以选择？
                        if (Columns[displayController.ColName].ValueType.Name.Contains("Boolean") && displayController.ColName != "Selected")
                        {
                            Columns[displayController.ColName].ReadOnly = this.ReadOnly;
                        }
                        #endregion

                    }

                }

                //如果
                if (this.Columns.Count != ColumnDisplays.Count && this.Columns.Count > 5)
                {
                    //ColumnDisplays中不存在的列，但是this.Columns中存在，则设置为隐藏
                    foreach (DataGridViewColumn col in this.Columns)
                    {
                        if (!ColumnDisplays.Any(c => c.ColName == col.Name))
                        {
                            col.Visible = false;
                        }
                    }
                }

                BindColumnStyleFordgvSum();

            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        /// <summary>
        /// 求和列
        /// </summary>
        private void BindColumnStyleFordgvSum()
        {
            if (_dgvSumRow == null)
            {
                return;
            }
            //加载列样式
            foreach (ColDisplayController displayController in ColumnDisplays)
            {
                //加总的也要控制
                if (_dgvSumRow.Columns.Contains(displayController.ColName))
                {
                    #region
                    _dgvSumRow.Columns[displayController.ColName].HeaderText = displayController.ColDisplayText;
                    if (displayController.ColDisplayIndex < _dgvSumRow.ColumnCount)
                    {
                        _dgvSumRow.Columns[displayController.ColName].DisplayIndex = displayController.ColDisplayIndex;
                    }

                    _dgvSumRow.Columns[displayController.ColName].Width = displayController.ColWidth;
                    //Columns[displayController.ColName].DataPropertyName = displayController.DataPropertyName;
                    _dgvSumRow.Columns[displayController.ColName].Visible = displayController.Visible;
                    //if (displayController.ColName != "Selected")
                    //{
                    //    _dgvSumRow.Columns[displayController.ColName].ReadOnly = true;
                    //}
                    //else
                    //{
                    //    _dgvSumRow.Columns[displayController.ColName].ReadOnly = !UseCustomColumnDisplay;
                    //}
                    if (UseSelectedColumn)
                    {
                        _dgvSumRow.Columns[displayController.ColName].Visible = displayController.Visible;
                    }
                    if (displayController.Disable)
                    {
                        _dgvSumRow.Columns[displayController.ColName].Visible = false;
                    }
                    #endregion
                }
            }
        }


        /// <summary>
        /// 查询时会执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSumDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_enableFiltering)
            {
                CreateFilterControls();
            }
            //在数据绑定的关键位置添加布局挂起和恢复
            this.SuspendLayout();
            try
            {
                //求各前。判断一下
                if (IsShowSumRow)
                {
                    if (SumColumns == null || SumColumns.Length == 0)
                    {
                        //MessageBox.Show("统计列的属性，需要在数据源之前赋值！", "控件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SumData();
                    }
                }


                if (UseCustomColumnDisplay)
                {
                    #region 如果列为空时 再初始化一次
                    if (ColumnDisplays.Count == 0)
                    {
                        this.AllowDrop = true;
                        if (NeedSaveColumnsXml)
                        {
                            ColumnDisplays = customizeGrid.LoadColumnsListByCdc();
                        }


                        #region 将没有中文字段 比方ID，或对象集合这种都不启动

                        List<string> FieldNames = FieldNameList.Select(kv => kv.Key).ToList();
                        List<string> ColNamesDispays = ColumnDisplays.Select(c => c.ColName).ToList();
                        // 将 A 中不存于 B 中的元素存储到一个新的 List<A> 中
                        List<string> result = ColNamesDispays.Except(FieldNames).ToList();
                        foreach (string str in result)
                        {
                            ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == str).FirstOrDefault();
                            if (cdc != null)
                            {
                                cdc.Disable = true;
                            }
                        }
                        #endregion

                        //这里认为只执行一次？并且要把显示名的中文传过来，并且不在默认中文及控制显示列表中，就不显示了。
                        foreach (var item in FieldNameList)
                        {
                            ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == item.Key).FirstOrDefault();
                            if (cdc != null)
                            {
                                cdc.ColDisplayText = item.Value.Key;
                                if (!item.Value.Value)
                                {
                                    cdc.Visible = item.Value.Value;//如果默认不显示，则不参加控制
                                    cdc.Disable = true;
                                }
                                //特别处理选择列
                                if (cdc.ColName == "Selected")
                                {
                                    cdc.Visible = UseSelectedColumn;
                                    cdc.Disable = !UseSelectedColumn;
                                }
                            }
                            else
                            {

                            }
                        }
                    }

                    #endregion
                    // 加载列样式 这里会多次执行，就算关闭时也会执行，所以这里只是显示绑定到UI的样式，不能加载
                    BindColumnStyle();
                }

                //隐藏 FieldNameList
                if (Columns.Contains("FieldNameList"))
                {
                    Columns["FieldNameList"].Visible = false;
                }

                //隐藏 FieldNameList
                if (Columns.Contains("Selected"))
                {
                    if (Columns["Selected"].HeaderText == "Selected")
                    {
                        Columns["Selected"].Visible = false;
                    }
                }
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        private string xmlFileName = string.Empty;

        [Browsable(false)]
        public string XmlFileName
        {
            get
            {
                return xmlFileName;
            }
            set
            {
                xmlFileName = value;
                customizeGrid.XmlFileName = XmlFileName;
            }
        }


        private ContextMenuStrip _ContextMenuStrip;

        /// <summary>
        /// 重写右键菜单 为了合并
        /// 如果使用内置的菜单则这里设置时就要合并处理
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get { return _ContextMenuStrip; }
            set
            {
                //if (_ContextMenuStrip==null)
                //{
                _ContextMenuStrip = value;

                //}
                //else
                //{
                //    _ContextMenuStrip.Items.AddRange(value.Items);
                //}

            }
        }

        /// <summary>
        /// 显示菜单
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="menu"></param>
        /// <param name="e"></param>
        private void ShowMenu(DataGridView grid, ContextMenuStrip menu, DataGridViewCellMouseEventArgs e)
        {
            Point point = grid.PointToScreen(new Point(0, 0));
            int x = 0, y = 0;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Index >= e.ColumnIndex)
                    break;
                if (column.Visible)
                    x += column.Width;
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Index > e.RowIndex)
                    break;
                if (row.Visible)
                    y += row.Height;
            }
            menu.Show(grid.PointToScreen(new Point(x + e.X, y + e.Y)));
        }


        //public void SetColToCheckBox(int colIndex)
        //{
        //    #region  实现列头checkbox可选
        //    //自定义组件实现
        //    DatagridviewCheckboxHeaderCell ch = new DatagridviewCheckboxHeaderCell();
        //    ch.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    var checkboxCol = this.Columns[colIndex] as DataGridViewCheckBoxColumn;
        //    checkboxCol.HeaderCell = ch;
        //    checkboxCol.HeaderCell.Value = string.Empty;
        //    //if (!this.DesignMode)
        //    //{
        //    //    DatagridviewCheckboxHeaderCell cbHeader = new DatagridviewCheckboxHeaderCell();
        //    //    cbHeader.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    //    DataGridViewCheckBoxColumn checkboxCol = new DataGridViewCheckBoxColumn();
        //    //    checkboxCol.HeaderCell = cbHeader;
        //    //    checkboxCol.HeaderCell.Value = string.Empty;
        //    //    this.Columns.Insert(0, checkboxCol);
        //    //}
        //    #endregion
        //}

        //public void SetColToCheckBox(string colName)
        //{
        //    #region  实现列头checkbox可选
        //    //自定义组件实现
        //    DatagridviewCheckboxHeaderCell ch = new DatagridviewCheckboxHeaderCell();
        //    ch.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    var checkboxCol = this.Columns[colName] as DataGridViewCheckBoxColumn;
        //    checkboxCol.HeaderCell = ch;
        //    checkboxCol.HeaderCell.Value = string.Empty;
        //    //if (!this.DesignMode)
        //    //{
        //    //    DatagridviewCheckboxHeaderCell cbHeader = new DatagridviewCheckboxHeaderCell();
        //    //    cbHeader.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    //    DataGridViewCheckBoxColumn checkboxCol = new DataGridViewCheckBoxColumn();
        //    //    checkboxCol.HeaderCell = cbHeader;
        //    //    checkboxCol.HeaderCell.Value = string.Empty;
        //    //    this.Columns.Insert(0, checkboxCol);
        //    //}
        //    #endregion
        //}

        //public void SetColToCheckBoxOld<T>(Expression<Func<T, object>> exp)
        //{
        //    MemberInfo minfo = exp.GetMemberInfo();
        //    string colName = minfo.Name;
        //    #region  实现列头checkbox可选
        //    //自定义组件实现
        //    DatagridviewCheckboxHeaderCell ch = new DatagridviewCheckboxHeaderCell();
        //    ch.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    var checkboxCol = this.Columns[colName] as DataGridViewCheckBoxColumn;
        //    checkboxCol.HeaderCell = ch;
        //    string colText = string.Empty;
        //    this.FieldNameList.GetOrAdd(colName, colText);
        //    checkboxCol.HeaderCell.Value = colText;
        //    //if (!this.DesignMode)
        //    //{
        //    //    DatagridviewCheckboxHeaderCell cbHeader = new DatagridviewCheckboxHeaderCell();
        //    //    cbHeader.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
        //    //    DataGridViewCheckBoxColumn checkboxCol = new DataGridViewCheckBoxColumn();
        //    //    checkboxCol.HeaderCell = cbHeader;
        //    //    checkboxCol.HeaderCell.Value = string.Empty;
        //    //    this.Columns.Insert(0, checkboxCol);
        //    //}
        //    #endregion
        //}

        /// <summary>
        /// 只有继承于原生 DG才生效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        public void SetColToCheckBox<T>(Expression<Func<T, object>> exp)
        {
            MemberInfo minfo = exp.GetMemberInfo();
            string colName = minfo.Name;
            #region  实现列头checkbox可选
            //自定义组件实现
            DatagridViewCheckBoxHeaderCell ch = new DatagridViewCheckBoxHeaderCell();
            //ch.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
            ch.OnCheckBoxClicked += Ch_OnCheckBoxClicked;
            var checkboxCol = this.Columns[colName] as DataGridViewCheckBoxColumn;
            checkboxCol.HeaderCell = ch;
            KeyValuePair<string, bool> kvText;
            this.FieldNameList.TryGetValue(colName, out kvText);
            checkboxCol.HeaderCell.Value = kvText.Key;

            //if (!this.DesignMode)
            //{
            //    DatagridviewCheckboxHeaderCell cbHeader = new DatagridviewCheckboxHeaderCell();
            //    cbHeader.OnCheckBoxClicked += new DatagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);
            //    DataGridViewCheckBoxColumn checkboxCol = new DataGridViewCheckBoxColumn();
            //    checkboxCol.HeaderCell = cbHeader;
            //    checkboxCol.HeaderCell.Value = string.Empty;
            //    this.Columns.Insert(0, checkboxCol);
            //}
            #endregion
        }

        /// <summary>
        /// 显示列头选框 只有继承于原生 DG才生效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        public void SetColToCheckBoxNew<T>(Expression<Func<T, object>> exp)
        {
            if (!this.DesignMode)
            {
                MemberInfo minfo = exp.GetMemberInfo();
                string colName = minfo.Name;
                #region  实现列头checkbox可选
                //自定义组件实现
                DataGridViewCheckBoxColumnHeeaderCell ch = new DataGridViewCheckBoxColumnHeeaderCell();
                var checkboxCol = this.Columns[colName] as DataGridViewCheckBoxColumn;
                checkboxCol.HeaderCell = ch;
                KeyValuePair<string, bool> kvText;
                this.FieldNameList.TryGetValue(colName, out kvText);
                checkboxCol.HeaderCell.Value = kvText.Key;

            }
            #endregion
        }

        private void Ch_OnCheckBoxClicked(int columnIndex, bool state)
        {
            this.RefreshEdit();

            foreach (DataGridViewRow row in this.Rows)
            {
                if (!row.Cells[columnIndex].ReadOnly)
                {
                    row.Cells[columnIndex].Value = state;
                }
            }
            this.RefreshEdit();
        }


        private bool setContextMenu = false;


        /*
        /// <summary>
        /// 设置右键菜单，但是对不对参数进行设置。因为是引用的，会改变值
        /// </summary>
        /// <param name="_contextMenuStrip"></param>
        public ContextMenuStrip GetContextMenu(ContextMenuStrip _contextMenuStrip = null,
            List<EventHandler> AddContextClickList = null,
            List<ContextMenuController> AddContextMenuController = null, bool IsInsertTop = true)
        {
            // 创建一个全新的右键菜单副本
            ContextMenuStrip newContextMenuStrip = new ContextMenuStrip();

            //初始化右键菜单
            // 初始化内置右键菜单
            ContextMenuStrip internalMenu = new ContextMenuStrip();
            internalMenu.BackColor = Color.FromArgb(192, 255, 255);

            //如果需要通过设计时对属性值修改。
            //则不能在这个构造函数中操作。因为这时属性值不能获取
            internalMenu.Items.Clear();

            // 合并传入的菜单和内置菜单
            if (_contextMenuStrip != null)
            {
                //外面菜单有设置则加一个分隔符
                if (_contextMenuStrip.Items.Count > 0)
                {
                    ToolStripSeparator MyTss = new ToolStripSeparator();
                    _contextMenuStrip.Items.Add(MyTss);
                }

                ToolStripItem[] ts = new ToolStripItem[_contextMenuStrip.Items.Count];
                _contextMenuStrip.Items.CopyTo(ts, 0);

                internalMenu.Items.AddRange(ts);

            }


            //或者也可以指定内置哪些生效 合并外面的？
            if (Use是否使用内置右键功能)
            {
                #region 生成内置的右键菜单
                if (ContextClickList == null)
                {
                    ContextClickList = new List<EventHandler>();
                }
                ContextClickList.Clear();
                //外部定义的优先
                if (AddContextClickList != null && IsInsertTop == true)
                {
                    ContextClickList.AddRange(AddContextClickList.ToArray());
                }
                ContextClickList.Add(NewSumDataGridView_批量修改列值);
                ContextClickList.Add(NewSumDataGridView_复制单元数据);
                ContextClickList.Add(NewSumDataGridView_导出excel);
                ContextClickList.Add(NewSumDataGridView_保存数据到DB);
                ContextClickList.Add(NewSumDataGridView_自定义列);
                ContextClickList.Add(NewSumDataGridView_SelectedAll);
                //外部定义的优先
                if (AddContextClickList != null && IsInsertTop == false)
                {
                    ContextClickList.AddRange(AddContextClickList.ToArray());
                }
                // ContextClickList.Add(NewSumDataGridView_Test);
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                _ContextMenucCnfigurator.Clear();
                //只是初始化不重复添加
                if (AddContextMenuController != null && IsInsertTop == true)
                {
                    _ContextMenucCnfigurator.AddRange(AddContextMenuController.ToArray());
                    if (Use是否使用内置右键功能)
                    {
                        _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                    }
                }

                //if (_ContextMenucCnfigurator.Count == 0 && GetIsDesignMode())
                if (GetIsDesignMode())
                {
                    // _ContextMenucCnfigurator.Add(new ContextMenuController("【删除选中行】", true, false, "删除选中行"));
                    // _ContextMenucCnfigurator.Add(new ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【导出为Excel】", true, false, "NewSumDataGridView_导出excel"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                    //  _ContextMenucCnfigurator.Add(new ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【全选】", true, false, "NewSumDataGridView_SelectedAll"));
                    // _ContextMenucCnfigurator.Add(new ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
                }
                if (AddContextMenuController != null && IsInsertTop == false)
                {
                    if (Use是否使用内置右键功能)
                    {
                        _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                    }
                    _ContextMenucCnfigurator.AddRange(AddContextMenuController.ToArray());
                }

                #endregion

                //不能清掉。 有两种情况会自定义添加右键菜单。一种是通过控件这种不会通过权限控制
                //另一种是 代码生成代码功能 会通过权限控制
                //internalMenu.Items.Clear();

                foreach (var item in _ContextMenucCnfigurator)
                {
                    if (!item.IsShow)
                    {
                        continue;
                    }

                    if (item.IsSeparator)
                    {
                        if (internalMenu.Items.Count > 0)
                        {
                            //如果他的上级是分隔线 就不再添加
                            if (internalMenu.Items[internalMenu.Items.Count - 1].GetType() == typeof(ToolStripSeparator))
                            {
                                continue;
                            }
                        }
                        ToolStripSeparator ts1 = new ToolStripSeparator();
                        internalMenu.Items.Add(ts1);
                    }
                    else
                    {
                        EventHandler ehh = ContextClickList.Find(
                            delegate (EventHandler eh)
                            {
                                return eh.Method.Name == item.ClickEventName;
                            });
                        //如果较多的外部事件也可以做一个集合
                        if (ehh == null && item.ClickEventName == "删除选中行")
                        {
                            ehh = 删除选中行;
                        }
                        //排除重复的
                        if (!internalMenu.Items.ContainsKey(item.MenuText))
                        {
                            ToolStripItem toolStripItem = new ToolStripMenuItem(item.MenuText, null, ehh);
                            toolStripItem.Name = item.MenuText;
                            internalMenu.Items.Insert(0, toolStripItem);
                        }
                    }
                }
            }
            else
            {
                //删除选中行 如果用户 在控件端实现的这个事件，则单独加在这里
                if (删除选中行 != null)
                {
                    ContextMenuController cmc = new ContextMenuController("【删除选中行】", true, false, "删除选中行");
                    internalMenu.Items.Add(cmc.MenuText, null, 删除选中行);
                }
            }


            //如果最后是分隔线 则移除
            if (internalMenu.Items.Count > 0)
            {
                //如果他的上级是分隔线 就不再添加
                if (internalMenu.Items[internalMenu.Items.Count - 1].GetType() == typeof(ToolStripSeparator))
                {
                    internalMenu.Items.RemoveAt(internalMenu.Items.Count - 1);
                }
            }


            newContextMenuStrip = internalMenu;
            // 设置最终的右键菜单
            ContextMenuStrip = newContextMenuStrip;
            return newContextMenuStrip;

        }

        */



        #region 右键菜单控制器 合并等

        //顺序还有问题。先不管
        public ContextMenuStrip GetContextMenu(
            ContextMenuStrip _contextMenuStrip = null,
            List<EventHandler> AddContextClickList = null,
            List<ContextMenuController> AddContextMenuController = null,
            bool IsInsertTop = true)
        {
            // 创建新的上下文菜单
            ContextMenuStrip newContextMenuStrip = new ContextMenuStrip();

            // 添加条件：不在行头区域时才显示常规菜单项
            //if (_headerMenuShown)
            //{
            //    return new ContextMenuStrip();
            //}

            newContextMenuStrip.BackColor = Color.FromArgb(192, 255, 255);

            // 合并传入的菜单项
            if (_contextMenuStrip != null && _contextMenuStrip.Items.Count > 0)
            {
                MergeExternalMenuItems(newContextMenuStrip, _contextMenuStrip);
            }

            // 处理内置菜单项
            if (Use是否使用内置右键功能)
            {
                InitializeContextClickList(AddContextClickList, IsInsertTop);
                InitializeMenuConfigurator(AddContextMenuController, IsInsertTop);
                BuildMenuItems(newContextMenuStrip);
            }
            else
            {
                AddDeleteMenuItemIfNeeded(newContextMenuStrip);
            }

            // 清理末尾的分隔符
            CleanupTrailingSeparator(newContextMenuStrip);

            // 设置最终的右键菜单
            this.ContextMenuStrip = newContextMenuStrip;
            return newContextMenuStrip;
        }

        private void MergeExternalMenuItems(ContextMenuStrip target, ContextMenuStrip source)
        {
            // 深拷贝菜单项
            var items = new ToolStripItem[source.Items.Count];
            source.Items.CopyTo(items, 0);

            // 添加分隔符（如果最后一个项不是分隔符）
            if (items.Length > 0 && !(items.Last() is ToolStripSeparator))
            {
                target.Items.Add(new ToolStripSeparator());
            }

            target.Items.AddRange(items);
        }

        private void InitializeContextClickList(List<EventHandler> addList, bool insertTop)
        {
            ContextClickList.Clear();

            if (addList != null && insertTop)
            {
                ContextClickList.AddRange(addList);
            }

            // 添加内置事件处理程序
            ContextClickList.Add(NewSumDataGridView_批量修改列值);
            ContextClickList.Add(NewSumDataGridView_复制单元数据);
            ContextClickList.Add(NewSumDataGridView_导出excel);
            ContextClickList.Add(NewSumDataGridView_保存数据到DB);
            ContextClickList.Add(NewSumDataGridView_自定义列);
            ContextClickList.Add(NewSumDataGridView_SelectedAll);

            if (addList != null && !insertTop)
            {
                ContextClickList.AddRange(addList);
            }
        }

        private void InitializeMenuConfigurator(List<ContextMenuController> addControllers, bool insertTop)
        {
            _ContextMenucCnfigurator.Clear();

            if (addControllers != null && insertTop)
            {
                _ContextMenucCnfigurator.AddRange(addControllers);
                if (Use是否使用内置右键功能)
                {
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                }
            }

            // 添加默认配置
            if (GetIsDesignMode())
            {
                AddDefaultMenuConfigurations();
            }

            if (addControllers != null && !insertTop)
            {
                if (Use是否使用内置右键功能)
                {
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                }
                _ContextMenucCnfigurator.AddRange(addControllers);
            }
        }

        private void BuildMenuItems(ContextMenuStrip menu)
        {
            foreach (var config in _ContextMenucCnfigurator)
            {
                if (!config.IsShow) continue;

                if (config.IsSeparator)
                {
                    AddSeparatorIfNeeded(menu);
                }
                else
                {
                    AddMenuItem(menu, config);
                }
            }
        }

        private void AddMenuItem(ContextMenuStrip menu, ContextMenuController config)
        {
            if (menu.Items.ContainsKey(config.MenuText)) return;

            var item = new ToolStripMenuItem(config.MenuText)
            {
                Name = config.MenuText,
                Tag = config.ClickEventName
            };

            // 绑定事件处理程序
            if (!string.IsNullOrEmpty(config.ClickEventName))
            {
                var handler = FindEventHandler(config.ClickEventName);
                if (handler != null)
                {
                    item.Click += handler;
                }
            }

            menu.Items.Add(item);
        }

        private EventHandler FindEventHandler(string eventName)
        {
            return ContextClickList.FirstOrDefault(h =>
                h.Method.Name.Equals(eventName, StringComparison.Ordinal)) ??
                (eventName == "删除选中行" ? 删除选中行 : null);
        }

        private void AddSeparatorIfNeeded(ContextMenuStrip menu)
        {
            if (menu.Items.Count == 0) return;
            if (menu.Items[menu.Items.Count - 1] is ToolStripSeparator) return;

            menu.Items.Add(new ToolStripSeparator());
        }

        private void CleanupTrailingSeparator(ContextMenuStrip menu)
        {
            while (menu.Items.Count > 0 && menu.Items[menu.Items.Count - 1] is ToolStripSeparator)
            {
                menu.Items.RemoveAt(menu.Items.Count - 1);
            }
        }

        private void AddDefaultMenuConfigurations()
        {
            _ContextMenucCnfigurator.AddRange(new[]
            {
        new ContextMenuController("【复制单元格数据】", true, false, nameof(NewSumDataGridView_复制单元数据)),
        new ContextMenuController("【导出为Excel】", true, false, nameof(NewSumDataGridView_导出excel)),
        new ContextMenuController("【line】", true, true, ""),
        new ContextMenuController("【自定义显示列】", true, false, nameof(NewSumDataGridView_自定义列)),
        new ContextMenuController("【line】", true, true, ""),
        new ContextMenuController("【全选】", true, false, nameof(NewSumDataGridView_SelectedAll))
    });
        }

        private void AddDeleteMenuItemIfNeeded(ContextMenuStrip menu)
        {
            if (删除选中行 != null)
            {
                var item = new ToolStripMenuItem("【删除选中行】", null, 删除选中行);
                menu.Items.Add(item);
            }
        }

        #endregion



        private List<ContextMenuController> _ContextMenucCnfigurator = new List<ContextMenuController>();


        // [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        //[TypeConverter(typeof(System.ComponentModel.CollectionConverter))]//指定编辑器特性
        [TypeConverter(typeof(RUINORERP.UI.UControls.MenuControllerConverter))]
        //代码生成器产生对象内容的代码，而不是对象本身的代码。
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), MergableProperty(false)]//设定序列化特性
        [Category("行为"), Description("右键菜单控制器，控制右键菜单的显示等")]
        public List<ContextMenuController> ContextMenucCnfigurator
        {
            get
            {
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                return _ContextMenucCnfigurator;
            }
            set
            {
                _ContextMenucCnfigurator = value;
            }
        }



        private void NewSumDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

        }

        private void NewSumDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            //
        }

        private void NewSumDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                //认为当前行被修改为
                dgvEdit = true;
                // 如果这个行标记冲突。以后也要完善
                this.Rows[e.RowIndex].Tag = true;
            }

        }




        //自定义列
        private void NewSumDataGridView_自定义列(object sender, EventArgs e)
        {
            if (customizeGrid.ColumnDisplays.Count == 0)
            {
                customizeGrid.ColumnDisplays = ColumnDisplays;
            }
            customizeGrid.SetColumns();
        }

        private void NewSumDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //认为当前行被修改为
            dgvEdit = true;
            ///如果这个行标记冲突。以后也要完善
            this.Rows[e.RowIndex].Tag = true;
        }



        private bool dgvEdit = false;

        //保存数据到DB
        private void NewSumDataGridView_保存数据到DB(object sender, EventArgs e)
        {
            if (!dgvEdit)
            {
                MessageBox.Show("没有需要保存的数据。");
                return;
            }
            if (MessageBox.Show(this, "开始将数据保存到系统中\r\n 你确定要执行吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                int rs = UpdateDatafromDGToDB();
                if (rs > 0)
                {
                    dgvEdit = false;
                }
                MessageBox.Show(string.Format("成功保存数据 {0} 条数据。", rs));
            }
        }



        /// <summary>
        /// 返回默认列的数据格式
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="isAllCol"></param>
        /// <returns></returns>
        public delegate string DataCallBackEventHandler(string colName, bool isAllCol);
        public string DataCallBackEvent(string colName, bool isAllCol)
        {

            foreach (DataGridViewRow dr in this.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    dc.Selected = false;
                }

            }

            foreach (DataGridViewRow dr in this.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (dc.OwningColumn.Name == colName)
                    {
                        dc.Selected = true;
                        break;
                    }
                    else
                    {
                        dc.Selected = false;
                    }
                }
                if (!isAllCol)
                {
                    break;
                }

            }

            return GetDefaultDataType(colName);
        }


        /// <summary>
        /// 通过列来确定一下这个列的值的类型
        /// </summary>
        /// <param name="selectColName"></param>
        /// <returns></returns>
        private string GetDefaultDataType(string selectColName)
        {
            if (selectColName == "请选择" || selectColName == "")
            {
                return "文本";
            }
            string defaultTypeText = string.Empty;

            //设置个默认列，就是选择的单元格第一个 认为是这个列。
            //frm.ResultValue
            foreach (DataGridViewCell dcell in this.SelectedCells)
            {
                //根据这个列的属性，数据源来确定？
                if (this.DataSource is DataTable)
                {

                }

                if (dcell.OwningColumn.IsDataBound)
                {
                    object objrow = this.Rows[0].DataBoundItem;
                    //取对象列属性的值类型来判断
                    Type t = objrow.GetType();
                    PropertyInfo property = t.GetProperty(selectColName);
                    /*
                    if (property.PropertyType.Name == "Boolean")
                    {
                        defaultTypeText = frmBatchSetValues.BatchSetValueType.复选框.ToString();
                    }
                    if (property.PropertyType.Name == "String")
                    {
                        defaultTypeText = frmBatchSetValues.BatchSetValueType.文本.ToString();
                    }
                    if (property.PropertyType.Name == "Decimal" || property.PropertyType.FullName.Contains("Int32") || property.PropertyType.Name == "Decimal")
                    {
                        defaultTypeText = frmBatchSetValues.BatchSetValueType.数值.ToString();
                    }
                    */
                }

            }
            return defaultTypeText;
        }




        /// <summary>
        /// 框架的数据源时，才更新 没有逻辑性 KEY就是ID 主键
        /// </summary>
        /// <returns></returns>
        private int UpdateDatafromDGToDB()
        {
            //使用事务处理，加快速度
            List<KeyValuePair<string, List<IDataParameter>>> sqlList = new List<KeyValuePair<string, List<IDataParameter>>>();

            int counter = 0;

            #region 处理数据

            //使用事务处理，加快速度
            ///创建实例
            Type t = null;
            object si = null;

            //循环对象行，给值 ，更新
            foreach (DataGridViewRow dr in Rows)
            {

                if (dr.Tag != null && dr.DataBoundItem != null && dr.Cells[0].OwningColumn.IsDataBound)
                {

                    #region 处理导入的数据
                    try
                    {
                        t = dr.DataBoundItem.GetType();
                        //必须是更新
                        si = Activator.CreateInstance(t);
                        si = dr.DataBoundItem;
                        if (si == null)
                        {
                            //实际不应该到这步
                            MessageBox.Show("要更新的对象数据不能为空。");
                            continue;
                        }

                        ///检测这个列是否存在。
                        //获取属性信息,并判断是否存在
                        PropertyInfo property导入时间 = t.GetProperty("导入时间");
                        if (property导入时间 != null)
                        {
                            ReflectionHelper.SetPropertyValue(si, "导入时间", System.DateTime.Now);
                        }


                        if (int.Parse(ReflectionHelper.GetPropertyValue(si, "ID").ToString()) > 0)
                        {
                            KeyValuePair<string, List<IDataParameter>> updatesqlList = new KeyValuePair<string, List<IDataParameter>>();
                            MethodInfo GetUpdateTranSqlByParameter = t.GetMethod("GetUpdateTranSqlByParameter");//加载方法

                            object updatesqlobj = GetUpdateTranSqlByParameter.Invoke(si, null);//执行
                            updatesqlList = (KeyValuePair<string, List<IDataParameter>>)updatesqlobj;

                            sqlList.Add(updatesqlList);
                        }

                        counter++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        break;
                    }
                    #endregion
                }
            }
            #endregion
            if (counter > 0)
            {
                MethodInfo ExecuteTransactionByParameter = t.GetMethod("ExecuteTransactionByParameter", new Type[] { typeof(List<KeyValuePair<string, List<IDataParameter>>>) });//加载方法
                Object[] LastParas = new Object[] { sqlList };
                ExecuteTransactionByParameter.Invoke(si, LastParas);//执行
            }

            return counter;

        }

        /// <summary>
        /// 批量修改列值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSumDataGridView_批量修改列值(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> cols = new List<KeyValuePair<string, string>>();

            foreach (DataGridViewColumn dc in this.Columns)
            {
                cols.Add(new KeyValuePair<string, string>(dc.Name, dc.DataPropertyName));
            }
            if (this.SelectedCells.Count == 0)
            {

            }
            /*
            frmBatchSetValues frm = new frmBatchSetValues(DataCallBackEvent);
            //设置个默认列，就是选择的单元格第一个 认为是这个列。
            //frm.ResultValue
            foreach (DataGridViewCell dcell in this.SelectedCells)
            {
                //frm.SelectColumnNameIndex = dcell.ColumnIndex;
                frm.SelectColumnName = this.Columns[dcell.ColumnIndex].Name;
            }
            frm.DefaultValueType = (BatchSetValueType)Enum.Parse(typeof(BatchSetValueType), GetDefaultDataType(frm.SelectColumnName));

            frm.ColumnsName = cols;
            if (frm.ShowDialog() == DialogResult.OK)
            {

                if (frm.ModifyAllInTheCol)
                {
                    foreach (DataGridViewRow dr in this.Rows)
                    {
                        //当前选择的指定列才更新
                        dr.Cells[frm.SelectColumnName].Value = frm.ResultValue.ToString();
                        dgvEdit = true;
                        dr.Tag = true;
                    }
                }
                else
                {
                    foreach (DataGridViewCell dcell in this.SelectedCells)
                    {
                        try
                        {
                            //当前选择的指定列才更新
                            dcell.Value = frm.ResultValue.ToString();
                        }
                        catch (Exception rex)
                        {
                            MessageBox.Show("请先正确选择要修改的所在列的单元格" + rex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }


                this.EndEdit();
            }
            */
        }


        #region DataGridView复制粘贴删除功能
        //可在dgv中复制、剪切、粘贴、删除数据

        /// <summary>
        /// DataGridView复制
        /// </summary>
        /// <param name="dgv">DataGridView实例</param>
        public static void dgvCopy(DataGridView dgv)
        {
            if (dgv.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    var dataobj = dgv.GetClipboardContent();
                    if (dataobj != null)
                    {
                        Clipboard.SetDataObject(dataobj);
                    }

                }
                catch (Exception MyEx)
                {
                    MessageBox.Show(MyEx.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// DataGridView剪切
        /// </summary>
        /// <param name="dgv">DataGridView实例</param>
        public static void dgvCut(DataGridView dgv)
        {
            dgvCopy(dgv);
            try
            {
                dgvDel(dgv);
            }
            catch (Exception MyEx)
            {
                MessageBox.Show(MyEx.Message);
            }

        }

        ///<summary>
        /// DataGridView删除内容
        /// </summary>
        /// <param name="dgv">DataGridView实例</param>
        public static void dgvDel(DataGridView dgv)
        {
            try
            {
                int k = dgv.SelectedCells.Count;
                for (int i = 0; i < k; i++)
                {
                    dgv.SelectedCells[i].Value = "";
                }
            }
            catch (Exception MyEx)
            {
                MessageBox.Show(MyEx.Message);
            }
        }

        /// <summary>
        /// DataGridView粘贴
        /// </summary>
        /// <param name="dt">DataGridView数据源</param>
        /// <param name="dgv">DataGridView实例</param>
        public static void dgvPaste(DataGridView dgv)
        {
            try
            {
                //最后一行为新行
                int rowCount = dgv.Rows.Count - 1;
                int colCount = dgv.ColumnCount;
                //获取剪贴板内容
                string pasteText = Clipboard.GetText();
                //判断是否有字符存在
                if (string.IsNullOrEmpty(pasteText))
                    return;
                //以换行符分割的数组
                string[] lines = pasteText.Trim().Split('\n');
                int txtLength = lines.Length;

                //Lance.2015-12-03
                int cRowIndex = dgv.SelectedCells[0].RowIndex;
                int cColIndex = dgv.SelectedCells[0].ColumnIndex;
                for (int i = 0; i < txtLength; i++)
                {
                    string[] words = lines[i].Split('\t');
                    for (int j = 0; j < words.Length; j++)
                    {
                        dgv.Rows[cRowIndex + i].Cells[cColIndex + j].Value = words[j];
                    }
                }

            }
            catch (Exception MyEx)
            {
                MessageBox.Show(MyEx.Message);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys key)
        {
            if (key == (Keys.Control | Keys.Delete))
            {
                //Delete删除内容  //加个Ctrol为了区别直接delete
                dgvDel(this);
            }

            if (key == (Keys.Control | Keys.C))
            {
                //按下Ctrl+C复制
                dgvCopy(this);
            }

            if (key == (Keys.Control | Keys.V))
            {
                //按下Ctrl+V粘贴
                dgvPaste(this);
            }

            if (key == (Keys.Control | Keys.X))
            {
                dgvCut(this);
            }

            return base.ProcessCmdKey(ref msg, key);
        }
        #endregion



        private void NewSumDataGridView_SelectedAll(object sender, EventArgs e)
        {
            this.MultiSelect = true;

            this.SelectAll();
        }



        //【导出excel】
        private void NewSumDataGridView_导出excel(object sender, EventArgs e)
        {

            try
            {
                UIExcelHelper.ExportExcel(this);
                return;
                string savePath = string.Empty;
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Execl files (*.xls,xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    savePath = sf.FileName;

                    if (this.DataSource is DataTable)
                    {
                        DataTable dt = new DataTable();
                        //dt = this.DataSource as DataTable;
                        //HLH.Lib.Office.Excel.NopiExcelOpretaUtil.TableToExcel(dt, savePath);
                    }
                    else
                    {
                        //DataTable dt = HLH.Lib.List.IListDataSet.ObjectToTable(this.DataSource);
                        // HLH.Lib.Office.Excel.NopiExcelOpretaUtil.TableToExcel(dt, savePath);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("导出数据出错:" + ex.Message);
            }
        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (e.Cell == this.CurrentCell)
                {
                    e.Cell.Style.SelectionBackColor = Color.LightBlue;
                }
                else
                {
                    e.Cell.Style.SelectionBackColor = this.DefaultCellStyle.SelectionBackColor;
                }
            }
        }

        private void NewSumDataGridView_复制单元数据(object sender, EventArgs e)
        {
            // MessageBox.Show("复制单元数据");
            if (this.CurrentCell != null)
            {
                Clipboard.SetDataObject(this.CurrentCell.FormattedValue.ToString());
            }
            else
            {

            }
        }

        /// <summary>
        /// 初始化合计行
        /// </summary>
        private void InitSumRowDgv()
        {
            _dgvSumRow = new DataGridView();
            _dgvSumRow.Tag = "SUMDG";

            _dgvSumRow.BackgroundColor = this.BackgroundColor;
            // _dgvSumRow.BackgroundColor = Color.FromArgb(255, 192, 192);
            _dgvSumRow.ColumnHeadersVisible = false;
            _dgvSumRow.AllowUserToResizeColumns = false;
            _dgvSumRow.AllowUserToResizeRows = false;
            _dgvSumRow.ScrollBars = System.Windows.Forms.ScrollBars.None;
            _dgvSumRow.Visible = false;
            _dgvSumRow.Height = _sumRowHeight;
            _dgvSumRow.RowTemplate.Height = _sumRowHeight;
            _dgvSumRow.AllowUserToAddRows = false;
            //_dgvSumRow.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            _dgvSumRow.ReadOnly = true;
            _dgvSumRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _dgvSumRow.DefaultCellStyle.SelectionBackColor = _dgvSumRow.DefaultCellStyle.BackColor;
            _dgvSumRow.DefaultCellStyle.SelectionForeColor = _dgvSumRow.DefaultCellStyle.ForeColor;
            _dgvSumRow.Font = new Font("宋体", 10, FontStyle.Bold);
            _dgvSumRow.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgvSumRow_RowPostPaint);
            _dgvSumRow.DataError += _dgvSumRow_DataError;
            //_dgvSumRow.BringToFront();
            //this.SendToBack();
            //btn.BringToFront();//将控件放置所有控件最前端  
            //btn.SendToBack();//将控件放置所有控件最底端 


            // _dgvSumRow.BorderStyle = BorderStyle.None;
        }

        private void _dgvSumRow_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        /// <summary>
        /// 初始化合计dgv及滚动条
        /// </summary>
        private void InitSumDgvAndScrolBar()
        {

            if (this.Parent == null)
            {
                return;
            }
            // 调整Top位置计算
            int topOffset = _filterPanel.Height + (_isShowSumRow ? _sumRowHeight : 0);
            this.Top = topOffset;



            //滚动条
            _vScrollBar = new VScrollBar();
            _hScrollBar = new HScrollBar();
            //标记一下
            _vScrollBar.Tag = "SUMDG";
            _hScrollBar.Tag = "SUMDG";


            if (DesignMode)
            {
                base.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            }
            else
            {
                this.ScrollBars = ScrollBars.None;         //禁用dgv默认滚动条
            }
            this.Parent.Controls.Add(_vScrollBar);
            this.Parent.Controls.Add(_hScrollBar);

            _vScrollBar.Visible = false;
            _hScrollBar.Visible = false;

            //注册滚动条事件已代替dgv默认的滚动条
            _vScrollBar.Scroll += new ScrollEventHandler(vScrollBar_Scroll);
            _hScrollBar.Scroll += new ScrollEventHandler(hScrollBar_Scroll);
            InitSumRowDgv();
            this.Parent.Controls.Add(_dgvSumRow);

            this.SizeChanged += (s, e) =>
            {
                if (!_initSourceGriding)
                {
                    InitScrollWithSourceGrid();
                    this.Update();
                }
            };
        }


        /// <summary>
        /// 根据源Grid设置是否需展示滚动条
        /// </summary>
        private void InitScrollWithSourceGrid()
        {
            this.SuspendLayout();
            _vScrollBar?.SuspendLayout();
            _hScrollBar?.SuspendLayout();
            try
            {
                #region 初始化滚动条

                if (DesignMode) return;
                //如果没有使用合计功能 跳出来。
                if (!IsShowSumRow)
                {
                    return;
                }
                if (_initSourceGriding || this.Parent == null)
                {
                    return;
                }

                //初始化合计行
                if (_dgvSumRow == null)
                {
                    InitSumDgvAndScrolBar();
                }
                _initSourceGriding = true;

                if (_dock == DockStyle.Fill)
                {
                    this.Height = Parent.Height;
                    this.Width = Parent.Width;
                    this.Location = new Point(0, 0);
                }

                _dgvSourceMaxHeight = this.Height;           //dgvSource最大高度
                _dgvSourceMaxWidth = this.Width;             //dgvSource最大宽度


                if (_isShowSumRow)
                {
                    _dgvSourceMaxHeight -= _sumRowHeight;
                }
                if (_dgvSourceMaxHeight < RowHeight * 2)
                {
                    _initSourceGriding = false;
                    return;
                }

                this.Height = _dgvSourceMaxHeight;
                var displayDgvSumRowHeight = (_isShowSumRow && !DesignMode) ? _dgvSumRow.Height : 0;

                //   this.MouseWheel -= new MouseEventHandler(dgvSource_MouseWheel);
                #region 验证是否需要显示水平滚动条

                //需要展示水平滚动条
                if (this.DisplayedColumnCount(true) < this.Columns.Count)
                {
                    _dgvSourceMaxHeight -= _hScrollBar.Height;
                    this.Height = _dgvSourceMaxHeight;

                    _hScrollBar.Location = new Point(this.Location.X, this.Location.Y + this.Height + displayDgvSumRowHeight);
                    _hScrollBar.Width = _dgvSourceMaxWidth;
                    _hScrollBar.Visible = true;
                    _hScrollBar.BringToFront();
                    _hScrollBar.Minimum = 0;
                    _hScrollBar.SmallChange = AvgColWidth;
                    _hScrollBar.LargeChange = AvgColWidth * 2;
                    _hScrollBar.Maximum = ColsWidth;
                }
                else
                {
                    _hScrollBar.Visible = false;
                }
                #endregion

                //根据源dgv设置合计行
                _dgvSumRow.RowHeadersWidth = this.RowHeadersWidth - 1;

                #region 验证是否需要显示纵向滚动条

                var dgvSourceDisplayedRowCount = this.DisplayedRowCount(false);     //最多显示行数

                //不需要展示垂直滚动条
                if (dgvSourceDisplayedRowCount >= this.Rows.Count)
                {
                    _vScrollBar.Visible = false;
                    this.Width = _dgvSourceMaxWidth;
                    _dgvSumRow.Width = _dgvSourceMaxWidth;
                }
                else
                {
                    //需要展示垂直滚动条
                    _dgvSourceMaxWidth = this.Width - _vScrollBar.Width;

                    this.Width = _dgvSourceMaxWidth;
                    _vScrollBar.Height = this.Height + (_isShowSumRow ? _dgvSumRow.Height : 0);
                    _vScrollBar.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                    _vScrollBar.Visible = true;
                    _vScrollBar.Maximum = (this.Rows.Count - dgvSourceDisplayedRowCount + 2) * RowHeight;
                    _vScrollBar.Minimum = 0;
                    _vScrollBar.SmallChange = RowHeight;
                    _vScrollBar.LargeChange = RowHeight * 2;
                    _vScrollBar.BringToFront();
                }
                #endregion

                if (_isShowSumRow && !DesignMode)
                {
                    _dgvSumRow.Location = new Point(this.Location.X, this.Location.Y + _dgvSourceMaxHeight - 1);
                    _dgvSumRow.Width = this.Width;
                    _dgvSumRow.Visible = true;
                    _dgvSumRow.BringToFront();
                }
                else
                {
                    _dgvSumRow.Visible = false;
                }
                _initSourceGriding = false;
                #endregion
            }
            finally
            {
                this.ResumeLayout(true);
                _vScrollBar?.ResumeLayout();
                _hScrollBar?.ResumeLayout();
            }
        }


        /// <summary>
        /// DataGridView 列总宽.用于确定横向滚动条滚动值
        /// </summary>
        private int ColsWidth
        {
            get
            {
                int width = 0;
                foreach (DataGridViewColumn col in this.Columns)
                {
                    if (!col.Visible)
                    {
                        continue;
                    }
                    width += col.Width;
                }
                return width;
            }
        }

        /// <summary>
        /// DataGridView 列平均总宽,用于确定横向滚动条滚动值
        /// </summary>
        private int AvgColWidth
        {
            get
            {
                int width = 80;
                width = ColsWidth / this.Columns.Count;
                return width;
            }
        }

        /// <summary>
        /// 每行高度.用于确定纵向滚动条滚动值
        /// </summary>
        private int RowHeight
        {
            get
            {
                int height = 20;
                if (this.Rows.Count > 0)
                {
                    height = (this.Rows[0].Height - 3);
                }
                return height;
            }
        }

        /// <summary>
        /// 处理纵向滚动条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                this.FirstDisplayedScrollingRowIndex = e.NewValue / RowHeight;
            }
            catch (Exception)
            {


            }

        }

        /// <summary>
        /// 处理横向滚动条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int value = e.NewValue;
            this.HorizontalScrollingOffset = value;

            if (_isShowSumRow && _dgvSumRow != null)
            {
                _dgvSumRow.HorizontalScrollingOffset = value;
            }
        }

        /// <summary>
        /// 处理源dgv鼠标滚轮滚动事件,同步带动横向滚动条及纵向滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSource_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsShowSumRow)
            {
                return;
            }

            if (e.Delta == 0)
            {
                return;
            }
            if (!_vScrollBar.Visible) return;

            if ((_vScrollBar.Value - RowHeight) < 0 && e.Delta > 0)
            {
                _vScrollBar.Value = _vScrollBar.Minimum;
            }
            else if ((_vScrollBar.Value + RowHeight * 2) > _vScrollBar.Maximum && e.Delta < 0)
            {
                _vScrollBar.Value = _vScrollBar.Maximum;
            }
            else
            {
                _vScrollBar.Value -= Convert.ToInt32((e.Delta / Math.Abs(e.Delta))) * RowHeight;
            }
            try
            {
                this.FirstDisplayedScrollingRowIndex = _vScrollBar.Value / RowHeight;
            }
            catch (Exception)
            {

            }

        }

        private void this_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (_dgvSumRow != null)
            {
                if (_dgvSumRow.ColumnCount > 0)
                {
                    _dgvSumRow.Columns[e.Column.Index].Width = e.Column.Width;
                }

            }

        }

        private void this_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            if (_dgvSumRow != null)
            {
                if (_dgvSumRow.ColumnCount > 0)
                {
                    _dgvSumRow.RowHeadersWidth = this.RowHeadersWidth - 1;
                }
            }
        }


        /// <summary>
        /// 需要添加合计的datagridviewrow 列名称
        /// </summary>
        [Description("获取或设置需要用于求和的列名")]
        public string[] SumColumns
        {
            get;
            set;
        }

        //因为可能要动态控制所有这里设置没有用。不然就要设置一个key value true false
        ///// <summary>
        ///// 需要添加合计的datagridviewrow 列名称
        ///// </summary>
        //[Description("获取或设置不可见的列集合")]
        //public string[] InvisibleCols
        //{
        //    get;
        //    set;
        //}



        private void AddDgvSumRowColumns()
        {
            if (_dgvSumRow.Columns.Count == 0 || this.Columns.Count != _dgvSumRow.Columns.Count)
            {
                _dgvSumRow.Columns.Clear();

                foreach (DataGridViewColumn col in this.Columns)
                {
                    if (col.CellTemplate is DataGridViewCheckBoxCell)
                    {
                        //加总行 不显示 checkbox
                        var tempCol = new DataGridViewColumn(new DataGridViewTextBoxCell());
                        tempCol.Name = col.Name;
                        tempCol.DataPropertyName = col.DataPropertyName;
                        tempCol.DataPropertyName = string.Empty;
                        tempCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        tempCol.DefaultCellStyle.Format = _sumCellFormat;
                        _dgvSumRow.Columns.Add(tempCol);
                    }
                    else
                    {
                        var tempCol = (DataGridViewColumn)col.Clone();
                        tempCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        tempCol.DefaultCellStyle.Format = _sumCellFormat;
                        _dgvSumRow.Columns.Add(tempCol);
                    }
                }
            }
        }


        /// <summary>
        /// 合计数据
        /// </summary>
        private void SumData()
        {
            if (this.Columns.Count <= 0 || SumColumns == null || SumColumns.Length == 0)
            {
                return;
            }
            // 确保合计行已初始化
            if (_dgvSumRow == null)
            {
                InitSumRowDgv();
            }

            if (_dgvSumRow.Columns.Count != this.Columns.Count)
            {
                AddDgvSumRowColumns();
            }

            if (_dgvSumRow.Rows.Count != 1)
            {
                _dgvSumRow.Rows.Clear();
                _dgvSumRow.Rows.Add(1);
            }
            // 清除之前的合计值
            for (int i = 0; i < _dgvSumRow.Columns.Count; i++)
            {
                _dgvSumRow[i, 0].Value = "";
            }




            if (this.Rows.Count <= 0 || SumColumns == null || SumColumns.Length == 0)
            {
                return;
            }
            this.SuspendLayout();
            try
            {
                // 清除之前的合计值
                if (_dgvSumRow.Rows.Count == 0)
                    _dgvSumRow.Rows.Add(1);
                for (int i = 0; i < _dgvSumRow.Columns.Count; i++)
                {
                    _dgvSumRow[i, 0].Value = "";
                }

                var sumRowDataDic = new Dictionary<int, decimal>();

                #region 按设置的需要合计的列求和
                Array.ForEach(SumColumns, col =>
                {
                    if (!_dgvSumRow.Columns.Contains(col))
                    {
                        return;
                    }
                    var tempSumVal = 0m;
                    var colIndex = _dgvSumRow.Columns[col].Index;
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].IsNewRow) continue;

                        if (this[colIndex, i].Value == null || this[colIndex, i].Value == DBNull.Value)
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(this[colIndex, i].Value.ToString()))
                        {
                            continue;
                        }

                        var tempVal = 0m;
                        //try
                        //{
                        //    //这里要优化，当值为空时，可以跳过
                        //    tempVal = (decimal)Convert.ChangeType(this[colIndex, i].Value, typeof(decimal));
                        //}
                        //catch
                        //{
                        //}
                        //tempSumVal += tempVal;

                        if (decimal.TryParse(this[colIndex, i].Value.ToString(), out decimal value))
                        {
                            tempSumVal += value;
                        }
                    }
                    sumRowDataDic[colIndex] = tempSumVal;
                });
                #endregion

                if (sumRowDataDic.Count > 0)
                {
                    sumRowDataDic.Keys.ToList().ForEach(colIndex =>
                    {
                        _dgvSumRow[colIndex, 0].Value = sumRowDataDic[colIndex];
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算合计行时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        /// <summary>
        /// 获取合计行
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private DataGridViewRow SumRow
        {
            get
            {
                return (_isShowSumRow && _dgvSumRow.Rows.Count > 0) ? _dgvSumRow.Rows[0] : null;
            }
        }



        /*
        /// <summary>
        /// 属性会变动到 InitializeComponent中生成代码
        /// </summary>
        [Description("重写基类属性")]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
        public new object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                if (value is System.Collections.IList)
                {

                    //   HLH.Lib.List.BindingSortCollection<CodeTypeOfExpression(>


                    base.DataSource = value;
                }
                else
                {
                    base.DataSource = value;
                }

            }
        }
        */



        //当数据源改变,重新计算合计,与合计行列头重绘
        private void this_DataSourceChanged(object sender, EventArgs e)
        {
            //数据重置时，认为不需要修改，才查询出来呢。
            dgvEdit = false;
            ///这个方法是因为数据出来是 确定是否显示滚动条 ，方法中其他没有多验证了。by 2020
            InitScrollWithSourceGrid();
            //求各前。判断一下
            if (IsShowSumRow)
            {
                if (SumColumns == null)
                {
                    MessageBox.Show("统计列的属性，需要在数据源之前赋值！", "控件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SumData();
                }
            }



            //认为是 数据驱动的。画面的显示方面已经固定
            if (this.Dock == DockStyle.Fill)
            {
                ////如果没有父容器，则添加
                if (this.Parent != null)
                {
                    //认为表格直接加载到窗体的。 则先把窗体的控件全移到panel中
                    if (this.Parent.GetType().BaseType.FullName == "System.Windows.Forms.Form")
                    {
                        ///移动的只有四个控件。其他不是这个控制里面的。不处理
                        List<Control> Clist = new List<Control>();
                        List<Control> Otherlist = new List<Control>();
                        Form frm = this.Parent as Form;
                        foreach (Control item in frm.Controls)
                        {
                            ///加入了四个控制  一个主DG，一个SUNDG两个滚动条。
                            ///主dg 不应该店用tag属性，使用其他方法区别
                            if ((item.Tag != null && item.Tag.ToString() == "SUMDG") || (item is NewSumDataGridView))
                            {
                                Clist.Add(item);

                            }
                            else
                            {
                                Otherlist.Add(item);
                            }
                        }
                        frm.Controls.Clear();
                        _panel.Controls.AddRange(Clist.ToArray());
                        frm.Controls.Add(_panel);
                        frm.Controls.AddRange(Otherlist.ToArray());
                        InitScrollWithSourceGrid();
                    }
                }

            }

            //属性值 判断

            if (string.IsNullOrEmpty(XmlFileName) && UseCustomColumnDisplay && this.DataSource != null && this.Rows.Count > 0)
            {

                MessageBox.Show("用于控制列显示的配置文件名XmlFileName不能为空。");
            }
            if ((FieldNameList == null || FieldNameList.Count == 0) && UseCustomColumnDisplay && this.DataSource != null && this.Rows.Count > 0)
            {
                MessageBox.Show("用于控制列显示的集合不能为空。");
            }
            SetSelectedColumn(UseSelectedColumn);
            if (UseCustomColumnDisplay)
            {
                this.AllowDrop = true;
                if (NeedSaveColumnsXml)
                {
                    ColumnDisplays = customizeGrid.LoadColumnsListByCdc();
                }


                #region 将没有中文字段 比方ID，或对象集合这种都不启动

                List<string> FieldNames = FieldNameList.Select(kv => kv.Key).ToList();
                List<string> ColNamesDispays = ColumnDisplays.Select(c => c.ColName).ToList();
                // 将 A 中不存于 B 中的元素存储到一个新的 List<A> 中
                List<string> result = ColNamesDispays.Except(FieldNames).ToList();
                foreach (string str in result)
                {
                    ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == str).FirstOrDefault();
                    if (cdc != null)
                    {
                        cdc.Disable = true;
                        cdc.Visible = false;
                    }
                }
                #endregion

                //这里认为只执行一次？并且要把显示名的中文传过来，并且不在默认中文及控制显示列表中，就不显示了。
                foreach (var item in FieldNameList)
                {
                    ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == item.Key).FirstOrDefault();
                    if (cdc != null)
                    {
                        cdc.ColDisplayText = item.Value.Key;
                        if (!item.Value.Value)
                        {
                            cdc.Visible = item.Value.Value;//如果默认不显示，则不参加控制
                            cdc.Disable = !cdc.Visible;
                        }
                        //特别处理选择列
                        if (cdc.ColName == "Selected")
                        {
                            cdc.Visible = UseSelectedColumn;
                            cdc.Disable = !UseSelectedColumn;
                        }
                    }
                    else
                    {

                    }
                }

                ColDisplayController cdcSelected = ColumnDisplays.Where(c => c.ColName == "Selected").FirstOrDefault();
                if (cdcSelected != null)
                {
                    cdcSelected.Visible = UseSelectedColumn;
                }

                // 加载列样式
                BindColumnStyle();
            }

        }




        #region 实际选择列右键全选不选 前提是数据源基础中有一个属性为Selected
        private ContextMenuStrip CreateSelectedAllContextMenuStrip()
        {
            ContextMenuStrip contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items.Add("勾选");
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);
            contentMenu1.Items[3].Click += new EventHandler(contentMenu1_Checked);
            return contentMenu1;
        }

        private void contentMenu1_CheckAll(object sender, EventArgs e)
        {
            this.EndEdit();
            foreach (DataGridViewRow dr in this.Rows)
            {
                dr.Cells["Selected"].Value = true;
                dr.Cells["Selected"].Selected = true;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", true);
            }
        }
        private void contentMenu1_CheckNo(object sender, EventArgs e)
        {
            this.EndEdit();
            foreach (DataGridViewRow dr in this.Rows)
            {
                dr.Cells["Selected"].Value = false;
                dr.Cells["Selected"].Selected = false;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", true);
            }
        }
        private void contentMenu1_Inverse(object sender, EventArgs e)
        {
            this.EndEdit();
            foreach (DataGridViewRow dr in this.Rows)
            {
                dr.Cells["Selected"].Value = !((bool)dr.Cells["Selected"].Value);
                dr.Cells["Selected"].Selected = !dr.Cells["Selected"].Selected;
            }

        }

        private void contentMenu1_Checked(object sender, EventArgs e)
        {
            this.EndEdit();
            foreach (DataGridViewRow dr in this.Rows)
            {
                if (dr.Selected)
                {
                    dr.Cells["Selected"].Value = !((bool)dr.Cells["Selected"].Value);
                    dr.Cells["Selected"].Selected = !dr.Cells["Selected"].Selected;
                }
            }

        }


        #endregion


        /// <summary>
        /// 绘制合计行行头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSumRow_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var rectangle = new Rectangle(e.RowBounds.Location.X + 1, e.RowBounds.Location.Y + 1,
                _dgvSumRow.RowHeadersWidth - 3, e.RowBounds.Height - 3);

            Color 合计背景色 = System.Drawing.Color.FromArgb(240, 222, 197);

            e.Graphics.FillRectangle(new SolidBrush(合计背景色), rectangle);

            Color hjc = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            TextRenderer.DrawText(e.Graphics, "合计", _dgvSumRow.RowHeadersDefaultCellStyle.Font, rectangle, hjc, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            //TextRenderer.DrawText(e.Graphics, "合计", _dgvSumRow.RowHeadersDefaultCellStyle.Font, rectangle, _dgvSumRow.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }


        /// <summary>
        /// 获取或设置Dock,该属性已被重新
        /// </summary>
        [Description("获取或设置Dock,该属性已被重写")]
        public new DockStyle Dock
        {
            get { return _dock; }
            set
            {
                _dock = value;
                if (value == DockStyle.Fill)
                {
                    if (Parent != null)
                    {
                        this.Size = new Size(Parent.Width, Parent.Height);
                        this.Location = new Point(0, 0);
                    }
                    else
                    {
                        ////如果没有父容器，则添加
                        //if (this.Parent == null)
                        //{
                        //    _panel.Controls.Add(this);
                        //    this.Size = new Size(Parent.MaximumSize.Width - 20, Parent.MaximumSize.Height - 20);
                        //    this.Location = new Point(0, 0);
                        //}

                    }
                    this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                }
                else
                {
                    this.Height = Parent.Height - 20;
                    this.Width = Parent.Width - 20;
                }
            }
        }

        /// <summary>
        /// BorderStyle属性已被重写,该值固定为None,设置无效
        /// </summary>
        [Description("BorderStyle属性已被重写,该值固定为None,设置无效")]
        public new BorderStyle BorderStyle
        {
            get { return System.Windows.Forms.BorderStyle.None; }
            set { }
        }

        /// <summary>
        /// 获取或设置合计行单元格格式化字符串
        /// </summary>
        [Description("获取或设置合计行单元格格式化字符串")]
        public string SumRowCellFormat
        {
            get { return _sumCellFormat; }
            set { _sumCellFormat = value; }
        }

        /// <summary>
        /// 获取或设置是否显示合计行
        /// </summary>
        [Description("获取或设置是否显示合计行，要在绑定数据前设置这个属性")]
        public bool IsShowSumRow
        {
            get { return _isShowSumRow; }
            set
            {
                _isShowSumRow = value;
                InitScrollWithSourceGrid();
            }
        }


        private ConcurrentDictionary<string, KeyValuePair<string, bool>> _FieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();


        /// <summary>
        /// 列的显示，unitName,<单位,true>
        /// 列名，列中文，是否显示
        /// </summary>
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList { get => _FieldNameList; set => _FieldNameList = value; }



        /*
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {

        }

           /// <summary>
        /// 属性会变动到 InitializeComponent中生成代码
        /// </summary>
        [Description("重写基类属性")]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
        public override string Text
        {
        get
        {
            return base.Text;
        }
        set
        {
            base.Text = value;
            this.Name = "btn" + value;

            MessageBox.Show(this.Name);
        }
        }


        /*
        [TypeConverter(typeof(System.ComponentModel.CollectionConverter))]//指定编辑器特性
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]//设定序列化特性
        [Category("外观"), Description("图像文件集")]

        */




        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
            }
            catch (Exception)
            {

            }

        }

        private void NewSumDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            return;
            if (Rows.Count > 0)
            {
                //画一下总行数行号

                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    DataGridViewPaintParts paintParts =
                        e.PaintParts & ~DataGridViewPaintParts.Focus;

                    e.Paint(e.ClipBounds, paintParts);
                    e.Handled = true;
                }

                if (e.ColumnIndex == -1 && e.RowIndex == 1)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        (Rows.Count).ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            }

        }

        //画行号
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (Rows.Count > 0)
            {
                if (!CustomRowNo)
                {
                    #region 画行号


                    if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                    {
                        DataGridViewPaintParts paintParts =
                            e.PaintParts & ~DataGridViewPaintParts.Focus;

                        e.Paint(e.ClipBounds, paintParts);
                        e.Handled = true;
                    }

                    if (e.ColumnIndex < 0 && e.RowIndex >= 0)
                    {
                        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                        Rectangle indexRect = e.CellBounds;
                        indexRect.Inflate(-2, -2);

                        TextRenderer.DrawText(e.Graphics,
                            (e.RowIndex + 1).ToString(),
                            e.CellStyle.Font,
                            indexRect,
                            e.CellStyle.ForeColor,
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                        e.Handled = true;
                    }

                    #endregion

                }
                //画总行数行号
                if (e.ColumnIndex < 0 && e.RowIndex < 0)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        (Rows.Count + "#").ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            }
        }

        #region 分页功能实现

        /// <summary>
        /// 初始化分页面板
        /// </summary>
        private void InitializePaginationPanel()
        {
            // 分页面板初始化
            _paginationPanel = new KryptonPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.WhiteSmoke,
                Visible = _enablePagination,
                Padding = new Padding(10, 5, 10, 5)
            };

            // 创建分页控件
            CreatePaginationControls();

            // 添加到控件集合
            this.Controls.Add(_paginationPanel);

            // 将分页面板置于底层
            _paginationPanel.SendToBack();
        }

        /// <summary>
        /// 创建分页控件
        /// </summary>
        private void CreatePaginationControls()
        {
            _paginationPanel.Controls.Clear();
            _paginationPanel.SuspendLayout();

            try
            {
                // 使用流式布局确保控件自适应
                var flowLayout = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Padding = new Padding(10, 5, 10, 5)
                };

                // 1. 左侧：记录统计信息
                var lblInfo = new KryptonLabel
                {
                    Text = "共 0 条记录",
                    AutoSize = true,
                    Margin = new Padding(0, 5, 15, 0),
                    StateCommon = {
                        ShortText = {
                            Font = new Font("Microsoft YaHei UI", 9f),
                            Color1 = Color.FromArgb(102, 102, 102)
                        }
                    }
                };
                flowLayout.Controls.Add(lblInfo);

                // 添加分隔符
                flowLayout.Controls.Add(CreateSeparator());

                // 2. 页面导航按钮
                // 首页按钮
                var btnFirst = new KryptonButton
                {
                    Text = "首页",
                    Size = new Size(50, 25),
                    Enabled = false,
                    Margin = new Padding(0, 0, 5, 0)
                };
                btnFirst.Click += (s, e) => GoToPage(1);
                flowLayout.Controls.Add(btnFirst);

                // 上一页按钮
                var btnPrev = new KryptonButton
                {
                    Text = "上一页",
                    Size = new Size(60, 25),
                    Enabled = false,
                    Margin = new Padding(0, 0, 5, 0)
                };
                btnPrev.Click += (s, e) => GoToPage(_paginationInfo.PageIndex - 1);
                flowLayout.Controls.Add(btnPrev);

                // 页码显示
                var lblPage = new KryptonLabel
                {
                    Text = "第 1 页",
                    AutoSize = true,
                    Margin = new Padding(5, 5, 5, 0),
                    StateCommon = {
                        ShortText = {
                            Font = new Font("Microsoft YaHei UI", 9f),
                            Color1 = Color.FromArgb(51, 51, 51)
                        }
                    }
                };
                flowLayout.Controls.Add(lblPage);

                // 下一页按钮
                var btnNext = new KryptonButton
                {
                    Text = "下一页",
                    Size = new Size(60, 25),
                    Enabled = false,
                    Margin = new Padding(0, 0, 5, 0)
                };
                btnNext.Click += (s, e) => GoToPage(_paginationInfo.PageIndex + 1);
                flowLayout.Controls.Add(btnNext);

                // 末页按钮
                var btnLast = new KryptonButton
                {
                    Text = "末页",
                    Size = new Size(50, 25),
                    Enabled = false,
                    Margin = new Padding(0, 0, 5, 0)
                };
                btnLast.Click += (s, e) => GoToPage(_paginationInfo.TotalPages);
                flowLayout.Controls.Add(btnLast);

                // 添加分隔符
                flowLayout.Controls.Add(CreateSeparator());

                // 3. 页面大小选择
                var lblPageSize = new KryptonLabel
                {
                    Text = "每页:",
                    AutoSize = true,
                    Margin = new Padding(0, 5, 5, 0),
                    StateCommon = {
                        ShortText = {
                            Font = new Font("Microsoft YaHei UI", 9f),
                            Color1 = Color.FromArgb(102, 102, 102)
                        }
                    }
                };
                flowLayout.Controls.Add(lblPageSize);

                var cmbPageSize = new KryptonComboBox
                {
                    Width = 60,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Margin = new Padding(0, 0, 15, 0)
                };
                cmbPageSize.Items.AddRange(_pageSizeOptions.Cast<object>().ToArray());
                cmbPageSize.SelectedItem = _paginationInfo.PageSize;
                cmbPageSize.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbPageSize.SelectedItem != null)
                    {
                        _paginationInfo.PageSize = (int)cmbPageSize.SelectedItem;
                        _paginationInfo.PageIndex = 1;
                        OnPaginationChanged();
                    }
                };
                flowLayout.Controls.Add(cmbPageSize);

                // 4. 页码跳转
                var lblJump = new KryptonLabel
                {
                    Text = "跳至:",
                    AutoSize = true,
                    Margin = new Padding(0, 5, 5, 0),
                    StateCommon = {
                        ShortText = {
                            Font = new Font("Microsoft YaHei UI", 9f),
                            Color1 = Color.FromArgb(102, 102, 102)
                        }
                    }
                };
                flowLayout.Controls.Add(lblJump);

                var txtPage = new KryptonTextBox
                {
                    Width = 40,
                    Margin = new Padding(0, 0, 5, 0)
                };
                flowLayout.Controls.Add(txtPage);

                var btnGo = new KryptonButton
                {
                    Text = "GO",
                    Size = new Size(40, 25)
                };
                btnGo.Click += (s, e) =>
                {
                    if (int.TryParse(txtPage.Text, out int page) && page > 0 && page <= _paginationInfo.TotalPages)
                    {
                        GoToPage(page);
                    }
                };
                flowLayout.Controls.Add(btnGo);

                _paginationPanel.Controls.Add(flowLayout);
            }
            finally
            {
                _paginationPanel.ResumeLayout(true);
            }
        }

        /// <summary>
        /// 创建分隔符
        /// </summary>
        private Control CreateSeparator()
        {
            return new Label
            {
                Text = "|",
                AutoSize = true,
                Margin = new Padding(5, 5, 5, 0),
                ForeColor = Color.FromArgb(204, 204, 204),
                Font = new Font("Microsoft YaHei UI", 9f)
            };
        }

        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="pageIndex">目标页码</param>
        private void GoToPage(int pageIndex)
        {
            if (pageIndex < 1 || pageIndex > _paginationInfo.TotalPages)
                return;

            _paginationInfo.PageIndex = pageIndex;
            OnPaginationChanged();
        }

        /// <summary>
        /// 分页变更事件处理
        /// </summary>
        private void OnPaginationChanged()
        {
            PaginationChanged?.Invoke(this, _paginationInfo);
            UpdatePaginationUI();
        }

        /// <summary>
        /// 更新分页UI状态
        /// </summary>
        private void UpdatePaginationUI()
        {
            if (!_enablePagination) return;

            // 更新按钮状态
            var btnFirst = _paginationPanel.Controls.OfType<KryptonButton>().FirstOrDefault(b => b.Text == "首页");
            var btnPrev = _paginationPanel.Controls.OfType<KryptonButton>().FirstOrDefault(b => b.Text == "上一页");
            var btnNext = _paginationPanel.Controls.OfType<KryptonButton>().FirstOrDefault(b => b.Text == "下一页");
            var btnLast = _paginationPanel.Controls.OfType<KryptonButton>().FirstOrDefault(b => b.Text == "末页");

            if (btnFirst != null) btnFirst.Enabled = _paginationInfo.PageIndex > 1;
            if (btnPrev != null) btnPrev.Enabled = _paginationInfo.PageIndex > 1;
            if (btnNext != null) btnNext.Enabled = _paginationInfo.PageIndex < _paginationInfo.TotalPages;
            if (btnLast != null) btnLast.Enabled = _paginationInfo.PageIndex < _paginationInfo.TotalPages;

            // 更新页码显示
            var lblPage = _paginationPanel.Controls.OfType<KryptonLabel>().FirstOrDefault(l => l.Text.StartsWith("第"));
            if (lblPage != null) lblPage.Text = $"第 {_paginationInfo.PageIndex} 页 / 共 {_paginationInfo.TotalPages} 页";

            // 更新记录统计
            var lblInfo = _paginationPanel.Controls.OfType<KryptonLabel>().FirstOrDefault(l => l.Text.StartsWith("共"));
            if (lblInfo != null) lblInfo.Text = $"共 {_paginationInfo.TotalCount} 条记录，显示 {_paginationInfo.StartRecord}-{_paginationInfo.EndRecord}";
        }

        /// <summary>
        /// 更新分页面板可见性
        /// </summary>
        private void UpdatePaginationPanelVisibility()
        {
            if (_paginationPanel != null)
            {
                _paginationPanel.Visible = _enablePagination;
            }
        }

        /// <summary>
        /// 设置分页数据
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        public void SetPaginationData(long totalCount, int pageIndex = 1, int pageSize = 20)
        {
            _paginationInfo.TotalCount = totalCount;
            _paginationInfo.PageIndex = pageIndex;
            _paginationInfo.PageSize = pageSize;

            UpdatePaginationUI();
        }

        /// <summary>
        /// 设置分页数据（从SqlSugar的RefAsync<int>获取总记录数）
        /// </summary>
        /// <param name="totalCount">总记录数引用</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        public void SetPaginationData(RefAsync<int> totalCount, int pageIndex = 1, int pageSize = 20)
        {
            _paginationInfo.TotalCount = totalCount;
            _paginationInfo.PageIndex = pageIndex;
            _paginationInfo.PageSize = pageSize;

            UpdatePaginationUI();
        }

        /// <summary>
        /// 绑定分页数据（适用于SqlSugar的ToPageListAsync方法）
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="data">分页数据列表</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        public void BindPaginationData<TEntity>(List<TEntity> data, long totalCount, int pageIndex = 1, int pageSize = 20)
        {
            SetPaginationData(totalCount, pageIndex, pageSize);

            // 绑定数据到DataGridView
            if (this.DataSource is BindingSource bindingSource)
            {
                bindingSource.DataSource = data?.ToBindingSortCollection() ?? new List<TEntity>().ToBindingSortCollection();
            }
            else
            {
                this.DataSource = data?.ToBindingSortCollection() ?? new List<TEntity>().ToBindingSortCollection();
            }
        }

        /// <summary>
        /// 绑定分页数据（适用于SqlSugar的ToPageListAsync方法，使用RefAsync<int>）
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="data">分页数据列表</param>
        /// <param name="totalCount">总记录数引用</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        public void BindPaginationData<TEntity>(List<TEntity> data, RefAsync<int> totalCount, int pageIndex = 1, int pageSize = 20)
        {
            SetPaginationData(totalCount, pageIndex, pageSize);

            // 绑定数据到DataGridView
            if (this.DataSource is BindingSource bindingSource)
            {
                bindingSource.DataSource = data?.ToBindingSortCollection() ?? new List<TEntity>().ToBindingSortCollection();
            }
            else
            {
                this.DataSource = data?.ToBindingSortCollection() ?? new List<TEntity>().ToBindingSortCollection();
            }
        }

        /// <summary>
        /// 智能启用分页功能（根据数据量自动判断）
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="autoEnableThreshold">自动启用阈值（默认1000条）</param>
        public void SmartEnablePagination(long totalCount, int autoEnableThreshold = 1000)
        {
            if (totalCount > autoEnableThreshold)
            {
                EnablePagination = true;
                SetPaginationData(totalCount);
            }
            else
            {
                EnablePagination = false;
            }
        }

        #endregion
    }




    [Serializable]
    [TypeConverter(typeof(MenuControllerConverter))]
    public class ContextMenuController
    {
        public ContextMenuController()
        {
            menuText = "menuText1";
        }
        private string menuText = string.Empty;
        private bool isShow = true;
        private string _clickEventName = string.Empty;
        public string MenuText { get => menuText; set => menuText = value; }
        public bool IsShow { get => isShow; set => isShow = value; }
        public string ClickEventName { get => _clickEventName; set => _clickEventName = value; }
        /// <summary>
        /// 是否为分割线
        /// </summary>
        public bool IsSeparator { get => isSeparator; set => isSeparator = value; }

        private bool isSeparator = false;

        //        public ContextMenuControler(string _menuText, EventHandler _click, bool isSeparatorLine)
        public ContextMenuController(string _menuText, bool isShow, bool isSeparatorLine, string _click)
        {
            menuText = _menuText;
            _clickEventName = _click;
            isSeparator = isSeparatorLine;
            IsShow = isShow;
        }


    }



    // 新增一个枚举用于表示数据类型
    public enum ColumnDataType
    {
        String,
        Integer,
        Decimal,
        Boolean,
        DateTime,
        Other
    }

    // 筛选类型枚举
    public enum FilterType
    {
        Contains,
        StartsWith,
        Equals,
        NotEqual
    }


}