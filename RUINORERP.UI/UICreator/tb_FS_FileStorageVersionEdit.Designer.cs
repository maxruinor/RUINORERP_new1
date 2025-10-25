// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:20
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
    partial class tb_FS_FileStorageVersionEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblFileId = new Krypton.Toolkit.KryptonLabel();
this.cmbFileId = new Krypton.Toolkit.KryptonComboBox();

this.lblVersionNo = new Krypton.Toolkit.KryptonLabel();
this.txtVersionNo = new Krypton.Toolkit.KryptonTextBox();

this.lblStorageFileName = new Krypton.Toolkit.KryptonLabel();
this.txtStorageFileName = new Krypton.Toolkit.KryptonTextBox();
this.txtStorageFileName.Multiline = true;

this.lblUpdateReason = new Krypton.Toolkit.KryptonLabel();
this.txtUpdateReason = new Krypton.Toolkit.KryptonTextBox();
this.txtUpdateReason.Multiline = true;

this.lblHashValue = new Krypton.Toolkit.KryptonLabel();
this.txtHashValue = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
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
this.lblVersionNo.AutoSize = true;
this.lblVersionNo.Location = new System.Drawing.Point(100,50);
this.lblVersionNo.Name = "lblVersionNo";
this.lblVersionNo.Size = new System.Drawing.Size(41, 12);
this.lblVersionNo.TabIndex = 2;
this.lblVersionNo.Text = "版本号";
this.txtVersionNo.Location = new System.Drawing.Point(173,46);
this.txtVersionNo.Name = "txtVersionNo";
this.txtVersionNo.Size = new System.Drawing.Size(100, 21);
this.txtVersionNo.TabIndex = 2;
this.Controls.Add(this.lblVersionNo);
this.Controls.Add(this.txtVersionNo);

           //#####255StorageFileName###String
this.lblStorageFileName.AutoSize = true;
this.lblStorageFileName.Location = new System.Drawing.Point(100,75);
this.lblStorageFileName.Name = "lblStorageFileName";
this.lblStorageFileName.Size = new System.Drawing.Size(41, 12);
this.lblStorageFileName.TabIndex = 3;
this.lblStorageFileName.Text = "存储文件名";
this.txtStorageFileName.Location = new System.Drawing.Point(173,71);
this.txtStorageFileName.Name = "txtStorageFileName";
this.txtStorageFileName.Size = new System.Drawing.Size(100, 21);
this.txtStorageFileName.TabIndex = 3;
this.Controls.Add(this.lblStorageFileName);
this.Controls.Add(this.txtStorageFileName);

           //#####300UpdateReason###String
this.lblUpdateReason.AutoSize = true;
this.lblUpdateReason.Location = new System.Drawing.Point(100,100);
this.lblUpdateReason.Name = "lblUpdateReason";
this.lblUpdateReason.Size = new System.Drawing.Size(41, 12);
this.lblUpdateReason.TabIndex = 4;
this.lblUpdateReason.Text = "存储路径";
this.txtUpdateReason.Location = new System.Drawing.Point(173,96);
this.txtUpdateReason.Name = "txtUpdateReason";
this.txtUpdateReason.Size = new System.Drawing.Size(100, 21);
this.txtUpdateReason.TabIndex = 4;
this.Controls.Add(this.lblUpdateReason);
this.Controls.Add(this.txtUpdateReason);

           //#####64HashValue###String
this.lblHashValue.AutoSize = true;
this.lblHashValue.Location = new System.Drawing.Point(100,125);
this.lblHashValue.Name = "lblHashValue";
this.lblHashValue.Size = new System.Drawing.Size(41, 12);
this.lblHashValue.TabIndex = 5;
this.lblHashValue.Text = "文件哈希值";
this.txtHashValue.Location = new System.Drawing.Point(173,121);
this.txtHashValue.Name = "txtHashValue";
this.txtHashValue.Size = new System.Drawing.Size(100, 21);
this.txtHashValue.TabIndex = 5;
this.Controls.Add(this.lblHashValue);
this.Controls.Add(this.txtHashValue);

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
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 6;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试175Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,175);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 7;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,171);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 7;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 7;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFileId );
this.Controls.Add(this.cmbFileId );

                this.Controls.Add(this.lblVersionNo );
this.Controls.Add(this.txtVersionNo );

                this.Controls.Add(this.lblStorageFileName );
this.Controls.Add(this.txtStorageFileName );

                this.Controls.Add(this.lblUpdateReason );
this.Controls.Add(this.txtUpdateReason );

                this.Controls.Add(this.lblHashValue );
this.Controls.Add(this.txtHashValue );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_FS_FileStorageVersionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FS_FileStorageVersionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblFileId;
private Krypton.Toolkit.KryptonComboBox cmbFileId;

    
        
              private Krypton.Toolkit.KryptonLabel lblVersionNo;
private Krypton.Toolkit.KryptonTextBox txtVersionNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblStorageFileName;
private Krypton.Toolkit.KryptonTextBox txtStorageFileName;

    
        
              private Krypton.Toolkit.KryptonLabel lblUpdateReason;
private Krypton.Toolkit.KryptonTextBox txtUpdateReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblHashValue;
private Krypton.Toolkit.KryptonTextBox txtHashValue;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

