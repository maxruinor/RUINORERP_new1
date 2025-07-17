
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/17/2025 16:59:39
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 蓄水登记表
    /// </summary>
    partial class tb_EOP_WaterStorageRegisterQuery
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
     
     this.lblWSRNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWSRNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPlatformFeeAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlatformFeeAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOrderDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOrderDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50WSRNo###String
this.lblWSRNo.AutoSize = true;
this.lblWSRNo.Location = new System.Drawing.Point(100,25);
this.lblWSRNo.Name = "lblWSRNo";
this.lblWSRNo.Size = new System.Drawing.Size(41, 12);
this.lblWSRNo.TabIndex = 1;
this.lblWSRNo.Text = "蓄水编号";
this.txtWSRNo.Location = new System.Drawing.Point(173,21);
this.txtWSRNo.Name = "txtWSRNo";
this.txtWSRNo.Size = new System.Drawing.Size(100, 21);
this.txtWSRNo.TabIndex = 1;
this.Controls.Add(this.lblWSRNo);
this.Controls.Add(this.txtWSRNo);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,50);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 2;
this.lblPlatformOrderNo.Text = "平台单号";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,46);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 2;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####PlatformType###Int32
//属性测试75PlatformType
//属性测试75PlatformType

           //#####Employee_ID###Int64
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "业务员";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####ProjectGroup_ID###Int64
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,125);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 5;
this.lblProjectGroup_ID.Text = "项目组";
//111======125
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,121);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 5;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,150);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 6;
this.lblTotalAmount.Text = "总金额";
//111======150
this.txtTotalAmount.Location = new System.Drawing.Point(173,146);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 6;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####PlatformFeeAmount###Decimal
this.lblPlatformFeeAmount.AutoSize = true;
this.lblPlatformFeeAmount.Location = new System.Drawing.Point(100,175);
this.lblPlatformFeeAmount.Name = "lblPlatformFeeAmount";
this.lblPlatformFeeAmount.Size = new System.Drawing.Size(41, 12);
this.lblPlatformFeeAmount.TabIndex = 7;
this.lblPlatformFeeAmount.Text = "平台费用";
//111======175
this.txtPlatformFeeAmount.Location = new System.Drawing.Point(173,171);
this.txtPlatformFeeAmount.Name ="txtPlatformFeeAmount";
this.txtPlatformFeeAmount.Size = new System.Drawing.Size(100, 21);
this.txtPlatformFeeAmount.TabIndex = 7;
this.Controls.Add(this.lblPlatformFeeAmount);
this.Controls.Add(this.txtPlatformFeeAmount);

           //#####OrderDate###DateTime
this.lblOrderDate.AutoSize = true;
this.lblOrderDate.Location = new System.Drawing.Point(100,200);
this.lblOrderDate.Name = "lblOrderDate";
this.lblOrderDate.Size = new System.Drawing.Size(41, 12);
this.lblOrderDate.TabIndex = 8;
this.lblOrderDate.Text = "订单日期";
//111======200
this.dtpOrderDate.Location = new System.Drawing.Point(173,196);
this.dtpOrderDate.Name ="dtpOrderDate";
this.dtpOrderDate.Size = new System.Drawing.Size(100, 21);
this.dtpOrderDate.TabIndex = 8;
this.Controls.Add(this.lblOrderDate);
this.Controls.Add(this.dtpOrderDate);

           //#####500ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,225);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 9;
this.lblShippingAddress.Text = "收货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,221);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 9;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,250);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 10;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,246);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 10;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,275);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 11;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,271);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 11;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

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
//属性测试375Modified_by

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,400);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 16;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,396);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 16;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,425);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 17;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,421);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 17;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblWSRNo );
this.Controls.Add(this.txtWSRNo );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblPlatformFeeAmount );
this.Controls.Add(this.txtPlatformFeeAmount );

                this.Controls.Add(this.lblOrderDate );
this.Controls.Add(this.dtpOrderDate );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_EOP_WaterStorageRegisterQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWSRNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWSRNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlatformFeeAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlatformFeeAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOrderDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOrderDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTrackNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


