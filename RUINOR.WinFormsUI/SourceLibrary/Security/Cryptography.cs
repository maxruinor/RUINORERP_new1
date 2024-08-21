using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace SourceLibrary.Security
{
	public class Cryptography
	{
		public class DES
		{
			#region Cryptography Code

			/// <summary>
			/// 
			/// </summary>
			/// <param name="p_strInput"></param>
			/// <param name="p_Key8chars">Deve essere lunga 8 caratteri</param>
			/// <returns></returns>
			public static string EncryptString(string p_strInput, string p_Key8chars)
			{
				string tmp;
				using (MemoryStream l_InputStream = new MemoryStream())
				{
					StreamWriter l_InputStreamWriter = new StreamWriter(l_InputStream);
					l_InputStreamWriter.Write(p_strInput);
					l_InputStreamWriter.Flush();
					l_InputStream.Seek(0,SeekOrigin.Begin);

					using (MemoryStream l_OutputStream = new MemoryStream())
					{
						EncryptStream(l_InputStream,l_OutputStream,p_Key8chars);
						l_OutputStream.Flush();
						l_OutputStream.Seek(0,SeekOrigin.Begin);

						tmp = Convert.ToBase64String(l_OutputStream.ToArray());

						l_OutputStream.Close();
					}

					l_InputStreamWriter.Close();
				}

				return tmp;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="p_strInput"></param>
			/// <param name="p_Key8chars">Deve essere lunga 8 caratteri</param>
			/// <returns></returns>
			public static string DecryptString(string p_strInput, string p_Key8chars)
			{
				string tmp;
				using (MemoryStream l_InputStream = new MemoryStream())
				{
					byte[] l_InputArray = Convert.FromBase64String(p_strInput);
					l_InputStream.Write(l_InputArray,0,l_InputArray.Length);
					l_InputStream.Flush();
					l_InputStream.Seek(0,SeekOrigin.Begin);

					using (MemoryStream l_OutputStream = new MemoryStream())
					{
						DecryptStream(l_InputStream,l_OutputStream,p_Key8chars);

						StreamReader l_OutputStreamReader = new StreamReader(l_OutputStream);
						tmp = l_OutputStreamReader.ReadToEnd();

						l_OutputStreamReader.Close();
					}

					l_InputStream.Close();
				}

				return tmp;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="p_StreamInput"></param>
			/// <param name="p_StreamOutput"></param>
			/// <param name="p_Key8chars">Deve essere lunga 8 caratteri</param>
			public static void EncryptStream(Stream p_StreamInput, Stream p_StreamOutput, string p_Key8chars)
			{
				DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();
				DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				DESProvider.IV = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				ICryptoTransform DESEncrypt = DESProvider.CreateEncryptor();

				using (CryptoStream cryptoStream = new CryptoStream(p_StreamOutput, DESEncrypt, CryptoStreamMode.Write))
				{
					byte[] bytearrayinput = new byte[p_StreamInput.Length];
					p_StreamInput.Read(bytearrayinput, 0, bytearrayinput.Length);
					cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
					cryptoStream.FlushFinalBlock();
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="p_StreamInput"></param>
			/// <param name="p_StreamOutput"></param>
			/// <param name="p_Key8chars">Deve essere lunga 8 caratteri</param>
			public static void DecryptStream(Stream p_StreamInput, Stream p_StreamOutput, string p_Key8chars)
			{
				DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();
				DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				DESProvider.IV = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				ICryptoTransform desDecrypt= DESProvider.CreateDecryptor();

				using (CryptoStream cryptostreamDecr = new CryptoStream(p_StreamOutput, desDecrypt, CryptoStreamMode.Write))
				{
					byte[] buffer = new byte[p_StreamInput.Length];
					p_StreamInput.Read(buffer,0,buffer.Length);
					cryptostreamDecr.Write(buffer,0,buffer.Length);
					cryptostreamDecr.FlushFinalBlock();
				}
				p_StreamOutput.Seek(0,SeekOrigin.Begin);
			}
			#endregion
		}
	}


}
