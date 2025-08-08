
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:48
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    partial class tb_PaymentMethodQuery
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
     
     this.lblPaytype_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaytype_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCash = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkCash = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkCash.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Paytype_Name###String
this.lblPaytype_Name.AutoSize = true;
this.lblPaytype_Name.Location = new System.Drawing.Point(100,25);
this.lblPaytype_Name.Name = "lblPaytype_Name";
this.lblPaytype_Name.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_Name.TabIndex = 1;
this.lblPaytype_Name.Text = "付款方式";
this.txtPaytype_Name.Location = new System.Drawing.Point(173,21);
this.txtPaytype_Name.Name = "txtPaytype_Name";
this.txtPaytype_Name.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_Name.TabIndex = 1;
this.Controls.Add(this.lblPaytype_Name);
this.Controls.Add(this.txtPaytype_Name);

           //#####50Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,50);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 2;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,46);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 2;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

           //#####Sort###Int32

           //#####Cash###Boolean
this.lblCash.AutoSize = true;
this.lblCash.Location = new System.Drawing.Point(100,100);
this.lblCash.Name = "lblCash";
this.lblCash.Size = new System.Drawing.Size(41, 12);
this.lblCash.TabIndex = 4;
this.lblCash.Text = "即时收付款";
this.chkCash.Location = new System.Drawing.Point(173,96);
this.chkCash.Name = "chkCash";
this.chkCash.Size = new System.Drawing.Size(100, 21);
this.chkCash.TabIndex = 4;
this.Controls.Add(this.lblCash);
this.Controls.Add(this.chkCash);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaytype_Name );
this.Controls.Add(this.txtPaytype_Name );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                
                this.Controls.Add(this.lblCash );
this.Controls.Add(this.chkCash );

                    
            this.Name = "tb_PaymentMethodQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaytype_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCash;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkCash;

    
    
   
 





    }
}


