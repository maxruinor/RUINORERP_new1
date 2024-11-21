using RUINORERP.Business.Security;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using HLH.Lib.List;
using System.Linq.Expressions;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using System.Collections.Concurrent;
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using System.Windows.Navigation;
using Netron.GraphLib;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Helper;
using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.Outlook.Grid;

namespace RUINORERP.UI.UserCenter.DataParts
{

    /// <summary>
    /// 一个生产模块的总体界面，可以做一些列隐藏。详细时再打开
    /// </summary>
    public partial class UCMRP : UserControl
    {
        public UCMRP()
        {
            InitializeComponent();
            GridRelated.SetRelatedInfo<tb_ProductionPlan>(c => c.PPNo);
            GridRelated.SetRelatedInfo<tb_ManufacturingOrder>(c => c.MONO);
            GridRelated.SetRelatedInfo<tb_MaterialRequisition>(c => c.MaterialRequisitionNO);
            GridRelated.SetRelatedInfo<tb_MaterialReturn>(c => c.BillNo);
            GridRelated.SetRelatedInfo<tb_FinishedGoodsInv>(c => c.DeliveryBillNo);
            GridRelated.SetRelatedInfo<tb_ProductionDemand>(c => c.PDNo);
        }
        public GridViewRelated GridRelated { get; set; } = new GridViewRelated();
        private async Task<T> GetProdDetail<T>(long ProdDetailID) where T : class
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            T prodDetail = null;

            if (BizCacheHelper.Manager.NewTableList.ContainsKey(typeof(T).Name))
            {
                var nkv = BizCacheHelper.Manager.NewTableList[typeof(T).Name];
                if (nkv.Key != null)
                {
                    object obj = BizCacheHelper.Instance.GetEntity<T>(ProdDetailID);
                    if (obj != null && obj.GetType().Name != "Object")
                    {
                        prodDetail = obj as T;
                    }
                    else
                    {
                        prodDetail = await MainForm.Instance.AppContext.Db.Queryable<T>().Where(p => p.GetPropertyValue(PKCol).ToString().Equals(ProdDetailID.ToString())).SingleAsync();
                    }
                }
            }
            return prodDetail;
        }

        public List<tb_ProductionPlan> PURList = new List<tb_ProductionPlan>();

