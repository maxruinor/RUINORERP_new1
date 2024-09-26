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
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using Krypton.Toolkit;
using NPOI.SS.Formula.Functions;
using Netron.GraphLib;
using RUINORERP.UI.SysConfig;
using SourceGrid.Cells.Editors;
using RUINORERP.UI.MRP.MP;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("费用报销单", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收付账款, BizType.费用报销单)]
    public partial class UCExpenseClaim : BaseBillEditGeneric<tb_FM_ExpenseClaim, tb_FM_ExpenseClaimDetail>
    {
        public UCExpenseClaim()
        {
            InitializeComponent();
            base.OnBindDataToUIEvent += UCStockIn_OnBindDataToUIEvent;

        }
        public void UCStockIn_OnBindDataToUIEvent(object entity)
        {
            BindData(entity as tb_FM_ExpenseClaim);
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
        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_FM_ExpenseClaim);
        }
        public void BindData(tb_FM_ExpenseClaim entity)
        {
            if (entity == null)
            {
                chkClaimEmployee.Enabled = false;
                return;
            }

            EditEntity = entity;
            if (entity.ClaimMainID > 0)
            {
                entity.PrimaryKeyID = entity.ClaimMainID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                chkClaimEmployee.Enabled = true;
                entity.DataStatus = (int)DataStatus.草稿;
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                }
                entity.DocumentDate = System.DateTime.Now;
                if (CurMenuInfo != null)
                {
                    entity.ClaimNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.费用报销单);
                }
            }


            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimNo, txtClaimNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
            DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.DocumentDate, dtpDocumentDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimlAmount.ToString(), txtClaimlAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovedAmount.ToString(), txtApprovedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.IncludeTax, chkIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            txtstatus.ReadOnly = true;
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
                            chkClaimEmployee.Enabled = true;
                        }
                    };
                }
                sgh.LoadItemDataToGrid<tb_FM_ExpenseClaimDetail>(grid1, sgd, entity.tb_FM_ExpenseClaimDetails, c => c.ClaimSubID);
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
                    chkClaimEmployee.Enabled = true;
                }

                base.ToolBarEnabledControl(entity);
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
            };
            // 模拟按下 Tab 键
            SendKeys.Send("{TAB}");//为了显示远程图片列
        }

        private void Grid1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail> dc = Startup.GetFromFac<tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail>>();
        List<tb_FM_ExpenseClaimDetail> list = new List<tb_FM_ExpenseClaimDetail>();
        List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
        private void UCStockIn_Load(object sender, EventArgs e)
        {
            if (CurMenuInfo != null)
            {
                lbl盘点单.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_ExpenseClaimDetail>();

            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimMainID);
            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimSubID);
            listCols.SetCol_DefaultValue<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, 0.00M);

            //listCols.SetCol_ReadOnly<tb_FM_OtherExpenseDetail>(c => c.CNName);

            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImage, CustomFormatType.Image);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImagePath, CustomFormatType.WebPathImage);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.SubtotalPirceAmount);
            }*/
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.TotalAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount);

            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b, c) => a.TotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.TotalAmount - b.TaxAmount, c => c.UntaxedAmount);

            ////反算成交单价，目标列能重复添加。已经优化好了。
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.SubtotalAmount / b.Quantity, c => c.TransactionPrice);//-->成交价是结果列
            ////反算折扣
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);

            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);



            //应该只提供一个结构
            List<tb_FM_ExpenseClaimDetail> lines = new List<tb_FM_ExpenseClaimDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ExpenseClaimDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
        }

        private void Sgh_OnAddDataRow(object rowObj)
        {
            if (chkClaimEmployee.Checked)
            {
                sgh.SetCellValue<tb_FM_ExpenseClaimDetail>(sgd, c => c.Employee_ID, EditEntity.Employee_ID);
            }

            // 获取应用程序的执行目录下的 temp 文件夹路径
            // string tempPath = DirectoryHelper.GetTempPath();
            // 生成唯一的文件名
            // string newFileName = Guid.NewGuid().ToString() + ".jpg"; // 假设保存为 JPG 格式
            //sgh.SetCellValue<tb_FM_ExpenseClaimDetail>(sgd, c => c.EvidenceImagePath, newFileName);
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
                details = details.Where(c => c.TotalAmount > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.ClaimlAmount = details.Sum(c => c.TotalAmount);
                EditEntity.ApprovedAmount = EditEntity.ClaimlAmount;
                EditEntity.UntaxedAmount = details.Sum(C => C.UntaxedAmount);
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
            ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
            var temppath = configManager.GetValue("LocImageDirectory");
            if (string.IsNullOrEmpty(temppath))
            {
                MainForm.Instance.uclog.AddLog("请先配置图片存储路径", UILogType.错误);
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtClaimlAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ExpenseClaimDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ExpenseClaimDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.TotalAmount > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                EditEntity.tb_FM_ExpenseClaimDetails = details;
                //处理图片
                foreach (tb_FM_ExpenseClaimDetail detail in EditEntity.tb_FM_ExpenseClaimDetails)
                {
                    PropertyInfo[] props = typeof(tb_FM_ExpenseClaimDetail).GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        var col = sgd[prop.Name];
                        if (col != null)
                        {
                            if (col.CustomFormat == CustomFormatType.WebPathImage)
                            {
                                //string newvalue = detail.GetPropertyValue(prop.Name).ToString();
                                // newvalue = newvalue.Replace(temppath,")"
                                //detail.SetPropertyValue(prop.Name, newvalue);
                            }
                        }
                    }
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

                ReturnMainSubResults<tb_FM_ExpenseClaim> SaveResult = new ReturnMainSubResults<tb_FM_ExpenseClaim>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_ExpenseClaimDetails);
                        if (uploadImg)
                        {
                            //更新图片名后保存到数据库
                            int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_ExpenseClaimDetail>(EditEntity.tb_FM_ExpenseClaimDetails)
                                .UpdateColumns(t => new { t.EvidenceImagePath })
                                .ExecuteCommandAsync();
                            if (ImgCounter > 0)
                            {
                                MainForm.Instance.PrintInfoLog($"图片保存成功,{ImgCounter}。");
                            }

                        }
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ClaimNo}。");
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
        /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
        /// </summary>
        /// <param name="RemoteSave"></param>
        /// <returns></returns>
        private async Task<bool> SaveImage(bool RemoteSave)
        {
            bool result = true;
            foreach (tb_FM_ExpenseClaimDetail detail in EditEntity.tb_FM_ExpenseClaimDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_ExpenseClaimDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
                            int totalRowsFlag = grid1.RowsCount;
                            if (grid1.HasSummary)
                            {
                                totalRowsFlag--;
                            }
                            for (int i = 1; i < totalRowsFlag; i++)
                            {
                                var model = grid1[i, col.ColIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

                                if (grid1[i, col.ColIndex].Value == null)
                                {
                                    continue;
                                }
                                string fileName = string.Empty;
                                if (grid1[i, col.ColIndex].Value.ToString().Contains(".jpg") && grid1[i, col.ColIndex].Value.ToString() == detail.GetPropertyValue(prop.Name).ToString())
                                {
                                    fileName = grid1[i, col.ColIndex].Value.ToString();
                                    //  fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                    //if (grid1[i, col.ColIndex].Tag == null && valueImageWeb.CellImageBytes != null)
                                    if (valueImageWeb.CellImageBytes != null)
                                    {
                                        //保存到本地
                                        //if (EditEntity.DataStatus == (int)DataStatus.草稿)
                                        //{
                                        //    //保存在本地临时目录
                                        //    ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        //    grid1[i, col.ColIndex].Tag = ImageHashHelper.GenerateHash(valueImageWeb.CellImageBytes);
                                        //}
                                        //else
                                        //{
                                        //上传到服务器，删除本地
                                        //实际应该可以直接传二进制数据，但是暂时没有实现，所以先保存到本地，再上传
                                        //ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                        string uploadRsult = await httpWebService.UploadImageAsyncOK("http://192.168.0.99:8080/upload/", fileName, valueImageWeb.CellImageBytes, "upload");
                                        //string uploadRsult = await HttpHelper.UploadImageAsyncOK("http://192.168.0.99:8080/upload/", fileName, "upload");
                                        if (true)
                                        {
                                            MainForm.Instance.PrintInfoLog(uploadRsult);
                                        }


                                        //}
                                    }
                                }

                                //UploadImage("http://127.0.0.1/upload", "D:/test.jpg", "upload");
                                // string uploadRsult = await HttpHelper.UploadImageAsync(AppContext.WebServerUrl + @"/upload", fileName, "amw");
                                //                            string uploadRsult = await HttpHelper.UploadImage(AppContext.WebServerUrl + @"/upload", fileName, "upload");

                            }


                        }
                    }
                }
            }
            return result;
        }



        protected override async Task<bool> Submit()
        {
            bool rs = await base.Submit();
            if (rs)
            {
                ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                var temppath = configManager.GetValue("WebServerUrl");
                if (string.IsNullOrEmpty(temppath))
                {
                    MainForm.Instance.uclog.AddLog("请先配置图片服务器路径", UILogType.错误);
                }
            }
            return true;
        }

        public override async Task<bool> DeleteRemoteImages()
        {
            if (EditEntity == null || EditEntity.tb_FM_ExpenseClaimDetails == null)
            {
                return false;
            }
            bool result = true;
            foreach (tb_FM_ExpenseClaimDetail detail in EditEntity.tb_FM_ExpenseClaimDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_ExpenseClaimDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {

                            if (detail.GetPropertyValue(prop.Name) != null
                                && detail.GetPropertyValue(prop.Name).ToString().Contains(".jpg"))
                            {
                                string imageNameValue = detail.GetPropertyValue(prop.Name).ToString();
                                //比较是否更新了图片数据
                                //old_new 无后缀文件名
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = new SourceGrid.Cells.Models.ValueImageWeb();
                                valueImageWeb.CellImageHashName = imageNameValue;
                                string oldfileName = valueImageWeb.GetOldRealfileName();
                                string newfileName = valueImageWeb.GetNewRealfileName();
                                string TempFileName = string.Empty;
                                //fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                //保存在本地临时目录 删除
                                if (System.IO.File.Exists(TempFileName))
                                {
                                    System.IO.File.Delete(TempFileName);
                                }
                                //上传到服务器，删除本地
                                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                string deleteRsult = await httpWebService.DeleteImageAsync(newfileName, "delete123");
                                MainForm.Instance.PrintInfoLog(deleteRsult);
                            }
                        }
                    }

                }
                return result;
            }
            return result;
        }
    }
}