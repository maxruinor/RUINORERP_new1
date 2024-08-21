using System;
using System.Xml;

namespace HLH.Lib.Xml
{
    /// <summary>
    /// XMLHelper XML�ĵ�����������
    /// </summary>
    public class XMLHelper
    {
        public XMLHelper()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }


        #region XML�ĵ��ڵ��ѯ�Ͷ�ȡ
        /// <summary>
        /// ѡ��ƥ��XPath���ʽ�ĵ�һ���ڵ�XmlNode.
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���")</param>
        /// <returns>����XmlNode</returns>
        public static XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex; //������Զ������Լ����쳣����
            }
        }

        /// <summary>
        /// ѡ��ƥ��XPath���ʽ�Ľڵ��б�XmlNodeList.
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���")</param>
        /// <returns>����XmlNodeList</returns>
        public static XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                return xmlNodeList;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex; //������Զ������Լ����쳣����
            }
        }

        /// <summary>
        /// ѡ��ƥ��XPath���ʽ�ĵ�һ���ڵ��ƥ��xmlAttributeName������XmlAttribute.
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <param name="xmlAttributeName">Ҫƥ��xmlAttributeName����������</param>
        /// <returns>����xmlAttributeName</returns>
        public static XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
        {
            string content = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            XmlAttribute xmlAttribute = null;
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    if (xmlNode.Attributes.Count > 0)
                    {
                        xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return xmlAttribute;
        }
        #endregion

        #region XML�ĵ������ͽڵ�����Ե���ӡ��޸�
        /// <summary>
        /// ����һ��XML�ĵ�
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="rootNodeName">XML�ĵ����ڵ�����(��ָ��һ�����ڵ�����)</param>
        /// <param name="version">XML�ĵ��汾��(����Ϊ:"1.0")</param>
        /// <param name="encoding">XML�ĵ����뷽ʽ</param>
        /// <param name="standalone">��ֵ������"yes"��"no",���Ϊnull,Save��������XML������д����������</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool CreateXmlDocument(string xmlFileName, string rootNodeName, string version, string encoding, string standalone)
        {
            bool isSuccess = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
                XmlNode root = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(xmlDeclaration);
                xmlDoc.AppendChild(root);
                xmlDoc.Save(xmlFileName);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }

        /// <summary>
        /// ����ƥ��XPath���ʽ�ĵ�һ���ڵ������������ӽڵ�(����˽ڵ��Ѵ�����׷��һ���µ�ͬ���ڵ�
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <param name="xmlNodeName">Ҫƥ��xmlNodeName�Ľڵ�����</param>
        /// <param name="innerText">�ڵ��ı�ֵ</param>
        /// <param name="xmlAttributeName">Ҫƥ��xmlAttributeName����������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool CreateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //�治���ڴ˽ڵ㶼����
                    XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                    subElement.InnerXml = innerText;

                    //������Ժ�ֵ��������Ϊ�����ڴ��½ڵ�����������
                    if (!string.IsNullOrEmpty(xmlAttributeName) && !string.IsNullOrEmpty(value))
                    {
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        subElement.Attributes.Append(xmlAttribute);
                    }

                    xmlNode.AppendChild(subElement);
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }

        /// <summary>
        /// ����ƥ��XPath���ʽ�ĵ�һ���ڵ�����������������ӽڵ�(����ڵ���������,�������򴴽�)
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <param name="xmlNodeName">Ҫƥ��xmlNodeName�Ľڵ�����</param>
        /// <param name="innerText">�ڵ��ı�ֵ</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool CreateOrUpdateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText)
        {
            bool isSuccess = false;
            bool isExistsNode = false;//��ʶ�ڵ��Ƿ����
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //����xpath�ڵ��µ������ӽڵ�
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        if (node.Name.ToLower() == xmlNodeName.ToLower())
                        {
                            //���ڴ˽ڵ������
                            node.InnerXml = innerText;
                            isExistsNode = true;
                            break;
                        }
                    }
                    if (!isExistsNode)
                    {
                        //�����ڴ˽ڵ��򴴽�
                        XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                        subElement.InnerXml = innerText;
                        xmlNode.AppendChild(subElement);
                    }
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }

        /// <summary>
        /// ����ƥ��XPath���ʽ�ĵ�һ���ڵ��������������������(������Դ��������,�������򴴽�)
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <param name="xmlAttributeName">Ҫƥ��xmlAttributeName����������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool CreateOrUpdateXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;//��ʶ�����Ƿ����
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //����xpath�ڵ��е���������
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //�ڵ��д��ڴ����������
                            attribute.Value = value;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (!isExistsAttribute)
                    {
                        //�ڵ��в����ڴ������򴴽�
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        xmlNode.Attributes.Append(xmlAttribute);
                    }
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }
        #endregion


        #region XML�ĵ��ڵ�����Ե�ɾ��
        /// <summary>
        /// ɾ��ƥ��XPath���ʽ�ĵ�һ���ڵ�(�ڵ��е���Ԫ��ͬʱ�ᱻɾ��)
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)
        {
            bool isSuccess = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //ɾ���ڵ�
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }

        /// <summary>
        /// ɾ��ƥ��XPath���ʽ�ĵ�һ���ڵ��е�ƥ�����xmlAttributeName������
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <param name="xmlAttributeName">Ҫɾ����xmlAttributeName����������</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                XmlAttribute xmlAttribute = null;
                if (xmlNode != null)
                {
                    //����xpath�ڵ��е���������
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //�ڵ��д��ڴ�����
                            xmlAttribute = attribute;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (isExistsAttribute)
                    {
                        //ɾ���ڵ��е�����
                        xmlNode.Attributes.Remove(xmlAttribute);
                    }
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }

        /// <summary>
        /// ɾ��ƥ��XPath���ʽ�ĵ�һ���ڵ��е���������
        /// </summary>
        /// <param name="xmlFileName">XML�ĵ���ȫ�ļ���(��������·��)</param>
        /// <param name="xpath">Ҫƥ���XPath���ʽ(����:"//�ڵ���//�ӽڵ���</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public static bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)
        {
            bool isSuccess = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //����XML�ĵ�
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //����xpath�ڵ��е���������
                    xmlNode.Attributes.RemoveAll();
                }
                xmlDoc.Save(xmlFileName); //���浽XML�ĵ�
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //������Զ������Լ����쳣����
            }
            return isSuccess;
        }
        #endregion

    }
}


