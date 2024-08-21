using System;
using System.Drawing;
namespace RUINORERP.Common.Helper
{
	/// <summary>
	/// stringtools 的摘要说明。
	/// </summary>
	public class stringtools
	{
		public stringtools()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		public static bool startwithnumeric(string s)
		{
			if (s==null||s.Length==0) return false;
			char ch=s[0];
			if (ch>='0' && ch<='9') return true;
			return false;
		}
		public static bool stringinarray(string s,string[] arr)
		{
			if (arr==null) return false;
			for (int i=0;i<arr.Length;i++)
			{
				if (arr[i]==s) return true;
			}
			return false;
		}
		public static string noxml(string s)
		{
			s=s.Replace(">","&gt;");
			s=s.Replace("<","&lt;");
			s=s.Replace("&","&amp");
			return s;
		}
		public static string restoremxl(string s)
		{
			s=s.Replace("&gt;",">");
			s=s.Replace("&lt;","<");
			s=s.Replace("&amp","&");
			return s;

		}
		public static string SizeToString(Size size)
		{
			return size.Width.ToString()+":"+size.Height.ToString();
		}
		public static Size StringToSize(string s)
		{
			string s1=partof(s,':',0);
			string s2=partof(s,':',1);
			return new Size(Convert.ToInt32(s1),Convert.ToInt32(s2));
		}
		public static string removeendpart(string s,string px)
		{
			int index=s.LastIndexOf(px);
			if (index==-1) return s;

			return s.Substring(0,index);
		}
		public static string removeend(string s,string px)
		{
			int d=px.Length;
			return s.Substring(0,s.Length-d);
		}
		public static string partbetween(string s,string start,string end)
		{
			int index=s.IndexOf(start);
			if (index==-1) index=0;
			else index+=start.Length;
			int index2=s.Substring(index).IndexOf(end);
			if (index2==-1) return s.Substring(index);
			else return s.Substring(index).Substring(0,index2);
		}
		public static string removelastchar(string s,char ch)
		{
			if (s[s.Length-1]==ch) return s.Substring(0,s.Length-1);
			return s;
		}
		public static string lastrightpart(string s,string sep)
		{
			int index=s.LastIndexOf(sep);
			if (index==-1) return null;
			
			else return s.Substring(index+sep.Length);
		}
		public static string lastrightpart(string s,char ch)
		{
			int index=s.LastIndexOf(ch);
			if (index==-1) return s;
			else return s.Substring(index+1);
		}
		public static string firstpart(string s,char ch)
		{
			int index=s.IndexOf(ch);
			if (index==-1) return s;
			else return s.Substring(0,index);

		}
		public static string secondpart(string s,char ch)
		{
			if (s!=null)
			{
			string[] t=s.Split(ch);
				return t[1];} else return null;
		}
		public static string partof(string s,char ch,int index)
		{
			if (s!=null) 
			{
				string[] t=s.Split(ch);
				return t[index];
			}else return null;
		}
		public static string partafter(string s,char ch)
		{
			if (s==null) return null;
			int index=s.IndexOf(ch);
			if (index==-1) return s;
			else return s.Substring(index+1);
		}
		public static string[] partsplit(string s,char ch,int n)
		{
			
			if (s==null) return null;
			string[] items=new string[n];
			int i=0;
			int start=0;
			while(i<n)
			{
				if (i==n-1) 
				{
					items[i]=s.Substring(start);

				}
				else 
				{
					int index=s.IndexOf(ch,start);
					if (index==-1) 
					{
							items[i]=s.Substring(start);
						return items;
					}
					else 
						items[i]=s.Substring(start,index-start);
					start=index+1;
				}
				i++;
			}
			return items;
		}
		public static string initvalue(string define,string name)
		{
	
			string[] defs=define.Split(',');
			for (int i=0;i<defs.Length;i++)
			{
				string s=defs[i];
				//查询 字符串是否是name=xxx各式的

				string l=firstpart(s,'=');
				if (l==name) return secondpart(s,'=');

			}
			return null;
		}


		//public static string props(object obj,string define,char sep)
		//{
		//   string[] temp=define.Split('&');
		//	string x="";
		//	for (int i=0;i<temp.Length;i++)
		//	{
		//		if (x!="") x+=sep;
		//		x+=Convert.ToString(cenetcom.util.refutil.tools.propfieldvalue(obj,temp[i]));
		//	}
		//	return x;
		//}
	}
}
