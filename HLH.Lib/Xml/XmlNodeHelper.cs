using System;
using System.Collections.Generic;
using System.Xml;



namespace HLH.Lib.Xml
{

    /// <summary>
    /// XmlNode ������, �������ö�ȡ����, ���߲��ҽڵ��.
    /// </summary>
    /// <typeparam name="N">XmlNode ������������.</typeparam>
    public partial class XmlNodeHelper<N>
        where N : XmlNode
    {
        private static readonly List<XmlNodeHelper<XmlNode>> sharedFetchedNodeHelpers = new List<XmlNodeHelper<XmlNode>>();

        /// <summary>
        /// ��ȡ����ĵõ��� XmlNode �ӽڵ㸨����.
        /// </summary>
        public static List<XmlNodeHelper<XmlNode>> SharedFetchedNodeHelpers
        {
            get { return sharedFetchedNodeHelpers; }
        }

        #region " GetAttributeValue "
#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="nodeHelper">XmlNode ������.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>����ֵ.</returns>
		public static object GetAttributeValue ( XmlNodeHelper<XmlNode> nodeHelper, string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNodeHelper<XmlNode> nodeHelper, string name, object defaultValue)
#endif
        { return GetAttributeValue<object>(nodeHelper, name, defaultValue); }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="nodeHelper">XmlNode ������.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>����ֵ.</returns>
		public static T GetAttributeValue<T> ( XmlNodeHelper<XmlNode> nodeHelper, string name = null, T defaultValue = default(T) )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNodeHelper<XmlNode> nodeHelper, string name, T defaultValue)
#endif
        {

            if (null == nodeHelper)
                return default(T);

            return GetAttributeValue<T>(nodeHelper.node, name, defaultValue);
        }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="node">XmlNode ��.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>����ֵ.</returns>
		public static object GetAttributeValue ( XmlNode node, string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNode node, string name, object defaultValue)
#endif
        { return GetAttributeValue<object>(node, name, defaultValue); }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="node">XmlNode ��.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>����ֵ.</returns>
		public static T GetAttributeValue<T> ( XmlNode node, string name = null, T defaultValue = default(T) )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNode node, string name, T defaultValue)
#endif
        {
            T value = default(T);

            FetchAttributeValue<T>(ref value, node, name, defaultValue);
            return value;
        }
        #endregion

        #region " FetchAttributeValue "
#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="nodeHelper">XmlNode ������.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public static bool FetchAttributeValue ( ref object value, XmlNodeHelper<XmlNode> nodeHelper, string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNodeHelper<XmlNode> nodeHelper, string name, object defaultValue)
#endif
        { return FetchAttributeValue<object>(ref value, nodeHelper, name, defaultValue); }

#if PARAM
		/// <summary>
		/// ��ȡ����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="nodeHelper">XmlNode ������.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public static bool FetchAttributeValue<T> ( ref T value, XmlNodeHelper<XmlNode> nodeHelper, string name = null, T defaultValue = default(T) )
#else
        /// <summary>
        /// ��ȡ����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNodeHelper<XmlNode> nodeHelper, string name, T defaultValue)
#endif
        {

            if (null == nodeHelper)
                return false;

            return FetchAttributeValue<T>(ref value, nodeHelper.node, name, defaultValue);
        }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="node">XmlNode ��.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public static bool FetchAttributeValue ( ref object value, XmlNode node, string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNode node, string name, object defaultValue)
#endif
        { return FetchAttributeValue<object>(ref value, node, name, defaultValue); }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="node">XmlNode ��.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public static bool FetchAttributeValue<T> ( ref T value, XmlNode node, string name = null, T defaultValue = default(T) )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNode node, string name, T defaultValue)
#endif
        {

            if (null == node)
                return false;

            if (string.IsNullOrEmpty(name))
                name = "value";

            XmlAttribute attribute = node.Attributes[name];

            if (null != attribute)
                try
                {
                    value = StringConvert.ToObject<T>(attribute.Value);

                    return true;
                }
                catch
                { }

            if (null == defaultValue)
                value = default(T);
            else
                value = defaultValue;

            return false;
        }
        #endregion

        #region " FlushAttributeValue "
