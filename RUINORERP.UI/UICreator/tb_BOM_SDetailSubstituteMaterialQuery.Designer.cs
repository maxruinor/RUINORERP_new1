
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:07:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定
    /// </summary>
    partial class tb_BOM_SDetailSubstituteMaterialQuery
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
     
     this.lblSubID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSubID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUnitConversion_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnitConversion_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUsedQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUsedQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblLossRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLossRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInstallPosition = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInstallPosition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPositionNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPositionNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitlCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitlCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPositionDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPositionDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblOutputRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutputRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####SubID###Int64
//属性测试25SubID
this.lblSubID.AutoSize = true;
this.lblSubID.Location = new System.Drawing.Point(100,25);
this.lblSubID.Name = "lblSubID";
this.lblSubID.Size = new System.Drawing.Size(41, 12);
this.lblSubID.TabIndex = 1;
this.lblSubID.Text = "";
//111======25
this.cmbSubID.Location = new System.Drawing.Point(173,21);
this.cmbSubID.Name ="cmbSubID";
this.cmbSubID.Size = new System.Drawing.Size(100, 21);
this.cmbSubID.TabIndex = 1;
this.Controls.Add(this.lblSubID);
this.Controls.Add(this.cmbSubID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
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

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,75);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 3;
this.lblSKU.Text = "SKU";
this.txtSKU.Location = new System.Drawing.Point(173,71);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 3;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "子件属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Unit_ID###Int64
//属性测试125Unit_ID
//属性测试125Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,125);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 5;
this.lblUnit_ID.Text = "单位";
//111======125
this.cmbUnit_ID.Location = new System.Drawing.Point(173,121);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 5;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####UnitConversion_ID###Int64
//属性测试150UnitConversion_ID
//属性测试150UnitConversion_ID
//属性测试150UnitConversion_ID
this.lblUnitConversion_ID.AutoSize = true;
this.lblUnitConversion_ID.Location = new System.Drawing.Point(100,150);
this.lblUnitConversion_ID.Name = "lblUnitConversion_ID";
this.lblUnitConversion_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnitConversion_ID.TabIndex = 6;
this.lblUnitConversion_ID.Text = "单位换算";
//111======150
this.cmbUnitConversion_ID.Location = new System.Drawing.Point(173,146);
this.cmbUnitConversion_ID.Name ="cmbUnitConversion_ID";
this.cmbUnitConversion_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnitConversion_ID.TabIndex = 6;
this.Controls.Add(this.lblUnitConversion_ID);
this.Controls.Add(this.cmbUnitConversion_ID);

           //#####UsedQty###Decimal
this.lblUsedQty.AutoSize = true;
this.lblUsedQty.Location = new System.Drawing.Point(100,175);
this.lblUsedQty.Name = "lblUsedQty";
this.lblUsedQty.Size = new System.Drawing.Size(41, 12);
this.lblUsedQty.TabIndex = 7;
this.lblUsedQty.Text = "用量";
//111======175
this.txtUsedQty.Location = new System.Drawing.Point(173,171);
this.txtUsedQty.Name ="txtUsedQty";
this.txtUsedQty.Size = new System.Drawing.Size(100, 21);
this.txtUsedQty.TabIndex = 7;
this.Controls.Add(this.lblUsedQty);
this.Controls.Add(this.txtUsedQty);

           //#####Radix###Int32
//属性测试200Radix
//属性测试200Radix
//属性测试200Radix
//属性测试200Radix

           //#####LossRate###Decimal
this.lblLossRate.AutoSize = true;
this.lblLossRate.Location = new System.Drawing.Point(100,225);
this.lblLossRate.Name = "lblLossRate";
this.lblLossRate.Size = new System.Drawing.Size(41, 12);
this.lblLossRate.TabIndex = 9;
this.lblLossRate.Text = "损耗率";
//111======225
this.txtLossRate.Location = new System.Drawing.Point(173,221);
this.txtLossRate.Name ="txtLossRate";
this.txtLossRate.Size = new System.Drawing.Size(100, 21);
this.txtLossRate.TabIndex = 9;
this.Controls.Add(this.lblLossRate);
this.Controls.Add(this.txtLossRate);

           //#####50InstallPosition###String
this.lblInstallPosition.AutoSize = true;
this.lblInstallPosition.Location = new System.Drawing.Point(100,250);
this.lblInstallPosition.Name = "lblInstallPosition";
this.lblInstallPosition.Size = new System.Drawing.Size(41, 12);
this.lblInstallPosition.TabIndex = 10;
this.lblInstallPosition.Text = "组装位置";
this.txtInstallPosition.Location = new System.Drawing.Point(173,246);
this.txtInstallPosition.Name = "txtInstallPosition";
this.txtInstallPosition.Size = new System.Drawing.Size(100, 21);
this.txtInstallPosition.TabIndex = 10;
this.Controls.Add(this.lblInstallPosition);
this.Controls.Add(this.txtInstallPosition);

           //#####50PositionNo###String
