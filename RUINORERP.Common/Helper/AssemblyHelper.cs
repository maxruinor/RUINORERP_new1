using System;
using System.Reflection;
using System.Collections.Generic;
using RUINORERP.Common.Extensions;
namespace RUINORERP.Common.Helper
{
    public static class AssemblyHelper
    {
        #region GetAssemblyPath

        public static string GetAssemblyPath(Type type)
        {
            return GetAssemblyPath(type.Assembly);
        }

        public static string GetAssemblyPath(Assembly assembly)
        {
            string path = assembly.CodeBase;
            Uri uri = new Uri(path);

            // If it wasn't loaded locally, use the Location
            if (!uri.IsFile)
                return assembly.Location;

            if (uri.IsUnc)
                return path.Substring(Uri.UriSchemeFile.Length + 1);

            return uri.LocalPath;
        }

        #endregion

        #region GetDirectoryName
        public static string GetDirectoryName(Assembly assembly)
        {
            return System.IO.Path.GetDirectoryName(GetAssemblyPath(assembly));
        }
        #endregion


        public static List<Assembly> GetReferanceAssemblies(this AppDomain domain)
        {
            var list = new List<Assembly>();
            domain.GetAssemblies().ForEach(i =>
            {
                GetReferanceAssemblies(i, list);
            });
            return list;
        }
        public static void GetReferanceAssemblies(Assembly assembly, List<Assembly> list)
        {
            assembly.GetReferencedAssemblies().ForEach(i =>
            {
                var ass = Assembly.Load(i);
                if (!list.Contains(ass))
                {
                    list.Add(ass);
                    GetReferanceAssemblies(ass, list);
                }
            });
        }


        public static List<Assembly> GetReferanceAssemblies(this AppDomain domain,bool GetRUINOR)
        {
            var list = new List<Assembly>();
            domain.GetAssemblies().CastToList<Assembly>().ForEach(i =>
            {
                if (i.FullName.ToLower().Contains("ruinor"))
                {
                    list.Add(i);
                   // GetReferanceAssemblies(i, list);
                }
               
            });
            return list;
        }

    }
}
