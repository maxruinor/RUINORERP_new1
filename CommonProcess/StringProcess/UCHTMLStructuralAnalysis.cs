using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Lex;
using System.Xml.XPath;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System.Collections;

namespace CommonProcess.StringProcess
{
    public partial class UCHTMLStructuralAnalysis : UCMyBase
    {
        public UCHTMLStructuralAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 取树中多个选择的集合，因为是多级节点，所以要用递归
        /// </summary>
        /// <param name="IsReverse">是否反向处理【如果为是，则必须保证反向选取的是 原来选中的 同一个深度的兄弟节点】</param>
        private void GetCheckedTreeNodes(TreeNodeCollection TreeNodes, bool IsReverse)
        {
            foreach (TreeNode tn in TreeNodes)
            {
                if (tn.Checked)
                {
                    if (!IsReverse)
                    {
                        treeNodesForChecked.Add(tn, tn.Level);
                    }

                }
                else
                {
                    if (IsReverse && a == b && tn.Level == a)
                    {
                        treeNodesForChecked.Add(tn, tn.Level);
                    }
                }

                //如果本级没有节点被选中，则子节点
                if (tn.Nodes.Count > 0 && treeNodesForChecked.Count == 0)
                {
                    GetCheckedTreeNodes(tn.Nodes, IsReverse);
                }
            }

        }

        /// <summary>
        /// 当多个选择后，处理时再处理这个集合
        /// </summary>
        private Dictionary<TreeNode, int> treeNodesForChecked = new Dictionary<TreeNode, int>();

        /// <summary>
        /// 如果为真，则是保存选中的节点结构，否则移除选中的结构
        /// </summary>
        public bool SaveSelectNodes { get; set; }

        //选择的节点的深度 必须a=b
        int a = 0;
        int b = 0;
        int c = 0;//临时存放



        /// <summary>
        /// 如果选择多个节点时，同时移除多个内容正常处理，反之要保留多个选择的节点内容，则需要特殊处理
        /// 如果要保留，则因为循环时每次是选择一个节点内容，所以思路是 在保证都是相同深度节点时，反向选择。将没选择中的，移除处理
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public string ProcessString(string sourceString)
        {


            ///
            treeNodesForChecked.Clear();
            string rs = sourceString;
            a = 1;
            b = 1;
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(sourceString);

            //bool samehtml = CheckHtmlStructure(htmlDoc);
            //if (samehtml)
            //{
            //    printInfoMessage("相同", Color.Red);
            //}

            #region 单独去掉指定标签格式  单独执行，因为会破坏下面的结构验证
            if (rdb移除指定标签格式.Checked)
            {
                #region ListView


                foreach (ListViewItem item in lvForHtml.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Text.Split('|')[0])
                        {
                            //DescendantNodesAndSelf

                            case "标签属性":
                                #region


                                foreach (var tag in htmlDoc.DocumentNode.Descendants())
                                {
                                    if (tag.HasAttributes && tag.Attributes.Count > 0)
                                    {
                                        IEnumerable<HtmlAttribute> attrs = tag.ChildAttributes("style");   //获取子节点与自身的所有名为class的属性集合
                                        foreach (HtmlAttribute attr in attrs)
                                        {
                                            tag.Attributes.Remove(attr);
                                        }

                                        IEnumerable<HtmlAttribute> battrs = tag.ChildAttributes("bgcolor");   //获取子节点与自身的所有名为class的属性集合
                                        foreach (HtmlAttribute attr in battrs)
                                        {
                                            tag.Attributes.Remove(attr);
                                        }

                                        if (tag.InnerText.ToString().Trim().Length == 0)
                                        {
                                            // rs = rs.Replace(tag.OuterHtml, "");
                                        }
                                    }



                                }

                                //if (rs != htmlDoc.DocumentNode.OuterHtml)
                                //{
                                //    htmlDoc.LoadHtml(rs);
                                //}


                                #endregion
                                break;

                            case "空内容标签":
                                #region

                                if (htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]) != null)
                                {
                                    foreach (var tag in htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]))
                                    {
                                        if (tag.InnerText.ToString().Trim().Length == 0)
                                        {
                                            rs = rs.Replace(tag.OuterHtml, "");
                                        }

                                    }

                                    if (rs != htmlDoc.DocumentNode.OuterHtml)
                                    {
                                        htmlDoc.LoadHtml(rs);
                                    }
                                }
                                break;

                                #endregion

                            case "HTML注释":
                                #region
                                if (htmlDoc.DocumentNode.SelectNodes("//comment()") != null)
                                {
                                    foreach (var tag in htmlDoc.DocumentNode.SelectNodes("//comment()"))
                                    {
                                        tag.Remove();
                                    }
                                }
                                rs = htmlDoc.DocumentNode.OuterHtml;
                                #endregion
                                break;

