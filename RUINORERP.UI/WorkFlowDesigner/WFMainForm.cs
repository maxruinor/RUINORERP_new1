using System;
using System.Drawing.Printing;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.GraphLib;
using System.Diagnostics;
using System.IO;
using Netron.Neon;
using System.Xml;
using System.Reflection;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using Krypton.Navigator;
using Krypton.Workspace;
using static Netron.GraphLib.UI.GraphControl;
using Netron.GraphLib.UI;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using RUINORERP.WF;
using System.Collections.Generic;

namespace RUINORERP.UI.WorkFlowDesigner
{

    [MenuAttrAssemblyInfo("可视化设计器", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.流程设计)]
    /// <summary>
    /// The MainForm collects a wide spectrum of features offered by the Netron graph control
    /// in one application.
    /// </summary>
    public class WFMainForm : UserControl
    {
        #region Constants
        public static readonly Color LightLightColor = Color.LightGray;
        public static readonly string CaptionPrefix = "Cobalt [Graph Library 2.2]";
        #endregion

        #region Delegates and events
        /// <summary>
        /// Transfers a notification to the splashscreen about the module being loaded
        /// </summary>
        public delegate void LoadingInfo(string moduleName);
        /// <summary>
        /// Occurs when a module is being loaded (related to the splashscreen)
        /// </summary>
        public event LoadingInfo Loading;
        #endregion

        #region Fields


        /* though I'm a big fan of code documentation I haven't commented these fields
		 * for the rather obvious reason that it wouldn't be more enlighting than what you see
		 * in the form-designer of Visual Studio
		 */


        private bool m_bLayoutCalled = false;
        bool mLoaded = false;
        private DateTime m_dt;
        private DeserializeDockContent deserializeDockContent;

        internal System.Windows.Forms.StatusBar sb;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuPrintPreview;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveImage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuStartLayout;
        private System.Windows.Forms.ToolStripMenuItem mnuStopLayout;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom100;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom50;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom200;
        private System.Windows.Forms.ImageList TabImages;
        private System.Windows.Forms.ToolStripMenuItem mnuLayoutExample;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mnuNoNewConnections;
        private System.Windows.Forms.ToolStripMenuItem mnuNoNewShapes;
        private System.Windows.Forms.ToolStripMenuItem mnuBackground;
        private System.Windows.Forms.ToolStripMenuItem mnuSnap;


        private System.Windows.Forms.ToolStripMenuItem mnuSave2GraphML;
        private System.Windows.Forms.ToolStripMenuItem mnuNewDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuFancyConnections;
        private System.Windows.Forms.ToolStripMenuItem mnuMultiPrint;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenGraphML;
        private System.Windows.Forms.ToolStripMenuItem mnuGraphProperties;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem mnuSpringEmbedder;
        private System.Windows.Forms.ToolStripMenuItem mnuTreeLayout;
        private System.Windows.Forms.ToolStripMenuItem mnuRandomizerLayout;
        private System.Windows.Forms.ContextMenu outputMenu;
        private System.Windows.Forms.MenuItem mnuClearAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem mnuAnalysis;
        private System.Windows.Forms.ToolStripMenuItem mnuSpanningTree;
        private System.Windows.Forms.ToolStripMenuItem mnuLayers;
        private System.Windows.Forms.ToolStripMenuItem mnuZorderExample;
        private System.Windows.Forms.ToolStripMenuItem mnuLayeringExample;
        private System.Windows.Forms.ImageList ButtonImages;
        private System.Windows.Forms.ToolStripMenuItem mnuTrees;
        private System.Windows.Forms.ToolStripMenuItem mnuTreeAsCode;
        private System.Windows.Forms.ToolStripMenuItem mnuRandomTree;
        private System.Windows.Forms.ToolStripMenuItem mnuRandomNodeAddition;
        private System.Windows.Forms.ToolStripMenuItem mnuNoLinkFrom;
        private System.Windows.Forms.ToolStripMenuItem mnuItemCannotMove;
        private System.Windows.Forms.ToolStripMenuItem mnuContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuShapeEvents;
        private Mediator mediator;
        private System.Windows.Forms.ToolStripMenuItem mnuWindows;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowShapes;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowFavs;
        private System.Windows.Forms.ToolStripMenuItem mnuBrowser;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomIn;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomOut;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuWFNodeWindowProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuOutput;
        private System.Windows.Forms.ToolStripMenuItem mnuEditor;
        private System.Windows.Forms.ToolStripMenuItem mnuEditorMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlighting;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlightXML;
        private System.Windows.Forms.ToolStripMenuItem mnuExamples;
        private System.Windows.Forms.ToolStripMenuItem mnuDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuApplication;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlightHTML;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlighCsharp;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlightVBNet;
        private System.Windows.Forms.ToolStripMenuItem mnuHighlightNone;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenTextFile;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveTextFile;
        private System.Windows.Forms.ToolStripMenuItem mnuNMLToEditor;
        internal System.Windows.Forms.ToolStripMenuItem mnuShowNMLInDiagram;
        private System.Windows.Forms.ToolStripMenuItem mnuEditorCut;
        private System.Windows.Forms.ToolStripMenuItem mnuEditorCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuEditorPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuEditorDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuColorVisit;
        private System.Windows.Forms.ToolStripMenuItem mnuDiagramBrowser;
        private System.Windows.Forms.ToolStripMenuItem mnuCreateTemplate;
        private System.Windows.Forms.ToolStripMenuItem mnuCreateSVG;
        private System.Windows.Forms.ToolStripMenuItem mnuShowMode;
        private System.Windows.Forms.ToolStripMenuItem mnuSave2HTML;
        private System.Windows.Forms.ToolStripMenuItem mnuCreateImageShape;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuCut;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuRedo;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyAsImage;
        private System.Windows.Forms.ToolStripMenuItem mnuClassInheritance;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;



        private ToolStripContainer toolStripContainer1;
        private MenuStrip WFMenuStrip;
        private ToolStripMenuItem 流程设计元素库ToolStripMenuItem;
        private ToolStripMenuItem jsonTo编辑器ToolStripMenuItem;
        private ToolStripMenuItem 测试工作流ToolStripMenuItem;
        private Netron.Neon.DockPanel dockPanel;

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public WFMainForm()
        {
            //this gets the state of the docking as it was the last run
            deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //part of the default Visual Studio setup
            InitializeComponent();
            //the root of the whole application connecting the different parts;
            mediator = new Mediator(this);
            mediator.DockPanel = dockPanel;

            #region Docking extender
            //Commenting out these lines will give you the traditional style.
            //If you use the Netron.Neon.Docking.Extenders.Blue.Extender you'll get another style.
            //The VS2005 mimics the Visual Studio tabs and colors
            dockPanel.Extender.AutoHideTabFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.AutoHideTabFromBaseFactory();
            dockPanel.Extender.DockPaneTabFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneTabFromBaseFactory();
            dockPanel.Extender.AutoHideStripFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.AutoHideStripFromBaseFactory();
            dockPanel.Extender.DockPaneCaptionFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneCaptionFromBaseFactory();
            dockPanel.Extender.DockPaneStripFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneStripFromBaseFactory();

            #endregion
            //if deserialization of the docking didn't work, this will set the default menu state
            SetContentMenu(TabTypes.Unknown, false);


        }
        #endregion

