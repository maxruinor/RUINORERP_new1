
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:09:43
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
    partial class tb_InventorySnapshotQuery
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

this.lblSnapshotTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpSnapshotTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####Quantity###Int32

           //#####InitInventory###Int32

           //#####Rack_ID###Int64

           //#####On_the_way_Qty###Int32

           //#####Sale_Qty###Int32

           //#####MakingQty###Int32

           //#####NotOutQty###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                
                
                
                
                
                
                
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

                    
            this.Name = "tb_InventorySnapshotQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSnapshotTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpSnapshotTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


