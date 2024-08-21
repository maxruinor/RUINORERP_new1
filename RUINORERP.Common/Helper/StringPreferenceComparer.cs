using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{

	//https://gitee.com/dlgcy/dotnetcodes
	//上面是一个类库 功能类

	/// <summary>
	/// [dlgcy] 字符串偏好比较器
	/// </summary>
	public class StringPreferenceComparer : IComparer<string>
	{
		/// <summary>
		/// 字符串中的分隔符
		/// </summary>
		private readonly string _splitStr;

		/// <summary>
		/// 偏好的排序列表
		/// </summary>
		public List<List<string>> _preferenceList;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="preferenceList">偏好的排序列表</param>
		/// <param name="splitStr">字符串中的分隔符</param>
		public StringPreferenceComparer(List<List<string>> preferenceList, string splitStr)
		{
			_splitStr = splitStr;
			_preferenceList = preferenceList ?? new List<List<string>>();
		}

		/// <inheritdoc />
		public int Compare(string x, string y)
		{
			if (!_preferenceList.Any())
			{
				return DefaultCompare(x, y);
			}

			var strsX = x?.Split(new[] { _splitStr }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
			var strsY = y?.Split(new[] { _splitStr }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

			if (!strsX.Any() && !strsY.Any())
			{
				return 0;
			}
			else if (strsX.Any() && !strsY.Any())
			{
				return 1;
			}
			else if (!strsX.Any() && strsY.Any())
			{
				return -1;
			}

			int countX = strsX.Length;
			int countY = strsY.Length;
			int index = 0;

			while (index < countX && index < countY)
			{
				string currentPartX = strsX[index];
				string currentPartY = strsY[index];

				List<string> matchedList = SearchMatchedList(currentPartX, currentPartY);
				if (matchedList == null)
				{
					return DefaultCompare(currentPartX, currentPartY);
				}

				int compareX = -1;
				int compareY = -1;
				for (int i = 0; i < matchedList.Count; i++)
				{
					string str = matchedList[i];
					if (compareX == -1 && currentPartX.Contains(str))
					{
						compareX = i;
					}

					if (compareY == -1 && currentPartY.Contains(str))
					{
						compareY = i;
					}
				}

				if (compareX != compareY)
				{
					return compareX > compareY ? 1 : -1;
				}

				index++;
			}

			if (countX == countY)
			{
				return DefaultCompare(x, y);
			}
			else if (countX < countY)
			{
				return DefaultCompare(x, strsY.Take(countX).ToString());
			}
			else
			{
				return DefaultCompare(strsX.Take(countY).ToString(), y);
			}
		}

		/// <summary>
		/// 默认比较
		/// </summary>
		private int DefaultCompare(string x, string y)
		{
			return string.Compare(x, y, false, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// 搜寻匹配的列表
		/// </summary>
		/// <param name="currentPartX">X字符串的当前比较部分</param>
		/// <param name="currentPartY">Y字符串的当前比较部分</param>
		/// <returns></returns>
		private List<string> SearchMatchedList(string currentPartX, string currentPartY)
		{
			List<string> matchedList = null;
			foreach (var list in _preferenceList)
			{
				if (list.Exists(currentPartX.Contains) && list.Exists(currentPartY.Contains))
				{
					matchedList = list;
					break;
				}
			}

			return matchedList;
		}
	}
}

