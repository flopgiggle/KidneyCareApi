using System;
using System.Collections.Generic;

namespace KidneyCareApi.Common
{
    public interface ICacheHelper
    {

        /// <summary>
        /// 泛型获取数据缓存1
        /// </summary>
        /// <param name="keyGroup"></param>
        /// <param name="cacheKey">键</param>
        T Get<T>(CacheKeyGroup keyGroup, string cacheKey);


        /// <summary>
        /// 设置数据缓存1
        /// </summary>
        bool Add<T>(CacheKeyGroup keyGroup, string keyInfo, T objObject, TimeSpan timeout);


        /// <summary>
        /// 移除指定分组的缓存1
        /// </summary>
        bool DeleteFromGroup(CacheKeyGroup keyGroup, string keyInfo);

        /// <summary>
        /// 不使用超长的key值
        /// </summary>
        string ChangeKey(string key);

        /// <summary>
        /// 插入到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyGroup"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool AddToHash<T>(CacheKeyGroup cacheKeyGroup, string key, T value);

        /// <summary>
        /// 从hash表删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyGroup"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool DeleteFromHash(CacheKeyGroup cacheKeyGroup, string key);

        /// <summary>
        /// 获取hash中说有信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyGroup"></param>
        /// <returns></returns>
        Dictionary<string, T> GetToAllDataInHash<T>(CacheKeyGroup cacheKeyGroup);

        bool DeleteAllFromHash(CacheKeyGroup cacheKeyGroup);
    }
}