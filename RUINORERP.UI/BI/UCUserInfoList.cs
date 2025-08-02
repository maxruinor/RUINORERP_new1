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

using RUINORERP.Model.Base;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("用户信息", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCUserInfoList : BaseForm.BaseListGeneric<tb_UserInfo>
    {
        public UCUserInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCUserInfoEdit);
           
        }

        public override void BuildInvisibleCols()
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                base.InvisibleColsExp.Add(c => c.Password);
                base.InvisibleColsExp.Add(c => c.IsSuperUser);
            }
           
        }

    }
}
