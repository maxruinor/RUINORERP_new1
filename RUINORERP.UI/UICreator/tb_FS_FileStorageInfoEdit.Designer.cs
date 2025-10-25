// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:19
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 文件信息元数据表
    /// </summary>
    partial class tb_FS_FileStorageInfoEdit
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
     this.lblOriginalFileName = new Krypton.Toolkit.KryptonLabel();
this.txtOriginalFileName = new Krypton.Toolkit.KryptonTextBox();
this.txtOriginalFileName.Multiline = true;

this.lblStorageFileName = new Krypton.Toolkit.KryptonLabel();
this.txtStorageFileName = new Krypton.Toolkit.KryptonTextBox();
this.txtStorageFileName.Multiline = true;

this.lblBusinessType = new Krypton.Toolkit.KryptonLabel();
this.txtBusinessType = new Krypton.Toolkit.KryptonTextBox();

this.lblFileType = new Krypton.Toolkit.KryptonLabel();
this.txtFileType = new Krypton.Toolkit.KryptonTextBox();

this.lblFileSize = new Krypton.Toolkit.KryptonLabel();
this.txtFileSize = new Krypton.Toolkit.KryptonTextBox();

this.lblHashValue = new Krypton.Toolkit.KryptonLabel();
this.txtHashValue = new Krypton.Toolkit.KryptonTextBox();

this.lblStorageProvider = new Krypton.Toolkit.KryptonLabel();
this.txtStorageProvider = new Krypton.Toolkit.KryptonTextBox();

this.lblStoragePath = new Krypton.Toolkit.KryptonLabel();
this.txtStoragePath = new Krypton.Toolkit.KryptonTextBox();
this.txtStoragePath.Multiline = true;

this.lblCurrentVersion = new Krypton.Toolkit.KryptonLabel();
this.txtCurrentVersion = new Krypton.Toolkit.KryptonTextBox();

this.lblStatus = new Krypton.Toolkit.KryptonLabel();
this.txtStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblExpireTime = new Krypton.Toolkit.KryptonLabel();
this.dtpExpireTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsRegistered = new Krypton.Toolkit.KryptonLabel();
this.chkIsRegistered = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsRegistered.Values.Text ="";

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblMetadata = new Krypton.Toolkit.KryptonLabel();
this.txtMetadata = new Krypton.Toolkit.KryptonTextBox();
this.txtMetadata.Multiline = true;

    
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
     
            //#####255OriginalFileName###String
this.lblOriginalFileName.AutoSize = true;
this.lblOriginalFileName.Location = new System.Drawing.Point(100,25);
this.lblOriginalFileName.Name = "lblOriginalFileName";
this.lblOriginalFileName.Size = new System.Drawing.Size(41, 12);
this.lblOriginalFileName.TabIndex = 1;
this.lblOriginalFileName.Text = "原始文件名";
this.txtOriginalFileName.Location = new System.Drawing.Point(173,21);
this.txtOriginalFileName.Name = "txtOriginalFileName";
this.txtOriginalFileName.Size = new System.Drawing.Size(100, 21);
this.txtOriginalFileName.TabIndex = 1;
this.Controls.Add(this.lblOriginalFileName);
this.Controls.Add(this.txtOriginalFileName);

           //#####255StorageFileName###String
this.lblStorageFileName.AutoSize = true;
this.lblStorageFileName.Location = new System.Drawing.Point(100,50);
this.lblStorageFileName.Name = "lblStorageFileName";
this.lblStorageFileName.Size = new System.Drawing.Size(41, 12);
this.lblStorageFileName.TabIndex = 2;
this.lblStorageFileName.Text = "存储文件名";
this.txtStorageFileName.Location = new System.Drawing.Point(173,46);
this.txtStorageFileName.Name = "txtStorageFileName";
this.txtStorageFileName.Size = new System.Drawing.Size(100, 21);
this.txtStorageFileName.TabIndex = 2;
this.Controls.Add(this.lblStorageFileName);
this.Controls.Add(this.txtStorageFileName);

           //#####BusinessType###Int32
this.lblBusinessType.AutoSize = true;
this.lblBusinessType.Location = new System.Drawing.Point(100,75);
this.lblBusinessType.Name = "lblBusinessType";
this.lblBusinessType.Size = new System.Drawing.Size(41, 12);
this.lblBusinessType.TabIndex = 3;
this.lblBusinessType.Text = "业务类型";
this.txtBusinessType.Location = new System.Drawing.Point(173,71);
this.txtBusinessType.Name = "txtBusinessType";
this.txtBusinessType.Size = new System.Drawing.Size(100, 21);
this.txtBusinessType.TabIndex = 3;
this.Controls.Add(this.lblBusinessType);
this.Controls.Add(this.txtBusinessType);

           //#####200FileType###String
this.lblFileType.AutoSize = true;
this.lblFileType.Location = new System.Drawing.Point(100,100);
this.lblFileType.Name = "lblFileType";
this.lblFileType.Size = new System.Drawing.Size(41, 12);
this.lblFileType.TabIndex = 4;
this.lblFileType.Text = "文件类型";
this.txtFileType.Location = new System.Drawing.Point(173,96);
this.txtFileType.Name = "txtFileType";
this.txtFileType.Size = new System.Drawing.Size(100, 21);
this.txtFileType.TabIndex = 4;
this.Controls.Add(this.lblFileType);
this.Controls.Add(this.txtFileType);

           //#####FileSize###Int64
