using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection.Emit;
using System.Reflection;

namespace RUINORERP.Model.Base
{

    public class Dynamic
    {
        //Dynamic d = new Dynamic();
        //d = d.Add("MyProperty", 42);
        public Dynamic Add<T>(string key, T value)
        {
            // 使用RunAndSave模式,确保VS调试器能够访问类型信息
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("RUINORERP.Model.dll"), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Dynamic.dll");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(Guid.NewGuid().ToString());
            typeBuilder.SetParent(this.GetType());
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(key, PropertyAttributes.None, typeof(T), Type.EmptyTypes);

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + key, MethodAttributes.Public, CallingConventions.HasThis, typeof(T), Type.EmptyTypes);
            ILGenerator getter = getMethodBuilder.GetILGenerator();
            getter.Emit(OpCodes.Ldarg_0);
            getter.Emit(OpCodes.Ldstr, key);
            getter.Emit(OpCodes.Callvirt, typeof(Dynamic).GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(typeof(T)));
            getter.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getMethodBuilder);

            Type type = typeBuilder.CreateType();

            Dynamic child = (Dynamic)Activator.CreateInstance(type);
            child.dictionary = this.dictionary;
            dictionary.Add(key, value);
            return child;
        }

        protected T Get<T>(string key)
        {
            return (T)dictionary[key];
        }

        private Dictionary<string, object> dictionary = new Dictionary<string, object>();
    }


    public class DynamicEntityDto: DynamicObject
    {
       // private readonly Dictionary<string, object> _properties;

        Dictionary<string, object> _properties = new Dictionary<string, object>();

        //public DynamicEntityDto(Dictionary<string, object> properties)
        //{
        //    _properties = properties;
        //}

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                _properties[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    

}
