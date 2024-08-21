using HLH.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{
    [Serializable]
    public class UC正则式提取Para : UCBasePara
    {

        public override string ProcessDo(string StrIn)
        {
        
            string rs = StrIn;

            if (startflag.Trim().Length > 0 || endflag.Trim().Length > 0)
            {
                if (!UseRegularMatch)
                {
                    if (startflag.Contains("[参数]") || endflag.Contains("[参数1]"))
                    {
                        //PrintDebugInfo("标记中带有[参数]，必须选取【正则参数模式】");
                    }
                }

                #region 正则匹配 去掉多余内容 应该针对 描述

                string resultForReg = string.Empty;
                string inputContent = string.Empty;
                string resultForRegCopy = string.Empty;

                inputContent = rs;


                resultForReg = HtmlDataAnalyzeTool.GetPartsContentForTest(startflag, endflag, rs, includeStartEndStr, is贪婪模式, CycleMatch, UseRegularMatch);
                //
                if (resultForReg.Trim().Length > 0 && this.CycleMatch)
                {
                    //string[] rssz = resultForReg.Split(new string[] { "#||#" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] rssz = resultForReg.Split(new string[] { txt循环分割字符 }, StringSplitOptions.RemoveEmptyEntries);
                    //PrintDebugInfo("循环结果数:" + rssz.ToString());
                }


                if (resultForReg.Trim().Length == 0)
                {
                    //
                }
                else
                {
                    string newString = string.Empty;
                    if (保留还是去掉提取的内容)
                    {
                        newString = resultForReg;
                    }
                    else
                    {
                        newString = rs.Replace(resultForReg, "");
                    }

                    if (提取结果替换)
                    {
                        resultForRegCopy = Str提取结果替换.Replace("[结果]", newString);
                        newString = resultForRegCopy;
                        // newString = rs.Replace(newString, resultForRegCopy.Replace(newString, ""));
                    }
                    if (newString != rs && newString.Length > 0)
                    {
                        rs = newString;
                    }
                }
                #endregion  //去掉多余内容
            }
            if (结果去首尾空格)
            {
                rs = rs.Trim();
            }
            return rs;
        }



        private bool useRegularMatch = false;

        /// <summary>
        /// 是否使用正则匹配内容
        /// </summary>
        public bool UseRegularMatch
        {
            get { return useRegularMatch; }
            set { useRegularMatch = value; }
        }


        private bool cycleMatch = false;

        /// <summary>
        /// 是否循环匹配
        /// </summary>
        public bool CycleMatch
        {
            get { return cycleMatch; }
            set { cycleMatch = value; }
        }


        private bool _循环匹配时去重复 = false;

        /// <summary>
        /// 开始标识
        /// </summary>
        public bool 循环匹配时去重复
        {
            get { return _循环匹配时去重复; }
            set { _循环匹配时去重复 = value; }
        }



        private string startflag = string.Empty;

        /// <summary>
        /// 开始标识
        /// </summary>
        public string Startflag
        {
            get { return startflag; }
            set { startflag = value; }
        }




        private string endflag = string.Empty;

        /// <summary>
        /// 结束标识
        /// </summary>
        public string Endflag
        {
            get { return endflag; }
            set { endflag = value; }
        }


        private bool includeStartEndStr = false;

        /// <summary>
        /// 是否包含开始结束标记
        /// </summary>
        public bool IncludeStartEndStr
        {
            get { return includeStartEndStr; }
            set { includeStartEndStr = value; }
        }

        private bool is贪婪模式 = false;


        public bool Is贪婪模式
        {
            get { return is贪婪模式; }
            set { is贪婪模式 = value; }
        }
        private bool _保留还是去掉提取的内容 = true;

        /// <summary>
        /// 保留提取结果为真
        /// </summary>
        public bool 保留还是去掉提取的内容
        {
            get { return _保留还是去掉提取的内容; }
            set { _保留还是去掉提取的内容 = value; }
        }


        private bool _结果去首尾空格 = true;


        public bool 结果去首尾空格
        {
            get { return _结果去首尾空格; }
            set { _结果去首尾空格 = value; }
        }

        public bool 提取结果替换 { get; set; }
        public string Str提取结果替换 { get; set; }

        private string _txt循环分割字符 = string.Empty;


        public string txt循环分割字符
        {
            get { return _txt循环分割字符; }
            set { _txt循环分割字符 = value; }
        }
    }
}
