
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:34
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
    /// 会计科目表，财务系统中使用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_Subject")]
    public partial class tb_FM_SubjectQueryDto:BaseEntityDto
    {
        public tb_FM_SubjectQueryDto()
        {

        }

    
     

        private long? _parent_subject_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "parent_subject_id",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "parent_subject_id",IsNullable = true,ColumnDescription = "" )]
        public long? parent_subject_id 
        { 
            get{return _parent_subject_id;}
            set{SetProperty(ref _parent_subject_id, value);}
        }
     

        private string _subject_code;
        /// <summary>
        /// 科目代码
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_code",ColDesc = "科目代码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "subject_code",Length=50,IsNullable = false,ColumnDescription = "科目代码" )]
        public string subject_code 
        { 
            get{return _subject_code;}
            set{SetProperty(ref _subject_code, value);}
        }
     

        private string _subject_name;
        /// <summary>
        /// 科目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_name",ColDesc = "科目名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "subject_name",Length=100,IsNullable = false,ColumnDescription = "科目名称" )]
        public string subject_name 
        { 
            get{return _subject_name;}
            set{SetProperty(ref _subject_name, value);}
        }
     

        private string _subject_en_name;
        /// <summary>
        /// 科目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_en_name",ColDesc = "科目名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "subject_en_name",Length=100,IsNullable = true,ColumnDescription = "科目名称" )]
        public string subject_en_name 
        { 
            get{return _subject_en_name;}
            set{SetProperty(ref _subject_en_name, value);}
        }
     

        private int _Subject_Type;
        /// <summary>
        /// 科目类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_Type",ColDesc = "科目类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Subject_Type",IsNullable = false,ColumnDescription = "科目类型" )]
        public int Subject_Type 
        { 
            get{return _Subject_Type;}
            set{SetProperty(ref _Subject_Type, value);}
        }
     

        private bool _Balance_direction;
        /// <summary>
        /// 余额方向
        /// </summary>
        [AdvQueryAttribute(ColName = "Balance_direction",ColDesc = "余额方向")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Balance_direction",IsNullable = false,ColumnDescription = "余额方向" )]
        public bool Balance_direction 
        { 
            get{return _Balance_direction;}
            set{SetProperty(ref _Balance_direction, value);}
        }
     

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private DateTime? _EndDate;
        /// <summary>
        /// 离职日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "离职日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "EndDate",IsNullable = true,ColumnDescription = "离职日期" )]
        public DateTime? EndDate 
        { 
            get{return _EndDate;}
            set{SetProperty(ref _EndDate, value);}
        }
     

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private byte[] _Images;
        /// <summary>
        /// 类目图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "类目图片")]
        [SugarColumn(ColumnDataType = "image",SqlParameterDbType ="Binary",ColumnName = "Images",Length=2147483647,IsNullable = true,ColumnDescription = "类目图片" )]
        public byte[] Images 
        { 
            get{return _Images;}
            set{SetProperty(ref _Images, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



