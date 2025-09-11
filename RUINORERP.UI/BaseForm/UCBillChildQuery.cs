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

namespace RUINORERP.UI.BaseForm
{


    public partial class UCBillChildQuery : UCBaseQuery
    {
        public Type entityType;

        public UCBillChildQuery()
        {
            InitializeComponent();
            GridRelated = new GridViewRelated();
            // newSumDataGridViewChild.CellFormatting += DataGridView1_CellFormatting;
            newSumDataGridViewChild.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridViewChild_CellDoubleClick);
        }
        public GridViewDisplayTextResolver DisplayTextResolver;
        private void UCBillChildQuery_Load(object sender, EventArgs e)
        {
            DisplayTextResolver = new GridViewDisplayTextResolver(entityType);
            newSumDataGridViewChild.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewChild.XmlFileName = this.Name + entityType.Name + "Child";
            newSumDataGridViewChild.FieldNameList = UIHelper.GetFieldNameColList(entityType);

            //统一在这里暂时隐藏外币相关
            foreach (var item in newSumDataGridViewChild.FieldNameList)
            {
                if (item.Key.Contains("Foreign") || item.Key.Contains("ExchangeRate"))
                {
                    if (!InvisibleCols.Contains(item.Key))
                    {
                        InvisibleCols.Add(item.Key);
                    }
                }
            }

            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewChild.FieldNameList.TryRemove(item, out kv);
            }
            newSumDataGridViewChild.BizInvisibleCols = InvisibleCols;
            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewChild.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                newSumDataGridViewChild.FieldNameList.TryAdd(item, Newkv);
                //newSumDataGridViewChild.FieldNameList.TryUpdate(item, Newkv, kv);
            }

            newSumDataGridViewChild.DataSource = bindingSourceChild;
            newSumDataGridViewChild.CellFormatting -= DataGridView1_CellFormatting;
            DisplayTextResolver.Initialize(newSumDataGridViewChild);
        }



        /// <summary>
        /// 双击单号导向单据的功能类
        /// </summary>
        public GridViewRelated GridRelated { get; set; }


        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void newSumDataGridViewChild_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
           
            if (newSumDataGridViewChild.CurrentRow != null && newSumDataGridViewChild.CurrentCell != null)
            {
                if (newSumDataGridViewChild.CurrentRow.DataBoundItem != null)
                {
                   GridRelated.GuideToForm(newSumDataGridViewChild.Columns[e.ColumnIndex].Name, newSumDataGridViewChild.CurrentRow);
                }

            }

        }


        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewChild.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (newSumDataGridViewChild.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
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
            string colDbName = newSumDataGridViewChild.Columns[e.ColumnIndex].Name;
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

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue(entityType, colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName) && colName != "System.Object")
            {
                e.Value = colName;
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }



    }
}
