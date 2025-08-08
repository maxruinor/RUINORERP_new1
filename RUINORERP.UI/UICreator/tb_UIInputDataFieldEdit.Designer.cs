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
    /// UI录入数据预设值表
    /// </summary>
    partial class tb_UIInputDataFieldEdit
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

this.lblCaption = new Krypton.Toolkit.KryptonLabel();
this.txtCaption = new Krypton.Toolkit.KryptonTextBox();

this.lblFieldName = new Krypton.Toolkit.KryptonLabel();
this.txtFieldName = new Krypton.Toolkit.KryptonTextBox();

this.lblBelongingObjectType = new Krypton.Toolkit.KryptonLabel();
this.txtBelongingObjectType = new Krypton.Toolkit.KryptonTextBox();

this.lblValueType = new Krypton.Toolkit.KryptonLabel();
this.txtValueType = new Krypton.Toolkit.KryptonTextBox();

this.lblControlWidth = new Krypton.Toolkit.KryptonLabel();
this.txtControlWidth = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblIsVisble = new Krypton.Toolkit.KryptonLabel();
this.chkIsVisble = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsVisble.Values.Text ="";

this.lblDefault1 = new Krypton.Toolkit.KryptonLabel();
this.txtDefault1 = new Krypton.Toolkit.KryptonTextBox();
this.txtDefault1.Multiline = true;

this.lblDefault2 = new Krypton.Toolkit.KryptonLabel();
this.txtDefault2 = new Krypton.Toolkit.KryptonTextBox();
this.txtDefault2.Multiline = true;

this.lblEnableDefault1 = new Krypton.Toolkit.KryptonLabel();
this.chkEnableDefault1 = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableDefault1.Values.Text ="";

this.lblEnableDefault2 = new Krypton.Toolkit.KryptonLabel();
this.chkEnableDefault2 = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableDefault2.Values.Text ="";

this.lblFocused = new Krypton.Toolkit.KryptonLabel();
this.chkFocused = new Krypton.Toolkit.KryptonCheckBox();
this.chkFocused.Values.Text ="";

this.lblDiffDays1 = new Krypton.Toolkit.KryptonLabel();
this.txtDiffDays1 = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffDays2 = new Krypton.Toolkit.KryptonLabel();
this.txtDiffDays2 = new Krypton.Toolkit.KryptonTextBox();

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

           //#####100Caption###String
this.lblCaption.AutoSize = true;
this.lblCaption.Location = new System.Drawing.Point(100,50);
this.lblCaption.Name = "lblCaption";
this.lblCaption.Size = new System.Drawing.Size(41, 12);
this.lblCaption.TabIndex = 2;
this.lblCaption.Text = "字段标题";
this.txtCaption.Location = new System.Drawing.Point(173,46);
this.txtCaption.Name = "txtCaption";
this.txtCaption.Size = new System.Drawing.Size(100, 21);
this.txtCaption.TabIndex = 2;
this.Controls.Add(this.lblCaption);
this.Controls.Add(this.txtCaption);

           //#####100FieldName###String
this.lblFieldName.AutoSize = true;
this.lblFieldName.Location = new System.Drawing.Point(100,75);
this.lblFieldName.Name = "lblFieldName";
this.lblFieldName.Size = new System.Drawing.Size(41, 12);
this.lblFieldName.TabIndex = 3;
this.lblFieldName.Text = "字段名";
this.txtFieldName.Location = new System.Drawing.Point(173,71);
this.txtFieldName.Name = "txtFieldName";
this.txtFieldName.Size = new System.Drawing.Size(100, 21);
this.txtFieldName.TabIndex = 3;
this.Controls.Add(this.lblFieldName);
this.Controls.Add(this.txtFieldName);

           //#####80BelongingObjectType###String
this.lblBelongingObjectType.AutoSize = true;
this.lblBelongingObjectType.Location = new System.Drawing.Point(100,100);
this.lblBelongingObjectType.Name = "lblBelongingObjectType";
this.lblBelongingObjectType.Size = new System.Drawing.Size(41, 12);
this.lblBelongingObjectType.TabIndex = 4;
this.lblBelongingObjectType.Text = "所属实体";
this.txtBelongingObjectType.Location = new System.Drawing.Point(173,96);
this.txtBelongingObjectType.Name = "txtBelongingObjectType";
this.txtBelongingObjectType.Size = new System.Drawing.Size(100, 21);
this.txtBelongingObjectType.TabIndex = 4;
this.Controls.Add(this.lblBelongingObjectType);
this.Controls.Add(this.txtBelongingObjectType);

           //#####50ValueType###String
this.lblValueType.AutoSize = true;
this.lblValueType.Location = new System.Drawing.Point(100,125);
this.lblValueType.Name = "lblValueType";
this.lblValueType.Size = new System.Drawing.Size(41, 12);
this.lblValueType.TabIndex = 5;
this.lblValueType.Text = "值类型";
this.txtValueType.Location = new System.Drawing.Point(173,121);
this.txtValueType.Name = "txtValueType";
this.txtValueType.Size = new System.Drawing.Size(100, 21);
this.txtValueType.TabIndex = 5;
this.Controls.Add(this.lblValueType);
this.Controls.Add(this.txtValueType);

           //#####ControlWidth###Int32
