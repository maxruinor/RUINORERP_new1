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
            AddButtonRule(DataStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripButtonExport", true, true);

            // 新建状态：允许修改、保存、删除、审核
            AddButtonRule(DataStatus.新建, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnModify", true, true);
            AddButtonRule(DataStatus.新建, "toolStripButtonSave", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnDelete", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnSubmit", false, false);
            AddButtonRule(DataStatus.新建, "toolStripbtnReview", true, true);
            AddButtonRule(DataStatus.新建, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.新建, "toolStripButtonExport", true, true);

            // 确认状态：允许审核、结案、打印、导出
            AddButtonRule(DataStatus.确认, "toolStripbtnAdd", false, false);
            AddButtonRule(DataStatus.确认, "toolStripbtnModify", false, false);
            AddButtonRule(DataStatus.确认, "toolStripButtonSave", false, false);
            AddButtonRule(DataStatus.确认, "toolStripbtnDelete", false, false);
            AddButtonRule(DataStatus.确认, "toolStripbtnSubmit", false, false);
            AddButtonRule(DataStatus.确认, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.确认, "toolStripBtnReverseReview", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonCaseClosed", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.确认, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonExport", true, true);

            // 完结状态：允许反结案、打印、导出
            AddButtonRule(DataStatus.完结, "toolStripbtnAdd", false, false);
            AddButtonRule(DataStatus.完结, "toolStripbtnModify", false, false);
            AddButtonRule(DataStatus.完结, "toolStripButtonSave", false, false);
            AddButtonRule(DataStatus.完结, "toolStripbtnDelete", false, false);
            AddButtonRule(DataStatus.完结, "toolStripbtnSubmit", false, false);
            AddButtonRule(DataStatus.完结, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.完结, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.完结, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.完结, "toolStripButtonAntiClosed", true, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonExport", true, true);

            // 作废状态：允许打印、导出
            AddButtonRule(DataStatus.作废, "toolStripbtnAdd", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnModify", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonSave", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnDelete", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnSubmit", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.作废, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonPrint", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonExport", false, false);
        }

        /// <summary>
        /// 初始化付款状态UI按钮规则
        /// </summary>
        private static void InitializePaymentStatusUIButtonRules()
        {
            // 付款状态按钮规则
            // 草稿状态
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(PaymentStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonPrint", true, true);
            
            // 待审核状态
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonPrint", true, true);
            
            // 已支付状态
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnAdd", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnModify", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonSave", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnDelete", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnReview", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化预付款状态UI按钮规则
        /// </summary>
        private static void InitializePrePaymentStatusUIButtonRules()
        {
            // 预付款状态按钮规则
            // 草稿状态
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonPrint", true, true);
            
            // 待审核状态
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonPrint", true, true);
            
            // 已生效状态
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripBtnReverseReview", true, true);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonPrint", true, true);
            
            // 待核销状态
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonPrint", true, true);
            
            // 部分核销状态
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonPrint", true, true);
            
            // 全额核销状态
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripBtnReverseReview", true, true);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonPrint", true, true);
            
            // 已结案状态
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化应收应付状态UI按钮规则
        /// </summary>
        private static void InitializeARAPStatusUIButtonRules()
        {
            // 应收应付状态按钮规则
            // 草稿状态
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonPrint", true, true);
            
            // 待审核状态
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonPrint", true, true);
            
            // 待支付状态
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonPrint", true, true);
            
            // 部分支付状态
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonPrint", true, true);
            
            // 全部支付状态
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripBtnReverseReview", true, true);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonPrint", true, true);
            
            // 坏账状态
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripBtnReverseReview", true, true);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonPrint", true, true);
            
            // 已冲销状态
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonPrint", true, true);
        }
    }
}