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
using RUINORERP.UI.UControls;
using RUINORERP.Business.Processor;
using SqlSugar;
using System.Reflection;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.CommonUI;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using FastReport.DevComponents.DotNetBar.Controls;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("按钮管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCButtonInfoList : BaseForm.BaseListGeneric<tb_ButtonInfo>
    {
        public UCButtonInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCButtonInfoEdit);
            if (!this.DesignMode)
            {
                MenuAssemblylist = UIHelper.RegisterForm();
            }
        }
        private void button提取重复数据_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.UseSelectedColumn)
            {
                dataGridView1.UseSelectedColumn = true;
            }
            // 定义参与比较的属性列表
            var ButtonProperties = new List<string>()
            .Include<tb_ButtonInfo>(c => c.BtnName)
            .Include<tb_ButtonInfo>(c => c.MenuID);
            var oklist = UITools.CheckDuplicateData<tb_ButtonInfo>(ListDataSoure.Cast<tb_ButtonInfo>().ToList(), ButtonProperties.ToList());

            #region 最新检查重复的方法
            List<long> buttonids = oklist.Select(c => c.ButtonInfo_ID).ToList();
            if (dataGridView1.UseSelectedColumn)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var dr = dataGridView1.Rows[i];
                    if (dr.DataBoundItem is tb_ButtonInfo buttonInfo)
                    {
                        if (buttonids.Contains(buttonInfo.ButtonInfo_ID))
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




        /// <summary>
        /// 在一个角色下的一个菜单窗体下 按钮不要重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button检查数据1_Click(object sender, EventArgs e)
        {
            List<tb_ButtonInfo> list = new List<tb_ButtonInfo>();
            list = ListDataSoure.Cast<tb_ButtonInfo>().ToList();

            //下面要判断list中是否有重复的数据。

            string pkName = UIHelper.GetPrimaryKeyColName(typeof(tb_ButtonInfo));
            // 使用 GroupBy 筛选出重复数据,排除掉主键，将其他所有列【SugarColumn】有效的，都参与比较
            // 创建一个用于获取所有键属性值的匿名函数
            // 创建分组键选择器(Tuple) 一个对象中，哪些字段属性参与比较

            ////按指定的字段集合来比较是否重复
            //var IncludeProperties = new List<string>()
            //.Include<tb_ButtonInfo>(c => c.MenuID)
            //.Include<tb_ButtonInfo>(c => c.BtnText)
            //.Include<tb_ButtonInfo>(c => c.FormName)
            //.Include<tb_ButtonInfo>(c => c.ClassPath)
            //.Include<tb_ButtonInfo>(c => c.BtnName);



            Func<tb_ButtonInfo, Tuple<object[]>> keySelector2 = p =>
            {
                PropertyInfo[] properties = typeof(tb_ButtonInfo).GetProperties()
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

            ListDataSoure.DataSource = duplicatesList.ToBindingSortCollection<tb_ButtonInfo>();
            dataGridView1.DataSource = ListDataSoure;
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
            toolStripButton提取重复数据.Click += new System.EventHandler(this.button提取重复数据_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton提取重复数据 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }


        #endregion


        public override void BuildRelatedDisplay()
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型
            ColDisplayTypes.Add(typeof(tb_MenuInfo));
        }
        #region
        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测按钮是否存在);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【检测按钮是否存在】", true, false, "NewSumDataGridView_检测按钮是否存在"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测按钮是否存在);


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
        List<MenuAttrAssemblyInfo> MenuAssemblylist = new List<MenuAttrAssemblyInfo>();
       
        private async void NewSumDataGridView_检测按钮是否存在(object sender, EventArgs e)
        {
            dataGridView1.UseSelectedColumn = true;

            // 提取所有按钮信息
            var buttonList = this.dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem as tb_ButtonInfo)
                .Where(bi => bi != null)
                .ToList();

            // 按ClassPath分组并获取唯一的类路径集合
            var uniqueClassPaths = buttonList
                .GroupBy(bi => bi.ClassPath)
                .Select(g => g.Key)
                .ToList();

            // 并行处理每个类路径
            await Task.WhenAll(uniqueClassPaths.Select(async classPath =>
            {
                // 查找菜单信息
                var menuInfo = MainForm.Instance.AppContext.UserMenuList
                    .FirstOrDefault(m => m.ClassPath == classPath);

                // 查找菜单属性信息
                var menuAttrInfo = MenuAssemblylist
                    .FirstOrDefault(mai => mai.ClassPath == classPath);

                if (menuInfo != null && menuAttrInfo != null)
                {
                    // 在UI线程中初始化菜单项
                    await MainForm.Instance.InvokeAsync(async () =>
                    {
                        var initModuleMenu = Startup.GetFromFac<InitModuleMenu>();
                        if (initModuleMenu != null)
                        {
                            var existingButtons = await initModuleMenu.InitToolStripItemAsync(menuAttrInfo, menuInfo, false);

                        // 获取当前类路径下的所有按钮
                        var classPathButtons = buttonList
                            .Where(bi => bi.ClassPath == classPath)
                            .ToList();

                            // 筛选出在buttonList中存在但不在existingButtons中的按钮
                            var buttonsToSelect = classPathButtons
                                .Where(bi => !existingButtons.Any(eb => eb.BtnName == bi.BtnName && eb.BtnText == bi.BtnText))
                                .ToList();

                            // 更新选中状态
                            foreach (var button in buttonsToSelect)
                            {
                                button.Selected = true;
                            }
                        }
                    });
                }
            }));
        }

        #endregion



        //因为tb_P4Button中引用了按钮表中的信息，所以要使用导航删除。但是一定要细心

        tb_ButtonInfoController<tb_ButtonInfo> childctr = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
        protected async override Task<bool> Delete()
        {
            bool rs = false;
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_ButtonInfo buttonInfo = dr.DataBoundItem as tb_ButtonInfo;
                rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_ButtonInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{buttonInfo.BtnText},{buttonInfo.BtnName}删除成功。");
                }
            }
            QueryAsync();
            return rs;
        }


    }
}
