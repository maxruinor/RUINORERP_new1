// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产需求分析表明细
    /// </summary>
    partial class tb_ProductionDemandDetailEdit
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
     this.lblPDID = new Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblID = new Krypton.Toolkit.KryptonLabel();
this.txtID = new Krypton.Toolkit.KryptonTextBox();

this.lblParentId = new Krypton.Toolkit.KryptonLabel();
this.txtParentId = new Krypton.Toolkit.KryptonTextBox();

this.lblNetRequirement = new Krypton.Toolkit.KryptonLabel();
this.txtNetRequirement = new Krypton.Toolkit.KryptonTextBox();

this.lblGrossRequirement = new Krypton.Toolkit.KryptonLabel();
this.txtGrossRequirement = new Krypton.Toolkit.KryptonTextBox();

this.lblNeedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtNeedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblMissingQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtMissingQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblRequirementDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblBookInventory = new Krypton.Toolkit.KryptonLabel();
this.txtBookInventory = new Krypton.Toolkit.KryptonTextBox();

this.lblAvailableStock = new Krypton.Toolkit.KryptonLabel();
this.txtAvailableStock = new Krypton.Toolkit.KryptonTextBox();

this.lblInTransitInventory = new Krypton.Toolkit.KryptonLabel();
this.txtInTransitInventory = new Krypton.Toolkit.KryptonTextBox();

this.lblMakeProcessInventory = new Krypton.Toolkit.KryptonLabel();
this.txtMakeProcessInventory = new Krypton.Toolkit.KryptonTextBox();

this.lblSale_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtSale_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblNotOutQty = new Krypton.Toolkit.KryptonLabel();
this.txtNotOutQty = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####PDID###Int64
//属性测试25PDID
//属性测试25PDID
//属性测试25PDID
this.lblPDID.AutoSize = true;
this.lblPDID.Location = new System.Drawing.Point(100,25);
this.lblPDID.Name = "lblPDID";
this.lblPDID.Size = new System.Drawing.Size(41, 12);
this.lblPDID.TabIndex = 1;
this.lblPDID.Text = "";
//111======25
this.cmbPDID.Location = new System.Drawing.Point(173,21);
this.cmbPDID.Name ="cmbPDID";
this.cmbPDID.Size = new System.Drawing.Size(100, 21);
this.cmbPDID.TabIndex = 1;
this.Controls.Add(this.lblPDID);
this.Controls.Add(this.cmbPDID);

           //#####ProdDetailID###Int64
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

           //#####Location_ID###Int64
//属性测试100Location_ID
//属性测试100Location_ID
//属性测试100Location_ID
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

           //#####BOM_ID###Int64
//属性测试125BOM_ID
//属性测试125BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,125);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 5;
this.lblBOM_ID.Text = "标准配方";
//111======125
this.cmbBOM_ID.Location = new System.Drawing.Point(173,121);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 5;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####ID###Int64
//属性测试150ID
//属性测试150ID
//属性测试150ID
//属性测试150ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,150);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 6;
this.lblID.Text = "";
this.txtID.Location = new System.Drawing.Point(173,146);
this.txtID.Name = "txtID";
this.txtID.Size = new System.Drawing.Size(100, 21);
this.txtID.TabIndex = 6;
this.Controls.Add(this.lblID);
this.Controls.Add(this.txtID);

           //#####ParentId###Int64
//属性测试175ParentId
//属性测试175ParentId
//属性测试175ParentId
//属性测试175ParentId
this.lblParentId.AutoSize = true;
this.lblParentId.Location = new System.Drawing.Point(100,175);
this.lblParentId.Name = "lblParentId";
this.lblParentId.Size = new System.Drawing.Size(41, 12);
this.lblParentId.TabIndex = 7;
this.lblParentId.Text = "";
this.txtParentId.Location = new System.Drawing.Point(173,171);
this.txtParentId.Name = "txtParentId";
this.txtParentId.Size = new System.Drawing.Size(100, 21);
this.txtParentId.TabIndex = 7;
this.Controls.Add(this.lblParentId);
this.Controls.Add(this.txtParentId);

           //#####NetRequirement###Int32
