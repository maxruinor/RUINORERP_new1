// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:06
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购商品建议
    /// </summary>
    partial class tb_PurGoodsRecommendDetailEdit
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

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRecommendPurPrice = new Krypton.Toolkit.KryptonLabel();
this.txtRecommendPurPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblActualRequiredQty = new Krypton.Toolkit.KryptonLabel();
this.txtActualRequiredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblRecommendQty = new Krypton.Toolkit.KryptonLabel();
this.txtRecommendQty = new Krypton.Toolkit.KryptonTextBox();

this.lblRequirementQty = new Krypton.Toolkit.KryptonLabel();
this.txtRequirementQty = new Krypton.Toolkit.KryptonTextBox();

this.lblRequirementDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRefBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillNO = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBillType = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillType = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBillID = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillID = new Krypton.Toolkit.KryptonTextBox();

this.lblPDCID_RowID = new Krypton.Toolkit.KryptonLabel();
this.txtPDCID_RowID = new Krypton.Toolkit.KryptonTextBox();

    
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

           //#####CustomerVendor_ID###Int64
//属性测试125CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,125);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 5;
this.lblCustomerVendor_ID.Text = "供应商";
//111======125
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,121);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 5;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####RecommendPurPrice###Decimal
this.lblRecommendPurPrice.AutoSize = true;
this.lblRecommendPurPrice.Location = new System.Drawing.Point(100,150);
this.lblRecommendPurPrice.Name = "lblRecommendPurPrice";
this.lblRecommendPurPrice.Size = new System.Drawing.Size(41, 12);
this.lblRecommendPurPrice.TabIndex = 6;
this.lblRecommendPurPrice.Text = "建议采购价";
//111======150
this.txtRecommendPurPrice.Location = new System.Drawing.Point(173,146);
this.txtRecommendPurPrice.Name ="txtRecommendPurPrice";
this.txtRecommendPurPrice.Size = new System.Drawing.Size(100, 21);
this.txtRecommendPurPrice.TabIndex = 6;
this.Controls.Add(this.lblRecommendPurPrice);
this.Controls.Add(this.txtRecommendPurPrice);

           //#####ActualRequiredQty###Int32
//属性测试175ActualRequiredQty
//属性测试175ActualRequiredQty
//属性测试175ActualRequiredQty
//属性测试175ActualRequiredQty
this.lblActualRequiredQty.AutoSize = true;
this.lblActualRequiredQty.Location = new System.Drawing.Point(100,175);
this.lblActualRequiredQty.Name = "lblActualRequiredQty";
this.lblActualRequiredQty.Size = new System.Drawing.Size(41, 12);
this.lblActualRequiredQty.TabIndex = 7;
this.lblActualRequiredQty.Text = "需求数量";
this.txtActualRequiredQty.Location = new System.Drawing.Point(173,171);
this.txtActualRequiredQty.Name = "txtActualRequiredQty";
this.txtActualRequiredQty.Size = new System.Drawing.Size(100, 21);
this.txtActualRequiredQty.TabIndex = 7;
this.Controls.Add(this.lblActualRequiredQty);
this.Controls.Add(this.txtActualRequiredQty);

           //#####RecommendQty###Int32
//属性测试200RecommendQty
//属性测试200RecommendQty
//属性测试200RecommendQty
//属性测试200RecommendQty
this.lblRecommendQty.AutoSize = true;
this.lblRecommendQty.Location = new System.Drawing.Point(100,200);
this.lblRecommendQty.Name = "lblRecommendQty";
this.lblRecommendQty.Size = new System.Drawing.Size(41, 12);
this.lblRecommendQty.TabIndex = 8;
this.lblRecommendQty.Text = "建议量";
this.txtRecommendQty.Location = new System.Drawing.Point(173,196);
this.txtRecommendQty.Name = "txtRecommendQty";
this.txtRecommendQty.Size = new System.Drawing.Size(100, 21);
this.txtRecommendQty.TabIndex = 8;
this.Controls.Add(this.lblRecommendQty);
this.Controls.Add(this.txtRecommendQty);

           //#####RequirementQty###Int32
//属性测试225RequirementQty
//属性测试225RequirementQty
//属性测试225RequirementQty
//属性测试225RequirementQty
this.lblRequirementQty.AutoSize = true;
this.lblRequirementQty.Location = new System.Drawing.Point(100,225);
this.lblRequirementQty.Name = "lblRequirementQty";
this.lblRequirementQty.Size = new System.Drawing.Size(41, 12);
this.lblRequirementQty.TabIndex = 9;
this.lblRequirementQty.Text = "请购量";
this.txtRequirementQty.Location = new System.Drawing.Point(173,221);
this.txtRequirementQty.Name = "txtRequirementQty";
this.txtRequirementQty.Size = new System.Drawing.Size(100, 21);
this.txtRequirementQty.TabIndex = 9;
this.Controls.Add(this.lblRequirementQty);
this.Controls.Add(this.txtRequirementQty);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,250);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 10;
this.lblRequirementDate.Text = "需求日期";
//111======250
this.dtpRequirementDate.Location = new System.Drawing.Point(173,246);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 10;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,275);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 11;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,271);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 11;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####100RefBillNO###String
this.lblRefBillNO.AutoSize = true;
this.lblRefBillNO.Location = new System.Drawing.Point(100,300);
this.lblRefBillNO.Name = "lblRefBillNO";
this.lblRefBillNO.Size = new System.Drawing.Size(41, 12);
this.lblRefBillNO.TabIndex = 12;
this.lblRefBillNO.Text = "生成单号";
this.txtRefBillNO.Location = new System.Drawing.Point(173,296);
this.txtRefBillNO.Name = "txtRefBillNO";
this.txtRefBillNO.Size = new System.Drawing.Size(100, 21);
this.txtRefBillNO.TabIndex = 12;
this.Controls.Add(this.lblRefBillNO);
this.Controls.Add(this.txtRefBillNO);

           //#####RefBillType###Int64
