// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 质检表
    /// </summary>
    partial class tb_QualityInspectionEdit
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
     this.lblInspectionDate = new Krypton.Toolkit.KryptonLabel();
this.dtpInspectionDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblInspectionResult = new Krypton.Toolkit.KryptonLabel();
this.txtInspectionResult = new Krypton.Toolkit.KryptonTextBox();
this.txtInspectionResult.Multiline = true;

this.lblProductID = new Krypton.Toolkit.KryptonLabel();
this.txtProductID = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####InspectionDate###DateTime
this.lblInspectionDate.AutoSize = true;
this.lblInspectionDate.Location = new System.Drawing.Point(100,25);
this.lblInspectionDate.Name = "lblInspectionDate";
this.lblInspectionDate.Size = new System.Drawing.Size(41, 12);
this.lblInspectionDate.TabIndex = 1;
this.lblInspectionDate.Text = "";
//111======25
this.dtpInspectionDate.Location = new System.Drawing.Point(173,21);
this.dtpInspectionDate.Name ="dtpInspectionDate";
this.dtpInspectionDate.ShowCheckBox =true;
this.dtpInspectionDate.Size = new System.Drawing.Size(100, 21);
this.dtpInspectionDate.TabIndex = 1;
this.Controls.Add(this.lblInspectionDate);
this.Controls.Add(this.dtpInspectionDate);

           //#####500InspectionResult###String
this.lblInspectionResult.AutoSize = true;
this.lblInspectionResult.Location = new System.Drawing.Point(100,50);
this.lblInspectionResult.Name = "lblInspectionResult";
this.lblInspectionResult.Size = new System.Drawing.Size(41, 12);
this.lblInspectionResult.TabIndex = 2;
this.lblInspectionResult.Text = "";
this.txtInspectionResult.Location = new System.Drawing.Point(173,46);
this.txtInspectionResult.Name = "txtInspectionResult";
this.txtInspectionResult.Size = new System.Drawing.Size(100, 21);
this.txtInspectionResult.TabIndex = 2;
this.Controls.Add(this.lblInspectionResult);
this.Controls.Add(this.txtInspectionResult);

           //#####ProductID###Int32
this.lblProductID.AutoSize = true;
this.lblProductID.Location = new System.Drawing.Point(100,75);
this.lblProductID.Name = "lblProductID";
this.lblProductID.Size = new System.Drawing.Size(41, 12);
this.lblProductID.TabIndex = 3;
this.lblProductID.Text = "";
this.txtProductID.Location = new System.Drawing.Point(173,71);
this.txtProductID.Name = "txtProductID";
this.txtProductID.Size = new System.Drawing.Size(100, 21);
this.txtProductID.TabIndex = 3;
this.Controls.Add(this.lblProductID);
this.Controls.Add(this.txtProductID);

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
                this.Controls.Add(this.lblInspectionDate );
this.Controls.Add(this.dtpInspectionDate );

                this.Controls.Add(this.lblInspectionResult );
this.Controls.Add(this.txtInspectionResult );

                this.Controls.Add(this.lblProductID );
this.Controls.Add(this.txtProductID );

                            // 
            // "tb_QualityInspectionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_QualityInspectionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblInspectionDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpInspectionDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblInspectionResult;
private Krypton.Toolkit.KryptonTextBox txtInspectionResult;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductID;
private Krypton.Toolkit.KryptonTextBox txtProductID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

