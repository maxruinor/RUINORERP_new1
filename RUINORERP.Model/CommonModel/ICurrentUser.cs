namespace RUINORERP.Model
{
    /// <summary>
    /// 当前用户接口
    /// 定义用户身份和会话信息的基本属性
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 用户ID - 用户表主键
        /// </summary>
        long UserID { get; set; }

        /// <summary>
        /// 用户名 - 登录名称
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 员工ID - 员工表外键
        /// </summary>
        long EmployeeId { get; set; }

        /// <summary>
        /// 显示姓名
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// 是否超级用户
        /// </summary>
        bool IsSuperUser { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        bool IsAuthorized { get; set; }

        /// <summary>
        /// 用户完整信息
        /// </summary>
        tb_UserInfo UserInfo { get; set; }
    }
}
