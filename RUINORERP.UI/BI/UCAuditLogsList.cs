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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;
using RUINORERP.Global;
using RUINORERP.Common.Extensions;
using System.Reflection;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;
using RUINORERP.UI.Monitoring.Auditing;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("审计日志管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCAuditLogsList : BaseForm.BaseListGeneric<tb_AuditLogs>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCAuditLogsList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCAuditLogsEdit);
            toolStripButtonAdd.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonSave.Visible = false;

            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_AuditLogs, int?>> exp;
            exp = (p) => p.ObjectType;
            base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(BizType)));

            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.ObjectType, typeof(BizType));

            Krypton.Toolkit.KryptonButton button检查数据 = new Krypton.Toolkit.KryptonButton();
            button检查数据.Text = "提取重复数据";
            button检查数据.ToolTipValues.Description = "提取重复数据，有一行会保留，没有显示出来。";
            button检查数据.ToolTipValues.EnableToolTips = true;
            button检查数据.ToolTipValues.Heading = "提示";
            button检查数据.Click += button检查数据_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(button检查数据);
        }


        public override void AddExcludeMenuList()
        {
            //审核不能删除   
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                base.AddExcludeMenuList(MenuItemEnums.删除);
            }
        }

        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_恢复单据数据);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【恢复单据数据】", true, false, "NewSumDataGridView_恢复单据数据"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_恢复单据数据);

            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            UIHelper.ControlContextMenuInvisible(CurMenuInfo, list);

            if (dataGridView1 != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = dataGridView1.GetContextMenu(dataGridView1.ContextMenuStrip
                    , ContextClickList, list, true
                    );
                dataGridView1.ContextMenuStrip = newContextMenuStrip;
            }
        }

        private void NewSumDataGridView_恢复单据数据(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                if (dataGridView1.CurrentRow.DataBoundItem is tb_AuditLogs item)
                {
                    //只有审核状态才可以转换为出库单
                    if (item.ObjectType == (int)BizType.盘点单)
                    {
                        if (string.IsNullOrEmpty(item.DataContent))
                        {
                            //if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            //{

                            //}
                            //else
                            //{
                            //    continue;
                            //}
                        }

                        tb_StocktakeController<tb_Stocktake> ctr = Startup.GetFromFac<tb_StocktakeController<tb_Stocktake>>();
                        tb_Stocktake entity = null;
                        //entity = EntityDataRestorer.DeserializeObject<tb_Stocktake>(item.DataContent, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
                        // 恢复实体对象
                        entity = EntityDataRestorer.RestoreEntity<tb_Stocktake>(item.DataContent);
                        entity.MainID = 0;
                        MenuPowerHelper menuPowerHelper;
                        menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                        tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_Stocktake) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                        if (RelatedMenuInfo != null)
                        {
                            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, entity);
                        }
                        return;
                    }

                }
            }

        }

        private void button检查数据_Click(object sender, EventArgs e)
        {
            ListDataSoure.DataSource = GetDuplicatesList();
            dataGridView1.DataSource = ListDataSoure;
        }



        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AuditLogs).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的日志
            var lambda = Expressionable.Create<tb_AuditLogs>()
         //.AndIF(!MainForm.Instance.AppContext.IsSuperUser && MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null, t => t.UserName == MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName + "(" + MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + ")")
         .AndIF(!MainForm.Instance.AppContext.IsSuperUser && MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee == null, t => t.UserName == MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName)
         .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        tb_AuditLogsController<tb_AuditLogs> pctr = Startup.GetFromFac<tb_AuditLogsController<tb_AuditLogs>>();
        protected async override Task<bool> Delete()
        {
            List<tb_AuditLogs> list = new List<tb_AuditLogs>();
            //如果是选择了多行。则批量删除
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                list.Add(dr.DataBoundItem as tb_AuditLogs);
            }
            bool rs = await pctr.DeleteAsync(list.Select(c => c.Audit_ID).ToArray());
            if (rs)
            {
                Query();
            }
            return rs;
        }


    }
}
