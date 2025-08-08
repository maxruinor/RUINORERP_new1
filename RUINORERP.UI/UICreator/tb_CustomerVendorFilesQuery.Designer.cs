
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 客户厂商认证文件表
    /// </summary>
    partial class tb_CustomerVendorFilesQuery
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
     
     this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblFileName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFileName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFileType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFileType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####CustomerVendor_ID###Int64
//属性测试25CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,25);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 1;
this.lblCustomerVendor_ID.Text = "";
//111======25
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,21);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 1;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####200FileName###String
this.lblFileName.AutoSize = true;
this.lblFileName.Location = new System.Drawing.Point(100,50);
this.lblFileName.Name = "lblFileName";
this.lblFileName.Size = new System.Drawing.Size(41, 12);
this.lblFileName.TabIndex = 2;
this.lblFileName.Text = "文件名";
this.txtFileName.Location = new System.Drawing.Point(173,46);
this.txtFileName.Name = "txtFileName";
this.txtFileName.Size = new System.Drawing.Size(100, 21);
this.txtFileName.TabIndex = 2;
this.Controls.Add(this.lblFileName);
this.Controls.Add(this.txtFileName);

           //#####50FileType###String
this.lblFileType.AutoSize = true;
this.lblFileType.Location = new System.Drawing.Point(100,75);
this.lblFileType.Name = "lblFileType";
this.lblFileType.Size = new System.Drawing.Size(41, 12);
this.lblFileType.TabIndex = 3;
this.lblFileType.Text = "文件类型";
this.txtFileType.Location = new System.Drawing.Point(173,71);
this.txtFileType.Name = "txtFileType";
this.txtFileType.Size = new System.Drawing.Size(100, 21);
this.txtFileType.TabIndex = 3;
this.Controls.Add(this.lblFileType);
this.Controls.Add(this.txtFileType);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblFileName );
this.Controls.Add(this.txtFileName );

                this.Controls.Add(this.lblFileType );
this.Controls.Add(this.txtFileType );

                    
            this.Name = "tb_CustomerVendorFilesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFileName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFileType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFileType;

    
    
   
 





    }
}


