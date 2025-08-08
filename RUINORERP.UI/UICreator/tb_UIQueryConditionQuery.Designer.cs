
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
    /// UI查询条件设置
    /// </summary>
    partial class tb_UIQueryConditionQuery
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

this.lblCaption = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCaption = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFieldName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFieldName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblValueType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtValueType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsVisble.Values.Text ="";

this.lblDefault1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefault1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDefault1.Multiline = true;

this.lblDefault2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefault2 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDefault2.Multiline = true;

this.lblEnableDefault1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEnableDefault1 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEnableDefault1.Values.Text ="";

this.lblEnableDefault2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEnableDefault2 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEnableDefault2.Values.Text ="";

this.lblUseLike = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkUseLike = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkUseLike.Values.Text ="";
this.chkUseLike.Checked = true;
this.chkUseLike.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblFocused = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkFocused = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkFocused.Values.Text ="";

this.lblMultiChoice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkMultiChoice = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkMultiChoice.Values.Text ="";



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

           //#####100Caption###String
this.lblCaption.AutoSize = true;
this.lblCaption.Location = new System.Drawing.Point(100,50);
this.lblCaption.Name = "lblCaption";
this.lblCaption.Size = new System.Drawing.Size(41, 12);
this.lblCaption.TabIndex = 2;
this.lblCaption.Text = "查询条件名";
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
this.lblFieldName.Text = "查询字段名";
this.txtFieldName.Location = new System.Drawing.Point(173,71);
this.txtFieldName.Name = "txtFieldName";
this.txtFieldName.Size = new System.Drawing.Size(100, 21);
this.txtFieldName.TabIndex = 3;
this.Controls.Add(this.lblFieldName);
this.Controls.Add(this.txtFieldName);

           //#####50ValueType###String
this.lblValueType.AutoSize = true;
this.lblValueType.Location = new System.Drawing.Point(100,100);
this.lblValueType.Name = "lblValueType";
this.lblValueType.Size = new System.Drawing.Size(41, 12);
this.lblValueType.TabIndex = 4;
this.lblValueType.Text = "值类型";
this.txtValueType.Location = new System.Drawing.Point(173,96);
this.txtValueType.Name = "txtValueType";
this.txtValueType.Size = new System.Drawing.Size(100, 21);
this.txtValueType.TabIndex = 4;
this.Controls.Add(this.lblValueType);
this.Controls.Add(this.txtValueType);

           //#####ControlWidth###Int32
//属性测试125ControlWidth

           //#####Sort###Int32
//属性测试150Sort

           //#####IsVisble###Boolean
this.lblIsVisble.AutoSize = true;
this.lblIsVisble.Location = new System.Drawing.Point(100,175);
this.lblIsVisble.Name = "lblIsVisble";
this.lblIsVisble.Size = new System.Drawing.Size(41, 12);
this.lblIsVisble.TabIndex = 7;
this.lblIsVisble.Text = "是否可见";
this.chkIsVisble.Location = new System.Drawing.Point(173,171);
this.chkIsVisble.Name = "chkIsVisble";
this.chkIsVisble.Size = new System.Drawing.Size(100, 21);
this.chkIsVisble.TabIndex = 7;
this.Controls.Add(this.lblIsVisble);
this.Controls.Add(this.chkIsVisble);

           //#####255Default1###String
this.lblDefault1.AutoSize = true;
this.lblDefault1.Location = new System.Drawing.Point(100,200);
this.lblDefault1.Name = "lblDefault1";
this.lblDefault1.Size = new System.Drawing.Size(41, 12);
this.lblDefault1.TabIndex = 8;
this.lblDefault1.Text = "默认值1";
this.txtDefault1.Location = new System.Drawing.Point(173,196);
this.txtDefault1.Name = "txtDefault1";
this.txtDefault1.Size = new System.Drawing.Size(100, 21);
this.txtDefault1.TabIndex = 8;
this.Controls.Add(this.lblDefault1);
this.Controls.Add(this.txtDefault1);

           //#####255Default2###String
this.lblDefault2.AutoSize = true;
this.lblDefault2.Location = new System.Drawing.Point(100,225);
this.lblDefault2.Name = "lblDefault2";
this.lblDefault2.Size = new System.Drawing.Size(41, 12);
this.lblDefault2.TabIndex = 9;
this.lblDefault2.Text = "默认值2";
this.txtDefault2.Location = new System.Drawing.Point(173,221);
this.txtDefault2.Name = "txtDefault2";
this.txtDefault2.Size = new System.Drawing.Size(100, 21);
this.txtDefault2.TabIndex = 9;
this.Controls.Add(this.lblDefault2);
this.Controls.Add(this.txtDefault2);

           //#####EnableDefault1###Boolean
