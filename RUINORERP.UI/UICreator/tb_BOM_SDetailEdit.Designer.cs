﻿// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:07:10
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
    partial class tb_BOM_SDetailEdit
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
     this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();

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

this.lblChild_BOM_Node_ID = new Krypton.Toolkit.KryptonLabel();
this.txtChild_BOM_Node_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSubstitute = new Krypton.Toolkit.KryptonLabel();
this.txtSubstitute = new Krypton.Toolkit.KryptonTextBox();

this.lblOutputRate = new Krypton.Toolkit.KryptonLabel();
this.txtOutputRate = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

    
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

           //#####200Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,100);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 4;
this.lblSummary.Text = "备注说明";
this.txtSummary.Location = new System.Drawing.Point(173,96);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 4;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

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
this.lblRadix.AutoSize = true;
this.lblRadix.Location = new System.Drawing.Point(100,200);
this.lblRadix.Name = "lblRadix";
this.lblRadix.Size = new System.Drawing.Size(41, 12);
this.lblRadix.TabIndex = 8;
this.lblRadix.Text = "基数";
this.txtRadix.Location = new System.Drawing.Point(173,196);
this.txtRadix.Name = "txtRadix";
this.txtRadix.Size = new System.Drawing.Size(100, 21);
this.txtRadix.TabIndex = 8;
this.Controls.Add(this.lblRadix);
this.Controls.Add(this.txtRadix);

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

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,300);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 12;
this.lblUnitCost.Text = "单位成本";
//111======300
this.txtUnitCost.Location = new System.Drawing.Point(173,296);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 12;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

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
this.lblManufacturingProcessID.AutoSize = true;
this.lblManufacturingProcessID.Location = new System.Drawing.Point(100,375);
this.lblManufacturingProcessID.Name = "lblManufacturingProcessID";
this.lblManufacturingProcessID.Size = new System.Drawing.Size(41, 12);
this.lblManufacturingProcessID.TabIndex = 15;
this.lblManufacturingProcessID.Text = "制程";
this.txtManufacturingProcessID.Location = new System.Drawing.Point(173,371);
this.txtManufacturingProcessID.Name = "txtManufacturingProcessID";
this.txtManufacturingProcessID.Size = new System.Drawing.Size(100, 21);
this.txtManufacturingProcessID.TabIndex = 15;
this.Controls.Add(this.lblManufacturingProcessID);
this.Controls.Add(this.txtManufacturingProcessID);

           //#####Child_BOM_Node_ID###Int64
//属性测试400Child_BOM_Node_ID
//属性测试400Child_BOM_Node_ID
//属性测试400Child_BOM_Node_ID
//属性测试400Child_BOM_Node_ID
this.lblChild_BOM_Node_ID.AutoSize = true;
this.lblChild_BOM_Node_ID.Location = new System.Drawing.Point(100,400);
this.lblChild_BOM_Node_ID.Name = "lblChild_BOM_Node_ID";
this.lblChild_BOM_Node_ID.Size = new System.Drawing.Size(41, 12);
this.lblChild_BOM_Node_ID.TabIndex = 16;
this.lblChild_BOM_Node_ID.Text = "子件配方";
this.txtChild_BOM_Node_ID.Location = new System.Drawing.Point(173,396);
this.txtChild_BOM_Node_ID.Name = "txtChild_BOM_Node_ID";
this.txtChild_BOM_Node_ID.Size = new System.Drawing.Size(100, 21);
this.txtChild_BOM_Node_ID.TabIndex = 16;
this.Controls.Add(this.lblChild_BOM_Node_ID);
this.Controls.Add(this.txtChild_BOM_Node_ID);

           //#####Substitute###Int64
//属性测试425Substitute
//属性测试425Substitute
//属性测试425Substitute
//属性测试425Substitute
this.lblSubstitute.AutoSize = true;
this.lblSubstitute.Location = new System.Drawing.Point(100,425);
this.lblSubstitute.Name = "lblSubstitute";
this.lblSubstitute.Size = new System.Drawing.Size(41, 12);
this.lblSubstitute.TabIndex = 17;
this.lblSubstitute.Text = "替代品";
this.txtSubstitute.Location = new System.Drawing.Point(173,421);
this.txtSubstitute.Name = "txtSubstitute";
this.txtSubstitute.Size = new System.Drawing.Size(100, 21);
this.txtSubstitute.TabIndex = 17;
this.Controls.Add(this.lblSubstitute);
this.Controls.Add(this.txtSubstitute);

           //#####OutputRate###Decimal
this.lblOutputRate.AutoSize = true;
this.lblOutputRate.Location = new System.Drawing.Point(100,450);
this.lblOutputRate.Name = "lblOutputRate";
this.lblOutputRate.Size = new System.Drawing.Size(41, 12);
this.lblOutputRate.TabIndex = 18;
this.lblOutputRate.Text = "产出率";
//111======450
this.txtOutputRate.Location = new System.Drawing.Point(173,446);
this.txtOutputRate.Name ="txtOutputRate";
this.txtOutputRate.Size = new System.Drawing.Size(100, 21);
this.txtOutputRate.TabIndex = 18;
this.Controls.Add(this.lblOutputRate);
this.Controls.Add(this.txtOutputRate);

           //#####Sort###Int32
//属性测试475Sort
//属性测试475Sort
//属性测试475Sort
//属性测试475Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,475);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 19;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,471);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 19;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

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
           // this.kryptonPanel1.TabIndex = 19;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

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

                this.Controls.Add(this.lblChild_BOM_Node_ID );
this.Controls.Add(this.txtChild_BOM_Node_ID );

                this.Controls.Add(this.lblSubstitute );
this.Controls.Add(this.txtSubstitute );

                this.Controls.Add(this.lblOutputRate );
this.Controls.Add(this.txtOutputRate );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                            // 
            // "tb_BOM_SDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BOM_SDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblChild_BOM_Node_ID;
private Krypton.Toolkit.KryptonTextBox txtChild_BOM_Node_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubstitute;
private Krypton.Toolkit.KryptonTextBox txtSubstitute;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutputRate;
private Krypton.Toolkit.KryptonTextBox txtOutputRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

