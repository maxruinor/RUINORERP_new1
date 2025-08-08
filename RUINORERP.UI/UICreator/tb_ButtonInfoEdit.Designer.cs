// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:14
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 字段信息表
    /// </summary>
    partial class tb_ButtonInfoEdit
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
     this.lblMenuID = new Krypton.Toolkit.KryptonLabel();
this.cmbMenuID = new Krypton.Toolkit.KryptonComboBox();

this.lblBtnName = new Krypton.Toolkit.KryptonLabel();
this.txtBtnName = new Krypton.Toolkit.KryptonTextBox();
this.txtBtnName.Multiline = true;

this.lblBtnText = new Krypton.Toolkit.KryptonLabel();
this.txtBtnText = new Krypton.Toolkit.KryptonTextBox();
this.txtBtnText.Multiline = true;

this.lblHotKey = new Krypton.Toolkit.KryptonLabel();
this.txtHotKey = new Krypton.Toolkit.KryptonTextBox();

this.lblButtonType = new Krypton.Toolkit.KryptonLabel();
this.txtButtonType = new Krypton.Toolkit.KryptonTextBox();

this.lblFormName = new Krypton.Toolkit.KryptonLabel();
this.txtFormName = new Krypton.Toolkit.KryptonTextBox();
this.txtFormName.Multiline = true;

this.lblClassPath = new Krypton.Toolkit.KryptonLabel();
this.txtClassPath = new Krypton.Toolkit.KryptonTextBox();
this.txtClassPath.Multiline = true;

this.lblIsForm = new Krypton.Toolkit.KryptonLabel();
this.chkIsForm = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsForm.Values.Text ="";

this.lblIsEnabled = new Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####MenuID###Int64
//属性测试25MenuID
this.lblMenuID.AutoSize = true;
this.lblMenuID.Location = new System.Drawing.Point(100,25);
this.lblMenuID.Name = "lblMenuID";
this.lblMenuID.Size = new System.Drawing.Size(41, 12);
this.lblMenuID.TabIndex = 1;
this.lblMenuID.Text = "所属菜单";
//111======25
this.cmbMenuID.Location = new System.Drawing.Point(173,21);
this.cmbMenuID.Name ="cmbMenuID";
this.cmbMenuID.Size = new System.Drawing.Size(100, 21);
this.cmbMenuID.TabIndex = 1;
this.Controls.Add(this.lblMenuID);
this.Controls.Add(this.cmbMenuID);

           //#####255BtnName###String
this.lblBtnName.AutoSize = true;
this.lblBtnName.Location = new System.Drawing.Point(100,50);
this.lblBtnName.Name = "lblBtnName";
this.lblBtnName.Size = new System.Drawing.Size(41, 12);
this.lblBtnName.TabIndex = 2;
this.lblBtnName.Text = "按钮名称";
this.txtBtnName.Location = new System.Drawing.Point(173,46);
this.txtBtnName.Name = "txtBtnName";
this.txtBtnName.Size = new System.Drawing.Size(100, 21);
this.txtBtnName.TabIndex = 2;
this.Controls.Add(this.lblBtnName);
this.Controls.Add(this.txtBtnName);

           //#####250BtnText###String
this.lblBtnText.AutoSize = true;
this.lblBtnText.Location = new System.Drawing.Point(100,75);
this.lblBtnText.Name = "lblBtnText";
this.lblBtnText.Size = new System.Drawing.Size(41, 12);
this.lblBtnText.TabIndex = 3;
this.lblBtnText.Text = "按钮文本";
this.txtBtnText.Location = new System.Drawing.Point(173,71);
this.txtBtnText.Name = "txtBtnText";
this.txtBtnText.Size = new System.Drawing.Size(100, 21);
this.txtBtnText.TabIndex = 3;
this.Controls.Add(this.lblBtnText);
this.Controls.Add(this.txtBtnText);

           //#####50HotKey###String
this.lblHotKey.AutoSize = true;
this.lblHotKey.Location = new System.Drawing.Point(100,100);
this.lblHotKey.Name = "lblHotKey";
this.lblHotKey.Size = new System.Drawing.Size(41, 12);
this.lblHotKey.TabIndex = 4;
this.lblHotKey.Text = "热键";
this.txtHotKey.Location = new System.Drawing.Point(173,96);
this.txtHotKey.Name = "txtHotKey";
this.txtHotKey.Size = new System.Drawing.Size(100, 21);
this.txtHotKey.TabIndex = 4;
this.Controls.Add(this.lblHotKey);
this.Controls.Add(this.txtHotKey);

           //#####100ButtonType###String
