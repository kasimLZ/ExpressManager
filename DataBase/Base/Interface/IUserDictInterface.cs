using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace DataBase.Base.Interface
{
    public interface IUserDictInterface : IRepositoryBase<UserDict>
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="selectedValue"></param>
		/// <returns></returns>
		SelectList SelectListByType(string dictType, object selectedValue = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="dictValue"></param>
		/// <returns></returns>
		string GetDictText(string dictType, string dictValue);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="dictValue"></param>
		/// <returns></returns>
		UserDict GetByTypeValue(string dictType, string dictValue);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="selectedValues"></param>
		/// <returns></returns>
		MultiSelectList SMultiSelectListByType(string dictType, IEnumerable selectedValues = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="selectedValues"></param>
		/// <returns></returns>
		IQueryable<UserDict> GetSelected(string dictType, object selectedValues = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictType"></param>
		/// <param name="selectedValues"></param>
		/// <returns></returns>
		string GetSelectedText(string dictType, object selectedValues = null);
	}
}
