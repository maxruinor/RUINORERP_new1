
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    partial class tb_ConNodeConditionsQuery
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
     
     this.lblField = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtField = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOperator = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOperator = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtValue = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Field###String
this.lblField.AutoSize = true;
this.lblField.Location = new System.Drawing.Point(100,25);
this.lblField.Name = "lblField";
this.lblField.Size = new System.Drawing.Size(41, 12);
this.lblField.TabIndex = 1;
this.lblField.Text = "表达式";
this.txtField.Location = new System.Drawing.Point(173,21);
this.txtField.Name = "txtField";
this.txtField.Size = new System.Drawing.Size(100, 21);
this.txtField.TabIndex = 1;
this.Controls.Add(this.lblField);
this.Controls.Add(this.txtField);

           //#####50Operator###String
this.lblOperator.AutoSize = true;
this.lblOperator.Location = new System.Drawing.Point(100,50);
this.lblOperator.Name = "lblOperator";
this.lblOperator.Size = new System.Drawing.Size(41, 12);
this.lblOperator.TabIndex = 2;
this.lblOperator.Text = "操作符";
this.txtOperator.Location = new System.Drawing.Point(173,46);
this.txtOperator.Name = "txtOperator";
this.txtOperator.Size = new System.Drawing.Size(100, 21);
this.txtOperator.TabIndex = 2;
this.Controls.Add(this.lblOperator);
this.Controls.Add(this.txtOperator);

           //#####50Value###String
this.lblValue.AutoSize = true;
this.lblValue.Location = new System.Drawing.Point(100,75);
this.lblValue.Name = "lblValue";
this.lblValue.Size = new System.Drawing.Size(41, 12);
this.lblValue.TabIndex = 3;
this.lblValue.Text = "表达式值";
this.txtValue.Location = new System.Drawing.Point(173,71);
this.txtValue.Name = "txtValue";
this.txtValue.Size = new System.Drawing.Size(100, 21);
this.txtValue.TabIndex = 3;
this.Controls.Add(this.lblValue);
this.Controls.Add(this.txtValue);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblField );
this.Controls.Add(this.txtField );

                this.Controls.Add(this.lblOperator );
this.Controls.Add(this.txtOperator );

                this.Controls.Add(this.lblValue );
this.Controls.Add(this.txtValue );

                    
            this.Name = "tb_ConNodeConditionsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblField;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtField;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOperator;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOperator;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblValue;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtValue;

    
    
   
 





    }
}


