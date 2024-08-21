using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using SqlSugar;
using System.Collections.Concurrent;
using System.Threading;

namespace RUINORERP.UI.Common
{


    /*
      //动态创建程序集
            AssemblyName DemoName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
            //动态创建模块
            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
            //动态创建类MyClass
            TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
            //动态创建字段
            FieldBuilder fb = tb.DefineField("", typeof(System.String), FieldAttributes.Private);  
            //动态创建构造函数
            Type[] clorType = new Type[] { typeof(System.String) };
            ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
            //生成指令
            ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldarg_1);
            ilg.Emit(OpCodes.Stfld, fb);
            ilg.Emit(OpCodes.Ret);
            //动态创建属性
            PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
            //动态创建方法
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
            MethodBuilder myMethod = tb.DefineMethod("get_Property", getSetAttr, typeof(string), Type.EmptyTypes);
            //生成指令
            ILGenerator numberGetIL = myMethod.GetILGenerator();
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fb);
            numberGetIL.Emit(OpCodes.Ret);
            //保存动态创建的程序集
            dynamicAssembly.Save(DemoName.Name + ".dll");
     */
    /// <summary>
    /// 动态反射 
    /// </summary>
    public static class EmitHelper
    {

        #region 更新实体模型 Type combinedType = MergeTypes(typeof(Foo), typeof(Bar));

