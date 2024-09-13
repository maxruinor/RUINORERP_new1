
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 银行账号信息表
    /// </summary>
    partial class tb_BankAccountQuery
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
     
     this.lblAccount_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAccount_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAccount_No = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAccount_No = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOpeningBank = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOpeningBank = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100Account_Name###String
this.lblAccount_Name.AutoSize = true;
this.lblAccount_Name.Location = new System.Drawing.Point(100,25);
this.lblAccount_Name.Name = "lblAccount_Name";
this.lblAccount_Name.Size = new System.Drawing.Size(41, 12);
this.lblAccount_Name.TabIndex = 1;
this.lblAccount_Name.Text = "账户名称";
this.txtAccount_Name.Location = new System.Drawing.Point(173,21);
this.txtAccount_Name.Name = "txtAccount_Name";
this.txtAccount_Name.Size = new System.Drawing.Size(100, 21);
this.txtAccount_Name.TabIndex = 1;
this.Controls.Add(this.lblAccount_Name);
this.Controls.Add(this.txtAccount_Name);

           //#####100Account_No###String
this.lblAccount_No.AutoSize = true;
this.lblAccount_No.Location = new System.Drawing.Point(100,50);
this.lblAccount_No.Name = "lblAccount_No";
this.lblAccount_No.Size = new System.Drawing.Size(41, 12);
this.lblAccount_No.TabIndex = 2;
this.lblAccount_No.Text = "账号";
this.txtAccount_No.Location = new System.Drawing.Point(173,46);
this.txtAccount_No.Name = "txtAccount_No";
this.txtAccount_No.Size = new System.Drawing.Size(100, 21);
this.txtAccount_No.TabIndex = 2;
this.Controls.Add(this.lblAccount_No);
this.Controls.Add(this.txtAccount_No);

           //#####100OpeningBank###String
this.lblOpeningBank.AutoSize = true;
this.lblOpeningBank.Location = new System.Drawing.Point(100,75);
this.lblOpeningBank.Name = "lblOpeningBank";
this.lblOpeningBank.Size = new System.Drawing.Size(41, 12);
this.lblOpeningBank.TabIndex = 3;
this.lblOpeningBank.Text = "开户行";
this.txtOpeningBank.Location = new System.Drawing.Point(173,71);
this.txtOpeningBank.Name = "txtOpeningBank";
this.txtOpeningBank.Size = new System.Drawing.Size(100, 21);
this.txtOpeningBank.TabIndex = 3;
this.Controls.Add(this.lblOpeningBank);
this.Controls.Add(this.txtOpeningBank);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,100);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 4;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,96);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 4;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAccount_Name );
this.Controls.Add(this.txtAccount_Name );

                this.Controls.Add(this.lblAccount_No );
this.Controls.Add(this.txtAccount_No );

                this.Controls.Add(this.lblOpeningBank );
this.Controls.Add(this.txtOpeningBank );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_BankAccountQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAccount_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_No;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAccount_No;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOpeningBank;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOpeningBank;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


