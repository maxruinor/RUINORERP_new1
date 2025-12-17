using System;
using System.Linq.Expressions;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using Krypton.Toolkit;
using SqlSugar;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 动态表达式构建器使用示例
    /// 展示如何在UCPurOrder等窗体中正确使用动态表达式构建器，完全避免硬编码字段名
    /// </summary>
    public static class DynamicExpressionBuilderUsageExample
    {
        /// <summary>
        /// 示例：为收款信息下拉框构建过滤表达式
        /// 替代硬编码的表达式：t => t.Is_enabled == true && t.CustomerVendor_ID == entity.CustomerVendor_ID
        /// 使用动态表达式构建器，完全动态处理表达式
        /// </summary>
        /// <param name="entity">采购订单实体</param>
        /// <param name="cmbPayeeInfoID">收款信息下拉框</param>
        public static void FilterPayeeInfoByCustomerVendor(tb_PurOrder entity, KryptonComboBox cmbPayeeInfoID)
        {
            // 使用动态表达式构建器，完全动态处理表达式，不预先指定字段名
            DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_FM_PayeeInfo>(
                key: "PayeeInfoID",
                value: "DisplayText",
                tableName: "tb_FM_PayeeInfo",
                cmbBox: cmbPayeeInfoID,
                expressionBuilder: q => q.And(t => t.Is_enabled == true)
                                  .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
            );
        }

        /// <summary>
        /// 示例：为供应商下拉框构建过滤表达式
        /// 替代硬编码的表达式：c => c.IsVendor == true && c.Is_enabled == true
        /// 使用动态表达式构建器，完全动态处理表达式
        /// </summary>
        /// <param name="cmbCustomerVendor_ID">供应商下拉框</param>
        public static void FilterCustomersByVendorStatus(KryptonComboBox cmbCustomerVendor_ID)
        {
            // 使用动态表达式构建器，完全动态处理表达式
            DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_CustomerVendor>(
                key: "CustomerVendor_ID",
                value: "CVName",
                tableName: "tb_CustomerVendor",
                cmbBox: cmbCustomerVendor_ID,
                expressionBuilder: q => q.And(c => c.IsVendor == true)
                                  .And(c => c.Is_enabled == true)
            );
        }

        /// <summary>
        /// 示例：根据多个条件过滤数据
        /// 使用动态表达式构建器，完全动态处理表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmbBox">下拉框</param>
        /// <param name="tableName">表名</param>
        /// <param name="expressionBuilder">表达式构建委托</param>
        public static void FilterByCustomExpression<T>(
            KryptonComboBox cmbBox,
            string tableName,
            Func<ISugarQueryable<T>, ISugarQueryable<T>> expressionBuilder) where T : class
        {
            // 使用动态表达式构建器，完全动态处理表达式
            DataBindingHelper.InitDataToCmbWithDynamicExpression<T>(
                key: "ID",
                value: "Name",
                tableName: tableName,
                cmbBox: cmbBox,
                expressionBuilder: expressionBuilder
            );
        }

        /// <summary>
        /// 示例：在UCPurOrder中使用动态表达式构建器替换原有代码
        /// </summary>
        /// <param name="entity">采购订单实体</param>
        /// <param name="cmbPayeeInfoID">收款信息下拉框</param>
        public static void UCPurOrderPayeeInfoFilter(tb_PurOrder entity, KryptonComboBox cmbPayeeInfoID)
        {
            // 原代码：
            // var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
            //     .And(t => t.Is_enabled == true)
            //     .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
            //     .ToExpression();
            
            // 新代码：使用动态表达式构建器，完全动态处理表达式
            DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_FM_PayeeInfo>(
                key: "PayeeInfoID",
                value: "DisplayText",
                tableName: "tb_FM_PayeeInfo",
                cmbBox: cmbPayeeInfoID,
                expressionBuilder: q => q.And(t => t.Is_enabled == true)
                                  .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
            );
        }

        /// <summary>
        /// 示例：使用动态表达式构建器创建复杂表达式
        /// </summary>
        public static void ComplexExpressionExample()
        {
            // 示例1：创建启用状态和ID匹配的组合表达式
            var expression1 = DynamicExpressionFactory.BuildFromSqlSugar<tb_FM_PayeeInfo>(
                q => q.And(t => t.Is_enabled == true)
                     .And(t => t.CustomerVendor_ID == 123)
            );

            // 示例2：创建自定义条件表达式
            var expression2 = DynamicExpressionFactory.BuildFromSqlSugar<tb_CustomerVendor>(
                q => q.And(c => c.IsVendor == true)
                     .And(c => c.Is_enabled == true)
                     .And(c => c.CVName.Contains("测试"))
            );

            // 示例3：直接使用表达式
            Expression<Func<tb_FM_PayeeInfo, bool>> directExpression = t => 
                t.Is_enabled == true && t.CustomerVendor_ID == 123;
            
            var processedExpression = DynamicExpressionFactory.Builder.ProcessExpression(directExpression);
        }

        /// <summary>
        /// 示例：处理复杂闭包变量表达式
        /// </summary>
        public static void ComplexClosureExample()
        {
            // 假设有以下变量
            var customerId = 123;
            var status = true;
            var keyword = "测试";
            
            // 创建包含闭包变量的表达式
            Expression<Func<tb_CustomerVendor, bool>> closureExpression = c =>
                c.Is_enabled == status && 
                c.CustomerVendor_ID == customerId && 
                c.CVName.Contains(keyword);
            
            // 使用动态表达式构建器处理闭包变量
            var processedExpression = DynamicExpressionFactory.Builder.ProcessExpression(closureExpression);
            
            // 安全地使用表达式进行过滤
            var sourceList = new List<tb_CustomerVendor>();
            var filteredList = DynamicExpressionFactory.SafeFilter(sourceList, processedExpression);
        }

        /// <summary>
        /// 示例：在数据库查询中使用动态表达式构建器
        /// </summary>
        public static void DatabaseQueryExample()
        {
            // 使用动态表达式构建器构建查询表达式
            var queryExpression = DynamicExpressionFactory.BuildFromSqlSugar<tb_PurOrder>(
                q => q.And(p => p.DataStatus == (int)DataStatus.确认)
                     .And(p => p.ApprovalStatus == (int)ApprovalStatus.审核通过)
                     .AndIf(true, p => p.Employee_ID == 123)  // 条件性添加表达式
            );
            
            // 可以在数据库查询中使用这个表达式
            // var db = Startup.GetFromFac<ISqlSugarClient>();
            // var results = db.Queryable<tb_PurOrder>().Where(queryExpression).ToList();
        }
    }
}