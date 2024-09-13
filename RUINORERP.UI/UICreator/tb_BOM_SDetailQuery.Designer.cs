
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:35
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    partial class tb_BOM_SDetailQuery
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
     
     this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRemarks = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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

this.lblMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblManufacturingCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtManufacturingCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalManufacturingCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalManufacturingCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPositionDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPositionDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblIsOutWork = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOutWork = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOutWork.Values.Text ="";


this.lblTotalSelfProductionAllCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalSelfProductionAllCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalOutsourcingAllCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalOutsourcingAllCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblOutputRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutputRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64
//属性测试25ProdDetailID
//属性测试25ProdDetailID
//属性测试25ProdDetailID
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "货品详情";
//111======25
this.cmbProdDetailID.Location = new System.Drawing.Point(173,21);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,50);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 2;
this.lblSKU.Text = "SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,46);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 2;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####BOM_ID###Int64
//属性测试75BOM_ID
//属性测试75BOM_ID
//属性测试75BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,75);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 3;
this.lblBOM_ID.Text = "对应BOM";
//111======75
this.cmbBOM_ID.Location = new System.Drawing.Point(173,71);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 3;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####200Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,100);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 4;
this.lblRemarks.Text = "备注说明";
this.txtRemarks.Location = new System.Drawing.Point(173,96);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 4;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

           //#####Unit_ID###Int64
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

           //#####MaterialCost###Decimal
this.lblMaterialCost.AutoSize = true;
this.lblMaterialCost.Location = new System.Drawing.Point(100,300);
this.lblMaterialCost.Name = "lblMaterialCost";
this.lblMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblMaterialCost.TabIndex = 12;
this.lblMaterialCost.Text = "物料成本";
//111======300
this.txtMaterialCost.Location = new System.Drawing.Point(173,296);
this.txtMaterialCost.Name ="txtMaterialCost";
this.txtMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtMaterialCost.TabIndex = 12;
this.Controls.Add(this.lblMaterialCost);
this.Controls.Add(this.txtMaterialCost);

           //#####SubtotalMaterialCost###Decimal
this.lblSubtotalMaterialCost.AutoSize = true;
this.lblSubtotalMaterialCost.Location = new System.Drawing.Point(100,325);
this.lblSubtotalMaterialCost.Name = "lblSubtotalMaterialCost";
this.lblSubtotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalMaterialCost.TabIndex = 13;
this.lblSubtotalMaterialCost.Text = "物料小计";
//111======325
this.txtSubtotalMaterialCost.Location = new System.Drawing.Point(173,321);
this.txtSubtotalMaterialCost.Name ="txtSubtotalMaterialCost";
this.txtSubtotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalMaterialCost.TabIndex = 13;
this.Controls.Add(this.lblSubtotalMaterialCost);
this.Controls.Add(this.txtSubtotalMaterialCost);

           //#####ManufacturingCost###Decimal
this.lblManufacturingCost.AutoSize = true;
this.lblManufacturingCost.Location = new System.Drawing.Point(100,350);
this.lblManufacturingCost.Name = "lblManufacturingCost";
this.lblManufacturingCost.Size = new System.Drawing.Size(41, 12);
this.lblManufacturingCost.TabIndex = 14;
this.lblManufacturingCost.Text = "制造费";
//111======350
this.txtManufacturingCost.Location = new System.Drawing.Point(173,346);
this.txtManufacturingCost.Name ="txtManufacturingCost";
this.txtManufacturingCost.Size = new System.Drawing.Size(100, 21);
this.txtManufacturingCost.TabIndex = 14;
this.Controls.Add(this.lblManufacturingCost);
this.Controls.Add(this.txtManufacturingCost);

           //#####OutManuCost###Decimal
this.lblOutManuCost.AutoSize = true;
this.lblOutManuCost.Location = new System.Drawing.Point(100,375);
this.lblOutManuCost.Name = "lblOutManuCost";
this.lblOutManuCost.Size = new System.Drawing.Size(41, 12);
this.lblOutManuCost.TabIndex = 15;
this.lblOutManuCost.Text = "托工费";
//111======375
this.txtOutManuCost.Location = new System.Drawing.Point(173,371);
this.txtOutManuCost.Name ="txtOutManuCost";
this.txtOutManuCost.Size = new System.Drawing.Size(100, 21);
this.txtOutManuCost.TabIndex = 15;
this.Controls.Add(this.lblOutManuCost);
this.Controls.Add(this.txtOutManuCost);

           //#####SubtotalManufacturingCost###Decimal
