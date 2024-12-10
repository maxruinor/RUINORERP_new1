
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:15
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
Parent_region_id主外字段不一致。
this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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
           //#####Sort###Int32
//属性测试100Sort
Parent_region_id主外字段不一致。
           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,125);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 5;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,121);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 5;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,150);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 6;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,146);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 6;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRegion_Name );
this.Controls.Add(this.txtRegion_Name );

                this.Controls.Add(this.lblRegion_code );
this.Controls.Add(this.txtRegion_code );

                Parent_region_id主外字段不一致。
                Parent_region_id主外字段不一致。
                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
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
    
        
              Parent_region_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


