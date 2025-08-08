
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 打印模板
    /// </summary>
    partial class tb_PrintTemplateQuery
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
     
     this.lblPrintConfigID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPrintConfigID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTemplate_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTemplate_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBizName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBizName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTemplateFileData = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTemplateFileData = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtTemplateFileData.Multiline = true;


this.lblIsDefaultTemplate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsDefaultTemplate = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsDefaultTemplate.Values.Text ="";

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
                 //#####PrintConfigID###Int64
//属性测试25PrintConfigID
this.lblPrintConfigID.AutoSize = true;
this.lblPrintConfigID.Location = new System.Drawing.Point(100,25);
this.lblPrintConfigID.Name = "lblPrintConfigID";
this.lblPrintConfigID.Size = new System.Drawing.Size(41, 12);
this.lblPrintConfigID.TabIndex = 1;
this.lblPrintConfigID.Text = "";
//111======25
this.cmbPrintConfigID.Location = new System.Drawing.Point(173,21);
this.cmbPrintConfigID.Name ="cmbPrintConfigID";
this.cmbPrintConfigID.Size = new System.Drawing.Size(100, 21);
this.cmbPrintConfigID.TabIndex = 1;
this.Controls.Add(this.lblPrintConfigID);
this.Controls.Add(this.cmbPrintConfigID);

           //#####100Template_Name###String
this.lblTemplate_Name.AutoSize = true;
this.lblTemplate_Name.Location = new System.Drawing.Point(100,50);
this.lblTemplate_Name.Name = "lblTemplate_Name";
this.lblTemplate_Name.Size = new System.Drawing.Size(41, 12);
this.lblTemplate_Name.TabIndex = 2;
this.lblTemplate_Name.Text = "模板名称";
this.txtTemplate_Name.Location = new System.Drawing.Point(173,46);
this.txtTemplate_Name.Name = "txtTemplate_Name";
this.txtTemplate_Name.Size = new System.Drawing.Size(100, 21);
this.txtTemplate_Name.TabIndex = 2;
this.Controls.Add(this.lblTemplate_Name);
this.Controls.Add(this.txtTemplate_Name);

           //#####BizType###Int32
//属性测试75BizType

           //#####30BizName###String
this.lblBizName.AutoSize = true;
this.lblBizName.Location = new System.Drawing.Point(100,100);
this.lblBizName.Name = "lblBizName";
this.lblBizName.Size = new System.Drawing.Size(41, 12);
this.lblBizName.TabIndex = 4;
this.lblBizName.Text = "业务名称";
this.txtBizName.Location = new System.Drawing.Point(173,96);
this.txtBizName.Name = "txtBizName";
this.txtBizName.Size = new System.Drawing.Size(100, 21);
this.txtBizName.TabIndex = 4;
this.Controls.Add(this.lblBizName);
this.Controls.Add(this.txtBizName);

           //#####2147483647TemplateFileData###String
this.lblTemplateFileData.AutoSize = true;
this.lblTemplateFileData.Location = new System.Drawing.Point(100,125);
this.lblTemplateFileData.Name = "lblTemplateFileData";
this.lblTemplateFileData.Size = new System.Drawing.Size(41, 12);
this.lblTemplateFileData.TabIndex = 5;
this.lblTemplateFileData.Text = "模板文件数据";
this.txtTemplateFileData.Location = new System.Drawing.Point(173,121);
this.txtTemplateFileData.Name = "txtTemplateFileData";
this.txtTemplateFileData.Size = new System.Drawing.Size(100, 21);
this.txtTemplateFileData.TabIndex = 5;
this.txtTemplateFileData.Multiline = true;
this.Controls.Add(this.lblTemplateFileData);
this.Controls.Add(this.txtTemplateFileData);

           //#####-1TemplateFileStream###Binary

           //#####IsDefaultTemplate###Boolean
this.lblIsDefaultTemplate.AutoSize = true;
this.lblIsDefaultTemplate.Location = new System.Drawing.Point(100,175);
this.lblIsDefaultTemplate.Name = "lblIsDefaultTemplate";
this.lblIsDefaultTemplate.Size = new System.Drawing.Size(41, 12);
this.lblIsDefaultTemplate.TabIndex = 7;
this.lblIsDefaultTemplate.Text = "默认模板";
this.chkIsDefaultTemplate.Location = new System.Drawing.Point(173,171);
this.chkIsDefaultTemplate.Name = "chkIsDefaultTemplate";
this.chkIsDefaultTemplate.Size = new System.Drawing.Size(100, 21);
this.chkIsDefaultTemplate.TabIndex = 7;
this.Controls.Add(this.lblIsDefaultTemplate);
this.Controls.Add(this.chkIsDefaultTemplate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,200);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 8;
this.lblCreated_at.Text = "创建时间";
//111======200
this.dtpCreated_at.Location = new System.Drawing.Point(173,196);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 8;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试225Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,250);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 10;
this.lblModified_at.Text = "修改时间";
//111======250
this.dtpModified_at.Location = new System.Drawing.Point(173,246);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 10;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试275Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,300);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 12;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,296);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 12;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPrintConfigID );
this.Controls.Add(this.cmbPrintConfigID );

                this.Controls.Add(this.lblTemplate_Name );
this.Controls.Add(this.txtTemplate_Name );

                
                this.Controls.Add(this.lblBizName );
this.Controls.Add(this.txtBizName );

                this.Controls.Add(this.lblTemplateFileData );
this.Controls.Add(this.txtTemplateFileData );

                
                this.Controls.Add(this.lblIsDefaultTemplate );
this.Controls.Add(this.chkIsDefaultTemplate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_PrintTemplateQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrintConfigID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPrintConfigID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTemplate_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTemplate_Name;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBizName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBizName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTemplateFileData;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTemplateFileData;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsDefaultTemplate;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsDefaultTemplate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