//属性测试200NetRequirement
//属性测试200NetRequirement
//属性测试200NetRequirement
//属性测试200NetRequirement
this.lblNetRequirement.AutoSize = true;
this.lblNetRequirement.Location = new System.Drawing.Point(100,200);
this.lblNetRequirement.Name = "lblNetRequirement";
this.lblNetRequirement.Size = new System.Drawing.Size(41, 12);
this.lblNetRequirement.TabIndex = 8;
this.lblNetRequirement.Text = "净需求";
this.txtNetRequirement.Location = new System.Drawing.Point(173,196);
this.txtNetRequirement.Name = "txtNetRequirement";
this.txtNetRequirement.Size = new System.Drawing.Size(100, 21);
this.txtNetRequirement.TabIndex = 8;
this.Controls.Add(this.lblNetRequirement);
this.Controls.Add(this.txtNetRequirement);

           //#####GrossRequirement###Int32
//属性测试225GrossRequirement
//属性测试225GrossRequirement
//属性测试225GrossRequirement
//属性测试225GrossRequirement
this.lblGrossRequirement.AutoSize = true;
this.lblGrossRequirement.Location = new System.Drawing.Point(100,225);
this.lblGrossRequirement.Name = "lblGrossRequirement";
this.lblGrossRequirement.Size = new System.Drawing.Size(41, 12);
this.lblGrossRequirement.TabIndex = 9;
this.lblGrossRequirement.Text = "毛需求";
this.txtGrossRequirement.Location = new System.Drawing.Point(173,221);
this.txtGrossRequirement.Name = "txtGrossRequirement";
this.txtGrossRequirement.Size = new System.Drawing.Size(100, 21);
this.txtGrossRequirement.TabIndex = 9;
this.Controls.Add(this.lblGrossRequirement);
this.Controls.Add(this.txtGrossRequirement);

           //#####NeedQuantity###Int32
//属性测试250NeedQuantity
//属性测试250NeedQuantity
//属性测试250NeedQuantity
//属性测试250NeedQuantity
this.lblNeedQuantity.AutoSize = true;
this.lblNeedQuantity.Location = new System.Drawing.Point(100,250);
this.lblNeedQuantity.Name = "lblNeedQuantity";
this.lblNeedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblNeedQuantity.TabIndex = 10;
this.lblNeedQuantity.Text = "实际需求";
this.txtNeedQuantity.Location = new System.Drawing.Point(173,246);
this.txtNeedQuantity.Name = "txtNeedQuantity";
this.txtNeedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtNeedQuantity.TabIndex = 10;
this.Controls.Add(this.lblNeedQuantity);
this.Controls.Add(this.txtNeedQuantity);

           //#####MissingQuantity###Int32
//属性测试275MissingQuantity
//属性测试275MissingQuantity
//属性测试275MissingQuantity
//属性测试275MissingQuantity
this.lblMissingQuantity.AutoSize = true;
this.lblMissingQuantity.Location = new System.Drawing.Point(100,275);
this.lblMissingQuantity.Name = "lblMissingQuantity";
this.lblMissingQuantity.Size = new System.Drawing.Size(41, 12);
this.lblMissingQuantity.TabIndex = 11;
this.lblMissingQuantity.Text = "缺少数量";
this.txtMissingQuantity.Location = new System.Drawing.Point(173,271);
this.txtMissingQuantity.Name = "txtMissingQuantity";
this.txtMissingQuantity.Size = new System.Drawing.Size(100, 21);
this.txtMissingQuantity.TabIndex = 11;
this.Controls.Add(this.lblMissingQuantity);
this.Controls.Add(this.txtMissingQuantity);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,300);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 12;
this.lblRequirementDate.Text = "需求日期";
//111======300
this.dtpRequirementDate.Location = new System.Drawing.Point(173,296);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 12;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####BookInventory###Int32
//属性测试325BookInventory
//属性测试325BookInventory
//属性测试325BookInventory
//属性测试325BookInventory
this.lblBookInventory.AutoSize = true;
this.lblBookInventory.Location = new System.Drawing.Point(100,325);
this.lblBookInventory.Name = "lblBookInventory";
this.lblBookInventory.Size = new System.Drawing.Size(41, 12);
this.lblBookInventory.TabIndex = 13;
this.lblBookInventory.Text = "账面库存";
this.txtBookInventory.Location = new System.Drawing.Point(173,321);
this.txtBookInventory.Name = "txtBookInventory";
this.txtBookInventory.Size = new System.Drawing.Size(100, 21);
this.txtBookInventory.TabIndex = 13;
this.Controls.Add(this.lblBookInventory);
this.Controls.Add(this.txtBookInventory);

           //#####AvailableStock###Int32
