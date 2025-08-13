// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库存快照表
    /// </summary>
    partial class tb_InventorySnapshotEdit
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
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblInitInventory = new Krypton.Toolkit.KryptonLabel();
this.txtInitInventory = new Krypton.Toolkit.KryptonTextBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.txtRack_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblOn_the_way_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtOn_the_way_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblSale_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtSale_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblMakingQty = new Krypton.Toolkit.KryptonLabel();
this.txtMakingQty = new Krypton.Toolkit.KryptonTextBox();

this.lblNotOutQty = new Krypton.Toolkit.KryptonLabel();
this.txtNotOutQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCostFIFO = new Krypton.Toolkit.KryptonLabel();
this.txtCostFIFO = new Krypton.Toolkit.KryptonTextBox();

this.lblCostMonthlyWA = new Krypton.Toolkit.KryptonLabel();
this.txtCostMonthlyWA = new Krypton.Toolkit.KryptonTextBox();

this.lblCostMovingWA = new Krypton.Toolkit.KryptonLabel();
this.txtCostMovingWA = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_AdvCost = new Krypton.Toolkit.KryptonLabel();
this.txtInv_AdvCost = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_Cost = new Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_SubtotalCostMoney = new Krypton.Toolkit.KryptonLabel();
this.txtInv_SubtotalCostMoney = new Krypton.Toolkit.KryptonTextBox();

this.lblLatestOutboundTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestStorageTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSnapshotTime = new Krypton.Toolkit.KryptonLabel();
this.dtpSnapshotTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    
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
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "产品详情";
this.txtProdDetailID.Location = new System.Drawing.Point(173,21);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "库位";
this.txtLocation_ID.Location = new System.Drawing.Point(173,46);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,75);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 3;
this.lblQuantity.Text = "实际库存";
this.txtQuantity.Location = new System.Drawing.Point(173,71);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 3;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####InitInventory###Int32
this.lblInitInventory.AutoSize = true;
this.lblInitInventory.Location = new System.Drawing.Point(100,100);
this.lblInitInventory.Name = "lblInitInventory";
this.lblInitInventory.Size = new System.Drawing.Size(41, 12);
this.lblInitInventory.TabIndex = 4;
this.lblInitInventory.Text = "期初数量";
this.txtInitInventory.Location = new System.Drawing.Point(173,96);
this.txtInitInventory.Name = "txtInitInventory";
this.txtInitInventory.Size = new System.Drawing.Size(100, 21);
this.txtInitInventory.TabIndex = 4;
this.Controls.Add(this.lblInitInventory);
this.Controls.Add(this.txtInitInventory);

           //#####Rack_ID###Int64
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,125);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 5;
this.lblRack_ID.Text = "货架";
this.txtRack_ID.Location = new System.Drawing.Point(173,121);
this.txtRack_ID.Name = "txtRack_ID";
this.txtRack_ID.Size = new System.Drawing.Size(100, 21);
this.txtRack_ID.TabIndex = 5;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.txtRack_ID);

           //#####On_the_way_Qty###Int32
this.lblOn_the_way_Qty.AutoSize = true;
this.lblOn_the_way_Qty.Location = new System.Drawing.Point(100,150);
this.lblOn_the_way_Qty.Name = "lblOn_the_way_Qty";
this.lblOn_the_way_Qty.Size = new System.Drawing.Size(41, 12);
this.lblOn_the_way_Qty.TabIndex = 6;
this.lblOn_the_way_Qty.Text = "在途库存";
this.txtOn_the_way_Qty.Location = new System.Drawing.Point(173,146);
this.txtOn_the_way_Qty.Name = "txtOn_the_way_Qty";
this.txtOn_the_way_Qty.Size = new System.Drawing.Size(100, 21);
this.txtOn_the_way_Qty.TabIndex = 6;
this.Controls.Add(this.lblOn_the_way_Qty);
this.Controls.Add(this.txtOn_the_way_Qty);

           //#####Sale_Qty###Int32
this.lblSale_Qty.AutoSize = true;
this.lblSale_Qty.Location = new System.Drawing.Point(100,175);
this.lblSale_Qty.Name = "lblSale_Qty";
this.lblSale_Qty.Size = new System.Drawing.Size(41, 12);
this.lblSale_Qty.TabIndex = 7;
this.lblSale_Qty.Text = "拟销售量";
this.txtSale_Qty.Location = new System.Drawing.Point(173,171);
this.txtSale_Qty.Name = "txtSale_Qty";
this.txtSale_Qty.Size = new System.Drawing.Size(100, 21);
this.txtSale_Qty.TabIndex = 7;
this.Controls.Add(this.lblSale_Qty);
this.Controls.Add(this.txtSale_Qty);

           //#####MakingQty###Int32
