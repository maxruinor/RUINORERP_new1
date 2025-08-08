// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:38
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库位表
    /// </summary>
    partial class tb_LocationEdit
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
     this.lblLocationType_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocationType_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblLocationCode = new Krypton.Toolkit.KryptonLabel();
this.txtLocationCode = new Krypton.Toolkit.KryptonTextBox();

this.lblTel = new Krypton.Toolkit.KryptonLabel();
this.txtTel = new Krypton.Toolkit.KryptonTextBox();

this.lblName = new Krypton.Toolkit.KryptonLabel();
this.txtName = new Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new Krypton.Toolkit.KryptonLabel();
this.txtDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####LocationType_ID###Int64
//属性测试25LocationType_ID
//属性测试25LocationType_ID
this.lblLocationType_ID.AutoSize = true;
this.lblLocationType_ID.Location = new System.Drawing.Point(100,25);
this.lblLocationType_ID.Name = "lblLocationType_ID";
this.lblLocationType_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocationType_ID.TabIndex = 1;
this.lblLocationType_ID.Text = "库位类型";
//111======25
this.cmbLocationType_ID.Location = new System.Drawing.Point(173,21);
this.cmbLocationType_ID.Name ="cmbLocationType_ID";
this.cmbLocationType_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocationType_ID.TabIndex = 1;
this.Controls.Add(this.lblLocationType_ID);
this.Controls.Add(this.cmbLocationType_ID);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "联系人";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,75);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 3;
this.lblIs_enabled.Text = "是否可用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,71);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 3;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####50LocationCode###String
this.lblLocationCode.AutoSize = true;
this.lblLocationCode.Location = new System.Drawing.Point(100,100);
this.lblLocationCode.Name = "lblLocationCode";
this.lblLocationCode.Size = new System.Drawing.Size(41, 12);
this.lblLocationCode.TabIndex = 4;
this.lblLocationCode.Text = "仓库代码";
this.txtLocationCode.Location = new System.Drawing.Point(173,96);
this.txtLocationCode.Name = "txtLocationCode";
this.txtLocationCode.Size = new System.Drawing.Size(100, 21);
this.txtLocationCode.TabIndex = 4;
this.Controls.Add(this.lblLocationCode);
this.Controls.Add(this.txtLocationCode);

           //#####20Tel###String
this.lblTel.AutoSize = true;
this.lblTel.Location = new System.Drawing.Point(100,125);
this.lblTel.Name = "lblTel";
this.lblTel.Size = new System.Drawing.Size(41, 12);
this.lblTel.TabIndex = 5;
this.lblTel.Text = "电话";
this.txtTel.Location = new System.Drawing.Point(173,121);
this.txtTel.Name = "txtTel";
this.txtTel.Size = new System.Drawing.Size(100, 21);
this.txtTel.TabIndex = 5;
this.Controls.Add(this.lblTel);
this.Controls.Add(this.txtTel);

           //#####50Name###String
this.lblName.AutoSize = true;
this.lblName.Location = new System.Drawing.Point(100,150);
this.lblName.Name = "lblName";
this.lblName.Size = new System.Drawing.Size(41, 12);
this.lblName.TabIndex = 6;
this.lblName.Text = "仓库名称";
this.txtName.Location = new System.Drawing.Point(173,146);
this.txtName.Name = "txtName";
this.txtName.Size = new System.Drawing.Size(100, 21);
this.txtName.TabIndex = 6;
this.Controls.Add(this.lblName);
this.Controls.Add(this.txtName);

           //#####100Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,175);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 7;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,171);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 7;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

           //#####Sort###Int32
//属性测试200Sort
//属性测试200Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,200);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 8;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,196);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 8;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

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
                this.Controls.Add(this.lblLocationType_ID );
this.Controls.Add(this.cmbLocationType_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblLocationCode );
this.Controls.Add(this.txtLocationCode );

                this.Controls.Add(this.lblTel );
this.Controls.Add(this.txtTel );

                this.Controls.Add(this.lblName );
this.Controls.Add(this.txtName );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                            // 
            // "tb_LocationEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_LocationEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblLocationType_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocationType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocationCode;
private Krypton.Toolkit.KryptonTextBox txtLocationCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblTel;
private Krypton.Toolkit.KryptonTextBox txtTel;

    
        
              private Krypton.Toolkit.KryptonLabel lblName;
private Krypton.Toolkit.KryptonTextBox txtName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDesc;
private Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

