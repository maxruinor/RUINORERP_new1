// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 付款账号管理
    /// </summary>
    partial class tb_FM_AccountEdit
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
     this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblSubject_id = new Krypton.Toolkit.KryptonLabel();
this.cmbSubject_id = new Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAccount_name = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_name = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_No = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_No = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_type = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_type = new Krypton.Toolkit.KryptonTextBox();

this.lblBank = new Krypton.Toolkit.KryptonLabel();
this.txtBank = new Krypton.Toolkit.KryptonTextBox();

this.lblOpeningBalance = new Krypton.Toolkit.KryptonLabel();
this.txtOpeningBalance = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrentBalance = new Krypton.Toolkit.KryptonLabel();
this.txtCurrentBalance = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####DepartmentID###Int64
//属性测试25DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,25);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 1;
this.lblDepartmentID.Text = "部门";
//111======25
this.cmbDepartmentID.Location = new System.Drawing.Point(173,21);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 1;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Subject_id###Int64
//属性测试50Subject_id
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
//属性测试75Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,75);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 3;
this.lblCurrency_ID.Text = "币种";
//111======75
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,71);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 3;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

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

           //#####Account_type###Int32
//属性测试150Account_type
//属性测试150Account_type
//属性测试150Account_type
this.lblAccount_type.AutoSize = true;
this.lblAccount_type.Location = new System.Drawing.Point(100,150);
this.lblAccount_type.Name = "lblAccount_type";
this.lblAccount_type.Size = new System.Drawing.Size(41, 12);
this.lblAccount_type.TabIndex = 6;
this.lblAccount_type.Text = "账户类型";
this.txtAccount_type.Location = new System.Drawing.Point(173,146);
this.txtAccount_type.Name = "txtAccount_type";
this.txtAccount_type.Size = new System.Drawing.Size(100, 21);
this.txtAccount_type.TabIndex = 6;
this.Controls.Add(this.lblAccount_type);
this.Controls.Add(this.txtAccount_type);

           //#####30Bank###String
this.lblBank.AutoSize = true;
this.lblBank.Location = new System.Drawing.Point(100,175);
this.lblBank.Name = "lblBank";
this.lblBank.Size = new System.Drawing.Size(41, 12);
this.lblBank.TabIndex = 7;
this.lblBank.Text = "所属银行";
this.txtBank.Location = new System.Drawing.Point(173,171);
this.txtBank.Name = "txtBank";
this.txtBank.Size = new System.Drawing.Size(100, 21);
this.txtBank.TabIndex = 7;
this.Controls.Add(this.lblBank);
this.Controls.Add(this.txtBank);

           //#####OpeningBalance###Decimal
this.lblOpeningBalance.AutoSize = true;
this.lblOpeningBalance.Location = new System.Drawing.Point(100,200);
this.lblOpeningBalance.Name = "lblOpeningBalance";
this.lblOpeningBalance.Size = new System.Drawing.Size(41, 12);
this.lblOpeningBalance.TabIndex = 8;
this.lblOpeningBalance.Text = "初始余额";
//111======200
this.txtOpeningBalance.Location = new System.Drawing.Point(173,196);
this.txtOpeningBalance.Name ="txtOpeningBalance";
this.txtOpeningBalance.Size = new System.Drawing.Size(100, 21);
this.txtOpeningBalance.TabIndex = 8;
this.Controls.Add(this.lblOpeningBalance);
this.Controls.Add(this.txtOpeningBalance);

           //#####CurrentBalance###Decimal
this.lblCurrentBalance.AutoSize = true;
this.lblCurrentBalance.Location = new System.Drawing.Point(100,225);
this.lblCurrentBalance.Name = "lblCurrentBalance";
this.lblCurrentBalance.Size = new System.Drawing.Size(41, 12);
this.lblCurrentBalance.TabIndex = 9;
this.lblCurrentBalance.Text = "当前余额";
//111======225
this.txtCurrentBalance.Location = new System.Drawing.Point(173,221);
this.txtCurrentBalance.Name ="txtCurrentBalance";
this.txtCurrentBalance.Size = new System.Drawing.Size(100, 21);
this.txtCurrentBalance.TabIndex = 9;
this.Controls.Add(this.lblCurrentBalance);
this.Controls.Add(this.txtCurrentBalance);

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
           // this.kryptonPanel1.TabIndex = 9;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblSubject_id );
this.Controls.Add(this.cmbSubject_id );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblAccount_name );
this.Controls.Add(this.txtAccount_name );

                this.Controls.Add(this.lblAccount_No );
this.Controls.Add(this.txtAccount_No );

                this.Controls.Add(this.lblAccount_type );
this.Controls.Add(this.txtAccount_type );

                this.Controls.Add(this.lblBank );
this.Controls.Add(this.txtBank );

                this.Controls.Add(this.lblOpeningBalance );
this.Controls.Add(this.txtOpeningBalance );

                this.Controls.Add(this.lblCurrentBalance );
this.Controls.Add(this.txtCurrentBalance );

                            // 
            // "tb_FM_AccountEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_AccountEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubject_id;
private Krypton.Toolkit.KryptonComboBox cmbSubject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_name;
private Krypton.Toolkit.KryptonTextBox txtAccount_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_No;
private Krypton.Toolkit.KryptonTextBox txtAccount_No;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_type;
private Krypton.Toolkit.KryptonTextBox txtAccount_type;

    
        
              private Krypton.Toolkit.KryptonLabel lblBank;
private Krypton.Toolkit.KryptonTextBox txtBank;

    
        
              private Krypton.Toolkit.KryptonLabel lblOpeningBalance;
private Krypton.Toolkit.KryptonTextBox txtOpeningBalance;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrentBalance;
private Krypton.Toolkit.KryptonTextBox txtCurrentBalance;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

