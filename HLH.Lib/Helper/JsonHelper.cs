using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web;

namespace HLH.Lib.Helper
{
    public interface IJsonHelper
    {
        void FindValueFormJsonString(string jsonString, ref List<string> data);

        //生成json格式字符串
        string GetJsonStringByObject(object aobj_data);
        string GetJsonStringByDataTable(DataTable atable_data);
        string GetJsonStringByDataTable(DataTable atable_data, string as_tablename);

        //json格式字符串转为各对象
        object GetObjectByJsonString(string as_jsonstring);
        object GetObjectByJsonString(string as_jsonstring, Type t);

        DataTable GetTableByJsonString(string as_jsonstring);
        DataTable GetTableByJsonString(string as_jsonstring, string as_tablename);

    }

    //
    /*
     1 []中括号代表的是一个数组；
2 {}大括号代表的是一个对象
3 双引号“”表示的是属性值
4 冒号：代表的是前后之间的关系，冒号前面是属性的名称，后面是属性的值，这个值可以是基本数据类型，也可以是引用数据类型。
     */
    /// <summary>
    /// json的帮助类 20161008,2021-3-10更新为 Newtonsoft.Json 12.0.0.0版本。
    /// </summary>
    public class JsonHelper : IJsonHelper
    {

        /// <summary>
        /// 将类似json文本格式化为标准json字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }


        #region 单例构造
        private static JsonHelper instance;
        private static readonly object syncroot = new object();
        public static JsonHelper GetInstance()
        {
            if (instance == null)
            {
                lock (syncroot)
                {
                    if (instance == null)
                    {
                        instance = new JsonHelper();
                    }
                }
            }
            return instance;
        }
        #endregion

        #region 私有方法
        #endregion

        #region IJsonHelper 方法和属性

        /// <summary>
        /// Object对象转为Json格式字符串
        /// add by Vincent.Q 11.01.12
        /// </summary>  // s_jsonstring= Regex.Replace(s_jsonstring,"null","");
        /// <param name="aobj_data"></param>
        /// <returns></returns>
        public string GetJsonStringByObject(object aobj_data, bool PassNullValue)
        {
            //,?"([A-Za-z]+)":null
            string s_jsonstring = string.Empty;
            if (aobj_data == null)
            {
                return string.Empty;
            }
            s_jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(aobj_data);
            if (PassNullValue)
            {
                //去掉null空值
                var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var json = JsonConvert.SerializeObject(aobj_data, Formatting.Indented, jsonSetting);
                s_jsonstring = json.ToString();
            }


            return s_jsonstring;

        }


        /// <summary>
        /// Object对象转为Json格式字符串
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="aobj_data"></param>
        /// <returns></returns>
        public string GetJsonStringByObject(object aobj_data)
        {
            string s_jsonstring = string.Empty;
            if (aobj_data == null)
            {
                return string.Empty;
            }
            s_jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(aobj_data);

            return s_jsonstring;

        }

        /// <summary>
        /// DataTable对象转为Json格式字符串
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="atable_data"></param>
        /// <returns></returns>
        public string GetJsonStringByDataTable(DataTable atable_data)
        {
            return this.GetJsonStringByDataTable(atable_data, "DataTable");

        }

        /// <summary>
        /// DataTable对象转为Json格式字符串
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="atable_data"></param>
        /// <param name="as_tablename"></param>
        /// <returns></returns>
        public string GetJsonStringByDataTable(DataTable atable_data, string as_tablename)
        {
            string s_jsonstring = string.Empty;

            //参数检测
            if (atable_data == null)
                return s_jsonstring;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartObject();
                jw.WritePropertyName(as_tablename);
                jw.WriteStartArray();

                foreach (DataRow dr in atable_data.Rows)
                {
                    jw.WriteStartObject();

                    foreach (DataColumn dc in atable_data.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        ser.Serialize(jw, dr[dc].ToString());
                    }

                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                jw.WriteEndObject();

                sw.Close();
                jw.Close();
            }

            return sb.ToString();

        }



        /// <summary>
        /// Json格式字符串转为Object对象
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="as_jsonstring"></param>
        /// <returns></returns>
        public object GetObjectByJsonString(string as_jsonstring, Type tp)
        {
            object obj_return = null;
            // as_jsonstring = as_jsonstring.Replace("/", "_").Replace("#", "_");
            obj_return = Newtonsoft.Json.JsonConvert.DeserializeObject(as_jsonstring, tp);

