
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品属性类型EVA
    /// </summary>
    partial class tb_ProdPropertyTypeQuery
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
     
     this.lblPropertyTypeName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyTypeName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPropertyTypeDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyTypeDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50PropertyTypeName###String
this.lblPropertyTypeName.AutoSize = true;
this.lblPropertyTypeName.Location = new System.Drawing.Point(100,25);
this.lblPropertyTypeName.Name = "lblPropertyTypeName";
this.lblPropertyTypeName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyTypeName.TabIndex = 1;
this.lblPropertyTypeName.Text = "属性类型名称";
this.txtPropertyTypeName.Location = new System.Drawing.Point(173,21);
this.txtPropertyTypeName.Name = "txtPropertyTypeName";
this.txtPropertyTypeName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyTypeName.TabIndex = 1;
this.Controls.Add(this.lblPropertyTypeName);
this.Controls.Add(this.txtPropertyTypeName);

           //#####100PropertyTypeDesc###String
this.lblPropertyTypeDesc.AutoSize = true;
this.lblPropertyTypeDesc.Location = new System.Drawing.Point(100,50);
this.lblPropertyTypeDesc.Name = "lblPropertyTypeDesc";
this.lblPropertyTypeDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyTypeDesc.TabIndex = 2;
this.lblPropertyTypeDesc.Text = "属性类型描述";
this.txtPropertyTypeDesc.Location = new System.Drawing.Point(173,46);
this.txtPropertyTypeDesc.Name = "txtPropertyTypeDesc";
this.txtPropertyTypeDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyTypeDesc.TabIndex = 2;
this.Controls.Add(this.lblPropertyTypeDesc);
this.Controls.Add(this.txtPropertyTypeDesc);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPropertyTypeName );
this.Controls.Add(this.txtPropertyTypeName );

                this.Controls.Add(this.lblPropertyTypeDesc );
this.Controls.Add(this.txtPropertyTypeDesc );

                    
            this.Name = "tb_ProdPropertyTypeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyTypeName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyTypeName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyTypeDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyTypeDesc;

    
    
   
 





    }
}