        public async Task<int> QueryMRPDataStatus(List<tb_ProductionPlan> _PURList = null)
        {
            try
            {
                //主要业务表的列头
                ConcurrentDictionary<string, string> colNames = new ConcurrentDictionary<string, string>();// UIHelper.GetFieldNameList<tb_ProductionPlan>();
                //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
                //colNames.TryAdd("ID", "ID");
                //colNames.TryAdd("ParentId", "上级ID");
                colNames.TryAdd("PPNo", "单号");
                colNames.TryAdd("RequirementDate", "计划日期");
                colNames.TryAdd("TotalQuantity", "需求数量");
                colNames.TryAdd("TotalCompletedQuantity", "交付数量");
                colNames.TryAdd("MainContent", "主要内容");
                colNames.TryAdd("Priority", "优先级");
                colNames.TryAdd("Process", "流程");
                colNames.TryAdd("ProgressBar", "进度条");
                colNames.TryAdd("Notes", "备注");
                colNames.TryAdd("EmpName", "计划员");//9
                colNames.TryAdd("Department", "部门");//10
                colNames.TryAdd("Project", "项目");//11
                colNames.TryAdd("DataStatus", "状态");//12
                KeyValuePair<string, string>[] array = colNames.ToArray();
                Array.Sort(array, new UCMRPCustomComparer());

                //后面加一个生产请购单，用于跟踪采购来料这块。
                if (_PURList != null)
                {
                    PURList = _PURList;
                }
                else
                {
                    PURList = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                  .Includes(c => c.tb_employee)
                  .Includes(c => c.tb_department)
                   .Includes(c => c.tb_projectgroup)
                  .Includes(c => c.tb_ProductionPlanDetails)
                  .Includes(c => c.tb_ProductionDemands, d => d.tb_ProductionDemandTargetDetails)
                  .AsNavQueryable()
                  .Includes(c => c.tb_ProductionDemands, d => d.tb_ProduceGoodsRecommendDetails, f => f.tb_ManufacturingOrders, e => e.tb_FinishedGoodsInvs, f => f.tb_FinishedGoodsInvDetails)
                  .AsNavQueryable()
                  .Includes(c => c.tb_ProductionDemands, d => d.tb_ProduceGoodsRecommendDetails, f => f.tb_ManufacturingOrders, c => c.tb_MaterialRequisitions)
                  .Where(c => (c.DataStatus == 2 || c.DataStatus == 4)).OrderBy(c => c.RequirementDate)
                  .ToListAsync();
                }
                kryptonTreeGridView1.ReadOnly = true;
                if (PURList.Count > 0)
                {

                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
                    kryptonTreeGridView1.SortColumnName = "RequirementDate";
                    kryptonTreeGridView1.DataSource = PURList.ToDataTable(array, true); // PURList.ToDataTable<Proc_WorkCenterPUR>(expColumns);
                    if (kryptonTreeGridView1.ColumnCount > 0)
                    {
                        kryptonTreeGridView1.Columns[0].Width = 220;
                        kryptonTreeGridView1.Columns[1].Width = 70;
                        kryptonTreeGridView1.Columns[2].Width = 70;
                        kryptonTreeGridView1.Columns[3].Width = 70;
                        kryptonTreeGridView1.Columns[4].Width = 250;
                        kryptonTreeGridView1.Columns[5].Width = 50;
                        kryptonTreeGridView1.Columns[6].Width = 70;
                        kryptonTreeGridView1.Columns[7].Width = 60;
                        kryptonTreeGridView1.Columns[8].Width = 150;
                        kryptonTreeGridView1.Columns[9].Width = 60;
                    }

                    foreach (var item in kryptonTreeGridView1.GridNodes)
                    {
                        //如果他是来自于订单。特殊标记一下

                        //找到计划单号：
                        string PPNo = item.Cells[0].Value.ToString();
                        var ProductionPlan = PURList.FirstOrDefault(p => p.PPNo == PPNo);
                        item.Tag = ProductionPlan;
                        item.Cells[0].Tag = "PPNo";
                        item.Cells[6].Value = "计划";
                        item.Cells[9].Value = ProductionPlan.tb_employee.Employee_Name;
                        if (ProductionPlan.tb_department != null)
                        {
                            item.Cells[10].Value = ProductionPlan.tb_department.DepartmentName;
                        }
                        if (ProductionPlan.tb_projectgroup != null)
                        {
                            item.Cells[11].Value = ProductionPlan.tb_projectgroup.ProjectGroupName;
                        }

                        string project = string.Empty;
                        #region 加载计划单明细
                        List<tb_ProductionPlanDetail> PlanDetails = ProductionPlan.tb_ProductionPlanDetails;
                        foreach (tb_ProductionPlanDetail PlanDetail in PlanDetails)
                        {
                            View_ProdDetail prodDetail = await GetProdDetail<View_ProdDetail>(PlanDetail.ProdDetailID);
                            tb_ProductType productType = await GetProdDetail<tb_ProductType>(prodDetail.Type_ID.Value);

                            project += $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.prop}" + ";";
                            //子级
                            KryptonTreeGridNodeRow PlanDetailsubrow = item.Nodes.Add(prodDetail.SKU);
                            PlanDetailsubrow.Cells[0].Tag = "PPNo";//保存列名 值对象=》tag(如果是明细则指向主表) 的列名。比方值是编号：则是PPNo
                            PlanDetailsubrow.Tag = ProductionPlan;//为了双击的时候能找到值对象。这里还是给主表对象。
                            PlanDetailsubrow.Cells[1].Value = "";//分析日期
                            PlanDetailsubrow.Cells[2].Value = PlanDetail.Quantity;
                            PlanDetailsubrow.Cells[3].Value = PlanDetail.CompletedQuantity;
                            PlanDetailsubrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                            PlanDetailsubrow.Cells[6].Value = "计划产品";
                            #region 在计划明细中加载自制品明细

                            List<tb_ProductionDemand> demandList = ProductionPlan.tb_ProductionDemands;
                            if (demandList != null && demandList.Count > 0)
                            {
                                foreach (tb_ProductionDemand demand in demandList)
                                {
                                    foreach (tb_ProduceGoodsRecommendDetail ProduceDetail in demand.tb_ProduceGoodsRecommendDetails.Where(c => c.ProdDetailID == PlanDetail.ProdDetailID))
                                    {
                                        //子级
                                        KryptonTreeGridNodeRow ProduceDetailrow = PlanDetailsubrow.Nodes.Add(demand.PDNo.ToString());
                                        ProduceDetailrow.Tag = demand;//为了双击的时候能找到值对象。这里还是给主表对象。
                                        ProduceDetailrow.Cells[0].Tag = "PDNo";//保存列名 值对象的列名。比方值是编号：则是PDNo
                                        ProduceDetailrow.Cells[1].Value = demand.AnalysisDate.ToString("yyyy-MM-dd");//分析日期
                                        ProduceDetailrow.Cells[2].Value = ProduceDetail.RequirementQty;//分析日期
                                        ProduceDetailrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        ProduceDetailrow.Cells[6].Value = "主需求";//$"{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        #region 制令单显示列
                                        BindMOData(ProduceDetail, ProduceDetailrow, prodDetail, productType);
                                        #endregion
                                        #region 半成品明细
                                        LoadSelfMadeProducts(demand, ProduceDetail, PlanDetailsubrow, prodDetail);
                                        #endregion

                                    }
                                }

                            }
                        }
                        #endregion
                        project = project.TrimEnd(';');
                        item.Cells[4].Value = project;
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
                //if (errorCount > 10)
                //{
                //    timer1.Stop();
                //}
            }
            if (PURList.Count > 3)
            {
                kryptonTreeGridView1.CollapseAll();
            }
            else
            {
                kryptonTreeGridView1.ExpandAll();
            }
            return PURList.Count;
        }

        /// <summary>
        /// 循环加载子件
        /// </summary>
        /// <param name="demand"></param>
        /// <param name="ProduceDetail"></param>
        /// <param name="ProduceDetailrow"></param>
        /// <param name="prodDetail"></param>
        private async void LoadSelfMadeProducts(tb_ProductionDemand demand, tb_ProduceGoodsRecommendDetail ProduceDetail, KryptonTreeGridNodeRow ProduceDetailrow, View_ProdDetail prodDetail)
        {
            List<tb_ProduceGoodsRecommendDetail> SubSelfDetails = new List<tb_ProduceGoodsRecommendDetail>();
            SubSelfDetails = demand.tb_ProduceGoodsRecommendDetails.Where(c => c.ParentId == ProduceDetail.ID).ToList();
            if (SubSelfDetails.Count > 0)
            {
                foreach (tb_ProduceGoodsRecommendDetail SubProduceDetail in SubSelfDetails)
                {
                    View_ProdDetail SubProdDetail = await GetProdDetail<View_ProdDetail>(SubProduceDetail.ProdDetailID);
                    tb_ProductType productType = await GetProdDetail<tb_ProductType>(SubProdDetail.Type_ID.Value);
                    //子级
                    KryptonTreeGridNodeRow SubProduceDetailrow = ProduceDetailrow.Nodes.Add(demand.PDNo.ToString());
                    SubProduceDetailrow.Tag = demand;//为了双击的时候能找到值对象。这里还是给主表对象。
                    SubProduceDetailrow.Cells[0].Tag = "PDNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                    SubProduceDetailrow.Cells[1].Value = demand.AnalysisDate.ToString("yyyy-MM-dd");//分析日期
                    SubProduceDetailrow.Cells[2].Value = SubProduceDetail.RequirementQty;
                    SubProduceDetailrow.Cells[4].Value = $"{productType.TypeName}:{SubProdDetail.CNName}{SubProdDetail.Specifications}{SubProdDetail.Model}{SubProdDetail.prop}";//项目
                    SubProduceDetailrow.Cells[6].Value = "次级需求";
                    BindMOData(SubProduceDetail, SubProduceDetailrow, SubProdDetail, productType);
                    LoadSelfMadeProducts(demand, SubProduceDetail, SubProduceDetailrow, SubProdDetail);
                }
            }
        }


        /// <summary>
        /// 加载子件的MO（成品 也一样）
        /// </summary>
        /// <param name="ProduceDetail"></param>
        /// <param name="ProduceDetailrow"></param>
        /// <param name="prodDetail"></param>
        private void BindMOData(tb_ProduceGoodsRecommendDetail ProduceDetail, KryptonTreeGridNodeRow ProduceDetailrow, View_ProdDetail prodDetail, tb_ProductType productType)
        {
            List<tb_ManufacturingOrder> ManufacturingOrders = ProduceDetail.tb_ManufacturingOrders.Where(c => c.PDCID == ProduceDetail.PDCID).ToList();
            if (ProduceDetail.tb_ManufacturingOrders.Count > 0 && ManufacturingOrders.Count == 0)
            {
                //可能是主需求做了mo。次级需求还没做MO
                ProduceDetailrow.Visible = false;
            }
            foreach (tb_ManufacturingOrder manufacturingOrder in ManufacturingOrders)
            {
                KryptonTreeGridNodeRow manufacturerow = ProduceDetailrow.Nodes.Add(manufacturingOrder.MONO);
                manufacturerow.Tag = manufacturingOrder;
                manufacturerow.Cells[0].Tag = "MONO";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                manufacturerow.Cells[1].Value = manufacturingOrder.Created_at.Value.ToString("yyyy-MM-dd"); //日期;
                manufacturerow.Cells[2].Value = manufacturingOrder.ManufacturingQty;
                manufacturerow.Cells[3].Value = manufacturingOrder.QuantityDelivered;
                manufacturerow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                manufacturerow.Cells[6].Value = "制令单";
                ProduceDetailrow.Cells[3].Value = manufacturingOrder.QuantityDelivered;//制令单的交付数量显示到上级的需求上
                if (manufacturingOrder.tb_MaterialRequisitions != null && manufacturingOrder.tb_MaterialRequisitions.Count > 0)
                {
                    #region 发料单 只显示一个状态
                    foreach (tb_MaterialRequisition MaterialRequisition in manufacturingOrder.tb_MaterialRequisitions)
                    {
                        KryptonTreeGridNodeRow MaterialRequisitionrow = manufacturerow.Nodes.Add(MaterialRequisition.MaterialRequisitionNO);
                        MaterialRequisitionrow.Tag = MaterialRequisition;
                        MaterialRequisitionrow.Cells[0].Tag = "MaterialRequisitionNO";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                        MaterialRequisitionrow.Cells[1].Value = MaterialRequisition.DeliveryDate.Value.ToString("yyyy-MM-dd");//日期
                        MaterialRequisitionrow.Cells[2].Value = MaterialRequisition.ExpectedQuantity;//预计数量
                        if (MaterialRequisition.DataStatus == (int)DataStatus.确认 || MaterialRequisition.DataStatus == (int)DataStatus.完结)
                        {
                            MaterialRequisitionrow.Cells[4].Value = "已发料";
                        }
                        MaterialRequisitionrow.Cells[6].Value = "发料";
                    }
                    #endregion
                }

                if (manufacturingOrder.tb_FinishedGoodsInvs != null && manufacturingOrder.tb_FinishedGoodsInvs.Count > 0)
                {
                    #region 缴库单 
                    foreach (tb_FinishedGoodsInv FinishedGoodsInv in manufacturingOrder.tb_FinishedGoodsInvs)
                    {
                        foreach (tb_FinishedGoodsInvDetail FinishedGoodsInvDetail in FinishedGoodsInv.tb_FinishedGoodsInvDetails)
                        {
                            #region 缴库单明细
                            KryptonTreeGridNodeRow FinishedGoodsDetailrow = manufacturerow.Nodes.Add(FinishedGoodsInv.DeliveryBillNo);
                            FinishedGoodsDetailrow.Tag = FinishedGoodsInv;//为了双击的时候能找到值对象。这里还是给主表对象。
                            FinishedGoodsDetailrow.Cells[0].Tag = "DeliveryBillNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                            FinishedGoodsDetailrow.Cells[1].Value = FinishedGoodsInv.DeliveryDate.ToString("yyyy-MM-dd"); //日期
                            FinishedGoodsDetailrow.Cells[2].Value = FinishedGoodsInvDetail.PayableQty;
                            FinishedGoodsDetailrow.Cells[3].Value = FinishedGoodsInvDetail.Qty;
                            if (FinishedGoodsInvDetail.PayableQty == FinishedGoodsInvDetail.Qty)
                            {
                                FinishedGoodsDetailrow.Cells[4].Value = "全部缴库";
                            }
                            else if (FinishedGoodsInvDetail.PayableQty > FinishedGoodsInvDetail.Qty)
                            {
                                FinishedGoodsDetailrow.Cells[4].Value = "部分缴库";
                            }
                            else if (0 == FinishedGoodsInvDetail.Qty)
                            {
                                FinishedGoodsDetailrow.Cells[4].Value = "未缴库";
                            }

                            FinishedGoodsDetailrow.Cells[6].Value = "缴库";
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }


        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private async void kryptonTreeGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            if (kryptonTreeGridView1.CurrentRow != null && kryptonTreeGridView1.CurrentCell != null)
            {
                if (kryptonTreeGridView1.CurrentRow.Tag is BaseEntity entity)
                {
                    if (kryptonTreeGridView1.CurrentCell.Tag != null)
                    {
                        //特殊情况处理 当前行的业务类型：销售出库  库存盘点 对应一个集合，再用原来的方法来处理
                        GridRelated.GuideToForm(kryptonTreeGridView1.CurrentCell.Tag.ToString(), entity);
                    }
                }
            }
            return;
            //导航到指向的单据界面
            //找到要打开的菜单  查询下级数据
            if (kryptonTreeGridView1.CurrentCell != null)
            {
                if (kryptonTreeGridView1.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_ProductionPlan).Name && m.ClassPath == ("RUINORERP.UI.PSI.PUR.UCMRPData")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_ProductionPlanController<tb_ProductionPlan> controller = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
                        tb_ProductionPlan MRPData = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, MRPData);
                    }
                }
            }


        }

        private void treeGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //这个特殊这里是第一行的行号
            if (e.RowIndex == 0 && e.ColumnIndex == -1)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }

