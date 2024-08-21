using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.AdvancedQuery
{

    public class TestModel
    {
        [RangeAttribute(1, 100)]
        public int Count { get; set; }
    }

    class Program111
    {
        static void Maintest()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(TestModel));
            GetModifiedTypes(types);
        }

        public static List<Type> GetModifiedTypes(List<Type> models)
        {
            Dictionary<Type, Dictionary<PropertyInfo, List<Attribute>>> modelInfo = new Dictionary<Type, Dictionary<PropertyInfo, List<Attribute>>>();
            foreach (Type model in models)
            {
                modelInfo.Add(model, new Dictionary<PropertyInfo, List<Attribute>>());

                foreach (PropertyInfo propertyInfo in model.GetProperties())
                {
                    var customAttributeDataList = propertyInfo.GetCustomAttributes(typeof(Attribute), true).Select(x => x as Attribute).ToList();

                    for (int i = 0; i < customAttributeDataList.Count(); i++)
                    {
                        var atr = customAttributeDataList[i] as dynamic;
                        customAttributeDataList[i] = GetModifiedAttribute(atr);
                    }
                    modelInfo[model].Add(propertyInfo, customAttributeDataList);
                }
            }

            ModifiedTypeGenerator mtg = new ModifiedTypeGenerator();
            return mtg.GenerateModifiedTypes(modelInfo);
        }

        public static Attribute GetModifieAttribute(Attribute attribute)
        {
            return attribute;
        }

        public static RangeAttribute GetModifiedAttribute(RangeAttribute attribute)
        {
            Random r = new Random();
            return new RangeAttribute(r.Next(), r.Next());
        }
    }

    /// <summary>
    /// 类的修改生成器
    /// </summary>
    public class ModifiedTypeGenerator
    {
        Dictionary<Type, Dictionary<PropertyInfo, List<Attribute>>> modelInfo;

        public List<Type> GenerateModifiedTypes(Dictionary<Type, Dictionary<PropertyInfo, List<Attribute>>> models)
        {
            modelInfo = models;
            List<Type> toReturn = new List<Type>();
            foreach (Type model in modelInfo.Keys)
            {
                TypeBuilder tb = CreateTypeBuilder(model);
                _ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
                foreach (PropertyInfo propertyInfo in modelInfo[model].Keys)
                {
                    CreateProperty(tb, model, propertyInfo);
                }
                Type newType = tb.CreateType();
                toReturn.Add(newType);
            }
            return toReturn;
        }

        public TypeBuilder CreateTypeBuilder(Type model)
        {
            string typeName = "modified_" + model.Name;
            AssemblyName assemblyName = new AssemblyName(typeName);

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");

            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName,
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout,
                        model
                    );
            return typeBuilder;
        }

        public void CreateProperty(TypeBuilder typeBuilder, Type model, PropertyInfo propertyInfo)
        {
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyInfo.Name, propertyInfo.PropertyType, FieldAttributes.Public);
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, propertyInfo.PropertyType, null);

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyInfo.PropertyType, Type.EmptyTypes);
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { propertyInfo.PropertyType });

            ILGenerator getIl = getMethodBuilder.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);


            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
            AddAttributes(propertyBuilder, model, propertyInfo);
        }


        public void AddAttributes(PropertyBuilder propertyBuilder, Type model, PropertyInfo propertyInfo)
        {
            foreach (var attribute in modelInfo[model][propertyInfo])
            {
                CustomAttributeData customAttributeData =
                         propertyInfo.CustomAttributes.First(x => x.AttributeType == attribute.GetType());
                //var constructorArguments =
                //    customAttributeData.ConstructorArguments.Select(x => x as object).ToArray();
                var constructorArguments = 
                      customAttributeData.ConstructorArguments.Select(x => (object)x.Value).ToArray();

                CustomAttributeBuilder customAttributeBuilder =
                    new CustomAttributeBuilder(customAttributeData.Constructor, constructorArguments);
                propertyBuilder.SetCustomAttribute(customAttributeBuilder);
            }
        }
    }
}
