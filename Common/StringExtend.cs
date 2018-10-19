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
		#region MD5

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

        public static string ToMD5(this Stream file)
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

		#endregion

		#region RSA

		public static string RSAEncrypt(this string normalstr)
		{
			var bytes = Encoding.Default.GetBytes(normalstr);
			var encryptBytes = new RSACryptoServiceProvider(new CspParameters()).Encrypt(bytes, false);
			return Convert.ToBase64String(encryptBytes);
		}

		public static string RSADecrypt(this string securityTxt)
		{
			try//必须使用Try catch,不然输入的字符串不是净荷明文程序就Gameover了
			{
				var bytes = Convert.FromBase64String(securityTxt);
				var DecryptBytes = new RSACryptoServiceProvider(new CspParameters()).Decrypt(bytes, false);
				return Encoding.Default.GetString(DecryptBytes);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		#endregion

		#region DES
		/// <summary>
		/// DES default encryption vector
		/// </summary>
		private readonly static byte[] DEFAULT_DES_VECTOR = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

		/// <summary>
		/// DES Encrypt
		/// </summary>
		/// <param name="normalTxt">Raw String</param>
		/// <param name="EncryptKey">Security Key</param>
		/// <param name="Vector">The encryption vector, if default vector is used by default</param>
		/// <returns></returns>
		public static string DesEncrypt(this string normalTxt, string EncryptKey, byte[] Vector = null)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			var key = Encoding.UTF8.GetBytes(EncryptKey.PadLeft(8, '0').Substring(0, 8));
			using (MemoryStream ms = new MemoryStream())
			{
				var encry = new DESCryptoServiceProvider();
				CryptoStream cs = new CryptoStream(ms, encry.CreateEncryptor(key, Vector ?? DEFAULT_DES_VECTOR), CryptoStreamMode.Write);
				cs.Write(bytes, 0, bytes.Length);
				cs.FlushFinalBlock();
				return Convert.ToBase64String(ms.ToArray());
				
			}
		}

		/// <summary>
		/// DES Decrypt
		/// </summary>
		/// <param name="securityTxt">Decrypt String</param>
		/// <param name="EncryptKey">Security Key</param>
		/// <param name="Vector">The encryption vector, if default vector is used by default</param>
		/// <returns></returns>
		public static string DesDecrypt(this string securityTxt, string EncryptKey, byte[] Vector = null)
		{
			try
			{
				var bytes = Convert.FromBase64String(securityTxt);
				var key = Encoding.UTF8.GetBytes(EncryptKey.PadLeft(8, '0').Substring(0, 8));
				using (MemoryStream ms = new MemoryStream())
				{
					var descrypt = new DESCryptoServiceProvider();
					CryptoStream cs = new CryptoStream(ms, descrypt.CreateDecryptor(key, Vector ?? DEFAULT_DES_VECTOR), CryptoStreamMode.Write);
					cs.Write(bytes, 0, bytes.Length);
					cs.FlushFinalBlock();
					return Encoding.UTF8.GetString(ms.ToArray());
				}
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		#endregion

		#region SHA
		public static string SHA1Encrypt(this string normalTxt)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			var SHA = new SHA1CryptoServiceProvider();
			var encryptbytes = SHA.ComputeHash(bytes);
			return Convert.ToBase64String(encryptbytes);
		}
		public static string SHA256Encrypt(this string normalTxt)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			var SHA256 = new SHA256CryptoServiceProvider();
			var encryptbytes = SHA256.ComputeHash(bytes);
			return Convert.ToBase64String(encryptbytes);
		}
		public static string SHA384Encrypt(this string normalTxt)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			var SHA384 = new SHA384CryptoServiceProvider();
			var encryptbytes = SHA384.ComputeHash(bytes);
			return Convert.ToBase64String(encryptbytes);
		}
		public static string SHA512Encrypt(this string normalTxt)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			var SHA512 = new SHA512CryptoServiceProvider();
			var encryptbytes = SHA512.ComputeHash(bytes);
			return Convert.ToBase64String(encryptbytes);
		}

		#endregion

		#region AES

		/// <summary>
		/// AES default encryption vector
		/// </summary>
		private readonly static byte[] DEFAULT_AES_VECTOR = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x05, 0x07, 0x08 };

		/// <summary>
		/// 
		/// </summary>
		/// <param name="normalTxt"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string AESEncrypt(this string normalTxt, string key)
		{
			var bytes = Encoding.Default.GetBytes(normalTxt);
			SymmetricAlgorithm des = Rijndael.Create();
			des.IV = DEFAULT_AES_VECTOR;
			des.Key = Encoding.Default.GetBytes(key);
			
			using (MemoryStream ms = new MemoryStream())
			{
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
				cs.Write(bytes, 0, bytes.Length);
				cs.FlushFinalBlock();
				return Convert.ToBase64String(ms.ToArray());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="securityTxt"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string AESDecrypt(this string securityTxt, string key)
		{
			try
			{
				var bytes = Convert.FromBase64String(securityTxt);
				SymmetricAlgorithm des = Rijndael.Create();
				des.Key = Encoding.Default.GetBytes(key);
				des.IV = DEFAULT_AES_VECTOR;
				using (MemoryStream ms = new MemoryStream())
				{
					CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
					cs.Write(bytes, 0, bytes.Length);
					cs.FlushFinalBlock();
					return Convert.ToBase64String(ms.ToArray());
				}
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		#endregion

		#region RC4

		/// <summary>RC4加密算法
		/// 返回进过rc4加密过的字符
		/// </summary>
		/// <param name="str">被加密的字符</param>
		/// <param name="ckey">密钥</param>
		public static string RC4Encrypt(this string str, string ckey)
		{
			int[] s = new int[256];
			for (int i = 0; i < 256; i++)
			{
				s[i] = i;
			}
			//密钥转数组
			char[] keys = ckey.ToCharArray();//密钥转字符数组
			int[] key = new int[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				key[i] = keys[i];
			}
			//明文转数组
			char[] datas = str.ToCharArray();
			int[] mingwen = new int[datas.Length];
			for (int i = 0; i < datas.Length; i++)
			{
				mingwen[i] = datas[i];
			}

			//通过循环得到256位的数组(密钥)
			int j = 0;
			int k = 0;
			int length = key.Length;
			int a;
			for (int i = 0; i < 256; i++)
			{
				a = s[i];
				j = (j + a + key[k]);
				if (j >= 256)
				{
					j = j % 256;
				}
				s[i] = s[j];
				s[j] = a;
				if (++k >= length)
				{
					k = 0;
				}
			}
			//根据上面的256的密钥数组 和 明文得到密文数组
			int x = 0, y = 0, a2, b, c;
			int length2 = mingwen.Length;
			int[] miwen = new int[length2];
			for (int i = 0; i < length2; i++)
			{
				x = x + 1;
				x = x % 256;
				a2 = s[x];
				y = y + a2;
				y = y % 256;
				s[x] = b = s[y];
				s[y] = a2;
				c = a2 + b;
				c = c % 256;
				miwen[i] = mingwen[i] ^ s[c];
			}
			//密文数组转密文字符
			char[] mi = new char[miwen.Length];
			for (int i = 0; i < miwen.Length; i++)
			{
				mi[i] = (char)miwen[i];
			}
			return new string(mi);
		}

		/// <summary>RC4解密算法
		/// 返回进过rc4解密过的字符
		/// </summary>
		/// <param name="str">被解密的字符</param>
		/// <param name="ckey">密钥</param>
		public static string RC4Decrypt(this string str, string ckey)
		{
			int[] s = new int[256];
			for (int i = 0; i < 256; i++)
			{
				s[i] = i;
			}
			//密钥转数组
			char[] keys = ckey.ToCharArray();//密钥转字符数组
			int[] key = new int[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				key[i] = keys[i];
			}
			//密文转数组
			char[] datas = str.ToCharArray();
			int[] miwen = new int[datas.Length];
			for (int i = 0; i < datas.Length; i++)
			{
				miwen[i] = datas[i];
			}

			//通过循环得到256位的数组(密钥)
			int j = 0;
			int k = 0;
			int length = key.Length;
			int a;
			for (int i = 0; i < 256; i++)
			{
				a = s[i];
				j = (j + a + key[k]);
				if (j >= 256)
				{
					j = j % 256;
				}
				s[i] = s[j];
				s[j] = a;
				if (++k >= length)
				{
					k = 0;
				}
			}
			//根据上面的256的密钥数组 和 密文得到明文数组
			int x = 0, y = 0, a2, b, c;
			int length2 = miwen.Length;
			int[] mingwen = new int[length2];
			for (int i = 0; i < length2; i++)
			{
				x = x + 1;
				x = x % 256;
				a2 = s[x];
				y = y + a2;
				y = y % 256;
				s[x] = b = s[y];
				s[y] = a2;
				c = a2 + b;
				c = c % 256;
				mingwen[i] = miwen[i] ^ s[c];
			}
			//明文数组转明文字符
			char[] ming = new char[mingwen.Length];
			for (int i = 0; i < mingwen.Length; i++)
			{
				ming[i] = (char)mingwen[i];
			}
			string mingwenstr = new string(ming);
			return mingwenstr;
		}
		#endregion

		public static string EscapeBase64SpeChar(this string str)
		{
			return str.Replace("+", "-").Replace("/", "_").Replace("=","!");
		}

		public static string RecoveryBase64SpeChar(this string str)
		{
			return str.Replace("-", "+").Replace("_", "/").Replace("!", "=");
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

		public static char Maxchar(this string str)
		{
			var temp = str;
			var maxcount = int.MinValue;
			char maxchr = char.MinValue;
			while (temp.Length > maxcount)
			{
				var chr = temp[0];
				var tmp = temp.Replace(chr.ToString(), "");
				var num = temp.Length - tmp.Length;
				if (num > maxcount)
				{
					maxcount = num;
					maxchr = chr;
				}
				temp = tmp;
			}
			return maxchr;
		}
    }
}
