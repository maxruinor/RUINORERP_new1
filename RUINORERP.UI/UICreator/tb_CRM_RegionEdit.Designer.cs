// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:19
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
    partial class tb_CRM_RegionEdit
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
     this.lblRegion_Name = new Krypton.Toolkit.KryptonLabel();
this.txtRegion_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblRegion_code = new Krypton.Toolkit.KryptonLabel();
this.txtRegion_code = new Krypton.Toolkit.KryptonTextBox();

this.lblParent_region_id = new Krypton.Toolkit.KryptonLabel();
this.txtParent_region_id = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

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
this.lblParent_region_id.AutoSize = true;
this.lblParent_region_id.Location = new System.Drawing.Point(100,75);
this.lblParent_region_id.Name = "lblParent_region_id";
this.lblParent_region_id.Size = new System.Drawing.Size(41, 12);
this.lblParent_region_id.TabIndex = 3;
this.lblParent_region_id.Text = " 父地区";
this.txtParent_region_id.Location = new System.Drawing.Point(173,71);
this.txtParent_region_id.Name = "txtParent_region_id";
this.txtParent_region_id.Size = new System.Drawing.Size(100, 21);
this.txtParent_region_id.TabIndex = 3;
this.Controls.Add(this.lblParent_region_id);
this.Controls.Add(this.txtParent_region_id);

           //#####Sort###Int32
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,100);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 4;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,96);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 4;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

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
           // this.kryptonPanel1.TabIndex = 6;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRegion_Name );
this.Controls.Add(this.txtRegion_Name );

                this.Controls.Add(this.lblRegion_code );
this.Controls.Add(this.txtRegion_code );

                this.Controls.Add(this.lblParent_region_id );
this.Controls.Add(this.txtParent_region_id );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_CRM_RegionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_RegionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRegion_Name;
private Krypton.Toolkit.KryptonTextBox txtRegion_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblRegion_code;
private Krypton.Toolkit.KryptonTextBox txtRegion_code;

    
        
              private Krypton.Toolkit.KryptonLabel lblParent_region_id;
private Krypton.Toolkit.KryptonTextBox txtParent_region_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

