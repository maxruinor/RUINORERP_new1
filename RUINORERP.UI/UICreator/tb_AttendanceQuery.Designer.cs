
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 考勤表
    /// </summary>
    partial class tb_AttendanceQuery
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
     
     this.lblbadgenumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtbadgenumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblusername = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtusername = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbldeptname = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtdeptname = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblsDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsDate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblstime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtstime = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtstime.Multiline = true;

this.lbleDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpeDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblt1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpt1 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblt2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpt2 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblt3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpt3 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblt4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpt4 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50badgenumber###String
this.lblbadgenumber.AutoSize = true;
this.lblbadgenumber.Location = new System.Drawing.Point(100,25);
this.lblbadgenumber.Name = "lblbadgenumber";
this.lblbadgenumber.Size = new System.Drawing.Size(41, 12);
this.lblbadgenumber.TabIndex = 1;
this.lblbadgenumber.Text = "";
this.txtbadgenumber.Location = new System.Drawing.Point(173,21);
this.txtbadgenumber.Name = "txtbadgenumber";
this.txtbadgenumber.Size = new System.Drawing.Size(100, 21);
this.txtbadgenumber.TabIndex = 1;
this.Controls.Add(this.lblbadgenumber);
this.Controls.Add(this.txtbadgenumber);

           //#####50username###String
this.lblusername.AutoSize = true;
this.lblusername.Location = new System.Drawing.Point(100,50);
this.lblusername.Name = "lblusername";
this.lblusername.Size = new System.Drawing.Size(41, 12);
this.lblusername.TabIndex = 2;
this.lblusername.Text = "姓名";
this.txtusername.Location = new System.Drawing.Point(173,46);
this.txtusername.Name = "txtusername";
this.txtusername.Size = new System.Drawing.Size(100, 21);
this.txtusername.TabIndex = 2;
this.Controls.Add(this.lblusername);
this.Controls.Add(this.txtusername);

           //#####60deptname###String
this.lbldeptname.AutoSize = true;
this.lbldeptname.Location = new System.Drawing.Point(100,75);
this.lbldeptname.Name = "lbldeptname";
this.lbldeptname.Size = new System.Drawing.Size(41, 12);
this.lbldeptname.TabIndex = 3;
this.lbldeptname.Text = "部门";
this.txtdeptname.Location = new System.Drawing.Point(173,71);
this.txtdeptname.Name = "txtdeptname";
this.txtdeptname.Size = new System.Drawing.Size(100, 21);
this.txtdeptname.TabIndex = 3;
this.Controls.Add(this.lbldeptname);
this.Controls.Add(this.txtdeptname);

           //#####100sDate###String
this.lblsDate.AutoSize = true;
this.lblsDate.Location = new System.Drawing.Point(100,100);
this.lblsDate.Name = "lblsDate";
this.lblsDate.Size = new System.Drawing.Size(41, 12);
this.lblsDate.TabIndex = 4;
this.lblsDate.Text = "开始时间";
this.txtsDate.Location = new System.Drawing.Point(173,96);
this.txtsDate.Name = "txtsDate";
this.txtsDate.Size = new System.Drawing.Size(100, 21);
this.txtsDate.TabIndex = 4;
this.Controls.Add(this.lblsDate);
this.Controls.Add(this.txtsDate);

           //#####255stime###String
this.lblstime.AutoSize = true;
this.lblstime.Location = new System.Drawing.Point(100,125);
this.lblstime.Name = "lblstime";
this.lblstime.Size = new System.Drawing.Size(41, 12);
this.lblstime.TabIndex = 5;
this.lblstime.Text = "时间组";
this.txtstime.Location = new System.Drawing.Point(173,121);
this.txtstime.Name = "txtstime";
this.txtstime.Size = new System.Drawing.Size(100, 21);
this.txtstime.TabIndex = 5;
this.Controls.Add(this.lblstime);
this.Controls.Add(this.txtstime);

           //#####eDate###DateTime
this.lbleDate.AutoSize = true;
this.lbleDate.Location = new System.Drawing.Point(100,150);
this.lbleDate.Name = "lbleDate";
this.lbleDate.Size = new System.Drawing.Size(41, 12);
this.lbleDate.TabIndex = 6;
this.lbleDate.Text = "结束时间";
//111======150
this.dtpeDate.Location = new System.Drawing.Point(173,146);
this.dtpeDate.Name ="dtpeDate";
this.dtpeDate.ShowCheckBox =true;
this.dtpeDate.Size = new System.Drawing.Size(100, 21);
this.dtpeDate.TabIndex = 6;
this.Controls.Add(this.lbleDate);
this.Controls.Add(this.dtpeDate);

           //#####t1###DateTime
