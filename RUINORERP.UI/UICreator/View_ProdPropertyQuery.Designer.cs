
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 
    /// </summary>
    partial class View_ProdPropertyQuery
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
     
     


this.lblPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdBaseID###Int64

           //#####ProdDetailID###Int64

           //#####Property_ID###Int64

           //#####20PropertyName###String
this.lblPropertyName.AutoSize = true;
this.lblPropertyName.Location = new System.Drawing.Point(100,100);
this.lblPropertyName.Name = "lblPropertyName";
this.lblPropertyName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyName.TabIndex = 4;
this.lblPropertyName.Text = "";
this.txtPropertyName.Location = new System.Drawing.Point(173,96);
this.txtPropertyName.Name = "txtPropertyName";
this.txtPropertyName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyName.TabIndex = 4;
this.Controls.Add(this.lblPropertyName);
this.Controls.Add(this.txtPropertyName);

           //#####PropertyValueID###Int64

           //#####20PropertyValueName###String
this.lblPropertyValueName.AutoSize = true;
this.lblPropertyValueName.Location = new System.Drawing.Point(100,150);
this.lblPropertyValueName.Name = "lblPropertyValueName";
this.lblPropertyValueName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueName.TabIndex = 6;
this.lblPropertyValueName.Text = "";
this.txtPropertyValueName.Location = new System.Drawing.Point(173,146);
this.txtPropertyValueName.Name = "txtPropertyValueName";
this.txtPropertyValueName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueName.TabIndex = 6;
this.Controls.Add(this.lblPropertyValueName);
this.Controls.Add(this.txtPropertyValueName);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                
                this.Controls.Add(this.lblPropertyName );
this.Controls.Add(this.txtPropertyName );

                
                this.Controls.Add(this.lblPropertyValueName );
this.Controls.Add(this.txtPropertyValueName );

                    
            this.Name = "View_ProdPropertyQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyValueName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyValueName;

    
    
   
 





    }
}


