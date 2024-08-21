/*
 * wiki: http://code.google.com/p/zsharedcode/wiki/XmlDocumentHelper
 * ������޷����д��ļ�, ��������ȱ��������ļ�, �����ؽ������������, ������ο�: http://code.google.com/p/zsharedcode/wiki/HowToDownloadAndUse
 * ԭʼ����: http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/.class/xml/XmlDocumentHelper.cs
 * �汾: .net 4.0, �����汾����������ͬ
 * 
 * ʹ�����: ���ļ��ǿ�Դ������ѵ�, ������Ȼ��Ҫ����, ���ز��� panzer ���֤ http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/panzer.license.txt ��������Ĳ�Ʒ��.
 * */

// HACK: ����Ŀ�ж��������� PARAM, ʹ���ṩĬ�ϲ����ķ���.

using System.Xml;

namespace HLH.Lib.Xml
{

    /// <summary>
    /// ���� XmlDocument �ĸ�����.
    /// </summary>
    public sealed partial class XmlDocumentHelper
        : XmlNodeHelper<XmlDocument>
    {
        private string filePath;
        private XmlNodeHelper<XmlNode> fileNodeHelper;
        private XmlNodeHelper<XmlNode> rootNodeHelper;
        private XmlNodeHelper<XmlNode> currentNodeHelper;

        /// <summary>
        /// ��ȡ�ļ��ڵ㸨����.
        /// </summary>
        public XmlNodeHelper<XmlNode> FileNodeHelper
        {
            get { return this.fileNodeHelper; }
        }

        /// <summary>
        /// ��ȡ���ڵ㸨����.
        /// </summary>
        public XmlNodeHelper<XmlNode> RootNodeHelper
        {
            get { return this.rootNodeHelper; }
        }

        /// <summary>
        /// ��ȡ���ڵ�.
        /// </summary>
        public XmlNode RootNode
        {
            get
            {

                if (null == this.rootNodeHelper)
                    return null;

                return this.rootNodeHelper.Node;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ�ڵ㸨����.
        /// </summary>
        public XmlNodeHelper<XmlNode> CurrentNodeHelper
        {
            get { return this.currentNodeHelper; }
        }

        /// <summary>
        /// ��ȡ��ǰ�ڵ�.
        /// </summary>
        public XmlNode CurrentNode
        {
            get
            {

                if (null == this.currentNodeHelper)
                    return null;

                return this.currentNodeHelper.Node;
            }
        }

        /// <summary>
        /// ��ȡ���ڵ㸨�����Ƿ�Ϊ��.
        /// </summary>
        public bool IsRootNodeHelperNull
        {
            get { return null == this.rootNodeHelper; }
        }

        /// <summary>
        /// ��ȡ��ǰ�ڵ㸨�����Ƿ�Ϊ��.
        /// </summary>
        public bool IsCurrentNodeHelperNull
        {
            get { return null == this.currentNodeHelper; }
        }

        /// <summary>
        /// ��ȡ��ǰ Xml �ļ���·��.
        /// </summary>
        public string FilePath
        {
            get { return this.filePath; }
        }

#if PARAM
		/// <summary>
		/// ����һ�� XmlDocument ������.
		/// </summary>
		/// <param name="filePath">Xml �ļ�·��, Ĭ�ϲ������κ��ļ�.</param>
		public XmlDocumentHelper ( string filePath = null )
#else
        /// <summary>
        /// ����һ�� XmlDocument ������.
        /// </summary>
        /// <param name="filePath">Xml �ļ�·��.</param>
        public XmlDocumentHelper(string filePath)
#endif
            : base("xml")
        { this.Load(filePath); }

        /// <summary>
        /// ���� Xml �ļ�.
        /// </summary>
        /// <param name="filePath">Xml �ļ�·��.</param>
        public void Load(string filePath)
        {

            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {

                if (null == this.node)
                    this.node = new XmlDocument();

                this.node.Load(filePath);
                this.filePath = filePath;
            }
            catch { }

            if (this.node.ChildNodes.Count == 2)
                this.rootNodeHelper = new XmlNodeHelper<XmlNode>(this.node.ChildNodes[1]);

            this.fileNodeHelper = new XmlNodeHelper<XmlNode>(this.node);
        }

        /// <summary>
        /// �� XmlDocument �е�����ָ���Ľڵ�.
        /// </summary>
        /// <param name="xPath">������·��.</param>
        /// <returns>�Ƿ񵼺��ɹ�.</returns>
        public bool Navigate(string xPath)
        {

            if (null == this.currentNodeHelper)
            {

                if (null == this.rootNodeHelper)
                    return false;

                this.currentNodeHelper = this.rootNodeHelper;
            }

            return this.currentNodeHelper.FetchNodeHelper(ref this.currentNodeHelper, xPath);
        }

#if PARAM
		/// <summary>
		/// ���� Xml �ļ�.
		/// </summary>
		/// <param name="filePath">Xml �ļ�·��, Ĭ��Ϊ����ʱ��·��.</param>
		public void Save ( string filePath = null )
#else
        /// <summary>
        /// ���� Xml �ļ�.
        /// </summary>
        /// <param name="filePath">Xml �ļ�·��.</param>
        public void Save(string filePath)
#endif
        {

            if (null == filePath)
            {

                if (null == this.filePath)
                    return;

                filePath = this.filePath;
            }

            try
            { this.node.Save(filePath); }
            catch { }

        }

        /// <summary>
        /// �������õ�ǰ�ڵ��λ��.
        /// </summary>
        public void Reset()
        { this.currentNodeHelper = null; }

    }

    partial class XmlDocumentHelper
    {
#if !PARAM
        /// <summary>
        /// ����һ�� XmlDocument ������.
        /// </summary>
        public XmlDocumentHelper()
            : this(null)
        { }

        /// <summary>
        /// ���� Xml �ļ�.
        /// </summary>
        public void Save()
        { this.Save(null); }
#endif
    }

}