#if PARAM
		/// <summary>
		/// ���ø���������ʾ XmlNode ������ֵ. 
		/// </summary>
		/// <param name="nodeHelper">XmlNode ������.</param>
		/// <param name="value">����ֵ.</param>
		/// <param name="name">���Ե�����, Ĭ��Ϊ null.</param>
		/// <returns>�Ƿ�ɹ�����.</returns>
		public static bool FlushAttributeValue ( XmlNodeHelper<XmlNode> nodeHelper, object value, string name = null )
#else
        /// <summary>
        /// ���ø���������ʾ XmlNode ������ֵ. 
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="value">����ֵ.</param>
        /// <param name="name">���Ե�����.</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public static bool FlushAttributeValue(XmlNodeHelper<XmlNode> nodeHelper, object value, string name)
#endif
        {

            if (null == nodeHelper)
                return false;

            return FlushAttributeValue(nodeHelper.node, value, name);
        }

#if PARAM
		/// <summary>
		/// ���ø���������ʾ XmlNode ������ֵ. 
		/// </summary>
		/// <param name="node">XmlNode ��.</param>
		/// <param name="value">����ֵ.</param>
		/// <param name="name">���Ե�����, Ĭ��Ϊ "value".</param>
		/// <returns>�Ƿ�ɹ�����.</returns>
		public static bool FlushAttributeValue ( XmlNode node, object value, string name = null )
#else
        /// <summary>
        /// ���ø���������ʾ XmlNode ������ֵ. 
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="value">����ֵ.</param>
        /// <param name="name">���Ե�����.</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public static bool FlushAttributeValue(XmlNode node, object value, string name)
