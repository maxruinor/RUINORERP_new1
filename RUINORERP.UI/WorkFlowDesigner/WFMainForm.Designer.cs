using Netron.Neon;
using System.Windows.Forms;
using System;

namespace RUINORERP.UI.WorkFlowDesigner
{
    public partial class WFMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.mnuWFNodeWindowProperties = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mnuProcessNavigation = new System.Windows.Forms.ToolStripMenuItem();
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
            this.测试工作流ToolStripMenuItem,
            this.mnuProcessNavigation});
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
            this.mnuWFNodeWindowProperties,
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
            // mnuProcessNavigation
            // 
            this.mnuProcessNavigation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewProcessNavigation,
            this.mnuOpenProcessNavigation,
            this.mnuSaveProcessNavigation,
            this.mnuProcessNavigationMode});
            this.mnuProcessNavigation.Name = "mnuProcessNavigation";
            this.mnuProcessNavigation.Size = new System.Drawing.Size(180, 22);
            this.mnuProcessNavigation.Text = "流程导航图";
            // 
            // mnuNewProcessNavigation
            // 
            this.mnuNewProcessNavigation.Name = "mnuNewProcessNavigation";
            this.mnuNewProcessNavigation.Size = new System.Drawing.Size(180, 22);
            this.mnuNewProcessNavigation.Text = "新建流程导航图";
            this.mnuNewProcessNavigation.Click += new System.EventHandler(this.mnuNewProcessNavigation_Click);
            // 
            // mnuOpenProcessNavigation
            // 
            this.mnuOpenProcessNavigation.Name = "mnuOpenProcessNavigation";
            this.mnuOpenProcessNavigation.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenProcessNavigation.Text = "打开流程导航图";
            this.mnuOpenProcessNavigation.Click += new System.EventHandler(this.mnuOpenProcessNavigation_Click);
            // 
            // mnuSaveProcessNavigation
            // 
            this.mnuSaveProcessNavigation.Name = "mnuSaveProcessNavigation";
            this.mnuSaveProcessNavigation.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveProcessNavigation.Text = "保存流程导航图";
            this.mnuSaveProcessNavigation.Click += new System.EventHandler(this.mnuSaveProcessNavigation_Click);
            // 
            // mnuProcessNavigationMode
            // 
            this.mnuProcessNavigationMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDesignMode,
            this.mnuPreviewMode});
            this.mnuProcessNavigationMode.Name = "mnuProcessNavigationMode";
            this.mnuProcessNavigationMode.Size = new System.Drawing.Size(180, 22);
            this.mnuProcessNavigationMode.Text = "模式切换";
            // 
            // mnuDesignMode
            // 
            this.mnuDesignMode.Name = "mnuDesignMode";
            this.mnuDesignMode.Size = new System.Drawing.Size(150, 22);
            this.mnuDesignMode.Text = "设计模式";
            this.mnuDesignMode.Click += new System.EventHandler(this.mnuDesignMode_Click);
            // 
            // mnuPreviewMode
            // 
            this.mnuPreviewMode.Name = "mnuPreviewMode";
            this.mnuPreviewMode.Size = new System.Drawing.Size(150, 22);
            this.mnuPreviewMode.Text = "预览模式";
            this.mnuPreviewMode.Click += new System.EventHandler(this.mnuPreviewMode_Click);
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
        private ToolStripMenuItem mnuProcessNavigation;
        private ToolStripMenuItem mnuNewProcessNavigation;
        private ToolStripMenuItem mnuOpenProcessNavigation;
        private ToolStripMenuItem mnuSaveProcessNavigation;
        private ToolStripMenuItem mnuProcessNavigationMode;
        private ToolStripMenuItem mnuDesignMode;
        private ToolStripMenuItem mnuPreviewMode;

        #endregion
    }
}