                            case "脚本":
                                #region
                                foreach (var tag in htmlDoc.DocumentNode.Descendants("noscript"))
                                {
                                    rs = rs.Replace(tag.OuterHtml, "");
                                }
                                foreach (var tag in htmlDoc.DocumentNode.Descendants("script"))
                                {
                                    rs = rs.Replace(tag.OuterHtml, "");
                                }
                                #endregion
                                break;
                            case "链接":
                            case "字体":
                                //因数链接/字体 里面的内容需要保留，所以这里特别处理
                                if (htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]) != null)
                                {
                                    foreach (var tag in htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]))
                                    {
                                        // tag.Remove();//这种去掉标签及内容
                                        rs = rs.Replace(tag.OuterHtml, tag.InnerHtml); //这种是去掉标签的，不包括内容
                                    }
                                    htmlDoc.LoadHtml(rs);
                                }
                                break;



                            default:
                                #region

                                if (htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]) != null)
                                {
                                    foreach (var tag in htmlDoc.DocumentNode.Descendants(item.Text.Split('|')[1]))
                                    {
                                        // tag.Remove();
                                        rs = rs.Replace(tag.OuterHtml, "");
                                    }

                                    if (rs != htmlDoc.DocumentNode.OuterHtml)
                                    {
                                        htmlDoc.LoadHtml(rs);
                                    }
                                }
                                rs = htmlDoc.DocumentNode.OuterHtml;

                                #endregion
                                break;
                        }
                    }


                }

                #endregion

                #region 右边 标签属性 及值 等等的处理20160919
                //手动选择时 
                if (chk标签属性.Checked && cmb标签属性.SelectedItem != null && cmb标签属性.SelectedItem.ToString() != "")
                {

                    string[] attrTags = cmb标签属性.SelectedItem.ToString().Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string attrTag in attrTags)
                    {
                        #region


                        foreach (var tag in htmlDoc.DocumentNode.Descendants())
                        {
                            if (tag.HasAttributes && tag.Attributes.Count > 0)
                            {
                                IEnumerable<HtmlAttribute> attrs = tag.ChildAttributes(attrTag);   //获取子节点与自身的所有名为class的属性集合
                                if (attrs != null)
                                {
                                    foreach (HtmlAttribute attr in attrs)
                                    {
                                        tag.Attributes.Remove(attr);
                                    }
                                    tag.WriteContentTo();
                                }

                            }

                        }

                        #endregion
                    }

                }


                #endregion


                #region 指定标签属性


                //因指定标签 里面的内容需要保留，所以这里特别处理
                if (chk指定标签属性.Checked && cmb指定标签的属性.SelectedItem != null && cmb指定标签的属性.SelectedItem.ToString() != "")
                {

                    string[] attrTags = cmb指定标签的属性.SelectedItem.ToString().Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string attrTag in attrTags)
                    {
                        #region
                        string tagName = string.Empty;
                        string tagAttrName = string.Empty;
                        tagName = attrTag.Split('|')[0];
                        tagAttrName = attrTag.Split('|')[1];

                        foreach (var tag in htmlDoc.DocumentNode.Descendants(tagName))
                        {
                            if (tag.HasAttributes && tag.Attributes.Count > 0)
                            {

                                IEnumerable<HtmlAttribute> attrs = tag.ChildAttributes(tagAttrName);   //获取子节点与自身的所有名为class的属性集合
                                if (attrs != null)
                                {
                                    foreach (HtmlAttribute attr in attrs)
                                    {
                                        tag.Attributes.Remove(attr);
                                    }
                                    tag.WriteContentTo();
                                }


                            }

                            //rs = Strings.Replace(rs, tag.OuterHtml, tag.InnerHtml, 1, -1, CompareMethod.Text); //这种是去掉标签的，不包括内容
                        }

                        //  htmlDoc.LoadHtml(rs);

                        #endregion
                    }

                }


                #endregion


                #region 去掉指定标签


                //因指定标签 里面的内容需要保留，所以这里特别处理
                if (chk指定标签.Checked && cmb指定标签.SelectedItem != null && cmb指定标签.SelectedItem.ToString() != "")
                {

                    string[] attrTags = cmb指定标签.SelectedItem.ToString().Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string attrTag in attrTags)
                    {
                        #region
                        foreach (var tag in htmlDoc.DocumentNode.Descendants(attrTag))
                        {
                            // tag.Remove();//这种去掉标签及内容
                            rs = Strings.Replace(rs, tag.OuterHtml, tag.InnerHtml, 1, -1, CompareMethod.Text); //这种是去掉标签的，不包括内容
                        }
                        htmlDoc.LoadHtml(rs);

                        #endregion
                    }

                }


                #endregion



                #region 空内容标签

                //手动选择时 
                if (chk空内容标签.Checked && cmb空内容标签.SelectedItem != null && cmb空内容标签.SelectedItem.ToString() != "")
                {

                    string[] attrTags = cmb空内容标签.SelectedItem.ToString().Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string attrTag in attrTags)
                    {
                        #region

                        if (htmlDoc.DocumentNode.Descendants(attrTag) != null)
                        {
                            List<HtmlNode> nodes = new List<HtmlNode>();

                            foreach (var tag in htmlDoc.DocumentNode.Descendants(attrTag))
                            {
                                if (tag.InnerText.ToString().Trim().Length == 0 || tag.InnerText.ToString().Replace("&nbsp;", "").Trim().Length == 0)
                                {
                                    nodes.Add(tag);

                                }
                            }

                            foreach (var item in nodes.ToArray())
                            {
                                // item.Remove();
                                HtmlNode temp = htmlDoc.DocumentNode.SelectSingleNode(item.XPath);
                                if (temp != null)
                                {
                                    temp.Remove();
                                }
                            }


                        }

                        #endregion
                    }

                }



                #endregion
                if (rs.Length > htmlDoc.DocumentNode.OuterHtml.Length)
                {
                    rs = htmlDoc.DocumentNode.OuterHtml;
                }

                return rs;
            }

            #endregion

            //用来保存选中节点的结构的临时数据，真实HTML用这个的outerhtml等属性来替换为空，后保留操作
            HtmlNode htmlNode = null;

            if (chkInputXpath.Checked && txtXpath.Text.Trim().Length > 0)
            {
                #region 手工指定 xpath表达式
                htmlNode = htmlDoc.DocumentNode.SelectSingleNode(txtXpath.Text);
                #region 赋值
                if (htmlNode != null)
                {
                    string selectHtmlText = string.Empty;

                    if (chk是否包含标签本身.Checked)
                    {
                        selectHtmlText = htmlNode.OuterHtml;
                    }
                    else
                    {
                        selectHtmlText = htmlNode.InnerHtml;
                    }

                    if (chkOutPutFindNodesForXpath.Checked)
                    {
                        printInfoMessage(selectHtmlText);
                    }
                    if (rdbSaveNode.Checked)
                    {
                        rs = selectHtmlText;
                    }
                    else
                    {
                        // rs = rs.Replace(selectHtmlText, "");
                        rs = Strings.Replace(rs, selectHtmlText, "", 1, -1, CompareMethod.Text);
                    }
                }
                #endregion


                #endregion
            }
            else
            {
                #region 非手工指定

                GetCheckedTreeNodes(treeView1.Nodes, false);
                //如果没有加载参考节点则提示
                if (treeView1.Nodes.Count == 0 || treeNodesForChecked.Count == 0)
                {
                    printInfoMessage("请加载参考节点,或选择要操作的节点，并设置指定操作 移除或保存。");
                    return rs;
                }

                int firstFlag = 0;
                foreach (KeyValuePair<TreeNode, int> kv in treeNodesForChecked)
                {
                    firstFlag++;
                    if (firstFlag == 1)
                    {
                        a = kv.Value;
                        b = kv.Value;
                    }

                    b = kv.Value;
                    if (a == b)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                    //}
                }

                if (a != b && treeNodesForChecked.Count > 1)
                {
                    printInfoMessage("当选择多个节点时，必须是同一层级关系的节点，并且不建议都为顶级节点。");
                    return rs;
                }
                else
                {
                    if (a == 0)
                    {
                        printInfoMessage("如果没有手工指定时，必须选择要处理的节点，并且不能为顶级节点。", Color.Red);
                        return rs;
                    }
                }

                //结果是否需要真实保存到数据库中
                bool NeedSaveNode = rdbSaveNode.Checked;

                //如果多个节点，并且要保留时，则反向处理
                if (treeNodesForChecked.Count > 1 && NeedSaveNode)
                {
                    //选择了多个节点时
                    treeNodesForChecked.Clear();
                    GetCheckedTreeNodes(treeView1.Nodes, true);
                    NeedSaveNode = false;
                }


                //这里已经完成了替换，多次替换
                foreach (KeyValuePair<TreeNode, int> kv in treeNodesForChecked)
                {
                    try
                    {
                        HtmlNode tag = (kv.Key as TreeNode).Tag as HtmlNode;
                        if (tag.Attributes.Contains("id"))
                        {
                            htmlNode = htmlDoc.GetElementbyId(tag.Attributes["id"].Value);
                        }
                        if (htmlNode == null)
                        {
                            htmlNode = htmlDoc.DocumentNode.SelectSingleNode(tag.XPath);
                        }
                        if (htmlNode == null)
                        {
                            //如果操作的节点 只有一个 不管是保留还是移除，节点的内容 认为唯一 ，则可以不和结构，直接全文查找替换
                            //这时则不判断格式结构
                            if (chk如果节点内容唯一.Checked && treeNodesForChecked.Count == 1)
                            {
                                htmlNode = tag;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        htmlNode = null;
                    }
                    #region 赋值
                    if (htmlNode != null)
                    {
                        string selectHtmlText = string.Empty;

                        if (chk是否包含标签本身.Checked)
                        {
                            selectHtmlText = htmlNode.OuterHtml;
                        }
                        else
                        {
                            selectHtmlText = htmlNode.InnerHtml;
                        }

                        if (chkOutPutFindNodesForXpath.Checked)
                        {
                            printInfoMessage(selectHtmlText);
                        }
                        if (NeedSaveNode)
                        {
                            rs = selectHtmlText;
                        }
                        else
                        {
                            //这里采集下面的这个 能忽略大小写 无视大小写 速度最快的方法
                            if (chk只操作标签本身.Checked)
                            {
                                rs = Strings.Replace(rs, htmlNode.OuterHtml, htmlNode.InnerHtml, 1, -1, CompareMethod.Text); //这种是去掉标签的，不包括内容
                            }
                            else
                            {
                                rs = Strings.Replace(rs, selectHtmlText, "", 1, -1, CompareMethod.Text);
                            }
                        }
                        if (rs == sourceString)
                        {
                            printInfoMessage("没有替换掉匹配的节点，可能格式问题,通常需要提前补全标签，引号等，请尝试。");
                        }
                    }
                    else
                    {
                        printInfoMessage("没有找到匹配的节点，请尝试其他方式。");
                    }
                    #endregion



                }
                #endregion
            }

            //因为判断需要看 所有选择中的标签，而这些是否选中，是在上面的方法中实现。所以这个方法只能放在这个位置
            //如果操作的节点 只有一个 不管是保留还是移除，节点的内容 认为唯一 ，则可以不和结构，直接全文查找替换
            //这时则不判断格式结构
            if (!chk如果节点内容唯一.Checked)
            {
                #region 判断是否适合处理

                counter = 0;
                //这行数据 解析后，与参考结构比较，如果基本结构都不一样，则忽略处理  ,重点检测 选择的节点的那一级深度的结构
                //同级深度个数和xpath是否相同
                //1）参考结构，就是显示的那个树treeview， 1）对比总体结构，总级别
                //2）再对比选择的级别与深度
                //3）如果选择的节点内容都一样，则完全匹配，可以用这种结构性处理
                if (!CheckHtmlDomIsOK(htmlDoc, a))
                {
                    printInfoMessage("当前描述与参考结构不一致，忽略处理。");
                    return sourceString;
                }

                #endregion
            }


            //判断是否去掉注释和脚本
            return rs;
        }


        /// <summary>
        /// 判断结构时，
        /// </summary>
        int counter = 0;


        //同级深度个数和xpath是否相同
        /// <summary>
        /// 判断HTML结构是否一致
        /// </summary>
        /// <param name="htmlNodes">目标的结构</param>
        /// <param name="level">在参考树中选择的节点深度，因为多个节点必须一样，所以可以用一个值表示</param>
        /// <returns></returns>
        private bool CheckHtmlDomIsOK(HtmlAgilityPack.HtmlDocument htmlDoc, int level)
        {
            bool rs = true;
            #region   先判断总体深度和节点

            int t = 0;
            foreach (HtmlNode item in htmlDoc.DocumentNode.ChildNodes)
            {
                if (item.Name == "#text")
                {
                    continue;
                }
                t++;
            }


            if (t != treeView1.Nodes.Count)
            {
                return false;
            }

            #endregion


            //选中的节点的内容是否和目标结构中的值一样，如果是，则一定可以处理成功
            //这里已经完成了替换，多次替换
            foreach (KeyValuePair<TreeNode, int> kv in treeNodesForChecked)
            {
                HtmlNode tag = (kv.Key as TreeNode).Tag as HtmlNode;
                HtmlNode TempHtmlNode = htmlDoc.DocumentNode.SelectSingleNode(tag.XPath);
                if (TempHtmlNode == null)
                {
                    return false;
                }
            }

            //foreach (HtmlNode item in htmlNodes)
            //{
            //    if (item.XPath.Split(new char['/'], StringSplitOptions.RemoveEmptyEntries).Length == level)
            //    {
            //        counter++;
            //    }
            //    else
            //    {
            //        rs = CheckHtmlDomIsOK(item.ChildNodes, level);
            //    }
            //}

            ////认为找到了，指定深度的节点
            //if (counter > 0)
            //{
            //    if (counter == treeNodesForChecked.Count)
            //    {
            //        rs = true;
            //    }
            //}

            return rs;
        }


        public int ReferenceTreeMaxDepth { get; set; }

        /// <summary>
        /// 保存树的同深度级别的节点数 ，如树 顶级则在队列底
        /// </summary>
        public Queue HtmlNodesQty { get; set; }

        private Queue referenceNodesQty = new Queue();

        public Queue ReferenceNodesQty
        {
            get { return referenceNodesQty; }
            set { referenceNodesQty = value; }
        }

        private bool CheckHtmlStructure(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            HtmlAgilityPack.HtmlDocument ReferenceHtmlDoc = treeView1.Tag as HtmlAgilityPack.HtmlDocument;
            //if (ReferenceTreeMaxDepth == 0)
            //{
            //    //加1是因为 相同的数据源算出来多1，
            //    ReferenceTreeMaxDepth = getHtmlDepth(ReferenceHtmlDoc.DocumentNode);
            //}
            bool rs = true;
            //int htmldepth = getHtmlDepth(htmlDoc.DocumentNode);

            ////如果总深度不一样，则认为结构不一样
            //if (ReferenceTreeMaxDepth != htmldepth)
            //{
            //    rs = false;
            //}

            if (ReferenceNodesQty.Count == 0)
            {
                getHtmlNodesByLevel(ReferenceHtmlDoc.DocumentNode, ReferenceNodesQty);
            }


            HtmlNodesQty = new Queue();
            getHtmlNodesByLevel(htmlDoc.DocumentNode, HtmlNodesQty);
            //比较两个结构的节点数和深度
            if (ReferenceNodesQty.Count != HtmlNodesQty.Count)
            {
                rs = false;
            }




            return rs;
        }



        /// <summary>
        /// 得到参考结构的最大深度
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private int getHtmlDepth(HtmlNode node)
        {
            int max = 0;
            if (node.ChildNodes.Count == 0)
                return 0;
            else
            {
                foreach (HtmlNode item in node.ChildNodes)
                {
                    if (getHtmlDepth(item) > max)
                    {
                        max = getHtmlDepth(item);
                    }
                }
                return max + 1;
            }
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="treeNode"></param>23120
        /// <returns></returns>
        private void getHtmlNodesByLevel(HtmlNode Node, Queue nodesQueue)
        {
            if (Node.ChildNodes.Count == 0)
                return;
            else
            {
                if (Node.ChildNodes.Count > 0)
                {
                    List<HtmlNode> nodes = new List<HtmlNode>();
                    foreach (HtmlNode item in Node.ChildNodes)
                    {
                        if (item.Name != "#text")
                        {
                            nodes.Add(item);
                        }
                    }
                    nodesQueue.Enqueue(nodes.Count);
                }

                foreach (HtmlNode item in Node.ChildNodes)
                {
                    if (item.Name == "#text")
                    {
                        continue;
                    }
                    getHtmlNodesByLevel(item, nodesQueue);
                }
                return;
            }
        }


        /// <summary>
        /// 得到参考结构的最大深度 方法正确，可以不用。直接用html结构
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private int getTreeViewDepth(TreeNode treeNode)
        {
            int max = 0;
            if (treeNode.Nodes.Count == 0)
                return 0;
            else
            {
                foreach (TreeNode node in treeNode.Nodes)
                {
                    if (getTreeViewDepth(node) > max)
                        max = getTreeViewDepth(node);
                }
                return max + 1;
            }
        }



        private void rdbSaveNode_CheckedChanged(object sender, EventArgs e)
        {
            SaveSelectNodes = true;
        }

        private void rdbRemoveNode_CheckedChanged(object sender, EventArgs e)
        {
            SaveSelectNodes = false;
            //chkOutPutFindNodesForXpath.Checked = rdbRemoveNode.Checked;
            //chkOutPutFindNodesForXpath.Visible = rdbRemoveNode.Checked;
        }

        private void UCHTMLStructuralAnalysis_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            lvForHtml.Items.Clear();

            lvForHtml.Items.Add(new ListViewItem("脚本|script"));
            lvForHtml.Items.Add(new ListViewItem("样式|style"));
            lvForHtml.Items.Add(new ListViewItem("图片|img"));
            lvForHtml.Items.Add(new ListViewItem("链接|a"));
            lvForHtml.Items.Add(new ListViewItem("字体|font"));
            lvForHtml.Items.Add(new ListViewItem("HTML注释|<!---->"));

            cmb空内容标签.Items.Add("span");
            cmb空内容标签.Items.Add("tr");
            cmb空内容标签.Items.Add("td");
            cmb空内容标签.Items.Add("p");
            cmb空内容标签.Items.Add("b");
            cmb空内容标签.Items.Add("table");
            cmb空内容标签.Items.Add("tbody");
            cmb空内容标签.Items.Add("font");
            cmb空内容标签.Items.Add("div");
            cmb空内容标签.Items.Add("dd");
            cmb空内容标签.Items.Add("dl");

            cmb标签属性.Items.Add("bgcolor");
            cmb标签属性.Items.Add("style");
            cmb标签属性.Items.Add("font");
            cmb标签属性.Items.Add("class");
            cmb标签属性.Items.Add("width");
            cmb标签属性.Items.Add("height");
            cmb标签属性.Items.Add("bordercolordark");
            cmb标签属性.Items.Add("bordercolor");
            cmb标签属性.Items.Add("bordercolorlight");
            cmb标签属性.Items.Add("title");



            cmb指定标签的属性.Items.Add("p|style");
            cmb指定标签的属性.Items.Add("div|style");
            cmb指定标签的属性.Items.Add("span|style");



            cmb指定标签.Items.Add("span");
            cmb指定标签.Items.Add("tr");
            cmb指定标签.Items.Add("p");
            cmb指定标签.Items.Add("b");
            cmb指定标签.Items.Add("table");
            cmb指定标签.Items.Add("tbody");
            cmb指定标签.Items.Add("font");
            cmb指定标签.Items.Add("div");
            cmb指定标签.Items.Add("dd");
            cmb指定标签.Items.Add("dl");


            /*
            lvForHtml.Items.Add(new ListViewItem("空内容标签|span"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|tr"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|p"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|table"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|div"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|tbody"));
            lvForHtml.Items.Add(new ListViewItem("空内容标签|font"));
            lvForHtml.Items.Add(new ListViewItem("标签属性|bgcolor"));
            //  lvForHtml.Items.Add(new ListViewItem("空内容标签|tbody"));
            //  lvForHtml.Items.Add(new ListViewItem("空内容标签|tbody"));
            */
        }


        /// <summary>
        /// 速度慢，但是这个类库可以补全丢失的标签 补全标签
        /// 不建议使用
        /// </summary>
        /// <param name="SourceString"></param>
        [Obsolete("速度慢")]
        public void LoadHtmlData(string SourceString)
        {
            treeView1.Nodes.Clear();
            Lexer lexer = new Lexer(SourceString);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(null);
            this.treeView1.Nodes.Clear();
            this.treeView1.Nodes.Add("root");
            TreeNode treeRoot = this.treeView1.Nodes[0];
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                this.RecursionHtmlNode(treeRoot, htmlNodes[i], false);
            }

        }


        HtmlAgilityPack.HtmlDocument ReferenceHtmlDoc = new HtmlAgilityPack.HtmlDocument();

        /// <summary>
        /// 速度快 当前正在使用的方法
        /// </summary>
        /// <param name="SourceString"></param>
        [Obsolete("速度快 相对而言")]
        public void LoadHtmlDataByHtmlAgilityPack(string SourceString)
        {
            referenceNodesQty = new Queue();

            treeView1.Nodes.Clear();
            ReferenceHtmlDoc = new HtmlAgilityPack.HtmlDocument();
            ReferenceHtmlDoc.LoadHtml(SourceString);

            treeView1.Tag = ReferenceHtmlDoc;

            HtmlNodeCollection nodes = ReferenceHtmlDoc.DocumentNode.ChildNodes;
            //  this.treeView1.Nodes.Add("root");
            //TreeNode treeRoot = this.treeView1.Nodes[0];

            foreach (HtmlNode tag in nodes)
            {
                if (tag.Name == "#text")
                {
                    continue;
                }
                string nodeString = tag.Name;
                if (tag.Attributes["ID"] != null)
                    nodeString = nodeString + " { id=\"" + tag.Attributes["ID"].Value + "\" }";
                if (tag.Attributes["CLASS"] != null)
                    nodeString = nodeString + " { class=\"" + tag.Attributes["CLASS"].Value + "\" }";
                if (tag.Attributes["STYLE"] != null)
                    nodeString = nodeString + " { style=\"" + tag.Attributes["STYLE"].Value + "\" }";
                if (tag.Attributes["HREF"] != null)
                    nodeString = nodeString + " { href=\"" + tag.Attributes["HREF"].Value + "\" }";


                TreeNode current = new TreeNode(nodeString);
                //current.Tag = tag.XPath;
                current.Tag = tag;//直接保存为HTML节点

                //StringBuilder sb = new StringBuilder();
                //XPathDocument doc = new XPathDocument(new
                //StringReader(sw.ToString()));
                //XPathNavigator nav = doc.CreateNavigator();
                //XPathNodeIterator nodes = nav.Select(xpath);
                //while (nodes.MoveNext())
                //{
                //    sb.Append(nodes.Current.Value);
                //}
                //return sb.ToString(); 


                treeView1.Nodes.Add(current);

                //the children nodes
                if (tag.ChildNodes != null && tag.ChildNodes.Count > 0)
                {
                    this.LoadNodesToTree(current, tag.ChildNodes, true);
                }

                //the sibling nodes
                //if (siblingRequired)
                //{
                //    INode sibling = htmlNode.NextSibling;
                //    while (sibling != null)
                //    {
                //        this.LoadNodesToTree(treeNode, sibling, false);
                //        sibling = sibling.NextSibling;
                //    }
                //}



            }


        }




        #region createxpath

        //委托传入一个字符串
        private delegate void SetValueToUI(string str);

        //key（Xpath）,value（整个节点）
        List<ObjXpath> XpathList = new List<ObjXpath>();
        private int Index = 0;
        //htmlDcoument对象用来访问Html文档
        HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();



        //分析Xpath的所有代码 暂时没有使用
        public void LoadHtmlDataByXpath(string SourceString)
        {
            treeView1.Nodes.Clear();
            try
            {
                //加载Html文档
                hd.LoadHtml(SourceString);
                Thread pingTask = new Thread(new ThreadStart(delegate
                {
                    //代码,线程要执行的代码
                    SartNode(SourceString);
                }));
                pingTask.Start();

            }
            catch (Exception ex)
            {
                printInfoMessage(ex.Message.Trim());
            }
        }


        //开始处理Node
        private void SartNode(string strhtml)
        {
            //htmlDcoument对象用来访问Html文档s
            HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
            //加载Html文档
            hd.LoadHtml(strhtml);
            HtmlNodeCollection htmllist = hd.DocumentNode.ChildNodes;
            Index = 0;
            XpathList.Clear();
            foreach (HtmlNode em in htmllist)
            {
                Setxpath(em);
            }
        }
        /// <summary>
        /// 递归获取Html Dom
        /// </summary>
        /// <param name="node">要处理的节点</param>
        private void Setxpath(HtmlNode node)
        {
            foreach (HtmlNode item in node.ChildNodes)
            {
                if (item.XPath.Contains("#"))
                {
                    continue;
                }
                if (item.ChildNodes.Count > 0)
                {
                    XpathList.Add(new ObjXpath() { id = Index.ToString(), Key = item.XPath, Value = "" });

                    SetValueToUI st = new SetValueToUI(UIContorol);
                    this.BeginInvoke(st, item.XPath);//调用代理 

                    // UIContorol(item.XPath);
                    Index++;
                    Setxpath(item);
                }
                else
                {
                    XpathList.Add(new ObjXpath() { id = Index.ToString(), Key = item.XPath, Value = "" });
                    SetValueToUI st = new SetValueToUI(UIContorol);
                    this.BeginInvoke(st, item.XPath);//调用代理 
                    Index++;
                }
            }
        }

        //使用委托给控件赋值
        private void UIContorol(string str)
        {
            TreeNode treeRoot = new TreeNode(str);
            this.treeView1.Nodes.Add(treeRoot);
        }




        // txtContents.Text = hd.DocumentNode.SelectSingleNode(txtPath.Text.Trim()).OuterHtml;




        public class ObjXpath
        {
            public string id { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #endregion


        private void LoadNodesToTree(TreeNode treeNode, HtmlNodeCollection htmlNodes, bool siblingRequired)
        {
            #region 加载到树

            foreach (HtmlNode tag in htmlNodes)
            {
                if (tag.Name == "#text")
                {
                    continue;
                }
                string nodeString = tag.Name;
                if (tag.Attributes["ID"] != null)
                    nodeString = nodeString + " { id=\"" + tag.Attributes["ID"].Value + "\" }";
                if (tag.Attributes["CLASS"] != null)
                    nodeString = nodeString + " { class=\"" + tag.Attributes["CLASS"].Value + "\" }";
                if (tag.Attributes["STYLE"] != null)
                    nodeString = nodeString + " { style=\"" + tag.Attributes["STYLE"].Value + "\" }";
                if (tag.Attributes["HREF"] != null)
                    nodeString = nodeString + " { href=\"" + tag.Attributes["HREF"].Value + "\" }";


                TreeNode current = new TreeNode(nodeString);


                // XpathList.Add(new ObjXpath() { id = Index.ToString(), Key = tag.XPath, Value = "" });

                //SetValueToUI st = new SetValueToUI(UIContorol);
                //  this.BeginInvoke(st, tag.XPath);//调用代理 

                // UIContorol(item.XPath);
                // Index++;
                //current.Tag = tag.XPath;
                current.Tag = tag;//直接保存为HTML节点，后面更好处理
                treeNode.Nodes.Add(current);


                //the children nodes
                if (tag.ChildNodes != null && tag.ChildNodes.Count > 0)
                {
                    this.LoadNodesToTree(current, tag.ChildNodes, true);
                }

                //the sibling nodes
                //if (siblingRequired)
                //{
                //    INode sibling = htmlNode.NextSibling;
                //    while (sibling != null)
                //    {
                //        this.LoadNodesToTree(treeNode, sibling, false);
                //        sibling = sibling.NextSibling;
                //    }
                //}



            }

            #endregion
        }


        private void RecursionHtmlNode(TreeNode treeNode, INode htmlNode, bool siblingRequired)
        {
            if (htmlNode == null || treeNode == null) return;

            TreeNode current = treeNode;
            //current node
            if (htmlNode is ITag)
            {
                ITag tag = (htmlNode as ITag);
                if (!tag.IsEndTag())
                {
                    string nodeString = tag.TagName;
                    if (tag.Attributes != null && tag.Attributes.Count > 0)
                    {
                        if (tag.Attributes["ID"] != null)
                            nodeString = nodeString + " { id=\"" + tag.Attributes["ID"].ToString() + "\" }";
                        if (tag.Attributes["CLASS"] != null)
                            nodeString = nodeString + " { class=\"" + tag.Attributes["CLASS"].ToString() + "\" }";
                        if (tag.Attributes["STYLE"] != null)
                            nodeString = nodeString + " { style=\"" + tag.Attributes["STYLE"].ToString() + "\" }";
                        if (tag.Attributes["HREF"] != null)
                            nodeString = nodeString + " { href=\"" + tag.Attributes["HREF"].ToString() + "\" }";
                    }
                    current = new TreeNode(nodeString);
                    treeNode.Nodes.Add(current);
                }
            }

            //the children nodes
            if (htmlNode.Children != null && htmlNode.Children.Count > 0)
            {
                this.RecursionHtmlNode(current, htmlNode.FirstChild, true);
            }

            //the sibling nodes
            if (siblingRequired)
            {
                INode sibling = htmlNode.NextSibling;
                while (sibling != null)
                {
                    this.RecursionHtmlNode(treeNode, sibling, false);
                    sibling = sibling.NextSibling;
                }
            }
        }


        public object OtherEventParameters { get; set; }

        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(UCHTMLStructuralAnalysis uc, object Parameters);


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;



        private void button1_Click(object sender, EventArgs e)
        {
            if (OtherEvent != null)
            {
                OtherEvent(this, OtherEventParameters);
            }
        }

        private void chkInputXpath_CheckedChanged(object sender, EventArgs e)
        {
            txtXpath.Visible = chkInputXpath.Checked;
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                HtmlNode tag = treeView1.SelectedNode.Tag as HtmlNode;
                txtXpath.Text = tag.XPath;
            }
        }

        private void rdb移除指定标签格式_CheckedChanged(object sender, EventArgs e)
        {
            gb指定要移除的标签.Visible = rdb移除指定标签格式.Checked;
        }

        private void chkSelectAll标签属性_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cmb标签属性.Items.Count; i++)
            {
                if (!string.IsNullOrEmpty(cmb标签属性.Items[i].ToString()))
                {
                    cmb标签属性.CheckBoxItems[i].Checked = chkSelectAll标签属性.Checked;
                }
            }
        }

        private void chkSelectAll空内容标签_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cmb空内容标签.Items.Count; i++)
            {
                if (!string.IsNullOrEmpty(cmb空内容标签.Items[i].ToString()))
                {
                    cmb空内容标签.CheckBoxItems[i].Checked = chkSelectAll空内容标签.Checked;
                }
            }
        }

        private void chkSelectAll指定标签_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cmb指定标签.Items.Count; i++)
            {
                if (!string.IsNullOrEmpty(cmb指定标签.Items[i].ToString()))
                {
                    cmb指定标签.CheckBoxItems[i].Checked = chkSelectAll指定标签.Checked;
                }
            }
        }
    }
}

