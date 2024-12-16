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
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("协作人列表", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    public partial class UCCRMCollaboratorList : BaseForm.BaseListGeneric<tb_CRM_Collaborator>
    {
        public UCCRMCollaboratorList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMCollaboratorEdit);
        }
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_Collaborator>()
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了只看到自己的
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


    }
}
