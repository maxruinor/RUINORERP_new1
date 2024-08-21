using System;

namespace SourceGrid2.General
{

  public enum VisualStyle
  {
    IDE   = 0,
    Plain = 1
  }

  public enum Direction
  {
    Vertical  = 0,
    Horizontal  = 1
  }

  public enum DrawState
  {
    Normal,
    Disable,
    Hot,
    Pressed,
  }

  /// <summary>
  /// Class help to simply version switching
  /// </summary>
  public class Version
  {
    public const string OwnVersion = "cenetcom.Controls, Version=1.0.5.0, Culture=neutral, PublicKeyToken=fe06e967d3cf723d";
    public const string DesignerVersion = "cenetcom.Controls.Designers, Version=1.0.5.0, Culture=neutral, PublicKeyToken=fe06e967d3cf723d";
  }

  // cenetcom.Controls.General.cenetcom.Controls.DesignerVersion
}