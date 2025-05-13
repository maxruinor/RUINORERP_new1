// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/13/2025 22:52:39
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 往来单位类型,如级别，电商，大客户，亚马逊等
    /// </summary>
    partial class tb_CustomerVendorTypeEdit
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
     this.lblTypeName = new Krypton.Toolkit.KryptonLabel();
this.txtTypeName = new Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new Krypton.Toolkit.KryptonLabel();
this.txtDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblBusinessPartnerType = new Krypton.Toolkit.KryptonLabel();
this.txtBusinessPartnerType = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50TypeName###String
this.lblTypeName.AutoSize = true;
this.lblTypeName.Location = new System.Drawing.Point(100,25);
this.lblTypeName.Name = "lblTypeName";
this.lblTypeName.Size = new System.Drawing.Size(41, 12);
this.lblTypeName.TabIndex = 1;
this.lblTypeName.Text = "类型等级名称";
this.txtTypeName.Location = new System.Drawing.Point(173,21);
this.txtTypeName.Name = "txtTypeName";
this.txtTypeName.Size = new System.Drawing.Size(100, 21);
this.txtTypeName.TabIndex = 1;
this.Controls.Add(this.lblTypeName);
this.Controls.Add(this.txtTypeName);

           //#####100Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,50);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 2;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,46);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 2;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

           //#####BusinessPartnerType###Int32
this.lblBusinessPartnerType.AutoSize = true;
this.lblBusinessPartnerType.Location = new System.Drawing.Point(100,75);
this.lblBusinessPartnerType.Name = "lblBusinessPartnerType";
this.lblBusinessPartnerType.Size = new System.Drawing.Size(41, 12);
this.lblBusinessPartnerType.TabIndex = 3;
this.lblBusinessPartnerType.Text = "";
this.txtBusinessPartnerType.Location = new System.Drawing.Point(173,71);
this.txtBusinessPartnerType.Name = "txtBusinessPartnerType";
this.txtBusinessPartnerType.Size = new System.Drawing.Size(100, 21);
this.txtBusinessPartnerType.TabIndex = 3;
this.Controls.Add(this.lblBusinessPartnerType);
this.Controls.Add(this.txtBusinessPartnerType);

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
           // this.kryptonPanel1.TabIndex = 3;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblTypeName );
this.Controls.Add(this.txtTypeName );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                this.Controls.Add(this.lblBusinessPartnerType );
this.Controls.Add(this.txtBusinessPartnerType );

                            // 
            // "tb_CustomerVendorTypeEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CustomerVendorTypeEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblTypeName;
private Krypton.Toolkit.KryptonTextBox txtTypeName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDesc;
private Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessPartnerType;
private Krypton.Toolkit.KryptonTextBox txtBusinessPartnerType;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

