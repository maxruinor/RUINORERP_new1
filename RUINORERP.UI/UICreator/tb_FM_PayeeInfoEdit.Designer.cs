// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:19
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收款信息，供应商报销人的收款账号
    /// </summary>
    partial class tb_FM_PayeeInfoEdit
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
     this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAccount_type = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_type = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_name = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_name = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_No = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_No = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentCodeImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentCodeImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtPaymentCodeImagePath.Multiline = true;

this.lblBelongingBank = new Krypton.Toolkit.KryptonLabel();
this.txtBelongingBank = new Krypton.Toolkit.KryptonTextBox();

this.lblOpeningBank = new Krypton.Toolkit.KryptonLabel();
this.txtOpeningBank = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

this.lblIsDefault = new Krypton.Toolkit.KryptonLabel();
this.chkIsDefault = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsDefault.Values.Text ="";

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

    
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
     
            //#####Employee_ID###Int64
//属性测试25Employee_ID
//属性测试25Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,25);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 1;
this.lblEmployee_ID.Text = "员工";
//111======25
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,21);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Account_type###Int32
//属性测试75Account_type
//属性测试75Account_type
this.lblAccount_type.AutoSize = true;
this.lblAccount_type.Location = new System.Drawing.Point(100,75);
this.lblAccount_type.Name = "lblAccount_type";
this.lblAccount_type.Size = new System.Drawing.Size(41, 12);
this.lblAccount_type.TabIndex = 3;
this.lblAccount_type.Text = "账户类型";
this.txtAccount_type.Location = new System.Drawing.Point(173,71);
this.txtAccount_type.Name = "txtAccount_type";
this.txtAccount_type.Size = new System.Drawing.Size(100, 21);
this.txtAccount_type.TabIndex = 3;
this.Controls.Add(this.lblAccount_type);
this.Controls.Add(this.txtAccount_type);

           //#####50Account_name###String
this.lblAccount_name.AutoSize = true;
this.lblAccount_name.Location = new System.Drawing.Point(100,100);
this.lblAccount_name.Name = "lblAccount_name";
this.lblAccount_name.Size = new System.Drawing.Size(41, 12);
this.lblAccount_name.TabIndex = 4;
this.lblAccount_name.Text = "账户名称";
this.txtAccount_name.Location = new System.Drawing.Point(173,96);
this.txtAccount_name.Name = "txtAccount_name";
this.txtAccount_name.Size = new System.Drawing.Size(100, 21);
this.txtAccount_name.TabIndex = 4;
this.Controls.Add(this.lblAccount_name);
this.Controls.Add(this.txtAccount_name);

           //#####100Account_No###String
this.lblAccount_No.AutoSize = true;
this.lblAccount_No.Location = new System.Drawing.Point(100,125);
this.lblAccount_No.Name = "lblAccount_No";
this.lblAccount_No.Size = new System.Drawing.Size(41, 12);
this.lblAccount_No.TabIndex = 5;
this.lblAccount_No.Text = "账号";
this.txtAccount_No.Location = new System.Drawing.Point(173,121);
this.txtAccount_No.Name = "txtAccount_No";
this.txtAccount_No.Size = new System.Drawing.Size(100, 21);
this.txtAccount_No.TabIndex = 5;
this.Controls.Add(this.lblAccount_No);
this.Controls.Add(this.txtAccount_No);

           //#####300PaymentCodeImagePath###String
this.lblPaymentCodeImagePath.AutoSize = true;
this.lblPaymentCodeImagePath.Location = new System.Drawing.Point(100,150);
this.lblPaymentCodeImagePath.Name = "lblPaymentCodeImagePath";
this.lblPaymentCodeImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentCodeImagePath.TabIndex = 6;
this.lblPaymentCodeImagePath.Text = "收款码";
this.txtPaymentCodeImagePath.Location = new System.Drawing.Point(173,146);
this.txtPaymentCodeImagePath.Name = "txtPaymentCodeImagePath";
this.txtPaymentCodeImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentCodeImagePath.TabIndex = 6;
this.Controls.Add(this.lblPaymentCodeImagePath);
this.Controls.Add(this.txtPaymentCodeImagePath);

           //#####50BelongingBank###String
