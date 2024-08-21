using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;


namespace SourceLibrary.Utility
{
	public class Serialization
	{
		#region Serialization Code
		public static object BinDeserialize(Stream p_Stream)
		{
			BinaryFormatter f = new BinaryFormatter();
			object tmp;
			tmp = f.Deserialize(p_Stream);
			return tmp;
		}

		public static void BinSerialize(Stream p_Stream, object p_Object)
		{
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(p_Stream,p_Object);
		}

		public static object BinDeserialize(string p_strFileName)
		{
			object tmp;
			using (FileStream l_Stream = new FileStream(p_strFileName,FileMode.Open,FileAccess.Read))
			{
				tmp = BinDeserialize(l_Stream);
				l_Stream.Close();
			}
			return tmp;
		}

		public static void BinSerialize(string p_strFileName, object p_Object)
		{
			using (FileStream l_Stream = new FileStream(p_strFileName,FileMode.Create,FileAccess.Write))
			{
				BinSerialize(l_Stream,p_Object);
				l_Stream.Close();
			}
		}

		#endregion
	}

	public class Shell
	{
		public static void OpenFile(string p_File)
		{
			ExecCommand(p_File);
		}

		public static void ExecCommand(string p_Command)
		{
			System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(p_Command);
			p.UseShellExecute = true;
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo = p;
			process.Start();
		}
	}
}