this.lblSubtotalManufacturingCost.AutoSize = true;
this.lblSubtotalManufacturingCost.Location = new System.Drawing.Point(100,400);
this.lblSubtotalManufacturingCost.Name = "lblSubtotalManufacturingCost";
this.lblSubtotalManufacturingCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalManufacturingCost.TabIndex = 16;
this.lblSubtotalManufacturingCost.Text = "制造费小计";
//111======400
this.txtSubtotalManufacturingCost.Location = new System.Drawing.Point(173,396);
this.txtSubtotalManufacturingCost.Name ="txtSubtotalManufacturingCost";
this.txtSubtotalManufacturingCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalManufacturingCost.TabIndex = 16;
this.Controls.Add(this.lblSubtotalManufacturingCost);
this.Controls.Add(this.txtSubtotalManufacturingCost);

           //#####SubtotalOutManuCost###Decimal
this.lblSubtotalOutManuCost.AutoSize = true;
this.lblSubtotalOutManuCost.Location = new System.Drawing.Point(100,425);
this.lblSubtotalOutManuCost.Name = "lblSubtotalOutManuCost";
this.lblSubtotalOutManuCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalOutManuCost.TabIndex = 17;
this.lblSubtotalOutManuCost.Text = "托工费小计";
//111======425
this.txtSubtotalOutManuCost.Location = new System.Drawing.Point(173,421);
this.txtSubtotalOutManuCost.Name ="txtSubtotalOutManuCost";
this.txtSubtotalOutManuCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalOutManuCost.TabIndex = 17;
this.Controls.Add(this.lblSubtotalOutManuCost);
this.Controls.Add(this.txtSubtotalOutManuCost);

           //#####100PositionDesc###String
this.lblPositionDesc.AutoSize = true;
this.lblPositionDesc.Location = new System.Drawing.Point(100,450);
this.lblPositionDesc.Name = "lblPositionDesc";
this.lblPositionDesc.Size = new System.Drawing.Size(41, 12);
this.lblPositionDesc.TabIndex = 18;
this.lblPositionDesc.Text = "位号描述";
this.txtPositionDesc.Location = new System.Drawing.Point(173,446);
this.txtPositionDesc.Name = "txtPositionDesc";
this.txtPositionDesc.Size = new System.Drawing.Size(100, 21);
this.txtPositionDesc.TabIndex = 18;
this.Controls.Add(this.lblPositionDesc);
this.Controls.Add(this.txtPositionDesc);

           //#####ManufacturingProcessID###Int64
//属性测试475ManufacturingProcessID
//属性测试475ManufacturingProcessID
//属性测试475ManufacturingProcessID
//属性测试475ManufacturingProcessID

           //#####IsOutWork###Boolean
this.lblIsOutWork.AutoSize = true;
this.lblIsOutWork.Location = new System.Drawing.Point(100,500);
this.lblIsOutWork.Name = "lblIsOutWork";
this.lblIsOutWork.Size = new System.Drawing.Size(41, 12);
this.lblIsOutWork.TabIndex = 20;
this.lblIsOutWork.Text = "是否托外";
this.chkIsOutWork.Location = new System.Drawing.Point(173,496);
this.chkIsOutWork.Name = "chkIsOutWork";
this.chkIsOutWork.Size = new System.Drawing.Size(100, 21);
this.chkIsOutWork.TabIndex = 20;
this.Controls.Add(this.lblIsOutWork);
this.Controls.Add(this.chkIsOutWork);

           //#####Child_BOM_Node_ID###Int64
//属性测试525Child_BOM_Node_ID
//属性测试525Child_BOM_Node_ID
//属性测试525Child_BOM_Node_ID
//属性测试525Child_BOM_Node_ID

           //#####TotalSelfProductionAllCost###Decimal
