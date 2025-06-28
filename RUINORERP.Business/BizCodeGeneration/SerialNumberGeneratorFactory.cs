using RUINORERP.Extensions.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizCodeGeneration
{
    public class SerialNumberGeneratorFactory
    {
        private static readonly Dictionary<GeneratorType, ISerialNumberGenerator> _generators = new Dictionary<GeneratorType, ISerialNumberGenerator>();

        static SerialNumberGeneratorFactory()
        {
            // 注册Redis生成器
            RegisterGenerator(GeneratorType.Redis, new RedisSerialNumberGenerator(RedisHelper.Db));

            // 可扩展其他生成器（如雪花算法）
            // RegisterGenerator(GeneratorType.Snowflake, new SnowflakeGenerator());
        }

        public static void RegisterGenerator(GeneratorType type, ISerialNumberGenerator generator)
        {
            _generators[type] = generator;
        }

        public static ISerialNumberGenerator GetGenerator(GeneratorType type = GeneratorType.Redis)
        {
            if (!_generators.ContainsKey(type))
                throw new ArgumentException($"未注册的生成器类型: {type}");
            return _generators[type];
        }
    }
}
