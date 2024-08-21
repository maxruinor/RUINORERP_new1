using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace HLH.Lib.Helper
{
    public class DataConversionHelper
    {

        /// <summary>
        /// 1、将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 2、将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        // <summary> 
        /// 3、字节流转换成图片 
        /// </summary> 
        /// <param name="byt">要转换的字节流</param> 
        /// <returns>转换得到的Image对象</returns> 
        public static Image BytToImg(byte[] byt)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byt);
                Image img = Image.FromStream(ms);
                return img;
            }
            catch (Exception ex)
            {
                log4netHelper.error("StreamHelper.BytToImg 异常", ex);
                return null;
            }
        }

        /// <summary>
        ///  4、图片转换成字节流 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }


        /// <summary>
        /// 5、把图片Url转化成Image对象
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static Image Url2Img(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return null;
                }

                WebRequest webreq = WebRequest.Create(imageUrl);
                WebResponse webres = webreq.GetResponse();
                Stream stream = webres.GetResponseStream();
                Image image;
                image = Image.FromStream(stream);
                stream.Close();

                return image;
            }
            catch (Exception ex)
            {
                log4netHelper.error("StreamHelper.Url2Img 异常", ex);
            }

            return null;
        }



        /// <summary>
        /// 6、把本地图片路径转成Image对象
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Image ImagePath2Img(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    return null;
                }
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.FileStream fs = new System.IO.FileStream(imagePath, System.IO.FileMode.Open);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                    //System.Drawing.Image image = new Bitmap(result);
                    fs.Close();
                    return image;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log4netHelper.error("StreamHelper.ImagePath2Img 异常", ex);
                return null;
            }
        }



    }
}
