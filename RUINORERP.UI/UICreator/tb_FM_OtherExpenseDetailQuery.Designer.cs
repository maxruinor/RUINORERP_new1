
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:34:12
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    partial class tb_FM_OtherExpenseDetailQuery
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
     
     this.lblExpenseMainID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbExpenseMainID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Account_id主外字段不一致。Subject_id主外字段不一致。
this.lblExpenseName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExpenseName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtExpenseName.Multiline = true;

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Account_id主外字段不一致。Subject_id主外字段不一致。
this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Account_id主外字段不一致。Subject_id主外字段不一致。
Account_id主外字段不一致。this.lblExpenseType_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbExpenseType_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Subject_id主外字段不一致。
Account_id主外字段不一致。Subject_id主外字段不一致。
this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Account_id主外字段不一致。Subject_id主外字段不一致。
Account_id主外字段不一致。Subject_id主外字段不一致。
this.lblCheckOutDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCheckOutDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Account_id主外字段不一致。Subject_id主外字段不一致。
this.lblEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ExpenseMainID###Int64
//属性测试25ExpenseMainID
//属性测试25ExpenseMainID
this.lblExpenseMainID.AutoSize = true;
this.lblExpenseMainID.Location = new System.Drawing.Point(100,25);
this.lblExpenseMainID.Name = "lblExpenseMainID";
this.lblExpenseMainID.Size = new System.Drawing.Size(41, 12);
this.lblExpenseMainID.TabIndex = 1;
this.lblExpenseMainID.Text = "";
//111======25
this.cmbExpenseMainID.Location = new System.Drawing.Point(173,21);
this.cmbExpenseMainID.Name ="cmbExpenseMainID";
this.cmbExpenseMainID.Size = new System.Drawing.Size(100, 21);
this.cmbExpenseMainID.TabIndex = 1;
this.Controls.Add(this.lblExpenseMainID);
this.Controls.Add(this.cmbExpenseMainID);

           //#####300ExpenseName###String
this.lblExpenseName.AutoSize = true;
this.lblExpenseName.Location = new System.Drawing.Point(100,50);
this.lblExpenseName.Name = "lblExpenseName";
this.lblExpenseName.Size = new System.Drawing.Size(41, 12);
this.lblExpenseName.TabIndex = 2;
this.lblExpenseName.Text = "事由";
this.txtExpenseName.Location = new System.Drawing.Point(173,46);
this.txtExpenseName.Name = "txtExpenseName";
this.txtExpenseName.Size = new System.Drawing.Size(100, 21);
this.txtExpenseName.TabIndex = 2;
this.Controls.Add(this.lblExpenseName);
this.Controls.Add(this.txtExpenseName);

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
this.lblDepartmentID.Text = "发生部门";
//111======100
this.cmbDepartmentID.Location = new System.Drawing.Point(173,96);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 4;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ExpenseType_id###Int64
//属性测试125ExpenseType_id
//属性测试125ExpenseType_id
//属性测试125ExpenseType_id
//属性测试125ExpenseType_id
//属性测试125ExpenseType_id
//属性测试125ExpenseType_id
Account_id主外字段不一致。//属性测试125ExpenseType_id
this.lblExpenseType_id.AutoSize = true;
this.lblExpenseType_id.Location = new System.Drawing.Point(100,125);
this.lblExpenseType_id.Name = "lblExpenseType_id";
this.lblExpenseType_id.Size = new System.Drawing.Size(41, 12);
this.lblExpenseType_id.TabIndex = 5;
this.lblExpenseType_id.Text = "费用类型";
//111======125
this.cmbExpenseType_id.Location = new System.Drawing.Point(173,121);
this.cmbExpenseType_id.Name ="cmbExpenseType_id";
this.cmbExpenseType_id.Size = new System.Drawing.Size(100, 21);
this.cmbExpenseType_id.TabIndex = 5;
this.Controls.Add(this.lblExpenseType_id);
this.Controls.Add(this.cmbExpenseType_id);

           //#####Account_id###Int64
