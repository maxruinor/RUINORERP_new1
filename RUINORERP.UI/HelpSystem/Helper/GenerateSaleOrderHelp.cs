using System;
using System.IO;
using RUINORERP.Model;
using RUINORERP.UI.HelpSystem.Helper;
using RUINORERP.UI.HelpSystem.Components;

namespace RUINORERP.UI.HelpSystem.Helper
{
    /// <summary>
    /// 专门用于生成销售订单帮助内容的工具
    /// </summary>
    public class GenerateSaleOrderHelp
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("开始生成销售订单完整帮助内容...");
            
            // 销售订单帮助内容根目录
            string basePath = @"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\HelpContent";
            
            // 生成销售订单实体所有字段的帮助内容
            GenerateAllSaleOrderFieldHelp(basePath);
            
            // 生成销售订单窗体帮助内容
            GenerateSaleOrderFormHelp(basePath);
            
            // 生成销售管理模块帮助内容
            GenerateSalesModuleHelp(basePath);
            
            Console.WriteLine("销售订单帮助内容生成完成！");
            Console.WriteLine("所有帮助内容已保存到: " + basePath);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// 生成销售订单所有字段的帮助内容
        /// </summary>
        private static void GenerateAllSaleOrderFieldHelp(string basePath)
        {
            var entityType = typeof(tb_SaleOrder);
            var properties = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
                
            int count = 0;
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                
                // 检查是否已经有对应的帮助文件
                var fieldHelpPath = Path.Combine(basePath, "Fields", entityType.Name, $"{propertyName}.md");
                
                // 如果文件不存在，则生成它
                if (!File.Exists(fieldHelpPath))
                {
                    var columnAttr = property.GetCustomAttribute<SugarColumnAttribute>();
                    var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
                    var advQueryAttr = property.GetCustomAttribute<AdvQueryAttribute>();
                    
                    var fieldName = FieldNameRecognizer.Recognize(propertyName);
                    var fieldDescription = descAttr?.Description ?? advQueryAttr?.ColDesc ?? fieldName;
                    
                    var content = GenerateSingleFieldHelp(propertyName, fieldName, fieldDescription, columnAttr);
                    
                    // 确保目录存在
                    var directory = Path.GetDirectoryName(fieldHelpPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    
                    File.WriteAllText(fieldHelpPath, content);
                    Console.WriteLine($"已生成字段帮助: {propertyName}");
                    count++;
                }
                else
                {
                    Console.WriteLine($"跳过已存在的字段帮助: {propertyName}");
                }
            }
            
            Console.WriteLine($"共生成了 {count} 个销售订单字段帮助文件");
        }
        
        /// <summary>
        /// 生成单个字段的帮助内容
        /// </summary>
        private static string GenerateSingleFieldHelp(string propertyName, string fieldName, string fieldDescription, SugarColumnAttribute columnAttr)
        {
            var content = $@"# {fieldName}

## 字段说明
{fieldDescription}

## 基本信息
- **字段名称**: {fieldDescription}  ({propertyName})
- **字段类型**: {(columnAttr != null ? GetColumnType(columnAttr) : "未知")}
- **是否必填**: {(columnAttr != null ? (columnAttr.IsNullable ? "否" : "是") : "未知")}
- **最大长度**: {(columnAttr != null ? GetColumnLength(columnAttr) : "未知")}
{(columnAttr != null && !string.IsNullOrEmpty(columnAttr.ColumnDescription) ? "- **列描述**: " + columnAttr.ColumnDescription : "")}

## 使用说明
该字段用于输入或选择{fieldName}。{(columnAttr != null && !columnAttr.IsNullable ? "此字段为必填字段，不能为空。" : "")}

## 业务含义
{(GetBusinessMeaning(propertyName, fieldDescription))}

## 输入要求
{(GetInputRequirements(propertyName, columnAttr))}

## 注意事项
- {(columnAttr != null && !columnAttr.IsNullable ? "请确保输入有效的" + fieldName + "信息" : "")}
- 输入内容应符合业务规范
- 数据格式需满足系统要求

## 相关帮助
- 如需更多信息，请联系系统管理员
- 该字段的具体业务含义请参考相关业务文档

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";

            return content;
        }
        
