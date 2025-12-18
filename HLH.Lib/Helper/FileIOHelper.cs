using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace HLH.Lib.Helper.FileIOHelper
{
    /// <summary>
    /// FileDirectoryUtility 类,方法不包含异常处理
    /// </summary>
    public class FileDirectoryUtility
    {
        /// <summary>
        /// 生成PDF文件
        /// </summary>
        /// <param name="strbase64"></param>
        /// <param name="savepath"></param>
        public static void base64ToPdf(string strbase64, string savepath)
        {
            strbase64 = strbase64.Replace(' ', '+');
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(strbase64));
            FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] b = stream.ToArray();
            //byte[] b = stream.GetBuffer();
            fs.Write(b, 0, b.Length);
            fs.Close();


        }


        #region 取得文件后缀名
        /****************************************
          * 函数名称：GetPostfixStr
          * 功能说明：取得文件后缀名
          * 参     数：filename:文件名称
          * 调用示列：
          *            string filename = "aaa.aspx";        
          *            string s = EC.FileObj.GetPostfixStr(filename);         
         *****************************************/
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }
        #endregion



        #region 写文件
        /****************************************
          * 函数名称：WriteFile
          * 功能说明：写文件,会覆盖掉以前的内容
          * 参     数：Path:文件路径,Strings:文本内容
          * 调用示列：
          *            string Path = Server.MapPath("Default2.aspx");       
          *            string Strings = "这是我写的内容啊";
          *            EC.FileObj.WriteFile(Path,Strings);
         *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        /// <param name="enc">生成的编码</param>
        public static void WriteFile(string Path, string Strings, Encoding enc)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, false, enc);
            f2.Write(Strings);
            f2.Close();
            f2.Dispose();
        }

        /// <summary>
        /// 保存文件，返回保存成功后的文件路径
        /// </summary>
        /// <param name="Path">带文件名的路径</param>
        /// <param name="Strings">文件内容</param>
        /// <param name="enc">生成的编码</param>
        public static string SaveFile(string PathwithFileName, string Strings, Encoding enc)
        {
            System.IO.FileInfo fi = new FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }

            if (!System.IO.File.Exists(PathwithFileName))
            {
                System.IO.FileStream f = System.IO.File.Open(PathwithFileName, FileMode.CreateNew, FileAccess.ReadWrite);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(PathwithFileName, false, enc);
            f2.Write(Strings);
            f2.Close();
            f2.Dispose();
            return PathwithFileName;
        }

        #endregion




        #region 读文件

        /* https://blog.csdn.net/weixin_34088583/article/details/85950159
         * 
         * 代码页

名称

显示名称

37

IBM037

IBM EBCDIC（美国 - 加拿大）

437

IBM437

OEM 美国

500

IBM500

IBM EBCDIC（国际）

708

ASMO-708

阿拉伯字符 (ASMO 708)

720

DOS-720

阿拉伯字符 (DOS)

737

ibm737

希腊字符 (DOS)

775

ibm775

波罗的海字符 (DOS)

850

ibm850

西欧字符 (DOS)

852

ibm852

中欧字符 (DOS)

855

IBM855

OEM 西里尔语

857

ibm857

土耳其字符 (DOS)

858

IBM00858

OEM 多语言拉丁语 I

860

IBM860

葡萄牙语 (DOS)

861

ibm861

冰岛语 (DOS)

862

DOS-862

希伯来字符 (DOS)

863

IBM863

加拿大法语 (DOS)

864

IBM864

阿拉伯字符 (864)

865

IBM865

北欧字符 (DOS)

866

cp866

西里尔字符 (DOS)

869

ibm869

现代希腊字符 (DOS)

870

IBM870

IBM EBCDIC（多语言拉丁语 2）

874

windows-874

泰语 (Windows)

875

cp875

IBM EBCDIC（现代希腊语）

932

shift_jis

日语 (Shift-JIS)

936

gb2312

简体中文 (GB2312)

*

949

ks_c_5601-1987

朝鲜语

950

big5

繁体中文 (Big5)

1026

IBM1026

IBM EBCDIC（土耳其拉丁语 5）

1047

IBM01047

IBM 拉丁语 1

1140

IBM01140

IBM EBCDIC（美国 - 加拿大 - 欧洲）

1141

IBM01141

IBM EBCDIC（德国 - 欧洲）

1142

IBM01142

IBM EBCDIC（丹麦 - 挪威 - 欧洲）

1143

IBM01143

IBM EBCDIC（芬兰 - 瑞典 - 欧洲）

1144

IBM01144

IBM EBCDIC（意大利 - 欧洲）

1145

IBM01145

IBM EBCDIC（西班牙 - 欧洲）

1146

IBM01146

IBM EBCDIC（英国 - 欧洲）

1147

IBM01147

IBM EBCDIC（法国 - 欧洲）

1148

IBM01148

IBM EBCDIC（国际 - 欧洲）

1149

IBM01149

IBM EBCDIC（冰岛语 - 欧洲）

1200

utf-16

Unicode

*

1201

UnicodeFFFE

Unicode (Big-Endian)

*

1250

windows-1250

中欧字符 (Windows)

1251

windows-1251

西里尔字符 (Windows)

1252

Windows-1252

西欧字符 (Windows)

*

1253

windows-1253

希腊字符 (Windows)

1254

windows-1254

土耳其字符 (Windows)

1255

windows-1255

希伯来字符 (Windows)

1256

windows-1256

阿拉伯字符 (Windows)

1257

windows-1257

波罗的海字符 (Windows)

1258

windows-1258

越南字符 (Windows)

1361

Johab

朝鲜语 (Johab)

10000

macintosh

西欧字符 (Mac)

10001

x-mac-japanese

日语 (Mac)

10002

x-mac-chinesetrad

繁体中文 (Mac)

10003

x-mac-korean

朝鲜语 (Mac)

*

10004

x-mac-arabic

阿拉伯字符 (Mac)

10005

x-mac-hebrew

希伯来字符 (Mac)

10006

x-mac-greek

希腊字符 (Mac)

10007

x-mac-cyrillic

西里尔字符 (Mac)

10008

x-mac-chinesesimp

简体中文 (Mac)

*

10010

x-mac-romanian

罗马尼亚语 (Mac)

10017

x-mac-ukrainian

乌克兰语 (Mac)

10021

x-mac-thai

泰语 (Mac)

10029

x-mac-ce

中欧字符 (Mac)

10079

x-mac-icelandic

冰岛语 (Mac)

10081

x-mac-turkish

土耳其字符 (Mac)

10082

x-mac-croatian

克罗地亚语 (Mac)

20000

x-Chinese-CNS

繁体中文 (CNS)

20001

x-cp20001

TCA 台湾

20002

x-Chinese-Eten

繁体中文 (Eten)

20003

x-cp20003

IBM5550 台湾

20004

x-cp20004

TeleText 台湾

20005

x-cp20005

Wang 台湾

20105

x-IA5

西欧字符 (IA5)

20106

x-IA5-German

德语 (IA5)

20107

x-IA5-Swedish

瑞典语 (IA5)

20108

x-IA5-Norwegian

挪威语 (IA5)

20127

us-ascii

US-ASCII

*

20261

x-cp20261

T.61

20269

x-cp20269

ISO-6937

20273

IBM273

IBM EBCDIC（德国）

20277

IBM277

IBM EBCDIC（丹麦 - 挪威）

20278

IBM278

IBM EBCDIC（芬兰 - 瑞典）

20280

IBM280

IBM EBCDIC（意大利）

20284

IBM284

IBM EBCDIC（西班牙）

20285

IBM285

IBM EBCDIC（英国）

20290

IBM290

IBM EBCDIC（日语片假名）

20297

IBM297

IBM EBCDIC（法国）

20420

IBM420

IBM EBCDIC（阿拉伯语）

20423

IBM423

IBM EBCDIC（希腊语）

20424

IBM424

IBM EBCDIC（希伯来语）

20833

x-EBCDIC-KoreanExtended

IBM EBCDIC（朝鲜语扩展）

20838

IBM-Thai

IBM EBCDIC（泰语）

20866

koi8-r

西里尔字符 (KOI8-R)

20871

IBM871

IBM EBCDIC（冰岛语）

20880

IBM880

IBM EBCDIC（西里尔俄语）

20905

IBM905

IBM EBCDIC（土耳其语）

20924

IBM00924

IBM 拉丁语 1

20932

EUC-JP

日语（JIS 0208-1990 和 0212-1990）

20936

x-cp20936

简体中文 (GB2312-80)

*

20949

x-cp20949

朝鲜语 Wansung

*

21025

cp1025

IBM EBCDIC（西里尔塞尔维亚 - 保加利亚语）

21866

koi8-u

西里尔字符 (KOI8-U)

28591

iso-8859-1

西欧字符 (ISO)

*

28592

iso-8859-2

中欧字符 (ISO)

28593

iso-8859-3

拉丁语 3 (ISO)

28594

iso-8859-4

波罗的海字符 (ISO)

28595

iso-8859-5

西里尔字符 (ISO)

28596

iso-8859-6

阿拉伯字符 (ISO)

28597

iso-8859-7

希腊字符 (ISO)

28598

iso-8859-8

希伯来字符 (ISO-Visual)

*

28599

iso-8859-9

土耳其字符 (ISO)

28603

iso-8859-13

爱沙尼亚语 (ISO)

28605

iso-8859-15

拉丁语 9 (ISO)

29001

x-Europa

欧罗巴

38598

iso-8859-8-i

希伯来字符 (ISO-Logical)

*

50220

iso-2022-jp

日语 (JIS)

*

50221

csISO2022JP

日语（JIS- 允许 1 字节假名）

*

50222

iso-2022-jp

日语（JIS- 允许 1 字节假名 - SO/SI）

*

50225

iso-2022-kr

朝鲜语 (ISO)

*

50227

x-cp50227

简体中文 (ISO-2022)

*

51932

euc-jp

日语 (EUC)

*

51936

EUC-CN

简体中文 (EUC)

*

51949

euc-kr

朝鲜语 (EUC)

*

52936

hz-gb-2312

简体中文 (HZ)

*

54936

GB18030

简体中文 (GB18030)

*

57002

x-iscii-de

ISCII 梵文

*

57003

x-iscii-be

ISCII 孟加拉语

*

57004

x-iscii-ta

ISCII 泰米尔语

*

57005

x-iscii-te

ISCII 泰卢固语

*

57006

x-iscii-as

ISCII 阿萨姆语

*

57007

x-iscii-or

ISCII 奥里雅语

*

57008

x-iscii-ka

ISCII 卡纳达语

*

57009

x-iscii-ma

ISCII 马拉雅拉姆语

*

57010

x-iscii-gu

ISCII 古吉拉特语

*

57011

x-iscii-pa

ISCII 旁遮普语

*

65000

utf-7

Unicode (UTF-7)

*

65001

utf-8

Unicode (UTF-8)

*

65005

utf-32

Unicode (UTF-32)

*

65006

utf-32BE

Unicode (UTF-32 Big-Endian)

               Encoding e1 = Encoding.GetEncoding( 12000 );

      // Get a UTF-32 encoding by name.
      Encoding e2 = Encoding.GetEncoding( "utf-32" );

      // Check their equality.
      System.Diagnostics.Debug.WriteLine( "e1 equals e2? {0}", e1.Equals( e2 ) );
         */
        /****************************************
          * 函数名称：ReadFile
          * 功能说明：读取文本内容
          * 参     数：Path:文件路径
          * 调用示列：
          *            string Path = Server.MapPath("Default2.aspx");       
          *            string s = EC.FileObj.ReadFile(Path);
         *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path, Encoding encode)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, encode);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }


        #endregion


        #region 追加文件
        /****************************************
          * 函数名称：FileAdd
          * 功能说明：追加文件内容
          * 参     数：Path:文件路径,strings:内容
          * 调用示列：
          *            string Path = Server.MapPath("Default2.aspx");     
          *            string Strings = "新追加内容";
          *            EC.FileObj.FileAdd(Path, Strings);
         *****************************************/
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
        }
        #endregion



        /// <summary>
        /// 路径分割符
        /// </summary>
        private const string PATH_SPLIT_CHAR = "\\";

        /// <summary>
        /// 构造函数
        /// </summary>
        private FileDirectoryUtility()
        {
        }



        //sSrcPath   :源文件夹       D:\\test 
        //sDescPath:目标文件夹   E:\\temp 
        public static void CopyDirectory(string sSrcPath, string sDescPath)
        {
            try
            {
                if (System.IO.Directory.Exists(sSrcPath))
                {
                    if (!System.IO.Directory.Exists(sDescPath))
                        System.IO.Directory.CreateDirectory(sDescPath);
                }
                else
                {
                    return;
                }

                string[] sSrcDSet = System.IO.Directory.GetDirectories(sSrcPath);
                foreach (string sSrcD in sSrcDSet)
                {
                    string sD = sSrcD;
                    sD = sD.Replace(sSrcPath, sDescPath);
                    CopyDirectory(sSrcD, sD);
                }

                string[] sSrcFSet = System.IO.Directory.GetFiles(sSrcPath);
                foreach (string sSrcF in sSrcFSet)
                {
                    string sF = sSrcF;
                    sF = sF.Replace(sSrcPath, sDescPath);
                    System.IO.File.Copy(sSrcF, sF, true);
                }
            }
            catch (ArgumentNullException)
            {
                System.Diagnostics.Debug.WriteLine("Path   is   a   null   reference. ");
            }
            catch (System.Security.SecurityException)
            {
                System.Diagnostics.Debug.WriteLine("The   caller   does   not   have   the   required   permission. ");
            }
            catch (ArgumentException)
            {
                System.Diagnostics.Debug.WriteLine("Path   is   an   empty   string,   contains   only   white   spaces,   or   contains   invalid   characters. ");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("The   path   encapsulated   in   the   Directory   object   does   not   exist. ");
            }
        }


        /// <summary>
        /// 复制指定目录的所有文件,不包含子目录及子目录中的文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,表示覆盖同名文件,否则不覆盖</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite)
        {
            CopyFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        /// 复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //复制当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }
            //复制子目录
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    CopyFiles(sourceSubDir, targetSubDir, overWrite, true);
                }
            }
        }

        /// <summary>
        /// 剪切指定目录的所有文件,不包含子目录
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite)
        {
            MoveFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        /// 剪切指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="moveSubDir">如果为true,包含目录,否则不包含</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite, bool moveSubDir)
        {
            //移动当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Delete(targetFileName);
                        File.Move(sourceFileName, targetFileName);
                    }
                }
                else
                {
                    File.Move(sourceFileName, targetFileName);
                }
            }
            if (moveSubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    MoveFiles(sourceSubDir, targetSubDir, overWrite, true);
                    Directory.Delete(sourceSubDir);
                }
            }
        }

        /// <summary>
        /// 删除指定目录的所有文件，不包含子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        public static void DeleteFiles(string targetDir)
        {
            DeleteFiles(targetDir, false);
        }

        /// <summary>
        /// 删除指定目录的所有文件和子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        /// <param name="delSubDir">如果为true,包含对子目录的操作</param>
        public static void DeleteFiles(string targetDir, bool delSubDir)
        {
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }
            if (delSubDir)
            {
                DirectoryInfo dir = new DirectoryInfo(targetDir);
                foreach (DirectoryInfo subDi in dir.GetDirectories())
                {
                    DeleteFiles(subDi.FullName, true);
                    subDi.Delete();
                }
            }
        }

        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }

        /// <summary>
        /// 建立子目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        /// <param name="subDirName">子目录名称</param>
        public static void CreateDirectory(string parentDir, string subDirName)
        {
            CreateDirectory(parentDir + PATH_SPLIT_CHAR + subDirName);
        }

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteDirectory(string targetDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(targetDir);
            if (dirInfo.Exists)
            {
                DeleteFiles(targetDir, true);
                dirInfo.Delete(true);
            }
        }

        /// <summary>
        /// 删除指定目录的所有子目录,不包括对当前目录文件的删除
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteSubDirectory(string targetDir)
        {
            foreach (string subDir in Directory.GetDirectories(targetDir))
            {
                DeleteDirectory(subDir);
            }
        }

        /// <summary>
        /// 将指定目录下的子目录和文件生成xml文档
        /// </summary>
        /// <param name="targetDir">根目录</param>
        /// <returns>返回XmlDocument对象</returns>
        public static XmlDocument CreateXml(string targetDir)
        {
            XmlDocument myDocument = new XmlDocument();
            XmlDeclaration declaration = myDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            myDocument.AppendChild(declaration);
            XmlElement rootElement = myDocument.CreateElement(targetDir.Substring(targetDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));
            myDocument.AppendChild(rootElement);
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("File");
                childElement.InnerText = fileName.Substring(fileName.LastIndexOf(PATH_SPLIT_CHAR) + 1);
                rootElement.AppendChild(childElement);
            }
            foreach (string directory in Directory.GetDirectories(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Directory");
                childElement.SetAttribute("Name", directory.Substring(directory.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                rootElement.AppendChild(childElement);
                CreateBranch(directory, childElement, myDocument);
            }
            return myDocument;
        }

        /// <summary>
        /// 生成Xml分支
        /// </summary>
        /// <param name="targetDir">子目录</param>
        /// <param name="xmlNode">父目录XmlDocument</param>
        /// <param name="myDocument">XmlDocument对象</param>
        private static void CreateBranch(string targetDir, XmlElement xmlNode, XmlDocument myDocument)
        {
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("File");
                childElement.InnerText = fileName.Substring(fileName.LastIndexOf(PATH_SPLIT_CHAR) + 1);
                xmlNode.AppendChild(childElement);
            }
            foreach (string directory in Directory.GetDirectories(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Directory");
                childElement.SetAttribute("Name", directory.Substring(directory.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                xmlNode.AppendChild(childElement);
                CreateBranch(directory, childElement, myDocument);
            }
        }


        /// <summary>
        /// 判断两个文件内容是否一致
        /// </summary>
        /// <param name="fileName1"></param>
        /// <param name="fileName2"></param>
        /// <returns></returns>
        public static bool IsFilesEqual(string fileName1, string fileName2)
        {
            using (HashAlgorithm hashAlg = HashAlgorithm.Create())
            {
                using (FileStream fs1 = new FileStream(fileName1, FileMode.Open), fs2 = new FileStream(fileName2, FileMode.Open))
                {
                    byte[] hashBytes1 = hashAlg.ComputeHash(fs1);
                    byte[] hashBytes2 = hashAlg.ComputeHash(fs2);

                    // 比较哈希码
                    return (BitConverter.ToString(hashBytes1) == BitConverter.ToString(hashBytes2));
                }
            }
        }



        /// <summary>
        ///  Function:  删除指定目录中的所有文件及其子目录（递归方式）
        ///  Author:     白云一缕
        /// </summary>
        /// <param name="strDirector"></param>
        public static void DeleteAllSubFileAndSubDirectory(string strDirector)
        {
            if (!Directory.Exists(strDirector))
            {
                //文件不存在，退出
                return;
            }

            //删除文件
            string[] strFiles = Directory.GetFiles(strDirector);
            if (strFiles != null && strFiles.Length > 0)
            {
                foreach (string str in strFiles)
                {
                    File.Delete(str);
                }
            }

            //删除目录
            string[] strDirectories = Directory.GetDirectories(strDirector);
            if (strDirectories != null && strDirectories.Length > 0)
            {
                foreach (string str in strDirectories)
                {
                    DeleteAllSubFileAndSubDirectory(str);
                }
            }
            Directory.Delete(strDirector);
        }


    }
}