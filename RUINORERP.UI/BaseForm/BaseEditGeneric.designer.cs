namespace RUINORERP.UI.BaseForm
{
    partial class BaseEditGeneric<T>
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTipBase
            // 
            this.toolTipBase.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipBase_Popup);
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
            this.timerForToolTip.Tick += new System.EventHandler(this.timerForToolTip_Tick);
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // BaseEditGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 591);
            this.Name = "BaseEditGeneric";
            this.PaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.BaseEdit_Load);
            this.Shown += new System.EventHandler(this.BaseEditGeneric_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        internal System.Windows.Forms.ToolTip toolTipBase;
        private System.Windows.Forms.Timer timerForToolTip;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
    }
}
