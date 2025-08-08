
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据
    /// </summary>
    partial class tb_UIMenuPersonalizationQuery
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

this.lblUserPersonalizedID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUserPersonalizedID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblEnableQuerySettings = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEnableQuerySettings = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEnableQuerySettings.Values.Text ="";

this.lblEnableInputPresetValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEnableInputPresetValue = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEnableInputPresetValue.Values.Text ="";




this.lblDefaultLayout = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout.Multiline = true;

this.lblDefaultLayout2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout2 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout2.Multiline = true;

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
this.lblMenuID.Text = "关联菜单";
//111======25
this.cmbMenuID.Location = new System.Drawing.Point(173,21);
this.cmbMenuID.Name ="cmbMenuID";
this.cmbMenuID.Size = new System.Drawing.Size(100, 21);
this.cmbMenuID.TabIndex = 1;
this.Controls.Add(this.lblMenuID);
this.Controls.Add(this.cmbMenuID);

           //#####UserPersonalizedID###Int64
//属性测试50UserPersonalizedID
//属性测试50UserPersonalizedID
this.lblUserPersonalizedID.AutoSize = true;
this.lblUserPersonalizedID.Location = new System.Drawing.Point(100,50);
this.lblUserPersonalizedID.Name = "lblUserPersonalizedID";
this.lblUserPersonalizedID.Size = new System.Drawing.Size(41, 12);
this.lblUserPersonalizedID.TabIndex = 2;
this.lblUserPersonalizedID.Text = "用户角色设置";
//111======50
this.cmbUserPersonalizedID.Location = new System.Drawing.Point(173,46);
this.cmbUserPersonalizedID.Name ="cmbUserPersonalizedID";
this.cmbUserPersonalizedID.Size = new System.Drawing.Size(100, 21);
this.cmbUserPersonalizedID.TabIndex = 2;
this.Controls.Add(this.lblUserPersonalizedID);
this.Controls.Add(this.cmbUserPersonalizedID);

           //#####QueryConditionCols###Int32
//属性测试75QueryConditionCols
//属性测试75QueryConditionCols

           //#####EnableQuerySettings###Boolean
this.lblEnableQuerySettings.AutoSize = true;
this.lblEnableQuerySettings.Location = new System.Drawing.Point(100,100);
this.lblEnableQuerySettings.Name = "lblEnableQuerySettings";
this.lblEnableQuerySettings.Size = new System.Drawing.Size(41, 12);
this.lblEnableQuerySettings.TabIndex = 4;
this.lblEnableQuerySettings.Text = "启用查询预设值";
this.chkEnableQuerySettings.Location = new System.Drawing.Point(173,96);
this.chkEnableQuerySettings.Name = "chkEnableQuerySettings";
this.chkEnableQuerySettings.Size = new System.Drawing.Size(100, 21);
this.chkEnableQuerySettings.TabIndex = 4;
this.Controls.Add(this.lblEnableQuerySettings);
this.Controls.Add(this.chkEnableQuerySettings);

           //#####EnableInputPresetValue###Boolean
this.lblEnableInputPresetValue.AutoSize = true;
this.lblEnableInputPresetValue.Location = new System.Drawing.Point(100,125);
this.lblEnableInputPresetValue.Name = "lblEnableInputPresetValue";
this.lblEnableInputPresetValue.Size = new System.Drawing.Size(41, 12);
this.lblEnableInputPresetValue.TabIndex = 5;
this.lblEnableInputPresetValue.Text = "启用录入预设值";
this.chkEnableInputPresetValue.Location = new System.Drawing.Point(173,121);
this.chkEnableInputPresetValue.Name = "chkEnableInputPresetValue";
this.chkEnableInputPresetValue.Size = new System.Drawing.Size(100, 21);
this.chkEnableInputPresetValue.TabIndex = 5;
this.Controls.Add(this.lblEnableInputPresetValue);
this.Controls.Add(this.chkEnableInputPresetValue);

           //#####FavoritesMenu###SByte

           //#####BaseWidth###Int32
//属性测试175BaseWidth
//属性测试175BaseWidth

           //#####Sort###Int32
//属性测试200Sort
//属性测试200Sort

           //#####2147483647DefaultLayout###String
this.lblDefaultLayout.AutoSize = true;
this.lblDefaultLayout.Location = new System.Drawing.Point(100,225);
this.lblDefaultLayout.Name = "lblDefaultLayout";
this.lblDefaultLayout.Size = new System.Drawing.Size(41, 12);
this.lblDefaultLayout.TabIndex = 9;
this.lblDefaultLayout.Text = "默认布局";
this.txtDefaultLayout.Location = new System.Drawing.Point(173,221);
this.txtDefaultLayout.Name = "txtDefaultLayout";
this.txtDefaultLayout.Size = new System.Drawing.Size(100, 21);
this.txtDefaultLayout.TabIndex = 9;
this.txtDefaultLayout.Multiline = true;
this.Controls.Add(this.lblDefaultLayout);
this.Controls.Add(this.txtDefaultLayout);

           //#####2147483647DefaultLayout2###String
this.lblDefaultLayout2.AutoSize = true;
this.lblDefaultLayout2.Location = new System.Drawing.Point(100,250);
this.lblDefaultLayout2.Name = "lblDefaultLayout2";
this.lblDefaultLayout2.Size = new System.Drawing.Size(41, 12);
this.lblDefaultLayout2.TabIndex = 10;
this.lblDefaultLayout2.Text = "默认布局";
this.txtDefaultLayout2.Location = new System.Drawing.Point(173,246);
this.txtDefaultLayout2.Name = "txtDefaultLayout2";
this.txtDefaultLayout2.Size = new System.Drawing.Size(100, 21);
this.txtDefaultLayout2.TabIndex = 10;
this.txtDefaultLayout2.Multiline = true;
this.Controls.Add(this.lblDefaultLayout2);
this.Controls.Add(this.txtDefaultLayout2);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMenuID );
this.Controls.Add(this.cmbMenuID );

                this.Controls.Add(this.lblUserPersonalizedID );
this.Controls.Add(this.cmbUserPersonalizedID );

                
                this.Controls.Add(this.lblEnableQuerySettings );
this.Controls.Add(this.chkEnableQuerySettings );

                this.Controls.Add(this.lblEnableInputPresetValue );
this.Controls.Add(this.chkEnableInputPresetValue );

                
                
                
                this.Controls.Add(this.lblDefaultLayout );
this.Controls.Add(this.txtDefaultLayout );

                this.Controls.Add(this.lblDefaultLayout2 );
this.Controls.Add(this.txtDefaultLayout2 );

                    
            this.Name = "tb_UIMenuPersonalizationQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMenuID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUserPersonalizedID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUserPersonalizedID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEnableQuerySettings;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEnableQuerySettings;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEnableInputPresetValue;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEnableInputPresetValue;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultLayout;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefaultLayout;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultLayout2;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefaultLayout2;

    
    
   
 





    }
}


