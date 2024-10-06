using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Memory;
namespace RUINORERP.IIS.Comm
{
    public partial class Utils
    {
        public static string HideCell(string cell)
        {
            try
            {
                if (!string.IsNullOrEmpty(cell)) return "";
                return Regex.Replace(cell, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
            }
            catch
            {
                return "";
            }
        }

        public static string HideCertNo(string certNo)
        {
            if (string.IsNullOrEmpty(certNo) || (certNo.Length != 15 && certNo.Length != 18)) return "";
            var begin = certNo[..6];
            var end = certNo[14..];
            return begin + "********" + end;
        }

        public static string HideName(string certName)
        {
            if (string.IsNullOrWhiteSpace(certName)) return "";

            if (certName.Length == 1) return certName;
            if (certName.Length == 2) return string.Concat(certName.AsSpan(0, 1), "*");

            var begin = certName.Substring(0, 1);
            var end = certName.Substring(certName.Length - 1);

            return begin + "*" + end;
        }

    }
}