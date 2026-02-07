using AutoMapper;
using DevAge.Windows.Forms;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BI;
using RUINORERP.UI.Common;
using RUINORERP.UI.MRP.MP;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Report;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINOR.WinFormsUI.CustomPictureBox;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using Image = System.Drawing.Image;
using RUINORERP.PacketSpec.Models.FileManagement;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("费用报销单", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用报销, BizType.费用报销单)]
    public partial class UCExpenseClaim : BaseBillEditGeneric<tb_FM_ExpenseClaim, tb_FM_ExpenseClaimDetail>, IToolStripMenuInfoAuth
    {
        public UCExpenseClaim()
        {
            InitializeComponent();
        }
        public override void AddExcludeMenuList()
        {
            //通过付款单来联动结案
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.作废);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
        }


        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            //加载关联的单据
            if (base.EditEntity is tb_FM_ExpenseClaim expenseClaim)
            {
                //如果有出库，则查应收
                if (expenseClaim.DataStatus >= (int)DataStatus.确认)
                {
                    var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                    .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                        && c.SourceBizType == (int)BizType.费用报销单
                                                                    && c.SourceBillId == expenseClaim.ClaimMainID)
                                                                    .ToListAsync();
                    foreach (var item in receivablePayables)
                    {
                        var rqp = new Model.CommonModel.RelatedQueryParameter();
                        if (item.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {
                            rqp.bizType = BizType.应付款单;
                        }
                        else
                        {
                            rqp.bizType = BizType.应收款单;
                        }
                        rqp.billId = item.ARAPId;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        if (item.IsForCommission)
                        {
                            RelatedMenuItem.Text = $"{rqp.bizType}[佣金]:{item.ARAPNo}";
                        }
                        else
                        {
                            RelatedMenuItem.Text = $"{rqp.bizType}:{item.ARAPNo}";
                        }

                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ARAPId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }
            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }

        public override void BindData(tb_FM_ExpenseClaim entity, ActionStatus actionStatus)
        {

            if (entity == null)
            {
                return;
            }

            EditEntity = entity;
            if (entity.ClaimMainID > 0)
            {
                entity.PrimaryKeyID = entity.ClaimMainID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
                if (entity.DataStatus == (int)DataStatus.完结)
                {
                    cmbPayStatus.Visible = true;
                    lblPayStatus.Visible = true;
                    lblPaytype_ID.Visible = true;
                    cmbPaytype_ID.Visible = true;
                }
                else
                {
                    cmbPayStatus.Visible = false;
                    lblPayStatus.Visible = false;
                    lblPaytype_ID.Visible = false;
                    cmbPaytype_ID.Visible = false;
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                cmbPayStatus.Visible = false;
                lblPayStatus.Visible = false;
                lblPaytype_ID.Visible = false;
                cmbPaytype_ID.Visible = false;

                //默认优化报销自己
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    EditEntity.Employee_ID = entity.Employee_ID;
                }

                entity.DocumentDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.ClaimNo))
                {
                    entity.ClaimNo = ClientBizCodeService.GetBizBillNo(BizType.费用报销单);
                }

                //新增时，默认币别为人民币
                if (cmbCurrency_ID.Items.Count > 1)
                {
                    cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
                }
                entity.Currency_ID = MainForm.Instance.AppContext.BaseCurrency.Currency_ID;
            }

            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_ExpenseClaim, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全额预付, PayStatus.部分预付);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimNo, txtClaimNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.Employee_ID.HasValue && c.Employee_ID.Value == entity.Employee_ID);
            DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.DocumentDate, dtpDocumentDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimAmount.ToString(), txtClaimlAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_ExpenseClaimValidator>(), kryptonPanel1.Controls);
                //UIBaseTool uIBaseTool = new();
                //uIBaseTool.CurMenuInfo = CurMenuInfo;
                //uIBaseTool.AddEditableQueryControl<tb_Employee>(cmbEmployee_ID, false);
                LoadPayeeInfo(entity);
                #region 报销人 可以选择 可以添加
                var lambdaEmp = Expressionable.Create<tb_Employee>()
                                .And(t => t.Is_enabled == true)
                                .ToExpression();
                BaseProcessor baseProcessorEmp = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Employee).Name + "Processor");
                QueryFilter queryFilterEmp = baseProcessorEmp.GetQueryFilter();
                queryFilterEmp.FilterLimitExpressions.Add(lambdaEmp);
                DataBindingHelper.InitFilterForControlByExpCanEdit<tb_Employee>(entity, cmbEmployee_ID, c => c.Employee_Name, queryFilterEmp, true);

                #endregion
            }
            else
            {
                //加载收款信息
                if (entity.PayeeInfoID > 0)
                {
                    //cmbPayeeInfoID.SelectedIndex = cmbPayeeInfoID.FindStringExact(emp.Account_name);
                    var obj = _cacheManager.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_FM_PayeeInfo cv)
                        {
                            //添加收款信息。展示给财务看
                            if (!string.IsNullOrEmpty(cv.PaymentCodeImagePath))
                            {
                                btnInfo.Tag = cv;
                                btnInfo.Visible = true;
                            }
                            else
                            {
                                btnInfo.Tag = string.Empty;
                                btnInfo.Visible = false;
                            }
                        }
                    }
                }
            }
            if (entity.tb_FM_ExpenseClaimDetails != null && entity.tb_FM_ExpenseClaimDetails.Count > 0)
            {
                //新建和草稿时子表编辑也可以保存。
                foreach (var item in entity.tb_FM_ExpenseClaimDetails)
                {
                    item.PropertyChanged += (sender, s1) =>
                    {
                        //权限允许
                        if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                        {
                            EditEntity.ActionStatus = ActionStatus.修改;
                        }
                    };
                }
                sgh.LoadItemDataToGrid<tb_FM_ExpenseClaimDetail>(grid1, sgd, entity.tb_FM_ExpenseClaimDetails, c => c.ClaimSubID);
                // 模拟按下 Tab 键
                SendKeys.Send("{TAB}");//为了显示远程图片列
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_ExpenseClaimDetail>(grid1, sgd, new List<tb_FM_ExpenseClaimDetail>(), c => c.ClaimSubID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }
                if (entity.Employee_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ExpenseClaim>(c => c.Employee_ID))
                {
                    LoadPayeeInfo(entity);
                }

                //如果报销人有变化，带出对应的收款方式
                if (entity.PayeeInfoID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ExpenseClaim>(c => c.PayeeInfoID))
                {
                    var obj = _cacheManager.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_FM_PayeeInfo cv)
                        {

                            //添加收款信息。展示给财务看
                            if (!string.IsNullOrEmpty(cv.PaymentCodeImagePath))
                            {
                                btnInfo.Tag = cv;
                                btnInfo.Visible = true;
                            }
                            else
                            {
                                btnInfo.Tag = string.Empty;
                                btnInfo.Visible = false;
                            }
                        }

                    }
                }



            };

            //显示 打印状态 如果是草稿状态 不显示打印
            if ((DataStatus)EditEntity.DataStatus != DataStatus.草稿)
            {
                toolStripbtnPrint.Enabled = true;
                if (EditEntity.PrintStatus == 0)
                {
                    lblPrintStatus.Text = "未打印";
                }
                else
                {
                    lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
                }
            }
            else
            {
                toolStripbtnPrint.Enabled = false;
            }

            // 异步下载并显示结案凭证图片（不阻塞BindData方法）
            if (picboxCloseCaseImagePath != null)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await DownloadImageAsync(entity, picboxCloseCaseImagePath, c => c.CloseCaseImagePath);
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "异步下载结案凭证图片异常");
                    }
                });
            }

            // 异步下载并显示明细报销凭证图片
            if (entity.tb_FM_ExpenseClaimDetails != null && entity.tb_FM_ExpenseClaimDetails.Count > 0)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await LoadDetailImagesAsync(entity.tb_FM_ExpenseClaimDetails);
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "异步下载明细报销凭证图片异常");
                    }
                });
            }

            base.BindData(entity);
        }


        private void LoadPayeeInfo(tb_FM_ExpenseClaim entity)
        {
            cmbPayeeInfoID.DataBindings.Clear();
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.Employee_ID.HasValue && c.Employee_ID.Value == entity.Employee_ID);
            #region  收款信息可以根据报销人带出 ，并且可以添加

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PayeeInfo>()
                            .And(t => t.Is_enabled == true)
                            .And(t => t.Employee_ID == entity.Employee_ID)//限制了只能处理自己 的收款信息
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterC, true);

            #endregion
        }

        /// <summary>
        /// 异步加载明细报销凭证图片
        /// </summary>
        /// <param name="details">报销明细列表</param>
        private async Task LoadDetailImagesAsync(List<tb_FM_ExpenseClaimDetail> details)
        {
            if (details == null || details.Count == 0) return;

            try
            {
                int loadedCount = 0;
                var fileService = Startup.GetFromFac<FileBusinessService>();

                foreach (var detail in details)
                {
                    if (!string.IsNullOrEmpty(detail.EvidenceImagePath))
                    {
                        try
                        {
                            // 查找对应的网格行
                            int rowIndex = FindGridRowIndex(detail);
                            if (rowIndex > 0)
                            {
                                int colIndex = GetEvidenceImageColumnIndex();
                                if (colIndex >= 0)
                                {
                                    // 下载图片
                                    var imageData = await DownloadDetailImageAsync(detail, fileService);
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        // 更新单元格显示
                                        await UpdateGridCellImageAsync(rowIndex, colIndex, imageData, detail.EvidenceImagePath);
                                        loadedCount++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError(ex, $"加载明细 {detail.ClaimSubID} 的图片失败");
                        }
                    }
                }

                if (loadedCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功加载 {loadedCount} 张明细报销凭证图片");
                    // 刷新网格显示
                    this.Invoke(new Action(() =>
                    {
                        grid1.Invalidate();
                    }));
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "加载明细报销凭证图片异常");
            }
        }

        /// <summary>
        /// 查找明细对应的网格行索引
        /// </summary>
        private int FindGridRowIndex(tb_FM_ExpenseClaimDetail detail)
        {
            try
            {
                for (int i = 1; i < grid1.RowsCount; i++)
                {
                    var rowData = grid1.Rows[i].RowData;
                    if (rowData is tb_FM_ExpenseClaimDetail rowDetail)
                    {
                        if (rowDetail.ClaimSubID == detail.ClaimSubID)
                        {
                            return i;
                        }
                    }
                }
            }
            catch { }
            return -1;
        }

        /// <summary>
        /// 获取报销凭证图片列的索引
        /// </summary>
        private int GetEvidenceImageColumnIndex()
        {
            try
            {
                var col = sgd["EvidenceImagePath"];
                if (col != null)
                {
                    var colInfo = grid1.Columns.GetColumnInfo(col.UniqueId);
                    if (colInfo != null)
                    {
                        return colInfo.Index;
                    }
                }
            }
            catch { }
            return -1;
        }

        /// <summary>
        /// 下载明细图片
        /// </summary>
        private async Task<byte[]> DownloadDetailImageAsync(tb_FM_ExpenseClaimDetail detail, FileBusinessService fileService)
        {
            try
            {
                var downloadResponse = await fileService.DownloadImageAsync(detail, "EvidenceImagePath");
                if (downloadResponse != null && downloadResponse.Count > 0)
                {
                    var firstResponse = downloadResponse[0];
                    if (firstResponse.IsSuccess && firstResponse.FileStorageInfos != null && firstResponse.FileStorageInfos.Count > 0)
                    {
                        return firstResponse.FileStorageInfos[0].FileData;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, $"下载明细 {detail.ClaimSubID} 图片失败");
            }
            return null;
        }

        /// <summary>
        /// 更新网格单元格图片
        /// </summary>
        private async Task UpdateGridCellImageAsync(int rowIndex, int colIndex, byte[] imageData, string imagePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        var cell = grid1[rowIndex, colIndex];
                        if (cell != null)
                        {
                            // 获取或创建 ValueImageWeb 模型
                            var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb;
                            if (model == null)
                            {
                                valueImageWeb = new SourceGrid.Cells.Models.ValueImageWeb();
                                cell.Model.AddModel(valueImageWeb);
                            }
                            else
                            {
                                valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                            }

                            // 设置图片数据
                            valueImageWeb.CellImageBytes = imageData;
                            valueImageWeb.CellImageHashName = imagePath;

                            // 设置单元格值
                            cell.Value = imagePath;

                            // 设置视图
                            if (!(cell.View is SourceGrid.Cells.Views.RemoteImageView))
                            {
                                cell.View = new SourceGrid.Cells.Views.RemoteImageView();
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "更新网格单元格图片失败");
                }
            });
        }

        /// <summary>
        /// 加载图片数据
        /// </summary>
        /// <param name="CloseCaseImagePath">结案凭证图片路径</param>
        private async Task LoadImageData(string CloseCaseImagePath)
        {
            if (!string.IsNullOrWhiteSpace(CloseCaseImagePath))
            {
                // 检查是否为多图片路径（包含分号分隔的多个路径）
                if (CloseCaseImagePath.Contains(";"))
                {
                    // 启用多图片支持模式
                    picboxCloseCaseImagePath.MultiImageSupport = true;
                    picboxCloseCaseImagePath.ImagePaths = CloseCaseImagePath;
                }
                else
                {
                    // 单图片模式 - 从文件服务器下载图片
                    picboxCloseCaseImagePath.MultiImageSupport = false;

                    try
                    {
                        var ctrpay = Startup.GetFromFac<FileBusinessService>();
                        var list = await ctrpay.DownloadImageAsync(EditEntity);

                        if (list != null && list.Count > 0)
                        {
                            List<byte[]> imageDataList = new List<byte[]>();
                            List<ImageInfo> imageInfos = new List<ImageInfo>();

                            foreach (var downloadResponse in list)
                            {
                                if (downloadResponse.IsSuccess && downloadResponse.FileStorageInfos != null)
                                {
                                    foreach (var fileStorageInfo in downloadResponse.FileStorageInfos)
                                    {
                                        if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                                        {
                                            imageDataList.Add(fileStorageInfo.FileData);
                                            imageInfos.Add(ctrpay.ConvertToImageInfo(fileStorageInfo));
                                        }
                                    }
                                }
                            }

                            if (imageDataList.Count > 0)
                            {
                                picboxCloseCaseImagePath.LoadImages(imageDataList, imageInfos, true);
                                picboxCloseCaseImagePath.Visible = true;
                                MainForm.Instance.uclog.AddLog($"成功加载 {imageDataList.Count} 张结案凭证图片");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "下载结案凭证图片异常");
                        MainForm.Instance.uclog.AddLog($"下载结案凭证图片出错：{ex.Message}");
                    }
                }
            }
            else
            {
                picboxCloseCaseImagePath.Visible = false;
            }
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ExpenseClaim).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ExpenseClaim>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }

        private void Grid1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail> dc = Startup.GetFromFac<tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail>>();
        List<tb_FM_ExpenseClaimDetail> list = new List<tb_FM_ExpenseClaimDetail>();
        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UCStockIn_Load(object sender, EventArgs e)
        {
            if (CurMenuInfo != null)
            {
                lbl盘点单.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();


            // 为结案凭证图片控件添加双击事件，支持上传图片
            picboxCloseCaseImagePath.DoubleClick += MagicPictureBox1_DoubleClick;

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_ExpenseClaimDetail>();
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimMainID);
            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimSubID);
            listCols.SetCol_DefaultValue<tb_FM_ExpenseClaimDetail>(c => c.TaxRate, 0.00);
            listCols.SetCol_DefaultValue<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, 0.00M);

            listCols.SetCol_ReadOnly<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount);
            listCols.SetCol_ReadOnly<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.SingleAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, CustomFormatType.CurrencyFormat);
            //            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImage, CustomFormatType.Image);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImagePath, CustomFormatType.WebPathImage);

            listCols.SetCol_DataFilter<tb_FM_ExpenseClaimDetail, tb_FM_ExpenseType>(c => c.ExpenseType_id,
                 DataFilter<tb_FM_ExpenseType>.Where(p => p.ReceivePaymentType == (int)ReceivePaymentType.付款)
                 );

            sgd = new SourceGridDefine(grid1, listCols, true);

            sgd.GridMasterData = EditEntity;
    
            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b, c) => a.SingleAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.SingleAmount - b.TaxAmount, c => c.UntaxedAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.SingleAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount);

            //应该只提供一个结构
            List<tb_FM_ExpenseClaimDetail> lines = new List<tb_FM_ExpenseClaimDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ExpenseClaimDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;

            // 添加网格双击事件处理（用于图片编辑）
            grid1.MouseDoubleClick += Grid1_MouseDoubleClick;

            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private void Sgh_OnAddDataRow(object rowObj)
        {



        }

        /// <summary>
        /// 网格鼠标双击事件处理
        /// 用于处理报销凭证图片的双击编辑
        /// </summary>
        private void Grid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取点击位置对应的单元格
                var position = grid1.PositionAtPoint(e.Location);
                if (position.IsEmpty()) return;

                // 检查是否是报销凭证图片列
                var col = sgd["EvidenceImagePath"];
                if (col == null) return;

                var colInfo = grid1.Columns.GetColumnInfo(col.UniqueId);
                if (colInfo == null || position.Column != colInfo.Index) return;

                // 获取单元格
                var cell = grid1[position];
                if (cell == null) return;

                // 检查是否有图片
                bool hasImage = false;
                var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                {
                    hasImage = valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0;
                }

                if (hasImage)
                {
                    // 显示右键菜单选项
                    ShowImageContextMenu(position.Row, position.Column, e.Location);
                }
                else
                {
                    // 没有图片，进入编辑模式
                    grid1.Selection.SelectCell(position, true);
                    // 使用网格的编辑功能启动编辑
                    var editContext = new SourceGrid.CellContext(grid1, position);
                    if (cell.Editor != null)
                    {
                        cell.Editor.EnableEdit = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理网格双击事件失败");
            }
        }

        /// <summary>
        /// 显示图片右键菜单
        /// </summary>
        private void ShowImageContextMenu(int rowIndex, int colIndex, Point location)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            // 查看大图
            ToolStripMenuItem viewItem = new ToolStripMenuItem("查看大图");
            viewItem.Click += (s, e) => ViewDetailImage(rowIndex, colIndex);
            menu.Items.Add(viewItem);

            // 替换图片
            ToolStripMenuItem replaceItem = new ToolStripMenuItem("替换图片");
            replaceItem.Click += async (s, e) => await ReplaceDetailImageFromDialog(rowIndex);
            menu.Items.Add(replaceItem);

            menu.Items.Add(new ToolStripSeparator());

            // 删除图片
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("删除图片");
            deleteItem.Click += (s, e) => DeleteDetailImage(rowIndex, colIndex);
            menu.Items.Add(deleteItem);

            // 显示菜单
            menu.Show(grid1, location);
        }

        /// <summary>
        /// 查看明细大图
        /// </summary>
        private void ViewDetailImage(int rowIndex, int colIndex)
        {
            try
            {
                var cell = grid1[rowIndex, colIndex];
                if (cell == null) return;

                var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                {
                    if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                    {
                        using (var ms = new System.IO.MemoryStream(valueImageWeb.CellImageBytes))
                        {
                            var image = System.Drawing.Image.FromStream(ms);
                            frmPictureViewer viewer = new frmPictureViewer();
                            viewer.PictureBoxViewer.Image = image;
                            viewer.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "查看明细大图失败");
                MessageBox.Show($"查看图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 从对话框替换明细图片
        /// </summary>
        private async Task ReplaceDetailImageFromDialog(int rowIndex)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp|所有文件|*.*";
                    dlg.Title = "选择新的报销凭证图片";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // 使用 Task.Run 包装同步方法以实现异步
                        byte[] imageData = await Task.Run(() => System.IO.File.ReadAllBytes(dlg.FileName));
                        string fileName = System.IO.Path.GetFileName(dlg.FileName);
                        await ReplaceDetailImage(rowIndex, imageData, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "替换图片失败");
                MessageBox.Show($"替换图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_FM_ExpenseClaimDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_ExpenseClaimDetail>;
                details = details.Where(c => c.SingleAmount != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("单项金额必须大于0");
                    return;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.ClaimAmount = details.Sum(c => c.SingleAmount);
                EditEntity.UntaxedAmount = details.Sum(C => C.UntaxedAmount);

                //添加总计金额小于0的提示
                if (EditEntity.ClaimAmount < 0)
                {
                    MainForm.Instance.uclog.AddLog("警告：报销费用总计小于0，请调整明细金额！", Global.UILogType.警告);
                }

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }



        List<tb_FM_ExpenseClaimDetail> details = new List<tb_FM_ExpenseClaimDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtClaimlAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ExpenseClaimDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ExpenseClaimDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.SingleAmount != 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("明细记录中，【单项金额】不能为零，请录入有效记录！");
                    return false;
                }

                if (details.Sum(c => c.TaxAmount) > 0)
                {
                    EditEntity.IncludeTax = true;
                }

                EditEntity.tb_FM_ExpenseClaimDetails = details;
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.ClaimAmount = details.Sum(c => c.SingleAmount);
                EditEntity.UntaxedAmount = details.Sum(C => C.UntaxedAmount);

                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.ClaimAmount != details.Sum(c => c.SingleAmount))
                {
                    if (MessageBox.Show("总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.UntaxedAmount != details.Sum(c => c.UntaxedAmount))
                {
                    if (MessageBox.Show("未税总金额和明细未税金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (NeedValidated && EditEntity.ClaimAmount != details.Sum(c => c.SingleAmount))
                {
                    if (MessageBox.Show("核准总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.ClaimAmount < 0)
                {
                    //总计金额不能为负数，强制不允许保存
                    MessageBox.Show("报销费用总计不能小于0，请调整明细金额！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_ExpenseClaimDetail>(details))
                {
                    return false;
                }

                // 处理结案凭证图片上传
                if (NeedValidated && picboxCloseCaseImagePath.Image != null)
                {
                    string fileName = $"CloseCase_{EditEntity.ClaimNo}_{DateTime.Now:yyyyMMddHHmmss}.png";
                    string fileId = "";// await UploadCloseCaseImage(magicPictureBox1.Image, fileName);
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        EditEntity.CloseCaseImagePath = fileId;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("结案凭证图片上传失败。", Global.UILogType.错误);
                        // 根据业务需求决定是否阻止保存
                        // return false;
                    }
                }

                if (NeedValidated)
                {
                    // 处理明细凭证图片上传（使用文件服务器方式）
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_ExpenseClaimDetails);
                    if (uploadImg)
                    {
                        MainForm.Instance.PrintInfoLog($"明细凭证图片保存成功。");
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("明细凭证图片上传出错。");
                        return false;
                    }
                }

                ReturnMainSubResults<tb_FM_ExpenseClaim> SaveResult = new ReturnMainSubResults<tb_FM_ExpenseClaim>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        EditEntity.AcceptChanges();
                        EditEntity.tb_FM_ExpenseClaimDetails.ForEach(c => c.AcceptChanges());

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ClaimNo}。");

                        // 保存成功后上传结案凭证图片（只上传变更的图片）
                        if (picboxCloseCaseImagePath != null)
                        {
                            var updatedImages = picboxCloseCaseImagePath.GetImagesNeedingUpdate();
                            if (updatedImages.Count > 0)
                            {
                                await UploadImageAsync(EditEntity, picboxCloseCaseImagePath, "结案凭证");
                            }
                        }

                        // 清理已替换的旧图片
                        await CleanupOldImagesAsync();
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }
            else
            {
                MainForm.Instance.uclog.AddLog("加载状态下无法保存");
                return false;
            }
        }





        /// <summary>
        /// 删除报销单 - 重写基类方法
        /// </summary>
        /// <returns>删除结果</returns>
        protected async override Task<ReturnResults<tb_FM_ExpenseClaim>> Delete()
        {
            ReturnResults<tb_FM_ExpenseClaim> rss = new ReturnResults<tb_FM_ExpenseClaim>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(EditEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.EmpID)
                        {
                            MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_ExpenseClaimController<tb_FM_ExpenseClaim> ctr = Startup.GetFromFac<tb_FM_ExpenseClaimController<tb_FM_ExpenseClaim>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_ExpenseClaim);
                    if (rs)
                    {
                        // 删除成功后，清空图片控件
                        if (picboxCloseCaseImagePath != null)
                        {
                            picboxCloseCaseImagePath.ClearImage();
                        }

                        // 删除远程图片
                        var ctrpay = Startup.GetFromFac<FileBusinessService>();
                        await ctrpay.DeleteImagesAsync(EditEntity, false);

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        Exit(this);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                }
            }
            return rss;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (sender is KryptonButton btninfo)
            {
                if (btninfo.Tag != null)
                {
                    if (!string.IsNullOrWhiteSpace(btninfo.Tag.ToString()))
                    {
                        //tb_FM_PayeeInfo payeeInfo = btninfo.Tag as tb_FM_PayeeInfo;

                        #region 显示收款详情信息

                        object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
                        if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                            frmaddg.CurMenuInfo = this.CurMenuInfo;
                            frmaddg.Text = "收款账号详情";
                            frmaddg.bindingSourceEdit.DataSource = new List<tb_FM_PayeeInfo>();
                            object obj = frmaddg.bindingSourceEdit.AddNew();
                            obj = btninfo.Tag;
                            tb_FM_PayeeInfo payeeInfo = obj as tb_FM_PayeeInfo;
                            BaseEntity bty = payeeInfo as BaseEntity;
                            bty.ActionStatus = ActionStatus.加载;
                            frmaddg.BindData(bty);
                            if (frmaddg.ShowDialog() == DialogResult.OK)
                            {

                            }
                        }
                        #endregion
                        //HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //try
                        //{
                        //    byte[] img = await httpWebService.DownloadImgFileAsync(btninfo.Tag.ToString());
                        //    frmPictureViewer pictureViewer = new frmPictureViewer();
                        //    pictureViewer.PictureBoxViewer.Image = ImageHelper.byteArrayToImage(img);
                        //    pictureViewer.ShowDialog();
                        //}
                        //catch (Exception ex)
                        //{
                        //    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                        //}
                    }

                }
            }
        }

        /// <summary>
        /// 结案凭证图片控件双击事件处理
        /// </summary>
        private void MagicPictureBox1_DoubleClick(object sender, EventArgs e)
        {
            // MagicPictureBox已经内置了文件服务器的上传和下载功能
            // 无需额外处理，双击即可触发内置的上传/查看功能
        }

        #region 明细图片删除和替换功能

        /// <summary>
        /// 删除明细图片
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param>
        public void DeleteDetailImage(int rowIndex, int colIndex)
        {
            try
            {
                if (rowIndex <= 0 || rowIndex >= grid1.RowsCount) return;
                if (colIndex < 0 || colIndex >= grid1.ColumnsCount) return;

                var cell = grid1[rowIndex, colIndex];
                if (cell == null) return;

                // 确认删除
                if (MessageBox.Show("确定要删除这张报销凭证图片吗？", "确认删除", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                // 获取模型数据
                var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                {
                    // 记录需要删除的旧文件
                    string oldImagePath = valueImageWeb.CellImageHashName;
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        // 将旧文件路径添加到待删除列表
                        AddImageToDeleteList(oldImagePath);
                    }

                    // 清空图片数据
                    valueImageWeb.CellImageBytes = null;
                    valueImageWeb.SetImageNewHash(string.Empty);
                    cell.Value = null;
                    cell.View = sgd.ViewNormal;

                    // 更新明细对象
                    var rowData = grid1.Rows[rowIndex].RowData;
                    if (rowData is tb_FM_ExpenseClaimDetail detail)
                    {
                        detail.EvidenceImagePath = null;
                        detail.SetPropertyValue("EvidenceImagePath", null);
                    }

                    // 刷新单元格
                    var position = new SourceGrid.Position(rowIndex, colIndex);
                    grid1.InvalidateCell(position);

                    MainForm.Instance.uclog.AddLog("报销凭证图片已删除");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "删除明细图片失败");
                MessageBox.Show($"删除图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 待删除图片列表
        /// </summary>
        private List<string> _imagesToDelete = new List<string>();

        /// <summary>
        /// 添加图片到待删除列表
        /// </summary>
        private void AddImageToDeleteList(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && !_imagesToDelete.Contains(imagePath))
            {
                _imagesToDelete.Add(imagePath);
            }
        }

        /// <summary>
        /// 删除服务器上的旧图片
        /// </summary>
        private async Task DeleteOldImagesAsync()
        {
            if (_imagesToDelete == null || _imagesToDelete.Count == 0) return;

            try
            {
                var fileService = Startup.GetFromFac<FileBusinessService>();
                int successCount = 0;

                foreach (var imagePath in _imagesToDelete.ToList())
                {
                    try
                    {
                        // 构建删除请求
                        var deleteRequest = new FileDeleteRequest
                        {
                            BusinessNo = EditEntity?.ClaimNo ?? EditEntity?.ClaimMainID.ToString(),
                            BusinessId = EditEntity?.ClaimMainID ?? 0,
                            BusinessType = (int)BizType.费用报销单,
                            PhysicalDelete = false // 逻辑删除
                        };

                        // 注意：这里需要根据实际的文件存储信息构建FileStorageInfo
                        // 简化处理：直接调用删除方法
                        var deleteResult = await fileService.DeleteImagesAsync(EditEntity, false);
                        if (deleteResult != null && deleteResult.IsSuccess)
                        {
                            successCount++;
                            _imagesToDelete.Remove(imagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, $"删除图片 {imagePath} 失败");
                    }
                }

                if (successCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功删除 {successCount} 张旧图片");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "删除旧图片异常");
            }
        }

        /// <summary>
        /// 替换明细图片
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="newImageData">新图片数据</param>
        /// <param name="fileName">文件名</param>
        public async Task ReplaceDetailImage(int rowIndex, byte[] newImageData, string fileName)
        {
            try
            {
                if (rowIndex <= 0 || rowIndex >= grid1.RowsCount) return;

                int colIndex = GetEvidenceImageColumnIndex();
                if (colIndex < 0) return;

                var cell = grid1[rowIndex, colIndex];
                if (cell == null) return;

                // 获取旧图片路径
                var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                {
                    string oldImagePath = valueImageWeb.CellImageHashName;
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        // 将旧文件添加到待删除列表
                        AddImageToDeleteList(oldImagePath);
                    }

                    // 设置新图片
                    string newHash = ImageHashHelper.GenerateHash(newImageData);
                    valueImageWeb.SetImageNewHash(newHash);
                    valueImageWeb.CellImageBytes = newImageData;
                    valueImageWeb.CellImageHashName = fileName;
                    cell.Value = fileName;

                    // 设置视图
                    if (!(cell.View is SourceGrid.Cells.Views.RemoteImageView))
                    {
                        cell.View = new SourceGrid.Cells.Views.RemoteImageView();
                    }

                    // 更新明细对象
                    var rowData = grid1.Rows[rowIndex].RowData;
                    if (rowData is tb_FM_ExpenseClaimDetail detail)
                    {
                        detail.EvidenceImagePath = fileName;
                    }

                    // 刷新单元格
                    var position = new SourceGrid.Position(rowIndex, colIndex);
                    grid1.InvalidateCell(position);

                    MainForm.Instance.uclog.AddLog("报销凭证图片已替换");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "替换明细图片失败");
                MessageBox.Show($"替换图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 检查并删除已替换的旧图片（在保存后调用）
        /// </summary>
        public async Task CleanupOldImagesAsync()
        {
            await DeleteOldImagesAsync();
        }

        #endregion

    }
}


