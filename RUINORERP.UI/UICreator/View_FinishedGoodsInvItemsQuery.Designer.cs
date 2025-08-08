
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 缴库明细统计
    /// </summary>
    partial class View_FinishedGoodsInvItemsQuery
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
     
     this.lblDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblMONo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMONo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();







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


this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;


this.lblprop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtprop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOutSourced.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50DeliveryBillNo###String
this.lblDeliveryBillNo.AutoSize = true;
this.lblDeliveryBillNo.Location = new System.Drawing.Point(100,25);
this.lblDeliveryBillNo.Name = "lblDeliveryBillNo";
this.lblDeliveryBillNo.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryBillNo.TabIndex = 1;
this.lblDeliveryBillNo.Text = "";
this.txtDeliveryBillNo.Location = new System.Drawing.Point(173,21);
this.txtDeliveryBillNo.Name = "txtDeliveryBillNo";
this.txtDeliveryBillNo.Size = new System.Drawing.Size(100, 21);
this.txtDeliveryBillNo.TabIndex = 1;
this.Controls.Add(this.lblDeliveryBillNo);
this.Controls.Add(this.txtDeliveryBillNo);

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####CustomerVendor_ID###Int64

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,125);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 5;
this.lblDeliveryDate.Text = "";
//111======125
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,121);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 5;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####50MONo###String
this.lblMONo.AutoSize = true;
this.lblMONo.Location = new System.Drawing.Point(100,200);
this.lblMONo.Name = "lblMONo";
this.lblMONo.Size = new System.Drawing.Size(41, 12);
this.lblMONo.TabIndex = 8;
this.lblMONo.Text = "";
this.txtMONo.Location = new System.Drawing.Point(173,196);
this.txtMONo.Name = "txtMONo";
this.txtMONo.Size = new System.Drawing.Size(100, 21);
this.txtMONo.TabIndex = 8;
this.Controls.Add(this.lblMONo);
this.Controls.Add(this.txtMONo);

           //#####Unit_ID###Int64

           //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####Rack_ID###Int64

           //#####PayableQty###Int32

           //#####Qty###Int32

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,375);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 15;
this.lblUnitCost.Text = "";
//111======375
this.txtUnitCost.Location = new System.Drawing.Point(173,371);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 15;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####UnpaidQty###Int32

           //#####NetMachineHours###Decimal
this.lblNetMachineHours.AutoSize = true;
this.lblNetMachineHours.Location = new System.Drawing.Point(100,425);
this.lblNetMachineHours.Name = "lblNetMachineHours";
this.lblNetMachineHours.Size = new System.Drawing.Size(41, 12);
this.lblNetMachineHours.TabIndex = 17;
this.lblNetMachineHours.Text = "";
//111======425
this.txtNetMachineHours.Location = new System.Drawing.Point(173,421);
this.txtNetMachineHours.Name ="txtNetMachineHours";
this.txtNetMachineHours.Size = new System.Drawing.Size(100, 21);
this.txtNetMachineHours.TabIndex = 17;
this.Controls.Add(this.lblNetMachineHours);
this.Controls.Add(this.txtNetMachineHours);

           //#####NetWorkingHours###Decimal
this.lblNetWorkingHours.AutoSize = true;
this.lblNetWorkingHours.Location = new System.Drawing.Point(100,450);
this.lblNetWorkingHours.Name = "lblNetWorkingHours";
this.lblNetWorkingHours.Size = new System.Drawing.Size(41, 12);
this.lblNetWorkingHours.TabIndex = 18;
this.lblNetWorkingHours.Text = "";
//111======450
this.txtNetWorkingHours.Location = new System.Drawing.Point(173,446);
this.txtNetWorkingHours.Name ="txtNetWorkingHours";
this.txtNetWorkingHours.Size = new System.Drawing.Size(100, 21);
this.txtNetWorkingHours.TabIndex = 18;
this.Controls.Add(this.lblNetWorkingHours);
this.Controls.Add(this.txtNetWorkingHours);

           //#####ApportionedCost###Decimal
