
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
    partial class tb_ButtonInfoQuery
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
     
     this.lblMenuID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMenuID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBtnName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBtnName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBtnName.Multiline = true;

this.lblBtnText = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBtnText = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBtnText.Multiline = true;

this.lblHotKey = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHotKey = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblButtonType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtButtonType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFormName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFormName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFormName.Multiline = true;

this.lblClassPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClassPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtClassPath.Multiline = true;

this.lblIsForm = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsForm = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsForm.Values.Text ="";

this.lblIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_ButtonInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMenuID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBtnName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBtnName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBtnText;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBtnText;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHotKey;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHotKey;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblButtonType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtButtonType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFormName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFormName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClassPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClassPath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsForm;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsForm;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsEnabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