        /// <summary>
        /// 合并多个实体 成功创建属性
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type MergeTypesNew(params Type[] types)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyBuilder builder =
                domain.DefineDynamicAssembly(new AssemblyName("CombinedAssembly"),
                AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule("DynamicModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("CombinedType");

            //这里构建SugarColumn一个构造函数，注意参数个数
            //因为目前多处用SugarColumn这个来标识
            //            var attrCtorParams = new Type[] { typeof(string), typeof(string) };
            var attrCtorParams = new Type[] { };
            var attrCtorInfo = typeof(SugarColumn).GetConstructor(attrCtorParams);

            foreach (var type in types)
            {
                var props = GetProperties(type);

                foreach (var prop in props)
                {
                    //添加字段
                    // typeBuilder.DefineField("_" + prop.Key, prop.Value, FieldAttributes.Public);

                    //添加属性（内部已经添加了字段）
                    PropertyBuilder newProp = AddProperty(typeBuilder, prop.Key, prop.Value);
                    //添加特性
                    //var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { prop.Key, "是" });
                    //var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] {  });
                    //newProp.SetCustomAttribute(attrBuilder);
                }
            }

            return typeBuilder.CreateType();


        }


        #endregion


        /// <summary>
        /// 合并变成只是字段
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type MergeTypes(params Type[] types)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyBuilder builder = domain.DefineDynamicAssembly(new AssemblyName("CombinedAssembly"),
                AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule("DynamicModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("CombinedType");
            foreach (var type in types)
            {
                var props = GetProperties(type);
                foreach (var prop in props)
                {
                    //定义的是字段，不是属性
                    typeBuilder.DefineField(prop.Key, prop.Value, FieldAttributes.Public);
                }
            }

            Type rs = typeBuilder.CreateType();
            //builder.Save("MytestMain.dll");//可以保存下次使用
            return rs;
        }


        /// <summary>
        /// 合并了属性
        /// </summary>
        /// <param name="MergeAll"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type MergeTypes(bool MergeAll, params Type[] types)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyBuilder asmbuilder = domain.DefineDynamicAssembly(new AssemblyName("CombinedAssembly"), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = asmbuilder.DefineDynamicModule("DynamicModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("CombinedType");



            //       ConstructorBuilder constructor =
            //typeBuilder.DefineConstructor(MethodAttributes.Public |
            //   MethodAttributes.HideBySig |
            //   MethodAttributes.SpecialName |
            //   MethodAttributes.RTSpecialName,
            //  CallingConventions.Standard, new Type[] { typeof(IDataReader) });



            SugarColumn entityAttr;
            foreach (Type type in types)
            {
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {



                    typeBuilder.DefineField("_" + prop.Name, prop.PropertyType, FieldAttributes.Public);


                    PropertyBuilder property_builder = typeBuilder
                      .DefineProperty(prop.Name, PropertyAttributes.HasDefault,
                        prop.PropertyType, null);

                    MethodBuilder get_method1 = typeBuilder.DefineMethod("get_" + prop.Name,
                      MethodAttributes.Public |
                        MethodAttributes.SpecialName |
                        MethodAttributes.HideBySig);


                    MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                    // Define the "set" accessor method 
                    MethodBuilder mbNewPropSetAccessor = typeBuilder.DefineMethod("set_" + prop.Name, getSetAttr);



                    ILGenerator il = get_method1.GetILGenerator();
                    il.Emit(OpCodes.Ldarg_0);

                    ILGenerator ilset = mbNewPropSetAccessor.GetILGenerator();
                    ilset.Emit(OpCodes.Ldarg_1);

                    property_builder.SetGetMethod(get_method1);//ok

                    foreach (Attribute attr in prop.GetCustomAttributes(true))
                    {

                        entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            if (entityAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (entityAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (entityAttr.IsPrimaryKey)
                            {
                                continue;
                            }
                            if (entityAttr.ColumnDescription.Trim().Length > 0)
                            {
                                var attribType = typeof(SugarColumn);
                                var cab = new CustomAttributeBuilder(
                                    attribType.GetConstructor(Type.EmptyTypes), // constructor selection
                                    new object[0], // constructor arguments - none
                                    new[] { // properties to assign to
        attribType.GetProperty("ColumnName"),
        attribType.GetProperty("ColumnDescription"),
                                     },
                                    new object[] { // values for property assignment
        entityAttr.ColumnName,
      entityAttr.ColumnDescription
                                    });

                                //CustomAttributeData cad = prop.GetCustomAttributesData();
                                //cad.ToAttributeBuilder()


                                //asmbuilder.SetCustomAttribute(cab);
                                property_builder.SetCustomAttribute(cab);
                            }
                        }
                    }


                    //property_builder.SetGetMethod(mbNewPropSetAccessor);
                }
            }

            Type rs = typeBuilder.CreateType();
            //builder.Save("MytestMain.dll");//可以保存下次使用
            return rs;
        }

        /// <summary>
        /// 得到属性名称和类型字典
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<string, Type> GetProperties(Type type)
        {
            return type.GetProperties().ToDictionary(p => p.Name, p => p.PropertyType);
        }

        private static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName)
        {
            #region  动态创建字段


            // string MemberName = "Watson_ok";
            Type memberType = typeof(string);
            FieldBuilder fbNumber = tb.DefineField("m_" + MemberName, memberType, FieldAttributes.Private);


            PropertyBuilder pbNumber = tb.DefineProperty(
                MemberName,
                System.Reflection.PropertyAttributes.HasDefault,
                memberType,
                null);


            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;


            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                "get_" + MemberName,
                getSetAttr,
                memberType,
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_" + MemberName,
                getSetAttr,
                null,
                new Type[] { memberType });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);
            #endregion

            return pbNumber;
        }

