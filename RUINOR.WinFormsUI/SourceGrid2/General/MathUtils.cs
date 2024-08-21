#region file using directives
using System;
using System.Collections;
#endregion

namespace SourceGrid2.General
{
	public class MathUtils
	{
		private MathUtils( ){}

    /// <summary>
    /// Return average value of collection items
    /// </summary>
    /// <param name="collection">Colelction of TimeSpans values</param>
    /// <returns></returns>
    public static TimeSpan Average( ICollection collection )
    {
      if( collection == null )
        throw new ArgumentNullException( "collection" );

      if( collection.Count == 0 )
        throw new ArgumentException( "Collection must not be empty", "collection" );

      TimeSpan retVal = Accumulate( collection );
      
      return new TimeSpan( ( long )( retVal.Ticks/collection.Count ) );
    }
    /// <summary>
    /// Return sum of all value stored in collection
    /// </summary>
    /// <param name="collection">collection of TimeSpan values</param>
    /// <returns>Returned summ of all values from collection</returns>
    public static TimeSpan Accumulate( ICollection collection )
    {
      if( collection == null )
        throw new ArgumentNullException( "collection" );

      TimeSpan retVal = TimeSpan.Zero;

      IEnumerator enumColl = collection.GetEnumerator( );
      enumColl.Reset( );

      while( enumColl.MoveNext( ) )
      {
        // if collection value is not a TimeSpan type then cast exception 
        // will thown here
        retVal += ( TimeSpan )enumColl.Current;
      }

      return retVal;
    }
	}
}
