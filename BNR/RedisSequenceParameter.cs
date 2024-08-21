using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace BNR
{

    /// <summary>
    /// {RedisSeq:ORDER/00000} 
    /// 序号参数
    /// </summary>
    [ParameterType("redis")]
    public class RedisSequenceParameter : IParameterHandler
    {
        public RedisSequenceParameter()
        {

        }
        public RedisSequenceParameter(IDatabase db)
        {
            mRedisDB = db;
        }

        private IDatabase mRedisDB;
        public BNRFactory Factory { get; set; }


      
        public  void Execute(StringBuilder sb, string value)
        {
            string[] properties = value.Split('/');
            StringBuilder key = new StringBuilder();
            string[] items = RuleAnalysis.Execute(properties[0]);
            foreach (string p in items)
            {
                string[] sps = RuleAnalysis.GetProperties(p);
                IParameterHandler handler = null;
                if (Factory.Handlers.TryGetValue(sps[0], out handler))
                {
                    handler.Execute(key, sps[1]);
                }
            }
            var redisKey = key.ToString();//参数名 {redis:{S:ORDER}{D:dd}/0000}  //key按天增加
            var number =  mRedisDB.StringIncrement(redisKey);
            sb.Append(number.ToString(properties[1]));

        }
    
      
        /*
        public void Execute(StringBuilder sb, string value)
        {
            string[] properties = value.Split('/');
            StringBuilder key = new StringBuilder();
            string[] items = RuleAnalysis.Execute(properties[0]);
            foreach (string p in items)
            {
                string[] sps = RuleAnalysis.GetProperties(p);
                key.Append(sps[0]);
                break;
            }
            var redisKey = key.ToString();
            var number = mRedisDB.StringIncrement(redisKey);
            sb.Append(number.ToString(properties[1]));

        }
        */

        //void IParameterHandler.Execute(StringBuilder sb, string value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
