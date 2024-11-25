namespace RUINORERP.UI.BaseForm
{
    partial class BaseBillEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseBillEdit));
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnSubmit = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnPrint = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripbtnReview = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnReverseReview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton结案 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton付款调整 = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnFunction = new System.Windows.Forms.ToolStripDropDownButton();
            this.复制性新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripbtnProperty = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnClose = new System.Windows.Forms.ToolStripButton();
            this.tslLocked = new System.Windows.Forms.ToolStripLabel();
            this.bwRemoting = new System.ComponentModel.BackgroundWorker();
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.bindingSourceSub = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).BeginInit();
            this.SuspendLayout();
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripbtnAdd,
            this.toolStripBtnCancel,
            this.toolStripbtnModify,
            this.toolStripbtnDelete,
            this.toolStripbtnQuery,
            this.toolStripButtonRefresh,
            this.toolStripButtonSave,
            this.toolStripbtnSubmit,
            this.toolStripbtnPrint,
            this.toolStripSeparator1,
            this.toolStripbtnReview,
            this.toolStripBtnReverseReview,
            this.toolStripButton结案,
            this.toolStripButton付款调整,
            this.toolStripbtnFunction,
            this.toolStripbtnProperty,
            this.toolStripbtnClose,
            this.tslLocked});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(869, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripbtnAdd
            // 
            this.toolStripbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnAdd.Image")));
            this.toolStripbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnAdd.Name = "toolStripbtnAdd";
            this.toolStripbtnAdd.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnAdd.Text = "新增";
            // 
            // toolStripBtnCancel
            // 
            this.toolStripBtnCancel.Image = global::RUINORERP.UI.Properties.Resources.cancel;
            this.toolStripBtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnCancel.Name = "toolStripBtnCancel";
            this.toolStripBtnCancel.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnCancel.Text = "取消";
            // 
            // toolStripbtnModify
            // 
            this.toolStripbtnModify.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnModify.Image")));
            this.toolStripbtnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnModify.Name = "toolStripbtnModify";
            this.toolStripbtnModify.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnModify.Text = "修改";
            // 
            // toolStripbtnDelete
            // 
            this.toolStripbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnDelete.Image")));
            this.toolStripbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnDelete.Name = "toolStripbtnDelete";
            this.toolStripbtnDelete.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnDelete.Text = "删除";
            // 
            // toolStripbtnQuery
            // 
            this.toolStripbtnQuery.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnQuery.Image")));
            this.toolStripbtnQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnQuery.Name = "toolStripbtnQuery";
            this.toolStripbtnQuery.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnQuery.Text = "查询";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonRefresh.Text = "刷新";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonSave.Text = "保存";
            // 
            // toolStripbtnSubmit
            // 
            this.toolStripbtnSubmit.Image = global::RUINORERP.UI.Properties.Resources.ok;
            this.toolStripbtnSubmit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnSubmit.Name = "toolStripbtnSubmit";
            this.toolStripbtnSubmit.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnSubmit.Text = "提交";
            // 
            // toolStripbtnPrint
            // 
            this.toolStripbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.toolStripbtnPrint.Image = global::RUINORERP.UI.Properties.Resources.print1;
            this.toolStripbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnPrint.Name = "toolStripbtnPrint";
            this.toolStripbtnPrint.Size = new System.Drawing.Size(65, 22);
            this.toolStripbtnPrint.Text = "打印";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "预览";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem2.Text = "设计";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripbtnReview
            // 
            this.toolStripbtnReview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnReview.Image")));
            this.toolStripbtnReview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnReview.Name = "toolStripbtnReview";
            this.toolStripbtnReview.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnReview.Text = "审核";
            // 
            // toolStripBtnReverseReview
            // 
            this.toolStripBtnReverseReview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnReverseReview.Image")));
            this.toolStripBtnReverseReview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnReverseReview.Name = "toolStripBtnReverseReview";
            this.toolStripBtnReverseReview.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnReverseReview.Text = "反审";
            // 
            // toolStripButton结案
            // 
            this.toolStripButton结案.Image = global::RUINORERP.UI.Properties.Resources.ok;
            this.toolStripButton结案.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton结案.Name = "toolStripButton结案";
            this.toolStripButton结案.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton结案.Text = "结案";
            // 
            // toolStripButton付款调整
            // 
            this.toolStripButton付款调整.Image = global::RUINORERP.UI.Properties.Resources.US_Dollar;
            this.toolStripButton付款调整.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton付款调整.Name = "toolStripButton付款调整";
            this.toolStripButton付款调整.Size = new System.Drawing.Size(79, 22);
            this.toolStripButton付款调整.Text = "付款调整";
            // 
            // toolStripbtnFunction
            // 
            this.toolStripbtnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制性新增ToolStripMenuItem});
            this.toolStripbtnFunction.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnFunction.Image")));
            this.toolStripbtnFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnFunction.Name = "toolStripbtnFunction";
            this.toolStripbtnFunction.Size = new System.Drawing.Size(62, 22);
            this.toolStripbtnFunction.Text = "功能";
            // 
            // 复制性新增ToolStripMenuItem
            // 
            this.复制性新增ToolStripMenuItem.Name = "复制性新增ToolStripMenuItem";
            this.复制性新增ToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.复制性新增ToolStripMenuItem.Text = "复制性新增";
            // 
            // toolStripbtnProperty
            // 
            this.toolStripbtnProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnProperty.Image")));
            this.toolStripbtnProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnProperty.Name = "toolStripbtnProperty";
            this.toolStripbtnProperty.Size = new System.Drawing.Size(53, 20);
            this.toolStripbtnProperty.Text = "属性";
            // 
            // toolStripbtnClose
            // 
            this.toolStripbtnClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnClose.Image")));
            this.toolStripbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnClose.Name = "toolStripbtnClose";
            this.toolStripbtnClose.Size = new System.Drawing.Size(53, 20);
            this.toolStripbtnClose.Text = "关闭";
            // 
            // tslLocked
            // 
            this.tslLocked.Image = global::RUINORERP.UI.Properties.Resources.lock_security_padlock_secure_icon;
            this.tslLocked.Name = "tslLocked";
            this.tslLocked.Size = new System.Drawing.Size(62, 16);
            this.tslLocked.Text = "已锁定";
            this.tslLocked.Visible = false;
            // 
            // bwRemoting
            // 
            this.bwRemoting.WorkerReportsProgress = true;
            this.bwRemoting.WorkerSupportsCancellation = true;
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // BaseBillEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "BaseBillEdit";
            this.Size = new System.Drawing.Size(869, 524);
            this.Load += new System.EventHandler(this.BaseBillEdit_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolTip toolTipBase;
        public System.Windows.Forms.Timer timerForToolTip;
        public System.Windows.Forms.BindingSource bindingSourceSub;
        public System.Windows.Forms.ToolStrip toolStrip1;
        public System.Windows.Forms.ToolStripButton toolStripbtnDelete;
        public System.Windows.Forms.ToolStripButton toolStripbtnModify;
        public System.Windows.Forms.ToolStripButton toolStripButtonSave;
        public System.Windows.Forms.ToolStripButton toolStripbtnQuery;
        public System.Windows.Forms.ToolStripButton toolStripbtnClose;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        public System.Windows.Forms.ToolStripButton toolStripbtnProperty;
        public System.Windows.Forms.ToolStripButton toolStripbtnReview;
        public System.Windows.Forms.ToolStripButton toolStripBtnReverseReview;
        public System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        public System.Windows.Forms.ToolStripDropDownButton toolStripbtnFunction;
        public System.Windows.Forms.ToolStripMenuItem 复制性新增ToolStripMenuItem;
        public System.Windows.Forms.ToolStripButton toolStripbtnSubmit;
        public System.Windows.Forms.ToolStripButton toolStripbtnAdd;
        internal System.Windows.Forms.ToolStripButton toolStripBtnCancel;
        public System.Windows.Forms.ToolStripButton toolStripButton结案;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        public System.Windows.Forms.ToolStripSplitButton toolStripbtnPrint;
        public System.Windows.Forms.ToolStripButton toolStripButton付款调整;
        private System.ComponentModel.BackgroundWorker bwRemoting;
        public System.Windows.Forms.ToolStripLabel tslLocked;
    }
}