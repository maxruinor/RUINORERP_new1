using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// 字段名称识别器
    /// 智能识别和转换控件名称为可读的字段名称
    /// </summary>
    public static class FieldNameRecognizer
    {
        #region 字段

        /// <summary>
        /// 控件前缀映射
        /// Key: 控件前缀
        /// Value: 中文名称
        /// </summary>
        private static readonly Dictionary<string, string> ControlPrefixMap = new Dictionary<string, string>
        {
            // 文本框
            { "txt", "" },
            { "tbx", "" },

            // 下拉框
            { "cmb", "" },
            { "ddl", "" },
            { "cbo", "" },

            // 日期选择器
            { "dtp", "" },
            { "date", "" },

            // 标签
            { "lbl", "" },
            { "lab", "" },

            // 复选框
            { "chk", "" },
            { "cbx", "" },

            // 单选按钮
            { "rdg", "" },
            { "rdo", "" },
            { "rad", "" },

            // 数字输入框
            { "num", "" },
            { "nud", "" },

            // 按钮
            { "btn", "" },
            { "bt", "" },

            // 分组框
            { "grp", "" },
            { "gb", "" },

            // 面板
            { "pnl", "" },
            { "pan", "" },

            // 其他
            { "txt", "" },
            { "txt", "" }
        };

        /// <summary>
        /// 常见字段后缀映射
        /// Key: 英文后缀
        /// Value: 中文翻译
        /// </summary>
        private static readonly Dictionary<string, string> FieldSuffixMap = new Dictionary<string, string>
        {
            // 基本信息
            { "No", "编号" },
            { "ID", "编号" },
            { "Code", "编码" },
            { "Name", "名称" },
            { "Title", "标题" },
            { "Desc", "描述" },
            { "Remark", "备注" },
            { "Note", "说明" },
            { "Memo", "备注" },

            // 日期相关
            { "Date", "日期" },
            { "Time", "时间" },
            { "Year", "年份" },
            { "Month", "月份" },
            { "Day", "日期" },

            // 人员相关
            { "User", "用户" },
            { "Admin", "管理员" },
            { "Manager", "经理" },
            { "Operator", "操作员" },
            { "Creator", "创建人" },
            { "Modifier", "修改人" },

            // 状态相关
            { "Status", "状态" },
            { "State", "状态" },
            { "Flag", "标记" },
            { "Is", "" },
            { "Has", "" },

            // 数值相关
            { "Qty", "数量" },
            { "Amount", "金额" },
            { "Price", "价格" },
            { "Cost", "成本" },
            { "Total", "合计" },
            { "Sum", "总和" },
            { "Count", "计数" },
            { "Rate", "费率" },
            { "Tax", "税额" },
            { "Discount", "折扣" },

            // 业务相关
            { "Order", "订单" },
            { "Bill", "单据" },
            { "Invoice", "发票" },
            { "Receipt", "收据" },
            { "Payment", "付款" },
            { "Collection", "收款" },
            { "Delivery", "交货" },
            { "Ship", "发货" },
            { "Receive", "收货" },
            { "Purchase", "采购" },
            { "Sale", "销售" },

            // 客户供应商相关
            { "Customer", "客户" },
            { "Supplier", "供应商" },
            { "Vendor", "供应商" },
            { "Client", "客户" },

            // 产品相关
            { "Product", "产品" },
            { "Item", "物料" },
            { "Material", "材料" },
            { "Goods", "商品" },
            { "SKU", "SKU" },

            // 财务相关
            { "Account", "账户" },
            { "Currency", "币别" },
            { "Exchange", "汇率" },
            { "Balance", "余额" },

            // 地址相关
            { "Address", "地址" },
            { "City", "城市" },
            { "Province", "省份" },
            { "Country", "国家" },
            { "Zip", "邮编" },

            // 联系方式
            { "Phone", "电话" },
            { "Mobile", "手机" },
            { "Email", "邮箱" },
            { "Fax", "传真" },

            // 其他
            { "Type", "类型" },
            { "Kind", "种类" },
            { "Category", "类别" },
            { "Class", "分类" },
            { "Group", "分组" },
            { "Sort", "排序" },
            { "Level", "级别" },
            { "Priority", "优先级" }
        };

        /// <summary>
        /// 常见业务术语映射
        /// </summary>
        private static readonly Dictionary<string, string> BusinessTermMap = new Dictionary<string, string>
        {
            { "SaleOrder", "销售订单" },
            { "PurOrder", "采购订单" },
            { "SaleOut", "销售出库" },
            { "PurIn", "采购入库" },
            { "SaleReturn", "销售退货" },
            { "PurReturn", "采购退货" },
            { "Manufacturing", "生产" },
            { "Assembling", "组装" },
            { "Disassembling", "拆卸" },
            { "Stocktaking", "盘点" },
            { "Transfer", "调拨" },
            { "Payment", "付款" },
            { "Collection", "收款" },
            { "Invoice", "发票" },
            { "Receipt", "收据" },
            { "CustomerVendor", "客户/供应商" },
            { "PlatformOrderNo", "平台订单号" },
            { "CustomerPONo", "客户PO号" },
            { "DeliveryDate", "交货日期" },
            { "OrderDate", "订单日期" },
            { "PreDeliveryDate", "预交期" },
            { "TransportType", "运输方式" },
            { "DeliveryAddress", "送货地址" },
            { "PaymentType", "付款类型" },
            { "PaymentStatus", "付款状态" },
            { "ApprovalResult", "审批结果" },
            { "ApprovalOpinion", "审批意见" },
            { "BusinessPersonnel", "业务员" },
            { "ProjectTeam", "项目组" },
            { "ReceivableAccount", "收款账户" }
        };

        #endregion

        #region 公共方法

        /// <summary>
        /// 从控件名称识别字段名称
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>识别的字段名称</returns>
        public static string Recognize(string controlName)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                return "未知字段";
            }

            // 1. 移除控件前缀
            string fieldName = RemoveControlPrefix(controlName);

            // 2. 尝试匹配业务术语
            foreach (var term in BusinessTermMap)
            {
                if (fieldName.Equals(term.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return term.Value;
                }
            }

            // 3. 拆分并翻译各个部分
            string translated = TranslateFieldName(fieldName);

            return translated;
        }

        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>字段描述</returns>
        public static string GetFieldDescription(string controlName)
        {
            string fieldName = Recognize(controlName);

            // 根据字段类型生成描述
            if (controlName.Contains("No") || controlName.Contains("ID"))
            {
                return $"该字段为{fieldName},由系统自动生成,通常不可修改。";
            }
            else if (controlName.Contains("Date") || controlName.Contains("Time"))
            {
                return $"该字段用于选择{fieldName}。";
            }
            else if (controlName.Contains("Status") || controlName.Contains("State"))
            {
                return $"该字段表示记录的{fieldName}。";
            }
            else if (controlName.Contains("User") || controlName.Contains("Admin"))
            {
                return $"该字段用于指定{fieldName}。";
            }
            else
            {
                return $"该字段用于输入或选择{fieldName}。";
            }
        }

        /// <summary>
        /// 获取字段数据类型
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>字段数据类型</returns>
        public static string GetFieldType(string controlName)
        {
            if (controlName.Contains("No") || controlName.Contains("ID"))
            {
                return "字符串(编号)";
            }
            else if (controlName.Contains("Date") || controlName.Contains("Time"))
            {
                return "日期时间";
            }
            else if (controlName.Contains("Qty") || controlName.Contains("Count"))
            {
                return "整数";
            }
            else if (controlName.Contains("Amount") || controlName.Contains("Price") ||
                     controlName.Contains("Cost") || controlName.Contains("Tax") ||
                     controlName.Contains("Rate"))
            {
                return "小数";
            }
            else if (controlName.Contains("Status") || controlName.Contains("State") ||
                     controlName.Contains("Is") || controlName.Contains("Has"))
            {
                return "布尔值/枚举";
            }
            else
            {
                return "字符串";
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 移除控件前缀
        /// </summary>
        private static string RemoveControlPrefix(string controlName)
        {
            foreach (var prefix in ControlPrefixMap.Keys)
            {
                if (controlName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return controlName.Substring(prefix.Length);
                }
            }

            return controlName;
        }

        /// <summary>
        /// 翻译字段名称
        /// </summary>
        private static string TranslateFieldName(string fieldName)
        {
            // 使用正则表达式分割驼峰命名
            var parts = Regex.Split(fieldName, @"(?<!^)(?=[A-Z])");

            var translatedParts = new List<string>();

            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;

                // 尝试匹配后缀映射
                bool matched = false;
                foreach (var suffix in FieldSuffixMap)
                {
                    if (part.Equals(suffix.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        translatedParts.Add(suffix.Value);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    // 如果没有匹配,尝试智能翻译
                    translatedParts.Add(IntelligentTranslate(part));
                }
            }

            return string.Join("", translatedParts);
        }

        /// <summary>
        /// 智能翻译单词
        /// </summary>
        private static string IntelligentTranslate(string word)
        {
            // 这里可以添加更复杂的翻译逻辑
            // 简单实现: 如果单词全大写,可能是缩写,直接返回
            if (word.Equals(word.ToUpper()))
            {
                return word;
            }

            // 否则返回原词(可以后续接入翻译API)
            return word;
        }

        #endregion
    }
}
