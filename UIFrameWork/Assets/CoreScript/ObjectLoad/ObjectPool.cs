using System.Collections.Generic;
using UnityEngine;

namespace CoreScript.ObjectLoad
{
    public class ObjectPool<T> : IObjectPool<T> where T : Object
    {
        private readonly List<T> _instanceQueue = new();

        public T Get()
        {
            if (_instanceQueue.Count > 0)
            {
                var dequeue = _instanceQueue[0];
                _instanceQueue.Remove(dequeue);
                return dequeue;
            }

            return null;
        }

        public void Return(T instance)
        {
            if (_instanceQueue.Contains(instance))
            {
                Debug.Log($"contain instance");
                return;
            }

            _instanceQueue.Add(instance);
        }

        public void Remove(T instance)
        {
            if (_instanceQueue.Contains(instance) == false)
            {
                Debug.Log($"contain instance");
                return;
            }

            _instanceQueue.Remove(instance);
        }
    }
}