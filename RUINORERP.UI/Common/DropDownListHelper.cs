// ----------------------------------------------------------------
// Description : ���ڳ�ʼ��UI����������б�
// Author      : watson
// Create date : �������� ��ʽ��2009-09-17
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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
        /// <param name="auto">�Ƿ����Զ���ɹ���</param>
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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
        /// <param name="auto">�Ƿ����Զ���ɹ���</param>
        /// <param name="add��ѡ��">  ������Դ����ʽ������������ӣ�ֻ��������Դ�������� ("��ѡ��", "-1")</param>
        public static void InitDropList(BindingSource bs, KryptonComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add��ѡ��)
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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
        /// <param name="auto">�Ƿ����Զ���ɹ���</param>
        /// <param name="add��ѡ��">  ������Դ����ʽ������������ӣ�ֻ��������Դ�������� ("��ѡ��", "-1")</param>
        public static void InitDropList(BindingSource bs, KryptonComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            if (DropStyle == ComboBoxStyle.DropDownList)
            {
                // ������������
                cmb.EnableSearch = false;
            }
            else
            {
                // ������������
                cmb.EnableSearch = false;
                cmb.SearchDelay = 500; // ����400ms�ӳ�
            }
            //cmb.DataSource = null;
            //cmb.DataBindings.Clear();
            cmb.BeginUpdate();
            if (!cmb.EnableSearch)
            {
                #region �Զ���ȫ

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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
        /// <param name="auto">�Ƿ����Զ���ɹ���</param>
        /// <param name="add��ѡ��">  ������Դ����ʽ������������ӣ�ֻ��������Դ�������� ("��ѡ��", "-1")</param>
        public static void InitDropList(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add��ѡ��)
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
        /// �����б�İ�
        /// </summary>
        /// <param name="ds">Ҫ�󶨵�����Դ(��)</param>
        /// <param name="cmb">Ҫ�󶨵Ŀؼ���</param>
        /// <param name="ValueMember">ֵ������</param>
        /// <param name="DisplayMember">��ʾ������</param>
        /// <param name="DropStyle">�������</param>
        /// <param name="auto">�Ƿ����Զ���ɹ���</param>
        public static void InitDropList(DataSet ds, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add��ѡ��)
        {
            if (add��ѡ��)
            {
                System.Data.DataRow dr = ds.Tables[0].NewRow();

                dr[0] = "��ѡ��";
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
        /// ����ö�����ͣ�Ϊ�����������
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
            cmbox.Items.Insert(0, new CmbItem("-1", "��ѡ��"));
            cmbox.SelectedIndex = -1;
        }

        /// <summary>
        /// ����ö�����ͣ�Ϊ�����������
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
            cmbox.Items.Insert(0, new CmbItem("-1", "��ѡ��"));
            cmbox.SelectedIndex = -1;
        }


        /// <summary>
        /// ����ö�����ͣ�Ϊ�����������
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
                cmbox.Items.Insert(0, new CmbItem("-1", "��ѡ��"));
            }

            cmbox.SelectedIndex = -1;
        }



        /// <summary>
        /// ��ʼ��DropDownList,�������б��ֵ�洢�ڶ����ı���
        /// <para>  ucAddress1.cmbʡ.SelectedValue = int.Parse(saint.Addressʡ·); Ҫint��</para>
        /// </summary>
        /// <param name="list">�洢�б�ֵ�ı�</param>
        /// <param name="cmb">DropDownList�ؼ�</param>
        /// <param name="ValueMember">�洢ֵ���ֶ�</param>
        /// <param name="DisplayMember">�洢�ı��ֶ�</param>
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
                //cmb.Items.Insert(0, new ListItem("��ѡ��", "-1"));

                cmb.EndUpdate();
            }
            else
            {
                // cmb.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            }
        }


        /// <summary>
        /// ��ʼ��DropDownList,�������б��ֵ�洢�ڶ����ı���
        /// </summary>
        /// <param name="list">�洢�б�ֵ�ı�</param>
        /// <param name="cmb">DropDownList�ؼ�</param>
        /// <param name="ValueMember">�洢ֵ���ֶ�</param>
        /// <param name="DisplayMember">�洢�ı��ֶ�</param>
        /// <param name="add��ѡ��">  ������Դ����ʽ������������ӣ�ֻ��������Դ�������� ("��ѡ��", "-1")</param>
        public static void InitDropList<T>(List<T> list, System.Windows.Forms.ComboBox cmb, string ValueMember, string DisplayMember, bool add��ѡ��)
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
                ///������Դ����ʽ������������ӣ�ֻ��������Դ��������
                ///cmb.Items.Insert(0, new ListItem("��ѡ��", "-1"));

            }
            else
            {
                /// cmb.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            }
        }



        public static void InitDropList(List<string> list, System.Windows.Forms.ComboBox dr, bool IsAdd��ѡ��)
        {
            if (list != null)
            {
                foreach (string var in list)
                {
                    dr.Items.Add(var);
                }
                if (IsAdd��ѡ��)
                {
                    dr.Items.Insert(0, new CmbItem("��ѡ��", "-1"));
                }

            }
            else
            {
                dr.Items.Insert(0, new CmbItem("��ѡ��", "-1"));
            }
            dr.SelectedIndex = -1;
        }


        public static void InitDropList(List<KeyValuePair<string, string>> list, System.Windows.Forms.ComboBox dr, bool IsAdd��ѡ��)
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, string> kv in list)
                {
                    CmbItem item = new CmbItem(kv.Key, kv.Value);

                    dr.Items.Add(item);

                    //  dr.Items.Add(kv.Key + "|" + kv.Value);
                }
                if (IsAdd��ѡ��)
                {
                    CmbItem item = new CmbItem("-1", "��ѡ��");
                    dr.Items.Insert(0, item);
                }

            }
            else
            {
                CmbItem item = new CmbItem("-1", "��ѡ��");
                dr.Items.Insert(0, item);
            }
            dr.SelectedIndex = -1;
        }




    }
}
