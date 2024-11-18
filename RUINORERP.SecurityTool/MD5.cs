using System;
using System.Text;

namespace SecurityCore
{
 /**//// <summary>
 /// Summary description for MD5.
 /// </summary>
  class MD5
 {
  const int BITS_TO_A_BYTE = 8;
  const int BYTES_TO_A_WORD = 4;
  const int BITS_TO_A_WORD = 32; 
  private static long[] m_lOnBits = new long[30 + 1];
  private static long[] m_l2Power = new long[30 + 1];

  private static long LShift(long lValue, long iShiftBits)
  {
   long LShift = 0;
   if (iShiftBits == 0)
   {
    LShift = lValue;
    return LShift;
   }
   else
   {
    if( iShiftBits == 31)
    {
     if (Convert.ToBoolean(lValue & 1))
     {
      LShift = 0x80000000;
     }
     else
     {
      LShift = 0;
     }
     return LShift;
    }
    else
    {
     if( iShiftBits < 0 || iShiftBits > 31)
     {
      // Err.Raise 6;
     }
    }
   }

   if (Convert.ToBoolean((lValue & m_l2Power[31 - iShiftBits])))
   {
    LShift = ((lValue & m_lOnBits[31 - (iShiftBits + 1)]) * m_l2Power[iShiftBits]) | 0x80000000;
   }
   else
   {
    LShift = ((lValue & m_lOnBits[31 - iShiftBits]) * m_l2Power[iShiftBits]);
   }

   return LShift;
  }

  private static long RShift(long lValue, long iShiftBits)
  {
   long RShift = 0;
   if (iShiftBits == 0)
   {
    RShift = lValue;
    return RShift;
   }
   else
   {
    if( iShiftBits == 31)
    {
     if (Convert.ToBoolean(lValue & 0x80000000))
     {
      RShift = 1;
     }
     else
     {
      RShift = 0;
     }
     return RShift;
    }
    else
    {
     if( iShiftBits < 0 || iShiftBits > 31)
     {
      // Err.Raise 6;
     }
    }
   }

   RShift = (lValue & 0x7FFFFFFE) / m_l2Power[iShiftBits];

   if (Convert.ToBoolean((lValue & 0x80000000)))
   {
    RShift = (RShift | (0x40000000 / m_l2Power[iShiftBits - 1]));
   }

   return RShift;
  }

  private static long RotateLeft(long lValue, long iShiftBits)
  {
   long RotateLeft = 0;
   RotateLeft = LShift(lValue, iShiftBits) | RShift(lValue, (32 - iShiftBits));
   return RotateLeft;
  }

  private static long AddUnsigned(long lX, long lY)
  {
   long AddUnsigned = 0;
   long lX4 = 0;
   long lY4 = 0;
   long lX8 = 0;
   long lY8 = 0;
   long lResult = 0;

   lX8 = lX & 0x80000000;
   lY8 = lY & 0x80000000;
   lX4 = lX & 0x40000000;
   lY4 = lY & 0x40000000;

   lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
   if (Convert.ToBoolean(lX4 & lY4))
   {
    lResult = lResult ^ 0x80000000 ^ lX8 ^ lY8;
   }
   else if( Convert.ToBoolean(lX4 | lY4))
   {
    if (Convert.ToBoolean(lResult & 0x40000000))
    {
     lResult = lResult ^ 0xC0000000 ^ lX8 ^ lY8;
    }
    else
    {
     lResult = lResult ^ 0x40000000 ^ lX8 ^ lY8;
    }
   }
   else
   {
    lResult = lResult ^ lX8 ^ lY8;
   }
   AddUnsigned = lResult;
   return AddUnsigned;
  }

  private static long md5_F(long x, long y, long z)
  {
   long md5_F = 0;
   md5_F = (x & y) | (( ~x) & z);
   return md5_F;
  }

  private static long md5_G(long x, long y, long z)
  {
   long md5_G = 0;
   md5_G = (x & z) | (y & ( ~z));
   return md5_G;
  }

