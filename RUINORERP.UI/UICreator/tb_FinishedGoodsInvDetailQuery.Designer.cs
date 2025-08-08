
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 成品入库单明细
    /// </summary>
    partial class tb_FinishedGoodsInvDetailQuery
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
     
     this.lblFG_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbFG_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();



this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblNetMachineHours = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNetMachineHours = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNetWorkingHours = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNetWorkingHours = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblManuFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtManuFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductionAllCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductionAllCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####FG_ID###Int64
//属性测试25FG_ID
//属性测试25FG_ID
//属性测试25FG_ID
this.lblFG_ID.AutoSize = true;
this.lblFG_ID.Location = new System.Drawing.Point(100,25);
this.lblFG_ID.Name = "lblFG_ID";
this.lblFG_ID.Size = new System.Drawing.Size(41, 12);
this.lblFG_ID.TabIndex = 1;
this.lblFG_ID.Text = "缴库单";
//111======25
this.cmbFG_ID.Location = new System.Drawing.Point(173,21);
this.cmbFG_ID.Name ="cmbFG_ID";
this.cmbFG_ID.Size = new System.Drawing.Size(100, 21);
this.cmbFG_ID.TabIndex = 1;
this.Controls.Add(this.lblFG_ID);
this.Controls.Add(this.cmbFG_ID);

           //#####Unit_ID###Int64
//属性测试50Unit_ID
//属性测试50Unit_ID
//属性测试50Unit_ID
//属性测试50Unit_ID
//属性测试50Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,50);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 2;
this.lblUnit_ID.Text = "单位";
//111======50
this.cmbUnit_ID.Location = new System.Drawing.Point(173,46);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 2;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####ProdDetailID###Int64
//属性测试75ProdDetailID
//属性测试75ProdDetailID
//属性测试75ProdDetailID
//属性测试75ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,75);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 3;
this.lblProdDetailID.Text = "货品详情";
//111======75
this.cmbProdDetailID.Location = new System.Drawing.Point(173,71);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 3;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Location_ID###Int64
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

           //#####Rack_ID###Int64
//属性测试125Rack_ID
//属性测试125Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,125);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 5;
this.lblRack_ID.Text = "货架";
//111======125
this.cmbRack_ID.Location = new System.Drawing.Point(173,121);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 5;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####PayableQty###Int32
//属性测试150PayableQty
//属性测试150PayableQty
//属性测试150PayableQty
//属性测试150PayableQty
//属性测试150PayableQty

           //#####Qty###Int32
//属性测试175Qty
//属性测试175Qty
//属性测试175Qty
//属性测试175Qty
//属性测试175Qty

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,200);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 8;
this.lblUnitCost.Text = "单位成本";
//111======200
this.txtUnitCost.Location = new System.Drawing.Point(173,196);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 8;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####UnpaidQty###Int32
//属性测试225UnpaidQty
//属性测试225UnpaidQty
//属性测试225UnpaidQty
//属性测试225UnpaidQty
//属性测试225UnpaidQty

           //#####NetMachineHours###Decimal
this.lblNetMachineHours.AutoSize = true;
this.lblNetMachineHours.Location = new System.Drawing.Point(100,250);
this.lblNetMachineHours.Name = "lblNetMachineHours";
this.lblNetMachineHours.Size = new System.Drawing.Size(41, 12);
this.lblNetMachineHours.TabIndex = 10;
this.lblNetMachineHours.Text = "单位实际机时";
//111======250
this.txtNetMachineHours.Location = new System.Drawing.Point(173,246);
this.txtNetMachineHours.Name ="txtNetMachineHours";
this.txtNetMachineHours.Size = new System.Drawing.Size(100, 21);
this.txtNetMachineHours.TabIndex = 10;
this.Controls.Add(this.lblNetMachineHours);
this.Controls.Add(this.txtNetMachineHours);

           //#####NetWorkingHours###Decimal
this.lblNetWorkingHours.AutoSize = true;
this.lblNetWorkingHours.Location = new System.Drawing.Point(100,275);
this.lblNetWorkingHours.Name = "lblNetWorkingHours";
this.lblNetWorkingHours.Size = new System.Drawing.Size(41, 12);
this.lblNetWorkingHours.TabIndex = 11;
this.lblNetWorkingHours.Text = "单位实际工时";
//111======275
this.txtNetWorkingHours.Location = new System.Drawing.Point(173,271);
this.txtNetWorkingHours.Name ="txtNetWorkingHours";
this.txtNetWorkingHours.Size = new System.Drawing.Size(100, 21);
this.txtNetWorkingHours.TabIndex = 11;
this.Controls.Add(this.lblNetWorkingHours);
this.Controls.Add(this.txtNetWorkingHours);

           //#####ApportionedCost###Decimal
