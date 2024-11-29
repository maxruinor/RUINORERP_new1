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
using Netron.Neon.TextEditor.Actions;
using Org.BouncyCastle.Asn1.X509;
using RUINORERP.UI.MRP.MP;

namespace RUINORERP.UI.UserCenter.DataParts
{

    /// <summary>
    /// 一个生产模块的总体界面，可以做一些列隐藏。详细时再打开
    /// </summary>
    public partial class UCPUR : UserControl
    {
        public UCPUR()
        {
            InitializeComponent();
            GridRelated.SetRelatedInfo<tb_SaleOrder>(c => c.SOrderNo);
            GridRelated.SetRelatedInfo<tb_PurEntry>(c => c.PurEntryNo);
            GridRelated.SetRelatedInfo<tb_PurOrder>(c => c.PurOrderNo);
            GridRelated.SetRelatedInfo<tb_PurReturnEntry>(c => c.PurReEntryNo);
            GridRelated.SetRelatedInfo<tb_PurEntry>(c => c.PurEntryNo);
            GridRelated.SetRelatedInfo<tb_PurEntryRe>(c => c.PurEntryReNo);
            GridRelated.SetRelatedInfo<tb_BuyingRequisition>(c => c.PuRequisitionNo);
            this.Dock = DockStyle.Fill;
        }
        public GridViewRelated GridRelated { get; set; } = new GridViewRelated();
      

        public List<tb_PurOrder> OrderList = new List<tb_PurOrder>();

