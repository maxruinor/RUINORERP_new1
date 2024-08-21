using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{

    [Serializable]
    public class UCRepairStringPara : UCBasePara
    {

        private bool is去首尾字符;
        private bool is去全角字符;
        private bool is替换中间字符;
        private bool is首尾添加字符;
        private bool is结果去首尾空格;

        public bool Is去首尾字符 { get => is去首尾字符; set => is去首尾字符 = value; }
        public bool Is去全角字符 { get => is去全角字符; set => is去全角字符 = value; }
        public bool Is替换中间字符 { get => is替换中间字符; set => is替换中间字符 = value; }
        public bool Is首尾添加字符 { get => is首尾添加字符; set => is首尾添加字符 = value; }
        public bool Is结果去首尾空格 { get => is结果去首尾空格; set => is结果去首尾空格 = value; }
        public string str首尾字符 { get => _str首尾字符; set => _str首尾字符 = value; }
        public string regexStr { get => _RegexStr; set => _RegexStr = value; }
        public string cmbRegexStr { get => _cmbRegexStr; set => _cmbRegexStr = value; }
        public string str被替换内容 { get => _str被替换内容; set => _str被替换内容 = value; }
        public string str要替换的内容 { get => _str要替换的内容; set => _str要替换的内容 = value; }
        public string str首尾添加字符表达式 { get => _str首尾添加字符表达式; set => _str首尾添加字符表达式 = value; }
        public string str移除头部内容 { get => _str移除头部内容; set => _str移除头部内容 = value; }
        public string str移除尾部内容 { get => _str移除尾部内容; set => _str移除尾部内容 = value; }

        private string _str首尾字符 = string.Empty;
        private string _RegexStr = string.Empty;
        private string _cmbRegexStr = string.Empty;
        private string _str被替换内容 = string.Empty;
        private string _str要替换的内容 = string.Empty;
        private string _str首尾添加字符表达式 = string.Empty;
        private string _str移除头部内容 = string.Empty;
        private string _str移除尾部内容 = string.Empty;



        public override string ProcessDo(string StrIn)
        {
            string rs = StrIn;
            #region 去首尾字符

            if (is去首尾字符 && str首尾字符.Length > 0)
            {
                if (str首尾字符.Trim().Length > 1)
                {
                    rs = StrIn.TrimStart(str首尾字符.ToCharArray()).TrimEnd(str首尾字符.ToCharArray());
                }
                if (str首尾字符.Trim().Length == 1)
                {
                    char temp = Convert.ToChar(str首尾字符.Trim());
                    rs = StrIn.TrimStart(temp);
                }
                rs = rs.Trim();
            }


            #region 去掉全角字符
            if (is去全角字符)
            {
                #region 去掉全角字符  一般为描述性 长字段

                string regexstr = string.Empty;
                if (!string.IsNullOrEmpty(cmbRegexStr) && cmbRegexStr != "自定义正则式")
                {
                    regexstr = cmbRegexStr;
                    regexstr = regexstr.Split('：')[1];
                }
                else
                {
                    regexstr = regexStr;
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
                                //TODO:要补充功能
                                PrintDebugInfo("字符替换".ToString() + ":" + m[i].Groups[0].Value);
                            }

                        }
                    }
                    rs = _s.Trim();

                }
                #endregion


            }
            #endregion



            #region 替换中间字符
            if (is替换中间字符)
            {
                #region 替换内容
                //忽略大小写
                rs = Microsoft.VisualBasic.Strings.Replace(rs, str被替换内容, str要替换的内容, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
                #endregion

            }
            #endregion


            #endregion

            if (is首尾添加字符)
            {
                rs = _str首尾添加字符表达式.Replace("[原始值]", rs);
            }
            rs = rs.TrimStart(str移除头部内容.ToCharArray());
            rs = rs.TrimEnd(str移除尾部内容.ToCharArray());
            if (is结果去首尾空格)
            {
                rs = rs.Trim();
            }
            return rs;

        }


    }
}
