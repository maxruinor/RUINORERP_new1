// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
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
    partial class tb_AttendanceEdit
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
     this.lblbadgenumber = new Krypton.Toolkit.KryptonLabel();
this.txtbadgenumber = new Krypton.Toolkit.KryptonTextBox();

this.lblusername = new Krypton.Toolkit.KryptonLabel();
this.txtusername = new Krypton.Toolkit.KryptonTextBox();

this.lbldeptname = new Krypton.Toolkit.KryptonLabel();
this.txtdeptname = new Krypton.Toolkit.KryptonTextBox();

this.lblsDate = new Krypton.Toolkit.KryptonLabel();
this.txtsDate = new Krypton.Toolkit.KryptonTextBox();

this.lblstime = new Krypton.Toolkit.KryptonLabel();
this.txtstime = new Krypton.Toolkit.KryptonTextBox();
this.txtstime.Multiline = true;

this.lbleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpeDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblt1 = new Krypton.Toolkit.KryptonLabel();
this.dtpt1 = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblt2 = new Krypton.Toolkit.KryptonLabel();
this.dtpt2 = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblt3 = new Krypton.Toolkit.KryptonLabel();
this.dtpt3 = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblt4 = new Krypton.Toolkit.KryptonLabel();
this.dtpt4 = new Krypton.Toolkit.KryptonDateTimePicker();

    
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
     
            //#####30badgenumber###String
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
           // this.kryptonPanel1.TabIndex = 10;

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

                            // 
            // "tb_AttendanceEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_AttendanceEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblbadgenumber;
private Krypton.Toolkit.KryptonTextBox txtbadgenumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblusername;
private Krypton.Toolkit.KryptonTextBox txtusername;

    
        
              private Krypton.Toolkit.KryptonLabel lbldeptname;
private Krypton.Toolkit.KryptonTextBox txtdeptname;

    
        
              private Krypton.Toolkit.KryptonLabel lblsDate;
private Krypton.Toolkit.KryptonTextBox txtsDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblstime;
private Krypton.Toolkit.KryptonTextBox txtstime;

    
        
              private Krypton.Toolkit.KryptonLabel lbleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpeDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblt1;
private Krypton.Toolkit.KryptonDateTimePicker dtpt1;

    
        
              private Krypton.Toolkit.KryptonLabel lblt2;
private Krypton.Toolkit.KryptonDateTimePicker dtpt2;

    
        
              private Krypton.Toolkit.KryptonLabel lblt3;
private Krypton.Toolkit.KryptonDateTimePicker dtpt3;

    
        
              private Krypton.Toolkit.KryptonLabel lblt4;
private Krypton.Toolkit.KryptonDateTimePicker dtpt4;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

