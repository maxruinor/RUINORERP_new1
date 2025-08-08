
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:23
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
    partial class tb_UIGridSettingQuery
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
     
     this.lblUIMenuPID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUIMenuPID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblGridKeyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGridKeyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtGridKeyName.Multiline = true;

this.lblColsSetting = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtColsSetting = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtColsSetting.Multiline = true;

this.lblGridType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGridType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_UIGridSettingQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUIMenuPID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUIMenuPID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGridKeyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGridKeyName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblColsSetting;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtColsSetting;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGridType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGridType;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


