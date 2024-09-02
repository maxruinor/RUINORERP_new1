using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

namespace RUINORERP.Business.Security
{

    /// <summary>
    /// 特殊授权管理器
    /// 系统配置管理器
    /// </summary>
    public class AuthorizeController
    {

        /// <summary>
        /// 查询页布局自定义
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetQueryPageLayoutCustomize(ApplicationContext context)
        {
            //如果对应的权限组没有配置，则使用系统级的默认值
            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.QueryPageLayoutCustomize;
            }
            else
            {
                return context.rolePropertyConfig.QueryPageLayoutCustomize;
            }
        }



        /// <summary>
        /// 查询表格列自定义
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetQueryGridColCustomize(ApplicationContext context)
        {
            //如果对应的权限组没有配置，则使用系统级的默认值
            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.QueryGridColCustomize;
            }
            else
            {
                return context.rolePropertyConfig.QueryGridColCustomize;
            }
        }


        /// <summary>
        /// 单据表格列自定义
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetBillGridColCustomize(ApplicationContext context)
        {
            //如果对应的权限组没有配置，则使用系统级的默认值
            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.BillGridColCustomize;
            }
            else
            {
                return context.rolePropertyConfig.BillGridColCustomize;
            }
        }




        /// <summary>
        /// 获取角色的销售范围限制GetSaleLimitedAuthorization
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetSaleLimitedAuth(ApplicationContext context)
        {
            if (context.IsSuperUser)
            {
                return false;
            }
            //如果对应的权限组没有配置，则使用系统级的默认值
            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.SaleBizLimited;
            }
            else
            {
                return context.rolePropertyConfig.SaleBizLimited;
            }
        }


        /// <summary>
        /// 获取角色的采购范围限制GetPurchsaeBizLimitedAuthorization
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetPurBizLimitedAuth(ApplicationContext context)
        {
            if (context.IsSuperUser)
            {
                return false;
            }

            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.PurchsaeBizLimited;
            }
            else
            {
                return context.rolePropertyConfig.PurchsaeBizLimited;
            }
        }




        /// <summary>
        /// 获取角色中属性的责任人独占是否启用
        /// 如果这个角色中启用了，则客户资料中勾选了专属责任人的，只能本人能查到数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns>启用为真</returns>
        public static bool GetExclusiveLimitedAuth(ApplicationContext context)
        {
            bool rs = false;
            if (context.IsSuperUser)
            {
                return false;
            }
            if (context.rolePropertyConfig != null)
            {
                rs = context.rolePropertyConfig.ExclusiveLimited;
            }
            return rs;
        }


        /// <summary>
        /// 获取角色的金额精度
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static int GetMoneyDataPrecision(ApplicationContext context)
        {
            if (context.IsSuperUser)
            {
                return 4;
            }

            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.MoneyDataPrecision;
            }
            else
            {
                return context.rolePropertyConfig.MoneyDataPrecision;
            }
        }


        /// <summary>
        /// 显示调试信息，如果错误信息会一直记录，不用判断。这个只是select语句的调试信息  etc
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true才显示</returns>
        public static bool GetShowDebugInfoAuthorization(ApplicationContext context)
        {
            //先看系统级的配置
            if (context.SysConfig.ShowDebugInfo)
            {
                //角色中没有配置，则按系统级取值
                if (context.rolePropertyConfig == null)
                {
                    return context.SysConfig.ShowDebugInfo;
                }
                else
                {
                    return context.rolePropertyConfig.ShowDebugInfo;
                }
            }
            else
            {
                //关掉系统的。如果是超级用户 就看是不是调试状态
                if (context.IsSuperUser && context.SysConfig.IsDebug)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
