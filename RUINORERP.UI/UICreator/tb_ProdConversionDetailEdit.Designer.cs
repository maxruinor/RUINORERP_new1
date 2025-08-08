// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:55
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品转换单明细
    /// </summary>
    partial class tb_ProdConversionDetailEdit
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
     ProdDetailID_from主外字段不一致。this.lblConversionID = new Krypton.Toolkit.KryptonLabel();
this.cmbConversionID = new Krypton.Toolkit.KryptonComboBox();
ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。
ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.lblProdDetailID_from = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID_from = new Krypton.Toolkit.KryptonTextBox();

this.lblBarCode_from = new Krypton.Toolkit.KryptonLabel();
this.txtBarCode_from = new Krypton.Toolkit.KryptonTextBox();
this.txtBarCode_from.Multiline = true;

this.lblSKU_from = new Krypton.Toolkit.KryptonLabel();
this.txtSKU_from = new Krypton.Toolkit.KryptonTextBox();
this.txtSKU_from.Multiline = true;

ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.lblType_ID_from = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID_from = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName_from = new Krypton.Toolkit.KryptonLabel();
this.txtCNName_from = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName_from.Multiline = true;

this.lblModel_from = new Krypton.Toolkit.KryptonLabel();
this.txtModel_from = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications_from = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications_from = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications_from.Multiline = true;

this.lblproperty_from = new Krypton.Toolkit.KryptonLabel();
this.txtproperty_from = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty_from.Multiline = true;

ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.lblConversionQty = new Krypton.Toolkit.KryptonLabel();
this.txtConversionQty = new Krypton.Toolkit.KryptonTextBox();

ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.lblProdDetailID_to = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID_to = new Krypton.Toolkit.KryptonTextBox();

this.lblBarCode_to = new Krypton.Toolkit.KryptonLabel();
this.txtBarCode_to = new Krypton.Toolkit.KryptonTextBox();
this.txtBarCode_to.Multiline = true;

this.lblSKU_to = new Krypton.Toolkit.KryptonLabel();
this.txtSKU_to = new Krypton.Toolkit.KryptonTextBox();
this.txtSKU_to.Multiline = true;

this.lblTargetInitCost = new Krypton.Toolkit.KryptonLabel();
this.txtTargetInitCost = new Krypton.Toolkit.KryptonTextBox();

ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.lblType_ID_to = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID_to = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName_to = new Krypton.Toolkit.KryptonLabel();
this.txtCNName_to = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName_to.Multiline = true;

this.lblModel_to = new Krypton.Toolkit.KryptonLabel();
this.txtModel_to = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications_to = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications_to = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications_to.Multiline = true;

this.lblproperty_to = new Krypton.Toolkit.KryptonLabel();
this.txtproperty_to = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty_to.Multiline = true;

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
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
     
            //#####ConversionID###Int64
//属性测试25ConversionID
ProdDetailID_from主外字段不一致。//属性测试25ConversionID
this.lblConversionID.AutoSize = true;
this.lblConversionID.Location = new System.Drawing.Point(100,25);
this.lblConversionID.Name = "lblConversionID";
this.lblConversionID.Size = new System.Drawing.Size(41, 12);
this.lblConversionID.TabIndex = 1;
this.lblConversionID.Text = "组合单";
//111======25
this.cmbConversionID.Location = new System.Drawing.Point(173,21);
this.cmbConversionID.Name ="cmbConversionID";
this.cmbConversionID.Size = new System.Drawing.Size(100, 21);
this.cmbConversionID.TabIndex = 1;
this.Controls.Add(this.lblConversionID);
this.Controls.Add(this.cmbConversionID);

           //#####ProdDetailID_from###Int64
//属性测试50ProdDetailID_from
ProdDetailID_from主外字段不一致。//属性测试50ProdDetailID_from
//属性测试50ProdDetailID_from
ProdDetailID_to主外字段不一致。//属性测试50ProdDetailID_from
Type_ID_from主外字段不一致。//属性测试50ProdDetailID_from
Type_ID_to主外字段不一致。this.lblProdDetailID_from.AutoSize = true;
this.lblProdDetailID_from.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID_from.Name = "lblProdDetailID_from";
this.lblProdDetailID_from.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID_from.TabIndex = 2;
this.lblProdDetailID_from.Text = "来源产品";
this.txtProdDetailID_from.Location = new System.Drawing.Point(173,46);
this.txtProdDetailID_from.Name = "txtProdDetailID_from";
this.txtProdDetailID_from.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID_from.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID_from);
this.Controls.Add(this.txtProdDetailID_from);

           //#####255BarCode_from###String
