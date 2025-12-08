using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 文件: IBusinessStatusManager.cs
    /// 版本: V3增强版 - 业务性状态动态管理接口
    /// 说明: 定义业务性状态动态管理的核心接口
    /// 创建日期: 2024年
    /// 作者: RUINOR ERP开发团队
    /// 
    /// 版本标识：
    /// V3增强版: 增加对业务性状态的动态支持，实现业务上下文感知
    /// 功能: 提供业务性状态的动态注册、验证、转换和上下文管理
    /// </summary>

    /// <summary>
    /// 业务性状态管理器接口
    /// 提供业务性状态的动态管理功能，支持不同业务场景的状态定义和转换
    /// </summary>
    public interface IBusinessStatusManager
    {
        /// <summary>
        /// 注册业务状态类型
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="configuration">状态配置</param>
        void RegisterBusinessStatusType(Type statusType, BusinessStatusTypeConfiguration configuration);

        /// <summary>
        /// 获取业务状态类型配置
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>状态配置</returns>
        BusinessStatusTypeConfiguration GetBusinessStatusTypeConfiguration(Type statusType);

        /// <summary>
        /// 注册业务状态转换规则
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="rule">转换规则</param>
        void RegisterBusinessTransitionRule(Type statusType, object fromStatus, object toStatus, BusinessTransitionRule rule);

        /// <summary>
        /// 验证业务状态转换是否允许
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>验证结果</returns>
        BusinessTransitionValidationResult ValidateBusinessTransition(Type statusType, object fromStatus, object toStatus, BusinessContext context);

        /// <summary>
        /// 执行业务状态转换
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>转换结果</returns>
        BusinessTransitionResult ExecuteBusinessTransition(Type statusType, object entity, object fromStatus, object toStatus, BusinessContext context);

        /// <summary>
        /// 获取业务状态可转换的状态列表
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<object> GetAvailableBusinessTransitions(Type statusType, object fromStatus, BusinessContext context);

        /// <summary>
        /// 注册业务状态操作权限规则
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="action">操作名称</param>
        /// <param name="rule">权限规则</param>
        void RegisterBusinessActionRule(Type statusType, object status, string action, BusinessActionRule rule);

        /// <summary>
        /// 检查业务状态操作权限
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="action">操作名称</param>
        /// <param name="context">业务上下文</param>
        /// <returns>权限检查结果</returns>
        bool CheckBusinessActionPermission(Type statusType, object status, string action, BusinessContext context);

        /// <summary>
        /// 获取业务状态显示信息
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>显示信息</returns>
        BusinessStatusDisplayInfo GetBusinessStatusDisplayInfo(Type statusType, object status, BusinessContext context);

        /// <summary>
        /// 获取业务状态元数据
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>状态元数据</returns>
        BusinessStatusMetadata GetBusinessStatusMetadata(Type statusType);
    }

    /// <summary>
    /// 业务状态类型配置
    /// </summary>
    public class BusinessStatusTypeConfiguration
    {
        /// <summary>
        /// 状态类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 适用业务模块
        /// </summary>
        public string[] ApplicableModules { get; set; }

        /// <summary>
        /// 状态属性定义
        /// </summary>
        public Dictionary<string, BusinessStatusPropertyDefinition> PropertyDefinitions { get; set; }

        /// <summary>
        /// 是否支持动态状态
        /// </summary>
        public bool SupportsDynamicStatus { get; set; }

        /// <summary>
        /// 状态值提供程序
        /// </summary>
        public IBusinessStatusValueProvider StatusValueProvider { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessStatusTypeConfiguration()
        {
            ApplicableModules = new string[0];
            PropertyDefinitions = new Dictionary<string, BusinessStatusPropertyDefinition>();
        }
    }

    /// <summary>
    /// 业务状态属性定义
    /// </summary>
    public class BusinessStatusPropertyDefinition
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 是否必需
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 验证规则
        /// </summary>
        public List<IBusinessStatusValidationRule> ValidationRules { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessStatusPropertyDefinition()
        {
            ValidationRules = new List<IBusinessStatusValidationRule>();
        }
    }

    /// <summary>
    /// 业务状态转换规则
    /// </summary>
    public class BusinessTransitionRule
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 验证函数
        /// </summary>
        public Func<BusinessContext, bool> Validator { get; set; }

        /// <summary>
        /// 转换前置处理
        /// </summary>
        public Func<BusinessContext, BusinessTransitionResult> PreTransitionHandler { get; set; }

        /// <summary>
        /// 转换后置处理
        /// </summary>
        public Func<BusinessContext, BusinessTransitionResult> PostTransitionHandler { get; set; }

        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool RequiresApproval { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessTransitionRule()
        {
            Priority = 0;
        }
    }

    /// <summary>
    /// 业务状态操作权限规则
    /// </summary>
    public class BusinessActionRule
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 权限验证函数
        /// </summary>
        public Func<BusinessContext, bool> PermissionValidator { get; set; }

        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool RequiresApproval { get; set; }

        /// <summary>
        /// 适用角色
        /// </summary>
        public string[] ApplicableRoles { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessActionRule()
        {
            ApplicableRoles = new string[0];
        }
    }

    /// <summary>
    /// 业务上下文
    /// </summary>
    public class BusinessContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] UserRoles { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 实体对象
        /// </summary>
        public object Entity { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessContext()
        {
            UserRoles = new string[0];
            AdditionalData = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// 业务状态转换验证结果
    /// </summary>
    public class BusinessTransitionValidationResult
    {
        /// <summary>
        /// 是否允许转换
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// 验证消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 需要的审批级别
        /// </summary>
        public int RequiredApprovalLevel { get; set; }

        /// <summary>
        /// 额外信息
        /// </summary>
        public Dictionary<string, object> AdditionalInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessTransitionValidationResult()
        {
            AdditionalInfo = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// 业务状态转换结果
    /// </summary>
    public class BusinessTransitionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 源状态
        /// </summary>
        public object FromStatus { get; set; }

        /// <summary>
        /// 目标状态
        /// </summary>
        public object ToStatus { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessTransitionResult()
        {
            TransitionTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 业务状态显示信息
    /// </summary>
    public class BusinessStatusDisplayInfo
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 颜色代码
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否为终态
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessStatusDisplayInfo()
        {
        }
    }

    /// <summary>
    /// 业务状态元数据
    /// </summary>
    public class BusinessStatusMetadata
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; set; }

        /// <summary>
        /// 状态值列表
        /// </summary>
        public List<object> StatusValues { get; set; }

        /// <summary>
        /// 状态属性映射
        /// </summary>
        public Dictionary<object, Dictionary<string, object>> StatusProperties { get; set; }

        /// <summary>
        /// 状态转换规则映射
        /// </summary>
        public Dictionary<object, List<object>> TransitionRules { get; set; }

        /// <summary>
        /// 操作权限规则映射
        /// </summary>
        public Dictionary<object, Dictionary<string, BusinessActionRule>> ActionRules { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessStatusMetadata()
        {
            StatusValues = new List<object>();
            StatusProperties = new Dictionary<object, Dictionary<string, object>>();
            TransitionRules = new Dictionary<object, List<object>>();
            ActionRules = new Dictionary<object, Dictionary<string, BusinessActionRule>>();
        }
    }

    /// <summary>
    /// 业务状态值提供程序接口
    /// </summary>
    public interface IBusinessStatusValueProvider
    {
        /// <summary>
        /// 获取所有状态值
        /// </summary>
        /// <param name="context">业务上下文</param>
        /// <returns>状态值列表</returns>
        IEnumerable<object> GetAllStatusValues(BusinessContext context);

        /// <summary>
        /// 获取状态值显示信息
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>显示信息</returns>
        BusinessStatusDisplayInfo GetStatusDisplayInfo(object statusValue, BusinessContext context);

        /// <summary>
        /// 验证状态值是否有效
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>是否有效</returns>
        bool IsValidStatusValue(object statusValue, BusinessContext context);
    }

    /// <summary>
    /// 业务状态验证规则接口
    /// </summary>
    public interface IBusinessStatusValidationRule
    {
        /// <summary>
        /// 验证状态值
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>验证结果</returns>
        bool Validate(object statusValue, BusinessContext context);

        /// <summary>
        /// 获取验证错误消息
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>错误消息</returns>
        string GetErrorMessage(object statusValue, BusinessContext context);
    }
}