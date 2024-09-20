using Microsoft.Extensions.Configuration;

namespace RUINORERP.WebServerConsole.Comm
{
    public class AppConfig
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 读取appsettings.json配置文件，需引用包：
        /// Microsoft.Extensions.Configuration
        /// Microsoft.Extensions.Configuration.FileExtensions
        /// Microsoft.Extensions.Configuration.Json
        /// </summary>
        static AppConfig()
        {
            #if DEBUG
                   Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            #else
                   Configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            #endif
        }

    }
}