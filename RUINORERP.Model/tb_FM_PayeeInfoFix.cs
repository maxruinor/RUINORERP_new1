
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
    /// 这里是一个例子。
    /// 后面按这个规律统一处理。如果实现中带有图片的字段。则需要额外处理
    /// 字段名：RowImage
    /// 后面如果支持多图则可能是List<key,value> key是图片名，value是图片对象
    /// </summary>
    public partial class tb_FM_PayeeInfo : BaseEntity, ICloneable
    {
        // 组合显示的属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]//如果为false绑定时取不到值？
        public string DisplayText
        {
            get
            {
                // 处理"请选择"的特殊情况
                if (PayeeInfoID == -1)
                {
                    return "请选择";
                }

                if (!string.IsNullOrEmpty(this.Details))
                {
                    return Details;
                }
                // 处理可能为null的字段，使用空值合并运算符提供默认值
                string accountType = ((AccountType)Account_type).ToString();
                string accountName = Account_name ?? "未命名";
                string accountNo = Account_No ?? "无账号";
                string belongingBank = BelongingBank ?? "无银行信息";

                // 组合所有字段
                return $"{accountType}-{accountName}-{accountNo}-{belongingBank}";
            }
        }
    }
}

