using System;
using System.Linq.Expressions;
using RUINORERP.Global.Extensions;
namespace RUINORERP.Global.Model
{
    public class KeyNamePair
    {
        public KeyNamePair()
        {

        }
        public KeyNamePair(string _Key)
        {
            Key = _Key;
        }

        public KeyNamePair(string _Key, string _Name)
        {
            Key = _Key;
            Name = _Name;
        }

        public virtual void SetKeyName<T1>(T1 entity) where T1 : class
        {

        }

        public virtual void SetKeyName<T1, T2>(T1 key, T2 name) where T1 : class where T2 : class
        {

        }


        public string Key { get; set; }
        public string Name { get; set; }


        Type _fkTableType;
        /// <summary>
        /// 如果需要从关联外键取数据的话，则需要知道表名
        /// </summary>
        public Type FkTableType { get => _fkTableType; set => _fkTableType = value; }


    }

    public class KeyNamePair<T1> : KeyNamePair
    {


        public Expression<Func<T1, object>> ColKeyExp { get => colKeyExp; set => colKeyExp = value; }


        public KeyNamePair()
        {

        }
        public KeyNamePair(T1 _Key)
        {

        }

        Expression<Func<T1, object>> colKeyExp;



        public KeyNamePair(Expression<Func<T1, object>> colKeyExp)
        {
            this.colKeyExp = colKeyExp;
            this.Key = colKeyExp.GetMemberInfoGlobal().Name;
        }


    }



    public class KeyNamePair<T1, T2> : KeyNamePair
    {

        public Expression<Func<T1, object>> ColKeyExp { get => colKeyExp; set => colKeyExp = value; }
        public Expression<Func<T2, object>> ColNameExp { get => colNameExp; set => colNameExp = value; }


        Expression<Func<T1, object>> colKeyExp;
        Expression<Func<T2, object>> colNameExp;


        public KeyNamePair(Expression<Func<T1, object>> colKeyExp, Expression<Func<T2, object>> colNameExp)
        {
            this.colKeyExp = colKeyExp;
            this.colNameExp = colNameExp;
            this.FkTableType = typeof(T2);
            this.Key = colKeyExp.GetMemberInfoGlobal().Name;
            this.Name = colNameExp.GetMemberInfoGlobal().Name;

        }


    }


}



