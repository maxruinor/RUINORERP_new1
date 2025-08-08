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
    partial class tb_ProdCategoriesEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblCategory_name = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_name = new Krypton.Toolkit.KryptonTextBox();

this.lblCategoryCode = new Krypton.Toolkit.KryptonLabel();
this.txtCategoryCode = new Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

Parent_id主外字段不一致。this.lblCategoryLevel = new Krypton.Toolkit.KryptonLabel();
this.txtCategoryLevel = new Krypton.Toolkit.KryptonTextBox();

Parent_id主外字段不一致。this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

Parent_id主外字段不一致。this.lblParent_id = new Krypton.Toolkit.KryptonLabel();
this.txtParent_id = new Krypton.Toolkit.KryptonTextBox();


this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
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
Parent_id主外字段不一致。this.lblCategoryLevel.AutoSize = true;
this.lblCategoryLevel.Location = new System.Drawing.Point(100,100);
this.lblCategoryLevel.Name = "lblCategoryLevel";
this.lblCategoryLevel.Size = new System.Drawing.Size(41, 12);
this.lblCategoryLevel.TabIndex = 4;
this.lblCategoryLevel.Text = "";
this.txtCategoryLevel.Location = new System.Drawing.Point(173,96);
this.txtCategoryLevel.Name = "txtCategoryLevel";
this.txtCategoryLevel.Size = new System.Drawing.Size(100, 21);
this.txtCategoryLevel.TabIndex = 4;
this.Controls.Add(this.lblCategoryLevel);
this.Controls.Add(this.txtCategoryLevel);

           //#####Sort###Int32
//属性测试125Sort
Parent_id主外字段不一致。this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,125);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 5;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,121);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 5;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

           //#####Parent_id###Int64
//属性测试150Parent_id
Parent_id主外字段不一致。this.lblParent_id.AutoSize = true;
this.lblParent_id.Location = new System.Drawing.Point(100,150);
this.lblParent_id.Name = "lblParent_id";
this.lblParent_id.Size = new System.Drawing.Size(41, 12);
this.lblParent_id.TabIndex = 6;
this.lblParent_id.Text = "父类";
this.txtParent_id.Location = new System.Drawing.Point(173,146);
this.txtParent_id.Name = "txtParent_id";
this.txtParent_id.Size = new System.Drawing.Size(100, 21);
this.txtParent_id.TabIndex = 6;
this.Controls.Add(this.lblParent_id);
this.Controls.Add(this.txtParent_id);

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

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 8;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCategory_name );
this.Controls.Add(this.txtCategory_name );

                this.Controls.Add(this.lblCategoryCode );
this.Controls.Add(this.txtCategoryCode );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                Parent_id主外字段不一致。this.Controls.Add(this.lblCategoryLevel );
this.Controls.Add(this.txtCategoryLevel );

                Parent_id主外字段不一致。this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                Parent_id主外字段不一致。this.Controls.Add(this.lblParent_id );
this.Controls.Add(this.txtParent_id );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_ProdCategoriesEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdCategoriesEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCategory_name;
private Krypton.Toolkit.KryptonTextBox txtCategory_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategoryCode;
private Krypton.Toolkit.KryptonTextBox txtCategoryCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              Parent_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCategoryLevel;
private Krypton.Toolkit.KryptonTextBox txtCategoryLevel;

    
        
              Parent_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              Parent_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblParent_id;
private Krypton.Toolkit.KryptonTextBox txtParent_id;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

