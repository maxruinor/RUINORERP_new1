
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
    /// 制令单明细统计
    /// </summary>
    partial class View_ManufacturingOrderItemsQuery
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
     
     this.lblMONO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMONO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();




this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOutSourced.Values.Text ="";



this.lblShouldSendQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShouldSendQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblActualSentQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActualSentQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBOM_NO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBOM_NO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100MONO###String
this.lblMONO.AutoSize = true;
this.lblMONO.Location = new System.Drawing.Point(100,25);
this.lblMONO.Name = "lblMONO";
this.lblMONO.Size = new System.Drawing.Size(41, 12);
this.lblMONO.TabIndex = 1;
this.lblMONO.Text = "";
this.txtMONO.Location = new System.Drawing.Point(173,21);
this.txtMONO.Name = "txtMONO";
this.txtMONO.Size = new System.Drawing.Size(100, 21);
this.txtMONO.TabIndex = 1;
this.Controls.Add(this.lblMONO);
this.Controls.Add(this.txtMONO);

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####CustomerVendor_ID###Int64

           //#####CustomerVendor_ID_Out###Int64

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

           //#####ManufacturingQty###Int32

           //#####DataStatus###Int32

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsOutSourced###Boolean
this.lblIsOutSourced.AutoSize = true;
this.lblIsOutSourced.Location = new System.Drawing.Point(100,275);
this.lblIsOutSourced.Name = "lblIsOutSourced";
this.lblIsOutSourced.Size = new System.Drawing.Size(41, 12);
this.lblIsOutSourced.TabIndex = 11;
this.lblIsOutSourced.Text = "";
this.chkIsOutSourced.Location = new System.Drawing.Point(173,271);
this.chkIsOutSourced.Name = "chkIsOutSourced";
this.chkIsOutSourced.Size = new System.Drawing.Size(100, 21);
this.chkIsOutSourced.TabIndex = 11;
this.Controls.Add(this.lblIsOutSourced);
this.Controls.Add(this.chkIsOutSourced);

           //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####ShouldSendQty###Decimal
this.lblShouldSendQty.AutoSize = true;
this.lblShouldSendQty.Location = new System.Drawing.Point(100,350);
this.lblShouldSendQty.Name = "lblShouldSendQty";
this.lblShouldSendQty.Size = new System.Drawing.Size(41, 12);
this.lblShouldSendQty.TabIndex = 14;
this.lblShouldSendQty.Text = "";
//111======350
this.txtShouldSendQty.Location = new System.Drawing.Point(173,346);
this.txtShouldSendQty.Name ="txtShouldSendQty";
this.txtShouldSendQty.Size = new System.Drawing.Size(100, 21);
this.txtShouldSendQty.TabIndex = 14;
this.Controls.Add(this.lblShouldSendQty);
this.Controls.Add(this.txtShouldSendQty);

           //#####ActualSentQty###Decimal
this.lblActualSentQty.AutoSize = true;
this.lblActualSentQty.Location = new System.Drawing.Point(100,375);
this.lblActualSentQty.Name = "lblActualSentQty";
this.lblActualSentQty.Size = new System.Drawing.Size(41, 12);
this.lblActualSentQty.TabIndex = 15;
this.lblActualSentQty.Text = "";
//111======375
this.txtActualSentQty.Location = new System.Drawing.Point(173,371);
this.txtActualSentQty.Name ="txtActualSentQty";
this.txtActualSentQty.Size = new System.Drawing.Size(100, 21);
this.txtActualSentQty.TabIndex = 15;
this.Controls.Add(this.lblActualSentQty);
this.Controls.Add(this.txtActualSentQty);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,400);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 16;
this.lblUnitCost.Text = "";
//111======400
this.txtUnitCost.Location = new System.Drawing.Point(173,396);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 16;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalUnitCost###Decimal
this.lblSubtotalUnitCost.AutoSize = true;
this.lblSubtotalUnitCost.Location = new System.Drawing.Point(100,425);
this.lblSubtotalUnitCost.Name = "lblSubtotalUnitCost";
this.lblSubtotalUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUnitCost.TabIndex = 17;
this.lblSubtotalUnitCost.Text = "";
//111======425
this.txtSubtotalUnitCost.Location = new System.Drawing.Point(173,421);
this.txtSubtotalUnitCost.Name ="txtSubtotalUnitCost";
this.txtSubtotalUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUnitCost.TabIndex = 17;
this.Controls.Add(this.lblSubtotalUnitCost);
this.Controls.Add(this.txtSubtotalUnitCost);

           //#####BOM_ID###Int64

           //#####50BOM_NO###String
this.lblBOM_NO.AutoSize = true;
this.lblBOM_NO.Location = new System.Drawing.Point(100,475);
this.lblBOM_NO.Name = "lblBOM_NO";
this.lblBOM_NO.Size = new System.Drawing.Size(41, 12);
this.lblBOM_NO.TabIndex = 19;
this.lblBOM_NO.Text = "";
this.txtBOM_NO.Location = new System.Drawing.Point(173,471);
this.txtBOM_NO.Name = "txtBOM_NO";
this.txtBOM_NO.Size = new System.Drawing.Size(100, 21);
this.txtBOM_NO.TabIndex = 19;
this.Controls.Add(this.lblBOM_NO);
this.Controls.Add(this.txtBOM_NO);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,500);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 20;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,496);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 20;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,525);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 21;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,521);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 21;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ProdBaseID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,575);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 23;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,571);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 23;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,600);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 24;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,596);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 24;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,625);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 25;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,621);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 25;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Quantity###Int32

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,675);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 27;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,671);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 27;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,700);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 28;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,696);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 28;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,725);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 29;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,721);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 29;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMONO );
this.Controls.Add(this.txtMONO );

                
                
                
                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsOutSourced );
this.Controls.Add(this.chkIsOutSourced );

                
                
                this.Controls.Add(this.lblShouldSendQty );
this.Controls.Add(this.txtShouldSendQty );

                this.Controls.Add(this.lblActualSentQty );
this.Controls.Add(this.txtActualSentQty );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalUnitCost );
this.Controls.Add(this.txtSubtotalUnitCost );

                
                this.Controls.Add(this.lblBOM_NO );
this.Controls.Add(this.txtBOM_NO );

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

                
                
                    
            this.Name = "View_ManufacturingOrderItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMONO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMONO;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOutSourced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOutSourced;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShouldSendQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShouldSendQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActualSentQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActualSentQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUnitCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_NO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_NO;

    
        
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

    
        
              
    
        
              
    
    
   
 





    }
}


