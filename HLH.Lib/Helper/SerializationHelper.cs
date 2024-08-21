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
        /// ��һ�������л�Ϊһ���ļ�
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void Serialize(object data, string filePath)
        {
            try
            {
                // 1. Open the file
                //���ļ�
                StreamWriter fs = new StreamWriter(filePath, false);

                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Serialize the dataset object using the binary formatter
                    //������������л����ڴ�����
                    formatter.Serialize(streamMemory, data);

                    // 3. Encrypt the binary data
                    //��ת��Ϊ����ֺ����ʽ,�ٽ��м���
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());
                    string cipherData = DataProtection.Encrypt(binaryData, DataProtection.Store.User);

                    // 4. Write the data to a file/
                    //������д���ļ�
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
                    //�ر�����ļ�
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
        /// ��һ�������л�Ϊһ���ļ� 
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
        /// ��һ�������л�Ϊһ���ļ� 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void Serialize(object data, string filePath, bool isDecrypt)
        {
            try
            {
                // 1. Open the file
                //���ļ�
                StreamWriter fs = new StreamWriter(filePath, false);

                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Serialize the dataset object using the binary formatter
                    //������������л����ڴ�����
                    formatter.Serialize(streamMemory, data);

                    // 3. Encrypt the binary data
                    //��ת��Ϊ����ֺ����ʽ,�ٽ��м���
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());
                    if (isDecrypt)
                    {
                        string cipherData = DataProtection.Encrypt(binaryData, DataProtection.Store.User);

                        // 4. Write the data to a file/
                        //������д���ļ�
                        fs.Write(cipherData);
                    }
                    else
                    {
                        string cipherData = binaryData;

                        // 4. Write the data to a file/
                        //������д���ļ�
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
                    //�ر�����ļ�
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
        /// �����л�,���ļ����л�һ������ ���ܣ���ͬ�������޷�ʹ��
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object Deserialize(string filePath)
        {

            object data = new object();

            try
            {
                // 1. Open the file
                //���ļ�
                StreamReader sr = new StreamReader(filePath);

                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Read the binary data, and convert it to a string
                    //���ַ�������ʽ��������
                    string cipherData = sr.ReadToEnd();

                    // 3. Decrypt the binary data
                    //������
                    byte[] binaryData = Convert.FromBase64String(DataProtection.Decrypt(cipherData, DataProtection.Store.User));

                    // 4. Rehydrate the dataset
                    //�����л�Ϊ����
                    streamMemory = new MemoryStream(binaryData);
                    data = formatter.Deserialize(streamMemory);
                }
                catch (Exception ex)
                {
                    // data could not be deserialized
                    //���ܵõ�����,����Ϊ��
                    data = null;
                    //  HLH.Lib.Helper.log4netHelper.error("���ܵõ�����,����Ϊ��", ex);
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
        /// �����л�,���ļ����л�һ������ �����ܣ���ͬ�����Ͽ���ʹ��
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
                //���ļ�
                StreamReader sr = new StreamReader(filePath);

                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. Read the binary data, and convert it to a string
                    //���ַ�������ʽ��������
                    string cipherData = sr.ReadToEnd();

                    // 3. Decrypt the binary data
                    //������
                    if (isDecrypt)
                    {
                        byte[] binaryData = Convert.FromBase64String(DataProtection.Decrypt(cipherData, DataProtection.Store.User));
                        // 4. Rehydrate the dataset
                        //�����л�Ϊ����
                        streamMemory = new MemoryStream(binaryData);
                        data = formatter.Deserialize(streamMemory);
                    }
                    else
                    {
                        byte[] binaryData = Convert.FromBase64String(cipherData);
                        // 4. Rehydrate the dataset
                        //�����л�Ϊ����
                        streamMemory = new MemoryStream(binaryData);
                        data = formatter.Deserialize(streamMemory);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    // data could not be deserialized
                    //���ܵõ�����,����Ϊ��
                    data = null;
                    //  HLH.Lib.Helper.log4netHelper.error("���ܵõ�����,����Ϊ��", ex);
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
        /// �����л�,���ļ����л�һ������ �����ܣ���ͬ�����Ͽ���ʹ��
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
                    //��������
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