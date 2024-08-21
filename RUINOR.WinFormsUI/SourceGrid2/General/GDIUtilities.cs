#region file using directives
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

using SourceGrid2.Win32;
#endregion

namespace SourceGrid2.General
{
  #region 3D Styles Enums
  /// <summary>
  /// Styles of canvas which emulate 3d effect on screen
  /// </summary>
  public enum Canvas3DStyle
  {
    /// <summary>
    /// 3D canvas must be draw in one line. Canvas
    /// </summary>
    Single,
    /// <summary>
    /// Raised canvas 3d style. Canvas will take two pixels from all sides.
    /// </summary>
    Raised,
    /// <summary>
    /// Upped canvas 3d style. Canvas will take two pixels from all sides.
    /// </summary>
    Upped,
    /// <summary>
    /// Used when two or more items must be look like one 3d area. such items
    /// splitted by special 3d vertical line. Mostly used for headers drawing.
    /// </summary>
    Title,
    /// <summary>
    /// Flat style of 3d canvas. Single line border.
    /// </summary>
    Flat
  }
  /// <summary>
  /// Specify styles which item can have for highlighting
  /// </summary>
  public enum HightlightStyle
  {
    /// <summary>
    /// Mouse cursor enter item area.
    /// </summary>
    Active,
    /// <summary>
    /// Item is selected, contains focus or checked.
    /// </summary>
    Selected
  }
  #endregion
	#region ArrowGlyph
	public enum ArrowGlyph
	{
		Up,
		Down,
		Left,
		Right
	}
	#endregion
  public class GDIUtils
  {
    #region Class Members
    /// <summary>
    ///
    /// </summary>
    private SolidBrush m_brushDark = null;
    /// <summary>
    ///
    /// </summary>
    private SolidBrush m_brushDarkDark = null;
    /// <summary>
    ///
    /// </summary>
    private SolidBrush m_brushLight = null;
    /// <summary>
    ///
    /// </summary>
    private SolidBrush m_brushLightLight = null;
    /// <summary>
    ///
    /// </summary>
    private Color m_clrDark = Color.Empty;
    /// <summary>
    ///
    /// </summary>
    private Color m_clrDarkDark = Color.Empty;
    /// <summary>
    ///
    /// </summary>
    private Color m_clrLight ;
    /// <summary>
    ///
    /// </summary>
    private Color m_clrLightLight=Color.Black ;
    /// <summary>
    ///
    /// </summary>
    private Pen m_penDark = null;
    /// <summary>
    ///
    /// </summary>
    private Pen m_penDarkDark = null;
    /// <summary>
    ///
    /// </summary>
    private Pen m_penLight = null;
    /// <summary>
    ///
    /// </summary>
    private Pen m_penLightLight = null;
    #endregion

