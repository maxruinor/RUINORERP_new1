using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HLH.WinControl.ComBoBoxEx;


namespace CommonProcess.StringProcess
{
    public partial class UCFindSpecialChar : UCMyBase
    {
        public UCFindSpecialChar()
        {
            InitializeComponent();
        }

        public string KeyID { get; set; }


        public static List<KeyValuePair<string, string>> RegexForStr
        {
            get
            {
                List<KeyValuePair<string, string>> m_tags = new List<KeyValuePair<string, string>>();
                m_tags.Add(new KeyValuePair<string, string>("中文字符的正则", @"[\u4e00-\u9fa5]"));
                m_tags.Add(new KeyValuePair<string, string>("双字节(包括汉字)", @"[^\x00-\xff]"));
                m_tags.Add(new KeyValuePair<string, string>("匹配空行的正则表达式", @"\n[\s| ]*\r"));
                m_tags.Add(new KeyValuePair<string, string>("非英文字符", @"[^a-z0-9A-Z_]+"));
                return m_tags;
            }

        }

        public static List<string> SpecialCharList
        {
            get
            {
                List<string> m_tags = new List<string>();
                m_tags.Add("℃| Degree ");
                m_tags.Add("°C| Degree ");
                m_tags.Add("Φ| Diameter ");
                m_tags.Add("Original| ");
                m_tags.Add("original| ");
                m_tags.Add("Ω| ohm ");
                m_tags.Add("：| :");
                m_tags.Add("（| (");
                m_tags.Add("）| )");
                m_tags.Add("～| ~");
                m_tags.Add("–| -");
                m_tags.Add("’| '");
                m_tags.Add("％| %");
                m_tags.Add("≥| >=");
                m_tags.Add("，|,");
                m_tags.Add("。|.");
                m_tags.Add("☆|*");
                m_tags.Add("≤|<=");
                m_tags.Add("°| degree ");
                m_tags.Add("〞| \" ");
                

                return m_tags;
            }


        }


        private void UCFindSpecialChar_Load(object sender, EventArgs e)
        {
            watermarkTextBox1.Visible = true;
            HLH.Lib.Helper.DropDownListHelper.InitDropList(RegexForStr, cmbRegexStr, false);


            //cmbCountry.Items.Add(oif.lblContryName.Text);
            StatusList _StatusList = new StatusList();
            ListSelectionWrapper<Status> StatusSelections;

            _StatusList = new StatusList();

            for (int i = 0; i < SpecialCharList.Count; i++)
            {
                Status item = new Status(i, SpecialCharList[i]);
                // item.ItemValue = SpecialChar[i].Value;
                _StatusList.Add(item);
            }

            StatusSelections = new ListSelectionWrapper<Status>(_StatusList, "Name");

            cmbSpcChar.DataSource = StatusSelections;
            cmbSpcChar.DisplayMemberSingleItem = "Name";
            cmbSpcChar.DisplayMember = "NameConcatenated";
            cmbSpcChar.ValueMember = "Selected";

            //cmbSpcChar.CheckBoxItems[3].DataBindings.DefaultDataSourceUpdateMode
            //    = DataSourceUpdateMode.OnPropertyChanged;
            cmbSpcChar.DataBindings.DefaultDataSourceUpdateMode
                = DataSourceUpdateMode.OnPropertyChanged;

            //StatusSelections.FindObjectWithItem(UpdatedStatus).Selected = true;

        }



        public string ProcessString(string SourceString)
        {
            string rs = SourceString;

            if (rdb特殊字符正则式.Checked)
            {
                #region 去掉全角字符  一般为描述性 长字段

                string regexstr = string.Empty;
                if (!string.IsNullOrEmpty(cmbRegexStr.SelectedItem.ToString()))
                {
                    regexstr = cmbRegexStr.SelectedItem.ToString();
                }

                if (regexstr.Trim().Length > 0)
                {
                    Regex rg = new Regex(regexstr);
                    MatchCollection m = rg.Matches(rs);
                    if (m.Count >= 1)
                    {
                        for (int i = 0; i < m.Count; i++)
                        {
                            if (rs.Contains(m[i].Groups[0].Value) && m[i].Groups[0].Value.Length > 0)
                            {
                                printInfoMessage(KeyID.ToString() + ":" + m[i].Groups[0].Value, Color.Red);
                                if (m.Count > 50)
                                {
                                    printInfoMessage(KeyID.ToString() + ":结果太多，省略显示。");
                                    break;
                                }
                            }

                        }
                    }

                    if (chk替换字符.Checked)
                    {
                        #region 替换内容
                        // rs = rs.Replace(m[i].Groups[0].Value, watermarkTextBox1.Text);
                        #endregion
                        string _s = Regex.Replace(rs, regexstr, watermarkTextBox1.Text);
                        rs = _s.Trim();
                    }

                }
                #endregion
            }

            if (rdb常见特列字符.Checked)
            {
                List<string> tempCheckList = new List<string>();
                //全选时
                if (cmbSpcChar.CheckBoxItems.Count == cmbSpcChar.Items.Count)
                {
                    foreach (var item in cmbSpcChar.Items)
                    {
                        ObjectSelectionWrapper<Status> ss = item as ObjectSelectionWrapper<Status>;
                        tempCheckList.Add(ss.Item.Name);
                    }
                }
                else
                {
                    foreach (var item in cmbSpcChar.CheckBoxItems)
                    {
                        if (item.Checked && item.Text.Trim().Length > 0)
                        {
                            tempCheckList.Add(item.Text);
                        }
                    }

                }
                foreach (var item in tempCheckList)
                {
                    //如果是单个选择，同在替换中可以自定义需要替换的字符。如果是多个，则按默认的执行
                    #region 如果选择了替换字符，则按替换
                    if (!string.IsNullOrEmpty(cmbSpcChar.SelectedItem.ToString()))
                    {
                        string schar = item.Split('|')[0];
                        Regex rg = new Regex(schar);
                        MatchCollection m = rg.Matches(rs);
                        if (m.Count >= 1)
                        {
                            for (int i = 0; i < m.Count; i++)
                            {
                                if (rs.Contains(m[i].Groups[0].Value) && m[i].Groups[0].Value.Length > 0)
                                {
                                    printInfoMessage(KeyID.ToString() + ":" + m[i].Groups[0].Value);
                                }
                            }
                        }

                        if (chk替换字符.Checked)
                        {
                            string _s = string.Empty;
                            if (rdb常见特列字符.Checked && watermarkTextBox1.Text.Length > 0)
                            {
                                _s = Regex.Replace(rs, schar, watermarkTextBox1.Text);
                            }
                            else
                            {
                                _s = Regex.Replace(rs, schar, item.Split('|')[1]);
                            }
                            rs = _s.Trim();
                        }
                    }

                    #endregion
                }


            }





            return rs;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in SpecialCharList)
            {
                cmbSpcChar.CheckBoxItems[item].Checked = chkSelectAll.Checked;
            }
        }

        private void cmbRegexStr_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblForRexDesc.Text = (cmbRegexStr.SelectedItem as HLH.Lib.CmbItem).Key;

        }

        private void rdb特殊字符正则式_CheckedChanged(object sender, EventArgs e)
        {
            lblForRexDesc.Visible = rdb特殊字符正则式.Checked;
        }

        private void rdb常见特列字符_CheckedChanged(object sender, EventArgs e)
        {
            lblForRexDesc.Visible = rdb特殊字符正则式.Checked;
        }


    }
}
