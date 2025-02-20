
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:56
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
 
    public partial class tb_CRM_Region: BaseEntity, ICloneable
    { 


    

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Parent_region_id))]
        public virtual tb_CRM_Region tb_crm_region { get; set; }
                

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Region.Parent_region_id))]
        public virtual List<tb_CRM_Region> tb_CRM_Regions { get; set; }
        //tb_CRM_Region.Region_ID)
        //Region_ID.FK_REGION_REF_REGION)
        //tb_CRM_Region.Parent_region_id)


        #endregion


 

 
        
 
    }
}

