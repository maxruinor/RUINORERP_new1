
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:42
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    partial class tb_MaterialReturnQuery
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


this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOutgoing.Values.Text ="";

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTotalQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblMR_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMR_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGeneEvidence.Values.Text ="";

this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50BillNo###String
this.lblBillNo.AutoSize = true;
this.lblBillNo.Location = new System.Drawing.Point(100,25);
this.lblBillNo.Name = "lblBillNo";
this.lblBillNo.Size = new System.Drawing.Size(41, 12);
this.lblBillNo.TabIndex = 1;
this.lblBillNo.Text = "退料单号";
this.txtBillNo.Location = new System.Drawing.Point(173,21);
this.txtBillNo.Name = "txtBillNo";
this.txtBillNo.Size = new System.Drawing.Size(100, 21);
this.txtBillNo.TabIndex = 1;
this.Controls.Add(this.lblBillNo);
this.Controls.Add(this.txtBillNo);

           //#####BillType###Int32
//属性测试50BillType
//属性测试50BillType
//属性测试50BillType
//属性测试50BillType

           //#####Employee_ID###Int64
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "经办人";
//111======75
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,71);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DepartmentID###Int64
//属性测试100DepartmentID
//属性测试100DepartmentID
//属性测试100DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,100);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 4;
this.lblDepartmentID.Text = "生产部门";
//111======100
this.cmbDepartmentID.Location = new System.Drawing.Point(173,96);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 4;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Outgoing###Boolean
this.lblOutgoing.AutoSize = true;
this.lblOutgoing.Location = new System.Drawing.Point(100,125);
this.lblOutgoing.Name = "lblOutgoing";
this.lblOutgoing.Size = new System.Drawing.Size(41, 12);
this.lblOutgoing.TabIndex = 5;
this.lblOutgoing.Text = "外发加工";
this.chkOutgoing.Location = new System.Drawing.Point(173,121);
this.chkOutgoing.Name = "chkOutgoing";
this.chkOutgoing.Size = new System.Drawing.Size(100, 21);
this.chkOutgoing.TabIndex = 5;
this.Controls.Add(this.lblOutgoing);
this.Controls.Add(this.chkOutgoing);

           //#####CustomerVendor_ID###Int64
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,150);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 6;
this.lblCustomerVendor_ID.Text = "加工厂商";
//111======150
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,146);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 6;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####TotalQty###Decimal
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,175);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 7;
this.lblTotalQty.Text = "总数量";
//111======175
this.txtTotalQty.Location = new System.Drawing.Point(173,171);
this.txtTotalQty.Name ="txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 7;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalCostAmount###Decimal
this.lblTotalCostAmount.AutoSize = true;
this.lblTotalCostAmount.Location = new System.Drawing.Point(100,200);
this.lblTotalCostAmount.Name = "lblTotalCostAmount";
this.lblTotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalCostAmount.TabIndex = 8;
this.lblTotalCostAmount.Text = "总成本";
//111======200
this.txtTotalCostAmount.Location = new System.Drawing.Point(173,196);
this.txtTotalCostAmount.Name ="txtTotalCostAmount";
this.txtTotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalCostAmount.TabIndex = 8;
this.Controls.Add(this.lblTotalCostAmount);
this.Controls.Add(this.txtTotalCostAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,225);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 9;
this.lblTotalAmount.Text = "总金额";
//111======225
this.txtTotalAmount.Location = new System.Drawing.Point(173,221);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 9;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,250);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 10;
this.lblReturnDate.Text = "退料日期";
//111======250
this.dtpReturnDate.Location = new System.Drawing.Point(173,246);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 10;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,275);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 11;
this.lblCreated_at.Text = "创建时间";
//111======275
this.dtpCreated_at.Location = new System.Drawing.Point(173,271);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 11;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试300Created_by
//属性测试300Created_by
//属性测试300Created_by
//属性测试300Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试350Modified_by
//属性测试350Modified_by
//属性测试350Modified_by
//属性测试350Modified_by

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,375);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 15;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,371);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 15;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####MR_ID###Int64
//属性测试400MR_ID
this.lblMR_ID.AutoSize = true;
this.lblMR_ID.Location = new System.Drawing.Point(100,400);
this.lblMR_ID.Name = "lblMR_ID";
this.lblMR_ID.Size = new System.Drawing.Size(41, 12);
this.lblMR_ID.TabIndex = 16;
this.lblMR_ID.Text = "领料单";
//111======400
this.cmbMR_ID.Location = new System.Drawing.Point(173,396);
this.cmbMR_ID.Name ="cmbMR_ID";
this.cmbMR_ID.Size = new System.Drawing.Size(100, 21);
this.cmbMR_ID.TabIndex = 16;
this.Controls.Add(this.lblMR_ID);
this.Controls.Add(this.cmbMR_ID);

           //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,425);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 17;
this.lblMaterialRequisitionNO.Text = "领料单号";
this.txtMaterialRequisitionNO.Location = new System.Drawing.Point(173,421);
this.txtMaterialRequisitionNO.Name = "txtMaterialRequisitionNO";
this.txtMaterialRequisitionNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialRequisitionNO.TabIndex = 17;
this.Controls.Add(this.lblMaterialRequisitionNO);
this.Controls.Add(this.txtMaterialRequisitionNO);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,450);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 18;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,446);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 18;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试475DataStatus
//属性测试475DataStatus
//属性测试475DataStatus
//属性测试475DataStatus

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,500);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 20;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,496);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 20;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试525Approver_by
//属性测试525Approver_by
//属性测试525Approver_by
//属性测试525Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,550);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 22;
this.lblApprover_at.Text = "审批时间";
//111======550
this.dtpApprover_at.Location = new System.Drawing.Point(173,546);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 22;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####GeneEvidence###Boolean
this.lblGeneEvidence.AutoSize = true;
this.lblGeneEvidence.Location = new System.Drawing.Point(100,600);
this.lblGeneEvidence.Name = "lblGeneEvidence";
this.lblGeneEvidence.Size = new System.Drawing.Size(41, 12);
this.lblGeneEvidence.TabIndex = 24;
this.lblGeneEvidence.Text = "产生凭证";
this.chkGeneEvidence.Location = new System.Drawing.Point(173,596);
this.chkGeneEvidence.Name = "chkGeneEvidence";
this.chkGeneEvidence.Size = new System.Drawing.Size(100, 21);
this.chkGeneEvidence.TabIndex = 24;
this.Controls.Add(this.lblGeneEvidence);
this.Controls.Add(this.chkGeneEvidence);

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,625);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 25;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,621);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 25;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试650PrintStatus
//属性测试650PrintStatus
//属性测试650PrintStatus
//属性测试650PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBillNo );
this.Controls.Add(this.txtBillNo );

                
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblOutgoing );
this.Controls.Add(this.chkOutgoing );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalCostAmount );
this.Controls.Add(this.txtTotalCostAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblMR_ID );
this.Controls.Add(this.cmbMR_ID );

                this.Controls.Add(this.lblMaterialRequisitionNO );
this.Controls.Add(this.txtMaterialRequisitionNO );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblGeneEvidence );
this.Controls.Add(this.chkGeneEvidence );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                    
            this.Name = "tb_MaterialReturnQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBillNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutgoing;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOutgoing;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMR_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMR_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialRequisitionNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialRequisitionNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGeneEvidence;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGeneEvidence;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
    
   
 





    }
}


