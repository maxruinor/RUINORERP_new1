
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:40
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 库位表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Location")]
    public partial class tb_LocationQueryDto:BaseEntityDto
    {
        public tb_LocationQueryDto()
        {

        }

    
     

        private long? _LocationType_ID;
        /// <summary>
        /// 库位类型
        /// </summary>
        [AdvQueryAttribute(ColName = "LocationType_ID",ColDesc = "库位类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "LocationType_ID",IsNullable = true,ColumnDescription = "库位类型" )]
        [FKRelationAttribute("tb_LocationType","LocationType_ID")]
        public long? LocationType_ID 
        { 
            get{return _LocationType_ID;}
            set{SetProperty(ref _LocationType_ID, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "联系人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "联系人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private string _LocationCode;
        /// <summary>
        /// 仓库代码
        /// </summary>
        [AdvQueryAttribute(ColName = "LocationCode",ColDesc = "仓库代码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "LocationCode",Length=50,IsNullable = false,ColumnDescription = "仓库代码" )]
        public string LocationCode 
        { 
            get{return _LocationCode;}
            set{SetProperty(ref _LocationCode, value);}
        }
     

        private string _Tel;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Tel",ColDesc = "电话")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Tel",Length=20,IsNullable = true,ColumnDescription = "电话" )]
        public string Tel 
        { 
            get{return _Tel;}
            set{SetProperty(ref _Tel, value);}
        }
     

        private string _Name;
        /// <summary>
        /// 仓库名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "仓库名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Name",Length=50,IsNullable = false,ColumnDescription = "仓库名称" )]
        public string Name 
        { 
            get{return _Name;}
            set{SetProperty(ref _Name, value);}
        }
     

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Desc",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc 
        { 
            get{return _Desc;}
            set{SetProperty(ref _Desc, value);}
        }


       
    }
}



