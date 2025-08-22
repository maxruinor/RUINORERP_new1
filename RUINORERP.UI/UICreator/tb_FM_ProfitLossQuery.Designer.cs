
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 损溢费用单
    /// </summary>
    partial class tb_FM_ProfitLossQuery
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
     
     this.lblProfitLossNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProfitLossNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblPostDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPostDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsExpenseType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsExpenseType = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsExpenseType.Values.Text ="";

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ProfitLossNo###String
this.lblProfitLossNo.AutoSize = true;
this.lblProfitLossNo.Location = new System.Drawing.Point(100,25);
this.lblProfitLossNo.Name = "lblProfitLossNo";
this.lblProfitLossNo.Size = new System.Drawing.Size(41, 12);
this.lblProfitLossNo.TabIndex = 1;
this.lblProfitLossNo.Text = "单据编号";
this.txtProfitLossNo.Location = new System.Drawing.Point(173,21);
this.txtProfitLossNo.Name = "txtProfitLossNo";
this.txtProfitLossNo.Size = new System.Drawing.Size(100, 21);
this.txtProfitLossNo.TabIndex = 1;
this.Controls.Add(this.lblProfitLossNo);
this.Controls.Add(this.txtProfitLossNo);

           //#####SourceBizType###Int32
//属性测试50SourceBizType
//属性测试50SourceBizType

           //#####SourceBillId###Int64
//属性测试75SourceBillId
//属性测试75SourceBillId

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 4;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####ProfitLossDirection###Int32
//属性测试125ProfitLossDirection
//属性测试125ProfitLossDirection

           //#####PostDate###DateTime
this.lblPostDate.AutoSize = true;
this.lblPostDate.Location = new System.Drawing.Point(100,150);
this.lblPostDate.Name = "lblPostDate";
this.lblPostDate.Size = new System.Drawing.Size(41, 12);
this.lblPostDate.TabIndex = 6;
this.lblPostDate.Text = "记账时间";
//111======150
this.dtpPostDate.Location = new System.Drawing.Point(173,146);
this.dtpPostDate.Name ="dtpPostDate";
this.dtpPostDate.Size = new System.Drawing.Size(100, 21);
this.dtpPostDate.TabIndex = 6;
this.Controls.Add(this.lblPostDate);
this.Controls.Add(this.dtpPostDate);

           //#####IsExpenseType###Boolean
this.lblIsExpenseType.AutoSize = true;
this.lblIsExpenseType.Location = new System.Drawing.Point(100,175);
this.lblIsExpenseType.Name = "lblIsExpenseType";
this.lblIsExpenseType.Size = new System.Drawing.Size(41, 12);
this.lblIsExpenseType.TabIndex = 7;
this.lblIsExpenseType.Text = "费用记账";
this.chkIsExpenseType.Location = new System.Drawing.Point(173,171);
this.chkIsExpenseType.Name = "chkIsExpenseType";
this.chkIsExpenseType.Size = new System.Drawing.Size(100, 21);
this.chkIsExpenseType.TabIndex = 7;
this.Controls.Add(this.lblIsExpenseType);
this.Controls.Add(this.chkIsExpenseType);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,200);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 8;
this.lblTotalAmount.Text = "总金额本币";
//111======200
this.txtTotalAmount.Location = new System.Drawing.Point(173,196);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 8;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####DepartmentID###Int64
//属性测试225DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,225);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 9;
this.lblDepartmentID.Text = "部门";
//111======225
this.cmbDepartmentID.Location = new System.Drawing.Point(173,221);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 9;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试250ProjectGroup_ID
//属性测试250ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,250);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 10;
this.lblProjectGroup_ID.Text = "项目组";
//111======250
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,246);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 10;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,275);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 11;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,271);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 11;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####TaxTotalAmount###Decimal
this.lblTaxTotalAmount.AutoSize = true;
this.lblTaxTotalAmount.Location = new System.Drawing.Point(100,300);
this.lblTaxTotalAmount.Name = "lblTaxTotalAmount";
this.lblTaxTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxTotalAmount.TabIndex = 12;
this.lblTaxTotalAmount.Text = "税额总计";
//111======300
this.txtTaxTotalAmount.Location = new System.Drawing.Point(173,296);
this.txtTaxTotalAmount.Name ="txtTaxTotalAmount";
this.txtTaxTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxTotalAmount.TabIndex = 12;
this.Controls.Add(this.lblTaxTotalAmount);
this.Controls.Add(this.txtTaxTotalAmount);

           //#####UntaxedTotalAmont###Decimal
this.lblUntaxedTotalAmont.AutoSize = true;
this.lblUntaxedTotalAmont.Location = new System.Drawing.Point(100,325);
this.lblUntaxedTotalAmont.Name = "lblUntaxedTotalAmont";
this.lblUntaxedTotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedTotalAmont.TabIndex = 13;
this.lblUntaxedTotalAmont.Text = "未税总计";
//111======325
this.txtUntaxedTotalAmont.Location = new System.Drawing.Point(173,321);
this.txtUntaxedTotalAmont.Name ="txtUntaxedTotalAmont";
this.txtUntaxedTotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedTotalAmont.TabIndex = 13;
this.Controls.Add(this.lblUntaxedTotalAmont);
this.Controls.Add(this.txtUntaxedTotalAmont);

           //#####DataStatus###Int32
//属性测试350DataStatus
//属性测试350DataStatus

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,375);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 15;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,371);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 15;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试425Created_by
//属性测试425Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试475Modified_by
//属性测试475Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,525);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 21;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,521);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 21;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试550Approver_by
//属性测试550Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,575);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 23;
this.lblApprover_at.Text = "审批时间";
//111======575
this.dtpApprover_at.Location = new System.Drawing.Point(173,571);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 23;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProfitLossNo );
this.Controls.Add(this.txtProfitLossNo );

                
                
                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                
                this.Controls.Add(this.lblPostDate );
this.Controls.Add(this.dtpPostDate );

                this.Controls.Add(this.lblIsExpenseType );
this.Controls.Add(this.chkIsExpenseType );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxTotalAmount );
this.Controls.Add(this.txtTaxTotalAmount );

                this.Controls.Add(this.lblUntaxedTotalAmont );
this.Controls.Add(this.txtUntaxedTotalAmont );

                
                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                    
            this.Name = "tb_FM_ProfitLossQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProfitLossNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProfitLossNo;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPostDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPostDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsExpenseType;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsExpenseType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedTotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedTotalAmont;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
    
   
 





    }
}


