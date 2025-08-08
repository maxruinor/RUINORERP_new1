// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 费用报销单明细
    /// </summary>
    partial class tb_FM_ExpenseClaimDetailEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblClaimMainID = new Krypton.Toolkit.KryptonLabel();
this.cmbClaimMainID = new Krypton.Toolkit.KryptonComboBox();

this.lblClaimName = new Krypton.Toolkit.KryptonLabel();
this.txtClaimName = new Krypton.Toolkit.KryptonTextBox();
this.txtClaimName.Multiline = true;

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblExpenseType_id = new Krypton.Toolkit.KryptonLabel();
this.cmbExpenseType_id = new Krypton.Toolkit.KryptonComboBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();

this.lblSubject_id = new Krypton.Toolkit.KryptonLabel();
this.cmbSubject_id = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblTranDate = new Krypton.Toolkit.KryptonLabel();
this.dtpTranDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSingleAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSingleAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblEvidenceImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
            //#####ClaimMainID###Int64
//属性测试25ClaimMainID
//属性测试25ClaimMainID
//属性测试25ClaimMainID
//属性测试25ClaimMainID
//属性测试25ClaimMainID
//属性测试25ClaimMainID
this.lblClaimMainID.AutoSize = true;
this.lblClaimMainID.Location = new System.Drawing.Point(100,25);
this.lblClaimMainID.Name = "lblClaimMainID";
this.lblClaimMainID.Size = new System.Drawing.Size(41, 12);
this.lblClaimMainID.TabIndex = 1;
this.lblClaimMainID.Text = "";
//111======25
this.cmbClaimMainID.Location = new System.Drawing.Point(173,21);
this.cmbClaimMainID.Name ="cmbClaimMainID";
this.cmbClaimMainID.Size = new System.Drawing.Size(100, 21);
this.cmbClaimMainID.TabIndex = 1;
this.Controls.Add(this.lblClaimMainID);
this.Controls.Add(this.cmbClaimMainID);

           //#####300ClaimName###String
this.lblClaimName.AutoSize = true;
this.lblClaimName.Location = new System.Drawing.Point(100,50);
this.lblClaimName.Name = "lblClaimName";
this.lblClaimName.Size = new System.Drawing.Size(41, 12);
this.lblClaimName.TabIndex = 2;
this.lblClaimName.Text = "事由";
this.txtClaimName.Location = new System.Drawing.Point(173,46);
this.txtClaimName.Name = "txtClaimName";
this.txtClaimName.Size = new System.Drawing.Size(100, 21);
this.txtClaimName.TabIndex = 2;
this.Controls.Add(this.lblClaimName);
this.Controls.Add(this.txtClaimName);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "报销部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ExpenseType_id###Int64
//属性测试100ExpenseType_id
//属性测试100ExpenseType_id
this.lblExpenseType_id.AutoSize = true;
this.lblExpenseType_id.Location = new System.Drawing.Point(100,100);
this.lblExpenseType_id.Name = "lblExpenseType_id";
this.lblExpenseType_id.Size = new System.Drawing.Size(41, 12);
this.lblExpenseType_id.TabIndex = 4;
this.lblExpenseType_id.Text = "费用类型";
//111======100
this.cmbExpenseType_id.Location = new System.Drawing.Point(173,96);
this.cmbExpenseType_id.Name ="cmbExpenseType_id";
this.cmbExpenseType_id.Size = new System.Drawing.Size(100, 21);
this.cmbExpenseType_id.TabIndex = 4;
this.Controls.Add(this.lblExpenseType_id);
this.Controls.Add(this.cmbExpenseType_id);

           //#####Account_id###Int64
//属性测试125Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,125);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 5;
this.lblAccount_id.Text = "支付账号";
//111======125
this.cmbAccount_id.Location = new System.Drawing.Point(173,121);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 5;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####Subject_id###Int64
//属性测试150Subject_id
//属性测试150Subject_id
//属性测试150Subject_id
//属性测试150Subject_id
this.lblSubject_id.AutoSize = true;
this.lblSubject_id.Location = new System.Drawing.Point(100,150);
this.lblSubject_id.Name = "lblSubject_id";
this.lblSubject_id.Size = new System.Drawing.Size(41, 12);
this.lblSubject_id.TabIndex = 6;
this.lblSubject_id.Text = "会计科目";
//111======150
this.cmbSubject_id.Location = new System.Drawing.Point(173,146);
this.cmbSubject_id.Name ="cmbSubject_id";
this.cmbSubject_id.Size = new System.Drawing.Size(100, 21);
this.cmbSubject_id.TabIndex = 6;
this.Controls.Add(this.lblSubject_id);
this.Controls.Add(this.cmbSubject_id);

           //#####ProjectGroup_ID###Int64
