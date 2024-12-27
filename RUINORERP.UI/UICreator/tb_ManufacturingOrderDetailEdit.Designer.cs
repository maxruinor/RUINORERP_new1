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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    partial class tb_ManufacturingOrderDetailEdit
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
     this.lblMOID = new Krypton.Toolkit.KryptonLabel();
this.cmbMOID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblIsKeyMaterial = new Krypton.Toolkit.KryptonLabel();
this.chkIsKeyMaterial = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsKeyMaterial.Values.Text ="";

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblID = new Krypton.Toolkit.KryptonLabel();
this.txtID = new Krypton.Toolkit.KryptonTextBox();

this.lblParentId = new Krypton.Toolkit.KryptonLabel();
this.txtParentId = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblBOM_NO = new Krypton.Toolkit.KryptonLabel();
this.txtBOM_NO = new Krypton.Toolkit.KryptonTextBox();

this.lblShouldSendQty = new Krypton.Toolkit.KryptonLabel();
this.txtShouldSendQty = new Krypton.Toolkit.KryptonTextBox();

this.lblActualSentQty = new Krypton.Toolkit.KryptonLabel();
this.txtActualSentQty = new Krypton.Toolkit.KryptonTextBox();

this.lblOverSentQty = new Krypton.Toolkit.KryptonLabel();
this.txtOverSentQty = new Krypton.Toolkit.KryptonTextBox();

this.lblWastageQty = new Krypton.Toolkit.KryptonLabel();
this.txtWastageQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrentIinventory = new Krypton.Toolkit.KryptonLabel();
this.txtCurrentIinventory = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.txtBOM_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblIsExternalProduce = new Krypton.Toolkit.KryptonLabel();
this.chkIsExternalProduce = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsExternalProduce.Values.Text ="";

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblAssemblyPosition = new Krypton.Toolkit.KryptonLabel();
this.txtAssemblyPosition = new Krypton.Toolkit.KryptonTextBox();
this.txtAssemblyPosition.Multiline = true;

this.lblAlternativeProducts = new Krypton.Toolkit.KryptonLabel();
this.txtAlternativeProducts = new Krypton.Toolkit.KryptonTextBox();

this.lblPrelevel_BOM_Desc = new Krypton.Toolkit.KryptonLabel();
this.txtPrelevel_BOM_Desc = new Krypton.Toolkit.KryptonTextBox();

