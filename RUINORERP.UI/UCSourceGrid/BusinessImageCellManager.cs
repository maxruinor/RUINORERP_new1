using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SourceGrid.Cells;
using SourceGrid.Cells.Models;
using RUINORERP.Common.Helper;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 业务图片单元格管理器1
    /// 专门用于处理SourceGrid单元格与业务图片的关联管理
    /// </summary>
    public class BusinessImageCellManager
    {
        /// <summary>
        /// 为单元格设置业务ID（加载数据时即使没有图片也要设置BusinessId）
        /// 这样用户后续添加新图片时才能获取到BusinessId
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="ownerTableName">业务表名</param>
        /// <param name="relatedField">关联字段名（可选）</param>
        public static void SetCellBusinessId(Cell cell, long businessId, string ownerTableName, string relatedField = null)
        {
            if (cell == null) return;

            try
            {
                // 获取或创建 ValueImageWeb 模型
                var model = cell.Model.FindModel(typeof(ValueImageWeb));
                ValueImageWeb valueImageWeb;
                if (model == null)
                {
                    valueImageWeb = new ValueImageWeb();
                    cell.Model.AddModel(valueImageWeb);
                }
                else
                {
                    valueImageWeb = (ValueImageWeb)model;
                }

                // 关键：设置业务ID，即使没有图片也要设置
                valueImageWeb.BusinessId = businessId;
                valueImageWeb.OwnerTableName = ownerTableName;

                // 如果提供了 RelatedField，也需要设置
                if (!string.IsNullOrEmpty(relatedField))
                {
                    valueImageWeb.RelatedField = relatedField;
                }

                // 如果单元格Tag是ImageInfo，也需要更新
                if (cell.Tag is RUINORERP.Lib.BusinessImage.ImageInfo imageInfo)
                {
                    imageInfo.BusinessId = businessId;
                    if (!string.IsNullOrEmpty(ownerTableName))
                    {
                        imageInfo.OwnerTableName = ownerTableName;
                    }
                    if (!string.IsNullOrEmpty(relatedField))
                    {
                        imageInfo.RelatedField = relatedField;
                    }
                }
            }
            catch
            {
                // 静默处理异常，避免影响主流程
            }
        }

        /// <summary>
        /// 为单元格设置业务ID（泛型版本，自动获取表名）
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="cell">单元格</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="relatedField">关联字段名（可选）</param>
        public static void SetCellBusinessId<T>(Cell cell, long businessId, string relatedField = null)
        {
            SetCellBusinessId(cell, businessId, typeof(T).Name, relatedField);
        }

        /// <summary>
        /// 为单元格设置业务ID（使用表达式指定关联字段）
        /// 使用示例：SetCellBusinessId(cell, businessId, (Product p) => p.ImagesPath)
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="cell">单元格</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="relatedFieldExpression">关联字段的表达式，如 (t) => t.ImagesPath</param>
        public static void SetCellBusinessId<T>(Cell cell, long businessId, Expression<Func<T, object>> relatedFieldExpression)
        {
            string relatedField = null;
            if (relatedFieldExpression != null)
            {
                relatedField = GetPropertyNameFromExpression(relatedFieldExpression);
            }
            SetCellBusinessId(cell, businessId, typeof(T).Name, relatedField);
        }

        /// <summary>
        /// 为单元格设置业务ID（完整泛型版本，使用表达式指定关联字段）
        /// 使用示例：SetCellBusinessId(cell, businessId, typeof(Product).Name, (Product p) => p.ImagesPath)
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="cell">单元格</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="ownerTableName">业务表名（如果为null则使用typeof(T).Name）</param>
        /// <param name="relatedFieldExpression">关联字段的表达式，如 (t) => t.ImagesPath</param>
        public static void SetCellBusinessId<T>(Cell cell, long businessId, string ownerTableName, Expression<Func<T, object>> relatedFieldExpression)
        {
            string relatedField = null;
            if (relatedFieldExpression != null)
            {
                relatedField = GetPropertyNameFromExpression(relatedFieldExpression);
            }
            string tableName = string.IsNullOrEmpty(ownerTableName) ? typeof(T).Name : ownerTableName;
            SetCellBusinessId(cell, businessId, tableName, relatedField);
        }

        /// <summary>
        /// 从表达式中获取属性名称
        /// </summary>
        private static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
                return null;

            // 处理直接属性访问，如 c => c.Property
            if (expression.Body is MemberExpression memberExpr &&
                memberExpr.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo.Name;
            }

            // 处理值类型装箱的情况，如 c => (object)c.IntProperty
            if (expression.Body is UnaryExpression unaryExpr &&
                unaryExpr.NodeType == ExpressionType.Convert &&
                unaryExpr.Operand is MemberExpression operandMemberExpr &&
                operandMemberExpr.Member is PropertyInfo operandPropertyInfo)
            {
                return operandPropertyInfo.Name;
            }

            throw new ArgumentException(
                "Expression must be a simple property access (e.g., c => c.Property)",
                nameof(expression));
        }

        /// <summary>
        /// 批量为单元格设置业务ID
        /// </summary>
        /// <param name="cellIdPairs">单元格和业务ID的键值对</param>
        /// <param name="ownerTableName">业务表名</param>
        public static void BatchSetCellBusinessId(IEnumerable<(Cell cell, long businessId)> cellIdPairs, string ownerTableName)
        {
            foreach (var (cell, businessId) in cellIdPairs)
            {
                SetCellBusinessId(cell, businessId, ownerTableName);
            }
        }
    }
}
