using System;
using System.Windows.Forms;
using System.Collections;


namespace SourceGrid2.General
{
	/// <summary>
	/// CustomDragger provides easy way for handling Drag&Drop operations
	/// </summary>
  public class CustomDragger
  {
    #region Class constants
    // Dragging begins after mouse was moved by DraggerDelay pixels
    private readonly int DraggerDelay = SystemInformation.DragSize.Width;
    #endregion

    #region Class Variables
    private int           mouseX;
    private int           mouseY;
    private bool          dragging;
    protected object      obj;
    private Control       m_parent;
    private Control       m_accepter;
    private bool          buttonDowned;
    #endregion

    #region Internal Classes
    public class DragDropInternal
    {
      #region Class constants
      public const int DEF_MEMORY_RESERVE = 16;
      #endregion

      #region Class members
      private ArrayList m_dataTypes;
      private ArrayList m_dataValues;
      #endregion

      #region Class Properties
      /// <summary>
      /// Is Empty. Read only.
      /// </summary>
      public bool IsEmpty
      {
        get
        {
          return ( m_dataTypes.Count == 0 );
        }
      }
      #endregion

      #region Class Initialize/Finalize methods
      public DragDropInternal( )
      {
        m_dataTypes = new ArrayList( DEF_MEMORY_RESERVE );
        m_dataValues = new ArrayList( DEF_MEMORY_RESERVE );
      }
      #endregion

      #region Class Helper Methods
      public void Add( string type, object value )
      {
        m_dataTypes.Add( type );
        m_dataValues.Add( value );
      }

      public ArrayList listByType( string type )
      {
        ArrayList res = new ArrayList( DEF_MEMORY_RESERVE );

        for( int i = 0; i < m_dataTypes.Count; i++ )
        {
          if( ( string )m_dataTypes[i] == type )
          {
            res.Add( m_dataValues[i] );
          }
        }

        return res;
      }

      public void Clear( )
      {
        m_dataTypes.Clear( );
        m_dataValues.Clear( );
      }
      #endregion
    }

    public class DataRequestArgs : EventArgs
    {
      #region DataRequestArgs Class members
      private DragDropInternal m_data;
      private int m_x;
      private int m_y;
      #endregion

      #region DataRequestArgs Class Properties
      public int X
      {
        get{return m_x;}
      }
      public int Y
      {
        get{return m_y;}
      }

      public DragDropInternal Data
      {
        get
        {
          return m_data;
        }
      }

      #endregion

      #region DataRequestArgs Class Initialize/Finalize methods
      private DataRequestArgs( )
      {
      }

      public DataRequestArgs( DragDropInternal data, int X, int Y )
      {
        m_data = data;
        m_x = X;
        m_y = Y;
      }
      #endregion
    }

    public class DropRequestArgs : DataRequestArgs
    {
      #region DropRequestArgs Class members
      private int m_keyState;
      #endregion

      #region DropRequestArgs Class Properties
      public int KeyState
      {
        get
        {
          return m_keyState;
        }
      }
      #endregion

      #region DropRequestArgs Class Initialize/Finalize methods
      public DropRequestArgs( DragDropInternal data, int X, int Y, int keyState )
        : base( data, X, Y )
      {
        m_keyState = keyState;
      }
      #endregion
    }

    public class DropEffectsArgs : DropRequestArgs
    {
      #region DropEffectsArgs Class members
      private DragDropEffects m_effects;
      #endregion

      #region DropEffectsArgs Class Properties
      public DragDropEffects Effects
      {
        get
        {
          return m_effects;
        }
        set
        {
          m_effects = value;
        }
      }

      #endregion

      #region DropEffectsArgs Class Initialize/Finalize methods
      public DropEffectsArgs( DragDropInternal data, DragDropEffects effects, int X, int Y, int keyState )
        : base( data, X, Y, keyState )
      {
        m_effects = effects;
      }
      #endregion
    }
    #endregion

    #region Class delegates
    /// <summary>
    ///
    /// </summary>
    public delegate void DataRequestEventHandler( object sender, DataRequestArgs e );
    /// <summary>
    ///
    /// </summary>
    public delegate void DropEffectsEventHandler( object sender, DropEffectsArgs e );
    /// <summary>
    ///
    /// </summary>
    public delegate void DropDataEventHandler( object sender, DropRequestArgs e );
    #endregion

    #region Class events declaration
    /// <summary>
    ///
    /// </summary>
    public event DataRequestEventHandler  OnDataRequest;
    /// <summary>
    ///
    /// </summary>
    public event DropEffectsEventHandler  OnEffectsRequest;
    /// <summary>
    ///
    /// </summary>
    public event DropDataEventHandler     OnDataDrop;
    #endregion