this.lblTotalSelfProductionAllCost.AutoSize = true;
this.lblTotalSelfProductionAllCost.Location = new System.Drawing.Point(100,550);
this.lblTotalSelfProductionAllCost.Name = "lblTotalSelfProductionAllCost";
this.lblTotalSelfProductionAllCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalSelfProductionAllCost.TabIndex = 22;
this.lblTotalSelfProductionAllCost.Text = "自产总成本";
//111======550
this.txtTotalSelfProductionAllCost.Location = new System.Drawing.Point(173,546);
this.txtTotalSelfProductionAllCost.Name ="txtTotalSelfProductionAllCost";
this.txtTotalSelfProductionAllCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalSelfProductionAllCost.TabIndex = 22;
this.Controls.Add(this.lblTotalSelfProductionAllCost);
this.Controls.Add(this.txtTotalSelfProductionAllCost);

           //#####TotalOutsourcingAllCost###Decimal
this.lblTotalOutsourcingAllCost.AutoSize = true;
this.lblTotalOutsourcingAllCost.Location = new System.Drawing.Point(100,575);
this.lblTotalOutsourcingAllCost.Name = "lblTotalOutsourcingAllCost";
this.lblTotalOutsourcingAllCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalOutsourcingAllCost.TabIndex = 23;
this.lblTotalOutsourcingAllCost.Text = "外发总成本";
//111======575
this.txtTotalOutsourcingAllCost.Location = new System.Drawing.Point(173,571);
this.txtTotalOutsourcingAllCost.Name ="txtTotalOutsourcingAllCost";
this.txtTotalOutsourcingAllCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalOutsourcingAllCost.TabIndex = 23;
this.Controls.Add(this.lblTotalOutsourcingAllCost);
this.Controls.Add(this.txtTotalOutsourcingAllCost);

           //#####Substitute###Int64
//属性测试600Substitute
//属性测试600Substitute
//属性测试600Substitute
//属性测试600Substitute

           //#####OutputRate###Decimal
this.lblOutputRate.AutoSize = true;
this.lblOutputRate.Location = new System.Drawing.Point(100,625);
this.lblOutputRate.Name = "lblOutputRate";
this.lblOutputRate.Size = new System.Drawing.Size(41, 12);
this.lblOutputRate.TabIndex = 25;
this.lblOutputRate.Text = "产出率";
//111======625
this.txtOutputRate.Location = new System.Drawing.Point(173,621);
this.txtOutputRate.Name ="txtOutputRate";
this.txtOutputRate.Size = new System.Drawing.Size(100, 21);
this.txtOutputRate.TabIndex = 25;
this.Controls.Add(this.lblOutputRate);
this.Controls.Add(this.txtOutputRate);

           //#####Sort###Int32
//属性测试650Sort
//属性测试650Sort
//属性测试650Sort
//属性测试650Sort

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

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

                this.Controls.Add(this.lblMaterialCost );
this.Controls.Add(this.txtMaterialCost );

                this.Controls.Add(this.lblSubtotalMaterialCost );
this.Controls.Add(this.txtSubtotalMaterialCost );

                this.Controls.Add(this.lblManufacturingCost );
this.Controls.Add(this.txtManufacturingCost );

                this.Controls.Add(this.lblOutManuCost );
this.Controls.Add(this.txtOutManuCost );

                this.Controls.Add(this.lblSubtotalManufacturingCost );
this.Controls.Add(this.txtSubtotalManufacturingCost );

                this.Controls.Add(this.lblSubtotalOutManuCost );
this.Controls.Add(this.txtSubtotalOutManuCost );

                this.Controls.Add(this.lblPositionDesc );
this.Controls.Add(this.txtPositionDesc );

                
                this.Controls.Add(this.lblIsOutWork );
this.Controls.Add(this.chkIsOutWork );

                
                this.Controls.Add(this.lblTotalSelfProductionAllCost );
this.Controls.Add(this.txtTotalSelfProductionAllCost );

                this.Controls.Add(this.lblTotalOutsourcingAllCost );
this.Controls.Add(this.txtTotalOutsourcingAllCost );

                
                this.Controls.Add(this.lblOutputRate );
this.Controls.Add(this.txtOutputRate );

                
                    
            this.Name = "tb_BOM_SDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemarks;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemarks;

    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblManufacturingCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtManufacturingCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutManuCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutManuCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalManufacturingCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalManufacturingCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalOutManuCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalOutManuCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPositionDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPositionDesc;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOutWork;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOutWork;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalSelfProductionAllCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalSelfProductionAllCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalOutsourcingAllCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalOutsourcingAllCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutputRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutputRate;

    
        
              
    
    
   
 





    }
}


