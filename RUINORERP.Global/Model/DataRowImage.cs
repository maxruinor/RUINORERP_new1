using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.Model
{
    /// <summary>
    /// 一个图片名包含的元素：24/10/01J9XPW73YMB56ZWP3FJFH5Q68-f82df05f9d34bb4a84e6856b235e00e7_f82df05f9d34bb4a84e6856b235e00e7
    /// 前缀：24/10/年月来区别子文件夹
    /// 文件名：J9XPW73YMB56ZWP3FJFH5Q68 ，唯一的一个文件名，因为后面是hash值，相同图片会一样。但是保存到服务器时是可以多行数据 使用相同的图片文件名的
    /// hash值：f82df05f9d34bb4a84e6856b235e00e7  旧的hash值
    /// newhash值：f82df05f9d34bb4a84e6856b235e00e7  新的hash值
    /// 如果第一次上传，newhash值和hash值一样
    /// </summary>
    public class DataRowImage
    {
        //旧的hash值，除了第一次和数据库取出。其它都是修改newhash.实际作用是文件名。用于判断是否修改过

        public string oldhash { get; set; }
        //以新的为标准，旧的是用来比较的。
        public string newhash { get; set; }
        /// <summary>
        /// 生成唯一文件名  Ulid.NewUlid().ToString()
        /// </summary>
        public string realName { get; set; }


        public Image image { get; set; }
        public byte[] ImageBytes { get; set; }




        /// <summary>
        /// 更新全名。如果旧的为空。则旧的为新的。
        /// 当newhash改变时，更新hash值，这时应该是图片上传成功。要覆盖旧名，新旧一样。方便后面更新
        /// </summary>
        /// <param name="_newhash"></param>
        /// <returns></returns>
        public string UpdateImageName(string _newhash)
        {
            oldhash = _newhash;
            ImageFullName = Dir + realName + "-" + oldhash + "_" + newhash;
            return ImageFullName; ;
        }


        public string Dir { get; set; }// = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/";






        private string _ImageFullName;

        /// <summary>
        /// 实际就是上面四个部分的组合拼接
        /// </summary>
        public string ImageFullName
        {
            get
            {
                return _ImageFullName;
            }
            set
            {
                //严格按四个部分来组合
                this._ImageFullName = value;
                if (_ImageFullName != null)
                {
                    int realnameIndex = _ImageFullName.IndexOf("-");
                    if (string.IsNullOrEmpty(oldhash))
                    {
                        oldhash = _ImageFullName.IndexOf("_") >= 0 ? ImageFullName.Substring(realnameIndex + 1, ImageFullName.IndexOf("_") - realnameIndex - 1) : "";
                    }
                    if (string.IsNullOrEmpty(newhash))
                    {
                        newhash = _ImageFullName.IndexOf("_") >= 0 ? ImageFullName.Substring(ImageFullName.IndexOf("_") + 1) : "";
                    }
                    if (string.IsNullOrEmpty(Dir))
                    {
                        Dir = string.Join("/", _ImageFullName.Split('/').Take(2)) + "/";
                        if (Dir.Length > 10)
                        {
                            Dir = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/";
                        }
                    }
                    if (string.IsNullOrEmpty(realName))
                    {
                        realName = string.Join("-", _ImageFullName.Split('-').Take(1));
                        if (string.IsNullOrEmpty(realName))
                        {
                            realName = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/" + Ulid.NewUlid().ToString();
                        }
                    }
                }

            }
        }

        public string GetUploadfileName()
        {
            return Dir + realName + "-" + newhash;
        }

        //public string GetOldRealfileName()
        //{
        //    return realName + "-" + oldhash;
        //}






        /// <summary>
        /// 设置一个新的hash值
        /// 新旧变化只适用于下载后加载，与上传时选中拖入粘贴有关。
        /// </summary>
        /// <param name="ParaNewHash"></param>
        public void SetImageNewHash(string ParaNewHash)
        {
            newhash = ParaNewHash;
            if (string.IsNullOrEmpty(Dir))
            {
                Dir = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/";
            }
            if (string.IsNullOrEmpty(realName))
            {
                realName = Ulid.NewUlid().ToString();
            }
            //下载时才会更新为真正的旧hash值
            if (string.IsNullOrEmpty(oldhash))
            {
                oldhash = string.Empty;
            }
            _ImageFullName = Dir + realName + "-" + oldhash + "_" + newhash;
        }
    }
}
