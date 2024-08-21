//-----------------------------------------------------------------------
// <copyright file="ContextDictionary.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Dictionary type that is serializable</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace RUINORERP.Model
{
  /// <summary>
  /// Dictionary type that is serializable
  /// with the SerializationFormatterFactory.GetFormatter().
  /// </summary>
  [Serializable()]
  public class ContextDictionary : HybridDictionary
  {
    /// <summary>
    /// Get a value from the dictionary, or return null
    /// if the key is not found in the dictionary.
    /// </summary>
    /// <param name="key">Key of value to get from dictionary.</param>
    public  object GetValueOrNull(string key)
    {
      if (this.Contains(key))
        return this[key];
      return null;
    }


  }
}