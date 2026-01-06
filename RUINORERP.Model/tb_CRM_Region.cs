
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:44
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
    /// <summary>
    /// 销售分区表-大中华区
    /// </summary>
    [Serializable()]
    [Description("销售分区表")]
    [SugarTable("tb_CRM_Region")]
    public partial class tb_CRM_Region: BaseEntity, ICloneable
    {
        public tb_CRM_Region()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("销售分区表-大中华区tb_CRM_Region" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Region_ID;
        /// <summary>
        /// 地区
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Region_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "地区" , IsPrimaryKey = true)]
        public long Region_ID
        { 
            get{return _Region_ID;}
            set{
            SetProperty(ref _Region_ID, value);
                base.PrimaryKeyID = _Region_ID;
            }
        }

        private string _Region_Name;
        /// <summary>
        /// 地区名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_Name",ColDesc = "地区名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Region_Name" ,Length=50,IsNullable = true,ColumnDescription = "地区名称" )]
        public string Region_Name
        { 
            get{return _Region_Name;}
            set{
            SetProperty(ref _Region_Name, value);
                        }
        }

        private string _Region_code;
        /// <summary>
        /// 地区代码
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_code",ColDesc = "地区代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Region_code" ,Length=20,IsNullable = true,ColumnDescription = "地区代码" )]
        public string Region_code
        { 
            get{return _Region_code;}
            set{
            SetProperty(ref _Region_code, value);
                        }
        }

        private long? _Parent_region_id;
        /// <summary>
        ///  父地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_region_id",ColDesc = " 父地区")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Parent_region_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = " 父地区" )]
        public long? Parent_region_id
        { 
            get{return _Parent_region_id;}
            set{
            SetProperty(ref _Parent_region_id, value);
                        }
        }

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
                        }
        }

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Provinces.Region_ID))]
        public virtual List<tb_Provinces> tb_Provinceses { get; set; }
        //tb_Provinces.Region_ID)
        //Region_ID.FK_TB_PROVI_REFERENCE_TB_CRM_R)
        //tb_CRM_Region.Region_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Customer.Region_ID))]
        public virtual List<tb_CRM_Customer> tb_CRM_Customers { get; set; }
        //tb_CRM_Customer.Region_ID)
        //Region_ID.FK_CRM_Customer_REF_CRM_Region)
        //tb_CRM_Region.Region_ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_CRM_Region loctype = (tb_CRM_Region)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