//属性测试350AvailableStock
//属性测试350AvailableStock
//属性测试350AvailableStock
//属性测试350AvailableStock
this.lblAvailableStock.AutoSize = true;
this.lblAvailableStock.Location = new System.Drawing.Point(100,350);
this.lblAvailableStock.Name = "lblAvailableStock";
this.lblAvailableStock.Size = new System.Drawing.Size(41, 12);
this.lblAvailableStock.TabIndex = 14;
this.lblAvailableStock.Text = "可用库存";
this.txtAvailableStock.Location = new System.Drawing.Point(173,346);
this.txtAvailableStock.Name = "txtAvailableStock";
this.txtAvailableStock.Size = new System.Drawing.Size(100, 21);
this.txtAvailableStock.TabIndex = 14;
this.Controls.Add(this.lblAvailableStock);
this.Controls.Add(this.txtAvailableStock);

           //#####InTransitInventory###Int32
//属性测试375InTransitInventory
//属性测试375InTransitInventory
//属性测试375InTransitInventory
//属性测试375InTransitInventory
this.lblInTransitInventory.AutoSize = true;
this.lblInTransitInventory.Location = new System.Drawing.Point(100,375);
this.lblInTransitInventory.Name = "lblInTransitInventory";
this.lblInTransitInventory.Size = new System.Drawing.Size(41, 12);
this.lblInTransitInventory.TabIndex = 15;
this.lblInTransitInventory.Text = "在途库存";
this.txtInTransitInventory.Location = new System.Drawing.Point(173,371);
this.txtInTransitInventory.Name = "txtInTransitInventory";
this.txtInTransitInventory.Size = new System.Drawing.Size(100, 21);
this.txtInTransitInventory.TabIndex = 15;
this.Controls.Add(this.lblInTransitInventory);
this.Controls.Add(this.txtInTransitInventory);

           //#####MakeProcessInventory###Int32
//属性测试400MakeProcessInventory
//属性测试400MakeProcessInventory
//属性测试400MakeProcessInventory
//属性测试400MakeProcessInventory
this.lblMakeProcessInventory.AutoSize = true;
this.lblMakeProcessInventory.Location = new System.Drawing.Point(100,400);
this.lblMakeProcessInventory.Name = "lblMakeProcessInventory";
this.lblMakeProcessInventory.Size = new System.Drawing.Size(41, 12);
this.lblMakeProcessInventory.TabIndex = 16;
this.lblMakeProcessInventory.Text = "在制库存";
this.txtMakeProcessInventory.Location = new System.Drawing.Point(173,396);
this.txtMakeProcessInventory.Name = "txtMakeProcessInventory";
this.txtMakeProcessInventory.Size = new System.Drawing.Size(100, 21);
this.txtMakeProcessInventory.TabIndex = 16;
this.Controls.Add(this.lblMakeProcessInventory);
this.Controls.Add(this.txtMakeProcessInventory);

           //#####Sale_Qty###Int32
