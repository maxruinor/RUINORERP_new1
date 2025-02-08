
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:51
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
    /// 考勤表
    /// </summary>
    [Serializable()]
    [Description("考勤表")]
    [SugarTable("tb_Attendance")]
    public partial class tb_Attendance: BaseEntity, ICloneable
    {
        public tb_Attendance()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("考勤表tb_Attendance" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private string _badgenumber;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "badgenumber",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "badgenumber" ,Length=30,IsNullable = true,ColumnDescription = "" )]
        public string badgenumber
        { 
            get{return _badgenumber;}
            set{
            SetProperty(ref _badgenumber, value);
                        }
        }

        private string _username;
        /// <summary>
        /// 姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "username",ColDesc = "姓名")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "username" ,Length=50,IsNullable = true,ColumnDescription = "姓名" )]
        public string username
        { 
            get{return _username;}
            set{
            SetProperty(ref _username, value);
                        }
        }

        private string _deptname;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "deptname",ColDesc = "部门")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "deptname" ,Length=60,IsNullable = true,ColumnDescription = "部门" )]
        public string deptname
        { 
            get{return _deptname;}
            set{
            SetProperty(ref _deptname, value);
                        }
        }

        private string _sDate;
        /// <summary>
        /// 开始时间
        /// </summary>
        [AdvQueryAttribute(ColName = "sDate",ColDesc = "开始时间")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "sDate" ,Length=100,IsNullable = true,ColumnDescription = "开始时间" )]
        public string sDate
        { 
            get{return _sDate;}
            set{
            SetProperty(ref _sDate, value);
                        }
        }

        private string _stime;
        /// <summary>
        /// 时间组
        /// </summary>
        [AdvQueryAttribute(ColName = "stime",ColDesc = "时间组")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "stime" ,Length=255,IsNullable = true,ColumnDescription = "时间组" )]
        public string stime
        { 
            get{return _stime;}
            set{
            SetProperty(ref _stime, value);
                        }
        }

        private DateTime? _eDate;
        /// <summary>
        /// 结束时间
        /// </summary>
        [AdvQueryAttribute(ColName = "eDate",ColDesc = "结束时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "eDate" ,IsNullable = true,ColumnDescription = "结束时间" )]
        public DateTime? eDate
        { 
            get{return _eDate;}
            set{
            SetProperty(ref _eDate, value);
                        }
        }

        private DateTime? _t1;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t1",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "t1" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? t1
        { 
            get{return _t1;}
            set{
            SetProperty(ref _t1, value);
                        }
        }

        private DateTime? _t2;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t2",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "t2" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? t2
        { 
            get{return _t2;}
            set{
            SetProperty(ref _t2, value);
                        }
        }

        private DateTime? _t3;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t3",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "t3" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? t3
        { 
            get{return _t3;}
            set{
            SetProperty(ref _t3, value);
                        }
        }

        private DateTime? _t4;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t4",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "t4" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? t4
        { 
            get{return _t4;}
            set{
            SetProperty(ref _t4, value);
                        }
        }

        #endregion

        #region 扩展属性


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Attendance);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_Attendance loctype = (tb_Attendance)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

