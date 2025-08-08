
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:43
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）
    /// </summary>
    partial class tb_ModuleDefinitionQuery
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
     
     this.lblModuleNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModuleNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblModuleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModuleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVisible = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkVisible = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkVisible.Values.Text ="";

this.lblAvailable = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkAvailable = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkAvailable.Values.Text ="";

this.lblIconFile_Path = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIconFile_Path = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50ModuleNo###String
this.lblModuleNo.AutoSize = true;
this.lblModuleNo.Location = new System.Drawing.Point(100,25);
this.lblModuleNo.Name = "lblModuleNo";
this.lblModuleNo.Size = new System.Drawing.Size(41, 12);
this.lblModuleNo.TabIndex = 1;
this.lblModuleNo.Text = "模块编号";
this.txtModuleNo.Location = new System.Drawing.Point(173,21);
this.txtModuleNo.Name = "txtModuleNo";
this.txtModuleNo.Size = new System.Drawing.Size(100, 21);
this.txtModuleNo.TabIndex = 1;
this.Controls.Add(this.lblModuleNo);
this.Controls.Add(this.txtModuleNo);

           //#####20ModuleName###String
this.lblModuleName.AutoSize = true;
this.lblModuleName.Location = new System.Drawing.Point(100,50);
this.lblModuleName.Name = "lblModuleName";
this.lblModuleName.Size = new System.Drawing.Size(41, 12);
this.lblModuleName.TabIndex = 2;
this.lblModuleName.Text = "模块名称";
this.txtModuleName.Location = new System.Drawing.Point(173,46);
this.txtModuleName.Name = "txtModuleName";
this.txtModuleName.Size = new System.Drawing.Size(100, 21);
this.txtModuleName.TabIndex = 2;
this.Controls.Add(this.lblModuleName);
this.Controls.Add(this.txtModuleName);

           //#####Visible###Boolean
this.lblVisible.AutoSize = true;
this.lblVisible.Location = new System.Drawing.Point(100,75);
this.lblVisible.Name = "lblVisible";
this.lblVisible.Size = new System.Drawing.Size(41, 12);
this.lblVisible.TabIndex = 3;
this.lblVisible.Text = "是否可见";
this.chkVisible.Location = new System.Drawing.Point(173,71);
this.chkVisible.Name = "chkVisible";
this.chkVisible.Size = new System.Drawing.Size(100, 21);
this.chkVisible.TabIndex = 3;
this.Controls.Add(this.lblVisible);
this.Controls.Add(this.chkVisible);

           //#####Available###Boolean
this.lblAvailable.AutoSize = true;
this.lblAvailable.Location = new System.Drawing.Point(100,100);
this.lblAvailable.Name = "lblAvailable";
this.lblAvailable.Size = new System.Drawing.Size(41, 12);
this.lblAvailable.TabIndex = 4;
this.lblAvailable.Text = "是否可用";
this.chkAvailable.Location = new System.Drawing.Point(173,96);
this.chkAvailable.Name = "chkAvailable";
this.chkAvailable.Size = new System.Drawing.Size(100, 21);
this.chkAvailable.TabIndex = 4;
this.Controls.Add(this.lblAvailable);
this.Controls.Add(this.chkAvailable);

           //#####100IconFile_Path###String
this.lblIconFile_Path.AutoSize = true;
this.lblIconFile_Path.Location = new System.Drawing.Point(100,125);
this.lblIconFile_Path.Name = "lblIconFile_Path";
this.lblIconFile_Path.Size = new System.Drawing.Size(41, 12);
this.lblIconFile_Path.TabIndex = 5;
this.lblIconFile_Path.Text = "图标路径";
this.txtIconFile_Path.Location = new System.Drawing.Point(173,121);
this.txtIconFile_Path.Name = "txtIconFile_Path";
this.txtIconFile_Path.Size = new System.Drawing.Size(100, 21);
this.txtIconFile_Path.TabIndex = 5;
this.Controls.Add(this.lblIconFile_Path);
this.Controls.Add(this.txtIconFile_Path);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblModuleNo );
this.Controls.Add(this.txtModuleNo );

                this.Controls.Add(this.lblModuleName );
this.Controls.Add(this.txtModuleName );

                this.Controls.Add(this.lblVisible );
this.Controls.Add(this.chkVisible );

                this.Controls.Add(this.lblAvailable );
this.Controls.Add(this.chkAvailable );

                this.Controls.Add(this.lblIconFile_Path );
this.Controls.Add(this.txtIconFile_Path );

                    
            this.Name = "tb_ModuleDefinitionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModuleNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModuleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVisible;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkVisible;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAvailable;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkAvailable;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIconFile_Path;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIconFile_Path;

    
    
   
 





    }
}


