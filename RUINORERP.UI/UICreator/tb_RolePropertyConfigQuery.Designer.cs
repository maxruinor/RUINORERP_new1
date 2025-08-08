
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 角色属性配置不同角色权限功能等不一样
    /// </summary>
    partial class tb_RolePropertyConfigQuery
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
     
     this.lblRolePropertyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRolePropertyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRolePropertyName.Multiline = true;




this.lblCurrencyDataPrecisionAutoAddZero = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkCurrencyDataPrecisionAutoAddZero = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkCurrencyDataPrecisionAutoAddZero.Values.Text ="";
this.chkCurrencyDataPrecisionAutoAddZero.Checked = true;
this.chkCurrencyDataPrecisionAutoAddZero.CheckState = System.Windows.Forms.CheckState.Checked;


this.lblShowDebugInfo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkShowDebugInfo = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkShowDebugInfo.Values.Text ="";

this.lblOwnershipControl = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOwnershipControl = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOwnershipControl.Values.Text ="";

this.lblSaleBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSaleBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSaleBizLimited.Values.Text ="";

this.lblDepartBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkDepartBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkDepartBizLimited.Values.Text ="";

this.lblPurchsaeBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPurchsaeBizLimited = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPurchsaeBizLimited.Values.Text ="";

this.lblQueryPageLayoutCustomize = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkQueryPageLayoutCustomize = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkQueryPageLayoutCustomize.Values.Text ="";

this.lblQueryGridColCustomize = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkQueryGridColCustomize = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkQueryGridColCustomize.Values.Text ="";

this.lblBillGridColCustomize = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkBillGridColCustomize = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkBillGridColCustomize.Values.Text ="";

this.lblExclusiveLimited = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkExclusiveLimited = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkExclusiveLimited.Values.Text ="";

this.lblDataBoardUnits = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDataBoardUnits = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDataBoardUnits.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####255RolePropertyName###String
this.lblRolePropertyName.AutoSize = true;
this.lblRolePropertyName.Location = new System.Drawing.Point(100,25);
this.lblRolePropertyName.Name = "lblRolePropertyName";
this.lblRolePropertyName.Size = new System.Drawing.Size(41, 12);
this.lblRolePropertyName.TabIndex = 1;
this.lblRolePropertyName.Text = "角色名称";
this.txtRolePropertyName.Location = new System.Drawing.Point(173,21);
this.txtRolePropertyName.Name = "txtRolePropertyName";
this.txtRolePropertyName.Size = new System.Drawing.Size(100, 21);
this.txtRolePropertyName.TabIndex = 1;
this.Controls.Add(this.lblRolePropertyName);
this.Controls.Add(this.txtRolePropertyName);

           //#####QtyDataPrecision###Int32

           //#####TaxRateDataPrecision###Int32

           //#####MoneyDataPrecision###Int32

           //#####CurrencyDataPrecisionAutoAddZero###Boolean
this.lblCurrencyDataPrecisionAutoAddZero.AutoSize = true;
this.lblCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(100,125);
this.lblCurrencyDataPrecisionAutoAddZero.Name = "lblCurrencyDataPrecisionAutoAddZero";
this.lblCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(41, 12);
this.lblCurrencyDataPrecisionAutoAddZero.TabIndex = 5;
this.lblCurrencyDataPrecisionAutoAddZero.Text = "金额精度自动补零";
this.chkCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(173,121);
this.chkCurrencyDataPrecisionAutoAddZero.Name = "chkCurrencyDataPrecisionAutoAddZero";
this.chkCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(100, 21);
this.chkCurrencyDataPrecisionAutoAddZero.TabIndex = 5;
this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero);
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero);

           //#####CostCalculationMethod###Int32

           //#####ShowDebugInfo###Boolean
this.lblShowDebugInfo.AutoSize = true;
this.lblShowDebugInfo.Location = new System.Drawing.Point(100,175);
this.lblShowDebugInfo.Name = "lblShowDebugInfo";
this.lblShowDebugInfo.Size = new System.Drawing.Size(41, 12);
this.lblShowDebugInfo.TabIndex = 7;
this.lblShowDebugInfo.Text = "调试信息";
this.chkShowDebugInfo.Location = new System.Drawing.Point(173,171);
this.chkShowDebugInfo.Name = "chkShowDebugInfo";
this.chkShowDebugInfo.Size = new System.Drawing.Size(100, 21);
this.chkShowDebugInfo.TabIndex = 7;
this.Controls.Add(this.lblShowDebugInfo);
this.Controls.Add(this.chkShowDebugInfo);

           //#####OwnershipControl###Boolean
