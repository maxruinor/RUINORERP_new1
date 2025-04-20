// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:08
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// UI表格设置
    /// </summary>
    partial class tb_UIGridSettingEdit
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
     this.lblUIMenuPID = new Krypton.Toolkit.KryptonLabel();
this.cmbUIMenuPID = new Krypton.Toolkit.KryptonComboBox();

this.lblGridKeyName = new Krypton.Toolkit.KryptonLabel();
this.txtGridKeyName = new Krypton.Toolkit.KryptonTextBox();
this.txtGridKeyName.Multiline = true;

this.lblColsSetting = new Krypton.Toolkit.KryptonLabel();
this.txtColsSetting = new Krypton.Toolkit.KryptonTextBox();
this.txtColsSetting.Multiline = true;

this.lblGridType = new Krypton.Toolkit.KryptonLabel();
this.txtGridType = new Krypton.Toolkit.KryptonTextBox();

this.lblColumnsMode = new Krypton.Toolkit.KryptonLabel();
this.txtColumnsMode = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####UIMenuPID###Int64
//属性测试25UIMenuPID
this.lblUIMenuPID.AutoSize = true;
this.lblUIMenuPID.Location = new System.Drawing.Point(100,25);
this.lblUIMenuPID.Name = "lblUIMenuPID";
this.lblUIMenuPID.Size = new System.Drawing.Size(41, 12);
this.lblUIMenuPID.TabIndex = 1;
this.lblUIMenuPID.Text = "菜单设置";
//111======25
this.cmbUIMenuPID.Location = new System.Drawing.Point(173,21);
this.cmbUIMenuPID.Name ="cmbUIMenuPID";
this.cmbUIMenuPID.Size = new System.Drawing.Size(100, 21);
this.cmbUIMenuPID.TabIndex = 1;
this.Controls.Add(this.lblUIMenuPID);
this.Controls.Add(this.cmbUIMenuPID);

           //#####255GridKeyName###String
this.lblGridKeyName.AutoSize = true;
this.lblGridKeyName.Location = new System.Drawing.Point(100,50);
this.lblGridKeyName.Name = "lblGridKeyName";
this.lblGridKeyName.Size = new System.Drawing.Size(41, 12);
this.lblGridKeyName.TabIndex = 2;
this.lblGridKeyName.Text = "表格名称";
this.txtGridKeyName.Location = new System.Drawing.Point(173,46);
this.txtGridKeyName.Name = "txtGridKeyName";
this.txtGridKeyName.Size = new System.Drawing.Size(100, 21);
this.txtGridKeyName.TabIndex = 2;
this.Controls.Add(this.lblGridKeyName);
this.Controls.Add(this.txtGridKeyName);

           //#####2147483647ColsSetting###String
this.lblColsSetting.AutoSize = true;
this.lblColsSetting.Location = new System.Drawing.Point(100,75);
this.lblColsSetting.Name = "lblColsSetting";
this.lblColsSetting.Size = new System.Drawing.Size(41, 12);
this.lblColsSetting.TabIndex = 3;
this.lblColsSetting.Text = "列设置信息";
this.txtColsSetting.Location = new System.Drawing.Point(173,71);
this.txtColsSetting.Name = "txtColsSetting";
this.txtColsSetting.Size = new System.Drawing.Size(100, 21);
this.txtColsSetting.TabIndex = 3;
this.txtColsSetting.Multiline = true;
this.Controls.Add(this.lblColsSetting);
this.Controls.Add(this.txtColsSetting);

           //#####50GridType###String
this.lblGridType.AutoSize = true;
this.lblGridType.Location = new System.Drawing.Point(100,100);
this.lblGridType.Name = "lblGridType";
this.lblGridType.Size = new System.Drawing.Size(41, 12);
this.lblGridType.TabIndex = 4;
this.lblGridType.Text = "表格类型";
this.txtGridType.Location = new System.Drawing.Point(173,96);
this.txtGridType.Name = "txtGridType";
this.txtGridType.Size = new System.Drawing.Size(100, 21);
this.txtGridType.TabIndex = 4;
this.Controls.Add(this.lblGridType);
this.Controls.Add(this.txtGridType);

           //#####ColumnsMode###Int32
//属性测试125ColumnsMode
this.lblColumnsMode.AutoSize = true;
this.lblColumnsMode.Location = new System.Drawing.Point(100,125);
this.lblColumnsMode.Name = "lblColumnsMode";
this.lblColumnsMode.Size = new System.Drawing.Size(41, 12);
this.lblColumnsMode.TabIndex = 5;
this.lblColumnsMode.Text = "列宽显示模式";
this.txtColumnsMode.Location = new System.Drawing.Point(173,121);
this.txtColumnsMode.Name = "txtColumnsMode";
this.txtColumnsMode.Size = new System.Drawing.Size(100, 21);
this.txtColumnsMode.TabIndex = 5;
this.Controls.Add(this.lblColumnsMode);
this.Controls.Add(this.txtColumnsMode);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "创建时间";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试175Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,175);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 7;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,171);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 7;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,200);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 8;
this.lblModified_at.Text = "修改时间";
//111======200
this.dtpModified_at.Location = new System.Drawing.Point(173,196);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 8;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试225Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,225);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 9;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,221);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 9;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 9;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUIMenuPID );
this.Controls.Add(this.cmbUIMenuPID );

                this.Controls.Add(this.lblGridKeyName );
this.Controls.Add(this.txtGridKeyName );

                this.Controls.Add(this.lblColsSetting );
this.Controls.Add(this.txtColsSetting );

                this.Controls.Add(this.lblGridType );
this.Controls.Add(this.txtGridType );

                this.Controls.Add(this.lblColumnsMode );
this.Controls.Add(this.txtColumnsMode );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_UIGridSettingEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UIGridSettingEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblUIMenuPID;
private Krypton.Toolkit.KryptonComboBox cmbUIMenuPID;

    
        
              private Krypton.Toolkit.KryptonLabel lblGridKeyName;
private Krypton.Toolkit.KryptonTextBox txtGridKeyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblColsSetting;
private Krypton.Toolkit.KryptonTextBox txtColsSetting;

    
        
              private Krypton.Toolkit.KryptonLabel lblGridType;
private Krypton.Toolkit.KryptonTextBox txtGridType;

    
        
              private Krypton.Toolkit.KryptonLabel lblColumnsMode;
private Krypton.Toolkit.KryptonTextBox txtColumnsMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

