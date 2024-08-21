using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace HLH.Lib.Xml
{
    public class Xlst2Xml
    {

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="stream">�ļ���</param>
        /// <param name="filePath">
        /// �ļ���URC���·����:directory\filename.gif
        /// ע��ǰ�治Ҫ�ӡ�\��
        /// </param>
        /// <param name="fileStoreType">
        /// �ļ���������
        /// ��ʹ��ö��FileStoreType�����ͣ��磺FileStoreType.Common.ToString();
        /// ע����������������͵����������ҳ���Ӧ���ļ�·��
        /// </param>
        public static void SaveFile(Stream stream, string filePath)
        {

            stream.Seek(0, SeekOrigin.Begin);
            string savePath = filePath;

            long dataToRead;
            byte[] buffer = new byte[10240];
            int length;


            try
            {

                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 10240))
                {
                    dataToRead = stream.Length;
                    while (dataToRead > 0)
                    {
                        length = stream.Read(buffer, 0, 10240);
                        fs.Write(buffer, 0, length);
                        buffer = new Byte[10240];
                        dataToRead = dataToRead - length;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {

            }
        }


        /// <summary>
        /// ʹ��xsltFilePath����ʽ��ת��Դ�ĵ�srcXml�������׳�TransformException�쳣��
        /// </summary>
        /// <param name="srcXml">ת��������xml�ĵ������ַ�����ʽ�ṩ��</param>
        /// <param name="xsltFilePath">��ʽ���ļ���·��</param>
        /// <param name="bXmlInstruction">����ĵ����Ƿ���Ҫ����<?xml version="1.0" encoding="UTF-8"?>����</param>
        /// <exception cref="TransformException"></exception>
        /// <returns>����ת���������ĵ��ַ�������UTF-8����</returns>
        public static string Transfer(string srcXml, string xsltFilePath)
        {


            // check parameters
            if (srcXml == null || xsltFilePath == null)
            {
                throw new Exception("����������Ϸ�!");
            }

            try
            {
                // load the srcXml
                StringReader reader = new StringReader(srcXml);
                XPathDocument srcDoc = new XPathDocument(reader);

                // load the xslt
                //---------------���ļ�����������Modified by cxq on 2010-05-24----------------------------
                //XsltSettings setting = new XsltSettings(false, true);
                //XslCompiledTransform xslTrans = new XslCompiledTransform();
                //xslTrans.Load(xsltFilePath, setting, new XmlUrlResolver());
                XslCompiledTransform xslTrans = GetTransformer(xsltFilePath);



                // create the writer
                MemoryStream ms = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(ms, new System.Text.UTF8Encoding(false));
                writer.Formatting = Formatting.None;
                //writer.Formatting = Formatting.Indented;
                //writer.Indentation = 1;
                //writer.IndentChar = '\t';

                writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");


                // transform the xslt
                xslTrans.Transform(srcDoc, null, writer);

                // return the result string
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// ʹ��xsltFilePath����ʽ��ת��Դ�ĵ�srcXml�������׳�TransformException�쳣��
        /// </summary>
        /// <param name="srcXml">ת��������xml�ĵ������ַ�����ʽ�ṩ��</param>
        /// <param name="xsltFilePath">��ʽ���ļ���·��</param>
        /// <param name="bXmlInstruction">����ĵ����Ƿ���Ҫ����<?xml version="1.0" encoding="UTF-8"?>����</param>
        /// <exception cref="TransformException"></exception>
        /// <returns>����ת���������ĵ��ַ�������UTF-8����</returns>
        public static string Transfer(string srcXml, string xsltFilePath, bool bXmlInstruction)
        {


            // check parameters
            if (srcXml == null || xsltFilePath == null)
            {
                throw new Exception("����������Ϸ�!");
            }

            try
            {
                // load the srcXml
                StringReader reader = new StringReader(srcXml);
                XPathDocument srcDoc = new XPathDocument(reader);

                // load the xslt
                //---------------���ļ�����������Modified by cxq on 2010-05-24----------------------------
                //XsltSettings setting = new XsltSettings(false, true);
                //XslCompiledTransform xslTrans = new XslCompiledTransform();
                //xslTrans.Load(xsltFilePath, setting, new XmlUrlResolver());
                XslCompiledTransform xslTrans = GetTransformer(xsltFilePath);

                // create the writer
                MemoryStream ms = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(ms, new System.Text.UTF8Encoding(false));
                writer.Formatting = Formatting.None;
                //writer.Formatting = Formatting.Indented;
                //writer.Indentation = 1;
                //writer.IndentChar = '\t';
                if (bXmlInstruction)
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
                }
                // Create the XsltArgumentList.
                // XsltArgumentList argList = new XsltArgumentList();


                //argList.AddParam("mypara", "", "888999");



                // transform the xslt
                xslTrans.Transform(srcDoc, null, writer);

                // return the result string
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("ת��ʧ��, " + ex.Message, ex);
            }
        }



        /// <summary>
        /// ��ȡxslt�������ļ������һ�������
        /// </summary>
        /// <param name="xsltFilePath">�ļ�·��</param>
        /// <returns>XslCompiledTransform</returns>
        private static XslCompiledTransform GetTransformer(string xsltFilePath)
        {
            XslCompiledTransform xslTrans;
            xsltFilePath = xsltFilePath.Trim().ToLower();

            Hashtable xsltSet = xsltSet = new Hashtable();

            if (xsltSet.Contains(xsltFilePath) == false)
            {
                XsltSettings setting = new XsltSettings(false, true);
                xslTrans = new XslCompiledTransform(true);
                xslTrans.Load(xsltFilePath, setting, new XmlUrlResolver());
                xsltSet.Add(xsltFilePath, xslTrans);
            }
            else
            {
                xslTrans = (XslCompiledTransform)xsltSet[xsltFilePath];
            }



            return xslTrans;
        }

    }
}
