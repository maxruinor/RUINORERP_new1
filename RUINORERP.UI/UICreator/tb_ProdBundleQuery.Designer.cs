
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:53
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品套装表
    /// </summary>
    partial class tb_ProdBundleQuery
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
     
     this.lblBundleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBundleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBundleName.Multiline = true;

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtImagesPath.Multiline = true;


this.lblWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMarket_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMarket_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";
this.chkIs_available.Checked = true;
this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####255BundleName###String
this.lblBundleName.AutoSize = true;
this.lblBundleName.Location = new System.Drawing.Point(100,25);
this.lblBundleName.Name = "lblBundleName";
this.lblBundleName.Size = new System.Drawing.Size(41, 12);
this.lblBundleName.TabIndex = 1;
this.lblBundleName.Text = "套装名称";
this.txtBundleName.Location = new System.Drawing.Point(173,21);
this.txtBundleName.Name = "txtBundleName";
this.txtBundleName.Size = new System.Drawing.Size(100, 21);
this.txtBundleName.TabIndex = 1;
this.Controls.Add(this.lblBundleName);
this.Controls.Add(this.txtBundleName);

           //#####255Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,50);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 2;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,46);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 2;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####Unit_ID###Int64
//属性测试75Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,75);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 3;
this.lblUnit_ID.Text = "套装单位";
//111======75
this.cmbUnit_ID.Location = new System.Drawing.Point(173,71);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 3;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####TargetQty###Int32
//属性测试100TargetQty

           //#####2000ImagesPath###String
this.lblImagesPath.AutoSize = true;
this.lblImagesPath.Location = new System.Drawing.Point(100,125);
this.lblImagesPath.Name = "lblImagesPath";
this.lblImagesPath.Size = new System.Drawing.Size(41, 12);
this.lblImagesPath.TabIndex = 5;
this.lblImagesPath.Text = "产品图片";
this.txtImagesPath.Location = new System.Drawing.Point(173,121);
this.txtImagesPath.Name = "txtImagesPath";
this.txtImagesPath.Size = new System.Drawing.Size(100, 21);
this.txtImagesPath.TabIndex = 5;
this.Controls.Add(this.lblImagesPath);
this.Controls.Add(this.txtImagesPath);

           //#####2147483647BundleImage###Binary

           //#####Weight###Decimal
this.lblWeight.AutoSize = true;
this.lblWeight.Location = new System.Drawing.Point(100,175);
this.lblWeight.Name = "lblWeight";
this.lblWeight.Size = new System.Drawing.Size(41, 12);
this.lblWeight.TabIndex = 7;
this.lblWeight.Text = "重量（千克）";
//111======175
this.txtWeight.Location = new System.Drawing.Point(173,171);
this.txtWeight.Name ="txtWeight";
this.txtWeight.Size = new System.Drawing.Size(100, 21);
this.txtWeight.TabIndex = 7;
this.Controls.Add(this.lblWeight);
this.Controls.Add(this.txtWeight);

           //#####Market_Price###Decimal
this.lblMarket_Price.AutoSize = true;
this.lblMarket_Price.Location = new System.Drawing.Point(100,200);
this.lblMarket_Price.Name = "lblMarket_Price";
this.lblMarket_Price.Size = new System.Drawing.Size(41, 12);
this.lblMarket_Price.TabIndex = 8;
this.lblMarket_Price.Text = "市场零售价";
//111======200
this.txtMarket_Price.Location = new System.Drawing.Point(173,196);
this.txtMarket_Price.Name ="txtMarket_Price";
this.txtMarket_Price.Size = new System.Drawing.Size(100, 21);
this.txtMarket_Price.TabIndex = 8;
this.Controls.Add(this.lblMarket_Price);
this.Controls.Add(this.txtMarket_Price);

           //#####255Notes###String
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

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,250);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 10;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,246);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 10;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,275);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 11;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,271);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 11;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,300);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 12;
this.lblCreated_at.Text = "创建时间";
//111======300
this.dtpCreated_at.Location = new System.Drawing.Point(173,296);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 12;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试325Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,350);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 14;
this.lblModified_at.Text = "修改时间";
//111======350
this.dtpModified_at.Location = new System.Drawing.Point(173,346);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 14;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试375Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,400);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 16;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,396);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 16;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试425DataStatus

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,450);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 18;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,446);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 18;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试475Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,500);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 20;
this.lblApprover_at.Text = "审批时间";
//111======500
this.dtpApprover_at.Location = new System.Drawing.Point(173,496);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 20;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,550);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 22;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,546);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 22;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试575PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBundleName );
this.Controls.Add(this.txtBundleName );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                
                this.Controls.Add(this.lblImagesPath );
this.Controls.Add(this.txtImagesPath );

                
                this.Controls.Add(this.lblWeight );
this.Controls.Add(this.txtWeight );

                this.Controls.Add(this.lblMarket_Price );
this.Controls.Add(this.txtMarket_Price );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                    
            this.Name = "tb_ProdBundleQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBundleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBundleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblImagesPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtImagesPath;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMarket_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMarket_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
    
   
 





    }
}


