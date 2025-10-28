
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/28/2025 17:14:17
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 文件版本表
    /// </summary>
    partial class tb_FS_FileStorageVersionQuery
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
     
     this.lblFileId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbFileId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblUpdateReason = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUpdateReason = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUpdateReason.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####FileId###Int64
//属性测试25FileId
this.lblFileId.AutoSize = true;
this.lblFileId.Location = new System.Drawing.Point(100,25);
this.lblFileId.Name = "lblFileId";
this.lblFileId.Size = new System.Drawing.Size(41, 12);
this.lblFileId.TabIndex = 1;
this.lblFileId.Text = "文件ID";
//111======25
this.cmbFileId.Location = new System.Drawing.Point(173,21);
this.cmbFileId.Name ="cmbFileId";
this.cmbFileId.Size = new System.Drawing.Size(100, 21);
this.cmbFileId.TabIndex = 1;
this.Controls.Add(this.lblFileId);
this.Controls.Add(this.cmbFileId);

           //#####VersionNo###Int32
//属性测试50VersionNo

           //#####300UpdateReason###String
this.lblUpdateReason.AutoSize = true;
this.lblUpdateReason.Location = new System.Drawing.Point(100,75);
this.lblUpdateReason.Name = "lblUpdateReason";
this.lblUpdateReason.Size = new System.Drawing.Size(41, 12);
this.lblUpdateReason.TabIndex = 3;
this.lblUpdateReason.Text = "存储路径";
this.txtUpdateReason.Location = new System.Drawing.Point(173,71);
this.txtUpdateReason.Name = "txtUpdateReason";
this.txtUpdateReason.Size = new System.Drawing.Size(100, 21);
this.txtUpdateReason.TabIndex = 3;
this.Controls.Add(this.lblUpdateReason);
this.Controls.Add(this.txtUpdateReason);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,100);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 4;
this.lblCreated_at.Text = "创建时间";
//111======100
this.dtpCreated_at.Location = new System.Drawing.Point(173,96);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 4;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试125Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,150);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 6;
this.lblModified_at.Text = "修改时间";
//111======150
this.dtpModified_at.Location = new System.Drawing.Point(173,146);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 6;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试175Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,200);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 8;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,196);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 8;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFileId );
this.Controls.Add(this.cmbFileId );

                
                this.Controls.Add(this.lblUpdateReason );
this.Controls.Add(this.txtUpdateReason );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_FS_FileStorageVersionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbFileId;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUpdateReason;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUpdateReason;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


