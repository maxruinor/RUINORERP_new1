using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using RUINORERP.Business;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.Common.Extensions;
using SHControls.DataGrid;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Collections.Concurrent;
using System.Threading;
using System.Reflection;



namespace RUINORERP.UI.BaseForm
{
    public partial class UCBillMasterQuery : UCBaseQuery
    {
        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// </summary>
        public Type entityType { get; set; }

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// </summary>
        //public Type ColDisplayType { get; set; }


        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> ColDisplayTypes { get; set; } = new List<Type>();

        public UCBillMasterQuery()
        {
            InitializeComponent();
            GridRelated = new GridViewRelated();
        }


        public GridViewDisplayTextResolver DisplayTextResolver;

        private void UCBillMasterQuery_Load(object sender, EventArgs e)
        {
            DisplayTextResolver = new GridViewDisplayTextResolver(entityType);
            newSumDataGridViewMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewMaster.XmlFileName = this.Name + entityType.Name + "BaseMasterQueryWithCondition";
            newSumDataGridViewMaster.FieldNameList = UIHelper.GetFieldNameColList(entityType);


            //统一在这里暂时隐藏外币相关
            foreach (var item in newSumDataGridViewMaster.FieldNameList)
            {
                if (item.Key.Contains("Foreign") || item.Key.Contains("ExchangeRate"))
                {
                    if (!InvisibleCols.Contains(item.Key))
                    {
                        InvisibleCols.Add(item.Key);
                    }
                }
            }


            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewMaster.FieldNameList.TryRemove(item, out kv);
            }

            newSumDataGridViewMaster.BizInvisibleCols = InvisibleCols;

            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewMaster.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                newSumDataGridViewMaster.FieldNameList.TryAdd(item, Newkv);
                //newSumDataGridViewMaster.FieldNameList.TryUpdate(item, Newkv, kv);
            }

            newSumDataGridViewMaster.DataSource = bindingSourceMaster;
            //newSumDataGridViewMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //newSumDataGridViewMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            bindingSourceMaster.CurrentItemChanged += BindingSourceMaster_CurrentItemChanged;
            bindingSourceMaster.CurrentChanged += BindingSourceMaster_CurrentChanged;
            //newSumDataGridViewMaster.ColumnDisplayControlToCaption(newSumDataGridViewMaster.FieldNameList);

            //newSumDataGridViewMaster.ReadOnly = false;

            //FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
            //dataGridView1.XmlFileName = "Query" + typeof(T).Name;
            //this.dataGridView1.FieldNameList = this.FieldNameList;
            //dataGridView1.ReadOnly = true;

            //List<T> list = new List<T>();
            //ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            //dataGridView1.DataSource = ListDataSoure;
            //dataGridView1.FieldNameList = this.FieldNameList;

