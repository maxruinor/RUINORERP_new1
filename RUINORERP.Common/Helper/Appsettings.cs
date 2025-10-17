using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Common.Helper
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class AppSettings
    {

        /*
          static void CreateConfig()
        {
            //这里可以弄一个ICONFIG
            IConfigurationBuilder configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            var builder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfigurationRoot configuration = builder.Build();
            string configValue = configuration.GetSection("AllowedHosts").Value;
        }
         */
        public static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        public AppSettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        public AppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 从IConfiguration初始化配置
        /// </summary>
        /// <param name="configuration">配置对象</param>
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (Configuration == null)
                {
                    return "";
                }

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)] ?? "";
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] sections)
        {
            try
            {
                if (Configuration == null)
                {
                    return new List<T>();
                }
                
                List<T> list = new List<T>();
                // 引用 Microsoft.Extensions.Configuration.Binder 包
                Configuration.Bind(string.Join(":", sections), list);
                return list;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }


        /// <summary>
        /// 根据路径  configuration["App:Name"];
        /// </summary>
        /// <param name="sectionsPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionsPath)
        {
            try
            {
                if (Configuration == null)
                {
                    return "";
                }
                return Configuration[sectionsPath] ?? "";
            }
            catch (Exception ex)
            {
            }

            return "";

        }
    }
}
