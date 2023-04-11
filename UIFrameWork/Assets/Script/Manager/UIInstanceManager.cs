namespace GBS.Resource
{
    using System;
    using UnityEngine;
    using UI;
    
    public class UIInstanceManager : MonobehaviourSingleton<UIInstanceManager>, IInstanceManager<UIBase>
    {
        private static readonly string _name = $"[{nameof(UIInstanceManager)}]";
            
        private readonly IObjectLoader<UIBase> _uiInstanceLoader = new ObjectLoader<UIBase>(
            resourcePool: () => new ObjectPool<UIBase>(),
            objectFactory: (key, action) =>
            {
                var resource = Resources.Load(key);
                if (resource is null)
                {
                    Debug.LogWarning($"{_name} ui prefab load fail _ {key}");
                    return null;
                }

                if (resource is not GameObject gob)
                {
                    Debug.LogWarning($"{_name} not gameObject _ {key}");
                    return null;
                }

                if (gob.TryGetComponent(typeof(UIBase), out var component) == false)
                {
                    Debug.LogWarning($"{_name} UIBase is null _ {key}");
                    return null;
                }

                var instance = Instantiate(gob);
                var uiBase = instance.GetComponent<UIBase>();
                uiBase.SetReturnCallBack(key, action);
                
                return uiBase;
            });

        public UIBase GetInstance(string name, Action onComplete)
        {
            return _uiInstanceLoader.Get(name, onComplete);
        }

        public void ReturnInstance(UIBase uiBase)
        {
            uiBase.SetParent(Instance.gameObject);

            _uiInstanceLoader.Return(uiBase.Key, uiBase);
        }

        public void RemoveInstance(UIBase uiBase)
        {
            _uiInstanceLoader.Remove(uiBase.Key, uiBase);
        }
    }
}