this.lblPrelevel_BOM_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPrelevel_BOM_ID = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####MOID###Int64
//属性测试25MOID
//属性测试25MOID
//属性测试25MOID
this.lblMOID.AutoSize = true;
this.lblMOID.Location = new System.Drawing.Point(100,25);
this.lblMOID.Name = "lblMOID";
this.lblMOID.Size = new System.Drawing.Size(41, 12);
this.lblMOID.TabIndex = 1;
this.lblMOID.Text = "";
//111======25
this.cmbMOID.Location = new System.Drawing.Point(173,21);
this.cmbMOID.Name ="cmbMOID";
this.cmbMOID.Size = new System.Drawing.Size(100, 21);
this.cmbMOID.TabIndex = 1;
this.Controls.Add(this.lblMOID);
this.Controls.Add(this.cmbMOID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####IsKeyMaterial###Boolean
this.lblIsKeyMaterial.AutoSize = true;
this.lblIsKeyMaterial.Location = new System.Drawing.Point(100,75);
this.lblIsKeyMaterial.Name = "lblIsKeyMaterial";
this.lblIsKeyMaterial.Size = new System.Drawing.Size(41, 12);
this.lblIsKeyMaterial.TabIndex = 3;
this.lblIsKeyMaterial.Text = "关键物料";
this.chkIsKeyMaterial.Location = new System.Drawing.Point(173,71);
this.chkIsKeyMaterial.Name = "chkIsKeyMaterial";
this.chkIsKeyMaterial.Size = new System.Drawing.Size(100, 21);
this.chkIsKeyMaterial.TabIndex = 3;
this.Controls.Add(this.lblIsKeyMaterial);
this.Controls.Add(this.chkIsKeyMaterial);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ID###Int64
//属性测试125ID
//属性测试125ID
//属性测试125ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,125);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 5;
this.lblID.Text = "";
this.txtID.Location = new System.Drawing.Point(173,121);
this.txtID.Name = "txtID";
this.txtID.Size = new System.Drawing.Size(100, 21);
this.txtID.TabIndex = 5;
this.Controls.Add(this.lblID);
this.Controls.Add(this.txtID);

           //#####ParentId###Int64
//属性测试150ParentId
//属性测试150ParentId
//属性测试150ParentId
this.lblParentId.AutoSize = true;
this.lblParentId.Location = new System.Drawing.Point(100,150);
this.lblParentId.Name = "lblParentId";
this.lblParentId.Size = new System.Drawing.Size(41, 12);
this.lblParentId.TabIndex = 6;
this.lblParentId.Text = "";
this.txtParentId.Location = new System.Drawing.Point(173,146);
this.txtParentId.Name = "txtParentId";
this.txtParentId.Size = new System.Drawing.Size(100, 21);
this.txtParentId.TabIndex = 6;
this.Controls.Add(this.lblParentId);
this.Controls.Add(this.txtParentId);

           //#####Location_ID###Int64
//属性测试175Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,175);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 7;
this.lblLocation_ID.Text = "库位";
//111======175
this.cmbLocation_ID.Location = new System.Drawing.Point(173,171);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 7;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####50BOM_NO###String
this.lblBOM_NO.AutoSize = true;
this.lblBOM_NO.Location = new System.Drawing.Point(100,200);
this.lblBOM_NO.Name = "lblBOM_NO";
this.lblBOM_NO.Size = new System.Drawing.Size(41, 12);
this.lblBOM_NO.TabIndex = 8;
this.lblBOM_NO.Text = "配方编号";
this.txtBOM_NO.Location = new System.Drawing.Point(173,196);
this.txtBOM_NO.Name = "txtBOM_NO";
this.txtBOM_NO.Size = new System.Drawing.Size(100, 21);
this.txtBOM_NO.TabIndex = 8;
this.Controls.Add(this.lblBOM_NO);
this.Controls.Add(this.txtBOM_NO);

           //#####ShouldSendQty###Decimal
this.lblShouldSendQty.AutoSize = true;
this.lblShouldSendQty.Location = new System.Drawing.Point(100,225);
this.lblShouldSendQty.Name = "lblShouldSendQty";
this.lblShouldSendQty.Size = new System.Drawing.Size(41, 12);
this.lblShouldSendQty.TabIndex = 9;
this.lblShouldSendQty.Text = "应发数";
//111======225
this.txtShouldSendQty.Location = new System.Drawing.Point(173,221);
this.txtShouldSendQty.Name ="txtShouldSendQty";
this.txtShouldSendQty.Size = new System.Drawing.Size(100, 21);
this.txtShouldSendQty.TabIndex = 9;
this.Controls.Add(this.lblShouldSendQty);
this.Controls.Add(this.txtShouldSendQty);

           //#####ActualSentQty###Decimal
this.lblActualSentQty.AutoSize = true;
this.lblActualSentQty.Location = new System.Drawing.Point(100,250);
this.lblActualSentQty.Name = "lblActualSentQty";
this.lblActualSentQty.Size = new System.Drawing.Size(41, 12);
this.lblActualSentQty.TabIndex = 10;
this.lblActualSentQty.Text = "实发数";
//111======250
this.txtActualSentQty.Location = new System.Drawing.Point(173,246);
this.txtActualSentQty.Name ="txtActualSentQty";
this.txtActualSentQty.Size = new System.Drawing.Size(100, 21);
this.txtActualSentQty.TabIndex = 10;
this.Controls.Add(this.lblActualSentQty);
this.Controls.Add(this.txtActualSentQty);

           //#####OverSentQty###Decimal
this.lblOverSentQty.AutoSize = true;
this.lblOverSentQty.Location = new System.Drawing.Point(100,275);
this.lblOverSentQty.Name = "lblOverSentQty";
this.lblOverSentQty.Size = new System.Drawing.Size(41, 12);
this.lblOverSentQty.TabIndex = 11;
this.lblOverSentQty.Text = "超发数";
//111======275
this.txtOverSentQty.Location = new System.Drawing.Point(173,271);
this.txtOverSentQty.Name ="txtOverSentQty";
this.txtOverSentQty.Size = new System.Drawing.Size(100, 21);
this.txtOverSentQty.TabIndex = 11;
this.Controls.Add(this.lblOverSentQty);
this.Controls.Add(this.txtOverSentQty);

           //#####WastageQty###Decimal
this.lblWastageQty.AutoSize = true;
this.lblWastageQty.Location = new System.Drawing.Point(100,300);
this.lblWastageQty.Name = "lblWastageQty";
this.lblWastageQty.Size = new System.Drawing.Size(41, 12);
this.lblWastageQty.TabIndex = 12;
this.lblWastageQty.Text = "损耗量";
//111======300
this.txtWastageQty.Location = new System.Drawing.Point(173,296);
this.txtWastageQty.Name ="txtWastageQty";
this.txtWastageQty.Size = new System.Drawing.Size(100, 21);
this.txtWastageQty.TabIndex = 12;
this.Controls.Add(this.lblWastageQty);
this.Controls.Add(this.txtWastageQty);

           //#####CurrentIinventory###Decimal
this.lblCurrentIinventory.AutoSize = true;
this.lblCurrentIinventory.Location = new System.Drawing.Point(100,325);
this.lblCurrentIinventory.Name = "lblCurrentIinventory";
this.lblCurrentIinventory.Size = new System.Drawing.Size(41, 12);
this.lblCurrentIinventory.TabIndex = 13;
this.lblCurrentIinventory.Text = "现有库存";
//111======325
this.txtCurrentIinventory.Location = new System.Drawing.Point(173,321);
this.txtCurrentIinventory.Name ="txtCurrentIinventory";
this.txtCurrentIinventory.Size = new System.Drawing.Size(100, 21);
this.txtCurrentIinventory.TabIndex = 13;
this.Controls.Add(this.lblCurrentIinventory);
this.Controls.Add(this.txtCurrentIinventory);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,350);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 14;
this.lblUnitCost.Text = "单位成本";
//111======350
this.txtUnitCost.Location = new System.Drawing.Point(173,346);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 14;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalUnitCost###Decimal
this.lblSubtotalUnitCost.AutoSize = true;
this.lblSubtotalUnitCost.Location = new System.Drawing.Point(100,375);
this.lblSubtotalUnitCost.Name = "lblSubtotalUnitCost";
this.lblSubtotalUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUnitCost.TabIndex = 15;
this.lblSubtotalUnitCost.Text = "成本小计";
//111======375
this.txtSubtotalUnitCost.Location = new System.Drawing.Point(173,371);
this.txtSubtotalUnitCost.Name ="txtSubtotalUnitCost";
this.txtSubtotalUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUnitCost.TabIndex = 15;
this.Controls.Add(this.lblSubtotalUnitCost);
this.Controls.Add(this.txtSubtotalUnitCost);

           //#####BOM_ID###Int64
