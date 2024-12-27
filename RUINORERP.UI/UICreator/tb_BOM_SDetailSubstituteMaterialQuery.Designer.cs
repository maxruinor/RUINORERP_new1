
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:52
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

this.lblIsKeyMaterial = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsKeyMaterial = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsKeyMaterial.Values.Text ="";

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

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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

           //#####IsKeyMaterial###Boolean
this.lblIsKeyMaterial.AutoSize = true;
this.lblIsKeyMaterial.Location = new System.Drawing.Point(100,50);
this.lblIsKeyMaterial.Name = "lblIsKeyMaterial";
this.lblIsKeyMaterial.Size = new System.Drawing.Size(41, 12);
this.lblIsKeyMaterial.TabIndex = 2;
this.lblIsKeyMaterial.Text = "关键物料";
this.chkIsKeyMaterial.Location = new System.Drawing.Point(173,46);
this.chkIsKeyMaterial.Name = "chkIsKeyMaterial";
this.chkIsKeyMaterial.Size = new System.Drawing.Size(100, 21);
this.chkIsKeyMaterial.TabIndex = 2;
this.Controls.Add(this.lblIsKeyMaterial);
this.Controls.Add(this.chkIsKeyMaterial);

           //#####ProdDetailID###Int64
//属性测试75ProdDetailID
//属性测试75ProdDetailID
//属性测试75ProdDetailID
//属性测试75ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,75);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 3;
this.lblProdDetailID.Text = "产品详情";
//111======75
this.cmbProdDetailID.Location = new System.Drawing.Point(173,71);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 3;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,100);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 4;
this.lblSKU.Text = "SKU";
this.txtSKU.Location = new System.Drawing.Point(173,96);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 4;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,125);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 5;
this.lblproperty.Text = "子件属性";
this.txtproperty.Location = new System.Drawing.Point(173,121);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 5;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Unit_ID###Int64
//属性测试150Unit_ID
//属性测试150Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,150);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 6;
this.lblUnit_ID.Text = "单位";
//111======150
this.cmbUnit_ID.Location = new System.Drawing.Point(173,146);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 6;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####UnitConversion_ID###Int64
//属性测试175UnitConversion_ID
//属性测试175UnitConversion_ID
//属性测试175UnitConversion_ID
this.lblUnitConversion_ID.AutoSize = true;
this.lblUnitConversion_ID.Location = new System.Drawing.Point(100,175);
this.lblUnitConversion_ID.Name = "lblUnitConversion_ID";
this.lblUnitConversion_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnitConversion_ID.TabIndex = 7;
this.lblUnitConversion_ID.Text = "单位换算";
//111======175
this.cmbUnitConversion_ID.Location = new System.Drawing.Point(173,171);
this.cmbUnitConversion_ID.Name ="cmbUnitConversion_ID";
this.cmbUnitConversion_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnitConversion_ID.TabIndex = 7;
this.Controls.Add(this.lblUnitConversion_ID);
this.Controls.Add(this.cmbUnitConversion_ID);

           //#####UsedQty###Decimal
this.lblUsedQty.AutoSize = true;
this.lblUsedQty.Location = new System.Drawing.Point(100,200);
this.lblUsedQty.Name = "lblUsedQty";
this.lblUsedQty.Size = new System.Drawing.Size(41, 12);
this.lblUsedQty.TabIndex = 8;
this.lblUsedQty.Text = "用量";
//111======200
this.txtUsedQty.Location = new System.Drawing.Point(173,196);
this.txtUsedQty.Name ="txtUsedQty";
this.txtUsedQty.Size = new System.Drawing.Size(100, 21);
this.txtUsedQty.TabIndex = 8;
this.Controls.Add(this.lblUsedQty);
this.Controls.Add(this.txtUsedQty);

           //#####Radix###Int32
//属性测试225Radix
//属性测试225Radix
//属性测试225Radix
//属性测试225Radix

           //#####LossRate###Decimal
this.lblLossRate.AutoSize = true;
this.lblLossRate.Location = new System.Drawing.Point(100,250);
this.lblLossRate.Name = "lblLossRate";
this.lblLossRate.Size = new System.Drawing.Size(41, 12);
this.lblLossRate.TabIndex = 10;
this.lblLossRate.Text = "损耗率";
//111======250
this.txtLossRate.Location = new System.Drawing.Point(173,246);
this.txtLossRate.Name ="txtLossRate";
this.txtLossRate.Size = new System.Drawing.Size(100, 21);
this.txtLossRate.TabIndex = 10;
this.Controls.Add(this.lblLossRate);
this.Controls.Add(this.txtLossRate);

           //#####50InstallPosition###String
