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
using System.Collections.Concurrent;
using RUINORERP.Common.Extensions;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Documents;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("产品类目", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductCategoriesList : BaseForm.BaseListWithTree<tb_ProdCategories>
    {
        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();
        //public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }

        public UCProductCategoriesList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCProductCategoriesEdit);

        
           
        }

        tb_ProdCategoriesController<tb_ProdCategories> ctr = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
        protected override async void Add()
        {
            if (ListDataSoure.Count == 0)
            {
                //弹出提示框：系统将默认为您添加人民币和美元币别。
                //MessageBox.Show("系统将默认为您添加类目根节点。");
                //第一次添加付款方式时，添加系统默认的值  优化
                //循环枚举DefaultPaymentMethod中的值，添加到表中
                tb_ProdCategories category = new tb_ProdCategories();
                category.Is_enabled = true;
                category.Category_name = "类目根节点";
                //这里不用产生雪花ID，因为是根节点，后面有逻辑是根据 parent_id来判断的  ID=0
                await MainForm.Instance.AppContext.Db.Insertable<tb_ProdCategories>(category).ExecuteCommandAsync();
                Query();
            }

            UCProductCategoriesEdit frmadd = new UCProductCategoriesEdit();
            frmadd.bindingSourceEdit = bindingSourceList;
            object obj = frmadd.bindingSourceEdit.AddNew();
            frmadd.categories = list;
            frmadd.BindData(obj as BaseEntity);
            //RevertCommand command = new RevertCommand();
            ///*
            //* 使用匿名委托，更加简单，而且匿名委托方法里还可以使用外部变量。
            //*/
            //command.UndoOperation = delegate ()
            //{
            //    //Undo操作会执行到的代码
            //    frmadd.bindingSourceEdit.Remove(obj);
            //};

            if (frmadd.ShowDialog() == DialogResult.OK)
            {
                base.ToolBarEnabledControl(MenuItemEnums.新增);
            }
            else
            {
                frmadd.bindingSourceEdit.CancelEdit();
                //command.Undo();
            }

        }

        protected async override void Delete()
        {
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                tb_ProdCategories loc = (tb_ProdCategories)this.bindingSourceList.Current;
                this.bindingSourceList.Remove(loc);

                await ctr.DeleteAsync(loc);
            }

        }

        protected override void Modify()
        {
            if (base.bindingSourceList.Current != null)
            {
                RevertCommand command = new RevertCommand();
                UCProductCategoriesEdit frmadd = new UCProductCategoriesEdit();
                frmadd.bindingSourceEdit = bindingSourceList;
                frmadd.categories = list;
                frmadd.BindData(base.bindingSourceList.Current as BaseEntity);
                //缓存当前编辑的对象。如果撤销就回原来的值
                // object obj = (base.bindingSourceList.Current as tb_LocationType).Clone();
                object oldobj = CloneHelper.DeepCloneObject_maxnew<tb_ProdCategories>(base.bindingSourceList.Current as tb_ProdCategories);
                int UpdateIndex = base.bindingSourceList.Position;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<tb_ProdCategories>(base.bindingSourceList[UpdateIndex], oldobj);
                };
                if (frmadd.ShowDialog() == DialogResult.Cancel)
                {
                    command.Undo();
                }
                else
                {
                    base.ToolBarEnabledControl(MenuItemEnums.修改);

                }
                dataGridView1.Refresh();
            }
        }
        List<tb_ProdCategories> list = new List<tb_ProdCategories>();
        protected async override void Query()
        {
            if (base.Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }

            }

            ListDataSoure.DataSourceChanged += ListDataSoure_DataSourceChanged;
            dataGridView1.ReadOnly = true;
            list = await ctr.QueryAsync();
            ListDataSoure.DataSource = list.ToBindingSortCollection();
            dataGridView1.DataSource = ListDataSoure;

            base.ShowToolBarOfList();
            base.ToolBarEnabledControl(MenuItemEnums.查询);
        }

        private void ListDataSoure_DataSourceChanged(object sender, EventArgs e)
        {
            BindingSortCollection<tb_ProdCategories> list = ListDataSoure.DataSource as BindingSortCollection<tb_ProdCategories>;
            List<tb_ProdCategories> clist = list.ToList<tb_ProdCategories>();
            UIProdCateHelper.BindToTreeView(clist, base.kryptonTreeView1);
        }

        protected async override void Save()
        {
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_ProdCategories;

                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                        BusinessHelper.Instance.InitEntity(entity);
                        await ctr.AddReEntityAsync(entity);
                        break;
                    case ActionStatus.修改:
                        BusinessHelper.Instance.EditEntity(entity);
                        await ctr.UpdateAsync(entity);
                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }

            }
            base.ToolBarEnabledControl(MenuItemEnums.保存);
        }


        protected override void Exit(object thisform)
        {
            base.Exit(this);
        }

        private void UCLocationTypeList_Load(object sender, EventArgs e)
        {
            ///显示列表对应的中文
            base.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_ProdCategories)); // UIHelper.GetFieldNameList<tb_ProdCategories>();
            base.Refreshs();
            dataGridView1.NeedSaveColumnsXml = true;
            base.dataGridView1.XmlFileName = typeof(tb_ProdCategories).Name;
            //dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        //集成到了基类实现
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Expression<Func<tb_ProdCategories, long?>> expKey = c => c.parent_id;
            //MemberInfo minfo = expKey.GetMemberInfo();
            //string key = minfo.Name;

            if (base.dataGridView1.Columns[e.ColumnIndex].Name == "Parent_id")
            {
                if (e.Value == null || e.Value.ToString() == "0")
                {
                    e.Value = "类目根节点";
                }
                else
                {
                    //这里要用缓存
                    BindingSortCollection<tb_ProdCategories> list = ListDataSoure.DataSource as BindingSortCollection<tb_ProdCategories>;
                    List<tb_ProdCategories> clist = list.ToList<tb_ProdCategories>();
                    tb_ProdCategories pc = clist.Find(t => t.Category_ID == int.Parse(e.Value.ToString()));
                    if (pc != null)
                    {
                        e.Value = pc.Category_name;
                    }

                }
                return;
            }


            //字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }
        }
    }
}
