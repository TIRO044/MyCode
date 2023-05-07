using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreScript.ObjectLoad
{
    public interface IObjectPool<T> where T : Object
    {
        T Get();
        void Return(T instance);
        void Remove(T instance);
    }

    public interface IObjectLoader<T> where T : Object
    {
        T Get(string key, Action onComplete);
        void GetAsync(string key, Action<AsyncOperation> onComplete);
        void Return(string key, T obj);
        void Remove(string key, T obj);
    }
}