using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

namespace RUINORERP.Business.Security
{
    public interface IAuthorizeController
    {
        bool GetQueryPageLayoutCustomize();
        bool GetQueryGridColCustomize();
        bool GetBillGridColCustomize();
        bool GetSaleLimitedAuth();
        bool GetOwnershipControl();
        bool GetPurBizLimitedAuth();
        bool GetExclusiveLimitedAuth();
        int GetMoneyDataPrecision();
        bool GetDebugInfoAuth();
        bool GetShowDebugInfoAuthorization();
    }


    /// <summary>
    /// 特殊授权管理器
    /// 系统配置管理器
    /// </summary>
    public class AuthorizeController : IAuthorizeController
    {
        private readonly ApplicationContext _context;

        public AuthorizeController(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool GetQueryPageLayoutCustomize()
        {
            return GetCustomizeOrDefault(_context.rolePropertyConfig?.QueryPageLayoutCustomize, _context.SysConfig.QueryPageLayoutCustomize);
        }

        public bool GetQueryGridColCustomize()
        {
            return GetCustomizeOrDefault(_context.rolePropertyConfig?.QueryGridColCustomize, _context.SysConfig.QueryGridColCustomize);
        }

        public bool GetBillGridColCustomize()
        {
            return GetCustomizeOrDefault(_context.rolePropertyConfig?.BillGridColCustomize, _context.SysConfig.BillGridColCustomize);
        }

        public bool GetSaleLimitedAuth()
        {
            return !_context.IsSuperUser && (_context.rolePropertyConfig?.SaleBizLimited ?? _context.SysConfig.SaleBizLimited);
        }

        public bool GetOwnershipControl()
        {
            return !_context.IsSuperUser && (_context.rolePropertyConfig?.OwnershipControl ?? _context.SysConfig.OwnershipControl);
        }

        public bool GetPurBizLimitedAuth()
        {
            return !_context.IsSuperUser && (_context.rolePropertyConfig?.PurchsaeBizLimited ?? _context.SysConfig.PurchsaeBizLimited);
        }

        public bool GetExclusiveLimitedAuth()
        {
            return !_context.IsSuperUser && (_context.rolePropertyConfig?.ExclusiveLimited ?? false);
        }

        public int GetMoneyDataPrecision()
        {
            return !_context.IsSuperUser ? (_context.rolePropertyConfig?.MoneyDataPrecision ?? _context.SysConfig.MoneyDataPrecision) : 4;
        }

        /// <summary>
        /// 系统是不是调试模式，如果是则可能会记录一些调试信息
        /// loginfo  debug
        /// </summary>
        /// <returns></returns>
        public bool IsDebug()
        {
            if (_context.SysConfig != null)
            {
                return _context.SysConfig?.IsDebug ?? _context.SysConfig.IsDebug;
            }
            else
            {
                return _context.IsSuperUser && _context.SysConfig.IsDebug;
            }
        }

        /// <summary>
        ///  显示调试信息权限
        /// </summary>
        /// <returns></returns>
        public bool GetDebugInfoAuth()
        {
            return GetShowDebugInfoAuthorization();
        }

        /// <summary>
        /// 先判断角色组权限，再判断系统配置。
        /// </summary>
        /// <returns></returns>
        public bool GetShowDebugInfoAuthorization()
        {

             if (_context.SysConfig.ShowDebugInfo)
             {
                 return _context.rolePropertyConfig?.ShowDebugInfo ?? _context.SysConfig.ShowDebugInfo;
             }
             else
             {
                 return _context.IsSuperUser && _context.SysConfig.IsDebug;
             }
        }    

        private bool GetCustomizeOrDefault(bool? roleProperty, bool sysConfigDefault)
        {
            return roleProperty ?? sysConfigDefault;
        }
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
        /// 获取角色的数据归属控制权，如果为真时则只能看到自己负责的数据
        /// 借入借出 这种 和  收款帐号。 自己的东西只能自己看自己处理？
        /// </summary>
        /// <param name="context"></param>
        /// <returns>不限制返回false</returns>
        public static bool GetOwnershipControl(ApplicationContext context)
        {
            if (context.IsSuperUser)
            {
                return false;
            }
            //如果对应的权限组没有配置，则使用系统级的默认值
            if (context.rolePropertyConfig == null)
            {
                return context.SysConfig.OwnershipControl;
            }
            else
            {
                return context.rolePropertyConfig.OwnershipControl;
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
                if (context.rolePropertyConfig.ShowDebugInfo)
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