this.lblBarCode_from.AutoSize = true;
this.lblBarCode_from.Location = new System.Drawing.Point(100,75);
this.lblBarCode_from.Name = "lblBarCode_from";
this.lblBarCode_from.Size = new System.Drawing.Size(41, 12);
this.lblBarCode_from.TabIndex = 3;
this.lblBarCode_from.Text = "来源条码";
this.txtBarCode_from.Location = new System.Drawing.Point(173,71);
this.txtBarCode_from.Name = "txtBarCode_from";
this.txtBarCode_from.Size = new System.Drawing.Size(100, 21);
this.txtBarCode_from.TabIndex = 3;
this.Controls.Add(this.lblBarCode_from);
this.Controls.Add(this.txtBarCode_from);

           //#####255SKU_from###String
this.lblSKU_from.AutoSize = true;
this.lblSKU_from.Location = new System.Drawing.Point(100,100);
this.lblSKU_from.Name = "lblSKU_from";
this.lblSKU_from.Size = new System.Drawing.Size(41, 12);
this.lblSKU_from.TabIndex = 4;
this.lblSKU_from.Text = "来源SKU";
this.txtSKU_from.Location = new System.Drawing.Point(173,96);
this.txtSKU_from.Name = "txtSKU_from";
this.txtSKU_from.Size = new System.Drawing.Size(100, 21);
this.txtSKU_from.TabIndex = 4;
this.Controls.Add(this.lblSKU_from);
this.Controls.Add(this.txtSKU_from);

           //#####Type_ID_from###Int64
//属性测试125Type_ID_from
ProdDetailID_from主外字段不一致。//属性测试125Type_ID_from
//属性测试125Type_ID_from
ProdDetailID_to主外字段不一致。//属性测试125Type_ID_from
Type_ID_from主外字段不一致。//属性测试125Type_ID_from
Type_ID_to主外字段不一致。this.lblType_ID_from.AutoSize = true;
this.lblType_ID_from.Location = new System.Drawing.Point(100,125);
this.lblType_ID_from.Name = "lblType_ID_from";
this.lblType_ID_from.Size = new System.Drawing.Size(41, 12);
this.lblType_ID_from.TabIndex = 5;
this.lblType_ID_from.Text = "来源产品类型";
this.txtType_ID_from.Location = new System.Drawing.Point(173,121);
this.txtType_ID_from.Name = "txtType_ID_from";
this.txtType_ID_from.Size = new System.Drawing.Size(100, 21);
this.txtType_ID_from.TabIndex = 5;
this.Controls.Add(this.lblType_ID_from);
this.Controls.Add(this.txtType_ID_from);

           //#####255CNName_from###String
this.lblCNName_from.AutoSize = true;
this.lblCNName_from.Location = new System.Drawing.Point(100,150);
this.lblCNName_from.Name = "lblCNName_from";
this.lblCNName_from.Size = new System.Drawing.Size(41, 12);
this.lblCNName_from.TabIndex = 6;
this.lblCNName_from.Text = "来源品名";
this.txtCNName_from.Location = new System.Drawing.Point(173,146);
this.txtCNName_from.Name = "txtCNName_from";
this.txtCNName_from.Size = new System.Drawing.Size(100, 21);
this.txtCNName_from.TabIndex = 6;
this.Controls.Add(this.lblCNName_from);
this.Controls.Add(this.txtCNName_from);

           //#####50Model_from###String
this.lblModel_from.AutoSize = true;
this.lblModel_from.Location = new System.Drawing.Point(100,175);
this.lblModel_from.Name = "lblModel_from";
this.lblModel_from.Size = new System.Drawing.Size(41, 12);
this.lblModel_from.TabIndex = 7;
this.lblModel_from.Text = "来源型号";
this.txtModel_from.Location = new System.Drawing.Point(173,171);
this.txtModel_from.Name = "txtModel_from";
this.txtModel_from.Size = new System.Drawing.Size(100, 21);
this.txtModel_from.TabIndex = 7;
this.Controls.Add(this.lblModel_from);
this.Controls.Add(this.txtModel_from);

           //#####1000Specifications_from###String
