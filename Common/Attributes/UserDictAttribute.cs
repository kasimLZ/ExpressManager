using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class UserDictAttribute: DataTypeAttribute , IEnumerableField, ITranslatable
	{

		public UserDictAttribute(string Type ,bool multi = false) : base("UserDict")
		{
			DictType = Type;
			Multi = multi;
		}

		public string DictType { get; set; }

		public bool Multi { get; set; }


		public char Split { get; set; } = ',';

		public string Key { get; set; } = "Value";

		string ITranslatable.FunctionOrPropertyName { get; set; }

		TranslatMode ITranslatable.Mode { get; set; } = TranslatMode.Dict;

		Type ITranslatable.ModelType { get; set; } = null;

		string ITranslatable.Value { get; set; } = null;
	}
}
