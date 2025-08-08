
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品属性值表
    /// </summary>
    partial class tb_ProdPropertyValueQuery
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
     
     this.lblProperty_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProperty_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPropertyValueDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Property_ID###Int64
//属性测试25Property_ID
this.lblProperty_ID.AutoSize = true;
this.lblProperty_ID.Location = new System.Drawing.Point(100,25);
this.lblProperty_ID.Name = "lblProperty_ID";
this.lblProperty_ID.Size = new System.Drawing.Size(41, 12);
this.lblProperty_ID.TabIndex = 1;
this.lblProperty_ID.Text = "属性";
//111======25
this.cmbProperty_ID.Location = new System.Drawing.Point(173,21);
this.cmbProperty_ID.Name ="cmbProperty_ID";
this.cmbProperty_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProperty_ID.TabIndex = 1;
this.Controls.Add(this.lblProperty_ID);
this.Controls.Add(this.cmbProperty_ID);

           //#####20PropertyValueName###String
this.lblPropertyValueName.AutoSize = true;
this.lblPropertyValueName.Location = new System.Drawing.Point(100,50);
this.lblPropertyValueName.Name = "lblPropertyValueName";
this.lblPropertyValueName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueName.TabIndex = 2;
this.lblPropertyValueName.Text = "属性值名称";
this.txtPropertyValueName.Location = new System.Drawing.Point(173,46);
this.txtPropertyValueName.Name = "txtPropertyValueName";
this.txtPropertyValueName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueName.TabIndex = 2;
this.Controls.Add(this.lblPropertyValueName);
this.Controls.Add(this.txtPropertyValueName);

           //#####50PropertyValueDesc###String
this.lblPropertyValueDesc.AutoSize = true;
this.lblPropertyValueDesc.Location = new System.Drawing.Point(100,75);
this.lblPropertyValueDesc.Name = "lblPropertyValueDesc";
this.lblPropertyValueDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueDesc.TabIndex = 3;
this.lblPropertyValueDesc.Text = "属性值描述";
this.txtPropertyValueDesc.Location = new System.Drawing.Point(173,71);
this.txtPropertyValueDesc.Name = "txtPropertyValueDesc";
this.txtPropertyValueDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueDesc.TabIndex = 3;
this.Controls.Add(this.lblPropertyValueDesc);
this.Controls.Add(this.txtPropertyValueDesc);

           //#####SortOrder###Int32
//属性测试100SortOrder

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,125);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 5;
this.lblCreated_at.Text = "创建时间";
//111======125
this.dtpCreated_at.Location = new System.Drawing.Point(173,121);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 5;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试150Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,175);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 7;
this.lblModified_at.Text = "修改时间";
//111======175
this.dtpModified_at.Location = new System.Drawing.Point(173,171);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 7;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试200Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,225);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 9;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,221);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 9;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试250DataStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProperty_ID );
this.Controls.Add(this.cmbProperty_ID );

                this.Controls.Add(this.lblPropertyValueName );
this.Controls.Add(this.txtPropertyValueName );

                this.Controls.Add(this.lblPropertyValueDesc );
this.Controls.Add(this.txtPropertyValueDesc );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                    
            this.Name = "tb_ProdPropertyValueQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProperty_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProperty_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyValueName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyValueName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyValueDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyValueDesc;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
    
   
 





    }
}


