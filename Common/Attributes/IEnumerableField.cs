using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
	public interface IEnumerableField
	{
		bool Multi { get; set; }

		char Split { get; set; }
	}
}
