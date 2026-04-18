using System;
using System.Collections.Generic;
using System.Data;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 验证错误信息
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// 行号（Excel行号，从1开始）
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误类型（Required / TypeMismatch / ForeignKeyNotFound / UniqueViolation等）
        /// </summary>
        public string ErrorType { get; set; }
    }

    /// <summary>
    /// 数据验证服务接口
    /// 负责导入前的数据验证，包括必填项、数据类型、外键关联等
    /// </summary>
    public interface IDataValidationService
    {
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="data">待验证的数据表</param>
        /// <param name="config">验证配置</param>
        /// <returns>验证错误列表，如果为空表示验证通过</returns>
        List<ValidationError> Validate(DataTable data, Models.ValidationConfig config);

        /// <summary>
        /// 检查唯一性约束
        /// 验证导入数据中的唯一性字段是否已存在于数据库中
        /// </summary>
        /// <param name="data">待验证的数据表</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="uniqueFields">唯一性字段列表</param>
        /// <returns>重复记录错误列表</returns>
        List<ValidationError> CheckUniqueValues(
            DataTable data, 
            string tableName, 
            List<string> uniqueFields);
    }
}
