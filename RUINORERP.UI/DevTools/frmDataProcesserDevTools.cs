using SMTAPI.Lib;
using NSoup;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;
using SMTAPI.Entity;
using Maticsoft.DBUtility;
using HtmlAgilityPack;
using System.Runtime.Remoting.Messaging;
using CommonProcess.StringProcess;
using MySqlEntity;

//NSoup 里引用了  3.5v的 System.Core 所以复制到这里 也引用了 发布时需要测试是否只安装 2.0的，能不能正常使用

namespace SMTAPI.ToolForm
{
    /// <summary>
    ///  将原来的一个功能抽出来公用为功能性修改数据的工具
    /// </summary>
    public partial class frmDataProcesserDevTools : frmBase
    {
        public frmDataProcesserDevTools()
        {
            InitializeComponent();
            _main = this;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private static frmDataProcesserDevTools _main;
        internal static frmDataProcesserDevTools Instance
        {
            get { return _main; }
        }


        List<string> Modules = new List<string>();

        #region 画行号

        private void DataGridShow_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        #endregion

        private List<KeyValuePair<string, string>> str双字节 = new List<KeyValuePair<string, string>>();
        ///随便用一个实体
        tbProductStockEntity entity = new tbProductStockEntity();

        private bool IsMysql = false;

        private void frmDataProcesser_Load(object sender, EventArgs e)
        {
            if (rdbMySQL.Checked)
            {
                IsMysql = true;
            }
            Modules.Add("检查HTML标签完整性");
            Modules.Add("字符修补替换");
            Modules.Add("字符提取替换");
            Modules.Add("设置指定字段值");
            Modules.Add("检测替换特殊字符");
            Modules.Add("自动补全标签");
            Modules.Add("HTML结构分析式处理");
            Modules.Add("智能打包处理");
            HLH.Lib.Helper.DropDownListHelper.InitDropList(Modules, cmbProcessTemplates, true);
            //加载处理的方法模块

            List<string> typelist = new List<string>();
            typelist.Add("请选择");
            //MultiUser.Instance

            DataSet ds = new DataSet();
            if (IsMysql)
            {
                ds = entity.GetDataSet("  select table_name from information_schema.tables where table_schema = 'MAXRUINORNEW'; ");
            }
            else
            {
                ds = entity.GetDataSet("  Select Name From SysObjects Where XType='U' order By Name ");
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                typelist.Add(dr[0].ToString());
            }
            HLH.Lib.Helper.DropDownListHelper.InitDropList(typelist, cmbtables, false);

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
            //查询完后，载入列
            LoadProcessColumns();
        }


        private void LoadProcessColumns()
        {
            string oldSelectItem = string.Empty;
            if (cmbColumns.SelectedItem != null)
            {
                oldSelectItem = cmbColumns.SelectedItem.ToString();
            }

            DataTable dt = new DataTable();
            dt = dataGridView1.DataSource as DataTable;


            List<string> tclist = new List<string>();

            foreach (DataColumn field in dt.Columns)
            {
                tclist.Add(field.Caption);
            }
            //加载表的字段
            cmbColumns.Items.Clear();

            HLH.Lib.Helper.DropDownListHelper.InitDropList(tclist, cmbColumns, true);

            cmbColumns.SelectedIndex = cmbColumns.FindString(oldSelectItem);
        }

        public void Query()
        {
            StringBuilder Conditions = new StringBuilder();
            Conditions.Append(" 1=1 ");




            if (chkSQL.Checked)
            {
                if (txtCSQL.Text.Trim().Length > 0)
                {
                    if (txtCSQL.Text.Trim().ToLower().StartsWith("and"))
                    {
                        Conditions.Append(txtCSQL.Text.Trim());
                    }
                    else
                    {
                        Conditions.Append(" and  " + txtCSQL.Text.Trim());
                    }
                }
            }


            if (cmbtables.SelectedItem != null && cmbtables.SelectedItem.ToString() != "请选择")
            {

                string tableName = cmbtables.SelectedItem.ToString();
                DataSet dsContent = new DataSet();
                if (IsMysql)
                {
                    if (chkTop.Checked)
                    {
                        dsContent = entity.GetDataSet(string.Format("select  * from {0} where {1} LIMIT 0,{2};", tableName, Conditions.ToString(), numericUpDown1.Value));
                    }
                    else
                    {
                        dsContent = entity.GetDataSet(string.Format("select * from {0} where {1}", tableName, Conditions.ToString()));
                    }
                }
                else
                {
                    if (chkTop.Checked)
                    {
                        dsContent = entity.GetDataSet(string.Format("select top {0} * from {1} where {2}", numericUpDown1.Value, tableName, Conditions.ToString()));
                    }
                    else
                    {
                        dsContent = entity.GetDataSet(string.Format("select * from {0} where {1}", tableName, Conditions.ToString()));
                    }
                }

                dataGridView1.DataSource = dsContent.Tables[0];

                PrintInfoLog("查询内容表行数:" + dsContent.Tables[0].Rows.Count.ToString());
            }

        }






        //本窗体很复杂

        //动态对字段作不同处理 1)补前 补后  补中间，去前后中间 去特殊字符，去正则式  区间操作




        /// <summary>
        /// 计算数值的方法
        /// </summary>
        /// <param name="oldStr"></param>
        /// <returns></returns>
        public string ProcessForDecimal(string sourceString, bool isSmart, string times)
        {

            decimal oldvalue = 0;
            if (!decimal.TryParse(sourceString, out oldvalue))
            {
                frmMain.Instance.PrintInfoLog("当前选择的字段值，转换为 数字类型时出错。");
                return sourceString;
            }


            if (isSmart)
            {
                #region 智能分析
                frmMain.Instance.PrintInfoLog("当前没有实现这个方法，请使用手工指定的方式。");
                return sourceString;
                #endregion
            }
            else
            {
                #region 手工指定
                int thisTimes = 0;
                if (!int.TryParse(times, out thisTimes))
                {
                    frmMain.Instance.PrintInfoLog("手工指定的倍数必须为int数值型。");
                    return sourceString;
                }
                oldvalue = oldvalue * thisTimes;
                #endregion

            }



            return oldvalue.ToString();
        }







        /// <summary>
        /// 检查标签完整性
        /// </summary>
        /// <param name="products_description"></param>
        /// <returns></returns>
        public static string SmartProcessDescription(string products_description)
        {
            string rs = string.Empty;
            //思路 用正则，分析出 指定结构 （通常 为 table  p）  ，如果结构 中 没有任何实际 字符，则认为可以去掉。
            //思路 用正则，分析出 指定结构 （通常 为 table  p）  ，如果结构 中 没有任何实际 字符，则认为可以去掉。
            products_description = products_description.Replace("&nbsp;&nbsp;", "&nbsp;");
            products_description = products_description.Replace("&nbsp;", " ");
            //去掉注释
            rs = products_description;
            rs = Regex.Replace(rs, @"<!--[\s\S]*?-->", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //去掉脚本
            string s = "<script[\\s\\S]*?</script>";
            MatchCollection ms = Regex.Matches(rs, s, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match m in ms)
            {
                rs = rs.Replace(m.Value, "");
            }


            #region Nsoup.dll功能

            NSoup.Nodes.Document doc = NSoupClient.Parse(rs);

            Elements ele = doc.GetAllElements();

            //对于标签，需要去掉的属性
            List<string> attrForRemove = new List<string>();
            attrForRemove.Add("width");
            attrForRemove.Add("height");
            attrForRemove.Add("cellpadding");
            attrForRemove.Add("cellspacing");
            attrForRemove.Add("font-family");
            //  attrForRemove.Add("style");
            attrForRemove.Add("face");
            attrForRemove.Add("border");
            attrForRemove.Add("class");
            //如果标签中没有实际有效内容，则去掉
            List<string> tagForRemove = new List<string>();
            tagForRemove.Add("table");
            tagForRemove.Add("tbody");
            tagForRemove.Add("p");
            tagForRemove.Add("span");
            tagForRemove.Add("div");
            tagForRemove.Add("img");
            tagForRemove.Add("tr");
            tagForRemove.Add("td");
            tagForRemove.Add("meta");
            tagForRemove.Add("title");
            tagForRemove.Add("font");
            //tagForRemove.Add("")


            foreach (Element item in ele)
            {
                if (item.TagName() == "span")
                {

                }
                foreach (string t in tagForRemove)
                {
                    if (item.NodeName == t)
                    {
                        if (item.Text().Trim().Length == 0)
                        {
                            item.Remove();
                        }
                    }
                }
                foreach (string a in attrForRemove)
                {
                    if (item.Attributes.ContainsKey(a))
                    {
                        item.RemoveAttr(a);
                    }
                }
            }

            string htmlC = doc.Html();
            rs = htmlC;

            #endregion

            //以下主要为了去掉 html head body等其它功能，上面已经更方便的实现
            List<string> rexList = new List<string>();
            rexList.Add(@"<tbody[\s\S]*?>[\s\S]*?</tbody[\s\S]*?>");
            rexList.Add(@"<form[\s\S]*?>[\s\S]*?</form[\s\S]*?>");
            rexList.Add(@"<table[\s\S]*?>[\s\S]*?</table[\s\S]*?>");
            rexList.Add(@"<p[\s\S]*?>[\s\S]*?</p[\s\S]*?>");
            rexList.Add(@"<span[\s\S]*?>[\s\S]*?</span[\s\S]*?>");
            rexList.Add(@"<head[\s\S]*?>|</head[\s\S]*?>");
            rexList.Add(@"<html[\s\S]*?>|</html[\s\S]*?>");
            rexList.Add(@"<body[\s\S]*?>|</body[\s\S]*?>");
            rexList.Add(@"<title[\s\S]*?>[\s\S]*?</title[\s\S]*?>");
            rexList.Add(@"<o:p[\s\S]*?>|</o:p[\s\S]*?>");


            foreach (string item in rexList)
            {
                Regex r = new Regex(item, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                MatchCollection m = r.Matches(htmlC);
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
                                if (m[i].Groups[c].Value.ToString().Trim().Length > 0)
                                {
                                    string subContent = m[i].Groups[c].Value.ToString();

                                    //取到表时，再去掉所有html标签，查看是否有实际有意义的内容。如果没有。则替换为空
                                    string newSubContent = Regex.Replace(subContent, @"<.*?>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                    if (newSubContent.Trim().Length == 0)
                                    {
                                        //认为是没有
                                        rs = rs.Replace(subContent, "");
                                    }
                                    if (item == "<title[\\s\\S]*?>[\\s\\S]*?</title[\\s\\S]*?>")
                                    {
                                        //直接去掉
                                        rs = rs.Replace(subContent, "");
                                    }
                                }

                            }
                            else
                            {
                                // txtEnd = txtEnd.Replace("[参数" + c.ToString() + "]", m[i].Groups[c].Value);
                            }
                        }
                    }

                }
            }

            return rs.Trim();
        }


