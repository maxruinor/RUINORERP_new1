
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:18
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

this.lblIsMainFile = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsMainFile = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsMainFile.Values.Text ="";

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

           //#####IsMainFile###Boolean
this.lblIsMainFile.AutoSize = true;
this.lblIsMainFile.Location = new System.Drawing.Point(100,100);
this.lblIsMainFile.Name = "lblIsMainFile";
this.lblIsMainFile.Size = new System.Drawing.Size(41, 12);
this.lblIsMainFile.TabIndex = 4;
this.lblIsMainFile.Text = "已注册";
this.chkIsMainFile.Location = new System.Drawing.Point(173,96);
this.chkIsMainFile.Name = "chkIsMainFile";
this.chkIsMainFile.Size = new System.Drawing.Size(100, 21);
this.chkIsMainFile.TabIndex = 4;
this.Controls.Add(this.lblIsMainFile);
this.Controls.Add(this.chkIsMainFile);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFileId );
this.Controls.Add(this.cmbFileId );

                
                this.Controls.Add(this.lblBusinessNo );
this.Controls.Add(this.txtBusinessNo );

                this.Controls.Add(this.lblIsMainFile );
this.Controls.Add(this.chkIsMainFile );

                    
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsMainFile;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsMainFile;

    
    
   
 





    }
}


