using RUINORERP.Model;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 复杂数据类型编辑器
    /// 目前应用于产品选择
    /// </summary>
    public class EditorQuery : SourceGrid.Cells.Editors.TextBoxButton
    {
        #region by watson 

        public delegate void SelectMultiRowData(object rows);

        /// <summary>
        /// 选数据时多行选择
        /// </summary>
        public event SelectMultiRowData OnSelectMultiRowData;

        #endregion
        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.DevAgeTextBoxButton Control
        {
            get
            {
                return (DevAge.Windows.Forms.DevAgeTextBoxButton)base.Control;
            }
        }


        Type queryobjType;
        private string _queryField = string.Empty;
        public EditorQuery()
            : base(typeof(string))
        {
            Control.DialogOpen += new EventHandler(Control_DialogOpen);

        }


        public bool CanMultiSelect { get; set; } = true;




        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryField">对象属性列名</param>
        /// <param name="p_Type">对象类型</param>
        public EditorQuery(string queryField, Type p_Type, bool canMultiSelect = true) : base(p_Type)
        {
            CanMultiSelect = canMultiSelect;
            queryobjType = p_Type;
            _queryField = queryField;
            Control.DialogOpen += new EventHandler(Control_DialogOpen);
        }

        public string QueryField { get => _queryField; set => _queryField = value; }

        public override string ValueToDisplayString(object p_Value)
        {
            //if (p_Value != null && p_Value.GetType() == queryobjType)
            //{
            //    return RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(p_Value, QueryField);
            //}
            //else
            //{
            return base.ValueToDisplayString(p_Value);
            //}

        }

        void Control_DialogOpen(object sender, EventArgs e)
        {

            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                ////设置一下默认仓库，思路是，首先保存了主表的数据对象，然后拿到主表的仓库字段
                ////这里是不是可以设置为事件来驱动,的并且可以指定字段
                //Expression<Func<View_ProdDetail, object>> warehouse = x => x.Location_ID;
                //if (dci.ParentGridDefine.DefineColumns.FirstOrDefault(c => c.ColName == warehouse.GetMemberInfo().Name) != null)
                //{
                //    dg.LocationID = dci.ParentGridDefine.GridData.GetPropertyValue(warehouse.GetMemberInfo().Name).ToLong();
                //}
                dg.CanMultiSelect = CanMultiSelect;
                dg.Text = "产品选择";
                dg.StartPosition = FormStartPosition.CenterScreen;
                dg.prodQuery.QueryField = this.QueryField;
                if (this.EditCellContext.Value != null)
                {
                    dg.prodQuery.QueryValue = this.EditCellContext.Value.ToString();
                }

                if (dg.ShowDialog() == DialogResult.OK)
                {
                    Control.Tag = dg.prodQuery.QueryObjects;
                    Control.Value = dg.prodQuery.QueryValue;
                    if (dg.prodQuery.QueryObjects.Count > 1)
                    {
                        if (OnSelectMultiRowData != null)
                        {
                            OnSelectMultiRowData(dg.prodQuery.QueryObjects);
                        }
                    }
                }
            }
        }


    }

}
