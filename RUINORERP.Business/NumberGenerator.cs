using RUINORERP.Extensions.Redis;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class NumberGenerator
    {
        public static string GetProdNo(tb_ProdCategories pc)
        {
            // string rule = "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";
            RedisHelper.ConnectionString = "192.168.0.254:6379";
            RedisHelper.DbNum = 1;
            RedisHelper.Db.StringSet("a", "AAA");
            RedisHelper.Db.StringIncrement("aa");
            //RedisHelper.StringSet("a", "456");
            string BizCode = RedisHelper.Db.StringGet("a").ToString() + RedisHelper.Db.StringGet("aa").ToString();// BNRFactory.Default.Create(rule);
            return BizCode;
        }
    }
}
