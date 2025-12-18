using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business.BNR
{
    public class BNRFactory
    {
        private Dictionary<string, IParameterHandler> mHandlers = new Dictionary<string, IParameterHandler>();
        private readonly DatabaseSequenceService _databaseSequenceService;
        private readonly ICacheManager<object> _cacheManager;
        [ThreadStatic]
        private static string _currentBusinessType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseSequenceService">数据库序列服务</param>
        /// <param name="cacheManager">缓存管理器</param>
        public BNRFactory(DatabaseSequenceService databaseSequenceService, ICacheManager<object> cacheManager)
        {
            _databaseSequenceService = databaseSequenceService;
            _cacheManager = cacheManager;
            Initialize();
        }

        ///// <summary>
        ///// 无参构造函数，用于兼容性
        ///// </summary>
        //public BNRFactory()
        //{
        //    Initialize();
        //}


        public IDictionary<string, IParameterHandler> Handlers
        {

            get
            {
                return mHandlers;
            }
        }

        /// <summary>
        /// 设置当前业务类型
        /// </summary>
        /// <param name="businessType">业务类型</param>
        public static void SetCurrentBusinessType(string businessType)
        {
            _currentBusinessType = businessType;
        }

        /// <summary>
        /// 获取当前业务类型
        /// </summary>
        /// <returns>当前业务类型</returns>
        public static string GetCurrentBusinessType()
        {
            return _currentBusinessType;
        }

        public void Initialize()
        {
            // 注册所有必要的参数处理器
            // 注册常量字符串处理器 {S:xxx}
            Register("S", new ConstantParameterHandler());
            // 注册日期处理器 {D:yyyyMMdd}
            Register("D", new DateParameterHandler());
            // 注册中文首字母处理器 {CN:中文}
            Register("CN", new ChineseSpellCodeParameter());
            // 注册十六进制日期处理器 {Hex:yyMM}
            Register("Hex", new HexDateSequenceParameter());
            
            // 注册Redis序列处理器 {redis:key/format}
            if (_cacheManager != null)
            {
                Register("redis", new RedisSequenceParameter(_cacheManager));
            }
            // 注册数据库序列处理器 {DB:key/format}
            if (_databaseSequenceService != null)
            {
                Register("DB", new DatabaseSequenceParameter(_databaseSequenceService));
            }
        }

        public void Register(System.Reflection.Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                Register(type);
            }
        }

        /// <summary>
        /// 11
        /// </summary>
        /// <param name="assembly"></param>
        public void Register(string name, RedisSequenceParameter rsp)
        {
            mHandlers[name] = rsp;
            mHandlers[name].Factory = this;
        }
        
        /// <summary>
        /// 注册数据库序号参数处理器
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="dsp">数据库序号参数处理器实例</param>
        public void Register(string name, DatabaseSequenceParameter dsp)
        {
            mHandlers[name] = dsp;
            mHandlers[name].Factory = this;
        }
        
        /// <summary>
        /// 注册通用参数处理器
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="handler">参数处理器实例</param>
        public void Register(string name, IParameterHandler handler)
        {
            mHandlers[name] = handler;
            mHandlers[name].Factory = this;
        }

        private static BNRFactory mDefault = null;

        public static BNRFactory Default
        {
            get
            {
                return mDefault;
            }
        }

        public void Register(Type type)
        {
            ParameterTypeAttribute[] result = (ParameterTypeAttribute[])type.GetCustomAttributes(typeof(ParameterTypeAttribute), false);
            if (result != null && result.Length > 0)
            {
                mHandlers[result[0].Name] = (IParameterHandler)Activator.CreateInstance(type);
                mHandlers[result[0].Name].Factory = this;
            }
        }

        //public void Register(RedisSequenceParameter type)
        //{
        //    ParameterTypeAttribute[] result = (ParameterTypeAttribute[])type.GetCustomAttributes(typeof(ParameterTypeAttribute), false);
        //    if (result != null && result.Length > 0)
        //    {
        //        mHandlers[result[0].Name] = (IParameterHandler)Activator.CreateInstance(type);
        //        mHandlers[result[0].Name].Factory = this;
        //    }
        //}

        public void Register<T>() where T : SequenceParameter
        {
            Register(typeof(T));
        }

        /// <summary>
        /// 根据规则创建编号
        /// </summary>
        /// <param name="rule">编号规则字符串，格式如：{S:ORD}{D:yyyyMMdd}{redis:ORD/00000}</param>
        /// <returns>生成的编号字符串</returns>
        /// <exception cref="ArgumentNullException">当规则字符串为null或空时抛出</exception>
        /// <exception cref="InvalidOperationException">当规则解析或执行过程中出错时抛出</exception>
        public string Create(string rule)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrEmpty(rule))
                {
                    throw new ArgumentNullException(nameof(rule), "编号规则不能为空");
                }
                
                // 执行规则解析
                string[] items = RuleAnalysis.Execute(rule);
                if (items == null || items.Length == 0)
                {
                    // 如果解析结果为空，可能是规则格式问题，尝试直接处理整个规则字符串
                    // 这种情况通常是没有使用占位符的简单字符串
                    return rule;
                }
                
                StringBuilder sb = new StringBuilder();
                
                // 处理每个解析出的参数项
                foreach (string item in items)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue; // 跳过空项
                    }
                    
                    try
                    {
                        string[] properties = RuleAnalysis.GetProperties(item);
                        if (properties == null || properties.Length < 2)
                        {
                            // 如果无法解析为属性，直接添加原始项
                            sb.Append(item);
                            continue;
                        }
                        
                        // 查找对应的参数处理器
                        string handlerName = properties[0].Trim();
                        IParameterHandler handler = null;
                        
                        if (mHandlers.TryGetValue(handlerName, out handler))
                        {
                            // 执行参数处理
                            handler.Execute(sb, properties[1]);
                        }
                        else
                        {
                            // 如果找不到处理器，添加警告信息但继续执行
                            // 这样可以避免因为单个未知处理器导致整个编号生成失败
                            System.Diagnostics.Debug.WriteLine($"警告：找不到参数处理器 '{handlerName}'，跳过处理项 '{item}'");
                            // 可以选择直接添加原始项，或者忽略
                            sb.Append(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录单个参数处理的错误，但不中断整体处理
                        System.Diagnostics.Debug.WriteLine($"处理参数项 '{item}' 时出错: {ex.Message}");
                        // 可以选择直接添加原始项，保证编号生成不中断
                        sb.Append(item);
                    }
                }
                
                return sb.ToString();
            }
            catch (Exception ex)
            {
                // 捕获所有异常并包装为更有意义的错误信息
                string errorMessage = $"编号生成失败 (规则: {rule}): {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                // 抛出异常，而不是返回空字符串，这样调用者可以捕获并处理异常
                throw new InvalidOperationException(errorMessage, ex);
            }
        }
    }
}