using System;
using System.IO;
using System.Runtime.Serialization;

namespace SourceLibrary.IO
{
	/// <summary>
	/// A class that helps save and load with stream persistence
	/// </summary>
	public class StreamPersistence
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected StreamPersistence()
		{
		}

		#region Write Function
		public static void Write(Stream p_Stream, string p_Value)
		{
			if (p_Value!=null)
			{
				Write(p_Stream, p_Value.Length);
				for (int i = 0; i < p_Value.Length; i++)
					Write(p_Stream, p_Value[i]);
			}
			else
				Write(p_Stream, (int)0);
		}
		public static void Write(Stream p_Stream, int p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, long p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, double p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, float p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, uint p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, ulong p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		//		public static void Write(Stream p_Stream, decimal p_Value)
		//		{
		//			Write(p_Stream, BitConverter.GetBytes(p_Value));
		//		}
		public static void Write(Stream p_Stream, char p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, short p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, bool p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, ushort p_Value)
		{
			Write(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, byte[] p_Bytes)
		{
			p_Stream.Write(p_Bytes,0,p_Bytes.Length);
		}
		#endregion

		#region Read Function
		public static void Read(Stream p_Stream, out double p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((double)0.0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToDouble(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out int p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((int)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToInt32(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out float p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((float)0.0f);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToSingle(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out long p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((long)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToInt64(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out uint p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((uint)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToUInt32(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out ulong p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((ulong)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToUInt64(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out short p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((short)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToInt16(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out ushort p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((ushort)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToUInt16(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out char p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((char)0);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToChar(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out bool p_Value)
		{
			byte[] l_tmp = BitConverter.GetBytes((bool)false);
			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
				throw new InvalidDataException();
			p_Value = BitConverter.ToBoolean(l_tmp,0);
		}
		public static void Read(Stream p_Stream, out string p_Value)
		{
			int l_Length;
			Read(p_Stream, out l_Length);
			if (l_Length > 0)
			{
				char[] l_tmp = new char[l_Length];
				for (int i = 0; i < l_Length; i++)
				{
					char c;
					Read(p_Stream, out c);
					l_tmp[i] = c;
				}
				p_Value = new string(l_tmp);
			}
			else
				p_Value = null;
		}
		public static void Read(Stream p_Stream, byte[] p_Value)
		{
			if (p_Stream.Read(p_Value,0,p_Value.Length) != p_Value.Length)
				throw new InvalidDataException();
		}
		//		public static void Read(Stream p_Stream, out decimal p_Value)
		//		{
		//			byte[] l_tmp = BitConverter.GetBytes((decimal)0.0);
		//			if (p_Stream.Read(l_tmp,0,l_tmp.Length) != l_tmp.Length)
		//				throw new InvalidDataException();
		//			p_Value = BitConverter.Todecimal(l_tmp,0);
		//		}
		#endregion
	}


	[Serializable]
	public class InvalidDataException : ApplicationException  
	{
		public InvalidDataException():
			base("Invalid data exception")
		{
		}

		public InvalidDataException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public InvalidDataException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
		protected InvalidDataException(SerializationInfo p_Info, StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
	}
}
