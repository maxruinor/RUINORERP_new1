using CacheManager.Core;
using RUINORERP.Server.BNR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Server.BNR
{
    public class BNRFactory
    {
        private Dictionary<string, IParameterHandler> mHandlers = new Dictionary<string, IParameterHandler>();
        private readonly DatabaseSequenceService _databaseSequenceService;
        private readonly ICacheManager<object> _cacheManager;

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

        /// <summary>
        /// 无参构造函数，用于兼容性
        /// </summary>
        public BNRFactory()
        {
            Initialize();
        }


        public IDictionary<string, IParameterHandler> Handlers
        {

            get
            {
                return mHandlers;
            }
        }

        public void Initialize()
        {
            // 只注册可用的参数处理器
            if (_cacheManager != null)
            {
                Register("redis", new RedisSequenceParameter(_cacheManager));
            }
            if (_databaseSequenceService != null)
            {
                Register("DB", new DatabaseSequenceParameter(_databaseSequenceService));
            }
            // 其他参数处理器可能需要通过其他方式注册或实现
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

        private static BNRFactory mDefault = null;

        public static BNRFactory Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new BNRFactory();
                    mDefault.Initialize();
                }
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
        /// 规则中可能有空格，在这个方法中全去掉了。
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public string Create(string rule)
        {
            string[] items = RuleAnalysis.Execute(rule);
            StringBuilder sb = new StringBuilder();
            foreach (string item in items)
            {
                string[] properties = RuleAnalysis.GetProperties(item);
                IParameterHandler handler = null;
                if (mHandlers.TryGetValue(properties[0].Trim(), out handler))
                {
                    handler.Execute(sb, properties[1]);
                }
            }
            return sb.ToString();
        }
    }
}