//属性测试425Sale_Qty
//属性测试425Sale_Qty
//属性测试425Sale_Qty
//属性测试425Sale_Qty
this.lblSale_Qty.AutoSize = true;
this.lblSale_Qty.Location = new System.Drawing.Point(100,425);
this.lblSale_Qty.Name = "lblSale_Qty";
this.lblSale_Qty.Size = new System.Drawing.Size(41, 12);
this.lblSale_Qty.TabIndex = 17;
this.lblSale_Qty.Text = "拟销售量";
this.txtSale_Qty.Location = new System.Drawing.Point(173,421);
this.txtSale_Qty.Name = "txtSale_Qty";
this.txtSale_Qty.Size = new System.Drawing.Size(100, 21);
this.txtSale_Qty.TabIndex = 17;
this.Controls.Add(this.lblSale_Qty);
this.Controls.Add(this.txtSale_Qty);

           //#####NotOutQty###Int32
//属性测试450NotOutQty
//属性测试450NotOutQty
//属性测试450NotOutQty
//属性测试450NotOutQty
this.lblNotOutQty.AutoSize = true;
this.lblNotOutQty.Location = new System.Drawing.Point(100,450);
this.lblNotOutQty.Name = "lblNotOutQty";
this.lblNotOutQty.Size = new System.Drawing.Size(41, 12);
this.lblNotOutQty.TabIndex = 18;
this.lblNotOutQty.Text = "未发数量";
this.txtNotOutQty.Location = new System.Drawing.Point(173,446);
this.txtNotOutQty.Name = "txtNotOutQty";
this.txtNotOutQty.Size = new System.Drawing.Size(100, 21);
this.txtNotOutQty.TabIndex = 18;
this.Controls.Add(this.lblNotOutQty);
this.Controls.Add(this.txtNotOutQty);

           //#####255Summary###String
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
                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblID );
this.Controls.Add(this.txtID );

                this.Controls.Add(this.lblParentId );
this.Controls.Add(this.txtParentId );

                this.Controls.Add(this.lblNetRequirement );
this.Controls.Add(this.txtNetRequirement );

                this.Controls.Add(this.lblGrossRequirement );
this.Controls.Add(this.txtGrossRequirement );

                this.Controls.Add(this.lblNeedQuantity );
this.Controls.Add(this.txtNeedQuantity );

                this.Controls.Add(this.lblMissingQuantity );
this.Controls.Add(this.txtMissingQuantity );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblBookInventory );
this.Controls.Add(this.txtBookInventory );

                this.Controls.Add(this.lblAvailableStock );
this.Controls.Add(this.txtAvailableStock );

                this.Controls.Add(this.lblInTransitInventory );
this.Controls.Add(this.txtInTransitInventory );

                this.Controls.Add(this.lblMakeProcessInventory );
this.Controls.Add(this.txtMakeProcessInventory );

                this.Controls.Add(this.lblSale_Qty );
this.Controls.Add(this.txtSale_Qty );

                this.Controls.Add(this.lblNotOutQty );
this.Controls.Add(this.txtNotOutQty );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_ProductionDemandDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProductionDemandDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPDID;
private Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblID;
private Krypton.Toolkit.KryptonTextBox txtID;

    
        
              private Krypton.Toolkit.KryptonLabel lblParentId;
private Krypton.Toolkit.KryptonTextBox txtParentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblNetRequirement;
private Krypton.Toolkit.KryptonTextBox txtNetRequirement;

    
        
              private Krypton.Toolkit.KryptonLabel lblGrossRequirement;
private Krypton.Toolkit.KryptonTextBox txtGrossRequirement;

    
        
              private Krypton.Toolkit.KryptonLabel lblNeedQuantity;
private Krypton.Toolkit.KryptonTextBox txtNeedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblMissingQuantity;
private Krypton.Toolkit.KryptonTextBox txtMissingQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblBookInventory;
private Krypton.Toolkit.KryptonTextBox txtBookInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblAvailableStock;
private Krypton.Toolkit.KryptonTextBox txtAvailableStock;

    
        
              private Krypton.Toolkit.KryptonLabel lblInTransitInventory;
private Krypton.Toolkit.KryptonTextBox txtInTransitInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblMakeProcessInventory;
private Krypton.Toolkit.KryptonTextBox txtMakeProcessInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblSale_Qty;
private Krypton.Toolkit.KryptonTextBox txtSale_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotOutQty;
private Krypton.Toolkit.KryptonTextBox txtNotOutQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

