
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:30
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 退料统计
    /// </summary>
    partial class View_MaterialReturnItemsQuery
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
     
     this.lblBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOutgoing.Values.Text ="";


this.lblReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";



this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50BillNo###String
this.lblBillNo.AutoSize = true;
this.lblBillNo.Location = new System.Drawing.Point(100,25);
this.lblBillNo.Name = "lblBillNo";
this.lblBillNo.Size = new System.Drawing.Size(41, 12);
this.lblBillNo.TabIndex = 1;
this.lblBillNo.Text = "";
this.txtBillNo.Location = new System.Drawing.Point(173,21);
this.txtBillNo.Name = "txtBillNo";
this.txtBillNo.Size = new System.Drawing.Size(100, 21);
this.txtBillNo.TabIndex = 1;
this.Controls.Add(this.lblBillNo);
this.Controls.Add(this.txtBillNo);

           //#####BillType###Int32

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####Outgoing###Boolean
this.lblOutgoing.AutoSize = true;
this.lblOutgoing.Location = new System.Drawing.Point(100,125);
this.lblOutgoing.Name = "lblOutgoing";
this.lblOutgoing.Size = new System.Drawing.Size(41, 12);
this.lblOutgoing.TabIndex = 5;
this.lblOutgoing.Text = "";
this.chkOutgoing.Location = new System.Drawing.Point(173,121);
this.chkOutgoing.Name = "chkOutgoing";
this.chkOutgoing.Size = new System.Drawing.Size(100, 21);
this.chkOutgoing.TabIndex = 5;
this.Controls.Add(this.lblOutgoing);
this.Controls.Add(this.chkOutgoing);

           //#####CustomerVendor_ID###Int64

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,175);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 7;
this.lblReturnDate.Text = "";
//111======175
this.dtpReturnDate.Location = new System.Drawing.Point(173,171);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 7;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,200);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 8;
this.lblCreated_at.Text = "";
//111======200
this.dtpCreated_at.Location = new System.Drawing.Point(173,196);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 8;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####255Notes###String
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

           //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,275);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 11;
this.lblMaterialRequisitionNO.Text = "";
this.txtMaterialRequisitionNO.Location = new System.Drawing.Point(173,271);
this.txtMaterialRequisitionNO.Name = "txtMaterialRequisitionNO";
this.txtMaterialRequisitionNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialRequisitionNO.TabIndex = 11;
this.Controls.Add(this.lblMaterialRequisitionNO);
this.Controls.Add(this.txtMaterialRequisitionNO);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,300);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 12;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,296);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 12;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32

           //#####ProdDetailID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,375);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 15;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,371);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 15;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,425);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 17;
this.lblCost.Text = "";
//111======425
this.txtCost.Location = new System.Drawing.Point(173,421);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 17;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,450);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 18;
this.lblPrice.Text = "";
//111======450
this.txtPrice.Location = new System.Drawing.Point(173,446);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 18;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,475);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 19;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,471);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 19;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,500);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 20;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,496);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 20;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,525);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 21;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,521);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 21;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,550);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 22;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,546);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 22;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,600);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 24;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,596);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 24;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBillNo );
this.Controls.Add(this.txtBillNo );

                
                
                
                this.Controls.Add(this.lblOutgoing );
this.Controls.Add(this.chkOutgoing );

                
                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblMaterialRequisitionNO );
this.Controls.Add(this.txtMaterialRequisitionNO );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                
                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                
                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                    
            this.Name = "View_MaterialReturnItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBillNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutgoing;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOutgoing;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialRequisitionNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialRequisitionNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
    
   
 





    }
}