            return obj_return;

        }


        /// <summary>
        /// Json格式字符串转为Object对象
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="as_jsonstring"></param>
        /// <returns></returns>
        public object GetObjectByJsonString(string as_jsonstring)
        {
            object obj_return = null;
            // as_jsonstring = as_jsonstring.Replace("/", "_").Replace("#", "_");
            obj_return = Newtonsoft.Json.JsonConvert.DeserializeObject(as_jsonstring);

            return obj_return;

        }



        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }







        /// <summary>
        /// Json格式字符串转为DataTable对象
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="as_jsonstring"></param>
        /// <returns></returns>
        public DataTable GetTableByJsonString(string as_jsonstring)
        {
            return this.GetTableByJsonString(as_jsonstring, "DataTable");

        }

        /// <summary>
        /// Json格式字符串转为DataTable对象  有错误by watson 2021-3-11
        /// add by Vincent.Q 11.01.12
        /// </summary>
        /// <param name="as_jsonstring"></param>
        /// <param name="as_tablename"></param>
        /// <returns></returns>
        public DataTable GetTableByJsonString(string as_jsonstring, string as_tablename)
        {
            bool b_initcolumn = false;
            DataTable table_return = null;

            //参数检测
            if (string.IsNullOrEmpty(as_jsonstring) || string.IsNullOrEmpty(as_tablename))
            {
                return table_return;
            }

            //获取json对象
            object obj_parm = this.GetObjectByJsonString(as_jsonstring);

            //转换后,有且只有一个对象,即DataTable
            //Newtonsoft.Json.JObject jso_parm = (JObject)obj_parm;
            Newtonsoft.Json.Linq.JObject jso_parm = (Newtonsoft.Json.Linq.JObject)obj_parm;
            //转为json数组
            Newtonsoft.Json.Linq.JArray jsa_parm = (JArray)jso_parm["DataTable"];

            //循环获取
            for (int i = 0; i < jsa_parm.Count; i++)
            {
                Newtonsoft.Json.Linq.JObject jso_item = (JObject)jsa_parm[i];

                //转为泛型处理
                //Dictionary<string, object> dic_item = (Dictionary<string, object>)jso_item;
                Dictionary<string, object> dic_item = null;
                //加载DataTable栏目信息
                if (!b_initcolumn)
                {
                    table_return = new DataTable();
                    foreach (string s_key in dic_item.Keys)
                    {
                        object obj_value = null;
                        dic_item.TryGetValue(s_key, out obj_value);

                        table_return.Columns.Add(s_key, typeof(string));
                    }
                    b_initcolumn = true;
                }

                //加载DataTable数据
                DataRow row_add = table_return.NewRow();
                foreach (string s_key in dic_item.Keys)
                {
                    object obj_value = null;
                    dic_item.TryGetValue(s_key, out obj_value);

                    row_add[s_key] = obj_value;
                }
                table_return.Rows.Add(row_add);

            }

            return table_return;

        }

        #endregion


        /// <summary>
        /// 根据不同类型创建对应实例对象
        /// </summary>
        /// <param name="subType"></param>
        /// <returns></returns>
        private object CreateMyInstance(Type subType, object instanceValue, object[] args)
        {
            Object sub = new object();

            string typeName = subType.FullName;
            switch (typeName)
            {
                case "System.String":
                    sub = Activator.CreateInstance(subType, new object[] { new char[1] { ' ' } });
                    sub = instanceValue.ToString();
                    break;

                case "System.xml":
                    sub = Activator.CreateInstance(subType, new object[] { new char[1] { ' ' } });
                    sub = instanceValue;
                    break;
                default:
                    sub = Activator.CreateInstance(subType);
                    #region 通过反射调用泛型方法，得到List中对象的值

                    Type st = typeof(JsonHelper);
                    object ob = Activator.CreateInstance(st);
                    MethodInfo method = st.GetMethod("JsonEntityLast", BindingFlags.Instance | BindingFlags.Public);
                    method = method.MakeGenericMethod(subType);
                    sub = method.Invoke(ob, args);
                    #endregion
                    break;
            }
            return sub;
        }