this.lblFileSize.AutoSize = true;
this.lblFileSize.Location = new System.Drawing.Point(100,125);
this.lblFileSize.Name = "lblFileSize";
this.lblFileSize.Size = new System.Drawing.Size(41, 12);
this.lblFileSize.TabIndex = 5;
this.lblFileSize.Text = "文件大小（字节）";
this.txtFileSize.Location = new System.Drawing.Point(173,121);
this.txtFileSize.Name = "txtFileSize";
this.txtFileSize.Size = new System.Drawing.Size(100, 21);
this.txtFileSize.TabIndex = 5;
this.Controls.Add(this.lblFileSize);
this.Controls.Add(this.txtFileSize);

           //#####64HashValue###String
this.lblHashValue.AutoSize = true;
this.lblHashValue.Location = new System.Drawing.Point(100,150);
this.lblHashValue.Name = "lblHashValue";
this.lblHashValue.Size = new System.Drawing.Size(41, 12);
this.lblHashValue.TabIndex = 6;
this.lblHashValue.Text = "文件哈希值";
this.txtHashValue.Location = new System.Drawing.Point(173,146);
this.txtHashValue.Name = "txtHashValue";
this.txtHashValue.Size = new System.Drawing.Size(100, 21);
this.txtHashValue.TabIndex = 6;
this.Controls.Add(this.lblHashValue);
this.Controls.Add(this.txtHashValue);

           //#####50StorageProvider###String
this.lblStorageProvider.AutoSize = true;
this.lblStorageProvider.Location = new System.Drawing.Point(100,175);
this.lblStorageProvider.Name = "lblStorageProvider";
this.lblStorageProvider.Size = new System.Drawing.Size(41, 12);
this.lblStorageProvider.TabIndex = 7;
this.lblStorageProvider.Text = "存储引擎";
this.txtStorageProvider.Location = new System.Drawing.Point(173,171);
this.txtStorageProvider.Name = "txtStorageProvider";
this.txtStorageProvider.Size = new System.Drawing.Size(100, 21);
this.txtStorageProvider.TabIndex = 7;
this.Controls.Add(this.lblStorageProvider);
this.Controls.Add(this.txtStorageProvider);

           //#####300StoragePath###String
this.lblStoragePath.AutoSize = true;
this.lblStoragePath.Location = new System.Drawing.Point(100,200);
this.lblStoragePath.Name = "lblStoragePath";
this.lblStoragePath.Size = new System.Drawing.Size(41, 12);
this.lblStoragePath.TabIndex = 8;
this.lblStoragePath.Text = "存储路径";
this.txtStoragePath.Location = new System.Drawing.Point(173,196);
this.txtStoragePath.Name = "txtStoragePath";
this.txtStoragePath.Size = new System.Drawing.Size(100, 21);
this.txtStoragePath.TabIndex = 8;
this.Controls.Add(this.lblStoragePath);
this.Controls.Add(this.txtStoragePath);

           //#####CurrentVersion###Int32
this.lblCurrentVersion.AutoSize = true;
this.lblCurrentVersion.Location = new System.Drawing.Point(100,225);
this.lblCurrentVersion.Name = "lblCurrentVersion";
this.lblCurrentVersion.Size = new System.Drawing.Size(41, 12);
this.lblCurrentVersion.TabIndex = 9;
this.lblCurrentVersion.Text = "版本号";
this.txtCurrentVersion.Location = new System.Drawing.Point(173,221);
this.txtCurrentVersion.Name = "txtCurrentVersion";
this.txtCurrentVersion.Size = new System.Drawing.Size(100, 21);
this.txtCurrentVersion.TabIndex = 9;
this.Controls.Add(this.lblCurrentVersion);
this.Controls.Add(this.txtCurrentVersion);

           //#####Status###Int32
this.lblStatus.AutoSize = true;
this.lblStatus.Location = new System.Drawing.Point(100,250);
this.lblStatus.Name = "lblStatus";
this.lblStatus.Size = new System.Drawing.Size(41, 12);
this.lblStatus.TabIndex = 10;
this.lblStatus.Text = "文件状态";
this.txtStatus.Location = new System.Drawing.Point(173,246);
this.txtStatus.Name = "txtStatus";
this.txtStatus.Size = new System.Drawing.Size(100, 21);
this.txtStatus.TabIndex = 10;
this.Controls.Add(this.lblStatus);
this.Controls.Add(this.txtStatus);

           //#####ExpireTime###DateTime