        private static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName, Type memberType)
        {
            #region  动态创建字段


            // string MemberName = "Watson_ok";
            // Type memberType = typeof(string);
            FieldBuilder fbNumber = tb.DefineField("m_" + MemberName, memberType, FieldAttributes.Private);

            PropertyBuilder pbNumber = tb.DefineProperty(
                MemberName,
                System.Reflection.PropertyAttributes.HasDefault,
                memberType,
                null);


            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;


            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                "get_" + MemberName,
                getSetAttr,
                memberType,
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_" + MemberName,
                getSetAttr,
                null,
                new Type[] { memberType });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);
            #endregion

            return pbNumber;
        }


        // if (!pi.CanWrite) continue;
        /*
        if (!pi.PropertyType.IsGenericType)
                           {
                               //非泛型
                               pi.SetValue(t, value==null ? null : Convert.ChangeType(value, pi.PropertyType), null);
                           }
                           else
                           {
                               //泛型Nullable<>
                               Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                               if (genericTypeDefinition == typeof(Nullable<>))
                               {
                                   pi.SetValue(t, value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType)), null);
                               }
                           }
*/

        public static ConcurrentQueue<KeyValuePair<string, PropertyInfo>> GetfieldNameList(Type combinedType)
        {
            SqlSugar.SugarColumn entityAttr;
            ConcurrentQueue<KeyValuePair<string, PropertyInfo>> fieldNameList = new ConcurrentQueue<KeyValuePair<string, PropertyInfo>>();
            foreach (PropertyInfo field in combinedType.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    entityAttr = attr as SqlSugar.SugarColumn;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColumnDescription == null)
                        {
                            continue;
                        }
                        if (entityAttr.IsIdentity)
                        {
                            continue;
                        }
                        if (entityAttr.IsPrimaryKey)
                        {
                            continue;
                        }
                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                        {
                            fieldNameList.Enqueue(new KeyValuePair<string, PropertyInfo>(entityAttr.ColumnDescription, field));
                        }
                    }
                }
            }
            return fieldNameList;
        }

        public static ConcurrentDictionary<string, PropertyInfo> GetfieldNameListbyCD(Type combinedType)
        {
            SqlSugar.SugarColumn entityAttr;
            ConcurrentDictionary<string, PropertyInfo> fieldNameList = new ConcurrentDictionary<string, PropertyInfo>();
            foreach (PropertyInfo field in combinedType.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    entityAttr = attr as SqlSugar.SugarColumn;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColumnDescription == null)
                        {
                            continue;
                        }
                        if (entityAttr.IsIdentity)
                        {
                            continue;
                        }
                        if (entityAttr.IsPrimaryKey)
                        {
                            continue;
                        }
                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                        {
                            fieldNameList.TryAdd(field.Name, field);
                        }
                    }
                }
            }
            return fieldNameList;
        }

        ///// <summary>
        ///// 这几个都有局限性 用SugarColumn过滤了一次
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static ConcurrentDictionary<string, PropertyInfo> GetPropertyInfoListbyCD<T>()
        //{
        //    SqlSugar.SugarColumn entityAttr;
        //    ConcurrentDictionary<string, PropertyInfo> fieldNameList = new ConcurrentDictionary<string, PropertyInfo>();
        //    foreach (PropertyInfo field in typeof(T).GetProperties())
        //    {
        //        foreach (Attribute attr in field.GetCustomAttributes(true))
        //        {
        //            entityAttr = attr as SqlSugar.SugarColumn;
        //            if (null != entityAttr)
        //            {
        //                if (entityAttr.ColumnDescription == null)
        //                {
        //                    continue;
        //                }
        //                //if (entityAttr.IsIdentity)
        //                //{
        //                //    continue;
        //                //}
        //                //if (entityAttr.IsPrimaryKey)
        //                //{
        //                //    continue;
        //                //}
        //                if (entityAttr.ColumnDescription.Trim().Length > 0)
        //                {
        //                    fieldNameList.TryAdd(field.Name, field);
        //                }
        //            }
        //        }
        //    }
        //    return fieldNameList;
        //}


        public static CustomAttributeBuilder ToAttributeBuilder(this CustomAttributeData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var constructorArguments = new List<object>();
            foreach (var ctorArg in data.ConstructorArguments)
            {
                constructorArguments.Add(ctorArg.Value);
            }

            var propertyArguments = new List<PropertyInfo>();
            var propertyArgumentValues = new List<object>();
            var fieldArguments = new List<FieldInfo>();
            var fieldArgumentValues = new List<object>();
            foreach (var namedArg in data.NamedArguments)
            {
                var fi = namedArg.MemberInfo as FieldInfo;
                var pi = namedArg.MemberInfo as PropertyInfo;

                if (fi != null)
                {
                    fieldArguments.Add(fi);
                    fieldArgumentValues.Add(namedArg.TypedValue.Value);
                }
                else if (pi != null)
                {
                    propertyArguments.Add(pi);
                    propertyArgumentValues.Add(namedArg.TypedValue.Value);
                }
            }
            return new CustomAttributeBuilder(
              data.Constructor,
              constructorArguments.ToArray(),
              propertyArguments.ToArray(),
              propertyArgumentValues.ToArray(),
              fieldArguments.ToArray(),
              fieldArgumentValues.ToArray());
        }


        public delegate TTarget MapMethod<TTarget, TSource>(TSource source);


        public static class FastMapper<TTarget, TSource>
        {
            private static MapMethod<TTarget, TSource> mapMethod;

            public static MapMethod<TTarget, TSource> GetMapMethod()
            {
                if (mapMethod == null)
                {
                    mapMethod = CreateMapMethod(typeof(TTarget), typeof(TSource));
                }
                return mapMethod;
            }

            public static TTarget Map(TSource source)
            {
                if (mapMethod == null)
                {
                    mapMethod = CreateMapMethod(typeof(TTarget), typeof(TSource));
                }
                return mapMethod(source);
            }

            private static MapMethod<TTarget, TSource> CreateMapMethod(Type targetType, Type sourceType)
            {


                DynamicMethod map = new DynamicMethod("Map", targetType, new Type[] { sourceType }, typeof(TTarget).Module);

                ILGenerator il = map.GetILGenerator();
                ConstructorInfo ci = targetType.GetConstructor(new Type[0]);
                il.DeclareLocal(targetType);
                il.Emit(OpCodes.Newobj, ci);
                il.Emit(OpCodes.Stloc_0);
                foreach (var sourcePropertyInfo in sourceType.GetProperties())
                {
                    var targetPropertyInfo = (from p in targetType.GetProperties()
                                              where p.Name == sourcePropertyInfo.Name && p.PropertyType == sourcePropertyInfo.PropertyType
                                              select p).FirstOrDefault();

                    if (targetPropertyInfo == null) continue;

                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourcePropertyInfo.GetGetMethod());
                    il.Emit(OpCodes.Callvirt, targetPropertyInfo.GetSetMethod());

                }
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);

                return (MapMethod<TTarget, TSource>)map.CreateDelegate(typeof(MapMethod<TTarget, TSource>));

            }
        }



        #region 

        public static Type BuildType(string className)
        {
            //AppDomain domain = AppDomain.CurrentDomain;
            AppDomain myDomain = Thread.GetDomain();
            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "MyDynamicAssembly";



            //创建一个永久程序集，设置为AssemblyBuilderAccess.RunAndSave。
            AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName,
                                                            AssemblyBuilderAccess.RunAndSave);

            //创建一个永久单模程序块。
            ModuleBuilder myModBuilder =
                myAsmBuilder.DefineDynamicModule(myAsmName.Name, myAsmName.Name + ".dll");
            //创建TypeBuilder。
            TypeBuilder myTypeBuilder = myModBuilder.DefineType(className,
                                                            TypeAttributes.Public);

            //创建类型。
            Type retval = myTypeBuilder.CreateType();

            //保存程序集，以便可以被Ildasm.exe解析，或被测试程序引用。
            myAsmBuilder.Save(myAsmName.Name + ".dll");
            return retval;
        }


        private static AssemblyName assmblyname;
        private static string DllName;
        private static AssemblyBuilder assemblybuilder;
        private static ModuleBuilder modulebuilder;
        private static TypeBuilder typebuilder;
        private static Type _dymaticType;

        public static Type DymaticType
        {
            get
            {
                return _dymaticType;
            }

            //set
            //{
            //    dymaticType = value;
            //}
        }

        public static void Create(string dllname)
        {
            DllName = dllname;
            assmblyname = new AssemblyName(DllName);
            ///2程序集生成器
            assemblybuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assmblyname, AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            ////3动态创建模块
            modulebuilder = assemblybuilder.DefineDynamicModule(assmblyname.Name, assmblyname.Name + ".dll");
        }
        public static void CreateClass(string NsClassName)
        {
            typebuilder = modulebuilder.DefineType(NsClassName, TypeAttributes.Public);
        }
        public static void CreateMember(string MemberName, Type memberType)
        {
            FieldBuilder fbNumber = typebuilder.DefineField(
              "m_" + MemberName,
             memberType,
              FieldAttributes.Private);


            PropertyBuilder pbNumber = typebuilder.DefineProperty(
                MemberName,
                System.Reflection.PropertyAttributes.HasDefault,
                memberType,
                null);


            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;


            MethodBuilder mbNumberGetAccessor = typebuilder.DefineMethod(
                "get_" + MemberName,
                getSetAttr,
                memberType,
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = typebuilder.DefineMethod(
                "set_" + MemberName,
                getSetAttr,
                null,
                new Type[] { memberType });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);
            ///最重要的是你最后要创建类型

        }
        public static Type SaveClass()
        {
            _dymaticType = typebuilder.CreateType();
            return DymaticType;
        }
        public static void Save()
        {
            assemblybuilder.Save(assmblyname.Name + ".dll");
        }
        ///// <summary>
        ///// 创建一个实体类并保存生成类型
        ///// </summary>
        ///// <param name="NsClassName"></param>
        ///// <param name="propertys"></param>
        //public void Execute(string NsClassName, Dictionary<string, Type> propertys)
        //{

        //    CreateClass(NsClassName);
        //    foreach (var item in propertys)
        //    {
        //        CreateMember(item.Key, item.Value);
        //    }
        //    SaveClass();
        //    Save();
        //}

        //public void Execute(List<M_DefineClass> _classes) {

        //    foreach (M_DefineClass _class in _classes)
        //    {
        //        CreateClass(_class.NsClassName);
        //        foreach (var prop in _class.Props)
        //        {
        //            CreateMember(prop.MemberName, prop.MemberType);
        //        }
        //        SaveClass();
        //    }

        //}

        //public void Test(string dllname, string NsClassName, string MemberName, Type memberType)
        //{
        //    //1设置程序集名称
        //    assmblyname = new AssemblyName(dllname);
        //    ///2程序集生成器
        //    assemblybuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assmblyname, AssemblyBuilderAccess.RunAndSave);
        //    3动态创建模块
        //    modulebuilder = assemblybuilder.DefineDynamicModule(assmblyname.Name, assmblyname.Name + ".dll");
        //    ///4.创建类
        //    typebuilder = modulebuilder.DefineType(NsClassName, TypeAttributes.Public);
        //    ///5.创建私有字段
        //    FieldBuilder fbNumber = typebuilder.DefineField(
        //     "m_" + MemberName,
        //     memberType,
        //     FieldAttributes.Private);

        //    ///6.创建共有属性
        //    PropertyBuilder pbNumber = typebuilder.DefineProperty(
        //        MemberName,
        //        System.Reflection.PropertyAttributes.HasDefault,
        //        memberType,
        //        null);


        //    MethodAttributes getSetAttr = MethodAttributes.Public |
        //        MethodAttributes.SpecialName | MethodAttributes.HideBySig;


        //    MethodBuilder mbNumberGetAccessor = typebuilder.DefineMethod(
        //        "get_" + MemberName,
        //        getSetAttr,
        //        memberType,
        //        Type.EmptyTypes);

        //    ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

        //    numberGetIL.Emit(OpCodes.Ldarg_0);
        //    numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
        //    numberGetIL.Emit(OpCodes.Ret);

        //    // Define the "set" accessor method for Number, which has no return
        //    // type and takes one argument of type int (Int32).
        //    MethodBuilder mbNumberSetAccessor = typebuilder.DefineMethod(
        //        "set_" + MemberName,
        //        getSetAttr,
        //        null,
        //        new Type[] { typeof(int) });

        //    ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
        //    // Load the instance and then the numeric argument, then store the
        //    // argument in the field.
        //    numberSetIL.Emit(OpCodes.Ldarg_0);
        //    numberSetIL.Emit(OpCodes.Ldarg_1);
        //    numberSetIL.Emit(OpCodes.Stfld, fbNumber);
        //    numberSetIL.Emit(OpCodes.Ret);

        //    // Last, map the "get" and "set" accessor methods to the 
        //    // PropertyBuilder. The property is now complete. 
        //    pbNumber.SetGetMethod(mbNumberGetAccessor);
        //    pbNumber.SetSetMethod(mbNumberSetAccessor);

        //    ///最重要的是你最后要创建类型
        //    Type t = typebuilder.CreateType();
        //    assemblybuilder.Save(assmblyname.Name + ".dll");

        //}

        #endregion
    }
}
