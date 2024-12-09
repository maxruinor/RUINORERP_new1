
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售分区表-大中华区
    /// </summary>
    partial class tb_CRM_RegionQuery
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
     
     this.lblRegion_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRegion_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRegion_code = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRegion_code = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

Parent_region_id主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Region_Name###String
this.lblRegion_Name.AutoSize = true;
this.lblRegion_Name.Location = new System.Drawing.Point(100,25);
this.lblRegion_Name.Name = "lblRegion_Name";
this.lblRegion_Name.Size = new System.Drawing.Size(41, 12);
this.lblRegion_Name.TabIndex = 1;
this.lblRegion_Name.Text = "地区名称";
this.txtRegion_Name.Location = new System.Drawing.Point(173,21);
this.txtRegion_Name.Name = "txtRegion_Name";
this.txtRegion_Name.Size = new System.Drawing.Size(100, 21);
this.txtRegion_Name.TabIndex = 1;
this.Controls.Add(this.lblRegion_Name);
this.Controls.Add(this.txtRegion_Name);

           //#####20Region_code###String
this.lblRegion_code.AutoSize = true;
this.lblRegion_code.Location = new System.Drawing.Point(100,50);
this.lblRegion_code.Name = "lblRegion_code";
this.lblRegion_code.Size = new System.Drawing.Size(41, 12);
this.lblRegion_code.TabIndex = 2;
this.lblRegion_code.Text = "地区代码";
this.txtRegion_code.Location = new System.Drawing.Point(173,46);
this.txtRegion_code.Name = "txtRegion_code";
this.txtRegion_code.Size = new System.Drawing.Size(100, 21);
this.txtRegion_code.TabIndex = 2;
this.Controls.Add(this.lblRegion_code);
this.Controls.Add(this.txtRegion_code);

           //#####Parent_region_id###Int64
//属性测试75Parent_region_id
Parent_region_id主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRegion_Name );
this.Controls.Add(this.txtRegion_Name );

                this.Controls.Add(this.lblRegion_code );
this.Controls.Add(this.txtRegion_code );

                Parent_region_id主外字段不一致。
                    
            this.Name = "tb_CRM_RegionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegion_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRegion_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegion_code;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRegion_code;

    
        
              Parent_region_id主外字段不一致。
    
    
   
 





    }
}


