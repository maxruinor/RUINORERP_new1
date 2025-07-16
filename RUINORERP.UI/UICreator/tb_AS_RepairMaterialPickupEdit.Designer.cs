// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:31:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修领料单
    /// </summary>
    partial class tb_AS_RepairMaterialPickupEdit
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
            this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
            this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // lblApprovalOpinions
            // 
            this.lblApprovalOpinions.Location = new System.Drawing.Point(109, 217);
            this.lblApprovalOpinions.Name = "lblApprovalOpinions";
            this.lblApprovalOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalOpinions.TabIndex = 17;
            this.lblApprovalOpinions.Values.Text = "审批意见";
            // 
            // txtApprovalOpinions
            // 
            this.txtApprovalOpinions.Location = new System.Drawing.Point(182, 213);
            this.txtApprovalOpinions.Name = "txtApprovalOpinions";
            this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 23);
            this.txtApprovalOpinions.TabIndex = 17;
            // 
            // lblApprover_by
            // 
            this.lblApprover_by.Location = new System.Drawing.Point(109, 242);
            this.lblApprover_by.Name = "lblApprover_by";
            this.lblApprover_by.Size = new System.Drawing.Size(49, 20);
            this.lblApprover_by.TabIndex = 18;
            this.lblApprover_by.Values.Text = "审批人";
            // 
            // txtApprover_by
            // 
            this.txtApprover_by.Location = new System.Drawing.Point(182, 238);
            this.txtApprover_by.Name = "txtApprover_by";
            this.txtApprover_by.Size = new System.Drawing.Size(100, 23);
            this.txtApprover_by.TabIndex = 18;
            // 
            // lblApprover_at
            // 
            this.lblApprover_at.Location = new System.Drawing.Point(109, 267);
            this.lblApprover_at.Name = "lblApprover_at";
            this.lblApprover_at.Size = new System.Drawing.Size(62, 20);
            this.lblApprover_at.TabIndex = 19;
            this.lblApprover_at.Values.Text = "审批时间";
            // 
            // dtpApprover_at
            // 
            this.dtpApprover_at.Location = new System.Drawing.Point(182, 263);
            this.dtpApprover_at.Name = "dtpApprover_at";
            this.dtpApprover_at.ShowCheckBox = true;
            this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
            this.dtpApprover_at.TabIndex = 19;
            // 
            // lblApprovalResults
            // 
            this.lblApprovalResults.Location = new System.Drawing.Point(109, 293);
            this.lblApprovalResults.Name = "lblApprovalResults";
            this.lblApprovalResults.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalResults.TabIndex = 21;
            this.lblApprovalResults.Values.Text = "审批结果";
            // 
            // chkApprovalResults
            // 
            this.chkApprovalResults.Location = new System.Drawing.Point(182, 289);
            this.chkApprovalResults.Name = "chkApprovalResults";
            this.chkApprovalResults.Size = new System.Drawing.Size(19, 13);
            this.chkApprovalResults.TabIndex = 21;
            this.chkApprovalResults.Values.Text = "";
            // 
            // tb_AS_RepairMaterialPickupEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblApprovalOpinions);
            this.Controls.Add(this.txtApprovalOpinions);
            this.Controls.Add(this.lblApprover_by);
            this.Controls.Add(this.txtApprover_by);
            this.Controls.Add(this.lblApprover_at);
            this.Controls.Add(this.dtpApprover_at);
            this.Controls.Add(this.lblApprovalResults);
            this.Controls.Add(this.chkApprovalResults);
            this.Name = "tb_AS_RepairMaterialPickupEdit";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
















    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;




    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