this.lblPositionNo.AutoSize = true;
this.lblPositionNo.Location = new System.Drawing.Point(100,275);
this.lblPositionNo.Name = "lblPositionNo";
this.lblPositionNo.Size = new System.Drawing.Size(41, 12);
this.lblPositionNo.TabIndex = 11;
this.lblPositionNo.Text = "位号";
this.txtPositionNo.Location = new System.Drawing.Point(173,271);
this.txtPositionNo.Name = "txtPositionNo";
this.txtPositionNo.Size = new System.Drawing.Size(100, 21);
this.txtPositionNo.TabIndex = 11;
this.Controls.Add(this.lblPositionNo);
this.Controls.Add(this.txtPositionNo);

           //#####UnitlCost###Decimal
this.lblUnitlCost.AutoSize = true;
this.lblUnitlCost.Location = new System.Drawing.Point(100,300);
this.lblUnitlCost.Name = "lblUnitlCost";
this.lblUnitlCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitlCost.TabIndex = 12;
this.lblUnitlCost.Text = "单位成本";
//111======300
this.txtUnitlCost.Location = new System.Drawing.Point(173,296);
this.txtUnitlCost.Name ="txtUnitlCost";
this.txtUnitlCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitlCost.TabIndex = 12;
this.Controls.Add(this.lblUnitlCost);
this.Controls.Add(this.txtUnitlCost);

           //#####SubtotalUnitCost###Decimal
this.lblSubtotalUnitCost.AutoSize = true;
this.lblSubtotalUnitCost.Location = new System.Drawing.Point(100,325);
this.lblSubtotalUnitCost.Name = "lblSubtotalUnitCost";
this.lblSubtotalUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUnitCost.TabIndex = 13;
this.lblSubtotalUnitCost.Text = "成本小计";
//111======325
this.txtSubtotalUnitCost.Location = new System.Drawing.Point(173,321);
this.txtSubtotalUnitCost.Name ="txtSubtotalUnitCost";
this.txtSubtotalUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUnitCost.TabIndex = 13;
this.Controls.Add(this.lblSubtotalUnitCost);
this.Controls.Add(this.txtSubtotalUnitCost);

           //#####100PositionDesc###String
this.lblPositionDesc.AutoSize = true;
this.lblPositionDesc.Location = new System.Drawing.Point(100,350);
this.lblPositionDesc.Name = "lblPositionDesc";
this.lblPositionDesc.Size = new System.Drawing.Size(41, 12);
this.lblPositionDesc.TabIndex = 14;
this.lblPositionDesc.Text = "位号描述";
this.txtPositionDesc.Location = new System.Drawing.Point(173,346);
this.txtPositionDesc.Name = "txtPositionDesc";
this.txtPositionDesc.Size = new System.Drawing.Size(100, 21);
this.txtPositionDesc.TabIndex = 14;
this.Controls.Add(this.lblPositionDesc);
this.Controls.Add(this.txtPositionDesc);

           //#####ManufacturingProcessID###Int64
//属性测试375ManufacturingProcessID
//属性测试375ManufacturingProcessID
//属性测试375ManufacturingProcessID
//属性测试375ManufacturingProcessID

           //#####OutputRate###Decimal
this.lblOutputRate.AutoSize = true;
this.lblOutputRate.Location = new System.Drawing.Point(100,400);
this.lblOutputRate.Name = "lblOutputRate";
this.lblOutputRate.Size = new System.Drawing.Size(41, 12);
this.lblOutputRate.TabIndex = 16;
this.lblOutputRate.Text = "产出率";
//111======400
this.txtOutputRate.Location = new System.Drawing.Point(173,396);
this.txtOutputRate.Name ="txtOutputRate";
this.txtOutputRate.Size = new System.Drawing.Size(100, 21);
this.txtOutputRate.TabIndex = 16;
this.Controls.Add(this.lblOutputRate);
this.Controls.Add(this.txtOutputRate);

           //#####PriorityUseType###Int32
//属性测试425PriorityUseType
//属性测试425PriorityUseType
//属性测试425PriorityUseType
//属性测试425PriorityUseType

           //#####Sort###Int32
//属性测试450Sort
//属性测试450Sort
//属性测试450Sort
//属性测试450Sort

           //#####200Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,475);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 19;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,471);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 19;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSubID );
this.Controls.Add(this.cmbSubID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblUnitConversion_ID );
this.Controls.Add(this.cmbUnitConversion_ID );

                this.Controls.Add(this.lblUsedQty );
this.Controls.Add(this.txtUsedQty );

                
                this.Controls.Add(this.lblLossRate );
this.Controls.Add(this.txtLossRate );

                this.Controls.Add(this.lblInstallPosition );
this.Controls.Add(this.txtInstallPosition );

                this.Controls.Add(this.lblPositionNo );
this.Controls.Add(this.txtPositionNo );

                this.Controls.Add(this.lblUnitlCost );
this.Controls.Add(this.txtUnitlCost );

                this.Controls.Add(this.lblSubtotalUnitCost );
this.Controls.Add(this.txtSubtotalUnitCost );

                this.Controls.Add(this.lblPositionDesc );
this.Controls.Add(this.txtPositionDesc );

                
                this.Controls.Add(this.lblOutputRate );
this.Controls.Add(this.txtOutputRate );

                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_BOM_SDetailSubstituteMaterialQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSubID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitConversion_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnitConversion_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUsedQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUsedQty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLossRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLossRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInstallPosition;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInstallPosition;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPositionNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPositionNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitlCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitlCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPositionDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPositionDesc;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutputRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutputRate;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


