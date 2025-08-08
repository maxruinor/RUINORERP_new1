
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:38
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 开票资料
    /// </summary>
    partial class tb_InvoiceInfoQuery
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
     
     this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPICompanyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPICompanyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPITaxID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPITaxID = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPIAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPIAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPITEL = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPITEL = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPIBankName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPIBankName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPIBankNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPIBankNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lbl信用天数 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt信用天数 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl往来余额 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt往来余额 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl应收款 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt应收款 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl预收款 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt预收款 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl纳税号 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt纳税号 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl开户行 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt开户行 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl银行帐号 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt银行帐号 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####CustomerVendor_ID###Int64
//属性测试25CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,25);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 1;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======25
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,21);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 1;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####200PICompanyName###String
this.lblPICompanyName.AutoSize = true;
this.lblPICompanyName.Location = new System.Drawing.Point(100,50);
this.lblPICompanyName.Name = "lblPICompanyName";
this.lblPICompanyName.Size = new System.Drawing.Size(41, 12);
this.lblPICompanyName.TabIndex = 2;
this.lblPICompanyName.Text = "公司名称";
this.txtPICompanyName.Location = new System.Drawing.Point(173,46);
this.txtPICompanyName.Name = "txtPICompanyName";
this.txtPICompanyName.Size = new System.Drawing.Size(100, 21);
this.txtPICompanyName.TabIndex = 2;
this.Controls.Add(this.lblPICompanyName);
this.Controls.Add(this.txtPICompanyName);

           //#####100PITaxID###String
this.lblPITaxID.AutoSize = true;
this.lblPITaxID.Location = new System.Drawing.Point(100,75);
this.lblPITaxID.Name = "lblPITaxID";
this.lblPITaxID.Size = new System.Drawing.Size(41, 12);
this.lblPITaxID.TabIndex = 3;
this.lblPITaxID.Text = "税号";
this.txtPITaxID.Location = new System.Drawing.Point(173,71);
this.txtPITaxID.Name = "txtPITaxID";
this.txtPITaxID.Size = new System.Drawing.Size(100, 21);
this.txtPITaxID.TabIndex = 3;
this.Controls.Add(this.lblPITaxID);
this.Controls.Add(this.txtPITaxID);

           //#####200PIAddress###String
this.lblPIAddress.AutoSize = true;
this.lblPIAddress.Location = new System.Drawing.Point(100,100);
this.lblPIAddress.Name = "lblPIAddress";
this.lblPIAddress.Size = new System.Drawing.Size(41, 12);
this.lblPIAddress.TabIndex = 4;
this.lblPIAddress.Text = "地址";
this.txtPIAddress.Location = new System.Drawing.Point(173,96);
this.txtPIAddress.Name = "txtPIAddress";
this.txtPIAddress.Size = new System.Drawing.Size(100, 21);
this.txtPIAddress.TabIndex = 4;
this.Controls.Add(this.lblPIAddress);
this.Controls.Add(this.txtPIAddress);

           //#####50PITEL###String
this.lblPITEL.AutoSize = true;
this.lblPITEL.Location = new System.Drawing.Point(100,125);
this.lblPITEL.Name = "lblPITEL";
this.lblPITEL.Size = new System.Drawing.Size(41, 12);
this.lblPITEL.TabIndex = 5;
this.lblPITEL.Text = "电话";
this.txtPITEL.Location = new System.Drawing.Point(173,121);
this.txtPITEL.Name = "txtPITEL";
this.txtPITEL.Size = new System.Drawing.Size(100, 21);
this.txtPITEL.TabIndex = 5;
this.Controls.Add(this.lblPITEL);
this.Controls.Add(this.txtPITEL);

           //#####150PIBankName###String
