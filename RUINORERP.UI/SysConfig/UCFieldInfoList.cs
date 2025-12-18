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
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.UControls;
using FastReport.Table;
using RUINORERP.Common.Extensions;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("字段管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCFieldInfoList : BaseForm.BaseListGeneric<tb_FieldInfo>, IContextMenuInfoAuth, IToolStripMenuInfoAuth
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

            dataGridView1.UseBatchEditColumn = true;
        }

        #region 添加 提取重复数据

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            ToolStripButton toolStripButton提取重复数据 = new System.Windows.Forms.ToolStripButton();
            toolStripButton提取重复数据.Text = "提取重复数据";
            //toolStripButton提取重复数据.Image = global::RUINORERP.UI.Properties.Resources.MakeSureCost;
            toolStripButton提取重复数据.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton提取重复数据.Name = "提取重复数据";
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton提取重复数据);
            toolStripButton提取重复数据.ToolTipText = "提取重复数据，有一行会保留，没有显示出来。。";
            toolStripButton提取重复数据.Click += new System.EventHandler(button提取重复数据_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton提取重复数据 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }
        private void button提取重复数据_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.UseSelectedColumn)
            {
                dataGridView1.UseSelectedColumn = true;
            }
            // 定义参与比较的属性列表
            var ButtonProperties = new List<string>()
            .Include<tb_FieldInfo>(c => c.FieldName)
            .Include<tb_FieldInfo>(c => c.IsChild)
            .Include<tb_FieldInfo>(c => c.MenuID);

            var oklist = UITools.CheckDuplicateData<tb_FieldInfo>(ListDataSoure.Cast<tb_FieldInfo>().ToList(), ButtonProperties.ToList());

            #region 最新检查重复的方法
            List<long> buttonids = oklist.Select(c => c.FieldInfo_ID).ToList();
            if (dataGridView1.UseSelectedColumn)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var dr = dataGridView1.Rows[i];
                    if (dr.DataBoundItem is tb_FieldInfo buttonInfo)
                    {
                        if (buttonids.Contains(buttonInfo.FieldInfo_ID))
                        {
                            dr.Cells["Selected"].Value = true;
                        }
                        else
                        {
                            dr.Cells["Selected"].Value = false;
                        }
                    }
                }

            }


            #endregion


        }

        #endregion


        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测字段是否存在);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【检测字段是否存在】", true, false, "NewSumDataGridView_检测字段是否存在"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测字段是否存在);


            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            UIHelper.ControlContextMenuInvisible(CurMenuInfo, list);

            if (dataGridView1 != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.dataGridView1.GetContextMenu(this.dataGridView1.ContextMenuStrip
                , ContextClickList, list, true
                    );
                dataGridView1.ContextMenuStrip = newContextMenuStrip;
            }


        }

        private void NewSumDataGridView_检测字段是否存在_old(object sender, EventArgs e)
        {
            dataGridView1.UseSelectedColumn = true;

            UIHelper.CheckValidation(this);
            List<tb_FieldInfo> CheckList = new List<tb_FieldInfo>();
            //多选模式时
            if (dataGridView1.UseSelectedColumn)
            {
                foreach (var item in bindingSourceList)
                {
                    if (item is tb_FieldInfo sourceEntity)
                    {
                        CheckList.Add(sourceEntity);
                    }
                }
            }

            //再将列表按 EntityName 分组处理。因为每组要创建一个对应的实例
            var groupList = CheckList.GroupBy(c => c.ClassPath).ToArray();
            foreach (var group in groupList)
            {

                string ClassPath = group.Key;
                List<tb_FieldInfo> list = group.ToList();
                //                var type = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + tableName);
                var type = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(ClassPath);
                //创建这个类型的实体
                var entity = Activator.CreateInstance(type);
                foreach (var item in list)
                {
                    if (!entity.ContainsProperty(item.FieldName))
                    {
                        //勾选
                        foreach (var field in bindingSourceList)
                        {
                            if (item is tb_FieldInfo sourceEntity)
                            {
                                if (sourceEntity.FieldName == item.FieldName)
                                {
                                    //勾选
                                    (sourceEntity as BaseEntity).Selected = true;
                                }
                            }
                        }
                    }
                }
            }

        }
        private void NewSumDataGridView_检测字段是否存在(object sender, EventArgs e)
        {
            dataGridView1.UseSelectedColumn = true;
            UIHelper.CheckValidation(this);

            // 提取所有需要检查的字段信息
            var checkList = bindingSourceList
                .OfType<tb_FieldInfo>()
                .ToList();

            // 按ClassPath分组处理
            var groupedFields = checkList
                .GroupBy(f => f.ClassPath)
                .ToList();

            // 缓存已加载的类型，避免重复加载
            var typeCache = new Dictionary<string, Type>();

            foreach (var group in groupedFields)
            {
                string classPath = group.Key;
                List<tb_FieldInfo> fieldsInClass = group.ToList();

                // 从缓存或程序集中获取类型
                if (!typeCache.TryGetValue(classPath, out Type entityType))
                {
                    try
                    {
                        entityType = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(classPath);
                        typeCache[classPath] = entityType;
                    }
                    catch (Exception ex)
                    {
                        // 记录错误日志，继续处理其他组
                        System.Diagnostics.Debug.WriteLine($"加载类型 {classPath} 时出错: {ex.Message}");
                        continue;
                    }
                }

                if (entityType == null)
                {
                    // 类型不存在，选中该组所有字段
                    foreach (var field in fieldsInClass)
                    {
                        field.Selected = true;
                    }
                    continue;
                }

                // 获取类型的所有属性
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

                // 检查每个字段是否存在于实体中
                foreach (var field in fieldsInClass)
                {
                    if (!properties.ContainsKey(field.FieldName))
                    {
                        field.Selected = true;
                    }
                }
            }
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


        public override void BuildRelatedDisplay()
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型
            ColDisplayTypes.Add(typeof(tb_MenuInfo));
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
            QueryAsync();
            return rs;
        }

    }
}
