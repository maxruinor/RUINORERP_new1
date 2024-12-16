using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using NPOI.SS.Formula.Functions;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.UI.SysConfig;
using TransInstruction;
using AutoUpdateTools;
using RUINORERP.Model.TransModel;
using RUINORERP.Business.CommService;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("跟进计划", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.跟进管理)]
    public partial class UCCRMFollowUpPlansList : BaseForm.BaseListGeneric<tb_CRM_FollowUpPlans>
    {
        public UCCRMFollowUpPlansList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMFollowUpPlansEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_FollowUpPlans, int?>> expPlanStatus;
            expPlanStatus = (p) => p.PlanStatus;
            base.ColNameDataDictionary.TryAdd(expPlanStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(FollowUpPlanStatus)));


        }
        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_FollowUpPlans).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_FollowUpPlans>()
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了只看到自己的 
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void Query(bool UseAutoNavQuery = false)
        {
            base.Query(true);
        }

        private void UCCRMFollowUpPlansList_Load(object sender, EventArgs e)
        {
            ContextMenuStrip newContextMenuStrip = base.dataGridView1.GetContextMenu(contextMenuStrip1);
            base.dataGridView1.ContextMenuStrip = newContextMenuStrip;
        }

        public override async Task<List<tb_CRM_FollowUpPlans>> Save()
        {
            List<tb_CRM_FollowUpPlans> list = await base.Save();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    //向服务器推送工作流提醒的列表 typeof(T).Name
                    if (item.PlanStatus == (int)FollowUpPlanStatus.未开始)
                    {
                        ClientReminderRequest request = new ClientReminderRequest();
                        request.BizPrimaryKey = item.PlanID;
                        request.BizType = BizType.CRM跟进计划;
                        request.StartTime = item.PlanStartDate;
                        request.EndTime = item.PlanEndDate;
                        request.ReminderSubject = item.PlanSubject;
                        request.ReminderContent = item.PlanContent;
                        request.RemindTargetID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                        request.RemindTargetName = MainForm.Instance.AppContext.CurUserInfo.Name;

                        OriginalData beatDataDel = ClientDataBuilder.工作流提醒请求(request);
                        MainForm.Instance.ecs.AddSendData(beatDataDel);
                    }
                }

            }
            return list;
        }
        private async void 添加跟进记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_FollowUpPlans plan)
                {
                    object frm = Activator.CreateInstance(typeof(UCCRMFollowUpRecordsEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CRM_FollowUpRecords> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpRecords>;
                        frmaddg.Text = "跟进记录编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpRecords>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CRM_FollowUpRecords EntityInfo = obj as tb_CRM_FollowUpRecords;
                        EntityInfo.Customer_id = plan.Customer_id;
                        EntityInfo.PlanID = plan.PlanID;
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CRM_FollowUpRecords> ctrRecords = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                            ReturnResults<tb_CRM_FollowUpRecords> result = await ctrRecords.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                //记录添加成功后。客户如果是新客户 则转换为 潜在客户
                                if (plan.tb_crm_customer != null)
                                {
                                    if (plan.tb_crm_customer.CustomerStatus == (int)CustomerStatus.新增客户)
                                    {
                                        plan.tb_crm_customer.CustomerStatus = (int)CustomerStatus.潜在客户;
                                        BaseController<tb_CRM_Customer> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                                        ReturnResults<tb_CRM_Customer> resultCustomer = await ctrContactInfo.BaseSaveOrUpdate(plan.tb_crm_customer);
                                        if (resultCustomer.Succeeded)
                                        {

                                        }
                                    }

                                    //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                                    //只处理需要缓存的表
                                    if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_FollowUpRecords).Name, out pair))
                                    {
                                        //如果有更新变动就上传到服务器再分发到所有客户端
                                        OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_FollowUpRecords>(result.ReturnObject);
                                        byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                        MainForm.Instance.ecs.client.Send(buffer);
                                    }
                                }
                                MainForm.Instance.ShowStatusText("添加成功!");
                            }
                            else
                            {
                                MainForm.Instance.ShowStatusText("添加失败!");
                            }
                        }
                    }
                }

            }
        }
    }
}
