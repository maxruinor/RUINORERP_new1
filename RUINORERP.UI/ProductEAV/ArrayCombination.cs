using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 多属性组合类
    /// </summary>
    public class ArrayCombination
    {
        struct Matrix
        {
            private string[] element;
            public string[] Element
            {
                get { return element == null ? new string[0] : element; }
                set { element = value; }
            }
            public Matrix(string[] s)
            {
                element = s;
            }
            public Matrix Multiply(Matrix m)
            {
                string[] s = new string[this.element.Length * m.element.Length];
                int index = 0;
                for (int i = 0; i < this.element.Length; i++)
                {
                    for (int j = 0; j < m.element.Length; j++)
                    {
                        s[index] = this.element[i] + "," + m.element[j];
                        index++;
                    }
                }
                return new Matrix(s);
            }
        }

        public static void PrintResult(params string[][] arrays)
        {
            Matrix m = new Matrix();
            foreach (string[] array in arrays)
            {
                if (m.Element.Length == 0)
                    m = new Matrix(array);
                else
                    m = m.Multiply(new Matrix(array));
            }
            foreach (string s in m.Element)
                System.Diagnostics.Debug.WriteLine(s);
        }

        public static List<string> Combination(List<string[]> arrays)
        {
            List<string> rs = new List<string>();

            Matrix m = new Matrix();
            foreach (string[] array in arrays)
            {
                if (m.Element.Length == 0)
                    m = new Matrix(array);
                else
                    m = m.Multiply(new Matrix(array));
            }
            foreach (string s in m.Element)
            {
                rs.Add(s);
            }
            return rs;
        }


        /// <summary>
        /// 将数组组合成串
        /// </summary>
        /// <param name="mylist"></param>
        /// <returns></returns>
        public static List<string> Combination(List<List<string>> mylist)
        {
            List<string> rs = new List<string>();

            Matrix m = new Matrix();
            foreach (List<string> array in mylist)
            {
                if (m.Element.Length == 0)
                    m = new Matrix(array.ToArray());
                else
                    m = m.Multiply(new Matrix(array.ToArray()));
            }
            foreach (string s in m.Element)
            {
                rs.Add(s);
            }
            return rs;
        }

        /// <summary>
        /// 排列组合
        /// </summary>
        /// <param name="kvps"></param>
        /// <returns></returns>
        public static List<string> Combination4Table(List<KeyValuePair<long, string[]>> kvps)
        {
            List<string[]> arrays = new List<string[]>();
            kvps.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
            foreach (KeyValuePair<long, string[]> kv in kvps)
            {
                if (kv.Value != null)
                {
                    if (kv.Value.Length > 0)
                    {
                        arrays.Add(kv.Value);
                    }
                }
            }


            List<string> rs = new List<string>();

            Matrix m = new Matrix();
            foreach (string[] array in arrays)
            {
                if (m.Element.Length == 0)
                    m = new Matrix(array);
                else
                    m = m.Multiply(new Matrix(array));
            }
            foreach (string s in m.Element)
            {
                rs.Add(s);
            }
            return rs;
        }
        //static void Main(string[] args)
        //{
        //    string[] s1 = { "红色", "蓝色", "白色" };
        //    string[] s2 = { "大", "中", "小" };
        //    string[] s3 = { "10克", "20克", "30克" };
        //    PrintResult(s1);
        //    System.Diagnostics.Debug.WriteLine("--------------------------");
        //    PrintResult(s1, s2);
        //    System.Diagnostics.Debug.WriteLine("--------------------------");
        //    PrintResult(s1, s2, s3);

        //}
    }
}