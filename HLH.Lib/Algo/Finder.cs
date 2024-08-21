using System.Collections.Generic;

namespace HLH.Lib.Algo
{
    /// <summary>
    /// Finder ������
    /// </summary>
    public class Finder
    {

        /// <summary>
        /// ����ֵ�ıȽ�ί��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="value1">ֵ1</param>
        /// <param name="value2">ֵ2</param>
        /// <returns>����ֵ,ֵ1����ֵ2����1,ֵ1С��ֵ2����-1,ֵ1����ֵ2����0</returns>
        public delegate int Compare<T>(T value1, T value2);


        /// <summary>
        /// ��һ��ֵ���뵽һ������ļ���
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="insertValue">Ҫ�����ֵ</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        public static void InsertToSort<T>(IList<T> myList, T insertValue, Compare<T> myCompareMethod)
        {
            int place = FindInsertPlace<T>(myList, insertValue, 0, myList.Count, myCompareMethod);
            myList.Insert(place, insertValue);
        }


        /// <summary>
        /// �Ƿ���ڴ�ֵ
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="inputKey">Ҫ����ֵ</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        /// <returns>����ֵ</returns>
        public static bool Contains<T>(IList<T> myList, T inputKey, Compare<T> myCompareMethod)
        {
            int place = FindPlace<T>(myList, inputKey, 0, myList.Count, myCompareMethod);
            if (place == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// ���ֲ��ҷ�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="inputKey">Ҫ���ҵ�ֵ</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        /// <returns>ֵ������,δ�ҵ�����-1</returns>
        public static int FindPlace<T>(IList<T> myList, T inputKey, Compare<T> myCompareMethod)
        {
            return FindPlace<T>(myList, inputKey, 0, myList.Count, myCompareMethod);
        }



        /// <summary>
        /// ���ֲ��ҷ�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="inputKey">Ҫ���ҵ�ֵ</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        /// <returns>ֵ������,δ�ҵ�����-1</returns>
        public static int FindPlace<T>(IList<T> myList, T inputKey, int start, int end, Compare<T> myCompareMethod)
        {
            if (myList.Count == 0) return -1;
            int nowplace = (int)((start + end) / 2);
            if (start == nowplace)
            {
                T nowvalue = myList[nowplace];
                if (myCompareMethod(nowvalue, inputKey) == 0)
                {
                    return nowplace;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                T nowvalue = myList[nowplace];
                if (myCompareMethod.Invoke(nowvalue, inputKey) == 1)
                {
                    return FindPlace(myList, inputKey, start, nowplace, myCompareMethod);
                }
                else if (myCompareMethod.Invoke(nowvalue, inputKey) == -1)
                {
                    return FindPlace(myList, inputKey, nowplace, end, myCompareMethod);
                }
                else
                {
                    return nowplace;
                }
            }
        }


        /// <summary>
        /// ����ֵ�Ĳ���λ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="inputKey">Ҫ���ҵ�ֵ</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        /// <returns>����λ��</returns>
        public static int FindInsertPlace<T>(IList<T> myList, T inputKey, Compare<T> myCompareMethod)
        {
            return FindInsertPlace<T>(myList, inputKey, 0, myList.Count, myCompareMethod);
        }



        /// <summary>
        /// ����ֵ�Ĳ���λ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="myList">Ŀ�꼯��</param>
        /// <param name="inputKey">Ҫ���ҵ�ֵ</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        /// <param name="myCompareMethod">����ֵ�ıȽϷ���</param>
        /// <returns>����λ��</returns>
        public static int FindInsertPlace<T>(IList<T> myList, T inputKey, int start, int end, Compare<T> myCompareMethod)
        {
            if (myList.Count == 0) return 0;
            int nowplace = (start + end) / 2;
            if (start == nowplace)
            {
                T nowvalue = myList[nowplace];
                if (myCompareMethod.Invoke(nowvalue, inputKey) == 1)
                {
                    return start;
                }
                else
                {
                    return end;
                }
            }
            else
            {
                T nowvalue = myList[nowplace];
                if (myCompareMethod.Invoke(nowvalue, inputKey) == 1)
                {
                    return FindInsertPlace(myList, inputKey, start, nowplace, myCompareMethod);
                }
                else
                {
                    return FindInsertPlace(myList, inputKey, nowplace, end, myCompareMethod);
                }
            }
        }
    }
}
