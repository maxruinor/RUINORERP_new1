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
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
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
                cmb.AutoCompleteCustomSource = sc;
                cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
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
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
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

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            cmb.BeginUpdate();
            cmb.SelectedIndex = -1;
        }




        /*
        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
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

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.EndUpdate();

            cmb.SelectedIndex = -1;
        }
        */

        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, KryptonComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            if (DropStyle == ComboBoxStyle.DropDownList)
            {
                // 启用搜索功能
                cmb.EnableSearch = false;
            }
            else
            {
                // 启用搜索功能
                cmb.EnableSearch = false;
                cmb.SearchDelay = 500; // 设置400ms延迟
            }
            //cmb.DataSource = null;
            //cmb.DataBindings.Clear();
            cmb.BeginUpdate();
            if (!cmb.EnableSearch)
            {
                #region 自动补全

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

                        cmb.AutoCompleteCustomSource = sc;
                        cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                    }
                }
                #endregion
            }

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
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add请选择)
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

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.BeginUpdate();

            cmb.SelectedIndex = -1;
        }


        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        public static void InitDropList(DataSet ds, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add请选择)
        {
            if (add请选择)
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

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            cmb.BeginUpdate();
            cmb.SelectedIndex = -1;
        }



        /// <summary>
        /// 根据枚举类型，为下拉框绑定数据
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
        /// 根据枚举类型，为下拉框绑定数据
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
        /// 根据枚举类型，为下拉框绑定数据
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




    }
}