//属性测试150ControlWidth
this.lblControlWidth.AutoSize = true;
this.lblControlWidth.Location = new System.Drawing.Point(100,150);
this.lblControlWidth.Name = "lblControlWidth";
this.lblControlWidth.Size = new System.Drawing.Size(41, 12);
this.lblControlWidth.TabIndex = 6;
this.lblControlWidth.Text = "控件宽度";
this.txtControlWidth.Location = new System.Drawing.Point(173,146);
this.txtControlWidth.Name = "txtControlWidth";
this.txtControlWidth.Size = new System.Drawing.Size(100, 21);
this.txtControlWidth.TabIndex = 6;
this.Controls.Add(this.lblControlWidth);
this.Controls.Add(this.txtControlWidth);

           //#####Sort###Int32
//属性测试175Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,175);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 7;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,171);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 7;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

           //#####IsVisble###Boolean
this.lblIsVisble.AutoSize = true;
this.lblIsVisble.Location = new System.Drawing.Point(100,200);
this.lblIsVisble.Name = "lblIsVisble";
this.lblIsVisble.Size = new System.Drawing.Size(41, 12);
this.lblIsVisble.TabIndex = 8;
this.lblIsVisble.Text = "是否可见";
this.chkIsVisble.Location = new System.Drawing.Point(173,196);
this.chkIsVisble.Name = "chkIsVisble";
this.chkIsVisble.Size = new System.Drawing.Size(100, 21);
this.chkIsVisble.TabIndex = 8;
this.Controls.Add(this.lblIsVisble);
this.Controls.Add(this.chkIsVisble);

           //#####255Default1###String
this.lblDefault1.AutoSize = true;
this.lblDefault1.Location = new System.Drawing.Point(100,225);
this.lblDefault1.Name = "lblDefault1";
this.lblDefault1.Size = new System.Drawing.Size(41, 12);
this.lblDefault1.TabIndex = 9;
this.lblDefault1.Text = "默认值1";
this.txtDefault1.Location = new System.Drawing.Point(173,221);
this.txtDefault1.Name = "txtDefault1";
this.txtDefault1.Size = new System.Drawing.Size(100, 21);
this.txtDefault1.TabIndex = 9;
this.Controls.Add(this.lblDefault1);
this.Controls.Add(this.txtDefault1);

           //#####255Default2###String
this.lblDefault2.AutoSize = true;
this.lblDefault2.Location = new System.Drawing.Point(100,250);
this.lblDefault2.Name = "lblDefault2";
this.lblDefault2.Size = new System.Drawing.Size(41, 12);
this.lblDefault2.TabIndex = 10;
this.lblDefault2.Text = "默认值2";
this.txtDefault2.Location = new System.Drawing.Point(173,246);
this.txtDefault2.Name = "txtDefault2";
this.txtDefault2.Size = new System.Drawing.Size(100, 21);
this.txtDefault2.TabIndex = 10;
this.Controls.Add(this.lblDefault2);
this.Controls.Add(this.txtDefault2);

           //#####EnableDefault1###Boolean
this.lblEnableDefault1.AutoSize = true;
this.lblEnableDefault1.Location = new System.Drawing.Point(100,275);
this.lblEnableDefault1.Name = "lblEnableDefault1";
this.lblEnableDefault1.Size = new System.Drawing.Size(41, 12);
this.lblEnableDefault1.TabIndex = 11;
this.lblEnableDefault1.Text = "启用默认值1";
this.chkEnableDefault1.Location = new System.Drawing.Point(173,271);
this.chkEnableDefault1.Name = "chkEnableDefault1";
this.chkEnableDefault1.Size = new System.Drawing.Size(100, 21);
this.chkEnableDefault1.TabIndex = 11;
this.Controls.Add(this.lblEnableDefault1);
this.Controls.Add(this.chkEnableDefault1);

           //#####EnableDefault2###Boolean
this.lblEnableDefault2.AutoSize = true;
this.lblEnableDefault2.Location = new System.Drawing.Point(100,300);
this.lblEnableDefault2.Name = "lblEnableDefault2";
this.lblEnableDefault2.Size = new System.Drawing.Size(41, 12);
this.lblEnableDefault2.TabIndex = 12;
this.lblEnableDefault2.Text = "启用默认值2";
this.chkEnableDefault2.Location = new System.Drawing.Point(173,296);
this.chkEnableDefault2.Name = "chkEnableDefault2";
this.chkEnableDefault2.Size = new System.Drawing.Size(100, 21);
this.chkEnableDefault2.TabIndex = 12;
this.Controls.Add(this.lblEnableDefault2);
this.Controls.Add(this.chkEnableDefault2);

           //#####Focused###Boolean
