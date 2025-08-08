
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？
    /// </summary>
    partial class tb_OutInStockTypeQuery
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

this.lblOutIn = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOutIn = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOutIn.Values.Text ="";

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

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

           //#####OutIn###Boolean
this.lblOutIn.AutoSize = true;
this.lblOutIn.Location = new System.Drawing.Point(100,75);
this.lblOutIn.Name = "lblOutIn";
this.lblOutIn.Size = new System.Drawing.Size(41, 12);
this.lblOutIn.TabIndex = 3;
this.lblOutIn.Text = "出入类型";
this.chkOutIn.Location = new System.Drawing.Point(173,71);
this.chkOutIn.Name = "chkOutIn";
this.chkOutIn.Size = new System.Drawing.Size(100, 21);
this.chkOutIn.TabIndex = 3;
this.Controls.Add(this.lblOutIn);
this.Controls.Add(this.chkOutIn);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,100);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 4;
this.lblIs_enabled.Text = "是否可用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,96);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 4;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblTypeName );
this.Controls.Add(this.txtTypeName );

                this.Controls.Add(this.lblTypeDesc );
this.Controls.Add(this.txtTypeDesc );

                this.Controls.Add(this.lblOutIn );
this.Controls.Add(this.chkOutIn );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                    
            this.Name = "tb_OutInStockTypeQuery";
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutIn;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOutIn;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
    
   
 





    }
}