//属性测试175ProjectGroup_ID
//属性测试175ProjectGroup_ID
//属性测试175ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,175);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 7;
this.lblProjectGroup_ID.Text = "所属项目";
//111======175
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,171);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 7;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####TranDate###DateTime
this.lblTranDate.AutoSize = true;
this.lblTranDate.Location = new System.Drawing.Point(100,200);
this.lblTranDate.Name = "lblTranDate";
this.lblTranDate.Size = new System.Drawing.Size(41, 12);
this.lblTranDate.TabIndex = 8;
this.lblTranDate.Text = "发生日期";
//111======200
this.dtpTranDate.Location = new System.Drawing.Point(173,196);
this.dtpTranDate.Name ="dtpTranDate";
this.dtpTranDate.Size = new System.Drawing.Size(100, 21);
this.dtpTranDate.TabIndex = 8;
this.Controls.Add(this.lblTranDate);
this.Controls.Add(this.dtpTranDate);

           //#####SingleAmount###Decimal
this.lblSingleAmount.AutoSize = true;
this.lblSingleAmount.Location = new System.Drawing.Point(100,225);
this.lblSingleAmount.Name = "lblSingleAmount";
this.lblSingleAmount.Size = new System.Drawing.Size(41, 12);
this.lblSingleAmount.TabIndex = 9;
this.lblSingleAmount.Text = "单项总金额";
//111======225
this.txtSingleAmount.Location = new System.Drawing.Point(173,221);
this.txtSingleAmount.Name ="txtSingleAmount";
this.txtSingleAmount.Size = new System.Drawing.Size(100, 21);
this.txtSingleAmount.TabIndex = 9;
this.Controls.Add(this.lblSingleAmount);
this.Controls.Add(this.txtSingleAmount);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,250);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 10;
this.lblIncludeTax.Text = "含税";
this.chkIncludeTax.Location = new System.Drawing.Point(173,246);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 10;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,275);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 11;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,271);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 11;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,300);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 12;
this.lblTaxRate.Text = "税率";
//111======300
this.txtTaxRate.Location = new System.Drawing.Point(173,296);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 12;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

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

           //#####UntaxedAmount###Decimal
this.lblUntaxedAmount.AutoSize = true;
this.lblUntaxedAmount.Location = new System.Drawing.Point(100,350);
this.lblUntaxedAmount.Name = "lblUntaxedAmount";
this.lblUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedAmount.TabIndex = 14;
this.lblUntaxedAmount.Text = "未税本位币";
//111======350
this.txtUntaxedAmount.Location = new System.Drawing.Point(173,346);
this.txtUntaxedAmount.Name ="txtUntaxedAmount";
this.txtUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedAmount.TabIndex = 14;
this.Controls.Add(this.lblUntaxedAmount);
this.Controls.Add(this.txtUntaxedAmount);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,375);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 15;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,371);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 15;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 15;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblClaimMainID );
this.Controls.Add(this.cmbClaimMainID );

                this.Controls.Add(this.lblClaimName );
this.Controls.Add(this.txtClaimName );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblExpenseType_id );
this.Controls.Add(this.cmbExpenseType_id );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblSubject_id );
this.Controls.Add(this.cmbSubject_id );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblTranDate );
this.Controls.Add(this.dtpTranDate );

                this.Controls.Add(this.lblSingleAmount );
this.Controls.Add(this.txtSingleAmount );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblUntaxedAmount );
this.Controls.Add(this.txtUntaxedAmount );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                            // 
            // "tb_FM_ExpenseClaimDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_ExpenseClaimDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblClaimMainID;
private Krypton.Toolkit.KryptonComboBox cmbClaimMainID;

    
        
              private Krypton.Toolkit.KryptonLabel lblClaimName;
private Krypton.Toolkit.KryptonTextBox txtClaimName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseType_id;
private Krypton.Toolkit.KryptonComboBox cmbExpenseType_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubject_id;
private Krypton.Toolkit.KryptonComboBox cmbSubject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTranDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpTranDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSingleAmount;
private Krypton.Toolkit.KryptonTextBox txtSingleAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

