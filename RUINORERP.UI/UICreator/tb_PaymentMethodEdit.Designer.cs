// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:48
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    partial class tb_PaymentMethodEdit
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
     this.lblPaytype_Name = new Krypton.Toolkit.KryptonLabel();
this.txtPaytype_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new Krypton.Toolkit.KryptonLabel();
this.txtDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblCash = new Krypton.Toolkit.KryptonLabel();
this.chkCash = new Krypton.Toolkit.KryptonCheckBox();
this.chkCash.Values.Text ="";

    
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
     
            //#####50Paytype_Name###String
this.lblPaytype_Name.AutoSize = true;
this.lblPaytype_Name.Location = new System.Drawing.Point(100,25);
this.lblPaytype_Name.Name = "lblPaytype_Name";
this.lblPaytype_Name.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_Name.TabIndex = 1;
this.lblPaytype_Name.Text = "付款方式";
this.txtPaytype_Name.Location = new System.Drawing.Point(173,21);
this.txtPaytype_Name.Name = "txtPaytype_Name";
this.txtPaytype_Name.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_Name.TabIndex = 1;
this.Controls.Add(this.lblPaytype_Name);
this.Controls.Add(this.txtPaytype_Name);

           //#####50Desc###String
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

           //#####Sort###Int32
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,75);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 3;
this.lblSort.Text = "现金";
this.txtSort.Location = new System.Drawing.Point(173,71);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 3;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

           //#####Cash###Boolean
this.lblCash.AutoSize = true;
this.lblCash.Location = new System.Drawing.Point(100,100);
this.lblCash.Name = "lblCash";
this.lblCash.Size = new System.Drawing.Size(41, 12);
this.lblCash.TabIndex = 4;
this.lblCash.Text = "即时收付款";
this.chkCash.Location = new System.Drawing.Point(173,96);
this.chkCash.Name = "chkCash";
this.chkCash.Size = new System.Drawing.Size(100, 21);
this.chkCash.TabIndex = 4;
this.Controls.Add(this.lblCash);
this.Controls.Add(this.chkCash);

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
           // this.kryptonPanel1.TabIndex = 4;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaytype_Name );
this.Controls.Add(this.txtPaytype_Name );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblCash );
this.Controls.Add(this.chkCash );

                            // 
            // "tb_PaymentMethodEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PaymentMethodEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPaytype_Name;
private Krypton.Toolkit.KryptonTextBox txtPaytype_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblDesc;
private Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblCash;
private Krypton.Toolkit.KryptonCheckBox chkCash;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

