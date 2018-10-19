using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace DataBase.Base.Service
{
    public class UserDictService : RepositoryBase<UserDict>, IUserDictInterface
    {
        public UserDictService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }

		public UserDict GetByTypeValue(string dictType, string dictValue)
		{
			return GetAll(a => a.DictType.DictTypeName == dictType && a.Value == dictValue).FirstOrDefault();
		}

		public string GetDictText(string dictType, string dictValue)
		{
			var dict = GetByTypeValue(dictType, dictValue);
			return dict != null ? dict.Text : string.Empty;
		}

		public IQueryable<UserDict> GetSelected(string dictType, object selectedValues = null)
		{
			var select = GetAll(a => a.DictType.DictType.Equals(dictType) && a.IsEnable && a.DictType.IsEnable);
			if (selectedValues != null)
			{
				 IList selectitem =
					selectedValues is string ?
					(selectedValues as string).Split(',','\\','/','s'):
					selectedValues as IList;
				if (selectitem != null && selectitem.Count > 0) {
					select = select.Where(a => selectitem.Contains(a.Value));
				}
			}
			return select;
		}

		public string GetSelectedText(string dictType, object selectedValues = null)
		{
		 	return string.Join(" ", GetSelected(dictType, selectedValues).Select(a => a.Text));
		}

		public SelectList SelectListByType(string dictType, object selectedValue = null)
		{
			var model = GetSelected(dictType).OrderBy(a => a.Sort).ThenByDescending(a => a.CreatedDate).Select(a => new SelectListItem { Value = a.Value, Text = a.Text, Selected = a.IsDefault }).ToList();
			model.Insert(0, new SelectListItem { Value = null, Text = "---请选择---", Selected = !model.Any(a => a.Selected) });
			return new SelectList(model, "Value", "Text", selectedValue ??  model.First(a => a.Selected).Value);
		}

		public MultiSelectList SMultiSelectListByType(string dictType, IEnumerable selectedValues = null)
		{
			return new MultiSelectList(GetSelected(dictType), "Value", "Text", selectedValues);
		}
	}
}
