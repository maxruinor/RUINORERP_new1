
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:28
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收付款单明细统计
    /// </summary>
    partial class View_FM_PaymentRecordItemsQuery
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
     
     
this.lblPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();






this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();




this.lblPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPaymentImagePath.Multiline = true;

this.lblReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReferenceNo.Multiline = true;

this.lblIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsReversed.Values.Text ="";


this.lblReversedOriginalNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReversedOriginalNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblReversedByPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReversedByPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PaymentId###Int64

           //#####30PaymentNo###String
this.lblPaymentNo.AutoSize = true;
this.lblPaymentNo.Location = new System.Drawing.Point(100,50);
this.lblPaymentNo.Name = "lblPaymentNo";
this.lblPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblPaymentNo.TabIndex = 2;
this.lblPaymentNo.Text = "";
this.txtPaymentNo.Location = new System.Drawing.Point(173,46);
this.txtPaymentNo.Name = "txtPaymentNo";
this.txtPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtPaymentNo.TabIndex = 2;
this.Controls.Add(this.lblPaymentNo);
this.Controls.Add(this.txtPaymentNo);

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,75);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 3;
this.lblSourceBillNo.Text = "";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,71);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 3;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####SourceBizType###Int32

           //#####ReceivePaymentType###Int32

           //#####Account_id###Int64

           //#####CustomerVendor_ID###Int64

           //#####PayeeInfoID###Int64

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,225);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 9;
this.lblPayeeAccountNo.Text = "";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,221);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 9;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####Currency_ID###Int64

           //#####TotalForeignAmount###Decimal
this.lblTotalForeignAmount.AutoSize = true;
this.lblTotalForeignAmount.Location = new System.Drawing.Point(100,275);
this.lblTotalForeignAmount.Name = "lblTotalForeignAmount";
this.lblTotalForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignAmount.TabIndex = 11;
this.lblTotalForeignAmount.Text = "";
//111======275
this.txtTotalForeignAmount.Location = new System.Drawing.Point(173,271);
this.txtTotalForeignAmount.Name ="txtTotalForeignAmount";
this.txtTotalForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignAmount.TabIndex = 11;
this.Controls.Add(this.lblTotalForeignAmount);
this.Controls.Add(this.txtTotalForeignAmount);

           //#####TotalLocalAmount###Decimal
this.lblTotalLocalAmount.AutoSize = true;
this.lblTotalLocalAmount.Location = new System.Drawing.Point(100,300);
this.lblTotalLocalAmount.Name = "lblTotalLocalAmount";
this.lblTotalLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalAmount.TabIndex = 12;
this.lblTotalLocalAmount.Text = "";
//111======300
this.txtTotalLocalAmount.Location = new System.Drawing.Point(173,296);
this.txtTotalLocalAmount.Name ="txtTotalLocalAmount";
this.txtTotalLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalAmount.TabIndex = 12;
this.Controls.Add(this.lblTotalLocalAmount);
this.Controls.Add(this.txtTotalLocalAmount);

           //#####PaymentDate###DateTime
this.lblPaymentDate.AutoSize = true;
this.lblPaymentDate.Location = new System.Drawing.Point(100,325);
this.lblPaymentDate.Name = "lblPaymentDate";
this.lblPaymentDate.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDate.TabIndex = 13;
this.lblPaymentDate.Text = "";
//111======325
this.dtpPaymentDate.Location = new System.Drawing.Point(173,321);
this.dtpPaymentDate.Name ="dtpPaymentDate";
this.dtpPaymentDate.ShowCheckBox =true;
this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
this.dtpPaymentDate.TabIndex = 13;
this.Controls.Add(this.lblPaymentDate);
this.Controls.Add(this.dtpPaymentDate);

           //#####Employee_ID###Int64

           //#####Paytype_ID###Int64

           //#####PaymentStatus###Int32

           //#####300PaymentImagePath###String
this.lblPaymentImagePath.AutoSize = true;
this.lblPaymentImagePath.Location = new System.Drawing.Point(100,425);
this.lblPaymentImagePath.Name = "lblPaymentImagePath";
this.lblPaymentImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentImagePath.TabIndex = 17;
this.lblPaymentImagePath.Text = "";
this.txtPaymentImagePath.Location = new System.Drawing.Point(173,421);
this.txtPaymentImagePath.Name = "txtPaymentImagePath";
this.txtPaymentImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentImagePath.TabIndex = 17;
this.Controls.Add(this.lblPaymentImagePath);
this.Controls.Add(this.txtPaymentImagePath);

           //#####300ReferenceNo###String
this.lblReferenceNo.AutoSize = true;
this.lblReferenceNo.Location = new System.Drawing.Point(100,450);
this.lblReferenceNo.Name = "lblReferenceNo";
this.lblReferenceNo.Size = new System.Drawing.Size(41, 12);
this.lblReferenceNo.TabIndex = 18;
this.lblReferenceNo.Text = "";
this.txtReferenceNo.Location = new System.Drawing.Point(173,446);
this.txtReferenceNo.Name = "txtReferenceNo";
this.txtReferenceNo.Size = new System.Drawing.Size(100, 21);
this.txtReferenceNo.TabIndex = 18;
this.Controls.Add(this.lblReferenceNo);
this.Controls.Add(this.txtReferenceNo);

           //#####IsReversed###Boolean
