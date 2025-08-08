// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:01
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产需求分析目标对象明细
    /// </summary>
    partial class tb_ProductionDemandTargetDetailEdit
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

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblNeedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtNeedQuantity = new Krypton.Toolkit.KryptonTextBox();

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

this.lblSaleQty = new Krypton.Toolkit.KryptonLabel();
this.txtSaleQty = new Krypton.Toolkit.KryptonTextBox();

this.lblNotIssueMaterialQty = new Krypton.Toolkit.KryptonLabel();
this.txtNotIssueMaterialQty = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblPPCID = new Krypton.Toolkit.KryptonLabel();
this.txtPPCID = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    
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

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####BOM_ID###Int64
//属性测试100BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,100);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 4;
this.lblBOM_ID.Text = "配方名称";
//111======100
this.cmbBOM_ID.Location = new System.Drawing.Point(173,96);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 4;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####NeedQuantity###Int32
//属性测试125NeedQuantity
//属性测试125NeedQuantity
//属性测试125NeedQuantity
//属性测试125NeedQuantity
this.lblNeedQuantity.AutoSize = true;
this.lblNeedQuantity.Location = new System.Drawing.Point(100,125);
this.lblNeedQuantity.Name = "lblNeedQuantity";
this.lblNeedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblNeedQuantity.TabIndex = 5;
this.lblNeedQuantity.Text = "需求数量";
this.txtNeedQuantity.Location = new System.Drawing.Point(173,121);
this.txtNeedQuantity.Name = "txtNeedQuantity";
this.txtNeedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtNeedQuantity.TabIndex = 5;
this.Controls.Add(this.lblNeedQuantity);
this.Controls.Add(this.txtNeedQuantity);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,150);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 6;
this.lblRequirementDate.Text = "需求日期";
//111======150
this.dtpRequirementDate.Location = new System.Drawing.Point(173,146);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 6;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####BookInventory###Int32
//属性测试175BookInventory
//属性测试175BookInventory
//属性测试175BookInventory
//属性测试175BookInventory
this.lblBookInventory.AutoSize = true;
this.lblBookInventory.Location = new System.Drawing.Point(100,175);
this.lblBookInventory.Name = "lblBookInventory";
this.lblBookInventory.Size = new System.Drawing.Size(41, 12);
this.lblBookInventory.TabIndex = 7;
this.lblBookInventory.Text = "账面库存";
this.txtBookInventory.Location = new System.Drawing.Point(173,171);
this.txtBookInventory.Name = "txtBookInventory";
this.txtBookInventory.Size = new System.Drawing.Size(100, 21);
this.txtBookInventory.TabIndex = 7;
this.Controls.Add(this.lblBookInventory);
this.Controls.Add(this.txtBookInventory);

           //#####AvailableStock###Int32
//属性测试200AvailableStock
//属性测试200AvailableStock
//属性测试200AvailableStock
//属性测试200AvailableStock
this.lblAvailableStock.AutoSize = true;
this.lblAvailableStock.Location = new System.Drawing.Point(100,200);
this.lblAvailableStock.Name = "lblAvailableStock";
this.lblAvailableStock.Size = new System.Drawing.Size(41, 12);
this.lblAvailableStock.TabIndex = 8;
this.lblAvailableStock.Text = "可用库存";
this.txtAvailableStock.Location = new System.Drawing.Point(173,196);
this.txtAvailableStock.Name = "txtAvailableStock";
this.txtAvailableStock.Size = new System.Drawing.Size(100, 21);
this.txtAvailableStock.TabIndex = 8;
this.Controls.Add(this.lblAvailableStock);
this.Controls.Add(this.txtAvailableStock);

           //#####InTransitInventory###Int32
