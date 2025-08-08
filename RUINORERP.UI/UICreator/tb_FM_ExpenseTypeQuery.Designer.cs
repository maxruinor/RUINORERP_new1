
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
    /// 业务类型 报销，员工借支还款，运费
    /// </summary>
    partial class tb_FM_ExpenseTypeQuery
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
     
     subject_id主外字段不一致。
this.lblExpense_name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExpense_name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEXPOrINC.Values.Text ="";
this.chkEXPOrINC.Checked = true;
this.chkEXPOrINC.CheckState = System.Windows.Forms.CheckState.Checked;

subject_id主外字段不一致。
this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####subject_id###Int64
//属性测试25subject_id
subject_id主外字段不一致。
           //#####50Expense_name###String
this.lblExpense_name.AutoSize = true;
this.lblExpense_name.Location = new System.Drawing.Point(100,50);
this.lblExpense_name.Name = "lblExpense_name";
this.lblExpense_name.Size = new System.Drawing.Size(41, 12);
this.lblExpense_name.TabIndex = 2;
this.lblExpense_name.Text = "费用业务名称";
this.txtExpense_name.Location = new System.Drawing.Point(173,46);
this.txtExpense_name.Name = "txtExpense_name";
this.txtExpense_name.Size = new System.Drawing.Size(100, 21);
this.txtExpense_name.TabIndex = 2;
this.Controls.Add(this.lblExpense_name);
this.Controls.Add(this.txtExpense_name);

           //#####EXPOrINC###Boolean
this.lblEXPOrINC.AutoSize = true;
this.lblEXPOrINC.Location = new System.Drawing.Point(100,75);
this.lblEXPOrINC.Name = "lblEXPOrINC";
this.lblEXPOrINC.Size = new System.Drawing.Size(41, 12);
this.lblEXPOrINC.TabIndex = 3;
this.lblEXPOrINC.Text = "收支标识";
this.chkEXPOrINC.Location = new System.Drawing.Point(173,71);
this.chkEXPOrINC.Name = "chkEXPOrINC";
this.chkEXPOrINC.Size = new System.Drawing.Size(100, 21);
this.chkEXPOrINC.TabIndex = 3;
this.Controls.Add(this.lblEXPOrINC);
this.Controls.Add(this.chkEXPOrINC);

           //#####ReceivePaymentType###Int32
//属性测试100ReceivePaymentType
subject_id主外字段不一致。
           //#####30Notes###String
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
                subject_id主外字段不一致。
                this.Controls.Add(this.lblExpense_name );
this.Controls.Add(this.txtExpense_name );

                this.Controls.Add(this.lblEXPOrINC );
this.Controls.Add(this.chkEXPOrINC );

                subject_id主外字段不一致。
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_FM_ExpenseTypeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpense_name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpense_name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEXPOrINC;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;

    
        
              subject_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