this.lblIsReversed.AutoSize = true;
this.lblIsReversed.Location = new System.Drawing.Point(100,475);
this.lblIsReversed.Name = "lblIsReversed";
this.lblIsReversed.Size = new System.Drawing.Size(41, 12);
this.lblIsReversed.TabIndex = 19;
this.lblIsReversed.Text = "";
this.chkIsReversed.Location = new System.Drawing.Point(173,471);
this.chkIsReversed.Name = "chkIsReversed";
this.chkIsReversed.Size = new System.Drawing.Size(100, 21);
this.chkIsReversed.TabIndex = 19;
this.Controls.Add(this.lblIsReversed);
this.Controls.Add(this.chkIsReversed);

           //#####ReversedOriginalId###Int64

           //#####30ReversedOriginalNo###String
this.lblReversedOriginalNo.AutoSize = true;
this.lblReversedOriginalNo.Location = new System.Drawing.Point(100,525);
this.lblReversedOriginalNo.Name = "lblReversedOriginalNo";
this.lblReversedOriginalNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedOriginalNo.TabIndex = 21;
this.lblReversedOriginalNo.Text = "";
this.txtReversedOriginalNo.Location = new System.Drawing.Point(173,521);
this.txtReversedOriginalNo.Name = "txtReversedOriginalNo";
this.txtReversedOriginalNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedOriginalNo.TabIndex = 21;
this.Controls.Add(this.lblReversedOriginalNo);
this.Controls.Add(this.txtReversedOriginalNo);

           //#####ReversedByPaymentId###Int64

           //#####30ReversedByPaymentNo###String
this.lblReversedByPaymentNo.AutoSize = true;
this.lblReversedByPaymentNo.Location = new System.Drawing.Point(100,575);
this.lblReversedByPaymentNo.Name = "lblReversedByPaymentNo";
this.lblReversedByPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedByPaymentNo.TabIndex = 23;
this.lblReversedByPaymentNo.Text = "";
this.txtReversedByPaymentNo.Location = new System.Drawing.Point(173,571);
this.txtReversedByPaymentNo.Name = "txtReversedByPaymentNo";
this.txtReversedByPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedByPaymentNo.TabIndex = 23;
this.Controls.Add(this.lblReversedByPaymentNo);
this.Controls.Add(this.txtReversedByPaymentNo);

           //#####PaymentDetailId###Int64

           //#####DepartmentID###Int64

           //#####ProjectGroup_ID###Int64

           //#####ForeignAmount###Decimal
this.lblForeignAmount.AutoSize = true;
this.lblForeignAmount.Location = new System.Drawing.Point(100,675);
this.lblForeignAmount.Name = "lblForeignAmount";
this.lblForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignAmount.TabIndex = 27;
this.lblForeignAmount.Text = "";
//111======675
this.txtForeignAmount.Location = new System.Drawing.Point(173,671);
this.txtForeignAmount.Name ="txtForeignAmount";
this.txtForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignAmount.TabIndex = 27;
this.Controls.Add(this.lblForeignAmount);
this.Controls.Add(this.txtForeignAmount);

           //#####LocalAmount###Decimal
this.lblLocalAmount.AutoSize = true;
this.lblLocalAmount.Location = new System.Drawing.Point(100,700);
this.lblLocalAmount.Name = "lblLocalAmount";
this.lblLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalAmount.TabIndex = 28;
this.lblLocalAmount.Text = "";
//111======700
this.txtLocalAmount.Location = new System.Drawing.Point(173,696);
this.txtLocalAmount.Name ="txtLocalAmount";
this.txtLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalAmount.TabIndex = 28;
this.Controls.Add(this.lblLocalAmount);
this.Controls.Add(this.txtLocalAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,725);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 29;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,721);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 29;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,750);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 30;
this.lblRemark.Text = "";
this.txtRemark.Location = new System.Drawing.Point(173,746);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 30;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,775);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 31;
this.lblCreated_at.Text = "";
//111======775
this.dtpCreated_at.Location = new System.Drawing.Point(173,771);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 31;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,825);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 33;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,821);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 33;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblPaymentNo );
this.Controls.Add(this.txtPaymentNo );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                
                
                
                
                
                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                
                this.Controls.Add(this.lblTotalForeignAmount );
this.Controls.Add(this.txtTotalForeignAmount );

                this.Controls.Add(this.lblTotalLocalAmount );
this.Controls.Add(this.txtTotalLocalAmount );

                this.Controls.Add(this.lblPaymentDate );
this.Controls.Add(this.dtpPaymentDate );

                
                
                
                this.Controls.Add(this.lblPaymentImagePath );
this.Controls.Add(this.txtPaymentImagePath );

                this.Controls.Add(this.lblReferenceNo );
this.Controls.Add(this.txtReferenceNo );

                this.Controls.Add(this.lblIsReversed );
this.Controls.Add(this.chkIsReversed );

                
                this.Controls.Add(this.lblReversedOriginalNo );
this.Controls.Add(this.txtReversedOriginalNo );

                
                this.Controls.Add(this.lblReversedByPaymentNo );
this.Controls.Add(this.txtReversedByPaymentNo );

                
                
                
                this.Controls.Add(this.lblForeignAmount );
this.Controls.Add(this.txtForeignAmount );

                this.Controls.Add(this.lblLocalAmount );
this.Controls.Add(this.txtLocalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "View_FM_PaymentRecordItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaymentNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaymentImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReferenceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReferenceNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsReversed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsReversed;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReversedOriginalNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReversedOriginalNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReversedByPaymentNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReversedByPaymentNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


