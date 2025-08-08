// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程图定义
    /// </summary>
    partial class tb_FlowchartDefinitionEdit
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
     this.lblModuleID = new Krypton.Toolkit.KryptonLabel();
this.cmbModuleID = new Krypton.Toolkit.KryptonComboBox();

this.lblFlowchartNo = new Krypton.Toolkit.KryptonLabel();
this.txtFlowchartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblFlowchartName = new Krypton.Toolkit.KryptonLabel();
this.txtFlowchartName = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ModuleID###Int64
//属性测试25ModuleID
this.lblModuleID.AutoSize = true;
this.lblModuleID.Location = new System.Drawing.Point(100,25);
this.lblModuleID.Name = "lblModuleID";
this.lblModuleID.Size = new System.Drawing.Size(41, 12);
this.lblModuleID.TabIndex = 1;
this.lblModuleID.Text = "模块";
//111======25
this.cmbModuleID.Location = new System.Drawing.Point(173,21);
this.cmbModuleID.Name ="cmbModuleID";
this.cmbModuleID.Size = new System.Drawing.Size(100, 21);
this.cmbModuleID.TabIndex = 1;
this.Controls.Add(this.lblModuleID);
this.Controls.Add(this.cmbModuleID);

           //#####50FlowchartNo###String
this.lblFlowchartNo.AutoSize = true;
this.lblFlowchartNo.Location = new System.Drawing.Point(100,50);
this.lblFlowchartNo.Name = "lblFlowchartNo";
this.lblFlowchartNo.Size = new System.Drawing.Size(41, 12);
this.lblFlowchartNo.TabIndex = 2;
this.lblFlowchartNo.Text = "流程图编号";
this.txtFlowchartNo.Location = new System.Drawing.Point(173,46);
this.txtFlowchartNo.Name = "txtFlowchartNo";
this.txtFlowchartNo.Size = new System.Drawing.Size(100, 21);
this.txtFlowchartNo.TabIndex = 2;
this.Controls.Add(this.lblFlowchartNo);
this.Controls.Add(this.txtFlowchartNo);

           //#####20FlowchartName###String
this.lblFlowchartName.AutoSize = true;
this.lblFlowchartName.Location = new System.Drawing.Point(100,75);
this.lblFlowchartName.Name = "lblFlowchartName";
this.lblFlowchartName.Size = new System.Drawing.Size(41, 12);
this.lblFlowchartName.TabIndex = 3;
this.lblFlowchartName.Text = "流程图名称";
this.txtFlowchartName.Location = new System.Drawing.Point(173,71);
this.txtFlowchartName.Name = "txtFlowchartName";
this.txtFlowchartName.Size = new System.Drawing.Size(100, 21);
this.txtFlowchartName.TabIndex = 3;
this.Controls.Add(this.lblFlowchartName);
this.Controls.Add(this.txtFlowchartName);

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
                this.Controls.Add(this.lblModuleID );
this.Controls.Add(this.cmbModuleID );

                this.Controls.Add(this.lblFlowchartNo );
this.Controls.Add(this.txtFlowchartNo );

                this.Controls.Add(this.lblFlowchartName );
this.Controls.Add(this.txtFlowchartName );

                            // 
            // "tb_FlowchartDefinitionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FlowchartDefinitionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblModuleID;
private Krypton.Toolkit.KryptonComboBox cmbModuleID;

    
        
              private Krypton.Toolkit.KryptonLabel lblFlowchartNo;
private Krypton.Toolkit.KryptonTextBox txtFlowchartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblFlowchartName;
private Krypton.Toolkit.KryptonTextBox txtFlowchartName;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