        //201610 10 优化 
        private void cmbProcessTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel4UC.Controls.Clear();
            Control myuc = new Control();
            if (panel4UC.Controls.Count == 1)
            {
                myuc = panel4UC.Controls[0];
            }
            if (myuc.Text == cmbProcessTemplates.SelectedItem.ToString())
            {
                //不要重新实例化控件
                return;
            }
            Control uc = new Control();
            switch (cmbProcessTemplates.SelectedItem.ToString())
            {
                case "检查HTML标签完整性":
                    //rs = SmartProcessDescription(SourceString);
                    break;
                case "字符修补替换":
                    uc = new UCRepairString();

                    break;

                case "字符提取替换":
                    uc = new UC正则式提取();
                    break;
                case "设置指定字段值":

                    UCSetSpecFieldValue ucc = new UCSetSpecFieldValue();
                    ucc.LoadProcessColumns(dataGridView1);
                    panel4UC.Controls.Add(ucc);

                    break;

                case "检测替换特殊字符":
                    uc = new UCFindSpecialChar();
                    break;


                case "智能打包处理":
                    //实质就是对数值类型的数据计算
                    uc = new UCSmartPackaging();
                    break;
                case "自动补全标签":
                    //没有界面显示，下面的方法中已经处理
                    break;

                case "HTML结构分析式处理":
                    UCHTMLStructuralAnalysis uchtml = new UCHTMLStructuralAnalysis();
                    uchtml.OtherEvent += uchtml_OtherEvent;
                    panel4UC.Controls.Add(uchtml);
                    break;


                default:
                    break;
            }
            if (panel4UC.Controls.Count == 0)
            {
                panel4UC.Controls.Add(uc);
            }