this.lblPIBankName.AutoSize = true;
this.lblPIBankName.Location = new System.Drawing.Point(100,150);
this.lblPIBankName.Name = "lblPIBankName";
this.lblPIBankName.Size = new System.Drawing.Size(41, 12);
this.lblPIBankName.TabIndex = 6;
this.lblPIBankName.Text = "开户行";
this.txtPIBankName.Location = new System.Drawing.Point(173,146);
this.txtPIBankName.Name = "txtPIBankName";
this.txtPIBankName.Size = new System.Drawing.Size(100, 21);
this.txtPIBankName.TabIndex = 6;
this.Controls.Add(this.lblPIBankName);
this.Controls.Add(this.txtPIBankName);

           //#####50PIBankNo###String
this.lblPIBankNo.AutoSize = true;
this.lblPIBankNo.Location = new System.Drawing.Point(100,175);
this.lblPIBankNo.Name = "lblPIBankNo";
this.lblPIBankNo.Size = new System.Drawing.Size(41, 12);
this.lblPIBankNo.TabIndex = 7;
this.lblPIBankNo.Text = "银行帐号";
this.txtPIBankNo.Location = new System.Drawing.Point(173,171);
this.txtPIBankNo.Name = "txtPIBankNo";
this.txtPIBankNo.Size = new System.Drawing.Size(100, 21);
this.txtPIBankNo.TabIndex = 7;
this.Controls.Add(this.lblPIBankNo);
this.Controls.Add(this.txtPIBankNo);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####10信用天数###String
this.lbl信用天数.AutoSize = true;
this.lbl信用天数.Location = new System.Drawing.Point(100,225);
this.lbl信用天数.Name = "lbl信用天数";
this.lbl信用天数.Size = new System.Drawing.Size(41, 12);
this.lbl信用天数.TabIndex = 9;
this.lbl信用天数.Text = "";
this.txt信用天数.Location = new System.Drawing.Point(173,221);
this.txt信用天数.Name = "txt信用天数";
this.txt信用天数.Size = new System.Drawing.Size(100, 21);
this.txt信用天数.TabIndex = 9;
this.Controls.Add(this.lbl信用天数);
this.Controls.Add(this.txt信用天数);

           //#####10往来余额###String
this.lbl往来余额.AutoSize = true;
this.lbl往来余额.Location = new System.Drawing.Point(100,250);
this.lbl往来余额.Name = "lbl往来余额";
this.lbl往来余额.Size = new System.Drawing.Size(41, 12);
this.lbl往来余额.TabIndex = 10;
this.lbl往来余额.Text = "";
this.txt往来余额.Location = new System.Drawing.Point(173,246);
this.txt往来余额.Name = "txt往来余额";
this.txt往来余额.Size = new System.Drawing.Size(100, 21);
this.txt往来余额.TabIndex = 10;
this.Controls.Add(this.lbl往来余额);
this.Controls.Add(this.txt往来余额);

           //#####10应收款###String
this.lbl应收款.AutoSize = true;
this.lbl应收款.Location = new System.Drawing.Point(100,275);
this.lbl应收款.Name = "lbl应收款";
this.lbl应收款.Size = new System.Drawing.Size(41, 12);
this.lbl应收款.TabIndex = 11;
this.lbl应收款.Text = "";
this.txt应收款.Location = new System.Drawing.Point(173,271);
this.txt应收款.Name = "txt应收款";
this.txt应收款.Size = new System.Drawing.Size(100, 21);
this.txt应收款.TabIndex = 11;
this.Controls.Add(this.lbl应收款);
this.Controls.Add(this.txt应收款);

           //#####10预收款###String
this.lbl预收款.AutoSize = true;
this.lbl预收款.Location = new System.Drawing.Point(100,300);
this.lbl预收款.Name = "lbl预收款";
this.lbl预收款.Size = new System.Drawing.Size(41, 12);
this.lbl预收款.TabIndex = 12;
this.lbl预收款.Text = "";
this.txt预收款.Location = new System.Drawing.Point(173,296);
this.txt预收款.Name = "txt预收款";
this.txt预收款.Size = new System.Drawing.Size(100, 21);
this.txt预收款.TabIndex = 12;
this.Controls.Add(this.lbl预收款);
this.Controls.Add(this.txt预收款);

           //#####10纳税号###String
