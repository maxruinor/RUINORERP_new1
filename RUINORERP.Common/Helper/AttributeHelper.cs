using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RUINORERP.Common.Helper
{

    public class AttributeHelper
    {
        /*
         //通常都會針對同一個Attribute做操作，因此這邊設計成可以先設定一個Attribute
	//再用Get來對不同對象來取得
	//如果只需要取一次，也可直接寫成
	//AttributeHelper.SetAttribute<DescriptionAttribute>().Get<TModel>(...);
	var descriptionAttr=AttributeHelper.SetAttribute<DescriptionAttribute>();
	//類別
    descriptionAttr.Get<Test>();
	//屬性
	descriptionAttr.Get<Test>(p=>p.Name);
	//欄位
	descriptionAttr.Get<Test>(p=>p.file);
	//建構子1
	descriptionAttr.Get<Test>(p=>new Test());
	//建構子2
	descriptionAttr.Get<Test>(p=>new Test(""));
	//方法1
	descriptionAttr.Get<Test>("GetString");
	//方法2
	descriptionAttr.Get<Test>("GetString",typeof(string));
	//事件
	descriptionAttr.Get<Test>("MyDelegate");
	//委派
	descriptionAttr.Get<Test>("MyEvent");
	//列舉1
	descriptionAttr.Get<付款方式>(p=>付款方式.信用卡);
	//列舉2
	付款方式 e=付款方式.現金;
	descriptionAttr.Get<付款方式>(e.ToString());
         */
        #region -- 屬性與建構子 --

        protected AttributeHelper()
        {
        }

        protected BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary> 設定BindingFlags </summary>
        public BindingFlags SettingBindingFlags { get { return _BindingFlags; } set { _BindingFlags = value; } }

        /// <summary> 要擷取的Attribute </summary>
        protected Type AttributeType { get; set; }

        #endregion

        #region -- 方法 --

        /// <summary> 設定要擷取的Attribute </summary>
        public static AttributeHelper<T> SetAttribute<T>()
        {
            var helper = new AttributeHelper<T>();
            helper.AttributeType = typeof(T);
            return helper;
        }

        protected object GetMemberAttributes(Type type)
        {
            return type.GetCustomAttributes(false).FirstOrDefault();
        }

        protected object GetMemberAttributes(Type type, ExpressionHelper.ExpressionMemberInfo expressionMemberInfo)
        {
            MemberInfo memberInfo = null;
            if (expressionMemberInfo.ParameterTypes == null)
            {
                memberInfo = type.GetMember(expressionMemberInfo.MemberName, _BindingFlags).FirstOrDefault();
            }
            else
            {
                memberInfo = type.GetMember(expressionMemberInfo.MemberName, _BindingFlags)
                                 .FirstOrDefault(p => ((MethodBase)p).GetParameters()
                                 .Select(s => s.ParameterType)
                                 .SequenceEqual(expressionMemberInfo.ParameterTypes)
                                 );
            }
            return GetResult(memberInfo);
        }

        private object GetResult(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException();

            var attr = memberInfo.GetCustomAttributes(AttributeType, false).FirstOrDefault();
            if (memberInfo == null) return null;

            return attr;
        }

        #endregion

        #region -- 快速使用區(如果有常用的Attribute，可以擴充在這邊) --

        public static DescriptionAttribute GetDescription<TModel>()
        {
            return AttributeHelper.SetAttribute<DescriptionAttribute>().Get<TModel>();
        }

        public static DescriptionAttribute GetDescription<TModel>(string memberName, params Type[] types)
        {
            return AttributeHelper.SetAttribute<DescriptionAttribute>().Get<TModel>(memberName, types);
        }

        public static DescriptionAttribute GetDescription<TModel>(Expression<Func<TModel, object>> func)
        {
            return AttributeHelper.SetAttribute<DescriptionAttribute>().Get<TModel>(func);
        }

        #endregion


        #region -- 快速使用區(如果有常用的Attribute，可以擴充在這邊) --

        public static SugarColumn GetSugarColumn<TModel>()
        {
            return AttributeHelper.SetAttribute<SugarColumn>().Get<TModel>();
        }

        public static SugarColumn GetSugarColumn<TModel>(string memberName, params Type[] types)
        {
            return AttributeHelper.SetAttribute<SugarColumn>().Get<TModel>(memberName, types);
        }

        public static SugarColumn GetSugarColumn<TModel>(Expression<Func<TModel, object>> func)
        {
            return AttributeHelper.SetAttribute<SugarColumn>().Get<TModel>(func);
        }

        #endregion

    }

    public class AttributeHelper<T> : AttributeHelper
    {
        protected internal AttributeHelper()
        {
        }

        /// <summary> 取得類別的Attribute </summary>
        public T Get<TModel>()
        {
            return (T)GetMemberAttributes(typeof(TModel));
        }

        /// <summary>
        /// 取得指定成員名稱的Attribute
        /// </summary>
        /// <typeparam name="TModel">目標型別</typeparam>
        /// <param name="memberName">成員名稱</param>
        /// <param name="types">如果是MethodBase，可傳入參數的Type</param>
        public T Get<TModel>(string memberName, params Type[] types)
        {
            return (T)GetMemberAttributes(typeof(TModel), new ExpressionHelper.ExpressionMemberInfo() { MemberName = memberName, ParameterTypes = types.ToList() });
        }


        /// <summary>
        /// 取得指定成員名稱的Attribute
        /// </summary>
        /// <typeparam name="TModel">目標型別</typeparam>
        /// <param name="func">可用Lambda的方式指定成員名稱</param>
        public T Get<TModel>(Expression<Func<TModel, object>> func)
        {
            var expressionMemberInfo = ExpressionHelper.GetMemberName(func);
            return (T)GetMemberAttributes(typeof(TModel), expressionMemberInfo);
        }
     
    }
}

