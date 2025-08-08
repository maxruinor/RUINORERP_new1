// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 客户关系配置表
    /// </summary>
    partial class tb_CRMConfigEdit
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
     this.lblCS_UseLeadsFunction = new Krypton.Toolkit.KryptonLabel();
this.chkCS_UseLeadsFunction = new Krypton.Toolkit.KryptonCheckBox();
this.chkCS_UseLeadsFunction.Values.Text ="";

this.lblCS_NewCustToLeadsCustDays = new Krypton.Toolkit.KryptonLabel();
this.txtCS_NewCustToLeadsCustDays = new Krypton.Toolkit.KryptonTextBox();

this.lblCS_SleepingCustomerDays = new Krypton.Toolkit.KryptonLabel();
this.txtCS_SleepingCustomerDays = new Krypton.Toolkit.KryptonTextBox();

this.lblCS_LostCustomersDays = new Krypton.Toolkit.KryptonLabel();
this.txtCS_LostCustomersDays = new Krypton.Toolkit.KryptonTextBox();

this.lblCS_ActiveCustomers = new Krypton.Toolkit.KryptonLabel();
this.txtCS_ActiveCustomers = new Krypton.Toolkit.KryptonTextBox();

this.lblLS_ConvCustHasFollowUpDays = new Krypton.Toolkit.KryptonLabel();
this.txtLS_ConvCustHasFollowUpDays = new Krypton.Toolkit.KryptonTextBox();

this.lblLS_ConvCustNoTransDays = new Krypton.Toolkit.KryptonLabel();
this.txtLS_ConvCustNoTransDays = new Krypton.Toolkit.KryptonTextBox();

this.lblLS_ConvCustLostDays = new Krypton.Toolkit.KryptonLabel();
this.txtLS_ConvCustLostDays = new Krypton.Toolkit.KryptonTextBox();

this.lblNoFollToPublicPoolDays = new Krypton.Toolkit.KryptonLabel();
this.txtNoFollToPublicPoolDays = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerNoOrderDays = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerNoOrderDays = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerNoFollowUpDays = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerNoFollowUpDays = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####CS_UseLeadsFunction###Boolean
this.lblCS_UseLeadsFunction.AutoSize = true;
this.lblCS_UseLeadsFunction.Location = new System.Drawing.Point(100,25);
this.lblCS_UseLeadsFunction.Name = "lblCS_UseLeadsFunction";
this.lblCS_UseLeadsFunction.Size = new System.Drawing.Size(41, 12);
this.lblCS_UseLeadsFunction.TabIndex = 1;
this.lblCS_UseLeadsFunction.Text = "是否使用线索功能";
this.chkCS_UseLeadsFunction.Location = new System.Drawing.Point(173,21);
this.chkCS_UseLeadsFunction.Name = "chkCS_UseLeadsFunction";
this.chkCS_UseLeadsFunction.Size = new System.Drawing.Size(100, 21);
this.chkCS_UseLeadsFunction.TabIndex = 1;
this.Controls.Add(this.lblCS_UseLeadsFunction);
this.Controls.Add(this.chkCS_UseLeadsFunction);

           //#####CS_NewCustToLeadsCustDays###Int32
this.lblCS_NewCustToLeadsCustDays.AutoSize = true;
this.lblCS_NewCustToLeadsCustDays.Location = new System.Drawing.Point(100,50);
this.lblCS_NewCustToLeadsCustDays.Name = "lblCS_NewCustToLeadsCustDays";
this.lblCS_NewCustToLeadsCustDays.Size = new System.Drawing.Size(41, 12);
this.lblCS_NewCustToLeadsCustDays.TabIndex = 2;
this.lblCS_NewCustToLeadsCustDays.Text = "新客转潜客天数";
this.txtCS_NewCustToLeadsCustDays.Location = new System.Drawing.Point(173,46);
this.txtCS_NewCustToLeadsCustDays.Name = "txtCS_NewCustToLeadsCustDays";
this.txtCS_NewCustToLeadsCustDays.Size = new System.Drawing.Size(100, 21);
this.txtCS_NewCustToLeadsCustDays.TabIndex = 2;
this.Controls.Add(this.lblCS_NewCustToLeadsCustDays);
this.Controls.Add(this.txtCS_NewCustToLeadsCustDays);

           //#####CS_SleepingCustomerDays###Int32
