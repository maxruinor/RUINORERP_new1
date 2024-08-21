// ----------------------------------------------------------------
// Description : ���ڳ�ʼ��UI����������б�
// Author      : watson
// Create date : �������� ��ʽ��2009-09-17
// Modify date : 
// Modify desc : 
// ----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HLH.Lib
{
    public class CmbItem
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public string desc { get; set; }


        public CmbItem(string pkey, string pname)
        {
            key = pkey;
            name = pname;
        }
    }

    /// <summary>
    /// ������ʹ����
    /// </summary>
    [Obsolete("������ʹ����")]
    public class ComBoBoxItem
    {
        private int key;

        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public ComBoBoxItem(int pkey, string pname)
        {
            key = pkey;
            name = pname;
        }
    }
}

namespace HLH.Lib.Helper
{



    public class DropDownListHelper
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


        public static void InitDropListForDictionary(Dictionary<string, string> list, System.Windows.Forms.ComboBox dr, bool IsAdd��ѡ��)
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, string> kv in list)
                {
                    dr.Items.Add(kv.Key + "|" + kv.Value);
                }
                if (IsAdd��ѡ��)
                {
                    dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
                }

            }
            else
            {
                dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            }
            dr.SelectedIndex = -1;
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
        /// <param name="enumTypeName"></param>
        public static void InitDropListForWin(ComboBox cmbox, Type enumTypeName)
        {
            cmbox.Items.Clear();
            cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
            Array enumValues = Enum.GetValues(enumTypeName);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            ComBoBoxItem item;
            while (e.MoveNext())
            {

                currentValue = (int)e.Current;
                currentName = e.Current.ToString();

                item = new ComBoBoxItem(currentValue, currentName);

                cmbox.Items.Add(item);
            }
            cmbox.Items.Insert(0, new ComBoBoxItem(-1, "��ѡ��"));
            cmbox.SelectedIndex = -1;
        }

        /// <summary>
        /// ����ö�����ͣ�Ϊ�����������
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="enumTypeName"></param>
        public static void InitDropListForWeb(System.Web.UI.WebControls.DropDownList dropDownList, Type enumTypeName)
        {
            dropDownList.Items.Clear();
            Array enumValues = Enum.GetValues(enumTypeName);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            ListItem item;
            while (e.MoveNext())
            {
                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                item = new ListItem(currentName, currentValue.ToString());
                dropDownList.Items.Add(item);
            }
            dropDownList.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            dropDownList.SelectedIndex = -1;
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dropDownList"></param>
        /// <param name="ValueMember"></param>
        /// <param name="DisplayMember"></param>
        /// <param name="addSpace"></param>
        public static void InitDropListForWeb<T>(List<T> list, System.Web.UI.WebControls.DropDownList dropDownList, string ValueMember, string DisplayMember, bool addSpace)
        {
            if (list != null)
            {
                dropDownList.Items.Clear();
                dropDownList.DataTextField = DisplayMember;
                dropDownList.DataValueField = ValueMember;
                dropDownList.DataSource = list;
                dropDownList.DataBind();

                if (addSpace)
                {
                    dropDownList.Items.Insert(0, new ListItem("��ѡ��", "-1"));
                    dropDownList.SelectedIndex = -1;
                }
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
                    dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
                }

            }
            else
            {
                dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
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



        /// <summary>
        /// ��ʼ��DropDownList,�������б��ֵ�洢�ڶ����ı���
        /// </summary>
        /// <param name="tablename">�洢�б�ֵ�ı�</param>
        /// <param name="dr">DropDownList�ؼ�</param>
        /// <param name="valueField">�洢ֵ���ֶ�</param>
        /// <param name="textField">�洢�ı��ֶ�</param>
        public static void InitDropList(DataTable dt, DropDownList dr, string valueField, string textField)
        {
            if (dt != null)
            {
                dr.DataSource = dt;
                dr.DataValueField = valueField;
                dr.DataTextField = textField;
                dr.DataBind();
                dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            }
            else
            {
                dr.Items.Insert(0, new ListItem("��ѡ��", "-1"));
            }

            dr.SelectedIndex = -1;
        }
    }
}
