namespace RUINORERP.UI.Report
{
    partial class RptDesignForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptDesignForm));
            FastReport.Design.DesignerSettings designerSettings1 = new FastReport.Design.DesignerSettings();
            FastReport.Design.DesignerRestrictions designerRestrictions1 = new FastReport.Design.DesignerRestrictions();
            FastReport.Export.Email.EmailSettings emailSettings1 = new FastReport.Export.Email.EmailSettings();
            FastReport.PreviewSettings previewSettings1 = new FastReport.PreviewSettings();
            FastReport.ReportSettings reportSettings1 = new FastReport.ReportSettings();
            this.designerControl1 = new FastReport.Design.StandardDesigner.DesignerControl();
            this.environmentSettings1 = new FastReport.EnvironmentSettings();
            this.bindingSourceTemplateDesign = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.designerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTemplateDesign)).BeginInit();
            this.SuspendLayout();
            // 
            // designerControl1
            // 
            this.designerControl1.AskSave = true;
            this.designerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designerControl1.LayoutState = resources.GetString("designerControl1.LayoutState");
            this.designerControl1.Location = new System.Drawing.Point(0, 0);
            this.designerControl1.Name = "designerControl1";
            this.designerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.designerControl1.ShowMainMenu = false;
            this.designerControl1.Size = new System.Drawing.Size(957, 679);
            this.designerControl1.TabIndex = 0;
            this.designerControl1.UIStyle = FastReport.Utils.UIStyle.Office2007Black;
            this.designerControl1.UIStateChanged += new System.EventHandler(this.designerControl1_UIStateChanged);
            // 
            // environmentSettings1
            // 
            designerSettings1.ApplicationConnection = null;
            designerSettings1.DefaultFont = new System.Drawing.Font("宋体", 9F);
            designerSettings1.Icon = ((System.Drawing.Icon)(resources.GetObject("designerSettings1.Icon")));
            designerSettings1.Restrictions = designerRestrictions1;
            designerSettings1.Text = "";
            this.environmentSettings1.DesignerSettings = designerSettings1;
            emailSettings1.Address = "";
            emailSettings1.Host = "";
            emailSettings1.MessageTemplate = "";
            emailSettings1.Name = "";
            emailSettings1.Password = "";
            emailSettings1.UserName = "";
            this.environmentSettings1.EmailSettings = emailSettings1;
            previewSettings1.Icon = ((System.Drawing.Icon)(resources.GetObject("previewSettings1.Icon")));
            previewSettings1.SaveInitialDirectory = null;
            previewSettings1.Text = "";
            this.environmentSettings1.PreviewSettings = previewSettings1;
            reportSettings1.ImageLocationRoot = null;
            this.environmentSettings1.ReportSettings = reportSettings1;
            this.environmentSettings1.UIStyle = FastReport.Utils.UIStyle.Office2007Blue;
            this.environmentSettings1.DesignerLoaded += new System.EventHandler(this.environmentSettings1_DesignerLoaded_1);
            this.environmentSettings1.ReportLoaded += new FastReport.Design.ReportLoadedEventHandler(this.environmentSettings1_ReportLoaded);
            // 
            // RptDesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 679);
            this.Controls.Add(this.designerControl1);
            this.Name = "RptDesignForm";
            this.Text = "RptDesignForm";
            this.Load += new System.EventHandler(this.RptDesignForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.designerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTemplateDesign)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastReport.Design.StandardDesigner.DesignerControl designerControl1;
        private FastReport.EnvironmentSettings environmentSettings1;
        public System.Windows.Forms.BindingSource bindingSourceTemplateDesign;
    }
}