this.lbl纳税号.AutoSize = true;
this.lbl纳税号.Location = new System.Drawing.Point(100,325);
this.lbl纳税号.Name = "lbl纳税号";
this.lbl纳税号.Size = new System.Drawing.Size(41, 12);
this.lbl纳税号.TabIndex = 13;
this.lbl纳税号.Text = "";
this.txt纳税号.Location = new System.Drawing.Point(173,321);
this.txt纳税号.Name = "txt纳税号";
this.txt纳税号.Size = new System.Drawing.Size(100, 21);
this.txt纳税号.TabIndex = 13;
this.Controls.Add(this.lbl纳税号);
this.Controls.Add(this.txt纳税号);

           //#####10开户行###String
this.lbl开户行.AutoSize = true;
this.lbl开户行.Location = new System.Drawing.Point(100,350);
this.lbl开户行.Name = "lbl开户行";
this.lbl开户行.Size = new System.Drawing.Size(41, 12);
this.lbl开户行.TabIndex = 14;
this.lbl开户行.Text = "";
this.txt开户行.Location = new System.Drawing.Point(173,346);
this.txt开户行.Name = "txt开户行";
this.txt开户行.Size = new System.Drawing.Size(100, 21);
this.txt开户行.TabIndex = 14;
this.Controls.Add(this.lbl开户行);
this.Controls.Add(this.txt开户行);

           //#####10银行帐号###String
this.lbl银行帐号.AutoSize = true;
this.lbl银行帐号.Location = new System.Drawing.Point(100,375);
this.lbl银行帐号.Name = "lbl银行帐号";
this.lbl银行帐号.Size = new System.Drawing.Size(41, 12);
this.lbl银行帐号.TabIndex = 15;
this.lbl银行帐号.Text = "";
this.txt银行帐号.Location = new System.Drawing.Point(173,371);
this.txt银行帐号.Name = "txt银行帐号";
this.txt银行帐号.Size = new System.Drawing.Size(100, 21);
this.txt银行帐号.TabIndex = 15;
this.Controls.Add(this.lbl银行帐号);
this.Controls.Add(this.txt银行帐号);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblPICompanyName );
this.Controls.Add(this.txtPICompanyName );

                this.Controls.Add(this.lblPITaxID );
this.Controls.Add(this.txtPITaxID );

                this.Controls.Add(this.lblPIAddress );
this.Controls.Add(this.txtPIAddress );

                this.Controls.Add(this.lblPITEL );
this.Controls.Add(this.txtPITEL );

                this.Controls.Add(this.lblPIBankName );
this.Controls.Add(this.txtPIBankName );

                this.Controls.Add(this.lblPIBankNo );
this.Controls.Add(this.txtPIBankNo );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lbl信用天数 );
this.Controls.Add(this.txt信用天数 );

                this.Controls.Add(this.lbl往来余额 );
this.Controls.Add(this.txt往来余额 );

                this.Controls.Add(this.lbl应收款 );
this.Controls.Add(this.txt应收款 );

                this.Controls.Add(this.lbl预收款 );
this.Controls.Add(this.txt预收款 );

                this.Controls.Add(this.lbl纳税号 );
this.Controls.Add(this.txt纳税号 );

                this.Controls.Add(this.lbl开户行 );
this.Controls.Add(this.txt开户行 );

                this.Controls.Add(this.lbl银行帐号 );
this.Controls.Add(this.txt银行帐号 );

                    
            this.Name = "tb_InvoiceInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPICompanyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPICompanyName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPITaxID;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPITaxID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPIAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPIAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPITEL;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPITEL;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPIBankName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPIBankName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPIBankNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPIBankNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl信用天数;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt信用天数;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl往来余额;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt往来余额;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl应收款;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt应收款;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl预收款;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt预收款;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl纳税号;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt纳税号;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl开户行;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt开户行;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl银行帐号;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt银行帐号;

    
    
   
 





    }
}


