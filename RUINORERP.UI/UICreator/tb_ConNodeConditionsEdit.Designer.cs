// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    partial class tb_ConNodeConditionsEdit
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
     this.lblField = new Krypton.Toolkit.KryptonLabel();
this.txtField = new Krypton.Toolkit.KryptonTextBox();

this.lblOperator = new Krypton.Toolkit.KryptonLabel();
this.txtOperator = new Krypton.Toolkit.KryptonTextBox();

this.lblValue = new Krypton.Toolkit.KryptonLabel();
this.txtValue = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50Field###String
this.lblField.AutoSize = true;
this.lblField.Location = new System.Drawing.Point(100,25);
this.lblField.Name = "lblField";
this.lblField.Size = new System.Drawing.Size(41, 12);
this.lblField.TabIndex = 1;
this.lblField.Text = "表达式";
this.txtField.Location = new System.Drawing.Point(173,21);
this.txtField.Name = "txtField";
this.txtField.Size = new System.Drawing.Size(100, 21);
this.txtField.TabIndex = 1;
this.Controls.Add(this.lblField);
this.Controls.Add(this.txtField);

           //#####50Operator###String
this.lblOperator.AutoSize = true;
this.lblOperator.Location = new System.Drawing.Point(100,50);
this.lblOperator.Name = "lblOperator";
this.lblOperator.Size = new System.Drawing.Size(41, 12);
this.lblOperator.TabIndex = 2;
this.lblOperator.Text = "操作符";
this.txtOperator.Location = new System.Drawing.Point(173,46);
this.txtOperator.Name = "txtOperator";
this.txtOperator.Size = new System.Drawing.Size(100, 21);
this.txtOperator.TabIndex = 2;
this.Controls.Add(this.lblOperator);
this.Controls.Add(this.txtOperator);

           //#####50Value###String
this.lblValue.AutoSize = true;
this.lblValue.Location = new System.Drawing.Point(100,75);
this.lblValue.Name = "lblValue";
this.lblValue.Size = new System.Drawing.Size(41, 12);
this.lblValue.TabIndex = 3;
this.lblValue.Text = "表达式值";
this.txtValue.Location = new System.Drawing.Point(173,71);
this.txtValue.Name = "txtValue";
this.txtValue.Size = new System.Drawing.Size(100, 21);
this.txtValue.TabIndex = 3;
this.Controls.Add(this.lblValue);
this.Controls.Add(this.txtValue);

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
                this.Controls.Add(this.lblField );
this.Controls.Add(this.txtField );

                this.Controls.Add(this.lblOperator );
this.Controls.Add(this.txtOperator );

                this.Controls.Add(this.lblValue );
this.Controls.Add(this.txtValue );

                            // 
            // "tb_ConNodeConditionsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ConNodeConditionsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblField;
private Krypton.Toolkit.KryptonTextBox txtField;

    
        
              private Krypton.Toolkit.KryptonLabel lblOperator;
private Krypton.Toolkit.KryptonTextBox txtOperator;

    
        
              private Krypton.Toolkit.KryptonLabel lblValue;
private Krypton.Toolkit.KryptonTextBox txtValue;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

