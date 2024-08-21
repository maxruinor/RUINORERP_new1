using System.Collections.Generic;
namespace HLH.Lib.Algo
{

    /// <summary>
    /// Sorter ������
    /// <para>���Ϳ��������㷨</para>
    /// </summary>
    public class Sorter
    {
        /**/
        /// <summary>
        /// ����ֵ�ıȽ�ί��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="value1">ֵ1</param>
        /// <param name="value2">ֵ2</param>
        /// <returns>����ֵ,ֵ1����ֵ2����1,ֵ1С��ֵ2����-1,ֵ1����ֵ2����0</returns>
        public delegate int Compare<T>(T value1, T value2);

        /**/
        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ҫ��������ļ���</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        public static void DimidiateSort<T>(IList<T> myList, Compare<T> myCompareMethod)
        {
            DimidiateSort<T>(myList, 0, myList.Count - 1, myCompareMethod);
        }
        /**/
        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ҫ��������ļ���</param>
        /// <param name="left">��ʼλ��</param>
        /// <param name="right">����λ��</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
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


