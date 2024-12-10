
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:36:34
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
    partial class tb_CRMConfigQuery
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
     
     this.lblCS_UseLeadsFunction = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkCS_UseLeadsFunction = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkCS_UseLeadsFunction.Values.Text ="";











this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####CS_SleepingCustomerDays###Int32

           //#####CS_LostCustomersDays###Int32

           //#####CS_ActiveCustomers###Int32

           //#####LS_ConvCustHasFollowUpDays###Int32

           //#####LS_ConvCustNoTransDays###Int32

           //#####LS_ConvCustLostDays###Int32

           //#####NoFollToPublicPoolDays###Int32

           //#####CustomerNoOrderDays###Int32

           //#####CustomerNoFollowUpDays###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCS_UseLeadsFunction );
this.Controls.Add(this.chkCS_UseLeadsFunction );

                
                
                
                
                
                
                
                
                
                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_CRMConfigQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCS_UseLeadsFunction;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkCS_UseLeadsFunction;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


