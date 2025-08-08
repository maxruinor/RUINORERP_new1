// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 基本单位
    /// </summary>
    partial class tb_UnitEdit
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
     this.lblUnitName = new Krypton.Toolkit.KryptonLabel();
this.txtUnitName = new Krypton.Toolkit.KryptonTextBox();
this.txtUnitName.Multiline = true;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblis_measurement_unit = new Krypton.Toolkit.KryptonLabel();
this.chkis_measurement_unit = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_measurement_unit.Values.Text ="";

    
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
     
            //#####255UnitName###String
this.lblUnitName.AutoSize = true;
this.lblUnitName.Location = new System.Drawing.Point(100,25);
this.lblUnitName.Name = "lblUnitName";
this.lblUnitName.Size = new System.Drawing.Size(41, 12);
this.lblUnitName.TabIndex = 1;
this.lblUnitName.Text = "单位名称";
this.txtUnitName.Location = new System.Drawing.Point(173,21);
this.txtUnitName.Name = "txtUnitName";
this.txtUnitName.Size = new System.Drawing.Size(100, 21);
this.txtUnitName.TabIndex = 1;
this.Controls.Add(this.lblUnitName);
this.Controls.Add(this.txtUnitName);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,50);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 2;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,46);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 2;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####is_measurement_unit###Boolean
this.lblis_measurement_unit.AutoSize = true;
this.lblis_measurement_unit.Location = new System.Drawing.Point(100,75);
this.lblis_measurement_unit.Name = "lblis_measurement_unit";
this.lblis_measurement_unit.Size = new System.Drawing.Size(41, 12);
this.lblis_measurement_unit.TabIndex = 3;
this.lblis_measurement_unit.Text = "是否可换算";
this.chkis_measurement_unit.Location = new System.Drawing.Point(173,71);
this.chkis_measurement_unit.Name = "chkis_measurement_unit";
this.chkis_measurement_unit.Size = new System.Drawing.Size(100, 21);
this.chkis_measurement_unit.TabIndex = 3;
this.Controls.Add(this.lblis_measurement_unit);
this.Controls.Add(this.chkis_measurement_unit);

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
           // this.kryptonPanel1.TabIndex = 3;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUnitName );
this.Controls.Add(this.txtUnitName );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblis_measurement_unit );
this.Controls.Add(this.chkis_measurement_unit );

                            // 
            // "tb_UnitEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UnitEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblUnitName;
private Krypton.Toolkit.KryptonTextBox txtUnitName;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_measurement_unit;
private Krypton.Toolkit.KryptonCheckBox chkis_measurement_unit;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

