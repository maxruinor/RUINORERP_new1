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
    /// 图片处理帮助类 - 统一版本
    /// 整合了HLH.Lib.Draw.ImageHelper、RUINORERP.Common.Helper.ImageHelper、RUINORERP.UI.Common.ImageHelper的所有功能
    /// </summary>
    public class ImageHelper
    {
        #region 哈希计算

        /// <summary>
        /// 计算图片文件的哈希值(MD5)
        /// </summary>
        /// <param name="filePath">图片文件路径</param>
        /// <returns>MD5哈希字符串(小写,无连字符)</returns>
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

        /// <summary>
        /// 计算图片对象的哈希值(MD5)
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns>MD5哈希字符串(小写,无连字符),如果图片为null返回空字符串</returns>
        public static string GetImageHash(Image img)
        {
            if (img == null)
            {
                return string.Empty;
            }
            byte[] bytes = ConvertImageToByteEx(img);
            return GenerateHash(bytes);
        }

        /// <summary>
        /// 计算字节数组的哈希值(MD5)
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>MD5哈希字符串(小写,无连字符)</returns>
        public static string GenerateHash(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 计算流的哈希值(MD5)
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>MD5哈希字符串(小写,无连字符)</returns>
        public static string GenerateHash(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 比较两个哈希值是否相等(不区分大小写)
        /// </summary>
        /// <param name="hash1">哈希值1</param>
        /// <param name="hash2">哈希值2</param>
        /// <returns>相等返回true,否则返回false</returns>
        public static bool AreHashesEqual(string hash1, string hash2)
        {
            return hash1.Equals(hash2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 比较两个图片文件是否相同(通过哈希值)
        /// </summary>
        /// <param name="filePath1">文件1路径</param>
        /// <param name="filePath2">文件2路径</param>
        /// <returns>相同返回true,否则返回false</returns>
        public static bool AreImagesEqual(string filePath1, string filePath2)
        {
            var hash1 = GetImageHash(filePath1);
            var hash2 = GetImageHash(filePath2);
            return hash1 == hash2;
        }

        #endregion

        #region 图片与字节数组转换

        /// <summary>
        /// 将图片转换为字节数组(GIF格式)
        /// </summary>
        /// <param name="imageIn">图片对象</param>
        /// <returns>字节数组</returns>
        public static byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Gif);
            return ms.ToArray();
        }

        /// <summary>
        /// 将字节数组转换为图片
        /// </summary>
        /// <param name="byteArrayIn">字节数组</param>
        /// <returns>图片对象</returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// 从字节数组获取图片
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>图片对象</returns>
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

        /// <summary>
        /// 将图片转换为字节数组(BMP格式)
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns>字节数组,如果图片为null返回null</returns>
        public byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }

        /// <summary>
        /// 将字节数组转换为图片对象
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>图片对象,如果为空返回null</returns>
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

        /// <summary>
        /// 将图片对象转换为字节数组(BMP格式,增强版)
        /// </summary>
        /// <param name="src">源图片</param>
        /// <returns>字节数组</returns>
        public static byte[] ConvertImageToByteEx(Image src)
        {
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            Image bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gs = Graphics.FromImage(bmp);
            Rectangle rr = new Rectangle(0, 0, rect.Width, rect.Height);
            if (src != null)
            {
                gs.DrawImage(src, rr, rect, GraphicsUnit.Pixel);
            }
            gs.Dispose();

            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;

            byte[] temp = stream.GetBuffer();
            stream.Close();
            return temp;
        }

        /// <summary>
        /// 将Bitmap对象转换为字节数组(BMP格式)
        /// </summary>
        /// <param name="src">源Bitmap</param>
        /// <returns>字节数组</returns>
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

            byte[] temp = stream.ToArray();
            stream.Close();
            return temp;
        }

        #endregion

        #region Base64转换

        /// <summary>
        /// 从文件获取图片
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>图片对象</returns>
        public static Image GetImgFromFile(string fileName)
        {
            return Image.FromFile(fileName);
        }

        /// <summary>
        /// 从base64字符串读取图片
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <returns>图片对象</returns>
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
        /// <returns>图片对象</returns>
        public static Image GetImgFromBase64Url(string base64Url)
        {
            string base64 = GetBase64String(base64Url);
            return GetImgFromBase64(base64);
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 默认使用jpg格式
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns>base64字符串</returns>
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
        /// <returns>base64字符串</returns>
        public static string ToBase64String(Image img, ImageFormat imageFormat)
        {
            MemoryStream memStream = new MemoryStream();
            img.Save(memStream, imageFormat);
            byte[] bytes = memStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        /// <summary>
        /// 将字节数组转为带URL前缀的base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>带URL前缀的base64字符串</returns>
        public static string ToBase64StringUrl(byte[] bytes)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将图片转为base64字符串
        /// 默认使用jpg格式,并添加data:image/jpg;base64,前缀
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns>带URL前缀的base64字符串</returns>
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
        /// <returns>带URL前缀的base64字符串</returns>
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
        /// <returns>纯base64字符串</returns>
        public static string GetBase64String(string base64UrlStr)
        {
            string parttern = "^(data:image/.*?;base64,).*?$";
            var match = Regex.Match(base64UrlStr, parttern);
            if (match.Groups.Count > 1)
                base64UrlStr = base64UrlStr.Replace(match.Groups[1].ToString(), "");

            return base64UrlStr;
        }

        #endregion

        #region 图片读取与显示

        /// <summary>
        /// 获取指定尺寸的图片
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="W">目标宽度</param>
        /// <param name="H">目标高度</param>
        /// <returns>调整大小后的图片,路径无效返回null</returns>
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
        /// 通过FileStream 来打开文件,这样可以实现不锁定Image文件,到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path">图片文件路径</param>
        /// <returns>Bitmap对象</returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path);
            int filelength = 0;
            filelength = (int)fs.Length;
            Byte[] image = new Byte[filelength];
            fs.Read(image, 0, filelength);
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        #endregion

        #region 水印

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="addText">水印上的文字</param>
        /// <param name="PathSource">原服务器图片路径</param>
        /// <param name="PathTarget">生成的带文字水印的图片路径</param>
        /// <param name="fontSize">字体大小</param>
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
        /// 在图片上增加文字水印(默认字体大小60)
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

        #endregion

        #region 缩略图生成

        /// <summary>
        /// 创建缩略图(从字节数组)
        /// </summary>
        /// <param name="data">图片字节数组</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图字节数组</returns>
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
            Size size = new Size((int)newWidth, (int)newHeight);
            Image bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }

        /// <summary>
        /// 创建缩略图(从字节数组,返回Image)
        /// </summary>
        /// <param name="data">图片字节数组</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图Image对象</returns>
        public static Image MakeThumbnailfromByte(byte[] data, double width, double height)
        {
            MemoryStream ms = new MemoryStream(data);
            Image image = Image.FromStream(ms);
            return MakeThumbnailfromImage(image, width, height);
        }

        /// <summary>
        /// 创建缩略图(从Image对象)
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图Image对象</returns>
        public static Image MakeThumbnailfromImage(Image image, double width, double height)
        {
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
            Size size = new Size((int)newWidth, (int)newHeight);
            Image bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return bitmap;
        }

        /// <summary>
        /// 创建缩略图(从URL或本地路径)
        /// </summary>
        /// <param name="src">来源页面,可以是相对地址或者绝对地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图字节数组</returns>
        public static byte[] MakeThumbnail(string src, double width, double height)
        {
            Image image;

            if (src.ToLower().IndexOf("http") == -1)
            {
                src = HttpContext.Current.Server.MapPath(src);
                image = Image.FromFile(src, true);
            }
            else
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
            Size size = new Size((int)newWidth, (int)newHeight);
            Image bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }

        /// <summary>
        /// 生成缩略图(保存到文件)
        /// </summary>
        /// <param name="originalImagePath">源图路径(物理路径)</param>
        /// <param name="thumbnailPath">缩略图路径(物理路径)</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW-指定高宽缩放,W-指定宽高按比例,H-指定高宽按比例,Cut-指定高宽裁减</param>    
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
                case "HW":
                    break;
                case "W":
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
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

            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
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

        /// <summary>
        /// 生成缩略图(返回Image对象)
        /// </summary>
        /// <param name="originalImagePath">源图路径(物理路径)</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW-指定高宽缩放,W-指定宽高按比例,H-指定高宽按比例,Cut-指定高宽裁减</param>
        /// <returns>缩略图Image对象</returns>
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
                case "HW":
                    break;
                case "W":
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
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

            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
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
        /// 生成缩略图(返回字节数组)
        /// </summary>
        /// <param name="originalImagePath">源图路径(物理路径)</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW-指定高宽缩放,W-指定宽高按比例,H-指定高宽按比例,Cut-指定高宽裁减</param>
        /// <returns>缩略图字节数组</returns>
        public static byte[] MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            MemoryStream ms = new MemoryStream();
            System.Drawing.Image img = MakeThumbnailNewImage(originalImagePath, width, height, mode);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        #endregion

        #region 图片压缩

        /// <summary>
        /// 根据设置的质量来压缩图片并保存到文件
        /// </summary>
        /// <param name="sFile">源文件路径</param>
        /// <param name="outPath">输出文件路径</param>
        /// <param name="flag">设置压缩的比例1-100</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
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
                    iSource.Save(outPath, jpegICIinfo, ep);
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
            }
        }

        /// <summary>
        /// 根据设置的质量来压缩图片(返回字节数组)
        /// </summary>
        /// <param name="strOriginalFile">源文件路径</param>
        /// <param name="flag">设置压缩的比例1-100</param>
        /// <returns>压缩后的图片字节数组</returns>
        public static byte[] GetPicThumbnail(string strOriginalFile, int flag)
        {
            byte[] buffer = null;
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(strOriginalFile);
            ImageFormat tFormat = iSource.RawFormat;

            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
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
                    iSource.Save(stream, jpegICIinfo, ep);
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
            }
        }

        /// <summary>
        /// 压缩图片(等比压缩)
        /// </summary>
        /// <param name="img">原图片</param>
        /// <param name="width">压缩后宽度</param>
        /// <returns>压缩后的图片</returns>
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
        /// <returns>压缩后的图片</returns>
        public static Image CompressImg(Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(img, width, height);
            return bitmap;
        }

        #endregion
    }
}
