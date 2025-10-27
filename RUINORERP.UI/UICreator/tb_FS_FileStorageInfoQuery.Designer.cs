
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:29
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
    partial class tb_FS_FileStorageInfoQuery
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
     
     this.lblOriginalFileName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOriginalFileName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtOriginalFileName.Multiline = true;

this.lblStorageFileName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStorageFileName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtStorageFileName.Multiline = true;

this.lblFileExtension = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFileExtension = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFileExtension.Multiline = true;


this.lblFileType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFileType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblHashValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHashValue = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblStorageProvider = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStorageProvider = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblStoragePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStoragePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtStoragePath.Multiline = true;



this.lblExpireTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpireTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblMetadata = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMetadata = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMetadata.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####255FileExtension###String
this.lblFileExtension.AutoSize = true;
this.lblFileExtension.Location = new System.Drawing.Point(100,75);
this.lblFileExtension.Name = "lblFileExtension";
this.lblFileExtension.Size = new System.Drawing.Size(41, 12);
this.lblFileExtension.TabIndex = 3;
this.lblFileExtension.Text = "文件扩展名";
this.txtFileExtension.Location = new System.Drawing.Point(173,71);
this.txtFileExtension.Name = "txtFileExtension";
this.txtFileExtension.Size = new System.Drawing.Size(100, 21);
this.txtFileExtension.TabIndex = 3;
this.Controls.Add(this.lblFileExtension);
this.Controls.Add(this.txtFileExtension);

           //#####BusinessType###Int32

           //#####200FileType###String
this.lblFileType.AutoSize = true;
this.lblFileType.Location = new System.Drawing.Point(100,125);
this.lblFileType.Name = "lblFileType";
this.lblFileType.Size = new System.Drawing.Size(41, 12);
this.lblFileType.TabIndex = 5;
this.lblFileType.Text = "文件类型";
this.txtFileType.Location = new System.Drawing.Point(173,121);
this.txtFileType.Name = "txtFileType";
this.txtFileType.Size = new System.Drawing.Size(100, 21);
this.txtFileType.TabIndex = 5;
this.Controls.Add(this.lblFileType);
this.Controls.Add(this.txtFileType);

           //#####FileSize###Int64

           //#####64HashValue###String
this.lblHashValue.AutoSize = true;
this.lblHashValue.Location = new System.Drawing.Point(100,175);
this.lblHashValue.Name = "lblHashValue";
this.lblHashValue.Size = new System.Drawing.Size(41, 12);
this.lblHashValue.TabIndex = 7;
this.lblHashValue.Text = "文件哈希值";
this.txtHashValue.Location = new System.Drawing.Point(173,171);
this.txtHashValue.Name = "txtHashValue";
this.txtHashValue.Size = new System.Drawing.Size(100, 21);
this.txtHashValue.TabIndex = 7;
this.Controls.Add(this.lblHashValue);
this.Controls.Add(this.txtHashValue);

           //#####50StorageProvider###String
this.lblStorageProvider.AutoSize = true;
this.lblStorageProvider.Location = new System.Drawing.Point(100,200);
this.lblStorageProvider.Name = "lblStorageProvider";
this.lblStorageProvider.Size = new System.Drawing.Size(41, 12);
this.lblStorageProvider.TabIndex = 8;
this.lblStorageProvider.Text = "存储引擎";
this.txtStorageProvider.Location = new System.Drawing.Point(173,196);
this.txtStorageProvider.Name = "txtStorageProvider";
this.txtStorageProvider.Size = new System.Drawing.Size(100, 21);
this.txtStorageProvider.TabIndex = 8;
this.Controls.Add(this.lblStorageProvider);
this.Controls.Add(this.txtStorageProvider);

           //#####300StoragePath###String
this.lblStoragePath.AutoSize = true;
this.lblStoragePath.Location = new System.Drawing.Point(100,225);
this.lblStoragePath.Name = "lblStoragePath";
this.lblStoragePath.Size = new System.Drawing.Size(41, 12);
this.lblStoragePath.TabIndex = 9;
this.lblStoragePath.Text = "存储路径";
this.txtStoragePath.Location = new System.Drawing.Point(173,221);
this.txtStoragePath.Name = "txtStoragePath";
this.txtStoragePath.Size = new System.Drawing.Size(100, 21);
this.txtStoragePath.TabIndex = 9;
this.Controls.Add(this.lblStoragePath);
this.Controls.Add(this.txtStoragePath);

           //#####CurrentVersion###Int32

           //#####Status###Int32

           //#####ExpireTime###DateTime
this.lblExpireTime.AutoSize = true;
this.lblExpireTime.Location = new System.Drawing.Point(100,300);
this.lblExpireTime.Name = "lblExpireTime";
this.lblExpireTime.Size = new System.Drawing.Size(41, 12);
this.lblExpireTime.TabIndex = 12;
this.lblExpireTime.Text = "过期时间";
//111======300
this.dtpExpireTime.Location = new System.Drawing.Point(173,296);
this.dtpExpireTime.Name ="dtpExpireTime";
this.dtpExpireTime.Size = new System.Drawing.Size(100, 21);
this.dtpExpireTime.TabIndex = 12;
this.Controls.Add(this.lblExpireTime);
this.Controls.Add(this.dtpExpireTime);

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblOriginalFileName );
this.Controls.Add(this.txtOriginalFileName );

                this.Controls.Add(this.lblStorageFileName );
this.Controls.Add(this.txtStorageFileName );

                this.Controls.Add(this.lblFileExtension );
this.Controls.Add(this.txtFileExtension );

                
                this.Controls.Add(this.lblFileType );
this.Controls.Add(this.txtFileType );

                
                this.Controls.Add(this.lblHashValue );
this.Controls.Add(this.txtHashValue );

                this.Controls.Add(this.lblStorageProvider );
this.Controls.Add(this.txtStorageProvider );

                this.Controls.Add(this.lblStoragePath );
this.Controls.Add(this.txtStoragePath );

                
                
                this.Controls.Add(this.lblExpireTime );
this.Controls.Add(this.dtpExpireTime );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblMetadata );
this.Controls.Add(this.txtMetadata );

                    
            this.Name = "tb_FS_FileStorageInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOriginalFileName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOriginalFileName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStorageFileName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStorageFileName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileExtension;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFileExtension;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFileType;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHashValue;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHashValue;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStorageProvider;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStorageProvider;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStoragePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStoragePath;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpireTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpireTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMetadata;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMetadata;

    
    
   
 





    }
}


