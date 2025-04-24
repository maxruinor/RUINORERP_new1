// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:27:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    partial class tb_PayMethodAccountMapperEdit
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
     this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();

this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpiryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpExpiryDate = new Krypton.Toolkit.KryptonDateTimePicker();

    
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
     
            //#####Paytype_ID###Int64
//属性测试25Paytype_ID
//属性测试25Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,25);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 1;
this.lblPaytype_ID.Text = "付款方式";
//111======25
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,21);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 1;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####Account_id###Int64
//属性测试50Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,50);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 2;
this.lblAccount_id.Text = "公司账户";
//111======50
this.cmbAccount_id.Location = new System.Drawing.Point(173,46);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 2;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####50Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,75);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 3;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,71);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 3;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####EffectiveDate###DateTime
this.lblEffectiveDate.AutoSize = true;
this.lblEffectiveDate.Location = new System.Drawing.Point(100,100);
this.lblEffectiveDate.Name = "lblEffectiveDate";
this.lblEffectiveDate.Size = new System.Drawing.Size(41, 12);
this.lblEffectiveDate.TabIndex = 4;
this.lblEffectiveDate.Text = "生效日期";
//111======100
this.dtpEffectiveDate.Location = new System.Drawing.Point(173,96);
this.dtpEffectiveDate.Name ="dtpEffectiveDate";
this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
this.dtpEffectiveDate.TabIndex = 4;
this.Controls.Add(this.lblEffectiveDate);
this.Controls.Add(this.dtpEffectiveDate);

           //#####ExpiryDate###DateTime
this.lblExpiryDate.AutoSize = true;
this.lblExpiryDate.Location = new System.Drawing.Point(100,125);
this.lblExpiryDate.Name = "lblExpiryDate";
this.lblExpiryDate.Size = new System.Drawing.Size(41, 12);
this.lblExpiryDate.TabIndex = 5;
this.lblExpiryDate.Text = "失效日期";
//111======125
this.dtpExpiryDate.Location = new System.Drawing.Point(173,121);
this.dtpExpiryDate.Name ="dtpExpiryDate";
this.dtpExpiryDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpiryDate.TabIndex = 5;
this.Controls.Add(this.lblExpiryDate);
this.Controls.Add(this.dtpExpiryDate);

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
                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblEffectiveDate );
this.Controls.Add(this.dtpEffectiveDate );

                this.Controls.Add(this.lblExpiryDate );
this.Controls.Add(this.dtpExpiryDate );

                            // 
            // "tb_PayMethodAccountMapperEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PayMethodAccountMapperEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpiryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpiryDate;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

