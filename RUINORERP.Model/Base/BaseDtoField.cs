using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base
{


    /// <summary>
    /// 查询实体参数的字段域
    /// </summary>
    [Serializable]
    public class BaseDtoField
    {
        string colName;

      
        string caption;
        string description;
  
        string fKTableName;
        string fKPrimarykey;
        public SugarColumn SugarCol { get; set; } = new SugarColumn();


        Type colDataType;
        /// <summary>
        /// 列的数据类型
        /// </summary>
        public Type ColDataType { get => colDataType; set => colDataType = value; }


        bool useLike;

        /// <summary>
        /// 是否使用包含等模糊查询
        /// </summary>
        public bool UseLike { get => useLike; set => useLike = value; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get => caption; set => caption = value; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// 列名
        /// </summary>
        public string FieldName { get => colName; set => colName = value; }


        #region 外键相关

        /// <summary>
        ///  外键关联属性,（生成的，依赖于DB的关联） 直接保存特性更简单？
        /// </summary>
        public FKRelationAttribute fKRelationAttribute { get; set; }

        bool isFKRelationAttribute;
        /// <summary>
        /// 是否为外键属性
        /// </summary>
        public bool IsFKRelationAttribute { get => isFKRelationAttribute; set => isFKRelationAttribute = value; }

        /// <summary>
        /// 外键相关的表名
        /// </summary>
        public string FKTableName { get => fKTableName; set => fKTableName = value; }


        /// <summary>
        /// 外键的key
        /// </summary>
      //  public string FKPrimarykey { get => fKPrimarykey; set => fKPrimarykey = value; }


        #endregion

        List<AdvExtQueryAttribute> extendedAttribute = new List<AdvExtQueryAttribute>();

        /// <summary>
        /// 对应列的扩展特性 用于模糊搜索时动态添加的数据
        /// </summary>
        public List<AdvExtQueryAttribute> ExtendedAttribute { get => extendedAttribute; set => extendedAttribute = value; }
    }
}
