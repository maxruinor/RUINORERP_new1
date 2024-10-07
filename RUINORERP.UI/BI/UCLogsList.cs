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


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("异常日志管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.异常日志)]
    public partial class UCLogsList : BaseForm.BaseListGeneric<Logs>
    {
        public UCLogsList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCLogsEdit);
            toolStripButtonAdd.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonSave.Visible = false;

            Krypton.Toolkit.KryptonButton button检查数据 = new Krypton.Toolkit.KryptonButton();
            button检查数据.Text = "提取重复数据";
            button检查数据.ToolTipValues.Description = "提取重复数据，有一行会保留，没有显示出来。";
            button检查数据.ToolTipValues.EnableToolTips = true;
            button检查数据.ToolTipValues.Heading = "提示";
            button检查数据.Click += button检查数据_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(button检查数据);
        }

        private void button检查数据_Click(object sender, EventArgs e)
        {
            ListDataSoure.DataSource = GetDuplicatesList();
            dataGridView1.DataSource = ListDataSoure;
        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(Logs).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的日志
            var lambda = Expressionable.Create<Logs>()
         .AndIF(!MainForm.Instance.AppContext.IsSuperUser && MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null, t => t.Operator == MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name)
         .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        LogsController<Logs> pctr = Startup.GetFromFac<LogsController<Logs>>();
        protected async override void Delete()
        {
            List<Logs> list = new List<Logs>();
            //如果是选择了多行。则批量删除
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                list.Add(dr.DataBoundItem as Logs);
            }
            bool rs = await pctr.DeleteAsync(list.Select(c => c.ID).ToArray());
            if (rs)
            {
                Query();
            }
            //await pctr.BaseDeleteAsync(list);
        }


    }
}