            //视图要指定类型才能找到外键 暂时还是要用格式化来显示名称项目
            if (entityType.Name.StartsWith("tb_"))
            {
                newSumDataGridViewMaster.CellFormatting -= DataGridView1_CellFormatting;
                DisplayTextResolver.Initialize(newSumDataGridViewMaster, ColDisplayTypes.ToArray());
            }

        }

        private void BindingSourceMaster_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void BindingSourceMaster_CurrentItemChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is BindingSource bindingSource)
            {
                if (OnSelectDataRow != null && bindingSource.Current != null)
                {
                    TriggerOnSelectDataRowWithDebounce(bindingSource.Current);
                }
            }
        }

        /// <summary>
        /// 显示汇总列
        /// </summary>
        public void ShowSummaryCols()
        {
            //List<string> SummaryCols
            //newSumDataGridViewMaster
            newSumDataGridViewMaster.IsShowSumRow = true;
            newSumDataGridViewMaster.SumColumns = SummaryCols.ToArray();
        }

        /// <summary>
        /// 带防抖机制的触发OnSelectDataRow事件
        /// 避免短时间内多次触发导致重复加载数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void TriggerOnSelectDataRowWithDebounce(object entity)
        {
            if (entity == null)
            {
                return;
            }

            // 获取实体的唯一标识
            var entityHash = GetEntityHash(entity);

            // 使用锁确保线程安全
            lock (_onSelectDataRowLock)
            {
                // 如果与上次选中的实体相同，直接返回
                if (entityHash != null && entityHash.Equals(_lastSelectedEntityHash))
                {
                    return;
                }

                // 更新上次选中的实体标识
                _lastSelectedEntityHash = entityHash;

                // 取消之前的防抖任务（如果有）
                _onSelectDataRowDebounceTokenSource?.Cancel();
                _onSelectDataRowDebounceTokenSource = new CancellationTokenSource();
            }

            // 异步执行防抖逻辑
            Task.Run(async () =>
            {
                try
                {
                    // 防抖延迟50毫秒，避免短时间内多次触发
                    await Task.Delay(50, _onSelectDataRowDebounceTokenSource.Token);

                    // 在UI线程中触发事件
                    if (!_onSelectDataRowDebounceTokenSource.Token.IsCancellationRequested && OnSelectDataRow != null)
                    {
                        if (newSumDataGridViewMaster.InvokeRequired)
                        {
                            newSumDataGridViewMaster.Invoke(new Action(() => OnSelectDataRow(entity)));
                        }
                        else
                        {
                            OnSelectDataRow(entity);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消，正常情况，不处理
                }
                catch (Exception ex)
                {
                    // 记录异常，但不影响主流程
                    System.Diagnostics.Debug.WriteLine($"触发OnSelectDataRow事件时发生异常: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// 获取实体的唯一哈希值，用于判断是否切换了数据行
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体哈希值</returns>
        private object GetEntityHash(object entity)
        {
            if (entity == null)
            {
                return null;
            }

            // 尝试获取主键属性的值作为唯一标识
            var pkProperty = entity.GetType().GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null ||
                                   p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase) ||
                                   p.Name.Equals(entity.GetType().Name + "ID", StringComparison.OrdinalIgnoreCase));

            if (pkProperty != null)
            {
                var pkValue = pkProperty.GetValue(entity);
                return pkValue;
            }

            // 如果没有找到主键属性，返回实体的HashCode
            return entity.GetHashCode();
        }



        public delegate void SelectDataRowHandler(object entity, object bizKey = null);

        /// <summary>
        /// 双击将数据载入到明细外部事件
        /// </summary>
        [Browsable(true), Description("双击将数据载入到明细外部事件")]
        public event SelectDataRowHandler OnSelectDataRow;

        /// <summary>
        /// 防抖用的CancellationTokenSource，用于避免短时间内多次触发数据加载
        /// </summary>
        private CancellationTokenSource _onSelectDataRowDebounceTokenSource;

        /// <summary>
        /// 上次选中的实体标识，用于判断是否真的切换了数据行
        /// </summary>
        private object _lastSelectedEntityHash;

        /// <summary>
        /// 用于同步的锁对象
        /// </summary>
        private readonly object _onSelectDataRowLock = new object();

        private void bindingSourceMaster_PositionChanged(object sender, EventArgs e)
        {
            if (OnSelectDataRow != null && newSumDataGridViewMaster.CurrentRow != null)
            {
                //OnSelectDataRow(bindingSourceMaster.Current);
            }
        }

        private void newSumDataGridViewMaster_DataSourceChanged(object sender, EventArgs e)
        {
            if (OnSelectDataRow != null && bindingSourceMaster.Current != null)
            {
                TriggerOnSelectDataRowWithDebounce(bindingSourceMaster.Current);
            }
        }


        /// <summary>
        /// 双击单号导向单据的功能类
        /// </summary>
        public GridViewRelated GridRelated { get; set; }


        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void newSumDataGridViewMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }

            if (newSumDataGridViewMaster.CurrentRow != null && newSumDataGridViewMaster.CurrentCell != null)
            {
                if (newSumDataGridViewMaster.CurrentRow.DataBoundItem != null)
                {
                    GridRelated.GuideToForm(newSumDataGridViewMaster.Columns[e.ColumnIndex].Name, newSumDataGridViewMaster.CurrentRow);
                }

            }
            //如果结果就一条。就同时显示到子表。因为多行切换才触发位置改变 这行不能去掉
            if (newSumDataGridViewMaster.RowCount == 1 && OnSelectDataRow != null && bindingSourceMaster.Current != null)
            {
                TriggerOnSelectDataRowWithDebounce(bindingSourceMaster.Current);
                return;
            }
        }



        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewMaster.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }

            //图片特殊处理
            if (newSumDataGridViewMaster.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                    return;
                }
            }
            //固定字典值显示
            string colDbName = newSumDataGridViewMaster.Columns[e.ColumnIndex].Name;
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
                        return;
                    }
                }
            }

            if (bindingSourceMaster.Current != null)
            {
                Type dataType = bindingSourceMaster.Current.GetType().GetProperty(newSumDataGridViewMaster.Columns[e.ColumnIndex].DataPropertyName).PropertyType;
                // We need to check whether the property is NULLABLE
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    dataType = dataType.GetGenericArguments()[0];
                }

                //下次优化时。要注释一下 什么类型的字段 数据 要特殊处理。实际可能又把另一个情况弄错。
                switch (dataType.FullName)
                {
                    case "System.Boolean":
                        break;
                    case "System.DateTime":
                        if (newSumDataGridViewMaster.Columns[e.ColumnIndex].HeaderText.Contains("日期"))
                        {
                            e.Value = DateTime.Parse(e.Value.ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case "System.Int32":
                    case "System.String":
                        if (dataType.FullName == "System.Int32"
                            && newSumDataGridViewMaster.Columns[e.ColumnIndex].HeaderText.Contains("状态"))
                        {

                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }

            }

            //动态字典值显示
            string colName = string.Empty;
            if (ColDisplayTypes != null && ColDisplayTypes.Count > 0)
            {
                colName = UIHelper.ShowGridColumnsNameValue(ColDisplayTypes.ToArray(), colDbName, e.Value);
            }
            else
            {
                colName = UIHelper.ShowGridColumnsNameValue(entityType, colDbName, e.Value);
            }
            if (!string.IsNullOrEmpty(colName) && colName != "System.Object")
            {
                e.Value = colName;
            }
            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }

        private void newSumDataGridViewMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }




    }
}
