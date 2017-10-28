using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Resources
{
    public static class Ch3ResourceManager
    {
        private static Dictionary<string, Object> _cache = new Dictionary<string, Object>();
        private static Dictionary<string, Object> _jsonCache = new Dictionary<string, Object>();

        public static void ReleaseAll()
        {
            foreach (Object resource in _cache.Values)
            {
                UnityEngine.Resources.UnloadAsset(resource);
            }

            foreach (Object resource in _jsonCache.Values)
            {
                UnityEngine.Resources.UnloadAsset(resource);
            }

            _cache.Clear();
            _jsonCache.Clear();
        }

        public static void Preload<T>(string p_resourceId) where T : Object
        {
            Load<T>(p_resourceId);
        }

        public static void Release(string p_resourceId)
        {
            Object resource;

            _cache.TryGetValue(p_resourceId, out resource);

            UnityEngine.Resources.UnloadAsset(resource);
            _cache.Remove(p_resourceId);
        }

        public static void Release(IEnumerable<string> p_resourceIds)
        {
            foreach (string resourceId in p_resourceIds)
            {
                Release(resourceId);
            }
        }

        public static T Load<T>(string p_resourceId) where T : Object
        {
            if (!_cache.ContainsKey(p_resourceId))
            {
                _cache.Add(p_resourceId, UnityEngine.Resources.Load<T>(p_resourceId));
            }

            return (T) _cache[p_resourceId];
        }

        public static T LoadJSON<T>(string p_resourceId) where T : Object
        {
            if (!_jsonCache.ContainsKey(p_resourceId))
            {
                TextAsset loadedResource = UnityEngine.Resources.Load<TextAsset>(p_resourceId);

                if (loadedResource == null)
                {
                    _jsonCache.Add(p_resourceId, null);
                }
                else
                {
                    _jsonCache.Add(p_resourceId, JsonUtility.FromJson<T>(loadedResource.text));
                }

                UnityEngine.Resources.UnloadAsset(loadedResource);
            }

            return (T) _jsonCache[p_resourceId];
        }
    }
}