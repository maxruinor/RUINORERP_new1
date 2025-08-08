
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
    partial class tb_LocationQuery
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
     
     this.lblLocationType_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocationType_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblLocationCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocationCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                    
            this.Name = "tb_LocationQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocationType_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocationType_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocationCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocationCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              
    
    
   
 





    }
}