this.lblApportionedCost.AutoSize = true;
this.lblApportionedCost.Location = new System.Drawing.Point(100,300);
this.lblApportionedCost.Name = "lblApportionedCost";
this.lblApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblApportionedCost.TabIndex = 12;
this.lblApportionedCost.Text = "单位分摊成本";
//111======300
this.txtApportionedCost.Location = new System.Drawing.Point(173,296);
this.txtApportionedCost.Name ="txtApportionedCost";
this.txtApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtApportionedCost.TabIndex = 12;
this.Controls.Add(this.lblApportionedCost);
this.Controls.Add(this.txtApportionedCost);

           //#####ManuFee###Decimal
this.lblManuFee.AutoSize = true;
this.lblManuFee.Location = new System.Drawing.Point(100,325);
this.lblManuFee.Name = "lblManuFee";
this.lblManuFee.Size = new System.Drawing.Size(41, 12);
this.lblManuFee.TabIndex = 13;
this.lblManuFee.Text = "单位制造费用";
//111======325
this.txtManuFee.Location = new System.Drawing.Point(173,321);
this.txtManuFee.Name ="txtManuFee";
this.txtManuFee.Size = new System.Drawing.Size(100, 21);
this.txtManuFee.TabIndex = 13;
this.Controls.Add(this.lblManuFee);
this.Controls.Add(this.txtManuFee);

           //#####MaterialCost###Decimal
this.lblMaterialCost.AutoSize = true;
this.lblMaterialCost.Location = new System.Drawing.Point(100,350);
this.lblMaterialCost.Name = "lblMaterialCost";
this.lblMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblMaterialCost.TabIndex = 14;
this.lblMaterialCost.Text = "单位材料成本";
//111======350
this.txtMaterialCost.Location = new System.Drawing.Point(173,346);
this.txtMaterialCost.Name ="txtMaterialCost";
this.txtMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtMaterialCost.TabIndex = 14;
this.Controls.Add(this.lblMaterialCost);
this.Controls.Add(this.txtMaterialCost);

           //#####ProductionAllCost###Decimal
this.lblProductionAllCost.AutoSize = true;
this.lblProductionAllCost.Location = new System.Drawing.Point(100,375);
this.lblProductionAllCost.Name = "lblProductionAllCost";
this.lblProductionAllCost.Size = new System.Drawing.Size(41, 12);
this.lblProductionAllCost.TabIndex = 15;
this.lblProductionAllCost.Text = "成本小计";
//111======375
this.txtProductionAllCost.Location = new System.Drawing.Point(173,371);
this.txtProductionAllCost.Name ="txtProductionAllCost";
this.txtProductionAllCost.Size = new System.Drawing.Size(100, 21);
this.txtProductionAllCost.TabIndex = 15;
this.Controls.Add(this.lblProductionAllCost);
this.Controls.Add(this.txtProductionAllCost);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,400);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 16;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,396);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 16;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,425);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 17;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,421);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 17;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFG_ID );
this.Controls.Add(this.cmbFG_ID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                
                
                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                
                this.Controls.Add(this.lblNetMachineHours );
this.Controls.Add(this.txtNetMachineHours );

                this.Controls.Add(this.lblNetWorkingHours );
this.Controls.Add(this.txtNetWorkingHours );

                this.Controls.Add(this.lblApportionedCost );
this.Controls.Add(this.txtApportionedCost );

                this.Controls.Add(this.lblManuFee );
this.Controls.Add(this.txtManuFee );

                this.Controls.Add(this.lblMaterialCost );
this.Controls.Add(this.txtMaterialCost );

                this.Controls.Add(this.lblProductionAllCost );
this.Controls.Add(this.txtProductionAllCost );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                    
            this.Name = "tb_FinishedGoodsInvDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFG_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbFG_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNetMachineHours;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNetMachineHours;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNetWorkingHours;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNetWorkingHours;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApportionedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApportionedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblManuFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtManuFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductionAllCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductionAllCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
    
   
 





    }
}


