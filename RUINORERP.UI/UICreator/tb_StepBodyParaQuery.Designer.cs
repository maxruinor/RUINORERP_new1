
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:18
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 步骤变量
    /// </summary>
    partial class tb_StepBodyParaQuery
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
     
     this.lblKey = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtKey = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDisplayName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDisplayName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtValue = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblStepBodyParaType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStepBodyParaType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Key###String
this.lblKey.AutoSize = true;
this.lblKey.Location = new System.Drawing.Point(100,25);
this.lblKey.Name = "lblKey";
this.lblKey.Size = new System.Drawing.Size(41, 12);
this.lblKey.TabIndex = 1;
this.lblKey.Text = "参数key";
this.txtKey.Location = new System.Drawing.Point(173,21);
this.txtKey.Name = "txtKey";
this.txtKey.Size = new System.Drawing.Size(100, 21);
this.txtKey.TabIndex = 1;
this.Controls.Add(this.lblKey);
this.Controls.Add(this.txtKey);

           //#####50Name###String
this.lblName.AutoSize = true;
this.lblName.Location = new System.Drawing.Point(100,50);
this.lblName.Name = "lblName";
this.lblName.Size = new System.Drawing.Size(41, 12);
this.lblName.TabIndex = 2;
this.lblName.Text = "参数名";
this.txtName.Location = new System.Drawing.Point(173,46);
this.txtName.Name = "txtName";
this.txtName.Size = new System.Drawing.Size(100, 21);
this.txtName.TabIndex = 2;
this.Controls.Add(this.lblName);
this.Controls.Add(this.txtName);

           //#####50DisplayName###String
this.lblDisplayName.AutoSize = true;
this.lblDisplayName.Location = new System.Drawing.Point(100,75);
this.lblDisplayName.Name = "lblDisplayName";
this.lblDisplayName.Size = new System.Drawing.Size(41, 12);
this.lblDisplayName.TabIndex = 3;
this.lblDisplayName.Text = "显示名称";
this.txtDisplayName.Location = new System.Drawing.Point(173,71);
this.txtDisplayName.Name = "txtDisplayName";
this.txtDisplayName.Size = new System.Drawing.Size(100, 21);
this.txtDisplayName.TabIndex = 3;
this.Controls.Add(this.lblDisplayName);
this.Controls.Add(this.txtDisplayName);

           //#####50Value###String
this.lblValue.AutoSize = true;
this.lblValue.Location = new System.Drawing.Point(100,100);
this.lblValue.Name = "lblValue";
this.lblValue.Size = new System.Drawing.Size(41, 12);
this.lblValue.TabIndex = 4;
this.lblValue.Text = "参数值";
this.txtValue.Location = new System.Drawing.Point(173,96);
this.txtValue.Name = "txtValue";
this.txtValue.Size = new System.Drawing.Size(100, 21);
this.txtValue.TabIndex = 4;
this.Controls.Add(this.lblValue);
this.Controls.Add(this.txtValue);

           //#####50StepBodyParaType###String
this.lblStepBodyParaType.AutoSize = true;
this.lblStepBodyParaType.Location = new System.Drawing.Point(100,125);
this.lblStepBodyParaType.Name = "lblStepBodyParaType";
this.lblStepBodyParaType.Size = new System.Drawing.Size(41, 12);
this.lblStepBodyParaType.TabIndex = 5;
this.lblStepBodyParaType.Text = "参数类型";
this.txtStepBodyParaType.Location = new System.Drawing.Point(173,121);
this.txtStepBodyParaType.Name = "txtStepBodyParaType";
this.txtStepBodyParaType.Size = new System.Drawing.Size(100, 21);
this.txtStepBodyParaType.TabIndex = 5;
this.Controls.Add(this.lblStepBodyParaType);
this.Controls.Add(this.txtStepBodyParaType);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblKey );
this.Controls.Add(this.txtKey );

                this.Controls.Add(this.lblName );
this.Controls.Add(this.txtName );

                this.Controls.Add(this.lblDisplayName );
this.Controls.Add(this.txtDisplayName );

                this.Controls.Add(this.lblValue );
this.Controls.Add(this.txtValue );

                this.Controls.Add(this.lblStepBodyParaType );
this.Controls.Add(this.txtStepBodyParaType );

                    
            this.Name = "tb_StepBodyParaQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblKey;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtKey;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDisplayName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDisplayName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblValue;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtValue;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStepBodyParaType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStepBodyParaType;

    
    
   
 





    }
}