//属性测试325RefBillType
//属性测试325RefBillType
//属性测试325RefBillType
//属性测试325RefBillType
this.lblRefBillType.AutoSize = true;
this.lblRefBillType.Location = new System.Drawing.Point(100,325);
this.lblRefBillType.Name = "lblRefBillType";
this.lblRefBillType.Size = new System.Drawing.Size(41, 12);
this.lblRefBillType.TabIndex = 13;
this.lblRefBillType.Text = "单据类型";
this.txtRefBillType.Location = new System.Drawing.Point(173,321);
this.txtRefBillType.Name = "txtRefBillType";
this.txtRefBillType.Size = new System.Drawing.Size(100, 21);
this.txtRefBillType.TabIndex = 13;
this.Controls.Add(this.lblRefBillType);
this.Controls.Add(this.txtRefBillType);

           //#####RefBillID###Int64
//属性测试350RefBillID
//属性测试350RefBillID
//属性测试350RefBillID
//属性测试350RefBillID
this.lblRefBillID.AutoSize = true;
this.lblRefBillID.Location = new System.Drawing.Point(100,350);
this.lblRefBillID.Name = "lblRefBillID";
this.lblRefBillID.Size = new System.Drawing.Size(41, 12);
this.lblRefBillID.TabIndex = 14;
this.lblRefBillID.Text = "生成单据";
this.txtRefBillID.Location = new System.Drawing.Point(173,346);
this.txtRefBillID.Name = "txtRefBillID";
this.txtRefBillID.Size = new System.Drawing.Size(100, 21);
this.txtRefBillID.TabIndex = 14;
this.Controls.Add(this.lblRefBillID);
this.Controls.Add(this.txtRefBillID);

           //#####PDCID_RowID###Int64
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID
this.lblPDCID_RowID.AutoSize = true;
this.lblPDCID_RowID.Location = new System.Drawing.Point(100,375);
this.lblPDCID_RowID.Name = "lblPDCID_RowID";
this.lblPDCID_RowID.Size = new System.Drawing.Size(41, 12);
this.lblPDCID_RowID.TabIndex = 15;
this.lblPDCID_RowID.Text = "生成单据";
this.txtPDCID_RowID.Location = new System.Drawing.Point(173,371);
this.txtPDCID_RowID.Name = "txtPDCID_RowID";
this.txtPDCID_RowID.Size = new System.Drawing.Size(100, 21);
this.txtPDCID_RowID.TabIndex = 15;
this.Controls.Add(this.lblPDCID_RowID);
this.Controls.Add(this.txtPDCID_RowID);

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
           // this.kryptonPanel1.TabIndex = 15;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblRecommendPurPrice );
this.Controls.Add(this.txtRecommendPurPrice );

                this.Controls.Add(this.lblActualRequiredQty );
this.Controls.Add(this.txtActualRequiredQty );

                this.Controls.Add(this.lblRecommendQty );
this.Controls.Add(this.txtRecommendQty );

                this.Controls.Add(this.lblRequirementQty );
this.Controls.Add(this.txtRequirementQty );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRefBillNO );
this.Controls.Add(this.txtRefBillNO );

                this.Controls.Add(this.lblRefBillType );
this.Controls.Add(this.txtRefBillType );

                this.Controls.Add(this.lblRefBillID );
this.Controls.Add(this.txtRefBillID );

                this.Controls.Add(this.lblPDCID_RowID );
this.Controls.Add(this.txtPDCID_RowID );

                            // 
            // "tb_PurGoodsRecommendDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PurGoodsRecommendDetailEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRecommendPurPrice;
private Krypton.Toolkit.KryptonTextBox txtRecommendPurPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblActualRequiredQty;
private Krypton.Toolkit.KryptonTextBox txtActualRequiredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblRecommendQty;
private Krypton.Toolkit.KryptonTextBox txtRecommendQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementQty;
private Krypton.Toolkit.KryptonTextBox txtRequirementQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillNO;
private Krypton.Toolkit.KryptonTextBox txtRefBillNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillType;
private Krypton.Toolkit.KryptonTextBox txtRefBillType;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillID;
private Krypton.Toolkit.KryptonTextBox txtRefBillID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPDCID_RowID;
private Krypton.Toolkit.KryptonTextBox txtPDCID_RowID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

