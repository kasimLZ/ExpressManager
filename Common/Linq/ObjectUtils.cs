using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Linq
{
	public static class ObjectUtils
	{
		public static Type GetCollectionItemType(Type collectionType)
		{
			Type[] typeArray = collectionType.GetInterfaces().Where(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IEnumerable<>)).ToArray();
			if (typeArray.Length != 1)
			{
				return null;
			}
			return typeArray[0].GetGenericArguments()[0];
		}

		public static ModelMetadata GetModelProperty(ModelMetadata metadata, string propertyName)
		{
			if (propertyName.Contains("[") && propertyName.Contains("]"))
			{
				string foreignKey = propertyName.Substring(0, propertyName.IndexOf("["));
				string str = propertyName.Substring(propertyName.IndexOf("[") + 1, (propertyName.IndexOf("]") - propertyName.IndexOf("[")) - 1);
				ModelMetadata metadata2 = (from pm in metadata.Properties
										   where pm.PropertyName.Equals(foreignKey)
										   select pm).SingleOrDefault<ModelMetadata>();
				if (metadata2 != null)
				{
					Type collectionItemType = GetCollectionItemType(metadata2.ModelType);
					if (collectionItemType != null)
					{
						return GetModelProperty(ModelMetadataProviders.Current.GetMetadataForType(null, collectionItemType), str);
					}
				}
				return null;
			}
			if (!propertyName.Contains("."))
			{
				return (from pm in metadata.Properties
						where pm.PropertyName.Equals(propertyName)
						select pm).SingleOrDefault<ModelMetadata>();
			}
			string mainProperty = propertyName.Substring(0, propertyName.IndexOf("."));
			string str2 = propertyName.Substring(propertyName.IndexOf(".") + 1);
			ModelMetadata metadata3 = (from pm in metadata.Properties
									   where pm.PropertyName.Equals(mainProperty)
									   select pm).SingleOrDefault<ModelMetadata>();
			if (metadata3 != null)
			{
				return GetModelProperty(metadata3, str2);
			}
			return null;
		}

		public static Type GetPropertyType(object model, string property)
		{
			if (property.Contains("."))
			{
				string mainKey = property.Substring(0, property.IndexOf("."));
				string str = property.Substring(property.IndexOf(".") + 1);
				if (model.GetType().GetProperties().Any<PropertyInfo>(item => item.Name == mainKey))
				{
					return GetPropertyType(Activator.CreateInstance(model.GetType().GetProperties().First<PropertyInfo>(item => (item.Name == mainKey)).PropertyType), str);
				}
				if ((model.GetType().GetGenericArguments().Count<Type>() > 0) && model.GetType().GetGenericArguments()[0].GetProperties().Any<PropertyInfo>(item => (item.Name == mainKey)))
				{
					return GetPropertyType(Activator.CreateInstance(model.GetType().GetGenericArguments()[0].GetProperties().First<PropertyInfo>(item => (item.Name == mainKey)).PropertyType), str);
				}
				return null;
			}
			if (model.GetType().GetProperties().Any<PropertyInfo>(item => item.Name == property))
			{
				return model.GetType().GetProperties().First<PropertyInfo>(item => (item.Name == property)).PropertyType;
			}
			if ((model.GetType().GetGenericArguments().Count<Type>() > 0) && model.GetType().GetGenericArguments()[0].GetProperties().Any<PropertyInfo>(item => (item.Name == property)))
			{
				return model.GetType().GetGenericArguments()[0].GetProperties().First<PropertyInfo>(item => (item.Name == property)).PropertyType;
			}
			return null;
		}

		public static Type GetPropertyType(Type modelType, string property)
		{
			if (property.Contains("."))
			{
				string mainKey = property.Substring(0, property.IndexOf("."));
				string str = property.Substring(property.IndexOf(".") + 1);
				if (modelType.GetProperties().Any<PropertyInfo>(item => item.Name == mainKey))
				{
					return GetPropertyType(Activator.CreateInstance(modelType.GetProperties().First<PropertyInfo>(item => (item.Name == mainKey)).PropertyType), str);
				}
				if ((modelType.GetGenericArguments().Count<Type>() > 0) && modelType.GetGenericArguments()[0].GetProperties().Any<PropertyInfo>(item => (item.Name == mainKey)))
				{
					return GetPropertyType(Activator.CreateInstance(modelType.GetGenericArguments()[0].GetProperties().First<PropertyInfo>(item => (item.Name == mainKey)).PropertyType), str);
				}
				return null;
			}
			if (modelType.GetProperties().Any<PropertyInfo>(item => item.Name == property))
			{
				return modelType.GetProperties().First<PropertyInfo>(item => (item.Name == property)).PropertyType;
			}
			if ((modelType.GetGenericArguments().Count<Type>() > 0) && modelType.GetGenericArguments()[0].GetProperties().Any<PropertyInfo>(item => (item.Name == property)))
			{
				return modelType.GetGenericArguments()[0].GetProperties().First<PropertyInfo>(item => (item.Name == property)).PropertyType;
			}
			return null;
		}

		public static object GetPropertyValue(object srcObj, string propertyName)
		{
			if (srcObj == null)
			{
				return null;
			}
			PropertyInfo property = srcObj.GetType().GetProperty(propertyName.Replace("[]", ""));
			if (property == null)
			{
				return null;
			}
			return property.GetValue(srcObj);
		}

		public static object GetPropValue(this object obj, string name)
		{
			char[] separator = new char[] { '.' };
			foreach (string str in name.Split(separator))
			{
				if (obj == null)
				{
					return null;
				}
				PropertyInfo property = obj.GetType().GetProperty(str);
				if (property == null)
				{
					return null;
				}
				obj = property.GetValue(obj, null);
			}
			return obj;
		}
		public static IEnumerable<T> ConvertIEnumerable<T>(this IEnumerable list)
		{
			List<T> Container = new List<T>();
			foreach (var item in list)
			{
				try { Container.Add((T)Convert.ChangeType(item, typeof(T))); } catch { }
			}
			return Container;
		}
	}
}


