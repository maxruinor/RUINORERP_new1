using System;

namespace HLH.Lib.Helper
{
    // xml序列化 如果失败 可以尝试 保证有 无参数构造函数
    /// <summary>
    /// 保存列名和列的数据类型。
    /// </summary>
    [Serializable]
    public class SuperValue
    {
        public string superStrValue { get; set; }

        public string superDataTypeName { get; set; }

        private object tag = new object();
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public SuperValue()
        {

        }

        public override string ToString()
        {
            if (superStrValue== null)
            {
                return string.Empty;
            }
            else
            {
                return superStrValue.ToString();
            }
       
        }

        public SuperValue(string strValue,object _tag)
        {
            superStrValue = strValue;
            tag = _tag;
        }

        public SuperValue(string strValue, string DataTypeName)
        {
            superStrValue = strValue;
            superDataTypeName = DataTypeName;
        }
    }


    /// <summary>
    /// 自定义的可序列化的键值对 用于listBox等操作
    /// </summary>
    [Serializable]
    public class SuperKeyValue
    {
        public SuperKeyValue()
        {

        }


        public SuperKeyValue(string k, SuperValue v)
        {

            key = k;
            value = v;
        }

        private string key = string.Empty;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private SuperValue value = new SuperValue(string.Empty, string.Empty);

        public SuperValue Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }


    /// <summary>
    /// 自定义的可序列化的键值对 用于保存匹配结果等操作
    /// </summary>
    [Serializable]
    public class SuperKeyValuePair
    {
        public SuperKeyValuePair()
        {

        }

        private object tag = new object();
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public SuperKeyValuePair(SuperValue k, SuperValue v)
        {

            key = k;
            value = v;
        }

        private SuperValue key = new SuperValue(string.Empty, string.Empty);
        public SuperValue Key
        {
            get { return key; }
            set { key = value; }
        }

        private SuperValue value = new SuperValue(string.Empty, string.Empty);

        public SuperValue Value
        {
            get { return this.value; }
            set { this.value = value; }
        }


    }


    /// <summary>
    /// 自定义的可序列化的键值对
    /// </summary>
    [Serializable]
    public class KeyValue<k, v>
    {
        public KeyValue()
        {

        }

        public KeyValue(string k, string v)
        {
            key = k;
            value = v;
        }

        private object key = new object();

        public object Key
        {
            get { return key; }
            set { key = value; }
        }

        private object value = new object();

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }



    /// <summary>
    /// 自定义的可序列化的键值对
    /// </summary>
    [Serializable]
    public class KeyValue
    {

        public enum ToStringenum
        {
            key,
            value
        }

        public KeyValue()
        {

        }
        public KeyValue(string k, object v)
        {
            key = k;
            value = v;
        }

        public KeyValue(object k, object v)
        {
            key = k;
            value = v;
        }
        public KeyValue(string k, string v)
        {
            key = k;
            value = v;
        }
        public override string ToString()
        {
            if (SetToStringenum.ToString() == "key")
            {
                return this.key.ToString();
            }
            else
            {
                return this.value.ToString();
            }
        }
        private object key = new object();

        private ToStringenum setToStringenum;



        public object Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        private object tag = new object();

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private object value = new object();

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public ToStringenum SetToStringenum { get => setToStringenum; set => setToStringenum = value; }
    }
}
