using Microsoft.Extensions.Caching.Memory;

namespace PunckCodersMvc.Configs;

public static class CacheManager
{
    private static readonly List<string> CacheKeys = new();

    public static void AddKey(string key)
    {
        if (!CacheKeys.Contains(key))
        {
            CacheKeys.Add(key);
        }
    }

    public static void RemoveKey(string key)
    {
        CacheKeys.Remove(key);
    }

    public static List<string> GetKeysByPrefix(string prefix)
    {
        return CacheKeys.Where(k => k.StartsWith(prefix)).ToList();
    }

    public static void ClearKeysByPrefix(IMemoryCache memoryCache, string prefix)
    {
        var keysToRemove = GetKeysByPrefix(prefix);
        foreach (var key in keysToRemove)
        {
            memoryCache.Remove(key);
            RemoveKey(key);
        }
    }

    public static void ClearAll(IMemoryCache memoryCache)
    {
        foreach (var key in CacheKeys)
        {
            memoryCache.Remove(key);
        }
        CacheKeys.Clear();
    }
}
