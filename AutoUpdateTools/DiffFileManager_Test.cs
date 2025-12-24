using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AULWriter
{
    /// <summary>
    /// 差异文件管理测试类
    /// </summary>
    public class DiffFileManager_Test
    {
        /// <summary>
        /// 差异文件数据模型
        /// </summary>
        public class DiffFileItem
        {
            /// <summary>
            /// 是否选中
            /// </summary>
            public bool IsSelected { get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 文件大小（KB）
            /// </summary>
            public string FileSize { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            public string LastModified { get; set; }
            /// <summary>
            /// 文件状态
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 完整文件路径
            /// </summary>
            public string FullFilePath { get; set; }
        }

        /// <summary>
        /// 测试差异文件模型
        /// </summary>
        public static void TestDiffFileItem()
        {
            DiffFileItem item = new DiffFileItem
            {
                IsSelected = true,
                FileName = "test.txt",
                FilePath = "test\test.txt",
                FileSize = "10.00 KB",
                LastModified = "2023-01-01 00:00:00",
                Status = "新增",
                FullFilePath = "C:\\test\test.txt"
            };

            Console.WriteLine("测试差异文件模型成功");
        }
    }
}