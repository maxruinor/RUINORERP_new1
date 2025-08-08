
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
    /// 产品属性表
    /// </summary>
    partial class tb_ProdPropertyQuery
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
     
     this.lblPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPropertyDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPropertyDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblInputType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInputType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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
                 //#####20PropertyName###String
this.lblPropertyName.AutoSize = true;
this.lblPropertyName.Location = new System.Drawing.Point(100,25);
this.lblPropertyName.Name = "lblPropertyName";
this.lblPropertyName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyName.TabIndex = 1;
this.lblPropertyName.Text = "属性名称";
this.txtPropertyName.Location = new System.Drawing.Point(173,21);
this.txtPropertyName.Name = "txtPropertyName";
this.txtPropertyName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyName.TabIndex = 1;
this.Controls.Add(this.lblPropertyName);
this.Controls.Add(this.txtPropertyName);

           //#####50PropertyDesc###String
this.lblPropertyDesc.AutoSize = true;
this.lblPropertyDesc.Location = new System.Drawing.Point(100,50);
this.lblPropertyDesc.Name = "lblPropertyDesc";
this.lblPropertyDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyDesc.TabIndex = 2;
this.lblPropertyDesc.Text = "属性描述";
this.txtPropertyDesc.Location = new System.Drawing.Point(173,46);
this.txtPropertyDesc.Name = "txtPropertyDesc";
this.txtPropertyDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyDesc.TabIndex = 2;
this.Controls.Add(this.lblPropertyDesc);
this.Controls.Add(this.txtPropertyDesc);

           //#####SortOrder###Int32

           //#####50InputType###String
this.lblInputType.AutoSize = true;
this.lblInputType.Location = new System.Drawing.Point(100,100);
this.lblInputType.Name = "lblInputType";
this.lblInputType.Size = new System.Drawing.Size(41, 12);
this.lblInputType.TabIndex = 4;
this.lblInputType.Text = "输入类型";
this.txtInputType.Location = new System.Drawing.Point(173,96);
this.txtInputType.Name = "txtInputType";
this.txtInputType.Size = new System.Drawing.Size(100, 21);
this.txtInputType.TabIndex = 4;
this.Controls.Add(this.lblInputType);
this.Controls.Add(this.txtInputType);

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPropertyName );
this.Controls.Add(this.txtPropertyName );

                this.Controls.Add(this.lblPropertyDesc );
this.Controls.Add(this.txtPropertyDesc );

                
                this.Controls.Add(this.lblInputType );
this.Controls.Add(this.txtInputType );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                    
            this.Name = "tb_ProdPropertyQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPropertyDesc;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInputType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInputType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
    
   
 





    }
}