            panel4UC.Controls[0].Text = cmbProcessTemplates.SelectedItem.ToString();


        }

        void uchtml_OtherEvent(UCHTMLStructuralAnalysis uc, object Parameters)
        {


            if (cmbColumns.SelectedItem == null || cmbProcessTemplates.SelectedItem == null || cmbColumns.SelectedItem.ToString() == "请选择" || cmbProcessTemplates.SelectedItem.ToString() == "请选择")
            {
                MessageBox.Show("请选择要操作的字段或处理方法");
                return;
            }

            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count > 0)
            {

                string SourceString = string.Empty;

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    // string ID = dr.Cells["ID"].Value.ToString();
                    SourceString = dr.Cells[cmbColumns.SelectedItem.ToString()].Value.ToString();
                    break;
                }

                DateTime _startTime = System.DateTime.Now;
                UCHTMLStructuralAnalysis uchtml = panel4UC.Controls[0] as UCHTMLStructuralAnalysis;
                //  uchtml.LoadHtmlData(SourceString);
                uchtml.LoadHtmlDataByHtmlAgilityPack(SourceString);
                // uchtml.LoadHtmlDataByXpath(SourceString);

                TimeSpan span = DateTime.Now - _startTime;

                PrintInfoLog("数据处理完成," + "消耗时间：" + span.TotalSeconds.ToString() + "秒", Color.Green);
            }
            else
            {
                MessageBox.Show("请选择要加载数据行。");
            }
        }





        private string ProcessData(string Key, string SourceString, DataGridViewRow dr)
        {
            string rs = SourceString;
            switch (cmbProcessTemplates.SelectedItem.ToString())
            {

                case "智能打包处理":
                    UCSmartPackaging ucsp = panel4UC.Controls[0] as UCSmartPackaging;
                    rs = ProcessForDecimal(SourceString, ucsp.isSmart, ucsp.txtPackagingQty.Text);
                    break;

                case "检查HTML标签完整性":
                    rs = SmartProcessDescription(SourceString);
                    break;

                case "字符修补替换":
                    UCRepairString ucr = panel4UC.Controls[0] as UCRepairString;
                    ucr.PrintProcessResult = chk显示处理结果.Checked;

                    rs = ucr.ProcessString(SourceString);
                    break;
                case "字符提取替换":
                    UC正则式提取 uce = panel4UC.Controls[0] as UC正则式提取;
                    //uce.PrintProcessResult = chk显示处理结果.Checked;
                    uce.KeyID = Key;
                    rs = uce.ProcessString(SourceString);
                    break;

                case "设置指定字段值":
                    UCSetSpecFieldValue ucc = panel4UC.Controls[0] as UCSetSpecFieldValue;
                    //uce.PrintProcessResult = chk显示处理结果.Checked;
                    if (ucc.rdb来源表中字段值.Checked)
                    {
                        if (ucc.cmbtarget.SelectedItem != null && ucc.cmbtarget.SelectedItem.ToString() != "请选择" && dr.Cells[ucc.cmbtarget.SelectedItem.ToString()].Value != null)
                        {
                            rs = dr.Cells[ucc.cmbtarget.SelectedItem.ToString()].Value.ToString();
                            if (ucc.chk选择字段修补.Checked)
                            {
                                rs = ucc.txt选择结果替换.Text.Replace("[结果]", rs);
                            }
                        }
                        else
                        {
                            rs = string.Empty;
                        }
                    }

                    if (ucc.rdb设置为固定字符.Checked)
                    {
                        rs = ucc.txt固定值.Text;
                    }
                    break;

                case "检测替换特殊字符":

                    UCFindSpecialChar udfinder = panel4UC.Controls[0] as UCFindSpecialChar;
                    udfinder.KeyID = Key;
                    rs = udfinder.ProcessString(SourceString);
                    break;

                case "自动补全标签":
                    //SourceString = SourceString;
                    //HtmlWeb htmlWeb = new HtmlWeb();
                    //HtmlAgilityPack.HtmlDocument htmlDoc = htmlWeb.Load(url)
                    //HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                    //string viewStateValue = htmlNode.Attributes["value"].Value;
                    //htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                    //string eventValidation = htmlNode.Attributes["value"].Value;
                    //htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@type='submit']");
                    //string submitName = htmlNode.Attributes["name"].Value;

                    // HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    // doc.LoadHtml(SourceString);

                    Winista.Text.HtmlParser.Parser parser = new Winista.Text.HtmlParser.Parser();
                    parser.InputHTML = SourceString;
                    Winista.Text.HtmlParser.Util.NodeList htmlNodes = parser.Parse(null);
                    string completeHtmlText = htmlNodes.AsHtml();
                    rs = completeHtmlText;
                    break;

                case "HTML结构分析式处理":

                    UCHTMLStructuralAnalysis uchtml = panel4UC.Controls[0] as UCHTMLStructuralAnalysis;
                    rs = uchtml.ProcessString(SourceString);
                    break;

                default:
                    break;
            }
            return rs;
        }




        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (cmbColumns.SelectedItem == null || cmbProcessTemplates.SelectedItem == null || cmbColumns.SelectedItem.ToString() == "请选择" || cmbProcessTemplates.SelectedItem.ToString() == "请选择")
            {
                MessageBox.Show("请选择要操作的字段或处理方法");
                return;
            }

            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count > 0)
            {

                //使用事务处理，加快速度
                Hashtable sqlList = new Hashtable();
                StringBuilder sbForLog = new StringBuilder();

                //因为更新的数据会有特殊字符 得用参数模式
                List<KeyValuePair<string, List<IDataParameter>>> sqlListByParameter = new List<KeyValuePair<string, List<IDataParameter>>>();

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    string ID = dr.Cells["ID"].Value.ToString();
                    string SourceString = dr.Cells[cmbColumns.SelectedItem.ToString()].Value.ToString();



                    string rs = ProcessData(ID, SourceString, dr);


                    if (rs.Equals(SourceString))
                    {
                        if (chk显示处理结果.Checked)
                        {
                            sbForLog.Append(string.Format("数据行:【{0}】中，列【{1}】值处理前后结果没有变化，作忽略处理。", ID, cmbColumns.SelectedItem.ToString())).Append("\r\n"); ;
                        }
                        continue;
                    }
                    if (chk显示处理结果.Checked)
                    {
                        sbForLog.Append("\r\n").Append("数据行:【" + ID + "】处理前结果为:");
                        sbForLog.Append(SourceString);

                        sbForLog.Append("\r\n").Append("数据行:【" + ID + "】处理后结果为:");
                        sbForLog.Append(rs);
                    }
                    if (chkSave.Checked)
                    {
                        if (IsMysql)
                        {
                            List<IDataParameter> paras = new List<IDataParameter>();
                            IDataParameter para1 = new MySql.Data.MySqlClient.MySqlParameter();
                            para1.DbType = DbType.String;
                            para1.ParameterName = cmbColumns.SelectedItem.ToString();
                            para1.Value = rs;
                            paras.Add(para1);


                            KeyValuePair<string, List<IDataParameter>> KV = new KeyValuePair<string, List<IDataParameter>>(" update " + cmbtables.SelectedItem.ToString() + "   set " + cmbColumns.SelectedItem.ToString() + "=" + "?" + cmbColumns.SelectedItem.ToString() + " where ID=" + ID + " ", paras);
                            sqlListByParameter.Add(KV);


                        }
                        else
                        {


                            //更新

                            //使用参数模式，因为 可能有些字段 如描述中有特别的字符。用sql出错
                            List<System.Data.SqlClient.SqlParameter> iData = new List<System.Data.SqlClient.SqlParameter>();
                            System.Data.SqlClient.SqlParameter newValue = new System.Data.SqlClient.SqlParameter("@" + cmbColumns.SelectedItem.ToString(), DbType.String);
                            newValue.Value = rs;
                            iData.Add(newValue);
                            string sql = " update " + cmbtables.SelectedItem.ToString() + "   set " + cmbColumns.SelectedItem.ToString() + "=" + "@" + cmbColumns.SelectedItem.ToString() + " where ID=" + ID + " ";
                            sqlList.Add(sql, iData.ToArray());
                        }
                        //SQLliteHelper.ExecuteNonQuery(" update SMTProductBatchUpload   set " + cmbColumns.SelectedItem.ToString() + "=" + "@" + cmbColumns.SelectedItem.ToString() + " where v_products_model=" + v_products_model + " ", iData.ToArray());
                    }
                }

                if (chkSave.Checked)
                {
                    if (IsMysql)
                    {
                        tbAccountlistEntity entity = new tbAccountlistEntity();
                        entity.ExecuteTransactionByParameter(sqlListByParameter);
                    }
                    else
                    {
                        DbHelperSQL.ExecuteSqlTran(sqlList);
                    }
                }


                PrintInfoLog(sbForLog.ToString(), Color.Red);
                PrintInfoLog("数据处理完成,共" + sqlList.Count + "行。", Color.Green);
            }
            else
            {
                MessageBox.Show("请选择要操作的数据行。");
            }
        }



        #region 打印日志
        public void PrintInfoLog(string msg)
        {
            try
            {
                this.btnProcess.Invoke(new EventHandler(delegate
                {
                    if (myRichTextBox1.richTextBox1.Lines.Length > 3000)
                    {
                        msg = "显示数据太长，不再显示。";
                    }
                    if (myRichTextBox1.richTextBox1.Lines.Length > 3001)
                    {
                        return;
                    }


                    myRichTextBox1.richTextBox1.SelectionColor = Color.Black;
                    myRichTextBox1.richTextBox1.AppendText(msg);
                    myRichTextBox1.richTextBox1.AppendText("\r\n");
                }
                ));
            }
            catch (Exception)
            {

            }
        }

        public void PrintInfoLog(string msg, Color c)
        {
            try
            {
                this.btnProcess.BeginInvoke(new EventHandler(delegate
                {
                    if (myRichTextBox1.richTextBox1.Lines.Length > 3000)
                    {
                        msg = "显示数据太长，不再显示。";
                    }
                    if (myRichTextBox1.richTextBox1.Lines.Length > 3001)
                    {
                        return;
                    }

                    myRichTextBox1.richTextBox1.SelectionColor = c;
                    myRichTextBox1.richTextBox1.AppendText(msg);
                    myRichTextBox1.richTextBox1.AppendText("\r\n");
                }
                 ));
            }
            catch (Exception)
            {

            }
        }

        #endregion



        private void 查看采集原始页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string url = dataGridView1.CurrentRow.Cells["参考网址"].Value.ToString();
                System.Diagnostics.Process.Start(url);
            }
        }

        private void 删除选择数据行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                if (MessageBox.Show("本操作是不能恢复的,同时对应产品图片也会删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    List<string> sqlList = new List<string>();
                    List<string> ImagesPath = new List<string>();
                    foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                    {
                        string ID = dr.Cells["ID"].Value.ToString();
                        string sql = string.Format("delete from  " + cmbtables.SelectedItem.ToString() + "  where ID ={0} ", ID);
                        sqlList.Add(sql);
                        string img = dr.Cells["v_products_image"].Value.ToString();
                        string[] images = img.Split(new string[] { "#||#" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in images)
                        {
                            if (System.IO.File.Exists(item))
                            {
                                ImagesPath.Add(item);
                            }
                        }
                    }
                    Maticsoft.DBUtility.DbHelperSQL.ExecuteSqlTran(sqlList);
                    //删除图片
                    foreach (string item in ImagesPath)
                    {
                        System.IO.File.Delete(item);
                        PrintInfoLog("成功删除图片：" + item);
                    }

                    Query();
                }
            }
        }

        private void cmbColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColumns.SelectedItem != null)
            {
                Clipboard.SetDataObject(cmbColumns.SelectedItem.ToString(), true);
                PrintInfoLog(cmbColumns.SelectedItem.ToString() + ",已经在剪贴版中.");
            }


        }

        private void 复制商品描述ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string v_products_model = dataGridView1.CurrentRow.Cells["v_products_model"].Value.ToString();
                Model.SMTProductBatchUpload entity = new Model.SMTProductBatchUpload();
                entity = new BLL.SMTProductBatchUpload().GetModel(v_products_model);
                Clipboard.SetDataObject(entity.v_products_description_1);
                PrintInfoLog("商品描述已经在成功复制在剪贴板中");
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.CurrentRow != null)
                {
                    //string sku = dataGridView1.CurrentRow.Cells["v_products_model"].Value.ToString();
                    //frmSMTProductEdit edit = new frmSMTProductEdit(sku);
                    //edit.ShowDialog();
                }
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel选择处理字段_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gb选择处理方法_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chk显示处理结果_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbl1_Click(object sender, EventArgs e)
        {

        }

        private void chkSave_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4UC_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void cmbtables_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void chkSQL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtCSQL_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkTop_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer数据表显示区_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void myRichTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void rdbMSSQL_CheckedChanged(object sender, EventArgs e)
        {
            IsMysql = rdbMySQL.Checked;
        }

        private void rdbMySQL_CheckedChanged(object sender, EventArgs e)
        {
            IsMysql = rdbMySQL.Checked;
        }
    }
}
;