this.lblCS_SleepingCustomerDays.AutoSize = true;
this.lblCS_SleepingCustomerDays.Location = new System.Drawing.Point(100,75);
this.lblCS_SleepingCustomerDays.Name = "lblCS_SleepingCustomerDays";
this.lblCS_SleepingCustomerDays.Size = new System.Drawing.Size(41, 12);
this.lblCS_SleepingCustomerDays.TabIndex = 3;
this.lblCS_SleepingCustomerDays.Text = "定义休眠客户天数";
this.txtCS_SleepingCustomerDays.Location = new System.Drawing.Point(173,71);
this.txtCS_SleepingCustomerDays.Name = "txtCS_SleepingCustomerDays";
this.txtCS_SleepingCustomerDays.Size = new System.Drawing.Size(100, 21);
this.txtCS_SleepingCustomerDays.TabIndex = 3;
this.Controls.Add(this.lblCS_SleepingCustomerDays);
this.Controls.Add(this.txtCS_SleepingCustomerDays);

           //#####CS_LostCustomersDays###Int32
this.lblCS_LostCustomersDays.AutoSize = true;
this.lblCS_LostCustomersDays.Location = new System.Drawing.Point(100,100);
this.lblCS_LostCustomersDays.Name = "lblCS_LostCustomersDays";
this.lblCS_LostCustomersDays.Size = new System.Drawing.Size(41, 12);
this.lblCS_LostCustomersDays.TabIndex = 4;
this.lblCS_LostCustomersDays.Text = "定义流失客户天数";
this.txtCS_LostCustomersDays.Location = new System.Drawing.Point(173,96);
this.txtCS_LostCustomersDays.Name = "txtCS_LostCustomersDays";
this.txtCS_LostCustomersDays.Size = new System.Drawing.Size(100, 21);
this.txtCS_LostCustomersDays.TabIndex = 4;
this.Controls.Add(this.lblCS_LostCustomersDays);
this.Controls.Add(this.txtCS_LostCustomersDays);

           //#####CS_ActiveCustomers###Int32
this.lblCS_ActiveCustomers.AutoSize = true;
this.lblCS_ActiveCustomers.Location = new System.Drawing.Point(100,125);
this.lblCS_ActiveCustomers.Name = "lblCS_ActiveCustomers";
this.lblCS_ActiveCustomers.Size = new System.Drawing.Size(41, 12);
this.lblCS_ActiveCustomers.TabIndex = 5;
this.lblCS_ActiveCustomers.Text = "定义活跃客户天数";
this.txtCS_ActiveCustomers.Location = new System.Drawing.Point(173,121);
this.txtCS_ActiveCustomers.Name = "txtCS_ActiveCustomers";
this.txtCS_ActiveCustomers.Size = new System.Drawing.Size(100, 21);
this.txtCS_ActiveCustomers.TabIndex = 5;
this.Controls.Add(this.lblCS_ActiveCustomers);
this.Controls.Add(this.txtCS_ActiveCustomers);

           //#####LS_ConvCustHasFollowUpDays###Int32
this.lblLS_ConvCustHasFollowUpDays.AutoSize = true;
this.lblLS_ConvCustHasFollowUpDays.Location = new System.Drawing.Point(100,150);
this.lblLS_ConvCustHasFollowUpDays.Name = "lblLS_ConvCustHasFollowUpDays";
this.lblLS_ConvCustHasFollowUpDays.Size = new System.Drawing.Size(41, 12);
this.lblLS_ConvCustHasFollowUpDays.TabIndex = 6;
this.lblLS_ConvCustHasFollowUpDays.Text = "转换为客户后有跟进天数";
this.txtLS_ConvCustHasFollowUpDays.Location = new System.Drawing.Point(173,146);
this.txtLS_ConvCustHasFollowUpDays.Name = "txtLS_ConvCustHasFollowUpDays";
this.txtLS_ConvCustHasFollowUpDays.Size = new System.Drawing.Size(100, 21);
this.txtLS_ConvCustHasFollowUpDays.TabIndex = 6;
this.Controls.Add(this.lblLS_ConvCustHasFollowUpDays);
this.Controls.Add(this.txtLS_ConvCustHasFollowUpDays);

           //#####LS_ConvCustNoTransDays###Int32
