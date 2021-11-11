using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Utilities.DbHelper
{
    public class RedisHelper
    {
        private static RedisClient _redisClient;

        //默认缓存过期时间单位秒  
        private static int _cacheTimeOut = 30 * 60;

        public static void Init(string ip, int port)
        {
            if (_redisClient == null)
            {
                _redisClient = new RedisClient(ip, port);
            }
        }

        /// <summary>
        /// 从redis获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>返回字符串类型值</returns>
        public static string Get(string key)
        {
            using (var redis = _redisClient)
            {
                try
                {
                    string value = redis.Get<string>(key);
                    return value;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 向redis保存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="keyvalue">value值</param>
        public static bool Set(string key, string keyvalue)
        {
            using (var redis = _redisClient)
            {
                try
                {
                    redis.Set(key, keyvalue);
                    redis.BgSave();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除键值
        /// </summary>
        /// <param name="key">键值</param>
        public static bool Del(string key)
        {
            using (var redis = _redisClient)
            {
                try
                {
                    redis.Del(key);
                    redis.BgSave();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #region Key/Value存储
        /// <summary>  
        /// 设置缓存  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="key">缓存建</param>  
        /// <param name="t">缓存值</param>  
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>  
        /// <returns></returns>  
        public static bool Set<T>(string key, T t, int timeout = 0)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    _cacheTimeOut = timeout;
                }
                _redisClient.Expire(key, _cacheTimeOut);
            }

            return _redisClient.Add(key, t);
        }
        /// <summary>  
        /// 获取  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static T Get<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }

        #endregion

        #region 链表操作
        /// <summary>  
        /// 根据IEnumerable数据添加链表  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="values"></param>  
        /// <param name="timeout"></param>  
        public static void AddList<T>(string listId, List<T> values, int timeout = 0)
        {
            _redisClient.Expire(listId, 60);
            IRedisTypedClient<T> iredisClient = _redisClient.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    _cacheTimeOut = timeout;
                }
                _redisClient.Expire(listId, _cacheTimeOut);
            }
            var redisList = iredisClient.Lists[listId];
            redisList.AddRange(values);
            iredisClient.Save();
        }

        /// <summary>  
        /// 获取链表  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <returns></returns>  
        public static List<T> GetList<T>(string listId)
        {
            try
            {
                IRedisTypedClient<T> iredisClient = _redisClient.As<T>();
                return iredisClient.Lists[listId].ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>  
        /// 根据lambada表达式删除符合条件的实体  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="func"></param>  
        public static void RemoveEntityFromList<T>(string listId, Func<T, bool> func)
        {
            IRedisTypedClient<T> iredisClient = _redisClient.As<T>();
            var redisList = iredisClient.Lists[listId];
            T value = redisList.Where(func).FirstOrDefault();
            redisList.RemoveValue(value);
            iredisClient.Save();
        }
        #endregion

        //释放资源  
        public static void Dispose()
        {
            if (_redisClient != null)
            {
                _redisClient.Dispose();
                _redisClient = null;
            }
            GC.Collect();//垃圾回收机制
        }
    }
}
