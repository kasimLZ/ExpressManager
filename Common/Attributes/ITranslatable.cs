using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
	public interface ITranslatable
	{
		TranslatMode Mode { get; set; }

		Type ModelType { get; set; }

		string Key { get; set; }

		string Value { get; set; }

		string FunctionOrPropertyName { get; set; }
	}

	[Flags]
	public enum TranslatMode
	{
		/// <summary>
		/// No translation
		/// </summary>
		None = 0,

		/// <summary>
		/// Get a comparison dictionary from the specified service, the calling method needs to be able to use without parameters
		/// </summary>
		Dict = 1,

		/// <summary>
		/// To obtain content indirectly through model attributes, you need to specify an attribute of type ICollection under the current model as the content field.
		/// </summary>
		Proxy = 2,

		/// <summary>
		/// 
		/// </summary>
		Property = 4
	}
}
