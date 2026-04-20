using AutoMapper;
using FastReport.DevComponents.DotNetBar.Controls;
using FluentValidation.Results;
using HLH.Lib.Security;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.POIFS.Properties;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math.Field;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.DataCorrectionServices;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;
using RUINORERP.Repository.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.Report;
using RUINORERP.UI.SS;
using RUINORERP.UI.UserCenter.DataParts;
using RUINORERP.UI.WorkFlowTester;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("数据校正中心", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataCorrectionCenter : UserControl
    {
        /// <summary>
        /// 数据修复项元数据
        /// </summary>
        public class CorrectionItemMetadata
        {
            /// <summary>
            /// 功能名称
            /// </summary>
            public string FunctionName { get; set; }
            
            /// <summary>
            /// 问题描述
            /// </summary>
            public string ProblemDescription { get; set; }
            
            /// <summary>
            /// 影响表清单
            /// </summary>
            public List<string> AffectedTables { get; set; } = new List<string>();
            
            /// <summary>
            /// 修复逻辑说明
            /// </summary>
            public string FixLogic { get; set; }
            
            /// <summary>
            /// 发生情形
            /// </summary>
            public string OccurrenceScenario { get; set; }
            
            /// <summary>
            /// TreeView节点文本（用于匹配）
            /// </summary>
            public string NodeText { get; set; }
        }
        
        /// <summary>
        /// 修复项元数据字典
        /// </summary>
        private Dictionary<string, CorrectionItemMetadata> _correctionItemsMetadata = new Dictionary<string, CorrectionItemMetadata>();
        
        /// <summary>
        /// 预览数据缓存（用于执行时直接使用）
        /// </summary>
        private DataTable _previewData;
        
        /// <summary>
        /// 当前选中的修复项
        /// </summary>
        private string _currentCorrectionItem;

        public UCDataCorrectionCenter()
        {
            InitializeComponent();
            InitializeCorrectionItemsMetadata();
        }


        Type[] ModelTypes;
        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        List<SugarTable> stlist = new List<SugarTable>();

        private void UCDataFix_Load(object sender, EventArgs e)
        {
            // 设置应用上下文（服务已通过BusinessDIConfig自动注册）
            try
            {
                var appContext = Startup.GetFromFac<RUINORERP.Model.Context.ApplicationContext>();
                DataCorrectionServiceManager.SetApplicationContext(appContext);
                
                richTextBoxLog.AppendText("✅ 数据校正服务管理器初始化成功\r\n");
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText($"⚠️ 数据修复服务初始化失败：{ex.Message}\r\n");
            }
            
            // ✅ 动态加载所有已注册的服务
            LoadCorrectionServices();
        }
        
        /// <summary>
        /// 动态加载所有已注册的数据校正服务
        /// </summary>
        private void LoadCorrectionServices()
        {
            try
            {
                richTextBoxLog.AppendText("正在加载数据校正服务...\r\n");
                
                // ✅ 从DI容器获取所有已注册的服务
                var services = DataCorrectionServiceManager.GetAllServices();
                
                if (services == null || services.Count == 0)
                {
                    richTextBoxLog.AppendText("⚠️ 未找到任何数据校正服务\r\n");
                    return;
                }
                
                richTextBoxLog.AppendText($"发现 {services.Count} 个数据校正服务\r\n");
                
                // ✅ 清空TreeView
                treeViewFunction.Nodes.Clear();
                _correctionItemsMetadata.Clear();
                
                // ✅ 动态添加每个服务到TreeView和元数据字典
                foreach (var service in services.OrderBy(s => s.FunctionName))
                {
                    // 添加到元数据字典
                    var metadata = new CorrectionItemMetadata
                    {
                        FunctionName = service.FunctionName,
                        ProblemDescription = service.ProblemDescription,
                        AffectedTables = service.AffectedTables,
                        FixLogic = service.FixLogic,
                        OccurrenceScenario = service.OccurrenceScenario,
                        NodeText = service.FunctionName  // 使用FunctionName作为节点文本
                    };
                    
                    _correctionItemsMetadata[service.FunctionName] = metadata;
                    
                    // 添加到TreeView
                    var node = new TreeNode(service.FunctionName)
                    {
                        Tag = service.CorrectionName  // 存储CorrectionName用于后续查找
                    };
                    
                    treeViewFunction.Nodes.Add(node);
                    
                    richTextBoxLog.AppendText($"  - {service.FunctionName} ({service.CorrectionName})\r\n");
                }
                
                richTextBoxLog.AppendText($"✅ 成功加载 {services.Count} 个服务\r\n\r\n");
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText($"❌ 加载服务失败：{ex.Message}\r\n");
                richTextBoxLog.AppendText($"详细信息：{ex.StackTrace}\r\n");
            }
        }
        
        /// <summary>
        /// 初始化修复项元数据（已废弃，改为动态加载）
        /// </summary>
        [Obsolete("请使用 LoadCorrectionServices() 动态加载服务")]
        private void InitializeCorrectionItemsMetadata()
        {
            // 菜单枚举类型修复
            _correctionItemsMetadata["菜单枚举类型修复"] = new CorrectionItemMetadata
            {
                FunctionName = "菜单枚举类型修复",
                ProblemDescription = "修复菜单设置中的BizType枚举值与代码中定义的枚举值不一致的问题。当代码维护修改枚举定义后，数据库中的菜单配置可能未同步更新。",
                AffectedTables = new List<string> { "tb_MenuInfo", "tb_PrintTemplate", "tb_PrintConfig" },
                FixLogic = "遍历所有菜单项，根据MenuName重新从硬编码的枚举映射中获取正确的BizType值，并更新到数据库。同时检查打印模板和打印配置的关联关系。",
                OccurrenceScenario = "1. 开发人员修改了业务类型枚举定义；2. 新增菜单但未正确配置BizType；3. 系统升级后枚举值发生变化",
                NodeText = "菜单枚举类型修复"
            };
            
            // 采购订单价格修复
            _correctionItemsMetadata["采购订单价格修复"] = new CorrectionItemMetadata
            {
                FunctionName = "采购订单价格修复",
                ProblemDescription = "修复采购订单主表与明细表价格不一致的问题，包括单价、金额、税额等字段。",
                AffectedTables = new List<string> { "tb_PurOrder", "tb_PurOrderDetail" },
                FixLogic = "根据明细表的实际数据重新计算主表的汇总价格，确保主表TotalAmount、TotalTax等字段与明细表合计一致。",
                OccurrenceScenario = "1. 手动修改明细价格后未更新主表；2. 并发操作导致数据不一致；3. 历史数据迁移错误",
                NodeText = "采购订单价格修复"
            };
            
            // 采购入库单价格修复
            _correctionItemsMetadata["采购入库单价格修复"] = new CorrectionItemMetadata
            {
                FunctionName = "采购入库单价格修复",
                ProblemDescription = "修复采购入库单主表与明细表价格不一致的问题。",
                AffectedTables = new List<string> { "tb_PurIn", "tb_PurInDetail" },
                FixLogic = "根据入库明细的实际数量和单价重新计算主表汇总金额，修正TotalAmount、TotalTax等字段。",
                OccurrenceScenario = "1. 入库时调整价格未同步主表；2. 退货后价格计算错误；3. 系统bug导致数据异常",
                NodeText = "采购入库单价格修复"
            };
            
            // 销售订单成本数量修复
            _correctionItemsMetadata["销售订单成本数量修复"] = new CorrectionItemMetadata
            {
                FunctionName = "销售订单成本数量修复",
                ProblemDescription = "修复销售订单的成本价和数量字段异常，确保成本计算的准确性。",
                AffectedTables = new List<string> { "tb_SaleOrder", "tb_SaleOrderDetail" },
                FixLogic = "根据产品最新成本价更新订单明细的成本字段，重新计算订单总成本。检查数量字段的合理性。",
                OccurrenceScenario = "1. 成本价调整后历史订单未更新；2. 负数数量或零数量异常；3. 成本计算逻辑变更",
                NodeText = "销售订单成本数量修复"
            };
            
            // 销售出库单成本数量修复
            _correctionItemsMetadata["销售出库单成本数量修复"] = new CorrectionItemMetadata
            {
                FunctionName = "销售出库单成本数量修复",
                ProblemDescription = "修复销售出库单的成本和数量数据，确保库存成本和毛利计算准确。",
                AffectedTables = new List<string> { "tb_SaleOut", "tb_SaleOutDetail" },
                FixLogic = "根据出库时的实际成本更新明细记录，重新计算出库单总成本。验证数量的合理性。",
                OccurrenceScenario = "1. 出库时成本获取失败；2. 部分出库导致数量异常；3. 成本核算方式变更",
                NodeText = "销售出库单成本数量修复"
            };
            
            // 生产计划数量修复
            _correctionItemsMetadata["生产计划数量修复"] = new CorrectionItemMetadata
            {
                FunctionName = "生产计划数量修复",
                ProblemDescription = "修复生产计划的数量相关字段，包括计划数量、已完成数量、未完成数量等。",
                AffectedTables = new List<string> { "tb_ProdPlan", "tb_ProdPlanDetail" },
                FixLogic = "根据实际的生产任务单和入库单重新计算计划的完成进度，更新CompletedQty、UnfinishedQty等字段。",
                OccurrenceScenario = "1. 生产任务取消后计划未更新；2. 超额生产导致数量异常；3. 手工调整未同步",
                NodeText = "生产计划数量修复"
            };
            
            // 制令单自制品修复
            _correctionItemsMetadata["制令单自制品修复"] = new CorrectionItemMetadata
            {
                FunctionName = "制令单自制品修复",
                ProblemDescription = "修复制令单中自制产品的标识和关联关系错误。",
                AffectedTables = new List<string> { "tb_ProdWorkOrder", "tb_ProdDetail" },
                FixLogic = "检查制令单关联的产品是否为自制类型，修正IsSelfMade标识。确保BOM关联正确。",
                OccurrenceScenario = "1. 产品类型变更后制令单未更新；2. BOM配置错误；3. 数据导入时标识丢失",
                NodeText = "制令单自制品修复"
            };
            
            // 属性重复的SKU检测
            _correctionItemsMetadata["属性重复的SKU检测"] = new CorrectionItemMetadata
            {
                FunctionName = "属性重复的SKU检测",
                ProblemDescription = "检测并修复具有相同属性组合的重复SKU记录，避免数据冗余和查询歧义。",
                AffectedTables = new List<string> { "tb_ProdDetail", "tb_SKUAttribute" },
                FixLogic = "查找具有相同ProdID和相同属性值组合的多条SKU记录，标记重复项并提供合并或删除建议。",
                OccurrenceScenario = "1. 多次创建相同属性的SKU；2. 数据导入时未去重；3. 属性修改导致重复",
                NodeText = "属性重复的SKU检测"
            };
            
            // 将销售客户转换为目标客户
            _correctionItemsMetadata["将销售客户转换为目标客户"] = new CorrectionItemMetadata
            {
                FunctionName = "将销售客户转换为目标客户",
                ProblemDescription = "将临时销售客户转换为正式的目标客户，建立完整的客户关系管理档案。",
                AffectedTables = new List<string> { "tb_SaleOrder", "tb_CustomerVendor", "tb_CRM_TargetCustomer" },
                FixLogic = "识别仅出现在销售订单中的临时客户，创建或关联到目标客户表，更新相关单据的客户引用。",
                OccurrenceScenario = "1. 快速开单时使用临时客户；2. CRM系统上线后需要整合历史客户；3. 客户信息完善",
                NodeText = "将销售客户转换为目标客户"
            };
            
            // 销售数量与明细数量和的检测
            _correctionItemsMetadata["销售数量与明细数量和的检测"] = new CorrectionItemMetadata
            {
                FunctionName = "销售数量与明细数量和的检测",
                ProblemDescription = "检测销售订单主表数量与明细数量总和是否一致，修复不一致的数据。",
                AffectedTables = new List<string> { "tb_SaleOrder", "tb_SaleOrderDetail" },
                FixLogic = "计算每个订单明细数量的总和，与主表TotalQty对比，不一致则更新主表数量。",
                OccurrenceScenario = "1. 增删明细后主表未更新；2. 并发修改导致不一致；3. 历史数据错误",
                NodeText = "销售数量与明细数量和的检测"
            };
            
            // 借出已还修复为完结
            _correctionItemsMetadata["借出已还修复为完结"] = new CorrectionItemMetadata
            {
                FunctionName = "借出已还修复为完结",
                ProblemDescription = "将已全部归还的借出单状态从'借出中'修正为'已完结'，确保状态准确性。",
                AffectedTables = new List<string> { "tb_LendOut" },
                FixLogic = "检查借出单的已还数量是否等于借出数量，如果相等且状态不是'完结'，则更新状态为完结。",
                OccurrenceScenario = "1. 归还后状态未自动更新；2. 手工关闭遗漏；3. 状态流转逻辑bug",
                NodeText = "借出已还修复为完结"
            };
            
            // 成本修复
            _correctionItemsMetadata["成本修复"] = new CorrectionItemMetadata
            {
                FunctionName = "成本修复",
                ProblemDescription = "修复库存产品的成本价异常，包括零成本、负成本、成本波动过大等问题。",
                AffectedTables = new List<string> { "tb_Inventory", "tb_ProdDetail", "tb_CostHistory" },
                FixLogic = "根据最近的采购入库价、生产成本或标准成本重新计算库存成本。提供预览功能让用户确认后再执行。",
                OccurrenceScenario = "1. 首次入库成本为零；2. 退货导致成本异常；3. 成本核算方法变更；4. 数据迁移错误",
                NodeText = "成本修复"
            };
            
            // 拟销在制在途修复
            _correctionItemsMetadata["拟销在制在途修复"] = new CorrectionItemMetadata
            {
                FunctionName = "拟销在制在途修复",
                ProblemDescription = "修复产品的拟销数量、在制数量、在途数量统计错误，确保可用量计算准确。",
                AffectedTables = new List<string> { "tb_Inventory", "tb_SaleOrder", "tb_PurOrder", "tb_ProdPlan" },
                FixLogic = "重新统计各产品的销售订单未交数量（拟销）、生产计划未完工数量（在制）、采购订单未入库数量（在途），更新库存表的对应字段。",
                OccurrenceScenario = "1. 订单取消后统计未更新；2. 状态变更未触发重算；3. 定时任务失败",
                NodeText = "拟销在制在途修复"
            };
            
            // 修复CRM跟进计划状态
            _correctionItemsMetadata["修复CRM跟进计划状态"] = new CorrectionItemMetadata
            {
                FunctionName = "修复CRM跟进计划状态",
                ProblemDescription = "修复CRM跟进计划的状态与实际跟进情况不一致的问题。",
                AffectedTables = new List<string> { "tb_CRM_FollowUpPlan" },
                FixLogic = "根据最后跟进时间和计划下次跟进时间，自动更新跟进计划状态（待跟进、已逾期、已完成等）。",
                OccurrenceScenario = "1. 跟进后状态未更新；2. 逾期判断逻辑错误；3. 批量导入数据状态缺失",
                NodeText = "修复CRM跟进计划状态"
            };
            
            // 用户密码加密
            _correctionItemsMetadata["用户密码加密"] = new CorrectionItemMetadata
            {
                FunctionName = "用户密码加密",
                ProblemDescription = "将明文存储的用户密码转换为加密存储，提升系统安全性。",
                AffectedTables = new List<string> { "tb_User" },
                FixLogic = "检测未加密的密码（非哈希格式），使用BCrypt或其他加密算法进行加密，更新PasswordHash字段。",
                OccurrenceScenario = "1. 历史数据使用明文密码；2. 系统安全升级；3. 合规要求",
                NodeText = "用户密码加密"
            };
            
            // 配方数量成本的检测
            _correctionItemsMetadata["配方数量成本的检测"] = new CorrectionItemMetadata
            {
                FunctionName = "配方数量成本的检测",
                ProblemDescription = "检测BOM配方中子件数量与成本计算是否正确，发现异常配方。",
                AffectedTables = new List<string> { "tb_BOM", "tb_BOMDetail", "tb_ProdDetail" },
                FixLogic = "验证BOM子件数量总和是否符合预期，计算理论成本并与实际成本对比，标记差异过大的配方。",
                OccurrenceScenario = "1. BOM录入错误；2. 子件成本变更后未重算；3. 配方版本管理混乱",
                NodeText = "配方数量成本的检测"
            };
            
            // 清空财务数据
            _correctionItemsMetadata["清空财务数据"] = new CorrectionItemMetadata
            {
                FunctionName = "清空财务数据",
                ProblemDescription = "【危险操作】清空指定期间的财务凭证、应收应付等数据，用于测试环境重置或错误数据清理。",
                AffectedTables = new List<string> { "tb_FM_Voucher", "tb_FM_ARAP", "tb_FM_Payment", "tb_FM_Receipt" },
                FixLogic = "根据选择的时间范围，删除相关的财务凭证和往来款项记录。需要二次确认，建议先备份数据。",
                OccurrenceScenario = "1. 测试环境数据重置；2. 期初数据重新录入；3. 错误的批量操作需要回滚",
                NodeText = "清空财务数据"
            };
            
            // 佣金数据修复[tb_SaleOrder]
            _correctionItemsMetadata["佣金数据修复[tb_SaleOrder]"] = new CorrectionItemMetadata
            {
                FunctionName = "佣金数据修复[tb_SaleOrder]",
                ProblemDescription = "修复销售订单的佣金计算错误，包括佣金比例、佣金金额等字段。",
                AffectedTables = new List<string> { "tb_SaleOrder", "tb_SaleOrderDetail", "tb_Employee" },
                FixLogic = "根据销售人员、客户等级、产品类别重新计算佣金比例和金额，更新订单主表和明细表的佣金字段。",
                OccurrenceScenario = "1. 佣金政策调整后历史订单未更新；2. 销售人员变更；3. 计算规则bug",
                NodeText = "佣金数据修复[tb_SaleOrder]"
            };
            
            // 佣金数据修复[tb_SaleOut]
            _correctionItemsMetadata["佣金数据修复[tb_SaleOut]"] = new CorrectionItemMetadata
            {
                FunctionName = "佣金数据修复[tb_SaleOut]",
                ProblemDescription = "修复销售出库单的佣金数据，确保与订单佣金一致或按出库规则重新计算。",
                AffectedTables = new List<string> { "tb_SaleOut", "tb_SaleOutDetail", "tb_SaleOrder" },
                FixLogic = "关联销售订单获取佣金信息，或根据出库规则重新计算出库佣金，更新出库单佣金字段。",
                OccurrenceScenario = "1. 部分出库导致佣金分摊错误；2. 订单佣金变更后出库单未同步；3. 退货处理不当",
                NodeText = "佣金数据修复[tb_SaleOut]"
            };
            
            // 采购订单未交数量修复
            _correctionItemsMetadata["采购订单未交数量修复"] = new CorrectionItemMetadata
            {
                FunctionName = "采购订单未交数量修复",
                ProblemDescription = "修复采购订单的未交数量字段，确保与已入库数量的逻辑一致性。",
                AffectedTables = new List<string> { "tb_PurOrder", "tb_PurOrderDetail", "tb_PurIn" },
                FixLogic = "根据采购订单的订购数量和已入库数量，重新计算未交数量（UnDeliveredQty = OrderQty - ReceivedQty）。",
                OccurrenceScenario = "1. 入库后未更新未交数量；2. 退货后未调整；3. 订单关闭逻辑错误",
                NodeText = "采购订单未交数量修复"
            };
            
            // 采购退货数量回写修复
            _correctionItemsMetadata["采购退货数量回写修复"] = new CorrectionItemMetadata
            {
                FunctionName = "采购退货数量回写修复",
                ProblemDescription = "修复采购退货后，原入库单和采购订单的数量回写错误。",
                AffectedTables = new List<string> { "tb_PurReturn", "tb_PurIn", "tb_PurOrder" },
                FixLogic = "根据退货单数量，扣减原入库单的已入库数量，更新采购订单的已入库和未交数量。",
                OccurrenceScenario = "1. 退货后未回写上游单据；2. 多次退货累计错误；3. 跨月退货处理不当",
                NodeText = "采购退货数量回写修复"
            };
            
            // 生产计划结案子单状态检测
            _correctionItemsMetadata["生产计划结案子单状态检测"] = new CorrectionItemMetadata
            {
                FunctionName = "生产计划结案子单状态检测",
                ProblemDescription = "检测生产计划结案时，子计划或关联任务单的状态是否正确关闭。",
                AffectedTables = new List<string> { "tb_ProdPlan", "tb_ProdWorkOrder" },
                FixLogic = "检查已结案的生产计划下是否还有未完成的子计划或制令单，标记异常情况供人工处理。",
                OccurrenceScenario = "1. 父计划结案但子任务仍在进行；2. 异常终止未正确关闭；3. 状态同步延迟",
                NodeText = "生产计划结案子单状态检测"
            };
            
            // 应收付单据业务日期修复
            _correctionItemsMetadata["应收付单据业务日期修复"] = new CorrectionItemMetadata
            {
                FunctionName = "应收付单据业务日期修复",
                ProblemDescription = "修复应收应付单据的业务日期与单据日期不一致的问题，确保账期计算准确。",
                AffectedTables = new List<string> { "tb_FM_ARAP", "tb_FM_Payment", "tb_FM_Receipt" },
                FixLogic = "检查业务日期（BusinessDate）是否合理，如为空或与单据日期相差过大，则根据业务规则修正。",
                OccurrenceScenario = "1. 补录单据时日期填写错误；2. 跨期业务日期未调整；3. 系统默认值不合理",
                NodeText = "应收付单据业务日期修复"
            };
            
            // 采购订单结案状态修复
            _correctionItemsMetadata["采购订单结案状态修复"] = new CorrectionItemMetadata
            {
                FunctionName = "采购订单结案状态修复",
                ProblemDescription = "修复采购入库单审核后，采购订单相关数据未正确更新的问题。包括：\n" +
                    "1. 订单明细的DeliveredQuantity（已交数量）未累加入库数量\n" +
                    "2. 订单明细的UndeliveredQty（未交数量）未扣减入库数量\n" +
                    "3. 订单主表的TotalUndeliveredQty（总未交数量）未汇总更新\n" +
                    "4. 当所有明细都已入库完成时，订单状态DataStatus未更新为8（结案）",
                AffectedTables = new List<string> { "tb_PurOrder", "tb_PurOrderDetail", "tb_PurEntry", "tb_PurEntryDetail" },
                FixLogic = "1. 从入库单明细按PurOrder_ChildID汇总实际入库数量\n" +
                    "2. 更新订单明细的DeliveredQuantity和UndeliveredQty\n" +
                    "3. 汇总更新订单主表的TotalUndeliveredQty\n" +
                    "4. 将TotalUndeliveredQty=0的订单状态更新为8（结案）",
                OccurrenceScenario = "1. 2026年4月17-18日期间审核的采购入库单存在BUG\n" +
                    "2. 入库单审核时未正确执行数量回写逻辑\n" +
                    "3. 订单已全部入库但状态仍为'已审核'而非'结案'\n" +
                    "4. 财务对账时发现订单数量与实际入库数量不一致",
                NodeText = "采购订单结案状态修复"
            };
        }
        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());
        
        /// <summary>
        /// treeView1节点选择后，自动勾选左侧对应的表
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 清空左侧所有勾选
            UncheckAllTables();
            
            // 如果没有选中节点，清空当前选中项并返回
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Text))
            {
                _currentCorrectionItem = null;
                richTextBoxLog.AppendText("ℹ️ 已取消功能选择，所有表已取消勾选\r\n");
                return;
            }
            
            string nodeName = e.Node.Text;
            
            // ✅ 关键：设置当前选中的修复项（与AfterCheck保持一致）
            _currentCorrectionItem = nodeName;
            
            // ✅ 关键：显示选中修复项的详细信息
            DisplayCorrectionItemDetails(nodeName);
            
            // 根据选中的功能节点，自动勾选对应的表
            AutoCheckRelatedTables(nodeName);
        }
        
        /// <summary>
        /// 取消左侧所有表的勾选
        /// </summary>
        private void UncheckAllTables()
        {
            foreach (TreeNode node in treeViewTableList.Nodes)
            {
                UncheckNodeRecursive(node);
            }
        }
        
        /// <summary>
        /// 递归取消节点勾选
        /// </summary>
        private void UncheckNodeRecursive(TreeNode node)
        {
            node.Checked = false;
            foreach (TreeNode child in node.Nodes)
            {
                UncheckNodeRecursive(child);
            }
        }
        
        /// <summary>
        /// 根据功能名称自动勾选相关的表
        /// </summary>
        private void AutoCheckRelatedTables(string functionName)
        {
            var checkedTables = new List<string>();
            
            // 尝试从元数据中获取影响的表
            if (_correctionItemsMetadata.TryGetValue(functionName, out var metadata))
            {
                foreach (var tableName in metadata.AffectedTables)
                {
                    if (CheckTableByName(tableName))
                    {
                        checkedTables.Add(tableName);
                    }
                }
            }
            else
            {
                // 如果没有元数据，尝试从服务中获取
                try
                {
                    var service = DataCorrectionServiceManager.GetService(functionName);
                    if (service != null && service.AffectedTables != null)
                    {
                        foreach (var tableName in service.AffectedTables)
                        {
                            if (CheckTableByName(tableName))
                            {
                                checkedTables.Add(tableName);
                            }
                        }
                    }
                }
                catch
                {
                    // 忽略错误，可能服务还未注册
                }
            }
            
            // 输出日志
            if (checkedTables.Count > 0)
            {
                richTextBoxLog.AppendText($"   已自动勾选 {checkedTables.Count} 个表：\r\n");
                foreach (var table in checkedTables)
                {
                    richTextBoxLog.AppendText($"   - {table}\r\n");
                }
            }
            else
            {
                richTextBoxLog.AppendText($"   ⚠️ 未找到与 {functionName} 相关的表\r\n");
            }
        }
        
        /// <summary>
        /// 根据表名勾选对应的节点
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否成功找到并勾选</returns>
        private bool CheckTableByName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;
            
            // 遍历左侧TreeView查找匹配的表
            foreach (TreeNode node in treeViewTableList.Nodes)
            {
                if (CheckNodeByNameRecursive(node, tableName))
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 递归查找并勾选匹配的表节点
        /// </summary>
        private bool CheckNodeByNameRecursive(TreeNode node, string tableName)
        {
            // 检查当前节点是否匹配（通过Name或Text）
            if (!string.IsNullOrEmpty(node.Name) && node.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase))
            {
                node.Checked = true;
                return true;
            }
            
            if (!string.IsNullOrEmpty(node.Text) && node.Text.Equals(tableName, StringComparison.OrdinalIgnoreCase))
            {
                node.Checked = true;
                return true;
            }
            
            // 递归检查子节点
            foreach (TreeNode child in node.Nodes)
            {
                if (CheckNodeByNameRecursive(child, tableName))
                    return true;
            }
            
            return false;
        }
        
        private async void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeViewTableList.SelectedNode != null && treeViewFunction.SelectedNode != null)
            {


                if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_FM_PayeeInfo).Name)
                {
                    #region PayeeInfo 详细信息补充
                    var ctrPayeeInfo = Startup.GetFromFac<tb_FM_PayeeInfoController<tb_FM_PayeeInfo>>();
                    List<tb_FM_PayeeInfo> PayeeInfos = ctrPayeeInfo.Query();

                    foreach (var PayeeInfo in PayeeInfos)
                    {
                        // 获取显示名称
                        string shortName = string.Empty;
                        if (PayeeInfo.CustomerVendor_ID.HasValue && PayeeInfo.CustomerVendor_ID.Value > 0)
                        {
                            var CustomerVendor = CacheManager.GetEntity<tb_CustomerVendor>(PayeeInfo.CustomerVendor_ID);
                            if (CustomerVendor != null)
                            {
                                shortName = CustomerVendor.ShortName;
                                if (string.IsNullOrEmpty(shortName))
                                {
                                    shortName = CustomerVendor.CVName;
                                }
                            }
                        }
                        else if (PayeeInfo.Employee_ID.HasValue && PayeeInfo.Employee_ID.Value > 0)
                        {
                            var Employee = CacheManager.GetEntity<tb_Employee>(PayeeInfo.Employee_ID);
                            if (Employee != null)
                            {
                                shortName = Employee.Employee_Name;
                            }
                        }

                        // 处理可能为null的字段，并收集非空字段
                        List<string> nonEmptyFields = new List<string>();

                        // 添加非空的显示名称
                        if (!string.IsNullOrEmpty(shortName))
                        {
                            nonEmptyFields.Add(shortName);
                        }

                        // 添加非空的账户类型
                        string accountType = ((AccountType)PayeeInfo.Account_type).ToString();
                        if (!string.IsNullOrEmpty(accountType))
                        {
                            nonEmptyFields.Add(accountType);
                        }

                        // 添加非空的账户名称
                        string accountName = PayeeInfo.Account_name;
                        if (!string.IsNullOrEmpty(accountName))
                        {
                            nonEmptyFields.Add(accountName);
                        }

                        // 添加非空的账号
                        string accountNo = PayeeInfo.Account_No;
                        if (!string.IsNullOrEmpty(accountNo))
                        {
                            nonEmptyFields.Add(accountNo);
                        }

                        // 添加非空的所属银行
                        string belongingBank = PayeeInfo.BelongingBank;
                        if (!string.IsNullOrEmpty(belongingBank))
                        {
                            nonEmptyFields.Add(belongingBank);
                        }

                        // 组合非空字段更新Details，使用连字符分隔
                        PayeeInfo.Details = string.Join("-", nonEmptyFields);
                    }

                    if (!chkTestMode.Checked)
                    {
                        await MainForm.Instance.AppContext.Db.Updateable(PayeeInfos).UpdateColumns(it => new { it.Details }).ExecuteCommandAsync();
                    }

                    #endregion
                }



                if (treeViewFunction.SelectedNode.Text == "菜单枚举类型修复")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_MenuInfo).Name)
                    {
                        #region 菜单枚举类型修复
                        tb_MenuInfoController<tb_MenuInfo> ctrMenuInfo = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
                        List<tb_MenuInfo> menuInfos = ctrMenuInfo.Query();

                        tb_PrintTemplateController<tb_PrintTemplate> ctrPrintTemplate = Startup.GetFromFac<tb_PrintTemplateController<tb_PrintTemplate>>();
                        List<tb_PrintTemplate> PrintTemplates = ctrPrintTemplate.Query();

                        tb_PrintConfigController<tb_PrintConfig> ctrPrintConfig = Startup.GetFromFac<tb_PrintConfigController<tb_PrintConfig>>();
                        List<tb_PrintConfig> PrintConfigs = ctrPrintConfig.Query();


                        for (int i = 0; i < menuInfos.Count; i++)
                        {
                            if (menuInfos[i].MenuName == "借出单")
                            {

                            }

                            //检查菜单设置中的枚举类型是不是和代码中的一致，因为代码可能会维护修改
                            if (menuInfos[i].BizType != null && menuInfos[i].BizType.HasValue)
                            {
                                int oldid = menuInfos[i].BizType.Value;

                                //新值来自枚举值硬编码
                                #region 取新值后对比旧值，不一样的就更新到菜单表中

                                // 获取当前正在执行的程序集
                                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                                // 已知类的全名（包括命名空间）
                                string className = menuInfos[i].ClassPath;
                                // 获取类型对象
                                Type type = currentAssembly.GetType(className);
                                if (type != null)
                                {
                                    #region 从最基础的窗体类中获取枚举值 如果与旧值不一样则更新
                                    // 使用 Activator 创建类的实例
                                    object instance = Activator.CreateInstance(type);
                                    var descType = typeof(MenuAttrAssemblyInfo);
                                    // 类型是否为窗体，否则跳过，进入下一个循环
                                    // 是否为自定义特性，否则跳过，进入下一个循环
                                    if (!type.IsDefined(descType, false))
                                        continue;

                                    // 强制为自定义特性
                                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                                    if (attribute == null || !attribute.Enabled)
                                        continue;
                                    // 域注入
                                    //Services.AddScoped(type);
                                    MenuAttrAssemblyInfo info = new MenuAttrAssemblyInfo();
                                    info = attribute;
                                    info.ClassName = type.Name;
                                    info.ClassType = type;
                                    info.ClassPath = type.FullName;
                                    info.Caption = attribute.Describe;
                                    info.MenuPath = attribute.MenuPath;
                                    info.UiType = attribute.UiType;
                                    if (attribute.MenuBizType.HasValue)
                                    {
                                        info.MenuBizType = attribute.MenuBizType.Value;
                                    }

                                    int newid = (int)((BizType)info.MenuBizType.Value);
                                    if (oldid == newid)
                                    {
                                        //richTextBoxLog.AppendText($"{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==>" + (BizType)menuInfos[i].BizType.Value + "\r\n");
                                    }
                                    else
                                    {
                                        richTextBoxLog.AppendText($"菜单信息：{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==========不应该是===={(BizType)menuInfos[i].BizType.Value}=======应该是{newid}" + "\r\n");
                                        if (!chkTestMode.Checked)
                                        {
                                            menuInfos[i].BizType = newid;
                                            await ctrMenuInfo.UpdateMenuInfo(menuInfos[i]);
                                        }
                                    }
                                    #endregion

                                    #region 打印配置和打印模板中也用了这个BizType 
                                    /*TODO: 打印模板与业务类型和名称无关。只是通过配置ID来关联的*/
                                    var printconfig = PrintConfigs.FirstOrDefault(p => p.BizName == menuInfos[i].MenuName);
                                    if (printconfig != null && printconfig.BizType != newid)
                                    {
                                        richTextBoxLog.AppendText($"打印配置：{printconfig.BizName}=>{printconfig.BizType} ==========不应该是===={(BizType)printconfig.BizType}=======应该是{newid}" + "\r\n");
                                        printconfig.BizType = newid;
                                        if (!chkTestMode.Checked)
                                        {
                                            await ctrPrintConfig.UpdateAsync(printconfig);
                                        }

                                        if (printconfig.tb_PrintTemplates != null && printconfig.tb_PrintTemplates.Count > 0)
                                        {
                                            var reportTemplate = printconfig.tb_PrintTemplates.FirstOrDefault(c => c.BizName == menuInfos[i].MenuName);
                                            if (reportTemplate != null && reportTemplate.BizType != newid)
                                            {
                                                richTextBoxLog.AppendText($"打印模板：{reportTemplate.BizName}=>{reportTemplate.BizType} ==========不应该是===={(BizType)reportTemplate.BizType.Value}=======应该是{newid}" + "\r\n");
                                                reportTemplate.BizType = newid;
                                                if (!chkTestMode.Checked)
                                                {
                                                    await ctrPrintTemplate.UpdateAsync(reportTemplate);
                                                }

                                            }
                                        }


                                    }


                                    #endregion


                                }
                                else
                                {
                                    MessageBox.Show($"{className}类型未找到,请确认菜单{menuInfos[i].MenuName}配置是否正确。");
                                }

                                #endregion



                            }


                        }

                        #endregion
                    }
                }
                if (treeViewFunction.SelectedNode.Text == "销售退货价格修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_SaleOutRe).Name)
                    {
                        #region 销售退货价格修复


                        var ctrSaleOutRe = Startup.GetFromFac<tb_SaleOutReController<tb_SaleOutRe>>();

                        var ctrSaleOutReDetail = Startup.GetFromFac<tb_SaleOutReDetailController<tb_SaleOutReDetail>>();
                        /*

                        List<tb_SaleOutReDetail> SaleOutReDetails = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutReDetail>()
                            .Includes(c => c.tb_saleoutre, b => b.tb_SaleOutReDetails)
                            .Where(c => c.TransactionPrice > 0)
                            .ToList();

                        for (int i = 0; i < SaleOutReDetails.Count; i++)
                        {
                            //PurOrderDetails[i].Discount = 1;

                            SaleOutReDetails[i].SubtotalAmount = SaleOutReDetails[i].UnitPrice * SaleOutReDetails[i].Quantity;

                            if (SaleOutReDetails[i].tb_purorder.tb_SaleOutReDetails.Count == 1)
                            {
                                SaleOutReDetails[i].tb_purorder.TotalAmount = SaleOutReDetails[i].SubtotalAmount;
                                SaleOutReDetails[i].tb_purorder.TotalAmount = SaleOutReDetails[i].SubtotalAmount + SaleOutReDetails[i].tb_purorder.ShipCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(SaleOutReDetails[i]);
                                if (SaleOutReDetails[i].tb_purorder.tb_SaleOutReDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(SaleOutReDetails[i].tb_purorder);
                                }
                            }
                        }


                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_SaleOutReDetail> PurOrderDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutReDetail>()
                            .Includes(c => c.tb_purorder, b => b.tb_SaleOutReDetails)
                            .Where(c => c.UnitPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails1.Count; i++)
                        {


                            PurOrderDetails1[i].SubtotalAmount = PurOrderDetails1[i].UnitPrice * PurOrderDetails1[i].Quantity;

                            if (PurOrderDetails1[i].tb_purorder.tb_SaleOutReDetails.Count == 1)
                            {
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount;
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount + PurOrderDetails1[i].tb_purorder.ShipCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails1[i]);
                                if (PurOrderDetails1[i].tb_purorder.tb_SaleOutReDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails1[i].tb_purorder);
                                }
                            }
                        }
                        */




                        #endregion
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurOrder).Name)
                    {
                        #region 采购订单金额修复

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrder> PurOrders = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                            .Includes(c => c.tb_PurOrderDetails)
                            .ToList();

                        for (int i = 0; i < PurOrders.Count; i++)
                        {
                            //检测明细小计：
                            for (int j = 0; j < PurOrders[i].tb_PurOrderDetails.Count; j++)
                            {
                                //如果明细的小计不等于成交价*数量
                                if (PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount !=
                                    PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}中的{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount = PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity;
                                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i].tb_PurOrderDetails[j]).UpdateColumns(t => new { t.SubtotalAmount }).ExecuteCommandAsync();
                                        richTextBoxLog.AppendText($"采购订单明细{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}的小计金额{PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                    }
                                }
                            }

                            //检测订单总计：
                            if (PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount) != PurOrders[i].TotalAmount)
                            {
                                if (chkTestMode.Checked)
                                {
                                    richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}总金额{PurOrders[i].TotalAmount}不等于他的明细的小计求各项总和：{PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount)} " + "\r\n");
                                }
                                else
                                {
                                    PurOrders[i].TotalAmount = PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount);
                                    PurOrders[i].TotalAmount = PurOrders[i].TotalAmount + PurOrders[i].ShipCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i]).UpdateColumns(t => new { t.TotalAmount }).ExecuteCommandAsync();
                                    richTextBoxLog.AppendText($"采购订单{PurOrders[i].PurOrderNo}的总金额修复成功：{totalamountCounter} " + "\r\n");
                                }
                            }
                            //检测总计：
                        }

                        #endregion
                    }
                }
                if (treeViewFunction.SelectedNode.Text == "采购订单价格修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurOrderDetail).Name)
                    {
                        #region 采购订单明细价格修复
                        //折扣修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1
                        //单价修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1

                        tb_PurOrderController<tb_PurOrder> ctrPurOrder = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();

                        tb_PurOrderDetailController<tb_PurOrderDetail> ctrPurOrderDetail = Startup.GetFromFac<tb_PurOrderDetailController<tb_PurOrderDetail>>();

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrderDetail> PurOrderDetails = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>()
                            .Includes(c => c.tb_purorder, b => b.tb_PurOrderDetails)
                            .Where(c => c.UnitPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails.Count; i++)
                        {
                            //PurOrderDetails[i].Discount = 1;

                            PurOrderDetails[i].SubtotalAmount = PurOrderDetails[i].UnitPrice * PurOrderDetails[i].Quantity;

                            if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount;
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount + PurOrderDetails[i].tb_purorder.ShipCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails[i]);
                                if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails[i].tb_purorder);
                                }
                            }
                        }


                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrderDetail> PurOrderDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>()
                            .Includes(c => c.tb_purorder, b => b.tb_PurOrderDetails)
                            .Where(c => c.UnitPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails1.Count; i++)
                        {


                            PurOrderDetails1[i].SubtotalAmount = PurOrderDetails1[i].UnitPrice * PurOrderDetails1[i].Quantity;

                            if (PurOrderDetails1[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount;
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount + PurOrderDetails1[i].tb_purorder.ShipCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails1[i]);
                                if (PurOrderDetails1[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails1[i].tb_purorder);
                                }
                            }
                        }





                        #endregion
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurOrder).Name)
                    {
                        #region 采购订单金额修复

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrder> PurOrders = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                            .Includes(c => c.tb_PurOrderDetails)
                            .ToList();

                        for (int i = 0; i < PurOrders.Count; i++)
                        {
                            //检测明细小计：
                            for (int j = 0; j < PurOrders[i].tb_PurOrderDetails.Count; j++)
                            {
                                //如果明细的小计不等于成交价*数量
                                if (PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount !=
                                    PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}中的{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount = PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity;
                                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i].tb_PurOrderDetails[j]).UpdateColumns(t => new { t.SubtotalAmount }).ExecuteCommandAsync();
                                        richTextBoxLog.AppendText($"采购订单明细{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}的小计金额{PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                    }
                                }
                            }

                            //检测订单总计：
                            if (PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount) != PurOrders[i].TotalAmount)
                            {
                                if (chkTestMode.Checked)
                                {
                                    richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}总金额{PurOrders[i].TotalAmount}不等于他的明细的小计求各项总和：{PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount)} " + "\r\n");
                                }
                                else
                                {
                                    PurOrders[i].TotalAmount = PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount);
                                    PurOrders[i].TotalAmount = PurOrders[i].TotalAmount + PurOrders[i].ShipCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i]).UpdateColumns(t => new { t.TotalAmount }).ExecuteCommandAsync();
                                    richTextBoxLog.AppendText($"采购订单{PurOrders[i].PurOrderNo}的总金额修复成功：{totalamountCounter} " + "\r\n");
                                }
                            }
                            //检测总计：
                        }

                        #endregion
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "应收付单据业务日期修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_FM_ReceivablePayable).Name)
                    {
                        #region 应收付单据业务日期修复


                        List<tb_FM_ReceivablePayable> ReceivablePayables = MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                            .Where(c => c.SourceBizType.HasValue)
                            .Where(c => !c.BusinessDate.HasValue)
                            .ToList();

                        // 按来源业务类型分组
                        var groupedByBizType = ReceivablePayables
                            .GroupBy(d => d.SourceBizType)
                            .ToList();

                        foreach (var bizTypeGroup in groupedByBizType)
                        {
                            // 按来源单号分组
                            long[] ids = bizTypeGroup
                                                .Where(d => d.SourceBillId.HasValue)
                                                .GroupBy(d => d.SourceBillId.Value)
                                                .Select(g => g.Key)
                                                .ToArray();
                            BizType bizType = (BizType)bizTypeGroup.Key;

                            switch (bizType)
                            {
                                case BizType.销售出库单:
                                    var SaleOuts = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                        .Where(c => ids.Contains(c.SaleOut_MainID))
                                        .ToList();
                                    foreach (var saleOut in SaleOuts)
                                    {
                                        var ReceivablePayable1 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == saleOut.SaleOut_MainID);
                                        if (ReceivablePayable1 != null)
                                        {
                                            ReceivablePayable1.DocumentDate = saleOut.Created_at.Value;
                                            ReceivablePayable1.BusinessDate = saleOut.OutDate;
                                        }
                                    }

                                    break;
                                case BizType.销售退回单:
                                    var SaleOutRes = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutRe>()
                                        .Where(c => ids.Contains(c.SaleOutRe_ID))
                                        .ToList();

                                    foreach (var saleOutRe in SaleOutRes)
                                    {
                                        var ReceivablePayable2 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == saleOutRe.SaleOutRe_ID);
                                        if (ReceivablePayable2 != null)
                                        {
                                            ReceivablePayable2.DocumentDate = saleOutRe.Created_at.Value;
                                            ReceivablePayable2.BusinessDate = saleOutRe.ReturnDate.Value;
                                        }
                                    }


                                    break;
                                case BizType.采购入库单:
                                    var PurEntries = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                                        .Where(c => ids.Contains(c.PurEntryID))
                                        .ToList();

                                    foreach (var purEntry in PurEntries)
                                    {
                                        var ReceivablePayable3 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == purEntry.PurEntryID);
                                        if (ReceivablePayable3 != null)
                                        {
                                            ReceivablePayable3.DocumentDate = purEntry.Created_at.Value;
                                            ReceivablePayable3.BusinessDate = purEntry.EntryDate;
                                        }
                                    }


                                    break;
                                case BizType.采购退货单:
                                    var PurEntriesRe = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryRe>()
                                        .Where(c => ids.Contains(c.PurEntryRe_ID))
                                        .ToList();

                                    foreach (var purEntryRe in PurEntriesRe)
                                    {
                                        var ReceivablePayable4 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == purEntryRe.PurEntryRe_ID);
                                        if (ReceivablePayable4 != null)
                                        {
                                            ReceivablePayable4.DocumentDate = purEntryRe.Created_at.Value;
                                            ReceivablePayable4.BusinessDate = purEntryRe.ReturnDate;
                                        }
                                    }
                                    break;
                                case BizType.维修工单:
                                    var RepairOrder = MainForm.Instance.AppContext.Db.Queryable<tb_AS_RepairOrder>()
                                        .Where(c => ids.Contains(c.RepairOrderID))
                                        .ToList();

                                    foreach (var repairOrder in RepairOrder)
                                    {
                                        var ReceivablePayable4 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == repairOrder.RepairOrderID);
                                        if (ReceivablePayable4 != null)
                                        {
                                            ReceivablePayable4.DocumentDate = repairOrder.Created_at.Value;
                                            ReceivablePayable4.BusinessDate = repairOrder.RepairStartDate;
                                        }
                                    }
                                    break;
                                case BizType.销售价格调整单:
                                case BizType.采购价格调整单:
                                    var Adjustment = MainForm.Instance.AppContext.Db.Queryable<tb_FM_PriceAdjustment>()
                                        .Where(c => ids.Contains(c.AdjustId))
                                        .ToList();

                                    foreach (var adjustment in Adjustment)
                                    {
                                        var ReceivablePayable4 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == adjustment.AdjustId);
                                        if (ReceivablePayable4 != null)
                                        {
                                            ReceivablePayable4.DocumentDate = adjustment.Created_at.Value;
                                            ReceivablePayable4.BusinessDate = adjustment.AdjustDate;
                                        }
                                    }
                                    break;
                                case BizType.采购退货入库:
                                    var PurReturnEntry = MainForm.Instance.AppContext.Db.Queryable<tb_PurReturnEntry>()
                                        .Where(c => ids.Contains(c.PurReEntry_ID))
                                        .ToList();

                                    foreach (var purReturnEntry in PurReturnEntry)
                                    {
                                        var ReceivablePayable4 = ReceivablePayables.FirstOrDefault(c => c.SourceBizType == (int)bizType && c.SourceBillId == purReturnEntry.PurReEntry_ID);
                                        if (ReceivablePayable4 != null)
                                        {
                                            ReceivablePayable4.DocumentDate = purReturnEntry.Created_at.Value;
                                            ReceivablePayable4.BusinessDate = purReturnEntry.BillDate;//
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }

                        }




                        //创建时间
                        if (chkTestMode.Checked)
                        {
                            richTextBoxLog.AppendText($"应收付单据{ReceivablePayables.Count}个,日期没有，要补上== " + "\r\n");
                        }
                        else
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(ReceivablePayables).UpdateColumns(t => new { t.DocumentDate, t.BusinessDate }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"应收付单据{ReceivablePayables.Count}个,日期，修复成功  " + "\r\n");
                        }

                        #endregion

                    }

                }

                if (treeViewFunction.SelectedNode.Text == "采购入库单价格修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntryDetail).Name)
                    {
                        #region 采购入库明细价格修复
                        //折扣修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1
                        //单价修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1

                        tb_PurEntryController<tb_PurEntry> ctrPurEntry = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();

                        tb_PurEntryDetailController<tb_PurEntryDetail> ctrPurEntryDetail = Startup.GetFromFac<tb_PurEntryDetailController<tb_PurEntryDetail>>();

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurEntryDetail> PurEntryDetails = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                            .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                            .Where(c => c.UnitPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails.Count; i++)
                        {

                            PurEntryDetails[i].SubtotalAmount = PurEntryDetails[i].UnitPrice * PurEntryDetails[i].Quantity;
                            if (!chkTestMode.Checked)
                            {

                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails[i]);

                            }
                            else
                            {
                                richTextBoxLog.AppendText($"1采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }


                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurEntryDetail> PurEntryDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                            .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                            .Where(c => c.UnitPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails1.Count; i++)
                        {


                            PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Quantity;

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);

                            }
                            else
                            {
                                richTextBoxLog.AppendText($"2采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }



                        ////折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        //PurEntryDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                        //    .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                        //    .Where(c => c.UnitPrice != c.TransactionPrice)
                        //    .ToList();

                        //for (int i = 0; i < PurEntryDetails1.Count; i++)
                        //{
                        //    //如果是成交价等于单价*折扣，跳过
                        //    if (PurEntryDetails1[i].TransactionPrice == PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount)
                        //    {
                        //        continue;
                        //    }
                        //    PurEntryDetails1[i].TransactionPrice = PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount;
                        //    PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].TransactionPrice * PurEntryDetails1[i].Quantity;

                        //    if (!chkTestMode.Checked)
                        //    {
                        //        await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);
                        //    }
                        //    else
                        //    {
                        //        richTextBoxLog.AppendText($"3采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                        //    }
                        //}


                        #endregion
                    }
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntry).Name)
                    {
                        #region 采购入库单金额修复

                        List<tb_PurEntry> tb_PurEntrys = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_PurEntryDetails)
                            .ToList();

                        for (int i = 0; i < tb_PurEntrys.Count; i++)
                        {
                            //检测明细小计：
                            for (int j = 0; j < tb_PurEntrys[i].tb_PurEntryDetails.Count; j++)
                            {
                                //如果明细的小计不等于成交价*数量
                                if (tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount !=
                                    tb_PurEntrys[i].tb_PurEntryDetails[j].UnitPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购入库单{tb_PurEntrys[i].PurEntryNo}中金额的{tb_PurEntrys[i].tb_PurEntryDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount = tb_PurEntrys[i].tb_PurEntryDetails[j].UnitPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity;
                                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(tb_PurEntrys[i].tb_PurEntryDetails[j]).UpdateColumns(t => new { t.SubtotalAmount }).ExecuteCommandAsync();
                                        richTextBoxLog.AppendText($"采购入库单明细{tb_PurEntrys[i].tb_PurEntryDetails[j].ProdDetailID}的小计金额{tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                    }
                                }
                            }

                            //检测订单总计：
                            if (tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount) != tb_PurEntrys[i].TotalAmount)
                            {
                                if (chkTestMode.Checked)
                                {
                                    richTextBoxLog.AppendText($"采购入库单金额：{tb_PurEntrys[i].PurEntryNo}总金额{tb_PurEntrys[i].TotalAmount}不等于他的明细的小计求各项总和：{tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount)} " + "\r\n");
                                }
                                else
                                {
                                    tb_PurEntrys[i].TotalAmount = tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount);
                                    tb_PurEntrys[i].TotalAmount = tb_PurEntrys[i].TotalAmount + tb_PurEntrys[i].ShipCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(tb_PurEntrys[i]).UpdateColumns(t => new { t.TotalAmount }).ExecuteCommandAsync();
                                    richTextBoxLog.AppendText($"采购入库单{tb_PurEntrys[i].PurEntryNo}的总金额修复成功：{totalamountCounter} " + "\r\n");
                                }
                            }
                            //检测总计：
                        }

                        #endregion

                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntry).Name)
                    {
                        #region 采购订单明细有单价成交价的。入库明细中没有时要修复
                        List<tb_PurEntry> MyPurEntrys = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_PurEntryDetails)
                             .Includes(c => c.tb_purorder)
                            .Includes(c => c.tb_purorder, d => d.tb_PurOrderDetails)
                            .ToList();


                        for (int a = 0; a < MyPurEntrys.Count; a++)
                        {
                            //如果入库明细单价为0时则检测订单明细中单价多少。
                            for (int b = 0; b < MyPurEntrys[a].tb_PurEntryDetails.Count; b++)
                            {
                                if (MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice == 0)
                                {
                                    //没有引用订单的跳过
                                    if (MyPurEntrys[a].tb_purorder == null)
                                    {
                                        continue;
                                    }
                                    var orderdetail = MyPurEntrys[a].tb_purorder.tb_PurOrderDetails.FirstOrDefault(c => c.ProdDetailID == MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID);
                                    if (orderdetail != null)
                                    {
                                        if (orderdetail.UnitPrice > 0)
                                        {
                                            //更新入库单明细
                                            if (chkTestMode.Checked)
                                            {
                                                richTextBoxLog.AppendText($"采购入库单明细{MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID}的价格需要修复。" + "\r\n");
                                            }
                                            else
                                            {
                                                MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice = orderdetail.UnitPrice;

                                                int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(MyPurEntrys[a].tb_PurEntryDetails[b]).UpdateColumns(t => new { t.SubtotalAmount, t.TaxAmount, t.UnitPrice }).ExecuteCommandAsync();
                                                richTextBoxLog.AppendText($"采购入库单明细{MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID}的小计金额{MyPurEntrys[a].tb_PurEntryDetails[b].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }


                if (treeViewFunction.SelectedNode.Text == "生产计划结案子单状态检测")
                {
                    //要回写到采购入库及退货中
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ProductionPlan).Name)
                    {
                        #region 生产计划结案子单状态检测

                        List<tb_ProductionDemand> needupdateProductionDemands = new List<tb_ProductionDemand>();
                        List<tb_ManufacturingOrder> needupdateManufacturingOrders = new List<tb_ManufacturingOrder>();

                        // 优化：分批处理大数据集，避免一次性加载过多数据到内存
                        const int BATCH_SIZE = 100;
                        int page = 1;
                        int totalProcessed = 0;
                        
                        while (true)
                        {
                            var batchPlans = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                                      .Includes(c => c.tb_ProductionDemands, d => d.tb_ManufacturingOrders)
                                      .Where(c => c.DataStatus == (int)DataStatus.完结)
                                      .Skip((page - 1) * BATCH_SIZE)
                                      .Take(BATCH_SIZE)
                                      .ToListAsync();
                            
                            if (!batchPlans.Any()) break; // 没有更多数据，退出循环
                            
                            foreach (var Plan in batchPlans)
                            {
                                var Demands = Plan.tb_ProductionDemands.Where(c => c.DataStatus != (int)DataStatus.完结).ToList();
                                for (int a = 0; a < Demands.Count; a++)
                                {
                                    var Demand = Demands[a];
                                    Demand.DataStatus = (int)DataStatus.完结;
                                    if (!needupdateProductionDemands.Contains(Demand))
                                    {
                                        needupdateProductionDemands.Add(Demand);
                                    }

                                    #region 存在制令单时才处理
                                    //存在退回单时才处理
                                    if (Demand.tb_ManufacturingOrders.Any())
                                    {
                                        foreach (var item in Demand.tb_ManufacturingOrders)
                                        {
                                            if (item.DataStatus != (int)DataStatus.完结)
                                            {
                                                item.DataStatus = (int)DataStatus.完结;
                                                if (!needupdateManufacturingOrders.Contains(item))
                                                {
                                                    needupdateManufacturingOrders.Add(item);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            
                            page++;
                            totalProcessed += batchPlans.Count;
                            
                            // 每处理 10 批输出进度日志
                            if (page % 10 == 0)
                            {
                                richTextBoxLog.AppendText($"已处理 {totalProcessed} 条生产计划记录...\r\n");
                            }
                        }


                        if (chkTestMode.Checked)
                        {
                            richTextBoxLog.AppendText($"生产计划结案子单状态检测修复 需求:{needupdateProductionDemands.Count}" + "\r\n");
                            richTextBoxLog.AppendText($"生产计划结案子单状态检测修复 制令单:{needupdateManufacturingOrders.Count}" + "\r\n");
                        }
                        else
                        {
                            int entrycounter = 0;
                            int ordercounter = 0;

                            if (needupdateProductionDemands.Any())
                            {
                                entrycounter = await MainForm.Instance.AppContext.Db.Updateable(needupdateProductionDemands).UpdateColumns(it => new { it.DataStatus }).ExecuteCommandAsync();
                            }

                            if (needupdateManufacturingOrders.Any())
                            {
                                ordercounter = await MainForm.Instance.AppContext.Db.Updateable(needupdateManufacturingOrders).UpdateColumns(it => new { it.DataStatus }).ExecuteCommandAsync();
                            }

                            richTextBoxLog.AppendText($"生产计划结案子单状态检测修复成功行数需求：{entrycounter} " + "\r\n");
                            richTextBoxLog.AppendText($"生产计划结案子单状态检测修复成功行数计划：{ordercounter} " + "\r\n");
                        }

                        #endregion
                    }

                }
                if (treeViewFunction.SelectedNode.Text == "采购退货数量回写修复")
                {
                    //要回写到采购入库及退货中
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntryRe).Name)
                    {
                        #region 采购退货数量回写修复

                        Dictionary<string, List<tb_PurEntryDetail>> needupdatePurEntryDetails = new Dictionary<string, List<tb_PurEntryDetail>>();
                        Dictionary<string, List<tb_PurOrderDetail>> needupdatePurOrderDetails = new Dictionary<string, List<tb_PurOrderDetail>>();


                        //回写订单
                        List<tb_PurOrder> PurOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                             .Includes(b => b.tb_PurOrderDetails)
                            .Includes(b => b.tb_PurEntries, c => c.tb_PurEntryDetails)
                            .AsNavQueryable()
                            .Includes(b => b.tb_PurEntries, c => c.tb_PurEntryRes, d => d.tb_PurEntryReDetails)
                            .Where(c => c.DataStatus >= (int)DataStatus.确认)
                            .ToListAsync();


                        foreach (var item in PurOrders)
                        {
                            var PurEntries = item.tb_PurEntries.Where(c => c.DataStatus >= (int)DataStatus.确认).ToList();
                            for (int a = 0; a < PurEntries.Count; a++)
                            {
                                var purEntry = PurEntries[a];
                                #region 存在退回单时才处理
                                //存在退回单时才处理
                                if (purEntry.tb_PurEntryRes.Any())
                                {
                                    foreach (var purEntryLines in purEntry.tb_PurEntryDetails)
                                    {
                                        purEntryLines.ReturnedQty = 0;
                                        var PurEntryRes = purEntry.tb_PurEntryRes.Where(c => c.DataStatus >= (int)DataStatus.确认).ToList();
                                        for (int b = 0; b < PurEntryRes.Count; b++)
                                        {
                                            var purentryRetrun = PurEntryRes[b];
                                            //采购退货明细
                                            var returnDetails = purentryRetrun.tb_PurEntryReDetails;

                                            var detail = returnDetails.FirstOrDefault(c => c.ProdDetailID == purEntryLines.ProdDetailID && c.Location_ID == purEntryLines.Location_ID);
                                            if (detail != null)
                                            {
                                                purEntryLines.ReturnedQty += detail.Quantity;
                                                if (needupdatePurEntryDetails.ContainsKey(purEntry.PurEntryNo))
                                                {
                                                    needupdatePurEntryDetails[purEntry.PurEntryNo].Add(purEntryLines);
                                                }
                                                else
                                                {
                                                    var lines = new List<tb_PurEntryDetail>();
                                                    lines.Add(purEntryLines);
                                                    needupdatePurEntryDetails.Add(purEntry.PurEntryNo, lines);
                                                }

                                            }
                                        }
                                    }
                                }
                                #endregion
                            }


                            foreach (var purorderdetail in item.tb_PurOrderDetails)
                            {
                                purorderdetail.TotalReturnedQty = 0;
                                for (int a = 0; a < PurEntries.Count; a++)
                                {
                                    var purEntry = PurEntries[a];
                                    //存在退回单时才处理
                                    if (purEntry.tb_PurEntryRes.Any())
                                    {
                                        var detail = purEntry.tb_PurEntryDetails.FirstOrDefault(c => c.ProdDetailID == purorderdetail.ProdDetailID && c.Location_ID == purorderdetail.Location_ID);
                                        if (detail == null)
                                        {
                                            continue;
                                        }
                                        purorderdetail.TotalReturnedQty += detail.ReturnedQty;

                                        if (needupdatePurOrderDetails.ContainsKey(item.PurOrderNo))
                                        {
                                            needupdatePurOrderDetails[item.PurOrderNo].Add(purorderdetail);
                                        }
                                        else
                                        {
                                            var lines = new List<tb_PurOrderDetail>();
                                            lines.Add(purorderdetail);
                                            needupdatePurOrderDetails.Add(item.PurOrderNo, lines);
                                        }

                                    }
                                }
                            }
                        }


                        if (chkTestMode.Checked)
                        {
                            foreach (var item in needupdatePurEntryDetails)
                            {
                                richTextBoxLog.AppendText($"采购退货时回写入库{item.Key}修复行数为:{item.Value.Count}" + "\r\n");
                            }
                            foreach (var item in needupdatePurOrderDetails)
                            {
                                richTextBoxLog.AppendText($"采购退货时回写订单单数{item.Key}修复行数为:{item.Value.Count}" + "\r\n");
                            }
                        }
                        else
                        {
                            int entrycounter = 0;
                            int ordercounter = 0;

                            foreach (var item in needupdatePurEntryDetails)
                            {
                                if (item.Value.Any())
                                {
                                    entrycounter = await MainForm.Instance.AppContext.Db.Updateable(item.Value).UpdateColumns(it => new { it.ReturnedQty }).ExecuteCommandAsync();
                                }
                                richTextBoxLog.AppendText($"采购退货时回写成功。入库{item.Key}修复行数为:{item.Value.Count}" + "\r\n");
                            }
                            foreach (var item in needupdatePurOrderDetails)
                            {
                                if (item.Value.Any())
                                {
                                    ordercounter = await MainForm.Instance.AppContext.Db.Updateable(item.Value).UpdateColumns(it => new { it.TotalReturnedQty }).ExecuteCommandAsync();
                                }

                                richTextBoxLog.AppendText($"采购退货时回写成功。订单单数{item.Key}修复行数为:{item.Value.Count}" + "\r\n");
                            }



                            richTextBoxLog.AppendText($"采购退货时回写入库修复成功行数：{entrycounter} " + "\r\n");
                            richTextBoxLog.AppendText($"采购退货时回写订单修复成功行数：{ordercounter} " + "\r\n");
                        }

                        #endregion
                    }

                }


                if (treeViewFunction.SelectedNode.Text == "借出已还修复为完结")
                {
                    List<tb_ProdBorrowing> items = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                       .Includes(c => c.tb_ProdBorrowingDetails)
                       .Where(c => c.DataStatus == (int)DataStatus.确认)
                       .ToList();

                    foreach (tb_ProdBorrowing item in items)
                    {
                        if (!item.TotalQty.Equals(item.tb_ProdBorrowingDetails.Sum(c => c.Qty)))
                        {
                            richTextBoxLog.AppendText($"借出总数量和明细和不对：{item.BorrowID}：{item.BorrowNo}" + "\r\n");
                        }

                        if (item.TotalQty.Equals(item.tb_ProdBorrowingDetails.Sum(c => c.ReQty)))
                        {
                            richTextBoxLog.AppendText($"借出数量等于归还数量：{item.BorrowID}：{item.BorrowNo}" + "\r\n");
                        }
                    }

                    if (chkTestMode.Checked)
                    {

                    }


                }

                if (treeViewFunction.SelectedNode.Text == "修复CRM跟进计划状态")
                {
                    #region 修复CRM跟进计划状态
                    List<tb_CRM_FollowUpPlans> followUpPlans = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpPlans>()
              .Includes(c => c.tb_CRM_FollowUpRecordses)
              .Where(c => c.PlanEndDate < System.DateTime.Today &&
              (c.PlanStatus != (int)FollowUpPlanStatus.已完成 || c.PlanStatus == (int)FollowUpPlanStatus.未执行)
              && c.PlanStatus != (int)FollowUpPlanStatus.已取消
              )
              .ToListAsync();

                    // 假设配置的延期天数存储在 DelayDays 变量中
                    int DelayDays = 3;

                    for (int i = 0; i < followUpPlans.Count; i++)
                    {
                        if (followUpPlans[i].tb_CRM_FollowUpRecordses.Count > 0)
                        {
                            followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.已完成;
                        }
                        else
                        {
                            if (followUpPlans[i].PlanEndDate < System.DateTime.Today)
                            {
                                TimeSpan timeSinceEnd = System.DateTime.Today - followUpPlans[i].PlanEndDate;
                                if (timeSinceEnd.TotalDays <= DelayDays)
                                {
                                    followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.延期中;
                                }
                                else
                                {
                                    followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.未执行;
                                }

                                //发送消息给执行人。
                            }
                        }


                    }

                    #endregion
                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"要修复的行数为:{followUpPlans.Count}" + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_FollowUpPlans>(followUpPlans).UpdateColumns(it => new { it.PlanStatus }).ExecuteCommandAsync();

                        richTextBoxLog.AppendText($"修复CRM跟进计划状态成功：{plancounter} " + "\r\n");
                    }

                }

                if (treeViewFunction.SelectedNode.Text == "拟销在制在途修复")
                {
                    List<tb_Inventory> UpdateInventories = new List<tb_Inventory>(); //要更新的Inventories
                    StringBuilder sb = new StringBuilder();
                    #region 拟销在制在途修复
                    List<tb_Inventory> inventories = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                      .Includes(c => c.tb_proddetail)
                      .Includes(c => c.tb_location)
                      .ToListAsync();
                    //按仓库按产品去各种业务单据中去找。找到更新
                    List<tb_SaleOrder> SaleOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                     .Includes(c => c.tb_SaleOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();
                    int totalSaleQty = 0;


                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalSaleQty = 0;

                        tb_Inventory inventory = inventories[i];
                        foreach (var item in SaleOrders)
                        {
                            totalSaleQty += item.tb_SaleOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.TotalDeliveredQty));
                        }

                        if (inventory.Sale_Qty != totalSaleQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} 拟销数量由{inventory.Sale_Qty}修复为:{totalSaleQty}" + "\r\n");
                            inventory.Sale_Qty = totalSaleQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion


                    #region 在途修复

                    //按仓库按产品去各种业务单据中去找。找到更新
                    List<tb_PurOrder> PurOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                     .Includes(c => c.tb_PurOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();

                    List<tb_PurEntryRe> PurEntryRes = await MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryRe>()
                 .Includes(c => c.tb_PurEntryReDetails)
                 .Where(c => c.DataStatus == (int)DataStatus.确认 && c.ProcessWay == (int)PurReProcessWay.需要返回)
                 .ToListAsync();

                    List<tb_MRP_ReworkReturn> ReworkReturns = await MainForm.Instance.AppContext.Db.Queryable<tb_MRP_ReworkReturn>()
                    .Includes(c => c.tb_MRP_ReworkReturnDetails)
                    .Where(c => c.DataStatus == (int)DataStatus.确认)
                    .ToListAsync();

                    int totalOntheWayQty = 0;
                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalOntheWayQty = 0;
                        tb_Inventory inventory = inventories[i];
                        foreach (var item in PurOrders)
                        {
                            totalOntheWayQty += item.tb_PurOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        foreach (var item in ReworkReturns)
                        {
                            totalOntheWayQty += item.tb_MRP_ReworkReturnDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        foreach (var item in PurEntryRes)
                        {
                            totalOntheWayQty += item.tb_PurEntryReDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        if (inventory.On_the_way_Qty != totalOntheWayQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} " +
                                $"在途数量由{inventory.On_the_way_Qty}修复为:{totalOntheWayQty}" + "\r\n");
                            inventory.On_the_way_Qty = totalOntheWayQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    #region 在制修复
                    //在制作中，是算主表的制作目标的产品。
                    List<tb_ManufacturingOrder> manufacturingOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                     .Includes(c => c.tb_ManufacturingOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();
                    int totalMakingQty = 0;

                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalMakingQty = 0;
                        tb_Inventory inventory = inventories[i];

                        totalMakingQty += manufacturingOrders.Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                            .Sum(c => (c.ManufacturingQty - c.QuantityDelivered));

                        if (inventory.MakingQty != totalMakingQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} " +
                                $"在制数量由{inventory.MakingQty}修复为:{totalMakingQty}" + "\r\n");
                            inventory.MakingQty = totalMakingQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    #region 未发数量修复


                    decimal totalNotOutQty = 0;

                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalNotOutQty = 0;
                        tb_Inventory inventory = inventories[i];
                        foreach (var item in manufacturingOrders)
                        {
                            totalNotOutQty += item.tb_ManufacturingOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.ShouldSendQty - c.ActualSentQty));
                        }


                        if (inventory.NotOutQty != totalNotOutQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID}" +
                                $"未发数量由{inventory.NotOutQty}修复为:{totalNotOutQty}" + "\r\n");
                            inventory.NotOutQty = totalNotOutQty.ToInt();
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    richTextBoxLog.AppendText(sb.ToString());
                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"拟销在制在途修复 数据行：{UpdateInventories.Count} " + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(UpdateInventories)
                            .UpdateColumns(it => new { it.Sale_Qty, it.On_the_way_Qty, it.NotOutQty, it.MakingQty }).ExecuteCommandAsync();
                        richTextBoxLog.AppendText($"拟销在制在途修复 数据行：{plancounter} " + "\r\n");
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "用户密码加密")
                {
                    #region 用户密码加密
                    List<tb_UserInfo> AllUsers = MainForm.Instance.AppContext.Db.Queryable<tb_UserInfo>()
                        .Where(c => c.Password.Length < 10)
                     .ToList();
                    for (int i = 0; i < AllUsers.Count; i++)
                    {
                        AllUsers[i].Notes = AllUsers[i].Password;
                        string enPwd = EncryptionHelper.AesEncryptByHashKey(AllUsers[i].Password, AllUsers[i].UserName);
                        AllUsers[i].Password = enPwd;
                        // string pwd = EncryptionHelper.AesDecryptByHashKey(enPwd, "张家歌");
                        richTextBoxLog.AppendText($"要修复的用户:{AllUsers[i].UserName}" + "\r\n");
                    }

                    #endregion
                    //一次性统计加密码一下密码：

                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"要修复的用户密码加密行数为:{AllUsers.Count}" + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_UserInfo>(AllUsers).UpdateColumns(it => new { it.Password, it.Notes }).ExecuteCommandAsync();

                        richTextBoxLog.AppendText($"要修复的用户密码加密状态成功：{plancounter} " + "\r\n");
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "成本修复")
                {
                    dataGridView1.DataSource = await CostFix(false);
                }

                if (treeViewFunction.SelectedNode.Text == "销售订单成本数量修复")
                {
                    List<tb_SaleOrderDetail> saleOrderDetails = new List<tb_SaleOrderDetail>();
                    List<tb_SaleOrder> updatelist = new List<tb_SaleOrder>();
                    #region 销售订单数量与明细数量和的检测
                    List<tb_SaleOrder> SaleOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                        .Includes(c => c.tb_SaleOrderDetails)
                       //.Where(c => c.SOrderNo == "SO0066")
                       //.Where(c => c.Created_at.HasValue && c.Created_at.Value < DateTime.Now.AddMonths(-15))
                       .ToListAsync();
                    foreach (tb_SaleOrder SaleOrder in SaleOrders)
                    {
                        for (int i = 0; i < SaleOrder.tb_SaleOrderDetails.Count; i++)
                        {
                            var detail = SaleOrder.tb_SaleOrderDetails[i];
                            bool needadd = false;
                            if (detail.TransactionPrice != detail.UnitPrice * detail.Discount)
                            {

                                if (detail.SubtotalTransAmount == detail.TransactionPrice * detail.Quantity)
                                {
                                    //说明是折扣问题
                                    richTextBoxLog.AppendText($"销售订单 说明是折扣问题 明细 成交价 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                    if (detail.Discount == 1)
                                    {
                                        detail.UnitPrice = detail.SubtotalTransAmount / detail.Quantity;
                                    }
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"销售订单 明细 成交价 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                }

                                detail.TransactionPrice = detail.UnitPrice * detail.Discount;
                                needadd = true;


                            }


                            if (detail.SubtotalTransAmount != detail.TransactionPrice * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细  成交价小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
                                needadd = true;
                            }

                            if (detail.CommissionAmount != detail.UnitCommissionAmount * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细  佣金小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.CommissionAmount = detail.UnitCommissionAmount * detail.Quantity;
                                needadd = true;
                            }


                            if (detail.SubtotalTaxAmount != detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate)
                            {
                                decimal tempTax = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                decimal diffpirce = Math.Abs(detail.SubtotalTaxAmount - tempTax);
                                if (diffpirce > 0.01m)
                                {
                                    richTextBoxLog.AppendText($"销售订单 明细税额小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                    detail.SubtotalTaxAmount = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                    needadd = true;
                                }

                            }

                            if (detail.SubtotalCostAmount != (detail.Cost + detail.CustomizedCost) * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细成本小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;
                                needadd = true;
                            }


                            if (needadd)
                            {
                                saleOrderDetails.Add(detail);
                                needadd = false;
                            }
                        }


                        bool needaddmain = false;
                        if (!SaleOrder.TotalQty.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity)))
                        {
                            SaleOrder.TotalQty = SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity);

                            richTextBoxLog.AppendText($"销售订单 总数量不对：{SaleOrder.SOrderNo}" + "\r\n");
                            needaddmain = true;
                        }

                        if (!SaleOrder.TotalCommissionAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.CommissionAmount)))
                        {
                            SaleOrder.TotalCommissionAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.CommissionAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总佣金不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }
                        if (!SaleOrder.TotalAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + SaleOrder.FreightIncome))
                        {
                            SaleOrder.TotalAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + SaleOrder.FreightIncome;
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总成交价 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }

                        if (!SaleOrder.TotalTaxAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount)))
                        {
                            SaleOrder.TotalTaxAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总税额 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }

                        if (!SaleOrder.TotalCost.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount)))
                        {
                            SaleOrder.TotalCost = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount);

                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总成本 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }
                        if (needaddmain)
                        {
                            updatelist.Add(SaleOrder);
                            needaddmain = false;
                        }
                    }


                    if (!chkTestMode.Checked)
                    {
                        if (saleOrderDetails.Count > 0)
                        {
                            int detailcounter = await MainForm.Instance.AppContext.Db.Updateable(saleOrderDetails).UpdateColumns(t => new
                            {
                                t.SubtotalCostAmount,
                                t.UnitPrice,
                                t.CommissionAmount,
                                t.TransactionPrice,
                                t.SubtotalTransAmount,
                                t.SubtotalTaxAmount
                            }).ExecuteCommandAsync();
                            if (detailcounter > 0)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细 修复成功：{detailcounter} " + "\r\n");
                            }
                        }

                        if (updatelist.Count > 0)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist)
                           .UpdateColumns(t => new
                           {
                               t.TotalQty,
                               t.TotalCost,
                               t.TotalAmount,
                               t.TotalCommissionAmount,
                               t.TotalTaxAmount,
                           }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"销售订单 主表 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                    else
                    {
                        richTextBoxLog.AppendText($"销售订单 主表{updatelist.Count} 明细 {saleOrderDetails.Count}  需要修复" + "\r\n");
                    }
                    #endregion
                }


                if (treeViewFunction.SelectedNode.Text == "销售出库单成本数量修复")
                {
                    List<tb_SaleOutDetail> saleOutDetails = new List<tb_SaleOutDetail>();
                    List<tb_SaleOut> updatelist = new List<tb_SaleOut>();
                    #region 销售出库数量与明细数量和的检测
                    List<tb_SaleOut> SaleOuts = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                       .ToListAsync();
                    foreach (tb_SaleOut Saleout in SaleOuts)
                    {
                        for (int i = 0; i < Saleout.tb_SaleOutDetails.Count; i++)
                        {
                            var detail = Saleout.tb_SaleOutDetails[i];
                            bool needadd = false;
                            if (detail.TransactionPrice != detail.UnitPrice * detail.Discount)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细 成交价 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.TransactionPrice = detail.UnitPrice * detail.Discount;
                                needadd = true;
                            }


                            if (detail.SubtotalTransAmount != detail.TransactionPrice * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细  成交价小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
                                needadd = true;
                            }

                            if (detail.SubtotalTaxAmount != detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate)
                            {
                                decimal tempTax = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                decimal diffpirce = Math.Abs(detail.SubtotalTaxAmount - tempTax);
                                if (diffpirce > 0.01m)
                                {
                                    richTextBoxLog.AppendText($"销售出库 明细税额小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                    detail.SubtotalTaxAmount = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                    needadd = true;
                                }

                            }

                            if (detail.CommissionAmount != detail.UnitCommissionAmount * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细  佣金小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.CommissionAmount = detail.UnitCommissionAmount * detail.Quantity;
                                needadd = true;
                            }

                            if (detail.SubtotalCostAmount != (detail.Cost + detail.CustomizedCost) * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细成本小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;
                                needadd = true;
                            }
                            if (needadd)
                            {
                                saleOutDetails.Add(detail);
                                needadd = false;
                            }
                        }


                        bool needaddmain = false;
                        if (!Saleout.TotalQty.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.Quantity)))
                        {
                            Saleout.TotalQty = Saleout.tb_SaleOutDetails.Sum(c => c.Quantity);

                            richTextBoxLog.AppendText($"销售出库 总数量不对：{Saleout.SaleOutNo}" + "\r\n");
                            needaddmain = true;
                        }

                        if (!Saleout.TotalCommissionAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.CommissionAmount)))
                        {
                            Saleout.TotalCommissionAmount = Saleout.tb_SaleOutDetails.Sum(c => c.CommissionAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总佣金不对：{Saleout.SaleOutNo}" + "\r\n");
                        }
                        if (!Saleout.TotalAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount) + Saleout.FreightIncome))
                        {
                            Saleout.TotalAmount = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount) + Saleout.FreightIncome;
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总成交价 不对：{Saleout.SaleOutNo}" + "\r\n");
                        }

                        if (!Saleout.TotalTaxAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount)))
                        {

                            decimal diffpirce = Math.Abs(Saleout.TotalTaxAmount - Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount));
                            if (diffpirce > 0.01m && ComparePrice(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount).ToDouble(), Saleout.TotalTaxAmount.ToDouble()) > 10)
                            {
                                Saleout.TotalTaxAmount = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);
                                needaddmain = true;
                                richTextBoxLog.AppendText($"销售出库 总税额 不对：{Saleout.SaleOutNo}" + "\r\n");
                            }

                        }

                        if (!Saleout.TotalCost.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + Saleout.FreightCost))
                        {
                            Saleout.TotalCost = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + Saleout.FreightCost;

                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总成本 不对：{Saleout.SaleOutNo}" + "\r\n");
                        }


                        if (needaddmain)
                        {
                            updatelist.Add(Saleout);
                            needaddmain = false;
                        }
                    }




                    if (!chkTestMode.Checked)
                    {
                        if (saleOutDetails.Count > 0)
                        {
                            int detailcounter = await MainForm.Instance.AppContext.Db.Updateable(saleOutDetails).UpdateColumns(t => new
                            {

                                t.SubtotalCostAmount,
                                t.UnitPrice,
                                t.CommissionAmount,
                                t.TransactionPrice,
                                t.SubtotalTransAmount,
                                t.SubtotalTaxAmount

                            }).ExecuteCommandAsync();
                            if (detailcounter > 0)
                            {
                                richTextBoxLog.AppendText($"销售出库 数量成本的检测 修复成功：{detailcounter} " + "\r\n");
                            }
                        }

                        if (updatelist.Count > 0)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist)
                           .UpdateColumns(t => new
                           {
                               t.TotalQty,
                               t.TotalCost,
                               t.TotalAmount,
                               t.TotalCommissionAmount,
                               t.TotalTaxAmount,

                           }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"销售出库 数量成本的检测 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                    else
                    {
                        richTextBoxLog.AppendText($"销售出库 主表{updatelist.Count} 明细 {saleOutDetails.Count}  需要修复" + "\r\n");
                    }
                    #endregion
                }


                if (treeViewFunction.SelectedNode.Text == "配方数量成本的检测")
                {
                    List<tb_BOM_S> bomupdatelist = new();

                    List<tb_BOM_SDetail> bomDetailUpdateList = new();
                    #region 明细要等于主表中的数量的检测
                    List<tb_BOM_S> BOM_Ss = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails)
                       .ToList();
                    foreach (tb_BOM_S bom in BOM_Ss.ToArray())
                    {

                        if (bom.BOM_No == "BS250510021")
                        {

                        }

                        if (!bom.TotalMaterialQty.Equals(bom.tb_BOM_SDetails.Sum(c => c.UsedQty)))
                        {
                            richTextBoxLog.AppendText($"配方主次表数量不一致：{bom.BOM_ID}：{bom.BOM_No} new:{bom.tb_BOM_SDetails.Sum(c => c.UsedQty)} old{bom.TotalMaterialQty}" + "\r\n");
                            bom.TotalMaterialQty = bom.tb_BOM_SDetails.Sum(c => c.UsedQty);
                            bomupdatelist.Add(bom);
                        }

                        bool isUpdate = false;
                        //小计检测
                        for (int i = 0; i < bom.tb_BOM_SDetails.Count; i++)
                        {
                            var detail = bom.tb_BOM_SDetails[i];

                            decimal tempSubtotal = detail.UsedQty * detail.UnitCost;
                            tempSubtotal = Math.Round(tempSubtotal, 4);
                            if (detail.SubtotalUnitCost != tempSubtotal)
                            {
                                richTextBoxLog.AppendText($"配方明细小计与数量*单价不一致：{bom.BOM_ID}：{bom.BOM_No} new:{tempSubtotal} old{detail.SubtotalUnitCost}" + "\r\n");
                                detail.SubtotalUnitCost = tempSubtotal;
                                bomDetailUpdateList.Add(detail);
                                isUpdate = true;
                            }
                        }

                        if (!bom.TotalMaterialCost.Equals(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)) || isUpdate)
                        {

                            decimal diffpirce = Math.Abs(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost) - bom.TotalMaterialCost);
                            if (diffpirce > 0.2m && ComparePrice(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost).ToDouble(), bom.TotalMaterialCost.ToDouble()) > 10)
                            {
                                richTextBoxLog.AppendText($"=====成本相差较大：{bom.BOM_ID}：{bom.BOM_No}   new {bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)}  old {bom.TotalMaterialCost}" + "\r\n");
                                richTextBoxLog.AppendText($"配方主次表材料成本不一致：{bom.BOM_ID}：{bom.BOM_No}   new {bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)}  old {bom.TotalMaterialCost}" + "\r\n");
                            }


                            bom.TotalMaterialCost = bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                            bom.OutProductionAllCosts = bom.TotalMaterialCost + bom.TotalOutManuCost + bom.OutApportionedCost;
                            bom.SelfProductionAllCosts = bom.TotalMaterialCost + bom.TotalSelfManuCost + bom.SelfApportionedCost;
                            bomupdatelist.Add(bom);
                        }

                    }

                    #endregion

                    if (!chkTestMode.Checked)
                    {
                        if (bomDetailUpdateList.Any())
                        {
                            int totalDetailCounter = await MainForm.Instance.AppContext.Db.Updateable(bomDetailUpdateList)
                           .UpdateColumns(t => new
                           {
                               t.SubtotalUnitCost,
                           }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复配方 明细小计数据 修复成功：{totalDetailCounter} " + "\r\n");
                        }

                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(bomupdatelist)
                            .UpdateColumns(t => new
                            {
                                t.TotalMaterialQty,
                                t.TotalMaterialCost,
                                t.OutProductionAllCosts,
                                t.SelfProductionAllCosts
                            }).ExecuteCommandAsync();
                        richTextBoxLog.AppendText($"修复配方数量成本的检测 修复成功：{totalamountCounter} " + "\r\n");
                    }

                }
                if (treeViewFunction.SelectedNode.Text == "销售数量与明细数量和的检测")
                {
                    #region 销售订单数量与明细数量和的检测
                    List<tb_SaleOrder> SaleOrders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                        .Includes(c => c.tb_SaleOrderDetails)
                       .ToList();
                    foreach (tb_SaleOrder Order in SaleOrders)
                    {
                        if (!Order.TotalQty.Equals(Order.tb_SaleOrderDetails.Sum(c => c.Quantity)))
                        {
                            richTextBoxLog.AppendText($"销售订单数量不对：{Order.SOrder_ID}：{Order.SOrderNo}" + "\r\n");
                        }
                    }

                    #endregion

                    #region 销售出库数量与明细数量和的检测
                    List<tb_SaleOut> SaleOuts = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                       .ToList();
                    foreach (tb_SaleOut Saleout in SaleOuts)
                    {
                        if (!Saleout.TotalQty.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.Quantity)))
                        {
                            richTextBoxLog.AppendText($"销售出库数量不对：{Saleout.SaleOut_MainID}：{Saleout.SaleOutNo}" + "\r\n");
                        }
                    }

                    #endregion

                }

                if (treeViewFunction.SelectedNode.Text == "将销售客户转换为目标客户")
                {
                    MessageBox.Show("只能执行一次。已经执行过了。");
                    //crm数据修复 只能执行一次。这里要注释掉。

                    #region 
                    List<tb_CustomerVendor> CustomerVendors = MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>()
                        .IncludesAllFirstLayer()
                        .Where(c => c.IsCustomer == true && !c.CVName.Contains("信保"))
                        .ToList();
                    List<tb_CRM_Customer> customers = new List<tb_CRM_Customer>();
                    foreach (var Customer in CustomerVendors)
                    {

                        tb_CRM_Customer entity = MainForm.Instance.mapper.Map<tb_CRM_Customer>(Customer);
                        entity.PrimaryKeyID= 0;
                        BusinessHelper.Instance.InitEntity(entity);
                        customers.Add(entity);
                    }
                    tb_CRM_CustomerController<tb_CRM_Customer> ctr = Startup.GetFromFac<tb_CRM_CustomerController<tb_CRM_Customer>>();
                    List<long> ids = await ctr.AddAsync(customers);

                    if (ids.Count > 0)
                    {
                        richTextBoxLog.AppendText($"保存成功：{ids.Count}条记录" + "\r\n");
                    }

                    #endregion

                }
                if (treeViewFunction.SelectedNode.Text == "属性重复的SKU检测")
                {
                    //思路是将属性全查出来。将属性按规则排序后比较

                    #region 判断是否有重复的属性值。将属性值添加到列表，按一定规则排序，然后判断是否有重复

                    List<tb_Prod_Attr_Relation> attr_Relations = MainForm.Instance.AppContext.Db.Queryable<tb_Prod_Attr_Relation>()
                        .IncludesAllFirstLayer()
                        .Where(c => c.PropertyValueID.HasValue && c.Property_ID.HasValue)
                        .ToList();
                    //首先将这些数据按品分组

                    //先找到主产品
                    var prodIDs = attr_Relations.GroupBy(c => c.ProdBaseID).Select(c => c.Key).ToList();
                    foreach (var prodID in prodIDs)
                    {
                        #region
                        List<string> DuplicateAttributes = new List<string>();
                        //根据主产品找到SKU详情
                        var prodDetai = attr_Relations.Where(c => c.ProdBaseID.Value == prodID).GroupBy(c => c.ProdDetailID).Select(c => c.Key).ToList();

                        //根据详情找到对应的所有属性值
                        foreach (var detail in prodDetai)
                        {
                            #region 找组合值 按一个顺序串起来加到一个集合再去比较重复
                            string sortedDaString = string.Empty;
                            foreach (var item in attr_Relations.Where(c => c.ProdDetailID.Value == detail))
                            {
                                // da 是一个 string 数组
                                string[] da = attr_Relations
                                .Where(c => c.ProdDetailID == item.ProdDetailID)
                                .ToList()
                                .Select(c => c.tb_prodpropertyvalue.PropertyValueName)
                                .ToArray();
                                // 将 da 转换为排序后的列表
                                List<string> sortedDa = da.OrderBy(x => x).ToList();

                                // 将排序后的列表转换为字符串
                                sortedDaString = string.Join(", ", sortedDa);
                            }
                            // 添加到 DuplicateAttributes 集合中
                            DuplicateAttributes.Add(sortedDaString);
                            #endregion
                        }

                        //这里是这个产品下面的所有SKU对应的属性值的,串起来的集合数量等于SKU的个数
                        // 找出 DuplicateAttributes 中的重复值
                        var duplicates = DuplicateAttributes
                            .GroupBy(s => s)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();

                        if (duplicates.Count > 0)
                        {
                            // 输出重复的值
                            foreach (var dup in duplicates)
                            {
                                richTextBoxLog.AppendText($"产品ID：{prodID}中的属性值重复:" + dup + "\r\n");
                            }
                        }
                        #endregion

                    }

                    #endregion

                }

                if (treeViewFunction.SelectedNode.Text == "佣金数据修复[tb_SaleOrder]")
                {
                    if (treeViewTableList.SelectedNode.Tag != null)
                    {
                        #region 佣金
                        if (treeViewTableList.SelectedNode.Name == typeof(tb_SaleOrder).Name)
                        {
                            List<long> ids = new List<long>();
                            List<tb_SaleOrderDetail> updateDetaillist = new();
                            List<tb_SaleOrderDetail> allDetailList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrderDetail>()
                                .Includes(c => c.tb_saleorder)
                           .Where(c => c.CommissionAmount > 0 && c.UnitCommissionAmount == 0)
                          .ToListAsync();
                            for (int o = 0; o < allDetailList.Count; o++)
                            {
                                var detail = allDetailList[o];
                                detail.UnitCommissionAmount = detail.CommissionAmount / detail.Quantity;
                                updateDetaillist.Add(detail);
                                if (!ids.Contains(detail.SOrder_ID))
                                {
                                    ids.Add(detail.SOrder_ID);
                                }
                            }

                            if (allDetailList.Count > 0)
                            {
                                //修复缴库库明细和等于主表的总数量
                                List<tb_SaleOrder> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                                    .Where(c => ids.Contains(c.SOrder_ID))
                                    .Includes(c => c.tb_SaleOrderDetails)
                                    .ToListAsync();
                                List<tb_SaleOrder> updatelist = new();
                                for (int i = 0; i < yjList.Count; i++)
                                {
                                    yjList[i].TotalCommissionAmount = yjList[i].tb_SaleOrderDetails.Sum(c => c.CommissionAmount);
                                    updatelist.Add(yjList[i]);
                                }
                                int totalamountCounter = 0;
                                int totaldetailcounter = 0;
                                if (!chkTestMode.Checked)
                                {
                                    totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UnitCommissionAmount }).ExecuteCommandAsync();
                                    totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCommissionAmount }).ExecuteCommandAsync();
                                }
                                richTextBoxLog.AppendText($"销售订单佣金数据修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                                richTextBoxLog.AppendText($"销售订单佣金数据修复 主表 修复成功：{totalamountCounter} " + "\r\n");
                            }
                            else
                            {
                                richTextBoxLog.AppendText($"没有需要修复的数据" + "\r\n");
                            }

                        }
                        #endregion
                        else
                        {
                            richTextBoxLog.Clear();
                            richTextBoxLog.AppendText($"请在左边选中要修复的表名：：{typeof(tb_SaleOrder).Name} " + "\r\n");
                        }
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "佣金数据修复[tb_SaleOut]")
                {
                    if (treeViewTableList.SelectedNode.Tag != null)
                    {
                        if (treeViewTableList.SelectedNode.Name == typeof(tb_SaleOut).Name)
                        {
                            #region 佣金
                            List<long> ids = new List<long>();
                            List<tb_SaleOutDetail> updateDetaillist = new();
                            List<tb_SaleOutDetail> allDetailList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutDetail>()
                                .Includes(c => c.tb_saleout)
                           .Where(c => c.CommissionAmount > 0 && c.UnitCommissionAmount == 0)
                          .ToListAsync();
                            for (int o = 0; o < allDetailList.Count; o++)
                            {
                                var detail = allDetailList[o];
                                detail.UnitCommissionAmount = detail.CommissionAmount / detail.Quantity;
                                updateDetaillist.Add(detail);
                                if (!ids.Contains(detail.SaleOut_MainID))
                                {
                                    ids.Add(detail.SaleOut_MainID);
                                    //richTextBoxLog.AppendText($"出库佣金数据修复 出库单号：：{detail.tb_saleout.SaleOutNo} " + "\r\n");
                                }
                            }
                            if (allDetailList.Count > 0)
                            {
                                //修复缴库库明细和等于主表的总数量
                                List<tb_SaleOut> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                    .Where(c => ids.Contains(c.SaleOut_MainID))
                                    .Includes(c => c.tb_SaleOutDetails)
                                    .ToListAsync();
                                List<tb_SaleOut> updatelist = new();
                                for (int i = 0; i < yjList.Count; i++)
                                {
                                    yjList[i].TotalCommissionAmount = yjList[i].tb_SaleOutDetails.Sum(c => c.CommissionAmount);
                                    updatelist.Add(yjList[i]);
                                }
                                int totalamountCounter = 0;
                                int totaldetailcounter = 0;
                                if (!chkTestMode.Checked)
                                {
                                    totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UnitCommissionAmount }).ExecuteCommandAsync();
                                    totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCommissionAmount }).ExecuteCommandAsync();
                                }
                                richTextBoxLog.AppendText($"出库佣金数据修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                                richTextBoxLog.AppendText($"出库佣金数据修复 主表 修复成功：{totalamountCounter} " + "\r\n");
                            }
                            else
                            {
                                richTextBoxLog.AppendText($"没有需要修复的数据" + "\r\n");
                            }
                            #endregion

                        }
                        else
                        {
                            richTextBoxLog.Clear();
                            richTextBoxLog.AppendText($"请在左边选中要修复的表名：：{typeof(tb_SaleOut).Name} " + "\r\n");
                        }
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "生产计划数量修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_FinishedGoodsInv).Name)
                    {
                        //修复缴库库明细和等于主表的总数量
                        List<tb_FinishedGoodsInv> FinishedGoodsInvList = await MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                            .Includes(c => c.tb_FinishedGoodsInvDetails)
                            .ToListAsync();
                        List<tb_FinishedGoodsInv> updatelist = new();
                        for (int i = 0; i < FinishedGoodsInvList.Count; i++)
                        {
                            if (FinishedGoodsInvList[i].TotalQty != FinishedGoodsInvList[i].tb_FinishedGoodsInvDetails.Sum(c => c.Qty))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    FinishedGoodsInvList[i].TotalQty = FinishedGoodsInvList[i].tb_FinishedGoodsInvDetails.Sum(c => c.Qty);
                                    updatelist.Add(FinishedGoodsInvList[i]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"{FinishedGoodsInvList[i]} 等待修复 \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {

                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalQty }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复缴库库明细和等于主表的总数量 修复成功：{totalamountCounter} " + "\r\n");
                        }
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ManufacturingOrder).Name)
                    {

                        //制令单已完成数量要等于=名下所有缴库单数量之和
                        var ManufacturingOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                            .Includes(c => c.tb_FinishedGoodsInvs)
                                     .ToListAsync();

                        List<tb_ManufacturingOrder> updatelist = new();
                        for (int i = 0; i < ManufacturingOrders.Count; i++)
                        {
                            if (ManufacturingOrders[i].QuantityDelivered != ManufacturingOrders[i].tb_FinishedGoodsInvs.Where(c => c.DataStatus == 4).Sum(c => c.TotalQty))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    ManufacturingOrders[i].QuantityDelivered = ManufacturingOrders[i].tb_FinishedGoodsInvs.Where(c => c.DataStatus == 4).Sum(c => c.TotalQty);
                                    updatelist.Add(ManufacturingOrders[i]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"{ManufacturingOrders[i].MONO} 等待修复 \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.QuantityDelivered }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复缴库单数量 修复成功：{totalamountCounter} " + "\r\n");
                        }
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ProductionPlan).Name)
                    {
                        //计划完成数量等于他名下的需求单下的所有制令单完成数量之和
                        var ProductionPlans = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                             .Includes(a => a.tb_ProductionPlanDetails)
                            .Includes(c => c.tb_ProductionDemands, b => b.tb_ManufacturingOrders)
                            .ToListAsync();

                        List<tb_ProductionPlan> updatelist = new();
                        for (int ii = 0; ii < ProductionPlans.Count; ii++)
                        {
                            List<tb_ProductionPlanDetail> updatePlanDetails = new List<tb_ProductionPlanDetail>();
                            for (int jj = 0; jj < ProductionPlans[ii].tb_ProductionPlanDetails.Count; jj++)
                            {
                                int totalqty = 0;
                                for (int kk = 0; kk < ProductionPlans[ii].tb_ProductionDemands.Count; kk++)
                                {
                                    totalqty += ProductionPlans[ii].tb_ProductionDemands[kk].tb_ManufacturingOrders.Where(c => (c.DataStatus == 4 || c.DataStatus == 8) && c.ProdDetailID == ProductionPlans[ii].tb_ProductionPlanDetails[jj].ProdDetailID).Sum(c => c.QuantityDelivered);
                                }
                                if (totalqty == 0)
                                {
                                    if (ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity > 0)
                                    {
                                        richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}计划明细中==>{totalqty}==========0时。明细保存的是{ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity} \r\n");
                                    }
                                    //如果制令单数量为0，则跳过
                                    continue;
                                }
                                if (totalqty != ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity)
                                {
                                    if (!chkTestMode.Checked)
                                    {

                                        ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity = totalqty;
                                        updatePlanDetails.Add(ProductionPlans[ii].tb_ProductionPlanDetails[jj]);
                                    }
                                    else
                                    {
                                        richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}计划明细{ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity}==>{totalqty} 等待修复！！！！！ \r\n");
                                    }
                                }

                            }

                            if (!chkTestMode.Checked)
                            {
                                int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatePlanDetails).UpdateColumns(t => new { t.CompletedQuantity }).ExecuteCommandAsync();
                                richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}修复计划明细数量 修复成功：{totalamountCounter} " + "\r\n");
                            }



                            if (ProductionPlans[ii].TotalCompletedQuantity != ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    ProductionPlans[ii].TotalCompletedQuantity = ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity);
                                    updatelist.Add(ProductionPlans[ii]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"PPNo:{ProductionPlans[ii].PPNo},{ProductionPlans[ii].TotalCompletedQuantity} 等待修复 为{ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity)} \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCompletedQuantity }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复生产计划总数量 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                }
                if (treeViewFunction.SelectedNode.Text == "采购订单未交数量修复")
                {
                    List<tb_PurOrderDetail> updateDetaillist = new();
                    List<tb_PurOrder> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                        .Includes(c => c.tb_PurOrderDetails)
                        .ToListAsync();

                    List<tb_PurOrder> updatelist = new();
                    for (int i = 0; i < yjList.Count; i++)
                    {
                        for (int j = 0; j < yjList[i].tb_PurOrderDetails.Count; j++)
                        {
                            yjList[i].tb_PurOrderDetails[j].UndeliveredQty = yjList[i].tb_PurOrderDetails[j].Quantity - yjList[i].tb_PurOrderDetails[j].DeliveredQuantity;
                            if (yjList[i].tb_PurOrderDetails[j].UndeliveredQty > 0)
                            {
                                updateDetaillist.Add(yjList[i].tb_PurOrderDetails[j]);
                            }
                        }

                        if (yjList[i].TotalUndeliveredQty != yjList[i].tb_PurOrderDetails.Sum(c => c.UndeliveredQty))
                        {
                            yjList[i].TotalUndeliveredQty = yjList[i].tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
                            updatelist.Add(yjList[i]);
                        }

                    }

                    int totalmasterCounter = 0;
                    int totaldetailcounter = 0;
                    if (!chkTestMode.Checked)
                    {
                        totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UndeliveredQty }).ExecuteCommandAsync();
                        totalmasterCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalUndeliveredQty }).ExecuteCommandAsync();
                    }
                    richTextBoxLog.AppendText($"采购订单未交数量修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                    richTextBoxLog.AppendText($"采购订单未交数量修复 主表 修复成功：{totalmasterCounter} " + "\r\n");
                }

                if (treeViewFunction.SelectedNode.Text == "制令单自制品修复")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ManufacturingOrder).Name)
                    {
                        List<tb_ManufacturingOrder> ManufacturingOrderList = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                           .Includes(c => c.tb_productiondemand, b => b.tb_ProduceGoodsRecommendDetails)
                           .ToListAsync();

                        List<tb_ManufacturingOrder> MoUpdatelist = new();
                        foreach (tb_ManufacturingOrder ManufacturingOrder in ManufacturingOrderList)
                        {
                            if (!ManufacturingOrder.PDCID.HasValue)
                            {
                                if (!chkTestMode.Checked)
                                {
                                    var prddetail = ManufacturingOrder.tb_productiondemand.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ProdDetailID == ManufacturingOrder.ProdDetailID);
                                    if (prddetail == null)
                                    {
                                        continue;
                                    }
                                    ManufacturingOrder.PDCID = prddetail.PDCID;
                                    MoUpdatelist.Add(ManufacturingOrder);
                                    richTextBoxLog.AppendText($"PDCID 为空， 成功修复为 {prddetail.PDCID} \r\n");
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"PDCID 为空， 等待修复为  \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(MoUpdatelist).UpdateColumns(t => new { t.PDCID }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"制令单 PDCID总数量 修复成功：{totalamountCounter} " + "\r\n");
                        }
                    }
                }

                if (treeViewFunction.SelectedNode.Text == "清空财务数据")
                {
                    //核销表 付款表  应收付表 预收付表
                    var StatementController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
                    var StatementList = StatementController.QueryByNav(c => c.StatementId > 0);
                    richTextBoxLog.AppendText($"即将清空财务数据 核销表：{StatementList.Count} " + "\r\n");

                    var PaymentRecordController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    var PaymentRecordList = PaymentRecordController.QueryByNav(c => c.PaymentId > 0);
                    richTextBoxLog.AppendText($"即将清空财务数据 付款表：{PaymentRecordList.Count} " + "\r\n");

                    var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                    var ReceivablePayableList = ReceivablePayableController.QueryByNav(c => c.ARAPId > 0);
                    richTextBoxLog.AppendText($"即将清空财务数据 应收付表：{ReceivablePayableList.Count} " + "\r\n");

                    var PreReceivedPaymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    var PreReceivedPaymentList = PreReceivedPaymentController.QueryByNav(c => c.PreRPID > 0);
                    richTextBoxLog.AppendText($"即将清空财务数据 预收付表：{PreReceivedPaymentList.Count} " + "\r\n");

                    int totalamountCounter = 0;
                    //销售出库 销售订单
                    if (!chkTestMode.Checked)
                    {
                        totalamountCounter += StatementList.Count;
                        totalamountCounter += PaymentRecordList.Count;
                        totalamountCounter += ReceivablePayableList.Count;
                        totalamountCounter += PreReceivedPaymentList.Count;
                        for (int i = 0; i < StatementList.Count; i++)
                        {
                            await StatementController.BaseDeleteByNavAsync(StatementList[i]);
                        }

                        for (int i = 0; i < PaymentRecordList.Count; i++)
                        {
                            await PaymentRecordController.BaseDeleteByNavAsync(PaymentRecordList[i]);
                        }

                        for (int i = 0; i < ReceivablePayableList.Count; i++)
                        {
                            await ReceivablePayableController.BaseDeleteByNavAsync(ReceivablePayableList[i]);
                        }

                        for (int i = 0; i < PreReceivedPaymentList.Count; i++)
                        {
                            await PreReceivedPaymentController.BaseDeleteByNavAsync(PreReceivedPaymentList[i]);
                        }


                        richTextBoxLog.AppendText($"清空财务数据总数量：{totalamountCounter} " + "\r\n");
                    }
                }


            }
        }

        private async Task<List<tb_Inventory>> CostFix(bool updateAllRows = false, string SKU = "", long ProdDetailID = 0)
        {
            List<tb_Inventory> Allitems = new List<tb_Inventory>();
            try
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.BeginTran();
                }
                //成本修复思路
                //1）成本本身修复，将所有入库明细按加权平均算一下。更新到库存里面。
                //2）修复所有出库明细，主要是销售出库，当然还有其它，比方借出，成本金额是重要的指标数据
                //3）成本修复 分  成品 外采和生产  因为这两种成本产生的方式不一样
                #region 成本本身修复
                Allitems = MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                           .AsNavQueryable()
                           .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails, e => e.tb_proddetail, f => f.tb_prod)
                           .Includes(c => c.tb_proddetail, d => d.tb_prod)
                           .WhereIF(!string.IsNullOrEmpty(SKU), c => c.tb_proddetail.SKU == SKU)
                            .WhereIF(ProdDetailID > 0, c => c.ProdDetailID == ProdDetailID)
                           // .IFWhere(c => c.tb_proddetail.SKU == "SKU7E881B4629")
                           .ToList();

                List<tb_Inventory> updateInvList = new List<tb_Inventory>();
                foreach (tb_Inventory item in Allitems)
                {
                    if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.UnitPrice) > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                        )
                    {
                        //参与成本计算的入库明细记录。要排除单价为0的项
                        var realDetails = item.tb_proddetail.tb_PurEntryDetails.Where(c => c.UnitPrice > 0).ToList();

                        //每笔的入库的数量*成交价/总数量
                        var transPrice = realDetails
                            .Where(c => c.Quantity > 0 && c.UnitPrice > 0)
                            .Sum(c => c.UnitPrice * c.Quantity) / realDetails.Sum(c => c.Quantity);
                        if (transPrice > 0)
                        {
                            //百分比
                            decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                            diffpirce = Math.Round(diffpirce, 2);
                            double percentDiff = ComparePrice(item.Inv_Cost.ToDouble(), transPrice.ToDouble());
                            if (percentDiff > 10)
                            {
                                richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                item.CostMovingWA = transPrice;
                                item.Inv_AdvCost = item.CostMovingWA;
                                item.Inv_Cost = item.CostMovingWA;
                                item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;

                                updateInvList.Add(item);
                            }
                        }
                    }
                }
                if (chkTestMode.Checked)
                {
                    richTextBoxLog.AppendText($"要修复的行数为:{Allitems.Count}" + "\r\n");
                }
                if (!chkTestMode.Checked)
                {
                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updateInvList).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                    richTextBoxLog.AppendText($"修复成本价格成功：{totalamountCounter} " + "\r\n");
                }
                #endregion

                if (updateAllRows)
                {
                    #region 更新相关数据

                    #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                    foreach (var child in updateInvList)
                    {

                        List<tb_BOM_S> orders = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                    .InnerJoin<tb_BOM_SDetail>((a, b) => a.BOM_ID == b.BOM_ID)
                    .Includes(a => a.tb_BOM_SDetails)
                    .Where(a => a.tb_BOM_SDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.BOM_ID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_BOM_SDetail> updateListbomdetail = new List<tb_BOM_SDetail>();
                        foreach (var bill in distinctbills)
                        {

                            foreach (var bomDetail in bill.tb_BOM_SDetails)
                            {
                                if (bomDetail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(bomDetail.UnitCost - child.Inv_Cost);
                                    if (diffpirce > 0.2m)
                                    {
                                        bomDetail.UnitCost = child.Inv_Cost;
                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                        updateListbomdetail.Add(bomDetail);
                                    }
                                }
                            }

                            if (updateListbomdetail.Count > 0)
                            {
                                bill.TotalMaterialCost = bill.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                                bill.OutProductionAllCosts = bill.TotalMaterialCost + bill.TotalOutManuCost + bill.OutApportionedCost;
                                bill.SelfProductionAllCosts = bill.TotalMaterialCost + bill.TotalSelfManuCost + bill.SelfApportionedCost;
                                if (!chkTestMode.Checked)
                                {
                                    await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_S>(bill).ExecuteCommandAsync();
                                }
                            }
                        }

                        if (!chkTestMode.Checked && updateListbomdetail.Count > 0)
                        {
                            await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_SDetail>(updateListbomdetail).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    #region 更新制令单价格,和BOM类似

                    foreach (var child in updateInvList)
                    {

                        List<tb_ManufacturingOrder> orders = MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                    .InnerJoin<tb_ManufacturingOrderDetail>((a, b) => a.MOID == b.MOID)
                    .Includes(a => a.tb_ManufacturingOrderDetails)
                    .Where(a => a.tb_ManufacturingOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.MOID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_ManufacturingOrderDetail> updateListdetail = new List<tb_ManufacturingOrderDetail>();
                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_ManufacturingOrderDetail Detail in bill.tb_ManufacturingOrderDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                    {
                                        Detail.UnitCost = child.Inv_Cost;
                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }

                            bill.TotalMaterialCost = bill.tb_ManufacturingOrderDetails.Sum(c => c.SubtotalUnitCost);
                            bill.TotalProductionCost = bill.TotalMaterialCost + bill.ApportionedCost + bill.TotalManuFee;
                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrder>(bill).ExecuteCommandAsync();
                            }
                        }
                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrderDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }

                    #endregion

                    #region 更新缴库单价格,和BOM类似,  要再计算缴款的成品的成本 再反向更新库存的成本 这种一般是有BOM的

                    foreach (var child in updateInvList)
                    {
                        List<tb_FinishedGoodsInv> orders = MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                        .InnerJoin<tb_FinishedGoodsInvDetail>((a, b) => a.FG_ID == b.FG_ID)
                        .Includes(a => a.tb_FinishedGoodsInvDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                        .Where(a => a.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.FG_ID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_FinishedGoodsInvDetail> updateListdetail = new List<tb_FinishedGoodsInvDetail>();

                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_FinishedGoodsInvDetail Detail in bill.tb_FinishedGoodsInvDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                    {
                                        Detail.MaterialCost = child.Inv_Cost;
                                        Detail.UnitCost = Detail.MaterialCost * Detail.ManuFee + Detail.ApportionedCost;
                                        Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                        //这时可以算出缴库的产品的单位成本
                                        var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                        if (nextInv != null)
                                        {
                                            nextInv.Inv_Cost = Detail.UnitCost;
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                            }
                                        }

                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }

                            bill.TotalMaterialCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.MaterialCost * c.Qty);
                            bill.TotalManuFee = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ManuFee * c.Qty);
                            bill.TotalApportionedCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ApportionedCost * c.Qty);
                            bill.TotalProductionCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ProductionAllCost);
                            //又进入下一轮更新了
                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInv>(bill).ExecuteCommandAsync();
                            }
                        }
                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInvDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }

                    #endregion


                    #region 销售订单 出库  退货 记录成本修复
                    foreach (var child in updateInvList)
                    {

                        List<tb_SaleOrder> orders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                         .InnerJoin<tb_SaleOrderDetail>((a, b) => a.SOrder_ID == b.SOrder_ID)
                        .Includes(a => a.tb_SaleOrderDetails)
                        .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutDetails)
                        .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                        .Where(a => a.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctOrders = orders
                        .GroupBy(o => o.SOrder_ID)
                        .Select(g => g.First())
                        .ToList();

                        richTextBoxLog.AppendText($"找到销售订单 {distinctOrders.Count} 条" + "\r\n");
                        foreach (var order in distinctOrders)
                        {
                            #region new

                            foreach (var Detail in order.tb_SaleOrderDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                        if (Detail.TaxRate > 0)
                                        {
                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                        }
                                    }
                                }
                            }
                            order.TotalCost = order.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount);
                            order.TotalAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + order.FreightIncome;
                            order.TotalQty = order.tb_SaleOrderDetails.Sum(c => c.Quantity);
                            order.TotalTaxAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                            richTextBoxLog.AppendText($"销售订单{order.SOrderNo}总金额：{order.TotalCost} " + "\r\n");

                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                            }
                            #region 销售出库
                            if (order.tb_SaleOuts != null)
                            {
                                foreach (var SaleOut in order.tb_SaleOuts)
                                {
                                    foreach (var saleoutdetails in SaleOut.tb_SaleOutDetails)
                                    {
                                        if (saleoutdetails.ProdDetailID == child.ProdDetailID)
                                        {
                                            saleoutdetails.Cost = child.Inv_Cost;
                                            saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                            saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                            if (saleoutdetails.TaxRate > 0)
                                            {
                                                saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                            }
                                        }
                                    }
                                    SaleOut.TotalCost = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + SaleOut.FreightCost;
                                    SaleOut.TotalAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount) + SaleOut.FreightIncome;
                                    SaleOut.TotalQty = SaleOut.tb_SaleOutDetails.Sum(c => c.Quantity);
                                    SaleOut.TotalTaxAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);

                                    richTextBoxLog.AppendText($"销售出库{SaleOut.SaleOutNo}总金额：{SaleOut.TotalCost} " + "\r\n");
                                    #region 销售退回
                                    if (SaleOut.tb_SaleOutRes != null)
                                    {
                                        foreach (var SaleOutRe in SaleOut.tb_SaleOutRes)
                                        {
                                            foreach (var SaleOutReDetail in SaleOutRe.tb_SaleOutReDetails)
                                            {
                                                if (SaleOutReDetail.ProdDetailID == child.ProdDetailID)
                                                {
                                                    SaleOutReDetail.Cost = child.Inv_Cost;
                                                    SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                    SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                    if (SaleOutReDetail.TaxRate > 0)
                                                    {
                                                        SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                    }
                                                    SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                }
                                            }
                                            SaleOutRe.TotalAmount = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalTransAmount);
                                            SaleOutRe.TotalQty = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.Quantity);

                                            if (SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails != null)
                                            {
                                                foreach (var Refurbished in SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails)
                                                {
                                                    if (Refurbished.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        Refurbished.Cost = child.Inv_Cost;
                                                        Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                    }
                                                }
                                            }


                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutRe>(SaleOutRe).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReDetail>(SaleOutRe.tb_SaleOutReDetails).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReRefurbishedMaterialsDetail>(SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails).ExecuteCommandAsync();
                                            }
                                            richTextBoxLog.AppendText($"销售退回{SaleOutRe.ReturnNo}总金额：{SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount)} " + "\r\n");
                                        }

                                    }

                                    #endregion

                                    if (!chkTestMode.Checked)
                                    {
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(SaleOut).ExecuteCommandAsync();
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(SaleOut.tb_SaleOutDetails).ExecuteCommandAsync();
                                    }
                                }

                            }
                            #endregion
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                            }
                            #endregion
                        }

                    }
                    #endregion


                    #region 借出单 归还
                    foreach (var child in updateInvList)
                    {
                        List<tb_ProdBorrowing> orders = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                        .InnerJoin<tb_ProdBorrowingDetail>((a, b) => a.BorrowID == b.BorrowID)
                       .Includes(a => a.tb_ProdBorrowingDetails)
                       .Includes(a => a.tb_ProdReturnings, c => c.tb_ProdReturningDetails)
                       .Where(a => a.tb_ProdBorrowingDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctbills = orders
                        .GroupBy(o => o.BorrowID)
                        .Select(g => g.First())
                        .ToList();
                        List<tb_ProdBorrowingDetail> updateListdetail = new List<tb_ProdBorrowingDetail>();
                        List<tb_ProdBorrowing> updateListMain = new List<tb_ProdBorrowing>();
                        foreach (var bill in distinctbills)
                        {
                            bool needupdate = false;
                            foreach (var Detail in bill.tb_ProdBorrowingDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                        updateListdetail.Add(Detail);
                                        needupdate = true;
                                    }
                                }
                            }

                            if (needupdate)
                            {
                                bill.TotalCost = bill.tb_ProdBorrowingDetails.Sum(c => c.SubtotalCostAmount);
                                updateListMain.Add(bill);
                            }

                            #region 归还单
                            if (bill.tb_ProdReturnings != null)
                            {
                                foreach (var borrow in bill.tb_ProdReturnings)
                                {
                                    foreach (var returning in borrow.tb_ProdReturningDetails)
                                    {
                                        if (returning.ProdDetailID == child.ProdDetailID)
                                        {
                                            returning.Cost = child.Inv_Cost;
                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                        }
                                    }
                                    borrow.TotalCost = borrow.tb_ProdReturningDetails.Sum(c => c.SubtotalCostAmount);

                                    if (!chkTestMode.Checked)
                                    {
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(borrow).ExecuteCommandAsync();
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(borrow.tb_ProdReturningDetails).ExecuteCommandAsync();
                                    }
                                }
                            }
                            #endregion
                        }
                        if (!chkTestMode.Checked)
                        {
                            await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(updateListMain).ExecuteCommandAsync();
                            await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(updateListdetail).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    #region 其它出库
                    foreach (var child in updateInvList)
                    {

                        List<tb_StockOut> orders = MainForm.Instance.AppContext.Db.Queryable<tb_StockOut>()
                        .InnerJoin<tb_StockOutDetail>((a, b) => a.MainID == b.MainID)
                        .Includes(a => a.tb_StockOutDetails)
                        .Where(a => a.tb_StockOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctbills = orders
                        .GroupBy(o => o.MainID)
                        .Select(g => g.First())
                        .ToList();

                        foreach (var bill in distinctbills)
                        {
                            List<tb_StockOutDetail> updateListdetail = new List<tb_StockOutDetail>();
                            foreach (tb_StockOutDetail Detail in bill.tb_StockOutDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                        updateListdetail.Add(Detail);
                                    }
                                }

                            }
                            bill.TotalCost = bill.tb_StockOutDetails.Sum(c => c.SubtotalCostAmount);
                            if (updateListdetail.Count > 0)
                            {
                                if (!chkTestMode.Checked)
                                {
                                    await MainForm.Instance.AppContext.Db.Updateable<tb_StockOutDetail>(updateListdetail).ExecuteCommandAsync();
                                }
                            }

                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_StockOut>(bill).ExecuteCommandAsync();
                            }

                        }
                    }
                    #endregion


                    #region 领料单

                    foreach (var child in updateInvList)
                    {
                        List<tb_MaterialRequisition> orders = MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                      .InnerJoin<tb_MaterialRequisitionDetail>((a, b) => a.MR_ID == b.MR_ID)
                      .Includes(a => a.tb_MaterialRequisitionDetails)
                      .Where(a => a.tb_MaterialRequisitionDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();
                        var distinctbills = orders
                        .GroupBy(o => o.MR_ID)
                        .Select(g => g.First())
                        .ToList();
                        List<tb_MaterialRequisitionDetail> updateListdetail = new List<tb_MaterialRequisitionDetail>();
                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_MaterialRequisitionDetail Detail in bill.tb_MaterialRequisitionDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }
                            if (updateListdetail.Count > 0)
                            {
                                bill.TotalCost = bill.tb_MaterialRequisitionDetails.Sum(c => c.SubtotalCost);
                            }
                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisition>(bill).ExecuteCommandAsync();
                            }
                        }

                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisitionDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }
                    #endregion


                    #endregion

                }



                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.CommitTran();
                }

            }


            catch (Exception ex)
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }
                throw ex;
            }
            return Allitems;
        }


        //写一个方法来实现两个价格的比较 前一个为原价，后一个为最新价格。
        //求最新价格大于前的价格的百分比。价格是decimal类型
        private double ComparePrice(double oldPrice, double newPrice)
        {
            if (oldPrice < 0 || newPrice < 0)
            {
                //如果有负 直接要求更新
                return 100;
                //throw new ArgumentException("Prices cannot be negative.");
            }

            if (oldPrice == 0)
            {
                // 如果原价为 0，无法计算百分比增长
                // 根据需求返回 -1 或 throw 异常
                return -1; // 或者抛出定制异常
            }
            double diffpirce = Math.Abs(newPrice - oldPrice);
            double percentage = (diffpirce / oldPrice * 100);
            return Math.Round(percentage, 2); // 四舍五入到 2 位小数
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {


        }

        private void LoadTree()
        {
            Assembly dalAssemble = AssemblyLoader.LoadAssembly("RUINORERP.Model");
            ModelTypes = dalAssemble.GetExportedTypes();
            typeNames = ModelTypes.Select(m => m.Name).ToList();
            treeViewTableList.Nodes.Clear();
            stlist.Clear();
            foreach (var type in ModelTypes)
            {
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable st)
                    {
                        //var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                        //var t = Startup.ServiceProvider.CreateInstance(type);//SugarColumn 或进一步取字段特性也可以
                        if (st.TableName.Contains("tb_") && !type.Name.Contains("QueryDto"))
                        {
                            //if (txtTableName.Text.Trim().Length > 0 && st.TableName.Contains(txtTableName.Text.Trim()))
                            //{
                            if (st.TableName.Contains("tb_MenuInfo") || st.TableName == "tb_MenuInfo")
                            {

                            }
                            TreeNode node = new TreeNode(st.TableName);
                            node.Name = st.TableName;
                            node.Tag = type;
                            treeViewTableList.Nodes.Add(node);
                            stlist.Add(st);
                            //}
                        }
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse && sender is TreeView tv)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
            //TreeViewSingleSelectedAndChecked(TreeView1, e);
            e.Node.Checked = true;
            node_AfterCheck(sender, e);
            
            // 显示选中修复项的详细信息
            DisplayCorrectionItemDetails(e.Node.Text);
        }
        
        /// <summary>
        /// 显示修复项的详细信息
        /// </summary>
        /// <param name="nodeText">节点文本</param>
        private void DisplayCorrectionItemDetails(string nodeText)
        {
            _currentCorrectionItem = nodeText;
            richTextBoxLog.Clear();
            
            if (_correctionItemsMetadata.TryGetValue(nodeText, out var metadata))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("========================================");
                sb.AppendLine($"功能名称：{metadata.FunctionName}");
                sb.AppendLine("========================================");
                sb.AppendLine();
                sb.AppendLine("【问题描述】");
                sb.AppendLine(metadata.ProblemDescription);
                sb.AppendLine();
                sb.AppendLine("【影响表清单】");
                foreach (var table in metadata.AffectedTables)
                {
                    sb.AppendLine($"  - {table}");
                }
                sb.AppendLine();
                sb.AppendLine("【修复逻辑】");
                sb.AppendLine(metadata.FixLogic);
                sb.AppendLine();
                sb.AppendLine("【发生情形】");
                sb.AppendLine(metadata.OccurrenceScenario);
                sb.AppendLine();
                sb.AppendLine("========================================");
                sb.AppendLine("操作提示：");
                sb.AppendLine("1. 点击【预览】按钮查看将要修改的数据");
                sb.AppendLine("2. 确认数据无误后，点击【执行修复】按钮");
                sb.AppendLine("3. 建议先在测试模式（勾选'测试模式'）下验证");
                sb.AppendLine("========================================");
                
                richTextBoxLog.AppendText(sb.ToString());
                
                // ✅ 动态生成查询条件UI
                GenerateQueryConditionUI(nodeText);
            }
            else
            {
                richTextBoxLog.AppendText($"未找到 '{nodeText}' 的详细说明。\r\n");
                richTextBoxLog.AppendText("请确保该修复项已配置元数据信息。\r\n");
                
                // 清空查询条件面板
                kryptonPanelQuery.Controls.Clear();
            }
        }



        void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // only do it if the node became checked:
            if (e.Node.Checked)
            {
                // for all the nodes in the tree...
                foreach (TreeNode cur_node in e.Node.TreeView.Nodes)
                {
                    // ... which are not the freshly checked one...
                    if (cur_node != e.Node)
                    {
                        // ... uncheck them
                        cur_node.Checked = false;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="e"></param>
        public static void TreeViewSingleSelectedAndChecked(Krypton.Toolkit.KryptonTreeView tv, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);

            }
        }

        private async void 执行选中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewFunction.SelectedNode.Text == "成本修复" && dataGridView1.CurrentRow != null
                && dataGridView1.CurrentRow.DataBoundItem is tb_Inventory inventory)
            {
                await CostFix(true, string.Empty, inventory.ProdDetailID);
            }
        }

       
        
        /// <summary>
        /// 预览按钮点击事件 - 显示将要修改的数据
        /// </summary>
        private async void btnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCorrectionItem))
            {
                MessageBox.Show("请先选择一个数据修复项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 获取对应的服务
            var service = DataCorrectionServiceManager.GetService(_currentCorrectionItem);
            if (service == null)
            {
                richTextBoxLog.AppendText($"未找到修复服务：{_currentCorrectionItem}\r\n");
                richTextBoxLog.AppendText("请使用旧的方式执行修复。\r\n");
                return;
            }
            
            richTextBoxLog.Clear();
            richTextBoxLog.AppendText($"正在预览【{service.FunctionName}】...\r\n\r\n");
            Application.DoEvents();
            
            try
            {
                // ✅ 从动态生成的UI读取查询参数
                var queryParameters = GetQueryParameters();
                
                if (queryParameters.Count > 0)
                {
                    richTextBoxLog.AppendText($"🔍 应用查询条件：\r\n");
                    foreach (var kvp in queryParameters)
                    {
                        richTextBoxLog.AppendText($"   {kvp.Key} = {kvp.Value}\r\n");
                    }
                    richTextBoxLog.AppendText($"\r\n");
                }
                
                // 调用服务的预览方法，传入查询参数
                var previewResults = await service.PreviewAsync(queryParameters);
                
                // ✅ 清空之前的数据
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                
                // ✅ 如果只有一个表，直接显示（原有逻辑）
                if (previewResults.Count == 1)
                {
                    var previewResult = previewResults[0];
                    richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n");
                    richTextBoxLog.AppendText($"表名：{previewResult.TableName}\r\n");
                    richTextBoxLog.AppendText($"说明：{previewResult.Description}\r\n");
                    richTextBoxLog.AppendText($"总记录数：{previewResult.TotalCount}\r\n");
                    richTextBoxLog.AppendText($"需要修复：{previewResult.NeedFixCount}\r\n");
                    richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n\r\n");
                    
                    if (previewResult.Data != null)
                    {
                        // 创建带复选框和行号的DataTable
                        var dtWithCheckbox = AddCheckboxColumn(previewResult.Data);
                        dataGridView1.DataSource = dtWithCheckbox;
                        
                        // 设置复选框列为第一列
                        if (dataGridView1.Columns.Count > 0)
                        {
                            dataGridView1.Columns[0].Width = 50;
                            dataGridView1.Columns[0].HeaderText = "选择";
                        }
                    }
                }
                // ✅ 如果有多个表，使用TabControl展示（主子表场景）
                else if (previewResults.Count > 1)
                {
                    ShowMultiTablePreview(previewResults);
                }
                
                richTextBoxLog.AppendText($"\r\n共预览 {previewResults.Count} 个表的数据。\r\n");
                richTextBoxLog.AppendText("请勾选需要修复的行，然后点击【执行修复】按钮。\r\n");
                richTextBoxLog.AppendText("提示：不勾选则默认修复所有数据。\r\n");
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText($"预览失败：{ex.Message}\r\n");
                richTextBoxLog.AppendText($"详细信息：{ex.StackTrace}\r\n");
            }
        }
        
        /// <summary>
        /// 为DataTable添加复选框列
        /// </summary>
        private DataTable AddCheckboxColumn(DataTable originalDt)
        {
            var dt = originalDt.Copy();
            
            // 在第一列插入复选框列
            var checkboxCol = new DataColumn("_Selected", typeof(bool));
            checkboxCol.DefaultValue = false;
            dt.Columns.Add(checkboxCol);
            
            // 将复选框列移到第一列
            checkboxCol.SetOrdinal(0);
            
            return dt;
        }
        
        /// <summary>
        /// DataGridView行绘制事件 - 显示行号
        /// </summary>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 绘制行号
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        
        /// <summary>
        /// 显示多表预览（使用TabControl）
        /// </summary>
        private void ShowMultiTablePreview(List<DataPreviewResult> previewResults)
        {
            // ✅ 清空splitContainer1.Panel2中的内容
            splitContainerMain.Panel2.Controls.Clear();
            
            // ✅ 创建TabControl
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Name = "tabControlPreview"
            };
            
            // ✅ 为每个表创建一个TabPage
            foreach (var previewResult in previewResults)
            {
                var tabPage = new TabPage
                {
                    Text = $"{previewResult.TableName} ({previewResult.TotalCount}条)",
                    Name = $"tab_{previewResult.TableName}"
                };
                
                // 在日志中显示统计信息
                richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n");
                richTextBoxLog.AppendText($"表名：{previewResult.TableName}\r\n");
                richTextBoxLog.AppendText($"说明：{previewResult.Description}\r\n");
                richTextBoxLog.AppendText($"总记录数：{previewResult.TotalCount}\r\n");
                richTextBoxLog.AppendText($"需要修复：{previewResult.NeedFixCount}\r\n");
                richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n\r\n");
                
                if (previewResult.Data != null && previewResult.Data.Rows.Count > 0)
                {
                    // ✅ 创建带复选框的DataTable
                    var dtWithCheckbox = AddCheckboxColumn(previewResult.Data);
                    
                    // ✅ 创建DataGridView
                    var dgv = new DataGridView
                    {
                        DataSource = dtWithCheckbox,
                        Dock = DockStyle.Fill,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        AllowUserToAddRows = false,
                        AllowUserToDeleteRows = false,
                        ReadOnly = false,
                        RowHeadersVisible = true,  // ✅ 显示行头（用于绘制行号）
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
                    };
                    
                    // ✅ 注册RowPostPaint事件以显示行号
                    dgv.RowPostPaint += dataGridView1_RowPostPaint;
                    
                    // ✅ 设置复选框列宽度
                    if (dgv.Columns.Count > 0)
                    {
                        dgv.Columns[0].Width = 50;
                        dgv.Columns[0].HeaderText = "选择";
                    }
                    
                    tabPage.Controls.Add(dgv);
                }
                else
                {
                    var lblEmpty = new Label
                    {
                        Text = "没有数据",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font(this.Font, FontStyle.Italic)
                    };
                    tabPage.Controls.Add(lblEmpty);
                }
                
                tabControl.TabPages.Add(tabPage);
            }
            
            // ✅ 将TabControl添加到Panel2
            splitContainerMain.Panel2.Controls.Add(tabControl);
            
            // ✅ 默认选中第一个Tab
            if (tabControl.TabPages.Count > 0)
            {
                tabControl.SelectedIndex = 0;
            }
        }
        
        /// <summary>
        /// 执行修复按钮点击事件
        /// </summary>
        private async void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCorrectionItem))
            {
                MessageBox.Show("请先选择一个数据修复项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 获取对应的服务
            var service = DataCorrectionServiceManager.GetService(_currentCorrectionItem);
            if (service == null)
            {
                MessageBox.Show($"未找到修复服务：{_currentCorrectionItem}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // 获取用户选中的行ID
            var selectedIds = GetSelectedRowIds();
            string selectInfo = "";
            if (selectedIds != null && selectedIds.Count > 0)
            {
                selectInfo = $"\r\n已选中 {selectedIds.Count} 条记录进行修复。";
            }
            else
            {
                selectInfo = "\r\n未选中任何记录，将修复所有预览数据。";
            }
            
            // 二次确认
            DialogResult result = MessageBox.Show(
                $"确定要执行【{service.FunctionName}】吗？{selectInfo}\r\n\r\n"
                + $"{(chkTestMode.Checked ? "当前为测试模式，不会真正修改数据。" : "⚠️ 警告：当前为非测试模式，将真正修改数据库！")}\r\n"
                + $"建议先在测试模式下验证结果。",
                "确认执行",
                MessageBoxButtons.YesNo,
                chkTestMode.Checked ? MessageBoxIcon.Question : MessageBoxIcon.Warning);
            
            if (result != DialogResult.Yes)
                return;
            
            richTextBoxLog.Clear();
            richTextBoxLog.AppendText($"开始执行【{service.FunctionName}】...\r\n");
            richTextBoxLog.AppendText($"时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n");
            richTextBoxLog.AppendText($"模式：{(chkTestMode.Checked ? "测试模式" : "正式模式")}\r\n");
            if (selectedIds != null && selectedIds.Count > 0)
            {
                richTextBoxLog.AppendText($"选中记录数：{selectedIds.Count}\r\n");
            }
            else
            {
                richTextBoxLog.AppendText("选中记录数：全部\r\n");
            }
            richTextBoxLog.AppendText($"\r\n");
            Application.DoEvents();
            
            try
            {
                // 构建参数，包含选中的ID列表
                var parameters = new Dictionary<string, object>();
                if (selectedIds != null && selectedIds.Count > 0)
                {
                    parameters["SelectedIds"] = selectedIds;
                }
                
                // 调用服务的执行方法
                var executionResult = await service.ExecuteAsync(chkTestMode.Checked, parameters);
                
                // 显示执行结果
                richTextBoxLog.AppendText($"\r\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n");
                richTextBoxLog.AppendText($"执行结果：{(executionResult.Success ? "✅ 成功" : "❌ 失败")}\r\n");
                richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n\r\n");
                
                richTextBoxLog.AppendText($"影响表数：{executionResult.AffectedTables.Count}\r\n");
                foreach (var kvp in executionResult.AffectedRows)
                {
                    richTextBoxLog.AppendText($"  - {kvp.Key}: {kvp.Value} 条记录\r\n");
                }
                
                richTextBoxLog.AppendText($"\r\n详细日志：\r\n");
                richTextBoxLog.AppendText($"----------------------------------------\r\n");
                foreach (var log in executionResult.Logs)
                {
                    richTextBoxLog.AppendText($"{log}\r\n");
                }
                
                if (!string.IsNullOrEmpty(executionResult.ErrorMessage))
                {
                    richTextBoxLog.AppendText($"\r\n❌ 错误信息：{executionResult.ErrorMessage}\r\n");
                }
                
                richTextBoxLog.AppendText($"\r\n⏱ 耗时：{executionResult.ElapsedMilliseconds}ms\r\n");
                richTextBoxLog.AppendText($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n");
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText($"\r\n❌ 执行失败：{ex.Message}\r\n");
                richTextBoxLog.AppendText($"详细信息：{ex.StackTrace}\r\n");
            }
        }
        
        /// <summary>
        /// 获取用户选中的行ID列表（支持单表和多表场景）
        /// </summary>
        private List<long> GetSelectedRowIds()
        {
            var selectedIds = new List<long>();
            
            // ✅ 检查是否是多表TabControl场景
            if (splitContainerMain.Panel2.Controls.Count > 0 && 
                splitContainerMain.Panel2.Controls[0] is TabControl tabControl)
            {
                // ✅ 多表场景：遍历所有TabPage中的DataGridView
                bool hasAnyChecked = false;
                
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    foreach (Control ctrl in tabPage.Controls)
                    {
                        if (ctrl is DataGridView dgv && dgv.DataSource is DataTable dt)
                        {
                            if (!dt.Columns.Contains("_Selected"))
                                continue;
                            
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["_Selected"] != DBNull.Value && Convert.ToBoolean(row["_Selected"]))
                                {
                                    hasAnyChecked = true;
                                    long id = ExtractRowId(row);
                                    if (id > 0)
                                    {
                                        selectedIds.Add(id);
                                    }
                                }
                            }
                        }
                    }
                }
                
                // 如果没有勾选任何行，返回null表示全选
                return hasAnyChecked ? (selectedIds.Count > 0 ? selectedIds : null) : null;
            }
            // ✅ 单表场景：使用原有的dataGridView1
            else if (dataGridView1.DataSource is DataTable dt)
            {
                // 检查是否有复选框列
                if (!dt.Columns.Contains("_Selected"))
                {
                    return null; // 没有复选框列，返回null表示全选
                }
                
                // 遍历所有行，找出勾选的行
                bool hasChecked = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["_Selected"] != DBNull.Value && Convert.ToBoolean(row["_Selected"]))
                    {
                        hasChecked = true;
                        
                        // 尝试从行中获取ID字段（不同服务可能有不同的ID字段名）
                        long id = ExtractRowId(row);
                        if (id > 0)
                        {
                            selectedIds.Add(id);
                        }
                    }
                }
                
                // 如果没有勾选任何行，返回null表示全选
                if (!hasChecked)
                {
                    return null;
                }
            }
            
            return selectedIds.Count > 0 ? selectedIds : null;
        }
        
        /// <summary>
        /// 从DataRow中提取ID值
        /// </summary>
        private long ExtractRowId(DataRow row)
        {
            // 尝试常见的ID字段名
            string[] idFieldNames = new[] 
            { 
                "订单ID", "Inv_ID", "PurOrder_ID", "SaleOrder_ID", 
                "ProdDetailID", "ID", "主键", "_ID"
            };
            
            foreach (var fieldName in idFieldNames)
            {
                if (row.Table.Columns.Contains(fieldName) && row[fieldName] != DBNull.Value)
                {
                    if (long.TryParse(row[fieldName].ToString(), out long id))
                    {
                        return id;
                    }
                }
            }
            
            return 0;
        }
        
        #region 预览功能实现
        
 
        /// <summary>
        /// 预览采购订单价格修复
        /// </summary>
        private async Task PreviewPurOrderPriceFix()
        {
            richTextBoxLog.AppendText("正在分析采购订单价格数据...\r\n");
            
            var orders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                .Includes(o => o.tb_PurOrderDetails)
                .Where(o => o.Created_at.HasValue && o.Created_at.Value > DateTime.Now.AddMonths(-6))
                .ToListAsync();
            
            DataTable dt = new DataTable();
            dt.Columns.Add("订单号", typeof(string));
            dt.Columns.Add("主表总金额", typeof(decimal));
            dt.Columns.Add("明细合计", typeof(decimal));
            dt.Columns.Add("差异", typeof(decimal));
            dt.Columns.Add("是否一致", typeof(string));
            
            int inconsistencyCount = 0;
            
            foreach (var order in orders.Take(100))
            {
                if (order.tb_PurOrderDetails == null || order.tb_PurOrderDetails.Count == 0)
                    continue;
                
                decimal detailTotal = order.tb_PurOrderDetails.Sum(d => d.SubtotalAmount);
                decimal diff = Math.Abs(order.TotalAmount - detailTotal);
                bool isConsistent = diff < 0.01m;
                
                if (!isConsistent)
                    inconsistencyCount++;
                
                DataRow row = dt.NewRow();
                row["订单号"] = order.PurOrderNo;
                row["主表总金额"] = order.TotalAmount;
                row["明细合计"] = detailTotal;
                row["差异"] = diff;
                row["是否一致"] = isConsistent ? "是" : "否";
                
                dt.Rows.Add(row);
            }
            
            _previewData = dt;
            dataGridView1.DataSource = dt;
            
            richTextBoxLog.AppendText($"检查了 {orders.Count} 个订单，发现 {inconsistencyCount} 个价格不一致的订单。\r\n");
            richTextBoxLog.AppendText($"已加载 {dt.Rows.Count} 条预览数据。\r\n");
        }
        
        /// <summary>
        /// 预览销售订单成本数量修复
        /// </summary>
        private async Task PreviewSaleOrderCostFix()
        {
            richTextBoxLog.AppendText("正在分析销售订单成本数据...\r\n");
            
            var orders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                .Includes(o => o.tb_SaleOrderDetails)
                .Where(o => o.Created_at.HasValue && o.Created_at.Value > DateTime.Now.AddMonths(-3))
                .ToListAsync();
            
            DataTable dt = new DataTable();
            dt.Columns.Add("订单号", typeof(string));
            dt.Columns.Add("产品SKU", typeof(string));
            dt.Columns.Add("订单成本", typeof(decimal));
            dt.Columns.Add("最新成本", typeof(decimal));
            dt.Columns.Add("差异%", typeof(decimal));
            
            int needFixCount = 0;
            
            foreach (var order in orders.Take(50))
            {
                if (order.tb_SaleOrderDetails == null)
                    continue;
                
                foreach (var detail in order.tb_SaleOrderDetails.Take(5))
                {
                    // 获取最新成本
                    var inventory = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                        .Where(i => i.ProdDetailID == detail.ProdDetailID)
                        .FirstAsync();
                    
                    if (inventory != null && inventory.Inv_Cost > 0)
                    {
                        decimal diffPercent = 0;
                        if (detail.Cost > 0)
                        {
                            diffPercent = Math.Abs(inventory.Inv_Cost - detail.Cost) / detail.Cost * 100;
                        }
                        
                        if (diffPercent > 5) // 差异超过5%
                        {
                            needFixCount++;
                            DataRow row = dt.NewRow();
                            row["订单号"] = order.SOrderNo;
                            row["产品SKU"] = detail.ProdDetailID;
                            row["订单成本"] = detail.Cost;
                            row["最新成本"] = inventory.Inv_Cost;
                            row["差异%"] = Math.Round(diffPercent, 2);
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            
            _previewData = dt;
            dataGridView1.DataSource = dt;
            
            richTextBoxLog.AppendText($"发现 {needFixCount} 条成本差异较大的记录。\r\n");
            richTextBoxLog.AppendText($"已加载 {dt.Rows.Count} 条预览数据。\r\n");
        }
        

        #endregion
        
        #region 执行修复功能实现
        
        /// <summary>
        /// 执行成本修复
        /// </summary>
        private async Task ExecuteCostFix()
        {
            richTextBoxLog.AppendText("开始执行成本修复...\r\n");
            
            // 调用现有的CostFix方法
            var result = await CostFix(false);
            
            if (result != null && result.Count > 0)
            {
                richTextBoxLog.AppendText($"成本修复完成，共处理 {result.Count} 条库存记录。\r\n");
                
                // 显示统计信息
                int fixedCount = result.Count(i => i.Inv_Cost > 0);
                richTextBoxLog.AppendText($"成功修复 {fixedCount} 条记录。\r\n");
            }
            else
            {
                richTextBoxLog.AppendText("未发现需要修复的成本数据。\r\n");
            }
        }
        
        /// <summary>
        /// 执行采购订单价格修复
        /// </summary>
        private async Task ExecutePurOrderPriceFix()
        {
            richTextBoxLog.AppendText("开始执行采购订单价格修复...\r\n");
            
            var orders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                .Includes(o => o.tb_PurOrderDetails)
                .ToListAsync();
            
            int fixedCount = 0;
            List<tb_PurOrder> updateList = new List<tb_PurOrder>();
            
            foreach (var order in orders)
            {
                if (order.tb_PurOrderDetails == null || order.tb_PurOrderDetails.Count == 0)
                    continue;
                
                decimal detailTotal = order.tb_PurOrderDetails.Sum(d => d.SubtotalAmount);
                decimal diff = Math.Abs(order.TotalAmount - detailTotal);
                
                if (diff >= 0.01m)
                {
                    order.TotalAmount = detailTotal;
                    updateList.Add(order);
                    fixedCount++;
                    
                    richTextBoxLog.AppendText($"修复订单 {order.PurOrderNo}: {order.TotalAmount} -> {detailTotal}\r\n");
                }
            }
            
            if (!chkTestMode.Checked && updateList.Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Updateable(updateList)
                    .UpdateColumns(o => new { o.TotalAmount })
                    .ExecuteCommandAsync();
                
                richTextBoxLog.AppendText($"\r\n已更新 {updateList.Count} 个订单的主表金额。\r\n");
            }
            else
            {
                richTextBoxLog.AppendText($"\r\n测试模式：共需修复 {fixedCount} 个订单，但未实际执行更新。\r\n");
            }
        }
        
        /// <summary>
        /// 执行销售订单成本数量修复
        /// </summary>
        private async Task ExecuteSaleOrderCostFix()
        {
            richTextBoxLog.AppendText("开始执行销售订单成本修复...\r\n");
            
            var orders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                .Includes(o => o.tb_SaleOrderDetails)
                .Where(o => o.Created_at.HasValue && o.Created_at.Value > DateTime.Now.AddMonths(-3))
                .ToListAsync();
            
            int fixedCount = 0;
            List<tb_SaleOrderDetail> updateDetails = new List<tb_SaleOrderDetail>();
            
            foreach (var order in orders)
            {
                if (order.tb_SaleOrderDetails == null)
                    continue;
                
                foreach (var detail in order.tb_SaleOrderDetails)
                {
                    var inventory = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                        .Where(i => i.ProdDetailID == detail.ProdDetailID)
                        .FirstAsync();
                    
                    if (inventory != null && inventory.Inv_Cost > 0)
                    {
                        decimal diffPercent = 0;
                        if (detail.Cost > 0)
                        {
                            diffPercent = Math.Abs(inventory.Inv_Cost - detail.Cost) / detail.Cost * 100;
                        }
                        
                        if (diffPercent > 5) // 差异超过5%
                        {
                            detail.Cost = inventory.Inv_Cost;
                            detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;
                            updateDetails.Add(detail);
                            fixedCount++;
                        }
                    }
                }
            }
            
            if (!chkTestMode.Checked && updateDetails.Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Updateable(updateDetails)
                    .UpdateColumns(d => new { d.Cost, d.SubtotalCostAmount })
                    .ExecuteCommandAsync();
                
                richTextBoxLog.AppendText($"\r\n已更新 {fixedCount} 条订单明细的成本。\r\n");
            }
            else
            {
                richTextBoxLog.AppendText($"\r\n测试模式：共需修复 {fixedCount} 条明细，但未实际执行更新。\r\n");
            }
        }
        
        /// <summary>
        /// 执行库存统计修复
        /// </summary>
        private async Task ExecuteInventoryStatisticsFix()
        {
            richTextBoxLog.AppendText("开始执行库存统计修复...\r\n");
            richTextBoxLog.AppendText("此功能需要重新计算所有产品的拟销、在制、在途数量。\r\n");
            richTextBoxLog.AppendText("由于计算复杂，建议在业务低峰期执行。\r\n\r\n");
            
            // 这里应该实现完整的统计重算逻辑
            // 为简化，仅显示提示信息
            
            richTextBoxLog.AppendText("提示：完整的统计修复需要：\r\n");
            richTextBoxLog.AppendText("1. 遍历所有未关闭的销售订单，统计拟销数量\r\n");
            richTextBoxLog.AppendText("2. 遍历所有进行中的生产计划，统计在制数量\r\n");
            richTextBoxLog.AppendText("3. 遍历所有未入库的采购订单，统计在途数量\r\n");
            richTextBoxLog.AppendText("4. 更新库存表的对应字段\r\n\r\n");
            
            richTextBoxLog.AppendText("当前为演示版本，未实现完整逻辑。\r\n");
        }
        
        #endregion
        
        #region 动态查询条件生成
        
        private BaseEntity _queryDtoProxy; // 查询DTO代理
        
        /// <summary>
        /// 动态生成查询条件UI
        /// </summary>
        /// <param name="correctionItemName">修复项名称</param>
        private void GenerateQueryConditionUI(string correctionItemName)
        {
            // 清空旧控件
            kryptonPanelQuery.Controls.Clear();
            
            // 获取服务
            var service = DataCorrectionServiceManager.GetService(correctionItemName);
            if (service == null)
            {
                return;
            }
            
            // 获取查询过滤器
            var queryFilter = service.GetQueryFilter();
            if (queryFilter == null)
            {
                // 不支持动态查询，显示提示
                var lblHint = new Krypton.Toolkit.KryptonLabel
                {
                    Text = "此修复项暂不支持动态查询条件",
                    Dock = DockStyle.Fill,
                };
                kryptonPanelQuery.Controls.Add(lblHint);
                return;
            }
            
            try
            {
                // ✅ 获取查询使用的实体类型
                var queryEntityType = service.GetQueryEntityType();
                if (queryEntityType == null)
                {
                    richTextBoxLog.AppendText($"❌ 服务未指定查询实体类型\r\n");
                    return;
                }
                
                // ✅ 使用 UIGenerateHelper 动态生成查询UI
                _queryDtoProxy = RUINORERP.UI.AdvancedUIModule.UIGenerateHelper.CreateQueryUI(
                    queryEntityType,             // 从服务获取的实体类型
                    false,                       // 不使用Like查询
                    kryptonPanelQuery,           // 容器面板
                    queryFilter,                 // 查询过滤器
                    null                         // 暂不支持个性化设置
                );
                
                richTextBoxLog.AppendText($"✅ 已生成查询条件UI（实体类型：{queryEntityType.Name}）\r\n");
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText($"❌ 生成查询条件UI失败：{ex.Message}\r\n");
                System.Diagnostics.Debug.WriteLine($"生成查询条件UI失败: {ex}");
            }
        }
        
        /// <summary>
        /// 从UI读取查询参数
        /// </summary>
        /// <returns>查询参数字典</returns>
        private Dictionary<string, object> GetQueryParameters()
        {
            var parameters = new Dictionary<string, object>();
            
            if (_queryDtoProxy == null)
            {
                return parameters;
            }
            
            try
            {
                // 遍历所有属性，读取用户输入
                var properties = _queryDtoProxy.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(_queryDtoProxy);
                    if (value != null)
                    {
                        // 日期类型需要特殊处理
                        if (prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(DateTime))
                        {
                            var dt = value.ToDateTime();
                            if (dt.Year > 1) // 有效日期（不是 DateTime.MinValue）
                            {
                                parameters[prop.Name] = dt;
                            }
                        }
                        else if (prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(int))
                        {
                            var intVal = value.ToInt();
                            if (intVal != -1 && intVal != 0) // 过滤无效值
                            {
                                parameters[prop.Name] = intVal;
                            }
                        }
                        else if (prop.PropertyType == typeof(long?) || prop.PropertyType == typeof(long))
                        {
                            var longVal = value.ToLong();
                            if (longVal != -1 && longVal != 0) // 过滤无效值
                            {
                                parameters[prop.Name] = longVal;
                            }
                        }
                        else
                        {
                            // 其他类型直接添加
                            if (!string.IsNullOrEmpty(value.ToString()))
                            {
                                parameters[prop.Name] = value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取查询参数失败: {ex}");
            }
            
            return parameters;
        }
        
        #endregion
    }
}
