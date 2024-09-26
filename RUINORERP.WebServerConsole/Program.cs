using System;
using System.Threading;
using SimpleHttp;
using System.IO;
using System.Linq;
using RUINORERP.WebServerConsole;
using RUINORERP.WebServerConsole.Comm;
using RUINORERP.Model.Context;

namespace RUINORERP.WebServerConsole
{
    static class Program
    {
        private static ApplicationContext _AppContextData;
        public static ApplicationContext AppContextData
        {
            get
            {
                if (_AppContextData == null)
                {
                    ApplicationContextManagerAsyncLocal applicationContextManagerAsyncLocal = new ApplicationContextManagerAsyncLocal();
                    applicationContextManagerAsyncLocal.Flag = "test" + System.DateTime.Now.ToString();
                    ApplicationContextAccessor applicationContextAccessor = new ApplicationContextAccessor(applicationContextManagerAsyncLocal);
                    _AppContextData = new ApplicationContext(applicationContextAccessor);
                    _AppContextData.Db = SqlSugarHelper.Db;
                }
                return _AppContextData;
            }
            set
            {
                _AppContextData = value;
            }
        }
        static void Main()
        {
            ConfigManager _configManager = new ConfigManager();
            ILoggerService loggerService = new Log4NetService();
            WebServer webserver = new WebServer(AppContextData, _configManager, loggerService, new AuthenticationService(), new AuthorizationService());
            webserver.RunWebServer();
        }
    }
}
