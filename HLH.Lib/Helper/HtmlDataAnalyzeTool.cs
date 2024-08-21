using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HLH.Lib.Helper
{
    public class HtmlDataAnalyzeTool
    {

        /// <summary>
        /// 提取部分内容支持(*)通配符
        /// </summary>
        /// <param name="txtStart">开始标记</param>
        /// <param name="txtEnd">结束标记</param>
        /// <param name="Content">解析的内容</param>
        /// <param name="IncludeConditions">是否包含标记</param>
        /// <param name="isGreedy">是否为贪婪模式（尽可能匹配到结束位置）</param>
        /// <param name="isGetAll">是否为循环模式（采集到多个结果时，返回多个#||#号分开）</param>
        /// <returns></returns>
        [Obsolete("过时")]
        public static string GetPartsContentForTestold(string txtStart, string txtEnd, string Content, bool IncludeConditions, bool isGreedy, bool isGetAll, bool useRegx)
        {
            string rs = string.Empty;

            if (useRegx)
            {
                //这时前面是参数，后面是结果的处理
                #region  使用正则式参数处理

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容

                    #region 先提取
                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            // string strRegex = @"([sS]*)前字符串(?<content>[\s\S]*?)后字符串";
                            //string strRegex = @"(?<content><span style([\s\S]*)<strong>?)";


                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");

                            string starttxt = string.Empty;
                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符


                            string strRegex = string.Empty;
                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)?)";
                            }

                            strRegex = @starttxt.Replace("[参数]", @"([\s\S]*?)?");


                            Regex r = new Regex(strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);



                            MatchCollection m = r.Matches(Content);

                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    //通常N个参数  为N+1个组，对应的值为组[0]为全部， 组[1]->1

                                    for (int c = 0; c < m[i].Groups.Count; c++)
                                    {
                                        if (c == 0)
                                        {
                                            //这里是全部值
                                        }
                                        else
                                        {
                                            txtEnd = txtEnd.Replace("[参数" + c.ToString() + "]", m[i].Groups[c].Value);
                                            rs = txtEnd;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                // frmMain.Instance.PrintInfoLog("使用正则提取部分内容时，结果为空。");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    #endregion


                    #endregion
                }


                #endregion
            }
            else
            {
                #region  提取部分内容

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容

                    int regexIndex = 0;
                    if (!IncludeConditions)
                    {
                        regexIndex = 1;
                    }

                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            // string strRegex = @"([sS]*)前字符串(?<content>[\s\S]*?)后字符串";
                            //string strRegex = @"(?<content><span style([\s\S]*)<strong>?)";

                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");


                            string strRegex1 = "(?<content>" + txtStart.Trim() + @"([\s\S]*)" + txtEnd.Trim() + "?)";
                            string starttxt = string.Empty;

                            string endtxt = string.Empty;

                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符

                            if (txtEnd.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                endtxt = txtEnd.Trim();
                            }
                            endtxt = endtxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");//换行符

                            string strRegex = string.Empty;
                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)" + @endtxt + "?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)" + @endtxt + "?)";
                            }
                            //  string strRegex = starttxt + @"(?<content>[\s\S]*?)" + endtxt;
                            Regex r = new Regex(@strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            MatchCollection m = r.Matches(Content);

                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    string newString = m[i].Groups[regexIndex].Value;
                                    if (Content.Contains(newString))
                                    {
                                        if (newString != Content)//说明提取到了有价值的数据
                                        {
                                            if (m.Count > 1 && isGetAll)
                                            {
                                                rs += newString + "#||#";
                                            }
                                            else
                                            {
                                                rs = newString;
                                            }
                                        }
                                    }
                                    if (m.Count > 1 && isGetAll)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        break;//结果大于1时。跳出大于的结果
                                    }
                                }

                            }
                            else
                            {
                                //frmMain.Instance.PrintInfoLog("固定标签，提取部分内容时，结果为空。");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    #endregion
                }
                else
                {
                    rs = Content;//如果没有任何限制，则原文返回
                }
                rs = rs.TrimEnd('|');

                #endregion
            }
            return rs;
        }



        /// <summary>
        /// 提取部分内容支持(*)通配符 20160803优化
        /// </summary>
        /// <param name="txtStart">开始标记</param>
        /// <param name="txtEnd">结束标记</param>
        /// <param name="Content">解析的内容</param>
        /// <param name="IncludeConditions">是否包含标记</param>
        /// <param name="isGreedy">是否为贪婪模式（尽可能匹配到结束位置）</param>
        /// <param name="isGetAll">是否为循环模式（采集到多个结果时，返回多个#||#号分开）</param>
        /// <returns></returns>
        public static string GetPartsContentForTest(string txtStart, string txtEnd, string Content, bool IncludeConditions, bool isGreedy, bool isGetAll, bool useRegx)
        {
            string rs = string.Empty;

            if (useRegx)
            {
                //这时前面是参数，后面是结果的处理
                #region  使用正则式参数处理

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容
                    int regexIndex = 0;
                    if (!IncludeConditions)
                    {
                        regexIndex = 1;
                    }
                    #region 先提取
                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");

                            string starttxt = string.Empty;
                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符


                            string strRegex = string.Empty;
                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)?)";
                            }

                            strRegex = @starttxt.Replace("[参数]", @"([\s\S]*?)?");


                            Regex r = new Regex(strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);



                            MatchCollection m = r.Matches(Content);
                            string tempTxtEnd = txtEnd;
                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    //从1开始是0为带标签的
                                    for (int g = 1; g < m[i].Groups.Count; g++)
                                    {
                                        txtEnd = txtEnd.Replace("[参数" + g.ToString() + "]", m[i].Groups[g].Value);
                                    }
                                    //通常N个参数  为N+1个组，对应的值为组[0]为全部，即包括前后标签， 组[1]->1
                                    // string newString = m[i].Groups[regexIndex].Value;
                                    string newString = txtEnd;
                                    txtEnd = tempTxtEnd;
                                    if (newString != Content)//说明提取到了有价值的数据
                                    {
                                        if (m.Count > 1 && isGetAll)
                                        {
                                            rs += newString + "#||#";
                                        }
                                        else
                                        {
                                            rs = newString;
                                        }
                                    }
                                    if (m.Count > 1 && isGetAll)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //>1时。得到一个有效值，才返回
                                        if (rs.Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;//结果大于1时。跳出大于的结果
                                        }
                                    }



                                    // txtEnd = txtEnd.Replace("[参数" + c.ToString() + "]", m[i].Groups[regexIndex].Value);
                                    //  rs = txtEnd;
                                }

                            }
                            //else
                            //{
                            //    if (frmMain.InstancePicker!=null)
                            //    {
                            //        frmMain.InstancePicker.PrintInfoLog("使用正则提取部分内容时，结果为空。");
                            //    }

                            //}

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    #endregion


                    #endregion
                }


                #endregion
            }
            else
            {
                #region  提取部分内容 没有[参数]

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容

                    int regexIndex = 0;
                    if (!IncludeConditions)
                    {
                        regexIndex = 1;
                    }

                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            // string strRegex = @"([sS]*)前字符串(?<content>[\s\S]*?)后字符串";
                            //string strRegex = @"(?<content><span style([\s\S]*)<strong>?)";

                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");


                            string strRegex1 = "(?<content>" + txtStart.Trim() + @"([\s\S]*)" + txtEnd.Trim() + "?)";
                            string starttxt = string.Empty;

                            string endtxt = string.Empty;

                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符

                            if (txtEnd.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                endtxt = txtEnd.Trim();
                            }
                            endtxt = endtxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");//换行符

                            string strRegex = string.Empty;
                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)" + @endtxt + "?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)" + @endtxt + "?)";
                            }
                            //  string strRegex = starttxt + @"(?<content>[\s\S]*?)" + endtxt;
                            Regex r = new Regex(@strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            MatchCollection m = r.Matches(Content);

                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    string newString = m[i].Groups[regexIndex].Value;
                                    if (Content.Contains(newString))
                                    {
                                        if (newString != Content)//说明提取到了有价值的数据
                                        {
                                            if (m.Count > 1 && isGetAll)
                                            {
                                                rs += newString + "#||#";
                                            }
                                            else
                                            {
                                                rs = newString;
                                            }
                                        }
                                    }
                                    if (m.Count > 1 && isGetAll)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //>1时。得到一个有效值，才返回
                                        if (rs.Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;//结果大于1时。跳出大于的结果
                                        }

                                    }
                                }

                            }
                            else
                            {
                                // frmMain.InstancePicker.PrintInfoLog("固定标签，提取部分内容时，结果为空。");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    #endregion
                }
                else
                {
                    rs = Content;//如果没有任何限制，则原文返回
                }
                rs = rs.TrimEnd('|');

                #endregion
            }
            rs = rs.TrimEnd(new char[] { '#', '|', '|', '#' });
            return rs;
        }


        /// <summary>
        /// 提取部分内容支持(*)通配符 20160909优化 加入可调试返回字段，重点是正则的返回
        /// </summary>
        /// <param name="txtStart">开始标记</param>
        /// <param name="txtEnd">结束标记</param>
        /// <param name="Content">解析的内容</param>
        /// <param name="IncludeConditions">是否包含标记</param>
        /// <param name="isGreedy">是否为贪婪模式（尽可能匹配到结束位置）</param>
        /// <param name="isGetAll">是否为循环模式（采集到多个结果时，返回多个#||#号分开）</param>
        /// <returns></returns>
        public static string GetPartsContentForTest(string txtStart, string txtEnd, string Content, bool IncludeConditions, bool isGreedy, bool isGetAll, bool useRegx, out string debugInfo)
        {
            string rs = string.Empty;
            string strRegex = string.Empty;
            if (useRegx)
            {
                //这时前面是参数，后面是结果的处理
                #region  使用正则式参数处理

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容
                    int regexIndex = 0;
                    if (!IncludeConditions)
                    {
                        regexIndex = 1;
                    }
                    #region 先提取
                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            // string strRegex = @"([sS]*)前字符串(?<content>[\s\S]*?)后字符串";
                            //string strRegex = @"(?<content><span style([\s\S]*)<strong>?)";


                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");

                            string starttxt = string.Empty;
                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符



                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)?)";
                            }

                            strRegex = @starttxt.Replace("[参数]", @"([\s\S]*?)?");


                            Regex r = new Regex(strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);



                            MatchCollection m = r.Matches(Content);
                            string tempTxtEnd = txtEnd;
                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    //从1开始是0为带标签的
                                    for (int g = 1; g < m[i].Groups.Count; g++)
                                    {
                                        txtEnd = txtEnd.Replace("[参数" + g.ToString() + "]", m[i].Groups[g].Value);
                                    }
                                    //通常N个参数  为N+1个组，对应的值为组[0]为全部，即包括前后标签， 组[1]->1
                                    // string newString = m[i].Groups[regexIndex].Value;
                                    string newString = txtEnd;
                                    txtEnd = tempTxtEnd;
                                    if (newString != Content)//说明提取到了有价值的数据
                                    {
                                        if (m.Count > 1 && isGetAll)
                                        {
                                            rs += newString + "#||#";
                                        }
                                        else
                                        {
                                            rs = newString;
                                        }
                                    }
                                    if (m.Count > 1 && isGetAll)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //>1时。得到一个有效值，才返回
                                        if (rs.Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;//结果大于1时。跳出大于的结果
                                        }
                                    }



                                    // txtEnd = txtEnd.Replace("[参数" + c.ToString() + "]", m[i].Groups[regexIndex].Value);
                                    //  rs = txtEnd;
                                }

                            }
                            //else
                            //{
                            //    if (frmMain.InstancePicker!=null)
                            //    {
                            //        frmMain.InstancePicker.PrintInfoLog("使用正则提取部分内容时，结果为空。");
                            //    }

                            //}

                        }
                    }
                    catch (Exception ex)
                    {

                        throw;

                    }
                    #endregion


                    #endregion
                }


                #endregion
            }
            else
            {
                #region  提取部分内容 没有[参数]

                if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                {
                    #region 替换内容

                    int regexIndex = 0;
                    if (!IncludeConditions)
                    {
                        regexIndex = 1;
                    }

                    try
                    {
                        if (txtStart.Trim().Length > 0 && txtEnd.Trim().Length > 0)
                        {
                            // string strRegex = @"([sS]*)前字符串(?<content>[\s\S]*?)后字符串";
                            //string strRegex = @"(?<content><span style([\s\S]*)<strong>?)";

                            //特殊字符先转义
                            txtStart = txtStart.Replace("$", @"\$");
                            txtEnd = txtEnd.Replace("$", @"\$");

                            txtStart = txtStart.Replace("\r", @"\r").Replace("\n", @"\n");
                            txtEnd = txtEnd.Replace("\r", @"\r").Replace("\n", @"\n");


                            string strRegex1 = "(?<content>" + txtStart.Trim() + @"([\s\S]*)" + txtEnd.Trim() + "?)";
                            string starttxt = string.Empty;

                            string endtxt = string.Empty;

                            if (txtStart.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    starttxt = txtStart.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                starttxt = txtStart.Trim();
                            }
                            starttxt = starttxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");  //换行符

                            if (txtEnd.Trim().Contains("(*)"))
                            {
                                if (isGreedy)
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*");
                                }
                                else
                                {
                                    endtxt = txtEnd.Trim().Replace("(*)", @"[\s\S]*?");
                                }
                            }
                            else
                            {
                                endtxt = txtEnd.Trim();
                            }
                            endtxt = endtxt.Replace("\n", "\r").Replace("(", @"\(").Replace(")", @"\)");//换行符


                            //是否为贪婪模式
                            if (isGreedy)
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*)" + @endtxt + "?)";
                            }
                            else
                            {
                                strRegex = "(?<content>" + @starttxt + @"([\s\S]*?)" + @endtxt + "?)";
                            }
                            //  string strRegex = starttxt + @"(?<content>[\s\S]*?)" + endtxt;
                            Regex r = new Regex(@strRegex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            MatchCollection m = r.Matches(Content);

                            if (m.Count >= 1)
                            {
                                for (int i = 0; i < m.Count; i++)
                                {
                                    string newString = m[i].Groups[regexIndex].Value;
                                    if (Content.Contains(newString))
                                    {
                                        if (newString != Content)//说明提取到了有价值的数据
                                        {
                                            if (m.Count > 1 && isGetAll)
                                            {
                                                rs += newString + "#||#";
                                            }
                                            else
                                            {
                                                rs = newString;
                                            }
                                        }
                                    }
                                    if (m.Count > 1 && isGetAll)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //>1时。得到一个有效值，才返回
                                        if (rs.Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;//结果大于1时。跳出大于的结果
                                        }

                                    }
                                }

                            }
                            else
                            {
                                // mess("固定标签，提取部分内容时，结果为空。");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    #endregion
                }
                else
                {
                    rs = Content;//如果没有任何限制，则原文返回
                }
                rs = rs.TrimEnd('|');

                #endregion
            }
            rs = rs.TrimEnd(new char[] { '#', '|', '|', '#' });
            debugInfo = strRegex;
            return rs;
        }




        public static string RemoveHtmlForTagList(string source)
        {
            string temp = source.ToString();
            List<Regex> regList = new List<Regex>();
            Regex regFont = new Regex(@"<font[\s\S]*?>|</font>");
            regList.Add(regFont);

            Regex regStyle = new Regex("style=\"[\\s\\S]*?\"");
            regList.Add(regStyle);

            Regex regTitle = new Regex("title=\"[\\s\\S]*?\"");
            regList.Add(regTitle);

            Regex regOnmouseover = new Regex("onmouseover=\"[\\s\\S]*?\"");
            regList.Add(regOnmouseover);

            Regex regOnmouseout = new Regex("onmouseout=\"[\\s\\S]*?\"");
            regList.Add(regOnmouseout);

            foreach (Regex item in regList)
            {
                //style="[\s\S]*?"
                MatchCollection match = item.Matches(source.ToString());
                for (int i = 0; i < match.Count; i++)
                {
                    temp = temp.Replace(match[i].Groups[0].Value, "");
                }
            }

            temp = FilterHTML(temp);
            return temp;
        }

        public static string RemoveHtmlForFont(string source)
        {
            string temp = source.ToString();
            Regex reg = new Regex(@"<font[\s\S]*?>|</font>");

            //style="[\s\S]*?"
            MatchCollection match = reg.Matches(source.ToString());

            for (int i = 0; i < match.Count; i++)
            {
                temp = temp.Replace(match[i].Groups[0].Value, "");
            }
            return temp;
        }

        public static string RemoveHtmlForStyle(string source)
        {
            string temp = source.ToString();
            Regex reg = new Regex("style=\"[\\s\\S]*?\"");

            //style="[\s\S]*?"
            MatchCollection match = reg.Matches(source.ToString());

            for (int i = 0; i < match.Count; i++)
            {
                temp = temp.Replace(match[i].Groups[0].Value, "");
            }
            return temp;
        }

        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            // Htmlstring.Replace("\r\n", "");
            // Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }


        /// <summary>  
        /// 正则双重过滤  
        /// </summary>  
        /// <param name="content"></param>  
        /// <param name="splitKey1"></param>  
        /// <param name="splitKey2"></param>  
        /// <param name="newChars"></param>  
        /// <returns></returns>  
        private static string GetReplace(string content, string splitKey1, string splitKey2, string newChars)
        {
            //splitKey1 第一个正则式匹配  

            //splitKey2 匹配结果中再次匹配进行替换  

            if (splitKey1 != null && splitKey1 != "" && splitKey2 != null && splitKey2 != "")
            {
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(splitKey1);
                System.Text.RegularExpressions.MatchCollection mc = rg.Matches(content);

                foreach (System.Text.RegularExpressions.Match mc1 in mc)
                {
                    string oldChar = mc1.ToString();
                    string newChar = new System.Text.RegularExpressions.Regex(splitKey2, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace(oldChar, newChars);
                    content = content.Replace(oldChar, newChar);
                }
                return content;
            }
            else
            {
                if (splitKey2 != null && splitKey2 != "")
                {
                    System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(splitKey2, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    return rg.Replace(content, newChars);
                }
            }
            return content;
        }


        /*此处过滤危险HTML方法*/
        public static string FilterHTML(string html)
        {
            if (html == null)
                return "";

            #region 过滤 style
            System.Text.RegularExpressions.Regex regex_style1 = new System.Text.RegularExpressions.Regex("(<style[//s//S]*?///style//s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_style2 = new System.Text.RegularExpressions.Regex("(<(style[//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_style1.Replace(html, "");
            html = regex_style2.Replace(html, "");
            #endregion

            #region 过滤 script
            System.Text.RegularExpressions.Regex regex_script1 = new System.Text.RegularExpressions.Regex("(<script[//s//S]*?///script//s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_script2 = new System.Text.RegularExpressions.Regex("(<(script[//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_script1.Replace(html, "");
            html = regex_script1.Replace(html, "");
            #endregion

            #region 过滤 <iframe> 标签
            System.Text.RegularExpressions.Regex regex_iframe1 = new System.Text.RegularExpressions.Regex("(<iframe [//s//S]+<iframe//s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_iframe2 = new System.Text.RegularExpressions.Regex("(<(iframe [//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_iframe1.Replace(html, "");
            html = regex_iframe2.Replace(html, "");
            #endregion

            #region 过滤 <frameset> 标签
            System.Text.RegularExpressions.Regex regex_frameset1 = new System.Text.RegularExpressions.Regex("(<frameset [//s//S]+<frameset //s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frameset2 = new System.Text.RegularExpressions.Regex("(<(frameset [//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frameset1.Replace(html, "");
            html = regex_frameset2.Replace(html, "");
            #endregion

            #region 过滤 <frame> 标签
            System.Text.RegularExpressions.Regex regex_frame1 = new System.Text.RegularExpressions.Regex("(<frame[//s//S]+<frame //s*>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_frame2 = new System.Text.RegularExpressions.Regex("(<(frame[//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_frame1.Replace(html, "");
            html = regex_frame2.Replace(html, "");
            #endregion

            #region 过滤 <form> 标签
            System.Text.RegularExpressions.Regex regex_form1 = new System.Text.RegularExpressions.Regex("(<(form [//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex_form2 = new System.Text.RegularExpressions.Regex("(<(/form[//s//S]*?)>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex_form1.Replace(html, "");
            html = regex_form2.Replace(html, "");
            #endregion

            #region 过滤 on: 的事件

            //过滤on 带单引号的 过滤on  带双引号的 过滤on 不带有引号的  
            // html = GetReplace(html, @"<[//s//S]+ (on)[a-zA-Z]{4,20} *= *[//S ]{3,}>",@"((on)[a-zA-Z]{4,20} *= *'[^']{3,}')|((on)[a-zA-Z]{4,20} *= */"[^/"]{3,}/")|((on)[a-zA-Z]{4,20} *= *[^>/ ]{3,})", "");  

            #endregion 过滤 on: 的事件

            #region 过滤 javascript: 的事件

            //  html = GetReplace(html, @"<[//s//S]+ (href|src|background|url|dynsrc|expression|codebase) *= *[ /"/']? *(javascript:)[//S]{1,}>"          ,@ "(' *(javascript|vbscript):([//S^'])*')|(/" *(javascript|vbscript):[//S^/"]*/")|([^=]*(javascript|vbscript):[^/> ]*)", "#");   
            #endregion 过滤 javascript: 的事件

            return html;
        }
    }



}