this.lblApportionedCost.AutoSize = true;
this.lblApportionedCost.Location = new System.Drawing.Point(100,475);
this.lblApportionedCost.Name = "lblApportionedCost";
this.lblApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblApportionedCost.TabIndex = 19;
this.lblApportionedCost.Text = "";
//111======475
this.txtApportionedCost.Location = new System.Drawing.Point(173,471);
this.txtApportionedCost.Name ="txtApportionedCost";
this.txtApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtApportionedCost.TabIndex = 19;
this.Controls.Add(this.lblApportionedCost);
this.Controls.Add(this.txtApportionedCost);

           //#####ManuFee###Decimal
this.lblManuFee.AutoSize = true;
this.lblManuFee.Location = new System.Drawing.Point(100,500);
this.lblManuFee.Name = "lblManuFee";
this.lblManuFee.Size = new System.Drawing.Size(41, 12);
this.lblManuFee.TabIndex = 20;
this.lblManuFee.Text = "";
//111======500
this.txtManuFee.Location = new System.Drawing.Point(173,496);
this.txtManuFee.Name ="txtManuFee";
this.txtManuFee.Size = new System.Drawing.Size(100, 21);
this.txtManuFee.TabIndex = 20;
this.Controls.Add(this.lblManuFee);
this.Controls.Add(this.txtManuFee);

           //#####MaterialCost###Decimal
this.lblMaterialCost.AutoSize = true;
this.lblMaterialCost.Location = new System.Drawing.Point(100,525);
this.lblMaterialCost.Name = "lblMaterialCost";
this.lblMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblMaterialCost.TabIndex = 21;
this.lblMaterialCost.Text = "";
//111======525
this.txtMaterialCost.Location = new System.Drawing.Point(173,521);
this.txtMaterialCost.Name ="txtMaterialCost";
this.txtMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtMaterialCost.TabIndex = 21;
this.Controls.Add(this.lblMaterialCost);
this.Controls.Add(this.txtMaterialCost);

           //#####ProductionAllCost###Decimal
this.lblProductionAllCost.AutoSize = true;
this.lblProductionAllCost.Location = new System.Drawing.Point(100,550);
this.lblProductionAllCost.Name = "lblProductionAllCost";
this.lblProductionAllCost.Size = new System.Drawing.Size(41, 12);
this.lblProductionAllCost.TabIndex = 22;
this.lblProductionAllCost.Text = "";
//111======550
this.txtProductionAllCost.Location = new System.Drawing.Point(173,546);
this.txtProductionAllCost.Name ="txtProductionAllCost";
this.txtProductionAllCost.Size = new System.Drawing.Size(100, 21);
this.txtProductionAllCost.TabIndex = 22;
this.Controls.Add(this.lblProductionAllCost);
this.Controls.Add(this.txtProductionAllCost);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,575);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 23;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,571);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 23;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,600);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 24;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,596);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 24;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ProdBaseID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,650);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 26;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,646);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 26;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,675);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 27;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,671);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 27;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,700);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 28;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,696);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 28;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Quantity###Int32

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,750);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 30;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,746);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 30;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,775);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 31;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,771);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 31;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,800);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 32;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,796);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 32;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

           //#####DataStatus###Int32

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,900);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 36;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,896);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 36;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsOutSourced###Boolean
this.lblIsOutSourced.AutoSize = true;
this.lblIsOutSourced.Location = new System.Drawing.Point(100,925);
this.lblIsOutSourced.Name = "lblIsOutSourced";
this.lblIsOutSourced.Size = new System.Drawing.Size(41, 12);
this.lblIsOutSourced.TabIndex = 37;
this.lblIsOutSourced.Text = "";
this.chkIsOutSourced.Location = new System.Drawing.Point(173,921);
this.chkIsOutSourced.Name = "chkIsOutSourced";
this.chkIsOutSourced.Size = new System.Drawing.Size(100, 21);
this.chkIsOutSourced.TabIndex = 37;
this.Controls.Add(this.lblIsOutSourced);
this.Controls.Add(this.chkIsOutSourced);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDeliveryBillNo );
this.Controls.Add(this.txtDeliveryBillNo );

                
                
                
                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblMONo );
this.Controls.Add(this.txtMONo );

                
                
                
                
                
                
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

                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                
                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsOutSourced );
this.Controls.Add(this.chkIsOutSourced );

                    
            this.Name = "View_FinishedGoodsInvItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeliveryBillNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMONo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMONo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
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

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblprop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOutSourced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOutSourced;

    
    
   
 





    }
}


