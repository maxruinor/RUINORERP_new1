using System.Collections.Generic;

namespace HLH.Lib.Helper
{
    public class FieldValueSplitClass
    {

        /// <summary>
        /// 以成对引号内数据外的逗号分割数据
        /// </summary>
        /// <param name="matchStr"></param>
        /// <returns></returns>
        // "sad,fas","aab",,"",66,
        public static List<string> GetPairsMatch(string matchStr)
        {
            string rs = string.Empty;
            List<string> mylist = new List<string>();
            List<char> chars = new List<char>();
            int counter = 0;
            for (int i = 0; i < matchStr.Length; i++)
            {

                char current = matchStr[i];
                switch (current)
                {
                    case '"':
                        //chars.Add(current);
                        counter++;
                        break;
                    case ',':
                        //中间了，前面的是一个字段。但是需要先看引号成对。要么没有。
                        int ys = counter % 2;
                        if (ys == 0)
                        {
                            //前面的为一个值
                            rs = new string(chars.ToArray());
                            chars.Clear();
                            counter = 0;
                            mylist.Add(rs);
                        }
                        else
                        {
                            chars.Add(current);
                        }
                        break;
                    default:
                        chars.Add(current);
                        break;
                }

                ///最后一个字符了。
                if (i == matchStr.Length - 1)
                {
                    //前面的为一个值
                    rs = new string(chars.ToArray());
                    chars.Clear();
                    counter = 0;
                    mylist.Add(rs);
                }
            }

            return mylist;
        }







        #region
        public static bool IsBracketsMatch(string matchStr)
        {
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < matchStr.Length; i++)
            {
                char current = matchStr[i];
                switch (current)
                {
                    case '(':
                    case '[':
                    case '{':
                        stack.Push(current);
                        break;
                    case ')':
                    case ']':
                    case '}':
                        if (stack.Count <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            char top = stack.Peek();
                            if (IsCouple(top, current))
                            {
                                stack.Pop();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (stack.Count <= 0) return true;
            return false;
        }

        public static bool IsCouple(char left, char right)
        {
            if (left == '(' && right == ')')
            {
                return true;
            }
            if (left == '[' && right == ']')
            {
                return true;
            }
            if (left == '{' && right == '}')
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
