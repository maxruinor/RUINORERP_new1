// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 
    /// </summary>
    partial class tb_ApprovalEdit
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
     this.lblBillType = new Krypton.Toolkit.KryptonLabel();
this.txtBillType = new Krypton.Toolkit.KryptonTextBox();

this.lblBillName = new Krypton.Toolkit.KryptonLabel();
this.txtBillName = new Krypton.Toolkit.KryptonTextBox();

this.lblBillEntityClassName = new Krypton.Toolkit.KryptonLabel();
this.txtBillEntityClassName = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalResults = new Krypton.Toolkit.KryptonTextBox();

this.lblGradedAudit = new Krypton.Toolkit.KryptonLabel();
this.chkGradedAudit = new Krypton.Toolkit.KryptonCheckBox();
this.chkGradedAudit.Values.Text ="";

this.lblModule = new Krypton.Toolkit.KryptonLabel();
this.txtModule = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50BillType###String
this.lblBillType.AutoSize = true;
this.lblBillType.Location = new System.Drawing.Point(100,25);
this.lblBillType.Name = "lblBillType";
this.lblBillType.Size = new System.Drawing.Size(41, 12);
this.lblBillType.TabIndex = 1;
this.lblBillType.Text = "单据类型";
this.txtBillType.Location = new System.Drawing.Point(173,21);
this.txtBillType.Name = "txtBillType";
this.txtBillType.Size = new System.Drawing.Size(100, 21);
this.txtBillType.TabIndex = 1;
this.Controls.Add(this.lblBillType);
this.Controls.Add(this.txtBillType);

           //#####100BillName###String
this.lblBillName.AutoSize = true;
this.lblBillName.Location = new System.Drawing.Point(100,50);
this.lblBillName.Name = "lblBillName";
this.lblBillName.Size = new System.Drawing.Size(41, 12);
this.lblBillName.TabIndex = 2;
this.lblBillName.Text = "单据名称";
this.txtBillName.Location = new System.Drawing.Point(173,46);
this.txtBillName.Name = "txtBillName";
this.txtBillName.Size = new System.Drawing.Size(100, 21);
this.txtBillName.TabIndex = 2;
this.Controls.Add(this.lblBillName);
this.Controls.Add(this.txtBillName);

           //#####50BillEntityClassName###String
this.lblBillEntityClassName.AutoSize = true;
this.lblBillEntityClassName.Location = new System.Drawing.Point(100,75);
this.lblBillEntityClassName.Name = "lblBillEntityClassName";
this.lblBillEntityClassName.Size = new System.Drawing.Size(41, 12);
this.lblBillEntityClassName.TabIndex = 3;
this.lblBillEntityClassName.Text = "";
this.txtBillEntityClassName.Location = new System.Drawing.Point(173,71);
this.txtBillEntityClassName.Name = "txtBillEntityClassName";
this.txtBillEntityClassName.Size = new System.Drawing.Size(100, 21);
this.txtBillEntityClassName.TabIndex = 3;
this.Controls.Add(this.lblBillEntityClassName);
this.Controls.Add(this.txtBillEntityClassName);

           //#####ApprovalResults###Int32
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,100);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 4;
this.lblApprovalResults.Text = "审核结果";
this.txtApprovalResults.Location = new System.Drawing.Point(173,96);
this.txtApprovalResults.Name = "txtApprovalResults";
this.txtApprovalResults.Size = new System.Drawing.Size(100, 21);
this.txtApprovalResults.TabIndex = 4;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.txtApprovalResults);

           //#####GradedAudit###Boolean
this.lblGradedAudit.AutoSize = true;
this.lblGradedAudit.Location = new System.Drawing.Point(100,125);
this.lblGradedAudit.Name = "lblGradedAudit";
this.lblGradedAudit.Size = new System.Drawing.Size(41, 12);
this.lblGradedAudit.TabIndex = 5;
this.lblGradedAudit.Text = "分级审核";
this.chkGradedAudit.Location = new System.Drawing.Point(173,121);
this.chkGradedAudit.Name = "chkGradedAudit";
this.chkGradedAudit.Size = new System.Drawing.Size(100, 21);
this.chkGradedAudit.TabIndex = 5;
this.Controls.Add(this.lblGradedAudit);
this.Controls.Add(this.chkGradedAudit);

           //#####Module###Int32
this.lblModule.AutoSize = true;
this.lblModule.Location = new System.Drawing.Point(100,150);
this.lblModule.Name = "lblModule";
this.lblModule.Size = new System.Drawing.Size(41, 12);
this.lblModule.TabIndex = 6;
this.lblModule.Text = "程序模块";
this.txtModule.Location = new System.Drawing.Point(173,146);
this.txtModule.Name = "txtModule";
this.txtModule.Size = new System.Drawing.Size(100, 21);
this.txtModule.TabIndex = 6;
this.Controls.Add(this.lblModule);
this.Controls.Add(this.txtModule);

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
           // this.kryptonPanel1.TabIndex = 6;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBillType );
this.Controls.Add(this.txtBillType );

                this.Controls.Add(this.lblBillName );
this.Controls.Add(this.txtBillName );

                this.Controls.Add(this.lblBillEntityClassName );
this.Controls.Add(this.txtBillEntityClassName );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.txtApprovalResults );

                this.Controls.Add(this.lblGradedAudit );
this.Controls.Add(this.chkGradedAudit );

                this.Controls.Add(this.lblModule );
this.Controls.Add(this.txtModule );

                            // 
            // "tb_ApprovalEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ApprovalEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblBillType;
private Krypton.Toolkit.KryptonTextBox txtBillType;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillName;
private Krypton.Toolkit.KryptonTextBox txtBillName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillEntityClassName;
private Krypton.Toolkit.KryptonTextBox txtBillEntityClassName;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonTextBox txtApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblGradedAudit;
private Krypton.Toolkit.KryptonCheckBox chkGradedAudit;

    
        
              private Krypton.Toolkit.KryptonLabel lblModule;
private Krypton.Toolkit.KryptonTextBox txtModule;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

