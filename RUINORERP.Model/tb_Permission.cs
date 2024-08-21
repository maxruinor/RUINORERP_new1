
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/17/2023 12:17:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RUINORERP.Model
{
    /// <summary>
    /// 权限表
    /// </summary>
    [SugarTable("tb_Permission")]
    public partial class tb_Permission: BaseEntity, ICloneable
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        [SugarColumn(ColumnName = "Permission_ID",ColumnDescription = "权限ID" , IsPrimaryKey = true)]
        [Required, MaxLength(32)]
        public int Permission_ID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [SugarColumn(ColumnName = "Permission_Name",ColumnDescription = "权限名称" )]
        [Required, MaxLength(32)]
        public string Permission_Name { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        [SugarColumn(ColumnName = "Description",ColumnDescription = "权限描述" )]
        [Required, MaxLength(32)]
        public string Description { get; set; }



        [SugarColumn(IsIgnore = true)]
        public virtual List<tb_UserInfo> tb_Users { get; set; }


        public new object  Clone()
        {
            tb_LocationType loctype = (tb_LocationType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}