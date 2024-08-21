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
    [MenuAttrAssemblyInfo("BOM清单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterialsQuery : BaseBillQueryMC<tb_BOM_S, tb_BOM_SDetail>
    {
        public UCBillOfMaterialsQuery()
        {
            InitializeComponent();
            //生成查询条件的相关实体
            // ReladtedEntityType = typeof(tb_BOM_S);
            base.RelatedBillEditCol = (c => c.BOM_No);
            this.Load += UCInventoryQuery_Load;
        }

        private void UCInventoryQuery_Load(object sender, EventArgs e)
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型
            // base._UCBillMasterQuery.ColDisplayType = typeof(tb_BOM_S);
        }


        protected async override void Delete(List<tb_BOM_S> Datas)
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
                        bool rs = await ctr.BaseDeleteByNavAsync(item as tb_BOM_S);
                        if (rs)
                        {
                            //清空对应产品明细中的BOM信息
                            if (item.tb_proddetail.BOM_ID.HasValue)
                            {
                                item.tb_proddetail.BOM_ID = null;
                                BaseController<tb_ProdDetail> ctrDetail = Startup.GetFromFacByName<BaseController<tb_ProdDetail>>(typeof(tb_ProdDetail).Name + "Controller");
                                await ctrDetail.BaseSaveOrUpdate(item.tb_proddetail);
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

        /// <summary>
        /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <returns></returns>
        public async override Task<bool> ReReview(List<tb_BOM_S> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_BOM_S> needApprovals = EditEntitys.Where(
                c => (c.ApprovalStatus.HasValue
                && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核
                && c.ApprovalResults.HasValue && c.ApprovalResults.Value)
                ).ToList();

            if (needApprovals.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要反审核的数据为：{needApprovals.Count}:请检查数据！");
                return false;
            }

            tb_BOM_SController<tb_BOM_S> ctr = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            bool Succeeded = await ctr.ReverseApproveAsync(needApprovals[0]);
            if (Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.Query(QueryDto);
            }
            return true;
        }

        public override void AddByCopy(List<tb_BOM_S> EditEntitys)
        {
            /*
            //这里是显示明细
            //要把单据信息传过去
            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath.Contains(typeof(M).Name.Replace("tb_", "UC").ToString().Replace("Query", ""))).FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                if (EditEntitys.Count > 0)
                {
                    M instance = Activator.CreateInstance(typeof(M), false) as M;
                    M instance2 = default(M);
                    instance2 = EditEntitys[0].Copy();

                    //要把单据信息传过去
                    menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance2);
                }
            }
            */
            base.AddByCopy(EditEntitys);
        }


        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            //System.Linq.Expressions.Expression<Func<, int?>> exprApprovalStatus;
            //exprApprovalStatus = (p) => p.ApprovalStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(ApprovalStatus)));


            //System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.PayStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            /*
            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);

            */
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_BOM_S>()
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
        public override void BuildQueryCondition()
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BOM_S).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalMaterialQty);
            base.MasterSummaryCols.Add(c => c.TotalMaterialQty);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.TotalMaterialCost);
            //指定子表中 主表ID列不需要显示，是不是可以统一处理？
            base.ChildInvisibleCols.Add(C => C.BOM_ID);
        }

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    base.newSumDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    base.newSumDataGridView1.XmlFileName = this.Name + nameof(tb_Inventory) + "View_ProdDetail";
        //    base.newSumDataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(View_ProdDetail));
        //    bindingSourceListMain.DataSource = new List<View_ProdDetail>();
        //    base.newSumDataGridView1.DataSource = null;
        //    绑定导航
        //    base.newSumDataGridView1.DataSource = bindingSourceListMain.DataSource;
        //}


        /*
        /// <summary>
        /// 查的是视图。特殊处理了一下。传入对应的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridView1.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = newSumDataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (newSumDataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理



        }

        */

    }
}