this.lblSpecifications_from.AutoSize = true;
this.lblSpecifications_from.Location = new System.Drawing.Point(100,200);
this.lblSpecifications_from.Name = "lblSpecifications_from";
this.lblSpecifications_from.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications_from.TabIndex = 8;
this.lblSpecifications_from.Text = "来源规格";
this.txtSpecifications_from.Location = new System.Drawing.Point(173,196);
this.txtSpecifications_from.Name = "txtSpecifications_from";
this.txtSpecifications_from.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications_from.TabIndex = 8;
this.Controls.Add(this.lblSpecifications_from);
this.Controls.Add(this.txtSpecifications_from);

           //#####255property_from###String
this.lblproperty_from.AutoSize = true;
this.lblproperty_from.Location = new System.Drawing.Point(100,225);
this.lblproperty_from.Name = "lblproperty_from";
this.lblproperty_from.Size = new System.Drawing.Size(41, 12);
this.lblproperty_from.TabIndex = 9;
this.lblproperty_from.Text = "来源属性";
this.txtproperty_from.Location = new System.Drawing.Point(173,221);
this.txtproperty_from.Name = "txtproperty_from";
this.txtproperty_from.Size = new System.Drawing.Size(100, 21);
this.txtproperty_from.TabIndex = 9;
this.Controls.Add(this.lblproperty_from);
this.Controls.Add(this.txtproperty_from);

           //#####ConversionQty###Int32
//属性测试250ConversionQty
ProdDetailID_from主外字段不一致。//属性测试250ConversionQty
//属性测试250ConversionQty
ProdDetailID_to主外字段不一致。//属性测试250ConversionQty
Type_ID_from主外字段不一致。//属性测试250ConversionQty
Type_ID_to主外字段不一致。this.lblConversionQty.AutoSize = true;
this.lblConversionQty.Location = new System.Drawing.Point(100,250);
this.lblConversionQty.Name = "lblConversionQty";
this.lblConversionQty.Size = new System.Drawing.Size(41, 12);
this.lblConversionQty.TabIndex = 10;
this.lblConversionQty.Text = "转换数量";
this.txtConversionQty.Location = new System.Drawing.Point(173,246);
this.txtConversionQty.Name = "txtConversionQty";
this.txtConversionQty.Size = new System.Drawing.Size(100, 21);
this.txtConversionQty.TabIndex = 10;
this.Controls.Add(this.lblConversionQty);
this.Controls.Add(this.txtConversionQty);

           //#####ProdDetailID_to###Int64
//属性测试275ProdDetailID_to
ProdDetailID_from主外字段不一致。//属性测试275ProdDetailID_to
//属性测试275ProdDetailID_to
ProdDetailID_to主外字段不一致。//属性测试275ProdDetailID_to
Type_ID_from主外字段不一致。//属性测试275ProdDetailID_to
Type_ID_to主外字段不一致。this.lblProdDetailID_to.AutoSize = true;
this.lblProdDetailID_to.Location = new System.Drawing.Point(100,275);
this.lblProdDetailID_to.Name = "lblProdDetailID_to";
this.lblProdDetailID_to.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID_to.TabIndex = 11;
this.lblProdDetailID_to.Text = "产品";
this.txtProdDetailID_to.Location = new System.Drawing.Point(173,271);
this.txtProdDetailID_to.Name = "txtProdDetailID_to";
this.txtProdDetailID_to.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID_to.TabIndex = 11;
this.Controls.Add(this.lblProdDetailID_to);
this.Controls.Add(this.txtProdDetailID_to);

           //#####255BarCode_to###String
this.lblBarCode_to.AutoSize = true;
this.lblBarCode_to.Location = new System.Drawing.Point(100,300);
this.lblBarCode_to.Name = "lblBarCode_to";
this.lblBarCode_to.Size = new System.Drawing.Size(41, 12);
this.lblBarCode_to.TabIndex = 12;
this.lblBarCode_to.Text = "目标条码";
this.txtBarCode_to.Location = new System.Drawing.Point(173,296);
this.txtBarCode_to.Name = "txtBarCode_to";
this.txtBarCode_to.Size = new System.Drawing.Size(100, 21);
this.txtBarCode_to.TabIndex = 12;
this.Controls.Add(this.lblBarCode_to);
this.Controls.Add(this.txtBarCode_to);

           //#####255SKU_to###String
this.lblSKU_to.AutoSize = true;
this.lblSKU_to.Location = new System.Drawing.Point(100,325);
this.lblSKU_to.Name = "lblSKU_to";
this.lblSKU_to.Size = new System.Drawing.Size(41, 12);
this.lblSKU_to.TabIndex = 13;
this.lblSKU_to.Text = "目标SKU";
this.txtSKU_to.Location = new System.Drawing.Point(173,321);
this.txtSKU_to.Name = "txtSKU_to";
this.txtSKU_to.Size = new System.Drawing.Size(100, 21);
this.txtSKU_to.TabIndex = 13;
this.Controls.Add(this.lblSKU_to);
this.Controls.Add(this.txtSKU_to);

           //#####TargetInitCost###Decimal
