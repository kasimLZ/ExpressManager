using Common.Attributes;
using Common.Attributes.Validation;
using Common.Linq;
using DataBase.Base.Infrastructure.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Helper
{
	public static class ModelHelper
	{
		public static void UpdateModel(object model, NameValueCollection collection)
		{
			foreach (var prop in model.GetType().GetProperties().Where(p => p.PropertyType.Equals(typeof(string)) && p.CustomAttributes.Any(a => typeof(IEnumerableField).IsAssignableFrom(a.AttributeType))))
			{
				var attribute = prop.GetAttribute<IEnumerableField>();
				if (attribute != null && attribute.Multi)
				{
					prop.SetValue(model, collection[prop.Name]?.Replace(',', attribute.Split));
				}
			}

			foreach (var prop in model.GetType().GetProperties().Where(p => p.CustomAttributes.Any(a => typeof(RequiredLengthAttribute).IsAssignableFrom(a.AttributeType))))
			{
				if (prop.GetValue(model) == null)
				{
					prop.SetValue(model, prop.PropertyType.CreateInstance());
				}
			}
		}

		public static object GetViewDict(this PropertyInfo prop, object value)
		{
			object result = null;
			var Trans = prop.GetAttribute<ITranslatable>();

			if (Trans is SelectListAttribute)
			{
				result = prop.StuffSelectViewData(value);
			}
			else if (Trans is UserDictAttribute)
			{
				result = prop.StuffUserDictViewData(value);
			}
			else
			{
				ITranslatable translatable = prop.GetAttribute<ITranslatable>();
				if (translatable == null || (translatable.Mode & TranslatMode.Dict) == 0 || translatable.ModelType == null) goto Trans_Finish;

				Type modelType = translatable.ModelType;
				if (modelType == null)
				{
					var datafor = prop.GetAttribute<DataForAttribute>();
					if (datafor != null)
					{
						modelType = prop.DeclaringType.GetProperty(datafor.DataScoure)?.PropertyType.GetElementType();
					}
				}
				if (modelType == null) goto Trans_Finish;

				var targetModel = translatable.ModelType.CreateInstance();
				var Repository = EntityExtensions.RepositoryFactory(targetModel as dynamic);

				if (Repository == null) goto Trans_Finish;

				var key = translatable.Key ?? translatable.ModelType.IdentifierPropertyName();
				dynamic target = targetModel.GetType().GetProperty(key).PropertyType.CreateInstance();
				IEnumerable list = value is IEnumerable ? value as IEnumerable : new dynamic[] { value };
				result = DoQuery(Repository, Convertlist(list, target), key, translatable.Value);
			}

			Trans_Finish:
			return result;
		}

		private static dynamic Convertlist<T>(IEnumerable list, T _target)
		{
			return list.ConvertIEnumerable<T>();
		}

		private static object DoQuery<TModel, TValue>(IRepositoryBase<TModel> repository, IEnumerable<TValue> Values, string key, string value) where TModel : class
		{
			var query = repository.GetAll();
			if (Values != null)
				query = query.Where(EntityExtensions.Contains<TModel, TValue>(key, Values));
			var propKey = typeof(TModel).GetProperty(key);
			var propValue = typeof(TModel).GetProperty(value);
			return query.ToList().ToDictionary(k => propKey.GetValue(k).ToString(), v => propValue.GetValue(v).ToString());

		}

		public static void SupplyViewData<TModel>(ViewDataDictionary ViewData, TModel model = null) where TModel : class, new()
		{
			foreach (var prop in typeof(TModel).GetProperties().Where(p => p.PropertyType.Equals(typeof(string)) && p.CustomAttributes.Any(a => typeof(ITranslatable).IsAssignableFrom(a.AttributeType))))
			{
				if (ViewData[prop.Name + "_Name"] == null)
				{
					Type type = (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string)) ? prop.GetElementType() : prop.PropertyType;
					ViewData[prop.Name + "_Name"] = GetViewDict(prop, ValueList(prop.GetAttribute<ITranslatable>(), model ?? new TModel(), type.CreateInstance() as dynamic, prop));
				}
			}
		}

		private static List<T> ValueList<T>(ITranslatable trans, object model, T _value, PropertyInfo info)
		{
			List<T> list = new List<T>();
			var value = info.GetValue(model);
			if (trans is IEnumerableField)
			{
				var ef = trans as IEnumerableField;
				if (value is string && ef.Multi)
					list.AddRange(value.ToString().Split(ef.Split).ConvertIEnumerable<T>());
				else if (model is IEnumerable)
					list.AddRange((model as IEnumerable).ConvertIEnumerable<T>());
				else
					try { list.Add((T)model); } catch { }

			}
			else
			{
				try { list.Add((T)model); } catch { }
			}
			return list.Distinct().ToList();
		}
	}
}