        /// <summary>
        /// 最新解析方法2016-08-8
        /// 2021-5-29修复bug，存在多个版本。暂时没有提取重构
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="fromParm"></param>
        /// <param name="pname"></param>
        /// <param name="dllModuleName">要解析的类所在的dll位置ex:  APIEntity.dll</param>
        /// <returns></returns>
        public T JsonEntityLast<T>(object fromParm, bool isAspnet, string dllModuleName)
        {
            #region 要反射的文件位置，因为是 asp.net类型可能会存在于临时文件中

            Assembly tempAssem = Assembly.GetExecutingAssembly();//不在当前了
            System.IO.FileInfo fi = new FileInfo(tempAssem.Location);
            //  string dllfileTop = System.IO.Path.Combine(fi.Directory.FullName, "APIEntity.dll");
            string dllfileTop = System.IO.Path.Combine(fi.Directory.FullName, dllModuleName);
            if (isAspnet)
            {
                string[] files = System.IO.Directory.GetFiles(HttpRuntime.CodegenDir, dllModuleName, System.IO.SearchOption.AllDirectories);
                if (files.Length == 1)
                {
                    dllfileTop = files[0];
                }
            }
            //引用的程序集合
            Assembly assem = Assembly.LoadFile(dllfileTop);
            #endregion
            T t = Activator.CreateInstance<T>();
            if (fromParm == null)
            {
                return (T)t;
            }
            string TempTypeName = fromParm.GetType().FullName;
            switch (TempTypeName)
            {
                case "Newtonsoft.Json.Linq.JObject":
                    Newtonsoft.Json.Linq.JObject jso = (JObject)fromParm;
                    #region 对象

                    PropertyInfo[] properties = typeof(T).GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        //结果不包括实体属性的值，则实体不全面，或key没有对应，通过json结果检查对应实体!!!!!" + "s_propertyname:" + s_propertyname + "不在里面 "
                        //也无法取值
                        if (!jso.ContainsKey(pi.Name))
                        {
                            continue;
                        }
                        Type type = pi.PropertyType;
                        try
                        {
                            //创建List实例 或数组 

                            if (type.IsGenericType || type.BaseType.Name == "CollectionBase")
                            {
                                #region 泛型处理
                                object myobj = Activator.CreateInstance(type);
                                System.Collections.IList ilist = myobj as System.Collections.IList;
                                try
                                {
                                    //这个时候，有可能的情况为 结果的类型为泛型，实际rs返回的是一个List<0>，则直接是一个对象。
                                    //可以取出这个对象，放到队列中，不用循环操作
                                    if (jso[pi.Name] is JObject)
                                    {
                                        #region 目标类型是泛型 结果为一个对象实际为这个泛型对象
                                        JObject entity = jso[pi.Name] as JObject;
                                        // Assembly assem = Assembly.LoadFile(dllfileTop);
                                        Type subType = assem.GetType(type.GetGenericArguments()[0].FullName, true);


                                        Object sub = Activator.CreateInstance(subType);
                                        #region 通过反射调用泛型方法，得到List中对象的值

                                        Type st = typeof(JsonHelper);
                                        object ob = Activator.CreateInstance(st);
                                        MethodInfo method = st.GetMethod("JsonEntityLast", BindingFlags.Instance | BindingFlags.Public);

                                        method = method.MakeGenericMethod(subType);
                                        object[] args = new object[] { entity, isAspnet, dllModuleName };
                                        sub = method.Invoke(ob, args);

                                        #endregion

                                        ilist.Add(sub);

                                        #endregion
                                    }
                                    else
                                    {
                                        Newtonsoft.Json.Linq.JArray list = jso[pi.Name] as JArray;
                                        if (list.Count > 0)
                                        {
                                            #region 目标类型和结果都是泛型时

                                            Type subType = typeof(System.Object);
                                            if (type.GetGenericArguments().Length > 0)
                                            {
                                                if (type.FullName.Contains(", mscorlib,"))
                                                {
                                                    Assembly assemBack = Assembly.Load("mscorlib.dll");
                                                    subType = assemBack.GetType(type.GetGenericArguments()[0].FullName, true);
                                                }
                                                else
                                                {
                                                    subType = assem.GetType(type.GetGenericArguments()[0].FullName, true);
                                                }

                                            }
                                            else if (type.BaseType.Name == "CollectionBase")
                                            {
                                                try
                                                {
                                                    subType = assem.GetType(type.FullName.Replace("Collection", ""), true);
                                                }
                                                catch (Exception subex)
                                                {
                                                    if (subex.Message.Contains("未能从程序集"))
                                                    {
                                                        Assembly assemBack = Assembly.Load("mscorlib.dll");
                                                        subType = assemBack.GetType("System." + type.Name.Replace("Collection", ""), true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("没有检测出集合类型,请调试代码");
                                            }

                                            for (int o = 0; o < list.Count; o++)
                                            {
                                                object[] args = new object[] { list[o], isAspnet, dllModuleName };
                                                Object sub = CreateMyInstance(subType, list[o], args);
                                                ilist.Add(sub);
                                            }

                                            #endregion
                                        }
                                    }
                                    pi.SetValue(t, ilist, null);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("没有检测出集合类型中的子类型,请调试代码" + ex);
                                }
                                #endregion
                            }
                            else
                            {

                                #region 对象属性  modify by watson 2017 0810
                                string moduleName = dllModuleName.Substring(0, dllModuleName.LastIndexOf("."));
                                //自定义类 反序列化对象中的子对象等   系统类型(string ,int etc)  分别处理
                                //分类 和 枚举 分别处理
                                if (pi.ToString().Contains(moduleName))
                                {
                                    if (pi.PropertyType.IsClass)
                                    {
                                        #region 如果是类
                                        try
                                        {
                                            object myobj = Activator.CreateInstance(type);
                                            JObject entity = jso[pi.Name] as JObject;
                                            if (entity != null)
                                            {
                                                Type st = typeof(JsonHelper);
                                                object o = Activator.CreateInstance(st);
                                                MethodInfo method = st.GetMethod("JsonEntityLast", BindingFlags.Instance | BindingFlags.Public);
                                                method = method.MakeGenericMethod(type);
                                                object[] args = new object[] { entity, isAspnet, dllModuleName };
                                                myobj = method.Invoke(o, args);
                                                pi.SetValue(t, myobj, null);
                                            }
                                            else
                                            {
                                                throw new Exception("对象为空。请调试代码" + pi.Name);
                                            }


                                        }
                                        catch (Exception exxx)
                                        {

                                        }
                                        #endregion
                                    }

                                    if (pi.PropertyType.IsEnum)
                                    {
                                        #region 如果是枚举
                                        try
                                        {
                                            object myobj = Activator.CreateInstance(type);
                                            object entity = jso[pi.Name];

                                            //动态创建程序集
                                            AssemblyName DemoName = new AssemblyName(assem.FullName);
                                            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
                                            //动态创建模块
                                            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");

                                            EnumBuilder eb = mb.DefineEnum(type.FullName, TypeAttributes.Public, typeof(int));
                                            Type enumType = eb.CreateType();
                                            System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
                                            int enumValue = 0;
                                            for (int i = 0; i < fields.Length; i++)
                                            {
                                                if (fields[i].Name == jso[pi.Name].ToString())
                                                {
                                                    enumValue = i;
                                                }
                                            }
                                            myobj = System.Enum.Parse(enumType, enumValue.ToString());
                                            pi.SetValue(t, myobj, null);
                                        }

                                        catch (Exception exxx)
                                        {

                                        }
                                        #endregion
                                    }
                                }
                                else if (pi.PropertyType.Namespace == "System")
                                {
                                    //系统类型
                                    try
                                    {
                                        if (jso[pi.Name] == null && pi.PropertyType.FullName == "System.String")
                                        {
                                            jso[pi.Name] = "";
                                        }
                                        if (pi.PropertyType.FullName == "System.Boolean")
                                        {
                                            int tempBool = 0;
                                            if (int.TryParse(jso[pi.Name].ToString(), out tempBool))
                                            {
                                                //如果为0，1 则转换一下
                                                if (tempBool == 1)
                                                {
                                                    jso[pi.Name] = "true";
                                                }
                                                if (tempBool == 0)
                                                {
                                                    jso[pi.Name] = "false";
                                                }
                                            }
                                        }
                                        object v = System.ComponentModel.TypeDescriptor.GetConverter(type).ConvertFrom(jso[pi.Name].ToString());
                                        pi.SetValue(t, v, null);
                                    }
                                    catch (Exception exx)
                                    {
                                        throw new Exception("系统属性 设置值时出错" + exx.Message);
                                    }
                                }
                                else
                                {
                                    //没有处理
                                    throw new Exception("反处理时，请调试代码完善");
                                }
                                #endregion
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(t.ToString() + "|" + pi.Name + "===" + ex.Message);
                        }
                    }

                    #endregion
                    break;
                case "Newtonsoft.Json.Linq.JArray":
                    Newtonsoft.Json.Linq.JArray jslist = (JArray)fromParm;
                    try
                    {
                        //object myobj = Activator.CreateInstance<T>();
                        T myobj = Activator.CreateInstance<T>();
                        System.Collections.IList ilist = myobj as System.Collections.IList;

                        #region 目标类型和结果都是泛型时

                        // Assembly assem = Assembly.LoadFile(dllfileTop);
                        if (t.GetType().GetGenericArguments().Length > 0)
                        {
                            Type subType = assem.GetType(t.GetType().GetGenericArguments()[0].FullName, true);
                            for (int o = 0; o < jslist.Count; o++)
                            {
                                Object sub = Activator.CreateInstance(subType);
                                #region 通过反射调用泛型方法，得到List中对象的值

                                Type st = typeof(JsonHelper);
                                object ob = Activator.CreateInstance(st);
                                MethodInfo method = st.GetMethod("JsonEntityLast", BindingFlags.Instance | BindingFlags.Public);

                                method = method.MakeGenericMethod(subType);
                                object[] args = new object[] { jslist[o], isAspnet, dllModuleName };
                                sub = method.Invoke(ob, args);

                                #endregion

                                ilist.Add(sub);
                            }
                        }
                        #endregion
                        t = myobj;
                    }

                    //  pi.SetValue(t, ilist, null);

                    catch (Exception ex)
                    {

                    }
                    break;

                default:
                    t = default(T);
                    break;
            }

            return (T)t;//类型转换并返回;
        }





        /// <summary>
        /// 设置实体属性值,根据属性名称
        /// add by Vincent.Q 10.12.30
        /// </summary>
        /// <param name="as_propertyname"></param>
        /// <param name="aobj_propertyvalue"></param>
        public void SetPropertyValue(object obj, string as_propertyname, object aobj_propertyvalue)
        {
            //获取实体类型
            Type t = obj.GetType();
            //获取属性信息,并判断是否存在
            PropertyInfo property = t.GetProperty(as_propertyname);
            if (property == null)
                return;

            string s_datatype = property.PropertyType.Name.Trim().ToLower();

            //属性赋值
            object obj_propertyvalue = null;
            switch (s_datatype)
            {
                case "nullable`1":
                    #region  不可空类型
                    string tempdataType = property.PropertyType.FullName.Trim().ToLower();
                    if (tempdataType.Contains("system.int32"))
                    {
                        int i32_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        i32_parm = Convert.ToInt32(aobj_propertyvalue);
                        obj_propertyvalue = i32_parm;
                    }

                    if (tempdataType.Contains("system.decimal"))
                    {
                        decimal i32_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        i32_parm = Convert.ToDecimal(aobj_propertyvalue);
                        obj_propertyvalue = i32_parm;
                    }



                    #endregion

                    break;


                case "object":
                    object objparm = new object();
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = "";
                    objparm = aobj_propertyvalue;
                    obj_propertyvalue = objparm;
                    break;

                case "boolean":

                    Boolean Boolean_parm = false;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0;
                    Boolean_parm = Convert.ToBoolean(aobj_propertyvalue);
                    obj_propertyvalue = Boolean_parm;
                    break;

                case "double":
                    Double double_parm = 0;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0;

                    double_parm = Convert.ToDouble(aobj_propertyvalue);
                    obj_propertyvalue = double_parm;
                    break;


                case "int64":
                    Int64 int64_parm = 0;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0;

                    int64_parm = Convert.ToInt64(aobj_propertyvalue);
                    obj_propertyvalue = int64_parm;
                    break;

                case "decimal":
                    decimal decimal_parm = 0;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0;

                    decimal_parm = Convert.ToDecimal(aobj_propertyvalue);
                    obj_propertyvalue = decimal_parm;
                    break;

                case "int32":
                    int i_parm = 0;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0;

                    i_parm = Convert.ToInt32(aobj_propertyvalue);
                    obj_propertyvalue = i_parm;
                    break;
                case "dec":
                    decimal dc_parm = 0.0m;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = 0.0m;

                    dc_parm = Convert.ToDecimal(aobj_propertyvalue);
                    obj_propertyvalue = dc_parm;
                    break;
                case "string":
                    string s_parm = string.Empty;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = "";

                    s_parm = aobj_propertyvalue.ToString();
                    obj_propertyvalue = s_parm;
                    break;
                case "dat":
                    DateTime dtm_parm = DateTime.MinValue;
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        obj_propertyvalue = DateTime.MinValue;

                    dtm_parm = Convert.ToDateTime(aobj_propertyvalue);
                    obj_propertyvalue = dtm_parm;
                    break;

                case "list`1":
                    object list_parm = new object();
                    if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                        aobj_propertyvalue = "";
                    list_parm = aobj_propertyvalue;
                    obj_propertyvalue = list_parm;
                    break;
                default:
                    //如果这个字段是基础实体类型，则
                    if (aobj_propertyvalue.GetType().BaseType.ToString() == "SMTAPI.Entity.BaseEntity")
                    {
                        object obj_parm = new object();
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = "";

                        obj_parm = aobj_propertyvalue;
                        obj_propertyvalue = obj_parm;
                    }
                    else
                    {
                        throw new Exception("请修改代码，处理这种数据类型。" + s_datatype);
                    }


                    break;
            }


            //非泛型 时可以用   property.SetValue(obj，Convert.ChangeType(value,property.PropertyType),null);

            if (!property.PropertyType.IsGenericType)
            {
                //非泛型
                t.GetProperty(as_propertyname).SetValue(obj, Convert.ChangeType(obj_propertyvalue, property.PropertyType), null);
            }
            else
            {
                //泛型Nullable<>
                Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    t.GetProperty(as_propertyname).SetValue(obj, Convert.ChangeType(obj_propertyvalue, Nullable.GetUnderlyingType(property.PropertyType)), null);
                }
            }

            if (property.PropertyType.IsEnum) //属性类型是否表示枚举
            {
                object enumName = Enum.ToObject(property.PropertyType, obj_propertyvalue);
                t.GetProperty(as_propertyname).SetValue(obj, enumName, null); //获取枚举值，设置属性值
            }
            else if (property.PropertyType.IsGenericType) //属性类型是否表示泛型
            {
                t.GetProperty(as_propertyname).SetValue(obj, obj_propertyvalue, null); //获取枚举值，设置属性值
            }
            else
            {
                //普通属性
                t.GetProperty(as_propertyname).SetValue(obj, obj_propertyvalue, null);
            }


        }

        /// <summary>
        /// 通过关键词找查josn结构的字串的对应的值
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public void FindValueFormJsonString(string jsonString, ref List<string> data)
        {
            /*
            Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject(item.Message) as Newtonsoft.Json.Linq.JObject;
            if (obj.HasValues)
            {
                if (obj)
                {

                }
            }
            if (inputData.leaf)
            {
                data.Add(inputData);
            }
            else
            {
                if (inputData.children == null) return;
                foreach (var item in inputData.children)
                {
                    if (item.leaf)
                    {
                        data.Add(item);
                    }
                    else
                    {
                       FindAll(item, ref data);
                    }
                }
            }*/

        }


    }










}



/*/**
 public IList test;
        IList MakeListOfType(Type listType)
        {
            Type type = typeof(List<>);
            Type specificListType = type.MakeGenericType(listType);

            return (IList)Activator.CreateInstance(specificListType);
        }
test = MakeListOfType(obj.GetType().GetGenericArguments()[0]);
object objItem = Activator.CreateInstance(obj.GetType().GetGenericArguments()[0]);
test.Add(objItem);
最后
SetValue(o, test, null);
 * 
 * 
 * 
 * private T GetModuleInfo(DataTable dt)
    {
        T t = default(T);
        T tobj=Activator.CreateInstance<T>();
        PropertyInfo[] pr = tobj.GetType().GetProperties();
        foreach (System.Reflection.PropertyInfo item in pr)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (item.Name.ToLower().Equals(dt.Columns[i].ColumnName.ToLower()))
                {
                    if (dt.Rows[0][i] != DBNull.Value)
                        item.SetValue(tobj, (dt.Rows[0][i]), null);
                    else
                        item.SetValue(tobj, null, null);
                }
            }
        }
        t = tobj;
        return t;
    }
 * 
 * 
 
 */
