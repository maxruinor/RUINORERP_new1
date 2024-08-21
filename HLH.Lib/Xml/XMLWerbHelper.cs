using System;
using System.Xml;

namespace HLH.Lib.Xml
{

    /**/
    /// <summary>
    /// XML������: windows������,Ҫ�þ���·��.web��.Ҫ�޸�
    /// ������XPath���ʽ����ȡ��Ӧ�ڵ�
    /// </summary>
    public class XMLWerbHelper
    {
        #region ����-----------------------------------------------------------


        /// <summary>
        /// xml�ļ�����·������
        /// </summary>
        /// <remarks>xml�ļ�����·������</remarks>
        public enum enumXmlPathType
        {

            /// <summary>
            /// ����·�� (�þ���)
            /// </summary>
            AbsolutePath,

            /// <summary>
            /// ����·��
            /// </summary>
            VirtualPath,


            /// <summary>
            /// xml��ʽ���ַ���
            /// </summary>
            XmlFormatString,
        }

        private string xmlFilePath;
        private enumXmlPathType xmlFilePathType;
        private XmlDocument xmlDoc = new XmlDocument();
        #endregion

        #region ����-----------------------------------------------------------

        /**/
        /// <summary>
        /// �ļ�·��
        /// </summary>
        /// <remarks>�ļ�·��</remarks>
        public string XmlFilePath
        {
            get
            {
                return this.xmlFilePath;
            }
            set
            {
                xmlFilePath = value;

            }
        }
        /**/
        /// <summary>
        /// �ļ�·������
        /// </summary>
        public enumXmlPathType XmlFilePathTyp
        {
            set
            {
                xmlFilePathType = value;
            }
        }
        #endregion

        #region ���캯��-------------------------------------------------------
        /**/
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param >�ļ�·��</param>
        public XMLWerbHelper(string tempXmlFilePath)
        {
            this.xmlFilePathType = enumXmlPathType.VirtualPath;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();

            //xmlDoc.Load( xmlFilePath ) ;
        }

        /**/
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param >�ļ�·��</param>
        /// <param >����</param>
        public XMLWerbHelper(string tempXmlFilePath, enumXmlPathType tempXmlFilePathType)
        {
            this.xmlFilePathType = tempXmlFilePathType;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();
        }

        /**/
        ///<summary>
        ///��ȡXmlDocumentʵ����
        ///</summary>    
        /// <returns>ָ����XML�����ļ���һ��xmldocumentʵ��</returns>
        private XmlDocument GetXmlDocument()
        {
            XmlDocument doc = null;

            if (this.xmlFilePathType == enumXmlPathType.AbsolutePath)
            {
                doc = GetXmlDocumentFromFile(xmlFilePath);
            }
            else if (this.xmlFilePathType == enumXmlPathType.VirtualPath)
            {
                doc = GetXmlDocumentFromFile(xmlFilePath);
            }
            else if (this.xmlFilePathType == enumXmlPathType.XmlFormatString)
            {
                doc = new XmlDocument();
                doc.LoadXml(xmlFilePath);
            }
            return doc;
        }

        private XmlDocument GetXmlDocumentFromFile(string tempXmlFilePath)
        {
            string xmlFileFullPath = tempXmlFilePath;
            xmlDoc.Load(xmlFileFullPath);
            //�����¼�����
            xmlDoc.NodeChanged += new XmlNodeChangedEventHandler(this.nodeUpdateEvent);
            xmlDoc.NodeInserted += new XmlNodeChangedEventHandler(this.nodeInsertEvent);
            xmlDoc.NodeRemoved += new XmlNodeChangedEventHandler(this.nodeDeleteEvent);
            return xmlDoc;
        }

        #endregion