//属性测试400BOM_ID
//属性测试400BOM_ID
//属性测试400BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,400);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 16;
this.lblBOM_ID.Text = "配方";
this.txtBOM_ID.Location = new System.Drawing.Point(173,396);
this.txtBOM_ID.Name = "txtBOM_ID";
this.txtBOM_ID.Size = new System.Drawing.Size(100, 21);
this.txtBOM_ID.TabIndex = 16;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.txtBOM_ID);

           //#####IsExternalProduce###Boolean
this.lblIsExternalProduce.AutoSize = true;
this.lblIsExternalProduce.Location = new System.Drawing.Point(100,425);
this.lblIsExternalProduce.Name = "lblIsExternalProduce";
this.lblIsExternalProduce.Size = new System.Drawing.Size(41, 12);
this.lblIsExternalProduce.TabIndex = 17;
this.lblIsExternalProduce.Text = "是否托外";
this.chkIsExternalProduce.Location = new System.Drawing.Point(173,421);
this.chkIsExternalProduce.Name = "chkIsExternalProduce";
this.chkIsExternalProduce.Size = new System.Drawing.Size(100, 21);
this.chkIsExternalProduce.TabIndex = 17;
this.Controls.Add(this.lblIsExternalProduce);
this.Controls.Add(this.chkIsExternalProduce);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,450);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 18;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,446);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 18;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####500AssemblyPosition###String
this.lblAssemblyPosition.AutoSize = true;
this.lblAssemblyPosition.Location = new System.Drawing.Point(100,475);
this.lblAssemblyPosition.Name = "lblAssemblyPosition";
this.lblAssemblyPosition.Size = new System.Drawing.Size(41, 12);
this.lblAssemblyPosition.TabIndex = 19;
this.lblAssemblyPosition.Text = "组装位置";
this.txtAssemblyPosition.Location = new System.Drawing.Point(173,471);
this.txtAssemblyPosition.Name = "txtAssemblyPosition";
this.txtAssemblyPosition.Size = new System.Drawing.Size(100, 21);
this.txtAssemblyPosition.TabIndex = 19;
this.Controls.Add(this.lblAssemblyPosition);
this.Controls.Add(this.txtAssemblyPosition);

           //#####10AlternativeProducts###String
