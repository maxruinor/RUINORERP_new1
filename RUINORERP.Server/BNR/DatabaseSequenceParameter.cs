using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RUINORERP.Server.BNR
{
    /// <summary>
    /// {DB:ORDER/00000} 
    /// 基于数据库的序号参数
    /// </summary>
    [ParameterType("DB")]
    public class DatabaseSequenceParameter : IParameterHandler
    {
        // 数据库序号服务
        private readonly DatabaseSequenceService _sequenceService;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService">数据库序列服务</param>
        public DatabaseSequenceParameter(DatabaseSequenceService sequenceService)
        {
            _sequenceService = sequenceService;
        }

        public BNRFactory Factory { get; set; }

        public void Execute(StringBuilder sb, string value)
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
            
            var sequenceKey = key.ToString(); // 参数名 {DB:{S:ORDER}{D:dd}/0000}  //key按天增加
            var number = _sequenceService.GetNextSequenceValue(sequenceKey);
            sb.Append(number.ToString(properties[1]));
        }
        
        // 移除静态方法，现在使用依赖注入

    }
}