this.lblTargetInitCost.AutoSize = true;
this.lblTargetInitCost.Location = new System.Drawing.Point(100,350);
this.lblTargetInitCost.Name = "lblTargetInitCost";
this.lblTargetInitCost.Size = new System.Drawing.Size(41, 12);
this.lblTargetInitCost.TabIndex = 14;
this.lblTargetInitCost.Text = "初始成本";
//111======350
this.txtTargetInitCost.Location = new System.Drawing.Point(173,346);
this.txtTargetInitCost.Name ="txtTargetInitCost";
this.txtTargetInitCost.Size = new System.Drawing.Size(100, 21);
this.txtTargetInitCost.TabIndex = 14;
this.Controls.Add(this.lblTargetInitCost);
this.Controls.Add(this.txtTargetInitCost);

           //#####Type_ID_to###Int64
//属性测试375Type_ID_to
ProdDetailID_from主外字段不一致。//属性测试375Type_ID_to
//属性测试375Type_ID_to
ProdDetailID_to主外字段不一致。//属性测试375Type_ID_to
Type_ID_from主外字段不一致。//属性测试375Type_ID_to
Type_ID_to主外字段不一致。this.lblType_ID_to.AutoSize = true;
this.lblType_ID_to.Location = new System.Drawing.Point(100,375);
this.lblType_ID_to.Name = "lblType_ID_to";
this.lblType_ID_to.Size = new System.Drawing.Size(41, 12);
this.lblType_ID_to.TabIndex = 15;
this.lblType_ID_to.Text = "目标产品类型";
this.txtType_ID_to.Location = new System.Drawing.Point(173,371);
this.txtType_ID_to.Name = "txtType_ID_to";
this.txtType_ID_to.Size = new System.Drawing.Size(100, 21);
this.txtType_ID_to.TabIndex = 15;
this.Controls.Add(this.lblType_ID_to);
this.Controls.Add(this.txtType_ID_to);

           //#####255CNName_to###String
this.lblCNName_to.AutoSize = true;
this.lblCNName_to.Location = new System.Drawing.Point(100,400);
this.lblCNName_to.Name = "lblCNName_to";
this.lblCNName_to.Size = new System.Drawing.Size(41, 12);
this.lblCNName_to.TabIndex = 16;
this.lblCNName_to.Text = "目标品名";
this.txtCNName_to.Location = new System.Drawing.Point(173,396);
this.txtCNName_to.Name = "txtCNName_to";
this.txtCNName_to.Size = new System.Drawing.Size(100, 21);
this.txtCNName_to.TabIndex = 16;
this.Controls.Add(this.lblCNName_to);
this.Controls.Add(this.txtCNName_to);

           //#####50Model_to###String
this.lblModel_to.AutoSize = true;
this.lblModel_to.Location = new System.Drawing.Point(100,425);
this.lblModel_to.Name = "lblModel_to";
this.lblModel_to.Size = new System.Drawing.Size(41, 12);
this.lblModel_to.TabIndex = 17;
this.lblModel_to.Text = "目标型号";
this.txtModel_to.Location = new System.Drawing.Point(173,421);
this.txtModel_to.Name = "txtModel_to";
this.txtModel_to.Size = new System.Drawing.Size(100, 21);
this.txtModel_to.TabIndex = 17;
this.Controls.Add(this.lblModel_to);
this.Controls.Add(this.txtModel_to);

           //#####1000Specifications_to###String
this.lblSpecifications_to.AutoSize = true;
this.lblSpecifications_to.Location = new System.Drawing.Point(100,450);
this.lblSpecifications_to.Name = "lblSpecifications_to";
this.lblSpecifications_to.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications_to.TabIndex = 18;
this.lblSpecifications_to.Text = "目标规格";
this.txtSpecifications_to.Location = new System.Drawing.Point(173,446);
this.txtSpecifications_to.Name = "txtSpecifications_to";
this.txtSpecifications_to.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications_to.TabIndex = 18;
this.Controls.Add(this.lblSpecifications_to);
this.Controls.Add(this.txtSpecifications_to);

           //#####255property_to###String
