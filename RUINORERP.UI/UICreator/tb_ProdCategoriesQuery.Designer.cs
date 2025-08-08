
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:54
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    partial class tb_ProdCategoriesQuery
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
     
     this.lblCategory_name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCategory_name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCategoryCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCategoryCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

Parent_id主外字段不一致。
Parent_id主外字段不一致。
Parent_id主外字段不一致。

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Category_name###String
this.lblCategory_name.AutoSize = true;
this.lblCategory_name.Location = new System.Drawing.Point(100,25);
this.lblCategory_name.Name = "lblCategory_name";
this.lblCategory_name.Size = new System.Drawing.Size(41, 12);
this.lblCategory_name.TabIndex = 1;
this.lblCategory_name.Text = "类别名称";
this.txtCategory_name.Location = new System.Drawing.Point(173,21);
this.txtCategory_name.Name = "txtCategory_name";
this.txtCategory_name.Size = new System.Drawing.Size(100, 21);
this.txtCategory_name.TabIndex = 1;
this.Controls.Add(this.lblCategory_name);
this.Controls.Add(this.txtCategory_name);

           //#####20CategoryCode###String
this.lblCategoryCode.AutoSize = true;
this.lblCategoryCode.Location = new System.Drawing.Point(100,50);
this.lblCategoryCode.Name = "lblCategoryCode";
this.lblCategoryCode.Size = new System.Drawing.Size(41, 12);
this.lblCategoryCode.TabIndex = 2;
this.lblCategoryCode.Text = "类别代码";
this.txtCategoryCode.Location = new System.Drawing.Point(173,46);
this.txtCategoryCode.Name = "txtCategoryCode";
this.txtCategoryCode.Size = new System.Drawing.Size(100, 21);
this.txtCategoryCode.TabIndex = 2;
this.Controls.Add(this.lblCategoryCode);
this.Controls.Add(this.txtCategoryCode);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,75);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 3;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,71);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 3;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####CategoryLevel###Int32
//属性测试100CategoryLevel
Parent_id主外字段不一致。
           //#####Sort###Int32
//属性测试125Sort
Parent_id主外字段不一致。
           //#####Parent_id###Int64
//属性测试150Parent_id
Parent_id主外字段不一致。
           //#####2147483647Images###Binary

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCategory_name );
this.Controls.Add(this.txtCategory_name );

                this.Controls.Add(this.lblCategoryCode );
this.Controls.Add(this.txtCategoryCode );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                Parent_id主外字段不一致。
                Parent_id主外字段不一致。
                Parent_id主外字段不一致。
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_ProdCategoriesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCategory_name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCategory_name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCategoryCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCategoryCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              Parent_id主外字段不一致。
    
        
              Parent_id主外字段不一致。
    
        
              Parent_id主外字段不一致。
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


