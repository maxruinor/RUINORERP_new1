// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 14:41:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度
    /// </summary>
    partial class tb_gl_CommentEdit
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
            this.lblBizTypeID = new Krypton.Toolkit.KryptonLabel();
            this.txtBizTypeID = new Krypton.Toolkit.KryptonTextBox();
            this.lblBusinessID = new Krypton.Toolkit.KryptonLabel();
            this.txtBusinessID = new Krypton.Toolkit.KryptonTextBox();
            this.lblDbTableName = new Krypton.Toolkit.KryptonLabel();
            this.txtDbTableName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCommentContent = new Krypton.Toolkit.KryptonLabel();
            this.txtCommentContent = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblBizTypeID
            // 
            this.lblBizTypeID.Location = new System.Drawing.Point(100, 25);
            this.lblBizTypeID.Name = "lblBizTypeID";
            this.lblBizTypeID.Size = new System.Drawing.Size(62, 20);
            this.lblBizTypeID.TabIndex = 1;
            this.lblBizTypeID.Values.Text = "业务类型";
            // 
            // txtBizTypeID
            // 
            this.txtBizTypeID.Location = new System.Drawing.Point(173, 21);
            this.txtBizTypeID.Name = "txtBizTypeID";
            this.txtBizTypeID.Size = new System.Drawing.Size(100, 23);
            this.txtBizTypeID.TabIndex = 1;
            // 
            // lblBusinessID
            // 
            this.lblBusinessID.Location = new System.Drawing.Point(100, 50);
            this.lblBusinessID.Name = "lblBusinessID";
            this.lblBusinessID.Size = new System.Drawing.Size(62, 20);
            this.lblBusinessID.TabIndex = 2;
            this.lblBusinessID.Values.Text = "关联业务";
            // 
            // txtBusinessID
            // 
            this.txtBusinessID.Location = new System.Drawing.Point(173, 46);
            this.txtBusinessID.Name = "txtBusinessID";
            this.txtBusinessID.Size = new System.Drawing.Size(100, 23);
            this.txtBusinessID.TabIndex = 2;
            // 
            // lblDbTableName
            // 
            this.lblDbTableName.Location = new System.Drawing.Point(100, 75);
            this.lblDbTableName.Name = "lblDbTableName";
            this.lblDbTableName.Size = new System.Drawing.Size(62, 20);
            this.lblDbTableName.TabIndex = 3;
            this.lblDbTableName.Values.Text = "关联表名";
            // 
            // txtDbTableName
            // 
            this.txtDbTableName.Location = new System.Drawing.Point(173, 71);
            this.txtDbTableName.Name = "txtDbTableName";
            this.txtDbTableName.Size = new System.Drawing.Size(100, 23);
            this.txtDbTableName.TabIndex = 3;
            // 
            // lblCommentContent
            // 
            this.lblCommentContent.Location = new System.Drawing.Point(100, 100);
            this.lblCommentContent.Name = "lblCommentContent";
            this.lblCommentContent.Size = new System.Drawing.Size(62, 20);
            this.lblCommentContent.TabIndex = 4;
            this.lblCommentContent.Values.Text = "批注内容";
            // 
            // txtCommentContent
            // 
            this.txtCommentContent.Location = new System.Drawing.Point(173, 96);
            this.txtCommentContent.Name = "txtCommentContent";
            this.txtCommentContent.Size = new System.Drawing.Size(100, 23);
            this.txtCommentContent.TabIndex = 4;
            // 
            // tb_gl_CommentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBizTypeID);
            this.Controls.Add(this.txtBizTypeID);
            this.Controls.Add(this.lblBusinessID);
            this.Controls.Add(this.txtBusinessID);
            this.Controls.Add(this.lblDbTableName);
            this.Controls.Add(this.txtDbTableName);
            this.Controls.Add(this.lblCommentContent);
            this.Controls.Add(this.txtCommentContent);
            this.Name = "tb_gl_CommentEdit";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblBizTypeID;
private Krypton.Toolkit.KryptonTextBox txtBizTypeID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessID;
private Krypton.Toolkit.KryptonTextBox txtBusinessID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDbTableName;
private Krypton.Toolkit.KryptonTextBox txtDbTableName;

    
        
              private Krypton.Toolkit.KryptonLabel lblCommentContent;
private Krypton.Toolkit.KryptonTextBox txtCommentContent;





    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