//属性测试150Account_id
//属性测试150Account_id
//属性测试150Account_id
//属性测试150Account_id
//属性测试150Account_id
//属性测试150Account_id
Account_id主外字段不一致。//属性测试150Account_id
//属性测试150Account_id
Subject_id主外字段不一致。
           //#####CustomerVendor_ID###Int64
//属性测试175CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,175);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 7;
this.lblCustomerVendor_ID.Text = "交易对象";
//111======175
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,171);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 7;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Subject_id###Int64
//属性测试200Subject_id
//属性测试200Subject_id
//属性测试200Subject_id
//属性测试200Subject_id
//属性测试200Subject_id
//属性测试200Subject_id
Account_id主外字段不一致。//属性测试200Subject_id
//属性测试200Subject_id
Subject_id主外字段不一致。
           //#####CheckOutDate###DateTime
this.lblCheckOutDate.AutoSize = true;
this.lblCheckOutDate.Location = new System.Drawing.Point(100,225);
this.lblCheckOutDate.Name = "lblCheckOutDate";
this.lblCheckOutDate.Size = new System.Drawing.Size(41, 12);
this.lblCheckOutDate.TabIndex = 9;
this.lblCheckOutDate.Text = "交易日期";
//111======225
this.dtpCheckOutDate.Location = new System.Drawing.Point(173,221);
this.dtpCheckOutDate.Name ="dtpCheckOutDate";
this.dtpCheckOutDate.Size = new System.Drawing.Size(100, 21);
this.dtpCheckOutDate.TabIndex = 9;
this.Controls.Add(this.lblCheckOutDate);
this.Controls.Add(this.dtpCheckOutDate);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,250);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 10;
this.lblTotalAmount.Text = "总金额";
//111======250
this.txtTotalAmount.Location = new System.Drawing.Point(173,246);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 10;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,275);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 11;
this.lblIncludeTax.Text = "含税";
this.chkIncludeTax.Location = new System.Drawing.Point(173,271);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 11;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####100Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,300);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 12;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,296);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 12;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,325);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 13;
this.lblTaxAmount.Text = "税额";
//111======325
this.txtTaxAmount.Location = new System.Drawing.Point(173,321);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 13;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,350);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 14;
this.lblTaxRate.Text = "税率";
//111======350
this.txtTaxRate.Location = new System.Drawing.Point(173,346);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 14;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####UntaxedAmount###Decimal
this.lblUntaxedAmount.AutoSize = true;
this.lblUntaxedAmount.Location = new System.Drawing.Point(100,375);
this.lblUntaxedAmount.Name = "lblUntaxedAmount";
this.lblUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedAmount.TabIndex = 15;
this.lblUntaxedAmount.Text = "未税本位币";
//111======375
this.txtUntaxedAmount.Location = new System.Drawing.Point(173,371);
this.txtUntaxedAmount.Name ="txtUntaxedAmount";
this.txtUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedAmount.TabIndex = 15;
this.Controls.Add(this.lblUntaxedAmount);
this.Controls.Add(this.txtUntaxedAmount);

           //#####ProjectGroup_ID###Int64
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,400);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 16;
this.lblProjectGroup_ID.Text = "所属项目";
//111======400
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,396);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 16;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,425);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 17;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,421);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 17;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblExpenseMainID );
this.Controls.Add(this.cmbExpenseMainID );

                this.Controls.Add(this.lblExpenseName );
this.Controls.Add(this.txtExpenseName );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                Account_id主外字段不一致。this.Controls.Add(this.lblExpenseType_id );
this.Controls.Add(this.cmbExpenseType_id );

                Account_id主外字段不一致。Subject_id主外字段不一致。
                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                Account_id主外字段不一致。Subject_id主外字段不一致。
                this.Controls.Add(this.lblCheckOutDate );
this.Controls.Add(this.dtpCheckOutDate );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblUntaxedAmount );
this.Controls.Add(this.txtUntaxedAmount );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                    
            this.Name = "tb_FM_OtherExpenseDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseMainID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbExpenseMainID;
Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpenseName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              Account_id主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseType_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbExpenseType_id;
Subject_id主外字段不一致。
    
        
              Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckOutDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCheckOutDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;
Account_id主外字段不一致。Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
    
   
 





    }
}