        /// <summary>
        /// 获取列类型
        /// </summary>
        private static string GetColumnType(SugarColumnAttribute attr)
        {
            if (attr == null) return "未知";
            
            var dataType = attr.ColumnDataType.ToLower();
            if (dataType.Contains("int"))
            {
                return "整数";
            }
            else if (dataType.Contains("decimal") || dataType.Contains("money") || dataType.Contains("float") || dataType.Contains("double"))
            {
                return "小数";
            }
            else if (dataType.Contains("datetime") || dataType.Contains("date") || dataType.Contains("time"))
            {
                return "日期时间";
            }
            else if (dataType.Contains("bit") || dataType.Contains("bool"))
            {
                return "布尔值";
            }
            else if (dataType.Contains("varchar") || dataType.Contains("nvarchar") || dataType.Contains("char"))
            {
                return "字符串";
            }
            else
            {
                return attr.ColumnDataType;
            }
        }

        /// <summary>
        /// 获取列长度
        /// </summary>
        private static string GetColumnLength(SugarColumnAttribute attr)
        {
            if (attr == null) return "未知";
            
            if (attr.ColumnDataType.Contains("varchar") || attr.ColumnDataType.Contains("nvarchar") || 
                attr.ColumnDataType.Contains("char"))
            {
                return attr.Length.ToString();
            }
            else if (attr.ColumnDataType.Contains("decimal") || attr.ColumnDataType.Contains("numeric"))
            {
                return $"({attr.DecimalDigits})";
            }
            else
            {
                return "N/A";
            }
        }
        
        /// <summary>
        /// 获取字段业务含义
        /// </summary>
        private static string GetBusinessMeaning(string propertyName, string fieldDescription)
        {
            switch (propertyName)
            {
                case "SOrder_ID":
                    return "销售订单的唯一标识符，由系统自动生成，用于区分不同的销售订单记录。";
                case "SOrderNo":
                    return "销售订单的业务编号，通常按照一定的编号规则生成，用于业务查询和跟踪。";
                case "PayStatus":
                    return "销售订单的付款状态，表示该订单的付款完成情况。";
                case "Paytype_ID":
                    return "关联的付款类型，指明该订单采用的付款方式。";
                case "VoucherImage":
                    return "付款凭证图片，用于上传和存储订单的付款凭证。";
                case "CustomerVendor_ID":
                    return "关联的客户信息，标识该订单属于哪个客户。";
                case "Account_id":
                    return "收款账户，指定该订单的款项应收取到哪个账户。";
                case "Currency_ID":
                    return "交易币别，标识该订单使用的货币类型。";
                case "ExchangeRate":
                    return "汇率，用于不同币别之间的金额转换计算。";
                case "Employee_ID":
                    return "业务员，标识负责该销售订单的员工。";
                case "ProjectGroup_ID":
                    return "项目组，将订单归类到特定的项目组进行管理。";
                case "ForeignFreightIncome":
                    return "运费收入外币，以外币计价的运费收入金额。";
                case "FreightIncome":
                    return "运费收入，以本币计价的运费收入金额。";
                case "TotalQty":
                    return "总数量，该订单中所有商品的总数量。";
                case "TotalCost":
                    return "总成本，该订单中所有商品的总成本金额。";
                case "TotalAmount":
                    return "总金额，该订单的总交易金额。";
                case "TotalCommissionAmount":
                    return "佣金金额，该订单产生的佣金总额。";
                case "TotalTaxAmount":
                    return "总税额，该订单的税费总额。";
                case "PreDeliveryDate":
                    return "预交日期，预计向客户交付货物的日期。";
                case "SaleDate":
                    return "订单日期，销售订单的实际创建日期。";
                case "ShippingAddress":
                    return "收货地址，指定客户接收货物的具体地址。";
                case "ShippingWay":
                    return "发货方式，指定向客户发货的物流方式。";
                case "ForeignTotalAmount":
                    return "金额外币，以外币计价的订单总金额。";
                case "CustomerPONo":
                    return "客户订单号，客户提供的原始订单编号。";
                case "ForeignDeposit":
                    return "订金外币，以外币计价的订单订金金额。";
                case "Deposit":
                    return "订金，订单的订金金额。";
                case "DeliveryDateConfirm":
                    return "交期确认，标识是否已确认交货日期。";
                case "TotalUntaxedAmount":
                    return "未税本位币，不含税的本币金额。";
                case "Created_at":
                    return "创建时间，记录该订单的创建时间戳。";
                case "Created_by":
                    return "创建人，记录该订单的创建人ID。";
                case "Modified_at":
                    return "修改时间，记录该订单的最后修改时间。";
                case "Modified_by":
                    return "修改人，记录该订单的最后修改人ID。";
                case "CloseCaseOpinions":
                    return "结案意见，在订单结案时填写的相关意见。";
                case "Notes":
                    return "备注，用于记录与该订单相关的补充信息。";
                case "IsCustomizedOrder":
                    return "定制单，标识该订单是否为定制订单。";
                case "isdeleted":
                    return "逻辑删除标记，用于软删除机制，标识该记录是否已被删除。";
                case "ApprovalOpinions":
                    return "审批意见，记录对该订单的审批意见。";
                case "Approver_by":
                    return "审批人，记录对该订单进行审批的人员ID。";
                case "Approver_at":
                    return "审批时间，记录对该订单的审批时间。";
                case "ApprovalStatus":
                    return "审批状态，表示该订单的审批进展情况。";
                case "ApprovalResults":
                    return "审批结果，标识该订单审批是否通过。";
                case "DataStatus":
                    return "数据状态，表示该订单的数据状态（如草稿、已审核、已完成等）。";
                case "KeepAccountsType":
                    return "立帐类型，标识该订单的立帐方式。";
                case "TaxDeductionType":
                    return "扣税类型，指定该订单适用的扣税方式。";
                case "OrderPriority":
                    return "紧急程度，标识该订单的紧急优先级。";
                case "PlatformOrderNo":
                    return "平台单号，电商平台或其他平台提供的订单编号。";
                case "PrintStatus":
                    return "打印状态，标识该订单的打印情况。";
                case "IsFromPlatform":
                    return "平台单，标识该订单是否来自电商平台。";
                case "RefBillID":
                    return "引用单据ID，标识该订单引用的上游单据ID。";
                case "RefNO":
                    return "引用单号，标识该订单引用的上游单据编号。";
                case "RefBizType":
                    return "引用单据类型，标识该订单引用的上游单据业务类型。";
                default:
                    return $"该字段的业务含义为：{fieldDescription}";
            }
        }
        
