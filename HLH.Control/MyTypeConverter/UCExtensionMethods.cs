using System;

namespace HLH.WinControl.MyTypeConverter
{
    //扩展函数定义简单数据类型转换：
    public static class UCExtensionMethods
    {
        #region 转换函数
        /// <summary>
        /// object 转换为 int
        /// </summary>
        /// <returns></returns>
        //private static int ObjToInt(object obj)
        //{
        //    if (obj == null)
        //        return 0;
        //    if (obj.Equals(DBNull.Value))
        //        return 0;

        //    int returnValue;

        //    if (int.TryParse(obj.ToString(), out returnValue))
        //    {
        //        return returnValue;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        /// <summary>
        /// 转换为boolean型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool ObjToBool(object obj)
        {
            if (obj == null)
                return false;
            if (obj.Equals(DBNull.Value))
                return false;

            bool returnValue;

            if (bool.TryParse(obj.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// object 转换为 int?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //private static int? ObjToIntNull(object obj)
        //{
        //    if (obj == null)
        //        return null;
        //    if (obj.Equals(DBNull.Value))
        //        return null;

        //    return ObjToInt(obj);
        //}


        /// <summary>
        /// object 转换为 string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string ObjToStr(object obj)
        {
            if (obj == null)
                return "";
            if (obj.Equals(DBNull.Value))
                return "";
            return Convert.ToString(obj);
        }


        /// <summary>
        /// object 转换为 decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static decimal ObjToDecimal(object obj)
        {
            if (obj == null)
                return 0;
            if (obj.Equals(DBNull.Value))
                return 0;

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// object 转换为 decimal?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static decimal? ObjToDecimalNull(object obj)
        {
            if (obj == null)
                return null;
            if (obj.Equals(DBNull.Value))
                return null;

            return ObjToDecimal(obj);
        }


        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static DateTime? ObjToDateNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 数据转换扩展函数

        /// <summary>
        /// object 转换为 int
        /// </summary>
        /// <returns></returns>
        //public static int ToInt(this object obj)
        //{
        //    return ObjToInt(obj);
        //}

        /// <summary>
        /// object 转换为 string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToStr(this object obj)
        {
            return ObjToStr(obj);
        }

        /// <summary>
        /// object 转换为 decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimalbyHLH(this object obj)
        {
            return ObjToDecimal(obj);
        }

        /// <summary>
        /// object 转换为 int?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public static int? ToIntNull(this object obj)
        //{
        //    return ObjToIntNull(obj);
        //}

        /// <summary>
        /// object 转换为 decimal?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNull(this object obj)
        {
            return ObjToDecimalNull(obj);
        }

        /// <summary>
        /// 转换为boolean型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this object obj)
        {
            return ObjToBool(obj);
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ToDateNull(this object obj)
        {
            return ObjToDateNull(obj);
        }

        /// <summary>
        /// 转换为Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this decimal value)
        {
            return Convert.ToDouble(value);
        }
        #endregion
    }
}