this.lblMakingQty.AutoSize = true;
this.lblMakingQty.Location = new System.Drawing.Point(100,200);
this.lblMakingQty.Name = "lblMakingQty";
this.lblMakingQty.Size = new System.Drawing.Size(41, 12);
this.lblMakingQty.TabIndex = 8;
this.lblMakingQty.Text = "在制数量";
this.txtMakingQty.Location = new System.Drawing.Point(173,196);
this.txtMakingQty.Name = "txtMakingQty";
this.txtMakingQty.Size = new System.Drawing.Size(100, 21);
this.txtMakingQty.TabIndex = 8;
this.Controls.Add(this.lblMakingQty);
this.Controls.Add(this.txtMakingQty);

           //#####NotOutQty###Int32
this.lblNotOutQty.AutoSize = true;
this.lblNotOutQty.Location = new System.Drawing.Point(100,225);
this.lblNotOutQty.Name = "lblNotOutQty";
this.lblNotOutQty.Size = new System.Drawing.Size(41, 12);
this.lblNotOutQty.TabIndex = 9;
this.lblNotOutQty.Text = "未发料量";
this.txtNotOutQty.Location = new System.Drawing.Point(173,221);
this.txtNotOutQty.Name = "txtNotOutQty";
this.txtNotOutQty.Size = new System.Drawing.Size(100, 21);
this.txtNotOutQty.TabIndex = 9;
this.Controls.Add(this.lblNotOutQty);
this.Controls.Add(this.txtNotOutQty);

           //#####CostFIFO###Decimal
this.lblCostFIFO.AutoSize = true;
this.lblCostFIFO.Location = new System.Drawing.Point(100,250);
this.lblCostFIFO.Name = "lblCostFIFO";
this.lblCostFIFO.Size = new System.Drawing.Size(41, 12);
this.lblCostFIFO.TabIndex = 10;
this.lblCostFIFO.Text = "先进先出成本";
//111======250
this.txtCostFIFO.Location = new System.Drawing.Point(173,246);
this.txtCostFIFO.Name ="txtCostFIFO";
this.txtCostFIFO.Size = new System.Drawing.Size(100, 21);
this.txtCostFIFO.TabIndex = 10;
this.Controls.Add(this.lblCostFIFO);
this.Controls.Add(this.txtCostFIFO);

           //#####CostMonthlyWA###Decimal
this.lblCostMonthlyWA.AutoSize = true;
this.lblCostMonthlyWA.Location = new System.Drawing.Point(100,275);
this.lblCostMonthlyWA.Name = "lblCostMonthlyWA";
this.lblCostMonthlyWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMonthlyWA.TabIndex = 11;
this.lblCostMonthlyWA.Text = "月加权平均成本";
//111======275
this.txtCostMonthlyWA.Location = new System.Drawing.Point(173,271);
this.txtCostMonthlyWA.Name ="txtCostMonthlyWA";
this.txtCostMonthlyWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMonthlyWA.TabIndex = 11;
this.Controls.Add(this.lblCostMonthlyWA);
this.Controls.Add(this.txtCostMonthlyWA);

           //#####CostMovingWA###Decimal
this.lblCostMovingWA.AutoSize = true;
this.lblCostMovingWA.Location = new System.Drawing.Point(100,300);
this.lblCostMovingWA.Name = "lblCostMovingWA";
this.lblCostMovingWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMovingWA.TabIndex = 12;
this.lblCostMovingWA.Text = "移动加权平均成本";
//111======300
this.txtCostMovingWA.Location = new System.Drawing.Point(173,296);
this.txtCostMovingWA.Name ="txtCostMovingWA";
this.txtCostMovingWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMovingWA.TabIndex = 12;
this.Controls.Add(this.lblCostMovingWA);
this.Controls.Add(this.txtCostMovingWA);

           //#####Inv_AdvCost###Decimal
