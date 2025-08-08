
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 质检表
    /// </summary>
    partial class tb_QualityInspectionQuery
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
     
     this.lblInspectionDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpInspectionDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblInspectionResult = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInspectionResult = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtInspectionResult.Multiline = true;


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####InspectionDate###DateTime
this.lblInspectionDate.AutoSize = true;
this.lblInspectionDate.Location = new System.Drawing.Point(100,25);
this.lblInspectionDate.Name = "lblInspectionDate";
this.lblInspectionDate.Size = new System.Drawing.Size(41, 12);
this.lblInspectionDate.TabIndex = 1;
this.lblInspectionDate.Text = "";
//111======25
this.dtpInspectionDate.Location = new System.Drawing.Point(173,21);
this.dtpInspectionDate.Name ="dtpInspectionDate";
this.dtpInspectionDate.ShowCheckBox =true;
this.dtpInspectionDate.Size = new System.Drawing.Size(100, 21);
this.dtpInspectionDate.TabIndex = 1;
this.Controls.Add(this.lblInspectionDate);
this.Controls.Add(this.dtpInspectionDate);

           //#####500InspectionResult###String
this.lblInspectionResult.AutoSize = true;
this.lblInspectionResult.Location = new System.Drawing.Point(100,50);
this.lblInspectionResult.Name = "lblInspectionResult";
this.lblInspectionResult.Size = new System.Drawing.Size(41, 12);
this.lblInspectionResult.TabIndex = 2;
this.lblInspectionResult.Text = "";
this.txtInspectionResult.Location = new System.Drawing.Point(173,46);
this.txtInspectionResult.Name = "txtInspectionResult";
this.txtInspectionResult.Size = new System.Drawing.Size(100, 21);
this.txtInspectionResult.TabIndex = 2;
this.Controls.Add(this.lblInspectionResult);
this.Controls.Add(this.txtInspectionResult);

           //#####ProductID###Int32

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblInspectionDate );
this.Controls.Add(this.dtpInspectionDate );

                this.Controls.Add(this.lblInspectionResult );
this.Controls.Add(this.txtInspectionResult );

                
                    
            this.Name = "tb_QualityInspectionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInspectionDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpInspectionDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInspectionResult;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInspectionResult;

    
        
              
    
    
   
 





    }
}


