
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 14:41:01
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
    partial class tb_gl_CommentQuery
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
        
     //for start
     
     

this.lblDbTableName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDbTableName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCommentContent = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCommentContent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####BizTypeID###Int32

           //#####BusinessID###Int64

           //#####100DbTableName###String
this.lblDbTableName.AutoSize = true;
this.lblDbTableName.Location = new System.Drawing.Point(100,75);
this.lblDbTableName.Name = "lblDbTableName";
this.lblDbTableName.Size = new System.Drawing.Size(41, 12);
this.lblDbTableName.TabIndex = 3;
this.lblDbTableName.Text = "关联表名";
this.txtDbTableName.Location = new System.Drawing.Point(173,71);
this.txtDbTableName.Name = "txtDbTableName";
this.txtDbTableName.Size = new System.Drawing.Size(100, 21);
this.txtDbTableName.TabIndex = 3;
this.Controls.Add(this.lblDbTableName);
this.Controls.Add(this.txtDbTableName);

           //#####200CommentContent###String
this.lblCommentContent.AutoSize = true;
this.lblCommentContent.Location = new System.Drawing.Point(100,100);
this.lblCommentContent.Name = "lblCommentContent";
this.lblCommentContent.Size = new System.Drawing.Size(41, 12);
this.lblCommentContent.TabIndex = 4;
this.lblCommentContent.Text = "批注内容";
this.txtCommentContent.Location = new System.Drawing.Point(173,96);
this.txtCommentContent.Name = "txtCommentContent";
this.txtCommentContent.Size = new System.Drawing.Size(100, 21);
this.txtCommentContent.TabIndex = 4;
this.Controls.Add(this.lblCommentContent);
this.Controls.Add(this.txtCommentContent);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,125);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 5;
this.lblCreated_at.Text = "创建时间";
//111======125
this.dtpCreated_at.Location = new System.Drawing.Point(173,121);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 5;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,175);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 7;
this.lblModified_at.Text = "修改时间";
//111======175
this.dtpModified_at.Location = new System.Drawing.Point(173,171);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 7;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblDbTableName );
this.Controls.Add(this.txtDbTableName );

                this.Controls.Add(this.lblCommentContent );
this.Controls.Add(this.txtCommentContent );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_gl_CommentQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDbTableName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDbTableName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCommentContent;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCommentContent;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


