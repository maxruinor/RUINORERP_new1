
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
    /// </summary>
    partial class tb_BuyingRequisitionQuery
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
     
     this.lblPuRequisitionNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPuRequisitionNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApplicationDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApplicationDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblPurpose = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurpose = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100PuRequisitionNo###String
this.lblPuRequisitionNo.AutoSize = true;
this.lblPuRequisitionNo.Location = new System.Drawing.Point(100,25);
this.lblPuRequisitionNo.Name = "lblPuRequisitionNo";
this.lblPuRequisitionNo.Size = new System.Drawing.Size(41, 12);
this.lblPuRequisitionNo.TabIndex = 1;
this.lblPuRequisitionNo.Text = "请购单号";
this.txtPuRequisitionNo.Location = new System.Drawing.Point(173,21);
this.txtPuRequisitionNo.Name = "txtPuRequisitionNo";
this.txtPuRequisitionNo.Size = new System.Drawing.Size(100, 21);
this.txtPuRequisitionNo.TabIndex = 1;
this.Controls.Add(this.lblPuRequisitionNo);
this.Controls.Add(this.txtPuRequisitionNo);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "申请人";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

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

           //#####DepartmentID###Int64
//属性测试100DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,100);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 4;
this.lblDepartmentID.Text = "使用部门";
//111======100
this.cmbDepartmentID.Location = new System.Drawing.Point(173,96);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 4;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####RefBillID###Int64
//属性测试125RefBillID
//属性测试125RefBillID
//属性测试125RefBillID

           //#####50RefBillNO###String
this.lblRefBillNO.AutoSize = true;
this.lblRefBillNO.Location = new System.Drawing.Point(100,150);
this.lblRefBillNO.Name = "lblRefBillNO";
this.lblRefBillNO.Size = new System.Drawing.Size(41, 12);
this.lblRefBillNO.TabIndex = 6;
this.lblRefBillNO.Text = "引用单据编号";
this.txtRefBillNO.Location = new System.Drawing.Point(173,146);
this.txtRefBillNO.Name = "txtRefBillNO";
this.txtRefBillNO.Size = new System.Drawing.Size(100, 21);
this.txtRefBillNO.TabIndex = 6;
this.Controls.Add(this.lblRefBillNO);
this.Controls.Add(this.txtRefBillNO);

           //#####RefBizType###Int32
//属性测试175RefBizType
//属性测试175RefBizType
//属性测试175RefBizType

           //#####ApplicationDate###DateTime
this.lblApplicationDate.AutoSize = true;
this.lblApplicationDate.Location = new System.Drawing.Point(100,200);
this.lblApplicationDate.Name = "lblApplicationDate";
this.lblApplicationDate.Size = new System.Drawing.Size(41, 12);
this.lblApplicationDate.TabIndex = 8;
this.lblApplicationDate.Text = "申请日期";
//111======200
this.dtpApplicationDate.Location = new System.Drawing.Point(173,196);
this.dtpApplicationDate.Name ="dtpApplicationDate";
this.dtpApplicationDate.Size = new System.Drawing.Size(100, 21);
this.dtpApplicationDate.TabIndex = 8;
this.Controls.Add(this.lblApplicationDate);
this.Controls.Add(this.dtpApplicationDate);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,225);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 9;
this.lblRequirementDate.Text = "需求日期";
//111======225
this.dtpRequirementDate.Location = new System.Drawing.Point(173,221);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.ShowCheckBox =true;
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 9;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####TotalQty###Int32
//属性测试250TotalQty
//属性测试250TotalQty
//属性测试250TotalQty

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,275);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 11;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,271);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 11;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DataStatus###Int32
//属性测试300DataStatus
//属性测试300DataStatus
//属性测试300DataStatus

           //#####200Purpose###String
this.lblPurpose.AutoSize = true;
this.lblPurpose.Location = new System.Drawing.Point(100,325);
this.lblPurpose.Name = "lblPurpose";
this.lblPurpose.Size = new System.Drawing.Size(41, 12);
this.lblPurpose.TabIndex = 13;
this.lblPurpose.Text = "用途";
this.txtPurpose.Location = new System.Drawing.Point(173,321);
this.txtPurpose.Name = "txtPurpose";
this.txtPurpose.Size = new System.Drawing.Size(100, 21);
this.txtPurpose.TabIndex = 13;
this.Controls.Add(this.lblPurpose);
this.Controls.Add(this.txtPurpose);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,350);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 14;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,346);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 14;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,400);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 16;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,396);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 16;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Approver_by###Int64
//属性测试425Approver_by
//属性测试425Approver_by
//属性测试425Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,450);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 18;
this.lblApprover_at.Text = "审批时间";
//111======450
this.dtpApprover_at.Location = new System.Drawing.Point(173,446);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 18;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,475);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 19;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,471);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 19;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,500);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 20;
this.lblCreated_at.Text = "创建时间";
//111======500
this.dtpCreated_at.Location = new System.Drawing.Point(173,496);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 20;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试525Created_by
//属性测试525Created_by
//属性测试525Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,550);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 22;
this.lblModified_at.Text = "修改时间";
//111======550
this.dtpModified_at.Location = new System.Drawing.Point(173,546);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 22;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试575Modified_by
//属性测试575Modified_by
//属性测试575Modified_by

           //#####PrintStatus###Int32
//属性测试600PrintStatus
//属性测试600PrintStatus
//属性测试600PrintStatus

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,625);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 25;
this.lblCloseCaseOpinions.Text = "结案情况";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,621);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 25;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPuRequisitionNo );
this.Controls.Add(this.txtPuRequisitionNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                
                this.Controls.Add(this.lblRefBillNO );
this.Controls.Add(this.txtRefBillNO );

                
                this.Controls.Add(this.lblApplicationDate );
this.Controls.Add(this.dtpApplicationDate );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
                this.Controls.Add(this.lblPurpose );
this.Controls.Add(this.txtPurpose );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                
                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                    
            this.Name = "tb_BuyingRequisitionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPuRequisitionNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPuRequisitionNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRefBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRefBillNO;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApplicationDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApplicationDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurpose;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurpose;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
    
   
 





    }
}


