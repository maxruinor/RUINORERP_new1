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
using RUINORERP.Business.CommService;
using RUINORERP.UI.CommonUI;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("财务审计管理", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.基础设置)]
    public partial class UCFMAuditLogsList : BaseForm.BaseListGeneric<tb_FM_AuditLogs>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {

        public UCFMAuditLogsList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFMAuditLogsEdit);
            toolStripButtonAdd.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonSave.Visible = false;

            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_FM_AuditLogs, int?>> exp;
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

            //mapper=new BizTypeMapper();

        }
        #region 添加 产品跟踪

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {

            ToolStripButton toolStripButton产品跟踪 = new System.Windows.Forms.ToolStripButton();
            toolStripButton产品跟踪.Text = "产品跟踪";
            toolStripButton产品跟踪.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton产品跟踪.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton产品跟踪.Name = "产品跟踪AssignmentToBizEmp";
            //if (MainForm.Instance.AppContext.IsSuperUser)
            //{
            //    toolStripButton产品跟踪.Visible = true;//默认
            //}
            //else
            //{
            //    toolStripButton产品跟踪.Visible = false;//默认隐藏
            //}

            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton产品跟踪);
            toolStripButton产品跟踪.ToolTipText = "根据审计日志数据来进行产品跟踪。";
            toolStripButton产品跟踪.Click += new System.EventHandler(this.toolStripButton产品跟踪_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton产品跟踪 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }



        private void toolStripButton产品跟踪_Click(object sender, EventArgs e)
        {
            UIHelper.CheckValidation(this);
            List<tb_FM_AuditLogs> updateList = new List<tb_FM_AuditLogs>();
            foreach (var item in bindingSourceList)
            {
                if (item is tb_FM_AuditLogs sourceEntity)
                {
                    updateList.Add(sourceEntity);
                }
            }

            if (updateList.Count > 0)
            {
                UCFMAuditLogsTracker frm = new UCFMAuditLogsTracker();
                frm.AuditLogs = updateList;
                frm.Show();
            }
        }


        #endregion

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
                if (dataGridView1.CurrentRow.DataBoundItem is tb_FM_AuditLogs item)
                {
                    #region  恢复单据

                    BizType bizType = (BizType)item.ObjectType.Value;
                    Type objType = Business.BizMapperService.EntityMappingHelper.GetEntityType(bizType);
                    if (!string.IsNullOrEmpty(item.DataContent))
                    {
                        if (MessageBox.Show($"当前单据{item.ObjectNo}：将重新生成，请谨慎操作\r\n确定生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            // 恢复实体对象
                            // 解析JSON数据
                            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.DataContent);
                            var entity = EntityDataRestorer.RestoreEntity(objType, data);
                            string PKColName = UIHelper.GetPrimaryKeyColName(objType);
                            //  object PKValue = entity.GetPropertyValue(PKColName);
                            entity.SetPropertyValue(PKColName, 0);

                            MenuPowerHelper menuPowerHelper;
                            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == objType.Name && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                            if (RelatedMenuInfo != null)
                            {
                                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, entity);
                            }
                            return;
                        }

                    }


                    /*
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
                    */
                    #endregion

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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_AuditLogs).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的日志
            var lambda = Expressionable.Create<tb_FM_AuditLogs>()
         //.AndIF(!MainForm.Instance.AppContext.IsSuperUser && MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null, t => t.UserName == MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName + "(" + MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + ")")
         .AndIF(!MainForm.Instance.AppContext.IsSuperUser && MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee == null, t => t.UserName == MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName)
         .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        tb_FM_AuditLogsController<tb_FM_AuditLogs> pctr = Startup.GetFromFac<tb_FM_AuditLogsController<tb_FM_AuditLogs>>();
        protected async override Task<bool> Delete()
        {
            List<tb_FM_AuditLogs> list = new List<tb_FM_AuditLogs>();
            //如果是选择了多行。则批量删除
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                list.Add(dr.DataBoundItem as tb_FM_AuditLogs);
            }
            bool rs = await pctr.DeleteAsync(list.Select(c => c.FMAudit_ID).ToArray());
            if (rs)
            {
                QueryAsync();
            }
            return rs;
        }

        private void UCAuditLogsList_Load(object sender, EventArgs e)
        {
            #region 双击单号后按业务类型查询显示对应业务窗体
            GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            GridRelated.SetComplexTargetField<tb_FM_AuditLogs>(c => c.ObjectType, c => c.ObjectNo);
       
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = Business.BizMapperService.EntityMappingHelper.GetEntityType((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.Name);
                GridRelated.SetRelatedInfo<tb_FM_AuditLogs>(c => c.ObjectNo, keyNamePair);
            }
            #endregion
        }
    }
}
