// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:56
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 商品信息汇总
    /// </summary>
    partial class tb_ProdInfoSummaryEdit
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
     this.lbl平均价格 = new Krypton.Toolkit.KryptonLabel();
this.txt平均价格 = new Krypton.Toolkit.KryptonTextBox();

this.lbl总销售量 = new Krypton.Toolkit.KryptonLabel();
this.txt总销售量 = new Krypton.Toolkit.KryptonTextBox();

this.lbl库存总量 = new Krypton.Toolkit.KryptonLabel();
this.txt库存总量 = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####平均价格###Decimal
this.lbl平均价格.AutoSize = true;
this.lbl平均价格.Location = new System.Drawing.Point(100,25);
this.lbl平均价格.Name = "lbl平均价格";
this.lbl平均价格.Size = new System.Drawing.Size(41, 12);
this.lbl平均价格.TabIndex = 1;
this.lbl平均价格.Text = "平均价格";
//111======25
this.txt平均价格.Location = new System.Drawing.Point(173,21);
this.txt平均价格.Name ="txt平均价格";
this.txt平均价格.Size = new System.Drawing.Size(100, 21);
this.txt平均价格.TabIndex = 1;
this.Controls.Add(this.lbl平均价格);
this.Controls.Add(this.txt平均价格);

           //#####总销售量###Int32
this.lbl总销售量.AutoSize = true;
this.lbl总销售量.Location = new System.Drawing.Point(100,50);
this.lbl总销售量.Name = "lbl总销售量";
this.lbl总销售量.Size = new System.Drawing.Size(41, 12);
this.lbl总销售量.TabIndex = 2;
this.lbl总销售量.Text = "总销售量";
this.txt总销售量.Location = new System.Drawing.Point(173,46);
this.txt总销售量.Name = "txt总销售量";
this.txt总销售量.Size = new System.Drawing.Size(100, 21);
this.txt总销售量.TabIndex = 2;
this.Controls.Add(this.lbl总销售量);
this.Controls.Add(this.txt总销售量);

           //#####库存总量###Int32
this.lbl库存总量.AutoSize = true;
this.lbl库存总量.Location = new System.Drawing.Point(100,75);
this.lbl库存总量.Name = "lbl库存总量";
this.lbl库存总量.Size = new System.Drawing.Size(41, 12);
this.lbl库存总量.TabIndex = 3;
this.lbl库存总量.Text = "库存总量";
this.txt库存总量.Location = new System.Drawing.Point(173,71);
this.txt库存总量.Name = "txt库存总量";
this.txt库存总量.Size = new System.Drawing.Size(100, 21);
this.txt库存总量.TabIndex = 3;
this.Controls.Add(this.lbl库存总量);
this.Controls.Add(this.txt库存总量);

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
                this.Controls.Add(this.lbl平均价格 );
this.Controls.Add(this.txt平均价格 );

                this.Controls.Add(this.lbl总销售量 );
this.Controls.Add(this.txt总销售量 );

                this.Controls.Add(this.lbl库存总量 );
this.Controls.Add(this.txt库存总量 );

                            // 
            // "tb_ProdInfoSummaryEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdInfoSummaryEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lbl平均价格;
private Krypton.Toolkit.KryptonTextBox txt平均价格;

    
        
              private Krypton.Toolkit.KryptonLabel lbl总销售量;
private Krypton.Toolkit.KryptonTextBox txt总销售量;

    
        
              private Krypton.Toolkit.KryptonLabel lbl库存总量;
private Krypton.Toolkit.KryptonTextBox txt库存总量;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

