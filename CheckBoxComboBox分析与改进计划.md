# CheckBoxComboBox控件分析与改进计划

## 一、问题描述

用户在使用自定义的CheckBoxComboBox控件时遇到以下异常：

```
设置 DataSource 属性后无法修改项集合。
   at System.Windows.Forms.ComboBox.CheckNoDataSource() in System.Windows.Forms\ComboBox.cs:line 3371
   at System.Windows.Forms.ComboBox.ObjectCollection.Insert(Int32 index, Object item) in System.Windows.Forms\ComboBox.cs:line 408
   at RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems() in E:\CodeRepository\SynologyDrive\RUINORERP\RUINOR.WinFormsUI\ChkComboBox\CheckBoxComboBox.cs:line 528
   at RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox.set_ValueMember(String value) in E:\CodeRepository\SynologyDrive\RUINORERP\RUINOR.WinFormsUI\ChkComboBox\CheckBoxComboBox.cs:line 184
   at RUINORERP.UI.Common.DataBindingHelper.InitDataToCmbChkWithCondition[T](String key, String value, String tableName, CheckBoxComboBox cmbBox, Expression`1 expCondition) in E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\Common\DataBindingHelper.cs:line 3873
   at RUINORERP.UI.Common.DataBindingHelper.BindData4CmbChkRefWithLimited[T](Object entity, String expkey, String expValue, String tableName, CheckBoxComboBox cmbBox, Expression`1 expCondition) in E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\Common\DataBindingHelper.cs:line 2267
```

## 二、问题分析

通过代码分析，我发现问题出在CheckBoxComboBox控件的以下几个方面：

### 1. 核心问题

当设置了CheckBoxComboBox的DataSource属性后，代码仍然尝试修改其Items集合，这违反了WinForms控件的基本规则：**设置DataSource后不能直接修改Items集合**。

### 2. 问题产生流程

1. `InitDataToCmbChkWithCondition`方法中设置了DataSource：
   ```csharp
   ListSelectionWrapper<CmbChkItem> selectionWrappers = new ListSelectionWrapper<CmbChkItem>(cmbItems, "Name");
   cmbBox.BeginUpdate();
   cmbBox.DataSource = selectionWrappers;
   ```

2. 随后设置ValueMember属性：
   ```csharp
   cmbBox.ValueMember = "Selected";
   ```

3. 在`set_ValueMember`方法中调用了`SynchroniseControlsWithComboBoxItems`：
   ```csharp
   public new string ValueMember
   {
       get { return base.ValueMember; }
       set
       {
           base.ValueMember = value;
           if (!string.IsNullOrEmpty(ValueMember))
               _CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
       }
   }
   ```

4. 在`SynchroniseControlsWithComboBoxItems`方法中，当`_CheckBoxComboBox._MustAddHiddenItem`为true时，尝试修改Items集合：
   ```csharp
   if (_CheckBoxComboBox._MustAddHiddenItem)
   {
       _CheckBoxComboBox.Items.Insert(
           0, _CheckBoxComboBox.GetCSVText(false)); // INVISIBLE ITEM
       _CheckBoxComboBox.SelectedIndex = 0;
       _CheckBoxComboBox._MustAddHiddenItem = false;
   }
   ```

5. 此时由于已经设置了DataSource，直接修改Items集合导致异常。

### 3. _MustAddHiddenItem标志的设置逻辑

`_MustAddHiddenItem`标志在以下情况下会被设置为true：

1. 当ComboBoxStyle改为DropDownList且DataSource为null且非设计模式时：
   ```csharp
   protected override void OnDropDownStyleChanged(EventArgs e)
   {
       base.OnDropDownStyleChanged(e);
       if (DropDownStyle == ComboBoxStyle.DropDownList
           && DataSource == null
           && !DesignMode)
           _MustAddHiddenItem = true;
   }
   ```

2. 当调用Clear()方法且ComboBoxStyle为DropDownList且DataSource为null时：
   ```csharp
   public void Clear()
   {
       this.Items.Clear();
       if (DropDownStyle == ComboBoxStyle.DropDownList && DataSource == null)
           _MustAddHiddenItem = true;
   }
   ```

3. 通过WndProc处理特定消息时（331表示清除操作）：
   ```csharp
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
       base.WndProc(ref m);
   }
   ```

问题在于，这些条件检查中虽然都包含了`DataSource == null`的判断，但在设置DataSource后，没有代码负责重置`_MustAddHiddenItem`标志，导致即使设置了DataSource，该标志仍可能为true。

## 三、改进计划

### 1. 修改CheckBoxComboBox类

#### 1.1 修改set_DataSource方法

在设置DataSource时，重置_MustAddHiddenItem标志，并确保不再尝试修改Items集合：

```csharp
public new object DataSource
{
    get { return base.DataSource; }
    set
    {
        base.DataSource = value;
        // 当设置DataSource时，重置_MustAddHiddenItem标志
        _MustAddHiddenItem = false;
        if (!string.IsNullOrEmpty(ValueMember))
            _CheckBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
    }
}
```

#### 1.2 修改SynchroniseControlsWithComboBoxItems方法

在修改Items集合前，增加DataSource检查：

```csharp
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
    
    // 其余代码保持不变
    // ...
}
```

### 2. 修改OnDropDownStyleChanged和Clear方法

确保在这些方法中也正确处理DataSource的情况：

```csharp
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
        // 当使用DataSource时，通过其他方式清空选择
        ClearSelection();
    }
}
```

### 3. 优化InitDataToCmbChkWithCondition方法

在DataBindingHelper类中，改进数据源绑定逻辑：

```csharp
public static void InitDataToCmbChkWithCondition<T>(string key, string value, string tableName, CheckBoxComboBox cmbBox, Expression<Func<T, bool>> expCondition) where T : class
{
    // 原有代码...
    
    List<CmbChkItem> cmbItems = new List<CmbChkItem>();
    foreach (var item in Newlist)
    {
        cmbItems.Add(new CmbChkItem(item.GetPropertyValue(key).ToString(), item.GetPropertyValue(value).ToString()));
    }

    ListSelectionWrapper<CmbChkItem> selectionWrappers = new ListSelectionWrapper<CmbChkItem>(cmbItems, "Name");
    
    cmbBox.BeginUpdate();
    try
    {
        // 先设置DisplayMember和ValueMember，再设置DataSource
        cmbBox.DisplayMemberSingleItem = "Name";
        cmbBox.DisplayMember = "NameConcatenated";
        cmbBox.ValueMember = "Selected";
        // 最后设置DataSource，避免触发不必要的SynchroniseControlsWithComboBoxItems调用
        cmbBox.DataSource = selectionWrappers;
        cmbBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
    }
    finally
    {
        cmbBox.EndUpdate();
    }
}
```

### 4. 添加注释和改进文档

在CheckBoxComboBox类中添加详细的注释，说明控件的特殊用途和使用注意事项：

```csharp
/// <summary>
/// 自定义多选下拉框控件，支持多选操作
/// 特别说明：
/// 1. 该控件有一个特殊的MultiChoiceResults属性，用于保存选中的数据项集合
/// 2. 当设置DataSource属性时，不能同时直接操作Items集合
/// 3. 建议通过BindData4CmbChkRefWithLimited方法进行数据绑定，以确保正确初始化
/// </summary>
public partial class CheckBoxComboBox : PopupComboBox
{
    // 类实现...
}
```

## 四、预期效果

1. 解决"设置DataSource属性后无法修改项集合"的异常
2. 确保在使用DataSource的情况下，控件仍能正常工作
3. 改进控件的健壮性和可用性
4. 提供清晰的注释和使用指南

## 五、测试计划

1. 使用BindData4CmbChkRefWithLimited方法绑定数据源，验证是否能正常工作
2. 测试在设置DataSource后，控件的下拉显示和选中功能是否正常
3. 测试各种组合情况下的行为，如更改DropDownStyle、调用Clear方法等
4. 验证MultiChoiceResults属性是否能正确保存和同步选中的项

## 六、风险评估

1. **兼容性风险**：修改可能会影响现有代码的行为，特别是依赖于当前实现细节的代码
2. **性能风险**：额外的检查可能会略微影响性能，但影响应该很小
3. **功能风险**：需要确保修改不会破坏控件的核心功能，如多选、显示选中项等

---

以上分析和改进计划旨在解决CheckBoxComboBox控件在使用DataSource时出现的异常问题，通过合理的代码修改和优化，确保控件在各种场景下都能正常工作。