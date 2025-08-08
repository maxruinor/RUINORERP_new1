
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库存表
    /// </summary>
    partial class tb_InventoryQuery
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
     
     this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();










this.lblCostFIFO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCostFIFO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCostMonthlyWA = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCostMonthlyWA = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCostMovingWA = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCostMovingWA = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInv_AdvCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_AdvCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInv_SubtotalCostMoney = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_SubtotalCostMoney = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64
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

           //#####Rack_ID###Int64
//属性测试50Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,50);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 2;
this.lblRack_ID.Text = "货架";
//111======50
this.cmbRack_ID.Location = new System.Drawing.Point(173,46);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 2;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

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

           //#####Quantity###Int32
//属性测试100Quantity
//属性测试100Quantity
//属性测试100Quantity

           //#####InitInventory###Int32
//属性测试125InitInventory
//属性测试125InitInventory
//属性测试125InitInventory

           //#####Alert_Use###Int32
//属性测试150Alert_Use
//属性测试150Alert_Use
//属性测试150Alert_Use

           //#####On_the_way_Qty###Int32
//属性测试175On_the_way_Qty
//属性测试175On_the_way_Qty
//属性测试175On_the_way_Qty

           //#####Sale_Qty###Int32
//属性测试200Sale_Qty
//属性测试200Sale_Qty
//属性测试200Sale_Qty

           //#####MakingQty###Int32
//属性测试225MakingQty
//属性测试225MakingQty
//属性测试225MakingQty

           //#####NotOutQty###Int32
//属性测试250NotOutQty
//属性测试250NotOutQty
//属性测试250NotOutQty

           //#####BatchNumber###Int32
//属性测试275BatchNumber
//属性测试275BatchNumber
//属性测试275BatchNumber

           //#####Alert_Quantity###Int32
//属性测试300Alert_Quantity
//属性测试300Alert_Quantity
//属性测试300Alert_Quantity

           //#####CostFIFO###Decimal
this.lblCostFIFO.AutoSize = true;
this.lblCostFIFO.Location = new System.Drawing.Point(100,325);
this.lblCostFIFO.Name = "lblCostFIFO";
this.lblCostFIFO.Size = new System.Drawing.Size(41, 12);
this.lblCostFIFO.TabIndex = 13;
this.lblCostFIFO.Text = "先进先出成本";
//111======325
this.txtCostFIFO.Location = new System.Drawing.Point(173,321);
this.txtCostFIFO.Name ="txtCostFIFO";
this.txtCostFIFO.Size = new System.Drawing.Size(100, 21);
this.txtCostFIFO.TabIndex = 13;
this.Controls.Add(this.lblCostFIFO);
this.Controls.Add(this.txtCostFIFO);

           //#####CostMonthlyWA###Decimal
this.lblCostMonthlyWA.AutoSize = true;
this.lblCostMonthlyWA.Location = new System.Drawing.Point(100,350);
this.lblCostMonthlyWA.Name = "lblCostMonthlyWA";
this.lblCostMonthlyWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMonthlyWA.TabIndex = 14;
this.lblCostMonthlyWA.Text = "月加权平均成本";
//111======350
this.txtCostMonthlyWA.Location = new System.Drawing.Point(173,346);
this.txtCostMonthlyWA.Name ="txtCostMonthlyWA";
this.txtCostMonthlyWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMonthlyWA.TabIndex = 14;
this.Controls.Add(this.lblCostMonthlyWA);
this.Controls.Add(this.txtCostMonthlyWA);

           //#####CostMovingWA###Decimal
this.lblCostMovingWA.AutoSize = true;
this.lblCostMovingWA.Location = new System.Drawing.Point(100,375);
this.lblCostMovingWA.Name = "lblCostMovingWA";
this.lblCostMovingWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMovingWA.TabIndex = 15;
this.lblCostMovingWA.Text = "移动加权平均成本";
//111======375
this.txtCostMovingWA.Location = new System.Drawing.Point(173,371);
this.txtCostMovingWA.Name ="txtCostMovingWA";
this.txtCostMovingWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMovingWA.TabIndex = 15;
this.Controls.Add(this.lblCostMovingWA);
this.Controls.Add(this.txtCostMovingWA);

           //#####Inv_AdvCost###Decimal
this.lblInv_AdvCost.AutoSize = true;
this.lblInv_AdvCost.Location = new System.Drawing.Point(100,400);
this.lblInv_AdvCost.Name = "lblInv_AdvCost";
this.lblInv_AdvCost.Size = new System.Drawing.Size(41, 12);
this.lblInv_AdvCost.TabIndex = 16;
this.lblInv_AdvCost.Text = "实际成本";
//111======400
this.txtInv_AdvCost.Location = new System.Drawing.Point(173,396);
this.txtInv_AdvCost.Name ="txtInv_AdvCost";
this.txtInv_AdvCost.Size = new System.Drawing.Size(100, 21);
this.txtInv_AdvCost.TabIndex = 16;
this.Controls.Add(this.lblInv_AdvCost);
this.Controls.Add(this.txtInv_AdvCost);

           //#####Inv_Cost###Decimal
