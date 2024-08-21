using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;

namespace HLH.Lib.Draw
{

    /// <summary>
    /// ͼƬ���������
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// ��ʾͼƬ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImage(string path, int W, int H)
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
        /// ͨ��FileStream �����ļ��������Ϳ���ʵ�ֲ�����Image�ļ�����ʱ�����ö��û�ͬʱ����Image�ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //����ļ����� 
            Byte[] image = new Byte[filelength]; //����һ���ֽ����� 
            fs.Read(image, 0, filelength); //���ֽ�����ȡ 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        /// <summary>
        /// ��ͼƬ����������ˮӡ
        /// </summary>
        /// <param name="addText">ˮӡ�ϵ�����</param>
        /// <param name="PathSource">ԭ������ͼƬ·��</param>
        /// <param name="PathTarget">���ɵĴ�����ˮӡ��ͼƬ·��</param>
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
        /// ��ͼƬ����������ˮӡ
        /// </summary>
        /// <param name="addText">ˮӡ�ϵ�����</param>
        /// <param name="PathSource">ԭ������ͼƬ·��</param>
        /// <param name="PathTarget">���ɵĴ�����ˮӡ��ͼƬ·��</param>
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
        /// ��ͼƬ������ͼƬˮӡ
        /// </summary>
        /// <param name="Path">ԭ������ͼƬ·��</param>
        /// <param name="Path_syp">���ɵĴ�ͼƬˮӡ��ͼƬ·��</param>
        /// <param name="Path_sypf">ˮӡͼƬ·��</param>
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

        #region ��������ͼ

        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="src">��Դҳ��
        /// ��������Ե�ַ���߾��Ե�ַ
        /// </param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <returns>�ֽ�����</returns>
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
            //ȡ��ͼƬ��С
            Size size = new Size((int)newWidth, (int)newHeight);
            //�½�һ��bmpͼƬ
            Image bitmap = new Bitmap(size.Width, size.Height);
            //�½�һ������
            Graphics g = Graphics.FromImage(bitmap);
            //���ø�������ֵ��
            g.InterpolationMode = InterpolationMode.High;
            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = SmoothingMode.HighQuality;
            //���һ�»���
            g.Clear(Color.White);
            //��ָ��λ�û�ͼ
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            //����������ȵ�����ͼ
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }

        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="src">��Դҳ��
        /// ��������Ե�ַ���߾��Ե�ַ
        /// </param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <returns>�ֽ�����</returns>
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
            //ȡ��ͼƬ��С
            Size size = new Size((int)newWidth, (int)newHeight);
            //�½�һ��bmpͼƬ
            Image bitmap = new Bitmap(size.Width, size.Height);
            //�½�һ������
            Graphics g = Graphics.FromImage(bitmap);
            //���ø�������ֵ��
            g.InterpolationMode = InterpolationMode.High;
            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = SmoothingMode.HighQuality;
            //���һ�»���
            g.Clear(Color.White);
            //��ָ��λ�û�ͼ
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

            //����������ȵ�����ͼ
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            //byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            //bitmap.Dispose();
            return bitmap;
        }


        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="src">��Դҳ��
        /// ��������Ե�ַ���߾��Ե�ַ
        /// </param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <returns>�ֽ�����</returns>
        public static byte[] MakeThumbnail(string src, double width, double height)
        {
            Image image;

            // ���·���ӱ���ֱ�Ӷ�ȡ
            if (src.ToLower().IndexOf("http") == -1)
            {
                src = HttpContext.Current.Server.MapPath(src);
                image = Image.FromFile(src, true);
            }
            else // ����·���� Http ��ȡ
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
            //ȡ��ͼƬ��С
            Size size = new Size((int)newWidth, (int)newHeight);
            //�½�һ��bmpͼƬ
            Image bitmap = new Bitmap(size.Width, size.Height);
            //�½�һ������
            Graphics g = Graphics.FromImage(bitmap);
            //���ø�������ֵ��
            g.InterpolationMode = InterpolationMode.High;
            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = SmoothingMode.HighQuality;
            //���һ�»���
            g.Clear(Color.White);
            //��ָ��λ�û�ͼ
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
            //����������ȵ�����ͼ
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }

        #endregion


        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
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
                case "HW"://ָ���߿����ţ����ܱ��Σ�                
                    break;
                case "W"://ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://ָ���߿�ü��������Σ�                
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

            //�½�һ��bmpͼƬ
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //��jpg��ʽ��������ͼ
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
                case "HW"://ָ���߿����ţ����ܱ��Σ�                
                    break;
                case "W"://ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://ָ���߿�ü��������Σ�                
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

            //�½�һ��bmpͼƬ
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {


                //��jpg��ʽ��������ͼ
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
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
        public static byte[] MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            MemoryStream ms = new MemoryStream();
            System.Drawing.Image img = MakeThumbnailNewImage(originalImagePath, width, height, mode);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();

        }


        /// <summary>
        /// �������õ�������ѹ��ͼƬ
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="outPath"></param>
        /// <param name="flag">����ѹ���ı���1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            //���´���Ϊ����ͼƬʱ������ѹ������  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//����ѹ���ı���1-100  
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
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile��ѹ�������·��  
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

            //���´���Ϊ����ͼƬʱ������ѹ������  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//����ѹ���ı���1-100  
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
                    iSource.Save(stream, jpegICIinfo, ep);//dFile��ѹ�������·��  
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
    }
}
