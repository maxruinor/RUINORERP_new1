using System;
using System.Windows.Forms;
using Netron.Neon;
using RUINORERP.WF;
using RUINORERP.WF.UI;
namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 用来设置工作流节点的属性窗体
    /// </summary>
    public class WFNodePropertiesTab : DockContent, ICobaltTab
    {
        #region Fields
        private Mediator mediator;
        public Panel UCPanel;
        private string identifier;
        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WFNodePropertiesTab));
            this.UCPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // UCPanel
            // 
            this.UCPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UCPanel.Location = new System.Drawing.Point(0, 0);
            this.UCPanel.Name = "UCPanel";
            this.UCPanel.Size = new System.Drawing.Size(297, 567);
            this.UCPanel.TabIndex = 0;
            // 
            // WFNodePropertiesTab
            // 
            this.AccessibleDescription = "The property grid allows you to change properties of shapes and the canvas. Doubl" +
    "e-click a shape to edit its properties.";
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(297, 567);
            this.Controls.Add(this.UCPanel);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WFNodePropertiesTab";
            this.ShowHint = Netron.Neon.DockState.DockRight;
            this.Load += new System.EventHandler(this.WFNodePropertiesTab_Load);
            this.ResumeLayout(false);

        }


        #region Properties

        public TabTypes TabType
        {
            get { return TabTypes.WFNodePropertiesTab; }
        }

        public string TabIdentifier
        {
            get { return this.identifier; }
            set { identifier = value; }
        }




        #endregion

        #region Constructor
        public WFNodePropertiesTab(Mediator mediator)
        {
            InitializeComponent();
            this.mediator = mediator;
        }
        #endregion

        #region Methods

        #endregion

        /// <summary>
        /// The default constructor
        /// </summary>
        public object _ObjectPropertyValue = null;

        /// <summary>
        /// Gets or sets the object property value
        /// </summary>
        public object ObjectPropertyValue
        {
            get { return _ObjectPropertyValue; }
            set { _ObjectPropertyValue = value; }
        }

        private void WFNodePropertiesTab_Load(object sender, EventArgs e)
        {
    
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }



        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
