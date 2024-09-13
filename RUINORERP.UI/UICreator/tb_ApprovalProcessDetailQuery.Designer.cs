
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 审核流程明细表
    /// </summary>
    partial class tb_ApprovalProcessDetailQuery
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
     
     this.lblApprovalID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbApprovalID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ApprovalID###Int64
//属性测试25ApprovalID
this.lblApprovalID.AutoSize = true;
this.lblApprovalID.Location = new System.Drawing.Point(100,25);
this.lblApprovalID.Name = "lblApprovalID";
this.lblApprovalID.Size = new System.Drawing.Size(41, 12);
this.lblApprovalID.TabIndex = 1;
this.lblApprovalID.Text = "";
//111======25
this.cmbApprovalID.Location = new System.Drawing.Point(173,21);
this.cmbApprovalID.Name ="cmbApprovalID";
this.cmbApprovalID.Size = new System.Drawing.Size(100, 21);
this.cmbApprovalID.TabIndex = 1;
this.Controls.Add(this.lblApprovalID);
this.Controls.Add(this.cmbApprovalID);

           //#####ApprovalResults###Int32
//属性测试50ApprovalResults

           //#####ApprovalOrder###Int32
//属性测试75ApprovalOrder

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblApprovalID );
this.Controls.Add(this.cmbApprovalID );

                
                
                    
            this.Name = "tb_ApprovalProcessDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbApprovalID;

    
        
              
    
        
              
    
    
   
 





    }
}


