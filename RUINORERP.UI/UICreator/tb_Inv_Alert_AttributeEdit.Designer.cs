// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 存货预警特性表
    /// </summary>
    partial class tb_Inv_Alert_AttributeEdit
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
     this.lblInventory_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbInventory_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAlertPeriod = new Krypton.Toolkit.KryptonLabel();
this.txtAlertPeriod = new Krypton.Toolkit.KryptonTextBox();

this.lblMax_quantity = new Krypton.Toolkit.KryptonLabel();
this.txtMax_quantity = new Krypton.Toolkit.KryptonTextBox();

this.lblMin_quantity = new Krypton.Toolkit.KryptonLabel();
this.txtMin_quantity = new Krypton.Toolkit.KryptonTextBox();

this.lblAlert_Activation = new Krypton.Toolkit.KryptonLabel();
this.chkAlert_Activation = new Krypton.Toolkit.KryptonCheckBox();
this.chkAlert_Activation.Values.Text ="";

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Inventory_ID###Int64
//属性测试25Inventory_ID
this.lblInventory_ID.AutoSize = true;
this.lblInventory_ID.Location = new System.Drawing.Point(100,25);
this.lblInventory_ID.Name = "lblInventory_ID";
this.lblInventory_ID.Size = new System.Drawing.Size(41, 12);
this.lblInventory_ID.TabIndex = 1;
this.lblInventory_ID.Text = "库存";
//111======25
this.cmbInventory_ID.Location = new System.Drawing.Point(173,21);
this.cmbInventory_ID.Name ="cmbInventory_ID";
this.cmbInventory_ID.Size = new System.Drawing.Size(100, 21);
this.cmbInventory_ID.TabIndex = 1;
this.Controls.Add(this.lblInventory_ID);
this.Controls.Add(this.cmbInventory_ID);

           //#####AlertPeriod###Int32
//属性测试50AlertPeriod
this.lblAlertPeriod.AutoSize = true;
this.lblAlertPeriod.Location = new System.Drawing.Point(100,50);
this.lblAlertPeriod.Name = "lblAlertPeriod";
this.lblAlertPeriod.Size = new System.Drawing.Size(41, 12);
this.lblAlertPeriod.TabIndex = 2;
this.lblAlertPeriod.Text = "预警周期";
this.txtAlertPeriod.Location = new System.Drawing.Point(173,46);
this.txtAlertPeriod.Name = "txtAlertPeriod";
this.txtAlertPeriod.Size = new System.Drawing.Size(100, 21);
this.txtAlertPeriod.TabIndex = 2;
this.Controls.Add(this.lblAlertPeriod);
this.Controls.Add(this.txtAlertPeriod);

           //#####Max_quantity###Int32
//属性测试75Max_quantity
this.lblMax_quantity.AutoSize = true;
this.lblMax_quantity.Location = new System.Drawing.Point(100,75);
this.lblMax_quantity.Name = "lblMax_quantity";
this.lblMax_quantity.Size = new System.Drawing.Size(41, 12);
this.lblMax_quantity.TabIndex = 3;
this.lblMax_quantity.Text = "库存上限";
this.txtMax_quantity.Location = new System.Drawing.Point(173,71);
this.txtMax_quantity.Name = "txtMax_quantity";
this.txtMax_quantity.Size = new System.Drawing.Size(100, 21);
this.txtMax_quantity.TabIndex = 3;
this.Controls.Add(this.lblMax_quantity);
this.Controls.Add(this.txtMax_quantity);

           //#####Min_quantity###Int32
//属性测试100Min_quantity
this.lblMin_quantity.AutoSize = true;
this.lblMin_quantity.Location = new System.Drawing.Point(100,100);
this.lblMin_quantity.Name = "lblMin_quantity";
this.lblMin_quantity.Size = new System.Drawing.Size(41, 12);
this.lblMin_quantity.TabIndex = 4;
this.lblMin_quantity.Text = "库存下限";
this.txtMin_quantity.Location = new System.Drawing.Point(173,96);
this.txtMin_quantity.Name = "txtMin_quantity";
this.txtMin_quantity.Size = new System.Drawing.Size(100, 21);
this.txtMin_quantity.TabIndex = 4;
this.Controls.Add(this.lblMin_quantity);
this.Controls.Add(this.txtMin_quantity);

           //#####Alert_Activation###Boolean
this.lblAlert_Activation.AutoSize = true;
this.lblAlert_Activation.Location = new System.Drawing.Point(100,125);
this.lblAlert_Activation.Name = "lblAlert_Activation";
this.lblAlert_Activation.Size = new System.Drawing.Size(41, 12);
this.lblAlert_Activation.TabIndex = 5;
this.lblAlert_Activation.Text = "预警激活";
this.chkAlert_Activation.Location = new System.Drawing.Point(173,121);
this.chkAlert_Activation.Name = "chkAlert_Activation";
this.chkAlert_Activation.Size = new System.Drawing.Size(100, 21);
this.chkAlert_Activation.TabIndex = 5;
this.Controls.Add(this.lblAlert_Activation);
this.Controls.Add(this.chkAlert_Activation);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "创建时间";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试175Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,175);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 7;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,171);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 7;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,200);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 8;
this.lblModified_at.Text = "修改时间";
//111======200
this.dtpModified_at.Location = new System.Drawing.Point(173,196);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 8;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试225Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,225);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 9;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,221);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 9;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 9;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblInventory_ID );
this.Controls.Add(this.cmbInventory_ID );

                this.Controls.Add(this.lblAlertPeriod );
this.Controls.Add(this.txtAlertPeriod );

                this.Controls.Add(this.lblMax_quantity );
this.Controls.Add(this.txtMax_quantity );

                this.Controls.Add(this.lblMin_quantity );
this.Controls.Add(this.txtMin_quantity );

                this.Controls.Add(this.lblAlert_Activation );
this.Controls.Add(this.chkAlert_Activation );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_Inv_Alert_AttributeEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_Inv_Alert_AttributeEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblInventory_ID;
private Krypton.Toolkit.KryptonComboBox cmbInventory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlertPeriod;
private Krypton.Toolkit.KryptonTextBox txtAlertPeriod;

    
        
              private Krypton.Toolkit.KryptonLabel lblMax_quantity;
private Krypton.Toolkit.KryptonTextBox txtMax_quantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblMin_quantity;
private Krypton.Toolkit.KryptonTextBox txtMin_quantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlert_Activation;
private Krypton.Toolkit.KryptonCheckBox chkAlert_Activation;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

