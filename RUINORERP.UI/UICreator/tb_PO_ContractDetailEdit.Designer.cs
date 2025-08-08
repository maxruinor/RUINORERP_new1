// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:49
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 合同明细
    /// </summary>
    partial class tb_PO_ContractDetailEdit
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
     this.lblPOContractID = new Krypton.Toolkit.KryptonLabel();
this.txtPOContractID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblItemName = new Krypton.Toolkit.KryptonLabel();
this.txtItemName = new Krypton.Toolkit.KryptonTextBox();

this.lblItemNumber = new Krypton.Toolkit.KryptonLabel();
this.txtItemNumber = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecification = new Krypton.Toolkit.KryptonLabel();
this.txtSpecification = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit = new Krypton.Toolkit.KryptonLabel();
this.txtUnit = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIsIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblRemarks = new Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new Krypton.Toolkit.KryptonTextBox();
this.txtRemarks.Multiline = true;

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
     
            //#####POContractID###Int64
this.lblPOContractID.AutoSize = true;
this.lblPOContractID.Location = new System.Drawing.Point(100,25);
this.lblPOContractID.Name = "lblPOContractID";
this.lblPOContractID.Size = new System.Drawing.Size(41, 12);
this.lblPOContractID.TabIndex = 1;
this.lblPOContractID.Text = "采购合同";
this.txtPOContractID.Location = new System.Drawing.Point(173,21);
this.txtPOContractID.Name = "txtPOContractID";
this.txtPOContractID.Size = new System.Drawing.Size(100, 21);
this.txtPOContractID.TabIndex = 1;
this.Controls.Add(this.lblPOContractID);
this.Controls.Add(this.txtPOContractID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品";
this.txtProdDetailID.Location = new System.Drawing.Point(173,46);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####100ItemName###String
this.lblItemName.AutoSize = true;
this.lblItemName.Location = new System.Drawing.Point(100,75);
this.lblItemName.Name = "lblItemName";
this.lblItemName.Size = new System.Drawing.Size(41, 12);
this.lblItemName.TabIndex = 3;
this.lblItemName.Text = "项目名称";
this.txtItemName.Location = new System.Drawing.Point(173,71);
this.txtItemName.Name = "txtItemName";
this.txtItemName.Size = new System.Drawing.Size(100, 21);
this.txtItemName.TabIndex = 3;
this.Controls.Add(this.lblItemName);
this.Controls.Add(this.txtItemName);

           //#####50ItemNumber###String
this.lblItemNumber.AutoSize = true;
this.lblItemNumber.Location = new System.Drawing.Point(100,100);
this.lblItemNumber.Name = "lblItemNumber";
this.lblItemNumber.Size = new System.Drawing.Size(41, 12);
this.lblItemNumber.TabIndex = 4;
this.lblItemNumber.Text = "项目编号";
this.txtItemNumber.Location = new System.Drawing.Point(173,96);
this.txtItemNumber.Name = "txtItemNumber";
this.txtItemNumber.Size = new System.Drawing.Size(100, 21);
this.txtItemNumber.TabIndex = 4;
this.Controls.Add(this.lblItemNumber);
this.Controls.Add(this.txtItemNumber);

           //#####100Specification###String
this.lblSpecification.AutoSize = true;
this.lblSpecification.Location = new System.Drawing.Point(100,125);
this.lblSpecification.Name = "lblSpecification";
this.lblSpecification.Size = new System.Drawing.Size(41, 12);
this.lblSpecification.TabIndex = 5;
this.lblSpecification.Text = "规格";
this.txtSpecification.Location = new System.Drawing.Point(173,121);
this.txtSpecification.Name = "txtSpecification";
this.txtSpecification.Size = new System.Drawing.Size(100, 21);
this.txtSpecification.TabIndex = 5;
this.Controls.Add(this.lblSpecification);
this.Controls.Add(this.txtSpecification);

           //#####20Unit###String
this.lblUnit.AutoSize = true;
this.lblUnit.Location = new System.Drawing.Point(100,150);
this.lblUnit.Name = "lblUnit";
this.lblUnit.Size = new System.Drawing.Size(41, 12);
this.lblUnit.TabIndex = 6;
this.lblUnit.Text = "单位";
this.txtUnit.Location = new System.Drawing.Point(173,146);
this.txtUnit.Name = "txtUnit";
this.txtUnit.Size = new System.Drawing.Size(100, 21);
this.txtUnit.TabIndex = 6;
this.Controls.Add(this.lblUnit);
this.Controls.Add(this.txtUnit);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,175);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 7;
this.lblQuantity.Text = "数量";
this.txtQuantity.Location = new System.Drawing.Point(173,171);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 7;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,200);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 8;
this.lblUnitPrice.Text = "单价";
//111======200
this.txtUnitPrice.Location = new System.Drawing.Point(173,196);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 8;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####SubtotalAmount###Decimal
this.lblSubtotalAmount.AutoSize = true;
this.lblSubtotalAmount.Location = new System.Drawing.Point(100,225);
this.lblSubtotalAmount.Name = "lblSubtotalAmount";
this.lblSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmount.TabIndex = 9;
this.lblSubtotalAmount.Text = "金额小计";
//111======225
this.txtSubtotalAmount.Location = new System.Drawing.Point(173,221);
this.txtSubtotalAmount.Name ="txtSubtotalAmount";
this.txtSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmount.TabIndex = 9;
this.Controls.Add(this.lblSubtotalAmount);
this.Controls.Add(this.txtSubtotalAmount);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,250);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 10;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,246);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 10;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,275);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 11;
this.lblTaxRate.Text = "税率";
//111======275
this.txtTaxRate.Location = new System.Drawing.Point(173,271);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 11;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,300);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 12;
this.lblTaxAmount.Text = "税额";
//111======300
this.txtTaxAmount.Location = new System.Drawing.Point(173,296);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 12;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####500Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,325);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 13;
this.lblRemarks.Text = "备注";
this.txtRemarks.Location = new System.Drawing.Point(173,321);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 13;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,375);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 15;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,371);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 15;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,425);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 17;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,421);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 17;
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
           // this.kryptonPanel1.TabIndex = 17;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPOContractID );
this.Controls.Add(this.txtPOContractID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblItemName );
this.Controls.Add(this.txtItemName );

                this.Controls.Add(this.lblItemNumber );
this.Controls.Add(this.txtItemNumber );

                this.Controls.Add(this.lblSpecification );
this.Controls.Add(this.txtSpecification );

                this.Controls.Add(this.lblUnit );
this.Controls.Add(this.txtUnit );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblSubtotalAmount );
this.Controls.Add(this.txtSubtotalAmount );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_PO_ContractDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PO_ContractDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPOContractID;
private Krypton.Toolkit.KryptonTextBox txtPOContractID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblItemName;
private Krypton.Toolkit.KryptonTextBox txtItemName;

    
        
              private Krypton.Toolkit.KryptonLabel lblItemNumber;
private Krypton.Toolkit.KryptonTextBox txtItemNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecification;
private Krypton.Toolkit.KryptonTextBox txtSpecification;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit;
private Krypton.Toolkit.KryptonTextBox txtUnit;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemarks;
private Krypton.Toolkit.KryptonTextBox txtRemarks;

    
        
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

