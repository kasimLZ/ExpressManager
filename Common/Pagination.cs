using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Linq;

namespace Common
{
    public interface IPagedList : IList
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
        Type EntityType { get; set; }
    }


    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
        Type EntityType { get; set; }
    }

    public class PagedList<T> : List<T>, IPagedList<T>, IPagedList
    {
        public PagedList() { }

        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            TotalCount = source.Count();
            PageSize = pageSize;
            PageIndex = index;
            EntityType = typeof(T);
            try
            {
                AddRange(source.Skip(((index - 1) * pageSize)).Take(pageSize));
            }
            catch (NotSupportedException e)
            {
                if (e.Message.Contains("OrderBy"))
                {
                    AddRange((IQueryable<T>)source.OrderBy(typeof(T).IdentifierPropertyName()).Skip(((index - 1) * pageSize)).Take(pageSize));
                }
            }
           
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public Type EntityType { get; set; }
    }

    public interface IPagedBase
    {
        void Set<T>(T model);
    }

    public static class Pagination
    {
        public static int pageSize = 20;

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<R> Translat<T, R>(this IPagedList<T> source) where R : IPagedBase, new()
        {
            return source.Translat(new R());
        }

        public static PagedList<R> Translat<T, R>(this IPagedList<T> source, R model) where R : IPagedBase,new ()
        {
            var result = new PagedList<R>{
                PageSize = source.PageSize,
                PageIndex = source.PageIndex,
                TotalCount = source.TotalCount,
                EntityType = source.EntityType
            };
            result.AddRange(source.Select(a => { var r = new R(); r.Set(a); return r; }));
            return result;
        }


    }
}
