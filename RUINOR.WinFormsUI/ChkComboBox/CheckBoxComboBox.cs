using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using HLH.Lib;
using System.Threading.Tasks;

namespace RUINOR.WinFormsUI.ChkComboBox
{
    /// <summary>
    /// 自定义多选下拉框控件，支持多选操作
    /// Martin Lottering : 2007-10-27
    /// --------------------------------
    /// This is a usefull control in Filters. Allows you to save space and can replace a Grouped Box of CheckBoxes.
    /// Currently used on the TasksFilter for Todoses, which means the user can select which Statusses to include
    /// in the "Search".
    /// This control does not implement a CheckBoxListBox, instead it adds a wrapper for the normal ComboBox and Items. 
    /// See the CheckBoxItems property.
    /// ----------------
    /// ALSO IMPORTANT: In Data Binding when setting the DataSource. The ValueMember must be a bool type property, because it will 
    /// be binded to the Checked property of the displayed CheckBox. Also see the DisplayMemberSingleItem for more information.
    /// ----------------
    /// 特别说明：
    /// 1. 该控件有一个特殊的MultiChoiceResults属性，用于保存选中的数据项集合
    /// 2. 当设置DataSource属性时，不能同时直接操作Items集合
    /// 3. 建议通过BindData4CmbChkRefWithLimited方法进行数据绑定，以确保正确初始化
    /// ----------------
    /// Extends the CodeProject PopupComboBox "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp"
    /// by Lukasz Swiatkowski.
    /// </summary>
    public partial class CheckBoxComboBox : PopupComboBox
    {

        #region by watson

        private List<object> _MultiChoiceResults = new List<object>();
        // 异步更新标志，避免重复触发异步更新
        private bool _isUpdatingStates = false;
        // 异步更新队列标志，表示有新的更新请求在等待
        private bool _hasPendingUpdate = false;