this.lblt1.AutoSize = true;
this.lblt1.Location = new System.Drawing.Point(100,175);
this.lblt1.Name = "lblt1";
this.lblt1.Size = new System.Drawing.Size(41, 12);
this.lblt1.TabIndex = 7;
this.lblt1.Text = "";
//111======175
this.dtpt1.Location = new System.Drawing.Point(173,171);
this.dtpt1.Name ="dtpt1";
this.dtpt1.ShowCheckBox =true;
this.dtpt1.Size = new System.Drawing.Size(100, 21);
this.dtpt1.TabIndex = 7;
this.Controls.Add(this.lblt1);
this.Controls.Add(this.dtpt1);

           //#####t2###DateTime
this.lblt2.AutoSize = true;
this.lblt2.Location = new System.Drawing.Point(100,200);
this.lblt2.Name = "lblt2";
this.lblt2.Size = new System.Drawing.Size(41, 12);
this.lblt2.TabIndex = 8;
this.lblt2.Text = "";
//111======200
this.dtpt2.Location = new System.Drawing.Point(173,196);
this.dtpt2.Name ="dtpt2";
this.dtpt2.ShowCheckBox =true;
this.dtpt2.Size = new System.Drawing.Size(100, 21);
this.dtpt2.TabIndex = 8;
this.Controls.Add(this.lblt2);
this.Controls.Add(this.dtpt2);

           //#####t3###DateTime
this.lblt3.AutoSize = true;
this.lblt3.Location = new System.Drawing.Point(100,225);
this.lblt3.Name = "lblt3";
this.lblt3.Size = new System.Drawing.Size(41, 12);
this.lblt3.TabIndex = 9;
this.lblt3.Text = "";
//111======225
this.dtpt3.Location = new System.Drawing.Point(173,221);
this.dtpt3.Name ="dtpt3";
this.dtpt3.ShowCheckBox =true;
this.dtpt3.Size = new System.Drawing.Size(100, 21);
this.dtpt3.TabIndex = 9;
this.Controls.Add(this.lblt3);
this.Controls.Add(this.dtpt3);

           //#####t4###DateTime
this.lblt4.AutoSize = true;
this.lblt4.Location = new System.Drawing.Point(100,250);
this.lblt4.Name = "lblt4";
this.lblt4.Size = new System.Drawing.Size(41, 12);
this.lblt4.TabIndex = 10;
this.lblt4.Text = "";
//111======250
this.dtpt4.Location = new System.Drawing.Point(173,246);
this.dtpt4.Name ="dtpt4";
this.dtpt4.ShowCheckBox =true;
this.dtpt4.Size = new System.Drawing.Size(100, 21);
this.dtpt4.TabIndex = 10;
this.Controls.Add(this.lblt4);
this.Controls.Add(this.dtpt4);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblbadgenumber );
this.Controls.Add(this.txtbadgenumber );

                this.Controls.Add(this.lblusername );
this.Controls.Add(this.txtusername );

                this.Controls.Add(this.lbldeptname );
this.Controls.Add(this.txtdeptname );

                this.Controls.Add(this.lblsDate );
this.Controls.Add(this.txtsDate );

                this.Controls.Add(this.lblstime );
this.Controls.Add(this.txtstime );

                this.Controls.Add(this.lbleDate );
this.Controls.Add(this.dtpeDate );

                this.Controls.Add(this.lblt1 );
this.Controls.Add(this.dtpt1 );

                this.Controls.Add(this.lblt2 );
this.Controls.Add(this.dtpt2 );

                this.Controls.Add(this.lblt3 );
this.Controls.Add(this.dtpt3 );

                this.Controls.Add(this.lblt4 );
this.Controls.Add(this.dtpt4 );

                    
            this.Name = "tb_AttendanceQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblbadgenumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtbadgenumber;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblusername;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtusername;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbldeptname;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtdeptname;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsDate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblstime;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtstime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbleDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpeDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblt1;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpt1;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblt2;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpt2;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblt3;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpt3;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblt4;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpt4;

    
    
   
 





    }
}


