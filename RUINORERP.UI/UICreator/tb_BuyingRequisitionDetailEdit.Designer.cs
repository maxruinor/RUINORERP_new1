// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 请购单明细表
    /// </summary>
    partial class tb_BuyingRequisitionDetailEdit
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
     this.lblPuRequisition_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPuRequisition_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblRequirementDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblActualRequiredQty = new Krypton.Toolkit.KryptonLabel();
this.txtActualRequiredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblEstimatedPrice = new Krypton.Toolkit.KryptonLabel();
this.txtEstimatedPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveredQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtDeliveredQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblPurchased = new Krypton.Toolkit.KryptonLabel();
this.chkPurchased = new Krypton.Toolkit.KryptonCheckBox();
this.chkPurchased.Values.Text ="";

    
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
     
            //#####PuRequisition_ID###Int64
//属性测试25PuRequisition_ID
//属性测试25PuRequisition_ID
this.lblPuRequisition_ID.AutoSize = true;
this.lblPuRequisition_ID.Location = new System.Drawing.Point(100,25);
this.lblPuRequisition_ID.Name = "lblPuRequisition_ID";
this.lblPuRequisition_ID.Size = new System.Drawing.Size(41, 12);
this.lblPuRequisition_ID.TabIndex = 1;
this.lblPuRequisition_ID.Text = "请购单";
//111======25
this.cmbPuRequisition_ID.Location = new System.Drawing.Point(173,21);
this.cmbPuRequisition_ID.Name ="cmbPuRequisition_ID";
this.cmbPuRequisition_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPuRequisition_ID.TabIndex = 1;
this.Controls.Add(this.lblPuRequisition_ID);
this.Controls.Add(this.cmbPuRequisition_ID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,75);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 3;
this.lblRequirementDate.Text = "需求日期";
//111======75
this.dtpRequirementDate.Location = new System.Drawing.Point(173,71);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.ShowCheckBox =true;
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 3;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

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

           //#####ActualRequiredQty###Int32
//属性测试125ActualRequiredQty
//属性测试125ActualRequiredQty
this.lblActualRequiredQty.AutoSize = true;
this.lblActualRequiredQty.Location = new System.Drawing.Point(100,125);
this.lblActualRequiredQty.Name = "lblActualRequiredQty";
this.lblActualRequiredQty.Size = new System.Drawing.Size(41, 12);
this.lblActualRequiredQty.TabIndex = 5;
this.lblActualRequiredQty.Text = "需求数量";
this.txtActualRequiredQty.Location = new System.Drawing.Point(173,121);
this.txtActualRequiredQty.Name = "txtActualRequiredQty";
this.txtActualRequiredQty.Size = new System.Drawing.Size(100, 21);
this.txtActualRequiredQty.TabIndex = 5;
this.Controls.Add(this.lblActualRequiredQty);
this.Controls.Add(this.txtActualRequiredQty);

           //#####Quantity###Int32
//属性测试150Quantity
//属性测试150Quantity
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,150);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 6;
this.lblQuantity.Text = "请购数量";
this.txtQuantity.Location = new System.Drawing.Point(173,146);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 6;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####EstimatedPrice###Decimal
this.lblEstimatedPrice.AutoSize = true;
this.lblEstimatedPrice.Location = new System.Drawing.Point(100,175);
this.lblEstimatedPrice.Name = "lblEstimatedPrice";
this.lblEstimatedPrice.Size = new System.Drawing.Size(41, 12);
this.lblEstimatedPrice.TabIndex = 7;
this.lblEstimatedPrice.Text = "预估价格";
//111======175
this.txtEstimatedPrice.Location = new System.Drawing.Point(173,171);
this.txtEstimatedPrice.Name ="txtEstimatedPrice";
this.txtEstimatedPrice.Size = new System.Drawing.Size(100, 21);
this.txtEstimatedPrice.TabIndex = 7;
this.Controls.Add(this.lblEstimatedPrice);
this.Controls.Add(this.txtEstimatedPrice);

           //#####DeliveredQuantity###Int32
//属性测试200DeliveredQuantity
//属性测试200DeliveredQuantity
this.lblDeliveredQuantity.AutoSize = true;
this.lblDeliveredQuantity.Location = new System.Drawing.Point(100,200);
this.lblDeliveredQuantity.Name = "lblDeliveredQuantity";
this.lblDeliveredQuantity.Size = new System.Drawing.Size(41, 12);
this.lblDeliveredQuantity.TabIndex = 8;
this.lblDeliveredQuantity.Text = "已交数量";
this.txtDeliveredQuantity.Location = new System.Drawing.Point(173,196);
this.txtDeliveredQuantity.Name = "txtDeliveredQuantity";
this.txtDeliveredQuantity.Size = new System.Drawing.Size(100, 21);
this.txtDeliveredQuantity.TabIndex = 8;
this.Controls.Add(this.lblDeliveredQuantity);
this.Controls.Add(this.txtDeliveredQuantity);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Purchased###Boolean
this.lblPurchased.AutoSize = true;
this.lblPurchased.Location = new System.Drawing.Point(100,250);
this.lblPurchased.Name = "lblPurchased";
this.lblPurchased.Size = new System.Drawing.Size(41, 12);
this.lblPurchased.TabIndex = 10;
this.lblPurchased.Text = "已采购";
this.chkPurchased.Location = new System.Drawing.Point(173,246);
this.chkPurchased.Name = "chkPurchased";
this.chkPurchased.Size = new System.Drawing.Size(100, 21);
this.chkPurchased.TabIndex = 10;
this.Controls.Add(this.lblPurchased);
this.Controls.Add(this.chkPurchased);

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
                this.Controls.Add(this.lblPuRequisition_ID );
this.Controls.Add(this.cmbPuRequisition_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblActualRequiredQty );
this.Controls.Add(this.txtActualRequiredQty );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblEstimatedPrice );
this.Controls.Add(this.txtEstimatedPrice );

                this.Controls.Add(this.lblDeliveredQuantity );
this.Controls.Add(this.txtDeliveredQuantity );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblPurchased );
this.Controls.Add(this.chkPurchased );

                            // 
            // "tb_BuyingRequisitionDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BuyingRequisitionDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPuRequisition_ID;
private Krypton.Toolkit.KryptonComboBox cmbPuRequisition_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblActualRequiredQty;
private Krypton.Toolkit.KryptonTextBox txtActualRequiredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblEstimatedPrice;
private Krypton.Toolkit.KryptonTextBox txtEstimatedPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveredQuantity;
private Krypton.Toolkit.KryptonTextBox txtDeliveredQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurchased;
private Krypton.Toolkit.KryptonCheckBox chkPurchased;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

