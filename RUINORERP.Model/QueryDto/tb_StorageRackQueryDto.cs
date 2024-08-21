
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:16
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
    /// 货架信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_StorageRack")]
    public partial class tb_StorageRackQueryDto:BaseEntityDto
    {
        public tb_StorageRackQueryDto()
        {

        }

    
     

        private long? _Location_ID;
        /// <summary>
        /// 所属仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "所属仓库")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = true,ColumnDescription = "所属仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private string _RackNO;
        /// <summary>
        /// 货架编号
        /// </summary>
        [AdvQueryAttribute(ColName = "RackNO",ColDesc = "货架编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RackNO",Length=50,IsNullable = false,ColumnDescription = "货架编号" )]
        public string RackNO 
        { 
            get{return _RackNO;}
            set{SetProperty(ref _RackNO, value);}
        }
     

        private string _RackName;
        /// <summary>
        /// 货架名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RackName",ColDesc = "货架名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RackName",Length=50,IsNullable = false,ColumnDescription = "货架名称" )]
        public string RackName 
        { 
            get{return _RackName;}
            set{SetProperty(ref _RackName, value);}
        }
     

        private string _RackLocation;
        /// <summary>
        /// 货架位置
        /// </summary>
        [AdvQueryAttribute(ColName = "RackLocation",ColDesc = "货架位置")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RackLocation",Length=100,IsNullable = true,ColumnDescription = "货架位置" )]
        public string RackLocation 
        { 
            get{return _RackLocation;}
            set{SetProperty(ref _RackLocation, value);}
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



