/**
 * 文件: UIControlRules.cs
 * 说明: UI操作按钮状态规则管理类，统一管理UI操作按钮的状态规则
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 功能: 提供UI操作按钮状态规则的统一管理，控制操作按钮的显示、可用属性
 * 注意：此类控制的是操作按钮（如新增、保存、提交等），而不是具体业务控件
 * 
 * 使用方法:
 * // 初始化UI控件规则
 * var uiRules = UIControlRules.InitializeDefaultRules();
 * 
 * // 获取按钮状态规则
 * var buttonRules = UIControlRules.GetButtonRules<DataStatus>(DataStatus.草稿);
 * 
 * // 添加自定义规则
 * UIControlRules.AddButtonRule<DataStatus>(DataStatus.草稿, "btnSave", true);
 */

using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// UI操作按钮状态规则管理类
    /// 提供UI操作按钮状态规则的统一管理，控制操作按钮的显示、可用属性
    /// 注意：此类控制的是操作按钮（如新增、保存、提交等），而不是具体业务控件
    /// </summary>
    public static class UIControlRules
    {
        /// <summary>
        /// 线程安全的延迟加载UI操作按钮状态规则实例
        /// </summary>
        private static readonly Lazy<Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>> _lazyUIButtonRules = 
            new Lazy<Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool)>>>>(InitializeDefaultRulesInternal, true);

        /// <summary>
        /// 线程安全的延迟加载操作权限规则实例
        /// </summary>
        private static readonly Lazy<Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>> _lazyActionPermissionRules = 
            new Lazy<Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>>(InitializeActionPermissionRulesInternal, true);

        /// <summary>
        /// 获取全局唯一的UI操作按钮状态规则实例
        /// </summary>
        public static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> UIButtonRules => _lazyUIButtonRules.Value;

        /// <summary>
        /// 获取全局唯一的操作权限规则实例
        /// </summary>
        public static Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> ActionPermissionRules => _lazyActionPermissionRules.Value;

        /// <summary>
        /// UI操作按钮状态规则缓存（保持向后兼容）
        /// </summary>
        private static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiButtonRules => UIButtonRules;

        /// <summary>
        /// 操作权限规则缓存（保持向后兼容）
        /// </summary>
        private static Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> _actionPermissionRules => ActionPermissionRules;

        /// <summary>
        /// 初始化默认UI操作按钮状态规则
        /// </summary>
        /// <returns>UI操作按钮状态规则字典</returns>
        public static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> InitializeDefaultRules()
        {
            // 直接返回延迟加载的实例，避免重复初始化
            return UIButtonRules;
        }

        /// <summary>
        /// 初始化默认操作权限规则
        /// </summary>
        /// <returns>操作权限规则字典</returns>
        public static Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> InitializeActionPermissionRules()
        {
            // 直接返回延迟加载的实例，避免重复初始化
            return ActionPermissionRules;
        }

        /// <summary>
        /// 内部初始化默认UI操作按钮状态规则
        /// </summary>
        /// <returns>UI操作按钮状态规则字典</returns>
        private static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> InitializeDefaultRulesInternal()
        {
            var uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>();

            // 初始化数据状态UI按钮规则
            InitializeDataStatusUIButtonRules(uiButtonRules);

            // 初始化付款状态UI按钮规则
            InitializePaymentStatusUIButtonRules(uiButtonRules);

            // 初始化预付款状态UI按钮规则
            InitializePrePaymentStatusUIButtonRules(uiButtonRules);

            // 初始化应收应付状态UI按钮规则
            InitializeARAPStatusUIButtonRules(uiButtonRules);

            // 初始化EntityStatus UI按钮规则
            InitializeEntityStatusUIButtonRules(uiButtonRules);

            return uiButtonRules;
        }

        /// <summary>
        /// 内部初始化默认操作权限规则
        /// </summary>
        /// <returns>操作权限规则字典</returns>
        private static Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> InitializeActionPermissionRulesInternal()
        {
            var actionPermissionRules = new Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>();

            // 初始化DataStatus操作权限规则
            InitializeDataStatusActionPermissionRules(actionPermissionRules);

            // 初始化PaymentStatus操作权限规则
            InitializePaymentStatusActionPermissionRules(actionPermissionRules);

            // 初始化PrePaymentStatus操作权限规则
            InitializePrePaymentStatusActionPermissionRules(actionPermissionRules);

            // 初始化ARAPStatus操作权限规则
            InitializeARAPStatusActionPermissionRules(actionPermissionRules);

            // 初始化StatementStatus操作权限规则
            InitializeStatementStatusActionPermissionRules(actionPermissionRules);

            return actionPermissionRules;
        }

           

        /// <summary>
        /// 获取按钮状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>按钮状态规则字典</returns>
        public static Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules<T>(T status) where T : Enum
        {
            InitializeDefaultRules();

            var statusType = typeof(T);
            if (_uiButtonRules.ContainsKey(statusType) && _uiButtonRules[statusType].ContainsKey(status))
            {
                return _uiButtonRules[statusType][status];
            }

            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取按钮状态规则（根据状态类型和值）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>按钮状态规则字典</returns>
        public static Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules(Type statusType, object status)
        {
            InitializeDefaultRules();

            if (_uiButtonRules.ContainsKey(statusType) && _uiButtonRules[statusType].ContainsKey(status))
            {
                return _uiButtonRules[statusType][status];
            }

            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取按钮状态规则（基于EntityStatus）
        /// </summary>
        /// <param name="entityStatus">实体状态对象</param>
        /// <returns>按钮状态规则字典</returns>
        public static Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules(EntityStatus entityStatus)
        {
            if (entityStatus == null)
                return new Dictionary<string, (bool Enabled, bool Visible)>();

            InitializeDefaultRules();

            // 获取当前状态类型和值
            var statusType = entityStatus.CurrentStatusType;
            var statusValue = entityStatus.CurrentStatus;

            // 如果状态类型和值都有效，获取对应的按钮规则
            if (statusType != null && statusValue != null && _uiButtonRules.ContainsKey(statusType))
            {
                if (_uiButtonRules[statusType].ContainsKey(statusValue))
                {
                    return _uiButtonRules[statusType][statusValue];
                }
            }

            // 如果没有找到匹配的规则，返回空字典
            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取操作权限规则（基于状态类型和值）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>允许的操作列表</returns>
        public static List<MenuItemEnums> GetActionPermissionRules(Type statusType, object status)
        {
            InitializeActionPermissionRules();

            if (_actionPermissionRules.ContainsKey(statusType) && _actionPermissionRules[statusType].ContainsKey(status))
            {
                return _actionPermissionRules[statusType][status];
            }

            return new List<MenuItemEnums>();
        }

        /// <summary>
        /// 获取操作权限规则（基于EntityStatus）
        /// </summary>
        /// <param name="entityStatus">实体状态对象</param>
        /// <returns>允许的操作列表</returns>
        public static List<MenuItemEnums> GetActionPermissionRules(EntityStatus entityStatus)
        {
            if (entityStatus == null)
                return new List<MenuItemEnums>();

            InitializeActionPermissionRules();

            // 获取当前状态类型和值
            var statusType = entityStatus.CurrentStatusType;
            var statusValue = entityStatus.CurrentStatus;

            // 如果状态类型和值都有效，获取对应的操作权限规则
            if (statusType != null && statusValue != null && _actionPermissionRules.ContainsKey(statusType))
            {
                if (_actionPermissionRules[statusType].ContainsKey(statusValue))
                {
                    return _actionPermissionRules[statusType][statusValue];
                }
            }

            // 如果没有找到匹配的规则，返回空列表
            return new List<MenuItemEnums>();
        }

        /// <summary>
        /// 添加操作权限规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="actions">允许的操作列表</param>
        public static void AddActionPermissionRule<T>(T status, List<MenuItemEnums> actions) where T : Enum
        {
            var statusType = typeof(T);
            if (!ActionPermissionRules.ContainsKey(statusType))
            {
                ActionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();
            }

            ActionPermissionRules[statusType][status] = actions;
        }

        /// <summary>
        /// 添加操作权限规则（内部使用，支持传入字典）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="actions">允许的操作列表</param>
        /// <param name="rules">规则字典</param>
        private static void AddActionPermissionRule<T>(T status, List<MenuItemEnums> actions, Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> rules) where T : Enum
        {
            var statusType = typeof(T);
            if (!rules.ContainsKey(statusType))
            {
                rules[statusType] = new Dictionary<object, List<MenuItemEnums>>();
            }

            rules[statusType][status] = actions;
        }

        /// <summary>
        /// 添加按钮状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public static void AddButtonRule<T>(T status, string buttonName, bool enabled, bool visible = true) where T : Enum
        {
            var statusType = typeof(T);
            if (!UIButtonRules.ContainsKey(statusType))
            {
                UIButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();
            }

            if (!UIButtonRules[statusType].ContainsKey(status))
            {
                UIButtonRules[statusType][status] = new Dictionary<string, (bool Enabled, bool Visible)>();
            }

            UIButtonRules[statusType][status][buttonName] = (enabled, visible);
        }

        /// <summary>
        /// 添加按钮状态规则（内部使用，支持传入字典）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        /// <param name="rules">规则字典</param>
        private static void AddButtonRule<T>(T status, string buttonName, bool enabled, bool visible, Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> rules) where T : Enum
        {
            var statusType = typeof(T);
            if (!rules.ContainsKey(statusType))
            {
                rules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();
            }

            if (!rules[statusType].ContainsKey(status))
            {
                rules[statusType][status] = new Dictionary<string, (bool Enabled, bool Visible)>();
            }

            rules[statusType][status][buttonName] = (enabled, visible);
        }

        /// <summary>
        /// 初始化数据状态UI按钮规则
        /// </summary>
        private static void InitializeDataStatusUIButtonRules(Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> uiButtonRules)
        {
            var statusType = typeof(DataStatus);
            uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();

            // 草稿状态：允许新增、修改、保存、删除、提交
            AddButtonRule(DataStatus.草稿, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripbtnSubmit", true, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripButtonCaseClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripButtonAntiClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripButtonPrint", false, true, uiButtonRules);
            AddButtonRule(DataStatus.草稿, "toolStripButtonExport", true, true, uiButtonRules);

            // 新建状态：允许修改、保存、删除、审核
            AddButtonRule(DataStatus.新建, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripbtnSubmit", false, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripbtnReview", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripButtonCaseClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripButtonAntiClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripButtonPrint", true, true, uiButtonRules);
            AddButtonRule(DataStatus.新建, "toolStripButtonExport", true, true, uiButtonRules);

            // 确认状态：允许审核、结案、打印、导出
            AddButtonRule(DataStatus.确认, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripbtnModify", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripButtonSave", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripbtnDelete", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripbtnSubmit", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripbtnReview", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripBtnReverseReview", true, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripButtonCaseClosed", true, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripButtonAntiClosed", false, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripButtonPrint", true, true, uiButtonRules);
            AddButtonRule(DataStatus.确认, "toolStripButtonExport", true, true, uiButtonRules);

            // 完结状态：允许反结案、打印、导出
            AddButtonRule(DataStatus.完结, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripbtnModify", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripButtonSave", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripbtnDelete", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripbtnSubmit", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripbtnReview", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripBtnReverseReview", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripButtonCaseClosed", false, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripButtonAntiClosed", true, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripButtonPrint", true, true, uiButtonRules);
            AddButtonRule(DataStatus.完结, "toolStripButtonExport", true, true, uiButtonRules);

            // 作废状态：允许打印、导出
            AddButtonRule(DataStatus.作废, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripButtonCaseClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripButtonAntiClosed", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripButtonPrint", false, false, uiButtonRules);
            AddButtonRule(DataStatus.作废, "toolStripButtonExport", false, false, uiButtonRules);
        }

        /// <summary>
        /// 初始化数据状态操作权限规则
        /// </summary>
        private static void InitializeDataStatusActionPermissionRules(Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> actionPermissionRules)
        {
            var statusType = typeof(DataStatus);
            actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(DataStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 新建状态：允许修改、删除、保存、提交
            AddActionPermissionRule(DataStatus.新建, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 确认状态：允许修改、保存、提交、审核、反审
            AddActionPermissionRule(DataStatus.确认, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.提交,
                MenuItemEnums.审核,
                MenuItemEnums.反审
            }, actionPermissionRules);

            // 完结状态：允许查询、打印、导出
            AddActionPermissionRule(DataStatus.完结, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);

            // 作废状态：允许查询、删除
            AddActionPermissionRule(DataStatus.作废, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.删除
            });
        }

        /// <summary>
        /// 初始化付款状态UI按钮规则
        /// </summary>
        private static void InitializePaymentStatusUIButtonRules(Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> uiButtonRules)
        {
            // 付款状态按钮规则
            // 草稿状态
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnSubmit", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonPrint", true, true, uiButtonRules);

            // 待审核状态
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnReview", true, true, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonPrint", true, true, uiButtonRules);

            // 已支付状态
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonPrint", true, true, uiButtonRules);
        }

        /// <summary>
        /// 初始化付款状态操作权限规则
        /// </summary>
        private static void InitializePaymentStatusActionPermissionRules(Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> actionPermissionRules)
        {
            var statusType = typeof(PaymentStatus);
            actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(PaymentStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(PaymentStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            }, actionPermissionRules);

            // 已支付状态：允许查询、打印、导出
            AddActionPermissionRule(PaymentStatus.已支付, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);
        }

        /// <summary>
        /// 初始化预付款状态UI按钮规则
        /// </summary>
        private static void InitializePrePaymentStatusUIButtonRules(Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> uiButtonRules)
        {
            // 预付款状态按钮规则
            // 草稿状态
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnSubmit", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonPrint", true, true, uiButtonRules);

            // 待审核状态
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnReview", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonPrint", true, true, uiButtonRules);

            // 已生效状态
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripBtnReverseReview", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonPrint", true, true, uiButtonRules);

            // 待核销状态
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonPrint", true, true, uiButtonRules);

            // 部分核销状态
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonPrint", true, true, uiButtonRules);

            // 全额核销状态
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripBtnReverseReview", true, true, uiButtonRules);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonPrint", true, true, uiButtonRules);

            // 已结案状态
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonPrint", true, true, uiButtonRules);
        }

        /// <summary>
        /// 初始化预付款状态操作权限规则
        /// </summary>
        private static void InitializePrePaymentStatusActionPermissionRules(Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> actionPermissionRules)
        {
            var statusType = typeof(PrePaymentStatus);
            actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(PrePaymentStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(PrePaymentStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            }, actionPermissionRules);

            // 已生效状态：允许查询、打印、导出、反审
            AddActionPermissionRule(PrePaymentStatus.已生效, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            }, actionPermissionRules);

            // 待核销状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.待核销, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);

            // 部分核销状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.部分核销, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 全额核销状态：允许查询、打印、导出、反审
            AddActionPermissionRule(PrePaymentStatus.全额核销, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 已结案状态：允许查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.已结案, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });
        }

        /// <summary>
        /// 初始化应收应付状态UI按钮规则
        /// </summary>
        private static void InitializeARAPStatusUIButtonRules(Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> uiButtonRules)
        {
            // 应收应付状态按钮规则
            // 草稿状态
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnSubmit", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonPrint", true, true, uiButtonRules);

            // 待审核状态
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnAdd", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnDelete", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnReview", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonPrint", true, true, uiButtonRules);

            // 待支付状态
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonPrint", true, true, uiButtonRules);

            // 部分支付状态
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnModify", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonSave", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonPrint", true, true, uiButtonRules);

            // 全部支付状态
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripBtnReverseReview", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonPrint", true, true, uiButtonRules);

            // 坏账状态
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripBtnReverseReview", true, true, uiButtonRules);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonPrint", true, true, uiButtonRules);

            // 已冲销状态
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnAdd", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnModify", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonSave", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnDelete", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnSubmit", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripBtnReverseReview", false, false, uiButtonRules);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonPrint", true, true, uiButtonRules);
        }

        /// <summary>
        /// 初始化应收应付状态操作权限规则
        /// </summary>
        private static void InitializeARAPStatusActionPermissionRules(Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> actionPermissionRules)
        {
            var statusType = typeof(ARAPStatus);
            actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(ARAPStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(ARAPStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            }, actionPermissionRules);

            // 待支付状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(ARAPStatus.待支付, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);

            // 部分支付状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(ARAPStatus.部分支付, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);

            // 全部支付状态：允许查询、打印、导出、反审
            AddActionPermissionRule(ARAPStatus.全部支付, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            }, actionPermissionRules);

            // 坏账状态：允许查询、打印、导出、反审
            AddActionPermissionRule(ARAPStatus.坏账, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            }, actionPermissionRules);

            // 已冲销状态：允许查询、打印、导出
            AddActionPermissionRule(ARAPStatus.已冲销, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);
        }

        /// <summary>
        /// 初始化对账单状态操作权限规则
        /// </summary>
        private static void InitializeStatementStatusActionPermissionRules(Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> actionPermissionRules)
        {
            var statusType = typeof(StatementStatus);
            actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 未确认状态：允许修改、删除、保存、提交
            AddActionPermissionRule(StatementStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            }, actionPermissionRules);

            // 已确认状态：允许查询、打印、导出、反审
            AddActionPermissionRule(StatementStatus.已确认, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            }, actionPermissionRules);

            // 已结案状态：允许查询、打印、导出
            AddActionPermissionRule(StatementStatus.已结清, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            }, actionPermissionRules);
        }

        /// <summary>
        /// 初始化EntityStatus UI按钮规则
        /// 基于EntityStatus的当前状态类型和值，应用对应的UI按钮规则
        /// </summary>
        private static void InitializeEntityStatusUIButtonRules(Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> uiButtonRules)
        {
            // EntityStatus本身不需要定义规则，因为它会根据当前状态类型和值
            // 自动映射到对应的状态枚举规则（DataStatus、PaymentStatus等）
            // GetButtonRules(EntityStatus)方法会处理这种映射

            // 这里可以添加一些通用的EntityStatus规则，如果需要的话
            // 例如，基于审核状态(ApprovalStatus)的按钮规则

            // 审核状态规则
            // 未审核状态
            AddButtonRule(ApprovalStatus.未审核, "toolStripbtnReview", true, true, uiButtonRules); // 假设0表示未审核
            AddButtonRule(ApprovalStatus.未审核, "toolStripBtnReverseReview", false, false, uiButtonRules);

            // 已审核通过状态
            AddButtonRule(ApprovalStatus.审核通过, "toolStripbtnReview", false, false, uiButtonRules); // 假设1表示已审核通过
            AddButtonRule(ApprovalStatus.审核通过, "toolStripBtnReverseReview", true, true, uiButtonRules);

            // 已审核拒绝状态
            AddButtonRule(ApprovalStatus.审核驳回, "toolStripbtnReview", true, true, uiButtonRules); // 假设2表示已审核拒绝
            AddButtonRule(ApprovalStatus.审核驳回, "toolStripBtnReverseReview", false, false, uiButtonRules);
        }
    }
}