using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL = Netron.GraphLib;
using System.Windows.Forms;
using Netron.Neon;
using System.Drawing;
using Netron.Neon.HtmlHelp.ChmDecoding;
using Netron.Neon.HtmlHelp;
using Netron.Neon.HtmlHelp.UIComponents;
using Netron.Neon.TextEditor;
using System.Xml;
using Netron.GraphLib;
using RUINORERP.WF.BizOperation.Condition;
using RUINORERP.WF.UI;
using RUINORERP.WF;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using RUINORERP.UI.WorkFlowTester;
using Netron.GraphLib.UI;
using SqlSugar;
using static Netron.GraphLib.UI.GraphControl;
using RUINORERP.WF.BizOperation.Steps;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 一个很重要的中间类
    /// </summary>
	public class Mediator
    {


        #region Fields

        #region Help system fields
        private DumpingInfo _dmpInfo = null;
        private HtmlHelpSystem _reader = null;
        string _prefURLPrefix = "mk:@MSITStore:";
        bool _prefUseHH2TreePics = false;
        private string LM_Key = @"Software\Netron\HtmlHelpViewer\";

        string _prefDumpOutput = "";
        DumpCompression _prefDumpCompression = DumpCompression.Medium;
        private InfoTypeCategoryFilter _filter = new InfoTypeCategoryFilter();
        DumpingFlags _prefDumpFlags = DumpingFlags.DumpBinaryTOC | DumpingFlags.DumpTextTOC |
            DumpingFlags.DumpTextIndex | DumpingFlags.DumpBinaryIndex |
            DumpingFlags.DumpUrlStr | DumpingFlags.DumpStrings;

        #endregion

        /// <summary>
        /// General purpose randomizer for the graph examples
        /// </summary>
        protected Random rnd;
        private TabFactory tabFactory;

        private SampleFactory samplesFactory;
        internal WFMainForm parent;
        private DockContent lastAdded;

        private DockPanel dockPanel;


        #endregion

        #region Properties

        public DockPanel DockPanel
        {
            get { return dockPanel; }
            set { dockPanel = value; }
        }


        #region Tab controls
        public Netron.GraphLib.UI.GraphControl GraphControl
        {
            get
            {
                if (tabFactory.GraphTab == null)
                    return null;
                else
                    return tabFactory.GraphTab.GraphControl;
            }
        }


        public TextEditorControl Editor
        {
            get
            {
                if (tabFactory.CodeTab == null)
                    return null;
                else
                    return tabFactory.CodeTab.Editor;
            }
        }

        public PropertyGrid Properties
        {
            get
            {
                if (tabFactory.PropertiesTab == null)
                    return null;
                else
                    return tabFactory.PropertiesTab.PropertyGrid;
            }
        }


        public NBrowser Browser
        {
            get
            {
                if (tabFactory.BrowserTab == null)
                    return null;
                else
                    return tabFactory.BrowserTab.Browser;
            }
        }




        public TocTree TocTree
        {
            get
            {
                if (tabFactory.ChmTocTab == null)
                    return null;
                else
                    return tabFactory.ChmTocTab.TocTree;
            }
        }




        public NOutput OutputBox
        {
            get { return tabFactory.OutputTab.Output; }
        }


        #endregion

        #region Tabs

        public BrowserTab BrowserTab
        {
            get { return tabFactory.BrowserTab; }
        }

        //		public BugTab BugTab
        //		{
        //			get{return tabFactory.BugTab;}
        //		}

        public CodeTab CodeTab
        {
            get { return tabFactory.CodeTab; }
        }

        public DiagramBrowserTab DiagramBrowserTab
        {
            get { return tabFactory.DiagramBrowserTab; }
        }

        public GraphTab GraphTab
        {
            get { return tabFactory.GraphTab; }
        }
        public ChmTocTab ChmTocTab
        {
            get { return tabFactory.ChmTocTab; }
        }

        public FavTab FavTab
        {
            get { return tabFactory.ShapeFavoritesTab; }
        }

        public OutputExTab OutputTab
        {
            get { return tabFactory.OutputTab; }
        }
        public ZoomTab ZoomTab
        {
            get { return tabFactory.ZoomTab; }
        }

        public PropertiesTab PropertiesTab
        {
            get { return tabFactory.PropertiesTab; }
        }

        public WFNodePropertiesTab WFNodePropertiesTab
        {
            get { return tabFactory.WFNodePropertiesTab; }
        }

        public ShapeViewerTab ShapeViewerTab
        {
            get { return tabFactory.ShapeViewerTab; }
        }
        public FlowNodeViewerTab FlowNodeViewerTab
        {
            get { return tabFactory.FlowNodeViewerTab; }
        }

        #endregion

        public StatusBar StatusBar
        {
            get { return parent.sb; }
        }

        public Random Randomizer
        {
            get { return rnd; }
        }
        #endregion

        #region Constructor
        public Mediator(WFMainForm main)
        {
            this.parent = main;

            tabFactory = new TabFactory(this);
            tabFactory.OnTabCreation += new TabInfo(OnTabCall);

            samplesFactory = new SampleFactory(this);

            //init the randomizer for the placement of shapes on the canvas
            rnd = new Random();


        }
        #endregion

        #region Methods

        public void SetCaption(string text)
        {
            parent.SetCaption(text);
        }


        public void SetLayoutAlgorithm(GL.GraphLayoutAlgorithms algorithm)
        {
            //change the UI
            parent.SetLayoutAlgorithmUIState(algorithm);
            //set the control
            this.GraphControl.GraphLayoutAlgorithm = algorithm;
        }

        /// <summary>
        /// Outputs messages from the graphcontrol to the output-tab
        /// </summary>
        /// <param name="msg"></param>
        public void Output(string msg)
        {
            //tabFactory.OutputTab.Show();
            OutputBox.WriteLine("Default", msg);
        }
        /// <summary>
        /// Outputs messages from the graphcontrol to the output-tab
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="level"></param>
        public void Output(string msg, GL.OutputInfoLevels level)
        {
            //tabFactory.OutputTab.Show();
            OutputBox.WriteLine(level.ToString(), msg);
        }

        #region Opening of the various tabs


        public void OpenOuputTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Output", "输出", TabTypes.Output)) as OutputExTab;
            OnShowTab(lastAdded);
        }

        public void OpenGraphTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Diagram", "图表", TabTypes.NetronDiagram)) as GraphTab;
            OnShowTab(lastAdded);
        }

        //		public void OpenBugTab()
        //		{
        //			lastAdded = tabFactory.GetTab(new TabCodon("Bug reporting", "Bug reporting", TabTypes.BugTab)) as BugTab;
        //			OnShowTab(lastAdded);
        //		}

        public void OpenBrowserTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Browser", "浏览器", TabTypes.Browser)) as BrowserTab;
            OnShowTab(lastAdded);
        }
        public void OpenZoomTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Zoom", "缩放", TabTypes.DiagramZoom)) as ZoomTab;
            OnShowTab(lastAdded);
        }
        public void OpenShapesTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Shapes", "形状", TabTypes.ShapesViewer)) as ShapeViewerTab;
            OnShowTab(lastAdded);
        }

        public void OpenFlowNodesTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Shapes", "流程图节点库", TabTypes.FlowNodeViewer)) as FlowNodeViewerTab;
            OnShowTab(lastAdded);
        }

        public void OpenFavsTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Templates", "模板", TabTypes.ShapeFavorites)) as FavTab;
            OnShowTab(lastAdded);
        }

        public void OpenDiagramBrowserTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Diagram browser", "图表浏览器", TabTypes.DiagramBrowser)) as DiagramBrowserTab;
            OnShowTab(lastAdded);
        }

        public void OpenChmTocTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Help content", "帮忙中心", TabTypes.ChmToc)) as ChmTocTab;
            OnShowTab(lastAdded);
        }

        public void OpenCodeTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Editor", "编辑器", TabTypes.Code)) as CodeTab;
            OnShowTab(lastAdded);
        }


        public void OpenPropsTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("Properties", "属性", TabTypes.PropertyGrid)) as PropertiesTab;
            (lastAdded as PropertiesTab).PropertyGrid.LineColor = WFMainForm.LightLightColor;
            (lastAdded as PropertiesTab).PropertyGrid.CommandsBackColor = WFMainForm.LightLightColor;
            (lastAdded as PropertiesTab).PropertyGrid.ViewBackColor = WFMainForm.LightLightColor;
            OnShowTab(lastAdded);
        }

        public void OpenWFNodePropertiesTab()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("节点属性", "节点属性", TabTypes.WFNodePropertiesTab)) as WFNodePropertiesTab;
            //(lastAdded as WFNodePropertiesTab).PropertyGrid.LineColor = WFMainForm.LightLightColor;
            //(lastAdded as WFNodePropertiesTab).PropertyGrid.CommandsBackColor = WFMainForm.LightLightColor;
            //(lastAdded as WFNodePropertiesTab).PropertyGrid.ViewBackColor = WFMainForm.LightLightColor;
            OnShowTab(lastAdded);
        }



        #endregion

        #region Sample handling


        public void LoadSample(Samples sample)
        {
            try
            {
                ISample spl = samplesFactory.GetSample(sample);
                if (spl == null)
                    return;
                ResetDefault();
                spl.Run();
            }
            catch (Exception exc)
            {
                Output(exc.Message, GL.OutputInfoLevels.Exception);
            }
        }


        /// <summary>
        /// Resets the default state of the graph control
        /// </summary>
        private void ResetDefault()
        {
            this.OpenGraphTab();
            GraphControl.NewDiagram(true);
            GraphControl.Layers.Clear();
            GraphControl.AllowAddConnection = true;
            GraphControl.AllowAddShape = true;
            GraphControl.AllowDeleteShape = true;
            GraphControl.AllowMoveShape = true;
            GraphControl.BackgroundType = GL.CanvasBackgroundType.FlatColor;
            GraphControl.BackColor = Color.WhiteSmoke;
            GraphControl.Snap = false;
            GraphControl.ShowGrid = false;
            GraphControl.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
            GraphControl.DefaultConnectionPath = "Default";

            GraphControl.GraphLayoutAlgorithm = GL.GraphLayoutAlgorithms.SpringEmbedder;
            parent.SetLayoutAlgorithmUIState(GL.GraphLayoutAlgorithms.SpringEmbedder);
        }
        /// <summary>
        /// Adds a certain amount of random nodes
        /// </summary>
        /// <param name="amount"></param>
        public void AddRandomNodes(int amount)
        {
            if (amount < 1) return;
            GL.Shape shape1, shape2;
            if (GraphControl.Shapes.Count == 0)
            {
                shape1 = GraphControl.AddBasicShape("根", new Point(100, 100));
                amount--;
            }

            for (int k = 0; k < amount; k++)
            {
                shape1 = GraphControl.Shapes[rnd.Next(0, GraphControl.Shapes.Count - 1)];
                shape2 = GraphControl.AddBasicShape("随机 " + k, new Point(rnd.Next(20, GraphControl.Width - 70), rnd.Next(20, GraphControl.Height - 30)));
                Connect(shape1, shape2);
            }
        }

        #endregion
        #region Shape creation utils
        public void SetShape(GL.Shape shape)
        {
            shape.X = Randomizer.Next(50, GraphControl.Width - 100);
            shape.Y = Randomizer.Next(50, GraphControl.Height - 20);
        }

        public GL.Connection Connect(GL.Shape s1, GL.Shape s2)
        {
            return GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);
        }
        public GL.Connection Connect(GL.Shape s1, GL.Shape s2, GL.ConnectionEnd lineEnd)
        {
            GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);
            con.LineEnd = lineEnd;
            return con;
        }
        public GL.Connection Connect(GL.Shape s1, GL.Shape s2, string linePath)
        {
            GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);
            con.LinePath = linePath;
            return con;
        }

        public GL.Connection Connect(GL.Shape s1, GL.Shape s2, string linePath, GL.ConnectionEnd lineEnd)
        {
            GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);
            con.LinePath = linePath;
            con.LineEnd = lineEnd;
            return con;
        }

        #endregion

        private void OnTabCall(DockContent tab)
        {
            TabTypes type = (tab as ICobaltTab).TabType;

            switch (type)
            {
                case TabTypes.Code:
                case TabTypes.NetronDiagram:
                case TabTypes.DataGrid:
                case TabTypes.World:
                case TabTypes.Browser:
                case TabTypes.Chart:
                case TabTypes.DiagramBrowser:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.Project:
                case TabTypes.PropertyGrid:
                    tab.Show(dockPanel, DockState.DockRight);
                    break;
                case TabTypes.WFNodePropertiesTab:
                    tab.Show(dockPanel, DockState.DockRight);
                    break;
                case TabTypes.Trace:
                case TabTypes.Output:
                    tab.Show(dockPanel, DockState.DockBottom);
                    break;
                case TabTypes.ShapesViewer:
                case TabTypes.FlowNodeViewer:
                case TabTypes.ShapeFavorites:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
            }
        }
        private void OnShowTab(DockContent tab)
        {
            if (tab == null) return;
            TabTypes type = (tab as ICobaltTab).TabType;
            tab.Show(dockPanel);
            //tab.MdiParent = this.parent;

        }

        public void ShowWFNodeProperties(object obj)
        {
            if (Properties == null)
                OpenWFNodePropertiesTab();
            UCNodePropEditBase uCNodePropEditBase = null;
            if (this.WFNodePropertiesTab.UCPanel.Controls.Count > 0 && (this.WFNodePropertiesTab.UCPanel.Controls[0] as UCNodePropEditBase).SetNodeValue.ToString() != obj.ToString())
            {
                this.WFNodePropertiesTab.UCPanel.Controls.Clear();
            }
            if (this.WFNodePropertiesTab.UCPanel.Controls.Count == 0)
            {
                uCNodePropEditBase = new UCNodePropEditBase();
                this.WFNodePropertiesTab.UCPanel.Controls.Add(uCNodePropEditBase);
            }
            switch (obj.ToString())
            {
                case "RUINORERP.WF.WorkFlowContextData":

                    if (!(this.WFNodePropertiesTab.UCPanel.Controls[0] is UCWFStartEdit))
                    {
                        this.WFNodePropertiesTab.UCPanel.Controls.Clear();
                        uCNodePropEditBase = new UCWFStartEdit();
                    }
                    else
                    {
                        uCNodePropEditBase = this.WFNodePropertiesTab.UCPanel.Controls[0] as UCNodePropEditBase;
                    }


                    break;
                case "RUINORERP.WF.BizOperation.Steps.ApprovalStep":
                    if (!(this.WFNodePropertiesTab.UCPanel.Controls[0] is ApprovalStepUI))
                    {
                        this.WFNodePropertiesTab.UCPanel.Controls.Clear();
                        uCNodePropEditBase = new WF.BizOperation.Steps.ApprovalStepUI();

                    }
                    else
                    {
                        uCNodePropEditBase = this.WFNodePropertiesTab.UCPanel.Controls[0] as UCNodePropEditBase;
                    }

                    break;
                case "RUINORERP.WF.BizOperation.Steps.SubmitStep":
                    if (!(this.WFNodePropertiesTab.UCPanel.Controls[0] is ApprovalStepUI))
                    {
                        this.WFNodePropertiesTab.UCPanel.Controls.Clear();
                        uCNodePropEditBase = new WF.BizOperation.Steps.ApprovalStepUI();

                    }
                    else
                    {
                        uCNodePropEditBase = this.WFNodePropertiesTab.UCPanel.Controls[0] as UCNodePropEditBase;
                    }

                    break;
                case "RUINORERP.WF.BizOperation.Condition.ConditionConfig":
                    if ( !(this.WFNodePropertiesTab.UCPanel.Controls[0] is ConditionConfigUI))
                    {
                        this.WFNodePropertiesTab.UCPanel.Controls.Clear();
                        uCNodePropEditBase = new ConditionConfigUI();
                    }
                    else
                    {
                        uCNodePropEditBase = this.WFNodePropertiesTab.UCPanel.Controls[0] as UCNodePropEditBase;
                    }

                    break;
                default:
                    break;
            }

            WFNodePropertiesTab.Text = uCNodePropEditBase.NodePropName;
            uCNodePropEditBase.SetNodeValue = obj;
            this.WFNodePropertiesTab.UCPanel.Controls.Add(uCNodePropEditBase);
            uCNodePropEditBase.Dock = DockStyle.Fill;

            this.WFNodePropertiesTab.ObjectPropertyValue = obj;
        }


        public void ShowProperties(object obj)
        {
            if (Properties == null)
                OpenPropsTab();

            this.Properties.SelectedObject = obj;
        }
        public void ShowProperties(object[] obj)
        {
            if (Properties == null)
                OpenPropsTab();

            this.Properties.SelectedObjects = obj;
        }

        public void Navigate(string url)
        {
            if (Browser == null)
                OpenBrowserTab();
            this.Browser.Navigate(url);
        }

        #endregion

        /// <summary>
        /// Hides all tha tabpages from the interface
        /// </summary>
        public void HideAllTabs()
        {
            foreach (DockContent dp in dockPanel.Contents)
            {
                dp.Hide();
            }
        }
        /// <summary>
        /// Opens a file in the editor
        /// </summary>
        public void OpenTextFile()
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "All files (*.*)|*.*| NML files (*.NML)|*.NML";
            fileChooser.FilterIndex = 2;//choose NML by default
            DialogResult result = fileChooser.ShowDialog();
            string filename;
            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Editor.LoadFile(filename);
                if (filename.ToLower().EndsWith("nml"))
                    Editor.SetHighlighting("XML");
                else if (filename.ToLower().EndsWith("cs"))
                    Editor.SetHighlighting("C#");
                else if (filename.ToLower().EndsWith(".vb"))
                    Editor.SetHighlighting("VBNET");
                else if (filename.ToLower().EndsWith(".htm"))
                    Editor.SetHighlighting("HTML");
                else if (filename.ToLower().EndsWith(".html"))
                    Editor.SetHighlighting("HTML");
                else
                    Editor.SetDefaultHighlighting();

                TestTextForNML();
            }
        }

        /// <summary>
        /// Saves the content of the editor
        /// </summary>
        public void SaveTextFile()
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.CheckFileExists = false;
            fileChooser.Filter = "All files (*.*)|*.*| NML files (*.NML)|*.NML";
            fileChooser.InitialDirectory = "\\c:";
            fileChooser.RestoreDirectory = true;
            DialogResult result = fileChooser.ShowDialog();
            string filename;

            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Editor.SaveFile(filename);
            }
        }

        /// <summary>
        /// Tests if the loaded text is an XML file with NML-root
        /// </summary>
        internal void TestTextForNML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Editor.Text);
                if (doc.DocumentElement.Name.ToLower() == "nml")
                    parent.mnuShowNMLInDiagram.Enabled = true;
            }
            catch
            {
                parent.mnuShowNMLInDiagram.Enabled = false;
            }
        }




        public void TransferNMLToEditor()
        {
            this.Editor.Text = this.GraphControl.GetNML();
            this.Editor.SetHighlighting("XML");
            this.OpenCodeTab();
        }

        /// <summary>
        /// 设置条件
        /// </summary>
        public void SetCondition(object sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            Connection selectConn = sender as Connection;

            //frmConditionConfig frm = new frmConditionConfig();
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    selectConn.Tag = "asdfa";
            //}

        }


        /// <summary>
        /// 设置下一步步骤
        /// </summary>
        public void SetNextStepByLine(object sender, ConnectionEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            if (sender is GraphControl graphControl)
            {

                #region 开始节点不能有多分支 NodeStepPropertyValue 这个属性的值，目前只有两种。一种是流程。一种是节点
                if (((e.From.BelongsTo.NodeStepPropertyValue) is WorkFlowContextData))
                {
                    //如果他的来源节点有两条引出的线。则认为有分支。则用选择的分支，否则就使用下一步功能。
                    List<Shape> StartBranchList = new List<Shape>();
                    foreach (Connector connector in e.From.BelongsTo.Connectors)
                    {
                        for (int i = 0; i < connector.Connections.Count; i++)
                        {
                            if (connector.Connections[i].From.BelongsTo.UID == e.From.BelongsTo.UID)
                            {
                                StartBranchList.Add(connector.Connections[i].From.BelongsTo);
                            }
                        }

                    }

                    if (StartBranchList.Count > 1)
                    {
                        MessageBox.Show("开始节点不能有多分支", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //删除这条线
                        //要调用GraphControl中的删除方法，因为他里面的保护级
                        graphControl.DeleteUIConnectionLine(e.Connection);
                        return;
                    }

                }
                #endregion 

                List<BaseNode> baseNodes = new List<BaseNode>();
                foreach (Shape shape in graphControl.Shapes)
                {
                    baseNodes.Add(shape as BaseNode);
                }
                if (graphControl.WorkflowData == null)
                {
                    #region 设置流程数据
                    this.GraphControl.WorkflowData = baseNodes.FirstOrDefault(c => c.NodeType == WFNodeType.Start).NodeStepPropertyValue;
                    #endregion
                }
                if (graphControl.WorkflowData != null)
                {
                    if (graphControl.WorkflowData is WorkFlowContextData workFlowConfigData)
                    {
                        //startRoolNode.Steps = e.To.BelongsTo;
                        List<BaseStepBody> baseStepBodies = baseNodes.Where(baseNodes => baseNodes.NodeType == WFNodeType.Step).Select(baseNodes => baseNodes.NodeStepPropertyValue as BaseStepBody).ToList();
                        //存在的数据不能重复添加
                        foreach (BaseStepBody bsb in baseStepBodies)
                        {
                            if (!workFlowConfigData.Steps.Contains(bsb))
                            {
                                workFlowConfigData.Steps.Add(bsb);
                            }
                        }

                    }
                }
            }

            //from 点 属所属的节点，to 点 属的steps属性
            if (e.To.BelongsTo is BaseNode bn)
            {
                if (bn.NodeType == WFNodeType.Step)
                {
                    if (e.From.BelongsTo.NodeStepPropertyValue is BaseStepBody nodeStep)
                    {
                        //如果他的来源节点有两条引出的线。则认为有分支。则用选择的分支，否则就使用下一步功能。
                        List<Shape> branchList = new List<Shape>();
                        List<Shape> TargetList = new List<Shape>();
                        foreach (Connector connector in e.From.BelongsTo.Connectors)
                        {
                            for (int i = 0; i < connector.Connections.Count; i++)
                            {
                                if (connector.Connections[i].From.BelongsTo.UID == e.From.BelongsTo.UID)
                                {
                                    branchList.Add(connector.Connections[i].From.BelongsTo);
                                    TargetList.Add(connector.Connections[i].To.BelongsTo);
                                }
                            }

                        }
                        //为分支
                        if (branchList.Count > 1)
                        {
                            #region 添加分支
                            nodeStep.NextStepId = null;
                            if (nodeStep.SelectNextStep == null)
                            {
                                nodeStep.SelectNextStep = new Dictionary<string, string>();
                            }

                            //添加分支
                            foreach (Shape s in TargetList)
                            {
                                string NextStepID = (s.NodeStepPropertyValue as BaseStepBody).Id;
                                if (!nodeStep.SelectNextStep.ContainsKey(NextStepID))
                                {
                                    nodeStep.SelectNextStep.Add(NextStepID, "");
                                }
                            }
                            //给分支的条件属性赋值,先初始化一下，两个条件线
                            //e.Connection.NodeStepPropertyValue = new ConditionConfig();

                            //找到当前线的发布节点的所有的点。
                            foreach (Connector connector in e.Connection.From.BelongsTo.Connectors)
                            {
                                //在发出节点的所有点对应的线。
                                for (int i = 0; i < connector.Connections.Count; i++)
                                {
                                    //如果这些线的终点是上面分支中的节点，就是条件线
                                    if (TargetList.Contains(connector.Connections[i].To.BelongsTo))
                                    {
                                        connector.Connections[i].NodeStepPropertyValue = new ConditionConfig();
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            nodeStep.NextStepId = (e.To.BelongsTo.NodeStepPropertyValue as BaseStepBody).Id;
                        }
                    }

                    //开始节点引出的时。
                    if (e.From.BelongsTo.NodeStepPropertyValue is WorkFlowContextData wfConfigData)
                    {
                        //将指向的节点删除后。再插入到第一个位置
                        wfConfigData.Steps.Remove(e.To.BelongsTo.NodeStepPropertyValue as BaseStepBody);
                        wfConfigData.Steps.Insert(0, e.To.BelongsTo.NodeStepPropertyValue as BaseStepBody);

                    }
                }
            }




        }


        /// <summary>
        /// 设置下一步步骤
        /// </summary>
        public void AddStep(object sender, Shape e)
        {
            if (sender == null)
            {
                return;
            }
            if (sender is GraphControl graphControl)
            {
                List<BaseNode> baseNodes = new List<BaseNode>();
                foreach (Shape shape in graphControl.Shapes)
                {
                    baseNodes.Add(shape as BaseNode);
                }
                if (graphControl.WorkflowData == null)
                {
                    #region 设置流程数据
                    if (baseNodes.FirstOrDefault(c => c.NodeType == WFNodeType.Start) != null)
                    {
                        this.GraphControl.WorkflowData = baseNodes.FirstOrDefault(c => c.NodeType == WFNodeType.Start).NodeStepPropertyValue;
                    }

                    #endregion
                }
                if (graphControl.WorkflowData != null)
                {
                    if (graphControl.WorkflowData is WorkFlowContextData workFlowConfigData)
                    {
                        //startRoolNode.Steps = e.To.BelongsTo;
                        List<BaseStepBody> baseStepBodies = baseNodes.Where(baseNodes => baseNodes.NodeType == WFNodeType.Step).Select(baseNodes => baseNodes.NodeStepPropertyValue as BaseStepBody).ToList();
                        //存在的数据不能重复添加
                        foreach (BaseStepBody bsb in baseStepBodies)
                        {
                            if (!workFlowConfigData.Steps.Contains(bsb))
                            {
                                if (bsb == null)
                                {

                                }
                                else
                                {
                                    workFlowConfigData.Steps.Add(bsb);
                                }

                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 删除下一步步骤，由于删除线导致的
        /// </summary>
        public void DeleteNextStepByLine(object sender, ConnectionEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            if (sender is GraphControl graphControl)
            {
                List<BaseNode> baseNodes = new List<BaseNode>();
                foreach (Shape shape in graphControl.Shapes)
                {
                    baseNodes.Add(shape as BaseNode);
                }
                //如果他的来源节点有两条引出的线。则认为有分支。则用选择的分支，否则就使用下一步功能。

                //保存了发出的节点，按线的走向，这个里面是相同的节点。
                List<Shape> branchList = new List<Shape>();

                //保存了发出的节点，按线的走向，这个里面是不同的节点。
                List<Shape> TargetList = new List<Shape>();

                foreach (Connector connector in e.From.BelongsTo.Connectors)
                {
                    for (int i = 0; i < connector.Connections.Count; i++)
                    {
                        if (connector.Connections[i].From.BelongsTo.UID == e.From.BelongsTo.UID)
                        {
                            branchList.Add(connector.Connections[i].From.BelongsTo);
                            TargetList.Add(connector.Connections[i].To.BelongsTo);
                        }
                    }
                }

                if (graphControl.WorkflowData == null)
                {
                    #region 设置流程数据
                    this.GraphControl.WorkflowData = baseNodes.FirstOrDefault(c => c.NodeType == WFNodeType.Start).NodeStepPropertyValue;
                    #endregion
                }

                //根据上面的 branchList  和 TargetList  可以判断  有没有分支。

                if (graphControl.WorkflowData != null)
                {
                    if (graphControl.WorkflowData is WorkFlowContextData workFlowConfigData)
                    {
                        if (branchList.Count == TargetList.Count && branchList.Count == 1)
                        {
                            #region 没有分支时
                            //删除这个线的终点的对象
                            if (e.To.BelongsTo.AdjacentNodes.Count == 1 && e.To.BelongsTo.NodeStepPropertyValue is BaseStepBody nodeStep)
                            {
                                workFlowConfigData.Steps.Remove(e.To.BelongsTo.NodeStepPropertyValue as BaseStepBody);
                            }
                            else
                            {
                                //,但是如果这个对象还有其他线指向他，则只将这个线发出端的NextStepId置空
                                if (e.From.BelongsTo.NodeStepPropertyValue is BaseStepBody UpLevelNodeStep)
                                {
                                    UpLevelNodeStep.NextStepId = null;
                                }
                            }
                            #endregion
                        }
                        if (branchList.Count == TargetList.Count && branchList.Count > 1)
                        {
                            #region 有分支时 减掉一条线呢？
                            //判断对其他节点和条件的影响，影响：NextStepId和SelectNextStep
                            //找到当前线的发布节点的所有的点。
                            for (int i = 0; i < TargetList.Count; i++)
                            {
                                if (TargetList[i].UID == e.To.BelongsTo.UID)
                                {
                                    //删除自己的线的终点的节点
                                    workFlowConfigData.Steps.Remove(TargetList[i].NodeStepPropertyValue as BaseStepBody);
                                    TargetList.Remove(TargetList[i]);
                                }
                            }

                            if (TargetList.Count > 1)
                            {
                                if (e.From.BelongsTo.NodeStepPropertyValue is BaseStepBody UpLevelNodeStep)
                                {
                                    UpLevelNodeStep.NextStepId = null;
                                    UpLevelNodeStep.SelectNextStep.Clear();
                                    foreach (var item in TargetList)
                                    {
                                        string NextStepID = (item.NodeStepPropertyValue as BaseStepBody).Id;
                                        if (!UpLevelNodeStep.SelectNextStep.ContainsKey(NextStepID))
                                        {
                                            UpLevelNodeStep.SelectNextStep.Add(NextStepID, "");
                                        }
                                    }


                                }
                            }
                            else
                            {
                                //只有一个节点，就是下一个
                                if (e.From.BelongsTo.NodeStepPropertyValue is BaseStepBody UpLevelNodeStep)
                                {
                                    UpLevelNodeStep.NextStepId = (TargetList[0].NodeStepPropertyValue as BaseStepBody).Id;
                                    UpLevelNodeStep.SelectNextStep.Clear();
                                }
                            }
                            #endregion
                        }



                    }
                }
            }



        }

        /// <summary>
        /// 删除步骤
        /// </summary>
        public void DeleteStep(object sender, Shape s)
        {
            if (sender == null)
            {
                return;
            }
            GraphControl graphControl = sender as GraphControl;

            //from 点 属所属的节点，to 点 属的steps属性
            if (s is BaseNode baseNode)
            {
                if (baseNode.NodeType == WFNodeType.Start)
                {
                    if (s.NodeStepPropertyValue is WorkFlowContextData startRoolNode)
                    {
                        //startRoolNode.Steps = e.To.BelongsTo;
                        startRoolNode.Steps.Remove(s.NodeStepPropertyValue as BaseStepBody);

                        graphControl.WorkflowData = null;

                    }
                }
                if (baseNode.NodeType == WFNodeType.Step)
                {
                    if (graphControl.WorkflowData != null && graphControl.WorkflowData is WorkFlowContextData workFlowConfigData)
                    {
                        if (s.NodeStepPropertyValue is BaseStepBody nodeStep)
                        {
                            workFlowConfigData.Steps.Remove(nodeStep);
                            //他上级的NextStepId也要置空

                            for (int i = 0; i < s.AdjacentNodes.Count; i++)
                            {
                                if (s.AdjacentNodes[i].NodeStepPropertyValue is BaseStepBody UpLevelNodeStep)
                                {
                                    UpLevelNodeStep.NextStepId = null;
                                }
                            }


                        }
                    }

                }

            }

        }









        /// <summary>
        /// 设置节点属性的值
        /// </summary>
        public void SetNodeProperty(object sender, object e)
        {
            if (sender == null)
            {
                return;
            }
            this.OpenWFNodePropertiesTab();
            this.ShowWFNodeProperties(e);

        }


        /// <summary>
        /// 转换为json格式的工作流内容填写到编辑器
        /// </summary>
        public void TransferWorkFlowJsonToEditor()
        {
            Service.JSONSerializer jSONSerializer = new Service.JSONSerializer(this.GraphControl);
            this.Editor.Text = jSONSerializer.Serialize();
            this.Editor.SetHighlighting("C#");
            this.OpenCodeTab();
        }

        /// <summary>
        ///  
        /// </summary>
        public async void TestWorkFlowByJson(object sender, EventArgs e)
        {
            Service.JSONSerializer jSONSerializer = new Service.JSONSerializer(this.GraphControl);
            jSONSerializer.JsonText = this.Editor.Text;
            await jSONSerializer.TestWorkflow();
        }


    }
}
