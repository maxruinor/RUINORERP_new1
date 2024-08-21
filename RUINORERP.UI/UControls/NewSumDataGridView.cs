using FastReport.DevComponents.DotNetBar.Controls;
using Krypton.Toolkit;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

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
    /// </summary>
    [Serializable]
    public class NewSumDataGridView : KryptonDataGridView
    {

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
                _UseSelectedColumn = value;
                SetSelectedColumn(_UseSelectedColumn);
            }
        }


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
            //ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        //

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

        private ContextMenuStrip _cMenus = new ContextMenuStrip();
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
        private List<EventHandler> ContextClickList = new List<EventHandler>();


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


        /// <summary>
        /// 初始化
        /// </summary>
        //[Designer(typeof(MyDesigner))]
        public NewSumDataGridView()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColumnWidthChanged += new DataGridViewColumnEventHandler(this_ColumnWidthChanged);
            this.DataSourceChanged += new EventHandler(this_DataSourceChanged);
            this.RowHeadersWidthChanged += new EventHandler(this_RowHeadersWidthChanged);
            this.MouseWheel += new MouseEventHandler(dgvSource_MouseWheel);
            this.CellEndEdit += NewSumDataGridView_CellEndEdit;
            this.DataError += NewSumDataGridView_DataError;
            this.CellValueChanged += NewSumDataGridView_CellValueChanged;
            this.CurrentCellChanged += NewSumDataGridView_CurrentCellChanged;
            this.CurrentCellDirtyStateChanged += NewSumDataGridView_CurrentCellDirtyStateChanged;
            this.DataBindingComplete += NewSumDataGridView_DataBindingComplete;
            this.ColumnDisplayIndexChanged += NewSumDataGridView_ColumnDisplayIndexChanged;
            this.MouseDown += NewSumDataGridView_MouseDown;
            this.CellClick += NewSumDataGridView_CellClick;
            //所以行号不需要特别处理，调用者也不需要实现，除非有特殊要求
            //this.CellPainting += NewSumDataGridView_CellPainting;
            this.ReadOnly = false;
            #region 后加by watson 参考自其他dg

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


            // this.FirstDisplayedScrollingRowIndex = this.Rows[this.Rows.Count - 1].Index;
            //this.RowHeadersWidth = 59;
            //this.this.Rows[this.Rows.Count + 1].Selected = true;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;

            #endregion
            //不占用原始的属性
            // this.Tag = "SUMDG";

            _panel.Dock = DockStyle.Fill;
            //     _panel.BackColor = Color.FromArgb(255, 192, 192);
            _panel.BorderStyle = BorderStyle.FixedSingle;
            _panel.Size = new Size(800, 500);

            //自定义列
            customizeGrid = new CustomizeGrid();
            customizeGrid.UseCustomColumnDisplay = UseCustomColumnDisplay;
            AllowUserToOrderColumns = UseCustomColumnDisplay;
            customizeGrid.targetDataGridView = this;



            //运行时，直接判断属性是否设置。如果没有就提示
            //或者在数据变动时提示
            //合并 设置右键菜单 只执行一次
            if (!setContextMenu)
            {
                SetContextMenu();
                setContextMenu = true;
            }

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
                        //Console.WriteLine("Selected Cell Value: " + columnIndex, rowIndex].Value);
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
                Console.WriteLine("Changed");
            }
          */
            Console.WriteLine("{0} 的位置改变到 {1} ", e.Column.Name, e.Column.DisplayIndex);
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
        public List<ColumnDisplayController> ColumnDisplays { get; set; } = new List<ColumnDisplayController>();

        public void SaveColumnStyle()
        {
            foreach (DataGridViewColumn dc in Columns)
            {
                ColumnDisplayController cdc = ColumnDisplays.Where(s => s.ColName == dc.Name).FirstOrDefault();
                if (cdc != null)
                {
                    cdc.ColDisplayText = dc.HeaderText;
                    //因为使用了开源的UI框架 krypton 在关闭时还会执行ColumnDisplayIndexChanged，将顺序打乱了。所以这里有一些问题
                    //所以通过添加一个属性和一个方法来判断，并且拖放时立即保存到List中。保存时就不从dg中取显示顺序了
                    //cdc.ColDisplayIndex = dc.DisplayIndex; 这个特别处理
                    cdc.ColWith = dc.Width;
                    cdc.ColEncryptedName = dc.Name;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                }
            }

            var cols = from ColumnDisplayController col in ColumnDisplays
                       orderby col.ColDisplayIndex
                       select col;
            customizeGrid.SaveColumnsList(cols.ToList());
        }

        /// <summary>
        /// 绑定列样式
        /// </summary>
        public void BindColumnStyle()
        {
            if (Columns.Count == 0)
            {
                return;
            }
            //foreach (var item in InvisibleCols)
            //{
            //    ColumnDisplayController cdcInv = ColumnDisplays.Where(c => c.ColName == item).FirstOrDefault();
            //    if (cdcInv != null)
            //    {
            //        cdcInv.Disable = true;
            //    }
            //}

            ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == "Selected").FirstOrDefault();
            if (cdc != null)
            {
                cdc.Disable = !UseSelectedColumn;
            }
            //加载列样式
            foreach (ColumnDisplayController displayController in ColumnDisplays)
            {
                if (Columns.Contains(displayController.ColName))
                {
                    Columns[displayController.ColName].HeaderText = displayController.ColDisplayText;
                    if (displayController.ColDisplayIndex < ColumnCount)
                    {
                        Columns[displayController.ColName].DisplayIndex = displayController.ColDisplayIndex;
                    }
                    Columns[displayController.ColName].Width = displayController.ColWith;
                    //Columns[displayController.ColName].DataPropertyName = displayController.DataPropertyName;
                    Columns[displayController.ColName].Visible = displayController.Visible;

                    //if (displayController.ColName != "Selected")
                    //{
                    //    Columns[displayController.ColName].ReadOnly = true;
                    //}

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

                }

            }


            BindColumnStyleFordgvSum();


        }

        private void BindColumnStyleFordgvSum()
        {
            if (_dgvSumRow == null)
            {
                return;
            }
            //加载列样式
            foreach (ColumnDisplayController displayController in ColumnDisplays)
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

                    _dgvSumRow.Columns[displayController.ColName].Width = displayController.ColWith;
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


            if (UseCustomColumnDisplay)
            {
                #region 如果列为空时 再初始化一次
                if (ColumnDisplays.Count == 0)
                {
                    this.AllowDrop = true;
                    ColumnDisplays = customizeGrid.LoadColumnsListByCdc();

                    #region 将没有中文字段 比方ID，或对象集合这种都不启动

                    List<string> FieldNames = FieldNameList.Select(kv => kv.Key).ToList();
                    List<string> ColNamesDispays = ColumnDisplays.Select(c => c.ColName).ToList();
                    // 将 A 中不存于 B 中的元素存储到一个新的 List<A> 中
                    List<string> result = ColNamesDispays.Except(FieldNames).ToList();
                    foreach (string str in result)
                    {
                        ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == str).FirstOrDefault();
                        if (cdc != null)
                        {
                            cdc.Disable = true;
                        }
                    }
                    #endregion

                    //这里认为只执行一次？并且要把显示名的中文传过来，并且不在默认中文及控制显示列表中，就不显示了。
                    foreach (var item in FieldNameList)
                    {
                        ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == item.Key).FirstOrDefault();
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
        private void SetContextMenu()
        {
            //初始化右键菜单
            _cMenus.BackColor = Color.FromArgb(192, 255, 255);

            //如果需要通过设计时对属性值修改。
            //则不能在这个构造函数中操作。因为这时属性值不能获取
            _cMenus.Items.Clear();
            //或者也可以指定内置哪些生效 合并外面的？
            if (Use是否使用内置右键功能)
            {
                #region 生成内置的右键菜单

                if (ContextClickList == null)
                {
                    ContextClickList = new List<EventHandler>();
                }

                ContextClickList.Add(NewSumDataGridView_批量修改列值);
                ContextClickList.Add(NewSumDataGridView_复制单元数据);
                ContextClickList.Add(NewSumDataGridView_导出excel);
                ContextClickList.Add(NewSumDataGridView_保存数据到DB);
                ContextClickList.Add(NewSumDataGridView_自定义列);
                ContextClickList.Add(NewSumDataGridView_SelectedAll);
                // ContextClickList.Add(NewSumDataGridView_Test);
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                //只是初始化不重复添加
                if (_ContextMenucCnfigurator.Count == 0 && GetIsDesignMode())
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


                #endregion

                foreach (var item in _ContextMenucCnfigurator)
                {
                    if (!item.IsShow)
                    {
                        continue;
                    }

                    if (item.IsSeparator)
                    {
                        ToolStripSeparator ts1 = new ToolStripSeparator();
                        _cMenus.Items.Add(ts1);
                    }
                    else
                    {
                        ////C#通过函数名字符串执行相应的函数
                        //EventHandler eh = new EventHandler(NewSumDataGridView_自定义列);
                        //Type t = typeof(NewSumDataGridView);//括号中的为所要使用的函数所在的类的类名。
                        //MethodInfo mt = t.GetMethod(item.ClickEventName, BindingFlags.NonPublic);
                        //if (mt == null)
                        //{
                        //    Console.WriteLine("没有获取到相应的函数！！");
                        //}
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
                        _cMenus.Items.Add(item.MenuText, null, ehh);
                    }
                }
            }
            else
            {
                //删除选中行 如果用户 在控件端实现的这个事件，则单独加在这里
                if (删除选中行 != null)
                {
                    ContextMenuController cmc = new ContextMenuController("【删除选中行】", true, false, "删除选中行");
                    _cMenus.Items.Add(cmc.MenuText, null, 删除选中行);
                }
            }

            //意思是如果设置过了。就不重复设置
            if (this.ContextMenuStrip != null)
            {
                if (Use是否使用内置右键功能)
                {
                    this.ContextMenuStrip.Tag = true;
                    ToolStripSeparator tss = new ToolStripSeparator();
                    this.ContextMenuStrip.Items.Add(tss);
                    //合并外面的
                    ToolStripItem[] ts = new ToolStripItem[_cMenus.Items.Count];
                    _cMenus.Items.CopyTo(ts, 0);
                    //item.DropDownItems //指下一级
                    //这里同级添加
                    this.ContextMenuStrip.Items.AddRange(ts);
                }
            }
            else
            {
                ContextMenuStrip = _cMenus;
            }

        }



        private List<ContextMenuController> _ContextMenucCnfigurator = new List<ContextMenuController>();


        // [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        //[TypeConverter(typeof(System.ComponentModel.CollectionConverter))]//指定编辑器特性
        [TypeConverter(typeof(RUINORERP.UI.UControls.MenuControllerConverter))]
        //代码生成器产生对象内容的代码，而不是对象本身的代码。
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), MergableProperty(false)]//设定序列化特性
        [Category("行为"), Description("右键菜单控制器，控制右键菜单的显示等")]
        private List<ContextMenuController> ContextMenucCnfigurator
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
                    Clipboard.SetDataObject(dgv.GetClipboardContent());
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

        private void NewSumDataGridView_Test(object sender, EventArgs e)
        {

            MessageBox.Show("tset");
        }

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
            _dgvSumRow.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
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
                    var tempCol = (DataGridViewColumn)col.Clone();
                    tempCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    tempCol.DefaultCellStyle.Format = _sumCellFormat;
                    _dgvSumRow.Columns.Add(tempCol);
                }
            }
        }


        /// <summary>
        /// 合计数据
        /// </summary>
        private void SumData()
        {
            if (this.Columns.Count <= 0)
            {
                return;
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
            //要刷新总计的结果 
            for (int i = 0; i < _dgvSumRow.Columns.Count; i++)
            {
                _dgvSumRow[i, 0].Value = "";
            }




            if (this.Rows.Count <= 0 || SumColumns == null || SumColumns.Length == 0)
            {
                return;
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
                    if (this[colIndex, i].Value == null)
                    {
                        continue;
                    }
                    if (this[colIndex, i].Value == DBNull.Value)
                    {
                        continue;
                    }
                    if (this[colIndex, i].Value.ToString() == "")
                    {
                        continue;
                    }

                    var tempVal = 0m;
                    try
                    {
                        //这里要优化，当值为空时，可以跳过
                        tempVal = (decimal)Convert.ChangeType(this[colIndex, i].Value, typeof(decimal));
                    }
                    catch
                    {
                    }
                    tempSumVal += tempVal;
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

            if (string.IsNullOrEmpty(XmlFileName) && UseCustomColumnDisplay)
            {
                MessageBox.Show("用于控制列显示的配置文件名XmlFileName不能为空。");
            }
            if ((FieldNameList == null || FieldNameList.Count == 0) && UseCustomColumnDisplay)
            {
                MessageBox.Show("用于控制列显示的集合不能为空。");
            }
            SetSelectedColumn(UseSelectedColumn);
            if (UseCustomColumnDisplay)
            {
                this.AllowDrop = true;
                ColumnDisplays = customizeGrid.LoadColumnsListByCdc();

                #region 将没有中文字段 比方ID，或对象集合这种都不启动

                List<string> FieldNames = FieldNameList.Select(kv => kv.Key).ToList();
                List<string> ColNamesDispays = ColumnDisplays.Select(c => c.ColName).ToList();
                // 将 A 中不存于 B 中的元素存储到一个新的 List<A> 中
                List<string> result = ColNamesDispays.Except(FieldNames).ToList();
                foreach (string str in result)
                {
                    ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == str).FirstOrDefault();
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
                    ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == item.Key).FirstOrDefault();
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

                ColumnDisplayController cdcSelected = ColumnDisplays.Where(c => c.ColName == "Selected").FirstOrDefault();
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
            base.OnPaint(e);
            //if (this.Rows.Count > 0)
            //{
            //    if (this.TopLeftHeaderCell.Value == null)
            //    {
            //        this.TopLeftHeaderCell.Value = this.Rows.Count.ToString();
            //    }
            //    if (this.Rows.Count.ToString() != this.TopLeftHeaderCell.Value.ToString())
            //    {
            //        this.TopLeftHeaderCell.Value = this.Rows.Count.ToString();
            //    }
            //}
            if (_panel.Controls.Count > 0)
            {
                if (this.IsShowSumRow)
                {

                }
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

}
