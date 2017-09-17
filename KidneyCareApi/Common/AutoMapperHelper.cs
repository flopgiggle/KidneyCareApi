using System.Collections;
using System.Collections.Generic;
using System.Data;
using AutoMapper;
using KidneyCareApi.Dto;

namespace KidneyCareApi.Common
{
    /// <summary>
    ///     AutoMapper扩展帮助类
    /// </summary>
    public static class AutoMapperHelper
    {

        static AutoMapperHelper()
        {
            
        }

        /// <summary>
        ///     类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            Mapper.Map(obj.GetType(), typeof (T));
            return Mapper.Map<T>(obj);
        }

        /// <summary>
        ///     Dto映射到实体对象
        /// </summary>
        public static T MapToEntity<T>(this object obj, T entityObj)
        {
            Mapper.Map(obj.GetType(), typeof(T));
            return Mapper.Map(obj, entityObj);
        }

        /// <summary>
        ///     集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            foreach (var first in source)
            {
                var type = first.GetType();
                Mapper.Map(type, typeof (TDestination));
                break;
            }
            return Mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        ///     集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            //IEnumerable<T> 类型需要创建元素的映射
            //Mapper.Map<TSource, TDestination>();
            return Mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        ///     类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;
            //Mapper.Map<TSource, TDestination>();
            return Mapper.Map(source, destination);
        }

        ///// <summary>
        /////     DataReader映射
        ///// </summary>
        //public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        //{
        //    Mapper.Map<IDataReader, IEnumerable<T>>();
        //    return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        //}
    }
}