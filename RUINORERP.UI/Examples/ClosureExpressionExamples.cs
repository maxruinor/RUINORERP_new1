using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RUINORERP.UI.Common;
using RUINORERP.Model;

namespace RUINORERP.UI.Examples
{
    /// <summary>
    /// 表达式闭包变量处理架构示例
    /// 展示如何使用增强的ExpressionSafeHelper处理包含闭包变量的强类型表达式
    /// </summary>
    public class ClosureExpressionExamples
    {
        /// <summary>
        /// 示例1：展示如何在数据绑定中使用包含闭包变量的表达式
        /// 这种方式保持了强类型检查和智能提示的优势
        /// </summary>
        public static void Example_DataBindingWithClosureExpression()
        {
            // 模拟实体和控件
            var entity = new tb_PurOrder { CustomerVendor_ID = 123 };
            var sourceList = new List<tb_FM_PayeeInfo>();
            
            // 原始表达式：包含闭包变量 entity.CustomerVendor_ID
            // 这种写法有编译时检查和智能提示的优势
            Expression<Func<tb_FM_PayeeInfo, bool>> expression = 
                t => t.Is_enabled == true && t.CustomerVendor_ID == entity.CustomerVendor_ID;
            
            // 使用增强的SafeFilterList方法处理闭包变量
            // 底层架构会自动分析表达式并提取闭包变量值
            var filteredList = DataBindingHelper.SafeFilterList(sourceList, expression);
            
            Console.WriteLine($"筛选结果: 找到 {filteredList.Count} 条记录");
        }
        
        /// <summary>
        /// 示例2：展示多种闭包变量表达式的处理
        /// </summary>
        public static void Example_MultipleClosureVariables()
        {
            // 模拟多个实体
            var customer = new tb_CustomerVendor { CustomerVendor_ID = 100, IsVendor = true };
            var user = new tb_Employee { Employee_ID = 500, Is_enabled = true };
            var currentDate = DateTime.Now;
            
            var sourceList = new List<tb_SaleOrder>();
            
            // 复杂表达式：包含多个闭包变量
            Expression<Func<tb_SaleOrder, bool>> complexExpression = 
                order => order.CustomerVendor_ID == customer.CustomerVendor_ID &&
                         order.Employee_ID == user.Employee_ID &&
                         order.OrderDate >= currentDate.AddDays(-30) &&
                         order.DataStatus == (int)DataStatus.确认;
            
            // 使用SafeFilterList处理复杂表达式
            var filteredList = DataBindingHelper.SafeFilterList(sourceList, complexExpression);
            
            Console.WriteLine($"复杂表达式筛选结果: 找到 {filteredList.Count} 条记录");
        }
        
        /// <summary>
        /// 示例3：展示逻辑运算符组合的闭包变量表达式
        /// </summary>
        public static void Example_LogicalOperatorsWithClosures()
        {
            var entity = new tb_PurOrder { CustomerVendor_ID = 123, PayStatus = 1 };
            var sourceList = new List<tb_FM_PayeeInfo>();
            
            // 包含OR和AND的复杂逻辑表达式
            Expression<Func<tb_FM_PayeeInfo, bool>> logicalExpression = 
                t => (t.Is_enabled == true && t.CustomerVendor_ID == entity.CustomerVendor_ID) ||
                     (t.Account_type == AccountType.对私 && t.PayStatus == entity.PayStatus);
            
            // 使用SafeFilterList处理逻辑表达式
            var filteredList = DataBindingHelper.SafeFilterList(sourceList, logicalExpression);
            
            Console.WriteLine($"逻辑表达式筛选结果: 找到 {filteredList.Count} 条记录");
        }
        
        /// <summary>
        /// 示例4：展示表达式架构的内部工作原理
        /// </summary>
        public static void Example_InternalArchitectureWorking()
        {
            var entity = new tb_PurOrder { CustomerVendor_ID = 123 };
            var sourceList = new List<tb_FM_PayeeInfo>();
            
            Expression<Func<tb_FM_PayeeInfo, bool>> expression = 
                t => t.Is_enabled == true && t.CustomerVendor_ID == entity.CustomerVendor_ID;
            
            try
            {
                // 1. 首先尝试直接编译表达式（会失败，因为有闭包变量）
                var compiledExpression = expression.Compile();
                // 这里会抛出异常：从作用域""引用了"RUINORERP.Model.tb_FM_PayeeInfo"类型的变量"t"，但该变量未定义
            }
            catch (Exception ex)
            {
                Console.WriteLine($"直接编译表达式失败: {ex.Message}");
                
                // 2. 使用增强的SafeFilterList方法
                var filteredList = DataBindingHelper.SafeFilterList(sourceList, expression);
                
                // 底层架构的工作流程：
                // a) 使用TryFilterWithConditionExtraction尝试提取条件
                // b) 如果失败，使用ManualFilterWithClosureVariables处理闭包变量
                // c) 闭包变量解析器分析表达式，提取条件和闭包变量值
                // d) 根据提取的条件评估每个项目
                
                Console.WriteLine($"使用增强架构处理成功: 找到 {filteredList.Count} 条记录");
            }
        }
    }
    
    /// <summary>
    /// 架构设计总结：
    /// 
    /// 1. 保持强类型表达式写法的优势：
    ///    - 编译时类型检查
    ///    - IDE智能提示
    ///    - 重构安全性
    /// 
    /// 2. 增强底层架构处理闭包变量：
    ///    - 通用闭包变量解析器
    ///    - 支持任意类型的条件和逻辑组合
    ///    - 自动提取闭包变量值
    /// 
    /// 3. 兼容现有代码：
    ///    - 不需要修改现有的使用方式
    ///    - 自动降级处理复杂表达式
    ///    - 保持性能优化
    /// 
    /// 4. 扩展性：
    ///    - 支持未来更复杂的表达式类型
    ///    - 易于调试和日志记录
    ///    - 可以进一步优化性能
    /// </summary>
}