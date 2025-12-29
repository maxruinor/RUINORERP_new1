
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/22/2024 18:15:11
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using System.Drawing;
using RUINORERP.Global;
using RUINORERP.Global.Model;
using RUINORERP.Global.EnumExt;
namespace RUINORERP.Model
{
    /// <summary>
    /// tb_ProdDetail 扩展类，用于存放临时字段和辅助功能
    /// 后面按这个规律统一处理。如果实现中带有图片的字段。则需要额外处理
    /// 字段名：RowImage
    /// 后面如果支持多图则可能是List<key,value> key是图片名，value是图片对象
    /// </summary>
    public partial class tb_ProdDetail : BaseEntity, ICloneable
    {
        /// <summary>
        /// 属性组名称，用于多属性组合显示，此字段不存储到数据库
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]
        public string PropertyGroupName { get; set; }

        /// <summary>
        /// 产品信息显示文本
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "产品信息")]
        [Browsable(true)]
        public string DisplayText
        {
            get
            {
                // 特殊值处理
                if (ProdDetailID == -1)
                    return "请选择";

                // 使用HashSet自动去重
                var uniqueParts = new HashSet<string>();

                // 添加各属性值，自动忽略重复项
                if (!string.IsNullOrWhiteSpace(tb_prod?.CNName))
                    uniqueParts.Add(tb_prod.CNName);

                if (!string.IsNullOrWhiteSpace(SKU))
                    uniqueParts.Add(SKU);

                if (!string.IsNullOrWhiteSpace(tb_prod?.Model))
                    uniqueParts.Add(tb_prod.Model);

                if (!string.IsNullOrWhiteSpace(tb_prod?.Specifications))
                    uniqueParts.Add(tb_prod.Specifications);

                // 拼接去重后的结果
                return string.Join("-", uniqueParts);
            }
        }
    }
}

