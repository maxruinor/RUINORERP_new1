using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{

    public class EditorQueryGeneric<T> : SourceGrid.Cells.Editors.TextBoxButton
    {

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
        public EditorQueryGeneric()
            : base(typeof(string))
        {
            Control.DialogOpen += new EventHandler(Control_DialogOpen);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryField">对象属性列名</param>
        /// <param name="p_Type">对象类型</param>
        public EditorQueryGeneric(string queryField, Type p_Type) : base(p_Type)
        {
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
                dg.StartPosition = FormStartPosition.CenterScreen;
                dg.prodQuery.QueryField = this.QueryField;
                if (dg.ShowDialog() == DialogResult.OK)
                {
                    Control.Tag = dg.prodQuery.QueryObjects;
                    Control.Value = dg.prodQuery.QueryValue;
                }
            }
        }
    }

}
