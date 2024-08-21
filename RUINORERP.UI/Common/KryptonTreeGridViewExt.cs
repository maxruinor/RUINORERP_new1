using Krypton.Toolkit.Suite.Extended.TreeGridView;
using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.UI.Common;


namespace RUINORERP.UI.Common
{
    public static class KryptonTreeGridViewExt
    {
 
        public static void SetHideColumns<T>(this KryptonTreeGridView kryptonTreeGrid, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            kryptonTreeGrid.SetHideColumns(minfo.Name);
        }
    }
}
