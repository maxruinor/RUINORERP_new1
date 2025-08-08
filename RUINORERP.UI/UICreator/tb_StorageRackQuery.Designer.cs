
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 货架信息表
    /// </summary>
    partial class tb_StorageRackQuery
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
     
     this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRackNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRackNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRackName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRackName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRackLocation = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRackLocation = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Location_ID###Int64
//属性测试25Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,25);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 1;
this.lblLocation_ID.Text = "所属仓库";
//111======25
this.cmbLocation_ID.Location = new System.Drawing.Point(173,21);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 1;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####50RackNO###String
this.lblRackNO.AutoSize = true;
this.lblRackNO.Location = new System.Drawing.Point(100,50);
this.lblRackNO.Name = "lblRackNO";
this.lblRackNO.Size = new System.Drawing.Size(41, 12);
this.lblRackNO.TabIndex = 2;
this.lblRackNO.Text = "货架编号";
this.txtRackNO.Location = new System.Drawing.Point(173,46);
this.txtRackNO.Name = "txtRackNO";
this.txtRackNO.Size = new System.Drawing.Size(100, 21);
this.txtRackNO.TabIndex = 2;
this.Controls.Add(this.lblRackNO);
this.Controls.Add(this.txtRackNO);

           //#####50RackName###String
this.lblRackName.AutoSize = true;
this.lblRackName.Location = new System.Drawing.Point(100,75);
this.lblRackName.Name = "lblRackName";
this.lblRackName.Size = new System.Drawing.Size(41, 12);
this.lblRackName.TabIndex = 3;
this.lblRackName.Text = "货架名称";
this.txtRackName.Location = new System.Drawing.Point(173,71);
this.txtRackName.Name = "txtRackName";
this.txtRackName.Size = new System.Drawing.Size(100, 21);
this.txtRackName.TabIndex = 3;
this.Controls.Add(this.lblRackName);
this.Controls.Add(this.txtRackName);

           //#####100RackLocation###String
this.lblRackLocation.AutoSize = true;
this.lblRackLocation.Location = new System.Drawing.Point(100,100);
this.lblRackLocation.Name = "lblRackLocation";
this.lblRackLocation.Size = new System.Drawing.Size(41, 12);
this.lblRackLocation.TabIndex = 4;
this.lblRackLocation.Text = "货架位置";
this.txtRackLocation.Location = new System.Drawing.Point(173,96);
this.txtRackLocation.Name = "txtRackLocation";
this.txtRackLocation.Size = new System.Drawing.Size(100, 21);
this.txtRackLocation.TabIndex = 4;
this.Controls.Add(this.lblRackLocation);
this.Controls.Add(this.txtRackLocation);

           //#####100Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,125);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 5;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,121);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 5;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRackNO );
this.Controls.Add(this.txtRackNO );

                this.Controls.Add(this.lblRackName );
this.Controls.Add(this.txtRackName );

                this.Controls.Add(this.lblRackLocation );
this.Controls.Add(this.txtRackLocation );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                    
            this.Name = "tb_StorageRackQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRackNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRackNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRackName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRackName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRackLocation;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRackLocation;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDesc;

    
    
   
 





    }
}


