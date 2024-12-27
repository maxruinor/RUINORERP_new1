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

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("批注管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.行政资料)]
    public partial class UCglCommentList : BaseForm.BaseListGeneric<tb_gl_Comment>
    {
        public UCglCommentList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCglCommentEdit);
        }


        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.DbTableName);
        }
    }
}
