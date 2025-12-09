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
using RUINORERP.Business.Processor;
using NPOI.Util;

namespace RUINORERP.UI.MRP.BOM
{
    [MenuAttrAssemblyInfo("产品配方查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterialsQuery : BaseBillQueryMC<View_BOM, tb_BOM_SDetail>
    {
        public UCBillOfMaterialsQuery()
        {
            InitializeComponent();
            //设置双击列指向哪个业务的单据。打开单据编辑界面，视图用SetRelatedInfo
            //base.RelatedBillEditCol = (c => c.BOM_No);
            this.Load += UCInventoryQuery_Load;
            //bom没有结案的情况
            toolStripButtonCaseClosed.Visible = false;
            //TODO 后面优化 一个手动添加忽略的按钮方法。

        }

        private void UCInventoryQuery_Load(object sender, EventArgs e)
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型

            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCBillMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_BOM_S, tb_BOM_S>(c => c.BOM_No, r => r.BOM_No);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            // base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_BOM_S));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));

        }


        protected async override void Delete(List<View_BOM> Datas)
        {
            if (Datas == null || Datas.Count == 0)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int counter = 0;
                foreach (var item in Datas)
                {
                    //https://www.runoob.com/w3cnote/csharp-enum.html
                    var dataStatus = (DataStatus)(item.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                    if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                    {
                        BaseController<tb_BOM_S> ctr = Startup.GetFromFacByName<BaseController<tb_BOM_S>>(typeof(tb_BOM_S).Name + "Controller");
                        var mybom = await ctr.BaseQueryByIdNavAsync(item.BOM_ID);
                        bool rs = await ctr.BaseDeleteByNavAsync(mybom as tb_BOM_S);
                        if (rs)
                        {
                            //清空对应产品明细中的BOM信息
                            if (mybom.tb_proddetail.BOM_ID.HasValue)
                            {
                                mybom.tb_proddetail.BOM_ID = null;
                                BaseController<tb_ProdDetail> ctrDetail = Startup.GetFromFacByName<BaseController<tb_ProdDetail>>(typeof(tb_ProdDetail).Name + "Controller");
                                await ctrDetail.BaseSaveOrUpdate(mybom.tb_proddetail);
                            }

                            counter++;
                        }
                    }
                    else
                    {
                        //
                        MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }


        public override void AddByCopy(List<View_BOM> EditEntitys)
        {
            base.AddByCopy(EditEntitys);
        }
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_BOM>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            .And(t => t.isdeleted == false)

                            // .And(t => t.is_enabled == true)
                            //.AndIF(MainForm.Instance.AppContext.SysConfig.SaleBizLimited && !MainForm.Instance.AppContext.IsSuperUser, t => t. == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {

            //var lambda = Expressionable.Create<tb_CustomerVendor>()
            //               .And(t => t.isdeleted == false)
            //               .And(t => t.Is_available == true)
            //               .And(t => t.IsCustomer == true)
            //               .And(t => t.Is_enabled == true)
            //               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            //               .ToExpression();//注意 这一句 不能少
            //QueryParameter<tb_SaleOrder> parameter = new QueryParameter<tb_SaleOrder>(c => c.CustomerVendor_ID);
            //parameter.SetFieldLimitCondition<tb_CustomerVendor>(lambda);
            //QueryParameters.Add(parameter);
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_BOM).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalMaterialQty);
            base.MasterSummaryCols.Add(c => c.OutProductionAllCosts);
            base.MasterSummaryCols.Add(c => c.SelfApportionedCost);
            base.MasterSummaryCols.Add(c => c.SelfProductionAllCosts);
            base.MasterSummaryCols.Add(c => c.TotalMaterialCost);
            base.MasterSummaryCols.Add(c => c.TotalOutManuCost);
            base.MasterSummaryCols.Add(c => c.TotalSelfManuCost);
            base.ChildSummaryCols.Add(c => c.SubtotalUnitCost);
            base.ChildSummaryCols.Add(c => c.UsedQty);
            //base.ChildInvisibleCols.Add(c => c.SubtotalUnitCost);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.BOM_ID);
            base.MasterInvisibleCols.Add(c => c.ProdDetailID);
            base.ChildInvisibleCols.Add(C => C.BOM_ID);
        }





    }
}
