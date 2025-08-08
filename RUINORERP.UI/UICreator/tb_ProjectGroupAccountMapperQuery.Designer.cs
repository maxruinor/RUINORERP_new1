
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    partial class tb_ProjectGroupAccountMapperQuery
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
     
     this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpiryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpiryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProjectGroup_ID###Int64
//属性测试25ProjectGroup_ID
//属性测试25ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,25);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 1;
this.lblProjectGroup_ID.Text = "项目组";
//111======25
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,21);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 1;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Account_id###Int64
//属性测试50Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,50);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 2;
this.lblAccount_id.Text = "公司账户";
//111======50
this.cmbAccount_id.Location = new System.Drawing.Point(173,46);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 2;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####50Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,75);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 3;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,71);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 3;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####EffectiveDate###DateTime
this.lblEffectiveDate.AutoSize = true;
this.lblEffectiveDate.Location = new System.Drawing.Point(100,100);
this.lblEffectiveDate.Name = "lblEffectiveDate";
this.lblEffectiveDate.Size = new System.Drawing.Size(41, 12);
this.lblEffectiveDate.TabIndex = 4;
this.lblEffectiveDate.Text = "生效日期";
//111======100
this.dtpEffectiveDate.Location = new System.Drawing.Point(173,96);
this.dtpEffectiveDate.Name ="dtpEffectiveDate";
this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
this.dtpEffectiveDate.TabIndex = 4;
this.Controls.Add(this.lblEffectiveDate);
this.Controls.Add(this.dtpEffectiveDate);

           //#####ExpiryDate###DateTime
this.lblExpiryDate.AutoSize = true;
this.lblExpiryDate.Location = new System.Drawing.Point(100,125);
this.lblExpiryDate.Name = "lblExpiryDate";
this.lblExpiryDate.Size = new System.Drawing.Size(41, 12);
this.lblExpiryDate.TabIndex = 5;
this.lblExpiryDate.Text = "失效日期";
//111======125
this.dtpExpiryDate.Location = new System.Drawing.Point(173,121);
this.dtpExpiryDate.Name ="dtpExpiryDate";
this.dtpExpiryDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpiryDate.TabIndex = 5;
this.Controls.Add(this.lblExpiryDate);
this.Controls.Add(this.dtpExpiryDate);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblEffectiveDate );
this.Controls.Add(this.dtpEffectiveDate );

                this.Controls.Add(this.lblExpiryDate );
this.Controls.Add(this.dtpExpiryDate );

                    
            this.Name = "tb_ProjectGroupAccountMapperQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpiryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpiryDate;

    
    
   
 





    }
}