    #region Class Properties
    /// <summary>
    ///
    /// </summary>
    public Color  Dark
    {
      get
      {
        return m_clrDark;
      }
      set
      {
        if( value != m_clrDark )
        {
          m_clrDark = value;

          // destoroy old values
          if( m_brushDark != null ) m_brushDark.Dispose( );
          if( m_penDark != null ) m_penDark.Dispose( );

          m_brushDark = new SolidBrush( m_clrDark );
          m_penDark = new Pen( m_brushDark );
        }
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Color  DarkDark
    {
      get
      {
        return m_clrDarkDark;
      }
      set
      {
       // if( value != m_clrDarkDark )
        {
          m_clrDarkDark = value;

          // destoroy old values
          if( m_brushDarkDark != null ) m_brushDarkDark.Dispose( );
          if( m_penDarkDark != null ) m_penDarkDark.Dispose( );

          m_brushDarkDark = new SolidBrush( m_clrDarkDark );
          m_penDarkDark = new Pen( m_brushDarkDark );
        }
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Color  Light
    {
      get
      {
        return m_clrLight;
      }
      set
      {
       if( value != m_clrLight )
        {
          m_clrLight = value;

          // destoroy old values
          if( m_brushLight != null ) m_brushLight.Dispose( );
          if( m_penLight != null ) m_penLight.Dispose( );

          m_brushLight = new SolidBrush( m_clrLight );
          m_penLight = new Pen( m_brushLight );
        }
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Color  LightLight
    {
      get
      {
        return m_clrLightLight;
      }
      set
      {
        if( value != m_clrLightLight )
        {
          m_clrLightLight = value;

          // destoroy old values
          if( m_brushLightLight != null ) m_brushLightLight.Dispose( );
          if( m_penLightLight != null ) m_penLightLight.Dispose( );

          m_brushLightLight = new SolidBrush( m_clrLightLight );
          m_penLightLight = new Pen( m_brushLightLight );
        }
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Brush  DarkBrush
    {
      get
      {
        return m_brushDark;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Brush  DarkDarkBrush
    {
      get
      {
        return m_brushDarkDark;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Brush  LightBrush
    {
      get
      {
        return m_brushLight;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Brush  LightLightBrush
    {
      get
      {
        return m_brushLightLight;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Pen    DarkPen
    {
      get
      {
        return m_penDark;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Pen    DarkDarkPen
    {
      get
      {
        return m_penDarkDark;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Pen    LightPen
    {
      get
      {
        return m_penLight;
      }
    }
    /// <summary>
    ///
    /// </summary>
    public Pen    LightLightPen
    {
      get
      {
        return m_penLightLight;
      }
    }
    /// <summary>
    ///
    /// </summary>
    static public StringFormat OneLineFormat
    {
      get
      {
        StringFormat format = new StringFormat( );

        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        format.Trimming = StringTrimming.EllipsisCharacter;
        format.FormatFlags = StringFormatFlags.LineLimit;
        format.HotkeyPrefix = HotkeyPrefix.Show;

        return format;
      }
    }
    /// <summary>
    ///
    /// </summary>
    static public StringFormat OneLineNoTrimming
    {
      get
      {
        StringFormat format = new StringFormat( );

        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        format.Trimming = StringTrimming.None;
        format.FormatFlags = StringFormatFlags.LineLimit;
        format.HotkeyPrefix = HotkeyPrefix.Show;

        return format;
      }
    }
    #endregion

    #region Initialize/Finilize functions
    /// <summary>
    /// Default Constructor
    /// </summary>
    public GDIUtils( )
    {
      this.Dark       = SystemColors.ControlDark;
      this.DarkDark   = SystemColors.ControlDarkDark;
      this.Light      = SystemColors.ControlLight;
      this.LightLight = SystemColors.ControlLightLight;
	
    }
    /// <summary>
    ///
    /// </summary>
    ~GDIUtils( )
    {
      this.Dispose( );
    }
    /// <summary>
    /// Destroy all pens and brushes used by class
    /// </summary>
    public void Dispose( )
    {
      Dispose( ref m_brushDark );
      Dispose( ref m_penDark );
      Dispose( ref m_brushDarkDark );
      Dispose( ref m_penDarkDark );
      Dispose( ref m_brushLight );
      Dispose( ref m_penLight );
      Dispose( ref m_brushLightLight );
      Dispose( ref m_penLightLight );
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="brush"></param>
    private void Dispose( ref SolidBrush brush )
    {
      if( brush != null )
      {
        brush.Dispose( );
        brush = null;
      }
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="pen"></param>
    private void Dispose( ref Pen pen )
    {
      if( pen != null )
      {
        pen.Dispose( );
        pen = null;
      }
    }
    #endregion

    #region Custom Drawing functions
    /// <summary>
    /// Draw 3d Line. 3D Line is a simple line wich contains one dark and one light line.
    /// By dark and light line we create optical 3D effect.
    /// </summary>
    /// <param name="graph">Graphics object which used by function to draw</param>
    /// <param name="pnt1">Start point</param>
    /// <param name="pnt2">End point</param>
    public void Draw3DLine( Graphics graph, Point pnt1, Point pnt2 )
    {
      if( graph == null )
        throw new ArgumentNullException( "graph" );

      Pen penDark = m_penDark;
      Pen penLight = m_penLightLight;

      Point[] arrPoint = { pnt1, pnt2 }; // create copy of Point input params
      graph.DrawLine( m_penLight, pnt1, pnt2 ); // draw first line

      if( pnt1.X == pnt2.X )
      {
        arrPoint[0].X--;
        arrPoint[1].X--;
      }
      else if( pnt1.Y == pnt2.Y )
      {
        arrPoint[0].Y--;
        arrPoint[1].Y--;
      }
      else
      {
        arrPoint[0].X--; arrPoint[0].Y--;
        arrPoint[1].X--; arrPoint[1].Y--;
      }

      graph.DrawLine( penDark, arrPoint[0], arrPoint[1] );
    }

    /// <summary>
    /// Draw 3D box according to style specification. There are four styles which our
    /// function know how to draw.
    /// </summary>
    /// <param name="graph">Graphics object used for drawing</param>
    /// <param name="rect">Box rectangle</param>
    /// <param name="style">Style of Box</param>
    public void Draw3DBox( Graphics graph, Rectangle rect, Canvas3DStyle style )
    {
      if( graph == null )
        throw new ArgumentNullException( "graph" );

      Point pnt1 = Point.Empty
        , pnt2 = Point.Empty
        , pnt4 = Point.Empty;

      Point[] arrPoints = new Point[4];

      switch( style )
      {
        case Canvas3DStyle.Flat:
          graph.DrawRectangle( m_penDark, rect );
          break;

        case Canvas3DStyle.Title:
          #region Canvas 3DStyle - Title
          graph.DrawRectangle( m_penDark, rect );

          pnt1.X = rect.X+1; pnt1.Y = rect.Y+1;
          pnt2.X = rect.X+1; pnt2.Y = rect.Height-1;
          pnt4.X = rect.Width-1; pnt4.Y = rect.Y+1;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLightLight, arrPoints );
          #endregion
          break;

        case Canvas3DStyle.Raised:
          #region Canvas 3DStyle Raised
          // draw left upper corner
          pnt1.X = rect.X; pnt1.Y = rect.Y;
          pnt2.X = rect.X + rect.Width; pnt2.Y = rect.Y;
          pnt4.X = rect.X; pnt4.Y = rect.Y + rect.Height;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penDark, arrPoints );

          pnt1.X++; pnt1.Y++;
          pnt2.X-=2; pnt2.Y++;
          pnt4.X++; pnt4.Y-=2;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penDarkDark, arrPoints );

          pnt1.X = rect.X + rect.Width; pnt1.Y = rect.Y + rect.Height;
          pnt2.X = rect.X; pnt2.Y = rect.Y + rect.Height;
          pnt4.X = rect.X + rect.Width; pnt4.Y = rect.Y;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLightLight, arrPoints );

          pnt1.X--; pnt1.Y--;
          pnt2.X++; pnt2.Y--;
          pnt4.X--; pnt4.Y++;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLight, arrPoints );
          #endregion
          break;

        case Canvas3DStyle.Upped:
          #region Canvas 3D Style Upped
          // draw left upper corner
          pnt1.X = rect.X; pnt1.Y = rect.Y;
          pnt2.X = rect.X + rect.Width; pnt2.Y = rect.Y;
          pnt4.X = rect.X; pnt4.Y = rect.Y + rect.Height;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLightLight, arrPoints );

          pnt1.X++; pnt1.Y++;
          pnt2.X-=2; pnt2.Y++;
          pnt4.X++; pnt4.Y-=2;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLight, arrPoints );

          pnt1.X = rect.X + rect.Width; pnt1.Y = rect.Y + rect.Height;
          pnt2.X = rect.X; pnt2.Y = rect.Y + rect.Height;
          pnt4.X = rect.X + rect.Width; pnt4.Y = rect.Y;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penDarkDark, arrPoints );

          pnt1.X--; pnt1.Y--;
          pnt2.X++; pnt2.Y--;
          pnt4.X--; pnt4.Y++;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penDark, arrPoints );
          #endregion
          break;

        case Canvas3DStyle.Single:
          #region Canvas 3D Style Single
          // draw left upper corner
          pnt1.X = rect.X; pnt1.Y = rect.Y;
          pnt2.X = rect.Width; pnt2.Y = rect.Y;
          pnt4.X = rect.X; pnt4.Y = rect.Height;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penDark, arrPoints );

          // draw right low corner
          pnt1.X = rect.Width; pnt1.Y = rect.Height;
          pnt2.X = rect.X; pnt2.Y = rect.Height;
          pnt4.X = rect.Width; pnt4.Y = rect.Y;

          // set new values to array of pointers
          arrPoints[0] = arrPoints[2] = pnt1;
          arrPoints[1] = pnt2; arrPoints[3] = pnt4;

          graph.DrawLines( m_penLightLight, arrPoints );
          #endregion
          break;
      }
    }

    /// <summary>
    /// Draw Active rectangle by blue colors
    /// </summary>
    /// <param name="graph">Graphic context where premitive must be draw</param>
    /// <param name="rect">Destination Rectangle</param>
    /// <param name="state">State of rectangle. Influence on colors by which rectangle
    /// will be drawed</param>
    public void DrawActiveRectangle( Graphics graph, Rectangle rect, HightlightStyle state )
    {
      if( graph == null )
        throw new ArgumentNullException( "graph" );

      Color highlight = ( state == HightlightStyle.Active ) ?
        ColorUtil.VSNetBackgroundColor : ColorUtil.VSNetSelectionColor;

      Color highBorder = SystemColors.Highlight;

      SolidBrush high = new SolidBrush( highlight );
      SolidBrush bord = new SolidBrush( highBorder );
      Pen penBord = new Pen( bord );

      graph.FillRectangle( high, rect );
      graph.DrawRectangle( penBord, rect );

      penBord.Dispose( );
      bord.Dispose( );
      high.Dispose( );
    }

    /// <summary>
    /// Draw Active rectangle.
    /// </summary>
    /// <param name="graph">Graphic context where premitive must be draw</param>
    /// <param name="rect">Destination Rectangle</param>
    /// <param name="state">State of rectangle. Influence on colors by which rectangle
    /// will be drawed</param>
    /// <param name="bSubRect">Indicate do we need rectangle width and height fix</param>
    public void DrawActiveRectangle( Graphics graph, Rectangle rect, HightlightStyle state, bool bSubRect )
    {
      if( graph == null )
        throw new ArgumentNullException( "graph" );

      Rectangle rc = ( bSubRect ) ? FixRectangleHeightWidth( rect ) : rect;
      DrawActiveRectangle( graph, rc, state );
    }

    /// <summary>
    /// Make rectangle width and height less on one pixel. This useful beacause
    /// in some cases rectangle last pixels does not shown.
    /// </summary>
    /// <param name="rect">Rectangle which context must be fixed</param>
    /// <returns>Return new Rectangle object which contains fixed values</returns>
    static public Rectangle FixRectangleHeightWidth( Rectangle rect )
    {
      return new Rectangle( rect.X, rect.Y, rect.Width - 1, rect.Height - 1 );
    }
    #endregion

    #region Class Helper Functions
    /// <summary>
    /// Calculate X and Y coordiante to place object in center of Rectangle
    /// </summary>
    /// <param name="rect">Destination Rectangle</param>
    /// <param name="sz">Object Size</param>
    /// <returns>Point class with X and Y coordinate of center</returns>
    static public Point CalculateCenter( Rectangle rect, Size sz )
    {
      Point pnt1 = new Point( 0 );

      pnt1.X = rect.X + ( rect.Width - sz.Width ) / 2;
      pnt1.Y = rect.Y + ( rect.Height - sz.Height ) / 2;

      return pnt1;
    }
    #endregion

    #region Static Public Methods
    /// <summary>
    /// Draw 3D styled Rectangle.
    /// </summary>
    /// <param name="g">Graphics canvas where rectangle must drawed</param>
    /// <param name="rc">Rectangle coordinates</param>
    /// <param name="clrTL">Color of Top Left corner of rectangle</param>
    /// <param name="clrBR">Color of Bottom Right corner of rectangle</param>
    static public void Draw3DRect( Graphics g, Rectangle rc, Color clrTL, Color clrBR )
    {
      Draw3DRect( g, rc.Left, rc.Top, rc.Width, rc.Height, clrTL, clrBR );
    }
	  static public void Draw3DRect( Graphics g, Rectangle rc, int deep,Color clrTL, Color clrBR )
	  {
		  Draw3DRect( g, rc.Left, rc.Top, rc.Width, rc.Height,deep, clrTL, clrBR );
	  }
    /// <summary>
    /// Draw 3D styled Rectangle.
    /// </summary>
    /// <param name="g">Graphics object</param>
    /// <param name="x">X top left corner coordinat of rectangle</param>
    /// <param name="y">Y top left corner coordinat of rectangle</param>
    /// <param name="width">Width of rectangle</param>
    /// <param name="height">Height of rectangle</param>
    /// <param name="clrTL">Color used for Top Left ( TL ) corner drawing</param>
    /// <param name="clrBR">Color used for Bottom Right ( BR ) corner drawing</param>
    static public void Draw3DRect( Graphics g, int x, int y, int width, int height, Color clrTL, Color clrBR )
    {
      if( g == null )
        throw new ArgumentNullException( "g" );

      using( Brush brushTL = new SolidBrush( clrTL ) )
      {
        using( Brush brushBR = new SolidBrush( clrBR ) )
        {
          g.FillRectangle( brushTL, x, y, width - 1, 1 );
          g.FillRectangle( brushTL, x, y, 1, height - 1 );
          g.FillRectangle( brushBR, x + width, y, -1, height );
          g.FillRectangle( brushBR, x, y + height, width, -1 );
        }
      }
    }
	  static public void Draw3DRect( Graphics g, int x, int y, int width, int height,int deep, Color clrTL, Color clrBR )
	  {
		  if( g == null )
			  throw new ArgumentNullException( "g" );

		  using( Brush brushTL = new SolidBrush( clrTL ) )
		  {
			  using( Brush brushBR = new SolidBrush( clrBR ) )
			  {
				  g.FillRectangle( brushTL, x, y, width - deep, deep );
				  g.FillRectangle( brushTL, x, y, deep, height - deep );
				  g.FillRectangle( brushBR, x + width, y, -deep, height );
				  g.FillRectangle( brushBR, x, y + height, width, -deep );
			  }
		  }
	  }
    /// <summary>
    /// Strech Bitmap using GDI API, not GDI+!!!
    /// </summary>
    /// <param name="gDest">graphics object</param>
    /// <param name="rcDest">Destination rectangle</param>
    /// <param name="bitmap">source Bitmap</param>
    static public void StrechBitmap( Graphics gDest, Rectangle rcDest, Bitmap bitmap )
    {
      if( gDest == null )
        throw new ArgumentNullException( "gDest" );

      if( rcDest == Rectangle.Empty )
        throw new ArgumentOutOfRangeException( "rcDest", "Destination area can not be zero size" );

      if( bitmap == null )
        throw new ArgumentNullException( "bitmap" );

      // Draw From bitmap
      IntPtr hDCTo = gDest.GetHdc( );
      WindowsAPI.SetStretchBltMode( hDCTo, StrechModeFlags.HALFTONE );
      IntPtr hDCFrom = WindowsAPI.CreateCompatibleDC( hDCTo );

      IntPtr hOldFromBitmap = WindowsAPI.SelectObject( hDCFrom, bitmap.GetHbitmap( ) );
      WindowsAPI.StretchBlt( hDCTo, rcDest.Left , rcDest.Top, rcDest.Width, rcDest.Height,
        hDCFrom, 0 , 0, bitmap.Width, bitmap.Height,
        PatBltTypes.SRCCOPY );

      // Cleanup
      WindowsAPI.SelectObject( hDCFrom, hOldFromBitmap );
      gDest.ReleaseHdc( hDCTo );

    }
    /// <summary>
    /// Strech Bitmap using GDI API, not GDI+!!!
    /// </summary>
    /// <param name="gDest">graphics object</param>
    /// <param name="rcDest">Destination rectangle</param>
    /// <param name="rcSource">Source rectangle of bitmap</param>
    /// <param name="bitmap">source Bitmap</param>
    static public void StrechBitmap( Graphics gDest, Rectangle rcDest, Rectangle rcSource, Bitmap bitmap )
    {
      if( gDest == null )
        throw new ArgumentNullException( "gDest" );

      if( rcDest == Rectangle.Empty )
        throw new ArgumentOutOfRangeException( "rcDest", "Destination area can not be zero size" );

      if( rcSource == Rectangle.Empty )
        throw new ArgumentOutOfRangeException( "rcSource", "Source area can not be zero size" );

      if( bitmap == null )
        throw new ArgumentNullException( "bitmap" );

      // Draw From bitmap
      IntPtr hDCTo = gDest.GetHdc( );
      WindowsAPI.SetStretchBltMode( hDCTo, StrechModeFlags.COLORONCOLOR );
      IntPtr hDCFrom = WindowsAPI.CreateCompatibleDC( hDCTo );

      IntPtr hOldFromBitmap = WindowsAPI.SelectObject( hDCFrom, bitmap.GetHbitmap( ) );
      WindowsAPI.StretchBlt( hDCTo, rcDest.Left , rcDest.Top, rcDest.Width, rcDest.Height,
        hDCFrom, rcSource.Left , rcSource.Top, rcSource.Width, rcSource.Height,
        PatBltTypes.SRCCOPY );

      // Cleanup
      WindowsAPI.SelectObject( hDCFrom, hOldFromBitmap );
      gDest.ReleaseHdc( hDCTo );
    }
    /// <summary>
    /// Strech Bitmap using ntive window GDI API, not GDI+!!!
    /// </summary>
    /// <param name="gDest">graphics object</param>
    /// <param name="rcDest">Destination rectangle</param>
    /// <param name="bitmap">source Bitmap</param>
    /// <returns>return Bitmap Streched to needed size</returns>
    static public Bitmap GetStrechedBitmap( Graphics gDest, Rectangle rcDest, Bitmap bitmap )
    {
      if( gDest == null )
        throw new ArgumentNullException( "gDest" );

      if( rcDest == Rectangle.Empty )
        throw new ArgumentOutOfRangeException( "rcDest", "Destination area can not be zero size" );

      if( bitmap == null )
        throw new ArgumentNullException( "bitmap" );

      // Draw To bitmap
      Bitmap newBitmap = new Bitmap( rcDest.Width, rcDest.Height );
      Graphics gBitmap = Graphics.FromImage( newBitmap );

      IntPtr hDCTo = gBitmap.GetHdc( );
      WindowsAPI.SetStretchBltMode( hDCTo, StrechModeFlags.COLORONCOLOR );
      IntPtr hDCFrom = WindowsAPI.CreateCompatibleDC( hDCTo );
      IntPtr hOldFromBitmap = WindowsAPI.SelectObject( hDCFrom, bitmap.GetHbitmap( ) );

      WindowsAPI.StretchBlt( hDCTo,
        rcDest.Left , rcDest.Top, rcDest.Width, rcDest.Height,
        hDCFrom, 0 , 0, bitmap.Width, bitmap.Height,
        PatBltTypes.SRCCOPY );

      // Cleanup
      WindowsAPI.SelectObject( hDCFrom, hOldFromBitmap );
      gBitmap.ReleaseHdc( hDCTo );

      return newBitmap;
    }
    /// <summary>
    /// Create Bitmap with specified size and infill. Bitmap send by parameter
    /// will be used to fill destination area. If bitmap is less then destination
    /// area then it will be tiled.
    /// </summary>
    /// <param name="sz"></param>
    /// <param name="bitmap">Infill of output bitmap</param>
    /// <returns>returns Bitmap filled by intput bitmap</returns>
    static public Bitmap GetTileBitmap( Size sz, Bitmap bitmap )
    {
      if( sz == Size.Empty )
        throw new ArgumentOutOfRangeException( "sz", "Destination area size can not be zero" );

      return GetTileBitmap( new Rectangle( Point.Empty, sz ), bitmap );
    }
    /// <summary>
    /// Create Bitmap with specified size and infill. Bitmap send by parameter
    /// will be used to fill destination area. If bitmap is less then destination
    /// area then it will be tiled.
    /// </summary>
    /// <param name="rcDest">Destination size</param>
    /// <param name="bitmap">Infill of output bitmap</param>
    /// <returns>returns Bitmap filled by intput bitmap</returns>
    static public Bitmap GetTileBitmap( Rectangle rcDest, Bitmap bitmap )
    {
      if( bitmap == null )
        throw new ArgumentNullException( "bitmap" );

      if( rcDest == Rectangle.Empty )
        throw new ArgumentOutOfRangeException( "rcDest", "Destination Rectangle size can not be zero" );

      Bitmap tiledBitmap = new Bitmap( rcDest.Width, rcDest.Height );

      using( Graphics g = Graphics.FromImage( tiledBitmap ) )
      {
        for( int i = 0; i < tiledBitmap.Width; i += bitmap.Width )
        {
          for( int j = 0; j < tiledBitmap.Height; j += bitmap.Height )
          {
            g.DrawImage( bitmap, new Point( i, j ) );
          }
        }
      }

      return tiledBitmap;
    }
    /// <summary>
    /// Draw arrow glyph in center of rectangle. Width and Height of arrow will be 5 and 3.
    /// For arrow drawng will be used SystemColors.Highlight color.
    /// TIP: use an odd number for the arrowWidth and arrowWidth/2+1 for the arrowHeight
    /// so that the arrow gets the same pixel number on the left and on the right and
    /// get symetrically painted
    /// </summary>
    /// <param name="g">Graphics objet</param>
    /// <param name="rc">Destination rectangle</param>
    /// <param name="up">Direction of Arrow</param>
    static public void DrawArrowGlyph( Graphics g, Rectangle rc, bool up )
    {
      DrawArrowGlyph( g, rc, up, SystemColors.Highlight );
    }
    /// <summary>
    /// Draw arrow glyph in center of rectangle. Width and Height of arrow will be 5 and 3.
    /// TIP: use an odd number for the arrowWidth and arrowWidth/2+1 for the arrowHeight
    /// so that the arrow gets the same pixel number on the left and on the right and
    /// get symetrically painted
    /// </summary>
    /// <param name="g">Graphics objet</param>
    /// <param name="rc">Destination rectangle</param>
    /// <param name="up">Direction of Arrow</param>
    /// <param name="clr">Color which must be used for drawing</param>
    static public void DrawArrowGlyph( Graphics g, Rectangle rc, bool up, Color clr )
    {
      DrawArrowGlyph( g, rc, up, new SolidBrush( clr ) );
    }
    /// <summary>
    /// Draw arrow glyph in center of rectangle. Width and Height of arrow will be 5 and 3.
    /// TIP: use an odd number for the arrowWidth and arrowWidth/2+1 for the arrowHeight
    /// so that the arrow gets the same pixel number on the left and on the right and
    /// get symetrically painted
    /// </summary>
    /// <param name="g">Graphics object</param>
    /// <param name="rc">Destination Rectangle</param>
    /// <param name="up">Direction of arrow</param>
    /// <param name="brush">Brush which must be used for drawing</param>
    static public void DrawArrowGlyph( Graphics g, Rectangle rc, bool up, Brush brush )
    {
      // Draw arrow glyph with the default size of 5 pixel wide and 3 pixel high
      DrawArrowGlyph( g, rc, 5, 3, up, brush );
    }
	 
	  static public void DrawArrowGlyph(Graphics g, Rectangle rc, int arrowWidth, int arrowHeight, ArrowGlyph arrowGlyph, Brush brush)
	  {
		  // Tip: use an odd number for the arrowWidth and 
		  // arrowWidth/2+1 for the arrowHeight 
		  // so that the arrow gets the same pixel number
		  // on the left and on the right and get symetrically painted
			
		  Point[] pts = new Point[3];
		  int yMiddle = rc.Top + rc.Height/2-arrowHeight/2+1;
		  int xMiddle = rc.Left + rc.Width/2;
			
		  if ( arrowGlyph == ArrowGlyph.Up )
		  {
			  pts[0] = new Point(xMiddle, yMiddle-2);
			  pts[1] = new Point(xMiddle-arrowWidth/2-1, yMiddle+arrowHeight-1);
			  pts[2] = new Point(xMiddle+arrowWidth/2+1,  yMiddle+arrowHeight-1);
				
		  }
		  else if ( arrowGlyph == ArrowGlyph.Down )
		  {
			  //yMiddle -= 1;
			  pts[0] = new Point(xMiddle-arrowWidth/2, yMiddle);
			  pts[1] = new Point(xMiddle+arrowWidth/2+1,  yMiddle);
			  pts[2] = new Point(xMiddle, yMiddle+arrowHeight);
		  }
		  else if ( arrowGlyph == ArrowGlyph.Left )
		  {
			  yMiddle = rc.Top + rc.Height/2;
			  pts[0] = new Point(xMiddle-arrowHeight/2,  yMiddle);
			  pts[1] = new Point(pts[0].X+arrowHeight, yMiddle-arrowWidth/2-1);
			  pts[2] = new Point(pts[0].X+arrowHeight,  yMiddle+arrowWidth/2+1);

		  }
		  else if ( arrowGlyph == ArrowGlyph.Right )
		  {
			  yMiddle = rc.Top + rc.Height/2;
			  pts[0] = new Point(xMiddle+arrowHeight/2+1,  yMiddle);
			  pts[1] = new Point(pts[0].X-arrowHeight, yMiddle-arrowWidth/2-1);
			  pts[2] = new Point(pts[0].X-arrowHeight,  yMiddle+arrowWidth/2+1);
		  }

		  g.FillPolygon(brush, pts);
	  }

	
    /// <summary>
    /// Draw arrow glyph in center of rectangle.
    /// TIP: use an odd number for the arrowWidth and arrowWidth/2+1 for the arrowHeight
    /// so that the arrow gets the same pixel number on the left and on the right and
    /// get symetrically painted
    /// </summary>
    /// <param name="g">Graphics object</param>
    /// <param name="rc">Destination Rectangle. Arraow Gliph will be placed into center
    /// of destination rectangle</param>
    /// <param name="width">width of arrow</param>
    /// <param name="height">height of arrow</param>
    /// <param name="isUp">Direction arrow</param>
    /// <param name="clr">Color which must be used by arrow draw function</param>
    static public void DrawArrowGlyph( Graphics g, Rectangle rc, int width, int height, bool isUp, Color clr )
    {
      DrawArrowGlyph( g, rc, width, height, isUp, new SolidBrush( clr ) );
    }
    /// <summary>
    /// Draw arrow glyph in center of rectangle.
    /// TIP: use an odd number for the arrowWidth and arrowWidth/2+1 for the arrowHeight
    /// so that the arrow gets the same pixel number on the left and on the right and
    /// get symetrically painted
    /// </summary>
    /// <param name="g">Graphics object</param>
    /// <param name="rc">Destination Rectangle. Arraow Gliph will be placed into center
    /// of destination rectangle</param>
    /// <param name="width">width of arrow</param>
    /// <param name="height">height of arrow</param>
    /// <param name="isUp">Direction arrow</param>
    /// <param name="brush">Brush which must be used for drawing</param>
    static public void DrawArrowGlyph( Graphics g, Rectangle rc, int width, int height, bool isUp, Brush brush )
    {
      if( g == null )
        throw new ArgumentNullException( "g" );

      if( rc == Rectangle.Empty )
        throw new ArgumentException( "Destination rectangle can not be empty" );

      if( rc.Width < width )
        throw new ArgumentOutOfRangeException( "width", "Must be less then Destination rectangle width" );

      if( rc.Height < height )
        throw new ArgumentOutOfRangeException( "height", "Must be less then Destination rectangle height" );

      if( brush == null )
        throw new ArgumentNullException( "brush" );

      Point[] pts = new Point[3];
      int yMiddle = rc.Top + rc.Height/2 - height/2+1;
      int xMiddle = rc.Left + rc.Width/2;
      int yArrowHeight  = yMiddle + height;
      int xArrowWidthR  = xMiddle + width/2;
      int xArrowWidthL  = xMiddle - width/2;

      if( isUp )
      {
        pts[0] = new Point( xMiddle, yMiddle-2 );
        pts[1] = new Point( xArrowWidthL - 1, yArrowHeight - 1 );
        pts[2] = new Point( xArrowWidthR + 1, yArrowHeight - 1 );

      }
      else
      {
        pts[0] = new Point( xArrowWidthL, yMiddle );
        pts[1] = new Point( xArrowWidthR + 1,  yMiddle );
        pts[2] = new Point( xMiddle, yArrowHeight );
      }

      g.FillPolygon( brush, pts );
    }

    #endregion
  }
}