        #region ��ȡ����ָ�����ƵĽڵ�
        /**/
        /// <summary>
        /// ����:
        /// ��ȡ����ָ�����ƵĽڵ�(XmlNodeList)
        /// </summary>
        /// <param >�ڵ�����</param>
        public XmlNodeList GetXmlNodeList(string strNode)
        {
            XmlNodeList strReturn = null;
            try
            {
                //����ָ��·����ȡ�ڵ�
                XmlNodeList xmlNode = xmlDoc.SelectNodes(strNode);
                if (!(xmlNode == null))
                {
                    strReturn = xmlNode;
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region ��ȡָ���ڵ��ָ������ֵ---------------------------------------
        /**/
        /// <summary>
        /// ����:
        /// ��ȡָ���ڵ��ָ������ֵ(Value)
        /// </summary>
        /// <param >�ڵ�����</param>
        /// <param >�˽ڵ������</param>
        /// <returns></returns>
        public string GetXmlNodeAttributeValue(string strNode, string strAttribute)
        {
            string strReturn = "";
            try
            {
                //����ָ��·����ȡ�ڵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (!(xmlNode == null))
                {
                    strReturn = xmlNode.Attributes.GetNamedItem(strAttribute).Value;

                    /**/
                    ////��ȡ�ڵ�����ԣ���ѭ��ȡ����Ҫ������ֵ
                    //XmlAttributeCollection xmlAttr = xmlNode.Attributes;
                    //for (int i = 0; i < xmlAttr.Count; i++)
                    //{
                    //    if (xmlAttr.Item(i).Name == strAttribute)
                    //    {
                    //        strReturn = xmlAttr.Item(i).Value;
                    //        break;
                    //    }
                    //}
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region ��ȡָ���ڵ��ֵ-----------------------------------------------
        /**/
        /// <summary>
        /// ����:
        /// ��ȡָ���ڵ��ֵ(InnerText)
        /// </summary>
        /// <param >�ڵ�����</param>
        /// <returns></returns>
        public string GetXmlNodeValue(string strNode)
        {
            string strReturn = String.Empty;
            try
            {
                //����·����ȡ�ڵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (xmlDoc.GetElementsByTagName(strNode).Count > 0)
                {
                    xmlNode = xmlDoc.GetElementsByTagName(strNode).Item(0);
                }
                if (!(xmlNode == null))
                    strReturn = xmlNode.InnerText;
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region ��ȡָ���ڵ�-----------------------------------------------
        /**/
        /// <summary>
        /// ����:
        /// ��ȡָ���ڵ��ֵ(InnerText)
        /// </summary>
        /// <param >�ڵ�����</param>
        /// <returns></returns>
        public XmlNode GetXmlNode(string strNode)
        {
            XmlNode xmlNode = null;
            try
            {



                //����·����ȡ�ڵ�
                xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (xmlDoc.GetElementsByTagName(strNode).Count > 0)
                {
                    xmlNode = xmlDoc.GetElementsByTagName(strNode).Item(0);
                }

            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return xmlNode;
        }
        #endregion


        #region ���ýڵ�ֵ-----------------------------------------------------
        /**/
        /// <summary>
        /// ����:
        /// ���ýڵ�ֵ(InnerText)
        /// </summary>
        /// <param >�ڵ������</param>
        /// <param >�ڵ�ֵ</param>
        public void SetXmlNodeValue(string xmlNodePath, string xmlNodeValue)
        {
            try
            {
                //��������Ϊ���������Ľڵ���и�ֵ
                XmlNodeList xmlNode = this.xmlDoc.SelectNodes(xmlNodePath);
                if (!(xmlNode == null))
                {
                    foreach (XmlNode xn in xmlNode)
                    {
                        xn.InnerText = xmlNodeValue;
                    }
                }
                /**/
                /**/
                /**/
                /*
            * ����ָ��·����ȡ�ڵ�
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xmlNodePath) ;            
            //���ýڵ�ֵ
            if (!(xmlNode==null))
            xmlNode.InnerText = xmlNodeValue ;*/
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion

        #region ���ýڵ������ֵ-----------------------------------------------

        /// <summary>
        /// ����:
        /// ���ýڵ������ֵ    
        /// </summary>
        /// <param name="xmlNodePath" >�ڵ�����·�� ./ControlConfig </param>
        /// <param   name="xmlNodeAttribute" >��������</param>
        /// <param   name="xmlNodeAttributeValue" >����ֵ</param>
        public void SetXmlNodeAttributeValue(string xmlNodePath, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                //��������Ϊ���������Ľڵ�����Ը�ֵ
                XmlNodeList xmlNode = this.xmlDoc.SelectNodes(xmlNodePath);
                if (!(xmlNode == null))
                {
                    foreach (XmlNode xn in xmlNode)
                    {
                        XmlAttributeCollection xmlAttr = xn.Attributes;
                        for (int i = 0; i < xmlAttr.Count; i++)
                        {
                            if (xmlAttr.Item(i).Name == xmlNodeAttribute)
                            {
                                xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                                break;
                            }
                        }
                    }
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /// <summary>
        /// ����:
        /// ���ýڵ������ֵ    
        /// </summary>
        /// <param name="xmlNodePath" >�ڵ�����·�� ./ControlConfig </param>
        /// <param   name="xmlNodeAttribute" >��������</param>
        /// <param   name="xmlNodeAttributeValue" >����ֵ</param>
        public void SetXmlNodeAttributeValue(string xmlNodePath, string NodeInnerText, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                //��������Ϊ���������Ľڵ�����Ը�ֵ
                XmlNodeList xmlNode = this.xmlDoc.SelectNodes(xmlNodePath);
                if (!(xmlNode == null))
                {
                    foreach (XmlNode xn in xmlNode)
                    {
                        if (xn.InnerText == NodeInnerText)
                        {
                            XmlAttributeCollection xmlAttr = xn.Attributes;
                            for (int i = 0; i < xmlAttr.Count; i++)
                            {
                                if (xmlAttr.Item(i).Name == xmlNodeAttribute)
                                {
                                    xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                                    break;
                                }
                            }
                            break;
                        }

                    }
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }


        #endregion

        #region ���-----------------------------------------------------------
        /**/
        /// <summary>
        /// ��ȡXML�ļ��ĸ�Ԫ��
        /// </summary>
        public XmlNode GetXmlRoot()
        {
            return xmlDoc.DocumentElement;
        }

        /**/
        /// <summary>
        /// �ڸ��ڵ�����Ӹ��ڵ�
        /// </summary>
        public void AddParentNode(string parentNode)
        {
            try
            {
                XmlNode root = GetXmlRoot();
                XmlNode parentXmlNode = xmlDoc.CreateElement(parentNode);
                root.AppendChild(parentXmlNode);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ��һ���Ѿ����ڵĸ��ڵ��в���һ���ӽڵ�,�������ӽڵ�.
        /// </summary>
        /// <param >���ڵ�</param>
        /// <param >�ֽڵ�����</param>
        public XmlNode AddChildNode(string parentNodePath, string childnodename)
        {
            XmlNode childXmlNode = null;
            try
            {
                XmlNode parentXmlNode = xmlDoc.SelectSingleNode(parentNodePath);
                if (!((parentXmlNode) == null))//����˽ڵ����
                {
                    childXmlNode = xmlDoc.CreateElement(childnodename);
                    parentXmlNode.AppendChild(childXmlNode);
                }
                else
                {//��������ھͷŸ��ڵ����
                    this.GetXmlRoot().AppendChild(childXmlNode);
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return childXmlNode;
        }
        /**/
        /// <summary>
        /// ��һ���Ѿ����ڵĸ��ڵ��в���һ���ӽڵ�,�����һ������
        /// </summary>
        public void AddChildNode(string parentNodePath, string childnodename, string NodeAttribute, string
NodeAttributeValue)
        {
            try
            {
                XmlNode parentXmlNode = xmlDoc.SelectSingleNode(parentNodePath);
                XmlNode childXmlNode = null;
                if (!((parentXmlNode) == null))//����˽ڵ����
                {
                    childXmlNode = xmlDoc.CreateElement(childnodename);

                    //�������
                    XmlAttribute nodeAttribute = this.xmlDoc.CreateAttribute(NodeAttribute);
                    nodeAttribute.Value = NodeAttributeValue;
                    childXmlNode.Attributes.Append(nodeAttribute);

                    parentXmlNode.AppendChild(childXmlNode);
                }
                else
                {//��������ھͷŸ��ڵ����
                    this.GetXmlRoot().AppendChild(childXmlNode);
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ��һ���ڵ��������,ֵΪ��
        /// </summary>
        /// <param >�ڵ�·��</param>
        /// <param >������</param>
        public void AddAttribute(string NodePath, string NodeAttribute)
        {
            privateAddAttribute(NodePath, NodeAttribute, "");
        }
        /**/
        /// <summary>
        /// ��һ���ڵ��������,����ֵ***
        /// </summary>
        public void AddAttribute(XmlNode childXmlNode, string NodeAttribute, string NodeAttributeValue)
        {
            XmlAttribute nodeAttribute = this.xmlDoc.CreateAttribute(NodeAttribute);
            nodeAttribute.Value = NodeAttributeValue;
            childXmlNode.Attributes.Append(nodeAttribute);
        }

        /**/
        /// <summary>
        /// ��һ���ڵ��������
        /// </summary>
        /// <param >�ڵ�·��</param>
        /// <param >������</param>
        /// <param >����ֵ</param>
        private void privateAddAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            try
            {
                XmlNode nodePath = xmlDoc.SelectSingleNode(NodePath);
                if (!(nodePath == null))
                {
                    XmlAttribute nodeAttribute = this.xmlDoc.CreateAttribute(NodeAttribute);
                    nodeAttribute.Value = NodeAttributeValue;
                    nodePath.Attributes.Append(nodeAttribute);
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        /**/
        /// <summary>
        ///  ��һ���ڵ��������,����ֵ
        /// </summary>
        /// <param >�ڵ�</param>
        /// <param >������</param>
        /// <param >����ֵ</param>
        public void AddAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            privateAddAttribute(NodePath, NodeAttribute, NodeAttributeValue);
        }
        #endregion

        #region ɾ��-----------------------------------------------------------
        /**/
        /// <summary>
        /// ɾ���ڵ��һ������
        /// </summary>
        /// <param >�ڵ����ڵ�xpath���ʽ</param>
        /// <param >������</param>
        public void DeleteAttribute(string NodePath, string NodeAttribute)
        {
            XmlNodeList nodePath = this.xmlDoc.SelectNodes(NodePath);
            if (!(nodePath == null))
            {
                foreach (XmlNode tempxn in nodePath)
                {
                    XmlAttributeCollection xmlAttr = tempxn.Attributes;
                    for (int i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name == NodeAttribute)
                        {
                            tempxn.Attributes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        /**/
        /// <summary>
        /// ɾ���ڵ��һ������,��������ֵ���ڸ�����ֵʱ
        /// </summary>
        /// <param >�ڵ����ڵ�xpath���ʽ</param>
        /// <param >����</param>
        /// <param >ֵ</param>
        public void DeleteAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            XmlNodeList nodePath = this.xmlDoc.SelectNodes(NodePath);
            if (!(nodePath == null))
            {
                foreach (XmlNode tempxn in nodePath)
                {
                    XmlAttributeCollection xmlAttr = tempxn.Attributes;
                    for (int i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name == NodeAttribute && xmlAttr.Item(i).Value == NodeAttributeValue)
                        {
                            tempxn.Attributes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        /**/
        /// <summary>
        /// ɾ���ڵ�
        /// </summary>
        /// <param ></param>
        /// <remarks></remarks>
        public void DeleteXmlNode(string tempXmlNode)
        {
            XmlNodeList nodePath = this.xmlDoc.SelectNodes(tempXmlNode);
            if (!(nodePath == null))
            {
                foreach (XmlNode xn in nodePath)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
            }
        }

        #endregion

        #region XML�ĵ��¼�----------------------------------------------------
        /**/
        /// <summary>
        /// �ڵ�����¼�
        /// </summary>
        /// <param ></param>
        /// <param ></param>
        private void nodeInsertEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        /**/
        /// <summary>
        /// �ڵ�ɾ���¼�
        /// </summary>
        /// <param ></param>
        /// <param ></param>
        private void nodeDeleteEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        /**/
        /// <summary>
        /// �ڵ�����¼�
        /// </summary>
        /// <param ></param>
        /// <param ></param>
        private void nodeUpdateEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        #endregion

        #region ����XML�ļ�----------------------------------------------------
        /**/
        /// <summary>
        /// ����: 
        /// ����XML�ļ�
        /// </summary>
        public void SaveXmlDocument()
        {
            try
            {
                //�������õĽ��
                if (this.xmlFilePathType == enumXmlPathType.AbsolutePath)
                {
                    Savexml(xmlFilePath);
                }
                else if (this.xmlFilePathType == enumXmlPathType.VirtualPath)
                {
                    // Savexml(HttpContext.Current.Server.MapPath(xmlFilePath));
                    //Savexml(System.Windows.Forms.Application.StartupPath(xmlFilePath));
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ����: 
        /// ����XML�ļ�    
        /// </summary>
        public void SaveXmlDocument(string tempXMLFilePath)
        {
            try
            {
                //�������õĽ��
                Savexml(tempXMLFilePath);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param ></param>
        private void Savexml(string filepath)
        {
            xmlDoc.Save(filepath);
        }


        /// <summary>
        /// ���½ڵ���� 
        /// </summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="content"></param>
        public void Replace(string nodeName, string content)
        {

            XmlNodeList list = xmlDoc.GetElementsByTagName(nodeName);//SelectNodes("book");

            foreach (XmlNode xn in list)
            {
                XmlElement xe = (XmlElement)xn;

                if (xe.Name == nodeName)
                {
                    xe.InnerText = content;

                }

            }
        }

        /// <summary>
        /// ���½ڵ�����
        /// </summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="content"></param>
        public void ReplaceNodeName(XmlNodeList list, string OldNode, string NewNode, string NewNodeContent)
        {


            foreach (XmlNode xn in list)
            {
                XmlElement xe = (XmlElement)xn;

                if (xe.Name == OldNode)
                {
                    this.DeleteXmlNode(OldNode);
                    this.AddChildNode(xe.ParentNode.Name, NewNode);
                    break;
                }

            }
        }


        public void AddNode(XmlNode xn)
        {
            this.xmlDoc.AppendChild(xn);
        }


        #endregion
    }
}



