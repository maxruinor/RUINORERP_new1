namespace RUINORERP.Common.SnowflakeIdHelper
{

    /// <summary>
    /// 雪花ID生成工具
    /// </summary>
    public static class IdHelper
    {
        internal static SnowflakeIdWorker IdWorker { get; set; }

        internal static IdHelperBootstrapper IdHelperBootstrapper { get; set; }


        //public static long WorkerId { get => IdWorker.workerId; }

        /// <summary>
        /// 获取String型雪花Id
        /// </summary>
        /// <returns></returns>
        public static string GetId()
        {
            return GetLongId().ToString();
        }

        /// <summary>
        /// 获取long型雪花Id
        /// </summary>
        /// <returns></returns>
        public static long GetLongId()
        {
            // 如果IdWorker未初始化，则自动初始化，目前客户端和服务器都执行了初始化：new IdHelperBootstrapper().SetWorkderId(1).Boot();
            if (IdWorker == null)
            {
                // 使用默认配置初始化雪花ID生成器
                IdHelperBootstrapper = new IdHelperBootstrapper().SetWorkderId(1);
                IdHelperBootstrapper.Boot();
            }
            
            return IdWorker.NextId();
        }
    }
}