this.lblFocused.AutoSize = true;
this.lblFocused.Location = new System.Drawing.Point(100,325);
this.lblFocused.Name = "lblFocused";
this.lblFocused.Size = new System.Drawing.Size(41, 12);
this.lblFocused.TabIndex = 13;
this.lblFocused.Text = "默认焦点";
this.chkFocused.Location = new System.Drawing.Point(173,321);
this.chkFocused.Name = "chkFocused";
this.chkFocused.Size = new System.Drawing.Size(100, 21);
this.chkFocused.TabIndex = 13;
this.Controls.Add(this.lblFocused);
this.Controls.Add(this.chkFocused);

           //#####DiffDays1###Int32
//属性测试350DiffDays1
this.lblDiffDays1.AutoSize = true;
this.lblDiffDays1.Location = new System.Drawing.Point(100,350);
this.lblDiffDays1.Name = "lblDiffDays1";
this.lblDiffDays1.Size = new System.Drawing.Size(41, 12);
this.lblDiffDays1.TabIndex = 14;
this.lblDiffDays1.Text = "差异天数1";
this.txtDiffDays1.Location = new System.Drawing.Point(173,346);
this.txtDiffDays1.Name = "txtDiffDays1";
this.txtDiffDays1.Size = new System.Drawing.Size(100, 21);
this.txtDiffDays1.TabIndex = 14;
this.Controls.Add(this.lblDiffDays1);
this.Controls.Add(this.txtDiffDays1);

           //#####DiffDays2###Int32
//属性测试375DiffDays2
this.lblDiffDays2.AutoSize = true;
this.lblDiffDays2.Location = new System.Drawing.Point(100,375);
this.lblDiffDays2.Name = "lblDiffDays2";
this.lblDiffDays2.Size = new System.Drawing.Size(41, 12);
this.lblDiffDays2.TabIndex = 15;
this.lblDiffDays2.Text = "差异天数2";
this.txtDiffDays2.Location = new System.Drawing.Point(173,371);
this.txtDiffDays2.Name = "txtDiffDays2";
this.txtDiffDays2.Size = new System.Drawing.Size(100, 21);
this.txtDiffDays2.TabIndex = 15;
this.Controls.Add(this.lblDiffDays2);
this.Controls.Add(this.txtDiffDays2);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试425Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,425);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 17;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,421);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 17;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试475Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,475);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 19;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,471);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 19;
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
           // this.kryptonPanel1.TabIndex = 19;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUIMenuPID );
this.Controls.Add(this.cmbUIMenuPID );

                this.Controls.Add(this.lblCaption );
this.Controls.Add(this.txtCaption );

                this.Controls.Add(this.lblFieldName );
this.Controls.Add(this.txtFieldName );

                this.Controls.Add(this.lblBelongingObjectType );
this.Controls.Add(this.txtBelongingObjectType );

                this.Controls.Add(this.lblValueType );
this.Controls.Add(this.txtValueType );

                this.Controls.Add(this.lblControlWidth );
this.Controls.Add(this.txtControlWidth );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblIsVisble );
this.Controls.Add(this.chkIsVisble );

                this.Controls.Add(this.lblDefault1 );
this.Controls.Add(this.txtDefault1 );

                this.Controls.Add(this.lblDefault2 );
this.Controls.Add(this.txtDefault2 );

                this.Controls.Add(this.lblEnableDefault1 );
this.Controls.Add(this.chkEnableDefault1 );

                this.Controls.Add(this.lblEnableDefault2 );
this.Controls.Add(this.chkEnableDefault2 );

                this.Controls.Add(this.lblFocused );
this.Controls.Add(this.chkFocused );

                this.Controls.Add(this.lblDiffDays1 );
this.Controls.Add(this.txtDiffDays1 );

                this.Controls.Add(this.lblDiffDays2 );
this.Controls.Add(this.txtDiffDays2 );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_UIInputDataFieldEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UIInputDataFieldEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblCaption;
private Krypton.Toolkit.KryptonTextBox txtCaption;

    
        
              private Krypton.Toolkit.KryptonLabel lblFieldName;
private Krypton.Toolkit.KryptonTextBox txtFieldName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBelongingObjectType;
private Krypton.Toolkit.KryptonTextBox txtBelongingObjectType;

    
        
              private Krypton.Toolkit.KryptonLabel lblValueType;
private Krypton.Toolkit.KryptonTextBox txtValueType;

    
        
              private Krypton.Toolkit.KryptonLabel lblControlWidth;
private Krypton.Toolkit.KryptonTextBox txtControlWidth;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsVisble;
private Krypton.Toolkit.KryptonCheckBox chkIsVisble;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefault1;
private Krypton.Toolkit.KryptonTextBox txtDefault1;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefault2;
private Krypton.Toolkit.KryptonTextBox txtDefault2;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableDefault1;
private Krypton.Toolkit.KryptonCheckBox chkEnableDefault1;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableDefault2;
private Krypton.Toolkit.KryptonCheckBox chkEnableDefault2;

    
        
              private Krypton.Toolkit.KryptonLabel lblFocused;
private Krypton.Toolkit.KryptonCheckBox chkFocused;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffDays1;
private Krypton.Toolkit.KryptonTextBox txtDiffDays1;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffDays2;
private Krypton.Toolkit.KryptonTextBox txtDiffDays2;

    
        
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