this.lblInv_AdvCost.AutoSize = true;
this.lblInv_AdvCost.Location = new System.Drawing.Point(100,325);
this.lblInv_AdvCost.Name = "lblInv_AdvCost";
this.lblInv_AdvCost.Size = new System.Drawing.Size(41, 12);
this.lblInv_AdvCost.TabIndex = 13;
this.lblInv_AdvCost.Text = "实际成本";
//111======325
this.txtInv_AdvCost.Location = new System.Drawing.Point(173,321);
this.txtInv_AdvCost.Name ="txtInv_AdvCost";
this.txtInv_AdvCost.Size = new System.Drawing.Size(100, 21);
this.txtInv_AdvCost.TabIndex = 13;
this.Controls.Add(this.lblInv_AdvCost);
this.Controls.Add(this.txtInv_AdvCost);

           //#####Inv_Cost###Decimal
this.lblInv_Cost.AutoSize = true;
this.lblInv_Cost.Location = new System.Drawing.Point(100,350);
this.lblInv_Cost.Name = "lblInv_Cost";
this.lblInv_Cost.Size = new System.Drawing.Size(41, 12);
this.lblInv_Cost.TabIndex = 14;
this.lblInv_Cost.Text = "产品成本";
//111======350
this.txtInv_Cost.Location = new System.Drawing.Point(173,346);
this.txtInv_Cost.Name ="txtInv_Cost";
this.txtInv_Cost.Size = new System.Drawing.Size(100, 21);
this.txtInv_Cost.TabIndex = 14;
this.Controls.Add(this.lblInv_Cost);
this.Controls.Add(this.txtInv_Cost);

           //#####Inv_SubtotalCostMoney###Decimal
this.lblInv_SubtotalCostMoney.AutoSize = true;
this.lblInv_SubtotalCostMoney.Location = new System.Drawing.Point(100,375);
this.lblInv_SubtotalCostMoney.Name = "lblInv_SubtotalCostMoney";
this.lblInv_SubtotalCostMoney.Size = new System.Drawing.Size(41, 12);
this.lblInv_SubtotalCostMoney.TabIndex = 15;
this.lblInv_SubtotalCostMoney.Text = "成本小计";
//111======375
this.txtInv_SubtotalCostMoney.Location = new System.Drawing.Point(173,371);
this.txtInv_SubtotalCostMoney.Name ="txtInv_SubtotalCostMoney";
this.txtInv_SubtotalCostMoney.Size = new System.Drawing.Size(100, 21);
this.txtInv_SubtotalCostMoney.TabIndex = 15;
this.Controls.Add(this.lblInv_SubtotalCostMoney);
this.Controls.Add(this.txtInv_SubtotalCostMoney);

           //#####LatestOutboundTime###DateTime
this.lblLatestOutboundTime.AutoSize = true;
this.lblLatestOutboundTime.Location = new System.Drawing.Point(100,400);
this.lblLatestOutboundTime.Name = "lblLatestOutboundTime";
this.lblLatestOutboundTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestOutboundTime.TabIndex = 16;
this.lblLatestOutboundTime.Text = "最新出库时间";
//111======400
this.dtpLatestOutboundTime.Location = new System.Drawing.Point(173,396);
this.dtpLatestOutboundTime.Name ="dtpLatestOutboundTime";
this.dtpLatestOutboundTime.ShowCheckBox =true;
this.dtpLatestOutboundTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestOutboundTime.TabIndex = 16;
this.Controls.Add(this.lblLatestOutboundTime);
this.Controls.Add(this.dtpLatestOutboundTime);

           //#####LatestStorageTime###DateTime
this.lblLatestStorageTime.AutoSize = true;
this.lblLatestStorageTime.Location = new System.Drawing.Point(100,425);
this.lblLatestStorageTime.Name = "lblLatestStorageTime";
this.lblLatestStorageTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestStorageTime.TabIndex = 17;
this.lblLatestStorageTime.Text = "最新入库时间";
//111======425
this.dtpLatestStorageTime.Location = new System.Drawing.Point(173,421);
this.dtpLatestStorageTime.Name ="dtpLatestStorageTime";
this.dtpLatestStorageTime.ShowCheckBox =true;
this.dtpLatestStorageTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestStorageTime.TabIndex = 17;
this.Controls.Add(this.lblLatestStorageTime);
this.Controls.Add(this.dtpLatestStorageTime);

           //#####LastInventoryDate###DateTime
