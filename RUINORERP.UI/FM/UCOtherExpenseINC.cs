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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.UI.ATechnologyStack;


namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("其他费用收入", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用收入)]
    public partial class UCOtherExpenseINC : BaseBillEditGeneric<tb_FM_OtherExpense, tb_FM_OtherExpenseDetail>, ISharedIdentification
    {
        public UCOtherExpenseINC()
        {
            InitializeComponent();
        }
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_FM_OtherExpense, actionStatus);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            rdb收入.Visible = false;
            rdb支出.Visible = false;
            lblEXPOrINC.Visible = false;
            kryptonGroupBox4.Visible = false;
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
        }

        public override void BindData(tb_FM_OtherExpense entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (entity.ExpenseMainID > 0)
            {
                entity.PrimaryKeyID = entity.ExpenseMainID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                }
                entity.DocumentDate = System.DateTime.Now;
                if (CurMenuInfo != null)
                {
                    lbl盘点单.Text = CurMenuInfo.CaptionCN;
                    if (CurMenuInfo.CaptionCN.Contains("支出"))
                    {
                        entity.EXPOrINC = false;
                        entity.ExpenseNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.其他费用支出);
                    }

                    if (CurMenuInfo.CaptionCN.Contains("收入"))
                    {
                        entity.EXPOrINC = true;
                        entity.ExpenseNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.其他费用收入);
                    }
                }



            }

            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_FM_OtherExpense>(entity, t => t.EXPOrINC, rdb收入, rdb支出);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ExpenseNo, txtExpenseNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.DocumentDate, dtpDocumentDate, false);
            //DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.EXPOrINC, chkEXPOrINC, false);
            //有默认值
            
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_OtherExpense>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_OtherExpense>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            txtstatus.ReadOnly = true;
            if (entity.tb_FM_OtherExpenseDetails != null && entity.tb_FM_OtherExpenseDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_FM_OtherExpenseDetail>(grid1, sgd, entity.tb_FM_OtherExpenseDetails, c => c.ExpenseSubID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_OtherExpenseDetail>(grid1, sgd, new List<tb_FM_OtherExpenseDetail>(), c => c.ExpenseSubID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {

                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                }

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
            //显示结案凭证图片
            LoadImageData(entity.CloseCaseImagePath);
            base.BindData(entity);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_OtherExpense).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                                .And(t => t.EXPOrINC == true)
                            // .And(t => t.Is_enabled == true)
                            //报销人员限制，财务不限制
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }
        private async void LoadImageData(string CloseCaseImagePath)
        {
            if (!string.IsNullOrWhiteSpace(CloseCaseImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                try
                {
                    byte[] img = await httpWebService.DownloadImgFileAsync(CloseCaseImagePath);
                    magicPictureBox1.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                    magicPictureBox1.Visible = true;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                }
            }
            else
            {
                magicPictureBox1.Visible = false;
            }
        }
 
        public override async Task<bool> DeleteRemoteImages()
        {

            if (EditEntity == null || EditEntity.tb_FM_OtherExpenseDetails == null)
            {
                return false;
            }

            #region 删除主图的结案图。一般没有结案是没有的。结案就不会有结案图了。也有特殊情况。

            if (!string.IsNullOrEmpty(EditEntity.CloseCaseImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                string deleteRsult = await httpWebService.DeleteImageAsync(EditEntity.CloseCaseImagePath, "delete123");
                MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
            }
            #endregion

            bool result = true;
            foreach (tb_FM_OtherExpenseDetail detail in EditEntity.tb_FM_OtherExpenseDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_OtherExpenseDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            if (detail.GetPropertyValue(prop.Name) != null
                                && detail.GetPropertyValue(prop.Name).ToString().Contains("-"))
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
            }
            return result;
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_OtherExpenseDetailController<tb_FM_OtherExpenseDetail> dc = Startup.GetFromFac<tb_FM_OtherExpenseDetailController<tb_FM_OtherExpenseDetail>>();
        List<tb_FM_OtherExpenseDetail> list = new List<tb_FM_OtherExpenseDetail>();

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


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_OtherExpenseDetail>();

            listCols.SetCol_NeverVisible<tb_FM_OtherExpenseDetail>(c => c.ExpenseMainID);
            listCols.SetCol_NeverVisible<tb_FM_OtherExpenseDetail>(c => c.ExpenseSubID);



            //listCols.SetCol_ReadOnly<tb_FM_OtherExpenseDetail>(c => c.CNName);


            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.UntaxedAmount, CustomFormatType.CurrencyFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.SubtotalPirceAmount);
            }*/
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.TotalAmount);
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.UntaxedAmount);

            listCols.SetCol_Formula<tb_FM_OtherExpenseDetail>((a, b, c) => a.TotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_FM_OtherExpenseDetail>((a, b) => a.TotalAmount - b.TaxAmount, c => c.UntaxedAmount);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);


            //应该只提供一个结构
            List<tb_FM_OtherExpenseDetail> lines = new List<tb_FM_OtherExpenseDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_OtherExpenseDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
         UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
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
                List<tb_FM_OtherExpenseDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_OtherExpenseDetail>;
                details = details.Where(c => c.TotalAmount > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TotalAmount);

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }

        List<tb_FM_OtherExpenseDetail> details = new List<tb_FM_OtherExpenseDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_OtherExpenseDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_OtherExpenseDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.TotalAmount > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                EditEntity.tb_FM_OtherExpenseDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_OtherExpenseDetail>(details))
                {
                    return false;
                }
                //必需选一个
                if (NeedValidated && (!rdb支出.Checked && !rdb收入.Checked))
                {
                    MessageBox.Show("收入和支出必需选一个！");
                    return false;
                }
                if (NeedValidated)
                {//处理图片
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_OtherExpenseDetails);
                    if (uploadImg)
                    {
                        ////更新图片名后保存到数据库
                        //int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_ExpenseClaimDetail>(EditEntity.tb_FM_ExpenseClaimDetails)
                        //    .UpdateColumns(t => new { t.EvidenceImagePath })
                        //    .ExecuteCommandAsync();
                        //if (ImgCounter > 0)
                        //{
                        MainForm.Instance.PrintInfoLog($"图片保存成功,。");
                        //}
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("图片上传出错。");
                        return false;
                    }
                }

                ReturnMainSubResults<tb_FM_OtherExpense> SaveResult = new ReturnMainSubResults<tb_FM_OtherExpense>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ExpenseNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;

            }
            return false;
        }

        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>
        protected async override Task<ApprovalEntity> Review()
        {
            ApprovalEntity ae = await base.Review();
            if (ae.ApprovalResults)
            {
                EditEntity.DataStatus = (int)DataStatus.完结;
                await AppContext.Db.Updateable(EditEntity).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
            }
            return ae;
        }

      



    }
}
