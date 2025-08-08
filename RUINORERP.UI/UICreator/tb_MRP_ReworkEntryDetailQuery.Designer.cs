
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:44
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 返工入库单明细
    /// </summary>
    partial class tb_MRP_ReworkEntryDetailQuery
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
     
     this.lblReworkEntryID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbReworkEntryID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ReworkEntryID###Int64
//属性测试25ReworkEntryID
//属性测试25ReworkEntryID
//属性测试25ReworkEntryID
//属性测试25ReworkEntryID
this.lblReworkEntryID.AutoSize = true;
this.lblReworkEntryID.Location = new System.Drawing.Point(100,25);
this.lblReworkEntryID.Name = "lblReworkEntryID";
this.lblReworkEntryID.Size = new System.Drawing.Size(41, 12);
this.lblReworkEntryID.TabIndex = 1;
this.lblReworkEntryID.Text = "返工入库单";
//111======25
this.cmbReworkEntryID.Location = new System.Drawing.Point(173,21);
this.cmbReworkEntryID.Name ="cmbReworkEntryID";
this.cmbReworkEntryID.Size = new System.Drawing.Size(100, 21);
this.cmbReworkEntryID.TabIndex = 1;
this.Controls.Add(this.lblReworkEntryID);
this.Controls.Add(this.cmbReworkEntryID);

           //#####ProdDetailID###Int64
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

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,75);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 3;
this.lblCustomertModel.Text = "客户型号";
this.txtCustomertModel.Location = new System.Drawing.Point(173,71);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 3;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####Location_ID###Int64
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

           //#####Rack_ID###Int64
//属性测试125Rack_ID
//属性测试125Rack_ID
//属性测试125Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,125);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 5;
this.lblRack_ID.Text = "货架";
//111======125
this.cmbRack_ID.Location = new System.Drawing.Point(173,121);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 5;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,150);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 6;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,146);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 6;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32
//属性测试175Quantity
//属性测试175Quantity
//属性测试175Quantity
//属性测试175Quantity

           //#####ReworkFee###Decimal
this.lblReworkFee.AutoSize = true;
this.lblReworkFee.Location = new System.Drawing.Point(100,200);
this.lblReworkFee.Name = "lblReworkFee";
this.lblReworkFee.Size = new System.Drawing.Size(41, 12);
this.lblReworkFee.TabIndex = 8;
this.lblReworkFee.Text = "返工费用";
//111======200
this.txtReworkFee.Location = new System.Drawing.Point(173,196);
this.txtReworkFee.Name ="txtReworkFee";
this.txtReworkFee.Size = new System.Drawing.Size(100, 21);
this.txtReworkFee.TabIndex = 8;
this.Controls.Add(this.lblReworkFee);
this.Controls.Add(this.txtReworkFee);

           //#####SubtotalReworkFee###Decimal
this.lblSubtotalReworkFee.AutoSize = true;
this.lblSubtotalReworkFee.Location = new System.Drawing.Point(100,225);
this.lblSubtotalReworkFee.Name = "lblSubtotalReworkFee";
this.lblSubtotalReworkFee.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalReworkFee.TabIndex = 9;
this.lblSubtotalReworkFee.Text = "返工费用小计";
//111======225
this.txtSubtotalReworkFee.Location = new System.Drawing.Point(173,221);
this.txtSubtotalReworkFee.Name ="txtSubtotalReworkFee";
this.txtSubtotalReworkFee.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalReworkFee.TabIndex = 9;
this.Controls.Add(this.lblSubtotalReworkFee);
this.Controls.Add(this.txtSubtotalReworkFee);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,250);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 10;
this.lblUnitCost.Text = "成本";
//111======250
this.txtUnitCost.Location = new System.Drawing.Point(173,246);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 10;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,275);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 11;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======275
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,271);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 11;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,300);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 12;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,296);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 12;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblReworkEntryID );
this.Controls.Add(this.cmbReworkEntryID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblReworkFee );
this.Controls.Add(this.txtReworkFee );

                this.Controls.Add(this.lblSubtotalReworkFee );
this.Controls.Add(this.txtSubtotalReworkFee );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_MRP_ReworkEntryDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReworkEntryID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbReworkEntryID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReworkFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReworkFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalReworkFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalReworkFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


