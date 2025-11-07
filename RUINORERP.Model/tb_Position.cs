
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:02
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
    /// 位置信息
    /// </summary>
    [Serializable()]
    [Description("位置信息")]
    [SugarTable("tb_Position")]
    public partial class tb_Position: BaseEntity, ICloneable
    {
        public tb_Position()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("位置信息tb_Position" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Position_Id;
        /// <summary>
        /// 位置信息
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Position_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "位置信息" , IsPrimaryKey = true)]
        public long Position_Id
        { 
            get{return _Position_Id;}
            set{
            SetProperty(ref _Position_Id, value);
                base.PrimaryKeyID = _Position_Id;
            }
        }

        private string _Left;
        /// <summary>
        /// 左边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Left",ColDesc = "左边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Left" ,Length=50,IsNullable = false,ColumnDescription = "左边距" )]
        public string Left
        { 
            get{return _Left;}
            set{
            SetProperty(ref _Left, value);
                        }
        }

        private string _Right;
        /// <summary>
        /// 右边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Right",ColDesc = "右边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Right" ,Length=50,IsNullable = true,ColumnDescription = "右边距" )]
        public string Right
        { 
            get{return _Right;}
            set{
            SetProperty(ref _Right, value);
                        }
        }

        private string _Bottom;
        /// <summary>
        /// 下边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Bottom",ColDesc = "下边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Bottom" ,Length=50,IsNullable = true,ColumnDescription = "下边距" )]
        public string Bottom
        { 
            get{return _Bottom;}
            set{
            SetProperty(ref _Bottom, value);
                        }
        }

        private string _Top;
        /// <summary>
        /// 上边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Top",ColDesc = "上边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Top" ,Length=50,IsNullable = true,ColumnDescription = "上边距" )]
        public string Top
        { 
            get{return _Top;}
            set{
            SetProperty(ref _Top, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProcessStep.Position_Id))]
        public virtual List<tb_ProcessStep> tb_ProcessSteps { get; set; }
        //tb_ProcessStep.Position_Id)
        //Position_Id.FK_TB_PROCE_REFERENCE_TB_POSIT)
        //tb_Position.Position_Id)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Position loctype = (tb_Position)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