this.lblAlternativeProducts.AutoSize = true;
this.lblAlternativeProducts.Location = new System.Drawing.Point(100,500);
this.lblAlternativeProducts.Name = "lblAlternativeProducts";
this.lblAlternativeProducts.Size = new System.Drawing.Size(41, 12);
this.lblAlternativeProducts.TabIndex = 20;
this.lblAlternativeProducts.Text = "替代品";
this.txtAlternativeProducts.Location = new System.Drawing.Point(173,496);
this.txtAlternativeProducts.Name = "txtAlternativeProducts";
this.txtAlternativeProducts.Size = new System.Drawing.Size(100, 21);
this.txtAlternativeProducts.TabIndex = 20;
this.Controls.Add(this.lblAlternativeProducts);
this.Controls.Add(this.txtAlternativeProducts);

           //#####100Prelevel_BOM_Desc###String
this.lblPrelevel_BOM_Desc.AutoSize = true;
this.lblPrelevel_BOM_Desc.Location = new System.Drawing.Point(100,525);
this.lblPrelevel_BOM_Desc.Name = "lblPrelevel_BOM_Desc";
this.lblPrelevel_BOM_Desc.Size = new System.Drawing.Size(41, 12);
this.lblPrelevel_BOM_Desc.TabIndex = 21;
this.lblPrelevel_BOM_Desc.Text = "上级配方名称";
this.txtPrelevel_BOM_Desc.Location = new System.Drawing.Point(173,521);
this.txtPrelevel_BOM_Desc.Name = "txtPrelevel_BOM_Desc";
this.txtPrelevel_BOM_Desc.Size = new System.Drawing.Size(100, 21);
this.txtPrelevel_BOM_Desc.TabIndex = 21;
this.Controls.Add(this.lblPrelevel_BOM_Desc);
this.Controls.Add(this.txtPrelevel_BOM_Desc);

           //#####Prelevel_BOM_ID###Int64
