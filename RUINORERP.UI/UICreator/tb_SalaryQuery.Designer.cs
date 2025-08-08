
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
    /// 工资表
    /// </summary>
    partial class tb_SalaryQuery
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
     
     this.lblSalaryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpSalaryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblBaseSalary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBaseSalary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBonus = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBonus = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDeduction = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeduction = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblActualSalary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActualSalary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####SalaryDate###DateTime
this.lblSalaryDate.AutoSize = true;
this.lblSalaryDate.Location = new System.Drawing.Point(100,25);
this.lblSalaryDate.Name = "lblSalaryDate";
this.lblSalaryDate.Size = new System.Drawing.Size(41, 12);
this.lblSalaryDate.TabIndex = 1;
this.lblSalaryDate.Text = "";
//111======25
this.dtpSalaryDate.Location = new System.Drawing.Point(173,21);
this.dtpSalaryDate.Name ="dtpSalaryDate";
this.dtpSalaryDate.ShowCheckBox =true;
this.dtpSalaryDate.Size = new System.Drawing.Size(100, 21);
this.dtpSalaryDate.TabIndex = 1;
this.Controls.Add(this.lblSalaryDate);
this.Controls.Add(this.dtpSalaryDate);

           //#####BaseSalary###Decimal
this.lblBaseSalary.AutoSize = true;
this.lblBaseSalary.Location = new System.Drawing.Point(100,50);
this.lblBaseSalary.Name = "lblBaseSalary";
this.lblBaseSalary.Size = new System.Drawing.Size(41, 12);
this.lblBaseSalary.TabIndex = 2;
this.lblBaseSalary.Text = "";
//111======50
this.txtBaseSalary.Location = new System.Drawing.Point(173,46);
this.txtBaseSalary.Name ="txtBaseSalary";
this.txtBaseSalary.Size = new System.Drawing.Size(100, 21);
this.txtBaseSalary.TabIndex = 2;
this.Controls.Add(this.lblBaseSalary);
this.Controls.Add(this.txtBaseSalary);

           //#####Bonus###Decimal
this.lblBonus.AutoSize = true;
this.lblBonus.Location = new System.Drawing.Point(100,75);
this.lblBonus.Name = "lblBonus";
this.lblBonus.Size = new System.Drawing.Size(41, 12);
this.lblBonus.TabIndex = 3;
this.lblBonus.Text = "";
//111======75
this.txtBonus.Location = new System.Drawing.Point(173,71);
this.txtBonus.Name ="txtBonus";
this.txtBonus.Size = new System.Drawing.Size(100, 21);
this.txtBonus.TabIndex = 3;
this.Controls.Add(this.lblBonus);
this.Controls.Add(this.txtBonus);

           //#####Deduction###Decimal
this.lblDeduction.AutoSize = true;
this.lblDeduction.Location = new System.Drawing.Point(100,100);
this.lblDeduction.Name = "lblDeduction";
this.lblDeduction.Size = new System.Drawing.Size(41, 12);
this.lblDeduction.TabIndex = 4;
this.lblDeduction.Text = "";
//111======100
this.txtDeduction.Location = new System.Drawing.Point(173,96);
this.txtDeduction.Name ="txtDeduction";
this.txtDeduction.Size = new System.Drawing.Size(100, 21);
this.txtDeduction.TabIndex = 4;
this.Controls.Add(this.lblDeduction);
this.Controls.Add(this.txtDeduction);

           //#####ActualSalary###Decimal
this.lblActualSalary.AutoSize = true;
this.lblActualSalary.Location = new System.Drawing.Point(100,125);
this.lblActualSalary.Name = "lblActualSalary";
this.lblActualSalary.Size = new System.Drawing.Size(41, 12);
this.lblActualSalary.TabIndex = 5;
this.lblActualSalary.Text = "";
//111======125
this.txtActualSalary.Location = new System.Drawing.Point(173,121);
this.txtActualSalary.Name ="txtActualSalary";
this.txtActualSalary.Size = new System.Drawing.Size(100, 21);
this.txtActualSalary.TabIndex = 5;
this.Controls.Add(this.lblActualSalary);
this.Controls.Add(this.txtActualSalary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSalaryDate );
this.Controls.Add(this.dtpSalaryDate );

                this.Controls.Add(this.lblBaseSalary );
this.Controls.Add(this.txtBaseSalary );

                this.Controls.Add(this.lblBonus );
this.Controls.Add(this.txtBonus );

                this.Controls.Add(this.lblDeduction );
this.Controls.Add(this.txtDeduction );

                this.Controls.Add(this.lblActualSalary );
this.Controls.Add(this.txtActualSalary );

                    
            this.Name = "tb_SalaryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalaryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpSalaryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBaseSalary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBaseSalary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBonus;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBonus;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeduction;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeduction;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActualSalary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActualSalary;

    
    
   
 





    }
}


