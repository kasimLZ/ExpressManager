using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public static class StringExtend
    {
        public static string ToMD5(this string str)
        {
            return Encoding.UTF8.GetBytes(str).ToMD5();
        }
        public static string ToMD5(this byte[] bytes)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                StringBuilder builder = new StringBuilder();
                bytes = provider.ComputeHash(bytes);
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2").ToLower());
                return builder.ToString();
            }
        }
        public static string ToMD5(Stream file)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] data = md5.ComputeHash(file);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
            RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
            RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            return Htmlstring;
        }

        public static String ViewElementId(this string name)
        {
            int sec = DateTime.Now.Millisecond;
            long number = new Random(sec).Next(0, int.MaxValue);
            return name + (string.IsNullOrEmpty(name) ?"" : "_") + (((long)sec << BitConverter.GetBytes(number).Length) | number).ToString();
        }


    }
}