        #region Methods

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WFMainForm));
            this.sb = new System.Windows.Forms.StatusBar();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyAsImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave2GraphML = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenGraphML = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNMLToEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.jsonTo编辑器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMultiPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGraphProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStartLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStopLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpringEmbedder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTreeLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRandomizerLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpanningTree = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuColorVisit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLayers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCreateTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCreateImageShape = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCreateSVG = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave2HTML = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExamples = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLayoutExample = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNoNewConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNoNewShapes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNoLinkFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemCannotMove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSnap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFancyConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZorderExample = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLayeringExample = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrees = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTreeAsCode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRandomTree = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRandomNodeAddition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShapeEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClassInheritance = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenTextFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveTextFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlighting = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlightXML = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlightHTML = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlighCsharp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlightVBNet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighlightNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditorCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditorCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditorPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditorDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNMLInDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowShapes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowFavs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowProperties = new System.Windows.Forms.ToolStripMenuItem();
            mnuWFNodeWindowProperties = new ToolStripMenuItem();
            this.mnuOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiagramBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.流程设计元素库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputMenu = new System.Windows.Forms.ContextMenu();
            this.mnuClearAll = new System.Windows.Forms.MenuItem();
            this.TabImages = new System.Windows.Forms.ImageList(this.components);
            this.ButtonImages = new System.Windows.Forms.ImageList(this.components);
            this.dockPanel = new Netron.Neon.DockPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.WFMenuStrip = new System.Windows.Forms.MenuStrip();
            this.测试工作流ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.WFMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 498);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(928, 22);
            this.sb.TabIndex = 11;
            // 
            // mnuApplication
            // 
            this.mnuApplication.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowMode});
            this.mnuApplication.Name = "mnuApplication";
            this.mnuApplication.Size = new System.Drawing.Size(71, 20);
            this.mnuApplication.Text = "应用程序";
            // 
            // mnuShowMode
            // 
            this.mnuShowMode.Name = "mnuShowMode";
            this.mnuShowMode.Size = new System.Drawing.Size(126, 22);
            this.mnuShowMode.Text = "演示模式";
            this.mnuShowMode.Click += new System.EventHandler(this.mnuShowMode_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndo,
            this.mnuRedo,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuDelete,
            this.mnuCopyAsImage,
            this.mnuSelectAll});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(45, 20);
            this.mnuEdit.Text = "编辑";
            // 
            // mnuUndo
            // 
            this.mnuUndo.Enabled = false;
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.Size = new System.Drawing.Size(139, 22);
            this.mnuUndo.Text = "撤销";
            // 
            // mnuRedo
            // 
            this.mnuRedo.Enabled = false;
            this.mnuRedo.Name = "mnuRedo";
            this.mnuRedo.Size = new System.Drawing.Size(139, 22);
            this.mnuRedo.Text = "重做";
            // 
            // mnuCut
            // 
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.Size = new System.Drawing.Size(139, 22);
            this.mnuCut.Text = "剪切";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(139, 22);
            this.mnuCopy.Text = "复制";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.Size = new System.Drawing.Size(139, 22);
            this.mnuPaste.Text = "粘贴";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(139, 22);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuCopyAsImage
            // 
            this.mnuCopyAsImage.Name = "mnuCopyAsImage";
            this.mnuCopyAsImage.Size = new System.Drawing.Size(139, 22);
            this.mnuCopyAsImage.Text = "复制为图片";
            this.mnuCopyAsImage.Click += new System.EventHandler(this.mnuCopyAsImage_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.Size = new System.Drawing.Size(139, 22);
            this.mnuSelectAll.Text = "全选";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuDiagram
            // 
            this.mnuDiagram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveDiagram,
            this.mnuNewDiagram,
            this.mnuOpenDiagram,
            this.mnuSave2GraphML,
            this.mnuOpenGraphML,
            this.mnuNMLToEditor,
            this.jsonTo编辑器ToolStripMenuItem,
            this.mnuMultiPrint,
            this.mnuPrintPreview,
            this.mnuContextMenu,
            this.mnuGraphProperties,
            this.mnuSaveImage,
            this.ToolStripMenuItem1,
            this.mnuAnalysis,
            this.mnuLayers,
            this.mnuCreateTemplate,
            this.mnuCreateImageShape,
            this.mnuCreateSVG,
            this.mnuSave2HTML,
            this.测试工作流ToolStripMenuItem});
            this.mnuDiagram.Name = "mnuDiagram";
            this.mnuDiagram.Size = new System.Drawing.Size(45, 20);
            this.mnuDiagram.Text = "图表";
            // 
            // mnuSaveDiagram
            // 
            this.mnuSaveDiagram.Name = "mnuSaveDiagram";
            this.mnuSaveDiagram.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveDiagram.Text = "保存";
            this.mnuSaveDiagram.Click += new System.EventHandler(this.mnuSaveDiagram_Click);
            // 
            // mnuNewDiagram
            // 
            this.mnuNewDiagram.Name = "mnuNewDiagram";
            this.mnuNewDiagram.Size = new System.Drawing.Size(180, 22);
            this.mnuNewDiagram.Text = "新建";
            this.mnuNewDiagram.Click += new System.EventHandler(this.mnuNewDiagram_Click);
            // 
            // mnuOpenDiagram
            // 
            this.mnuOpenDiagram.Name = "mnuOpenDiagram";
            this.mnuOpenDiagram.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenDiagram.Text = "打开";
            this.mnuOpenDiagram.Click += new System.EventHandler(this.mnuOpenDiagram_Click);
            // 
            // mnuSave2GraphML
            // 
            this.mnuSave2GraphML.Name = "mnuSave2GraphML";
            this.mnuSave2GraphML.Size = new System.Drawing.Size(180, 22);
            this.mnuSave2GraphML.Text = "保存为NML";
            this.mnuSave2GraphML.Click += new System.EventHandler(this.mnuSave2NML_Click);
            // 
            // mnuOpenGraphML
            // 
            this.mnuOpenGraphML.Name = "mnuOpenGraphML";
            this.mnuOpenGraphML.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenGraphML.Text = "打开NML";
            this.mnuOpenGraphML.Click += new System.EventHandler(this.mnuOpenGraphML_Click);
            // 
            // mnuNMLToEditor
            // 
            this.mnuNMLToEditor.Name = "mnuNMLToEditor";
            this.mnuNMLToEditor.Size = new System.Drawing.Size(180, 22);
            this.mnuNMLToEditor.Text = "NML to 编辑器";
            this.mnuNMLToEditor.Click += new System.EventHandler(this.mnuNMLToEditor_Click);
            // 
            // jsonTo编辑器ToolStripMenuItem
            // 
            this.jsonTo编辑器ToolStripMenuItem.Name = "jsonTo编辑器ToolStripMenuItem";
            this.jsonTo编辑器ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.jsonTo编辑器ToolStripMenuItem.Text = "Json To 编辑器";
            this.jsonTo编辑器ToolStripMenuItem.Click += new System.EventHandler(this.jsonTo编辑器ToolStripMenuItem_Click);
            // 
            // mnuMultiPrint
            // 
            this.mnuMultiPrint.Name = "mnuMultiPrint";
            this.mnuMultiPrint.Size = new System.Drawing.Size(180, 22);
            this.mnuMultiPrint.Text = "多页打印";
            this.mnuMultiPrint.Click += new System.EventHandler(this.mnuMultiPrint_Click);
            // 
            // mnuPrintPreview
            // 
            this.mnuPrintPreview.Name = "mnuPrintPreview";
            this.mnuPrintPreview.Size = new System.Drawing.Size(180, 22);
            this.mnuPrintPreview.Text = "打印预览";
            this.mnuPrintPreview.Click += new System.EventHandler(this.mnuPrintPreview_Click);
            // 
            // mnuContextMenu
            // 
            this.mnuContextMenu.Checked = true;
            this.mnuContextMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuContextMenu.Name = "mnuContextMenu";
            this.mnuContextMenu.Size = new System.Drawing.Size(180, 22);
            this.mnuContextMenu.Click += new System.EventHandler(this.ContextMenuSwitch_Click);
            // 
            // mnuGraphProperties
            // 
            this.mnuGraphProperties.Name = "mnuGraphProperties";
            this.mnuGraphProperties.Size = new System.Drawing.Size(180, 22);
            this.mnuGraphProperties.Text = "图形属性";
            this.mnuGraphProperties.Click += new System.EventHandler(this.mnuGraphProperties_Click);
            // 
            // mnuSaveImage
            // 
            this.mnuSaveImage.Name = "mnuSaveImage";
            this.mnuSaveImage.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveImage.Text = "Save graph image";
            this.mnuSaveImage.Click += new System.EventHandler(this.mnuSaveImage_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStartLayout,
            this.mnuStopLayout,
            this.ToolStripMenuItem10,
            this.mnuSpringEmbedder,
            this.mnuTreeLayout,
            this.mnuRandomizerLayout,
            this.ToolStripMenuItem12});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItem1.Text = "布局";
            // 
            // mnuStartLayout
            // 
            this.mnuStartLayout.Name = "mnuStartLayout";
            this.mnuStartLayout.Size = new System.Drawing.Size(167, 22);
            this.mnuStartLayout.Text = "Start layout";
            this.mnuStartLayout.Click += new System.EventHandler(this.mnuStartLayout_Click);
            // 
            // mnuStopLayout
            // 
            this.mnuStopLayout.Name = "mnuStopLayout";
            this.mnuStopLayout.Size = new System.Drawing.Size(167, 22);
            this.mnuStopLayout.Text = "Stop layout";
            this.mnuStopLayout.Click += new System.EventHandler(this.mnuStopLayout_Click);
            // 
            // ToolStripMenuItem10
            // 
            this.ToolStripMenuItem10.Name = "ToolStripMenuItem10";
            this.ToolStripMenuItem10.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItem10.Text = "-";
            // 
            // mnuSpringEmbedder
            // 
            this.mnuSpringEmbedder.Checked = true;
            this.mnuSpringEmbedder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuSpringEmbedder.Name = "mnuSpringEmbedder";
            this.mnuSpringEmbedder.Size = new System.Drawing.Size(167, 22);
            this.mnuSpringEmbedder.Text = "Spring-embedder";
            this.mnuSpringEmbedder.Click += new System.EventHandler(this.mnuSpringEmbedder_Click);
            // 
            // mnuTreeLayout
            // 
            this.mnuTreeLayout.Name = "mnuTreeLayout";
            this.mnuTreeLayout.Size = new System.Drawing.Size(167, 22);
            this.mnuTreeLayout.Text = "Tree layout";
            this.mnuTreeLayout.Click += new System.EventHandler(this.mnuTreeLayout_Click);
            // 
            // mnuRandomizerLayout
            // 
            this.mnuRandomizerLayout.Name = "mnuRandomizerLayout";
            this.mnuRandomizerLayout.Size = new System.Drawing.Size(167, 22);
            this.mnuRandomizerLayout.Text = "Randomizer";
            this.mnuRandomizerLayout.Click += new System.EventHandler(this.mnuRandomizerLayout_Click);
            // 
            // ToolStripMenuItem12
            // 
            this.ToolStripMenuItem12.Name = "ToolStripMenuItem12";
            this.ToolStripMenuItem12.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItem12.Text = "-";
            // 
            // mnuAnalysis
            // 
            this.mnuAnalysis.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSpanningTree,
            this.mnuColorVisit});
            this.mnuAnalysis.Name = "mnuAnalysis";
            this.mnuAnalysis.Size = new System.Drawing.Size(180, 22);
            this.mnuAnalysis.Text = "Analysis";
            // 
            // mnuSpanningTree
            // 
            this.mnuSpanningTree.Name = "mnuSpanningTree";
            this.mnuSpanningTree.Size = new System.Drawing.Size(147, 22);
            this.mnuSpanningTree.Text = "Spanning tree";
            this.mnuSpanningTree.Click += new System.EventHandler(this.mnuSpanningTree_Click);
            // 
            // mnuColorVisit
            // 
            this.mnuColorVisit.Name = "mnuColorVisit";
            this.mnuColorVisit.Size = new System.Drawing.Size(147, 22);
            this.mnuColorVisit.Text = "Colored visit";
            this.mnuColorVisit.Click += new System.EventHandler(this.mnuColoredVisit_Click);
            // 
            // mnuLayers
            // 
            this.mnuLayers.Name = "mnuLayers";
            this.mnuLayers.Size = new System.Drawing.Size(180, 22);
            this.mnuLayers.Text = "Layers";
            this.mnuLayers.Click += new System.EventHandler(this.mnuLayers_Click);
            // 
            // mnuCreateTemplate
            // 
            this.mnuCreateTemplate.Name = "mnuCreateTemplate";
            this.mnuCreateTemplate.Size = new System.Drawing.Size(180, 22);
            this.mnuCreateTemplate.Text = "Create template";
            this.mnuCreateTemplate.Click += new System.EventHandler(this.mnuCreateTemplate_Click);
            // 
            // mnuCreateImageShape
            // 
            this.mnuCreateImageShape.Name = "mnuCreateImageShape";
            this.mnuCreateImageShape.Size = new System.Drawing.Size(180, 22);
            this.mnuCreateImageShape.Text = "Create image shape";
            this.mnuCreateImageShape.Click += new System.EventHandler(this.mnuCreateImageShape_Click);
            // 
            // mnuCreateSVG
            // 
            this.mnuCreateSVG.Name = "mnuCreateSVG";
            this.mnuCreateSVG.Size = new System.Drawing.Size(180, 22);
            this.mnuCreateSVG.Text = "Save to SVG";
            this.mnuCreateSVG.Click += new System.EventHandler(this.mnuCreateSVG_Click);
            // 
            // mnuSave2HTML
            // 
            this.mnuSave2HTML.Name = "mnuSave2HTML";
            this.mnuSave2HTML.Size = new System.Drawing.Size(180, 22);
            this.mnuSave2HTML.Text = "Save to HTML";
            this.mnuSave2HTML.Click += new System.EventHandler(this.mnuSave2HTML_Click);
            // 
            // mnuExamples
            // 
            this.mnuExamples.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLayoutExample,
            this.ToolStripMenuItem5,
            this.mnuBackground,
            this.mnuSnap,
            this.mnuFancyConnections,
            this.mnuZorderExample,
            this.mnuLayeringExample,
            this.mnuTrees,
            this.mnuShapeEvents,
            this.mnuClassInheritance});
            this.mnuExamples.Name = "mnuExamples";
            this.mnuExamples.Size = new System.Drawing.Size(45, 20);
            this.mnuExamples.Text = "示例";
            // 
            // mnuLayoutExample
            // 
            this.mnuLayoutExample.Name = "mnuLayoutExample";
            this.mnuLayoutExample.Size = new System.Drawing.Size(187, 22);
            this.mnuLayoutExample.Text = "Layout a graph";
            this.mnuLayoutExample.Click += new System.EventHandler(this.mnuLayoutExample_Click);
            // 
            // ToolStripMenuItem5
            // 
            this.ToolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNoNewConnections,
            this.mnuNoNewShapes,
            this.mnuNoLinkFrom,
            this.mnuItemCannotMove});
            this.ToolStripMenuItem5.Name = "ToolStripMenuItem5";
            this.ToolStripMenuItem5.Size = new System.Drawing.Size(187, 22);
            this.ToolStripMenuItem5.Text = "Constraints examples";
            // 
            // mnuNoNewConnections
            // 
            this.mnuNoNewConnections.Name = "mnuNoNewConnections";
            this.mnuNoNewConnections.Size = new System.Drawing.Size(226, 22);
            this.mnuNoNewConnections.Text = "No new connections";
            this.mnuNoNewConnections.Click += new System.EventHandler(this.mnuNoNewConnections_Click);
            // 
            // mnuNoNewShapes
            // 
            this.mnuNoNewShapes.Name = "mnuNoNewShapes";
            this.mnuNoNewShapes.Size = new System.Drawing.Size(226, 22);
            this.mnuNoNewShapes.Text = "No new shapes";
            this.mnuNoNewShapes.Click += new System.EventHandler(this.mnuNoNewShapes_Click);
            // 
            // mnuNoLinkFrom
            // 
            this.mnuNoLinkFrom.Name = "mnuNoLinkFrom";
            this.mnuNoLinkFrom.Size = new System.Drawing.Size(226, 22);
            this.mnuNoLinkFrom.Text = "No connections from \'Item1\'";
            this.mnuNoLinkFrom.Click += new System.EventHandler(this.mnuNoLinkFrom_Click);
            // 
            // mnuItemCannotMove
            // 
            this.mnuItemCannotMove.Name = "mnuItemCannotMove";
            this.mnuItemCannotMove.Size = new System.Drawing.Size(226, 22);
            this.mnuItemCannotMove.Text = "\'Item 1\' cannot move";
            this.mnuItemCannotMove.Click += new System.EventHandler(this.mnuItemCannotMove_Click);
            // 
            // mnuBackground
            // 
            this.mnuBackground.Name = "mnuBackground";
            this.mnuBackground.Size = new System.Drawing.Size(187, 22);
            this.mnuBackground.Text = "Background";
            this.mnuBackground.Click += new System.EventHandler(this.mnuBackground_Click);
            // 
            // mnuSnap
            // 
            this.mnuSnap.Name = "mnuSnap";
            this.mnuSnap.Size = new System.Drawing.Size(187, 22);
            this.mnuSnap.Text = "Grid and snap";
            this.mnuSnap.Click += new System.EventHandler(this.mnuSnap_Click);
            // 
            // mnuFancyConnections
            // 
            this.mnuFancyConnections.Name = "mnuFancyConnections";
            this.mnuFancyConnections.Size = new System.Drawing.Size(187, 22);
            this.mnuFancyConnections.Text = "Fancy connections";
            this.mnuFancyConnections.Click += new System.EventHandler(this.mnuFancyConnections_Click);
            // 
            // mnuZorderExample
            // 
            this.mnuZorderExample.Name = "mnuZorderExample";
            this.mnuZorderExample.Size = new System.Drawing.Size(187, 22);
            this.mnuZorderExample.Text = "Z-ordering";
            this.mnuZorderExample.Click += new System.EventHandler(this.mnuZorderExample_Click);
            // 
            // mnuLayeringExample
            // 
            this.mnuLayeringExample.Name = "mnuLayeringExample";
            this.mnuLayeringExample.Size = new System.Drawing.Size(187, 22);
            this.mnuLayeringExample.Text = "Layering";
            this.mnuLayeringExample.Click += new System.EventHandler(this.mnuLayeringExample_Click);
            // 
            // mnuTrees
            // 
            this.mnuTrees.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTreeAsCode,
            this.mnuRandomTree,
            this.mnuRandomNodeAddition});
            this.mnuTrees.Name = "mnuTrees";
            this.mnuTrees.Size = new System.Drawing.Size(187, 22);
            this.mnuTrees.Text = "Trees";
            // 
            // mnuTreeAsCode
            // 
            this.mnuTreeAsCode.Name = "mnuTreeAsCode";
            this.mnuTreeAsCode.Size = new System.Drawing.Size(171, 22);
            this.mnuTreeAsCode.Text = "As code";
            this.mnuTreeAsCode.Click += new System.EventHandler(this.mnuTreeAsCode_Click);
            // 
            // mnuRandomTree
            // 
            this.mnuRandomTree.Name = "mnuRandomTree";
            this.mnuRandomTree.Size = new System.Drawing.Size(171, 22);
            this.mnuRandomTree.Text = "Random";
            this.mnuRandomTree.Click += new System.EventHandler(this.mnuRandomTree_Click);
            // 
            // mnuRandomNodeAddition
            // 
            this.mnuRandomNodeAddition.Name = "mnuRandomNodeAddition";
            this.mnuRandomNodeAddition.Size = new System.Drawing.Size(171, 22);
            this.mnuRandomNodeAddition.Text = "Add random node";
            this.mnuRandomNodeAddition.Click += new System.EventHandler(this.mnuRandomNodeAddition_Click);
            // 
            // mnuShapeEvents
            // 
            this.mnuShapeEvents.Name = "mnuShapeEvents";
            this.mnuShapeEvents.Size = new System.Drawing.Size(187, 22);
            this.mnuShapeEvents.Text = "Shape events";
            this.mnuShapeEvents.Click += new System.EventHandler(this.mnuShapeEvents_Click);
            // 
            // mnuClassInheritance
            // 
            this.mnuClassInheritance.Name = "mnuClassInheritance";
            this.mnuClassInheritance.Size = new System.Drawing.Size(187, 22);
            this.mnuClassInheritance.Text = "Class inheritance";
            this.mnuClassInheritance.Click += new System.EventHandler(this.mnuClassInheritance_Click);
            // 
            // mnuZoom
            // 
            this.mnuZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoom200,
            this.mnuZoom100,
            this.mnuZoom50,
            this.mnuZoomIn,
            this.mnuZoomOut});
            this.mnuZoom.Name = "mnuZoom";
            this.mnuZoom.Size = new System.Drawing.Size(45, 20);
            this.mnuZoom.Text = "缩放";
            // 
            // mnuZoom200
            // 
            this.mnuZoom200.Name = "mnuZoom200";
            this.mnuZoom200.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom200.Text = "200%";
            this.mnuZoom200.Click += new System.EventHandler(this.mnuZoom200_Click);
            // 
            // mnuZoom100
            // 
            this.mnuZoom100.Name = "mnuZoom100";
            this.mnuZoom100.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom100.Text = "100%";
            this.mnuZoom100.Click += new System.EventHandler(this.mnuZoom100_Click);
            // 
            // mnuZoom50
            // 
            this.mnuZoom50.Name = "mnuZoom50";
            this.mnuZoom50.Size = new System.Drawing.Size(102, 22);
            this.mnuZoom50.Text = "50%";
            this.mnuZoom50.Click += new System.EventHandler(this.mnuZoom50_Click);
            // 
            // mnuZoomIn
            // 
            this.mnuZoomIn.Name = "mnuZoomIn";
            this.mnuZoomIn.Size = new System.Drawing.Size(102, 22);
            this.mnuZoomIn.Text = "放大";
            this.mnuZoomIn.Click += new System.EventHandler(this.mnuZoomIn_Click);
            // 
            // mnuZoomOut
            // 
            this.mnuZoomOut.Name = "mnuZoomOut";
            this.mnuZoomOut.Size = new System.Drawing.Size(102, 22);
            this.mnuZoomOut.Text = "缩小";
            this.mnuZoomOut.Click += new System.EventHandler(this.mnuZoomOut_Click);
            // 
            // mnuEditorMenu
            // 
            this.mnuEditorMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenTextFile,
            this.mnuSaveTextFile,
            this.mnuHighlighting,
            this.mnuEditorCut,
            this.mnuEditorCopy,
            this.mnuEditorPaste,
            this.mnuEditorDelete,
            this.mnuShowNMLInDiagram});
            this.mnuEditorMenu.Name = "mnuEditorMenu";
            this.mnuEditorMenu.Size = new System.Drawing.Size(58, 20);
            this.mnuEditorMenu.Text = "编辑器";
            // 
            // mnuOpenTextFile
            // 
            this.mnuOpenTextFile.Name = "mnuOpenTextFile";
            this.mnuOpenTextFile.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenTextFile.Text = "打开文件...";
            this.mnuOpenTextFile.Click += new System.EventHandler(this.mnuOpenTextFile_Click);
            // 
            // mnuSaveTextFile
            // 
            this.mnuSaveTextFile.Name = "mnuSaveTextFile";
            this.mnuSaveTextFile.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveTextFile.Text = "保存文件...";
            this.mnuSaveTextFile.Click += new System.EventHandler(this.mnuSaveTextFile_Click);
            // 
            // mnuHighlighting
            // 
            this.mnuHighlighting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHighlightXML,
            this.mnuHighlightHTML,
            this.mnuHighlighCsharp,
            this.mnuHighlightVBNet,
            this.mnuHighlightNone});
            this.mnuHighlighting.Name = "mnuHighlighting";
            this.mnuHighlighting.Size = new System.Drawing.Size(180, 22);
            this.mnuHighlighting.Text = "高亮显示";
            // 
            // mnuHighlightXML
            // 
            this.mnuHighlightXML.Name = "mnuHighlightXML";
            this.mnuHighlightXML.Size = new System.Drawing.Size(110, 22);
            this.mnuHighlightXML.Text = "XML";
            this.mnuHighlightXML.Click += new System.EventHandler(this.mnuHighlightXML_Click);
            // 
            // mnuHighlightHTML
            // 
            this.mnuHighlightHTML.Name = "mnuHighlightHTML";
            this.mnuHighlightHTML.Size = new System.Drawing.Size(110, 22);
            this.mnuHighlightHTML.Text = "HTML";
            this.mnuHighlightHTML.Click += new System.EventHandler(this.mnuHighlightHTML_Click);
            // 
            // mnuHighlighCsharp
            // 
            this.mnuHighlighCsharp.Name = "mnuHighlighCsharp";
            this.mnuHighlighCsharp.Size = new System.Drawing.Size(110, 22);
            this.mnuHighlighCsharp.Text = "C#";
            this.mnuHighlighCsharp.Click += new System.EventHandler(this.mnuHighlighCsharp_Click);
            // 
            // mnuHighlightVBNet
            // 
            this.mnuHighlightVBNet.Name = "mnuHighlightVBNet";
            this.mnuHighlightVBNet.Size = new System.Drawing.Size(110, 22);
            this.mnuHighlightVBNet.Text = "VB.Net";
            this.mnuHighlightVBNet.Click += new System.EventHandler(this.mnuHighlightVBNet_Click);
            // 
            // mnuHighlightNone
            // 
            this.mnuHighlightNone.Name = "mnuHighlightNone";
            this.mnuHighlightNone.Size = new System.Drawing.Size(110, 22);
            this.mnuHighlightNone.Text = "None";
            this.mnuHighlightNone.Click += new System.EventHandler(this.mnuHighlightNone_Click);
            // 
            // mnuEditorCut
            // 
            this.mnuEditorCut.Name = "mnuEditorCut";
            this.mnuEditorCut.Size = new System.Drawing.Size(180, 22);
            this.mnuEditorCut.Text = "剪切";
            this.mnuEditorCut.Click += new System.EventHandler(this.mnuEditorCut_Click);
            // 
            // mnuEditorCopy
            // 
            this.mnuEditorCopy.Name = "mnuEditorCopy";
            this.mnuEditorCopy.Size = new System.Drawing.Size(180, 22);
            this.mnuEditorCopy.Text = "复制";
            this.mnuEditorCopy.Click += new System.EventHandler(this.mnuEditorCopy_Click);
            // 
            // mnuEditorPaste
            // 
            this.mnuEditorPaste.Name = "mnuEditorPaste";
            this.mnuEditorPaste.Size = new System.Drawing.Size(180, 22);
            this.mnuEditorPaste.Text = "粘贴";
            this.mnuEditorPaste.Click += new System.EventHandler(this.mnuEditorPaste_Click);
            // 
            // mnuEditorDelete
            // 
            this.mnuEditorDelete.Name = "mnuEditorDelete";
            this.mnuEditorDelete.Size = new System.Drawing.Size(180, 22);
            this.mnuEditorDelete.Text = "删除";
            this.mnuEditorDelete.Click += new System.EventHandler(this.mnuEditorDelete_Click);
            // 
            // mnuShowNMLInDiagram
            // 
            this.mnuShowNMLInDiagram.Name = "mnuShowNMLInDiagram";
            this.mnuShowNMLInDiagram.Size = new System.Drawing.Size(180, 22);
            this.mnuShowNMLInDiagram.Text = "在图表中显示NML";
            this.mnuShowNMLInDiagram.Click += new System.EventHandler(this.mnuShowNMLInDiagram_Click);
            // 
            // mnuWindows
            // 
            this.mnuWindows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWindowDiagram,
            this.mnuWindowZoom,
            this.mnuWindowShapes,
            this.mnuWindowFavs,
            this.mnuBrowser,
            this.mnuWindowProperties,
            mnuWFNodeWindowProperties,
            this.mnuOutput,
            this.mnuEditor,
            this.mnuDiagramBrowser,
            this.流程设计元素库ToolStripMenuItem});
            this.mnuWindows.Name = "mnuWindows";
            this.mnuWindows.Size = new System.Drawing.Size(45, 20);
            this.mnuWindows.Text = "窗口";
            // 
            // mnuWindowDiagram
            // 
            this.mnuWindowDiagram.Name = "mnuWindowDiagram";
            this.mnuWindowDiagram.Size = new System.Drawing.Size(165, 22);
            this.mnuWindowDiagram.Text = "图表";
            this.mnuWindowDiagram.Click += new System.EventHandler(this.mnuWindowDiagram_Click);
            // 
            // mnuWindowZoom
            // 
            this.mnuWindowZoom.Name = "mnuWindowZoom";
            this.mnuWindowZoom.Size = new System.Drawing.Size(165, 22);
            this.mnuWindowZoom.Text = "缩放";
            this.mnuWindowZoom.Click += new System.EventHandler(this.mnuWindowZoom_Click);
            // 
            // mnuWindowShapes
            // 
            this.mnuWindowShapes.Name = "mnuWindowShapes";
            this.mnuWindowShapes.Size = new System.Drawing.Size(165, 22);
            this.mnuWindowShapes.Text = "形状";
            this.mnuWindowShapes.Click += new System.EventHandler(this.mnuWindowShapes_Click);
            // 
            // mnuWindowFavs
            // 
            this.mnuWindowFavs.Name = "mnuWindowFavs";
            this.mnuWindowFavs.Size = new System.Drawing.Size(165, 22);
            this.mnuWindowFavs.Text = "模板";
            this.mnuWindowFavs.Click += new System.EventHandler(this.mnuWindowFavs_Click);
            // 
            // mnuBrowser
            // 
            this.mnuBrowser.Name = "mnuBrowser";
            this.mnuBrowser.Size = new System.Drawing.Size(165, 22);
            this.mnuBrowser.Text = "浏览器";
            this.mnuBrowser.Click += new System.EventHandler(this.mnuBrowser_Click);
            // 
            // mnuWindowProperties
            // 
            this.mnuWindowProperties.Name = "mnuWindowProperties";
            this.mnuWindowProperties.Size = new System.Drawing.Size(165, 22);
            this.mnuWindowProperties.Text = "属性";
            this.mnuWindowProperties.Click += new System.EventHandler(this.mnuWindowProperties_Click);

            // 
            // mnuWFNodeWindowProperties
            // 
            this.mnuWFNodeWindowProperties.Name = "mnuWFNodeWindowProperties";
            this.mnuWFNodeWindowProperties.Size = new System.Drawing.Size(165, 22);
            this.mnuWFNodeWindowProperties.Text = "节点属性";
            this.mnuWFNodeWindowProperties.Click += new System.EventHandler(this.mnuWFNodeWindowProperties_Click);

            // 
            // mnuOutput
            // 
            this.mnuOutput.Name = "mnuOutput";
            this.mnuOutput.Size = new System.Drawing.Size(165, 22);
            this.mnuOutput.Text = "输出";
            this.mnuOutput.Click += new System.EventHandler(this.mnuOutput_Click);
            // 
            // mnuEditor
            // 
            this.mnuEditor.Name = "mnuEditor";
            this.mnuEditor.Size = new System.Drawing.Size(165, 22);
            this.mnuEditor.Text = "编辑器";
            this.mnuEditor.Click += new System.EventHandler(this.mnuEditor_Click);
            // 
            // mnuDiagramBrowser
            // 
            this.mnuDiagramBrowser.Name = "mnuDiagramBrowser";
            this.mnuDiagramBrowser.Size = new System.Drawing.Size(165, 22);
            this.mnuDiagramBrowser.Text = "图表浏览器";
            this.mnuDiagramBrowser.Click += new System.EventHandler(this.mnuDiagramBrowser_Click);
            // 
            // 流程设计元素库ToolStripMenuItem
            // 
            this.流程设计元素库ToolStripMenuItem.Name = "流程设计元素库ToolStripMenuItem";
            this.流程设计元素库ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.流程设计元素库ToolStripMenuItem.Text = "流程设计元素库";
            this.流程设计元素库ToolStripMenuItem.Click += new System.EventHandler(this.流程设计元素库ToolStripMenuItem_Click);
            // 
            // outputMenu
            // 
            this.outputMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuClearAll});
            // 
            // mnuClearAll
            // 
            this.mnuClearAll.Index = 0;
            this.mnuClearAll.Text = "Clear all";
            // 
            // TabImages
            // 
            this.TabImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabImages.ImageStream")));
            this.TabImages.TransparentColor = System.Drawing.Color.Transparent;
            this.TabImages.Images.SetKeyName(0, "");
            this.TabImages.Images.SetKeyName(1, "");
            this.TabImages.Images.SetKeyName(2, "");
            this.TabImages.Images.SetKeyName(3, "");
            // 
            // ButtonImages
            // 
            this.ButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ButtonImages.ImageStream")));
            this.ButtonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ButtonImages.Images.SetKeyName(0, "");
            this.ButtonImages.Images.SetKeyName(1, "");
            this.ButtonImages.Images.SetKeyName(2, "");
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(928, 474);
            this.dockPanel.TabIndex = 16;
            this.dockPanel.ActiveDocumentChanged += new System.EventHandler(this.dockPanel_ActiveDocumentChanged);
            this.dockPanel.ActiveContentChanged += new System.EventHandler(this.dockPanel_ActiveContentChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.dockPanel);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(928, 474);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(928, 498);
            this.toolStripContainer1.TabIndex = 17;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.WFMenuStrip);
            // 
            // WFMenuStrip
            // 
            this.WFMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.WFMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.WFMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApplication,
            this.mnuEdit,
            this.mnuDiagram,
            this.mnuExamples,
            this.mnuZoom,
            this.mnuEditorMenu,
            this.mnuWindows});
            this.WFMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.WFMenuStrip.Name = "WFMenuStrip";
            this.WFMenuStrip.Size = new System.Drawing.Size(928, 24);
            this.WFMenuStrip.TabIndex = 3;
            this.WFMenuStrip.Text = "menuStrip1";
            // 
            // 测试工作流ToolStripMenuItem
            // 
            this.测试工作流ToolStripMenuItem.Name = "测试工作流ToolStripMenuItem";
            this.测试工作流ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.测试工作流ToolStripMenuItem.Text = "测试工作流";
            this.测试工作流ToolStripMenuItem.Click += new System.EventHandler(this.测试工作流ToolStripMenuItem_Click);
            // 
            // WFMainForm
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.sb);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(840, 520);
            this.Name = "WFMainForm";
            this.Size = new System.Drawing.Size(928, 520);
            this.Load += new System.EventHandler(this.WFMainForm_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.MainForm_Layout);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.WFMenuStrip.ResumeLayout(false);
            this.WFMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        private void mnuWFNodeWindowProperties_Click(object sender, EventArgs e)
        {
            mediator.OpenWFNodePropertiesTab();
        }
        #endregion

        #region Saving/serialization related
        /// <summary>
        /// Opens an existing nml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpenGraphML_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "NML files (*.nml)|*.nml";
            DialogResult result = fileChooser.ShowDialog();
            string filename;
            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    mediator.GraphControl.NewDiagram(true);
                    mediator.GraphControl.OpenNML(filename);
                }
                catch (Exception exc)
                {
                    mediator.Output(exc.Message);
                }
            }
        }

        /// <summary>
        /// Saves the diagram to NML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSave2NML_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.CheckFileExists = false;

            fileChooser.Filter = "NML files (*.nml)|*.nml";
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
                mediator.GraphControl.SaveNMLAs(filename);
                MessageBox.Show("The graph was saved to '" + filename + "'", "NML saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }

        /// <summary>
        /// Saves the diagram to a binary format using .Net's BinarySerializer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveDiagram_Click(object sender, System.EventArgs e)
        {
            SaveDiagramBinary();
        }

        private void SaveDiagramBinary()
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.CheckFileExists = false;
            fileChooser.Filter = "Netron Graphs (*.netron)|*.netron";
            fileChooser.InitialDirectory = "\\c:";
            fileChooser.RestoreDirectory = true;
            fileChooser.Title = "Save diagram to binary file";
            DialogResult result = fileChooser.ShowDialog();
            string filename;

            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                if (mediator.GraphControl.SaveAs(filename))
                    MessageBox.Show("The diagram was saved in '" + filename + "'", "Save info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("The diagram was not saved.", "Save info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        /// <summary>
        /// Opens a binary serialized diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpenDiagram_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "Netron diagram (*.netron)|*.netron";
            DialogResult result = fileChooser.ShowDialog();
            string filename;
            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                mediator.GraphControl.NewDiagram(true);
                mediator.GraphControl.Open(filename);
            }

        }

        /// <summary>
        /// Saves a screenshot of the canvas to your c:\ directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveImage_Click(object sender, System.EventArgs e)
        {
            /* // Use the following code if you want to change the directory;
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			
			fileChooser.Filter = "JPG files (*.jpg)|*.jpg";
			fileChooser.InitialDirectory = "\\c:";
			fileChooser.RestoreDirectory = true;
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			*/
            {
                string filename = "c:\\NetronDiagramCanvas.jpg";
                mediator.GraphControl.SaveImage(filename, true);
                MessageBox.Show(@"The diagram image was saved in '" + filename + "'", "Save info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }


        #endregion

        #region Printing
        /// <summary>
        /// Shows the print-preview dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPrintPreview_Click(object sender, System.EventArgs e)
        {
            PrintDocument p = new PrintDocument();
            p.PrintPage += new PrintPageEventHandler(mediator.GraphControl.PrintCanvas);

            PrintPreviewDialog prev = new PrintPreviewDialog();
            prev.Document = p;
            prev.ShowDialog(this);
            return;
            /* this is the print directly
			PrintDialog d = new PrintDialog();
			d.Document = p ;
			if (d.ShowDialog() == DialogResult.OK)
			{
				p.Print();
			}
			*/
        }

        /// <summary>
        /// Allows multi-page printing if the diagram spans multiple pages.
        /// Thanks to Fabio for this example.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuMultiPrint_Click(object sender, System.EventArgs e)
        {

            PrinterSettings pset = null;// new PrinterSettings();


            NetronPrinter p = new NetronPrinter(pset, mediator.GraphControl);
            //p.PrintPage += new PrintPageEventHandler(mediator.GraphControl.PrintCanvas);

            PrintPreviewDialog prev = new PrintPreviewDialog();
            prev.UseAntiAlias = true;
            prev.Document = p;
            prev.ShowDialog(this);
            return;
        }

        #endregion

        #region Zoom
        /// <summary>
        /// Magnifies the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuZoom200_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Zoom = 2F;
        }

        /// <summary>
        /// Magnifies the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuZoom100_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Zoom = 1F;
        }

        /// <summary>
        /// Shrinks the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuZoom50_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Zoom = 0.5F;
        }


        #endregion

        #region Layout
        /// <summary>
        /// Starts the layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuStartLayout_Click(object sender, System.EventArgs e)
        {
            if (mediator.GraphControl.Shapes.Count < 1)
            {
                MessageBox.Show("Add first some shapes to the canvas before running the layout algorithm. \n You can use either the context menu or drag-drop from the shapes library.", "No shapes on the canvas", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (mediator.GraphControl.Connections.Count < 1)
            {
                MessageBox.Show(" Shapes are layed out if they are connected together. \n Drag connections between shapes by first clicking on a connector and dragging the line to another connector..", "No connections on the canvas", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            mediator.GraphControl.StartLayout();
        }

        /// <summary>
        /// Stops the layout process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuStopLayout_Click(object sender, System.EventArgs e)
        {
            if (mediator.GraphControl.Shapes.Count < 1)
                MessageBox.Show("Add first some shapes to the canvas before running the layout algorithm. \n You can use either the context menu or drag-drop from the shapes library.", "No shapes on the canvas", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            mediator.GraphControl.StopLayout();
        }

        /// <summary>
        /// Changes the algorithm to the spring-embedder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSpringEmbedder_Click(object sender, System.EventArgs e)
        {
            mnuTreeLayout.Checked = false;
            mnuSpringEmbedder.Checked = true;
            mnuRandomizerLayout.Checked = false;
            mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.SpringEmbedder;
        }

        /// <summary>
        /// Changes the algorithm to the 'tree' type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTreeLayout_Click(object sender, System.EventArgs e)
        {
            mnuTreeLayout.Checked = true;
            mnuSpringEmbedder.Checked = false;
            mnuRandomizerLayout.Checked = false;
            mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.Tree;
        }

        /// <summary>
        /// Changes the layout algorithm to the 'random' type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRandomizerLayout_Click(object sender, System.EventArgs e)
        {
            mnuTreeLayout.Checked = false;
            mnuSpringEmbedder.Checked = false;
            mnuRandomizerLayout.Checked = true;
            mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.Randomizer;
        }


        #endregion

        #region Help links

        private void mnuAbout_Click(object sender, System.EventArgs e)
        {
            SplashForm frm = new SplashForm(false);
            frm.ShowDialog(this);

        }

        private void mnuCredits_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("http://netron.sourceforge.net/ewiki/netron.php?id=CreditsAndAknowledgements");

        }

        private void mnuGenericHelp_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("http://netron.sourceforge.net/wp/");
        }

        #endregion

        #region Favorites
        private void Favorites_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {

            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // Show the standard Move icon.
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                // Show the standard Copy icon.
                e.Effect = DragDropEffects.Copy;
            }
        }



        #endregion

        #region Outputter




        #endregion

        #region Layer interface display
        private void mnuLayers_Click(object sender, System.EventArgs e)
        {
            using (LayersDialog frm = new LayersDialog(mediator.GraphControl))
            {
                //GraphLayerCollection layers = mediator.GraphControl.Layers;
                frm.Manager.LoadLayers(mediator.GraphControl);
                DialogResult res = frm.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    frm.Manager.UpdateLayerData();

                }
                else
                    return;
            }
        }
        #endregion

        #region Examples

        #region Class structures
        private void mnuBasicClases_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.GraphLibClasses);
        }
        #endregion

        #region Interfaces
        private void mnuUMLInterface_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.GraphLibInterfaces);
        }


        #endregion

        #region Custom bagrounds
        /// <summary>
        /// Sets a gradient background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuBackground_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This example positions shapes at random positions and random z-depth and starts the spring-embedder algorithm. You can stop the layout by pressing CTRL-SHIFT-L and restart it again with CTRL-L. The background is a bitmap and only shows the possibility to use flat colors, gradients or images as a background.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.Background);

        }

        #endregion

        #region Grid & snap
        /// <summary>
        /// Shows how to set the snapping on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSnap_Click(object sender, System.EventArgs e)
        {

            MessageBox.Show("This example shows how you can constraint shape positions with the grid. The grid, grid-size and snap can be set in the canvas properties (right-click the canvas and select 'properties'). You can also change the location of shapes via the CTRL-arrow keys.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.Snap);

        }

        #endregion

        #region Utilities to generate the examples

        /// <summary>
        /// Sets the algorithm and changes the interface accordingly
        /// </summary>
        /// <param name="algorithm"></param>
        public void SetLayoutAlgorithmUIState(GraphLayoutAlgorithms algorithm)
        {

            switch (algorithm)
            {
                case GraphLayoutAlgorithms.Randomizer:
                    this.mnuRandomizerLayout.Checked = true;
                    this.mnuSpringEmbedder.Checked = false;
                    this.mnuTreeLayout.Checked = false;
                    break;
                case GraphLayoutAlgorithms.SpringEmbedder:
                    this.mnuRandomizerLayout.Checked = false;
                    this.mnuSpringEmbedder.Checked = true;
                    this.mnuTreeLayout.Checked = false;
                    break;
                case GraphLayoutAlgorithms.Tree:
                    this.mnuRandomizerLayout.Checked = false;
                    this.mnuSpringEmbedder.Checked = false;
                    this.mnuTreeLayout.Checked = true;
                    break;

            }
        }



        #endregion

        #region Specific item cannot move
        private void mnuItemCannotMove_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.ItemCannotMove);
        }

        #endregion

        #region No new connection
        private void mnuNoNewConnections_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.NoNewConnections);
        }
        #endregion

        private void mnuAddSomeShapes_Click(object sender, System.EventArgs e)
        {
            //mediator.LoadSample(Samples.
        }

        #region Class inheritance

        private void mnuClassInheritance_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("See the code of this example to see how you can access custom shapes (i.e. shapes not compiled in the graph library) via code.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.ClassInheritance);
        }
        #endregion


        #region Fancy connections

        private void mnuFancyConnections_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This example shows a sample of custom connections defined in the Entitology library. Note that the location of the shape is random and can sometimes be an unhappy one, simply restart this example again in that case.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.FancyConnections);
        }

        #endregion

        #region Z-order

        private void mnuZorderExample_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This example shows how the z-depth can help you achieve a 3D look in diagrams. The alpha color of the shapes and connections is in function of the z-order. You can stop/Start the layout at any time via CTRL-SHIFT-L and CTRL-L respectively.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.ZOrder);
        }



        #endregion

        #region Layering
        private void mnuLayeringExample_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.Layering);
        }
        #endregion

        #region Spring-embedder layout 
        private void mnuLayoutExample_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This example positions shapes at random positions and starts the spring-embedder algorithm. You can stop the layout by pressing CTRL-SHIFT-L and restart it again with CTRL-L.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.Layout);
        }
        #endregion

        #region With controls
        private void mnuWithControls_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.Controls);
        }

        #endregion

        #region No linking from specific item
        private void mnuNoLinkFrom_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.NoLinking);
        }
        #endregion

        #region Shape events
        /// <summary>
        /// Shows how you can attach mouse-event handler to specific shapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShapeEvents_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.ShapeEvents);



        }


        #endregion

        #region Trees
        /// <summary>
        /// Creates a random tree graph and applies the tree layout thereafter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRandomTree_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Restart this example to get a different tree. Use CTRL-SHIFT-A in this example to add a random node to the tree.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.RandomTree);
        }

        /// <summary>
        /// Add a single random node to the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRandomNodeAddition_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Focus();
            mediator.AddRandomNodes(1);
            //SetLayoutAlgorithmUIState(GraphLayoutAlgorithms.Tree);

            mediator.GraphControl.StartLayout();


        }



        private void mnuTreeAsCode_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Use CTRL-SHIFT-A in this example to add a random node to the tree.", "API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mediator.LoadSample(Samples.TreeAsCode);
        }
        #endregion

        #region No new shapes
        /// <summary>
        /// Disallows the addition of new shapes on the control level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNoNewShapes_Click(object sender, System.EventArgs e)
        {
            mediator.LoadSample(Samples.NoNewShapes);

        }

        #endregion

        #endregion

        #region Diverse elements

        /// <summary>
        /// Clears the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNewDiagram_Click(object sender, System.EventArgs e)
        {
            CreateNewDiagram();

        }
        /// <summary>
        /// Creates a new diagram
        /// </summary>
        internal void CreateNewDiagram()
        {
            //AskForSaving();
            mediator.OpenGraphTab();
            mediator.GraphControl.NewDiagram(true);
        }
        /// <summary>
        /// Asks the user if he/she wants to save the current diagram.
        /// </summary>
        internal void AskForSaving()
        {
            if (mediator.GraphControl.Shapes.Count > 0)
            {
                DialogResult res = MessageBox.Show("是否要在清除画布之前保存图表？", "新建图表", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    SaveDiagramBinary();
                }
            }
        }

        /// <summary>
        /// Contextmenu switch of the graphcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuSwitch_Click(object sender, System.EventArgs e)
        {
            mnuContextMenu.Checked = !mnuContextMenu.Checked;
            mediator.GraphControl.EnableContextMenu = !mediator.GraphControl.EnableContextMenu;
        }



        /// <summary>
        /// Opens the graph-info dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGraphProperties_Click(object sender, System.EventArgs e)
        {
            GraphPropertiesDialog props = new GraphPropertiesDialog(mediator.GraphControl.Abstract.GraphInformation);
            DialogResult res = props.ShowDialog();

            if (res == DialogResult.OK)
            {
                mediator.GraphControl.Abstract.GraphInformation = props.GraphInformation;
            }


        }

        /// <summary>
        /// Displays a spanning tree via Prim's algorithm.
        /// This show how to use the graph-analysis sub-library which was formerly delivered in the 
        /// NetronDataStructures library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSpanningTree_Click(object sender, System.EventArgs e)
        {
            Netron.GraphLib.Analysis.GraphAnalyzer analyzer = new Netron.GraphLib.Analysis.GraphAnalyzer(mediator.GraphControl.Abstract, true);
            mediator.Output(analyzer.MatrixForm());
            mediator.Output("A spanning tree starting from the 0-th node:");
            //GraphLib.Analysis.IGraph g = GraphLib.Analysis.Algorithms.KruskalsAlgorithm(analyzer);
            Netron.GraphLib.Analysis.IGraph g = Netron.GraphLib.Analysis.Algorithms.PrimsAlgorithm(analyzer, 0);
            int nodeCount = analyzer.Count;
            IEnumerator numer = g.Edges.GetEnumerator();

            while (numer.MoveNext())
            {
                try
                {
                    Netron.GraphLib.Analysis.IEdge edge = numer.Current as Netron.GraphLib.Analysis.IEdge;//g.GetEdge(v,w);
                    analyzer.GetShape(edge.V0.Number).ShapeColor = Color.Orange;
                    analyzer.GetShape(edge.V1.Number).ShapeColor = Color.Orange;
                    analyzer.GetConnection(edge.V0.Number, edge.V1.Number).LineColor = Color.Orange;
                }
                catch
                { continue; }
            }
            #region alternatively
            //			for(int v=0; v<nodeCount; v++)
            //			{
            //				
            //				for(int w=0; w<nodeCount; w++)
            //				{
            //					try
            //					{
            //						GraphLib.Analysis.IEdge edge = g.GetEdge(v,w);
            //						analyzer.GetShape(v).ShapeColor = Color.Orange;
            //						analyzer.GetShape(w).ShapeColor = Color.Orange;
            //						analyzer.GetConnection(v,w).LineColor = Color.Orange;
            //					}
            //					catch(Exception)
            //					{							
            //						continue;
            //					}
            //				}				
            //			}
            #endregion
        }


        /// <summary>
        /// Shows a simple splash-screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Trace.WriteLine(Environment.NewLine);
            Trace.WriteLine(Environment.NewLine);
            Trace.WriteLine("Cobalt v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Trace.WriteLine(DateTime.Now.ToLongTimeString() + ", loading the application.");
            Trace.WriteLine(Environment.NewLine);

            SetCaption("");
#if DEBUG
            try
            {
                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                if (File.Exists(configFile))
                    dockPanel.LoadFromXml(configFile, deserializeDockContent);
            }
            catch
            {
                Trace.WriteLine("The deserialization of the docking didn't work, probably a different version.");
            }
#endif

        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            //Stop any layout-thread(s)
            mediator.GraphControl.StopLayout();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #endregion

        ///// <summary>
        ///// Serializes the docking state before exiting the application.
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //	base.OnClosing (e);
        //	string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
        //	dockPanel.SaveAsXml(configFile);
        //}

        private bool editflag;
        /// <summary>
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }

        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }

        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }



        }

        public void SetCaption(string text)
        {
            if (text.Length == 0)
                this.Text = CaptionPrefix;
            else
                this.Text = CaptionPrefix + ": " + text;
        }


        /// <summary>
        /// Shows the diagram tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuWindowDiagram_Click(object sender, System.EventArgs e)
        {
            mediator.OpenGraphTab();
        }


        /// <summary>
        /// Shows the properties tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuWindowProperties_Click(object sender, System.EventArgs e)
        {
            mediator.OpenPropsTab();
        }

        /// <summary>
        /// Shows the output tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOutput_Click(object sender, System.EventArgs e)
        {
            mediator.OpenOuputTab();
        }

        /// <summary>
        /// Shows the zoom tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuWindowZoom_Click(object sender, System.EventArgs e)
        {
            mediator.OpenZoomTab();
        }

        /// <summary>
        /// Shows the shape-viewer tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuWindowShapes_Click(object sender, System.EventArgs e)
        {
            mediator.OpenShapesTab();
        }

        /// <summary>
        /// Shows the shape favorites tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuWindowFavs_Click(object sender, System.EventArgs e)
        {
            mediator.OpenFavsTab();
        }


        /// <summary>
        /// Shows the browser tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuBrowser_Click(object sender, System.EventArgs e)
        {
            mediator.OpenBrowserTab();
        }

        /// <summary>
        /// Shows the help content tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHelpContent_Click(object sender, System.EventArgs e)
        {
            //you can show the help inside the application;
            //mediator.OpenChmTocTab();
            Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\NetronGraphLibrary.chm");
        }

        /// <summary>
        /// Part of the deserialization of the docking environment
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        private DockContent GetContentFromPersistString(string persistString)
        {
            if (!mLoaded)
                RaiseLoading("加载标签页 '" + persistString + "'...");
            if (persistString == typeof(BrowserTab).ToString())
                return mediator.BrowserTab;
            else if (persistString == typeof(ChmTocTab).ToString())
                return mediator.ChmTocTab;
            else if (persistString == typeof(FavTab).ToString())
                return mediator.FavTab;
            else if (persistString == typeof(OutputExTab).ToString())
                return mediator.OutputTab;
            else if (persistString == typeof(PropertiesTab).ToString())
                return mediator.PropertiesTab;
            else if (persistString == typeof(WFNodePropertiesTab).ToString())
                return mediator.WFNodePropertiesTab;
            else if (persistString == typeof(ShapeViewerTab).ToString())
                return mediator.ShapeViewerTab;
            else if (persistString == typeof(FlowNodeViewerTab).ToString())
                return mediator.FlowNodeViewerTab;
            else if (persistString == typeof(ZoomTab).ToString())
                return mediator.ZoomTab;
            else if (persistString == typeof(GraphTab).ToString())
                return mediator.GraphTab;
            //			else if (persistString == typeof(BugTab).ToString())
            //				return mediator.BugTab;
            else if (persistString == typeof(CodeTab).ToString())
                return mediator.CodeTab;
            else if (persistString == typeof(DiagramBrowserTab).ToString())
                return mediator.DiagramBrowserTab;
            else
            {

                return null;
            }
        }

        /// <summary>
        /// Show the bug reporter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //		private void mnuWindowBugReporter_Click(object sender, System.EventArgs e)
        //		{
        //			mediator.OpenBugTab();
        //			mediator.BugTab.EnableEditing = true;
        //		}

        /// <summary>
        /// Zoom in the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuZoomIn_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Zoom += 0.2f;
        }

        /// <summary>
        /// Zoom out the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuZoomOut_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Zoom -= 0.2f;
        }



        private void RTFBox_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void MainForm_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            if (m_bLayoutCalled == false)
            {
                m_bLayoutCalled = true;
                m_dt = DateTime.Now;
                //if( SplashScreen.SplashForm != null )
                //	SplashScreen.SplashForm.Owner = this;
                //this.Activate();
                System.Threading.Thread.Sleep(100);
                SplashScreen.CloseForm();
                //timer1.Start();
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            TimeSpan ts = DateTime.Now.Subtract(m_dt);
            if (ts.TotalSeconds > 10)
                Exit(this);
        }
        /// <summary>
        /// Raise the loading-event
        /// </summary>
        /// <param name="moduleName"></param>
        internal void RaiseLoading(string moduleName)
        {
            if (Loading != null)
                Loading(moduleName);
        }
        /// <summary>
        /// Used together with the multithreaded splashscreen to give feedback of the 
        /// loaded modules.
        /// </summary>
        public void PreLoad()
        {
            if (mLoaded)
            {
                //	just return. this code can't execute twice!
                return;
            }
            RaiseLoading("Docking deserialization...");
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, deserializeDockContent);

            //new TabPages.TestTab().Show(dockPanel);

            //			mediator.OpenPropsTab();
            //			mediator.OpenGraphTab();
            //RaiseLoading("Loading preliminary documentation...");
            //mediator.LoadCHM("NetronGraphLib2.2beta.chm");
            /*
			SplashScreen.SetStatus("Loading module 1");
			System.Threading.Thread.Sleep(500);
			SplashScreen.SetStatus("Loading module 2");
			System.Threading.Thread.Sleep(300);
			SplashScreen.SetStatus("Loading module 3");
			System.Threading.Thread.Sleep(900);
			SplashScreen.SetStatus("Loading module 4");
			System.Threading.Thread.Sleep(100);
			
*/
            //	flag that we have loaded all we need.
            mLoaded = true;
        }

        #endregion

        /// <summary>
        /// Opens the text editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEditor_Click(object sender, System.EventArgs e)
        {
            mediator.OpenCodeTab();
        }

        /// <summary>
        /// Changes the main menu when the active content is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dockPanel_ActiveContentChanged(object sender, System.EventArgs e)
        {
            ContentFocusChanged();
        }

        /// <summary>
        ///  Changes the main menu when the active document is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dockPanel_ActiveDocumentChanged(object sender, System.EventArgs e)
        {
            ContentFocusChanged();
        }

        /// <summary>
        /// Changes some ambient things like the menu in function of the current content/tab
        /// </summary>
        private void ContentFocusChanged()
        {
            if (mnuShowMode.Checked) return;

            DockContent dockContent = this.dockPanel.ActiveContent;
            if (dockContent == null) return;

            if (typeof(CodeTab).IsInstanceOfType(dockContent))
                SetContentMenu(TabTypes.Code, true);
            else
                SetContentMenu(TabTypes.Code, false);

            if (typeof(GraphTab).IsInstanceOfType(dockContent) || typeof(ZoomTab).IsInstanceOfType(dockContent) || typeof(ShapeViewerTab).IsInstanceOfType(dockContent) || typeof(FavTab).IsInstanceOfType(dockContent))
                SetContentMenu(TabTypes.NetronDiagram, true);
            else
                SetContentMenu(TabTypes.NetronDiagram, false);

        }

        /// <summary>
        /// Changes the main menu in function of the given tab content
        /// </summary>
        /// <param name="tabType"></param>
        /// <param name="value"></param>
        private void SetContentMenu(TabTypes tabType, bool value)
        {
            switch (tabType)
            {
                case TabTypes.NetronDiagram:
                    mnuExamples.Visible = value;
                    mnuZoom.Visible = value;
                    mnuDiagram.Visible = value;
                    if (value) mediator.TestTextForNML();
                    mnuEdit.Visible = value;
                    break;
                case TabTypes.Code:
                    mnuEditorMenu.Visible = value;
                    break;
                case TabTypes.Unknown:
                    mnuExamples.Visible = value;
                    mnuZoom.Visible = value;
                    mnuDiagram.Visible = value;
                    mnuEditorMenu.Visible = value;
                    break;
            }
        }



        /// <summary>
        /// Opens a text file in the editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpenTextFile_Click(object sender, System.EventArgs e)
        {
            OpenTextFile();
        }



        private void OpenTextFile()
        {
            mediator.OpenTextFile();
        }

        /// <summary>
        /// Saves the content of the editor to a file
        /// </summary>
        private void SaveTextFile()
        {
            mediator.SaveTextFile();
        }
        /// <summary>
        /// Transfer the presumed NML content to the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShowNMLInDiagram_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.NewDiagram(true);
            mediator.GraphControl.OpenNMLFragment(mediator.Editor.Text);
        }

        /// <summary>
        /// The presumed XML in the text-editor will be re-formatted and indented
        /// to something more readable. 
        /// However, for some reason this does not work...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatXML()
        {

            try
            {



                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.ASCII);
                writer.Formatting = System.Xml.Formatting.Indented;
                //writer.WriteRaw(mediator.Editor.Text);

                writer.WriteString(mediator.Editor.Text);

                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

                //StringReader reader = new StringReader();
                int count = 0;
                stream.Seek(0, SeekOrigin.Begin);

                byte[] byteArray = new byte[stream.Length];

                while (count < stream.Length)
                {
                    byteArray[count++] = Convert.ToByte(stream.ReadByte());
                }

                // Decode the byte array into a char array 
                // and write it to the console.
                char[] charArray = new char[asciiEncoding.GetCharCount(byteArray, 0, count)];
                asciiEncoding.GetDecoder().GetChars(byteArray, 0, count, charArray, 0);

                string s = new string(charArray);
                mediator.Editor.Text = s;
                stream.Close();
                writer.Close();
            }
            catch (Exception exc)
            {
                mediator.Output(exc.Message);
            }
        }

        /// <summary>
        /// Transfers the diagram'sNML to the editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNMLToEditor_Click(object sender, System.EventArgs e)
        {
            this.mediator.TransferNMLToEditor();
        }


        private void mnuSaveTextFile_Click(object sender, System.EventArgs e)
        {
            SaveTextFile();
        }

        #region Cut, copy, paste, delete
        private void mnuEditorCut_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.X | Keys.Control);
        }

        private void mnuEditorCopy_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.C | Keys.Control);
        }

        private void mnuEditorPaste_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.V | Keys.Control);
        }

        private void mnuEditorDelete_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.D | Keys.Control);
        }
        #endregion

        #region Highlighting
        private void mnuHighlightHTML_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.SetHighlighting("HTML");
        }


        private void mnuHighlightXML_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.SetHighlighting("XML");
        }

        private void mnuHighlighCsharp_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.SetHighlighting("C#");
        }

        private void mnuHighlightVBNet_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.SetHighlighting("VBNET");
        }

        private void mnuHighlightNone_Click(object sender, System.EventArgs e)
        {
            mediator.Editor.SetDefaultHighlighting();
        }

        #endregion

        #region Colored visit
        /// <summary>
        /// Demonstrates the use of the analysis service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuColoredVisit_Click(object sender, System.EventArgs e)
        {
            Netron.GraphLib.Analysis.GraphAnalyzer analyzer = new Netron.GraphLib.Analysis.GraphAnalyzer(this.mediator.GraphControl.Abstract, true);
            MyVisitor visitor = new MyVisitor(analyzer);
            analyzer.DepthFirstTraversal(visitor, 0);
        }

        /// <summary>
        /// Sample IVisitor implementation
        /// </summary>
        public class MyVisitor : Netron.GraphLib.Analysis.AbstractPrePostVisitor
        {

            private Netron.GraphLib.Analysis.GraphAnalyzer analyzer;

            private Random rnd = new Random();

            public MyVisitor(Netron.GraphLib.Analysis.GraphAnalyzer analyzer)
            {
                this.analyzer = analyzer;
            }

            public override void Visit(object obj)
            {
                Netron.GraphLib.Analysis.IVertex vertex = obj as Netron.GraphLib.Analysis.IVertex;
                if (vertex != null)
                {
                    Shape shape = analyzer.GetShape(vertex.Number);
                    shape.ShapeColor = Color.FromArgb(rnd.Next(10, 200), rnd.Next(10, 200), rnd.Next(10, 200));
                }

            }


        }
        #endregion



        private void mnuDiagramBrowser_Click(object sender, System.EventArgs e)
        {
            mediator.OpenDiagramBrowserTab();
        }

        /// <summary>
        /// Creates a template from the selected shapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCreateTemplate_Click(object sender, System.EventArgs e)
        {
            if (mediator.GraphControl.SelectedShapes.Count < 1)
            {
                MessageBox.Show("Make first a selection in the diagram; a template is a subset of an existing diagram", "No shapes selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (TemplatePropertiesDialog dialog = new TemplatePropertiesDialog())
            {
                DialogResult res = dialog.ShowDialog(this);
                if (res == DialogResult.OK)
                {

                    EntityBundle bundle = mediator.GraphControl.BundleSelection();
                    bundle.Name = dialog.TemplateName.Text;
                    bundle.Description = dialog.Description.Text;
                    mediator.FavTab.AddFavorite(bundle, dialog.TemplateName.Text.Trim(), dialog.Description.Text.Trim());
                    mediator.Output("New template '" + dialog.TemplateName.Text.Trim() + "' was added. Double-click the template name in the favorites to load it in the diagram.", OutputInfoLevels.Info);
                }
            }
        }

        /// <summary>
        /// Output SVG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCreateSVG_Click(object sender, System.EventArgs e)
        {
            mediator.Output("SVG export is still in an experimental stadium, see the SVGSerializer class for more.", OutputInfoLevels.Info);
            string tmpFile = Path.GetTempFileName();
            tmpFile = Path.ChangeExtension(tmpFile, ".svg");
            Netron.GraphLib.IO.SVG.SVGSerializer.SaveAs(tmpFile, mediator.GraphControl);
            try
            {
                Process.Start(tmpFile);
            }
            catch
            {
                //probably the SVG plugin is not installed
                MessageBox.Show(@"The Cobalt application tried to launch the application associeted to SVG file types but this resulted in an exception. Probably you haven't installed a SVG-viewer. If you're using IE, see Adobe's SVG-site http://www.adobe.com/svg/viewer/install/ for a browser plugin. For FireFox, Opera and other browser you'll need a stand-alone application; see the SVG site for more http://svg.org/.", "Error; no SVG application?", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuShowMode_Click(object sender, System.EventArgs e)
        {
            mnuShowMode.Checked = !mnuShowMode.Checked;
            ShowMode(mnuShowMode.Checked);
        }


        private void ShowMode(bool value)
        {
            mediator.HideAllTabs();

            //set the menu
            this.mnuApplication.Visible = true;
            this.mnuEditorMenu.Visible = !value;
            this.mnuWindows.Visible = !value;
            this.mnuDiagram.Visible = !value;
            this.mnuExamples.Visible = !value;
            this.mnuZoom.Visible = !value;


            mediator.OpenGraphTab();

            if (value)
            {
                //this.WindowState = FormWindowState.Maximized;
                // this.FormBorderStyle = FormBorderStyle.None;
                mediator.GraphTab.AllowRedocking = false;
            }
            else
            {
                //  this.FormBorderStyle = FormBorderStyle.Sizable;
                mediator.GraphTab.AllowRedocking = true;
            }


        }

        private void mnuSave2HTML_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.CheckFileExists = false;

            fileChooser.Filter = "HTML files (*.htm; *.html)|*.htm; *.html";
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
                Netron.GraphLib.IO.HTML.HTMLExporter exporter = new Netron.GraphLib.IO.HTML.HTMLExporter(mediator.GraphControl);
                exporter.SaveAs(filename);
            }
        }

        private void mnuCreateImageShape_Click(object sender, System.EventArgs e)
        {
            if (mediator.GraphControl.SelectedShapes.Count < 1)
            {
                MessageBox.Show("Make first a selection in the diagram; a new ImageShape will be created on the basis of the selected shapes and connections (i.e. a screenshot). You can save the newly created ImageShape thereafter as a template to re-use it in other diagrams.", "No shapes selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                EntityBundle bundle = mediator.GraphControl.GroupSelection();
                Shape shape = mediator.GraphControl.AddShape("47D016B9-990A-436c-ADE8-B861714EBE5A", new PointF(bundle.Rectangle.X, bundle.Rectangle.Y));
                if (shape == null)
                {
                    MessageBox.Show("This feature depends on the reflected Entitology shape library. The library or the ImageShape could not be found however and the creation of the shape failed. Please check the configuration of the application and the outputted exception message for more details regarding this error.", "Shape or library not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                PropertyInfo info = shape.GetType().GetProperty("Image");
                info.SetValue(shape, bundle.BundleImage, null);
                shape.Invalidate();
            }
            catch (Exception exc)
            {
                mediator.Output(exc.Message);
            }
        }

        private void mnuCopy_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Copy();
        }

        private void mnuPaste_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Paste();
        }

        private void mnuCut_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Cut();
        }

        private void mnuDelete_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.Delete();
        }

        private void mnuCopyAsImage_Click(object sender, System.EventArgs e)
        {
            if (mediator.GraphControl.SelectedShapes.Count < 1)
            {
                MessageBox.Show("Nothing selected: make a diagram selection first.", "No shapes selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mediator.GraphControl.CopyAsImage();


        }

        private void mnuSelectAll_Click(object sender, System.EventArgs e)
        {
            mediator.GraphControl.SelectAll(true);
        }

        private void mnuChangeLog_Click(object sender, System.EventArgs e)
        {
            Process.Start("ChangeLog.txt");
        }

        private void mnuSampleDiagrams_Click(object sender, System.EventArgs e)
        {
            Process.Start(@"..\..\..\HTML\default.htm");
        }

        private void mnuReadme_Click(object sender, System.EventArgs e)
        {
            Process.Start(@"..\..\..\HTML\Readme20050728.htm");
        }

        private void WFMainForm_Load(object sender, EventArgs e)
        {
            this.WFMenuStrip.Items.Add(this.mnuSaveDiagram);
            //默认的开设计器
            mediator.OpenGraphTab();
            mediator.GraphControl.OnToJsonAction += GraphControl_OnToJsonAction;
            mediator.GraphControl.OnTestWorkflowAction += GraphControl_OnTestWorkflow;
            mediator.GraphControl.OnSetCondition += GraphControl_OnSetCondition;
            mediator.GraphControl.OnSetNextStepByLine += GraphControl_OnSetNextStep;
            mediator.GraphControl.OnDeleteConnectionLine += GraphControl_OnDeleteConnectionLine;
            mediator.GraphControl.OnShapeRemoved += GraphControl_OnShapeRemoved;
            mediator.GraphControl.OnShapeAdded += GraphControl_OnShapeAdded;
            mediator.GraphControl.OnSetNodeProperties += GraphControl_OnSetNodeProperties;
            mediator.OpenFlowNodesTab();
        }

        private void GraphControl_OnShapeAdded(object sender, Shape shape)
        {
            this.mediator.AddStep(sender,shape);
        }

        private void GraphControl_OnShapeRemoved(object sender, Shape shape)
        {
            this.mediator.DeleteStep(sender, shape);
        }

        private void GraphControl_OnTestWorkflow(object sender, EventArgs e)
        {
            this.mediator.TestWorkFlowByJson(sender, e);
        }

        private void GraphControl_OnDeleteConnectionLine(object sender, ConnectionEventArgs e)
        {
            this.mediator.DeleteNextStepByLine(sender, e);
        }



        private void GraphControl_OnSetNextStep(object sender, ConnectionEventArgs e)
        {
            this.mediator.SetNextStepByLine(sender, e);
        }

        private void GraphControl_OnSetNodeProperties(object sender, object e)
        {
            this.mediator.SetNodeProperty(sender, e);
        }

        private void GraphControl_OnSetCondition(object sender, EventArgs e)
        {
            this.mediator.SetCondition(sender, e);
        }

        private void GraphControl_OnToJsonAction(object sender, EventArgs e)
        {
            this.mediator.TransferWorkFlowJsonToEditor();
        }

        private void 流程设计元素库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediator.OpenFlowNodesTab();
        }

        private void jsonTo编辑器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.TransferWorkFlowJsonToEditor();
        }

        private void 测试工作流ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.TestWorkFlowByJson(sender, e);
        }
    }
}

