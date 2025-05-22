
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:54
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
    /// 城市表
    /// </summary>
    [Serializable()]
    [Description("城市表")]
    [SugarTable("tb_Cities")]
    public partial class tb_Cities: BaseEntity, ICloneable
    {
        public tb_Cities()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("城市表tb_Cities" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _CityID;
        /// <summary>
        /// 城市
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CityID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "城市" , IsPrimaryKey = true)]
        public long CityID
        { 
            get{return _CityID;}
            set{
            SetProperty(ref _CityID, value);
                base.PrimaryKeyID = _CityID;
            }
        }

        private long? _ProvinceID;
        /// <summary>
        /// 省
        /// </summary>
        [AdvQueryAttribute(ColName = "ProvinceID",ColDesc = "省")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProvinceID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "省" )]
        [FKRelationAttribute("tb_Provinces","ProvinceID")]
        public long? ProvinceID
        { 
            get{return _ProvinceID;}
            set{
            SetProperty(ref _ProvinceID, value);
                        }
        }

        private string _CityCNName;
        /// <summary>
        /// 城市中文名
        /// </summary>
        [AdvQueryAttribute(ColName = "CityCNName",ColDesc = "城市中文名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CityCNName" ,Length=80,IsNullable = true,ColumnDescription = "城市中文名" )]
        public string CityCNName
        { 
            get{return _CityCNName;}
            set{
            SetProperty(ref _CityCNName, value);
                        }
        }

        private string _CityENName;
        /// <summary>
        /// 城市英文名
        /// </summary>
        [AdvQueryAttribute(ColName = "CityENName",ColDesc = "城市英文名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CityENName" ,Length=80,IsNullable = true,ColumnDescription = "城市英文名" )]
        public string CityENName
        { 
            get{return _CityENName;}
            set{
            SetProperty(ref _CityENName, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProvinceID))]
        public virtual tb_Provinces tb_provinces { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Customer.CityID))]
        public virtual List<tb_CRM_Customer> tb_CRM_Customers { get; set; }
        //tb_CRM_Customer.CityID)
        //CityID.FK_TB_CRM_C_REFERENCE_TB_CITIE)
        //tb_Cities.CityID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        public override object Clone()
        {
            tb_Cities loctype = (tb_Cities)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