#endif
        {

            if (null == value || null == node)
                return false;

            if (null == name)
                name = "value";

            XmlAttribute attribute = node.Attributes[name];

            if (null == attribute)
                return false;

            attribute.Value = StringConvert.ToString(value);
            return true;
        }
        #endregion

        #region " Fetch(Get)NodeHelper(s) "
        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNodeHelper">��ʼ�� XmlNode ������.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public static XmlNodeHelper<XmlNode> GetNodeHelper(XmlNodeHelper<XmlNode> beginNodeHelper, string xPath)
        {

            if (null == beginNodeHelper)
                return null;

            return GetNodeHelper(beginNodeHelper.node, xPath);
        }
        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public static XmlNodeHelper<XmlNode> GetNodeHelper(XmlNode beginNode, string xPath)
        {
            XmlNodeHelper<XmlNode> nodeHelper = null;

            FetchNodeHelper(ref nodeHelper, beginNode, xPath);
            return nodeHelper;
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelper">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="beginNodeHelper">��ʼ�� XmlNode ������.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelper(ref XmlNodeHelper<XmlNode> nodeHelper, XmlNodeHelper<XmlNode> beginNodeHelper, string xPath)
        {
            if (null == beginNodeHelper)
                return false;

            return FetchNodeHelper(ref nodeHelper, beginNodeHelper.node, xPath);
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelper(XmlNode beginNode, string xPath)
        {
            XmlNodeHelper<XmlNode> nodeHelper = null;

            return FetchNodeHelper(ref nodeHelper, beginNode, xPath);
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelper">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelper(ref XmlNodeHelper<XmlNode> nodeHelper, XmlNode beginNode, string xPath)
        {
            List<XmlNodeHelper<XmlNode>> nodeHelpers = GetNodeHelpers(beginNode, xPath);

            if (nodeHelpers.Count == 0)
                return false;

            nodeHelper = nodeHelpers[0];
            sharedFetchedNodeHelpers.Clear();
            sharedFetchedNodeHelpers.Add(nodeHelper);
            return true;
        }

        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNodeHelper">��ʼ�� XmlNode ������.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public static List<XmlNodeHelper<XmlNode>> GetNodeHelpers(XmlNodeHelper<XmlNode> beginNodeHelper, string xPath)
        {

            if (null == beginNodeHelper)
                return new List<XmlNodeHelper<XmlNode>>();

            return GetNodeHelpers(beginNodeHelper.node, xPath);
        }
        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public static List<XmlNodeHelper<XmlNode>> GetNodeHelpers(XmlNode beginNode, string xPath)
        {
            List<XmlNodeHelper<XmlNode>> nodeHelpers = new List<XmlNodeHelper<XmlNode>>();

            FetchNodeHelpers(ref nodeHelpers, beginNode, xPath);
            return nodeHelpers;
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelpers">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="beginNodeHelper">��ʼ�� XmlNode ������.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelpers(ref List<XmlNodeHelper<XmlNode>> nodeHelpers, XmlNodeHelper<XmlNode> beginNodeHelper, string xPath)
        {

            if (null == beginNodeHelper)
                return false;

            return FetchNodeHelpers(ref nodeHelpers, beginNodeHelper.node, xPath);
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelpers(XmlNode beginNode, string xPath)
        {
            List<XmlNodeHelper<XmlNode>> nodeHelpers = null;

            return FetchNodeHelpers(ref nodeHelpers, beginNode, xPath);
        }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelpers">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="beginNode">��ʼ�� XmlNode ��.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public static bool FetchNodeHelpers(ref List<XmlNodeHelper<XmlNode>> nodeHelpers, XmlNode beginNode, string xPath)
        {

            if (null == nodeHelpers)
                nodeHelpers = new List<XmlNodeHelper<XmlNode>>();

            if (null == beginNode || string.IsNullOrEmpty(xPath))
                return false;

            try
            {

                foreach (XmlNode node in beginNode.SelectNodes(xPath))
                    nodeHelpers.Add(new XmlNodeHelper<XmlNode>(node));

            }
            catch { }

            if (nodeHelpers.Count == 0)
                return false;

            sharedFetchedNodeHelpers.Clear();
            sharedFetchedNodeHelpers.AddRange(nodeHelpers.ToArray());
            return true;
        }
        #endregion

        #region " AppendNode "
        /// <summary>
        /// ��� Xml �ڵ�.
        /// </summary>
        /// <param name="parentNodeHelper">��ӵ��Ľڵ�ĸ�����.</param>
        /// <param name="childNodeHelper">��ӽڵ�ĸ�����.</param>
        public static void AppendNode(XmlNodeHelper<XmlNode> parentNodeHelper, XmlNodeHelper<XmlNode> childNodeHelper)
        {

            if (null == parentNodeHelper || null == parentNodeHelper.node || null == childNodeHelper)
                return;

            try
            { parentNodeHelper.node.InnerXml += childNodeHelper.OuterXml; }
            catch { }

        }
        #endregion

        private readonly string name;
        protected N node;
        private readonly List<XmlNodeHelper<XmlNode>> childNodeHelpers = new List<XmlNodeHelper<XmlNode>>();

        private readonly SortedList<string, object> attributes = new SortedList<string, object>();

        private readonly List<XmlNodeHelper<XmlNode>> fetchedNodeHelpers = new List<XmlNodeHelper<XmlNode>>();

        /// <summary>
        /// ��ȡ�ڵ�����.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// ��ȡ�̳��� XmlNode �Ľڵ�.
        /// </summary>
        public N Node
        {
            get { return this.node; }
        }

        /// <summary>
        /// ��ȡ�ӽڵ�� XmlNodeHelper.
        /// </summary>
        public List<XmlNodeHelper<XmlNode>> ChildNodeHelpers
        {
            get { return this.childNodeHelpers; }
        }

        /// <summary>
        /// ��ȡ XmlNodeHelper �Լ� ChildNodeHelpers ���ɵ� OuterXml.
        /// </summary>
        public string OuterXml
        {
            get
            {
                string xml = string.Format("<{0}", this.name);

                foreach (string attributeName in this.attributes.Keys)
                    xml += string.Format(" {0}=\"{1}\"", attributeName, StringConvert.ToString(this.attributes[attributeName]));

                if (this.childNodeHelpers.Count == 0)
                    xml += " />";
                else
                {
                    xml += ">";

                    foreach (XmlNodeHelper<XmlNode> childNodeHelper in this.childNodeHelpers)
                        if (null != childNodeHelper)
                            xml += childNodeHelper.OuterXml;

                    xml += string.Format("</{0}>", this.name);
                }

                return xml;
            }
        }

        /// <summary>
        /// ��ȡ�õ��� XmlNode �ӽڵ㸨����.
        /// </summary>
        public List<XmlNodeHelper<XmlNode>> FetchedNodeHelpers
        {
            get { return this.fetchedNodeHelpers; }
        }

        /// <summary>
        /// ��ȡ���ýڵ������.
        /// </summary>
        /// <param name="attributeName">��������.</param>
        /// <returns>����ֵ.</returns>
        public object this[string attributeName]
        {
            get
            {

                if (string.IsNullOrEmpty(attributeName) || !this.attributes.ContainsKey(attributeName))
                    return string.Empty;

                return this.attributes[attributeName];
            }
            set
            {

                if (string.IsNullOrEmpty(attributeName))
                    return;

                if (this.attributes.ContainsKey(attributeName))
                    this.attributes[attributeName] = value;
                else
                    this.attributes.Add(attributeName, value);

            }
        }

        /// <summary>
        /// �� XmlNode ����һ��������.
        /// </summary>
        /// <param name="node">XmlNode.</param>
        public XmlNodeHelper(N node)
        {

            if (null == node)
                throw new ArgumentNullException("node", " Xml �ڵ㲻��Ϊ��");

            this.name = node.Name;
            this.node = node;
        }

        /// <summary>
        /// ����ӵ�����ƺ��ӽڵ�� XmlNode ������.
        /// </summary>
        /// <param name="name">�ڵ�����.</param>
        /// <param name="childNodeHelpers">�ӽڵ�� XmlNode ������.</param>
        public XmlNodeHelper(string name, params XmlNodeHelper<XmlNode>[] childNodeHelpers)
        {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", " �ڵ����Ʋ���Ϊ��");

            if (null != childNodeHelpers)
                foreach (XmlNodeHelper<XmlNode> childNodeHelper in childNodeHelpers)
                    if (null != childNodeHelper)
                        this.childNodeHelpers.Add(childNodeHelper);

            this.name = name;
        }

        #region " GetAttributeValue "
#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>����ֵ.</returns>
		public object GetAttributeValue ( string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public object GetAttributeValue(string name, object defaultValue)
#endif
        { return GetAttributeValue<object>(this.node, name, defaultValue); }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>����ֵ.</returns>
		public T GetAttributeValue<T> ( string name = null, T defaultValue = default(T) )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public T GetAttributeValue<T>(string name, T defaultValue)
#endif
        { return GetAttributeValue<T>(this.node, name, defaultValue); }
        #endregion

        #region " FetchAttributeValue "
#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ null.</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public bool FetchAttributeValue ( ref object value, string name = null, object defaultValue = null )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue(ref object value, string name, object defaultValue)
#endif
        { return FetchAttributeValue<object>(ref value, this.node, name, defaultValue); }

#if PARAM
		/// <summary>
		/// �õ�����������ʾ XmlNode ������ֵ.
		/// </summary>
		/// <typeparam name="T">���Ե�����.</typeparam>
		/// <param name="value">���ص�����ֵ.</param>
		/// <param name="name">��������, Ĭ��Ϊ "value".</param>
		/// <param name="defaultValue">����Ĭ��ֵ, Ĭ��Ϊ default(T).</param>
		/// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
		public bool FetchAttributeValue<T> ( ref T value, string name=null, T defaultValue = default(T) )
#else
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="name">��������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue<T>(ref T value, string name, T defaultValue)
#endif
        { return FetchAttributeValue<T>(ref value, this.node, name, defaultValue); }
        #endregion

        #region " FlushAttributeValue "
        /// <summary>
        /// ���ø���������ʾ XmlNode �� value ����ֵ. 
        /// </summary>
        /// <param name="value">����ֵ.</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public bool FlushAttributeValue(object value)
        {
            // �޷�ʹ�� FlushAttributeValue ( this.node, value );
            return FlushAttributeValue(this.node, value, null);
        }
        /// <summary>
        /// ���ø���������ʾ XmlNode ������ֵ. 
        /// </summary>
        /// <param name="value">����ֵ.</param>
        /// <param name="name">���Ե�����, Ĭ��Ϊ "value".</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public bool FlushAttributeValue(object value, string name)
        { return FlushAttributeValue(this.node, value, name); }
        #endregion

        #region " Fetch(Get)NodeHelper(s) "
        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public XmlNodeHelper<XmlNode> GetNodeHelper(string xPath)
        { return GetNodeHelper(this.node, xPath); }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public bool FetchNodeHelper(string xPath)
        { return FetchNodeHelper(this.node, xPath); }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelper">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public bool FetchNodeHelper(ref XmlNodeHelper<XmlNode> nodeHelper, string xPath)
        {
            bool isSuccess = FetchNodeHelper(ref nodeHelper, this.node, xPath);

            this.fetchedNodeHelpers.Clear();
            this.fetchedNodeHelpers.Add(nodeHelper);

            return isSuccess;
        }
        /// <summary>
        /// �õ� XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�������ӽڵ㸨����.</returns>
        public List<XmlNodeHelper<XmlNode>> GetNodeHelpers(string xPath)
        { return GetNodeHelpers(this.node, xPath); }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public bool FetchNodeHelpers(string xPath)
        { return FetchNodeHelpers(this.node, xPath); }
        /// <summary>
        /// ��ȡ XmlNode ���ӽڵ�ĸ�����.
        /// </summary>
        /// <param name="nodeHelpers">���ص��ӽڵ�ĸ�����.</param>
        /// <param name="xPath">���������� xpath.</param>
        /// <returns>�Ƿ��������ӽڵ�.</returns>
        public bool FetchNodeHelpers(ref List<XmlNodeHelper<XmlNode>> nodeHelpers, string xPath)
        {
            bool isSuccess = FetchNodeHelpers(ref nodeHelpers, this.node, xPath);

            this.fetchedNodeHelpers.Clear();
            this.fetchedNodeHelpers.AddRange(nodeHelpers.ToArray());

            return isSuccess;
        }
        #endregion

        #region " AppendNode "
        /// <summary>
        /// ��� Xml �ڵ�.
        /// </summary>
        /// <param name="childNodeHelper">��ӽڵ�ĸ�����.</param>
        public void AppendNode(XmlNodeHelper<XmlNode> childNodeHelper)
        { /* AppendNode ( this, childNodeHelper ); */ }
        #endregion

    }

    partial class XmlNodeHelper<N>
    {
#if !PARAM
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNodeHelper<XmlNode> nodeHelper)
        { return GetAttributeValue<object>(nodeHelper, null, null); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNodeHelper<XmlNode> nodeHelper, object defaultValue)
        { return GetAttributeValue<object>(nodeHelper, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNodeHelper<XmlNode> nodeHelper, string name)
        { return GetAttributeValue<object>(nodeHelper, name, null); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNodeHelper<XmlNode> nodeHelper)
        { return GetAttributeValue<T>(nodeHelper, null, default(T)); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNodeHelper<XmlNode> nodeHelper, T defaultValue)
        { return GetAttributeValue<T>(nodeHelper, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNodeHelper<XmlNode> nodeHelper, string name)
        { return GetAttributeValue<T>(nodeHelper, name, default(T)); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNode node)
        { return GetAttributeValue<object>(node, null, null); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNode node, object defaultValue)
        { return GetAttributeValue<object>(node, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public static object GetAttributeValue(XmlNode node, string name)
        { return GetAttributeValue<object>(node, name, null); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="node">XmlNode ��.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNode node)
        { return GetAttributeValue<T>(node, null, default(T)); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNode node, T defaultValue)
        { return GetAttributeValue<T>(node, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public static T GetAttributeValue<T>(XmlNode node, string name)
        { return GetAttributeValue<T>(node, name, default(T)); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNode node)
        { return FetchAttributeValue<T>(ref value, node, null, default(T)); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNode node, T defaultValue)
        { return FetchAttributeValue<T>(ref value, node, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNode node, string name)
        { return FetchAttributeValue<T>(ref value, node, name, default(T)); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNode node)
        { return FetchAttributeValue<object>(ref value, node, null, null); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNode node, object defaultValue)
        { return FetchAttributeValue<object>(ref value, node, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNode node, string name)
        { return FetchAttributeValue<object>(ref value, node, name, null); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNodeHelper<XmlNode> nodeHelper)
        { return FetchAttributeValue<T>(ref value, nodeHelper, null, default(T)); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNodeHelper<XmlNode> nodeHelper, T defaultValue)
        { return FetchAttributeValue<T>(ref value, nodeHelper, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue<T>(ref T value, XmlNodeHelper<XmlNode> nodeHelper, string name)
        { return FetchAttributeValue<T>(ref value, nodeHelper, name, default(T)); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNodeHelper<XmlNode> nodeHelper)
        { return FetchAttributeValue<object>(ref value, nodeHelper, null, null); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNodeHelper<XmlNode> nodeHelper, object defaultValue)
        { return FetchAttributeValue<object>(ref value, nodeHelper, null, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public static bool FetchAttributeValue(ref object value, XmlNodeHelper<XmlNode> nodeHelper, string name)
        { return FetchAttributeValue<object>(ref value, nodeHelper, name, null); }

        /// <summary>
        /// ���ø���������ʾ XmlNode �� value ����ֵ. 
        /// </summary>
        /// <param name="node">XmlNode ��.</param>
        /// <param name="value">����ֵ.</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public static bool FlushAttributeValue(XmlNode node, object value)
        { return FlushAttributeValue(node, value, null); }

        /// <summary>
        /// ���ø���������ʾ XmlNode �� value ����ֵ. 
        /// </summary>
        /// <param name="nodeHelper">XmlNode ������.</param>
        /// <param name="value">����ֵ.</param>
        /// <returns>�Ƿ�ɹ�����.</returns>
        public static bool FlushAttributeValue(XmlNodeHelper<XmlNode> nodeHelper, object value)
        { return FlushAttributeValue(nodeHelper, value, null); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <returns>����ֵ.</returns>
        public object GetAttributeValue()
        { return GetAttributeValue<object>(this.node); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public object GetAttributeValue(object defaultValue)
        { return GetAttributeValue<object>(this.node, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public object GetAttributeValue(string name)
        { return GetAttributeValue<object>(this.node, name); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <returns>����ֵ.</returns>
        public T GetAttributeValue<T>()
        { return GetAttributeValue<T>(this.node); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>����ֵ.</returns>
        public T GetAttributeValue<T>(T defaultValue)
        { return GetAttributeValue<T>(this.node, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="name">��������.</param>
        /// <returns>����ֵ.</returns>
        public T GetAttributeValue<T>(string name)
        { return GetAttributeValue<T>(this.node, name); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue(ref object value)
        { return FetchAttributeValue<object>(ref value, this.node); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue(ref object value, object defaultValue)
        { return FetchAttributeValue<object>(ref value, this.node, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue(ref object value, string name)
        { return FetchAttributeValue<object>(ref value, this.node, name); }

        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue<T>(ref T value)
        { return FetchAttributeValue<T>(ref value, this.node); }
        /// <summary>
        /// �õ�����������ʾ XmlNode �� value ����ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="defaultValue">����Ĭ��ֵ.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue<T>(ref T value, T defaultValue)
        { return FetchAttributeValue<T>(ref value, this.node, defaultValue); }
        /// <summary>
        /// �õ�����������ʾ XmlNode ������ֵ.
        /// </summary>
        /// <typeparam name="T">���Ե�����.</typeparam>
        /// <param name="value">���ص�����ֵ.</param>
        /// <param name="name">��������.</param>
        /// <returns>�Ƿ�ɹ���ȡ�� XmlNode ������.</returns>
        public bool FetchAttributeValue<T>(ref T value, string name)
        { return FetchAttributeValue<T>(ref value, this.node, name); }
#endif
    }

}