this.lblEnableDefault1.AutoSize = true;
this.lblEnableDefault1.Location = new System.Drawing.Point(100,250);
this.lblEnableDefault1.Name = "lblEnableDefault1";
this.lblEnableDefault1.Size = new System.Drawing.Size(41, 12);
this.lblEnableDefault1.TabIndex = 10;
this.lblEnableDefault1.Text = "启用默认值1";
this.chkEnableDefault1.Location = new System.Drawing.Point(173,246);
this.chkEnableDefault1.Name = "chkEnableDefault1";
this.chkEnableDefault1.Size = new System.Drawing.Size(100, 21);
this.chkEnableDefault1.TabIndex = 10;
this.Controls.Add(this.lblEnableDefault1);
this.Controls.Add(this.chkEnableDefault1);

           //#####EnableDefault2###Boolean
this.lblEnableDefault2.AutoSize = true;
this.lblEnableDefault2.Location = new System.Drawing.Point(100,275);
this.lblEnableDefault2.Name = "lblEnableDefault2";
this.lblEnableDefault2.Size = new System.Drawing.Size(41, 12);
this.lblEnableDefault2.TabIndex = 11;
this.lblEnableDefault2.Text = "启用默认值2";
this.chkEnableDefault2.Location = new System.Drawing.Point(173,271);
this.chkEnableDefault2.Name = "chkEnableDefault2";
this.chkEnableDefault2.Size = new System.Drawing.Size(100, 21);
this.chkEnableDefault2.TabIndex = 11;
this.Controls.Add(this.lblEnableDefault2);
this.Controls.Add(this.chkEnableDefault2);

           //#####UseLike###Boolean
this.lblUseLike.AutoSize = true;
this.lblUseLike.Location = new System.Drawing.Point(100,300);
this.lblUseLike.Name = "lblUseLike";
this.lblUseLike.Size = new System.Drawing.Size(41, 12);
this.lblUseLike.TabIndex = 12;
this.lblUseLike.Text = "启用模糊查询";
this.chkUseLike.Location = new System.Drawing.Point(173,296);
this.chkUseLike.Name = "chkUseLike";
this.chkUseLike.Size = new System.Drawing.Size(100, 21);
this.chkUseLike.TabIndex = 12;
this.Controls.Add(this.lblUseLike);
this.Controls.Add(this.chkUseLike);

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

           //#####MultiChoice###Boolean
this.lblMultiChoice.AutoSize = true;
this.lblMultiChoice.Location = new System.Drawing.Point(100,350);
this.lblMultiChoice.Name = "lblMultiChoice";
this.lblMultiChoice.Size = new System.Drawing.Size(41, 12);
this.lblMultiChoice.TabIndex = 14;
this.lblMultiChoice.Text = "默认多选";
this.chkMultiChoice.Location = new System.Drawing.Point(173,346);
this.chkMultiChoice.Name = "chkMultiChoice";
this.chkMultiChoice.Size = new System.Drawing.Size(100, 21);
this.chkMultiChoice.TabIndex = 14;
this.Controls.Add(this.lblMultiChoice);
this.Controls.Add(this.chkMultiChoice);

           //#####DiffDays1###Int32
//属性测试375DiffDays1

           //#####DiffDays2###Int32
//属性测试400DiffDays2

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,425);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 17;
this.lblCreated_at.Text = "创建时间";
//111======425
this.dtpCreated_at.Location = new System.Drawing.Point(173,421);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 17;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试450Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,475);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 19;
this.lblModified_at.Text = "修改时间";
//111======475
this.dtpModified_at.Location = new System.Drawing.Point(173,471);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 19;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试500Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUIMenuPID );
this.Controls.Add(this.cmbUIMenuPID );

                this.Controls.Add(this.lblCaption );
this.Controls.Add(this.txtCaption );

                this.Controls.Add(this.lblFieldName );
this.Controls.Add(this.txtFieldName );

                this.Controls.Add(this.lblValueType );
this.Controls.Add(this.txtValueType );

                
                
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

                this.Controls.Add(this.lblUseLike );
this.Controls.Add(this.chkUseLike );

                this.Controls.Add(this.lblFocused );
this.Controls.Add(this.chkFocused );

                this.Controls.Add(this.lblMultiChoice );
this.Controls.Add(this.chkMultiChoice );

                
                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_UIQueryConditionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUIMenuPID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUIMenuPID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCaption;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCaption;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFieldName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFieldName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblValueType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtValueType;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsVisble;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsVisble;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefault1;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefault1;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefault2;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefault2;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEnableDefault1;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEnableDefault1;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEnableDefault2;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEnableDefault2;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUseLike;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkUseLike;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFocused;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkFocused;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMultiChoice;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkMultiChoice;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


