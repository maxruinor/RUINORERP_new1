
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 
    /// </summary>
    partial class tb_ApprovalQuery
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
     
     this.lblBillType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBillType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBillName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBillName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBillEntityClassName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBillEntityClassName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblGradedAudit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGradedAudit = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGradedAudit.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50BillType###String
this.lblBillType.AutoSize = true;
this.lblBillType.Location = new System.Drawing.Point(100,25);
this.lblBillType.Name = "lblBillType";
this.lblBillType.Size = new System.Drawing.Size(41, 12);
this.lblBillType.TabIndex = 1;
this.lblBillType.Text = "单据类型";
this.txtBillType.Location = new System.Drawing.Point(173,21);
this.txtBillType.Name = "txtBillType";
this.txtBillType.Size = new System.Drawing.Size(100, 21);
this.txtBillType.TabIndex = 1;
this.Controls.Add(this.lblBillType);
this.Controls.Add(this.txtBillType);

           //#####100BillName###String
this.lblBillName.AutoSize = true;
this.lblBillName.Location = new System.Drawing.Point(100,50);
this.lblBillName.Name = "lblBillName";
this.lblBillName.Size = new System.Drawing.Size(41, 12);
this.lblBillName.TabIndex = 2;
this.lblBillName.Text = "单据名称";
this.txtBillName.Location = new System.Drawing.Point(173,46);
this.txtBillName.Name = "txtBillName";
this.txtBillName.Size = new System.Drawing.Size(100, 21);
this.txtBillName.TabIndex = 2;
this.Controls.Add(this.lblBillName);
this.Controls.Add(this.txtBillName);

           //#####50BillEntityClassName###String
this.lblBillEntityClassName.AutoSize = true;
this.lblBillEntityClassName.Location = new System.Drawing.Point(100,75);
this.lblBillEntityClassName.Name = "lblBillEntityClassName";
this.lblBillEntityClassName.Size = new System.Drawing.Size(41, 12);
this.lblBillEntityClassName.TabIndex = 3;
this.lblBillEntityClassName.Text = "";
this.txtBillEntityClassName.Location = new System.Drawing.Point(173,71);
this.txtBillEntityClassName.Name = "txtBillEntityClassName";
this.txtBillEntityClassName.Size = new System.Drawing.Size(100, 21);
this.txtBillEntityClassName.TabIndex = 3;
this.Controls.Add(this.lblBillEntityClassName);
this.Controls.Add(this.txtBillEntityClassName);

           //#####ApprovalResults###Int32

           //#####GradedAudit###Boolean
this.lblGradedAudit.AutoSize = true;
this.lblGradedAudit.Location = new System.Drawing.Point(100,125);
this.lblGradedAudit.Name = "lblGradedAudit";
this.lblGradedAudit.Size = new System.Drawing.Size(41, 12);
this.lblGradedAudit.TabIndex = 5;
this.lblGradedAudit.Text = "分级审核";
this.chkGradedAudit.Location = new System.Drawing.Point(173,121);
this.chkGradedAudit.Name = "chkGradedAudit";
this.chkGradedAudit.Size = new System.Drawing.Size(100, 21);
this.chkGradedAudit.TabIndex = 5;
this.Controls.Add(this.lblGradedAudit);
this.Controls.Add(this.chkGradedAudit);

           //#####Module###Int32

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBillType );
this.Controls.Add(this.txtBillType );

                this.Controls.Add(this.lblBillName );
this.Controls.Add(this.txtBillName );

                this.Controls.Add(this.lblBillEntityClassName );
this.Controls.Add(this.txtBillEntityClassName );

                
                this.Controls.Add(this.lblGradedAudit );
this.Controls.Add(this.chkGradedAudit );

                
                    
            this.Name = "tb_ApprovalQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBillType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBillName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillEntityClassName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBillEntityClassName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGradedAudit;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGradedAudit;

    
        
              
    
    
   
 





    }
}


