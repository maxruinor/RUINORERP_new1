using System.Collections.Generic;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限上下文
    /// 包含权限解析所需的上下文信息
    /// </summary>
    public class RowAuthContext
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 主角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 用户的所有角色ID列表
        /// </summary>
        public List<long> RoleIds { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public long? EmployeeId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 扩展属性字典,用于存储自定义参数
        /// </summary>
        public Dictionary<string, object> ExtendedProperties { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RowAuthContext()
        {
            RoleIds = new List<long>();
            ExtendedProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取扩展属性值
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="key">属性键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性值</returns>
        public T GetExtendedProperty<T>(string key, T defaultValue = default)
        {
            if (ExtendedProperties != null && ExtendedProperties.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置扩展属性值
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        public void SetExtendedProperty<T>(string key, T value)
        {
            if (ExtendedProperties == null)
            {
                ExtendedProperties = new Dictionary<string, object>();
            }
            ExtendedProperties[key] = value;
        }
    }
}
