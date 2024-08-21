
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:01
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
    /// 
    /// </summary>
    [Serializable()]
    [Description("Logs")]
    [SugarTable("Logs")]
    public partial class Logs: BaseEntity, ICloneable
    {
        public Logs()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Logs" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            base.PrimaryKeyID = _ID;
            SetProperty(ref _ID, value);
            }
        }

        private DateTime? _Date;
        /// <summary>
        /// 时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Date",ColDesc = "时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Date" ,IsNullable = true,ColumnDescription = "时间" )]
        public DateTime? Date
        { 
            get{return _Date;}
            set{
            SetProperty(ref _Date, value);
            }
        }

        private string _Level;
        /// <summary>
        /// 级别
        /// </summary>
        [AdvQueryAttribute(ColName = "Level",ColDesc = "级别")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Level" ,Length=10,IsNullable = true,ColumnDescription = "级别" )]
        public string Level
        { 
            get{return _Level;}
            set{
            SetProperty(ref _Level, value);
            }
        }

        private string _Logger;
        /// <summary>
        /// 记录器
        /// </summary>
        [AdvQueryAttribute(ColName = "Logger",ColDesc = "记录器")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Logger" ,Length=50,IsNullable = true,ColumnDescription = "记录器" )]
        public string Logger
        { 
            get{return _Logger;}
            set{
            SetProperty(ref _Logger, value);
            }
        }

        private string _Message;
        /// <summary>
        /// 消息
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "消息")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Message" ,Length=3000,IsNullable = true,ColumnDescription = "消息" )]
        public string Message
        { 
            get{return _Message;}
            set{
            SetProperty(ref _Message, value);
            }
        }

        private string _Exception;
        /// <summary>
        /// 异常
        /// </summary>
        [AdvQueryAttribute(ColName = "Exception",ColDesc = "异常")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "Exception" ,Length=2147483647,IsNullable = true,ColumnDescription = "异常" )]
        public string Exception
        { 
            get{return _Exception;}
            set{
            SetProperty(ref _Exception, value);
            }
        }

        private string _Operator;
        /// <summary>
        /// 操作者
        /// </summary>
        [AdvQueryAttribute(ColName = "Operator",ColDesc = "操作者")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Operator" ,Length=50,IsNullable = true,ColumnDescription = "操作者" )]
        public string Operator
        { 
            get{return _Operator;}
            set{
            SetProperty(ref _Operator, value);
            }
        }

        private string _ModName;
        /// <summary>
        /// 模块名
        /// </summary>
        [AdvQueryAttribute(ColName = "ModName",ColDesc = "模块名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ModName" ,Length=50,IsNullable = true,ColumnDescription = "模块名" )]
        public string ModName
        { 
            get{return _ModName;}
            set{
            SetProperty(ref _ModName, value);
            }
        }

        private string _Path;
        /// <summary>
        /// 路径
        /// </summary>
        [AdvQueryAttribute(ColName = "Path",ColDesc = "路径")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Path" ,Length=100,IsNullable = true,ColumnDescription = "路径" )]
        public string Path
        { 
            get{return _Path;}
            set{
            SetProperty(ref _Path, value);
            }
        }

        private string _ActionName;
        /// <summary>
        /// 动作
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionName",ColDesc = "动作")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ActionName" ,Length=50,IsNullable = true,ColumnDescription = "动作" )]
        public string ActionName
        { 
            get{return _ActionName;}
            set{
            SetProperty(ref _ActionName, value);
            }
        }

        private string _IP;
        /// <summary>
        /// 网络地址
        /// </summary>
        [AdvQueryAttribute(ColName = "IP",ColDesc = "网络地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "IP" ,Length=20,IsNullable = true,ColumnDescription = "网络地址" )]
        public string IP
        { 
            get{return _IP;}
            set{
            SetProperty(ref _IP, value);
            }
        }

        private string _MAC;
        /// <summary>
        /// 物理地址
        /// </summary>
        [AdvQueryAttribute(ColName = "MAC",ColDesc = "物理地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MAC" ,Length=30,IsNullable = true,ColumnDescription = "物理地址" )]
        public string MAC
        { 
            get{return _MAC;}
            set{
            SetProperty(ref _MAC, value);
            }
        }

        private string _MachineName;
        /// <summary>
        /// 电脑名
        /// </summary>
        [AdvQueryAttribute(ColName = "MachineName",ColDesc = "电脑名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MachineName" ,Length=50,IsNullable = true,ColumnDescription = "电脑名" )]
        public string MachineName
        { 
            get{return _MachineName;}
            set{
            SetProperty(ref _MachineName, value);
            }
        }

        private long? _User_ID;
        /// <summary>
        /// 用户
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "用户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "User_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "用户" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long? User_ID
        { 
            get{return _User_ID;}
            set{
            SetProperty(ref _User_ID, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(User_ID))]
        public virtual tb_UserInfo tb_userinfo { get; set; }



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
                    Type type = typeof(Logs);
                    
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
            Logs loctype = (Logs)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

