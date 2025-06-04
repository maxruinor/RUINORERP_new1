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
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Model.CommonModel;
using Krypton.Toolkit;
using RUINORERP.UI.MRP.MP;

using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.UI.PSI.PUR
{



    [MenuAttrAssemblyInfo("请购单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.请购单)]
    public partial class UCBuyingRequisition : BaseBillEditGeneric<tb_BuyingRequisition, tb_BuyingRequisition>
    {
        public UCBuyingRequisition()
        {
            InitializeComponent();


        }

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_BuyingRequisition, actionStatus);
        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BuyingRequisition).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

        }

        public override void BindData(tb_BuyingRequisition entity, ActionStatus actionStatus)
        {
            if (actionStatus == ActionStatus.删除)
            {
                return;
            }
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.PuRequisition_ID > 0)
            {
                entity.PrimaryKeyID = entity.PuRequisition_ID;
                entity.ActionStatus = ActionStatus.加载;
                // entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.PuRequisitionNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.请购单);
                entity.ApplicationDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_BuyingRequisitionDetails != null && entity.tb_BuyingRequisitionDetails.Count > 0)
                {
                    entity.tb_BuyingRequisitionDetails.ForEach(c => c.PuRequisition_ID = 0);
                    entity.tb_BuyingRequisitionDetails.ForEach(c => c.PuRequisition_ChildID = 0);
                }
            }


            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.PuRequisitionNo, txtPuRequisitionNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Purpose, txtPurpose, BindDataType4TextBox.Text, false);
            
            DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.RequirementDate, dtpRequirementDate, false);
            DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.ApplicationDate, dtpApplicationDate, false);

            DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);


            DataBindingHelper.BindData4ControlByEnum<tb_BuyingRequisition>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_BuyingRequisition>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_BuyingRequisitionDetails != null && entity.tb_BuyingRequisitionDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_BuyingRequisitionDetail>(grid1, sgd, entity.tb_BuyingRequisitionDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_BuyingRequisitionDetail>(grid1, sgd, new List<tb_BuyingRequisitionDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }


                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.RefBillID.HasValue && entity.RefBillID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_BuyingRequisition>(c => c.RefBillID))
                {
                    LoadRefBillData(entity.RefBillID);
                }


                //显示 打印状态 如果是草稿状态 不显示打印


            };

            ShowPrintStatus(lblPrintStatus, EditEntity);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_BuyingRequisitionValidator>(), kryptonSplitContainer1.Panel1.Controls);

            }

            if (entity.RefBizType.HasValue)
            {
                #region 引用到了不同的单据，需要特殊处理

                if (entity.RefBizType == (int)BizType.需求分析)
                {
                    /* ----------这个引用到了 不同的单据：需求分析。手动。销售订单。其他？。需要特殊处理 ----------- */

                    //先绑定这个。InitFilterForControl 这个才生效
                    DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, v => v.RefBillNO, txtRefBillID, BindDataType4TextBox.Text, true);
                    DataBindingHelper.BindData4TextBoxWithTagQuery<tb_BuyingRequisition>(entity, v => v.RefBillID, txtRefBillID, true);

                    //创建表达式  草稿 结案 和没有提交的都不显示
                    var lambdaOrder = Expressionable.Create<tb_PurOrder>()
                                    .And(t => t.DataStatus == (int)DataStatus.确认)
                                     .And(t => t.isdeleted == false)
                                     .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                                    .ToExpression();//注意 这一句 不能少
                    //base.InitFilterForControl<tb_PurOrder, tb_PurOrderQueryDto>(entity, txtPurOrderNO, c => c.PurOrderNo, lambdaOrder, ctrPurorder.GetQueryParameters());

                    BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BuyingRequisition).Name + "Processor");
                    QueryFilter queryFilter = basePro.GetQueryFilter();

                    queryFilter.FilterLimitExpressions.Add(lambdaOrder);//意思是只有审核确认的。没有结案的。才能查询出来。

                    DataBindingHelper.InitFilterForControlByExp<tb_BuyingRequisition>(entity, txtRefBillID, c => c.RefBillNO, queryFilter);

                }



                #endregion
            }

            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            //指定了关键字段ProdDetailID
            //true 为 Selected
            // listCols = sgh.GetGridColumns<ProductSharePart, tb_BuyingRequisitionDetail>(c => c.ProdDetailID, true);
            listCols = sgh.GetGridColumns<ProductSharePart, tb_BuyingRequisitionDetail, InventoryInfo>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.PuRequisition_ID);
            listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.PuRequisition_ChildID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_ReadOnly<tb_BuyingRequisitionDetail>(c => c.DeliveredQuantity);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.Cost);
                //listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.SubtotalCostAmount);
                //listCols.SetCol_NeverVisible<tb_BuyingRequisitionDetail>(c => c.SubtotalPirceAmount);
            }
            */

            listCols.SetCol_Summary<tb_BuyingRequisitionDetail>(c => c.Quantity);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_BuyingRequisitionDetail>(sgd, f => f.prop, t => t.property);


            //应该只提供一个结构
            List<tb_BuyingRequisitionDetail> lines = new List<tb_BuyingRequisitionDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            sgd.SetDependencyObject<ProductSharePart, tb_BuyingRequisitionDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_BuyingRequisitionDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            //Clear(sgd);
            ShowSelectedColumn();
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
        }

        private void Sgh_OnLoadMultiRowData(object rows, Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_BuyingRequisitionDetail> details = new List<tb_BuyingRequisitionDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_BuyingRequisitionDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_BuyingRequisitionDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_BuyingRequisitionDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_BuyingRequisitionDetail> details = sgd.BindingSourceLines.DataSource as List<tb_BuyingRequisitionDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                EditEntity.TotalQty = details.Sum(c => c.Quantity);


            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_BuyingRequisitionDetail> details = new List<tb_BuyingRequisitionDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_BuyingRequisitionDetail> detailentity = bindingSourceSub.DataSource as List<tb_BuyingRequisitionDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (details.Count == 0 && NeedValidated)
                {
                    System.Windows.Forms.MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                EditEntity.tb_BuyingRequisitionDetails = details;
                foreach (var item in details)
                {
                    item.tb_buyingrequisition = EditEntity;
                }
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 0 && NeedValidated)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_BuyingRequisitionDetail>(details))
                {
                    return false;
                }



                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                ReturnMainSubResults<tb_BuyingRequisition> SaveResult = new ReturnMainSubResults<tb_BuyingRequisition>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PuRequisitionNo}。");
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

        tb_BuyingRequisitionController<tb_BuyingRequisition> ctr = Startup.GetFromFac<tb_BuyingRequisitionController<tb_BuyingRequisition>>();

        /*
        protected async override Task<ApprovalEntity> Review()
        {
            if (EditEntity == null)
            {
                return null;
            }
            //如果已经审核通过，则不能重复审核
            if (EditEntity.ApprovalStatus.HasValue)
            {
                if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
                {
                    if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                    {
                        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                        return null;
                    }
                }
            }
            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_BuyingRequisition oldobj = CloneHelper.DeepCloneObject<tb_BuyingRequisition>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_BuyingRequisition>(EditEntity, oldobj);
            };
            ApprovalEntity ae = await base.Review();
            if (EditEntity == null)
            {
                return null;
            }
            if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
            {
                return null;
            }

            // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            //因为只需要更新主表
            //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
            // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
            ReturnResults<bool> rmrs = await ctr.ApprovalAsync(EditEntity);
            if (rmrs.Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.审核);
                //如果审核结果为不通过时，审核不是灰色。
                if (!ae.ApprovalResults)
                {
                    toolStripbtnReview.Enabled = true;
                }
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
                MainForm.Instance.PrintInfoLog(rmrs.ErrorMsg);
            }

            return ae;
        }
        */
        /*
        protected async override void ReReview()
        {
            if (EditEntity == null)
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                return;
            }


            if (EditEntity.tb_BuyingRequisitionDetails == null || EditEntity.tb_BuyingRequisitionDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return;
            }

            RevertCommand command = new RevertCommand();

            tb_BuyingRequisition oldobj = CloneHelper.DeepCloneObject<tb_BuyingRequisition>(EditEntity);
            command.UndoOperation = delegate ()
            {
                CloneHelper.SetValues<tb_BuyingRequisition>(EditEntity, oldobj);
            };
           
            bool Succeeded = await ctr.AntiApprovalAsync(EditEntity);
            if (Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.反审);
                toolStripbtnReview.Enabled = true;

            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.PuRequisitionNo}反审失败,请联系管理员！", Color.Red);
            }
    
        }
        */

        /// <summary>
        /// 结案
        /// </summary>
        protected override async Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }

            //要审核过，并且通过了，才能结案。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.未审核 && EditEntity.DataStatus == (int)DataStatus.确认)
            {
                MainForm.Instance.uclog.AddLog("已经审核的单据才能结案。");
                return false;
            }
            if (EditEntity.tb_BuyingRequisitionDetails == null || EditEntity.tb_BuyingRequisitionDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return false;
            }

            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_BuyingRequisition>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData<tb_BuyingRequisition>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                RevertCommand command = new RevertCommand();
                tb_BuyingRequisition oldobj = CloneHelper.DeepCloneObject<tb_BuyingRequisition>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    CloneHelper.SetValues<tb_BuyingRequisition>(EditEntity, oldobj);
                };
                List<tb_BuyingRequisition> _PurOrders = [EditEntity];
                ReturnResults<bool> returnResults = await ctr.BatchCloseCaseAsync(_PurOrders);
                if (returnResults.Succeeded)
                {

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    //审核成功
                    base.ToolBarEnabledControl(MenuItemEnums.结案);
                    toolStripbtnReview.Enabled = true;

                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{EditEntity.PuRequisitionNo}结案失败,请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }

        }


        string orderid = string.Empty;


        private void LoadRefBillData(long? _RefBillID)
        {
            ButtonSpecAny bsa = (txtRefBillID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            //请购单 可来自于销售订单，也可以来自于需求分析中的采购建议部分。当然也可以手动输入

            var purorder = bsa.Tag as tb_SaleOrder;
            /*
            purorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>().Where(c => c.PurOrder_ID == _RefBillID)
             .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
            .SingleAsync();
            _purorder = purorder;
            //新增时才可以转单
            if (purorder != null)
            {
                orderid = purorder.PurOrder_ID.ToString();
                
                tb_PurEntry entity = mapper.Map<tb_PurEntry>(purorder);
                List<tb_PurEntryDetail> details = mapper.Map<List<tb_PurEntryDetail>>(purorder.tb_PurOrderDetails);

                entity.EntryDate = System.DateTime.Now;
                List<tb_PurEntryDetail> NewDetails = new List<tb_PurEntryDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].PurOrder_ChildID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_PurOrderDetail item = purorder.tb_PurOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.PurOrder_ChildID == details[i].PurOrder_ChildID);
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = details[i].TransactionPrice * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一

                        tb_PurOrderDetail item = purorder.tb_PurOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = details[i].TransactionPrice * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }
                        #endregion
                    }




                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{entity.PurOrder_NO}已全部入库，请检查是否正在重复入库！");
                }

                StringBuilder msg = new StringBuilder();
                foreach (var item in tipsMsg)
                {
                    msg.Append(item).Append("\r\n");

                }
                if (tipsMsg.Count > 0)
                {
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                entity.tb_PurEntryDetails = NewDetails;
                entity.PurOrder_ID = purorder.PurOrder_ID;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.PurOrder_NO = purorder.PurOrderNo;
                entity.TotalAmount = NewDetails.Sum(c => c.SubtotalAmount);
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.ActualAmount = entity.ShippingCost + entity.TotalAmount;
                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = entit564546y.Customer
                    entity.PurOrder_NO = entity.PurOrder_NO;
                }
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity);
            }

            */

        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void btnTransferToPurOrder_Click(object sender, EventArgs e)
        {
            //如果拆分，则要判断选择的行数。否则全部转换为采购订单草稿
            List<tb_BuyingRequisitionDetail> details = sgd.BindingSourceLines.DataSource as List<tb_BuyingRequisitionDetail>;
            details = details.Where(c => c.ProdDetailID > 0).ToList();
            if (details.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("请先选择产品数据");
                return;
            }
            else
            {
                List<tb_BuyingRequisitionDetail> buyingRequisitionDetails = new List<tb_BuyingRequisitionDetail>();
                Expression<Func<tb_BuyingRequisitionDetail, object>> expSelected = c => c.Selected;
                SGDefineColumnItem selected = sgd.DefineColumns.Find(c => c.ColName == expSelected.GetMemberInfo().Name);
                int selectRealIndex = sgd.grid.Columns.GetColumnInfo(selected.UniqueId).Index;
                // List<int> SelectedRows = new List<int>();
                if (selected != null && chkSplitRequisitionDetails.Checked)
                {
                    //转选择中的 
                    foreach (GridRow row in grid1.Rows)
                    {
                        if (row.RowData == null)
                        {
                            continue;
                        }
                        if (grid1[row.Index, selectRealIndex].Value is Boolean bl)
                        {
                            if (bl)
                            {
                                tb_BuyingRequisitionDetail detail = row.RowData as tb_BuyingRequisitionDetail;
                                buyingRequisitionDetails.Add(detail);
                                //SelectedRows.Add(row.Index);
                            }
                        }
                    }
                }
                else
                {
                    //全部转换为采购订单草稿
                    buyingRequisitionDetails.AddRange(details);
                }



                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_PurOrder).Name && m.ClassPath == "RUINORERP.UI.PSI.PUR." + typeof(UCPurOrder).Name).FirstOrDefault();

                tb_PurOrder _PurOrder = BuliderPurOrder(buyingRequisitionDetails);
                if (RelatedBillMenuInfo != null && _PurOrder != null)
                {
                    //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                    menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, _PurOrder);
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("没有采购建议数据 或没有使用【采购订单】菜单的权限，无法生成");
                }
            }


        }


        public tb_PurOrder BuliderPurOrder(List<tb_BuyingRequisitionDetail> details)
        {
            //新增采购订单

            tb_PurOrder entity = new tb_PurOrder();
            List<tb_PurOrderDetail> NewDetails = new List<tb_PurOrderDetail>();
            entity.PurDate = System.DateTime.Now;
            entity.RefNO = EditEntity.PuRequisitionNo;
            entity.RefBillID = EditEntity.PuRequisition_ID;
            entity.RefBizType = (int)BizType.请购单;
            entity.DataStatus = (int)DataStatus.草稿;
            List<string> tipsMsg = new List<string>();
            

            for (int i = 0; i < details.Count; i++)
            {
                tb_PurOrderDetail orderDetail = MainForm.Instance.mapper.Map<tb_PurOrderDetail>(details[i]);
                if (details[i].Purchased.HasValue && details[i].Purchased == true)
                {
                    continue; //已经采购的忽略
                }
                NewDetails.Add(orderDetail);
            }
            if (NewDetails.Count == 0)
            {
                tipsMsg.Add($"请选择要转为采购订单的产品数据！");
            }
            StringBuilder msg = new StringBuilder();
            foreach (var item in tipsMsg)
            {
                msg.Append(item).Append("\r\n");

            }
            if (tipsMsg.Count > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            entity.tb_PurOrderDetails = NewDetails;

            entity.TotalAmount = NewDetails.Sum(c => c.SubtotalAmount);
            entity.TotalQty = NewDetails.Sum(c => c.Quantity);
            entity.TotalAmount = entity.ShipCost + entity.TotalAmount;

            entity.DataStatus = (int)DataStatus.草稿;
            entity.ApprovalStatus = (int)ApprovalStatus.未审核;
            entity.ApprovalResults = null;
            entity.ApprovalOpinions = "";
            entity.Modified_at = null;
            entity.Modified_by = null;
            entity.Approver_at = null;
            entity.Approver_by = null;
            entity.PrintStatus = 0;
            entity.ActionStatus = ActionStatus.新增;


            BusinessHelper.Instance.InitEntity(entity);
            return entity;

        }


        private void chkSplitRequisitionDetails_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedColumn();
        }

        private void ShowSelectedColumn()
        {
            SGDefineColumnItem selected = listCols.Find(c => c.ColName == "Selected");
            if (selected != null && selected.UniqueId != null)
            {
                int selectRealIndex = sgd.grid.Columns.GetColumnInfo(selected.UniqueId).Index;
                grid1.Columns[selectRealIndex].Visible = chkSplitRequisitionDetails.Checked;
            }

        }
    }
}

