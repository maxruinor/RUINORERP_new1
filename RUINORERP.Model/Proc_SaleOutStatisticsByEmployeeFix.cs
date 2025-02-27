
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 20:42:09
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    public partial class Proc_SaleOutStatisticsByEmployee : BaseViewEntity
    {
        //手动添加一个属性来定义到 关联的对象 ？
        public override void InitRelatedTableTypes()
        {
            base.SetRelatedTableTypes<tb_Employee>();
            base.SetRelatedTableTypes<tb_ProjectGroup>();
        }

    }
}

