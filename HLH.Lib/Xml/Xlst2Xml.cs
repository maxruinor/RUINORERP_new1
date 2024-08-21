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
        /// 保存文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="filePath">
        /// 文件的URC相对路径如:directory\filename.gif
        /// 注：前面不要加“\”
        /// </param>
        /// <param name="fileStoreType">
        /// 文件储存类型
        /// 请使用枚举FileStoreType的类型，如：FileStoreType.Common.ToString();
        /// 注：程序会根据这个类型到参数表中找出对应的文件路径
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
        /// 使用xsltFilePath的样式表转换源文档srcXml；方法抛出TransformException异常。
        /// </summary>
        /// <param name="srcXml">转换的输入xml文档，以字符串方式提供；</param>
        /// <param name="xsltFilePath">样式表文件的路径</param>
        /// <param name="bXmlInstruction">输出文档中是否需要包括<?xml version="1.0" encoding="UTF-8"?>声明</param>
        /// <exception cref="TransformException"></exception>
        /// <returns>返回转换后的输出文档字符串，以UTF-8编码</returns>
        public static string Transfer(string srcXml, string xsltFilePath)
        {


            // check parameters
            if (srcXml == null || xsltFilePath == null)
            {
                throw new Exception("输入参数不合法!");
            }

            try
            {
                // load the srcXml
                StringReader reader = new StringReader(srcXml);
                XPathDocument srcDoc = new XPathDocument(reader);

                // load the xslt
                //---------------将文件缓存起来，Modified by cxq on 2010-05-24----------------------------
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
        /// 使用xsltFilePath的样式表转换源文档srcXml；方法抛出TransformException异常。
        /// </summary>
        /// <param name="srcXml">转换的输入xml文档，以字符串方式提供；</param>
        /// <param name="xsltFilePath">样式表文件的路径</param>
        /// <param name="bXmlInstruction">输出文档中是否需要包括<?xml version="1.0" encoding="UTF-8"?>声明</param>
        /// <exception cref="TransformException"></exception>
        /// <returns>返回转换后的输出文档字符串，以UTF-8编码</returns>
        public static string Transfer(string srcXml, string xsltFilePath, bool bXmlInstruction)
        {


            // check parameters
            if (srcXml == null || xsltFilePath == null)
            {
                throw new Exception("输入参数不合法!");
            }

            try
            {
                // load the srcXml
                StringReader reader = new StringReader(srcXml);
                XPathDocument srcDoc = new XPathDocument(reader);

                // load the xslt
                //---------------将文件缓存起来，Modified by cxq on 2010-05-24----------------------------
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
                throw new Exception("转换失败, " + ex.Message, ex);
            }
        }



        /// <summary>
        /// 获取xslt的配置文件，并且缓存起来
        /// </summary>
        /// <param name="xsltFilePath">文件路径</param>
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
