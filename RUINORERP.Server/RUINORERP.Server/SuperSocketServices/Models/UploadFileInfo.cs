using System;

namespace RUINORERP.Server.Commands
{
    internal class UploadFileInfo
    {
        public string FileId { get; set; }
        public string OriginalName { get; set; }
        public string Category { get; set; }
        public long Size { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime LastModified { get; set; }
    }
}