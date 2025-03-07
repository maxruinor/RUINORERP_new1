
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/18/2024 22:55:50
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{

    public partial class View_Inventory : BaseViewEntity
    {

        //手动添加一个属性来定义到 关联的对象 ？
        public override void InitRelatedTableTypes()
        {
            base.SetRelatedTableTypes<tb_Inventory>();
            base.SetRelatedTableTypes<tb_Prod>();
        }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Inventory_ID))]
        public virtual tb_Inventory tb_inventory { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }
   

    }
}

