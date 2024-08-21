using System;
using System.Drawing;

namespace SourceLibrary.Drawing
{
	/// <summary>
	/// Drawing utility functions
	/// </summary>
	public class ControlPaint
	{
		private ControlPaint()
		{
		}

		public static Color CalculateTransparentColor(Color front, Color back, int alpha) 
		{
			Color alphaFront;
			Color alphaBack;
			float local2;
			float local3;
			float local4;
			float local5;
			float local6;
			float local7;
			float local8;
			byte local9;
			float alphaBack0;
			byte alphaBack1;
			float alphaBack2;
			byte alphaBack3;

			alphaFront = Color.FromArgb(255, front);
			alphaBack = Color.FromArgb(255, back);
			local2 = (float) alphaFront.R;
			local3 = (float) alphaFront.G;
			local4 = (float) alphaFront.B;
			local5 = (float) alphaBack.R;
			local6 = (float) alphaBack.G;
			local7 = (float) alphaBack.B;
			local8 = local2 * (float) alpha / 255 + local5 * (float) 255 - alpha / 255;
			local9 = (byte) local8;
			alphaBack0 = local3 * (float) alpha / 255 + local6 * (float) 255 - alpha / 255;
			alphaBack1 = (byte) alphaBack0;
			alphaBack2 = local4 * (float) alpha / 255 + local7 * (float) 255 - alpha / 255;
			alphaBack3 = (byte) alphaBack2;
			return Color.FromArgb(255, local9, alphaBack1, alphaBack3);
		}


		/// <summary>
		/// Draw a 3D border inside the specified rectangle using a linear gradient border color.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="p_HeaderRectangle"></param>
		/// <param name="p_BackColor"></param>
		/// <param name="p_DarkColor"></param>
		/// <param name="p_LightColor"></param>
		/// <param name="p_DarkGradientNumber">The width of the dark border</param>
		/// <param name="p_LightGradientNumber">The width of the light border</param>
		/// <param name="p_Style"></param>
		public static void DrawGradient3DBorder(Graphics g, 
			Rectangle p_HeaderRectangle, 
			Color p_BackColor, 
			Color p_DarkColor, 
			Color p_LightColor,
			int p_DarkGradientNumber,
			int p_LightGradientNumber,
			Gradient3DBorderStyle p_Style)
		{
			Color l_TopLeft, l_BottomRight;
			int l_TopLeftWidth, l_BottomRightWidth;
			if (p_Style == Gradient3DBorderStyle.Raised)
			{
				l_TopLeft = p_LightColor;
				l_TopLeftWidth = p_LightGradientNumber;
				l_BottomRight = p_DarkColor;
				l_BottomRightWidth = p_DarkGradientNumber;
			}
			else
			{
				l_TopLeft = p_DarkColor;
				l_TopLeftWidth = p_DarkGradientNumber;
				l_BottomRight = p_LightColor;
				l_BottomRightWidth = p_LightGradientNumber;
			}

			//TopLeftBorder
			//			Color[] l_TopLeftGradient = CalculateColorGradient(p_BackColor, l_TopLeft, l_TopLeftWidth);
			Color[] l_TopLeftGradient = CalculateColorGradient(p_BackColor, l_TopLeft, 2);
			using (Pen l_Pen = new Pen(l_TopLeftGradient[0],1.0f))
			{
				for (int i = 0; i < l_TopLeftGradient.Length; i++)
				{
					l_Pen.Color = l_TopLeftGradient[l_TopLeftGradient.Length - (i+1)];

					//top
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Top+i, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Top+i);

					//Left
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Top+i, p_HeaderRectangle.Left+i, p_HeaderRectangle.Bottom-(i+1));
				}
			}

			//BottomRightBorder
			Color[] l_BottomRightGradient = CalculateColorGradient(p_BackColor, l_BottomRight, l_BottomRightWidth);
			using (Pen l_Pen = new Pen(l_BottomRightGradient[0],1.0f))
			{
				for (int i = 0; i < l_BottomRightGradient.Length; i++)
				{
					l_Pen.Color = l_BottomRightGradient[l_BottomRightGradient.Length - (i+1)];

					//bottom
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Bottom-(i+1), p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Bottom-(i+1));

					//right
					g.DrawLine(l_Pen, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Top+i, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Bottom-(i+1));
				}
			}
		}

		public static void DrawSingle3DBorder(Graphics g, 
			Rectangle p_HeaderRectangle, 
			Color p_BackColor, 
			Color p_DarkColor, 
			Color p_LightColor,
			int p_DarkGradientNumber,
			int p_LightGradientNumber,
			Gradient3DBorderStyle p_Style)
		{
			int x,y,width,height;
			x=p_HeaderRectangle.X;
			y=p_HeaderRectangle.Y;
			height=p_HeaderRectangle.Height-1;
			width=p_HeaderRectangle.Width-1;
			
//			
//				using( Brush brushTL = new SolidBrush( p_LightColor ) )
//				{
//					using( Brush brushBR = new SolidBrush( p_DarkColor ) )
//					{
//						g.FillRectangle( brushTL, x, y, width - 1, 1 );
//						g.FillRectangle( brushTL, x, y, 1, height - 1 );
//						g.FillRectangle( brushBR, x + width, y, -1, height );
//						g.FillRectangle( brushBR, x, y + height, width, -1 );
//					}
//				}
			
			Pen p1=new Pen(p_DarkColor);
			Pen p2=new Pen(p_LightColor);
			//g.DrawLine(p2,x,y,x,y+height);
			g.DrawLine(p2,x,y,x+width,y);
			g.DrawLine(p1,x+width,y,x+width,y+height);
			g.DrawLine(p1,x,y+height,x+width,y+height);

		
		}

		/// <summary>
		/// 在开始颜色和结束颜色之间插入指定次数
		/// </summary>
		/// <param name="p_StartColor"></param>
		/// <param name="p_EndColor"></param>
		/// <param name="p_NumberOfGradients"></param>
		/// <returns></returns>
		public static Color[] CalculateColorGradient(Color p_StartColor, Color p_EndColor, int p_NumberOfGradients)
		{
			if (p_NumberOfGradients<2)
				throw new ArgumentException("Invalid Number of gradients, must be 2 or more");
			Color[] l_Colors = new Color[p_NumberOfGradients];
			l_Colors[0] = p_StartColor;
			l_Colors[l_Colors.Length-1] = p_EndColor;

			float l_IncrementR = ((float)(p_EndColor.R-p_StartColor.R)) / (float)p_NumberOfGradients;
			float l_IncrementG = ((float)(p_EndColor.G-p_StartColor.G)) / (float)p_NumberOfGradients;
			float l_IncrementB = ((float)(p_EndColor.B-p_StartColor.B)) / (float)p_NumberOfGradients;

			for (int i = 1; i < (l_Colors.Length-1); i++)
			{
				l_Colors[i] = Color.FromArgb( (int) (p_StartColor.R + l_IncrementR*(float)i ), 
					(int) (p_StartColor.G + l_IncrementG*(float)i ),
					(int) (p_StartColor.B + l_IncrementB*(float)i ) );
			}

			return l_Colors;
		}
	}

	public enum Gradient3DBorderStyle
	{
		Raised = 1,
		Sunken = 2
	}
}
