namespace RUINORERP.TopServer.Model
{
    /// <summary>
    /// 注册服务结果模型
    /// </summary>
    public class RegistrationServiceResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 注册码（生成时）
        /// </summary>
        public string? RegistrationCode { get; set; }

        /// <summary>
        /// 公司注册信息（查询时）
        /// </summary>
        public CompanyRegistration? Registration { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int RemainingDays { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static RegistrationServiceResult Succeed(string message, string? registrationCode = null)
        {
            return new RegistrationServiceResult
            {
                Success = true,
                Message = message,
                RegistrationCode = registrationCode
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static RegistrationServiceResult Failed(string message)
        {
            return new RegistrationServiceResult
            {
                Success = false,
                Message = message
            };
        }

        /// <summary>
        /// 创建查询结果
        /// </summary>
        public static RegistrationServiceResult QueryResult(CompanyRegistration registration)
        {
            return new RegistrationServiceResult
            {
                Success = true,
                Message = "查询成功",
                Registration = registration,
                IsExpired = registration.IsExpired,
                RemainingDays = registration.RemainingDays
            };
        }
    }
}
