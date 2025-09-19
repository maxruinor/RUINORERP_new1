using System;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 通用基类模型 - 所有模型的基础抽象类
    /// 提供创建时间、更新时间、时间戳等通用属性和验证方法
    /// 同时整合了包信息和命令相关的通用属性和方法
    /// </summary>
    public abstract class BaseModel
    {
        #region 通用属性
        
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";
        
        #endregion

        #region 包相关信息属性
        
        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }
        
        #endregion

        #region 命令相关信息属性
        
        /// <summary>
        /// 命令执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 执行结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }
        
        /// <summary>
        /// 兼容性属性 - 成功标志
        /// </summary>
        public bool Success 
        { 
            get => IsSuccess; 
            set => IsSuccess = value; 
        }
        
        #endregion

        #region 通用方法
        
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public virtual bool IsValid()
        {
            return CreatedTime <= DateTime.UtcNow &&
                   CreatedTime >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            // 清理包体数据
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return Body?.Length ?? 0;
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>模型信息字符串</returns>
        public override string ToString()
        {
            return $"BaseModel[Created:{CreatedTime:yyyy-MM-dd HH:mm:ss} UTC]";
        }
        
        #endregion
    }
}