//属性测试550Prelevel_BOM_ID
//属性测试550Prelevel_BOM_ID
//属性测试550Prelevel_BOM_ID
this.lblPrelevel_BOM_ID.AutoSize = true;
this.lblPrelevel_BOM_ID.Location = new System.Drawing.Point(100,550);
this.lblPrelevel_BOM_ID.Name = "lblPrelevel_BOM_ID";
this.lblPrelevel_BOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblPrelevel_BOM_ID.TabIndex = 22;
this.lblPrelevel_BOM_ID.Text = "上级配方";
this.txtPrelevel_BOM_ID.Location = new System.Drawing.Point(173,546);
this.txtPrelevel_BOM_ID.Name = "txtPrelevel_BOM_ID";
this.txtPrelevel_BOM_ID.Size = new System.Drawing.Size(100, 21);
this.txtPrelevel_BOM_ID.TabIndex = 22;
this.Controls.Add(this.lblPrelevel_BOM_ID);
this.Controls.Add(this.txtPrelevel_BOM_ID);

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
           // this.kryptonPanel1.TabIndex = 22;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMOID );
this.Controls.Add(this.cmbMOID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblIsKeyMaterial );
this.Controls.Add(this.chkIsKeyMaterial );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblID );
this.Controls.Add(this.txtID );

                this.Controls.Add(this.lblParentId );
this.Controls.Add(this.txtParentId );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblBOM_NO );
this.Controls.Add(this.txtBOM_NO );

                this.Controls.Add(this.lblShouldSendQty );
this.Controls.Add(this.txtShouldSendQty );

                this.Controls.Add(this.lblActualSentQty );
this.Controls.Add(this.txtActualSentQty );

                this.Controls.Add(this.lblOverSentQty );
this.Controls.Add(this.txtOverSentQty );

                this.Controls.Add(this.lblWastageQty );
this.Controls.Add(this.txtWastageQty );

                this.Controls.Add(this.lblCurrentIinventory );
this.Controls.Add(this.txtCurrentIinventory );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalUnitCost );
this.Controls.Add(this.txtSubtotalUnitCost );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.txtBOM_ID );

                this.Controls.Add(this.lblIsExternalProduce );
this.Controls.Add(this.chkIsExternalProduce );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblAssemblyPosition );
this.Controls.Add(this.txtAssemblyPosition );

                this.Controls.Add(this.lblAlternativeProducts );
this.Controls.Add(this.txtAlternativeProducts );

                this.Controls.Add(this.lblPrelevel_BOM_Desc );
this.Controls.Add(this.txtPrelevel_BOM_Desc );

                this.Controls.Add(this.lblPrelevel_BOM_ID );
this.Controls.Add(this.txtPrelevel_BOM_ID );

                            // 
            // "tb_ManufacturingOrderDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ManufacturingOrderDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblMOID;
private Krypton.Toolkit.KryptonComboBox cmbMOID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsKeyMaterial;
private Krypton.Toolkit.KryptonCheckBox chkIsKeyMaterial;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblID;
private Krypton.Toolkit.KryptonTextBox txtID;

    
        
              private Krypton.Toolkit.KryptonLabel lblParentId;
private Krypton.Toolkit.KryptonTextBox txtParentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_NO;
private Krypton.Toolkit.KryptonTextBox txtBOM_NO;

    
        
              private Krypton.Toolkit.KryptonLabel lblShouldSendQty;
private Krypton.Toolkit.KryptonTextBox txtShouldSendQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblActualSentQty;
private Krypton.Toolkit.KryptonTextBox txtActualSentQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblOverSentQty;
private Krypton.Toolkit.KryptonTextBox txtOverSentQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblWastageQty;
private Krypton.Toolkit.KryptonTextBox txtWastageQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrentIinventory;
private Krypton.Toolkit.KryptonTextBox txtCurrentIinventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCost;
private Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalUnitCost;
private Krypton.Toolkit.KryptonTextBox txtSubtotalUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonTextBox txtBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsExternalProduce;
private Krypton.Toolkit.KryptonCheckBox chkIsExternalProduce;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblAssemblyPosition;
private Krypton.Toolkit.KryptonTextBox txtAssemblyPosition;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlternativeProducts;
private Krypton.Toolkit.KryptonTextBox txtAlternativeProducts;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrelevel_BOM_Desc;
private Krypton.Toolkit.KryptonTextBox txtPrelevel_BOM_Desc;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrelevel_BOM_ID;
private Krypton.Toolkit.KryptonTextBox txtPrelevel_BOM_ID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

