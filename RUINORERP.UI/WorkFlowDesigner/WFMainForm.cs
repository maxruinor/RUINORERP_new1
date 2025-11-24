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
    public partial class WFMainForm : UserControl
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


        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public WFMainForm()
        {
            //the root of the whole application connecting the different parts;
            mediator = new Mediator(this);
            
            //this gets the state of the docking as it was the last run
            deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //part of the default Visual Studio setup
            InitializeComponent();
            
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
            //确保mediator不为空
            if (mediator != null)
            {
                //默认的开设计器
                mediator.OpenGraphTab();
                
                //确保GraphControl不为空再绑定事件
                if (mediator.GraphControl != null)
                {
                    mediator.GraphControl.OnToJsonAction += GraphControl_OnToJsonAction;
                    mediator.GraphControl.OnTestWorkflowAction += GraphControl_OnTestWorkflow;
                    mediator.GraphControl.OnSetCondition += GraphControl_OnSetCondition;
                    mediator.GraphControl.OnSetNextStepByLine += GraphControl_OnSetNextStep;
                    mediator.GraphControl.OnDeleteConnectionLine += GraphControl_OnDeleteConnectionLine;
                    mediator.GraphControl.OnShapeRemoved += GraphControl_OnShapeRemoved;
                    mediator.GraphControl.OnShapeAdded += GraphControl_OnShapeAdded;
                    mediator.GraphControl.OnSetNodeProperties += GraphControl_OnSetNodeProperties;
                }
                
                mediator.OpenFlowNodesTab();
            }
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

        #region 流程导航图相关事件处理

        private void mnuNewProcessNavigation_Click(object sender, EventArgs e)
        {
            this.mediator.NewProcessNavigation();
        }

        private void mnuOpenProcessNavigation_Click(object sender, EventArgs e)
        {
            this.mediator.OpenProcessNavigation();
        }

        private void mnuSaveProcessNavigation_Click(object sender, EventArgs e)
        {
            this.mediator.SaveProcessNavigation();
        }

        private void mnuDesignMode_Click(object sender, EventArgs e)
        {
            this.mediator.SwitchProcessNavigationMode(ProcessNavigationMode.设计模式);
        }

        private void mnuPreviewMode_Click(object sender, EventArgs e)
        {
            this.mediator.SwitchProcessNavigationMode(ProcessNavigationMode.预览模式);
        }

        #endregion
    }
}

