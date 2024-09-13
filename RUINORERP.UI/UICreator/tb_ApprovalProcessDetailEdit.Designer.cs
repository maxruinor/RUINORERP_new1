// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 审核流程明细表
    /// </summary>
    partial class tb_ApprovalProcessDetailEdit
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
     this.lblApprovalID = new Krypton.Toolkit.KryptonLabel();
this.cmbApprovalID = new Krypton.Toolkit.KryptonComboBox();

this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalResults = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOrder = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOrder = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ApprovalID###Int64
//属性测试25ApprovalID
this.lblApprovalID.AutoSize = true;
this.lblApprovalID.Location = new System.Drawing.Point(100,25);
this.lblApprovalID.Name = "lblApprovalID";
this.lblApprovalID.Size = new System.Drawing.Size(41, 12);
this.lblApprovalID.TabIndex = 1;
this.lblApprovalID.Text = "";
//111======25
this.cmbApprovalID.Location = new System.Drawing.Point(173,21);
this.cmbApprovalID.Name ="cmbApprovalID";
this.cmbApprovalID.Size = new System.Drawing.Size(100, 21);
this.cmbApprovalID.TabIndex = 1;
this.Controls.Add(this.lblApprovalID);
this.Controls.Add(this.cmbApprovalID);

           //#####ApprovalResults###Int32
//属性测试50ApprovalResults
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,50);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 2;
this.lblApprovalResults.Text = "审核结果";
this.txtApprovalResults.Location = new System.Drawing.Point(173,46);
this.txtApprovalResults.Name = "txtApprovalResults";
this.txtApprovalResults.Size = new System.Drawing.Size(100, 21);
this.txtApprovalResults.TabIndex = 2;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.txtApprovalResults);

           //#####ApprovalOrder###Int32
//属性测试75ApprovalOrder
this.lblApprovalOrder.AutoSize = true;
this.lblApprovalOrder.Location = new System.Drawing.Point(100,75);
this.lblApprovalOrder.Name = "lblApprovalOrder";
this.lblApprovalOrder.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOrder.TabIndex = 3;
this.lblApprovalOrder.Text = "审核顺序";
this.txtApprovalOrder.Location = new System.Drawing.Point(173,71);
this.txtApprovalOrder.Name = "txtApprovalOrder";
this.txtApprovalOrder.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOrder.TabIndex = 3;
this.Controls.Add(this.lblApprovalOrder);
this.Controls.Add(this.txtApprovalOrder);

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
                this.Controls.Add(this.lblApprovalID );
this.Controls.Add(this.cmbApprovalID );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.txtApprovalResults );

                this.Controls.Add(this.lblApprovalOrder );
this.Controls.Add(this.txtApprovalOrder );

                            // 
            // "tb_ApprovalProcessDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ApprovalProcessDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblApprovalID;
private Krypton.Toolkit.KryptonComboBox cmbApprovalID;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonTextBox txtApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOrder;
private Krypton.Toolkit.KryptonTextBox txtApprovalOrder;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

