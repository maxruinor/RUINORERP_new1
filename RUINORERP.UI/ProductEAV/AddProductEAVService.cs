using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV
{

    /// <summary>
    /// 从各平台，或其它网站 或采集 等批量添加产品数据
    /// </summary>
    public class AddProductEAVService
    {
        /// <summary>
        /// 上传产品
        /// </summary>
        /// <returns></returns>
        public static bool UploadImages(internalproductEntity entity)
        {
            bool rs = false;
            //MultiUser.Instance.ProductLibraryImagesPath

            HLH.Lib.Helper.FTPHelper.FTPClient client = new HLH.Lib.Helper.FTPHelper.FTPClient();
            //client.RemoteHost = "ftp.minigadgetshop.com";
            client.RemoteHost = "173.255.213.19";
            client.RemotePass = "max2017";
            client.RemoteUser = "ftpruinor";
            client.RemotePort = 21;
            client.RemotePath = "/var/www/myconnshop/html/images";
            string imagesDir = "/var/www/myconnshop/html/images";
            string[] images = entity.ProductsImages.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
            //string[] images = entity.ProductsImages.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> imagesTemp = new List<string>();
            foreach (string item in images)
            {
                string imagesPath = System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, item);
                if (System.IO.File.Exists(imagesPath))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(imagesPath);
                    client.ChDir(imagesDir + @"/v/" + info.Directory.Name);
                    client.Put(info.DirectoryName, info.Name);
                    client.ChDir(imagesDir + @"/s/" + info.Directory.Name);
                    client.Put(info.DirectoryName, info.Name);
                    client.ChDir(imagesDir + @"/l/" + info.Directory.Name);
                    client.Put(info.DirectoryName, info.Name);
                }

            }

            //client.ChDir("ruinor/html/huanTest/");
            //client.ChDir("html");
            //client.ChDir("huanTest");
            //client.MkDir("huanTest");
            //client.Put("d:/", "1658.jpg");
            //client.Delete("ruinor/html/huanTest/1.jpg");

            return rs;
        }


        /// <summary>
        /// 上传产品
        /// </summary>
        /// <returns></returns>
        public static bool UploadImagesBy(internalproductEntity entity)
        {
            bool rs = false;
            //MultiUser.Instance.ProductLibraryImagesPath

            HLH.Lib.Helper.FTPHelper.FtpWeb client = new HLH.Lib.Helper.FTPHelper.FtpWeb("173.255.213.19", "/var/www/myconnshop/html/images", "max2017", "ftpruinor");

            //client.RemoteHost = "ftp.minigadgetshop.com";
            //client.RemoteHost = "173.255.213.19";
            //client.RemotePass = "max2017";
            //client.RemoteUser = "ftpruinor";
            //client.RemotePort = 21;
            //client.RemotePath = "/var/www/myconnshop/html/images";

            string[] images = entity.ProductsImages.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> imagesTemp = new List<string>();
            foreach (string item in images)
            {
                string imagesPath = System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, item);
                if (System.IO.File.Exists(imagesPath))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(imagesPath);
                    string dirV = @"v/" + info.Directory.Name;
                    client.CheckDirectoryExist(System.IO.Path.Combine(client.ftpRemotePath, dirV));
                    client.DirectoryExist(dirV);
                    client.Upload(info.Name);
                    //client.ChDir(@"v/" + info.Directory.Name);
                    //client.Put(info.DirectoryName, info.Name);
                    //client.ChDir(@"s/" + info.Directory.Name);
                    //client.Put(info.DirectoryName, info.Name);
                    //client.ChDir(@"l/" + info.Directory.Name);
                    //client.Put(info.DirectoryName, info.Name);
                }

            }

            //client.ChDir("ruinor/html/huanTest/");
            //client.ChDir("html");
            //client.ChDir("huanTest");
            //client.MkDir("huanTest");
            //client.Put("d:/", "1658.jpg");
            //client.Delete("ruinor/html/huanTest/1.jpg");

            return rs;
        }
    }
}
