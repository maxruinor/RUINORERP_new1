
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:37
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

           //#####255FormName###String
this.lblFormName.AutoSize = true;
this.lblFormName.Location = new System.Drawing.Point(100,125);
this.lblFormName.Name = "lblFormName";
this.lblFormName.Size = new System.Drawing.Size(41, 12);
this.lblFormName.TabIndex = 5;
this.lblFormName.Text = "窗体名称";
this.txtFormName.Location = new System.Drawing.Point(173,121);
this.txtFormName.Name = "txtFormName";
this.txtFormName.Size = new System.Drawing.Size(100, 21);
this.txtFormName.TabIndex = 5;
this.Controls.Add(this.lblFormName);
this.Controls.Add(this.txtFormName);

           //#####500ClassPath###String
this.lblClassPath.AutoSize = true;
this.lblClassPath.Location = new System.Drawing.Point(100,150);
this.lblClassPath.Name = "lblClassPath";
this.lblClassPath.Size = new System.Drawing.Size(41, 12);
this.lblClassPath.TabIndex = 6;
this.lblClassPath.Text = "类路径";
this.txtClassPath.Location = new System.Drawing.Point(173,146);
this.txtClassPath.Name = "txtClassPath";
this.txtClassPath.Size = new System.Drawing.Size(100, 21);
this.txtClassPath.TabIndex = 6;
this.Controls.Add(this.lblClassPath);
this.Controls.Add(this.txtClassPath);

           //#####IsForm###Boolean
this.lblIsForm.AutoSize = true;
this.lblIsForm.Location = new System.Drawing.Point(100,175);
this.lblIsForm.Name = "lblIsForm";
this.lblIsForm.Size = new System.Drawing.Size(41, 12);
this.lblIsForm.TabIndex = 7;
this.lblIsForm.Text = "是否为窗体";
this.chkIsForm.Location = new System.Drawing.Point(173,171);
this.chkIsForm.Name = "chkIsForm";
this.chkIsForm.Size = new System.Drawing.Size(100, 21);
this.chkIsForm.TabIndex = 7;
this.Controls.Add(this.lblIsForm);
this.Controls.Add(this.chkIsForm);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,200);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 8;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,196);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 8;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
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

    
    
   
 





    }
}


