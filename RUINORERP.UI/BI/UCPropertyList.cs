﻿using System;
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


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("属性管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.仓库资料)]
    public partial class UCPropertyList : BaseForm.BaseListGeneric<tb_ProdProperty>
    {
        public UCPropertyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCPropertyEdit);

        }

        


    }
}
