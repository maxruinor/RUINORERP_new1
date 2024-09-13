
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:38
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 请购单明细表
    /// </summary>
    partial class tb_BuyingRequisitionDetailQuery
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
     
     this.lblPuRequisition_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPuRequisition_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;



this.lblEstimatedPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEstimatedPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblPurpose = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurpose = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPurpose.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblPurchased = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPurchased = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPurchased.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PuRequisition_ID###Int64
//属性测试25PuRequisition_ID
//属性测试25PuRequisition_ID
this.lblPuRequisition_ID.AutoSize = true;
this.lblPuRequisition_ID.Location = new System.Drawing.Point(100,25);
this.lblPuRequisition_ID.Name = "lblPuRequisition_ID";
this.lblPuRequisition_ID.Size = new System.Drawing.Size(41, 12);
this.lblPuRequisition_ID.TabIndex = 1;
this.lblPuRequisition_ID.Text = "请购单";
//111======25
this.cmbPuRequisition_ID.Location = new System.Drawing.Point(173,21);
this.cmbPuRequisition_ID.Name ="cmbPuRequisition_ID";
this.cmbPuRequisition_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPuRequisition_ID.TabIndex = 1;
this.Controls.Add(this.lblPuRequisition_ID);
this.Controls.Add(this.cmbPuRequisition_ID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,75);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 3;
this.lblRequirementDate.Text = "需求日期";
//111======75
this.dtpRequirementDate.Location = new System.Drawing.Point(173,71);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.ShowCheckBox =true;
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 3;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

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

           //#####ActualRequiredQty###Int32
//属性测试125ActualRequiredQty
//属性测试125ActualRequiredQty

           //#####Quantity###Int32
//属性测试150Quantity
//属性测试150Quantity

           //#####EstimatedPrice###Decimal
this.lblEstimatedPrice.AutoSize = true;
this.lblEstimatedPrice.Location = new System.Drawing.Point(100,175);
this.lblEstimatedPrice.Name = "lblEstimatedPrice";
this.lblEstimatedPrice.Size = new System.Drawing.Size(41, 12);
this.lblEstimatedPrice.TabIndex = 7;
this.lblEstimatedPrice.Text = "预估价格";
//111======175
this.txtEstimatedPrice.Location = new System.Drawing.Point(173,171);
this.txtEstimatedPrice.Name ="txtEstimatedPrice";
this.txtEstimatedPrice.Size = new System.Drawing.Size(100, 21);
this.txtEstimatedPrice.TabIndex = 7;
this.Controls.Add(this.lblEstimatedPrice);
this.Controls.Add(this.txtEstimatedPrice);

           //#####DeliveredQuantity###Int32
//属性测试200DeliveredQuantity
//属性测试200DeliveredQuantity

           //#####500Purpose###String
this.lblPurpose.AutoSize = true;
this.lblPurpose.Location = new System.Drawing.Point(100,225);
this.lblPurpose.Name = "lblPurpose";
this.lblPurpose.Size = new System.Drawing.Size(41, 12);
this.lblPurpose.TabIndex = 9;
this.lblPurpose.Text = "用途";
this.txtPurpose.Location = new System.Drawing.Point(173,221);
this.txtPurpose.Name = "txtPurpose";
this.txtPurpose.Size = new System.Drawing.Size(100, 21);
this.txtPurpose.TabIndex = 9;
this.Controls.Add(this.lblPurpose);
this.Controls.Add(this.txtPurpose);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Purchased###Boolean
this.lblPurchased.AutoSize = true;
this.lblPurchased.Location = new System.Drawing.Point(100,275);
this.lblPurchased.Name = "lblPurchased";
this.lblPurchased.Size = new System.Drawing.Size(41, 12);
this.lblPurchased.TabIndex = 11;
this.lblPurchased.Text = "已采购";
this.chkPurchased.Location = new System.Drawing.Point(173,271);
this.chkPurchased.Name = "chkPurchased";
this.chkPurchased.Size = new System.Drawing.Size(100, 21);
this.chkPurchased.TabIndex = 11;
this.Controls.Add(this.lblPurchased);
this.Controls.Add(this.chkPurchased);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPuRequisition_ID );
this.Controls.Add(this.cmbPuRequisition_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                this.Controls.Add(this.lblEstimatedPrice );
this.Controls.Add(this.txtEstimatedPrice );

                
                this.Controls.Add(this.lblPurpose );
this.Controls.Add(this.txtPurpose );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblPurchased );
this.Controls.Add(this.chkPurchased );

                    
            this.Name = "tb_BuyingRequisitionDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPuRequisition_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPuRequisition_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEstimatedPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEstimatedPrice;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurpose;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurpose;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurchased;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPurchased;

    
    
   
 





    }
}


