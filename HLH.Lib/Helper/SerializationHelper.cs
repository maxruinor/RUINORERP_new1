using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace HLH.Lib.Helper
{
    // The SerializationHelper class contains Serialize / Deserialize methods for 
    // storing data on the client machine securely.  Data objects are first serialized
    // to a binary stream, then encrypted, then written to disk (typically in the
    // UserApplicationDataPath location).
    public class SerializationHelper
    {
        /// <summary>
        /// 将一对象序列化为一个文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void Serialize(object data, string filePath)
        {
            try
            {
                // 1. Open the file
                //打开文件
                StreamWriter fs = new StreamWriter(filePath, false);

                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Serialize the dataset object using the binary formatter
                    //将这个对象序列化到内存流中
                    formatter.Serialize(streamMemory, data);

                    // 3. Encrypt the binary data
                    //先转化为字体趾的形式,再进行加密
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());
                    string cipherData = DataProtection.Encrypt(binaryData, DataProtection.Store.User);

                    // 4. Write the data to a file/
                    //将数据写入文件
                    fs.Write(cipherData);
                }
                catch (Exception e)
                {
                    throw e;
                    // HLH.Lib.Helper.log4netHelper.error("Serialize", e);
                    //System.Windows.Forms.MessageBox.Show(e.Message);
                }

                finally
                {
                    // 5. Close the file
                    //关闭这个文件
                    fs.Flush();
                    fs.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
                // Eat exception if this fails.
                // HLH.Lib.Helper.log4netHelper.error("Serialize", ex);
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 将一对象序列化为一个文件 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void SerializeXML(object data, string filePath, bool isDecrypt)
        {
            try
            {
                System.IO.FileInfo info = new FileInfo(filePath);
                HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.SaveFile(info.Name, HLH.Lib.Xml.XmlUtil.Serializer(data.GetType(), data), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw ex;
                // Eat exception if this fails.
                //HLH.Lib.Helper.log4netHelper.error("Serialize", ex);
                //  System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 将一对象序列化为一个文件 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void Serialize(object data, string filePath, bool isDecrypt)
        {
            try
            {
                // 1. Open the file
                //打开文件
                StreamWriter fs = new StreamWriter(filePath, false);

                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Serialize the dataset object using the binary formatter
                    //将这个对象序列化到内存流中
                    formatter.Serialize(streamMemory, data);

                    // 3. Encrypt the binary data
                    //先转化为字体趾的形式,再进行加密
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());
                    if (isDecrypt)
                    {
                        string cipherData = DataProtection.Encrypt(binaryData, DataProtection.Store.User);

                        // 4. Write the data to a file/
                        //将数据写入文件
                        fs.Write(cipherData);
                    }
                    else
                    {
                        string cipherData = binaryData;

                        // 4. Write the data to a file/
                        //将数据写入文件
                        fs.Write(cipherData);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    // HLH.Lib.Helper.log4netHelper.error("Serialize", e);
                    // System.Windows.Forms.MessageBox.Show(e.Message);
                }

                finally
                {
                    // 5. Close the file
                    //关闭这个文件
                    fs.Flush();
                    fs.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
                // Eat exception if this fails.
                //HLH.Lib.Helper.log4netHelper.error("Serialize", ex);
                //  System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }




        /// <summary>
        /// 反序列化,从文件序列化一个对象 加密，不同电脑上无法使用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object Deserialize(string filePath)
        {

            object data = new object();

            try
            {
                // 1. Open the file
                //打开文件
                StreamReader sr = new StreamReader(filePath);

                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Read the binary data, and convert it to a string
                    //以字符串的形式读出数据
                    string cipherData = sr.ReadToEnd();

                    // 3. Decrypt the binary data
                    //并解密
                    byte[] binaryData = Convert.FromBase64String(DataProtection.Decrypt(cipherData, DataProtection.Store.User));

                    // 4. Rehydrate the dataset
                    //反序列化为对象
                    streamMemory = new MemoryStream(binaryData);
                    data = formatter.Deserialize(streamMemory);
                }
                catch (Exception ex)
                {
                    // data could not be deserialized
                    //不能得到数据,设置为空
                    data = null;
                    //  HLH.Lib.Helper.log4netHelper.error("不能得到数据,设置为空", ex);
                }
                finally
                {
                    // 5. Close the reader
                    sr.Close();
                }
            }
            catch
            {
                // file doesn't exist

                data = null;
            }

            return data;
        }

        public static byte[] SerializeDataEntity(object dataEntity)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, dataEntity);
                return memoryStream.ToArray();
            }
        }

        public static object DeserializeDataEntity(byte[] serializedData)
        {
            using (MemoryStream memoryStream = new MemoryStream(serializedData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (object)formatter.Deserialize(memoryStream);
            }
        }


        /// <summary>
        /// 反序列化,从文件序列化一个对象 不加密，不同电脑上可以使用
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isDecrypt"></param>
        /// <returns></returns>
        public static object Deserialize(string filePath, bool isDecrypt)
        {

            object data = new object();

            try
            {
                // 1. Open the file
                //打开文件
                StreamReader sr = new StreamReader(filePath);

                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Read the binary data, and convert it to a string
                    //以字符串的形式读出数据
                    string cipherData = sr.ReadToEnd();

                    // 3. Decrypt the binary data
                    //并解密
                    if (isDecrypt)
                    {
                        byte[] binaryData = Convert.FromBase64String(DataProtection.Decrypt(cipherData, DataProtection.Store.User));
                        // 4. Rehydrate the dataset
                        //反序列化为对象
                        streamMemory = new MemoryStream(binaryData);
                        data = formatter.Deserialize(streamMemory);
                    }
                    else
                    {
                        byte[] binaryData = Convert.FromBase64String(cipherData);
                        // 4. Rehydrate the dataset
                        //反序列化为对象
                        streamMemory = new MemoryStream(binaryData);
                        data = formatter.Deserialize(streamMemory);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    // data could not be deserialized
                    //不能得到数据,设置为空
                    data = null;
                    //  HLH.Lib.Helper.log4netHelper.error("不能得到数据,设置为空", ex);
                }
                finally
                {
                    // 5. Close the reader
                    sr.Close();
                }
            }
            catch
            {
                // file doesn't exist

                data = null;
            }

            return data;
        }
        /// <summary>
        /// 反序列化,从文件序列化一个对象 不加密，不同电脑上可以使用
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isDecrypt"></param>
        /// <returns></returns>
        public static object DeserializeXml(Type type, string filePath)
        {
            object obj = null;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string xml = HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.ReadFile(filePath, System.Text.Encoding.UTF8);
                    //载入配置
                    obj = HLH.Lib.Xml.XmlUtil.Deserialize(type, xml);
                }
            }
            catch (Exception ex)
            {
                // file doesn't exist
                obj = null;
            }

            return obj;
        }

    }
}