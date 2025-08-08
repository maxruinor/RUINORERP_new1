
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程图定义
    /// </summary>
    partial class tb_FlowchartDefinitionQuery
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
     
     this.lblModuleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbModuleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblFlowchartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFlowchartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFlowchartName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFlowchartName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ModuleID###Int64
//属性测试25ModuleID
this.lblModuleID.AutoSize = true;
this.lblModuleID.Location = new System.Drawing.Point(100,25);
this.lblModuleID.Name = "lblModuleID";
this.lblModuleID.Size = new System.Drawing.Size(41, 12);
this.lblModuleID.TabIndex = 1;
this.lblModuleID.Text = "模块";
//111======25
this.cmbModuleID.Location = new System.Drawing.Point(173,21);
this.cmbModuleID.Name ="cmbModuleID";
this.cmbModuleID.Size = new System.Drawing.Size(100, 21);
this.cmbModuleID.TabIndex = 1;
this.Controls.Add(this.lblModuleID);
this.Controls.Add(this.cmbModuleID);

           //#####50FlowchartNo###String
this.lblFlowchartNo.AutoSize = true;
this.lblFlowchartNo.Location = new System.Drawing.Point(100,50);
this.lblFlowchartNo.Name = "lblFlowchartNo";
this.lblFlowchartNo.Size = new System.Drawing.Size(41, 12);
this.lblFlowchartNo.TabIndex = 2;
this.lblFlowchartNo.Text = "流程图编号";
this.txtFlowchartNo.Location = new System.Drawing.Point(173,46);
this.txtFlowchartNo.Name = "txtFlowchartNo";
this.txtFlowchartNo.Size = new System.Drawing.Size(100, 21);
this.txtFlowchartNo.TabIndex = 2;
this.Controls.Add(this.lblFlowchartNo);
this.Controls.Add(this.txtFlowchartNo);

           //#####20FlowchartName###String
this.lblFlowchartName.AutoSize = true;
this.lblFlowchartName.Location = new System.Drawing.Point(100,75);
this.lblFlowchartName.Name = "lblFlowchartName";
this.lblFlowchartName.Size = new System.Drawing.Size(41, 12);
this.lblFlowchartName.TabIndex = 3;
this.lblFlowchartName.Text = "流程图名称";
this.txtFlowchartName.Location = new System.Drawing.Point(173,71);
this.txtFlowchartName.Name = "txtFlowchartName";
this.txtFlowchartName.Size = new System.Drawing.Size(100, 21);
this.txtFlowchartName.TabIndex = 3;
this.Controls.Add(this.lblFlowchartName);
this.Controls.Add(this.txtFlowchartName);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblModuleID );
this.Controls.Add(this.cmbModuleID );

                this.Controls.Add(this.lblFlowchartNo );
this.Controls.Add(this.txtFlowchartNo );

                this.Controls.Add(this.lblFlowchartName );
this.Controls.Add(this.txtFlowchartName );

                    
            this.Name = "tb_FlowchartDefinitionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbModuleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFlowchartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFlowchartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFlowchartName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFlowchartName;

    
    
   
 





    }
}


