using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public static class DateTimeExtensions
	{
		private readonly static DateTime UtcZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long UnixStamp()
		{
			return UnixStamp(DateTime.UtcNow);
		}

		public static long UnixStamp(this DateTime dateTime)
		{
			return Convert.ToInt64((DateTime.UtcNow - UtcZero).TotalSeconds * 1000);
		}

		public static DateTime DecryptUnixStamp(this long code)
		{
		    return UtcZero.Date.AddMilliseconds(code).ToLocalTime();
		}
		
	}
}
