
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库存视图-得从表取不能视图套视图
    /// </summary>
    partial class View_InventoryQuery
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
     
     
this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblprop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtprop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();








this.lblBrand = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBrand = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();







this.lblInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInv_SubtotalCostMoney = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_SubtotalCostMoney = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Inventory_ID###Int64

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,50);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 2;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,46);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 2;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,75);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 3;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,71);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 3;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,100);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 4;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,96);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 4;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,125);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 5;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,121);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 5;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,150);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 6;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,146);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 6;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,175);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 7;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,171);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 7;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Quantity###Int32

           //#####Type_ID###Int64

           //#####Unit_ID###Int64

           //#####Category_ID###Int64

           //#####CustomerVendor_ID###Int64

           //#####DepartmentID###Int64

           //#####SourceType###Int32

           //#####50Brand###String
this.lblBrand.AutoSize = true;
this.lblBrand.Location = new System.Drawing.Point(100,375);
this.lblBrand.Name = "lblBrand";
this.lblBrand.Size = new System.Drawing.Size(41, 12);
this.lblBrand.TabIndex = 15;
this.lblBrand.Text = "";
this.txtBrand.Location = new System.Drawing.Point(173,371);
this.txtBrand.Name = "txtBrand";
this.txtBrand.Size = new System.Drawing.Size(100, 21);
this.txtBrand.TabIndex = 15;
this.Controls.Add(this.lblBrand);
this.Controls.Add(this.txtBrand);

           //#####Rack_ID###Int64

           //#####Alert_Quantity###Int32

           //#####On_the_way_Qty###Int32

           //#####Sale_Qty###Int32

           //#####MakingQty###Int32

           //#####NotOutQty###Int32

           //#####Inv_Cost###Decimal
this.lblInv_Cost.AutoSize = true;
this.lblInv_Cost.Location = new System.Drawing.Point(100,550);
this.lblInv_Cost.Name = "lblInv_Cost";
this.lblInv_Cost.Size = new System.Drawing.Size(41, 12);
this.lblInv_Cost.TabIndex = 22;
this.lblInv_Cost.Text = "";
//111======550
this.txtInv_Cost.Location = new System.Drawing.Point(173,546);
this.txtInv_Cost.Name ="txtInv_Cost";
this.txtInv_Cost.Size = new System.Drawing.Size(100, 21);
this.txtInv_Cost.TabIndex = 22;
this.Controls.Add(this.lblInv_Cost);
this.Controls.Add(this.txtInv_Cost);

           //#####Inv_SubtotalCostMoney###Decimal
this.lblInv_SubtotalCostMoney.AutoSize = true;
this.lblInv_SubtotalCostMoney.Location = new System.Drawing.Point(100,575);
this.lblInv_SubtotalCostMoney.Name = "lblInv_SubtotalCostMoney";
this.lblInv_SubtotalCostMoney.Size = new System.Drawing.Size(41, 12);
this.lblInv_SubtotalCostMoney.TabIndex = 23;
this.lblInv_SubtotalCostMoney.Text = "";
//111======575
this.txtInv_SubtotalCostMoney.Location = new System.Drawing.Point(173,571);
this.txtInv_SubtotalCostMoney.Name ="txtInv_SubtotalCostMoney";
this.txtInv_SubtotalCostMoney.Size = new System.Drawing.Size(100, 21);
this.txtInv_SubtotalCostMoney.TabIndex = 23;
this.Controls.Add(this.lblInv_SubtotalCostMoney);
this.Controls.Add(this.txtInv_SubtotalCostMoney);

           //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####BOM_ID###Int64

           //#####LatestStorageTime###DateTime
this.lblLatestStorageTime.AutoSize = true;
this.lblLatestStorageTime.Location = new System.Drawing.Point(100,675);
this.lblLatestStorageTime.Name = "lblLatestStorageTime";
this.lblLatestStorageTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestStorageTime.TabIndex = 27;
this.lblLatestStorageTime.Text = "";
//111======675
this.dtpLatestStorageTime.Location = new System.Drawing.Point(173,671);
this.dtpLatestStorageTime.Name ="dtpLatestStorageTime";
this.dtpLatestStorageTime.ShowCheckBox =true;
this.dtpLatestStorageTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestStorageTime.TabIndex = 27;
this.Controls.Add(this.lblLatestStorageTime);
this.Controls.Add(this.dtpLatestStorageTime);

           //#####LatestOutboundTime###DateTime
this.lblLatestOutboundTime.AutoSize = true;
this.lblLatestOutboundTime.Location = new System.Drawing.Point(100,700);
this.lblLatestOutboundTime.Name = "lblLatestOutboundTime";
this.lblLatestOutboundTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestOutboundTime.TabIndex = 28;
this.lblLatestOutboundTime.Text = "";
//111======700
this.dtpLatestOutboundTime.Location = new System.Drawing.Point(173,696);
this.dtpLatestOutboundTime.Name ="dtpLatestOutboundTime";
this.dtpLatestOutboundTime.ShowCheckBox =true;
this.dtpLatestOutboundTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestOutboundTime.TabIndex = 28;
this.Controls.Add(this.lblLatestOutboundTime);
this.Controls.Add(this.dtpLatestOutboundTime);

           //#####LastInventoryDate###DateTime
this.lblLastInventoryDate.AutoSize = true;
this.lblLastInventoryDate.Location = new System.Drawing.Point(100,725);
this.lblLastInventoryDate.Name = "lblLastInventoryDate";
this.lblLastInventoryDate.Size = new System.Drawing.Size(41, 12);
this.lblLastInventoryDate.TabIndex = 29;
this.lblLastInventoryDate.Text = "";
//111======725
this.dtpLastInventoryDate.Location = new System.Drawing.Point(173,721);
this.dtpLastInventoryDate.Name ="dtpLastInventoryDate";
this.dtpLastInventoryDate.ShowCheckBox =true;
this.dtpLastInventoryDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastInventoryDate.TabIndex = 29;
this.Controls.Add(this.lblLastInventoryDate);
this.Controls.Add(this.dtpLastInventoryDate);

           //#####250Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,750);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 30;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,746);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 30;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                
                
                
                
                
                this.Controls.Add(this.lblBrand );
this.Controls.Add(this.txtBrand );

                
                
                
                
                
                
                this.Controls.Add(this.lblInv_Cost );
this.Controls.Add(this.txtInv_Cost );

                this.Controls.Add(this.lblInv_SubtotalCostMoney );
this.Controls.Add(this.txtInv_SubtotalCostMoney );

                
                
                
                this.Controls.Add(this.lblLatestStorageTime );
this.Controls.Add(this.dtpLatestStorageTime );

                this.Controls.Add(this.lblLatestOutboundTime );
this.Controls.Add(this.dtpLatestOutboundTime );

                this.Controls.Add(this.lblLastInventoryDate );
this.Controls.Add(this.dtpLastInventoryDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "View_InventoryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblprop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBrand;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBrand;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_Cost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_SubtotalCostMoney;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_SubtotalCostMoney;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


