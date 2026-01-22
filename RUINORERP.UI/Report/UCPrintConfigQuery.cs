using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.Common.Extensions;
using System.Collections;
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Report
{

    [MenuAttrAssemblyInfo("打印配置查询", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具, BizType.默认数据)]
    public partial class UCPrintConfigQuery : BaseBillQueryMC<tb_PrintConfig, tb_PrintTemplate>
    {
        public UCPrintConfigQuery()
        {
            InitializeComponent();
 
        }

     
         public override void BuildLimitQueryConditions()
        {
            ////创建表达式
            //var lambda = Expressionable.Create<tb_SaleOrder>()
            //                 //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
            //                 // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
            //                 .And(t => t.isdeleted == false)

            //                   // .And(t => t.Is_enabled == true)
            //                   .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制

            //                .ToExpression();//注意 这一句 不能少
            //base.LimitQueryConditions = lambda;
            //QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

 

        public override void BuildSummaryCols()
        {


        }

        public override void BuildInvisibleCols()
        {

        }

        public override void AddExcludeMenuList()
        {
            toolStripButtonCaseClosed.Visible = false;
            toolStripbtnSubmit.Visible = false;
            toolStripbtnApprove.Visible = false;
            tsbtnAntiApproval.Visible = false;
            AddExcludeMenuList(MenuItemEnums.反结案);
            AddExcludeMenuList(MenuItemEnums.反审);
            AddExcludeMenuList(MenuItemEnums.审核);
            AddExcludeMenuList(MenuItemEnums.导入);
            AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList();
        }





    }
}
