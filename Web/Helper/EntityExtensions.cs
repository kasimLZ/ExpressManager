using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Common.Attributes;
using System.Linq.Expressions;
using System.Collections;
using Common.Linq;
using System.Web.Mvc;
using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Web.Helpers;

namespace Web.Helper
{
    public static class EntityExtensions
    {
        public static void FillModelProperty<T>(this T Model) where T : class
        {
            var ModelType = Model.GetType();
			foreach (var prop in Model.GetUpdataProperties())
				prop.FillProptype(Model);
        }

		public static void FillProptype<T>(this PropertyInfo prop, T Model) where T: class
		{
			DataForAttribute dataFor = prop.GetAttribute<DataForAttribute>();
			if (dataFor == null) return;
			if (prop.GetValue(Model) == null) prop.SetValue(Model, Activator.CreateInstance(prop.PropertyType));
			if (string.IsNullOrWhiteSpace(dataFor.DataScoure) || string.IsNullOrEmpty(dataFor.Value)) return;
			var Source = Model.GetType().GetProperty(dataFor.DataScoure).GetValue(Model) as IEnumerable<dynamic>;
			if (Source == null) return;
			prop.SetValue(Model, GetEnumerableItemValue(Source, (dynamic)prop.GetValue(Model), dataFor.Value));
		}

        public static List<T> GetEnumerableItemValue<T>(IEnumerable<dynamic> DataSource, List<T> Target, string SourceProperty)
        {
            try 
            {
                return DataSource.Select(a => (T)ObjectUtils.GetPropValue(a, SourceProperty)).ToList();
            }
            catch(Exception)
            {
                return new List<T>();
            }
        }

        public static List<T> GetEnumerableItemValue<T>(IEnumerable<dynamic> DataSource, string SourceProperty)
        {
            return GetEnumerableItemValue(DataSource, new List<T>(), SourceProperty);
        }

        public static void UpdateProperty<TModel>(this TModel Model) where TModel : DbSetBase
        {
            var ModelType = Model.GetType();

            foreach (var prop in Model.GetUpdataProperties())
            {
                DataForAttribute dataFor = prop.GetAttribute<DataForAttribute>();
				if (dataFor.Mode.Equals(UpdateMode.None)) continue;

                var TargetProperty = ModelType.GetProperty(dataFor.DataScoure.Split('.')[0]);
                if (TargetProperty == null) continue;

                UpdateProperty(Model, (dynamic)Activator.CreateInstance(TargetProperty.PropertyType.GenericTypeArguments[0]), 
                    (dynamic)prop.GetValue(Model) ?? ((dynamic)Activator.CreateInstance(prop.PropertyType)), prop, prop.GetAttribute<DataForAttribute>());
            }
        }

        public static void UpdateProperty<TModel, FKModel, Data>(this TModel Model, FKModel ForeignKeyModel, List<Data> DataList, PropertyInfo Prop, DataForAttribute DataFor) 
            where TModel : DbSetBase, new()
            where FKModel : DbSetBase, new()
        {
            if (DataFor.Mode.Equals(UpdateMode.None)) return;
			
            var TargetProperty = Model.GetType().GetProperty(DataFor.DataScoure);
            if (TargetProperty == null) return;

            var TargetModelType = typeof(FKModel);
            var Repository = DependencyResolver.Current.GetService<IRepositoryBase<FKModel>>();
            if (Repository == null) return;

            var VirtualProp = TargetModelType.GetProperties().FirstOrDefault(a => a.PropertyType.Equals(TargetProperty.DeclaringType));
            if (VirtualProp == null) return;
            var IdentityProp = TargetModelType.GetProperties().FirstOrDefault(a => a.CustomAttributes.Any(b => b.AttributeType == typeof(ForeignKeyAttribute)
                                        && b.ConstructorArguments.Any(c => c.Value.ToString() == VirtualProp.Name)));
            if (IdentityProp == null) return;

            var ForeignEntityList = Repository.GetAll().Where(Equal<FKModel>(IdentityProp.Name ,Model.Id)).ToList();
            var CommmonKey = ForeignEntityList.AsQueryable().Select(Select<FKModel, Data>(DataFor.Value)).Intersect(DataList).ToList();
            switch (DataFor.Mode)
            {
                case UpdateMode.Replace:
                    foreach(FKModel fk in ForeignEntityList.AsQueryable().Where(NotContains<FKModel, Data>(DataFor.Value, CommmonKey)))
                    {
                        Repository.Remove(fk);
                    }
                    goto case UpdateMode.Add;
                case UpdateMode.Add:
                    foreach (var key in DataList.Except(CommmonKey))
                    {
                        var FKLink = new FKModel();
                        IdentityProp.SetValue(FKLink, Model.Id);
                        TargetModelType.GetProperty(DataFor.Value).SetValue(FKLink, key);
                        Repository.Add(FKLink);
                    }
                    break;
                case UpdateMode.Filling:
                    break;
            }
        }

        public static Expression<Func<T, bool>> Equal<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "a");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

		public static Expression<Func<T, TKey>> Select<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

		public static Expression<Func<T, bool>> NotContains<T,D>(string propertyName, IEnumerable<D> propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(List<D>).GetMethod("Contains", new[] { typeof(D) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(List<D>));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call( constant, method, member)), parameter);
        }

		public static Expression<Func<T, bool>> Contains<T, D>(string propertyName, IEnumerable<D> propertyValue)
		{
			ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
			MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
			MethodInfo method = typeof(List<D>).GetMethod("Contains", new[] { typeof(D) });
			ConstantExpression constant = Expression.Constant(propertyValue, typeof(List<D>));
			return Expression.Lambda<Func<T, bool>>(Expression.Call(constant, method, member), parameter);
		}
		
        public static IEnumerable<PropertyInfo> GetUpdataProperties<T>(this T Model)
        {
            return Model.GetType().GetProperties().Where(a => a.CustomAttributes.Any(b => b.AttributeType == typeof(DataForAttribute)));
        }

        public static IRepositoryBase<TModel> RepositoryFactory<TModel>(TModel Model) where TModel : class
        {
            return (IRepositoryBase<TModel>)(DependencyResolver.Current.GetService(typeof(IRepositoryBase<>).MakeGenericType(typeof(TModel))));
        }
        
    }
}