    #region Class constructor
    /// <summary>
    /// Disable for user default constructor
    /// </summary>
    private CustomDragger( )
    {
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="accepter"></param>
    public CustomDragger( Control parent, Control accepter )
    {
      m_parent = parent;
      m_accepter = accepter;

      AttachEvents( );
    }
    #endregion

    #region Class events attachments
    /// <summary>
    ///
    /// </summary>
    protected void AttachEvents( )
    {
      if( m_parent != null && m_accepter != null )
      {
        m_parent.Disposed   += new EventHandler( Parent_Disposed );
        m_parent.MouseDown  += new MouseEventHandler( Parent_MouseDown );
        m_parent.MouseMove  += new MouseEventHandler( Parent_MouseMove );
        m_parent.MouseUp    += new MouseEventHandler( Parent_MouseUp );
        m_parent.MouseLeave += new EventHandler( Parent_MouseLeave );

        m_accepter.Disposed += new EventHandler( Parent_Disposed );
        m_accepter.DragOver += new DragEventHandler( Acceptor_DragOver );
        m_accepter.DragDrop += new DragEventHandler( Acceptor_DragDrop );
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void DetachEvents( )
    {
      if( m_parent != null && m_accepter != null )
      {
        m_parent.Disposed   -= new EventHandler( Parent_Disposed );
        m_parent.MouseDown  -= new MouseEventHandler( Parent_MouseDown );
        m_parent.MouseMove  -= new MouseEventHandler( Parent_MouseMove );
        m_parent.MouseUp    -= new MouseEventHandler( Parent_MouseUp );
        m_parent.MouseLeave -= new EventHandler( Parent_MouseLeave );

        m_accepter.Disposed -= new EventHandler( Parent_Disposed );
        m_accepter.DragOver -= new DragEventHandler( Acceptor_DragOver );
        m_accepter.DragDrop -= new DragEventHandler( Acceptor_DragDrop );
      }
    }
    #endregion

    #region Mouse event handlers
    private void Parent_Disposed( object sender, EventArgs e )
    {
      DetachEvents( );
    }

    private void BeginDragging ( object sender )
    {
      dragging = true;
      DragDropInternal data = new DragDropInternal( );

      if( OnDataRequest != null )
      {
        OnDataRequest( sender, new DataRequestArgs( data, mouseX, mouseY ) );
      }

      if( !data.IsEmpty )
      {
        m_parent.DoDragDrop( data,
          DragDropEffects.All | DragDropEffects.Link |
          DragDropEffects.Copy | DragDropEffects.Scroll );
      }
    }

    private void Parent_MouseMove( object sender, MouseEventArgs e )
    {
      if( !dragging && e.Button == MouseButtons.Left )
      {
        if( Math.Abs( mouseX - e.X ) >= DraggerDelay || Math.Abs( mouseY - e.Y ) >= DraggerDelay )
        {
          BeginDragging( sender );
        }
      }
    }

    private void Parent_MouseLeave( object sender, EventArgs e )
    {
      if( !dragging && buttonDowned )
      {
        BeginDragging( sender );
      }
    }

    private void Parent_MouseDown( object sender, MouseEventArgs e )
    {
      buttonDowned = false;

      if( e.Button == MouseButtons.Left )
      {
        mouseX = e.X;
        mouseY = e.Y;
        buttonDowned = true;
        dragging = false;
      }
    }

    private void Parent_MouseUp( object sender, MouseEventArgs e )
    {
      buttonDowned = false;
    }

    private void Acceptor_DragOver( object sender, DragEventArgs e )
    {
      System.Drawing.Point point = new System.Drawing.Point ( e.X, e.Y );
      point = ( ( Control )sender ).PointToClient( point );

      DragDropInternal data = new DragDropInternal( );

      DropEffectsArgs arg = new DropEffectsArgs( ( DragDropInternal )e.Data.GetData( data.GetType( ) ),
        DragDropEffects.Copy, point.X, point.Y, e.KeyState );

      if( OnEffectsRequest != null )
      {
        OnEffectsRequest( sender, arg );
      }

      e.Effect = arg.Effects;
    }

    private void Acceptor_DragDrop( object sender, DragEventArgs e )
    {
      if( OnDataDrop != null )
      {
        DragDropInternal data = new DragDropInternal( );

        if( e.Data.GetDataPresent( data.GetType( ) ) )
        {
          System.Drawing.Point point = new System.Drawing.Point ( e.X, e.Y );
          point = (( Control )sender ).PointToClient( point );

          OnDataDrop( sender, new DropRequestArgs( ( DragDropInternal )e.Data.GetData( data.GetType( ) ),
            point.X, point.Y, e.KeyState ) );
        }
      }
    }
    #endregion
  }
}
