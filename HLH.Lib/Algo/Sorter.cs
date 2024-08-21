using System.Collections.Generic;
namespace HLH.Lib.Algo
{

    /// <summary>
    /// Sorter 排序类
    /// <para>泛型快速排序算法</para>
    /// </summary>
    public class Sorter
    {
        /**/
        /// <summary>
        /// 两个值的比较委托
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>返回值,值1大于值2返回1,值1小于值2返回-1,值1等于值2返回0</returns>
        public delegate int Compare<T>(T value1, T value2);

        /**/
        /// <summary>
        /// 二分排序法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">要进行排序的集合</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        public static void DimidiateSort<T>(IList<T> myList, Compare<T> myCompareMethod)
        {
            DimidiateSort<T>(myList, 0, myList.Count - 1, myCompareMethod);
        }
        /**/
        /// <summary>
        /// 二分排序法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">要进行排序的集合</param>
        /// <param name="left">起始位置</param>
        /// <param name="right">结束位置</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        public static void DimidiateSort<T>(IList<T> myList, int left, int right, Compare<T> myCompareMethod)
        {
            if (left < right)
            {
                T s = myList[(right + left) / 2];
                int i = left - 1;
                int j = right + 1;
                T temp = default(T);
                while (true)
                {
                    do
                    {
                        i++;
                    }
                    while (i < right && myCompareMethod(myList[i], s) == -1);
                    do
                    {
                        j--;
                    }
                    while (j > left && myCompareMethod(myList[j], s) == 1);
                    if (i >= j)
                        break;
                    temp = myList[i];
                    myList[i] = myList[j];
                    myList[j] = temp;
                }
                DimidiateSort(myList, left, i - 1, myCompareMethod);
                DimidiateSort(myList, j + 1, right, myCompareMethod);
            }
        }
    }
}


