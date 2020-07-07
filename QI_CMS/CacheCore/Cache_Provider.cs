using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

public interface ICacheProvider
{
    object Get(string key);
    void Set(string key, object data, int cacheTime);
    bool IsSet(string key);
    void Invalidate(string key);
    void RemoveAll();
    void RemoveByFirstName(string key);
}

public class Cache_Provider : ICacheProvider
{
    private ObjectCache Cache
    {
        get { return MemoryCache.Default; }
    }

    public object Get(string key)
    {
        return Cache[key];
    }

    public void Set(string key, object data, int cacheTime)
    {
        var policy = new CacheItemPolicy();
        policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

        Cache.Add(new CacheItem(key, data), policy);
    }

    public bool IsSet(string key)
    {
        return (Cache[key] != null);
    }

    public void Invalidate(string key)
    {
        Cache.Remove(key);
    }

    public void RemoveAll()
    {
        List<string> cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
        foreach (string cacheKey in cacheKeys)
        {
            Cache.Remove(cacheKey);
        }
    }

    public void RemoveByFirstName(string name)
    {
        try
        {
            List<string> cacheKeys =
                Cache.Where(kvp => kvp.Key.ToLower().IndexOf(name.ToLower()) == 0).Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                Cache.Remove(cacheKey);
            }
        }
        catch { }
    }
    public void RemoveByKey(string cacheKey)
    {
        try
        {
            Cache.Remove(cacheKey);
        }
        catch { }
    }

    public List<string> GetAll()
    {
        var cacheKeys = new List<string>();
        cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
        return cacheKeys;
    }
}
