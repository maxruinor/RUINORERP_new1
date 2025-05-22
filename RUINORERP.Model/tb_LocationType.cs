
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:04
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
    /// 库位类别
    /// </summary>
    [Serializable()]
    [Description("库位类别")]
    [SugarTable("tb_LocationType")]
    public partial class tb_LocationType: BaseEntity, ICloneable
    {
        public tb_LocationType()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("库位类别tb_LocationType" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _LocationType_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LocationType_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long LocationType_ID
        { 
            get{return _LocationType_ID;}
            set{
            SetProperty(ref _LocationType_ID, value);
                base.PrimaryKeyID = _LocationType_ID;
            }
        }

        private string _TypeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeName" ,Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        public string TypeName
        { 
            get{return _TypeName;}
            set{
            SetProperty(ref _TypeName, value);
                        }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Location.LocationType_ID))]
        public virtual List<tb_Location> tb_Locations { get; set; }
        //tb_Location.LocationType_ID)
        //LocationType_ID.FK_TB_LOCAT_REFERENCE_TB_LOCAT)
        //tb_LocationType.LocationType_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        

        public override object Clone()
        {
            tb_LocationType loctype = (tb_LocationType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