this.lblOwnershipControl.AutoSize = true;
this.lblOwnershipControl.Location = new System.Drawing.Point(100,200);
this.lblOwnershipControl.Name = "lblOwnershipControl";
this.lblOwnershipControl.Size = new System.Drawing.Size(41, 12);
this.lblOwnershipControl.TabIndex = 8;
this.lblOwnershipControl.Text = "数据归属控制";
this.chkOwnershipControl.Location = new System.Drawing.Point(173,196);
this.chkOwnershipControl.Name = "chkOwnershipControl";
this.chkOwnershipControl.Size = new System.Drawing.Size(100, 21);
this.chkOwnershipControl.TabIndex = 8;
this.Controls.Add(this.lblOwnershipControl);
this.Controls.Add(this.chkOwnershipControl);

           //#####SaleBizLimited###Boolean
this.lblSaleBizLimited.AutoSize = true;
this.lblSaleBizLimited.Location = new System.Drawing.Point(100,225);
this.lblSaleBizLimited.Name = "lblSaleBizLimited";
this.lblSaleBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblSaleBizLimited.TabIndex = 9;
this.lblSaleBizLimited.Text = "销售业务范围限制";
this.chkSaleBizLimited.Location = new System.Drawing.Point(173,221);
this.chkSaleBizLimited.Name = "chkSaleBizLimited";
this.chkSaleBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkSaleBizLimited.TabIndex = 9;
this.Controls.Add(this.lblSaleBizLimited);
this.Controls.Add(this.chkSaleBizLimited);

           //#####DepartBizLimited###Boolean
this.lblDepartBizLimited.AutoSize = true;
this.lblDepartBizLimited.Location = new System.Drawing.Point(100,250);
this.lblDepartBizLimited.Name = "lblDepartBizLimited";
this.lblDepartBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblDepartBizLimited.TabIndex = 10;
this.lblDepartBizLimited.Text = "部门范围限制";
this.chkDepartBizLimited.Location = new System.Drawing.Point(173,246);
this.chkDepartBizLimited.Name = "chkDepartBizLimited";
this.chkDepartBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkDepartBizLimited.TabIndex = 10;
this.Controls.Add(this.lblDepartBizLimited);
this.Controls.Add(this.chkDepartBizLimited);

           //#####PurchsaeBizLimited###Boolean
this.lblPurchsaeBizLimited.AutoSize = true;
this.lblPurchsaeBizLimited.Location = new System.Drawing.Point(100,275);
this.lblPurchsaeBizLimited.Name = "lblPurchsaeBizLimited";
this.lblPurchsaeBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblPurchsaeBizLimited.TabIndex = 11;
this.lblPurchsaeBizLimited.Text = "采购业务范围限制";
this.chkPurchsaeBizLimited.Location = new System.Drawing.Point(173,271);
this.chkPurchsaeBizLimited.Name = "chkPurchsaeBizLimited";
this.chkPurchsaeBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkPurchsaeBizLimited.TabIndex = 11;
this.Controls.Add(this.lblPurchsaeBizLimited);
this.Controls.Add(this.chkPurchsaeBizLimited);

           //#####QueryPageLayoutCustomize###Boolean
this.lblQueryPageLayoutCustomize.AutoSize = true;
this.lblQueryPageLayoutCustomize.Location = new System.Drawing.Point(100,300);
this.lblQueryPageLayoutCustomize.Name = "lblQueryPageLayoutCustomize";
this.lblQueryPageLayoutCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryPageLayoutCustomize.TabIndex = 12;
this.lblQueryPageLayoutCustomize.Text = "查询页布局自定义";
this.chkQueryPageLayoutCustomize.Location = new System.Drawing.Point(173,296);
this.chkQueryPageLayoutCustomize.Name = "chkQueryPageLayoutCustomize";
this.chkQueryPageLayoutCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryPageLayoutCustomize.TabIndex = 12;
this.Controls.Add(this.lblQueryPageLayoutCustomize);
this.Controls.Add(this.chkQueryPageLayoutCustomize);

           //#####QueryGridColCustomize###Boolean
this.lblQueryGridColCustomize.AutoSize = true;
this.lblQueryGridColCustomize.Location = new System.Drawing.Point(100,325);
this.lblQueryGridColCustomize.Name = "lblQueryGridColCustomize";
this.lblQueryGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryGridColCustomize.TabIndex = 13;
this.lblQueryGridColCustomize.Text = "查询表格列自定义";
this.chkQueryGridColCustomize.Location = new System.Drawing.Point(173,321);
this.chkQueryGridColCustomize.Name = "chkQueryGridColCustomize";
this.chkQueryGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryGridColCustomize.TabIndex = 13;
this.Controls.Add(this.lblQueryGridColCustomize);
this.Controls.Add(this.chkQueryGridColCustomize);

           //#####BillGridColCustomize###Boolean
