// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程步骤 转移条件集合
    /// </summary>
    partial class tb_NextNodesEdit
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
     this.lblConNodeConditions_Id = new Krypton.Toolkit.KryptonLabel();
this.cmbConNodeConditions_Id = new Krypton.Toolkit.KryptonComboBox();

this.lblNexNodeName = new Krypton.Toolkit.KryptonLabel();
this.txtNexNodeName = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ConNodeConditions_Id###Int64
//属性测试25ConNodeConditions_Id
this.lblConNodeConditions_Id.AutoSize = true;
this.lblConNodeConditions_Id.Location = new System.Drawing.Point(100,25);
this.lblConNodeConditions_Id.Name = "lblConNodeConditions_Id";
this.lblConNodeConditions_Id.Size = new System.Drawing.Size(41, 12);
this.lblConNodeConditions_Id.TabIndex = 1;
this.lblConNodeConditions_Id.Text = "条件";
//111======25
this.cmbConNodeConditions_Id.Location = new System.Drawing.Point(173,21);
this.cmbConNodeConditions_Id.Name ="cmbConNodeConditions_Id";
this.cmbConNodeConditions_Id.Size = new System.Drawing.Size(100, 21);
this.cmbConNodeConditions_Id.TabIndex = 1;
this.Controls.Add(this.lblConNodeConditions_Id);
this.Controls.Add(this.cmbConNodeConditions_Id);

           //#####50NexNodeName###String
this.lblNexNodeName.AutoSize = true;
this.lblNexNodeName.Location = new System.Drawing.Point(100,50);
this.lblNexNodeName.Name = "lblNexNodeName";
this.lblNexNodeName.Size = new System.Drawing.Size(41, 12);
this.lblNexNodeName.TabIndex = 2;
this.lblNexNodeName.Text = "下节点名称";
this.txtNexNodeName.Location = new System.Drawing.Point(173,46);
this.txtNexNodeName.Name = "txtNexNodeName";
this.txtNexNodeName.Size = new System.Drawing.Size(100, 21);
this.txtNexNodeName.TabIndex = 2;
this.Controls.Add(this.lblNexNodeName);
this.Controls.Add(this.txtNexNodeName);

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
           // this.kryptonPanel1.TabIndex = 2;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblConNodeConditions_Id );
this.Controls.Add(this.cmbConNodeConditions_Id );

                this.Controls.Add(this.lblNexNodeName );
this.Controls.Add(this.txtNexNodeName );

                            // 
            // "tb_NextNodesEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_NextNodesEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblConNodeConditions_Id;
private Krypton.Toolkit.KryptonComboBox cmbConNodeConditions_Id;

    
        
              private Krypton.Toolkit.KryptonLabel lblNexNodeName;
private Krypton.Toolkit.KryptonTextBox txtNexNodeName;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