this.lblInv_Cost.AutoSize = true;
this.lblInv_Cost.Location = new System.Drawing.Point(100,425);
this.lblInv_Cost.Name = "lblInv_Cost";
this.lblInv_Cost.Size = new System.Drawing.Size(41, 12);
this.lblInv_Cost.TabIndex = 17;
this.lblInv_Cost.Text = "货品成本";
//111======425
this.txtInv_Cost.Location = new System.Drawing.Point(173,421);
this.txtInv_Cost.Name ="txtInv_Cost";
this.txtInv_Cost.Size = new System.Drawing.Size(100, 21);
this.txtInv_Cost.TabIndex = 17;
this.Controls.Add(this.lblInv_Cost);
this.Controls.Add(this.txtInv_Cost);

           //#####Inv_SubtotalCostMoney###Decimal
this.lblInv_SubtotalCostMoney.AutoSize = true;
this.lblInv_SubtotalCostMoney.Location = new System.Drawing.Point(100,450);
this.lblInv_SubtotalCostMoney.Name = "lblInv_SubtotalCostMoney";
this.lblInv_SubtotalCostMoney.Size = new System.Drawing.Size(41, 12);
this.lblInv_SubtotalCostMoney.TabIndex = 18;
this.lblInv_SubtotalCostMoney.Text = "成本小计";
//111======450
this.txtInv_SubtotalCostMoney.Location = new System.Drawing.Point(173,446);
this.txtInv_SubtotalCostMoney.Name ="txtInv_SubtotalCostMoney";
this.txtInv_SubtotalCostMoney.Size = new System.Drawing.Size(100, 21);
this.txtInv_SubtotalCostMoney.TabIndex = 18;
this.Controls.Add(this.lblInv_SubtotalCostMoney);
this.Controls.Add(this.txtInv_SubtotalCostMoney);

           //#####LatestOutboundTime###DateTime
this.lblLatestOutboundTime.AutoSize = true;
this.lblLatestOutboundTime.Location = new System.Drawing.Point(100,475);
this.lblLatestOutboundTime.Name = "lblLatestOutboundTime";
this.lblLatestOutboundTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestOutboundTime.TabIndex = 19;
this.lblLatestOutboundTime.Text = "最新出库时间";
//111======475
this.dtpLatestOutboundTime.Location = new System.Drawing.Point(173,471);
this.dtpLatestOutboundTime.Name ="dtpLatestOutboundTime";
this.dtpLatestOutboundTime.ShowCheckBox =true;
this.dtpLatestOutboundTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestOutboundTime.TabIndex = 19;
this.Controls.Add(this.lblLatestOutboundTime);
this.Controls.Add(this.dtpLatestOutboundTime);

           //#####LatestStorageTime###DateTime
this.lblLatestStorageTime.AutoSize = true;
this.lblLatestStorageTime.Location = new System.Drawing.Point(100,500);
this.lblLatestStorageTime.Name = "lblLatestStorageTime";
this.lblLatestStorageTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestStorageTime.TabIndex = 20;
this.lblLatestStorageTime.Text = "最新入库时间";
//111======500
this.dtpLatestStorageTime.Location = new System.Drawing.Point(173,496);
this.dtpLatestStorageTime.Name ="dtpLatestStorageTime";
this.dtpLatestStorageTime.ShowCheckBox =true;
this.dtpLatestStorageTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestStorageTime.TabIndex = 20;
this.Controls.Add(this.lblLatestStorageTime);
this.Controls.Add(this.dtpLatestStorageTime);

           //#####LastInventoryDate###DateTime
this.lblLastInventoryDate.AutoSize = true;
this.lblLastInventoryDate.Location = new System.Drawing.Point(100,525);
this.lblLastInventoryDate.Name = "lblLastInventoryDate";
this.lblLastInventoryDate.Size = new System.Drawing.Size(41, 12);
this.lblLastInventoryDate.TabIndex = 21;
this.lblLastInventoryDate.Text = "最后盘点时间";
//111======525
this.dtpLastInventoryDate.Location = new System.Drawing.Point(173,521);
this.dtpLastInventoryDate.Name ="dtpLastInventoryDate";
this.dtpLastInventoryDate.ShowCheckBox =true;
this.dtpLastInventoryDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastInventoryDate.TabIndex = 21;
this.Controls.Add(this.lblLastInventoryDate);
this.Controls.Add(this.dtpLastInventoryDate);

           //#####250Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,550);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 22;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,546);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 22;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,575);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 23;
this.lblCreated_at.Text = "创建时间";
//111======575
this.dtpCreated_at.Location = new System.Drawing.Point(173,571);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 23;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试600Created_by
//属性测试600Created_by
//属性测试600Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,625);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 25;
this.lblModified_at.Text = "修改时间";
//111======625
this.dtpModified_at.Location = new System.Drawing.Point(173,621);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 25;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试650Modified_by
//属性测试650Modified_by
//属性测试650Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                
                
                
                
                
                
                
                
                
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

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_InventoryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCostFIFO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCostFIFO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCostMonthlyWA;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCostMonthlyWA;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCostMovingWA;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCostMovingWA;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_AdvCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_AdvCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_Cost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_SubtotalCostMoney;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_SubtotalCostMoney;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


