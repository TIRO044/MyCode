using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreScript.ObjectLoad
{
    using Object = UnityEngine.Object;
    
    public class ObjectLoader<T> : IObjectLoader<T> where T : Object
    {
        private readonly Dictionary<string, IObjectPool<T>> _objectPoolDic = new();
        private readonly Func<IObjectPool<T>> _resourcePool;
        private readonly Func<string, Action<string, T>, T> _objectFactory;

        public ObjectLoader(Func<IObjectPool<T>> resourcePool, Func<string, Action<string, T>, T> objectFactory)
        {
            _resourcePool = resourcePool;
            _objectFactory = objectFactory;
        }
       
        public T Get(string key, Action onComplete)
        {
            var poolObject = GetFromPool(key);
            if (poolObject is not null)
            {
                return poolObject;
            }

            var iPoolObject = _objectFactory?.Invoke(key, Return);
            if (iPoolObject is null)
            {
                Debug.LogWarning($"invalid Type instance _ {key} _ Type {typeof(T)}");
                return null;
            }

            onComplete?.Invoke();

            return iPoolObject;
        }

        public void GetAsync(string key, Action<AsyncOperation> onComplete)
        {
            // 미구현
        }

        public void Return(string key, T obj)
        {
            if (_objectPoolDic.TryGetValue(key, out var instancePool) == false)
            {
                instancePool = _resourcePool?.Invoke();
                if (instancePool is null)
                {
                    Debug.LogWarning($"return fail");
                    return;
                }

                instancePool.Return(obj);
                _objectPoolDic.Add(key, instancePool);
            }

            instancePool?.Return(obj);
        }

        public void Remove(string key, T obj)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"not found key _ {key}");
                return;
            }

            if (_objectPoolDic.TryGetValue(key, out var poolObj))
            {
                poolObj?.Remove(obj);
            }
        }

        private T GetFromPool(string key)
        {
            if (!_objectPoolDic.TryGetValue(key, out var pool))
            {
                return default;
            }

            var poolObj = pool.Get();
            return poolObj is null ? default : poolObj;
        }
    }
}