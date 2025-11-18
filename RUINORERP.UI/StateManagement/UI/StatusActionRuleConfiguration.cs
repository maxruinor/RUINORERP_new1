/**
 * 文件: StatusActionRuleConfiguration.cs
 * 说明: 状态操作规则配置类 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// 状态操作规则配置类 - v3版本
    /// 负责管理不同状态下的可执行操作规则
    /// </summary>
    public class StatusActionRuleConfiguration
    {
        #region 字段

        /// <summary>
        /// 状态操作规则字典
        /// 键：状态类型名称，值：状态规则字典
        /// 状态规则字典：键：状态值，值：允许的操作列表
        /// </summary>
        private readonly Dictionary<string, Dictionary<object, List<object>>> _statusActionRules;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态操作规则配置
        /// </summary>
        public StatusActionRuleConfiguration()
        {
            _statusActionRules = new Dictionary<string, Dictionary<object, List<object>>>();
            
            // 初始化默认规则
            InitializeDefaultRules();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 注册状态操作规则
        /// </summary>
        /// <typeparam name="TStatus">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="allowedActions">允许的操作列表</param>
        public void RegisterRule<TStatus>(TStatus status, params Enum[] allowedActions) where TStatus : Enum
        {
            var statusTypeName = typeof(TStatus).Name;
            
            if (!_statusActionRules.ContainsKey(statusTypeName))
            {
                _statusActionRules[statusTypeName] = new Dictionary<object, List<object>>();
            }
            
            _statusActionRules[statusTypeName][status] = allowedActions?.Cast<object>().ToList() ?? new List<object>();
        }

        /// <summary>
        /// 检查操作是否允许
        /// </summary>
        /// <typeparam name="TStatus">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否允许</returns>
        public bool IsActionAllowed<TStatus>(TStatus status, Enum action) where TStatus : Enum
        {
            var statusTypeName = typeof(TStatus).Name;
            
            if (!_statusActionRules.ContainsKey(statusTypeName))
            {
                return false;
            }
            
            var statusRules = _statusActionRules[statusTypeName];
            
            if (!statusRules.ContainsKey(status))
            {
                return false;
            }
            
            var allowedActions = statusRules[status];
            return allowedActions.Contains(action);
        }

        /// <summary>
        /// 获取指定状态下允许的操作列表
        /// </summary>
        /// <typeparam name="TStatus">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>允许的操作列表</returns>
        public IEnumerable<Enum> GetAllowedActions<TStatus>(TStatus status) where TStatus : Enum
        {
            var statusTypeName = typeof(TStatus).Name;
            
            if (!_statusActionRules.ContainsKey(statusTypeName))
            {
                return Enumerable.Empty<Enum>();
            }
            
            var statusRules = _statusActionRules[statusTypeName];
            
            if (!statusRules.ContainsKey(status))
            {
                return Enumerable.Empty<Enum>();
            }
            
            return statusRules[status].Cast<Enum>();
        }

        /// <summary>
        /// 清除指定状态类型的所有规则
        /// </summary>
        /// <typeparam name="TStatus">状态枚举类型</typeparam>
        public void ClearRules<TStatus>() where TStatus : Enum
        {
            var statusTypeName = typeof(TStatus).Name;
            
            if (_statusActionRules.ContainsKey(statusTypeName))
            {
                _statusActionRules.Remove(statusTypeName);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认规则
        /// </summary>
        private void InitializeDefaultRules()
        {
            // DataStatus默认规则
            InitializeDataStatusRules();
            
            // PrePaymentStatus默认规则
            InitializePrePaymentStatusRules();
            
            // ARAPStatus默认规则
            InitializeARAPStatusRules();
            
            // PaymentStatus默认规则
            InitializePaymentStatusRules();
        }

        /// <summary>
        /// 初始化DataStatus规则
        /// </summary>
        private void InitializeDataStatusRules()
        {
            // 草稿状态：允许保存、修改、删除、提交
            RegisterRule(DataStatus.草稿, 
                MenuItemEnums.保存, 
                MenuItemEnums.修改, 
                MenuItemEnums.删除, 
                MenuItemEnums.提交);
            
            // 新建状态：允许提交
            RegisterRule(DataStatus.新建, 
                MenuItemEnums.提交);
            
            // 确认状态：允许审核、结案
            RegisterRule(DataStatus.确认, 
                MenuItemEnums.审核, 
                MenuItemEnums.结案);
            
            // 完结状态：允许反结案
            RegisterRule(DataStatus.完结, 
                MenuItemEnums.反结案);
            
            // 作废状态：无操作
            RegisterRule(DataStatus.作废);
        }

        /// <summary>
        /// 初始化PrePaymentStatus规则
        /// </summary>
        private void InitializePrePaymentStatusRules()
        {
            // 草稿状态：允许保存、修改、删除、提交
            RegisterRule(PrePaymentStatus.草稿, 
                MenuItemEnums.保存, 
                MenuItemEnums.修改, 
                MenuItemEnums.删除, 
                MenuItemEnums.提交);
            
            // 待审核状态：允许审核
            RegisterRule(PrePaymentStatus.待审核, 
                MenuItemEnums.审核);
            
            // 待核销状态：允许结案
            RegisterRule(PrePaymentStatus.待核销, 
                MenuItemEnums.结案);
            
            // 已生效状态：允许反审
            RegisterRule(PrePaymentStatus.已生效, 
                MenuItemEnums.反审);
            
            // 全额核销状态：允许反结案
            RegisterRule(PrePaymentStatus.全额核销, 
                MenuItemEnums.反结案);
        }

        /// <summary>
        /// 初始化ARAPStatus规则
        /// </summary>
        private void InitializeARAPStatusRules()
        {
            // 草稿状态：允许保存、修改、删除、提交
            RegisterRule(ARAPStatus.草稿, 
                MenuItemEnums.保存, 
                MenuItemEnums.修改, 
                MenuItemEnums.删除, 
                MenuItemEnums.提交);
            
            // 待审核状态：允许审核
            RegisterRule(ARAPStatus.待审核, 
                MenuItemEnums.审核);
            
            // 待支付状态：允许结案
            RegisterRule(ARAPStatus.待支付, 
                MenuItemEnums.结案);
            
            // 部分支付状态：允许结案
            RegisterRule(ARAPStatus.部分支付, 
                MenuItemEnums.结案);
            
            // 全部支付状态：允许反结案
            RegisterRule(ARAPStatus.全部支付, 
                MenuItemEnums.反结案);
        }

        /// <summary>
        /// 初始化PaymentStatus规则
        /// </summary>
        private void InitializePaymentStatusRules()
        {
            // 草稿状态：允许保存、修改、删除、提交
            RegisterRule(PaymentStatus.草稿, 
                MenuItemEnums.保存, 
                MenuItemEnums.修改, 
                MenuItemEnums.删除, 
                MenuItemEnums.提交);
            
            // 待支付状态：允许审核
            RegisterRule(PaymentStatus.待审核, 
                MenuItemEnums.审核);
            
            // 已支付状态：允许反结案
            RegisterRule(PaymentStatus.已支付, 
                MenuItemEnums.反结案);
        }

        #endregion
    }
}