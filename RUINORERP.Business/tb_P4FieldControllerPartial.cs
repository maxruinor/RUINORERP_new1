using RUINORERP.Common.Helper;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public partial class tb_P4FieldController<T> : BaseController<T> where T : class
    {
        public  void InitAllField()
        {
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");

            Type[] ModelTypes = dalAssemble.GetExportedTypes();

            foreach (Type type in ModelTypes)
            {
                List<tb_FieldInfo> fields = new List<tb_FieldInfo>();
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable)
                    {
                        var t = base._appContext.GetRequiredService(type);//SugarColumn 或进一步取字段特性也可以
                        ConcurrentDictionary<string, string> cd = ReflectionHelper.GetPropertyValue(t, "FieldNameList") as ConcurrentDictionary<string, string>;
                        foreach (KeyValuePair<string, string> kv in cd)
                        {
                            tb_FieldInfo info = base._appContext.CreateInstance<tb_FieldInfo>();
                            info.ActionStatus = ActionStatus.新增;
                            info.ClassPath = type.FullName;
                            info.EntityName = type.Name;
                            info.FieldName = kv.Key;
                            info.FieldText = kv.Value;
                            fields.Add(info);
                        }
                         
                    }
                }


                //PropertyInfo[] props = item.GetProperties();
                //for (int i = 0; i < props.Length; i++)
                //{
                //    SugarTable
                //     CssPropertyAttribute att = Attribute.GetCustomAttribute(props[i], typeof(CssPropertyAttribute)) as CssPropertyAttribute;

                //    if (att != null)
                //    {
                //        _properties.Add(att.Name, props[i]);
                //        _defaults.Add(att.Name, GetDefaultValue(props[i]));
                //        _cssproperties.Add(props[i]);

                //        CssPropertyInheritedAttribute inh = Attribute.GetCustomAttribute(props[i], typeof(CssPropertyInheritedAttribute)) as CssPropertyInheritedAttribute;

                //        if (inh != null)
                //        {
                //            _inheritables.Add(props[i]);
                //        }
                //    }
                //}


            }
        }
    }
}
