﻿namespace CommonProcess
{
    partial class TextBoxContainButton
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBack = new System.Windows.Forms.TextBox();
            this.textBoxFront = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // textBoxBack
            // 
            this.textBoxBack.Location = new System.Drawing.Point(92, 3);
            this.textBoxBack.Name = "textBoxBack";
            this.textBoxBack.Size = new System.Drawing.Size(174, 21);
            this.textBoxBack.TabIndex = 1;
            // 
            // textBoxFront
            // 
            this.textBoxFront.Location = new System.Drawing.Point(92, 45);
            this.textBoxFront.Name = "textBoxFront";
            this.textBoxFront.Size = new System.Drawing.Size(174, 21);
            this.textBoxFront.TabIndex = 2;
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(294, 11);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(29, 23);
            this.button.TabIndex = 3;
            this.button.UseVisualStyleBackColor = true;
            // 
            // TextBoxContainButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button);
            this.Controls.Add(this.textBoxFront);
            this.Controls.Add(this.textBoxBack);
            this.Controls.Add(this.label1);
            this.Name = "TextBoxContainButton";
            this.Size = new System.Drawing.Size(384, 69);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBack;
        private System.Windows.Forms.TextBox textBoxFront;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button;
    }
}
