using HLH.Lib.Security;
using Newtonsoft.Json;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{
    public static class DataImportExportManager
    {
        public static string ExportDataAsync(List<tb_Prod> tb_ProdDetails, string fileName)
        {
            // 序列化对象到JSON字符串
            string jsonString = JsonConvert.SerializeObject(tb_ProdDetails);
            string encryptedString = AESHelper.Encrypt(jsonString, "1234567890123456");
            HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.SaveFile(fileName, encryptedString, Encoding.UTF8);
            // 返回加密的字符串
            return encryptedString;
        }
        public static List<tb_Prod> ImportDataAsync(string fileName)
        {
            // 读取文件
            string encryptedString = HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.ReadFile(fileName, Encoding.UTF8);
            // 解密
            string jsonString = AESHelper.Decrypt(encryptedString, "1234567890123456");
            // 反序列化JSON字符串回对象列表
            List<tb_Prod> list = JsonConvert.DeserializeObject<List<tb_Prod>>(jsonString);
            return list;
        }

    }




    /*
      // 假设tb_ProdDetails是查询出来的数据
        List<tb_ProdDetail> tb_ProdDetails = await GetTb_ProdDetails();

        // 导出数据
        string encryptedData = await DataExporter.ExportDataAsync(tb_ProdDetails);
        File.WriteAllText("data.json", encryptedData);

        // 导入数据
        string encryptedContent = File.ReadAllText("data.json");
        List<tb_ProdDetail> importedData = await DataImporter.ImportDataAsync(encryptedContent);

     
     */
}
