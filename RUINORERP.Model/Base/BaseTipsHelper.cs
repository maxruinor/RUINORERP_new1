using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RUINORERP.Model.Base
{
    public static class BaseTipsHelper
    {

        public static void AddInfo<T>(Expression<Func<T, string>> expression, string HelpInfo, ref ConcurrentDictionary<string, string> kvList)
        {
            string key = expression.Body.ToString().Split('.')[1];
            kvList.TryAdd(key, HelpInfo);
        }

        public static void AddInfo<T>(Expression<Func<T, string>> expression, string HelpInfo, ref List<KeyValuePair<string, string>> kvList)
        {
            string key = expression.Body.ToString().Split('.')[1];
            kvList.Add(new KeyValuePair<string, string>(key, HelpInfo));
        }

        public static System.Collections.Generic.List<KeyValuePair<string, string>> InfoFor<T>(Expression<Func<T, string>> expression, string HelpInfo)
        {
            System.Collections.Generic.List<KeyValuePair<string, string>> kvList = new List<KeyValuePair<string, string>>();

            kvList.Add(new KeyValuePair<string, string>());
            return kvList;
        }

        public static string SHowInfo<T>(Expression<Func<T, string>> expression)
        {
            return "";
        }

        public static System.Collections.Generic.List<KeyValuePair<string, string>> WithMessage(this string rule, string errorMessage)
        {
            System.Collections.Generic.List<KeyValuePair<string, string>> kv = new List<KeyValuePair<string, string>>();
            kv.Add(new KeyValuePair<string, string>());
            return kv;
        }
    }
}
