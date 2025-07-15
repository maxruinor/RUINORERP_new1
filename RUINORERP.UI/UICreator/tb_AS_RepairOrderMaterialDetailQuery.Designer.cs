
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:43:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修物料明细表
    /// </summary>
    partial class tb_AS_RepairOrderMaterialDetailQuery
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
     
     this.lblRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";

this.lblIsCritical = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsCritical = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsCritical.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####RepairOrderID###Int64
//属性测试25RepairOrderID
//属性测试25RepairOrderID
//属性测试25RepairOrderID
this.lblRepairOrderID.AutoSize = true;
this.lblRepairOrderID.Location = new System.Drawing.Point(100,25);
this.lblRepairOrderID.Name = "lblRepairOrderID";
this.lblRepairOrderID.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderID.TabIndex = 1;
this.lblRepairOrderID.Text = "维修工单";
//111======25
this.cmbRepairOrderID.Location = new System.Drawing.Point(173,21);
this.cmbRepairOrderID.Name ="cmbRepairOrderID";
this.cmbRepairOrderID.Size = new System.Drawing.Size(100, 21);
this.cmbRepairOrderID.TabIndex = 1;
this.Controls.Add(this.lblRepairOrderID);
this.Controls.Add(this.cmbRepairOrderID);

           //#####Location_ID###Int64
//属性测试50Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "库位";
//111======50
this.cmbLocation_ID.Location = new System.Drawing.Point(173,46);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

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

           //#####ProdDetailID###Int64
//属性测试100ProdDetailID
//属性测试100ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,100);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 4;
this.lblProdDetailID.Text = "维修物料";
//111======100
this.cmbProdDetailID.Location = new System.Drawing.Point(173,96);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 4;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Quantity###Int32
//属性测试125Quantity
//属性测试125Quantity
//属性测试125Quantity

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,150);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 6;
this.lblUnitPrice.Text = "单价";
//111======150
this.txtUnitPrice.Location = new System.Drawing.Point(173,146);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 6;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,175);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 7;
this.lblSubtotalTransAmount.Text = "小计";
//111======175
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,171);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 7;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,200);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 8;
this.lblTaxRate.Text = "税率";
//111======200
this.txtTaxRate.Location = new System.Drawing.Point(173,196);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 8;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,225);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 9;
this.lblSubtotalTaxAmount.Text = "税额";
//111======225
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,221);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 9;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,250);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 10;
this.lblSubtotalUntaxedAmount.Text = "未税本位币";
//111======250
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,246);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 10;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,275);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 11;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,271);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 11;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,300);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 12;
this.lblCost.Text = "成本";
//111======300
this.txtCost.Location = new System.Drawing.Point(173,296);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 12;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,325);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 13;
this.lblSubtotalCost.Text = "成本小计";
//111======325
this.txtSubtotalCost.Location = new System.Drawing.Point(173,321);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 13;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,350);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 14;
this.lblGift.Text = "赠品";
this.chkGift.Location = new System.Drawing.Point(173,346);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 14;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####IsCritical###Boolean
this.lblIsCritical.AutoSize = true;
this.lblIsCritical.Location = new System.Drawing.Point(100,375);
this.lblIsCritical.Name = "lblIsCritical";
this.lblIsCritical.Size = new System.Drawing.Size(41, 12);
this.lblIsCritical.TabIndex = 15;
this.lblIsCritical.Text = "是否关键物料";
this.chkIsCritical.Location = new System.Drawing.Point(173,371);
this.chkIsCritical.Name = "chkIsCritical";
this.chkIsCritical.Size = new System.Drawing.Size(100, 21);
this.chkIsCritical.TabIndex = 15;
this.Controls.Add(this.lblIsCritical);
this.Controls.Add(this.chkIsCritical);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRepairOrderID );
this.Controls.Add(this.cmbRepairOrderID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                
                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblSubtotalTransAmount );
this.Controls.Add(this.txtSubtotalTransAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                this.Controls.Add(this.lblGift );
this.Controls.Add(this.chkGift );

                this.Controls.Add(this.lblIsCritical );
this.Controls.Add(this.chkIsCritical );

                    
            this.Name = "tb_AS_RepairOrderMaterialDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRepairOrderID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsCritical;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsCritical;

    
    
   
 





    }
}


