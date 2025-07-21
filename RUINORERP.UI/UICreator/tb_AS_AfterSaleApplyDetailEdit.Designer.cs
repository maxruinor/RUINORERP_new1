// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后申请单明细
    /// </summary>
    partial class tb_AS_AfterSaleApplyDetailEdit
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
     this.lblASApplyID = new Krypton.Toolkit.KryptonLabel();
this.cmbASApplyID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblFaultDescription = new Krypton.Toolkit.KryptonLabel();
this.txtFaultDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtFaultDescription.Multiline = true;

this.lblInitialQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtInitialQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblConfirmedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtConfirmedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
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
     
            //#####ASApplyID###Int64
//属性测试25ASApplyID
this.lblASApplyID.AutoSize = true;
this.lblASApplyID.Location = new System.Drawing.Point(100,25);
this.lblASApplyID.Name = "lblASApplyID";
this.lblASApplyID.Size = new System.Drawing.Size(41, 12);
this.lblASApplyID.TabIndex = 1;
this.lblASApplyID.Text = "售后申请单";
//111======25
this.cmbASApplyID.Location = new System.Drawing.Point(173,21);
this.cmbASApplyID.Name ="cmbASApplyID";
this.cmbASApplyID.Size = new System.Drawing.Size(100, 21);
this.cmbASApplyID.TabIndex = 1;
this.Controls.Add(this.lblASApplyID);
this.Controls.Add(this.cmbASApplyID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,75);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 3;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,71);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 3;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
//属性测试100Location_ID
//属性测试100Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,100);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 4;
this.lblLocation_ID.Text = "库位";
//111======100
this.cmbLocation_ID.Location = new System.Drawing.Point(173,96);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 4;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####500FaultDescription###String
this.lblFaultDescription.AutoSize = true;
this.lblFaultDescription.Location = new System.Drawing.Point(100,125);
this.lblFaultDescription.Name = "lblFaultDescription";
this.lblFaultDescription.Size = new System.Drawing.Size(41, 12);
this.lblFaultDescription.TabIndex = 5;
this.lblFaultDescription.Text = "问题描述";
this.txtFaultDescription.Location = new System.Drawing.Point(173,121);
this.txtFaultDescription.Name = "txtFaultDescription";
this.txtFaultDescription.Size = new System.Drawing.Size(100, 21);
this.txtFaultDescription.TabIndex = 5;
this.Controls.Add(this.lblFaultDescription);
this.Controls.Add(this.txtFaultDescription);

           //#####InitialQuantity###Int32
//属性测试150InitialQuantity
//属性测试150InitialQuantity
//属性测试150InitialQuantity
this.lblInitialQuantity.AutoSize = true;
this.lblInitialQuantity.Location = new System.Drawing.Point(100,150);
this.lblInitialQuantity.Name = "lblInitialQuantity";
this.lblInitialQuantity.Size = new System.Drawing.Size(41, 12);
this.lblInitialQuantity.TabIndex = 6;
this.lblInitialQuantity.Text = "客户申报数量";
this.txtInitialQuantity.Location = new System.Drawing.Point(173,146);
this.txtInitialQuantity.Name = "txtInitialQuantity";
this.txtInitialQuantity.Size = new System.Drawing.Size(100, 21);
this.txtInitialQuantity.TabIndex = 6;
this.Controls.Add(this.lblInitialQuantity);
this.Controls.Add(this.txtInitialQuantity);

           //#####ConfirmedQuantity###Int32
//属性测试175ConfirmedQuantity
//属性测试175ConfirmedQuantity
//属性测试175ConfirmedQuantity
this.lblConfirmedQuantity.AutoSize = true;
this.lblConfirmedQuantity.Location = new System.Drawing.Point(100,175);
this.lblConfirmedQuantity.Name = "lblConfirmedQuantity";
this.lblConfirmedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblConfirmedQuantity.TabIndex = 7;
this.lblConfirmedQuantity.Text = "复核数量";
this.txtConfirmedQuantity.Location = new System.Drawing.Point(173,171);
this.txtConfirmedQuantity.Name = "txtConfirmedQuantity";
this.txtConfirmedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtConfirmedQuantity.TabIndex = 7;
this.Controls.Add(this.lblConfirmedQuantity);
this.Controls.Add(this.txtConfirmedQuantity);

           //#####100CustomerPartNo###String
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

           //#####DeliveredQty###Int32
//属性测试225DeliveredQty
//属性测试225DeliveredQty
//属性测试225DeliveredQty
this.lblDeliveredQty.AutoSize = true;
this.lblDeliveredQty.Location = new System.Drawing.Point(100,225);
this.lblDeliveredQty.Name = "lblDeliveredQty";
this.lblDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblDeliveredQty.TabIndex = 9;
this.lblDeliveredQty.Text = "交付数量";
this.txtDeliveredQty.Location = new System.Drawing.Point(173,221);
this.txtDeliveredQty.Name = "txtDeliveredQty";
this.txtDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtDeliveredQty.TabIndex = 9;
this.Controls.Add(this.lblDeliveredQty);
this.Controls.Add(this.txtDeliveredQty);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,250);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 10;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,246);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 10;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

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
           // this.kryptonPanel1.TabIndex = 10;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblASApplyID );
this.Controls.Add(this.cmbASApplyID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblFaultDescription );
this.Controls.Add(this.txtFaultDescription );

                this.Controls.Add(this.lblInitialQuantity );
this.Controls.Add(this.txtInitialQuantity );

                this.Controls.Add(this.lblConfirmedQuantity );
this.Controls.Add(this.txtConfirmedQuantity );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblDeliveredQty );
this.Controls.Add(this.txtDeliveredQty );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_AS_AfterSaleApplyDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_AS_AfterSaleApplyDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblASApplyID;
private Krypton.Toolkit.KryptonComboBox cmbASApplyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblFaultDescription;
private Krypton.Toolkit.KryptonTextBox txtFaultDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblInitialQuantity;
private Krypton.Toolkit.KryptonTextBox txtInitialQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblConfirmedQuantity;
private Krypton.Toolkit.KryptonTextBox txtConfirmedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

