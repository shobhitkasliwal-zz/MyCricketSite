using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public static class MyExtensionMethods
    {
        public static string ReplaceNull(this Object obj, string ret = "")
        {
            return ((obj != null && !DBNull.Value.Equals(obj)) ? obj.ToString() : "");
        }

        public static bool IsNull(this Object obj, Object ret = null)
        {
            return (obj != null && !DBNull.Value.Equals(obj));
        }

        public static string Right(this string s, int count)
        {
            string newString = String.Empty;
            if (s != null && count > 0)
            {
                int startIndex = s.Length - count;
                if (startIndex > 0)
                    newString = s.Substring(startIndex, count);
                else
                    newString = s;
            }
            return newString;
        }

        public static string Left(this string s, int count)
        {
            if (count == 0 || s.Length == 0)
                return "";
            else if (s.Length <= count)
                return s;
            else
                return s.Substring(0, count);
        }
    }
