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

namespace RUINORERP.UI.UserCenter.DataParts
{

    /// <summary>
    /// 一个生产模块的总体界面，可以做一些列隐藏。详细时再打开
    /// </summary>
    public partial class UCSale : UserControl
    {
        public UCSale()
        {
            InitializeComponent();
            GridRelated.SetRelatedInfo<tb_SaleOrder>(c => c.SOrderNo);
            GridRelated.SetRelatedInfo<tb_SaleOut>(c => c.SaleOutNo);
            GridRelated.SetRelatedInfo<tb_PurOrder>(c => c.PurOrderNo);
            GridRelated.SetRelatedInfo<tb_SaleOutRe>(c => c.ReturnNo);
            GridRelated.SetRelatedInfo<tb_PurEntry>(c => c.PurEntryNo);
            GridRelated.SetRelatedInfo<tb_PurEntryRe>(c => c.PurEntryReNo);
            GridRelated.SetRelatedInfo<tb_BuyingRequisition>(c => c.PuRequisitionNo);
            this.Dock = DockStyle.Fill;
        }
        public GridViewRelated GridRelated { get; set; } = new GridViewRelated();

        public List<tb_SaleOrder> SaleOrderList = new List<tb_SaleOrder>();

        GridViewDisplayHelper displayHelper = new GridViewDisplayHelper();
        public async Task<int> QueryData(List<tb_SaleOrder> _PURList = null)
        {
            try
            {
                //主要业务表的列头
                ConcurrentDictionary<string, string> colNames = new ConcurrentDictionary<string, string>();// UIHelper.GetFieldNameList<tb_SaleOrder>();
                //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
                //colNames.TryAdd("ID", "ID");
                //colNames.TryAdd("ParentId", "上级ID");
                colNames.TryAdd("SOrderNo", "单号");
                colNames.TryAdd("SaleDate", "单据日期");
                colNames.TryAdd("TotalQty", "销售数量");
                colNames.TryAdd("SendQty", "发货数量");
                colNames.TryAdd("MainContent", "主要内容");
                colNames.TryAdd("Priority", "优先级");
                colNames.TryAdd("Process", "流程");
                colNames.TryAdd("ProgressBar", "进度条");
                colNames.TryAdd("Notes", "备注");
                colNames.TryAdd("EmpName", "销售员");//9
                colNames.TryAdd("CustomerVendor", "客户");//10
                colNames.TryAdd("Project", "项目");//11
                colNames.TryAdd("DataStatus", "状态");//12
                KeyValuePair<string, string>[] array = colNames.ToArray();
                Array.Sort(array, new UCSaleCustomComparer());

                //后面加一个生产请购单，用于跟踪采购来料这块。
                if (_PURList != null)
                {
                    SaleOrderList = _PURList;
                }
                else
                {
                    SaleOrderList = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_SaleOrder>()
                  //.Includes(c => c.tb_employee)
                  .Includes(c => c.tb_SaleOrderDetails)
                  //.Includes(c => c.tb_projectgroup)
                  //.Includes(c => c.tb_customervendor)
                  //.Includes(c => c.tb_OrderPackings)
                  //.Includes(c => c.tb_paymentmethod)
                  .Includes(c => c.tb_PurOrders, d => d.tb_PurOrderDetails)
                  .Includes(c => c.tb_PurOrders, d => d.tb_PurEntries, f => f.tb_PurEntryDetails)
                  .Includes(c => c.tb_SaleOuts, d => d.tb_SaleOutRes, f => f.tb_SaleOutReDetails)
                  .Includes(c => c.tb_SaleOuts, d => d.tb_SaleOutRes, f => f.tb_SaleOutReRefurbishedMaterialsDetails)
                  .Includes(c => c.tb_SaleOuts, d => d.tb_SaleOutDetails)
                  .Where(c => (c.DataStatus == 2 || c.DataStatus == 4)
                  && c.isdeleted == false
                  && c.ApprovalResults.HasValue && c.ApprovalResults == true
                  && c.ApprovalStatus.HasValue && c.ApprovalStatus == (int)ApprovalStatus.已审核)
                  .OrderBy(c => c.SaleDate)
                  .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                                                                                                                                                                                     //.WithCache(60) // 缓存60秒
                  .ToPageListAsync(1, 1000); // 第一页，每页最多显示2000条
                }
                kryptonTreeGridView1.ReadOnly = true;
                if (SaleOrderList.Count > 0)
                {
                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
                    kryptonTreeGridView1.SortColumnName = "SaleDate";
                    kryptonTreeGridView1.DataSource = SaleOrderList.ToDataTable(array, true); // PURList.ToDataTable<Proc_WorkCenterPUR>(expColumns);
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
                        string SOrderNo = item.Cells[0].Value.ToString();
                        var SaleOrder = SaleOrderList.FirstOrDefault(p => p.SOrderNo == SOrderNo);
                        item.Tag = SaleOrder;
                        item.Cells[0].Tag = "SOrderNo";
                        item.Cells[6].Value = "订单";
                        #region 显示进度条
                        double processBarValue = 0d;
                        if (SaleOrder.TotalQty == 0)
                        {
                            processBarValue = 0d; // 或者其他默认值
                        }
                        else
                        {
                            processBarValue = ((double)SaleOrder.tb_SaleOrderDetails.Sum(s => s.TotalDeliveredQty) / SaleOrder.TotalQty) * 100;
                        }
                        item.Cells[7].Value = processBarValue;
                        if (item.Cells[0] is KryptonTreeGridCell barCell)
                        {
                            barCell.ProcessBarValue = processBarValue;
                        }

                        #endregion

                        if (SaleOrder.tb_employee != null)
                        {
                            item.Cells[9].Value = SaleOrder.tb_employee.Employee_Name;
                        }
                        else
                        {
                            item.Cells[9].Value = displayHelper.GetGridViewDisplayText(nameof(tb_Employee), nameof(tb_Employee.Employee_Name), SaleOrder.Employee_ID);

                        }


                        if (SaleOrder.tb_customervendor != null)
                        {
                            item.Cells[10].Value = SaleOrder.tb_customervendor.CVName;
                        }
                        else
                        {
                            item.Cells[10].Value = displayHelper.GetGridViewDisplayText(nameof(tb_CustomerVendor), nameof(tb_CustomerVendor.CustomerVendor_ID), SaleOrder.CustomerVendor_ID);
                        }
                        if (SaleOrder.tb_projectgroup != null)
                        {
                            item.Cells[11].Value = SaleOrder.tb_projectgroup.ProjectGroupName;
                        }
                        else
                        {
                            item.Cells[11].Value = displayHelper.GetGridViewDisplayText(nameof(tb_ProjectGroup), nameof(tb_ProjectGroup.ProjectGroup_ID), SaleOrder.ProjectGroup_ID);
                        }
                        item.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(SaleOrder.DataStatus), SaleOrder.DataStatus).ToString();
                        string project = string.Empty;
                        #region 加载订单单明细
                        List<tb_SaleOrderDetail> SaleOrderDetails = SaleOrder.tb_SaleOrderDetails;
                        foreach (tb_SaleOrderDetail SaleOrderDetail in SaleOrderDetails)
                        {
                            View_ProdDetail prodDetail = UIBizSrvice.GetProdDetail<View_ProdDetail>(SaleOrderDetail.ProdDetailID);
                            tb_ProductType productType = new tb_ProductType();
                            if (prodDetail.Type_ID.HasValue)
                            {
                                productType = UIBizSrvice.GetProdDetail<tb_ProductType>(prodDetail.Type_ID.Value);
                            }

                            project += $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.prop}" + ";";
                            //子级
                            KryptonTreeGridNodeRow PlanDetailsubrow = item.Nodes.Add(prodDetail.SKU);
                            PlanDetailsubrow.Cells[0].Tag = "SOrderNo";//保存列名 值对象=》tag(如果是明细则指向主表) 的列名。比方值是编号：则是PPNo
                            PlanDetailsubrow.Tag = SaleOrder;//为了双击的时候能找到值对象。这里还是给主表对象。
                            PlanDetailsubrow.Cells[1].Value = "";//分析日期
                            PlanDetailsubrow.Cells[2].Value = SaleOrderDetail.Quantity;
                            PlanDetailsubrow.Cells[3].Value = SaleOrderDetail.TotalDeliveredQty;
                            PlanDetailsubrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                            PlanDetailsubrow.Cells[6].Value = "订单产品";
                            PlanDetailsubrow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(SaleOrder.DataStatus), SaleOrder.DataStatus).ToString();


                            #region 在销售订单明细中加载出库单

                            List<tb_SaleOut> SaleOutList = SaleOrder.tb_SaleOuts;
                            if (SaleOutList != null && SaleOutList.Count > 0)
                            {
                                foreach (tb_SaleOut SaleOut in SaleOutList)
                                {
                                    foreach (tb_SaleOutDetail SaleOutDetail in SaleOut.tb_SaleOutDetails.Where(c => c.ProdDetailID == SaleOrderDetail.ProdDetailID && c.Location_ID == SaleOrderDetail.Location_ID))
                                    {
                                        //子级
                                        KryptonTreeGridNodeRow SaleOutDetailrow = PlanDetailsubrow.Nodes.Add(SaleOut.SaleOutNo.ToString());
                                        SaleOutDetailrow.Tag = SaleOut;//为了双击的时候能找到值对象。这里还是给主表对象。
                                        SaleOutDetailrow.Cells[0].Tag = "SaleOutNo";//保存列名 值对象的列名。比方值是编号：则是PDNo
                                        SaleOutDetailrow.Cells[1].Value = SaleOut.OutDate.ToString("yyyy-MM-dd");//出库日期
                                        //ProduceDetailrow.Cells[2].Value = ProduceDetail.Quantity;
                                        SaleOutDetailrow.Cells[3].Value = SaleOutDetail.Quantity;
                                        SaleOutDetailrow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        SaleOutDetailrow.Cells[6].Value = "出库";//$"{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                                        SaleOutDetailrow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(SaleOut.DataStatus), SaleOut.DataStatus).ToString();
                                        #region 销售退回单
                                        LoadSaleOutReData(SaleOut, SaleOutDetailrow, SaleOrderDetail, prodDetail, productType);
                                        #endregion
                                        #region 采购订单
                                        //  LoadSelfMadeProducts(SaleOut, ProduceDetail, PlanDetailsubrow, prodDetail);
                                        #endregion

                                    }
                                }

                            }
                        }
                        #endregion
                        project = project.TrimEnd(';');
                        item.Cells[3].Value = SaleOrder.tb_SaleOuts.Sum(c => c.TotalQty);//已发货数量
                        item.Cells[4].Value = project;
                    }
                    #endregion
                }
                else
                {
                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
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
            if (SaleOrderList.Count > 3)
            {
                kryptonTreeGridView1.CollapseAll();
            }
            else
            {
                kryptonTreeGridView1.ExpandAll();
            }
            return SaleOrderList.Count;
        }


        /// <summary>
        /// 加载子件的MO（成品 也一样）
        /// </summary>
        /// <param name="ProduceDetail"></param>
        /// <param name="ProduceDetailrow"></param>
        /// <param name="prodDetail"></param>
        private void LoadSaleOutReData(tb_SaleOut SaleOut, KryptonTreeGridNodeRow ProduceDetailrow, tb_SaleOrderDetail SaleOrderDetail, View_ProdDetail prodDetail, tb_ProductType productType)
        {
            if (SaleOut.tb_SaleOutRes == null)
            {
                return;
            }
            List<tb_SaleOutRe> SaleOutReList = SaleOut.tb_SaleOutRes.Where(c => c.SaleOut_NO == SaleOut.SaleOutNo).ToList();

            foreach (tb_SaleOutRe SaleOutRe in SaleOutReList)
            {
                KryptonTreeGridNodeRow SaleOutReRow = ProduceDetailrow.Nodes.Add(SaleOutRe.ReturnNo);
                SaleOutReRow.Tag = SaleOutRe;
                SaleOutReRow.Cells[0].Tag = "ReturnNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                SaleOutReRow.Cells[1].Value = SaleOutRe.Created_at.Value.ToString("yyyy-MM-dd"); //日期;
                SaleOutReRow.Cells[2].Value = SaleOutRe.TotalQty;
                // SaleOutReRow.Cells[3].Value = SaleOutRe.TotalQty;
                SaleOutReRow.Cells[4].Value = $"{productType.TypeName}:{prodDetail.CNName}{prodDetail.Specifications}{prodDetail.Model}{prodDetail.prop}";//项目
                SaleOutReRow.Cells[6].Value = "销售退回";
                SaleOutReRow.Cells[12].Value = UIHelper.GetDisplayText(UIBizSrvice.GetFixedDataDict(), nameof(SaleOutRe.DataStatus), SaleOutRe.DataStatus).ToString();
                //ProduceDetailrow.Cells[3].Value = SaleOutRe.QuantityDelivered;//制令单的交付数量显示到上级的需求上
                if (SaleOutRe.tb_SaleOutReDetails != null && SaleOutRe.tb_SaleOutReDetails.Count > 0)
                {
                    #region 退回明细
                    foreach (tb_SaleOutReDetail SaleOutReDetail in SaleOutRe.tb_SaleOutReDetails.Where(c => c.ProdDetailID == prodDetail.ProdDetailID && c.Location_ID == SaleOrderDetail.Location_ID))
                    {
                        KryptonTreeGridNodeRow SaleOutReDetailrow = SaleOutReRow.Nodes.Add(SaleOutRe.ReturnNo);
                        SaleOutReDetailrow.Tag = SaleOutRe;
                        SaleOutReDetailrow.Cells[0].Tag = "ReturnNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                        SaleOutReDetailrow.Cells[1].Value = SaleOutRe.ReturnDate.Value.ToString("yyyy-MM-dd");//日期
                        SaleOutReDetailrow.Cells[2].Value = SaleOutReDetail.Quantity;//预计数量
                        //订单明细数量只一行时。可能多次出库，多次退回，所以这里不能直接用SaleOrderDetail.qtY
                        //if (SaleOutReDetail.Quantity == SaleOrderDetail.qt)
                        //{
                        //    SaleOutReDetailrow.Cells[4].Value = "全部退回";
                        //}
                        SaleOutReDetailrow.Cells[6].Value = "退回明细";
                    }
                    #endregion
                }

                if (SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails != null && SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails.Count > 0)
                {
                    #region 退回翻新用料明细 
                    foreach (tb_SaleOutReRefurbishedMaterialsDetail MaterialsDetail in SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails)
                    {
                        KryptonTreeGridNodeRow MaterialsDetailrow = SaleOutReRow.Nodes.Add(SaleOutRe.ReturnNo);
                        MaterialsDetailrow.Tag = MaterialsDetail;//为了双击的时候能找到值对象。这里还是给主表对象。
                        MaterialsDetailrow.Cells[0].Tag = "ReturnNo";// 保存列名 值对象的列名。比方值是编号：则是PDNo
                        MaterialsDetailrow.Cells[1].Value = SaleOutRe.ReturnDate.ToString("yyyy-MM-dd"); //日期
                        MaterialsDetailrow.Cells[2].Value = MaterialsDetail.Quantity;


                        //if (MaterialsDetail.PayableQty == FinishedGoodsInvDetail.Qty)
                        //{
                        //    MaterialsDetailrow.Cells[4].Value = "全部缴库";
                        //}
                        //else if (MaterialsDetail.PayableQty > FinishedGoodsInvDetail.Qty)
                        //{
                        //    MaterialsDetailrow.Cells[4].Value = "部分缴库";
                        //}
                        //else if (0 == FinishedGoodsInvDetail.Qty)
                        //{
                        //    MaterialsDetailrow.Cells[4].Value = "未缴库";
                        //}

                        MaterialsDetailrow.Cells[6].Value = "缴库";


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

    class UCSaleCustomComparer : IComparer<KeyValuePair<string, string>>
    {
        private readonly string[] desiredOrder = { "SOrderNo", "SaleDate", "TotalQty",
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
