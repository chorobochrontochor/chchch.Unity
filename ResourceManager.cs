using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch
{
    public static class ResourceManager
    {
        private static Dictionary<string, Object> _cache = new Dictionary<string, Object>();
        private static Dictionary<string, Object> _jsonCache = new Dictionary<string, Object>();

        public static void ReleaseAll()
        {
            foreach (Object resource in _cache.Values)
            {
                Resources.UnloadAsset(resource);
            }

            foreach (Object resource in _jsonCache.Values)
            {
                Resources.UnloadAsset(resource);
            }

            _cache.Clear();
            _jsonCache.Clear();
        }

        public static void Preload<T>(string resourceId) where T : Object
        {
            Load<T>(resourceId);
        }

        public static void Release(string resourceId)
        {
            Object resource;

            _cache.TryGetValue(resourceId, out resource);

            Resources.UnloadAsset(resource);
            _cache.Remove(resourceId);
        }

        public static void Release(IEnumerable<string> resourceIds)
        {
            foreach (string resourceId in resourceIds)
            {
                Release(resourceId);
            }
        }

        public static T Load<T>(string resourceId) where T : Object
        {
            if (!_cache.ContainsKey(resourceId))
            {
                _cache.Add(resourceId, Resources.Load<T>(resourceId));
            }

            return (T) _cache[resourceId];
        }

        public static T LoadJSON<T>(string resourceId) where T : Object
        {
            if (!_jsonCache.ContainsKey(resourceId))
            {
                TextAsset loadedResource = Resources.Load<TextAsset>(resourceId);

                if (loadedResource == null)
                {
                    _jsonCache.Add(resourceId, null);
                }
                else
                {
                    _jsonCache.Add(resourceId, JsonUtility.FromJson<T>(loadedResource.text));
                }

                Resources.UnloadAsset(loadedResource);
            }

            return (T) _jsonCache[resourceId];
        }
    }
}