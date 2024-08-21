using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class BarCodeCreator
    {
        public static string CreateProductBarCode(string cageID,bool IsOwner, string ProdDecimalCode)
        {
            //年月302 外采/自产0/1 类目编码888+助记码5个
            Ean13 ean13 = new Ean13();
            ean13.CountryCode = System.DateTime.Now.Year.ToString().Substring(3)+ DateTime.Now.ToString("MM"); 
            if (IsOwner)
            {
                ean13.ManufacturerCode = "1";//
            }
            else
            {
                ean13.ManufacturerCode = "0";//
            }
            ean13.ProductCode = "88812345";
            ean13.Scale = (float)Convert.ToDecimal(1.5f);
            ean13.CalculateChecksumDigit();
            return ean13.ToString();
        }

    }
}