this.lblExpireTime.AutoSize = true;
this.lblExpireTime.Location = new System.Drawing.Point(100,275);
this.lblExpireTime.Name = "lblExpireTime";
this.lblExpireTime.Size = new System.Drawing.Size(41, 12);
this.lblExpireTime.TabIndex = 11;
this.lblExpireTime.Text = "过期时间";
//111======275
this.dtpExpireTime.Location = new System.Drawing.Point(173,271);
this.dtpExpireTime.Name ="dtpExpireTime";
this.dtpExpireTime.Size = new System.Drawing.Size(100, 21);
this.dtpExpireTime.TabIndex = 11;
this.Controls.Add(this.lblExpireTime);
this.Controls.Add(this.dtpExpireTime);

           //#####IsRegistered###Boolean
this.lblIsRegistered.AutoSize = true;
this.lblIsRegistered.Location = new System.Drawing.Point(100,300);
this.lblIsRegistered.Name = "lblIsRegistered";
this.lblIsRegistered.Size = new System.Drawing.Size(41, 12);
this.lblIsRegistered.TabIndex = 12;
this.lblIsRegistered.Text = "已注册";
this.chkIsRegistered.Location = new System.Drawing.Point(173,296);
this.chkIsRegistered.Name = "chkIsRegistered";
this.chkIsRegistered.Size = new System.Drawing.Size(100, 21);
this.chkIsRegistered.TabIndex = 12;
this.Controls.Add(this.lblIsRegistered);
this.Controls.Add(this.chkIsRegistered);

           //#####200Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,325);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 13;
this.lblDescription.Text = "文件描述";
this.txtDescription.Location = new System.Drawing.Point(173,321);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 13;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,375);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 15;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,371);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 15;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,425);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 17;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,421);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 17;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####2147483647Metadata###String
this.lblMetadata.AutoSize = true;
this.lblMetadata.Location = new System.Drawing.Point(100,450);
this.lblMetadata.Name = "lblMetadata";
this.lblMetadata.Size = new System.Drawing.Size(41, 12);
this.lblMetadata.TabIndex = 18;
this.lblMetadata.Text = "扩展元数据";
this.txtMetadata.Location = new System.Drawing.Point(173,446);
this.txtMetadata.Name = "txtMetadata";
this.txtMetadata.Size = new System.Drawing.Size(100, 21);
this.txtMetadata.TabIndex = 18;
this.txtMetadata.Multiline = true;
this.Controls.Add(this.lblMetadata);
this.Controls.Add(this.txtMetadata);

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
           // this.kryptonPanel1.TabIndex = 18;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblOriginalFileName );
this.Controls.Add(this.txtOriginalFileName );

                this.Controls.Add(this.lblStorageFileName );
this.Controls.Add(this.txtStorageFileName );

                this.Controls.Add(this.lblBusinessType );
this.Controls.Add(this.txtBusinessType );

                this.Controls.Add(this.lblFileType );
this.Controls.Add(this.txtFileType );

                this.Controls.Add(this.lblFileSize );
this.Controls.Add(this.txtFileSize );

                this.Controls.Add(this.lblHashValue );
this.Controls.Add(this.txtHashValue );

                this.Controls.Add(this.lblStorageProvider );
this.Controls.Add(this.txtStorageProvider );

                this.Controls.Add(this.lblStoragePath );
this.Controls.Add(this.txtStoragePath );

                this.Controls.Add(this.lblCurrentVersion );
this.Controls.Add(this.txtCurrentVersion );

                this.Controls.Add(this.lblStatus );
this.Controls.Add(this.txtStatus );

                this.Controls.Add(this.lblExpireTime );
this.Controls.Add(this.dtpExpireTime );

                this.Controls.Add(this.lblIsRegistered );
this.Controls.Add(this.chkIsRegistered );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblMetadata );
this.Controls.Add(this.txtMetadata );

                            // 
            // "tb_FS_FileStorageInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FS_FileStorageInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblOriginalFileName;
private Krypton.Toolkit.KryptonTextBox txtOriginalFileName;

    
        
              private Krypton.Toolkit.KryptonLabel lblStorageFileName;
private Krypton.Toolkit.KryptonTextBox txtStorageFileName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessType;
private Krypton.Toolkit.KryptonTextBox txtBusinessType;

    
        
              private Krypton.Toolkit.KryptonLabel lblFileType;
private Krypton.Toolkit.KryptonTextBox txtFileType;

    
        
              private Krypton.Toolkit.KryptonLabel lblFileSize;
private Krypton.Toolkit.KryptonTextBox txtFileSize;

    
        
              private Krypton.Toolkit.KryptonLabel lblHashValue;
private Krypton.Toolkit.KryptonTextBox txtHashValue;

    
        
              private Krypton.Toolkit.KryptonLabel lblStorageProvider;
private Krypton.Toolkit.KryptonTextBox txtStorageProvider;

    
        
              private Krypton.Toolkit.KryptonLabel lblStoragePath;
private Krypton.Toolkit.KryptonTextBox txtStoragePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrentVersion;
private Krypton.Toolkit.KryptonTextBox txtCurrentVersion;

    
        
              private Krypton.Toolkit.KryptonLabel lblStatus;
private Krypton.Toolkit.KryptonTextBox txtStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpireTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpireTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsRegistered;
private Krypton.Toolkit.KryptonCheckBox chkIsRegistered;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblMetadata;
private Krypton.Toolkit.KryptonTextBox txtMetadata;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