        public async Task<int> QueryData(List<tb_PurOrder> _PURList = null)
        {
            try
            {
                //主要业务表的列头
                ConcurrentDictionary<string, string> colNames = new ConcurrentDictionary<string, string>();// UIHelper.GetFieldNameList<tb_SaleOrder>();
                //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
                //colNames.TryAdd("ID", "ID");
                //colNames.TryAdd("ParentId", "上级ID");
                colNames.TryAdd("PurOrderNo", "单号");
                colNames.TryAdd("PurDate", "单据日期");
                colNames.TryAdd("TotalQty", "采购数量");
                colNames.TryAdd("SendQty", "交付数量");
                colNames.TryAdd("MainContent", "主要内容");
                colNames.TryAdd("Priority", "优先级");
                colNames.TryAdd("Process", "流程");
                colNames.TryAdd("ProgressBar", "进度条");
                colNames.TryAdd("Notes", "备注");
                colNames.TryAdd("EmpName", "采购员");//9
                colNames.TryAdd("CustomerVendor", "供应商");//10
                colNames.TryAdd("Project", "项目");//11
                colNames.TryAdd("DataStatus", "状态");//12
                KeyValuePair<string, string>[] array = colNames.ToArray();
                Array.Sort(array, new UCPurCustomComparer());

                //后面加一个生产请购单，用于跟踪采购来料这块。
                if (_PURList != null)
                {
                    OrderList = _PURList;
                }
                else
                {
                    OrderList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                  .Includes(c => c.tb_employee)
                  .Includes(c => c.tb_PurOrderDetails)
                  .Includes(c => c.tb_department)
                  .Includes(c => c.tb_customervendor)
                  .Includes(c => c.tb_productiondemand)
                  .Includes(c => c.tb_paymentmethod)
                  .Includes(c => c.tb_PurOrderDetails)
                  .Includes(c => c.tb_saleorder)
                  .Includes(d => d.tb_PurEntries, f => f.tb_PurEntryDetails)
                  .Includes(c => c.tb_PurOrderRes, d => d.tb_PurOrderReDetails)
                  .Includes(c => c.tb_PurEntries, d => d.tb_PurEntryRes, f => f.tb_PurEntryReDetails)
                   .AsNavQueryable()
                   .Includes(c => c.tb_PurEntries, d => d.tb_PurEntryRes, f => f.tb_PurReturnEntries, g => g.tb_PurReturnEntryDetails)
                  .WhereIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了采购只看到自己的
                  .Where(c => (c.DataStatus == 2 || c.DataStatus == 4)).OrderBy(c => c.PurDate)
                    .WithCache(60) // 缓存60秒
                  .ToListAsync();
                }
                kryptonTreeGridView1.ReadOnly = true;
                if (OrderList.Count > 0)
                {
                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
                    kryptonTreeGridView1.SortColumnName = "PurDate";
                    kryptonTreeGridView1.DataSource = OrderList.ToDataTable(array, true); // PURList.ToDataTable<Proc_WorkCenterPUR>(expColumns);
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
                        kryptonTreeGridView1.Columns[10].Width = 60;
                    }

                    foreach (var item in kryptonTreeGridView1.GridNodes)
                    {
                        //如果他是来自于订单。特殊标记一下

                        //找到计划单号：
                        string PurOrderNo = item.Cells[0].Value.ToString();
                        var Order = OrderList.FirstOrDefault(p => p.PurOrderNo == PurOrderNo);
                        item.Tag = Order;
                        item.Cells[0].Tag = "PurOrderNo";
                        item.Cells[6].Value = "订单";
                        item.Cells[9].Value = Order.tb_employee.Employee_Name;
                        if (Order.tb_customervendor != null)
                        {
                            item.Cells[10].Value = Order.tb_customervendor.CVName;
                        }
                        if (Order.tb_department != null)
                        {
                            item.Cells[11].Value = Order.tb_department.DepartmentName;
                        }
                        item.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(Order.DataStatus), Order.DataStatus).ToString();
                        string project = string.Empty;
                        #region 加载订单单明细
                        List<tb_PurOrderDetail> OrderDetails = Order.tb_PurOrderDetails;
                        foreach (tb_PurOrderDetail OrderDetail in OrderDetails)
                        {
                            View_ProdDetail prodDetail = await UIBizSrvice.GetProdDetail<View_ProdDetail>(OrderDetail.ProdDetailID);
                            tb_ProductType productType = await UIBizSrvice.GetProdDetail<tb_ProductType>(prodDetail.Type_ID.Value);

                            project += $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.prop}" + ";";
                            //子级
                            KryptonTreeGridNodeRow PurOrderDetailsubrow = item.Nodes.Add(prodDetail.SKU);
                            PurOrderDetailsubrow.Cells[0].Tag = "PurOrderNo";//保存列名 值对象=》tag(如果是明细则指向主表) 的列名。比方值是编号：则是PPNo
                            PurOrderDetailsubrow.Tag = Order;//为了双击的时候能找到值对象。这里还是给主表对象。
                            PurOrderDetailsubrow.Cells[1].Value = "";//分析日期
                            PurOrderDetailsubrow.Cells[2].Value = OrderDetail.Quantity;
                            PurOrderDetailsubrow.Cells[3].Value = OrderDetail.DeliveredQuantity;//还有退回数量
                            PurOrderDetailsubrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                            PurOrderDetailsubrow.Cells[6].Value = "订单产品";
                            PurOrderDetailsubrow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(Order.DataStatus), Order.DataStatus).ToString();


                            #region 在采购订单明细中加载入库单

                            List<tb_PurEntry> PurEntryList = Order.tb_PurEntries;
                            if (PurEntryList != null && PurEntryList.Count > 0)
                            {
                                foreach (tb_PurEntry PurEntry in PurEntryList)
                                {
                                    foreach (tb_PurEntryDetail PurEntryDetail in PurEntry.tb_PurEntryDetails.Where(c => c.ProdDetailID == OrderDetail.ProdDetailID && c.Location_ID == OrderDetail.Location_ID))
                                    {
                                        //子级
                                        KryptonTreeGridNodeRow ProduceDetailrow = PurOrderDetailsubrow.Nodes.Add(PurEntry.PurEntryNo.ToString());
                                        ProduceDetailrow.Tag = PurEntry;//为了双击的时候能找到值对象。这里还是给主表对象。
                                        ProduceDetailrow.Cells[0].Tag = "PurEntryNo";//保存列名 值对象的列名。比方值是编号：则是PDNo
                                        ProduceDetailrow.Cells[1].Value = PurEntry.EntryDate.ToString("yyyy-MM-dd");//出库日期
                                        //ProduceDetailrow.Cells[2].Value = ProduceDetail.Quantity;
                                        ProduceDetailrow.Cells[3].Value = PurEntryDetail.Quantity;
                                        ProduceDetailrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        ProduceDetailrow.Cells[6].Value = "入库";//$"{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        ProduceDetailrow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(PurEntry.DataStatus), PurEntry.DataStatus).ToString();
                                        #region 采购退货单
                                        LoadPurEntryReData(PurEntry, ProduceDetailrow, OrderDetail, prodDetail, productType);
                                        #endregion
                                        #region 采购订单
                                        //  LoadSelfMadeProducts(PurEntry, ProduceDetail, PlanDetailsubrow, prodDetail);
                                        #endregion

                                    }
                                }

                            }
                        }
                        #endregion
                        project = project.TrimEnd(';');
                        item.Cells[3].Value = Order.tb_PurEntries.Sum(c => c.TotalQty);//已交付货数量
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
            if (OrderList.Count > 3)
            {
                kryptonTreeGridView1.CollapseAll();
            }
            else
            {
                kryptonTreeGridView1.ExpandAll();
            }
            return OrderList.Count;
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
                    View_ProdDetail SubProdDetail = await UIBizSrvice.GetProdDetail<View_ProdDetail>(SubProduceDetail.ProdDetailID);
                    tb_ProductType productType = await UIBizSrvice.GetProdDetail<tb_ProductType>(SubProdDetail.Type_ID.Value);
                    //子级
                    KryptonTreeGridNodeRow SubProduceDetailrow = ProduceDetailrow.Nodes.Add(demand.PDNo.ToString());
                    SubProduceDetailrow.Tag = demand;//为了双击的时候能找到值对象。这里还是给主表对象。
                    SubProduceDetailrow.Cells[0].Tag = "PDNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                    SubProduceDetailrow.Cells[1].Value = demand.AnalysisDate.ToString("yyyy-MM-dd");//分析日期
                    SubProduceDetailrow.Cells[2].Value = SubProduceDetail.RequirementQty;
                    SubProduceDetailrow.Cells[4].Value = $"{productType.TypeName}:{SubProdDetail.CNName}{SubProdDetail.Specifications}{SubProdDetail.Model}{SubProdDetail.prop}";//项目
                    SubProduceDetailrow.Cells[6].Value = "次级需求";
                    // LoadPurEntryReData(SubProduceDetail, SubProduceDetailrow, SubProdDetail, productType);
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
        private void LoadPurEntryReData(tb_PurEntry PurEntry, KryptonTreeGridNodeRow ProduceDetailrow, tb_PurOrderDetail SaleOrderDetail, View_ProdDetail prodDetail, tb_ProductType productType)
        {
            if (PurEntry.tb_PurEntryRes == null)
            {
                return;
            }
            List<tb_PurEntryRe> PurEntryReList = PurEntry.tb_PurEntryRes.Where(c => c.PurEntryNo == PurEntry.PurEntryNo).ToList();

            foreach (tb_PurEntryRe PurEntryRe in PurEntryReList)
            {
                KryptonTreeGridNodeRow PurEntryReRow = ProduceDetailrow.Nodes.Add(PurEntryRe.PurEntryReNo);
                PurEntryReRow.Tag = PurEntryRe;
                PurEntryReRow.Cells[0].Tag = "PurEntryReNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                PurEntryReRow.Cells[1].Value = PurEntryRe.Created_at.Value.ToString("yyyy-MM-dd"); //日期;
                PurEntryReRow.Cells[2].Value = PurEntryRe.TotalQty;
                // PurEntryReRow.Cells[3].Value = PurEntryRe.TotalQty;
                PurEntryReRow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                PurEntryReRow.Cells[6].Value = "采购入库退回";
                PurEntryReRow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(PurEntryRe.DataStatus), PurEntryRe.DataStatus).ToString();
                //ProduceDetailrow.Cells[3].Value = PurEntryRe.QuantityDelivered;//制令单的交付数量显示到上级的需求上
                if (PurEntryRe.tb_PurEntryReDetails != null && PurEntryRe.tb_PurEntryReDetails.Count > 0)
                {
                    #region 退回明细
                    foreach (tb_PurEntryReDetail PurEntryReDetail in PurEntryRe.tb_PurEntryReDetails.Where(c => c.ProdDetailID == prodDetail.ProdDetailID && c.Location_ID == SaleOrderDetail.Location_ID))
                    {
                        KryptonTreeGridNodeRow PurEntryReDetailrow = PurEntryReRow.Nodes.Add(PurEntryRe.PurEntryReNo);
                        PurEntryReDetailrow.Tag = PurEntryRe;
                        PurEntryReDetailrow.Cells[0].Tag = "PurEntryReNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                        PurEntryReDetailrow.Cells[1].Value = PurEntryRe.ReturnDate.ToString("yyyy-MM-dd");//日期
                        PurEntryReDetailrow.Cells[2].Value = PurEntryReDetail.Quantity;//预计数量
                        //订单明细数量只一行时。可能多次出库，多次退回，所以这里不能直接用SaleOrderDetail.qtY
                        //if (PurEntryReDetail.Quantity == SaleOrderDetail.qt)
                        //{
                        //    PurEntryReDetailrow.Cells[4].Value = "全部退回";
                        //}
                        PurEntryReDetailrow.Cells[6].Value = "退回明细";
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
                        //特殊情况处理 当前行的业务类型：采购出库  库存盘点 对应一个集合，再用原来的方法来处理
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
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_SaleOrder).Name && m.ClassPath == ("RUINORERP.UI.PSI.PUR.UCMRPData")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_SaleOrderController<tb_SaleOrder> controller = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                        tb_SaleOrder MRPData = await controller.BaseQueryByIdNavAsync(pid);
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

    class UCPurCustomComparer : IComparer<KeyValuePair<string, string>>
    {
        private readonly string[] desiredOrder = { "PurOrderNo", "PurDate", "TotalQty",
            "SendQty", "MainContent", "Priority", "Process", "ProgressBar", "Notes", "EmpName","CustomerVendor","Project","DataStatus" };
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
