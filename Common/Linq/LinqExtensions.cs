using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Linq
{
    public static class LinqExtensions
    {
        public static Type GetPropertyType(object model, string property)
        {
            if (property.Contains("."))
            {
                string mainKey = property.Substring(0, property.IndexOf("."));
                string str = property.Substring(property.IndexOf(".") + 1);
                if (model.GetType().GetProperties().Any(item => item.Name == mainKey))
                {
                    return GetPropertyType(Activator.CreateInstance(model.GetType().GetProperties().First(item => (item.Name == mainKey)).PropertyType), str);
                }
                if ((model.GetType().GetGenericArguments().Count() > 0) && model.GetType().GetGenericArguments()[0].GetProperties().Any(item => (item.Name == mainKey)))
                {
                    return GetPropertyType(Activator.CreateInstance(model.GetType().GetGenericArguments()[0].GetProperties().First(item => (item.Name == mainKey)).PropertyType), str);
                }
                return null;
            }
            if (model.GetType().GetProperties().Any(item => item.Name == property))
            {
                return model.GetType().GetProperties().First(item => (item.Name == property)).PropertyType;
            }
            if ((model.GetType().GetGenericArguments().Count() > 0) && model.GetType().GetGenericArguments()[0].GetProperties().Any(item => (item.Name == property)))
            {
                return model.GetType().GetGenericArguments()[0].GetProperties().First(item => (item.Name == property)).PropertyType;
            }
            return null;
        }


        public static IQueryable<T> Search<T>(this IQueryable<T> model, NameValueCollection collection)
        {
            if (collection.Count == 0) return model;
                foreach (string str in collection.AllKeys)
                {
                    string str2 = collection[str].Trim();
                    if (!string.IsNullOrEmpty(str2))
                    {
                        Guid guid;
                        string predicate = "";
                        Type propertyType = GetPropertyType(model, str);
                        if (propertyType != null)
                        {
                            if (propertyType == typeof(string))
                            {
                                predicate = str + ".Contains(@0)";
                                object[] values = new object[] { str2 };
                                model = model.Where<T>(predicate, values);
                            }
                            else
                            {
                                int num2;
                                if ((propertyType == typeof(int)) && int.TryParse(str2, out num2))
                                {
                                    predicate = str + "==@0";
                                    object[] objArray2 = new object[] { num2 };
                                    model = model.Where<T>(predicate, objArray2);
                                }
                                else if ((propertyType == typeof(Guid)) && Guid.TryParse(str2, out guid))
                                {
                                    predicate = str + "==@0";
                                    object[] objArray3 = new object[] { guid };
                                    model = model.Where<T>(predicate, objArray3);
                                }
                                else if (propertyType == typeof(Guid?))
                                {
                                    if (str2 != "null")
                                    {
                                        predicate = str + ".Value.ToString()==@0";
                                        object[] objArray4 = new object[] { str2 };
                                        model = model.Where<T>(predicate, objArray4);
                                    }
                                    else
                                    {
                                        predicate = str + ".HasValue==false";
                                        object[] objArray5 = new object[] { str2 };
                                        model = model.Where<T>(predicate, objArray5);
                                    }
                                }
                                else
                                {
                                    bool flag;
                                    if ((propertyType == typeof(bool)) && bool.TryParse(str2, out flag))
                                    {
                                        predicate = str + "==@0";
                                        object[] objArray6 = new object[] { flag };
                                        model = model.Where<T>(predicate, objArray6);
                                    }
                                    else if (((propertyType == typeof(DateTime)) || (propertyType == typeof(DateTime?))) && str2.Contains(" 到 "))
                                    {
                                        DateTime time;
                                        DateTime time2;
                                        string[] separator = new string[] { " 到 " };
                                        string[] strArray2 = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                        if (((strArray2.Length == 2) && DateTime.TryParse(strArray2[0], out time)) && DateTime.TryParse(strArray2[1], out time2))
                                        {
                                            time2 = time2.AddDays(1.0);
                                            predicate = str + ">= @0 and " + str + " < @1";
                                            object[] objArray7 = new object[] { time, time2 };
                                            model = model.Where<T>(predicate, objArray7);
                                        }
                                    }
                                }
                            }
                        }
                        else if (str.Contains("[") && str.Contains("]"))
                        {
                            string property = str.Substring(0, str.IndexOf("["));
                            string str5 = str.Substring(str.IndexOf("[") + 1, (str.IndexOf("]") - str.IndexOf("[")) - 1);
                            Type collectionType = GetPropertyType(model, property);
                            if (collectionType != null)
                            {
                                Type collectionItemType = ObjectUtils.GetCollectionItemType(collectionType);
                                if (collectionItemType != null)
                                {
                                    Type type4 = ObjectUtils.GetPropertyType(collectionItemType, str5);
                                    if (type4 != null)
                                    {
                                        if ((type4 == typeof(Guid)) && Guid.TryParse(str2, out guid))
                                        {
                                            predicate = property + ".Where(" + str5 + "==@0 && Deleted == false).Count() > 0";
                                            object[] objArray8 = new object[] { guid };
                                            model = model.Where<T>(predicate, objArray8);
                                        }
                                        else if (type4 == typeof(Guid?))
                                        {
                                            predicate = property + ".Where(" + str5 + ".Value.ToString()==@0 && Deleted == false).Count() > 0";
                                            object[] objArray9 = new object[] { str2 };
                                            model = model.Where<T>(predicate, objArray9);
                                        }
                                        else
                                        {
                                            predicate = property + ".Where(" + str5 + "==@0 && Deleted == false).Count() > 0";
                                            object[] objArray10 = new object[] { str2 };
                                            model = model.Where<T>(predicate, objArray10);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            return model;
        }

    }
}
