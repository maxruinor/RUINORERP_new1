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
using RUINORERP.UI.BaseForm;
using SqlSugar;
using RUINORERP.Business.Processor;
using MathNet.Numerics.Distributions;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("角色信息", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRoleInfoList : BaseListGeneric<tb_RoleInfo>
    {
        public UCRoleInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCRoleInfoEdit);

        }

        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_RoleInfo).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        public override async Task<List<tb_RoleInfo>> Save()
        {
            List<tb_RoleInfo> list = new List<tb_RoleInfo>();
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_RoleInfo;

                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        ReturnResults<tb_RoleInfo> rr = new ReturnResults<tb_RoleInfo>();
                        await Task.Delay(0);
                        if (entity.RoleID > 0)
                        {
                            //if (entity.tb_rolepropertyconfig != null && entity.tb_rolepropertyconfig.RolePropertyID > 0)
                            //{
                            //    //更新
                            //    MainForm.Instance.AppContext.Db.Updateable<tb_RolePropertyConfig>(entity.tb_rolepropertyconfig).ExecuteCommand(); //都是参数化实现
                            //    entity.RolePropertyID = entity.tb_rolepropertyconfig.RolePropertyID;
                            //}
                            //else if (entity.tb_rolepropertyconfig != null && entity.tb_rolepropertyconfig.RolePropertyID == 0)
                            //{
                            //    //新增
                            //    entity.tb_rolepropertyconfig = await MainForm.Instance.AppContext.Db.Insertable<tb_RolePropertyConfig>(entity.tb_rolepropertyconfig).ExecuteReturnEntityAsync(); //都是参数化实现
                            //    entity.RolePropertyID = entity.tb_rolepropertyconfig.RolePropertyID;
                            //}
                            ////逻辑更新主表 ，子表存在更新，不存在插入
                            rr.Succeeded = MainForm.Instance.AppContext.Db.UpdateNav(entity)
                            .Include(z1 => z1.tb_rolepropertyconfig)
                            .ExecuteCommand();
                           // rr.Succeeded = MainForm.Instance.AppContext.Db.Updateable<tb_RoleInfo>(entity).ExecuteCommand() > 0 ? true : false;
                        }
                        else
                        {
                            //完全新增
                            await MainForm.Instance.AppContext.Db.InsertNav<tb_RoleInfo>(entity)
                            .Include(z1 => z1.tb_rolepropertyconfig)//插入1层 Root->Books
                            .ExecuteReturnEntityAsync();
                        }
                        if (rr.Succeeded)
                        {
                            list.Add(rr.ReturnObject);
                            ToolBarEnabledControl(MenuItemEnums.保存);
                        }

                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }
            return list;
        }

        public async override void Query(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            dataGridView1.ReadOnly = true;

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);
            BaseController<tb_RoleInfo> ctr = Startup.GetFromFacByName<BaseController<tb_RoleInfo>>(typeof(tb_RoleInfo).Name + "Controller");

            //要导航查出角色属性，所以重写用了自动 导致的方法
            List<tb_RoleInfo> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize);

            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }


        //因为tb_P4FieldInfo中引用了字段表中的信息，所以要使用导航删除。但是一定要细心

        tb_RoleInfoController<tb_RoleInfo> childctr = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();
        protected async override Task<bool> Delete()
        {
            bool rs = false;
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_RoleInfo Info = dr.DataBoundItem as tb_RoleInfo;
                rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_RoleInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{Info.RoleName}删除成功。");
                }
            }
            Query();
            return rs;
        }

    }
}
