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
using RUINORERP.Global;
using RUINORERP.Business.Processor;
using SqlSugar;
using System.Linq.Dynamic.Core;
using static RUINORERP.UI.Common.DuplicateFinder;
using System.Reflection;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("字段管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCFieldInfoList : BaseForm.BaseListGeneric<tb_FieldInfo>
    {

        public UCFieldInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFieldInfoEdit);

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
            List<tb_FieldInfo> list = new List<tb_FieldInfo>();
            list = ListDataSoure.Cast<tb_FieldInfo>().ToList();
            string pkName = UIHelper.GetPrimaryKeyColName(typeof(tb_FieldInfo));
            //var keySelector1 = list.Select(p =>
            //{
            //    PropertyInfo[] properties = typeof(tb_FieldInfo).GetProperties()
            //        .Where(prop => prop.GetCustomAttribute<SugarColumn>()?.IsIgnore == false && prop.Name != pkName)
            //        .ToArray();
            //    var values = properties.Select(prop => prop.GetValue(p)).ToArray();
            //    return Tuple.Create(values);
            //});

            // 使用 GroupBy 筛选出重复数据,排除掉主键，将其他所有列【SugarColumn】有效的，都参与比较
            // 创建一个用于获取所有键属性值的匿名函数
            // 创建分组键选择器(Tuple) 一个对象中，哪些字段属性参与比较
            Func<tb_FieldInfo, Tuple<object[]>> keySelector2 = p =>
            {
                PropertyInfo[] properties = typeof(tb_FieldInfo).GetProperties()
                    .Where(prop => prop.GetCustomAttribute<SugarColumn>()?.IsIgnore == false && prop.Name != pkName)
                    .ToArray();
                var values = properties.Select(prop => prop.GetValue(p)).ToArray();
                return Tuple.Create(values);
            };
            
            // 使用自定义比较器进行分组
            var duplicatesList = list.GroupBy(
                keySelector2,
                new CustomTupleEqualityComparer<Tuple<object[]>>(new string[] { pkName }) // 使用适当的比较器
            ).Where(g => g.Count() > 1)
             .Select(g => g.Skip(1))//排除掉第一个元素，这个是第一个重复的元素，要保留
            .SelectMany(g => g)
            .ToList();

            ListDataSoure.DataSource = duplicatesList.ToBindingSortCollection<tb_FieldInfo>();
            dataGridView1.DataSource = ListDataSoure;
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FieldInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }


        //因为tb_P4FieldInfo中引用了字段表中的信息，所以要使用导航删除。但是一定要细心

        tb_FieldInfoController<tb_FieldInfo> childctr = Startup.GetFromFac<tb_FieldInfoController<tb_FieldInfo>>();

        public object groupedList { get; private set; }

        protected async override Task<bool> Delete()
        {
            bool rs = false;
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_FieldInfo buttonInfo = dr.DataBoundItem as tb_FieldInfo;
                 rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_FieldInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{buttonInfo.FieldText},{buttonInfo.FieldName}删除成功。");
                }
            }
            Query();
            return rs;
        }

    }
}
