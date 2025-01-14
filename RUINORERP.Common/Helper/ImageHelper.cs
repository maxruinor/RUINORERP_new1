using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace RUINORERP.Common.Helper
{

    /// <summary>
    /// 图片处理帮助类
    /// </summary>
    public class ImageHelper
    {
        public static string GetImageHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLowerInvariant();
                }
            }
        }

        public static string GetImageHash(Image image)
        {
            if (image == null)
            {
                return string.Empty;
            }
            using (var md5 = MD5.Create())
            {
                byte[] imageBytes = ConvertImageToByteEx(image);
                // 计算 hash
                var hashBytes = md5.ComputeHash(imageBytes);
                // 将 byte 数组转换为字符串
                return BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLowerInvariant();

            }
        }

        public static bool AreImagesEqual(string filePath1, string filePath2)
        {
            var hash1 = GetImageHash(filePath1);
            var hash2 = GetImageHash(filePath2);
            return hash1 == hash2;
        }

        public Image GetImageByBytes(byte[] bytes)
        {
            Image photo = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Write(bytes, 0, bytes.Length);
                photo = Image.FromStream(ms, true);

            }
            return photo;
        }



        public byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }





        /// <summary>
        /// 将字节转化为image对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image ConvertByteToImg(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return null;

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                Image img = Image.FromStream(stream);
                stream.Close();
                return img;
            }
        }



        public static byte[] ConvertImageToByteEx(Image src)
        {
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            Image bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gs = Graphics.FromImage(bmp);
            Rectangle rr = new Rectangle(0, 0, rect.Width, rect.Height);
            if (src != null)
            {
                gs.DrawImage(src, rr, rect, GraphicsUnit.Pixel);//07072/07074/07076
            }
            gs.Dispose();


            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;

            byte[] temp = stream.GetBuffer();
            stream.Close();
            return temp;
        }
        public static byte[] ConvertBitmapToByteEx(Bitmap src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gs = Graphics.FromImage(bmp);
            Rectangle rr = new Rectangle(0, 0, rect.Width, rect.Height);
            gs.DrawImage(src, rr, rect, GraphicsUnit.Pixel);
            gs.Dispose();

            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;

            byte[] temp = stream.ToArray(); // 使用 ToArray 而不是 GetBuffer
            stream.Close();
            return temp;
        }

        ///============================================================2023
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        public static Image GetImage(string path, int W, int H)
        {
            if (path.StartsWith("//") || string.IsNullOrEmpty(path))
            {
                return null;
            }
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
                System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
                System.Drawing.Image newrs = new Bitmap(result, W, H);
                fs.Close();
                return newrs;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="addText">水印上的文字</param>
        /// <param name="PathSource">原服务器图片路径</param>
        /// <param name="PathTarget">生成的带文字水印的图片路径</param>
        public static void AddWater(string addText, string PathSource, string PathTarget, float fontSize)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(PathSource);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", fontSize);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 15, 15);
            g.Dispose();

            image.Save(PathTarget);
            image.Dispose();
        }



        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="addText">水印上的文字</param>
        /// <param name="PathSource">原服务器图片路径</param>
        /// <param name="PathTarget">生成的带文字水印的图片路径</param>
        public static void AddWater(string addText, string PathSource, string PathTarget)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(PathSource);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(PathTarget);
            image.Dispose();
        }


        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }

        #region 生成缩略图

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="src">来源页面
        /// 可以是相对地址或者绝对地址
        /// </param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>字节数组</returns>
        public static byte[] MakeThumbnail(byte[] data, double width, double height)
        {
            MemoryStream ms = new MemoryStream(data);
            Image image = System.Drawing.Image.FromStream(ms);

            double newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newWidth = width;
                newHeight = image.Height * (newWidth / image.Width);
            }
            else
            {
                newHeight = height;
                newWidth = (newHeight / image.Height) * image.Width;
            }
            if (newWidth > width)
            {
                newWidth = width;
            }
            if (newHeight > height)
            {
                newHeight = height;
                newWidth = image.Width * (newHeight / image.Height);
            }
            //取得图片大小
            Size size = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            Image bitmap = new Bitmap(size.Width, size.Height);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            //保存高清晰度的缩略图
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="src">来源页面
        /// 可以是相对地址或者绝对地址
        /// </param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>字节数组</returns>
        public static Image MakeThumbnailfromByte(byte[] data, double width, double height)
        {
            MemoryStream ms = new MemoryStream(data);
            Image image = System.Drawing.Image.FromStream(ms);

            double newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newWidth = width;
                newHeight = image.Height * (newWidth / image.Width);
            }
            else
            {
                newHeight = height;
                newWidth = (newHeight / image.Height) * image.Width;
            }
            if (newWidth > width)
            {
                newWidth = width;
            }
            if (newHeight > height)
            {
                newHeight = height;
                newWidth = image.Width * (newHeight / image.Height);
            }
            //取得图片大小
            Size size = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            Image bitmap = new Bitmap(size.Width, size.Height);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

            //保存高清晰度的缩略图
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            //byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            //bitmap.Dispose();
            return bitmap;
        }


        /*
        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="src">来源页面
        /// 可以是相对地址或者绝对地址
        /// </param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>字节数组</returns>
        public static byte[] MakeThumbnail(string src, double width, double height)
        {
            Image image;

            // 相对路径从本机直接读取
            if (src.ToLower().IndexOf("http") == -1)
            {
                src = HttpContext.Current.Server.MapPath(src);
                image = Image.FromFile(src, true);
            }
            else // 绝对路径从 Http 读取
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(src);
                req.Method = "GET";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = resp.GetResponseStream();
                image = Image.FromStream(receiveStream);
                resp.Close();
                receiveStream.Close();
            }
            double newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newWidth = width;
                newHeight = image.Height * (newWidth / image.Width);
            }
            else
            {
                newHeight = height;
                newWidth = (newHeight / image.Height) * image.Width;
            }
            if (newWidth > width)
            {
                newWidth = width;
            }
            if (newHeight > height)
            {
                newHeight = height;
                newWidth = image.Width * (newHeight / image.Height);
            }
            //取得图片大小
            Size size = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            Image bitmap = new Bitmap(size.Width, size.Height);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            //保存高清晰度的缩略图
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }
        */
        #endregion


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        public static System.Drawing.Image MakeThumbnailNewImage(string originalImagePath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {


                //以jpg格式保存缩略图
                //  bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

            return bitmap;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static byte[] MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            MemoryStream ms = new MemoryStream();
            System.Drawing.Image img = MakeThumbnailNewImage(originalImagePath, width, height, mode);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();

        }


        /// <summary>
        /// 根据设置的质量来压缩图片
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="outPath"></param>
        /// <param name="flag">设置压缩的比例1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }

        public static byte[] GetPicThumbnail(string strOriginalFile, int flag)
        {
            byte[] buffer = null;
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(strOriginalFile);
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            MemoryStream stream = new MemoryStream();
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(stream, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(stream, tFormat);
                }

                buffer = stream.GetBuffer();
                return buffer;


            }
            catch
            {
                return buffer;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }


        /// <summary>
        /// 从文件获取图片
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static Image GetImgFromFile(string fileName)
        {
            return Image.FromFile(fileName);
        }

        /// <summary>
        /// 从base64字符串读入图片
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <returns></returns>
        public static Image GetImgFromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream memStream = new MemoryStream(bytes);
            Image img = Image.FromStream(memStream);

            return img;
        }

        /// <summary>
        /// 从URL格式的Base64图片获取真正的图片
        /// 即去掉data:image/jpg;base64,这样的格式
        /// </summary>
        /// <param name="base64Url">图片Base64的URL形式</param>
        /// <returns></returns>
        public static Image GetImgFromBase64Url(string base64Url)
        {
            string base64 = GetBase64String(base64Url);

            return GetImgFromBase64(base64);
        }

        /// <summary>
        /// 压缩图片
        /// 注:等比压缩
        /// </summary>
        /// <param name="img">原图片</param>
        /// <param name="width">压缩后宽度</param>
        /// <returns></returns>
        public static Image CompressImg(Image img, int width)
        {
            return CompressImg(img, width, (int)((double)width / img.Width * img.Height));
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="img">原图片</param>
        /// <param name="width">压缩后宽度</param>
        /// <param name="height">压缩后高度</param>
        /// <returns></returns>
        public static Image CompressImg(Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(img, width, height);

            return bitmap;
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 默认使用jpg格式
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns></returns>
        public static string ToBase64String(Image img)
        {
            return ToBase64String(img, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 使用指定格式
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <param name="imageFormat">指定格式</param>
        /// <returns></returns>
        public static string ToBase64String(Image img, ImageFormat imageFormat)
        {
            MemoryStream memStream = new MemoryStream();
            img.Save(memStream, imageFormat);
            byte[] bytes = memStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        public static string ToBase64StringUrl(byte[] bytes)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 默认使用jpg格式,并添加data:image/jpg;base64,前缀
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns></returns>
        public static string ToBase64StringUrl(Image img)
        {
            return "data:image/jpg;base64," + ToBase64String(img, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 使用指定格式,并添加data:image/jpg;base64,前缀
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <param name="imageFormat">指定格式</param>
        /// <returns></returns>
        public static string ToBase64StringUrl(Image img, ImageFormat imageFormat)
        {
            string base64 = ToBase64String(img, imageFormat);

            return $"data:image/{imageFormat.ToString().ToLower()};base64,{base64}";
        }

        /// <summary>
        /// 获取真正的图片base64数据
        /// 即去掉data:image/jpg;base64,这样的格式
        /// </summary>
        /// <param name="base64UrlStr">带前缀的base64图片字符串</param>
        /// <returns></returns>
        public static string GetBase64String(string base64UrlStr)
        {
            string parttern = "^(data:image/.*?;base64,).*?$";

            var match = Regex.Match(base64UrlStr, parttern);
            if (match.Groups.Count > 1)
                base64UrlStr = base64UrlStr.Replace(match.Groups[1].ToString(), "");

            return base64UrlStr;
        }

        ///// <summary>
        ///// 将图片的URL或者Base64字符串转为图片并上传到服务器，返回上传后的完整图片URL
        ///// </summary>
        ///// <param name="imgBase64OrUrl">URL地址或者Base64字符串</param>
        ///// <returns></returns>
        //public static string GetImgUrl(string imgBase64OrUrl)
        //{
        //    if (imgBase64OrUrl.Contains("data:image"))
        //    {
        //        Image img = GetImgFromBase64Url(imgBase64OrUrl);
        //        string fileName = $"{GuidHelper.GenerateKey()}.jpg";

        //        string dir = Path.Combine(AppSettings.WebRootPath, "Upload", "Img");
        //        if (!Directory.Exists(dir))
        //            Directory.CreateDirectory(dir);
        //        img.Save(Path.Combine(dir, fileName));

        //        return $"{AppSettings.WebRootPath}/Upload/Img/{fileName}";
        //    }

        //    return imgBase64OrUrl;
        //}
    }
}
