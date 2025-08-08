
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:17
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
    partial class tb_SO_ContractDetailQuery
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
        
     //for start
     
     this.lblSOContractID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSOContractID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblItemName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtItemName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblItemNumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtItemNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecification = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecification = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemarks = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemarks.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####SOContractID###Int64
//属性测试25SOContractID
this.lblSOContractID.AutoSize = true;
this.lblSOContractID.Location = new System.Drawing.Point(100,25);
this.lblSOContractID.Name = "lblSOContractID";
this.lblSOContractID.Size = new System.Drawing.Size(41, 12);
this.lblSOContractID.TabIndex = 1;
this.lblSOContractID.Text = "";
//111======25
this.cmbSOContractID.Location = new System.Drawing.Point(173,21);
this.cmbSOContractID.Name ="cmbSOContractID";
this.cmbSOContractID.Size = new System.Drawing.Size(100, 21);
this.cmbSOContractID.TabIndex = 1;
this.Controls.Add(this.lblSOContractID);
this.Controls.Add(this.cmbSOContractID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID

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
//属性测试175Quantity

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSOContractID );
this.Controls.Add(this.cmbSOContractID );

                
                this.Controls.Add(this.lblItemName );
this.Controls.Add(this.txtItemName );

                this.Controls.Add(this.lblItemNumber );
this.Controls.Add(this.txtItemNumber );

                this.Controls.Add(this.lblSpecification );
this.Controls.Add(this.txtSpecification );

                this.Controls.Add(this.lblUnit );
this.Controls.Add(this.txtUnit );

                
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

                    
            this.Name = "tb_SO_ContractDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOContractID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOContractID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblItemName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtItemName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblItemNumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtItemNumber;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecification;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecification;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnit;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemarks;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemarks;

    
    
   
 





    }
}


