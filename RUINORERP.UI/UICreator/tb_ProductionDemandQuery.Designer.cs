
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作
    /// </summary>
    partial class tb_ProductionDemandQuery
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
     
     this.lblPDNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPDNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAnalysisDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpAnalysisDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPPNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPPNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPPID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPPID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblPurAllItems = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPurAllItems = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPurAllItems.Values.Text ="";

this.lblSuggestBasedOn = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSuggestBasedOn = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSuggestBasedOn.Values.Text ="";

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100PDNo###String
this.lblPDNo.AutoSize = true;
this.lblPDNo.Location = new System.Drawing.Point(100,25);
this.lblPDNo.Name = "lblPDNo";
this.lblPDNo.Size = new System.Drawing.Size(41, 12);
this.lblPDNo.TabIndex = 1;
this.lblPDNo.Text = "需要分析单号";
this.txtPDNo.Location = new System.Drawing.Point(173,21);
this.txtPDNo.Name = "txtPDNo";
this.txtPDNo.Size = new System.Drawing.Size(100, 21);
this.txtPDNo.TabIndex = 1;
this.Controls.Add(this.lblPDNo);
this.Controls.Add(this.txtPDNo);

           //#####AnalysisDate###DateTime
this.lblAnalysisDate.AutoSize = true;
this.lblAnalysisDate.Location = new System.Drawing.Point(100,50);
this.lblAnalysisDate.Name = "lblAnalysisDate";
this.lblAnalysisDate.Size = new System.Drawing.Size(41, 12);
this.lblAnalysisDate.TabIndex = 2;
this.lblAnalysisDate.Text = "分析日期";
//111======50
this.dtpAnalysisDate.Location = new System.Drawing.Point(173,46);
this.dtpAnalysisDate.Name ="dtpAnalysisDate";
this.dtpAnalysisDate.Size = new System.Drawing.Size(100, 21);
this.dtpAnalysisDate.TabIndex = 2;
this.Controls.Add(this.lblAnalysisDate);
this.Controls.Add(this.dtpAnalysisDate);

           //#####100PPNo###String
this.lblPPNo.AutoSize = true;
this.lblPPNo.Location = new System.Drawing.Point(100,75);
this.lblPPNo.Name = "lblPPNo";
this.lblPPNo.Size = new System.Drawing.Size(41, 12);
this.lblPPNo.TabIndex = 3;
this.lblPPNo.Text = "计划单号";
this.txtPPNo.Location = new System.Drawing.Point(173,71);
this.txtPPNo.Name = "txtPPNo";
this.txtPPNo.Size = new System.Drawing.Size(100, 21);
this.txtPPNo.TabIndex = 3;
this.Controls.Add(this.lblPPNo);
this.Controls.Add(this.txtPPNo);

           //#####PPID###Int64
//属性测试100PPID
//属性测试100PPID
this.lblPPID.AutoSize = true;
this.lblPPID.Location = new System.Drawing.Point(100,100);
this.lblPPID.Name = "lblPPID";
this.lblPPID.Size = new System.Drawing.Size(41, 12);
this.lblPPID.TabIndex = 4;
this.lblPPID.Text = "计划单号";
//111======100
this.cmbPPID.Location = new System.Drawing.Point(173,96);
this.cmbPPID.Name ="cmbPPID";
this.cmbPPID.Size = new System.Drawing.Size(100, 21);
this.cmbPPID.TabIndex = 4;
this.Controls.Add(this.lblPPID);
this.Controls.Add(this.cmbPPID);

           //#####Employee_ID###Int64
//属性测试125Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "经办人";
//111======125
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,121);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DataStatus###Int32
//属性测试150DataStatus
//属性测试150DataStatus

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,175);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 7;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,171);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 7;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,225);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 9;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,221);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 9;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,275);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 11;
this.lblCreated_at.Text = "创建时间";
//111======275
this.dtpCreated_at.Location = new System.Drawing.Point(173,271);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 11;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试300Created_by
//属性测试300Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试350Modified_by
//属性测试350Modified_by

           //#####Approver_by###Int64
//属性测试375Approver_by
//属性测试375Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,400);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 16;
this.lblApprover_at.Text = "审批时间";
//111======400
this.dtpApprover_at.Location = new System.Drawing.Point(173,396);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 16;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####PrintStatus###Int32
//属性测试425PrintStatus
//属性测试425PrintStatus

           //#####PurAllItems###Boolean
this.lblPurAllItems.AutoSize = true;
this.lblPurAllItems.Location = new System.Drawing.Point(100,450);
this.lblPurAllItems.Name = "lblPurAllItems";
this.lblPurAllItems.Size = new System.Drawing.Size(41, 12);
this.lblPurAllItems.TabIndex = 18;
this.lblPurAllItems.Text = "采购建议含全部物料";
this.chkPurAllItems.Location = new System.Drawing.Point(173,446);
this.chkPurAllItems.Name = "chkPurAllItems";
this.chkPurAllItems.Size = new System.Drawing.Size(100, 21);
this.chkPurAllItems.TabIndex = 18;
this.Controls.Add(this.lblPurAllItems);
this.Controls.Add(this.chkPurAllItems);

           //#####SuggestBasedOn###Boolean
this.lblSuggestBasedOn.AutoSize = true;
this.lblSuggestBasedOn.Location = new System.Drawing.Point(100,475);
this.lblSuggestBasedOn.Name = "lblSuggestBasedOn";
this.lblSuggestBasedOn.Size = new System.Drawing.Size(41, 12);
this.lblSuggestBasedOn.TabIndex = 19;
this.lblSuggestBasedOn.Text = "建议依据";
this.chkSuggestBasedOn.Location = new System.Drawing.Point(173,471);
this.chkSuggestBasedOn.Name = "chkSuggestBasedOn";
this.chkSuggestBasedOn.Size = new System.Drawing.Size(100, 21);
this.chkSuggestBasedOn.TabIndex = 19;
this.Controls.Add(this.lblSuggestBasedOn);
this.Controls.Add(this.chkSuggestBasedOn);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPDNo );
this.Controls.Add(this.txtPDNo );

                this.Controls.Add(this.lblAnalysisDate );
this.Controls.Add(this.dtpAnalysisDate );

                this.Controls.Add(this.lblPPNo );
this.Controls.Add(this.txtPPNo );

                this.Controls.Add(this.lblPPID );
this.Controls.Add(this.cmbPPID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblPurAllItems );
this.Controls.Add(this.chkPurAllItems );

                this.Controls.Add(this.lblSuggestBasedOn );
this.Controls.Add(this.chkSuggestBasedOn );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_ProductionDemandQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPDNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAnalysisDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpAnalysisDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPPNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPPNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPPID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPPID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurAllItems;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPurAllItems;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSuggestBasedOn;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSuggestBasedOn;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


