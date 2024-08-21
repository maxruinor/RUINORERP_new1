using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public partial class FormFormat : Form
    {
        #region 成员变量
        private NumericUpDown numericUpDown;
        private Label labelNumeric;
        private string m_NullValue;
        private string m_sFormat;
        #endregion
        #region 属性
        public string NullValue
        {
            set
            {
                m_NullValue = value;
            }
            get
            {
                return m_NullValue;
            }
        }
        public string Format
        {
            set
            {
                m_sFormat = value;
            }
            get
            {
                return m_sFormat;
            }
        }
        #endregion
        #region 构造函数
        public FormFormat()
        {
            InitializeComponent();
            LoadControls();
        }
        public FormFormat(string sNullValue, string sFormat)
        {
            m_NullValue = sNullValue;
            m_sFormat = sFormat;
            InitializeComponent();
            LoadControls();
        }
        #endregion
        #region 方法
        private void LoadControls()
        {
            LoadNumericControls();
            LoadDateControls();
            SetCurrentValue();
        }
        private void SetCurrentValue()
        {
            if (m_sFormat == null || m_sFormat == String.Empty)
            {

                textBoxNull.Text = m_NullValue;
                listBoxFormatType.SelectedIndex = 0;
            }
            else
            {

                if (m_sFormat.Trim().Length == 1) //日期类型
                {
                    listBoxFormatType.SelectedIndex = 3;
                    string sVlaue = DateTime.Now.ToString(m_sFormat);
                    int i = listBoxDate.Items.IndexOf(sVlaue);
                    listBoxDate.SelectedIndex = i;
                }
                else //数值类型 
                {
                    string sType = m_sFormat.Substring(0, 1);
                    string sNumeric = m_sFormat.Substring(1, 1);
                    if (sType == "N")
                    {
                        listBoxFormatType.SelectedIndex = 1;
                    }
                    else if (sType == "C")
                    {
                        listBoxFormatType.SelectedIndex = 2;
                    }
                    else if (sType == "E")
                    {
                        listBoxFormatType.SelectedIndex = 4;
                    }
                    else
                    {
                    }
                    numericUpDown.Value = int.Parse(sNumeric);
                }
            }
        }
        private void SetControlsVisible(string sType)
        {
            switch (sType)
            {
                case "无格式设置":
                    NoneFormatVisible = true;
                    break;
                case "数字":
                    NumericControlVisible = true;
                    break;
                case "货币":
                    CurrencyControlVisible = true;
                    break;
                case "日期时间":
                    DateControlVisible = true;
                    break;
                case "科学型":
                    ScienceControlVisible = true;
                    break;
                default:
                    break;
            }
        }
        private void SaveValueFormat(string sType)
        {
            switch (sType)
            {
                case "无格式设置":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = null;
                    break;
                case "数字":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = "N" + numericUpDown.Value.ToString();
                    break;
                case "货币":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = "C" + numericUpDown.Value.ToString();
                    break;
                case "日期时间":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = GetDateValueFormat(listBoxDate.SelectedIndex);
                    break;
                case "科学型":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = "E" + numericUpDown.Value.ToString();
                    break;
                default:
                    break;
            }
        }
        private string GetDateValueFormat(int index)
        {
            string sFormat = "d";
            switch (index)
            {
                case 0:
                    sFormat = "d";
                    break;
                case 1:
                    sFormat = "D";
                    break;
                case 2:
                    sFormat = "f";
                    break;
                case 3:
                    sFormat = "F";
                    break;
                case 4:
                    sFormat = "g";
                    break;
                case 5:
                    sFormat = "G";
                    break;
                case 6:
                    sFormat = "t";
                    break;
                case 7:
                    sFormat = "T";
                    break;
                case 8:
                    sFormat = "M";
                    break;
                default:
                    break;
            }
            return sFormat;
        }
        //无格式设置
        private bool NoneFormatVisible
        {
            set
            {
                labelExplain.Text = "显示无修饰源中的值时不使用任何设置。";
                labelExample.Text = "-1234.5";
                DateControlVisible = !value;
                labelNumeric.Visible = !value;
                numericUpDown.Visible = !value;
            }
        }
        //数字
        private bool NumericControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "指定数字的格式。请注意，货币格式类型为货币值提供的专用的格式设置。";
                labelExample.Text = Decimal.Parse("-1234.5").ToString("N" + numericUpDown.Value.ToString());
            }
        }
        //货币
        private bool CurrencyControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "指定货币值的格式！";
                labelExample.Text = Decimal.Parse("-1234.57").ToString("C" + numericUpDown.Value.ToString());
            }
        }
        //科学
        private bool ScienceControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "指定使用科学技术法的值的格式！";
                labelExample.Text = Decimal.Parse("-1234.57").ToString("E" + numericUpDown.Value.ToString());
            }
        }
        //日期
        private bool DateControlVisible
        {
            set
            {
                labelDateType.Visible = value;
                listBoxDate.Visible = value;
                labelNumeric.Visible = !value;
                numericUpDown.Visible = !value;
                labelExplain.Text = "指定日期和时间值的格式！";
                object obj = listBoxFormatType.SelectedItem;
                labelExample.Text = obj.ToString();
            }
        }
        private void LoadDateControls()
        {
            listBoxDate.Items.Add(DateTime.Now.ToString("d"));
            listBoxDate.Items.Add(DateTime.Now.ToString("D"));
            listBoxDate.Items.Add(DateTime.Now.ToString("f"));
            listBoxDate.Items.Add(DateTime.Now.ToString("F"));
            listBoxDate.Items.Add(DateTime.Now.ToString("g"));
            listBoxDate.Items.Add(DateTime.Now.ToString("G"));
            listBoxDate.Items.Add(DateTime.Now.ToString("t"));
            listBoxDate.Items.Add(DateTime.Now.ToString("T"));
            listBoxDate.Items.Add(DateTime.Now.ToString("M"));
        }
        private void LoadNumericControls()
        {
            labelNumeric = new Label();
            labelNumeric.AutoSize = true;
            labelNumeric.Location = new System.Drawing.Point(3, 34);
            labelNumeric.Name = "label";
            labelNumeric.Size = new System.Drawing.Size(71, 12);
            labelNumeric.Text = "小数位数(&D)";

            numericUpDown = new NumericUpDown();
            numericUpDown.Location = new System.Drawing.Point(80, 31);
            numericUpDown.Name = "numericUpDown";
            numericUpDown.Size = new System.Drawing.Size(159, 21);
            numericUpDown.Value = 2;
            numericUpDown.ValueChanged += new EventHandler(numericUpDown_ValueChanged);

            panelControl.Controls.Add(labelNumeric);
            panelControl.Controls.Add(numericUpDown);
        }

        #endregion
        #region 事件
        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            object obj = listBoxFormatType.SelectedItem;
            if (obj != null)
            {
                switch (obj.ToString())
                {
                    case "无格式设置":
                        NoneFormatVisible = true;
                        break;
                    case "数字":
                        labelExample.Text = Decimal.Parse("-1234.5").ToString("N" + numericUpDown.Value.ToString());
                        break;
                    case "货币":
                        labelExample.Text = Decimal.Parse("-1234.57").ToString("C" + numericUpDown.Value.ToString());
                        break;
                    case "科学型":
                        labelExample.Text = Decimal.Parse("-1234.57").ToString("E" + numericUpDown.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
        }
        private void listBoxFormatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = listBoxFormatType.SelectedItem;
            if (obj != null)
            {
                SetControlsVisible(obj.ToString());
            }
        }

        private void listBoxDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = listBoxDate.SelectedItem;
            labelExample.Text = obj.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            object obj = listBoxFormatType.SelectedItem;
            SaveValueFormat(obj.ToString());
        }
        #endregion
    }
}