using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace KidneyCareApi.Common
{
    public class RedisCacheHelper : ICacheHelper
    {
        public static string RedisIp = Util.GetConfigByName("RedisIp");
        //public static List<string> ReadWriteHosts = new List<string>() { RedisIp };
        public static string[] ReadWriteHosts = new[] {
            RedisIp /*实例id:密码@访问地址:端口*/
        };
        public static IRedisClientsManager ClientManager = new PooledRedisClientManager(10, 10, ReadWriteHosts);

        //PooledRedisClientManager redisPoolManager = new PooledRedisClientManager(10, 10, ReadWriteHosts);
        //public IRedisClient Client;
        public static string RedisPws = Util.GetConfigByName("RedisPws");


        public RedisCacheHelper()
        {
            //Client = ClientManager.GetClient();
            //Client.Password = RedisPws;
        }


        /// <summary>
        /// 泛型获取数据缓存1
        /// </summary>
        /// <param name="keyGroup"></param>
        /// <param name="cacheKey">键</param>
        public T Get<T>(CacheKeyGroup keyGroup, string cacheKey)
        {
            using (var client = ClientManager.GetClient())
            {
                var key = "Grp:" + keyGroup.Description() + "_" + ChangeKey(cacheKey);
                var returnValue = client.Get<T>(key);
                client.Dispose();
                return returnValue;

            }
        }

        /// <summary>
        /// 设置数据缓存1
        /// </summary>
        public bool Add<T>(CacheKeyGroup keyGroup, string keyInfo, T objObject, TimeSpan timeout)
        {
            using (var client = ClientManager.GetClient())
            {
                string key = "Grp:" + keyGroup.Description() + "_" + ChangeKey(keyInfo);
                var returnValue = client.Set(key, objObject, timeout);
                client.Dispose();
                return returnValue;
            }
        }


        /// <summary>
        /// 设置数据到hash表
        /// </summary>
        /// <param name="cacheKeyGroup">缓存的业务分组,需自定义缓存的业务分类,避免缓存键值重复</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddToHash<T>(CacheKeyGroup cacheKeyGroup, string key, T value)
        {
            using (var client = ClientManager.GetClient())
            {
                client.RemoveEntryFromHash(cacheKeyGroup.Description(), key);
                var returnValue = client.SetEntryInHash(cacheKeyGroup.Description(), key,JsonConvert.SerializeObject(value));
                client.Dispose();
                return returnValue;
            }
        }

        /// <summary>
        /// 从hash表删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyGroup"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteFromHash(CacheKeyGroup cacheKeyGroup, string key)
        {
            using (var client = ClientManager.GetClient())
            {
                var returnValue = client.RemoveEntryFromHash(cacheKeyGroup.Description(), key);
                client.Dispose();
                return returnValue;
            }
        }

        public bool DeleteAllFromHash(CacheKeyGroup cacheKeyGroup)
        {
            using (var client = ClientManager.GetClient())
            {
                var returnValue = client.Remove(cacheKeyGroup.Description());
                client.Dispose();
                return returnValue;
            }
        }

        /// <summary>
        /// 获取hash值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyGroup"></param>
        /// <returns></returns>
        public Dictionary<string,T> GetToAllDataInHash<T>(CacheKeyGroup cacheKeyGroup)
        {
            using (var client = ClientManager.GetClient())
            {
                var returnValue = client.GetAllEntriesFromHash(cacheKeyGroup.Description());
                var returnDic = new Dictionary<string, T>();
                client.Dispose();
                foreach (var oneItem in returnValue)
                {
                    returnDic.Add(oneItem.Key, JsonConvert.DeserializeObject<T>(oneItem.Value));
                }
                return returnDic;
            }
        }

        /// <summary>
        /// 移除指定分组的缓存1
        /// </summary>
        public bool DeleteFromGroup(CacheKeyGroup keyGroup, string keyInfo)
        {
            using (var client = ClientManager.GetClient())
            {
                //client.Password = RedisPws;
                var key = "Grp:" + keyGroup.Description() + "_" + ChangeKey(keyInfo);
                var returnValue = client.Remove(key);
                client.Dispose();
                return returnValue;
            }
        }

        /// <summary>
        /// 不使用超长的key值
        /// </summary>
        public string ChangeKey(string key)
        {
            return key.Length > 50 ? GetMd5(key) : key;
        }

        public string GetMd5(string sDataIn)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(sDataIn);
            byte[] bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            var sTemp = bytHash.Aggregate("", (current, t) => current + t.ToString("X").PadLeft(2, '0'));
            return sTemp.ToLower();
        }
    }
}
