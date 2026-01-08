using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.SysConfig;
using System.Diagnostics;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.BI;
using RUINORERP.Global.CustomAttribute;
using System.Collections.Concurrent;
using RUINORERP.Common.Helper;
using RUINORERP.Common.CollectionExtension;

namespace RUINORERP.UI.CRM.DockUI
{
    /// <summary>
    /// 显示一个简约式的一个对象对应的多行数据。
    /// </summary>
    public partial class UCCRMContact : UserControl
    {
        public UCCRMContact()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();

        tb_CRM_Contact RecordEntity = null;
        public void BindData<T>(IEnumerable<object> list)
        {
            string tableName = typeof(T).Name;
            foreach (var field in typeof(T).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        FKValueColNameTBList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }

            //这里不这样了，直接用登录时查出来的。按菜单路径找到菜单 去再搜索 字段。
            //    显示按钮也一样的思路
            this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));

            dataGridView1.XmlFileName = tableName + "simple";

            //InvisibleCols = ExpressionHelper.ExpressionListToStringList(InvisibleColsExp);
            //ControlMasterColumnsInvisible(InvisibleCols);
            //foreach (var item in InvisibleCols)
            //{
            //    KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
            //    dataGridView1.FieldNameList.TryRemove(item, out kv);
            //}


            bindingSourceList.DataSource = list;//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;

            //RecordEntity = entity;
            //DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            //DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanStartDate, dtpPlanStartDate, false);
            //DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanEndDate, dtpPlanEndDate, false);

            //DataBindingHelper.BindData4CmbByEnum<tb_CRM_FollowUpPlans>(entity, k => k.PlanStatus, typeof(FollowUpPlanStatus), cmbPlanStatus, false);
            //DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanSubject, txtPlanSubject, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanContent, txtPlanContent, BindDataType4TextBox.Text, false);
        }


        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> ColDisplayTypes { get; set; } = new List<Type>();

        UITools iTools = new UITools();
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridView1.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    if (!(e.Value is byte[]))
                    {
                        return;
                    }
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(buf, true);
                    if (image != null)
                    {
                        //缩略图 这里用缓存 ?
                        System.Drawing.Image thumbnailthumbnail = UITools.CreateThumbnail(image, 100, 100);
                        e.Value = thumbnailthumbnail;
                        return;
                    }

                }
            }
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;


            //动态字典值显示
            string colName = string.Empty;
            if (ColDisplayTypes != null && ColDisplayTypes.Count > 0)
            {
                colName = UIHelper.ShowGridColumnsNameValue(ColDisplayTypes.ToArray(), colDbName, e.Value);
            }
            else
            {
                colName = UIHelper.ShowGridColumnsNameValue<tb_CRM_Contact>(colDbName, e.Value);
            }
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
                return;
            }



            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }


        private void klinklblDetail_LinkClicked(object sender, EventArgs e)
        {
            //打开详情
        }
    }
}