//属性测试225InTransitInventory
//属性测试225InTransitInventory
//属性测试225InTransitInventory
//属性测试225InTransitInventory
this.lblInTransitInventory.AutoSize = true;
this.lblInTransitInventory.Location = new System.Drawing.Point(100,225);
this.lblInTransitInventory.Name = "lblInTransitInventory";
this.lblInTransitInventory.Size = new System.Drawing.Size(41, 12);
this.lblInTransitInventory.TabIndex = 9;
this.lblInTransitInventory.Text = "在途库存";
this.txtInTransitInventory.Location = new System.Drawing.Point(173,221);
this.txtInTransitInventory.Name = "txtInTransitInventory";
this.txtInTransitInventory.Size = new System.Drawing.Size(100, 21);
this.txtInTransitInventory.TabIndex = 9;
this.Controls.Add(this.lblInTransitInventory);
this.Controls.Add(this.txtInTransitInventory);

           //#####MakeProcessInventory###Int32
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory
this.lblMakeProcessInventory.AutoSize = true;
this.lblMakeProcessInventory.Location = new System.Drawing.Point(100,250);
this.lblMakeProcessInventory.Name = "lblMakeProcessInventory";
this.lblMakeProcessInventory.Size = new System.Drawing.Size(41, 12);
this.lblMakeProcessInventory.TabIndex = 10;
this.lblMakeProcessInventory.Text = "在制库存";
this.txtMakeProcessInventory.Location = new System.Drawing.Point(173,246);
this.txtMakeProcessInventory.Name = "txtMakeProcessInventory";
this.txtMakeProcessInventory.Size = new System.Drawing.Size(100, 21);
this.txtMakeProcessInventory.TabIndex = 10;
this.Controls.Add(this.lblMakeProcessInventory);
this.Controls.Add(this.txtMakeProcessInventory);

           //#####SaleQty###Int32
//属性测试275SaleQty
//属性测试275SaleQty
//属性测试275SaleQty
//属性测试275SaleQty
this.lblSaleQty.AutoSize = true;
this.lblSaleQty.Location = new System.Drawing.Point(100,275);
this.lblSaleQty.Name = "lblSaleQty";
this.lblSaleQty.Size = new System.Drawing.Size(41, 12);
this.lblSaleQty.TabIndex = 11;
this.lblSaleQty.Text = "拟销售量";
this.txtSaleQty.Location = new System.Drawing.Point(173,271);
this.txtSaleQty.Name = "txtSaleQty";
this.txtSaleQty.Size = new System.Drawing.Size(100, 21);
this.txtSaleQty.TabIndex = 11;
this.Controls.Add(this.lblSaleQty);
this.Controls.Add(this.txtSaleQty);

           //#####NotIssueMaterialQty###Int32
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty
this.lblNotIssueMaterialQty.AutoSize = true;
this.lblNotIssueMaterialQty.Location = new System.Drawing.Point(100,300);
this.lblNotIssueMaterialQty.Name = "lblNotIssueMaterialQty";
this.lblNotIssueMaterialQty.Size = new System.Drawing.Size(41, 12);
this.lblNotIssueMaterialQty.TabIndex = 12;
this.lblNotIssueMaterialQty.Text = "未发料量";
this.txtNotIssueMaterialQty.Location = new System.Drawing.Point(173,296);
this.txtNotIssueMaterialQty.Name = "txtNotIssueMaterialQty";
this.txtNotIssueMaterialQty.Size = new System.Drawing.Size(100, 21);
this.txtNotIssueMaterialQty.TabIndex = 12;
this.Controls.Add(this.lblNotIssueMaterialQty);
this.Controls.Add(this.txtNotIssueMaterialQty);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,325);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 13;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,321);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 13;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####PPCID###Int64
//属性测试350PPCID
//属性测试350PPCID
//属性测试350PPCID
//属性测试350PPCID
this.lblPPCID.AutoSize = true;
this.lblPPCID.Location = new System.Drawing.Point(100,350);
this.lblPPCID.Name = "lblPPCID";
this.lblPPCID.Size = new System.Drawing.Size(41, 12);
this.lblPPCID.TabIndex = 14;
this.lblPPCID.Text = "";
this.txtPPCID.Location = new System.Drawing.Point(173,346);
this.txtPPCID.Name = "txtPPCID";
this.txtPPCID.Size = new System.Drawing.Size(100, 21);
this.txtPPCID.TabIndex = 14;
this.Controls.Add(this.lblPPCID);
this.Controls.Add(this.txtPPCID);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,375);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 15;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,371);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 15;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,400);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 16;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,396);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 16;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

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
           // this.kryptonPanel1.TabIndex = 16;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblNeedQuantity );
this.Controls.Add(this.txtNeedQuantity );

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

                this.Controls.Add(this.lblSaleQty );
this.Controls.Add(this.txtSaleQty );

                this.Controls.Add(this.lblNotIssueMaterialQty );
this.Controls.Add(this.txtNotIssueMaterialQty );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblPPCID );
this.Controls.Add(this.txtPPCID );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                            // 
            // "tb_ProductionDemandTargetDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProductionDemandTargetDetailEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblNeedQuantity;
private Krypton.Toolkit.KryptonTextBox txtNeedQuantity;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleQty;
private Krypton.Toolkit.KryptonTextBox txtSaleQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotIssueMaterialQty;
private Krypton.Toolkit.KryptonTextBox txtNotIssueMaterialQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblPPCID;
private Krypton.Toolkit.KryptonTextBox txtPPCID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

