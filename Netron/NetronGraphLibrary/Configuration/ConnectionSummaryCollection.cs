using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
    /// <summary>
    /// ʵ������ժҪ��ǿ���ͼ���
    /// </summary>
    public class ConnectionSummaryCollection : CollectionBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConnectionSummaryCollection()
		{
			
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="summary"></param>
		/// <returns></returns>
		public int Add(ConnectionSummary summary)
		{
			return this.InnerList.Add(summary);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public ConnectionSummary this[int index]
		{
			get{return this.InnerList[index] as ConnectionSummary;}
		}
	}
}
