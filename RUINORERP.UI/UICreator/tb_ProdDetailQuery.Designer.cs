
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    partial class tb_ProdDetailQuery
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
     
     this.lblProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBarCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtImagesPath.Multiline = true;


this.lblWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblStandard_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStandard_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransfer_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransfer_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWholesale_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWholesale_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMarket_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMarket_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSalePublish.Values.Text ="";
this.chkSalePublish.Checked = true;
this.chkSalePublish.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";
this.chkIs_available.Checked = true;
this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdBaseID###Int64
//属性测试25ProdBaseID
//属性测试25ProdBaseID
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,25);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 1;
this.lblProdBaseID.Text = "货品主信息";
//111======25
this.cmbProdBaseID.Location = new System.Drawing.Point(173,21);
this.cmbProdBaseID.Name ="cmbProdBaseID";
this.cmbProdBaseID.Size = new System.Drawing.Size(100, 21);
this.cmbProdBaseID.TabIndex = 1;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.cmbProdBaseID);

           //#####BOM_ID###Int64
//属性测试50BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,50);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 2;
this.lblBOM_ID.Text = "标准配方";
//111======50
this.cmbBOM_ID.Location = new System.Drawing.Point(173,46);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 2;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,75);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 3;
this.lblSKU.Text = "SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,71);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 3;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####50BarCode###String
this.lblBarCode.AutoSize = true;
this.lblBarCode.Location = new System.Drawing.Point(100,100);
this.lblBarCode.Name = "lblBarCode";
this.lblBarCode.Size = new System.Drawing.Size(41, 12);
this.lblBarCode.TabIndex = 4;
this.lblBarCode.Text = "条码";
this.txtBarCode.Location = new System.Drawing.Point(173,96);
this.txtBarCode.Name = "txtBarCode";
this.txtBarCode.Size = new System.Drawing.Size(100, 21);
this.txtBarCode.TabIndex = 4;
this.Controls.Add(this.lblBarCode);
this.Controls.Add(this.txtBarCode);

           //#####2000ImagesPath###String
this.lblImagesPath.AutoSize = true;
this.lblImagesPath.Location = new System.Drawing.Point(100,125);
this.lblImagesPath.Name = "lblImagesPath";
this.lblImagesPath.Size = new System.Drawing.Size(41, 12);
this.lblImagesPath.TabIndex = 5;
this.lblImagesPath.Text = "产品图片";
this.txtImagesPath.Location = new System.Drawing.Point(173,121);
this.txtImagesPath.Name = "txtImagesPath";
this.txtImagesPath.Size = new System.Drawing.Size(100, 21);
this.txtImagesPath.TabIndex = 5;
this.Controls.Add(this.lblImagesPath);
this.Controls.Add(this.txtImagesPath);

           //#####2147483647Images###Binary

           //#####Weight###Decimal
this.lblWeight.AutoSize = true;
this.lblWeight.Location = new System.Drawing.Point(100,175);
this.lblWeight.Name = "lblWeight";
this.lblWeight.Size = new System.Drawing.Size(41, 12);
this.lblWeight.TabIndex = 7;
this.lblWeight.Text = "重量（千克）";
//111======175
this.txtWeight.Location = new System.Drawing.Point(173,171);
this.txtWeight.Name ="txtWeight";
this.txtWeight.Size = new System.Drawing.Size(100, 21);
this.txtWeight.TabIndex = 7;
this.Controls.Add(this.lblWeight);
this.Controls.Add(this.txtWeight);

           //#####Standard_Price###Decimal
this.lblStandard_Price.AutoSize = true;
this.lblStandard_Price.Location = new System.Drawing.Point(100,200);
this.lblStandard_Price.Name = "lblStandard_Price";
this.lblStandard_Price.Size = new System.Drawing.Size(41, 12);
this.lblStandard_Price.TabIndex = 8;
this.lblStandard_Price.Text = "标准价";
//111======200
this.txtStandard_Price.Location = new System.Drawing.Point(173,196);
this.txtStandard_Price.Name ="txtStandard_Price";
this.txtStandard_Price.Size = new System.Drawing.Size(100, 21);
this.txtStandard_Price.TabIndex = 8;
this.Controls.Add(this.lblStandard_Price);
this.Controls.Add(this.txtStandard_Price);

           //#####Transfer_Price###Decimal
this.lblTransfer_Price.AutoSize = true;
this.lblTransfer_Price.Location = new System.Drawing.Point(100,225);
this.lblTransfer_Price.Name = "lblTransfer_Price";
this.lblTransfer_Price.Size = new System.Drawing.Size(41, 12);
this.lblTransfer_Price.TabIndex = 9;
this.lblTransfer_Price.Text = "调拨价格";
//111======225
this.txtTransfer_Price.Location = new System.Drawing.Point(173,221);
this.txtTransfer_Price.Name ="txtTransfer_Price";
this.txtTransfer_Price.Size = new System.Drawing.Size(100, 21);
this.txtTransfer_Price.TabIndex = 9;
this.Controls.Add(this.lblTransfer_Price);
this.Controls.Add(this.txtTransfer_Price);

           //#####Wholesale_Price###Decimal
