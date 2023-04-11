using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBS.UI
{
    using GameScene;
    using MVVM.View;

    public class UIBase : View
    {
        public GameScene.SceneType UISceneType { private set; get; }

        private GameObject Parent;
        // 굳이 이걸 두는 게 좋은 짓일까.. 
        private readonly List<UIBase> _childElement = new ();
        private RectTransform _myRectTransform;
        private bool _active;

        protected bool Active
        {
            get => _active;
            private set
            {
                _active = value;
                gameObject.SetActive(_active);
            }
        }

        public void SetParent(GameObject parent)
        {
            if (Parent == parent)
            {
                return;
            }

            if (Parent is not null)
            {
                var parentUIBase = Parent.GetComponent<UIBase>();
                if (parentUIBase is not null)
                {
                    parentUIBase.RemoveChild(this);
                }
            }

            if (parent is null)
            {
                Parent = null;
                _myRectTransform.SetParent(null, worldPositionStays: false);
            }
            else
            {
                Parent = parent;
                var parentUI = Parent.GetComponent<UIBase>();
                if (parentUI is not null)
                {
                    parentUI.AddChild(this);
                }
                
                _myRectTransform.SetParent(parent.transform, worldPositionStays: false);
            }
        }

        public void SetSceneType(GameScene.SceneType sceneType)
        {
            UISceneType = sceneType;
        }

        private void AddChild(UIBase uiBase)
        {
            if (uiBase is null)
            {
                Debug.Log("ui is null");
                return;
            }

            if (_childElement.Contains(uiBase))
            {
                Debug.Log($"already contain ui {uiBase.name}");
                return;
            }

            _childElement.Add(uiBase);
        }

        private void RemoveChild(UIBase uiBase)
        {
            if (uiBase is null)
            {
                Debug.Log("ui is null");
                return;
            }

            if (_childElement.Contains(uiBase) == false)
            {
                return;
            }

            _childElement.Remove(uiBase);
        }

        public void Open()
        {
            Active = true;
        }

        public void Close()
        {
            Active = false;
        }

        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }

        public virtual void ReceiveMessage(IUIMessage message) { }
        protected virtual void OnAwake() { }
        protected virtual void Enable() { }

        protected override void BindAfter()
        {
            OnAwake();
        }

        void OnEnable()
        {
            _myRectTransform = GetComponent<RectTransform>();
            Enable();
        }

        void OnDestroy()
        {
            // 직접 호출하지말고 콜백으로 하자
            UIManager.OnDestroy(this);
        }

        public string Key { private set; get; }
        private Action<string, UIBase> OnReturn { set; get; }
        
        public void SetReturnCallBack(string key, Action<string, UIBase> onReturn)
        {
            Key = key;
            OnReturn = onReturn;
        }

        public void Return()
        {
            // object pool에 반환한다.
            if (OnReturn != null)
            {
                if (string.IsNullOrEmpty(Key))
                {
                    Debug.LogWarning($"Key Is Null {Key}.. Return Fail");
                    return;
                }

                OnReturn.Invoke(Key, this);
            }
        }
    }
}