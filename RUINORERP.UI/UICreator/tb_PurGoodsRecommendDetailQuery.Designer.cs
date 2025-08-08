
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
    partial class tb_PurGoodsRecommendDetailQuery
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
     
     this.lblPDID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRecommendPurPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRecommendPurPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####RecommendQty###Int32
//属性测试200RecommendQty
//属性测试200RecommendQty
//属性测试200RecommendQty
//属性测试200RecommendQty

           //#####RequirementQty###Int32
//属性测试225RequirementQty
//属性测试225RequirementQty
//属性测试225RequirementQty
//属性测试225RequirementQty

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

           //#####RefBillID###Int64
//属性测试350RefBillID
//属性测试350RefBillID
//属性测试350RefBillID
//属性测试350RefBillID

           //#####PDCID_RowID###Int64
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID
//属性测试375PDCID_RowID

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                
                
                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRefBillNO );
this.Controls.Add(this.txtRefBillNO );

                
                
                
                    
            this.Name = "tb_PurGoodsRecommendDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRecommendPurPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRecommendPurPrice;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRefBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRefBillNO;

    
        
              
    
        
              
    
        
              
    
    
   
 





    }
}


