using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HLH.Lib.Helper;



namespace CommonProcess.StringProcess
{
    [Serializable]
    public partial class UC正则式提取 : UCMyBase, IUCBase
    {
        public UC正则式提取()
        {
            InitializeComponent();
        }


        public object OtherEventParameters { get; set; }

        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(UC正则式提取 uc, object Parameters);


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void LoadProcessColumns(DataGridView dataGridView1)
        {
            DataTable dt = new DataTable();
            dt = dataGridView1.DataSource as DataTable;

            List<string> tclist = new List<string>();

            foreach (DataColumn field in dt.Columns)
            {
                tclist.Add(field.Caption);
            }
            //加载表的字段
            cmbField.Items.Clear();
            HLH.Lib.Helper.DropDownListHelper.InitDropList(tclist, cmbField, true);

        }


        public void LoadProcessColumns(DataTable dt)
        {
            //DataTable dt = new DataTable();
            // dt = dataGridView1.DataSource as DataTable;

            List<string> tclist = new List<string>();

            foreach (DataColumn field in dt.Columns)
            {
                tclist.Add(field.Caption);
            }
            //加载表的字段
            cmbField.Items.Clear();
            HLH.Lib.Helper.DropDownListHelper.InitDropList(tclist, cmbField, true);

        }

        private string debugInfo = string.Empty;

        public string ProcessString(string SourceString)
        {
            string rs = SourceString;

            if (txtStart.Text.Trim().Length > 0 || txtEnd.Text.Trim().Length > 0)
            {
                if (!chk正则模式.Checked)
                {
                    if (txtStart.Text.Contains("[参数]") || txtEnd.Text.Contains("[参数1]"))
                    {
                        printInfoMessage("标记中带有[参数]，必须选取【正则参数模式】");
                    }
                }

                #region 正则匹配 去掉多余内容 应该针对 描述

                string resultForReg = string.Empty;
                string inputContent = string.Empty;
                string resultForRegCopy = string.Empty;

                inputContent = rs;


                resultForReg = HtmlDataAnalyzeTool.GetPartsContentForTest(txtStart.Text, txtEnd.Text, rs, chk是否包含开始结束的标记.Checked, chk是否为贪婪模式.Checked, chk循环匹配.Checked, chk正则模式.Checked);
                //
                if (resultForReg.Trim().Length > 0 && chk循环匹配.Checked)
                {
                    //string[] rssz = resultForReg.Split(new string[] { "#||#" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] rssz = resultForReg.Split(new string[] { txt循环分割字符.Text }, StringSplitOptions.RemoveEmptyEntries);
                    lblrsInfo.Text = "循环结果数:" + rssz.ToString();
                }


                if (resultForReg.Trim().Length == 0)
                {
                    //
                }
                else
                {
                    string newString = string.Empty;
                    if (rdb去掉提取部分.Checked)
                    {
                        newString = rs.Replace(resultForReg, "");
                    }
                    if (rdb保留提取部分.Checked)
                    {
                        newString = resultForReg;
                    }

                    if (chk提取结果替换.Checked)
                    {
                        resultForRegCopy = txt提取结果替换.Text.Replace("[结果]", newString);
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
            if (chk结果去首尾空格.Checked)
            {
                rs = rs.Trim();
            }
            return rs;
        }

        private void lklblhart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtStart.SelectedText.Trim().Length > 0)
            {
                txtStart.Text = txtStart.Text.Replace(txtStart.SelectedText, "[参数]");
            }
            else
            {
                //在光标位置插入

                txtStart.Text = txtStart.Text.Insert(txtStart.SelectionStart, "[参数]");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtEnd.SelectedText.Trim().Length > 0)
            {
                txtEnd.Text = txtEnd.Text.Replace(txtEnd.SelectedText, "[参数1]");
            }
            else
            {
                //在光标位置插入

                txtEnd.Text = txtEnd.Text.Insert(txtEnd.SelectionStart, "[参数1]");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtEnd.SelectedText.Trim().Length > 0)
            {
                txtEnd.Text = txtEnd.Text.Replace(txtEnd.SelectedText, "[参数2]");
            }
            else
            {
                //在光标位置插入

                txtEnd.Text = txtEnd.Text.Insert(txtEnd.SelectionStart, "[参数2]");
            }
        }


        public string KeyID { get; set; }

        private void chk正则模式_CheckedChanged(object sender, EventArgs e)
        {
            lklblhart.Visible = chk正则模式.Checked;
            groupBox使用正则式.Visible = chk正则模式.Checked;
            if (chk正则模式.Checked)
            {
                txtStart.EmptyTextTip = "<title>Suppliers, (*), [参数]</title>";
                txtEnd.EmptyTextTip = "补前[参数1]补后";

            }
            else
            {
                txtStart.EmptyTextTip = "【开始标记】";
                txtEnd.EmptyTextTip = "【结束标记】";
            }

        }

        private void btnDebugRegex_Click(object sender, EventArgs e)
        {
            if (OtherEvent != null)
            {
                string rs = HtmlDataAnalyzeTool.GetPartsContentForTest(txtStart.Text, txtEnd.Text, "tyutttu", chk是否包含开始结束的标记.Checked, chk是否为贪婪模式.Checked, chk循环匹配.Checked, chk正则模式.Checked, out debugInfo);
                OtherEventParameters = debugInfo;
                OtherEvent(this, OtherEventParameters);
            }
        }



        public void SaveDataFromUI(UCBasePara Para)
        {
            UC正则式提取Para para = new UC正则式提取Para();
            para = Para as UC正则式提取Para;
            para.CycleMatch = chk循环匹配.Checked;
            para.Endflag = txtEnd.Text;
            para.IncludeStartEndStr = chk是否包含开始结束的标记.Checked;
            para.Is贪婪模式 = chk是否为贪婪模式.Checked;
            para.Startflag = txtStart.Text;
            para.UseRegularMatch = chk正则模式.Checked;
            para.循环匹配时去重复 = chk循环匹配时去重复.Checked;
            para.保留还是去掉提取的内容 = rdb保留提取部分.Checked;
            para.txt循环分割字符 = txt循环分割字符.Text;
            para.提取结果替换 = chk提取结果替换.Checked;
            para.Str提取结果替换 = txt提取结果替换.Text;


        }

        public void LoadDataToUI(UCBasePara Para)
        {
            UC正则式提取Para para = new UC正则式提取Para();
            para = Para as UC正则式提取Para;
            chk循环匹配时去重复.Checked = para.循环匹配时去重复;
            chk是否包含开始结束的标记.Checked = para.IncludeStartEndStr;
            chk循环匹配.Checked = para.CycleMatch;
            chk正则模式.Checked = para.UseRegularMatch;
            txtStart.Text = para.Startflag;
            txtEnd.Text = para.Endflag;
            chk是否为贪婪模式.Checked = para.Is贪婪模式;
            rdb保留提取部分.Checked = para.保留还是去掉提取的内容;

            txt循环分割字符.Text = para.txt循环分割字符;
            chk提取结果替换.Checked = para.提取结果替换;
            txt提取结果替换.Text = para.Str提取结果替换;
        }

        private void linkLabel通配符_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtStart.SelectedText.Trim().Length > 0)
            {
                txtStart.Text = txtStart.Text.Replace(txtStart.SelectedText, "(*)");
            }
            else
            {
                //在光标位置插入

                txtStart.Text = txtStart.Text.Insert(txtStart.SelectionStart, "(*)");
            }
        }
    }
}
