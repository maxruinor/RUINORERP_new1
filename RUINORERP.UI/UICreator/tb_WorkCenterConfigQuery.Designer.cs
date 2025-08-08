
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 工作台配置表
    /// </summary>
    partial class tb_WorkCenterConfigQuery
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
     
     

this.lblOperable = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOperable = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOperable.Values.Text ="";

this.lblOnlyDisplay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOnlyDisplay = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOnlyDisplay.Values.Text ="";

this.lblToDoList = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtToDoList = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtToDoList.Multiline = true;

this.lblFrequentlyMenus = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFrequentlyMenus = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDataOverview = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDataOverview = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDataOverview.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####RoleID###Int64

           //#####User_ID###Int64

           //#####Operable###Boolean
this.lblOperable.AutoSize = true;
this.lblOperable.Location = new System.Drawing.Point(100,75);
this.lblOperable.Name = "lblOperable";
this.lblOperable.Size = new System.Drawing.Size(41, 12);
this.lblOperable.TabIndex = 3;
this.lblOperable.Text = "可操作";
this.chkOperable.Location = new System.Drawing.Point(173,71);
this.chkOperable.Name = "chkOperable";
this.chkOperable.Size = new System.Drawing.Size(100, 21);
this.chkOperable.TabIndex = 3;
this.Controls.Add(this.lblOperable);
this.Controls.Add(this.chkOperable);

           //#####OnlyDisplay###Boolean
this.lblOnlyDisplay.AutoSize = true;
this.lblOnlyDisplay.Location = new System.Drawing.Point(100,100);
this.lblOnlyDisplay.Name = "lblOnlyDisplay";
this.lblOnlyDisplay.Size = new System.Drawing.Size(41, 12);
this.lblOnlyDisplay.TabIndex = 4;
this.lblOnlyDisplay.Text = "仅展示";
this.chkOnlyDisplay.Location = new System.Drawing.Point(173,96);
this.chkOnlyDisplay.Name = "chkOnlyDisplay";
this.chkOnlyDisplay.Size = new System.Drawing.Size(100, 21);
this.chkOnlyDisplay.TabIndex = 4;
this.Controls.Add(this.lblOnlyDisplay);
this.Controls.Add(this.chkOnlyDisplay);

           //#####500ToDoList###String
this.lblToDoList.AutoSize = true;
this.lblToDoList.Location = new System.Drawing.Point(100,125);
this.lblToDoList.Name = "lblToDoList";
this.lblToDoList.Size = new System.Drawing.Size(41, 12);
this.lblToDoList.TabIndex = 5;
this.lblToDoList.Text = "待办事项";
this.txtToDoList.Location = new System.Drawing.Point(173,121);
this.txtToDoList.Name = "txtToDoList";
this.txtToDoList.Size = new System.Drawing.Size(100, 21);
this.txtToDoList.TabIndex = 5;
this.Controls.Add(this.lblToDoList);
this.Controls.Add(this.txtToDoList);

           //#####200FrequentlyMenus###String
this.lblFrequentlyMenus.AutoSize = true;
this.lblFrequentlyMenus.Location = new System.Drawing.Point(100,150);
this.lblFrequentlyMenus.Name = "lblFrequentlyMenus";
this.lblFrequentlyMenus.Size = new System.Drawing.Size(41, 12);
this.lblFrequentlyMenus.TabIndex = 6;
this.lblFrequentlyMenus.Text = "常用菜单";
this.txtFrequentlyMenus.Location = new System.Drawing.Point(173,146);
this.txtFrequentlyMenus.Name = "txtFrequentlyMenus";
this.txtFrequentlyMenus.Size = new System.Drawing.Size(100, 21);
this.txtFrequentlyMenus.TabIndex = 6;
this.Controls.Add(this.lblFrequentlyMenus);
this.Controls.Add(this.txtFrequentlyMenus);

           //#####500DataOverview###String
this.lblDataOverview.AutoSize = true;
this.lblDataOverview.Location = new System.Drawing.Point(100,175);
this.lblDataOverview.Name = "lblDataOverview";
this.lblDataOverview.Size = new System.Drawing.Size(41, 12);
this.lblDataOverview.TabIndex = 7;
this.lblDataOverview.Text = "数据概览";
this.txtDataOverview.Location = new System.Drawing.Point(173,171);
this.txtDataOverview.Name = "txtDataOverview";
this.txtDataOverview.Size = new System.Drawing.Size(100, 21);
this.txtDataOverview.TabIndex = 7;
this.Controls.Add(this.lblDataOverview);
this.Controls.Add(this.txtDataOverview);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblOperable );
this.Controls.Add(this.chkOperable );

                this.Controls.Add(this.lblOnlyDisplay );
this.Controls.Add(this.chkOnlyDisplay );

                this.Controls.Add(this.lblToDoList );
this.Controls.Add(this.txtToDoList );

                this.Controls.Add(this.lblFrequentlyMenus );
this.Controls.Add(this.txtFrequentlyMenus );

                this.Controls.Add(this.lblDataOverview );
this.Controls.Add(this.txtDataOverview );

                    
            this.Name = "tb_WorkCenterConfigQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOperable;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOperable;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOnlyDisplay;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOnlyDisplay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblToDoList;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtToDoList;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFrequentlyMenus;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFrequentlyMenus;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDataOverview;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDataOverview;

    
    
   
 





    }
}