this.lblBillGridColCustomize.AutoSize = true;
this.lblBillGridColCustomize.Location = new System.Drawing.Point(100,350);
this.lblBillGridColCustomize.Name = "lblBillGridColCustomize";
this.lblBillGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblBillGridColCustomize.TabIndex = 14;
this.lblBillGridColCustomize.Text = "单据表格列自定义";
this.chkBillGridColCustomize.Location = new System.Drawing.Point(173,346);
this.chkBillGridColCustomize.Name = "chkBillGridColCustomize";
this.chkBillGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkBillGridColCustomize.TabIndex = 14;
this.Controls.Add(this.lblBillGridColCustomize);
this.Controls.Add(this.chkBillGridColCustomize);

           //#####ExclusiveLimited###Boolean
this.lblExclusiveLimited.AutoSize = true;
this.lblExclusiveLimited.Location = new System.Drawing.Point(100,375);
this.lblExclusiveLimited.Name = "lblExclusiveLimited";
this.lblExclusiveLimited.Size = new System.Drawing.Size(41, 12);
this.lblExclusiveLimited.TabIndex = 15;
this.lblExclusiveLimited.Text = "启用责任人独占";
this.chkExclusiveLimited.Location = new System.Drawing.Point(173,371);
this.chkExclusiveLimited.Name = "chkExclusiveLimited";
this.chkExclusiveLimited.Size = new System.Drawing.Size(100, 21);
this.chkExclusiveLimited.TabIndex = 15;
this.Controls.Add(this.lblExclusiveLimited);
this.Controls.Add(this.chkExclusiveLimited);

           //#####500DataBoardUnits###String
this.lblDataBoardUnits.AutoSize = true;
this.lblDataBoardUnits.Location = new System.Drawing.Point(100,400);
this.lblDataBoardUnits.Name = "lblDataBoardUnits";
this.lblDataBoardUnits.Size = new System.Drawing.Size(41, 12);
this.lblDataBoardUnits.TabIndex = 16;
this.lblDataBoardUnits.Text = "";
this.txtDataBoardUnits.Location = new System.Drawing.Point(173,396);
this.txtDataBoardUnits.Name = "txtDataBoardUnits";
this.txtDataBoardUnits.Size = new System.Drawing.Size(100, 21);
this.txtDataBoardUnits.TabIndex = 16;
this.Controls.Add(this.lblDataBoardUnits);
this.Controls.Add(this.txtDataBoardUnits);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRolePropertyName );
this.Controls.Add(this.txtRolePropertyName );

                
                
                
                this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero );
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero );

                
                this.Controls.Add(this.lblShowDebugInfo );
this.Controls.Add(this.chkShowDebugInfo );

                this.Controls.Add(this.lblOwnershipControl );
this.Controls.Add(this.chkOwnershipControl );

                this.Controls.Add(this.lblSaleBizLimited );
this.Controls.Add(this.chkSaleBizLimited );

                this.Controls.Add(this.lblDepartBizLimited );
this.Controls.Add(this.chkDepartBizLimited );

                this.Controls.Add(this.lblPurchsaeBizLimited );
this.Controls.Add(this.chkPurchsaeBizLimited );

                this.Controls.Add(this.lblQueryPageLayoutCustomize );
this.Controls.Add(this.chkQueryPageLayoutCustomize );

                this.Controls.Add(this.lblQueryGridColCustomize );
this.Controls.Add(this.chkQueryGridColCustomize );

                this.Controls.Add(this.lblBillGridColCustomize );
this.Controls.Add(this.chkBillGridColCustomize );

                this.Controls.Add(this.lblExclusiveLimited );
this.Controls.Add(this.chkExclusiveLimited );

                this.Controls.Add(this.lblDataBoardUnits );
this.Controls.Add(this.txtDataBoardUnits );

                    
            this.Name = "tb_RolePropertyConfigQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRolePropertyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRolePropertyName;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrencyDataPrecisionAutoAddZero;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkCurrencyDataPrecisionAutoAddZero;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShowDebugInfo;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkShowDebugInfo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOwnershipControl;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOwnershipControl;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleBizLimited;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSaleBizLimited;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartBizLimited;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkDepartBizLimited;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurchsaeBizLimited;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPurchsaeBizLimited;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblQueryPageLayoutCustomize;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkQueryPageLayoutCustomize;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblQueryGridColCustomize;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkQueryGridColCustomize;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillGridColCustomize;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkBillGridColCustomize;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExclusiveLimited;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkExclusiveLimited;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDataBoardUnits;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDataBoardUnits;

    
    
   
 





    }
}