this.lblButtonType.AutoSize = true;
this.lblButtonType.Location = new System.Drawing.Point(100,125);
this.lblButtonType.Name = "lblButtonType";
this.lblButtonType.Size = new System.Drawing.Size(41, 12);
this.lblButtonType.TabIndex = 5;
this.lblButtonType.Text = "按钮类型";
this.txtButtonType.Location = new System.Drawing.Point(173,121);
this.txtButtonType.Name = "txtButtonType";
this.txtButtonType.Size = new System.Drawing.Size(100, 21);
this.txtButtonType.TabIndex = 5;
this.Controls.Add(this.lblButtonType);
this.Controls.Add(this.txtButtonType);

           //#####255FormName###String
this.lblFormName.AutoSize = true;
this.lblFormName.Location = new System.Drawing.Point(100,150);
this.lblFormName.Name = "lblFormName";
this.lblFormName.Size = new System.Drawing.Size(41, 12);
this.lblFormName.TabIndex = 6;
this.lblFormName.Text = "窗体名称";
this.txtFormName.Location = new System.Drawing.Point(173,146);
this.txtFormName.Name = "txtFormName";
this.txtFormName.Size = new System.Drawing.Size(100, 21);
this.txtFormName.TabIndex = 6;
this.Controls.Add(this.lblFormName);
this.Controls.Add(this.txtFormName);

           //#####500ClassPath###String
this.lblClassPath.AutoSize = true;
this.lblClassPath.Location = new System.Drawing.Point(100,175);
this.lblClassPath.Name = "lblClassPath";
this.lblClassPath.Size = new System.Drawing.Size(41, 12);
this.lblClassPath.TabIndex = 7;
this.lblClassPath.Text = "类路径";
this.txtClassPath.Location = new System.Drawing.Point(173,171);
this.txtClassPath.Name = "txtClassPath";
this.txtClassPath.Size = new System.Drawing.Size(100, 21);
this.txtClassPath.TabIndex = 7;
this.Controls.Add(this.lblClassPath);
this.Controls.Add(this.txtClassPath);

           //#####IsForm###Boolean
this.lblIsForm.AutoSize = true;
this.lblIsForm.Location = new System.Drawing.Point(100,200);
this.lblIsForm.Name = "lblIsForm";
this.lblIsForm.Size = new System.Drawing.Size(41, 12);
this.lblIsForm.TabIndex = 8;
this.lblIsForm.Text = "是否为窗体";
this.chkIsForm.Location = new System.Drawing.Point(173,196);
this.chkIsForm.Name = "chkIsForm";
this.chkIsForm.Size = new System.Drawing.Size(100, 21);
this.chkIsForm.TabIndex = 8;
this.Controls.Add(this.lblIsForm);
this.Controls.Add(this.chkIsForm);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,225);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 9;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,221);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 9;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,275);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 11;
this.lblCreated_at.Text = "创建时间";
//111======275
this.dtpCreated_at.Location = new System.Drawing.Point(173,271);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 11;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试300Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,300);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 12;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,296);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 12;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试350Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,350);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 14;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,346);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 14;
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
           // this.kryptonPanel1.TabIndex = 14;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMenuID );
this.Controls.Add(this.cmbMenuID );

                this.Controls.Add(this.lblBtnName );
this.Controls.Add(this.txtBtnName );

                this.Controls.Add(this.lblBtnText );
this.Controls.Add(this.txtBtnText );

                this.Controls.Add(this.lblHotKey );
this.Controls.Add(this.txtHotKey );

                this.Controls.Add(this.lblButtonType );
this.Controls.Add(this.txtButtonType );

                this.Controls.Add(this.lblFormName );
this.Controls.Add(this.txtFormName );

                this.Controls.Add(this.lblClassPath );
this.Controls.Add(this.txtClassPath );

                this.Controls.Add(this.lblIsForm );
this.Controls.Add(this.chkIsForm );

                this.Controls.Add(this.lblIsEnabled );
this.Controls.Add(this.chkIsEnabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_ButtonInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ButtonInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblMenuID;
private Krypton.Toolkit.KryptonComboBox cmbMenuID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBtnName;
private Krypton.Toolkit.KryptonTextBox txtBtnName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBtnText;
private Krypton.Toolkit.KryptonTextBox txtBtnText;

    
        
              private Krypton.Toolkit.KryptonLabel lblHotKey;
private Krypton.Toolkit.KryptonTextBox txtHotKey;

    
        
              private Krypton.Toolkit.KryptonLabel lblButtonType;
private Krypton.Toolkit.KryptonTextBox txtButtonType;

    
        
              private Krypton.Toolkit.KryptonLabel lblFormName;
private Krypton.Toolkit.KryptonTextBox txtFormName;

    
        
              private Krypton.Toolkit.KryptonLabel lblClassPath;
private Krypton.Toolkit.KryptonTextBox txtClassPath;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsForm;
private Krypton.Toolkit.KryptonCheckBox chkIsForm;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsEnabled;
private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
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

