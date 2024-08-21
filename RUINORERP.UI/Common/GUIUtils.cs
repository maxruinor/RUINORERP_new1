using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public static class GUIUtils
    {





        public static void Add<T>(this ControlBindingsCollection bindings, string propertyName, object dataSource, Expression<Func<T>> expr)
        {
            string dataMember = RUINORERP.Global.Utils.GetMemberName(expr);
            Binding bind = new Binding(propertyName, dataSource, dataMember);
          
                   bindings.Add(propertyName, dataSource, dataMember);
     
        }
        public static class ControlBindingProperty
        {
            public const string Text = "Text";
        }

    }
}
