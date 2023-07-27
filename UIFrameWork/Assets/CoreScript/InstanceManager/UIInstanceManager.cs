using System;
using CoreScript.ObjectLoad;
using CoreScript.Singleton;
using CoreScript.UIFramework.UIElement;
using UnityEngine;

namespace CoreScript.InstanceManager
{
    public class UIInstanceManager : MonobehaviourSingleton<UIInstanceManager>, IInstanceManager<UIElementBase>
    {
        private static readonly string _name = $"[{nameof(UIInstanceManager)}]";
            
        private readonly IObjectLoader<UIElementBase> _uiInstanceLoader = new ObjectLoader<UIElementBase>(
            resourcePool: () => new ObjectPool<UIElementBase>(),
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

                if (gob.TryGetComponent(typeof(UIElementBase), out var component) == false)
                {
                    Debug.LogWarning($"{_name} UIBase is null _ {key}");
                    return null;
                }

                var instance = Instantiate(gob);
                var uiBase = instance.GetComponent<UIElementBase>();
                uiBase.SetReturnCallBack(key, action);
                
                return uiBase;
            });

        public UIElementBase GetInstance(string name, Action onComplete)
        {
            return _uiInstanceLoader.Get(name, onComplete);
        }

        public void ReturnInstance(UIElementBase uiElementBase)
        {
            uiElementBase.SetParent(Instance.gameObject);

            _uiInstanceLoader.Return(uiElementBase.Key, uiElementBase);
        }

        public void RemoveInstance(UIElementBase uiElementBase)
        {
            _uiInstanceLoader.Remove(uiElementBase.Key, uiElementBase);
        }
    }
}