this.lblBelongingBank.AutoSize = true;
this.lblBelongingBank.Location = new System.Drawing.Point(100,175);
this.lblBelongingBank.Name = "lblBelongingBank";
this.lblBelongingBank.Size = new System.Drawing.Size(41, 12);
this.lblBelongingBank.TabIndex = 7;
this.lblBelongingBank.Text = "所属银行";
this.txtBelongingBank.Location = new System.Drawing.Point(173,171);
this.txtBelongingBank.Name = "txtBelongingBank";
this.txtBelongingBank.Size = new System.Drawing.Size(100, 21);
this.txtBelongingBank.TabIndex = 7;
this.Controls.Add(this.lblBelongingBank);
this.Controls.Add(this.txtBelongingBank);

           //#####60OpeningBank###String
this.lblOpeningBank.AutoSize = true;
this.lblOpeningBank.Location = new System.Drawing.Point(100,200);
this.lblOpeningBank.Name = "lblOpeningBank";
this.lblOpeningBank.Size = new System.Drawing.Size(41, 12);
this.lblOpeningBank.TabIndex = 8;
this.lblOpeningBank.Text = "开户行";
this.txtOpeningBank.Location = new System.Drawing.Point(173,196);
this.txtOpeningBank.Name = "txtOpeningBank";
this.txtOpeningBank.Size = new System.Drawing.Size(100, 21);
this.txtOpeningBank.TabIndex = 8;
this.Controls.Add(this.lblOpeningBank);
this.Controls.Add(this.txtOpeningBank);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsDefault###Boolean
this.lblIsDefault.AutoSize = true;
this.lblIsDefault.Location = new System.Drawing.Point(100,250);
this.lblIsDefault.Name = "lblIsDefault";
this.lblIsDefault.Size = new System.Drawing.Size(41, 12);
this.lblIsDefault.TabIndex = 10;
this.lblIsDefault.Text = "默认账号";
this.chkIsDefault.Location = new System.Drawing.Point(173,246);
this.chkIsDefault.Name = "chkIsDefault";
this.chkIsDefault.Size = new System.Drawing.Size(100, 21);
this.chkIsDefault.TabIndex = 10;
this.Controls.Add(this.lblIsDefault);
this.Controls.Add(this.chkIsDefault);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,275);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 11;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,271);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 11;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

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
           // this.kryptonPanel1.TabIndex = 11;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblAccount_type );
this.Controls.Add(this.txtAccount_type );

                this.Controls.Add(this.lblAccount_name );
this.Controls.Add(this.txtAccount_name );

                this.Controls.Add(this.lblAccount_No );
this.Controls.Add(this.txtAccount_No );

                this.Controls.Add(this.lblPaymentCodeImagePath );
this.Controls.Add(this.txtPaymentCodeImagePath );

                this.Controls.Add(this.lblBelongingBank );
this.Controls.Add(this.txtBelongingBank );

                this.Controls.Add(this.lblOpeningBank );
this.Controls.Add(this.txtOpeningBank );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsDefault );
this.Controls.Add(this.chkIsDefault );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                            // 
            // "tb_FM_PayeeInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PayeeInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_type;
private Krypton.Toolkit.KryptonTextBox txtAccount_type;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_name;
private Krypton.Toolkit.KryptonTextBox txtAccount_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_No;
private Krypton.Toolkit.KryptonTextBox txtAccount_No;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentCodeImagePath;
private Krypton.Toolkit.KryptonTextBox txtPaymentCodeImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblBelongingBank;
private Krypton.Toolkit.KryptonTextBox txtBelongingBank;

    
        
              private Krypton.Toolkit.KryptonLabel lblOpeningBank;
private Krypton.Toolkit.KryptonTextBox txtOpeningBank;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsDefault;
private Krypton.Toolkit.KryptonCheckBox chkIsDefault;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