        /// <summary>
        /// 获取输入要求
        /// </summary>
        private static string GetInputRequirements(string propertyName, SugarColumnAttribute columnAttr)
        {
            if (columnAttr != null)
            {
                if (columnAttr.ColumnDataType.ToLower().Contains("int") ||
                    columnAttr.ColumnDataType.ToLower().Contains("decimal") ||
                    columnAttr.ColumnDataType.ToLower().Contains("money") ||
                    columnAttr.ColumnDataType.ToLower().Contains("float") ||
                    columnAttr.ColumnDataType.ToLower().Contains("double"))
                {
                    return "请输入有效的数值。";
                }
                else if (columnAttr.ColumnDataType.ToLower().Contains("datetime") ||
                         columnAttr.ColumnDataType.ToLower().Contains("date"))
                {
                    return "请选择有效的日期时间。";
                }
                else if (columnAttr.ColumnDataType.ToLower().Contains("bit") ||
                         columnAttr.ColumnDataType.ToLower().Contains("bool"))
                {
                    return "请选择是或否。";
                }
                else
                {
                    return $"请输入有效的文本，最大长度{columnAttr.Length}个字符。";
                }
            }
            return "请输入有效的数据。";
        }
        
        /// <summary>
        /// 生成销售订单窗体帮助内容
        /// </summary>
        private static void GenerateSaleOrderFormHelp(string basePath)
        {
            var formPath = Path.Combine(basePath, "Forms", "UCSaleOrder.md");
            
            // 确保目录存在
            var directory = Path.GetDirectoryName(formPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            if (!File.Exists(formPath))
            {
                var content = $@"# 销售订单录入

## 窗体概述
本窗体用于销售订单的新建、编辑、查询和管理操作。

## 窗体信息
- **窗体名称**: UCSaleOrder
- **关联实体**: tb_SaleOrder

## 基本操作流程

### 新建记录
1. 点击工具栏【新建】按钮或按 【Ctrl+N】
2. 填写必填字段（订单编号、客户、业务员等）
3. 根据需要填写可选字段
4. 在明细区域添加订单商品
5. 点击【保存】按钮或按 【Ctrl+S】

### 编辑记录
1. 在查询界面找到要编辑的销售订单
2. 双击打开订单详情
3. 修改相关信息
4. 保存记录

### 删除记录
1. 查询要删除的销售订单
2. 点击【删除】按钮
3. 确认删除操作

## 主要字段说明
- **订单编号**: 由系统自动生成的唯一标识符
- **客户**: 选择该订单对应的客户
- **业务员**: 指定负责该订单的销售人员
- **订单日期**: 销售订单的创建日期
- **预交日期**: 预计向客户交付货物的日期
- **币别/汇率**: 指定交易使用的货币及汇率
- **收货地址**: 客户的收货地址信息
- **发货方式**: 指定货物的配送方式

## 子表信息
销售订单包含明细信息，可在下方的明细表格中进行增删改查操作。

## 快捷键
| 快捷键 | 功能说明 |
|--------|----------|
| F1 | 显示帮助 |
| Ctrl+S | 保存 |
| Ctrl+N | 新建 |
| Ctrl+F | 查询 |
| Ctrl+E | 编辑 |
| Del | 删除 |
| ESC | 关闭/取消 |

## 业务流程
1. 创建销售订单
2. 审核订单信息
3. 根据订单生成出库单
4. 完成发货流程
5. 处理收款事宜

## 注意事项
- 订单编号由系统自动生成，无需手动输入
- 客户信息必须选择有效客户
- 订单金额需与明细金额一致
- 发货前请确认收货地址准确无误

## 常见问题

### Q: 如何添加订单明细?
A: 在窗体下方的明细表格中点击新增按钮，然后输入商品信息。

### Q: 订单能否修改已审核的数据?
A: 已审核的订单需要先反审核才能修改。

### Q: 如何查看关联的出库单?
A: 在相关查询按钮中可以查看由此订单生成的出库单。

## 相关信息
- 联系系统管理员获取更多帮助
- 参考销售管理模块获取整体业务流程

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";
                
                File.WriteAllText(formPath, content);
                Console.WriteLine("已生成窗体帮助: UCSaleOrder.md");
            }
            else
            {
                Console.WriteLine("跳过已存在的窗体帮助: UCSaleOrder.md");
            }
        }
        
        /// <summary>
        /// 生成销售管理模块帮助内容
        /// </summary>
        private static void GenerateSalesModuleHelp(string basePath)
        {
            var modulePath = Path.Combine(basePath, "Modules", "SalesManagement.md");
            
            // 确保目录存在
            var directory = Path.GetDirectoryName(modulePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            if (!File.Exists(modulePath))
            {
                var content = $@"# 销售管理

## 模块概述
销售管理模块是RUINORERP系统的重要组成部分，用于管理企业的销售业务流程。该模块涵盖了从销售订单创建到出库、收款的完整销售链条。

## 主要功能
- **销售订单管理**: 创建、编辑、审核销售订单
- **销售出库管理**: 根据销售订单生成出库单
- **销售退货管理**: 处理销售退货业务
- **客户管理**: 维护客户信息和信用额度
- **报价管理**: 管理销售报价单据
- **合同管理**: 管理销售合同信息

## 业务流程
1. **订单创建**: 业务员创建销售订单
2. **订单审核**: 相关人员审核订单信息
3. **库存检查**: 检查库存是否满足订单需求
4. **出库处理**: 根据订单生成出库单并发货
5. **收款管理**: 跟踪订单收款情况
6. **售后服务**: 处理售后相关事务

## 关键单据
- **销售订单**: 销售业务的核心单据
- **销售出库单**: 记录货物出库信息
- **销售退货单**: 处理客户退货
- **销售发票**: 开具销售发票
- **收款单**: 记录收款信息

## 重要字段说明
- **客户**: 交易的对方单位
- **业务员**: 负责该笔业务的销售人员
- **订单金额**: 订单的总交易金额
- **交货日期**: 预计或实际交货时间
- **付款方式**: 客户选择的付款方式
- **收款状态**: 订单的收款完成情况

## 权限控制
- 不同角色有不同的操作权限
- 业务员只能查看自己的订单
- 管理员可以查看所有订单
- 审核权限需要特殊授权

## 报表分析
- 销售业绩报表
- 客户销售排行
- 产品销售排行
- 应收账款分析
- 销售趋势分析

## 注意事项
- 销售订单审核后一般不允许直接修改
- 发货前请确认客户信用状况
- 及时跟进应收账款回收
- 定期更新客户信息

## 常见问题
### Q: 如何修改已审核的销售订单?
A: 已审核的销售订单需要先反审核，然后才能进行修改。

### Q: 销售订单如何生成出库单?
A: 在销售订单界面点击生成出库单按钮，系统会根据订单信息自动生成出库单。

### Q: 如何查看客户的信用额度?
A: 在客户信息界面可以查看客户的信用额度和已用额度。

## 相关帮助
- 销售订单录入界面帮助
- 客户信息维护帮助
- 销售报表查询帮助

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";
                
                File.WriteAllText(modulePath, content);
                Console.WriteLine("已生成模块帮助: SalesManagement.md");
            }
            else
            {
                Console.WriteLine("跳过已存在的模块帮助: SalesManagement.md");
            }
        }
    }
}