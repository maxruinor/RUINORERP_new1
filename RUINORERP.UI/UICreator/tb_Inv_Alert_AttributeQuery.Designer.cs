
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 存货预警特性表
    /// </summary>
    partial class tb_Inv_Alert_AttributeQuery
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
     
     this.lblInventory_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbInventory_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();




this.lblAlert_Activation = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkAlert_Activation = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkAlert_Activation.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Inventory_ID###Int64
//属性测试25Inventory_ID
this.lblInventory_ID.AutoSize = true;
this.lblInventory_ID.Location = new System.Drawing.Point(100,25);
this.lblInventory_ID.Name = "lblInventory_ID";
this.lblInventory_ID.Size = new System.Drawing.Size(41, 12);
this.lblInventory_ID.TabIndex = 1;
this.lblInventory_ID.Text = "库存";
//111======25
this.cmbInventory_ID.Location = new System.Drawing.Point(173,21);
this.cmbInventory_ID.Name ="cmbInventory_ID";
this.cmbInventory_ID.Size = new System.Drawing.Size(100, 21);
this.cmbInventory_ID.TabIndex = 1;
this.Controls.Add(this.lblInventory_ID);
this.Controls.Add(this.cmbInventory_ID);

           //#####AlertPeriod###Int32
//属性测试50AlertPeriod

           //#####Max_quantity###Int32
//属性测试75Max_quantity

           //#####Min_quantity###Int32
//属性测试100Min_quantity

           //#####Alert_Activation###Boolean
this.lblAlert_Activation.AutoSize = true;
this.lblAlert_Activation.Location = new System.Drawing.Point(100,125);
this.lblAlert_Activation.Name = "lblAlert_Activation";
this.lblAlert_Activation.Size = new System.Drawing.Size(41, 12);
this.lblAlert_Activation.TabIndex = 5;
this.lblAlert_Activation.Text = "预警激活";
this.chkAlert_Activation.Location = new System.Drawing.Point(173,121);
this.chkAlert_Activation.Name = "chkAlert_Activation";
this.chkAlert_Activation.Size = new System.Drawing.Size(100, 21);
this.chkAlert_Activation.TabIndex = 5;
this.Controls.Add(this.lblAlert_Activation);
this.Controls.Add(this.chkAlert_Activation);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "创建时间";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试175Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,200);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 8;
this.lblModified_at.Text = "修改时间";
//111======200
this.dtpModified_at.Location = new System.Drawing.Point(173,196);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 8;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试225Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblInventory_ID );
this.Controls.Add(this.cmbInventory_ID );

                
                
                
                this.Controls.Add(this.lblAlert_Activation );
this.Controls.Add(this.chkAlert_Activation );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_Inv_Alert_AttributeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInventory_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbInventory_ID;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAlert_Activation;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkAlert_Activation;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


