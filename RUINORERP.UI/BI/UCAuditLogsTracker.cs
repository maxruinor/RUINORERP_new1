using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.Business.CommService;
using HLH.Lib.List;
using AutoMapper;
using Newtonsoft.Json;
using RUINORERP.Global;
using Microsoft.Extensions.Logging;
using System.Security.AccessControl;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("审计日志业务轨迹跟踪器", true, UIType.单表数据)]
    public partial class UCAuditLogsTracker : BaseEditGeneric<tb_AuditLogs>
    {
        public UCAuditLogsTracker()
        {
            InitializeComponent();
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            mapper = Startup.GetFromFac<BizTypeMapper>();
        }
        BizTypeMapper mapper = null;
        List<tb_AuditLogs> _AuditLogs = new List<tb_AuditLogs>();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();

        public List<tb_AuditLogs> AuditLogs { get => _AuditLogs; set => _AuditLogs = value; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                //bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            //首先通过条件 找到 明细的ProdDetailID

            AuditLogs = AuditLogs.OrderBy(c => c.ActionTime).ToList();
            BindingSortCollection<TrackerResult> ListDataSoure = new BindingSortCollection<TrackerResult>();
            var prodDetail = MainForm.Instance.list.Where(c => c.SKU == txtSKU.Text).FirstOrDefault();
            if (prodDetail != null)
            {
                // 过滤出包含指定ProdDetailID且是审核操作的记录
                var filteredLogs = AuditLogs
                    .Where(log => log.DataContent != null &&
                                log.DataContent.Contains(prodDetail.ProdDetailID.ToString()) &&
                                log.ActionType == "审核")
                    .ToList();

                // 按业务单号分组，获取每个业务单号的最后一次审核记录
                var lastAuditLogs = filteredLogs
                    .GroupBy(log => log.ObjectNo)
                    .Select(group => group.OrderByDescending(log => log.ActionTime).First())
                    .ToList();

                foreach (var AuditLog in lastAuditLogs)
                {
                    if (AuditLog.DataContent == null)
                    {
                        continue;
                    }
                 
                        try
                        {

                            #region  恢复单据
                            BizType bizType = (BizType)AuditLog.ObjectType.Value;
                            Type objType = mapper.GetTableType(bizType);

                            if (objType == null)
                            {
                                continue;
                            }
                            // 恢复实体对象
                            // 解析JSON数据
                            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(AuditLog.DataContent);
                            var entity = EntityDataRestorer.RestoreEntity(objType, data);
                            #endregion
                            switch (bizType)
                            {
                                case BizType.无对应数据:
                                    break;
                                case BizType.销售订单:
                                    break;
                                case BizType.销售出库单:
                                    tb_SaleOut saleOut = entity as tb_SaleOut;
                                    foreach (var item in saleOut.tb_SaleOutDetails)
                                    {
                                        if (item.ProdDetailID==prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "销售出库";
                                            result.Qty = -item.Quantity;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                       
                                    }
                                    break;
                                case BizType.销售退回单:
                                    tb_SaleOutRe saleOutRe = entity as tb_SaleOutRe;
                                    foreach (var item in saleOutRe.tb_SaleOutReDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "销售退回";
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.Qty = item.Quantity;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;
                                case BizType.采购入库单:
                                    tb_PurEntry purEntry = entity as tb_PurEntry;
                                    foreach (var item in purEntry.tb_PurEntryDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "采购入库";
                                            result.Qty = item.Quantity;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;
                                case BizType.采购退货单:
                                    tb_PurEntryRe purEntryRe = entity as tb_PurEntryRe;
                                    foreach (var item in purEntryRe.tb_PurEntryReDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "采购退货";
                                            result.Qty = -item.Quantity;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;
                                case BizType.其他入库单:
                                    tb_StockIn stockIn = entity as tb_StockIn;
                                    foreach (var item in stockIn.tb_StockInDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "其他入库";
                                            result.Qty = item.Qty;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;
                                case BizType.其他出库单:
                                    tb_StockOut stockOut = entity as tb_StockOut;
                                    foreach (var item in stockOut.tb_StockOutDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "其他出库";
                                            result.Qty = -item.Qty;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;

                                case BizType.报损单:
                                    break;
                                case BizType.报溢单:
                                    break;
                                case BizType.盘点单:
                                    tb_Stocktake stocktake = entity as tb_Stocktake;
                                    foreach (var item in stocktake.tb_StocktakeDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "盘点";
                                            result.Qty = item.DiffQty;
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;
                         
                             
                                case BizType.生产领料单:
                                    break;
                                case BizType.生产退料单:
                                    break;
                                case BizType.生产补料单:
                                    break;


                                case BizType.托外加工单:
                                    break;
                                case BizType.托外领料单:
                                    break;
                                case BizType.退料单:
                                    break;
                                case BizType.托外补料单:
                                    break;
                                case BizType.托外加工缴回单:
                                    break;

                                case BizType.费用报销单:
                                    break;
                                case BizType.库存查询:
                                    break;

                                case BizType.缴库单:
                                    tb_FinishedGoodsInv finishedGoodsInv = entity as tb_FinishedGoodsInv;
                                    foreach (var item in finishedGoodsInv.tb_FinishedGoodsInvDetails)
                                    {
                                        if (item.ProdDetailID == prodDetail.ProdDetailID)
                                        {
                                            TrackerResult result = new();
                                            result.BillNo = AuditLog.ObjectNo;
                                            result.Sku = prodDetail.SKU;
                                            result.ProdName = prodDetail.CNName;
                                            result.OperateType = "成品缴库";
                                            result.Qty = item.Qty;
                                            result.ActionTime = AuditLog.ActionTime.Value;
                                            result.Prodetailid = prodDetail.ProdDetailID;
                                            ListDataSoure.Add(result);
                                        }
                                    }
                                    break;

                                case BizType.产品分割单:
                                    break;
                                case BizType.产品组合单:
                                    break;
                                case BizType.借出单:
                                    break;
                                case BizType.归还单:
                                    break;
                                case BizType.套装组合:
                                    break;

                                case BizType.产品转换单:
                                    break;
                                case BizType.调拨单:
                                    break;
                                case BizType.采购退货入库:
                                    break;
                                case BizType.售后返厂退回:
                                    break;
                                case BizType.售后返厂入库:
                                    break;

                                case BizType.返工退库单:
                                    break;

                                case BizType.返工入库单:
                                    break;


                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError("审核跟踪时出错。", ex);
                        }

                   
                }

                //统计汇总
                TrackerResult SumResult = new();
                SumResult.ProdName = "行数：" + ListDataSoure.Count.ToString();
                SumResult.OperateType = "数量汇总";
                SumResult.Qty = ListDataSoure.Sum(c => c.Qty);
                ListDataSoure.Add(SumResult);

                dataGridView1.DataSource = ListDataSoure;
            }



            //AuditLogs
        }



        #region 画行号

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

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
        }

        #endregion



    }


    public class TrackerResult
    {
        private string sku;
        private string prodName;
        private int qty;
        private long prodetailid;
        private string _OperateType;
        private DateTime _actionTime;
        private string _billNo;
        public int Qty { get => qty; set => qty = value; }

        public string Sku { get => sku; set => sku = value; }
        public string ProdName { get => prodName; set => prodName = value; }

        public long Prodetailid { get => prodetailid; set => prodetailid = value; }
        public string OperateType { get => _OperateType; set => _OperateType = value; }
        public DateTime ActionTime { get => _actionTime; set => _actionTime = value; }
        public string BillNo { get => _billNo; set => _billNo = value; }
    }


}
