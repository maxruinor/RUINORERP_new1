// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 可视化排程
    /// </summary>
    partial class tb_ProduceViewScheduleEdit
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
     this.lblproduct_id = new Krypton.Toolkit.KryptonLabel();
this.txtproduct_id = new Krypton.Toolkit.KryptonTextBox();

this.lblquantity = new Krypton.Toolkit.KryptonLabel();
this.txtquantity = new Krypton.Toolkit.KryptonTextBox();

this.lblstart_date = new Krypton.Toolkit.KryptonLabel();
this.dtpstart_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblend_date = new Krypton.Toolkit.KryptonLabel();
this.dtpend_date = new Krypton.Toolkit.KryptonDateTimePicker();

    
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
     
            //#####product_id###Int32
this.lblproduct_id.AutoSize = true;
this.lblproduct_id.Location = new System.Drawing.Point(100,25);
this.lblproduct_id.Name = "lblproduct_id";
this.lblproduct_id.Size = new System.Drawing.Size(41, 12);
this.lblproduct_id.TabIndex = 1;
this.lblproduct_id.Text = "";
this.txtproduct_id.Location = new System.Drawing.Point(173,21);
this.txtproduct_id.Name = "txtproduct_id";
this.txtproduct_id.Size = new System.Drawing.Size(100, 21);
this.txtproduct_id.TabIndex = 1;
this.Controls.Add(this.lblproduct_id);
this.Controls.Add(this.txtproduct_id);

           //#####quantity###Int32
this.lblquantity.AutoSize = true;
this.lblquantity.Location = new System.Drawing.Point(100,50);
this.lblquantity.Name = "lblquantity";
this.lblquantity.Size = new System.Drawing.Size(41, 12);
this.lblquantity.TabIndex = 2;
this.lblquantity.Text = "生产数量";
this.txtquantity.Location = new System.Drawing.Point(173,46);
this.txtquantity.Name = "txtquantity";
this.txtquantity.Size = new System.Drawing.Size(100, 21);
this.txtquantity.TabIndex = 2;
this.Controls.Add(this.lblquantity);
this.Controls.Add(this.txtquantity);

           //#####start_date###DateTime
this.lblstart_date.AutoSize = true;
this.lblstart_date.Location = new System.Drawing.Point(100,75);
this.lblstart_date.Name = "lblstart_date";
this.lblstart_date.Size = new System.Drawing.Size(41, 12);
this.lblstart_date.TabIndex = 3;
this.lblstart_date.Text = "计划开始日期";
//111======75
this.dtpstart_date.Location = new System.Drawing.Point(173,71);
this.dtpstart_date.Name ="dtpstart_date";
this.dtpstart_date.ShowCheckBox =true;
this.dtpstart_date.Size = new System.Drawing.Size(100, 21);
this.dtpstart_date.TabIndex = 3;
this.Controls.Add(this.lblstart_date);
this.Controls.Add(this.dtpstart_date);

           //#####end_date###DateTime
this.lblend_date.AutoSize = true;
this.lblend_date.Location = new System.Drawing.Point(100,100);
this.lblend_date.Name = "lblend_date";
this.lblend_date.Size = new System.Drawing.Size(41, 12);
this.lblend_date.TabIndex = 4;
this.lblend_date.Text = "计划完成日期";
//111======100
this.dtpend_date.Location = new System.Drawing.Point(173,96);
this.dtpend_date.Name ="dtpend_date";
this.dtpend_date.ShowCheckBox =true;
this.dtpend_date.Size = new System.Drawing.Size(100, 21);
this.dtpend_date.TabIndex = 4;
this.Controls.Add(this.lblend_date);
this.Controls.Add(this.dtpend_date);

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
                this.Controls.Add(this.lblproduct_id );
this.Controls.Add(this.txtproduct_id );

                this.Controls.Add(this.lblquantity );
this.Controls.Add(this.txtquantity );

                this.Controls.Add(this.lblstart_date );
this.Controls.Add(this.dtpstart_date );

                this.Controls.Add(this.lblend_date );
this.Controls.Add(this.dtpend_date );

                            // 
            // "tb_ProduceViewScheduleEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProduceViewScheduleEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblproduct_id;
private Krypton.Toolkit.KryptonTextBox txtproduct_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblquantity;
private Krypton.Toolkit.KryptonTextBox txtquantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblstart_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpstart_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblend_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpend_date;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

