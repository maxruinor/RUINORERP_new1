
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:49
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 先采购合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同
    /// </summary>
    partial class tb_PO_ContractQuery
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
     
     this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTemplateId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbTemplateId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPOContractNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPOContractNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblContract_Date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpContract_Date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblContractType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkContractType = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkContractType.Values.Text ="";



this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";



this.lblClauseContent = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClauseContent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtClauseContent.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####CustomerVendor_ID###Int64
//属性测试25CustomerVendor_ID
//属性测试25CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,25);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 1;
this.lblCustomerVendor_ID.Text = "";
//111======25
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,21);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 1;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####ID###Int64
//属性测试50ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,50);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 2;
this.lblID.Text = "";
//111======50
this.cmbID.Location = new System.Drawing.Point(173,46);
this.cmbID.Name ="cmbID";
this.cmbID.Size = new System.Drawing.Size(100, 21);
this.cmbID.TabIndex = 2;
this.Controls.Add(this.lblID);
this.Controls.Add(this.cmbID);

           //#####TemplateId###Int64
//属性测试75TemplateId
//属性测试75TemplateId
//属性测试75TemplateId
this.lblTemplateId.AutoSize = true;
this.lblTemplateId.Location = new System.Drawing.Point(100,75);
this.lblTemplateId.Name = "lblTemplateId";
this.lblTemplateId.Size = new System.Drawing.Size(41, 12);
this.lblTemplateId.TabIndex = 3;
this.lblTemplateId.Text = "明细";
//111======75
this.cmbTemplateId.Location = new System.Drawing.Point(173,71);
this.cmbTemplateId.Name ="cmbTemplateId";
this.cmbTemplateId.Size = new System.Drawing.Size(100, 21);
this.cmbTemplateId.TabIndex = 3;
this.Controls.Add(this.lblTemplateId);
this.Controls.Add(this.cmbTemplateId);

           //#####50POContractNo###String
this.lblPOContractNo.AutoSize = true;
this.lblPOContractNo.Location = new System.Drawing.Point(100,100);
this.lblPOContractNo.Name = "lblPOContractNo";
this.lblPOContractNo.Size = new System.Drawing.Size(41, 12);
this.lblPOContractNo.TabIndex = 4;
this.lblPOContractNo.Text = "合同编号";
this.txtPOContractNo.Location = new System.Drawing.Point(173,96);
this.txtPOContractNo.Name = "txtPOContractNo";
this.txtPOContractNo.Size = new System.Drawing.Size(100, 21);
this.txtPOContractNo.TabIndex = 4;
this.Controls.Add(this.lblPOContractNo);
this.Controls.Add(this.txtPOContractNo);

           //#####Buyer###Int64
//属性测试125Buyer
//属性测试125Buyer
//属性测试125Buyer

           //#####Seller###Int64
//属性测试150Seller
//属性测试150Seller
//属性测试150Seller

           //#####Contract_Date###DateTime
this.lblContract_Date.AutoSize = true;
this.lblContract_Date.Location = new System.Drawing.Point(100,175);
this.lblContract_Date.Name = "lblContract_Date";
this.lblContract_Date.Size = new System.Drawing.Size(41, 12);
this.lblContract_Date.TabIndex = 7;
this.lblContract_Date.Text = "签署日期";
//111======175
this.dtpContract_Date.Location = new System.Drawing.Point(173,171);
this.dtpContract_Date.Name ="dtpContract_Date";
this.dtpContract_Date.ShowCheckBox =true;
this.dtpContract_Date.Size = new System.Drawing.Size(100, 21);
this.dtpContract_Date.TabIndex = 7;
this.Controls.Add(this.lblContract_Date);
this.Controls.Add(this.dtpContract_Date);

           //#####ContractType###Boolean
this.lblContractType.AutoSize = true;
this.lblContractType.Location = new System.Drawing.Point(100,200);
this.lblContractType.Name = "lblContractType";
this.lblContractType.Size = new System.Drawing.Size(41, 12);
this.lblContractType.TabIndex = 8;
this.lblContractType.Text = "合同类型";
this.chkContractType.Location = new System.Drawing.Point(173,196);
this.chkContractType.Name = "chkContractType";
this.chkContractType.Size = new System.Drawing.Size(100, 21);
this.chkContractType.TabIndex = 8;
this.Controls.Add(this.lblContractType);
this.Controls.Add(this.chkContractType);

           //#####Employee_ID###Int64
//属性测试225Employee_ID
//属性测试225Employee_ID
//属性测试225Employee_ID

           //#####TotalQty###Int32
//属性测试250TotalQty
//属性测试250TotalQty
//属性测试250TotalQty

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,275);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 11;
this.lblTotalAmount.Text = "总金额";
//111======275
this.txtTotalAmount.Location = new System.Drawing.Point(173,271);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 11;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,300);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 12;
this.lblCreated_at.Text = "创建时间";
//111======300
this.dtpCreated_at.Location = new System.Drawing.Point(173,296);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 12;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试325Created_by
//属性测试325Created_by
//属性测试325Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,350);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 14;
this.lblModified_at.Text = "修改时间";
//111======350
this.dtpModified_at.Location = new System.Drawing.Point(173,346);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 14;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试375Modified_by
//属性测试375Modified_by
//属性测试375Modified_by

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,400);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 16;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,396);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 16;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,425);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 17;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,421);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 17;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试450DataStatus
//属性测试450DataStatus
//属性测试450DataStatus

           //#####PrintStatus###Int32
//属性测试475PrintStatus
//属性测试475PrintStatus
//属性测试475PrintStatus

           //#####2147483647ClauseContent###String
this.lblClauseContent.AutoSize = true;
this.lblClauseContent.Location = new System.Drawing.Point(100,500);
this.lblClauseContent.Name = "lblClauseContent";
this.lblClauseContent.Size = new System.Drawing.Size(41, 12);
this.lblClauseContent.TabIndex = 20;
this.lblClauseContent.Text = "条款内容";
this.txtClauseContent.Location = new System.Drawing.Point(173,496);
this.txtClauseContent.Name = "txtClauseContent";
this.txtClauseContent.Size = new System.Drawing.Size(100, 21);
this.txtClauseContent.TabIndex = 20;
this.txtClauseContent.Multiline = true;
this.Controls.Add(this.lblClauseContent);
this.Controls.Add(this.txtClauseContent);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblID );
this.Controls.Add(this.cmbID );

                this.Controls.Add(this.lblTemplateId );
this.Controls.Add(this.cmbTemplateId );

                this.Controls.Add(this.lblPOContractNo );
this.Controls.Add(this.txtPOContractNo );

                
                
                this.Controls.Add(this.lblContract_Date );
this.Controls.Add(this.dtpContract_Date );

                this.Controls.Add(this.lblContractType );
this.Controls.Add(this.chkContractType );

                
                
                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                
                this.Controls.Add(this.lblClauseContent );
this.Controls.Add(this.txtClauseContent );

                    
            this.Name = "tb_PO_ContractQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTemplateId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbTemplateId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPOContractNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPOContractNo;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContract_Date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpContract_Date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContractType;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkContractType;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClauseContent;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClauseContent;

    
    
   
 





    }
}


