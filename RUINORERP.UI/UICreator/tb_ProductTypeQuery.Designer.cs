
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 17:35:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    partial class tb_ProductTypeQuery
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
     
     this.lblTypeName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTypeName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTypeDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTypeDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForSale = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkForSale = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkForSale.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50TypeName###String
this.lblTypeName.AutoSize = true;
this.lblTypeName.Location = new System.Drawing.Point(100,25);
this.lblTypeName.Name = "lblTypeName";
this.lblTypeName.Size = new System.Drawing.Size(41, 12);
this.lblTypeName.TabIndex = 1;
this.lblTypeName.Text = "类型名称";
this.txtTypeName.Location = new System.Drawing.Point(173,21);
this.txtTypeName.Name = "txtTypeName";
this.txtTypeName.Size = new System.Drawing.Size(100, 21);
this.txtTypeName.TabIndex = 1;
this.Controls.Add(this.lblTypeName);
this.Controls.Add(this.txtTypeName);

           //#####100TypeDesc###String
this.lblTypeDesc.AutoSize = true;
this.lblTypeDesc.Location = new System.Drawing.Point(100,50);
this.lblTypeDesc.Name = "lblTypeDesc";
this.lblTypeDesc.Size = new System.Drawing.Size(41, 12);
this.lblTypeDesc.TabIndex = 2;
this.lblTypeDesc.Text = "描述";
this.txtTypeDesc.Location = new System.Drawing.Point(173,46);
this.txtTypeDesc.Name = "txtTypeDesc";
this.txtTypeDesc.Size = new System.Drawing.Size(100, 21);
this.txtTypeDesc.TabIndex = 2;
this.Controls.Add(this.lblTypeDesc);
this.Controls.Add(this.txtTypeDesc);

           //#####ForSale###Boolean
this.lblForSale.AutoSize = true;
this.lblForSale.Location = new System.Drawing.Point(100,75);
this.lblForSale.Name = "lblForSale";
this.lblForSale.Size = new System.Drawing.Size(41, 12);
this.lblForSale.TabIndex = 3;
this.lblForSale.Text = "待销类型";
this.chkForSale.Location = new System.Drawing.Point(173,71);
this.chkForSale.Name = "chkForSale";
this.chkForSale.Size = new System.Drawing.Size(100, 21);
this.chkForSale.TabIndex = 3;
this.Controls.Add(this.lblForSale);
this.Controls.Add(this.chkForSale);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblTypeName );
this.Controls.Add(this.txtTypeName );

                this.Controls.Add(this.lblTypeDesc );
this.Controls.Add(this.txtTypeDesc );

                this.Controls.Add(this.lblForSale );
this.Controls.Add(this.chkForSale );

                    
            this.Name = "tb_ProductTypeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTypeName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTypeName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTypeDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTypeDesc;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForSale;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkForSale;

    
    
   
 





    }
}


