// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:51
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
    partial class tb_BOM_SDetailSubstituteMaterialEdit
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
     this.lblSubID = new Krypton.Toolkit.KryptonLabel();
this.cmbSubID = new Krypton.Toolkit.KryptonComboBox();

this.lblIsKeyMaterial = new Krypton.Toolkit.KryptonLabel();
this.chkIsKeyMaterial = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsKeyMaterial.Values.Text ="";

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblUnitConversion_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUnitConversion_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblUsedQty = new Krypton.Toolkit.KryptonLabel();
this.txtUsedQty = new Krypton.Toolkit.KryptonTextBox();

this.lblRadix = new Krypton.Toolkit.KryptonLabel();
this.txtRadix = new Krypton.Toolkit.KryptonTextBox();

this.lblLossRate = new Krypton.Toolkit.KryptonLabel();
this.txtLossRate = new Krypton.Toolkit.KryptonTextBox();

this.lblInstallPosition = new Krypton.Toolkit.KryptonLabel();
this.txtInstallPosition = new Krypton.Toolkit.KryptonTextBox();

this.lblPositionNo = new Krypton.Toolkit.KryptonLabel();
this.txtPositionNo = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblPositionDesc = new Krypton.Toolkit.KryptonLabel();
this.txtPositionDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblManufacturingProcessID = new Krypton.Toolkit.KryptonLabel();
this.txtManufacturingProcessID = new Krypton.Toolkit.KryptonTextBox();

this.lblOutputRate = new Krypton.Toolkit.KryptonLabel();
this.txtOutputRate = new Krypton.Toolkit.KryptonTextBox();

this.lblChild_BOM_Node_ID = new Krypton.Toolkit.KryptonLabel();
this.txtChild_BOM_Node_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPriorityUseType = new Krypton.Toolkit.KryptonLabel();
this.txtPriorityUseType = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblRadix.AutoSize = true;
this.lblRadix.Location = new System.Drawing.Point(100,225);
this.lblRadix.Name = "lblRadix";
this.lblRadix.Size = new System.Drawing.Size(41, 12);
this.lblRadix.TabIndex = 9;
this.lblRadix.Text = "基数";
this.txtRadix.Location = new System.Drawing.Point(173,221);
this.txtRadix.Name = "txtRadix";
this.txtRadix.Size = new System.Drawing.Size(100, 21);
this.txtRadix.TabIndex = 9;
this.Controls.Add(this.lblRadix);
this.Controls.Add(this.txtRadix);

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
this.lblManufacturingProcessID.AutoSize = true;
this.lblManufacturingProcessID.Location = new System.Drawing.Point(100,400);
this.lblManufacturingProcessID.Name = "lblManufacturingProcessID";
this.lblManufacturingProcessID.Size = new System.Drawing.Size(41, 12);
this.lblManufacturingProcessID.TabIndex = 16;
this.lblManufacturingProcessID.Text = "制程";
this.txtManufacturingProcessID.Location = new System.Drawing.Point(173,396);
this.txtManufacturingProcessID.Name = "txtManufacturingProcessID";
this.txtManufacturingProcessID.Size = new System.Drawing.Size(100, 21);
this.txtManufacturingProcessID.TabIndex = 16;
this.Controls.Add(this.lblManufacturingProcessID);
this.Controls.Add(this.txtManufacturingProcessID);

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
this.lblChild_BOM_Node_ID.AutoSize = true;
this.lblChild_BOM_Node_ID.Location = new System.Drawing.Point(100,450);
this.lblChild_BOM_Node_ID.Name = "lblChild_BOM_Node_ID";
this.lblChild_BOM_Node_ID.Size = new System.Drawing.Size(41, 12);
this.lblChild_BOM_Node_ID.TabIndex = 18;
this.lblChild_BOM_Node_ID.Text = "子件配方";
this.txtChild_BOM_Node_ID.Location = new System.Drawing.Point(173,446);
this.txtChild_BOM_Node_ID.Name = "txtChild_BOM_Node_ID";
this.txtChild_BOM_Node_ID.Size = new System.Drawing.Size(100, 21);
this.txtChild_BOM_Node_ID.TabIndex = 18;
this.Controls.Add(this.lblChild_BOM_Node_ID);
this.Controls.Add(this.txtChild_BOM_Node_ID);

           //#####PriorityUseType###Int32
//属性测试475PriorityUseType
//属性测试475PriorityUseType
//属性测试475PriorityUseType
//属性测试475PriorityUseType
this.lblPriorityUseType.AutoSize = true;
this.lblPriorityUseType.Location = new System.Drawing.Point(100,475);
this.lblPriorityUseType.Name = "lblPriorityUseType";
this.lblPriorityUseType.Size = new System.Drawing.Size(41, 12);
this.lblPriorityUseType.TabIndex = 19;
this.lblPriorityUseType.Text = "优先使用类型";
this.txtPriorityUseType.Location = new System.Drawing.Point(173,471);
this.txtPriorityUseType.Name = "txtPriorityUseType";
this.txtPriorityUseType.Size = new System.Drawing.Size(100, 21);
this.txtPriorityUseType.TabIndex = 19;
this.Controls.Add(this.lblPriorityUseType);
this.Controls.Add(this.txtPriorityUseType);

           //#####Sort###Int32
//属性测试500Sort
//属性测试500Sort
//属性测试500Sort
//属性测试500Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,500);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 20;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,496);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 20;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

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
           // this.kryptonPanel1.TabIndex = 21;

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

                this.Controls.Add(this.lblRadix );
this.Controls.Add(this.txtRadix );

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

                this.Controls.Add(this.lblManufacturingProcessID );
this.Controls.Add(this.txtManufacturingProcessID );

                this.Controls.Add(this.lblOutputRate );
this.Controls.Add(this.txtOutputRate );

                this.Controls.Add(this.lblChild_BOM_Node_ID );
this.Controls.Add(this.txtChild_BOM_Node_ID );

                this.Controls.Add(this.lblPriorityUseType );
this.Controls.Add(this.txtPriorityUseType );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_BOM_SDetailSubstituteMaterialEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BOM_SDetailSubstituteMaterialEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblSubID;
private Krypton.Toolkit.KryptonComboBox cmbSubID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsKeyMaterial;
private Krypton.Toolkit.KryptonCheckBox chkIsKeyMaterial;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitConversion_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnitConversion_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUsedQty;
private Krypton.Toolkit.KryptonTextBox txtUsedQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblRadix;
private Krypton.Toolkit.KryptonTextBox txtRadix;

    
        
              private Krypton.Toolkit.KryptonLabel lblLossRate;
private Krypton.Toolkit.KryptonTextBox txtLossRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblInstallPosition;
private Krypton.Toolkit.KryptonTextBox txtInstallPosition;

    
        
              private Krypton.Toolkit.KryptonLabel lblPositionNo;
private Krypton.Toolkit.KryptonTextBox txtPositionNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCost;
private Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalUnitCost;
private Krypton.Toolkit.KryptonTextBox txtSubtotalUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblPositionDesc;
private Krypton.Toolkit.KryptonTextBox txtPositionDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblManufacturingProcessID;
private Krypton.Toolkit.KryptonTextBox txtManufacturingProcessID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutputRate;
private Krypton.Toolkit.KryptonTextBox txtOutputRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblChild_BOM_Node_ID;
private Krypton.Toolkit.KryptonTextBox txtChild_BOM_Node_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPriorityUseType;
private Krypton.Toolkit.KryptonTextBox txtPriorityUseType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

