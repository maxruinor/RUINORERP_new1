using System;

namespace RUINORERP.UI.BaseForm
{
    partial class BaseBillEditGeneric<T,C>
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
                
                // 取消订阅锁状态变化
                try
                {
                    // 通过反射调用UnsubscribeFromLockStatusChanges方法
                    var method = typeof(BaseBillEditGeneric<T, C>).GetMethod("UnsubscribeFromLockStatusChanges", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    method?.Invoke(this, null);
                }
                catch (Exception ex)
                {
                    // 忽略取消订阅时的异常，避免影响正常的Dispose流程
                    System.Diagnostics.Debug.WriteLine($"取消订阅锁状态变化失败: {ex.Message}");
                }
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
            this.timerAutoSave = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.BlinkRate = 1000;
            this.errorProviderForAllInput.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            // 
            // timerAutoSave
            // 
            this.timerAutoSave.Interval = 1000;
            this.timerAutoSave.Tick += new System.EventHandler(this.timerAutoSave_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BaseBillEditGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BaseBillEditGeneric";
            this.Size = new System.Drawing.Size(927, 570);
            this.Load += new System.EventHandler(this.BaseBillEditGeneric_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerAutoSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
