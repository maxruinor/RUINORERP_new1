using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public partial class FormFormat : Form
    {
        #region ��Ա����
        private NumericUpDown numericUpDown;
        private Label labelNumeric;
        private string m_NullValue;
        private string m_sFormat;
        #endregion
        #region ����
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
        #region ���캯��
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
        #region ����
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

                if (m_sFormat.Trim().Length == 1) //��������
                {
                    listBoxFormatType.SelectedIndex = 3;
                    string sVlaue = DateTime.Now.ToString(m_sFormat);
                    int i = listBoxDate.Items.IndexOf(sVlaue);
                    listBoxDate.SelectedIndex = i;
                }
                else //��ֵ���� 
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
                case "�޸�ʽ����":
                    NoneFormatVisible = true;
                    break;
                case "����":
                    NumericControlVisible = true;
                    break;
                case "����":
                    CurrencyControlVisible = true;
                    break;
                case "����ʱ��":
                    DateControlVisible = true;
                    break;
                case "��ѧ��":
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
                case "�޸�ʽ����":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = null;
                    break;
                case "����":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = "N" + numericUpDown.Value.ToString();
                    break;
                case "����":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = "C" + numericUpDown.Value.ToString();
                    break;
                case "����ʱ��":
                    m_NullValue = textBoxNull.Text;
                    m_sFormat = GetDateValueFormat(listBoxDate.SelectedIndex);
                    break;
                case "��ѧ��":
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
        //�޸�ʽ����
        private bool NoneFormatVisible
        {
            set
            {
                labelExplain.Text = "��ʾ������Դ�е�ֵʱ��ʹ���κ����á�";
                labelExample.Text = "-1234.5";
                DateControlVisible = !value;
                labelNumeric.Visible = !value;
                numericUpDown.Visible = !value;
            }
        }
        //����
        private bool NumericControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "ָ�����ֵĸ�ʽ����ע�⣬���Ҹ�ʽ����Ϊ����ֵ�ṩ��ר�õĸ�ʽ���á�";
                labelExample.Text = Decimal.Parse("-1234.5").ToString("N" + numericUpDown.Value.ToString());
            }
        }
        //����
        private bool CurrencyControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "ָ������ֵ�ĸ�ʽ��";
                labelExample.Text = Decimal.Parse("-1234.57").ToString("C" + numericUpDown.Value.ToString());
            }
        }
        //��ѧ
        private bool ScienceControlVisible
        {
            set
            {
                labelNumeric.Visible = value;
                numericUpDown.Visible = value;
                DateControlVisible = !value;
                labelExplain.Text = "ָ��ʹ�ÿ�ѧ��������ֵ�ĸ�ʽ��";
                labelExample.Text = Decimal.Parse("-1234.57").ToString("E" + numericUpDown.Value.ToString());
            }
        }
        //����
        private bool DateControlVisible
        {
            set
            {
                labelDateType.Visible = value;
                listBoxDate.Visible = value;
                labelNumeric.Visible = !value;
                numericUpDown.Visible = !value;
                labelExplain.Text = "ָ�����ں�ʱ��ֵ�ĸ�ʽ��";
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
            labelNumeric.Text = "С��λ��(&D)";

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
        #region �¼�
        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            object obj = listBoxFormatType.SelectedItem;
            if (obj != null)
            {
                switch (obj.ToString())
                {
                    case "�޸�ʽ����":
                        NoneFormatVisible = true;
                        break;
                    case "����":
                        labelExample.Text = Decimal.Parse("-1234.5").ToString("N" + numericUpDown.Value.ToString());
                        break;
                    case "����":
                        labelExample.Text = Decimal.Parse("-1234.57").ToString("C" + numericUpDown.Value.ToString());
                        break;
                    case "��ѧ��":
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