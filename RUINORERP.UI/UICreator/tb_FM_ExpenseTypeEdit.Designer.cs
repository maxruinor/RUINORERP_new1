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
    partial class tb_FM_ExpenseTypeEdit
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
     subject_id主外字段不一致。this.lblsubject_id = new Krypton.Toolkit.KryptonLabel();
this.txtsubject_id = new Krypton.Toolkit.KryptonTextBox();

this.lblExpense_name = new Krypton.Toolkit.KryptonLabel();
this.txtExpense_name = new Krypton.Toolkit.KryptonTextBox();

this.lblEXPOrINC = new Krypton.Toolkit.KryptonLabel();
this.chkEXPOrINC = new Krypton.Toolkit.KryptonCheckBox();
this.chkEXPOrINC.Values.Text ="";
this.chkEXPOrINC.Checked = true;
this.chkEXPOrINC.CheckState = System.Windows.Forms.CheckState.Checked;

subject_id主外字段不一致。this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####subject_id###Int64
//属性测试25subject_id
subject_id主外字段不一致。this.lblsubject_id.AutoSize = true;
this.lblsubject_id.Location = new System.Drawing.Point(100,25);
this.lblsubject_id.Name = "lblsubject_id";
this.lblsubject_id.Size = new System.Drawing.Size(41, 12);
this.lblsubject_id.TabIndex = 1;
this.lblsubject_id.Text = "科目";
this.txtsubject_id.Location = new System.Drawing.Point(173,21);
this.txtsubject_id.Name = "txtsubject_id";
this.txtsubject_id.Size = new System.Drawing.Size(100, 21);
this.txtsubject_id.TabIndex = 1;
this.Controls.Add(this.lblsubject_id);
this.Controls.Add(this.txtsubject_id);

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
subject_id主外字段不一致。this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,100);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 4;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,96);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 4;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

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
           // this.kryptonPanel1.TabIndex = 5;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                subject_id主外字段不一致。this.Controls.Add(this.lblsubject_id );
this.Controls.Add(this.txtsubject_id );

                this.Controls.Add(this.lblExpense_name );
this.Controls.Add(this.txtExpense_name );

                this.Controls.Add(this.lblEXPOrINC );
this.Controls.Add(this.chkEXPOrINC );

                subject_id主外字段不一致。this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_FM_ExpenseTypeEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_ExpenseTypeEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              subject_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblsubject_id;
private Krypton.Toolkit.KryptonTextBox txtsubject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpense_name;
private Krypton.Toolkit.KryptonTextBox txtExpense_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblEXPOrINC;
private Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;

    
        
              subject_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