this.lblproperty_to.AutoSize = true;
this.lblproperty_to.Location = new System.Drawing.Point(100,475);
this.lblproperty_to.Name = "lblproperty_to";
this.lblproperty_to.Size = new System.Drawing.Size(41, 12);
this.lblproperty_to.TabIndex = 19;
this.lblproperty_to.Text = "目标属性";
this.txtproperty_to.Location = new System.Drawing.Point(173,471);
this.txtproperty_to.Name = "txtproperty_to";
this.txtproperty_to.Size = new System.Drawing.Size(100, 21);
this.txtproperty_to.TabIndex = 19;
this.Controls.Add(this.lblproperty_to);
this.Controls.Add(this.txtproperty_to);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,500);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 20;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,496);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 20;
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
           // this.kryptonPanel1.TabIndex = 20;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                ProdDetailID_from主外字段不一致。this.Controls.Add(this.lblConversionID );
this.Controls.Add(this.cmbConversionID );

                ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.Controls.Add(this.lblProdDetailID_from );
this.Controls.Add(this.txtProdDetailID_from );

                this.Controls.Add(this.lblBarCode_from );
this.Controls.Add(this.txtBarCode_from );

                this.Controls.Add(this.lblSKU_from );
this.Controls.Add(this.txtSKU_from );

                ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.Controls.Add(this.lblType_ID_from );
this.Controls.Add(this.txtType_ID_from );

                this.Controls.Add(this.lblCNName_from );
this.Controls.Add(this.txtCNName_from );

                this.Controls.Add(this.lblModel_from );
this.Controls.Add(this.txtModel_from );

                this.Controls.Add(this.lblSpecifications_from );
this.Controls.Add(this.txtSpecifications_from );

                this.Controls.Add(this.lblproperty_from );
this.Controls.Add(this.txtproperty_from );

                ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.Controls.Add(this.lblConversionQty );
this.Controls.Add(this.txtConversionQty );

                ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.Controls.Add(this.lblProdDetailID_to );
this.Controls.Add(this.txtProdDetailID_to );

                this.Controls.Add(this.lblBarCode_to );
this.Controls.Add(this.txtBarCode_to );

                this.Controls.Add(this.lblSKU_to );
this.Controls.Add(this.txtSKU_to );

                this.Controls.Add(this.lblTargetInitCost );
this.Controls.Add(this.txtTargetInitCost );

                ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。this.Controls.Add(this.lblType_ID_to );
this.Controls.Add(this.txtType_ID_to );

                this.Controls.Add(this.lblCNName_to );
this.Controls.Add(this.txtCNName_to );

                this.Controls.Add(this.lblModel_to );
this.Controls.Add(this.txtModel_to );

                this.Controls.Add(this.lblSpecifications_to );
this.Controls.Add(this.txtSpecifications_to );

                this.Controls.Add(this.lblproperty_to );
this.Controls.Add(this.txtproperty_to );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_ProdConversionDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdConversionDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              ProdDetailID_from主外字段不一致。private Krypton.Toolkit.KryptonLabel lblConversionID;
private Krypton.Toolkit.KryptonComboBox cmbConversionID;
ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。
    
        
              ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblProdDetailID_from;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblBarCode_from;
private Krypton.Toolkit.KryptonTextBox txtBarCode_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU_from;
private Krypton.Toolkit.KryptonTextBox txtSKU_from;

    
        
              ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblType_ID_from;
private Krypton.Toolkit.KryptonTextBox txtType_ID_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName_from;
private Krypton.Toolkit.KryptonTextBox txtCNName_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel_from;
private Krypton.Toolkit.KryptonTextBox txtModel_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications_from;
private Krypton.Toolkit.KryptonTextBox txtSpecifications_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty_from;
private Krypton.Toolkit.KryptonTextBox txtproperty_from;

    
        
              ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblConversionQty;
private Krypton.Toolkit.KryptonTextBox txtConversionQty;

    
        
              ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblProdDetailID_to;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblBarCode_to;
private Krypton.Toolkit.KryptonTextBox txtBarCode_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU_to;
private Krypton.Toolkit.KryptonTextBox txtSKU_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetInitCost;
private Krypton.Toolkit.KryptonTextBox txtTargetInitCost;

    
        
              ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblType_ID_to;
private Krypton.Toolkit.KryptonTextBox txtType_ID_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName_to;
private Krypton.Toolkit.KryptonTextBox txtCNName_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel_to;
private Krypton.Toolkit.KryptonTextBox txtModel_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications_to;
private Krypton.Toolkit.KryptonTextBox txtSpecifications_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty_to;
private Krypton.Toolkit.KryptonTextBox txtproperty_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

