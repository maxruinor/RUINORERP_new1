using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace CommonProcess.StringProcess
{

    [Serializable]
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UCRepairString : UCMyBase, IUCBase
    {
        public UCRepairString()
        {
            InitializeComponent();

        }

        public bool PrintProcessResult { get; set; }


        public string ProcessString(string SourceString)
        {
            string rs = SourceString;
            #region 去首尾字符

            if (chk去首尾字符.Checked && txt首尾字符.Text.Length > 0)
            {
                if (txt首尾字符.Text.Trim().Length > 1)
                {
                    rs = SourceString.TrimStart(txt首尾字符.Text.ToCharArray()).TrimEnd(txt首尾字符.Text.ToCharArray());
                }
                if (txt首尾字符.Text.Trim().Length == 1)
                {
                    char temp = Convert.ToChar(txt首尾字符.Text.Trim());
                    rs = SourceString.TrimStart(temp);
                }
                rs = rs.Trim();
            }


            #region 去掉全角字符
            if (chkRemovehanzi.Checked)
            {
                #region 去掉全角字符  一般为描述性 长字段

                string regexstr = string.Empty;
                if (!string.IsNullOrEmpty(cmbRegexStr.SelectedItem.ToString()) && cmbRegexStr.SelectedItem.ToString() != "自定义正则式")
                {
                    regexstr = cmbRegexStr.SelectedItem.ToString();
                    regexstr = regexstr.Split('：')[1];
                }
                else
                {
                    regexstr = wtxtRegexStr.Text;
                }

                if (regexstr.Trim().Length > 0)
                {
                    string _s = Regex.Replace(rs, regexstr, "");

                    Regex rg = new Regex(regexstr);
                    MatchCollection m = rg.Matches(rs);
                    if (m.Count >= 1)
                    {
                        for (int i = 0; i < m.Count; i++)
                        {
                            if (rs.Contains(m[i].Groups[0].Value) && m[i].Groups[0].Value.Length > 0)
                            {
                                printInfoMessage("字符替换".ToString() + ":" + m[i].Groups[0].Value, Color.Red);
                            }

                        }
                    }
                    rs = _s.Trim();

                }
                #endregion


            }
            #endregion



            #region 替换中间字符
            if (chk替换中间字符.Checked)
            {
                #region 替换内容
                //忽略大小写
                rs = Microsoft.VisualBasic.Strings.Replace(rs, txt替换前.Text.Trim(), txt替换后.Text, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
                #endregion

            }
            #endregion


            #endregion

            if (chk首尾添加字符.Checked)
            {
                rs = rs.Trim();
            }

            if (chk首尾添加字符.Checked)
            {
                rs = watermarktxt首尾添加字符.Text.Replace("[原始值]", rs);
            }
            return rs;
        }

        List<string> RegexStr = new List<string>();
        private void UCRepairString_Load(object sender, EventArgs e)
        {
            //title="[\s\S]*?"          任意字符 ([\s\S]*?)?
            //加载正则
            RegexStr.Add(@"中文字符的正则：[\u4e00-\u9fa5]");
            RegexStr.Add(@"双字节(包括汉字)：[^\x00-\xff]");
            RegexStr.Add(@"匹配空行的正则表达式：\n[\s| ]*\r");
            RegexStr.Add(@"非英文字符：[^a-z0-9A-Z_]+");
            RegexStr.Add(@"自定义正则式");
            //dr.Items.Insert(0, new ListItem("请选择", "-1"));
            initDropList(cmbRegexStr, RegexStr);
        }

        public static void initDropList(WinLib.ComboBoxEx cmb, List<string> list)
        {
            cmb.Items.Clear();
            if (list != null)
            {
                foreach (string var in list)
                {
                    cmb.Items.Add(var);
                }
            }
            cmb.SelectedIndex = -1;
        }

        private void cmbRegexStr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbRegexStr.SelectedItem.ToString()) && cmbRegexStr.SelectedItem.ToString() == "自定义正则式")
            {
                wtxtRegexStr.Visible = true;
            }
            else
            {
                wtxtRegexStr.Visible = false;
            }
        }



        //========

        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void SaveDataFromUI(UCBasePara aa)
        {
            UCRepairStringPara para = new UCRepairStringPara();
            para = aa as UCRepairStringPara;
            para.Is去全角字符 = chkRemovehanzi.Checked;
            para.Is去首尾字符 = chk去首尾字符.Checked;
            para.Is替换中间字符 = chk替换中间字符.Checked;
            para.Is结果去首尾空格 = chk结果去首尾空格.Checked;
            para.Is首尾添加字符 = chk首尾添加字符.Checked;

            para.regexStr = wtxtRegexStr.Text;
            if (cmbRegexStr.SelectedItem != null)
            {
                para.cmbRegexStr = cmbRegexStr.SelectedItem.ToString();
            }

            para.str被替换内容 = txt替换前.Text;
            para.str要替换的内容 = txt替换后.Text;
            para.str首尾字符 = txt首尾字符.Text;
            para.str首尾添加字符表达式 = watermarktxt首尾添加字符.Text;

            para.str移除头部内容 = txt移除头部内容.Text;
            para.str移除尾部内容 = txt移除尾部内容.Text;
        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UCRepairStringPara para = new UCRepairStringPara();
            para = aa as UCRepairStringPara;
            chkRemovehanzi.Checked = para.Is去全角字符;
            chk去首尾字符.Checked = para.Is去首尾字符;
            chk替换中间字符.Checked = para.Is替换中间字符;
            chk结果去首尾空格.Checked = para.Is结果去首尾空格;
            chk首尾添加字符.Checked = para.Is首尾添加字符;

            wtxtRegexStr.Text = para.regexStr;
            //cmbRegexStr.SelectedIndex = cmbRegexStr.FindString(para.CmbRegexStr);
            cmbRegexStr.SelectedText = para.cmbRegexStr;
            //TODO:这里赋值失效。可能是由于字串是特殊的 正则表达式。后面再优化
            cmbRegexStr.SelectedIndex = cmbRegexStr.FindStringExact(para.cmbRegexStr);
            txt替换前.Text = para.str被替换内容;
            txt替换后.Text = para.str要替换的内容;
            txt首尾字符.Text = para.str首尾字符;
            watermarktxt首尾添加字符.Text = para.str首尾添加字符表达式;


            txt移除头部内容.Text = para.str移除头部内容;
            txt移除尾部内容.Text = para.str移除尾部内容;
        }


    }
}
