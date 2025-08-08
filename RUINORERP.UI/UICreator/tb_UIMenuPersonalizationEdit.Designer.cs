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
    partial class tb_UIMenuPersonalizationEdit
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

this.lblUserPersonalizedID = new Krypton.Toolkit.KryptonLabel();
this.cmbUserPersonalizedID = new Krypton.Toolkit.KryptonComboBox();

this.lblQueryConditionCols = new Krypton.Toolkit.KryptonLabel();
this.txtQueryConditionCols = new Krypton.Toolkit.KryptonTextBox();

this.lblEnableQuerySettings = new Krypton.Toolkit.KryptonLabel();
this.chkEnableQuerySettings = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableQuerySettings.Values.Text ="";

this.lblEnableInputPresetValue = new Krypton.Toolkit.KryptonLabel();
this.chkEnableInputPresetValue = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableInputPresetValue.Values.Text ="";


this.lblBaseWidth = new Krypton.Toolkit.KryptonLabel();
this.txtBaseWidth = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblDefaultLayout = new Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout = new Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout.Multiline = true;

this.lblDefaultLayout2 = new Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout2 = new Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout2.Multiline = true;

    
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
this.lblQueryConditionCols.AutoSize = true;
this.lblQueryConditionCols.Location = new System.Drawing.Point(100,75);
this.lblQueryConditionCols.Name = "lblQueryConditionCols";
this.lblQueryConditionCols.Size = new System.Drawing.Size(41, 12);
this.lblQueryConditionCols.TabIndex = 3;
this.lblQueryConditionCols.Text = "条件显示列数量";
this.txtQueryConditionCols.Location = new System.Drawing.Point(173,71);
this.txtQueryConditionCols.Name = "txtQueryConditionCols";
this.txtQueryConditionCols.Size = new System.Drawing.Size(100, 21);
this.txtQueryConditionCols.TabIndex = 3;
this.Controls.Add(this.lblQueryConditionCols);
this.Controls.Add(this.txtQueryConditionCols);

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
this.lblBaseWidth.AutoSize = true;
this.lblBaseWidth.Location = new System.Drawing.Point(100,175);
this.lblBaseWidth.Name = "lblBaseWidth";
this.lblBaseWidth.Size = new System.Drawing.Size(41, 12);
this.lblBaseWidth.TabIndex = 7;
this.lblBaseWidth.Text = "排序";
this.txtBaseWidth.Location = new System.Drawing.Point(173,171);
this.txtBaseWidth.Name = "txtBaseWidth";
this.txtBaseWidth.Size = new System.Drawing.Size(100, 21);
this.txtBaseWidth.TabIndex = 7;
this.Controls.Add(this.lblBaseWidth);
this.Controls.Add(this.txtBaseWidth);

           //#####Sort###Int32
//属性测试200Sort
//属性测试200Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,200);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 8;
this.lblSort.Text = "基准宽度";
this.txtSort.Location = new System.Drawing.Point(173,196);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 8;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

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
           // this.kryptonPanel1.TabIndex = 10;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMenuID );
this.Controls.Add(this.cmbMenuID );

                this.Controls.Add(this.lblUserPersonalizedID );
this.Controls.Add(this.cmbUserPersonalizedID );

                this.Controls.Add(this.lblQueryConditionCols );
this.Controls.Add(this.txtQueryConditionCols );

                this.Controls.Add(this.lblEnableQuerySettings );
this.Controls.Add(this.chkEnableQuerySettings );

                this.Controls.Add(this.lblEnableInputPresetValue );
this.Controls.Add(this.chkEnableInputPresetValue );

                
                this.Controls.Add(this.lblBaseWidth );
this.Controls.Add(this.txtBaseWidth );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblDefaultLayout );
this.Controls.Add(this.txtDefaultLayout );

                this.Controls.Add(this.lblDefaultLayout2 );
this.Controls.Add(this.txtDefaultLayout2 );

                            // 
            // "tb_UIMenuPersonalizationEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UIMenuPersonalizationEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblUserPersonalizedID;
private Krypton.Toolkit.KryptonComboBox cmbUserPersonalizedID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQueryConditionCols;
private Krypton.Toolkit.KryptonTextBox txtQueryConditionCols;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableQuerySettings;
private Krypton.Toolkit.KryptonCheckBox chkEnableQuerySettings;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableInputPresetValue;
private Krypton.Toolkit.KryptonCheckBox chkEnableInputPresetValue;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblBaseWidth;
private Krypton.Toolkit.KryptonTextBox txtBaseWidth;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultLayout;
private Krypton.Toolkit.KryptonTextBox txtDefaultLayout;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultLayout2;
private Krypton.Toolkit.KryptonTextBox txtDefaultLayout2;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

