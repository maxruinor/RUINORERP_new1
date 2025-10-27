
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:27
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
    partial class tb_FS_BusinessRelationQuery
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


this.lblBusinessNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBusinessNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRelatedField = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRelatedField = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsActive = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";


this.lblIsMainFile = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsMainFile = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsMainFile.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


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

           //#####BusinessType###Int32
//属性测试50BusinessType

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFileId );
this.Controls.Add(this.cmbFileId );

                
                this.Controls.Add(this.lblBusinessNo );
this.Controls.Add(this.txtBusinessNo );

                this.Controls.Add(this.lblRelatedField );
this.Controls.Add(this.txtRelatedField );

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                
                this.Controls.Add(this.lblIsMainFile );
this.Controls.Add(this.chkIsMainFile );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_FS_BusinessRelationQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbFileId;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBusinessNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBusinessNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRelatedField;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRelatedField;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsActive;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsMainFile;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsMainFile;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


