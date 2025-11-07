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
using RUINORERP.UI.BaseForm;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Global;
using RUINORERP.Extensions.Middlewares;


namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("客户线索", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    public partial class UCLeadsList : BaseForm.BaseListGeneric<tb_CRM_Leads>
    {
        public UCLeadsList()
        {
            InitializeComponent();

            tb_CRMConfig CRMConfig = MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().First();
            if (CRMConfig != null)
            {
                if (!CRMConfig.CS_UseLeadsFunction)
                {
                    MessageBox.Show("请联系管理员开通线索功能.");
                    this.Controls.Clear();
                    return;
                }
            }

            base.EditForm = typeof(UCLeadsEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_Leads, int>> expLeadsStatus;
            expLeadsStatus = (p) => p.LeadsStatus;
            base.ColNameDataDictionary.TryAdd(expLeadsStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(LeadsStatus)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.LeadsStatus, typeof(LeadsStatus));

        }
        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_Leads>()
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采 
            .ToExpression();    //拥有权控制
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        private void UCLeadsList_Load(object sender, EventArgs e)
        {
            ContextMenuStrip newContextMenuStrip = base.dataGridView1.GetContextMenu(contextMenuStrip1);
            base.dataGridView1.ContextMenuStrip = newContextMenuStrip;
        }

        protected override Task<bool> Delete()
        {
            tb_CRM_Leads rowInfo = (tb_CRM_Leads)this.bindingSourceList.Current;
            if (rowInfo.Employee_ID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID && !MainForm.Instance.AppContext.IsSuperUser)
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只能删除自己的线索信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Task.FromResult(false);
            }
            if (rowInfo.tb_CRM_Customers != null || rowInfo.LeadsStatus != (int)LeadsStatus.新建)
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只有【新建】的线索,并且没有转换为目标客户时才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Task.FromResult(false);
            }
            return base.Delete();
        }
        private async void 转为目标客户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_Leads sourceEntity)
                {
                    object frm = Activator.CreateInstance(typeof(UCCRMCustomerEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CRM_Customer> frmaddg = frm as BaseEditGeneric<tb_CRM_Customer>;
                        frmaddg.CurMenuInfo = this.CurMenuInfo;
                        frmaddg.Text = "目标客户编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_Customer>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CRM_Customer EntityInfo = obj as tb_CRM_Customer;

                        MainForm.Instance.mapper.Map(sourceEntity, EntityInfo);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
                                                                                 // EntityInfo = mapper.Map<tb_CRM_Customer>(sourceEntity);
                        EntityInfo.LeadID = sourceEntity.LeadID;
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CRM_Customer> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                            try
                            {
                                ReturnResults<tb_CRM_Customer> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                                if (result.Succeeded)
                                {
                                    if (result.Succeeded)
                                    {
                                        base._eventDrivenCacheManager.UpdateEntity<tb_CRM_Customer>(result.ReturnObject);
                                       
                                       
                                    }
                                    MainForm.Instance.ShowStatusText("添加成功!");
                                }
                                else
                                {
                                    MainForm.Instance.ShowStatusText("添加失败!");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("AK_KEY_CUSTOMERNAME_TB_CRM_C"))
                                {
                                    MessageBox.Show("客户名称不能重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    throw;
                                }

                            }

                        }
                    }
                }
            }
        }
    }
}