this.lblLS_ConvCustNoTransDays.AutoSize = true;
this.lblLS_ConvCustNoTransDays.Location = new System.Drawing.Point(100,175);
this.lblLS_ConvCustNoTransDays.Name = "lblLS_ConvCustNoTransDays";
this.lblLS_ConvCustNoTransDays.Size = new System.Drawing.Size(41, 12);
this.lblLS_ConvCustNoTransDays.TabIndex = 7;
this.lblLS_ConvCustNoTransDays.Text = "转换为客户后无成交天数";
this.txtLS_ConvCustNoTransDays.Location = new System.Drawing.Point(173,171);
this.txtLS_ConvCustNoTransDays.Name = "txtLS_ConvCustNoTransDays";
this.txtLS_ConvCustNoTransDays.Size = new System.Drawing.Size(100, 21);
this.txtLS_ConvCustNoTransDays.TabIndex = 7;
this.Controls.Add(this.lblLS_ConvCustNoTransDays);
this.Controls.Add(this.txtLS_ConvCustNoTransDays);

           //#####LS_ConvCustLostDays###Int32
this.lblLS_ConvCustLostDays.AutoSize = true;
this.lblLS_ConvCustLostDays.Location = new System.Drawing.Point(100,200);
this.lblLS_ConvCustLostDays.Name = "lblLS_ConvCustLostDays";
this.lblLS_ConvCustLostDays.Size = new System.Drawing.Size(41, 12);
this.lblLS_ConvCustLostDays.TabIndex = 8;
this.lblLS_ConvCustLostDays.Text = "转换为客户后已丢失天数";
this.txtLS_ConvCustLostDays.Location = new System.Drawing.Point(173,196);
this.txtLS_ConvCustLostDays.Name = "txtLS_ConvCustLostDays";
this.txtLS_ConvCustLostDays.Size = new System.Drawing.Size(100, 21);
this.txtLS_ConvCustLostDays.TabIndex = 8;
this.Controls.Add(this.lblLS_ConvCustLostDays);
this.Controls.Add(this.txtLS_ConvCustLostDays);

           //#####NoFollToPublicPoolDays###Int32
this.lblNoFollToPublicPoolDays.AutoSize = true;
this.lblNoFollToPublicPoolDays.Location = new System.Drawing.Point(100,225);
this.lblNoFollToPublicPoolDays.Name = "lblNoFollToPublicPoolDays";
this.lblNoFollToPublicPoolDays.Size = new System.Drawing.Size(41, 12);
this.lblNoFollToPublicPoolDays.TabIndex = 9;
this.lblNoFollToPublicPoolDays.Text = "无跟进转换到公海的天数";
this.txtNoFollToPublicPoolDays.Location = new System.Drawing.Point(173,221);
this.txtNoFollToPublicPoolDays.Name = "txtNoFollToPublicPoolDays";
this.txtNoFollToPublicPoolDays.Size = new System.Drawing.Size(100, 21);
this.txtNoFollToPublicPoolDays.TabIndex = 9;
this.Controls.Add(this.lblNoFollToPublicPoolDays);
this.Controls.Add(this.txtNoFollToPublicPoolDays);

           //#####CustomerNoOrderDays###Int32
this.lblCustomerNoOrderDays.AutoSize = true;
this.lblCustomerNoOrderDays.Location = new System.Drawing.Point(100,250);
this.lblCustomerNoOrderDays.Name = "lblCustomerNoOrderDays";
this.lblCustomerNoOrderDays.Size = new System.Drawing.Size(41, 12);
this.lblCustomerNoOrderDays.TabIndex = 10;
this.lblCustomerNoOrderDays.Text = "客户无返单间隔提醒天数";
this.txtCustomerNoOrderDays.Location = new System.Drawing.Point(173,246);
this.txtCustomerNoOrderDays.Name = "txtCustomerNoOrderDays";
this.txtCustomerNoOrderDays.Size = new System.Drawing.Size(100, 21);
this.txtCustomerNoOrderDays.TabIndex = 10;
this.Controls.Add(this.lblCustomerNoOrderDays);
this.Controls.Add(this.txtCustomerNoOrderDays);

           //#####CustomerNoFollowUpDays###Int32
