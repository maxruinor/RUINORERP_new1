using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace RUINORERP.Common.Helper
{
    /*
     *  string filePath = "AutoUpdate.txt"; // 要监视的文件路径

    FileWatcher watcher = new FileWatcher(filePath);
    watcher.StartWatching();

    Console.WriteLine("按任意键停止监视...");
    Console.ReadKey();

    watcher.StopWatching();
     */
    public class FileWatcher
    {
        private FileSystemWatcher _fileWatcher;
        private string _filePath;
        private string content;
        public FileWatcher(string filePath)
        {
            _filePath = filePath;
        }

        public void StartWatching()
        {
            // 创建 FileSystemWatcher 实例
            _fileWatcher = new FileSystemWatcher();

            // 设置要监视的目录路径
            string directory = Path.GetDirectoryName(_filePath);
            if (directory == null)
            {
                throw new ArgumentException("文件路径无效。");
            }
            _fileWatcher.Path = directory;

            // 设置要监视的文件名
            string fileName = Path.GetFileName(_filePath);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("文件名无效。");
            }
            _fileWatcher.Filter = fileName;

            // 设置要监视的更改类型
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;

            // 添加事件处理程序
            _fileWatcher.Changed += OnFileChanged;

            // 开启事件监听
            _fileWatcher.EnableRaisingEvents = true;

            Console.WriteLine($"已开始监视文件: {_filePath}");
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == _filePath)
            {
                Console.WriteLine($"文件已变动: {e.FullPath}");
                // 在这里可以执行文件变动后的逻辑
                // 在这里可以读取文件内容或执行其他操作
                string content = File.ReadAllText(_filePath);
                Console.WriteLine("文件内容：");
                Console.WriteLine(content);
            }
        }

        public void StopWatching()
        {
            if (_fileWatcher != null)
            {
                // 停止引发事件
                _fileWatcher.EnableRaisingEvents = false;

                // 清理资源
                _fileWatcher.Dispose();
                _fileWatcher = null;

                Console.WriteLine("已停止监视文件");
            }
        }
    }
}