/*

����XML�ĵ�


            //����XML�ĵ����ڵ���
            string rootNodeName = "books";

            //����XML�ĵ������ļ�������������·����
            string xmlFileName = Application.StartupPath + @"\book.xml";

            XMLHelper.CreateXmlDocument(xmlFileName, rootNodeName, "1.0", "utf-8", null);
            MessageBox.Show("XML�ĵ������ɹ�:" + xmlFileName);


��XML�ĵ������һ���½ڵ�


            string xmlFileName = Application.StartupPath + @"\book.xml";
            string xpath = "/books";  //�����½ڵ�ĸ��ڵ�·��
            string nodename = "book";��//�����½ڵ�����,�ڸ��ڵ�������
            string nodetext = "�����½ڵ��е��ı�ֵ";

            bool isSuccess = XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath, nodename, nodetext);
            MessageBox.Show("XML�ڵ���ӻ���³ɹ�:" + isSuccess.ToString());



��XML�ĵ��е��ӽڵ����������޸ģ�����������޸ģ�һ���ӽڵ�,����name,author,date�ڵ�ȣ�


            string xmlFileName = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book";  //�������ӽڵ�ĸ��ڵ�·��
            string nodename = "name";��//�������ӽڵ�����,�ڸ��ڵ�������
            string nodetext = "�ҵ������ҵ���";

            bool isSuccess = XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath, nodename, nodetext);
            MessageBox.Show("XML�ڵ���ӻ���³ɹ�:" + isSuccess.ToString());

��XML�ĵ��е��ӽڵ����������޸ģ�����������޸ģ�һ���ӽڵ�����,����id,ISDN���Եȣ�


            string xmlFileName = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book"; //Ҫ�������ԵĽڵ�
            string attributeName = "id";��//����������,ISDN��Ҳ����ô������
            string attributeValue = "1";��//������ֵ

            bool isSuccess = XMLHelper.CreateOrUpdateXmlAttributeByXPath(xmlFileName, xpath, attributeName, attributeValue);
            MessageBox.Show("XML������ӻ���³ɹ�:" + isSuccess.ToString());

ɾ��XML�ĵ��е��ӽڵ㣺


            string xmlFileName = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book[@id='1']"; //Ҫɾ����idΪ1��book�ӽڵ�

            bool isSuccess = XMLHelper.DeleteXmlNodeByXPath(xmlFileName, xpath);
            MessageBox.Show("XML�ڵ�ɾ���ɹ�:" + isSuccess.ToString());

ɾ��XML�ĵ����ӽڵ�����ԣ�


            string xmlFileName = Application.StartupPath + @"\book.xml";
            //ɾ��idΪ2��book�ӽڵ��е�ISDN����
            string xpath = "/books/book[@id='2']";
            string attributeName = "ISDN";

            bool isSuccess = XMLHelper.DeleteXmlAttributeByXPath(xmlFileName, xpath, attributeName);
            MessageBox.Show("XML����ɾ���ɹ�:" + isSuccess.ToString());

��ȡXML�ĵ��е������ӽڵ㣺


            string xmlFileName = Application.StartupPath + @"\book.xml";
            //Ҫ����idΪ1��book�ӽڵ�
            string xpath = "/books/book[@id='1']";

            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
            string strAllNode = "";
            //�����ڵ������е��ӽڵ�
            foreach (XmlNode node in nodeList)
            {
                strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
            }

            MessageBox.Show("XML�ڵ��������ӽڵ���:" + strAllNode);
*/
