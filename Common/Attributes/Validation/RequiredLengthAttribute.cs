using Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes.Validation
{
	public class RequiredLengthAttribute : RequiredAttribute
	{
		public RequiredLengthAttribute()
		{
			MinLength = 0;
			MaxLength = long.MaxValue;
		}

		public long MaxLength { get; set; }

		public long MinLength { get; set; }

		public override bool IsValid(object value)
		{
			//No detection of NULL values
			if (value == null) return true;
			try
			{
				long count = 0;
				foreach (var i in (IEnumerable)value) count++;
				if (count < MinLength) return false;
				if (count > MaxLength) return false;
			}
			catch
			{
				return false;
			}
			return true;
		}

		public override string FormatErrorMessage(string name)
		{
			string str = string.Empty;
			if (MinLength == 0 && MaxLength == long.MaxValue) return str;
			else if (MinLength > 0 && MaxLength == long.MaxValue) str = "大于等于" + MinLength + "个";
			else if (MinLength == 0 && MaxLength < long.MaxValue) str = "小于等于" + MaxLength + "个";
			else str = "介于" + MinLength + "至" + MaxLength + "个之间";
			return string.Format("{0}选择数量必须{1}", name, str);
		}


	}
}
