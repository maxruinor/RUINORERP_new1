
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 项目及成员关系表
    /// </summary>
    partial class tb_ProjectGroupEmployeesQuery
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
     
     this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblAssigned = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkAssigned = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkAssigned.Values.Text ="";

this.lblDefaultGroup = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkDefaultGroup = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkDefaultGroup.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProjectGroup_ID###Int64
//属性测试25ProjectGroup_ID
//属性测试25ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,25);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 1;
this.lblProjectGroup_ID.Text = "项目组";
//111======25
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,21);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 1;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "员工";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Assigned###Boolean
this.lblAssigned.AutoSize = true;
this.lblAssigned.Location = new System.Drawing.Point(100,75);
this.lblAssigned.Name = "lblAssigned";
this.lblAssigned.Size = new System.Drawing.Size(41, 12);
this.lblAssigned.TabIndex = 3;
this.lblAssigned.Text = "已分配";
this.chkAssigned.Location = new System.Drawing.Point(173,71);
this.chkAssigned.Name = "chkAssigned";
this.chkAssigned.Size = new System.Drawing.Size(100, 21);
this.chkAssigned.TabIndex = 3;
this.Controls.Add(this.lblAssigned);
this.Controls.Add(this.chkAssigned);

           //#####DefaultGroup###Boolean
this.lblDefaultGroup.AutoSize = true;
this.lblDefaultGroup.Location = new System.Drawing.Point(100,100);
this.lblDefaultGroup.Name = "lblDefaultGroup";
this.lblDefaultGroup.Size = new System.Drawing.Size(41, 12);
this.lblDefaultGroup.TabIndex = 4;
this.lblDefaultGroup.Text = "默认组";
this.chkDefaultGroup.Location = new System.Drawing.Point(173,96);
this.chkDefaultGroup.Name = "chkDefaultGroup";
this.chkDefaultGroup.Size = new System.Drawing.Size(100, 21);
this.chkDefaultGroup.TabIndex = 4;
this.Controls.Add(this.lblDefaultGroup);
this.Controls.Add(this.chkDefaultGroup);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblAssigned );
this.Controls.Add(this.chkAssigned );

                this.Controls.Add(this.lblDefaultGroup );
this.Controls.Add(this.chkDefaultGroup );

                    
            this.Name = "tb_ProjectGroupEmployeesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAssigned;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkAssigned;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultGroup;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkDefaultGroup;

    
    
   
 





    }
}