  private static long md5_H(long x, long y, long z)
  {
   long md5_H = 0;
   md5_H = (x ^ y ^ z);
   return md5_H;
  }

  private static long md5_I(long x, long y, long z)
  {
   long md5_I = 0;
   md5_I = (y ^ (x | (~z)));
   return md5_I;
  }

  private static void md5_FF(ref long a, long b, long c, long d, long x, long s, long ac)
  {
   a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_F(b, c, d), x), ac));
   a = RotateLeft(a, s);
   a = AddUnsigned(a, b);
  }

  private static void md5_GG(ref long a, long b, long c, long d, long x, long s, long ac)
  {
   a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_G(b, c, d), x), ac));
   a = RotateLeft(a, s);
   a = AddUnsigned(a, b);
  }

  private static void md5_HH(ref long a, long b, long c, long d, long x, long s, long ac)
  {
   a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_H(b, c, d), x), ac));
   a = RotateLeft(a, s);
   a = AddUnsigned(a, b);
  }

  private static void md5_II(ref long a, long b, long c, long d, long x, long s, long ac)
  {
   a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_I(b, c, d), x), ac));
   a = RotateLeft(a, s);
   a = AddUnsigned(a, b);
  }

  private static long[] ConvertToWordArray(string sMessage)
  {
   long[] ConvertToWordArray = null;
   int lMessageLength = 0;
   int lNumberOfWords = 0;
   long[] lWordArray = null;
   int lBytePosition = 0;
   int lByteCount = 0;
   int lWordCount = 0;

   const int MODULUS_BITS = 512;
   const int CONGRUENT_BITS = 448;

   lMessageLength = sMessage.Length;
   lNumberOfWords = (((lMessageLength + ((MODULUS_BITS - CONGRUENT_BITS) / BITS_TO_A_BYTE)) / (MODULUS_BITS / BITS_TO_A_BYTE)) + 1) * (MODULUS_BITS / BITS_TO_A_WORD);
   lWordArray = new long[lNumberOfWords];

   lBytePosition = 0;
   lByteCount = 0;

   while(lByteCount < lMessageLength)
   {
    lWordCount = lByteCount / BYTES_TO_A_WORD;
    lBytePosition = (lByteCount % BYTES_TO_A_WORD) * BITS_TO_A_BYTE;
    lWordArray[lWordCount] = lWordArray[lWordCount] | LShift(Convert.ToByte(sMessage.Substring(lByteCount, 1).ToCharArray()[0]), lBytePosition);
    lByteCount = lByteCount + 1;
   }

   lWordCount = lByteCount / BYTES_TO_A_WORD;
   lBytePosition = (lByteCount % BYTES_TO_A_WORD) * BITS_TO_A_BYTE;
   lWordArray[lWordCount] = lWordArray[lWordCount] | LShift(0x80, lBytePosition);
   lWordArray[lNumberOfWords - 2] = LShift(lMessageLength, 3);
   lWordArray[lNumberOfWords - 1] = RShift(lMessageLength, 29);
   ConvertToWordArray = lWordArray;

   return ConvertToWordArray;
  }

  private static string WordToHex(long lValue)
  {
   string WordToHex = "";
   long lByte = 0;
   int lCount = 0;
   for(lCount = 0; lCount <= 3; lCount++)
   {
    lByte = RShift(lValue, lCount * BITS_TO_A_BYTE) & m_lOnBits[BITS_TO_A_BYTE - 1];
    WordToHex = WordToHex + (("0" + ToHex(lByte)).Substring(("0" + ToHex(lByte)).Length - 2));
   }
   return WordToHex;
  }

  private static string ToHex(long dec)
  {
   string strhex = "";
   while(dec > 0)
   {
    strhex = tohex(dec % 16) + strhex;
    dec = dec / 16;
   }
   return strhex;
  }
  
  private static string tohex(long hex)
  {
   string strhex = "";
   switch(hex)
   {
    case 10: strhex = "a"; break;
    case 11: strhex = "b"; break;
    case 12: strhex = "c"; break;
    case 13: strhex = "d"; break;
    case 14: strhex = "e"; break;
    case 15: strhex = "f"; break;
    default : strhex = hex.ToString(); break;
   }
   return strhex;
  }


  public static string Encrypt(string sMessage, int stype)
  {
   string MD5 = "";
   
   for(int i=0; i<=30; i++)
   {
    m_lOnBits[i] = Convert.ToInt64(Math.Pow(2, i+1) -1);
    m_l2Power[i] = Convert.ToInt64(Math.Pow(2, i));
   }

   long[] x = null;
   int k = 0;
   long AA = 0;
   long BB = 0;
   long CC = 0;
   long DD = 0;
   long a = 0;
   long b = 0;
   long c = 0;
   long d = 0;

   const int S11 = 7;
   const int S12 = 12;
   const int S13 = 17;
   const int S14 = 22;
   const int S21 = 5;
   const int S22 = 9;
   const int S23 = 14;
   const int S24 = 20;
   const int S31 = 4;
   const int S32 = 11;
   const int S33 = 16;
   const int S34 = 23;
   const int S41 = 6;
   const int S42 = 10;
   const int S43 = 15;
   const int S44 = 21;

   x = ConvertToWordArray(sMessage);

   a = 0x67452301;
   b = 0xEFCDAB89;
   c = 0x98BADCFE;
   d = 0x10325476;

   for(k = 0; k < x.Length; k += 16)
   {
    AA = a;
    BB = b;
    CC = c;
    DD = d;

    md5_FF(ref a, b, c, d, x[k + 0], S11, 0xD76AA478);
    md5_FF(ref d, a, b, c, x[k + 1], S12, 0xE8C7B756);
    md5_FF(ref c, d, a, b, x[k + 2], S13, 0x242070DB);
    md5_FF(ref b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
    md5_FF(ref a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
    md5_FF(ref d, a, b, c, x[k + 5], S12, 0x4787C62A);
    md5_FF(ref c, d, a, b, x[k + 6], S13, 0xA8304613);
    md5_FF(ref b, c, d, a, x[k + 7], S14, 0xFD469501);
    md5_FF(ref a, b, c, d, x[k + 8], S11, 0x698098D8);
    md5_FF(ref d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
    md5_FF(ref c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
    md5_FF(ref b, c, d, a, x[k + 11], S14, 0x895CD7BE);
    md5_FF(ref a, b, c, d, x[k + 12], S11, 0x6B901122);
    md5_FF(ref d, a, b, c, x[k + 13], S12, 0xFD987193);
    md5_FF(ref c, d, a, b, x[k + 14], S13, 0xA679438E);
    md5_FF(ref b, c, d, a, x[k + 15], S14, 0x49B40821);
    md5_GG(ref a, b, c, d, x[k + 1], S21, 0xF61E2562);
    md5_GG(ref d, a, b, c, x[k + 6], S22, 0xC040B340);
    md5_GG(ref c, d, a, b, x[k + 11], S23, 0x265E5A51);
    md5_GG(ref b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
    md5_GG(ref a, b, c, d, x[k + 5], S21, 0xD62F105D);
    md5_GG(ref d, a, b, c, x[k + 10], S22, 0x2441453);
    md5_GG(ref c, d, a, b, x[k + 15], S23, 0xD8A1E681);
    md5_GG(ref b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
    md5_GG(ref a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
    md5_GG(ref d, a, b, c, x[k + 14], S22, 0xC33707D6);
    md5_GG(ref c, d, a, b, x[k + 3], S23, 0xF4D50D87);
    md5_GG(ref b, c, d, a, x[k + 8], S24, 0x455A14ED);
    md5_GG(ref a, b, c, d, x[k + 13], S21, 0xA9E3E905);
    md5_GG(ref d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
    md5_GG(ref c, d, a, b, x[k + 7], S23, 0x676F02D9);
    md5_GG(ref b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
    md5_HH(ref a, b, c, d, x[k + 5], S31, 0xFFFA3942);
    md5_HH(ref d, a, b, c, x[k + 8], S32, 0x8771F681);
    md5_HH(ref c, d, a, b, x[k + 11], S33, 0x6D9D6122);
    md5_HH(ref b, c, d, a, x[k + 14], S34, 0xFDE5380C);
    md5_HH(ref a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
    md5_HH(ref d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
    md5_HH(ref c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
    md5_HH(ref b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
    md5_HH(ref a, b, c, d, x[k + 13], S31, 0x289B7EC6);
    md5_HH(ref d, a, b, c, x[k + 0], S32, 0xEAA127FA);
    md5_HH(ref c, d, a, b, x[k + 3], S33, 0xD4EF3085);
    md5_HH(ref b, c, d, a, x[k + 6], S34, 0x4881D05);
    md5_HH(ref a, b, c, d, x[k + 9], S31, 0xD9D4D039);
    md5_HH(ref d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
    md5_HH(ref c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
    md5_HH(ref b, c, d, a, x[k + 2], S34, 0xC4AC5665);
    md5_II(ref a, b, c, d, x[k + 0], S41, 0xF4292244);
    md5_II(ref d, a, b, c, x[k + 7], S42, 0x432AFF97);
    md5_II(ref c, d, a, b, x[k + 14], S43, 0xAB9423A7);
    md5_II(ref b, c, d, a, x[k + 5], S44, 0xFC93A039);
    md5_II(ref a, b, c, d, x[k + 12], S41, 0x655B59C3);
    md5_II(ref d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
    md5_II(ref c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
    md5_II(ref b, c, d, a, x[k + 1], S44, 0x85845DD1);
    md5_II(ref a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
    md5_II(ref d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
    md5_II(ref c, d, a, b, x[k + 6], S43, 0xA3014314);
    md5_II(ref b, c, d, a, x[k + 13], S44, 0x4E0811A1);
    md5_II(ref a, b, c, d, x[k + 4], S41, 0xF7537E82);
    md5_II(ref d, a, b, c, x[k + 11], S42, 0xBD3AF235);
    md5_II(ref c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
    md5_II(ref b, c, d, a, x[k + 9], S44, 0xEB86D391);

    a = AddUnsigned(a, AA);
    b = AddUnsigned(b, BB);
    c = AddUnsigned(c, CC);
    d = AddUnsigned(d, DD);
   }

   if (stype == 32)
   {
    MD5 = ((((WordToHex(a)) + (WordToHex(b))) + (WordToHex(c))) + (WordToHex(d))).ToLower();
   }
   else
   {
    MD5 = ((WordToHex(b)) + (WordToHex(c))).ToLower();
   }
//   MD5=MD5.Replace("0", "a");
//   MD5=MD5.Replace("1", "b");
//   MD5=MD5.Replace("2", "cs");
//   MD5=MD5.Replace("3", "d");
//   MD5=MD5.Replace("4", "e");
//   MD5=MD5.Replace("5", "f");
//   MD5=MD5.Replace("6", "k");
//   MD5=MD5.Replace("7", "s");
//
//   MD5=MD5.Replace("8", "r");
//   MD5=MD5.Replace("9", "ip");
//   MD5=MD5.Replace("j", "8");
//   MD5=MD5.Replace("o", "3");
//
//   MD5=MD5.Replace("a", "04");
//   MD5=MD5.Replace("m", "3");
//   MD5=MD5.Replace("x", "67");
//   MD5=MD5.Replace("p", "23");
//   MD5=MD5.Replace("g", "7");

//自已再加上个尾缀别人根本解不出来

   return MD5;
  }
 }
}

////倒序加1与顺序减1

//public static string DecryptStr(string rs) //顺序减1解码
//  {
//   byte[] by=new byte[rs.Length];
//   for(int i=0;i<=rs.Length-1;i++)
//   {
//    by[i]=(byte)((byte)rs[i]-1);
//   }
//   rs="";
//   for(int i=by.Length-1;i>=0;i--)
//   {
//    rs+=((char)by[i]).ToString();
//   }
//   return rs;
//  }

//public static string EncryptStr(string rs) //倒序加1加密
//  {
//   byte[] by=new byte[rs.Length];
//   for(int i=0;i<=rs.Length-1;i++)
//   {
//    by[i]=(byte)((byte)rs[i]+1);
//   }
//   rs="";
//   for(int i=by.Length-1;i>=0;i--)
//   {
//    rs+=((char)by[i]).ToString();
//   }
//   return rs;
//  }
////(3)又一种不同的密钥

//public static string EncryptMethod(string rs) //加密
//  {
//   byte[] desKey = new  byte[]{0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08};
//   byte[] desIV = new  byte[]{0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08};

//   DESCryptoServiceProvider des = new DESCryptoServiceProvider();
//   try
//   {
//    byte[] inputByteArray = Encoding.Default.GetBytes(rs);
//    //byte[] inputByteArray=Encoding.Unicode.GetBytes(rs);

//    des.Key = desKey;  // ASCIIEncoding.ASCII.GetBytes(sKey);
//    des.IV = desIV;   //ASCIIEncoding.ASCII.GetBytes(sKey);
//    MemoryStream ms = new MemoryStream();
//    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(),
//     CryptoStreamMode.Write);
//    //Write the byte array into the crypto stream
//    //(It will end up in the memory stream)
//    cs.Write(inputByteArray, 0, inputByteArray.Length);
//    cs.FlushFinalBlock();

//    //Get the data back from the memory stream, and into a string
//    StringBuilder ret = new StringBuilder();
//    foreach(byte b in ms.ToArray())
//    {
//     //Format as hex
//     ret.AppendFormat("{0:X2}", b);
//    }
//    ret.ToString();
//    return ret.ToString();
//   }
//   catch
//   {
//    return rs;
//   }
//   finally
//   {
//    des = null;
//   }
//  }
//public static string DecryptMethod(string rs)    //解密
//  {
//   byte[] desKey = new  byte[]{0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08};
//   byte[] desIV = new  byte[]{0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08};


//   DESCryptoServiceProvider des = new DESCryptoServiceProvider();
//   try
//   {
//    //Put the input string into the byte array
//    byte[] inputByteArray = new byte[rs.Length / 2];
//    for(int x = 0; x < rs.Length / 2; x++)
//    {
//     int i = (Convert.ToInt32(rs.Substring(x * 2, 2), 16));
//     inputByteArray[x] = (byte)i;
//    }

//    des.Key = desKey;   //ASCIIEncoding.ASCII.GetBytes(sKey);
//    des.IV = desIV;    //ASCIIEncoding.ASCII.GetBytes(sKey);
//    MemoryStream ms = new MemoryStream();
//    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(),CryptoStreamMode.Write);
//    //Flush the data through the crypto stream into the memory stream
//    cs.Write(inputByteArray, 0, inputByteArray.Length);
//    cs.FlushFinalBlock();

//    //Get the decrypted data back from the memory stream
//    StringBuilder ret = new StringBuilder();
 
//    return System.Text.Encoding.Default.GetString(ms.ToArray());
//   }
//   catch
//   {
//    return rs; 
//   }
//   finally
//   {
//    des = null;
//   }
//  }

////有些加密的还可以起到别的作用，如上次我在PDA上要post图片和一些说明文字到web服务器上中文的处理就是用了一些小巧的加密算法，因为.net的mobile上不支持一些特殊的算法（支持MD5但MD5不可解密的）上面写的两种都不支持所以只好另写：
////private string aa(string bb)
////  {

////   byte[] by=new byte[bb.Length];
////   by=System.Text.Encoding.UTF8.GetBytes(bb);
   
////   string r=Convert.ToBase64String(by);
////   return r;
////  }

////private string bb(string aa)
////  {
////   byte[] by=Convert.FromBase64String(aa);  
////   string r=Encoding.UTF8.GetString(by);
////   return r;
////  }

