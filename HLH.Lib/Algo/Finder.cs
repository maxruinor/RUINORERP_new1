using System.Collections.Generic;

namespace HLH.Lib.Algo
{
    /// <summary>
    /// Finder 查找类
    /// </summary>
    public class Finder
    {

        /// <summary>
        /// 两个值的比较委托
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>返回值,值1大于值2返回1,值1小于值2返回-1,值1等于值2返回0</returns>
        public delegate int Compare<T>(T value1, T value2);


        /// <summary>
        /// 把一个值插入到一个有序的集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="insertValue">要插入的值</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        public static void InsertToSort<T>(IList<T> myList, T insertValue, Compare<T> myCompareMethod)
        {
            int place = FindInsertPlace<T>(myList, insertValue, 0, myList.Count, myCompareMethod);
            myList.Insert(place, insertValue);
        }


        /// <summary>
        /// 是否存在此值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="inputKey">要检查的值</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        /// <returns>返回值</returns>
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
        /// 二分查找法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="inputKey">要查找的值</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        /// <returns>值的索引,未找到返回-1</returns>
        public static int FindPlace<T>(IList<T> myList, T inputKey, Compare<T> myCompareMethod)
        {
            return FindPlace<T>(myList, inputKey, 0, myList.Count, myCompareMethod);
        }



        /// <summary>
        /// 二分查找法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="inputKey">要查找的值</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        /// <returns>值的索引,未找到返回-1</returns>
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
        /// 查找值的插入位置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="inputKey">要查找的值</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        /// <returns>插入位置</returns>
        public static int FindInsertPlace<T>(IList<T> myList, T inputKey, Compare<T> myCompareMethod)
        {
            return FindInsertPlace<T>(myList, inputKey, 0, myList.Count, myCompareMethod);
        }



        /// <summary>
        /// 查找值的插入位置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="myList">目标集合</param>
        /// <param name="inputKey">要查找的值</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="myCompareMethod">两个值的比较方法</param>
        /// <returns>插入位置</returns>
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
