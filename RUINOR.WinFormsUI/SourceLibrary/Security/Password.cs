using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace SourceLibrary.Security
{
	/// <summary>
	/// Utilities for password management.
	/// </summary>
	public class Password
	{
		/// <summary>
		/// Password utilities using SH1 alghoritm
		/// </summary>
		public class SHA1
		{
			/// <summary>
			/// Hash the string p_Password using SH1 alghoritm (SHA1CryptoServiceProvider). 
			/// </summary>
			/// <param name="p_Password"></param>
			/// <returns></returns>
			public static string HashPassword(string p_Password)
			{
				SHA1CryptoServiceProvider l_shaProvider = new SHA1CryptoServiceProvider(); 

				byte[] data = Encoding.UTF8.GetBytes(p_Password);

				byte[] result = l_shaProvider.ComputeHash(data);

				return Convert.ToBase64String(result);
			}
		}
	}
}
