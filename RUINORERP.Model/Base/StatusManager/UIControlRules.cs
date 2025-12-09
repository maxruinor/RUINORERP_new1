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
        /// UI操作按钮状态规则缓存
        /// </summary>
        private static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiButtonRules;

        /// <summary>
        /// 初始化默认UI操作按钮状态规则
        /// </summary>
        /// <returns>UI操作按钮状态规则字典</returns>
        public static Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> InitializeDefaultRules()
        {
            if (_uiButtonRules != null)
                return _uiButtonRules;

            _uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>();

            // 初始化数据状态UI按钮规则
            InitializeDataStatusUIButtonRules();

            // 初始化付款状态UI按钮规则
            InitializePaymentStatusUIButtonRules();

            // 初始化预付款状态UI按钮规则
            InitializePrePaymentStatusUIButtonRules();

            // 初始化应收应付状态UI按钮规则
            InitializeARAPStatusUIButtonRules();

            return _uiButtonRules;
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
        /// 添加按钮状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public static void AddButtonRule<T>(T status, string buttonName, bool enabled, bool visible = true) where T : Enum
        {
            InitializeDefaultRules();

            var statusType = typeof(T);
            if (!_uiButtonRules.ContainsKey(statusType))
            {
                _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();
            }

            if (!_uiButtonRules[statusType].ContainsKey(status))
            {
                _uiButtonRules[statusType][status] = new Dictionary<string, (bool Enabled, bool Visible)>();
            }

            _uiButtonRules[statusType][status][buttonName] = (enabled, visible);
        }

        /// <summary>
        /// 初始化数据状态UI按钮规则
        /// </summary>
        private static void InitializeDataStatusUIButtonRules()
        {
            var statusType = typeof(DataStatus);
            _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();

            // 草稿状态：允许新增、修改、保存、删除、提交
            AddButtonRule(DataStatus.草稿, "btnAdd", true, true);
            AddButtonRule(DataStatus.草稿, "btnModify", true, true);
            AddButtonRule(DataStatus.草稿, "btnSave", true, true);
            AddButtonRule(DataStatus.草稿, "btnDelete", true, true);
            AddButtonRule(DataStatus.草稿, "btnSubmit", true, true);
            AddButtonRule(DataStatus.草稿, "btnApprove", false, false);
            AddButtonRule(DataStatus.草稿, "btnReverseApprove", false, false);
            AddButtonRule(DataStatus.草稿, "btnClose", false, false);
            AddButtonRule(DataStatus.草稿, "btnAntiClose", false, false);
            AddButtonRule(DataStatus.草稿, "btnPrint", true, true);
            AddButtonRule(DataStatus.草稿, "btnExport", true, true);

            // 新建状态：允许修改、保存、删除、审核
            AddButtonRule(DataStatus.新建, "btnAdd", true, true);
            AddButtonRule(DataStatus.新建, "btnModify", true, true);
            AddButtonRule(DataStatus.新建, "btnSave", true, true);
            AddButtonRule(DataStatus.新建, "btnDelete", true, true);
            AddButtonRule(DataStatus.新建, "btnSubmit", false, false);
            AddButtonRule(DataStatus.新建, "btnApprove", true, true);
            AddButtonRule(DataStatus.新建, "btnReverseApprove", false, false);
            AddButtonRule(DataStatus.新建, "btnClose", false, false);
            AddButtonRule(DataStatus.新建, "btnAntiClose", false, false);
            AddButtonRule(DataStatus.新建, "btnPrint", true, true);
            AddButtonRule(DataStatus.新建, "btnExport", true, true);

            // 确认状态：允许审核、结案、打印、导出
            AddButtonRule(DataStatus.确认, "btnAdd", false, false);
            AddButtonRule(DataStatus.确认, "btnModify", false, false);
            AddButtonRule(DataStatus.确认, "btnSave", false, false);
            AddButtonRule(DataStatus.确认, "btnDelete", false, false);
            AddButtonRule(DataStatus.确认, "btnSubmit", false, false);
            AddButtonRule(DataStatus.确认, "btnApprove", false, false);
            AddButtonRule(DataStatus.确认, "btnReverseApprove", true, true);
            AddButtonRule(DataStatus.确认, "btnClose", true, true);
            AddButtonRule(DataStatus.确认, "btnAntiClose", false, false);
            AddButtonRule(DataStatus.确认, "btnPrint", true, true);
            AddButtonRule(DataStatus.确认, "btnExport", true, true);

            // 完结状态：允许反结案、打印、导出
            AddButtonRule(DataStatus.完结, "btnAdd", false, false);
            AddButtonRule(DataStatus.完结, "btnModify", false, false);
            AddButtonRule(DataStatus.完结, "btnSave", false, false);
            AddButtonRule(DataStatus.完结, "btnDelete", false, false);
            AddButtonRule(DataStatus.完结, "btnSubmit", false, false);
            AddButtonRule(DataStatus.完结, "btnApprove", false, false);
            AddButtonRule(DataStatus.完结, "btnReverseApprove", false, false);
            AddButtonRule(DataStatus.完结, "btnClose", false, false);
            AddButtonRule(DataStatus.完结, "btnAntiClose", true, true);
            AddButtonRule(DataStatus.完结, "btnPrint", true, true);
            AddButtonRule(DataStatus.完结, "btnExport", true, true);

            // 作废状态：允许打印、导出
            AddButtonRule(DataStatus.作废, "btnAdd", false, false);
            AddButtonRule(DataStatus.作废, "btnModify", false, false);
            AddButtonRule(DataStatus.作废, "btnSave", false, false);
            AddButtonRule(DataStatus.作废, "btnDelete", false, false);
            AddButtonRule(DataStatus.作废, "btnSubmit", false, false);
            AddButtonRule(DataStatus.作废, "btnApprove", false, false);
            AddButtonRule(DataStatus.作废, "btnReverseApprove", false, false);
            AddButtonRule(DataStatus.作废, "btnClose", false, false);
            AddButtonRule(DataStatus.作废, "btnAntiClose", false, false);
            AddButtonRule(DataStatus.作废, "btnPrint", false, false);
            AddButtonRule(DataStatus.作废, "btnExport", false, false);
        }

        /// <summary>
        /// 初始化付款状态UI按钮规则
        /// </summary>
        private static void InitializePaymentStatusUIButtonRules()
        {
            // 付款状态按钮规则
            // 草稿状态
            AddButtonRule(PaymentStatus.草稿, "btnAdd", true, true);
            AddButtonRule(PaymentStatus.草稿, "btnModify", true, true);
            AddButtonRule(PaymentStatus.草稿, "btnSave", true, true);
            AddButtonRule(PaymentStatus.草稿, "btnDelete", true, true);
            AddButtonRule(PaymentStatus.草稿, "btnSubmit", true, true);
            AddButtonRule(PaymentStatus.草稿, "btnApprove", false, false);
            AddButtonRule(PaymentStatus.草稿, "btnReverseApprove", false, false);
            AddButtonRule(PaymentStatus.草稿, "btnPrint", true, true);
            
            // 待审核状态
            AddButtonRule(PaymentStatus.待审核, "btnAdd", true, true);
            AddButtonRule(PaymentStatus.待审核, "btnModify", true, true);
            AddButtonRule(PaymentStatus.待审核, "btnSave", true, true);
            AddButtonRule(PaymentStatus.待审核, "btnDelete", true, true);
            AddButtonRule(PaymentStatus.待审核, "btnSubmit", false, false);
            AddButtonRule(PaymentStatus.待审核, "btnApprove", true, true);
            AddButtonRule(PaymentStatus.待审核, "btnReverseApprove", false, false);
            AddButtonRule(PaymentStatus.待审核, "btnPrint", true, true);
            
            // 已支付状态
            AddButtonRule(PaymentStatus.已支付, "btnAdd", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnModify", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnSave", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnDelete", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnSubmit", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnApprove", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnReverseApprove", false, false);
            AddButtonRule(PaymentStatus.已支付, "btnPrint", true, true);
        }

        /// <summary>
        /// 初始化预付款状态UI按钮规则
        /// </summary>
        private static void InitializePrePaymentStatusUIButtonRules()
        {
            // 预付款状态按钮规则
            // 草稿状态
            AddButtonRule(PrePaymentStatus.草稿, "btnAdd", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "btnModify", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "btnSave", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "btnDelete", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "btnSubmit", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "btnReverseApprove", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "btnPrint", true, true);
            
            // 待审核状态
            AddButtonRule(PrePaymentStatus.待审核, "btnAdd", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "btnModify", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "btnSave", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "btnDelete", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "btnApprove", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "btnReverseApprove", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "btnPrint", true, true);
            
            // 已生效状态
            AddButtonRule(PrePaymentStatus.已生效, "btnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnModify", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnSave", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "btnReverseApprove", true, true);
            AddButtonRule(PrePaymentStatus.已生效, "btnPrint", true, true);
            
            // 待核销状态
            AddButtonRule(PrePaymentStatus.待核销, "btnAdd", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "btnModify", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "btnSave", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "btnDelete", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "btnReverseApprove", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "btnPrint", true, true);
            
            // 部分核销状态
            AddButtonRule(PrePaymentStatus.部分核销, "btnAdd", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "btnModify", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "btnSave", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "btnDelete", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "btnReverseApprove", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "btnPrint", true, true);
            
            // 全额核销状态
            AddButtonRule(PrePaymentStatus.全额核销, "btnAdd", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnModify", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnSave", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnDelete", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "btnReverseApprove", true, true);
            AddButtonRule(PrePaymentStatus.全额核销, "btnPrint", true, true);
            
            // 已结案状态
            AddButtonRule(PrePaymentStatus.已结案, "btnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnModify", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnSave", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnApprove", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnReverseApprove", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "btnPrint", true, true);
        }

        /// <summary>
        /// 初始化应收应付状态UI按钮规则
        /// </summary>
        private static void InitializeARAPStatusUIButtonRules()
        {
            // 应收应付状态按钮规则
            // 草稿状态
            AddButtonRule(ARAPStatus.草稿, "btnAdd", true, true);
            AddButtonRule(ARAPStatus.草稿, "btnModify", true, true);
            AddButtonRule(ARAPStatus.草稿, "btnSave", true, true);
            AddButtonRule(ARAPStatus.草稿, "btnDelete", true, true);
            AddButtonRule(ARAPStatus.草稿, "btnSubmit", true, true);
            AddButtonRule(ARAPStatus.草稿, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.草稿, "btnReverseApprove", false, false);
            AddButtonRule(ARAPStatus.草稿, "btnPrint", true, true);
            
            // 待审核状态
            AddButtonRule(ARAPStatus.待审核, "btnAdd", true, true);
            AddButtonRule(ARAPStatus.待审核, "btnModify", true, true);
            AddButtonRule(ARAPStatus.待审核, "btnSave", true, true);
            AddButtonRule(ARAPStatus.待审核, "btnDelete", true, true);
            AddButtonRule(ARAPStatus.待审核, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.待审核, "btnApprove", true, true);
            AddButtonRule(ARAPStatus.待审核, "btnReverseApprove", false, false);
            AddButtonRule(ARAPStatus.待审核, "btnPrint", true, true);
            
            // 待支付状态
            AddButtonRule(ARAPStatus.待支付, "btnAdd", false, false);
            AddButtonRule(ARAPStatus.待支付, "btnModify", true, true);
            AddButtonRule(ARAPStatus.待支付, "btnSave", true, true);
            AddButtonRule(ARAPStatus.待支付, "btnDelete", false, false);
            AddButtonRule(ARAPStatus.待支付, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.待支付, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.待支付, "btnReverseApprove", false, false);
            AddButtonRule(ARAPStatus.待支付, "btnPrint", true, true);
            
            // 部分支付状态
            AddButtonRule(ARAPStatus.部分支付, "btnAdd", false, false);
            AddButtonRule(ARAPStatus.部分支付, "btnModify", true, true);
            AddButtonRule(ARAPStatus.部分支付, "btnSave", true, true);
            AddButtonRule(ARAPStatus.部分支付, "btnDelete", false, false);
            AddButtonRule(ARAPStatus.部分支付, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.部分支付, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.部分支付, "btnReverseApprove", false, false);
            AddButtonRule(ARAPStatus.部分支付, "btnPrint", true, true);
            
            // 全部支付状态
            AddButtonRule(ARAPStatus.全部支付, "btnAdd", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnModify", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnSave", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnDelete", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.全部支付, "btnReverseApprove", true, true);
            AddButtonRule(ARAPStatus.全部支付, "btnPrint", true, true);
            
            // 坏账状态
            AddButtonRule(ARAPStatus.坏账, "btnAdd", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnModify", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnSave", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnDelete", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.坏账, "btnReverseApprove", true, true);
            AddButtonRule(ARAPStatus.坏账, "btnPrint", true, true);
            
            // 已冲销状态
            AddButtonRule(ARAPStatus.已冲销, "btnAdd", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnModify", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnSave", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnDelete", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnSubmit", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnApprove", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnReverseApprove", false, false);
            AddButtonRule(ARAPStatus.已冲销, "btnPrint", true, true);
        }
    }
}