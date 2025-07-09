// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后交付明细
    /// </summary>
    partial class tb_AS_AfterSaleDeliveryDetailEdit
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
     this.lblASDeliveryID = new Krypton.Toolkit.KryptonLabel();
this.cmbASDeliveryID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblSaleFlagCode = new Krypton.Toolkit.KryptonLabel();
this.txtSaleFlagCode = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblASApplyDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtASApplyDetailID = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ASDeliveryID###Int64
//属性测试25ASDeliveryID
this.lblASDeliveryID.AutoSize = true;
this.lblASDeliveryID.Location = new System.Drawing.Point(100,25);
this.lblASDeliveryID.Name = "lblASDeliveryID";
this.lblASDeliveryID.Size = new System.Drawing.Size(41, 12);
this.lblASDeliveryID.TabIndex = 1;
this.lblASDeliveryID.Text = "售后交付单";
//111======25
this.cmbASDeliveryID.Location = new System.Drawing.Point(173,21);
this.cmbASDeliveryID.Name ="cmbASDeliveryID";
this.cmbASDeliveryID.Size = new System.Drawing.Size(100, 21);
this.cmbASDeliveryID.TabIndex = 1;
this.Controls.Add(this.lblASDeliveryID);
this.Controls.Add(this.cmbASDeliveryID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32
//属性测试125Quantity
//属性测试125Quantity
//属性测试125Quantity
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,125);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 5;
this.lblQuantity.Text = "数量";
this.txtQuantity.Location = new System.Drawing.Point(173,121);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 5;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####200SaleFlagCode###String
this.lblSaleFlagCode.AutoSize = true;
this.lblSaleFlagCode.Location = new System.Drawing.Point(100,150);
this.lblSaleFlagCode.Name = "lblSaleFlagCode";
this.lblSaleFlagCode.Size = new System.Drawing.Size(41, 12);
this.lblSaleFlagCode.TabIndex = 6;
this.lblSaleFlagCode.Text = "标识代码";
this.txtSaleFlagCode.Location = new System.Drawing.Point(173,146);
this.txtSaleFlagCode.Name = "txtSaleFlagCode";
this.txtSaleFlagCode.Size = new System.Drawing.Size(100, 21);
this.txtSaleFlagCode.TabIndex = 6;
this.Controls.Add(this.lblSaleFlagCode);
this.Controls.Add(this.txtSaleFlagCode);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,175);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 7;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,171);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 7;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,200);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 8;
this.lblCustomerPartNo.Text = "客户型号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,196);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 8;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####ASApplyDetailID###Int64
//属性测试225ASApplyDetailID
//属性测试225ASApplyDetailID
//属性测试225ASApplyDetailID
this.lblASApplyDetailID.AutoSize = true;
this.lblASApplyDetailID.Location = new System.Drawing.Point(100,225);
this.lblASApplyDetailID.Name = "lblASApplyDetailID";
this.lblASApplyDetailID.Size = new System.Drawing.Size(41, 12);
this.lblASApplyDetailID.TabIndex = 9;
this.lblASApplyDetailID.Text = "";
this.txtASApplyDetailID.Location = new System.Drawing.Point(173,221);
this.txtASApplyDetailID.Name = "txtASApplyDetailID";
this.txtASApplyDetailID.Size = new System.Drawing.Size(100, 21);
this.txtASApplyDetailID.TabIndex = 9;
this.Controls.Add(this.lblASApplyDetailID);
this.Controls.Add(this.txtASApplyDetailID);

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
                this.Controls.Add(this.lblASDeliveryID );
this.Controls.Add(this.cmbASDeliveryID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblSaleFlagCode );
this.Controls.Add(this.txtSaleFlagCode );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblASApplyDetailID );
this.Controls.Add(this.txtASApplyDetailID );

                            // 
            // "tb_AS_AfterSaleDeliveryDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_AS_AfterSaleDeliveryDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblASDeliveryID;
private Krypton.Toolkit.KryptonComboBox cmbASDeliveryID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleFlagCode;
private Krypton.Toolkit.KryptonTextBox txtSaleFlagCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblASApplyDetailID;
private Krypton.Toolkit.KryptonTextBox txtASApplyDetailID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