this.lblWholesale_Price.AutoSize = true;
this.lblWholesale_Price.Location = new System.Drawing.Point(100,250);
this.lblWholesale_Price.Name = "lblWholesale_Price";
this.lblWholesale_Price.Size = new System.Drawing.Size(41, 12);
this.lblWholesale_Price.TabIndex = 10;
this.lblWholesale_Price.Text = "批发价格";
//111======250
this.txtWholesale_Price.Location = new System.Drawing.Point(173,246);
this.txtWholesale_Price.Name ="txtWholesale_Price";
this.txtWholesale_Price.Size = new System.Drawing.Size(100, 21);
this.txtWholesale_Price.TabIndex = 10;
this.Controls.Add(this.lblWholesale_Price);
this.Controls.Add(this.txtWholesale_Price);

           //#####Market_Price###Decimal
this.lblMarket_Price.AutoSize = true;
this.lblMarket_Price.Location = new System.Drawing.Point(100,275);
this.lblMarket_Price.Name = "lblMarket_Price";
this.lblMarket_Price.Size = new System.Drawing.Size(41, 12);
this.lblMarket_Price.TabIndex = 11;
this.lblMarket_Price.Text = "市场零售价";
//111======275
this.txtMarket_Price.Location = new System.Drawing.Point(173,271);
this.txtMarket_Price.Name ="txtMarket_Price";
this.txtMarket_Price.Size = new System.Drawing.Size(100, 21);
this.txtMarket_Price.TabIndex = 11;
this.Controls.Add(this.lblMarket_Price);
this.Controls.Add(this.txtMarket_Price);

           //#####Discount_Price###Decimal
this.lblDiscount_Price.AutoSize = true;
this.lblDiscount_Price.Location = new System.Drawing.Point(100,300);
this.lblDiscount_Price.Name = "lblDiscount_Price";
this.lblDiscount_Price.Size = new System.Drawing.Size(41, 12);
this.lblDiscount_Price.TabIndex = 12;
this.lblDiscount_Price.Text = "折扣价格";
//111======300
this.txtDiscount_Price.Location = new System.Drawing.Point(173,296);
this.txtDiscount_Price.Name ="txtDiscount_Price";
this.txtDiscount_Price.Size = new System.Drawing.Size(100, 21);
this.txtDiscount_Price.TabIndex = 12;
this.Controls.Add(this.lblDiscount_Price);
this.Controls.Add(this.txtDiscount_Price);

           //#####2147483647Image###Binary

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,350);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 14;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,346);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 14;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####SalePublish###Boolean
this.lblSalePublish.AutoSize = true;
this.lblSalePublish.Location = new System.Drawing.Point(100,375);
this.lblSalePublish.Name = "lblSalePublish";
this.lblSalePublish.Size = new System.Drawing.Size(41, 12);
this.lblSalePublish.TabIndex = 15;
this.lblSalePublish.Text = "参与分销";
this.chkSalePublish.Location = new System.Drawing.Point(173,371);
this.chkSalePublish.Name = "chkSalePublish";
this.chkSalePublish.Size = new System.Drawing.Size(100, 21);
this.chkSalePublish.TabIndex = 15;
this.Controls.Add(this.lblSalePublish);
this.Controls.Add(this.chkSalePublish);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,400);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 16;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,396);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 16;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,425);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 17;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,421);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 17;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,450);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 18;
this.lblCreated_at.Text = "创建时间";
//111======450
this.dtpCreated_at.Location = new System.Drawing.Point(173,446);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 18;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试475Created_by
//属性测试475Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,500);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 20;
this.lblModified_at.Text = "修改时间";
//111======500
this.dtpModified_at.Location = new System.Drawing.Point(173,496);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 20;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试525Modified_by
//属性测试525Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,550);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 22;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,546);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 22;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试575DataStatus
//属性测试575DataStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.cmbProdBaseID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblBarCode );
this.Controls.Add(this.txtBarCode );

                this.Controls.Add(this.lblImagesPath );
this.Controls.Add(this.txtImagesPath );

                
                this.Controls.Add(this.lblWeight );
this.Controls.Add(this.txtWeight );

                this.Controls.Add(this.lblStandard_Price );
this.Controls.Add(this.txtStandard_Price );

                this.Controls.Add(this.lblTransfer_Price );
this.Controls.Add(this.txtTransfer_Price );

                this.Controls.Add(this.lblWholesale_Price );
this.Controls.Add(this.txtWholesale_Price );

                this.Controls.Add(this.lblMarket_Price );
this.Controls.Add(this.txtMarket_Price );

                this.Controls.Add(this.lblDiscount_Price );
this.Controls.Add(this.txtDiscount_Price );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblSalePublish );
this.Controls.Add(this.chkSalePublish );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                    
            this.Name = "tb_ProdDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdBaseID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdBaseID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblImagesPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtImagesPath;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStandard_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStandard_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransfer_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransfer_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWholesale_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWholesale_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMarket_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMarket_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount_Price;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalePublish;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSalePublish;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
    
   
 





    }
}


