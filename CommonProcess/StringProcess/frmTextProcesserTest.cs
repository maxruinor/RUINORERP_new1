using CommonProcess.StringProcess;
using HLH.Lib.Helper;
using NSoup;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CommonProcess.StringProcess
{

    [XmlInclude(typeof(UCJson路径提取Para))]
    [XmlInclude(typeof(UC数组分割提取Para))]
    [XmlInclude(typeof(UC正则式提取Para))]
    /// <summary>
    /// 就是一个处理开关。进去一波。出来一波。但是中间会各种配置，保存？
    /// </summary>
    public partial class frmTextProcesserTest : Form
    {


        List<KeyValue<string, string>> processInputResult = new List<KeyValue<string, string>>();

        /// <summary>
        ///给一个输入。就有一个输出，，
        ///给一组输入，就有一组办出，，
        /// </summary>
        public List<KeyValue<string, string>> ProcessInputResult { get => processInputResult; set => processInputResult = value; }



        //给出一个事件，通知处面 将这个里面的处理结果在处理也处理一下。
        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(Form frmPro, object Parameters);



        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;



        public frmTextProcesserTest()
        {
            InitializeComponent();
        }

        StringProcessConfig config = new StringProcessConfig();

        private void frmTextProcesserTest_Load(object sender, EventArgs e)
        {
            //加载处理的方法模块
            HLH.Lib.Helper.DropDownListHelper.InitDropListForWin(cmbProcessTemplates, typeof(ProcessAction));

            if (ProcessInputResult != null)
            {
                if (ProcessInputResult.Count > 0)
                {
                    richTextBox源.Clear();
                    richTextBox源.Text = processInputResult[0].Key.ToString();
                }
            }

            string ConfigFileName = "aaProcessTestConfig.xml";
            config = HLH.Lib.Xml.XmlUtil.Deserialize(config.GetType(), HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.ReadFile(ConfigFileName, Encoding.UTF8)) as StringProcessConfig;
            if (config == null)
            {
                config = new StringProcessConfig();
            }
            foreach (KeyValue kv in config.Actions)
            {
                #region
                //只是为了显示在listbox中才这样转换？
                kv.SetToStringenum = KeyValue.ToStringenum.key;
                chklist动作队列.Items.Add(kv);
                chklist动作队列.SetItemChecked(chklist动作队列.Items.Count - 1, (kv.Value as UCBasePara).Available);
                #endregion

            }


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

            Document doc = NSoupClient.Parse(rs);

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

            //以下主要为了去掉 html head body等其他功能，上面已经更方便的实现
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

        public string ProcessData(string Key, string SourceString)
        {
            string rs = SourceString;

            string ac = cmbProcessTemplates.SelectedItem.ToString();
            ProcessAction spa = (ProcessAction)Enum.Parse(typeof(ProcessAction), ac);
            switch (spa)
            {
                case ProcessAction.移除HTML标签:
                    break;
                case ProcessAction.字符修补替换移除:
                    break;
                case ProcessAction.正则式提取:
                    break;
                case ProcessAction.设置指定字段值:
                    break;
                case ProcessAction.检测替换特殊字符:
                    break;
                case ProcessAction.自动补全标签:
                    break;
                case ProcessAction.HTML结构分析式处理:
                    break;
                case ProcessAction.智能打包处理:
                    break;
                case ProcessAction.分割数组提取:
                    UC数组分割提取 uchtml = panel4UC.Controls[0] as UC数组分割提取;
                    UC数组分割提取Para pro = new UC数组分割提取Para();
                    uchtml.SaveDataFromUI(pro);
                    //pro.SourceString = SourceString;
                    // pro.ResultString = pro.ProcessDo();
                    // rs = pro.ResultString;

                    break;
                case ProcessAction.Json属性路径提取:
                    UCJson路径提取Find usjosn = panel4UC.Controls[0] as UCJson路径提取Find;
                    UCJson路径提取Para ucjsons = new UCJson路径提取Para();
                    usjosn.SaveDataFromUI(ucjsons);
                    //  ucjsons.SourceString = SourceString;
                    // ucjsons.ResultString = ucjsons.ProcessDo();
                    // rs = ucjsons.ResultString;
                    break;
                default:
                    break;
            }

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
                    ucr.PrintProcessResult = true;

                    rs = ucr.ProcessString(SourceString);
                    break;
                case "字符提取替换":
                    UC正则式提取 uce = panel4UC.Controls[0] as UC正则式提取;
                    //uce.PrintProcessResult = chk显示处理结果.Checked;
                    uce.KeyID = Key;
                    rs = uce.ProcessString(SourceString);
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
                MessageBox.Show("当前选择的字段值，转换为 数字类型时出错。");
                return sourceString;
            }


            if (isSmart)
            {
                #region 智能分析
                MessageBox.Show("当前没有实现这个方法，请使用手工指定的方式。");
                return sourceString;
                #endregion
            }
            else
            {
                #region 手工指定
                int thisTimes = 0;
                if (!int.TryParse(times, out thisTimes))
                {
                    MessageBox.Show("手工指定的倍数必须为int数值型。");
                    return sourceString;
                }
                oldvalue = oldvalue * thisTimes;
                #endregion

            }



            return oldvalue.ToString();
        }


        private void btnProcess_Click(object sender, EventArgs e)
        {
            string rs = ProcessData("", richTextBox源.Text);
            richTextBox结果.Text = rs;

        }


        void uchtml_OtherEvent(UCHTMLStructuralAnalysis uc, object Parameters)
        {

            string SourceString = string.Empty;
            SourceString = richTextBox源.Text;
            UCHTMLStructuralAnalysis uchtml = panel4UC.Controls[0] as UCHTMLStructuralAnalysis;
            uchtml.LoadHtmlDataByHtmlAgilityPack(SourceString);

        }


        private void cmbProcessTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ac = cmbProcessTemplates.SelectedItem.ToString();
            ProcessAction spa = (ProcessAction)Enum.Parse(typeof(ProcessAction), ac);
            co.AddUCToUI(panel4UC, spa);

        }






        private void btn处理外部事件_Click(object sender, EventArgs e)
        {
            foreach (KeyValue<string, string> item in processInputResult)
            {
                item.Value = ProcessData("", item.Key.ToString());
            }
            //调用外部事件来隐藏其他，放大这个
            if (OtherEvent != null)
            {
                OtherEvent(this, processInputResult);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //config.
            string ConfigFileName = "aaProcessTestConfig.xml";
            HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.SaveFile(ConfigFileName, HLH.Lib.Xml.XmlUtil.Serializer(config.GetType(), config), Encoding.UTF8);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        UCBasePara co = new UCBasePara();
        private void btn模拟批处理_Click(object sender, EventArgs e)
        {
            if (chk解析JSON到对象.Checked)
            {
                string json = richTextBox结果.Text;
                HLH.Lib.Helper.JsonHelper jh = new JsonHelper();
                object objJson = jh.GetObjectByJsonString(richTextBox结果.Text);
                //SiteRules.aliexpress.SkuModule skumodule = new SiteRules.aliexpress.SkuModule();
                ////objJson 必需是对应类的json文本 
                //skumodule = jh.JsonEntityLast<SkuModule>(objJson, false, "SiteRules.dll");

            }
            richTextBox结果.Clear();
            //co.BeforeOnProcessEvent += Co_BeforeOnProcessEvent;
            // co.DebugEvent += Co_DebugEvent;
            UCBasePara ucp = new UCBasePara();
            ucp.DebugTrackerEvent += Ucp_DebugTrackerEvent;
            ucp.AfterOnProcessEvent += Ucp_AfterOnProcessEvent;

            //            richTextBox结果.Text = co.BatchProcess(config.Actions, richTextBox源.Text);
            //richTextBox结果.Text = co.BatchProcessTest(config.Actions, richTextBox源.Text);
            richTextBox结果.Text = ucp.BatchProcess(config.Actions, richTextBox源.Text);


        }

        private string Ucp_AfterOnProcessEvent(UCBasePara ucPara, object Parameters)
        {
            //传过来的参数 等于输出，一定要给到rs;
            string rs = Parameters.ToString();
            if (ucPara.Action == ProcessAction.下载)
            {
                UCDownloadFilePara para = ucPara as UCDownloadFilePara;
                rs = para.FilesToString();
            }
            return rs;
        }

        private void Ucp_DebugTrackerEvent(UCBasePara ucPara, object Parameters)
        {
            PrintInfoLog(Parameters.ToString(), Color.Red);
        }



        private string Co_BeforeOnProcessEvent(ProcessAction pa, object Parameters)
        {
            string rs = string.Empty;
            if (pa == ProcessAction.下载)
            {
                rs = "开始啦。下载。" + Parameters.ToString();
            }
            return rs;
        }



        private void chklist动作队列_SelectedIndexChanged(object sender, EventArgs e)
        {

            string ac = chklist动作队列.SelectedItem.ToString();
            KeyValue kv = chklist动作队列.SelectedItem as KeyValue;
            ProcessAction spa = (ProcessAction)Enum.Parse(typeof(ProcessAction), ac);
            co.AddUCToUI(panel4UC, spa, kv.Value as UCBasePara);
        }

        private void 删除全部动作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.Actions.Clear();
            chklist动作队列.Items.Clear();
            LoadActionsToListBox();
        }


        private void LoadActionsToListBox()
        {
            chklist动作队列.Items.Clear();
            foreach (KeyValue kv in config.Actions)
            {
                #region
                kv.SetToStringenum = KeyValue.ToStringenum.key;
                chklist动作队列.Items.Add(kv);
                //添加时
                chklist动作队列.SetItemChecked(chklist动作队列.Items.Count - 1, (kv.Value as UCBasePara).Available);
                #endregion

            }
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            string ac = cmbProcessTemplates.SelectedItem.ToString();
            ProcessAction spa = (ProcessAction)Enum.Parse(typeof(ProcessAction), ac);
            co.AddActions(panel4UC, config.Actions, spa);
            LoadActionsToListBox();
        }

        private void btnUpdateAction_Click(object sender, EventArgs e)
        {
            if (chklist动作队列.SelectedItem == null)
            {
                MessageBox.Show("请选中要操作的项。");
                return;
            }
            string ac = chklist动作队列.SelectedItem.ToString();
            ProcessAction spa = (ProcessAction)Enum.Parse(typeof(ProcessAction), ac);
            UCBasePara para = (chklist动作队列.SelectedItem as KeyValue).Value as UCBasePara;
            para.Available = chklist动作队列.GetItemChecked(chklist动作队列.SelectedIndex);
            co.UpdateActions(panel4UC, config.Actions, spa, para);
            LoadActionsToListBox();
        }

        private void 删除选中运作ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (chklist动作队列.SelectedItem == null)
            {
                MessageBox.Show("请选中要删除的项。");
                return;
            }
            UCBasePara para = (chklist动作队列.SelectedItem as KeyValue).Value as UCBasePara;
            KeyValue kv2 = config.Actions.Find(delegate (KeyValue d) { return d.Tag.ToString() == para.GUID; });
            config.Actions.Remove(kv2);
            LoadActionsToListBox();
        }

        private void 更新选中动作ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 显示异常等
        /// 所以显示的是在上大部分
        /// </summary>
        /// <param name="msg"></param>
        public void PrintInfoLog(string msg, Color c)
        {
            try
            {
                this.Invoke(new EventHandler(delegate
                {
                    //richTextBox1.SelectionColor = Color.Blue;

                    myRichMsg.richTextBox1.SelectionColor = c;
                    myRichMsg.richTextBox1.AppendText(@msg);
                    myRichMsg.richTextBox1.SelectionColor = Color.Black;
                    myRichMsg.richTextBox1.AppendText("\r\n");

                }
                ));
            }
            catch (Exception)
            {

            }
        }
    }
}