this.lblCustomerNoFollowUpDays.AutoSize = true;
this.lblCustomerNoFollowUpDays.Location = new System.Drawing.Point(100,275);
this.lblCustomerNoFollowUpDays.Name = "lblCustomerNoFollowUpDays";
this.lblCustomerNoFollowUpDays.Size = new System.Drawing.Size(41, 12);
this.lblCustomerNoFollowUpDays.TabIndex = 11;
this.lblCustomerNoFollowUpDays.Text = "客户无回访间隔提醒天数";
this.txtCustomerNoFollowUpDays.Location = new System.Drawing.Point(173,271);
this.txtCustomerNoFollowUpDays.Name = "txtCustomerNoFollowUpDays";
this.txtCustomerNoFollowUpDays.Size = new System.Drawing.Size(100, 21);
this.txtCustomerNoFollowUpDays.TabIndex = 11;
this.Controls.Add(this.lblCustomerNoFollowUpDays);
this.Controls.Add(this.txtCustomerNoFollowUpDays);

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
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 12;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,325);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 13;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,321);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 13;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,375);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 15;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,371);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 15;
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
           // this.kryptonPanel1.TabIndex = 15;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCS_UseLeadsFunction );
this.Controls.Add(this.chkCS_UseLeadsFunction );

                this.Controls.Add(this.lblCS_NewCustToLeadsCustDays );
this.Controls.Add(this.txtCS_NewCustToLeadsCustDays );

                this.Controls.Add(this.lblCS_SleepingCustomerDays );
this.Controls.Add(this.txtCS_SleepingCustomerDays );

                this.Controls.Add(this.lblCS_LostCustomersDays );
this.Controls.Add(this.txtCS_LostCustomersDays );

                this.Controls.Add(this.lblCS_ActiveCustomers );
this.Controls.Add(this.txtCS_ActiveCustomers );

                this.Controls.Add(this.lblLS_ConvCustHasFollowUpDays );
this.Controls.Add(this.txtLS_ConvCustHasFollowUpDays );

                this.Controls.Add(this.lblLS_ConvCustNoTransDays );
this.Controls.Add(this.txtLS_ConvCustNoTransDays );

                this.Controls.Add(this.lblLS_ConvCustLostDays );
this.Controls.Add(this.txtLS_ConvCustLostDays );

                this.Controls.Add(this.lblNoFollToPublicPoolDays );
this.Controls.Add(this.txtNoFollToPublicPoolDays );

                this.Controls.Add(this.lblCustomerNoOrderDays );
this.Controls.Add(this.txtCustomerNoOrderDays );

                this.Controls.Add(this.lblCustomerNoFollowUpDays );
this.Controls.Add(this.txtCustomerNoFollowUpDays );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_CRMConfigEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRMConfigEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCS_UseLeadsFunction;
private Krypton.Toolkit.KryptonCheckBox chkCS_UseLeadsFunction;

    
        
              private Krypton.Toolkit.KryptonLabel lblCS_NewCustToLeadsCustDays;
private Krypton.Toolkit.KryptonTextBox txtCS_NewCustToLeadsCustDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblCS_SleepingCustomerDays;
private Krypton.Toolkit.KryptonTextBox txtCS_SleepingCustomerDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblCS_LostCustomersDays;
private Krypton.Toolkit.KryptonTextBox txtCS_LostCustomersDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblCS_ActiveCustomers;
private Krypton.Toolkit.KryptonTextBox txtCS_ActiveCustomers;

    
        
              private Krypton.Toolkit.KryptonLabel lblLS_ConvCustHasFollowUpDays;
private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustHasFollowUpDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblLS_ConvCustNoTransDays;
private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustNoTransDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblLS_ConvCustLostDays;
private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustLostDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblNoFollToPublicPoolDays;
private Krypton.Toolkit.KryptonTextBox txtNoFollToPublicPoolDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerNoOrderDays;
private Krypton.Toolkit.KryptonTextBox txtCustomerNoOrderDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerNoFollowUpDays;
private Krypton.Toolkit.KryptonTextBox txtCustomerNoFollowUpDays;

    
        
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

