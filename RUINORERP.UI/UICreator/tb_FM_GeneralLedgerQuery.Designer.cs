
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 总账表来源于凭证分类汇总是财务报表的基础数据
    /// </summary>
    partial class tb_FM_GeneralLedgerQuery
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
     
     
this.lblSubject_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSubject_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTransactionDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTransactionDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblTransactionType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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
                 //#####GeneralLedgerID###Int64
//属性测试25GeneralLedgerID
//属性测试25GeneralLedgerID

           //#####Subject_id###Int64
//属性测试50Subject_id
//属性测试50Subject_id
this.lblSubject_id.AutoSize = true;
this.lblSubject_id.Location = new System.Drawing.Point(100,50);
this.lblSubject_id.Name = "lblSubject_id";
this.lblSubject_id.Size = new System.Drawing.Size(41, 12);
this.lblSubject_id.TabIndex = 2;
this.lblSubject_id.Text = "会计科目";
//111======50
this.cmbSubject_id.Location = new System.Drawing.Point(173,46);
this.cmbSubject_id.Name ="cmbSubject_id";
this.cmbSubject_id.Size = new System.Drawing.Size(100, 21);
this.cmbSubject_id.TabIndex = 2;
this.Controls.Add(this.lblSubject_id);
this.Controls.Add(this.cmbSubject_id);

           //#####Currency_ID###Int64
//属性测试75Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,75);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 3;
this.lblCurrency_ID.Text = "币别";
//111======75
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,71);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 3;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####TransactionDate###DateTime
this.lblTransactionDate.AutoSize = true;
this.lblTransactionDate.Location = new System.Drawing.Point(100,100);
this.lblTransactionDate.Name = "lblTransactionDate";
this.lblTransactionDate.Size = new System.Drawing.Size(41, 12);
this.lblTransactionDate.TabIndex = 4;
this.lblTransactionDate.Text = "交易日期";
//111======100
this.dtpTransactionDate.Location = new System.Drawing.Point(173,96);
this.dtpTransactionDate.Name ="dtpTransactionDate";
this.dtpTransactionDate.ShowCheckBox =true;
this.dtpTransactionDate.Size = new System.Drawing.Size(100, 21);
this.dtpTransactionDate.TabIndex = 4;
this.Controls.Add(this.lblTransactionDate);
this.Controls.Add(this.dtpTransactionDate);

           //#####50TransactionType###String
this.lblTransactionType.AutoSize = true;
this.lblTransactionType.Location = new System.Drawing.Point(100,125);
this.lblTransactionType.Name = "lblTransactionType";
this.lblTransactionType.Size = new System.Drawing.Size(41, 12);
this.lblTransactionType.TabIndex = 5;
this.lblTransactionType.Text = "交易类型";
this.txtTransactionType.Location = new System.Drawing.Point(173,121);
this.txtTransactionType.Name = "txtTransactionType";
this.txtTransactionType.Size = new System.Drawing.Size(100, 21);
this.txtTransactionType.TabIndex = 5;
this.Controls.Add(this.lblTransactionType);
this.Controls.Add(this.txtTransactionType);

           //#####200Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,150);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 6;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,146);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 6;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####Amount###Decimal
this.lblAmount.AutoSize = true;
this.lblAmount.Location = new System.Drawing.Point(100,175);
this.lblAmount.Name = "lblAmount";
this.lblAmount.Size = new System.Drawing.Size(41, 12);
this.lblAmount.TabIndex = 7;
this.lblAmount.Text = "金额";
//111======175
this.txtAmount.Location = new System.Drawing.Point(173,171);
this.txtAmount.Name ="txtAmount";
this.txtAmount.Size = new System.Drawing.Size(100, 21);
this.txtAmount.TabIndex = 7;
this.Controls.Add(this.lblAmount);
this.Controls.Add(this.txtAmount);

           //#####SourceBizType###Int32
//属性测试200SourceBizType
//属性测试200SourceBizType

           //#####SourceBillId###Int64
//属性测试225SourceBillId
//属性测试225SourceBillId

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,250);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 10;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,246);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 10;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

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

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,375);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 15;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,371);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 15;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,400);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 16;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,396);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 16;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
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

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,500);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 20;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,496);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 20;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试525DataStatus
//属性测试525DataStatus

           //#####PrintStatus###Int32
//属性测试550PrintStatus
//属性测试550PrintStatus

           //#####TransactionDirection###Int32
//属性测试575TransactionDirection
//属性测试575TransactionDirection

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblSubject_id );
this.Controls.Add(this.cmbSubject_id );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblTransactionDate );
this.Controls.Add(this.dtpTransactionDate );

                this.Controls.Add(this.lblTransactionType );
this.Controls.Add(this.txtTransactionType );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblAmount );
this.Controls.Add(this.txtAmount );

                
                
                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

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

                
                
                
                    
            this.Name = "tb_FM_GeneralLedgerQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubject_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSubject_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTransactionDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAmount;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
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


