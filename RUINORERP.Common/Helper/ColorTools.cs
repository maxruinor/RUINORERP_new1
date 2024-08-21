using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{
   
		public class ColorTools
	{
 
			public static Color Mix(Color c1, Color c2, float percent)
			{
				return Color.FromArgb(Convert.ToInt16((c1.R * percent + c2.R * (1 - percent))), Convert.ToInt16((c1.G * percent + c2.G * (1 - percent))), Convert.ToInt16((c1.B * percent + c2.B * (1 - percent))));
			}
			static Random r = new Random();
			public static Color RandomColor()
			{

				return Color.FromArgb(colorr(r), colorr(r), colorr(r));

			}
			public static Color GetColorFromXml(string cstr)
			{

				string[] colors = cstr.Split(',');

				Color LineColor = Color.FromArgb(Convert.ToInt32(colors[0]), Convert.ToInt32(colors[1]), Convert.ToInt32(colors[2]));
				return LineColor;

			}
			public static Color GetColorFromWeb(string cstr)
			{
				try
				{
					cstr = cstr.Substring(1);

					return Color.FromArgb(HexToInt(cstr.Substring(0, 2)), HexToInt(cstr.Substring(2, 2)), HexToInt(cstr.Substring(4, 2)));
				}
				catch (Exception)
				{
					return Color.Transparent;
				}
			}
			public static int HexToInt(string hex)
			{
				hex = hex.ToUpper();
				int a = 0;
				for (int i = 0; i < hex.Length; i++)
				{
					a *= 16;
					char c = hex[i];
					if (c < '9') a += c - '0';
					else a += c - 'A' + 10;
				}
				return a;
			}

			public static int colorr(Random r)
			{
				return (r.Next() % 255);
			}
		}
	}

