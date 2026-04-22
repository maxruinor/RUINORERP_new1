using CacheManager.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.BNR
{
    public class BNRFactory
    {
        private readonly ConcurrentDictionary<string, IParameterHandler> mHandlers = new ConcurrentDictionary<string, IParameterHandler>();
        private readonly DatabaseSequenceService _databaseSequenceService;
        private readonly ICacheManager<object> _cacheManager;
        
        // 使用AsyncLocal替代ThreadStatic，确保异步上下文正确传播
        private static readonly AsyncLocal<string> _currentBusinessType = new AsyncLocal<string>();

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
            _currentBusinessType.Value = businessType;
        }

        /// <summary>
        /// 获取当前业务类型
        /// </summary>
        /// <returns>当前业务类型</returns>
        public static string GetCurrentBusinessType()
        {
            return _currentBusinessType.Value;
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
            handler.Factory = this;
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
                var handler = (IParameterHandler)Activator.CreateInstance(type);
                mHandlers[result[0].Name] = handler;
                handler.Factory = this;
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
        /// <param name="rule">编号规则字符串，格式如：{S:ORD}{D:yyyyMMdd}{DB:ORDER/00000}</param>
        /// <returns>生成的编号字符串</returns>
        /// <exception cref="ArgumentNullException">当规则字符串为null或空时抛出</exception>
        /// <exception cref="InvalidOperationException">当规则解析或执行过程中出错时抛出</exception>
        public string Create(string rule)
        {
            if (string.IsNullOrEmpty(rule))
            {
                throw new ArgumentNullException(nameof(rule), "编号规则不能为空");
            }
            
            try
            {
                // 执行规则解析
                string[] items = RuleAnalysis.Execute(rule);
                if (items == null || items.Length == 0)
                {
                    // 如果解析结果为空，说明没有占位符，直接返回原始字符串
                    return rule;
                }
                
                StringBuilder sb = new StringBuilder();
                bool hasValidHandler = false;
                
                // 处理每个解析出的参数项
                foreach (string item in items)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    
                    try
                    {
                        string[] properties = RuleAnalysis.GetProperties(item);
                        if (properties == null || properties.Length < 2)
                        {
                            sb.Append(item);
                            continue;
                        }
                        
                        string handlerName = properties[0].Trim();
                        if (mHandlers.TryGetValue(handlerName, out IParameterHandler handler))
                        {
                            handler.Execute(sb, properties[1]);
                            hasValidHandler = true;
                        }
                        else
                        {
                            // 找不到处理器时，保留原始占位符以便排查，但记录警告
                            System.Diagnostics.Debug.WriteLine($"[BNR Warning] 未找到参数处理器 '{handlerName}'，规则: {rule}");
                            sb.Append($"{{{item}}}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 关键业务编号生成失败应抛出异常，而不是静默吞没
                        string errorMsg = $"处理参数项 '{item}' 时发生错误 (规则: {rule})";
                        System.Diagnostics.Debug.WriteLine($"[BNR Error] {errorMsg}: {ex.Message}");
                        throw new InvalidOperationException(errorMsg, ex);
                    }
                }
                
                string result = sb.ToString();
                
                // 如果结果中仍然包含花括号，说明有处理器执行失败或未被识别
                if (result.Contains("{") || result.Contains("}"))
                {
                    System.Diagnostics.Debug.WriteLine($"[BNR Warning] 生成的编号包含未处理的占位符: {result}");
                }
                
                return result;
            }
            catch (ArgumentNullException)
            {
                throw; // 重新抛出参数验证异常
            }
            catch (Exception ex)
            {
                string errorMessage = $"编号生成失败 (规则: {rule}): {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"[BNR Critical] {errorMessage}");
                throw new InvalidOperationException(errorMessage, ex);
            }
        }
    }
}