        /// <summary>
        /// 用来保存选择的结果
        /// 优化了大量数据处理性能
        /// 特别针对200个选中项的场景进行了优化
        /// </summary>
        [Localizable(true)]
        [Bindable(true)]
        [Description("获取或设置用来保存选择的结果的值")]
        public List<object> MultiChoiceResults
        {
            get { return _MultiChoiceResults; }
            set
            {
                // 快速路径：如果控件已释放，直接返回
                if (IsDisposed)
                {
                    return;
                }
                
                // 快速路径：如果引用相同，直接返回
                if (ReferenceEquals(_MultiChoiceResults, value))
                {
                    return;
                }
                
                // 准备新的结果列表
                List<object> newResults = value ?? new List<object>();
                
                // 检查是否需要实际更新（比较内容）
                bool needsUpdate = true;
                if (_MultiChoiceResults.Count == newResults.Count && _MultiChoiceResults.Count > 0)
                {
                    // 使用HashSet进行快速内容比较，避免不必要的更新
                    HashSet<object> existingValues = new HashSet<object>(_MultiChoiceResults);
                    HashSet<object> newValueHash = new HashSet<object>(newResults);
                    
                    // 如果两个集合内容相等，则不需要更新
                    needsUpdate = !existingValues.SetEquals(newValueHash);
                }
                
                // 只有在真正需要更新时才进行赋值和UI更新
                if (needsUpdate)
                {
                    // 赋值新的结果列表
                    _MultiChoiceResults = newResults;
                    
                    // 当属性值改变时，更新CheckBox的选中状态
                    // 确保控件已创建句柄且列表控件已初始化
                    if (IsHandleCreated && _CheckBoxComboBoxListControl != null)
                    {
                        int itemCount = _MultiChoiceResults.Count;
                        int checkBoxCount = CheckBoxItems.Count;
                        
                        // 快速路径：如果没有选项，直接返回
                        if (checkBoxCount == 0)
                        {
                            return;
                        }
                        
                        // 对于大量数据（超过50个选中项），使用异步更新
                        if (itemCount > 50)
                        {
                            // 如果已有异步更新在进行中，设置待处理标志
                            if (_isUpdatingStates)
                            {
                                _hasPendingUpdate = true;
                            }
                            else
                            {
                                // 执行异步更新
                                ExecuteAsyncUpdate();
                            }
                        }
                        else if (itemCount > 0 || (checkBoxCount > 0 && itemCount == 0))
                        {
                            // 小规模数据或清空操作，同步更新
                            UpdateCheckedStates();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 异步执行UpdateCheckedStates操作
        /// </summary>
        private void ExecuteAsyncUpdate()
        {
            // 设置正在更新标志
            _isUpdatingStates = true;
            
            // 使用基类提供的异步执行方法
            ExecuteAsyncOperation(
                async () =>
                {
                    // 后台线程中进行一些预处理工作，不直接操作UI
                    // 可以在这里进行一些计算密集型的操作
                    await Task.Delay(1); // 让出线程时间片，避免阻塞
                },
                () =>
                {
                    try
                    {
                        // UI线程中应用选中状态
                        UpdateCheckedStates();
                    }
                    finally
                    {
                        // 更新完成后重置标志
                        _isUpdatingStates = false;
                        
                        // 检查是否有待处理的更新请求
                        if (_hasPendingUpdate)
                        {
                            _hasPendingUpdate = false;
                            ExecuteAsyncUpdate();
                        }
                    }
                }
            );
        }

        #endregion

        #region CONSTRUCTOR

        public CheckBoxComboBox()
            : base()
        {
            InitializeComponent();
            _CheckBoxProperties = new CheckBoxProperties();
            _CheckBoxProperties.PropertyChanged += new EventHandler(_CheckBoxProperties_PropertyChanged);
            // Dumps the ListControl in a(nother) Container to ensure the ScrollBar on the ListControl does not
            // Paint over the Size grip. Setting the Padding or Margin on the Popup or host control does
            // not work as I expected. I don't think it can work that way.
            CheckBoxComboBoxListControlContainer ContainerControl = new CheckBoxComboBoxListControlContainer();
            _CheckBoxComboBoxListControl = new CheckBoxComboBoxListControl(this);
            _CheckBoxComboBoxListControl.Items.CheckBoxCheckedChanged += new EventHandler(Items_CheckBoxCheckedChanged);
            ContainerControl.Controls.Add(_CheckBoxComboBoxListControl);
            // This padding spaces neatly on the left-hand side and allows space for the size grip at the bottom.
            ContainerControl.Padding = new Padding(4, 0, 0, 14);
            // The ListControl FILLS the ListContainer.
            _CheckBoxComboBoxListControl.Dock = DockStyle.Fill;
            // The DropDownControl used by the base class. Will be wrapped in a popup by the base class.
            DropDownControl = ContainerControl;
            // Must be set after the DropDownControl is set, since the popup is recreated.
            // NOTE: I made the dropDown protected so that it can be accessible here. It was private.
            dropDown.Resizable = true;
        }

        #endregion

        #region PRIVATE FIELDS

        /// <summary>
        /// The checkbox list control. The public CheckBoxItems property provides a direct reference to its Items.
        /// </summary>
        internal CheckBoxComboBoxListControl _CheckBoxComboBoxListControl;
        /// <summary>
        /// In DataBinding operations, this property will be used as the DisplayMember in the CheckBoxComboBoxListBox.
        /// The normal/existing "DisplayMember" property is used by the TextBox of the ComboBox to display 
        /// a concatenated Text of the items selected. This concatenation and its formatting however is controlled 
        /// by the Binded object, since it owns that property.
        /// </summary>
        private string _DisplayMemberSingleItem = null;
        internal bool _MustAddHiddenItem = false;
        /// <summary>
        /// 存储业务性主键字段名
        /// </summary>
        private string _KeyFieldName = null;

        #endregion

        #region PRIVATE OPERATIONS

        /// <summary>
        /// Builds a CSV string of the items selected.
        /// </summary>
        internal string GetCSVText(bool skipFirstItem)
        {
            // 预分配容量，减少StringBuilder动态扩容的开销
            StringBuilder sb = new StringBuilder(MultiChoiceResults.Count * 30); // 假设平均每个项目30个字符
            int StartIndex = 
                DropDownStyle == ComboBoxStyle.DropDownList
                && DataSource == null
                && skipFirstItem
                    ? 1
                    : 0;
            
            // 优化：直接使用MultiChoiceResults来构建文本，避免遍历所有项
            // 但需要确保CheckBoxItems与MultiChoiceResults同步
            // 这里保留原逻辑，但添加了一些小优化
            for (int Index = StartIndex; Index <= _CheckBoxComboBoxListControl.Items.Count - 1; Index++)
            {
                CheckBoxComboBoxItem Item = _CheckBoxComboBoxListControl.Items[Index];
                if (Item.Checked)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    // 检查Item.Text是否为空或null
                    string itemText = Item.Text ?? "";
                    sb.Append(itemText);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// A direct reference to the Items of CheckBoxComboBoxListControl.
        /// You can use it to Get or Set the Checked status of items manually if you want.
        /// But do not manipulate the List itself directly, e.g. Adding and Removing, 
        /// since the list is synchronised when shown with the ComboBox.Items. So for changing 
        /// the list contents, use Items instead.
        /// </summary>
        [Browsable(false)]
        public CheckBoxComboBoxItemList CheckBoxItems
        {
            get
            {
                // Added to ensure the CheckBoxItems are ALWAYS
                // available for modification via code.
                if (_CheckBoxComboBoxListControl.Items.Count != Items.Count)
                    _CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
                return _CheckBoxComboBoxListControl.Items;
            }
        }
        /// <summary>
        /// The DataSource of the combobox. Refreshes the CheckBox wrappers when this is set.
        /// </summary>
        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;
                // 当设置DataSource时，重置_MustAddHiddenItem标志
                _MustAddHiddenItem = false;
                if (!string.IsNullOrEmpty(ValueMember))
                    // This ensures that at least the checkboxitems are available to be initialised.
                    _CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }
        /// <summary>
        /// The ValueMember of the combobox. Refreshes the CheckBox wrappers when this is set.
        /// </summary>
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set
            {
                base.ValueMember = value;
                if (!string.IsNullOrEmpty(ValueMember))
                    // This ensures that at least the checkboxitems are available to be initialised.
                    _CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }
        /// <summary>
        /// In DataBinding operations, this property will be used as the DisplayMember in the CheckBoxComboBoxListBox.
        /// The normal/existing "DisplayMember" property is used by the TextBox of the ComboBox to display 
        /// a concatenated Text of the items selected. This concatenation however is controlled by the Binded 
        /// object, since it owns that property.
        /// </summary>
        public string DisplayMemberSingleItem
        {
            get { if (string.IsNullOrEmpty(_DisplayMemberSingleItem)) return DisplayMember; else return _DisplayMemberSingleItem; }
            set { _DisplayMemberSingleItem = value; }
        }
        /// <summary>
        /// 获取或设置业务性的主键字段名，用于在绑定时从数据源对象中获取对应的值
        /// </summary>
        [Browsable(true)]
        [Description("获取或设置业务性的主键字段名，用于在绑定时从数据源对象中获取对应的值")]
        public string KeyFieldName
        {
            get { return _KeyFieldName; }
            set { _KeyFieldName = value; }
        }
        /// <summary>
        /// Made this property Browsable again, since the Base Popup hides it. This class uses it again.
        /// Gets an object representing the collection of the items contained in this 
        /// System.Windows.Forms.ComboBox.
        /// </summary>
        /// <returns>A System.Windows.Forms.ComboBox.ObjectCollection representing the items in 
        /// the System.Windows.Forms.ComboBox.
        /// </returns>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        #endregion

        #region EVENTS & EVENT HANDLERS

        public event EventHandler CheckBoxCheckedChanged;

        private void Items_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxCheckedChanged(sender, e);
        }

        #endregion

        #region EVENT CALLERS and OVERRIDES e.g. OnResize()

        protected void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 获取触发事件的CheckBox项
            CheckBoxComboBoxItem changedItem = sender as CheckBoxComboBoxItem;
            if (changedItem == null)
                return;
            
            // 使用标志避免不必要的UI更新
            bool needTextUpdate = false;
            
            ObjectSelectionWrapper<CmbChkItem> objectSelection = changedItem.ComboBoxItem as ObjectSelectionWrapper<CmbChkItem>;
            //数据源绑定情况
            if (objectSelection != null && objectSelection.Item != null && !string.IsNullOrEmpty(objectSelection.Item.Key))
            {
                string itemKey = objectSelection.Item.Key.ToString();
                
                if (changedItem.Checked)
                {
                    // 检查是否已存在相同的key值
                    bool exists = false;
                    // 优化：对于大量数据，可以考虑使用哈希表查找
                    if (MultiChoiceResults.Count > 50)
                    {
                        // 临时使用HashSet提高查找效率
                        HashSet<string> existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (var value in MultiChoiceResults)
                        {
                            if (value != null)
                                existingKeys.Add(value.ToString());
                        }
                        exists = existingKeys.Contains(itemKey);
                    }
                    else
                    {
                        // 小规模数据时直接遍历
                        foreach (var value in MultiChoiceResults)
                        {
                            if (value != null && string.Equals(itemKey, value.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                exists = true;
                                break;
                            }
                        }
                    }
                    
                    if (!exists)
                    {
                        // 添加原始类型的值
                        MultiChoiceResults.Add(objectSelection.Item.Key);
                        needTextUpdate = true;
                    }
                }
                else
                {
                    // 查找并移除对应的key值
                    int removeIndex = -1;
                    for (int i = 0; i < MultiChoiceResults.Count; i++)
                    {
                        var value = MultiChoiceResults[i];
                        if (value != null && string.Equals(itemKey, value.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            removeIndex = i;
                            break;
                        }
                    }
                    
                    if (removeIndex >= 0)
                    {
                        MultiChoiceResults.RemoveAt(removeIndex);
                        needTextUpdate = true;
                    }
                }
            }
            else
            {
                // 处理非ObjectSelectionWrapper类型的数据
                object comboBoxItem = changedItem.ComboBoxItem;
                if (changedItem.Checked)
                {
                    if (!MultiChoiceResults.Contains(comboBoxItem))
                    {
                        MultiChoiceResults.Add(comboBoxItem);
                        needTextUpdate = true;
                    }
                }
                else
                {
                    if (MultiChoiceResults.Contains(comboBoxItem))
                    {
                        MultiChoiceResults.Remove(comboBoxItem);
                        needTextUpdate = true;
                    }
                }
            }

            // 只有当数据发生变化时才更新UI文本
            if (needTextUpdate)
            {
                string ListText = GetCSVText(true);

                // The DropDownList style seems to require that the text
                // part of the "textbox" should match a single item.
                if (DropDownStyle != ComboBoxStyle.DropDownList)
                    Text = ListText;
                // This refreshes the Text of the first item (which is not visible)
                else if (DataSource == null)
                {
                    Items[0] = ListText;
                    // Keep the hidden item and first checkbox item in 
                    // sync in order to ensure the Synchronise process
                    // can match the items.
                    CheckBoxItems[0].ComboBoxItem = ListText;
                }
            }

            // 触发事件
            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// Will add an invisible item when the style is DropDownList,
        /// to help maintain the correct text in main TextBox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDownStyleChanged(EventArgs e)
        {
            base.OnDropDownStyleChanged(e);
            // 只有当DataSource为null时才设置_MustAddHiddenItem
            if (DropDownStyle == ComboBoxStyle.DropDownList
                && DataSource == null
                && !DesignMode)
                _MustAddHiddenItem = true;
            else
                _MustAddHiddenItem = false; // 其他情况重置标志
        }

        protected override void OnResize(EventArgs e)
        {
            // When the ComboBox is resized, the width of the dropdown 
            // is also resized to match the width of the ComboBox. I think it looks better.
            Size Size = new Size(Width, DropDownControl.Height);
            dropDown.Size = Size;
            base.OnResize(e);
        }

        #endregion

        #region PUBLIC OPERATIONS

        /// <summary>
        /// A function to clear/reset the list.
        /// (Ubiklou : http://www.codeproject.com/KB/combobox/extending_combobox.aspx?msg=2526813#xx2526813xx)
        /// </summary>
        public void Clear()
        {
            if (DataSource == null)
            {
                this.Items.Clear();
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                    _MustAddHiddenItem = true;
            }
            else
            {
                // 当使用DataSource时，使用原有的ClearSelection方法清空选择
                // 不直接操作Items集合以避免"设置DataSource属性后无法修改项集合"异常
                this.ClearSelection();
            }
        }        /// <summary>
                 /// Uncheck all items.
                 /// </summary>
        public void ClearSelection()
        {
            foreach (CheckBoxComboBoxItem Item in CheckBoxItems)
                if (Item.Checked)
                    Item.Checked = false;
        }

        /// <summary>
        /// 根据MultiChoiceResults更新所有CheckBox的选中状态
        /// 处理不同类型值的比较问题
        /// 使用哈希表优化性能，减少重复比较操作
        /// 特别针对大量数据（如200个选中项）进行了性能优化
        /// </summary>
        public void UpdateCheckedStates()
        {
            // 快速路径：如果控件已释放，直接返回
            if (IsDisposed)
            {
                return;
            }
            
            // 快速路径：如果没有选项或没有选中项，直接返回
            if (CheckBoxItems.Count == 0)
            {
                return;
            }
            
            bool hasSelectedItems = MultiChoiceResults.Count > 0;
            
            // 快速路径：如果没有选中项，快速取消所有选中状态
            if (!hasSelectedItems)
            {
                // 批量禁用UI更新，提高性能
                BeginUpdate();
                try
                {
                    bool hasChanges = false;
                    foreach (CheckBoxComboBoxItem cbItem in CheckBoxItems)
                    {
                        if (cbItem.Checked)
                        { 
                            cbItem.Checked = false;
                            hasChanges = true;
                        }
                    }
                    
                    // 只有在实际有更改时才更新文本
                    if (hasChanges && DropDownStyle != ComboBoxStyle.DropDownList)
                    {
                        Text = string.Empty;
                    }
                }
                finally
                {
                    EndUpdate();
                }
                return;
            }
            
            // 无论数据量大小，始终使用哈希表以确保最佳性能 - 特别是对于大量数据
            HashSet<string> resultValueHashSet = new HashSet<string>(MultiChoiceResults.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var value in MultiChoiceResults)
            {
                if (value != null)
                {
                    resultValueHashSet.Add(value.ToString());
                }
            }
            
            // 对于非包装类型，使用另一个哈希表来优化查找
            HashSet<object> nonWrappedItems = null;
            if (MultiChoiceResults.Count > 10) // 只有当数据量达到一定规模时才创建
            {
                nonWrappedItems = new HashSet<object>();
                foreach (var value in MultiChoiceResults)
                {
                    if (value != null && !(value is string) && !(value is ValueType))
                    {
                        nonWrappedItems.Add(value);
                    }
                }
            }
            
            // 批量更新减少UI刷新次数
            BeginUpdate();
            try
            {
                bool hasChanges = false;
                
                foreach (CheckBoxComboBoxItem cbItem in CheckBoxItems)
                {
                    bool isChecked = false;
                    
                    // 处理ObjectSelectionWrapper<CmbChkItem>类型
                    ObjectSelectionWrapper<CmbChkItem> wrapper = cbItem.ComboBoxItem as ObjectSelectionWrapper<CmbChkItem>;
                    if (wrapper != null && wrapper.Item != null && !string.IsNullOrEmpty(wrapper.Item.Key))
                    {
                        string itemKey = wrapper.Item.Key.ToString();
                        // 使用哈希表快速查找 - O(1)复杂度
                        isChecked = resultValueHashSet.Contains(itemKey);
                    }
                    else
                    {
                        object comboBoxItem = cbItem.ComboBoxItem;
                        if (comboBoxItem != null)
                        {
                            // 首先尝试使用哈希表查找
                            if (nonWrappedItems != null && nonWrappedItems.Contains(comboBoxItem))
                            {
                                isChecked = true;
                            }
                            // 对于字符串和值类型，使用字符串哈希表
                            else if (comboBoxItem is string || comboBoxItem is ValueType)
                            {
                                isChecked = resultValueHashSet.Contains(comboBoxItem.ToString());
                            }
                            // 最后才使用List.Contains
                            else
                            {
                                isChecked = MultiChoiceResults.Contains(comboBoxItem);
                            }
                        }
                    }
                    
                    // 只有状态改变时才更新，减少不必要的UI更新和事件触发
                    if (cbItem.Checked != isChecked)
                    {
                        cbItem.Checked = isChecked;
                        hasChanges = true;
                    }
                }
                
                // 只有在实际有更改时才更新文本
                if (hasChanges && DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    Text = GetCSVText(true);
                }
            }
            catch (Exception ex)
            {
                // 添加错误处理，确保即使出现异常也能正确结束更新
                System.Diagnostics.Debug.WriteLine($"Error in UpdateCheckedStates: {ex.Message}");
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region CHECKBOX PROPERTIES (DEFAULTS)

        private CheckBoxProperties _CheckBoxProperties;

        /// <summary>
        /// The properties that will be assigned to the checkboxes as default values.
        /// </summary>
        [Description("The properties that will be assigned to the checkboxes as default values.")]
        [Browsable(true)]
        public CheckBoxProperties CheckBoxProperties
        {
            get { return _CheckBoxProperties; }
            set { _CheckBoxProperties = value; _CheckBoxProperties_PropertyChanged(this, EventArgs.Empty); }
        }

        private void _CheckBoxProperties_PropertyChanged(object sender, EventArgs e)
        {
            foreach (CheckBoxComboBoxItem Item in CheckBoxItems)
                Item.ApplyProperties(CheckBoxProperties);
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            // 323 : Item Added
            // 331 : Clearing
            if (m.Msg == 331
                && DropDownStyle == ComboBoxStyle.DropDownList
                && DataSource == null)
            {
                _MustAddHiddenItem = true;
            }
            else if (m.Msg == 331)
            {
                _MustAddHiddenItem = false; // 当DataSource不为null时重置标志
            }

            base.WndProc(ref m);
        }
    }

    /// <summary>
    /// A container control for the ListControl to ensure the ScrollBar on the ListControl does not
    /// Paint over the Size grip. Setting the Padding or Margin on the Popup or host control does
    /// not work as I expected.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControlContainer : UserControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControlContainer()
            : base()
        {
            BackColor = SystemColors.Window;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Inherit;
            ResizeRedraw = true;
            // If you don't set this, then resize operations cause an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }
        #endregion

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        /// <summary>
        /// Prescribed by the Popup class to ensure Resize operations work correctly.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if ((Parent as Popup).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }
        #endregion
    }

    /// <summary>
    /// This ListControl that pops up to the User. It contains the CheckBoxComboBoxItems. 
    /// The items are docked DockStyle.Top in this control.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControl : ScrollableControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControl(CheckBoxComboBox owner)
            : base()
        {
            DoubleBuffered = true;
            _CheckBoxComboBox = owner;
            _Items = new CheckBoxComboBoxItemList(_CheckBoxComboBox);
            BackColor = SystemColors.Window;
            // AutoScaleMode = AutoScaleMode.Inherit;
            AutoScroll = true;
            ResizeRedraw = true;
            // if you don't set this, a Resize operation causes an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }



        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// Simply a reference to the CheckBoxComboBox.
        /// </summary>
        private CheckBoxComboBox _CheckBoxComboBox;



        /// <summary>
        /// A Typed list of ComboBoxCheckBoxItems.
        /// </summary>
        private CheckBoxComboBoxItemList _Items;

        #endregion

        public CheckBoxComboBoxItemList Items { get { return _Items; } }

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        /// <summary>
        /// Prescribed by the Popup control to enable Resize operations.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if ((Parent.Parent as Popup).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnVisibleChanged(EventArgs e)
        {
            // Synchronises the CheckBox list with the items in the ComboBox.
            SynchroniseControlsWithComboBoxItems();
            base.OnVisibleChanged(e);
        }
        /// <summary>
        /// Maintains the controls displayed in the list by keeping them in sync with the actual 
        /// items in the combobox. (e.g. removing and adding as well as ordering)
        /// </summary>
        public void SynchroniseControlsWithComboBoxItems()
        {
            SuspendLayout();
            // 只有当DataSource为null时才尝试修改Items集合
            if (_CheckBoxComboBox._MustAddHiddenItem && _CheckBoxComboBox.DataSource == null)
            {
                _CheckBoxComboBox.Items.Insert(
                    0, _CheckBoxComboBox.GetCSVText(false)); // INVISIBLE ITEM
                _CheckBoxComboBox.SelectedIndex = 0;
                _CheckBoxComboBox._MustAddHiddenItem = false;
            }

            // 检查是否需要完全重建控件列表
            bool needsRebuild = false;
            
            // 检查现有项是否与ComboBox项匹配
            if (_Items.Count != _CheckBoxComboBox.Items.Count)
            {
                needsRebuild = true;
            }
            else
            {
                for (int i = 0; i < _Items.Count; i++)
                {
                    if (_Items[i].ComboBoxItem != _CheckBoxComboBox.Items[i])
                    {
                        needsRebuild = true;
                        break;
                    }
                }
            }

            if (needsRebuild)
            {
                // 只有在需要时才清除和重建控件
                Controls.Clear();
                
                #region Disposes all items that are no longer in the combo box list

                for (int Index = _Items.Count - 1; Index >= 0; Index--)
                {
                    CheckBoxComboBoxItem Item = _Items[Index];
                    if (!_CheckBoxComboBox.Items.Contains(Item.ComboBoxItem))
                    {
                        _Items.Remove(Item);
                        Item.Dispose();
                    }
                }

                #endregion
                
                #region Recreate the list in the same order of the combo box items

                bool HasHiddenItem =
                    _CheckBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList
                    && _CheckBoxComboBox.DataSource == null
                    && !DesignMode;

                CheckBoxComboBoxItemList NewList = new CheckBoxComboBoxItemList(_CheckBoxComboBox);
                for (int Index0 = 0; Index0 <= _CheckBoxComboBox.Items.Count - 1; Index0++)
                {
                    object Object = _CheckBoxComboBox.Items[Index0];
                    CheckBoxComboBoxItem Item = null;
                    // The hidden item could match any other item when only
                    // one other item was selected.
                    if (Index0 == 0 && HasHiddenItem && _Items.Count > 0)
                        Item = _Items[0];
                    else
                    {
                        int StartIndex = HasHiddenItem
                            ? 1 // Skip the hidden item, it could match 
                            : 0;
                        for (int Index1 = StartIndex; Index1 <= _Items.Count - 1; Index1++)
                        {
                            if (_Items[Index1].ComboBoxItem == Object)
                            {
                                Item = _Items[Index1];
                                break;
                            }
                        }
                    }
                    if (Item == null)
                    {
                        Item = new CheckBoxComboBoxItem(_CheckBoxComboBox, Object);
                        Item.ApplyProperties(_CheckBoxComboBox.CheckBoxProperties);
                    }
                    NewList.Add(Item);
                    Item.Dock = DockStyle.Top;
                }
                _Items.Clear();
                _Items.AddRange(NewList);

                #endregion
                
                #region Add the items to the controls in reversed order to maintain correct docking order

                if (NewList.Count > 0)
                {
                    // This reverse helps to maintain correct docking order.
                    NewList.Reverse();
                    // If you get an error here that "Cannot convert to the desired 
                    // type, it probably means the controls are not binding correctly.
                    // The Checked property is binded to the ValueMember property. 
                    // It must be a bool for example.
                    //如果您在此处收到错误“无法转换为所需
                    //类型，这可能意味着控件绑定不正确。
                    //Checked属性绑定到ValueMember属性。
                    //例如，它一定是一个bool。
                    Controls.AddRange(NewList.ToArray());
                }

                #endregion
            }

            // Keep the first item invisible
            if (_CheckBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList
                && _CheckBoxComboBox.DataSource == null
                && !DesignMode)
                _CheckBoxComboBox.CheckBoxItems[0].Visible = false;

            // 同步完成后，更新CheckBox的选中状态
            _CheckBoxComboBox.UpdateCheckedStates();

            ResumeLayout();
        }

        #endregion
    }

    /// <summary>
    /// The CheckBox items displayed in the Popup of the ComboBox.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItem : CheckBox
    {
        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">A reference to the CheckBoxComboBox.</param>
        /// <param name="comboBoxItem">A reference to the item in the ComboBox.Items that this object is extending.</param>
        public CheckBoxComboBoxItem(CheckBoxComboBox owner, object comboBoxItem)
            : base()
        {
            DoubleBuffered = true;
            _CheckBoxComboBox = owner;
            _ComboBoxItem = comboBoxItem;
            if (_CheckBoxComboBox.DataSource != null)
                AddBindings();
            else
                // 检查comboBoxItem是否为null，避免空引用异常
                Text = comboBoxItem?.ToString() ?? "";

            // 检查该项是否应该被选中
            ObjectSelectionWrapper<CmbChkItem> wrapper = comboBoxItem as ObjectSelectionWrapper<CmbChkItem>;
            if (wrapper != null)
            {
                // 检查wrapper.Item是否为null
                if (wrapper.Item != null)
                {
                    Checked = _CheckBoxComboBox.MultiChoiceResults.Contains(wrapper.Item.Key);
                }
            }
            else
            {
                Checked = _CheckBoxComboBox.MultiChoiceResults.Contains(comboBoxItem);
            }
        }
        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// A reference to the CheckBoxComboBox.
        /// </summary>
        private CheckBoxComboBox _CheckBoxComboBox;
        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        private object _ComboBoxItem;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        public object ComboBoxItem
        {
            get { return _ComboBoxItem; }
            internal set { _ComboBoxItem = value; }
        }

        #endregion

        #region BINDING HELPER

        /// <summary>
        /// When using Data Binding operations via the DataSource property of the ComboBox. This
        /// adds the required Bindings for the CheckBoxes.
        /// </summary>
        public void AddBindings()
        {
            // Note, the text uses "DisplayMemberSingleItem", not "DisplayMember" (unless its not assigned)
            DataBindings.Add(
                "Text",
                _ComboBoxItem,
                _CheckBoxComboBox.DisplayMemberSingleItem
                );
            // The ValueMember must be a bool type property usable by the CheckBox.Checked.
            DataBindings.Add(
                "Checked",
                _ComboBoxItem,
                _CheckBoxComboBox.ValueMember,
                false,
                // This helps to maintain proper selection state in the Binded object,
                // even when the controls are added and removed.
                DataSourceUpdateMode.OnPropertyChanged,
                false, null, null);
            // Helps to maintain the Checked status of this
            // checkbox before the control is visible
            if (_ComboBoxItem is INotifyPropertyChanged)
                ((INotifyPropertyChanged)_ComboBoxItem).PropertyChanged +=
                    new PropertyChangedEventHandler(
                        CheckBoxComboBoxItem_PropertyChanged);
        }

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnCheckedChanged(EventArgs e)
        {
            // Found that when this event is raised, the bool value of the binded item is not yet updated.
            if (_CheckBoxComboBox.DataSource != null)
            {
                PropertyInfo PI = ComboBoxItem.GetType().GetProperty(_CheckBoxComboBox.ValueMember);
                PI.SetValue(ComboBoxItem, Checked, null);
            }
            base.OnCheckedChanged(e);
            // Forces a refresh of the Text displayed in the main TextBox of the ComboBox,
            // since that Text will most probably represent a concatenation of selected values.
            // Also see DisplayMemberSingleItem on the CheckBoxComboBox for more information.
            if (_CheckBoxComboBox.DataSource != null)
            {
                string OldDisplayMember = _CheckBoxComboBox.DisplayMember;
                _CheckBoxComboBox.DisplayMember = null;
                _CheckBoxComboBox.DisplayMember = OldDisplayMember;
            }
        }

        #endregion

        #region HELPER MEMBERS

        internal void ApplyProperties(CheckBoxProperties properties)
        {
            this.Appearance = properties.Appearance;
            this.AutoCheck = properties.AutoCheck;
            this.AutoEllipsis = properties.AutoEllipsis;
            this.AutoSize = properties.AutoSize;
            this.CheckAlign = properties.CheckAlign;
            this.FlatAppearance.BorderColor = properties.FlatAppearanceBorderColor;
            this.FlatAppearance.BorderSize = properties.FlatAppearanceBorderSize;
            this.FlatAppearance.CheckedBackColor = properties.FlatAppearanceCheckedBackColor;
            this.FlatAppearance.MouseDownBackColor = properties.FlatAppearanceMouseDownBackColor;
            this.FlatAppearance.MouseOverBackColor = properties.FlatAppearanceMouseOverBackColor;
            this.FlatStyle = properties.FlatStyle;
            this.ForeColor = properties.ForeColor;
            this.RightToLeft = properties.RightToLeft;
            this.TextAlign = properties.TextAlign;
            this.ThreeState = properties.ThreeState;
        }

        #endregion

        #region EVENT HANDLERS - ComboBoxItem (DataSource)

        /// <summary>
        /// Added this handler because the control doesn't seem 
        /// to initialize correctly until shown for the first
        /// time, which also means the summary text value
        /// of the combo is out of sync initially.
        /// </summary>
        private void CheckBoxComboBoxItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _CheckBoxComboBox.ValueMember)
                Checked =
                    (bool)_ComboBoxItem
                        .GetType()
                        .GetProperty(_CheckBoxComboBox.ValueMember)
                        .GetValue(_ComboBoxItem, null);
        }

        #endregion
    }

    /// <summary>
    /// A Typed List of the CheckBox items.
    /// Simply a wrapper for the CheckBoxComboBox.Items. A list of CheckBoxComboBoxItem objects.
    /// This List is automatically synchronised with the Items of the ComboBox and extended to
    /// handle the additional boolean value. That said, do not Add or Remove using this List, 
    /// it will be lost or regenerated from the ComboBox.Items.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItemList : List<CheckBoxComboBoxItem>
    {
        #region CONSTRUCTORS

        public CheckBoxComboBoxItemList(CheckBoxComboBox checkBoxComboBox)
        {
            _CheckBoxComboBox = checkBoxComboBox;
        }

        #endregion

        #region PRIVATE FIELDS

        private CheckBoxComboBox _CheckBoxComboBox;

        #endregion

        #region EVENTS, This could be moved to the list control if needed

        public event EventHandler CheckBoxCheckedChanged;

        protected void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
                handler(sender, e);
        }
        private void item_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxCheckedChanged(sender, e);
        }

        #endregion

        #region LIST MEMBERS & OBSOLETE INDICATORS

        [Obsolete("Do not add items to this list directly. Use the ComboBox items instead.", false)]
        public new void Add(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<CheckBoxComboBoxItem> collection)
        {
            foreach (CheckBoxComboBoxItem Item in collection)
                Item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.AddRange(collection);
        }

        public new void Clear()
        {
            foreach (CheckBoxComboBoxItem Item in this)
                Item.CheckedChanged -= item_CheckedChanged;
            base.Clear();
        }

        [Obsolete("Do not remove items from this list directly. Use the ComboBox items instead.", false)]
        public new bool Remove(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged -= item_CheckedChanged;
            return base.Remove(item);
        }

        #endregion

        #region DEFAULT PROPERTIES

        /// <summary>
        /// Returns the item with the specified displayName or Text.
        /// </summary>
        public CheckBoxComboBoxItem this[string displayName]
        {
            get
            {
                int StartIndex =
                    // An invisible item exists in this scenario to help 
                    // with the Text displayed in the TextBox of the Combo
                    _CheckBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList
                    && _CheckBoxComboBox.DataSource == null
                        ? 1 // Ubiklou : 2008-04-28 : Ignore first item. (http://www.codeproject.com/KB/combobox/extending_combobox.aspx?fid=476622&df=90&mpp=25&noise=3&sort=Position&view=Quick&select=2526813&fr=1#xx2526813xx)
                        : 0;
                for (int Index = StartIndex; Index <= Count - 1; Index++)
                {
                    CheckBoxComboBoxItem Item = this[Index];

                    string Text;
                    // The binding might not be active yet
                    if (string.IsNullOrEmpty(Item.Text)
                        // Ubiklou : 2008-04-28 : No databinding
                        && Item.DataBindings != null
                        && Item.DataBindings["Text"] != null
                        )
                    {
                        PropertyInfo PropertyInfo
                            = Item.ComboBoxItem.GetType().GetProperty(
                                Item.DataBindings["Text"].BindingMemberInfo.BindingMember);
                        Text = (string)PropertyInfo.GetValue(Item.ComboBoxItem, null);
                    }
                    else
                        Text = Item.Text;
                    if (Text.CompareTo(displayName) == 0)
                        return Item;
                }
                throw new ArgumentOutOfRangeException(String.Format("\"{0}\" does not exist in this combo box.", displayName));
            }
        }

        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckBoxProperties
    {
        public CheckBoxProperties() { }

        #region PRIVATE PROPERTIES

        private Appearance _Appearance = Appearance.Normal;
        private bool _AutoSize = false;
        private bool _AutoCheck = true;
        private bool _AutoEllipsis = false;
        private ContentAlignment _CheckAlign = ContentAlignment.MiddleLeft;
        private Color _FlatAppearanceBorderColor = Color.Empty;
        private int _FlatAppearanceBorderSize = 1;
        private Color _FlatAppearanceCheckedBackColor = Color.Empty;
        private Color _FlatAppearanceMouseDownBackColor = Color.Empty;
        private Color _FlatAppearanceMouseOverBackColor = Color.Empty;
        private FlatStyle _FlatStyle = FlatStyle.Standard;
        private Color _ForeColor = SystemColors.ControlText;
        private RightToLeft _RightToLeft = RightToLeft.No;
        private ContentAlignment _TextAlign = ContentAlignment.MiddleLeft;
        private bool _ThreeState = false;

        #endregion

        #region PUBLIC PROPERTIES

        [DefaultValue(Appearance.Normal)]
        public Appearance Appearance
        {
            get { return _Appearance; }
            set { _Appearance = value; OnPropertyChanged(); }
        }
        [DefaultValue(true)]
        public bool AutoCheck
        {
            get { return _AutoCheck; }
            set { _AutoCheck = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoEllipsis
        {
            get { return _AutoEllipsis; }
            set { _AutoEllipsis = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return _AutoSize; }
            set { _AutoSize = true; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment CheckAlign
        {
            get { return _CheckAlign; }
            set { _CheckAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceBorderColor
        {
            get { return _FlatAppearanceBorderColor; }
            set { _FlatAppearanceBorderColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(1)]
        public int FlatAppearanceBorderSize
        {
            get { return _FlatAppearanceBorderSize; }
            set { _FlatAppearanceBorderSize = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceCheckedBackColor
        {
            get { return _FlatAppearanceCheckedBackColor; }
            set { _FlatAppearanceCheckedBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseDownBackColor
        {
            get { return _FlatAppearanceMouseDownBackColor; }
            set { _FlatAppearanceMouseDownBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseOverBackColor
        {
            get { return _FlatAppearanceMouseOverBackColor; }
            set { _FlatAppearanceMouseOverBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle
        {
            get { return _FlatStyle; }
            set { _FlatStyle = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(RightToLeft.No)]
        public RightToLeft RightToLeft
        {
            get { return _RightToLeft; }
            set { _RightToLeft = value; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool ThreeState
        {
            get { return _ThreeState; }
            set { _ThreeState = value; OnPropertyChanged(); }
        }

        #endregion

        #region EVENTS AND EVENT CALLERS

        /// <summary>
        /// Called when any property changes.
        /// </summary>
        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged()
        {
            EventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
