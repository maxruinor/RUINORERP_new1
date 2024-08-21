using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BNR
{
    /*
    static long mCount;

        static void Main(string[] args)
        {
            //{N:ORDER2016/0000}
            //string[] rule = new string[]{ 
            //    "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}",
            //    "{S:OD}{CN:深圳}{D:yyyyMM}{N:{S:ORDER}{CN:深圳}{D:yyyyMM}/00000000}",
            //    "{S:SQ}{D:yyyy}{N:{S:SQ}{D:yyyy}/00000000}"
            //};
            string[] rule = new string[]{ 
               
                "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}",
                "{CN:广州}{D:yyyyMMdd}{N:{D:yyyyMMdd}/0000}{S:RJ}"
            };
            foreach (string item in rule)
            {
                string value = BNRFactory.Default.Create(item);
                Console.WriteLine(item);
                Console.WriteLine(value);
            }
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    foreach (string item in rule)
                    {
                        string value = BNRFactory.Default.Create(item);
                        // Console.WriteLine("{0}={1}", item, value);
                        System.Threading.Interlocked.Increment(ref mCount);
                    }
                }
            });
            while (true)
            {
                Console.WriteLine(mCount);
                System.Threading.Thread.Sleep(1000);
            }
            Console.Read();
        }
     */
    public class BNRFactory
    {
        private Dictionary<string, IParameterHandler> mHandlers = new Dictionary<string, IParameterHandler>();


        public IDictionary<string, IParameterHandler> Handlers
        {

            get
            {
                return mHandlers;
            }
        }

        public void Initialize()
        {
            Register(typeof(BNRFactory).Assembly);
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
