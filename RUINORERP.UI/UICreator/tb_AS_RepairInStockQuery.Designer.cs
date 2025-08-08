
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:07
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修入库单
    /// </summary>
    partial class tb_AS_RepairInStockQuery
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
     
     this.lblRepairInStockNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairInStockNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRepairOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblEntryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEntryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50RepairInStockNo###String
this.lblRepairInStockNo.AutoSize = true;
this.lblRepairInStockNo.Location = new System.Drawing.Point(100,25);
this.lblRepairInStockNo.Name = "lblRepairInStockNo";
this.lblRepairInStockNo.Size = new System.Drawing.Size(41, 12);
this.lblRepairInStockNo.TabIndex = 1;
this.lblRepairInStockNo.Text = "维修入库单号";
this.txtRepairInStockNo.Location = new System.Drawing.Point(173,21);
this.txtRepairInStockNo.Name = "txtRepairInStockNo";
this.txtRepairInStockNo.Size = new System.Drawing.Size(100, 21);
this.txtRepairInStockNo.TabIndex = 1;
this.Controls.Add(this.lblRepairInStockNo);
this.Controls.Add(this.txtRepairInStockNo);

           //#####RepairOrderID###Int64
//属性测试50RepairOrderID
//属性测试50RepairOrderID
//属性测试50RepairOrderID
//属性测试50RepairOrderID
this.lblRepairOrderID.AutoSize = true;
this.lblRepairOrderID.Location = new System.Drawing.Point(100,50);
this.lblRepairOrderID.Name = "lblRepairOrderID";
this.lblRepairOrderID.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderID.TabIndex = 2;
this.lblRepairOrderID.Text = "维修工单";
//111======50
this.cmbRepairOrderID.Location = new System.Drawing.Point(173,46);
this.cmbRepairOrderID.Name ="cmbRepairOrderID";
this.cmbRepairOrderID.Size = new System.Drawing.Size(100, 21);
this.cmbRepairOrderID.TabIndex = 2;
this.Controls.Add(this.lblRepairOrderID);
this.Controls.Add(this.cmbRepairOrderID);

           //#####50RepairOrderNo###String
this.lblRepairOrderNo.AutoSize = true;
this.lblRepairOrderNo.Location = new System.Drawing.Point(100,75);
this.lblRepairOrderNo.Name = "lblRepairOrderNo";
this.lblRepairOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderNo.TabIndex = 3;
this.lblRepairOrderNo.Text = "维修工单号";
this.txtRepairOrderNo.Location = new System.Drawing.Point(173,71);
this.txtRepairOrderNo.Name = "txtRepairOrderNo";
this.txtRepairOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtRepairOrderNo.TabIndex = 3;
this.Controls.Add(this.lblRepairOrderNo);
this.Controls.Add(this.txtRepairOrderNo);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "经办人员";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####CustomerVendor_ID###Int64
//属性测试125CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,125);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 5;
this.lblCustomerVendor_ID.Text = "客户";
//111======125
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,121);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 5;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####ProjectGroup_ID###Int64
//属性测试150ProjectGroup_ID
//属性测试150ProjectGroup_ID
//属性测试150ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,150);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 6;
this.lblProjectGroup_ID.Text = "项目小组";
//111======150
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,146);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 6;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####TotalQty###Int32
//属性测试175TotalQty
//属性测试175TotalQty
//属性测试175TotalQty
//属性测试175TotalQty

           //#####EntryDate###DateTime
this.lblEntryDate.AutoSize = true;
this.lblEntryDate.Location = new System.Drawing.Point(100,200);
this.lblEntryDate.Name = "lblEntryDate";
this.lblEntryDate.Size = new System.Drawing.Size(41, 12);
this.lblEntryDate.TabIndex = 8;
this.lblEntryDate.Text = "入库日期";
//111======200
this.dtpEntryDate.Location = new System.Drawing.Point(173,196);
this.dtpEntryDate.Name ="dtpEntryDate";
this.dtpEntryDate.Size = new System.Drawing.Size(100, 21);
this.dtpEntryDate.TabIndex = 8;
this.Controls.Add(this.lblEntryDate);
this.Controls.Add(this.dtpEntryDate);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,225);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 9;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,221);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 9;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "创建时间";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试275Created_by
//属性测试275Created_by
//属性测试275Created_by
//属性测试275Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试325Modified_by
//属性测试325Modified_by
//属性测试325Modified_by
//属性测试325Modified_by

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,350);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 14;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,346);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 14;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,375);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 15;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,371);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 15;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试400Approver_by
//属性测试400Approver_by
//属性测试400Approver_by
//属性测试400Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,425);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 17;
this.lblApprover_at.Text = "审批时间";
//111======425
this.dtpApprover_at.Location = new System.Drawing.Point(173,421);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 17;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,475);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 19;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,471);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 19;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试500PrintStatus
//属性测试500PrintStatus
//属性测试500PrintStatus
//属性测试500PrintStatus

           //#####DataStatus###Int32
//属性测试525DataStatus
//属性测试525DataStatus
//属性测试525DataStatus
//属性测试525DataStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRepairInStockNo );
this.Controls.Add(this.txtRepairInStockNo );

                this.Controls.Add(this.lblRepairOrderID );
this.Controls.Add(this.cmbRepairOrderID );

                this.Controls.Add(this.lblRepairOrderNo );
this.Controls.Add(this.txtRepairOrderNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                
                this.Controls.Add(this.lblEntryDate );
this.Controls.Add(this.dtpEntryDate );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                
                    
            this.Name = "tb_AS_RepairInStockQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairInStockNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairInStockNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRepairOrderID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEntryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEntryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              
    
    
   
 





    }
}


