// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/28/2025 17:43:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 文件业务关联表
    /// </summary>
    partial class tb_FS_BusinessRelationEdit
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

this.lblBusinessType = new Krypton.Toolkit.KryptonLabel();
this.txtBusinessType = new Krypton.Toolkit.KryptonTextBox();

this.lblBusinessNo = new Krypton.Toolkit.KryptonLabel();
this.txtBusinessNo = new Krypton.Toolkit.KryptonTextBox();

this.lblRelatedField = new Krypton.Toolkit.KryptonLabel();
this.txtRelatedField = new Krypton.Toolkit.KryptonTextBox();

this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";

this.lblVersionNo = new Krypton.Toolkit.KryptonLabel();
this.txtVersionNo = new Krypton.Toolkit.KryptonTextBox();

this.lblIsMainFile = new Krypton.Toolkit.KryptonLabel();
this.chkIsMainFile = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsMainFile.Values.Text ="";

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
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

           //#####BusinessType###Int32
//属性测试50BusinessType
this.lblBusinessType.AutoSize = true;
this.lblBusinessType.Location = new System.Drawing.Point(100,50);
this.lblBusinessType.Name = "lblBusinessType";
this.lblBusinessType.Size = new System.Drawing.Size(41, 12);
this.lblBusinessType.TabIndex = 2;
this.lblBusinessType.Text = "业务类型";
this.txtBusinessType.Location = new System.Drawing.Point(173,46);
this.txtBusinessType.Name = "txtBusinessType";
this.txtBusinessType.Size = new System.Drawing.Size(100, 21);
this.txtBusinessType.TabIndex = 2;
this.Controls.Add(this.lblBusinessType);
this.Controls.Add(this.txtBusinessType);

           //#####50BusinessNo###String
this.lblBusinessNo.AutoSize = true;
this.lblBusinessNo.Location = new System.Drawing.Point(100,75);
this.lblBusinessNo.Name = "lblBusinessNo";
this.lblBusinessNo.Size = new System.Drawing.Size(41, 12);
this.lblBusinessNo.TabIndex = 3;
this.lblBusinessNo.Text = "业务编号";
this.txtBusinessNo.Location = new System.Drawing.Point(173,71);
this.txtBusinessNo.Name = "txtBusinessNo";
this.txtBusinessNo.Size = new System.Drawing.Size(100, 21);
this.txtBusinessNo.TabIndex = 3;
this.Controls.Add(this.lblBusinessNo);
this.Controls.Add(this.txtBusinessNo);

           //#####50RelatedField###String
this.lblRelatedField.AutoSize = true;
this.lblRelatedField.Location = new System.Drawing.Point(100,100);
this.lblRelatedField.Name = "lblRelatedField";
this.lblRelatedField.Size = new System.Drawing.Size(41, 12);
this.lblRelatedField.TabIndex = 4;
this.lblRelatedField.Text = "关联字段";
this.txtRelatedField.Location = new System.Drawing.Point(173,96);
this.txtRelatedField.Name = "txtRelatedField";
this.txtRelatedField.Size = new System.Drawing.Size(100, 21);
this.txtRelatedField.TabIndex = 4;
this.Controls.Add(this.lblRelatedField);
this.Controls.Add(this.txtRelatedField);

           //#####IsActive###Boolean
this.lblIsActive.AutoSize = true;
this.lblIsActive.Location = new System.Drawing.Point(100,125);
this.lblIsActive.Name = "lblIsActive";
this.lblIsActive.Size = new System.Drawing.Size(41, 12);
this.lblIsActive.TabIndex = 5;
this.lblIsActive.Text = "活跃";
this.chkIsActive.Location = new System.Drawing.Point(173,121);
this.chkIsActive.Name = "chkIsActive";
this.chkIsActive.Size = new System.Drawing.Size(100, 21);
this.chkIsActive.TabIndex = 5;
this.Controls.Add(this.lblIsActive);
this.Controls.Add(this.chkIsActive);

           //#####VersionNo###Int32
//属性测试150VersionNo
this.lblVersionNo.AutoSize = true;
this.lblVersionNo.Location = new System.Drawing.Point(100,150);
this.lblVersionNo.Name = "lblVersionNo";
this.lblVersionNo.Size = new System.Drawing.Size(41, 12);
this.lblVersionNo.TabIndex = 6;
this.lblVersionNo.Text = "关联版本号";
this.txtVersionNo.Location = new System.Drawing.Point(173,146);
this.txtVersionNo.Name = "txtVersionNo";
this.txtVersionNo.Size = new System.Drawing.Size(100, 21);
this.txtVersionNo.TabIndex = 6;
this.Controls.Add(this.lblVersionNo);
this.Controls.Add(this.txtVersionNo);

           //#####IsMainFile###Boolean
this.lblIsMainFile.AutoSize = true;
this.lblIsMainFile.Location = new System.Drawing.Point(100,175);
this.lblIsMainFile.Name = "lblIsMainFile";
this.lblIsMainFile.Size = new System.Drawing.Size(41, 12);
this.lblIsMainFile.TabIndex = 7;
this.lblIsMainFile.Text = "主文件";
this.chkIsMainFile.Location = new System.Drawing.Point(173,171);
this.chkIsMainFile.Name = "chkIsMainFile";
this.chkIsMainFile.Size = new System.Drawing.Size(100, 21);
this.chkIsMainFile.TabIndex = 7;
this.Controls.Add(this.lblIsMainFile);
this.Controls.Add(this.chkIsMainFile);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,200);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 8;
this.lblCreated_at.Text = "创建时间";
//111======200
this.dtpCreated_at.Location = new System.Drawing.Point(173,196);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 8;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试225Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,225);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 9;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,221);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 9;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,250);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 10;
this.lblModified_at.Text = "修改时间";
//111======250
this.dtpModified_at.Location = new System.Drawing.Point(173,246);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 10;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试275Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,275);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 11;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,271);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 11;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,300);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 12;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,296);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 12;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

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
           // this.kryptonPanel1.TabIndex = 12;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFileId );
this.Controls.Add(this.cmbFileId );

                this.Controls.Add(this.lblBusinessType );
this.Controls.Add(this.txtBusinessType );

                this.Controls.Add(this.lblBusinessNo );
this.Controls.Add(this.txtBusinessNo );

                this.Controls.Add(this.lblRelatedField );
this.Controls.Add(this.txtRelatedField );

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                this.Controls.Add(this.lblVersionNo );
this.Controls.Add(this.txtVersionNo );

                this.Controls.Add(this.lblIsMainFile );
this.Controls.Add(this.chkIsMainFile );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "tb_FS_BusinessRelationEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FS_BusinessRelationEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessType;
private Krypton.Toolkit.KryptonTextBox txtBusinessType;

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessNo;
private Krypton.Toolkit.KryptonTextBox txtBusinessNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblRelatedField;
private Krypton.Toolkit.KryptonTextBox txtRelatedField;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsActive;
private Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
              private Krypton.Toolkit.KryptonLabel lblVersionNo;
private Krypton.Toolkit.KryptonTextBox txtVersionNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsMainFile;
private Krypton.Toolkit.KryptonCheckBox chkIsMainFile;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

