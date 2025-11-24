using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.UI.WorkFlowDesigner.Nodes;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 业务节点模板管理器
    /// 负责管理预定义的业务节点模板
    /// </summary>
    public class BusinessNodeTemplateManager
    {
        #region Fields

        private readonly Itb_MenuInfoServices _menuInfoService;
        private readonly Itb_ModuleDefinitionServices _moduleDefinitionService;
        private Dictionary<ERPBusinessModule, List<BusinessNodeTemplate>> _moduleTemplates;
        private Dictionary<ProcessNavigationNodeBusinessType, Color> _nodeTypeColors;

        #endregion

        #region Constructor

        public BusinessNodeTemplateManager()
        {
            _menuInfoService = Startup.GetFromFac<Itb_MenuInfoServices>();
            _moduleDefinitionService = Startup.GetFromFac<Itb_ModuleDefinitionServices>();
            InitializeNodeTypeColors();
            InitializeModuleTemplates();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// 初始化节点类型颜色
        /// </summary>
        private void InitializeNodeTypeColors()
        {
            _nodeTypeColors = new Dictionary<ProcessNavigationNodeBusinessType, Color>
            {
                { ProcessNavigationNodeBusinessType.通用节点, Color.LightGray },
                { ProcessNavigationNodeBusinessType.菜单节点, Color.LightBlue },
                { ProcessNavigationNodeBusinessType.模块节点, Color.LightGreen },
                { ProcessNavigationNodeBusinessType.流程节点, Color.LightYellow },
                { ProcessNavigationNodeBusinessType.外部系统节点, Color.LightCoral },
                { ProcessNavigationNodeBusinessType.数据源节点, Color.LightPink }
            };
        }

        /// <summary>
        /// 初始化模块模板
        /// </summary>
        private void InitializeModuleTemplates()
        {
            _moduleTemplates = new Dictionary<ERPBusinessModule, List<BusinessNodeTemplate>>();

            // 采购管理模块
            _moduleTemplates[ERPBusinessModule.采购管理] = new List<BusinessNodeTemplate>
            {
                new BusinessNodeTemplate
                {
                    Name = "采购申请",
                    Description = "发起采购申请流程",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightBlue,
                    Icon = "📋",
                    Category = "采购流程"
                },
                new BusinessNodeTemplate
                {
                    Name = "供应商管理",
                    Description = "管理供应商信息",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightBlue,
                    Icon = "🏢",
                    Category = "基础管理"
                },
                new BusinessNodeTemplate
                {
                    Name = "采购订单",
                    Description = "创建和管理采购订单",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightBlue,
                    Icon = "📄",
                    Category = "采购执行"
                },
                new BusinessNodeTemplate
                {
                    Name = "采购入库",
                    Description = "处理采购物品入库",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightBlue,
                    Icon = "📦",
                    Category = "采购执行"
                }
            };

            // 销售管理模块
            _moduleTemplates[ERPBusinessModule.销售管理] = new List<BusinessNodeTemplate>
            {
                new BusinessNodeTemplate
                {
                    Name = "客户管理",
                    Description = "管理客户信息",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightGreen,
                    Icon = "👥",
                    Category = "基础管理"
                },
                new BusinessNodeTemplate
                {
                    Name = "销售报价",
                    Description = "创建销售报价单",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightGreen,
                    Icon = "💰",
                    Category = "销售流程"
                },
                new BusinessNodeTemplate
                {
                    Name = "销售订单",
                    Description = "管理销售订单",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightGreen,
                    Icon = "📋",
                    Category = "销售执行"
                },
                new BusinessNodeTemplate
                {
                    Name = "销售出库",
                    Description = "处理销售出库",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightGreen,
                    Icon = "🚚",
                    Category = "销售执行"
                }
            };

            // 库存管理模块
            _moduleTemplates[ERPBusinessModule.库存管理] = new List<BusinessNodeTemplate>
            {
                new BusinessNodeTemplate
                {
                    Name = "库存查询",
                    Description = "查询库存状态",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightYellow,
                    Icon = "🔍",
                    Category = "库存查询"
                },
                new BusinessNodeTemplate
                {
                    Name = "库存盘点",
                    Description = "执行库存盘点",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightYellow,
                    Icon = "📊",
                    Category = "库存操作"
                },
                new BusinessNodeTemplate
                {
                    Name = "库存调拨",
                    Description = "处理库存调拨",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightYellow,
                    Icon = "🔄",
                    Category = "库存操作"
                },
                new BusinessNodeTemplate
                {
                    Name = "库存预警",
                    Description = "查看库存预警信息",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightYellow,
                    Icon = "⚠️",
                    Category = "库存监控"
                }
            };

            // 财务管理模块
            _moduleTemplates[ERPBusinessModule.财务管理] = new List<BusinessNodeTemplate>
            {
                new BusinessNodeTemplate
                {
                    Name = "应收管理",
                    Description = "管理应收账款",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightCoral,
                    Icon = "💵",
                    Category = "应收应付"
                },
                new BusinessNodeTemplate
                {
                    Name = "应付管理",
                    Description = "管理应付账款",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightCoral,
                    Icon = "💸",
                    Category = "应收应付"
                },
                new BusinessNodeTemplate
                {
                    Name = "财务报表",
                    Description = "查看财务报表",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightCoral,
                    Icon = "📈",
                    Category = "报表分析"
                },
                new BusinessNodeTemplate
                {
                    Name = "费用管理",
                    Description = "管理费用报销",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightCoral,
                    Icon = "💼",
                    Category = "费用管理"
                }
            };

            // 生产管理模块
            _moduleTemplates[ERPBusinessModule.生产管理] = new List<BusinessNodeTemplate>
            {
                new BusinessNodeTemplate
                {
                    Name = "生产计划",
                    Description = "制定生产计划",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightPink,
                    Icon = "📅",
                    Category = "计划管理"
                },
                new BusinessNodeTemplate
                {
                    Name = "生产订单",
                    Description = "管理生产订单",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightPink,
                    Icon = "🏭",
                    Category = "生产执行"
                },
                new BusinessNodeTemplate
                {
                    Name = "物料需求",
                    Description = "计算物料需求",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightPink,
                    Icon = "📦",
                    Category = "物料管理"
                },
                new BusinessNodeTemplate
                {
                    Name = "生产汇报",
                    Description = "生产进度汇报",
                    BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                    NodeType = ProcessNavigationNodeType.流程导航节点,
                    DefaultColor = Color.LightPink,
                    Icon = "📊",
                    Category = "生产监控"
                }
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取指定模块的节点模板
        /// </summary>
        /// <param name="module">业务模块</param>
        /// <returns>节点模板列表</returns>
        public List<BusinessNodeTemplate> GetModuleTemplates(ERPBusinessModule module)
        {
            if (_moduleTemplates.ContainsKey(module))
            {
                return _moduleTemplates[module];
            }
            return new List<BusinessNodeTemplate>();
        }

        /// <summary>
        /// 获取所有模块的节点模板
        /// </summary>
        /// <returns>所有节点模板</returns>
        public Dictionary<ERPBusinessModule, List<BusinessNodeTemplate>> GetAllModuleTemplates()
        {
            return _moduleTemplates;
        }

        /// <summary>
        /// 根据业务类型获取默认颜色
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <returns>默认颜色</returns>
        public Color GetNodeTypeColor(ProcessNavigationNodeBusinessType businessType)
        {
            if (_nodeTypeColors.ContainsKey(businessType))
            {
                return _nodeTypeColors[businessType];
            }
            return Color.LightGray;
        }

        /// <summary>
        /// 创建流程导航节点
        /// </summary>
        /// <param name="template">节点模板</param>
        /// <param name="position">节点位置</param>
        /// <returns>流程导航节点</returns>
        public ProcessNavigationNode CreateProcessNavigationNode(BusinessNodeTemplate template, PointF position)
        {
            var node = new ProcessNavigationNode
            {
                Text = template.Name,
                BusinessType = (ProcessNavigationNodeBusinessType)template.BusinessType,
                NodeType = (WFNodeType)template.NodeType,
                NodeColor = template.DefaultColor,
                Rectangle = new RectangleF(position.X, position.Y, 140, 80),
                ProcessName = template.Name,
                Description = template.Description
            };

            // 根据模板设置节点属性
            if (template.BusinessType == ProcessNavigationNodeBusinessType.菜单节点 && template.MenuID.HasValue)
            {
                node.MenuID = template.MenuID.Value.ToString();
            }
            else if (template.BusinessType == ProcessNavigationNodeBusinessType.模块节点 && template.ModuleID.HasValue)
            {
                node.ModuleID = template.ModuleID.Value;
            }
            else if (template.BusinessType == ProcessNavigationNodeBusinessType.流程节点 && template.ChildNavigationID.HasValue)
            {
                //TODO list
                //node.ChildNavigationID = template.ChildNavigationID.Value;
            }

            return node;
        }

        /// <summary>
        /// 从菜单创建节点模板
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <returns>节点模板</returns>
        public BusinessNodeTemplate CreateTemplateFromMenu(tb_MenuInfo menuInfo)
        {
            return new BusinessNodeTemplate
            {
                Name = menuInfo.MenuName,
                Description = $"菜单：{menuInfo.MenuName}",
                BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                NodeType = ProcessNavigationNodeType.流程导航节点,
                DefaultColor = GetNodeTypeColor(ProcessNavigationNodeBusinessType.菜单节点),
                MenuID = menuInfo.MenuID,
                FormName = menuInfo.FormName,
                ClassPath = menuInfo.UIPropertyIdentifier,
                Icon = "📋",
                Category = "菜单节点"
            };
        }

        /// <summary>
        /// 从模块创建节点模板
        /// </summary>
        /// <param name="moduleInfo">模块信息</param>
        /// <returns>节点模板</returns>
        public BusinessNodeTemplate CreateTemplateFromModule(tb_ModuleDefinition moduleInfo)
        {
            return new BusinessNodeTemplate
            {
                Name = moduleInfo.ModuleName,
                Description = $"模块：{moduleInfo.ModuleName}",
                BusinessType = ProcessNavigationNodeBusinessType.模块节点,
                NodeType = ProcessNavigationNodeType.流程导航节点,
                DefaultColor = GetNodeTypeColor(ProcessNavigationNodeBusinessType.模块节点),
                ModuleID = moduleInfo.ModuleID,
                Icon = "🏢",
                Category = "模块节点"
            };
        }

        /// <summary>
        /// 获取模块枚举描述
        /// </summary>
        /// <param name="module">业务模块</param>
        /// <returns>模块描述</returns>
        public string GetModuleDescription(ERPBusinessModule module)
        {
            switch (module)
            {
                case ERPBusinessModule.采购管理:
                    return "采购管理 - 包含采购申请、供应商管理、采购订单、采购入库等功能";
                case ERPBusinessModule.销售管理:
                    return "销售管理 - 包含客户管理、销售报价、销售订单、销售出库等功能";
                case ERPBusinessModule.库存管理:
                    return "库存管理 - 包含库存查询、库存盘点、库存调拨、库存预警等功能";
                case ERPBusinessModule.生产管理:
                    return "生产管理 - 包含生产计划、生产订单、物料需求、生产汇报等功能";
                case ERPBusinessModule.财务管理:
                    return "财务管理 - 包含应收管理、应付管理、财务报表、费用管理等功能";
                case ERPBusinessModule.客户关系管理:
                    return "客户关系管理 - 包含客户档案、客户跟进、客户服务等功能";
                case ERPBusinessModule.人力资源管理:
                    return "人力资源管理 - 包含员工档案、考勤管理、薪资管理等功能";
                case ERPBusinessModule.质量管理:
                    return "质量管理 - 包含质量检验、质量分析、质量改进等功能";
                case ERPBusinessModule.报表分析:
                    return "报表分析 - 包含业务报表、数据分析、决策支持等功能";
                case ERPBusinessModule.系统管理:
                    return "系统管理 - 包含用户管理、权限管理、系统配置等功能";
                default:
                    return "未分类模块";
            }
        }

        #endregion
    }

    /// <summary>
    /// 业务节点模板
    /// </summary>
    public class BusinessNodeTemplate
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public ProcessNavigationNodeBusinessType BusinessType { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public ProcessNavigationNodeType NodeType { get; set; }

        /// <summary>
        /// 默认颜色
        /// </summary>
        public Color DefaultColor { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 关联菜单ID
        /// </summary>
        public long? MenuID { get; set; }

        /// <summary>
        /// 关联模块ID
        /// </summary>
        public long? ModuleID { get; set; }

        /// <summary>
        /// 子流程导航图ID
        /// </summary>
        public long? ChildNavigationID { get; set; }

        /// <summary>
        /// 窗体名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 类路径
        /// </summary>
        public string ClassPath { get; set; }
    }
}