            if (e.RowIndex > 0)
            {
                #region 画行号
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    DataGridViewPaintParts paintParts =
                        e.PaintParts & ~DataGridViewPaintParts.Focus;

                    e.Paint(e.ClipBounds, paintParts);
                    e.Handled = true;
                }

                if (e.ColumnIndex < 0 && e.RowIndex >= 0)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        (e.RowIndex + 1).ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }

                #endregion
            }

            ////画总行数行号
            //if (e.ColumnIndex < 0 && e.RowIndex < 0)
            //{
            //    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            //    Rectangle indexRect = e.CellBounds;
            //    indexRect.Inflate(-2, -2);

            //    TextRenderer.DrawText(e.Graphics,
            //        (treeGridView1.Nodes.Count + "#").ToString(),
            //        e.CellStyle.Font,
            //        indexRect,
            //        e.CellStyle.ForeColor,
            //        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
            //    e.Handled = true;
            //}
        }

    }

    class UCMRPCustomComparer : IComparer<KeyValuePair<string, string>>
    {
        private readonly string[] desiredOrder = { "PPNo", "RequirementDate", "TotalQuantity",
            "TotalCompletedQuantity", "MainContent", "Priority", "Process", "ProgressBar", "Notes", "EmpName","Department","Project","DataStatus" };
        public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            int indexX = Array.IndexOf(desiredOrder, x.Key);
            int indexY = Array.IndexOf(desiredOrder, y.Key);

            if (indexX == -1 && indexY == -1)
            {
                return 0;  // 如果两个键都不在期望顺序列表中，认为相等（你可以根据实际需求调整这个逻辑）
            }
            else if (indexX == -1)
            {
                return 1;  // x的键不在期望顺序中，认为它应该排在后面
            }
            else if (indexY == -1)
            {
                return -1; // y的键不在期望顺序中，认为它应该排在后面
            }

            return indexX.CompareTo(indexY);
        }
    }
}
