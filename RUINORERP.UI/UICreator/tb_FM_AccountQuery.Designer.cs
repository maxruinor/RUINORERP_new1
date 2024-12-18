﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:07
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 账户管理，财务系统中使用
    /// </summary>
    partial class tb_FM_AccountQuery
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
     
     this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Subject_id主外字段不一致。
Subject_id主外字段不一致。
this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Subject_id主外字段不一致。
this.lblAccount_name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAccount_name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAccount_No = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAccount_No = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

Subject_id主外字段不一致。
this.lblBank = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBank = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOpeningBalance = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOpeningBalance = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrentBalance = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCurrentBalance = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
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
Subject_id主外字段不一致。
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
Subject_id主外字段不一致。
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                Subject_id主外字段不一致。
                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblAccount_name );
this.Controls.Add(this.txtAccount_name );

                this.Controls.Add(this.lblAccount_No );
this.Controls.Add(this.txtAccount_No );

                Subject_id主外字段不一致。
                this.Controls.Add(this.lblBank );
this.Controls.Add(this.txtBank );

                this.Controls.Add(this.lblOpeningBalance );
this.Controls.Add(this.txtOpeningBalance );

                this.Controls.Add(this.lblCurrentBalance );
this.Controls.Add(this.txtCurrentBalance );

                    
            this.Name = "tb_FM_AccountQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
Subject_id主外字段不一致。
    
        
              Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;
Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAccount_name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_No;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAccount_No;

    
        
              Subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBank;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBank;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOpeningBalance;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOpeningBalance;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrentBalance;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCurrentBalance;

    
    
   
 





    }
}


