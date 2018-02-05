using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ArrayExtensions
    {
        // Methods
        public static string[] MergerArray(string[] firstArray, string[] secondArray)
        {
            if (firstArray == null)
            {
                return secondArray;
            }
            if (secondArray == null)
            {
                return firstArray;
            }
            string[] array = new string[firstArray.Length + secondArray.Length];
            firstArray.CopyTo(array, 0);
            secondArray.CopyTo(array, firstArray.Length);
            return array;
        }

        public static string[] MergerArray(string[] firstArray, int byteLen)
        {
            string[] strArray;
            if (firstArray.Length > byteLen)
            {
                strArray = new string[byteLen];
                for (int j = 0; j < byteLen; j++)
                {
                    strArray[j] = firstArray[(j + firstArray.Length) - byteLen];
                }
                return strArray;
            }
            strArray = new string[byteLen];
            for (int i = 0; i < byteLen; i++)
            {
                strArray[i] = " ";
            }
            firstArray.CopyTo(strArray, (int)(byteLen - firstArray.Length));
            return strArray;
        }

        public static string[] MergerArray(string[] sourceArray, string str)
        {
            string[] array = new string[sourceArray.Length + 1];
            sourceArray.CopyTo(array, 0);
            array[sourceArray.Length] = str;
            return array;
        }

        public static string[] MergerArrays(this string[] firstArray, string[] secondArray)
        {
            return MergerArray(firstArray, secondArray);
        }

        public static string[] SplitArray(string[] sourceArray, int startIndex, int endIndex)
        {
            string[] strArray2;
            try
            {
                string[] strArray = new string[(endIndex - startIndex) + 1];
                for (int i = 0; i <= (endIndex - startIndex); i++)
                {
                    strArray[i] = sourceArray[i + startIndex];
                }
                strArray2 = strArray;
            }
            catch (IndexOutOfRangeException exception1)
            {
                throw new Exception(exception1.Message);
            }
            return strArray2;
        }
    }
}