this.lblLastInventoryDate.AutoSize = true;
this.lblLastInventoryDate.Location = new System.Drawing.Point(100,450);
this.lblLastInventoryDate.Name = "lblLastInventoryDate";
this.lblLastInventoryDate.Size = new System.Drawing.Size(41, 12);
this.lblLastInventoryDate.TabIndex = 18;
this.lblLastInventoryDate.Text = "最后盘点时间";
//111======450
this.dtpLastInventoryDate.Location = new System.Drawing.Point(173,446);
this.dtpLastInventoryDate.Name ="dtpLastInventoryDate";
this.dtpLastInventoryDate.ShowCheckBox =true;
this.dtpLastInventoryDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastInventoryDate.TabIndex = 18;
this.Controls.Add(this.lblLastInventoryDate);
this.Controls.Add(this.dtpLastInventoryDate);

           //#####SnapshotTime###DateTime
this.lblSnapshotTime.AutoSize = true;
this.lblSnapshotTime.Location = new System.Drawing.Point(100,475);
this.lblSnapshotTime.Name = "lblSnapshotTime";
this.lblSnapshotTime.Size = new System.Drawing.Size(41, 12);
this.lblSnapshotTime.TabIndex = 19;
this.lblSnapshotTime.Text = "修改时间";
//111======475
this.dtpSnapshotTime.Location = new System.Drawing.Point(173,471);
this.dtpSnapshotTime.Name ="dtpSnapshotTime";
this.dtpSnapshotTime.ShowCheckBox =true;
this.dtpSnapshotTime.Size = new System.Drawing.Size(100, 21);
this.dtpSnapshotTime.TabIndex = 19;
this.Controls.Add(this.lblSnapshotTime);
this.Controls.Add(this.dtpSnapshotTime);

           //#####250Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,500);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 20;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,496);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 20;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblInitInventory );
this.Controls.Add(this.txtInitInventory );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.txtRack_ID );

                this.Controls.Add(this.lblOn_the_way_Qty );
this.Controls.Add(this.txtOn_the_way_Qty );

                this.Controls.Add(this.lblSale_Qty );
this.Controls.Add(this.txtSale_Qty );

                this.Controls.Add(this.lblMakingQty );
this.Controls.Add(this.txtMakingQty );

                this.Controls.Add(this.lblNotOutQty );
this.Controls.Add(this.txtNotOutQty );

                this.Controls.Add(this.lblCostFIFO );
this.Controls.Add(this.txtCostFIFO );

                this.Controls.Add(this.lblCostMonthlyWA );
this.Controls.Add(this.txtCostMonthlyWA );

                this.Controls.Add(this.lblCostMovingWA );
this.Controls.Add(this.txtCostMovingWA );

                this.Controls.Add(this.lblInv_AdvCost );
this.Controls.Add(this.txtInv_AdvCost );

                this.Controls.Add(this.lblInv_Cost );
this.Controls.Add(this.txtInv_Cost );

                this.Controls.Add(this.lblInv_SubtotalCostMoney );
this.Controls.Add(this.txtInv_SubtotalCostMoney );

                this.Controls.Add(this.lblLatestOutboundTime );
this.Controls.Add(this.dtpLatestOutboundTime );

                this.Controls.Add(this.lblLatestStorageTime );
this.Controls.Add(this.dtpLatestStorageTime );

                this.Controls.Add(this.lblLastInventoryDate );
this.Controls.Add(this.dtpLastInventoryDate );

                this.Controls.Add(this.lblSnapshotTime );
this.Controls.Add(this.dtpSnapshotTime );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_InventorySnapshotEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_InventorySnapshotEdit";
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
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblInitInventory;
private Krypton.Toolkit.KryptonTextBox txtInitInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonTextBox txtRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOn_the_way_Qty;
private Krypton.Toolkit.KryptonTextBox txtOn_the_way_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSale_Qty;
private Krypton.Toolkit.KryptonTextBox txtSale_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblMakingQty;
private Krypton.Toolkit.KryptonTextBox txtMakingQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotOutQty;
private Krypton.Toolkit.KryptonTextBox txtNotOutQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostFIFO;
private Krypton.Toolkit.KryptonTextBox txtCostFIFO;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostMonthlyWA;
private Krypton.Toolkit.KryptonTextBox txtCostMonthlyWA;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostMovingWA;
private Krypton.Toolkit.KryptonTextBox txtCostMovingWA;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_AdvCost;
private Krypton.Toolkit.KryptonTextBox txtInv_AdvCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_Cost;
private Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_SubtotalCostMoney;
private Krypton.Toolkit.KryptonTextBox txtInv_SubtotalCostMoney;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSnapshotTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpSnapshotTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

