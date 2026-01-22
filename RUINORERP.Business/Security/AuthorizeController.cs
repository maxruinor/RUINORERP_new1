using RUINORERP.Common.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Model.ConfigModel;

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
        decimal GetAmountCalculationTolerance();
    }


    /// <summary>
    /// 特殊授权管理器
    /// 系统配置管理器
    /// 所有设置都是从个性化角色配置再到最终系统配置。按这个顺序来检查或生效。
    /// </summary>
    /// 
   // [NoWantIOCAttribute]
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

        /// <summary>
        /// 获取金额精度
        /// 优先级：角色配置 > 系统配置，如果都没有配置则返回默认值2
        /// </summary>
        /// <returns>金额小数位精度</returns>
        public int GetMoneyDataPrecision()
        {
            // 优先使用角色级配置，如果没有则使用系统级配置
            return _context.rolePropertyConfig?.MoneyDataPrecision ?? _context.SysConfig?.MoneyDataPrecision ?? 2;
        }

        /// <summary>
        /// 获取金额计算容差阈值
        /// 用于处理浮点数计算精度问题，适用于折扣、运费分摊等各种金额计算场景
        /// 当|计算金额-原始金额|≤阈值时按0处理
        /// 容差阈值会根据当前金额精度自动调整，确保不超过精度的最小值
        /// </summary>
        /// <returns>容差阈值，默认值基于金额精度：2位精度为0.01，3位精度为0.001，4位精度为0.0001</returns>
        public decimal GetAmountCalculationTolerance()
        {
            try
            {
                if (_context?.SysConfig?.FMConfig != null)
                {
                    var fmConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<FMConfiguration>(
                        _context.SysConfig.FMConfig);
                    if (fmConfig != null && fmConfig.AmountCalculationTolerance > 0)
                    {
                        // 获取当前金额精度
                        int moneyPrecision = GetMoneyDataPrecision();

                        // 计算最小允许的容差阈值（基于金额精度）
                        decimal minTolerance = (decimal)Math.Pow(10, -moneyPrecision);

                        // 确保配置的容差阈值不超过精度限制
                        decimal tolerance = fmConfig.AmountCalculationTolerance;

                        // 如果用户设置的容差阈值小于最小值，则使用最小值
                        if (tolerance < minTolerance)
                        {
                            tolerance = minTolerance;
                        }

                        // 不进行四舍五入，直接返回用户配置的值（如果大于最小值）
                        return tolerance;
                    }
                }
            }
            catch (Exception)
            {
                // 反序列化失败时使用默认值
            }

            // 根据金额精度返回默认容差阈值
            int precision = GetMoneyDataPrecision();
            return (decimal)Math.Pow(10, -precision);
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
        ///  这个意思是？待定
        ///  应该 是 是否允许自定义布局，
        ///  如果为直则可以，如果为假则不行只会加载越级用户设置的默认的。
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
        ///  这个意思是？待定
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
        /// 这个意思是？待定
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
        /// 借入借出 这种 和  收款账号。 自己的东西只能自己看自己处理？
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
        /// 获取金额精度，统一按系统配置中的金额精度参数
        /// </summary>
        /// <param name="context">应用上下文</param>
        /// <returns>系统配置中的金额精度，默认2位</returns>
        public static int GetMoneyDataPrecision(ApplicationContext context)
        {
            return context.SysConfig?.MoneyDataPrecision ?? 2;
        }


        /// <summary>
        /// 显示调试信息，如果错误信息会一直记录，不用判断。这个只是select语句的调试信息  etc
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true才显示</returns>
        public static bool GetShowDebugInfoAuthorization(ApplicationContext context)
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

        /// <summary>
        /// 是否启用财务模块
        /// </summary>
        /// <returns></returns>
        public bool EnableFinancialModule()
        {
            return  _context.SysConfig.EnableFinancialModule;
        }

        public bool EnableMultiCurrency()
        {
            return _context.SysConfig.EnableMultiCurrency;
        }

        public bool EnableContractModule()
        {
            return _context.SysConfig.EnableContractModule;
        }

        public bool EnableInvoiceModule()
        {
            return _context.SysConfig.EnableInvoiceModule;
        }

        public bool EnableVoucherModule()
        {
            return _context.SysConfig.EnableVoucherModule;
        }
    }
}
