using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public partial class tb_FS_FileStorageInfo
    {
        //只有业务系统中流转的文件，才会有图片数据
        /// <summary>
        /// 图片数据
        /// </summary>
        public byte[] FileData { get; set; }
    }
}
