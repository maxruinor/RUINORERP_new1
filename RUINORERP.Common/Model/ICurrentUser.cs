using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RUINORERP.Common.Model

{
    /// <summary>
    /// 当前用户 认为任何系统都会有一个当前用户
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        long Id { get; set; }

        DateTime CreateTime { get; set; }
        string CreateBy { get; set; }

        DateTime UpdateTime { get; set; }
        string UpdateBy { get; set; }

        bool IsDeleted { get; set; }

        ///// <summary>
        ///// 请求携带的Token
        ///// </summary>
        ///// <returns></returns>
        //string GetToken();

        /////// <summary>
        /////// 是否已认证
        /////// </summary>
        /////// <returns></returns>
        ////bool IsAuthenticated();

        ///// <summary>
        ///// 获取用户身份权限
        ///// </summary>
        ///// <returns></returns>
        //IEnumerable<Claim> GetClaimsIdentity();

        ///// <summary>
        ///// 根据权限类型获取详细权限
        ///// </summary>
        ///// <param name="claimType"></param>
        ///// <returns></returns>
        //List<string> GetClaimValueByType(string claimType);

        //List<string> GetUserInfoFromToken(string claimType);


    }
}