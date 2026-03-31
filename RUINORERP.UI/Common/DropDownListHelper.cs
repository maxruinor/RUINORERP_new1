// ----------------------------------------------------------------
// Description : 用于初始化UI层各种下拉列表
// Author      : watson
// Create date : 创建日期 格式：2009-09-17
// Modify date : 
// Modify desc : 
// ----------------------------------------------------------------
using Krypton.Toolkit;
using RUINORERP.Common.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using RUINOR.WinFormsUI.ChkComboBox;
using System.Web.UI.WebControls;


namespace RUINORERP.UI.Common
{
    public class ComboBoxHelper
    {


        //=======

        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        public static void InitDropListComboxDataBand(DataSet ds, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle)
        {
            cmb.BeginUpdate();
            cmb.DataSource = ds.Tables[0];
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;

            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sc.Add(dr[DisplayMember].ToString());
            }

            if (DropStyle == ComboBoxStyle.DropDown)
            {
                // 完全禁用 AutoComplete，避免干扰自定义智能搜索
                // AutoComplete 会自动选择第一个匹配项，导致下拉立即关闭
                // cmb.AutoCompleteCustomSource = sc;
                // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }

            cmb.EndUpdate();


            cmb.SelectedIndex = -1;



        }


        public static void InitDropListForDictionary(Dictionary<string, string> list, System.Windows.Forms.ComboBox cmbox, int defaultIndex, bool ShowKey)
        {
            if (list != null)
            {
                cmbox.Items.Clear();
                cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
                string currentValue;
                string currentName;

                //cmbox.DisplayMember = DisplayMember;
                // cmbox.ValueMember = ValueMember;

                if (ShowKey)
                {
                    foreach (KeyValuePair<string, string> kv in list)
                    {
                        // cmbox.Items.Add(kv.Key + "|" + kv.Value);
                        CmbItem item;
                        currentValue = kv.Key;
                        currentName = kv.Value;
                        item = new CmbItem(currentValue, currentName);
                        cmbox.Items.Add(item);
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> kv in list)
                    {
                        CmbItem item;
                        currentValue = kv.Key;
                        currentName = kv.Value;
                        item = new CmbItem(currentValue, currentName);
                        cmbox.Items.Add(item);
                        //cmbox.Items.Add(item);
                    }
                }
            }
            cmbox.SelectedIndex = defaultIndex;
        }




        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        public static void InitDropList(DataSet ds, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            cmb.BeginUpdate();
            cmb.DataSource = ds.Tables[0];
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                sc.Add(dr[DisplayMember].ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {
                    // 禁用 AutoComplete，避免干扰自定义智能搜索
                    // cmb.AutoCompleteCustomSource = sc;
                    // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            cmb.BeginUpdate();
            cmb.SelectedIndex = -1;
        }




        /*
        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        /// <param name="add请选择">  如果数据源为键值对形式，则在数据源的最前面加入("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, KryptonComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add请选择)
        {
            cmb.BeginUpdate();
            cmb.DataBindings.Clear();
            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();

            foreach (var dr in bs.List)
            {
                // sc.Add(dr[DisplayMember].ToString());
                sc.Add(ReflectionHelper.GetPropertyValue(dr, DisplayMember));
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {
                    // 禁用 AutoComplete，避免干扰自定义智能搜索
                    // cmb.AutoCompleteCustomSource = sc;
                    // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.EndUpdate();

            cmb.SelectedIndex = -1;
        }
        */

        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        /// <param name="add请选择">  如果数据源为键值对形式，则在数据源的最前面加入("请选择", "-1")</param>
        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="bs">数据源绑定源</param>
        /// <param name="cmb">Krypton下拉列表控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        public static void InitDropList(BindingSource bs, KryptonComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            // 检查是否需要在UI线程上执行
            if (cmb.InvokeRequired)
            {
                cmb.Invoke(new Action(() => InitDropList(bs, cmb, ValueMember, DisplayMember, DropStyle, auto)));
                return;
            }

            // 根据参数决定是否启用智能搜索功能
            // auto=true: 启用搜索，支持中文、拼音、模糊匹配（适用于业务员、产品等需要搜索的场景）
            // auto=false: 不启用搜索（适用于枚举、固定选项等场景）
            cmb.EnableSearch = auto;

            // 设置搜索延迟时间（仅在启用搜索时生效）
            if (auto)
            {
                cmb.SearchDelay = 300; // 设置300ms延迟以获得更好的响应速度
            }

            cmb.BeginUpdate();

            // 只有在启用搜索时才修改 DropStyle，否则使用传入的 DropStyle
            // 搜索功能需要用户能够输入文本，所以必须使用 DropDown
            if (cmb.EnableSearch)
            {
                cmb.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else
            {
                cmb.DropDownStyle = DropStyle; // 使用传入的 DropStyle（如 DropDownList）
            }

            // 仅在数据源不为空时构建自动完成集合
            // 注意：auto 参数现在用于控制是否启用搜索，这里只是为了兼容性保留
            if (auto && cmb.DropDownStyle == ComboBoxStyle.DropDown && bs != null && bs.List != null)
            {
                AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
                
                // 缓存反射属性信息以提高性能
                var type = bs.List.Count > 0 ? bs.List[0].GetType() : null;
                System.Reflection.PropertyInfo propInfo = type?.GetProperty(DisplayMember);
                
                foreach (var item in bs.List)
                {
                    if (propInfo != null)
                    {
                        // 使用缓存的PropertyInfo提高性能
                        var displayValue = propInfo.GetValue(item, null);
                        if (displayValue != null)
                        {
                            sc.Add(displayValue.ToString());
                        }
                    }
                    else
                    {
                        // 回退到原始反射方法
                        var displayValue = ReflectionHelper.GetPropertyValue(item, DisplayMember);
                        if (displayValue != null)
                        {
                            sc.Add(displayValue.ToString());
                        }
                    }
                }

                // 禁用 AutoComplete，避免干扰自定义智能搜索
                // cmb.AutoCompleteCustomSource = sc;
                // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }

            cmb.DataSource = bs;
            // 注意：DropDownStyle 已在前面设置为 effectiveDropStyle
            // cmb.DropDownStyle = DropStyle;  // 已注释，使用上面的 effectiveDropStyle
            
            // 统一设置 DisplayMember 和 ValueMember，确保行为一致
            // 只在当前值为空时才设置，避免覆盖已有的正确绑定
            if (string.IsNullOrEmpty(cmb.DisplayMember))
            {
                cmb.DisplayMember = DisplayMember;
            }
            
            if (string.IsNullOrEmpty(cmb.ValueMember))
            {
                cmb.ValueMember = ValueMember;
            }

            cmb.EndUpdate();
            cmb.SelectedIndex = -1;
        }


        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">值成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        /// <param name="add请选择">  如果数据源为键值对形式，则在数据源的最前面加入("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, CheckBoxComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            cmb.BeginUpdate();

            #region 自动完成

            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (var dr in bs.List)
            {
                // sc.Add(dr[DisplayMember].ToString());
                sc.Add(ReflectionHelper.GetPropertyValue(dr, DisplayMember).ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {
                    // 禁用 AutoComplete，避免干扰自定义智能搜索
                    // cmb.AutoCompleteCustomSource = sc;
                    // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            #endregion


            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            if (string.IsNullOrEmpty(cmb.ValueMember))
            {
                cmb.ValueMember = ValueMember;
            }
            else
            {

            }

            cmb.EndUpdate();
            cmb.SelectedIndex = -1;

        }




        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">값成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        /// <param name="addSelect">  如果数据源为键값对形式，则在数据源的最前面加入("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool addSelect)
        {
            cmb.BeginUpdate();


            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (var dr in bs.List)
            {

                // sc.Add(dr[DisplayMember].ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {
                    // 禁用 AutoComplete，避免干扰自定义智能搜索
                    // cmb.AutoCompleteCustomSource = sc;
                    // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.BeginUpdate();

            cmb.SelectedIndex = -1;
        }


        /// <summary>
        /// 初始化下拉列表控件
        /// </summary>
        /// <param name="ds">要填充的数据源(表)</param>
        /// <param name="cmb">要填充的控件</param>
        /// <param name="ValueMember">값成员名称</param>
        /// <param name="DisplayMember">显示成员名称</param>
        /// <param name="DropStyle">下拉样式</param>
        /// <param name="auto">是否启用自动完成</param>
        public static void InitDropList(DataSet ds, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool addSelect)
        {
            if (addSelect)
            {
                System.Data.DataRow dr = ds.Tables[0].NewRow();

                dr[0] = "请选择";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            cmb.BeginUpdate();
            cmb.DataSource = ds.Tables[0];
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                sc.Add(dr[DisplayMember].ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {
                    // 禁用 AutoComplete，避免干扰自定义智能搜索
                    // cmb.AutoCompleteCustomSource = sc;
                    // cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    // cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            cmb.BeginUpdate();
            cmb.SelectedIndex = -1;
        }



        /// <summary>
        /// 初始化枚举类型，为下拉列表控件赋值
        /// </summary>
        /// <param name="cmbox"></param>
        /// <param name="enumTypeName">typeof(enum)</param>
        public static void InitDropListForWin(ComboBox cmbox, Type enumTypeName)
        {
            cmbox.Items.Clear();
            cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
            Array enumValues = Enum.GetValues(enumTypeName);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            CmbItem item;
            while (e.MoveNext())
            {

                currentValue = (int)e.Current;
                currentName = e.Current.ToString();

                item = new CmbItem(currentValue.ToString(), currentName);

                cmbox.Items.Add(item);
            }
            cmbox.Items.Insert(0, new CmbItem("-1", "请选择"));
            cmbox.SelectedIndex = -1;
        }

        /// <summary>
        /// 初始化枚举类型，为下拉列表控件赋值
        /// </summary>
        /// <param name="cmbox"></param>
        /// <param name="enumTypeName"></param>
        public static void InitDropListForWin(KryptonComboBox cmbox, Type enumTypeName)
        {
            cmbox.Items.Clear();
            cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
            Array enumValues = Enum.GetValues(enumTypeName);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            CmbItem item;
            while (e.MoveNext())
            {

                currentValue = (int)e.Current;
                currentName = e.Current.ToString();

                item = new CmbItem(currentValue.ToString(), currentName);

                cmbox.Items.Add(item);
            }
            cmbox.Items.Insert(0, new CmbItem("-1", "请选择"));
            cmbox.SelectedIndex = -1;
        }


        /// <summary>
        /// 初始化枚举类型，为下拉列表控件赋值
        /// </summary>
        /// <param name="cmbox"></param>
        /// <param name="enumTypeName"></param>
        public static void InitDropListForWin(KryptonComboBox cmbox, Type enumTypeName, bool defaultchoose)
        {
            cmbox.Items.Clear();
            cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
            Array enumValues = Enum.GetValues(enumTypeName);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            CmbItem item;
            while (e.MoveNext())
            {

                currentValue = (int)e.Current;
                currentName = e.Current.ToString();

                item = new CmbItem(currentValue.ToString(), currentName);

                cmbox.Items.Add(item);
            }
            if (defaultchoose)
            {
                cmbox.Items.Insert(0, new CmbItem("-1", "请选择"));
            }

            cmbox.SelectedIndex = -1;
        }



        /// <summary>
        /// 初始化DropDownList,该下拉列表的值存储在独立的表中
        /// <para>  ucAddress1.cmb省.SelectedValue = int.Parse(saint.Address省路); 要int型</para>
        /// </summary>
        /// <param name="list">存储列表值的表</param>
        /// <param name="cmb">DropDownList控件</param>
        /// <param name="ValueMember">存储值域字段</param>
        /// <param name="DisplayMember">存储文本字段</param>
        public static void InitDropList<T>(List<T> list, System.Windows.Forms.ComboBox cmb, string ValueMember, string DisplayMember)
        {
            if (list != null)
            {
                //cmb.Items.Clear();
                cmb.BeginUpdate();

                cmb.DisplayMember = DisplayMember;
                cmb.ValueMember = ValueMember;
                cmb.DataSource = list;

                //AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
                //System.Reflection.PropertyInfo prop = typeof(T).GetProperty(DisplayMember);
                //foreach (T dr in list)
                //{
                //    sc.Add(prop.GetValue(dr, null).ToString());
                //}

                //if (cmb.DropDownStyle == ComboBoxStyle.DropDown)
                //{
                //    cmb.AutoCompleteCustomSource = sc;
                //    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //    cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //}
                //cmb.Items.Insert(0, new ListItem("请选择", "-1"));

                cmb.EndUpdate();
            }
            else
            {
                // cmb.Items.Insert(0, new ListItem("请选择", "-1"));
            }
        }


        /// <summary>
        /// 初始化DropDownList,该下拉列表的值存储在独立的表中
        /// </summary>
        /// <param name="list">存储列表值的表</param>
        /// <param name="cmb">DropDownList控件</param>
        /// <param name="ValueMember">存储值域字段</param>
        /// <param name="DisplayMember">存储文本字段</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitDropList<T>(List<T> list, System.Windows.Forms.ComboBox cmb, string ValueMember, string DisplayMember, bool add请选择)
        {
            if (list != null)
            {
                if (cmb.Items.Count > 0)
                {
                    cmb.DataSource = null;
                    cmb.Items.Clear();
                }

                cmb.BeginUpdate();
                cmb.DataSource = list;
                cmb.DisplayMember = DisplayMember;
                cmb.ValueMember = ValueMember;

                cmb.EndUpdate();
                ///是数据源绑定形式不可以自由添加，只能在数据源上做文章
                ///cmb.Items.Insert(0, new ListItem("请选择", "-1"));

            }
            else
            {
                /// cmb.Items.Insert(0, new ListItem("请选择", "-1"));
            }
        }



        public static void InitDropList(List<string> list, System.Windows.Forms.ComboBox dr, bool IsAdd请选择)
        {
            if (list != null)
            {
                foreach (string var in list)
                {
                    dr.Items.Add(var);
                }
                if (IsAdd请选择)
                {
                    dr.Items.Insert(0, new CmbItem("请选择", "-1"));
                }

            }
            else
            {
                dr.Items.Insert(0, new CmbItem("请选择", "-1"));
            }
            dr.SelectedIndex = -1;
        }


        public static void InitDropList(List<KeyValuePair<string, string>> list, System.Windows.Forms.ComboBox dr, bool IsAdd请选择)
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, string> kv in list)
                {
                    CmbItem item = new CmbItem(kv.Key, kv.Value);

                    dr.Items.Add(item);

                    //  dr.Items.Add(kv.Key + "|" + kv.Value);
                }
                if (IsAdd请选择)
                {
                    CmbItem item = new CmbItem("-1", "请选择");
                    dr.Items.Insert(0, item);
                }

            }
            else
            {
                CmbItem item = new CmbItem("-1", "请选择");
                dr.Items.Insert(0, item);
            }
            dr.SelectedIndex = -1;
        }

        /// <summary>
        /// 为下拉列表添加"请选择"选项
        /// </summary>
        /// <param name="dr">下拉控件</param>
        /// <param name="add">是否添加</param>
        public static void AddPleaseSelectOption(KryptonComboBox dr, bool add = true)
        {
            if (dr.DataSource != null)
            {
                if (add)
                {
                    CmbItem item = new CmbItem("-1", "请选择");
                    dr.Items.Insert(0, item);
                }

            }
            else
            {
                CmbItem item = new CmbItem("-1", "请选择");
                dr.Items.Insert(0, item);
            }
            dr.SelectedIndex = -1;
        }
    }
}
