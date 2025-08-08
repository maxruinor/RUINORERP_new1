
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:12
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 薪资发放表
    /// </summary>
    partial class tb_SalaryPaymentQuery
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
     
     this.lblsalary_month = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpsalary_month = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblamount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtamount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####salary_month###DateTime
this.lblsalary_month.AutoSize = true;
this.lblsalary_month.Location = new System.Drawing.Point(100,25);
this.lblsalary_month.Name = "lblsalary_month";
this.lblsalary_month.Size = new System.Drawing.Size(41, 12);
this.lblsalary_month.TabIndex = 1;
this.lblsalary_month.Text = "";
//111======25
this.dtpsalary_month.Location = new System.Drawing.Point(173,21);
this.dtpsalary_month.Name ="dtpsalary_month";
this.dtpsalary_month.ShowCheckBox =true;
this.dtpsalary_month.Size = new System.Drawing.Size(100, 21);
this.dtpsalary_month.TabIndex = 1;
this.Controls.Add(this.lblsalary_month);
this.Controls.Add(this.dtpsalary_month);

           //#####amount###Decimal
this.lblamount.AutoSize = true;
this.lblamount.Location = new System.Drawing.Point(100,50);
this.lblamount.Name = "lblamount";
this.lblamount.Size = new System.Drawing.Size(41, 12);
this.lblamount.TabIndex = 2;
this.lblamount.Text = "";
//111======50
this.txtamount.Location = new System.Drawing.Point(173,46);
this.txtamount.Name ="txtamount";
this.txtamount.Size = new System.Drawing.Size(100, 21);
this.txtamount.TabIndex = 2;
this.Controls.Add(this.lblamount);
this.Controls.Add(this.txtamount);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblsalary_month );
this.Controls.Add(this.dtpsalary_month );

                this.Controls.Add(this.lblamount );
this.Controls.Add(this.txtamount );

                    
            this.Name = "tb_SalaryPaymentQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsalary_month;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpsalary_month;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblamount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtamount;

    
    
   
 





    }
}