this.lblInstallPosition.AutoSize = true;
this.lblInstallPosition.Location = new System.Drawing.Point(100,275);
this.lblInstallPosition.Name = "lblInstallPosition";
this.lblInstallPosition.Size = new System.Drawing.Size(41, 12);
this.lblInstallPosition.TabIndex = 11;
this.lblInstallPosition.Text = "组装位置";
this.txtInstallPosition.Location = new System.Drawing.Point(173,271);
this.txtInstallPosition.Name = "txtInstallPosition";
this.txtInstallPosition.Size = new System.Drawing.Size(100, 21);
this.txtInstallPosition.TabIndex = 11;
this.Controls.Add(this.lblInstallPosition);
this.Controls.Add(this.txtInstallPosition);

           //#####50PositionNo###String
this.lblPositionNo.AutoSize = true;
this.lblPositionNo.Location = new System.Drawing.Point(100,300);
this.lblPositionNo.Name = "lblPositionNo";
this.lblPositionNo.Size = new System.Drawing.Size(41, 12);
this.lblPositionNo.TabIndex = 12;
this.lblPositionNo.Text = "位号";
this.txtPositionNo.Location = new System.Drawing.Point(173,296);
this.txtPositionNo.Name = "txtPositionNo";
this.txtPositionNo.Size = new System.Drawing.Size(100, 21);
this.txtPositionNo.TabIndex = 12;
this.Controls.Add(this.lblPositionNo);
this.Controls.Add(this.txtPositionNo);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,325);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 13;
this.lblUnitCost.Text = "单位成本";
//111======325
this.txtUnitCost.Location = new System.Drawing.Point(173,321);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 13;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalUnitCost###Decimal
this.lblSubtotalUnitCost.AutoSize = true;
this.lblSubtotalUnitCost.Location = new System.Drawing.Point(100,350);
this.lblSubtotalUnitCost.Name = "lblSubtotalUnitCost";
this.lblSubtotalUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUnitCost.TabIndex = 14;
this.lblSubtotalUnitCost.Text = "成本小计";
//111======350
this.txtSubtotalUnitCost.Location = new System.Drawing.Point(173,346);
this.txtSubtotalUnitCost.Name ="txtSubtotalUnitCost";
this.txtSubtotalUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUnitCost.TabIndex = 14;
this.Controls.Add(this.lblSubtotalUnitCost);
this.Controls.Add(this.txtSubtotalUnitCost);

           //#####100PositionDesc###String
this.lblPositionDesc.AutoSize = true;
this.lblPositionDesc.Location = new System.Drawing.Point(100,375);
this.lblPositionDesc.Name = "lblPositionDesc";
this.lblPositionDesc.Size = new System.Drawing.Size(41, 12);
this.lblPositionDesc.TabIndex = 15;
this.lblPositionDesc.Text = "位号描述";
this.txtPositionDesc.Location = new System.Drawing.Point(173,371);
this.txtPositionDesc.Name = "txtPositionDesc";
this.txtPositionDesc.Size = new System.Drawing.Size(100, 21);
this.txtPositionDesc.TabIndex = 15;
this.Controls.Add(this.lblPositionDesc);
this.Controls.Add(this.txtPositionDesc);

           //#####ManufacturingProcessID###Int64
//属性测试400ManufacturingProcessID
//属性测试400ManufacturingProcessID
//属性测试400ManufacturingProcessID
//属性测试400ManufacturingProcessID

           //#####OutputRate###Decimal
this.lblOutputRate.AutoSize = true;
this.lblOutputRate.Location = new System.Drawing.Point(100,425);
this.lblOutputRate.Name = "lblOutputRate";
this.lblOutputRate.Size = new System.Drawing.Size(41, 12);
this.lblOutputRate.TabIndex = 17;
this.lblOutputRate.Text = "产出率";
//111======425
this.txtOutputRate.Location = new System.Drawing.Point(173,421);
this.txtOutputRate.Name ="txtOutputRate";
this.txtOutputRate.Size = new System.Drawing.Size(100, 21);
this.txtOutputRate.TabIndex = 17;
this.Controls.Add(this.lblOutputRate);
this.Controls.Add(this.txtOutputRate);

           //#####Child_BOM_Node_ID###Int64
//属性测试450Child_BOM_Node_ID
//属性测试450Child_BOM_Node_ID
//属性测试450Child_BOM_Node_ID
//属性测试450Child_BOM_Node_ID

           //#####PriorityUseType###Int32
//属性测试475PriorityUseType
//属性测试475PriorityUseType
//属性测试475PriorityUseType
//属性测试475PriorityUseType

           //#####Sort###Int32
//属性测试500Sort
//属性测试500Sort
//属性测试500Sort
//属性测试500Sort

           //#####200Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,525);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 21;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,521);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 21;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSubID );
this.Controls.Add(this.cmbSubID );

                this.Controls.Add(this.lblIsKeyMaterial );
this.Controls.Add(this.chkIsKeyMaterial );

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

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsKeyMaterial;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsKeyMaterial;

    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
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


