using System;
using System.Collections.Generic;
using CoreScript.UIFramework.MVVM.View;
using UnityEngine;

namespace CoreScript.UIFramework.UIElement
{
    public class UIElementBase : ViewBase
    {
        private GameObject _parent;
        private readonly List<UIElementBase> _childElement = new ();
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
            if (this._parent == parent)
            {
                return;
            }

            if (this._parent is not null)
            {
                var parentUIBase = this._parent.GetComponent<UIElementBase>();
                if (parentUIBase is not null)
                {
                    parentUIBase.RemoveChild(this);
                }
            }

            if (parent is null)
            {
                this._parent = null;
                _myRectTransform.SetParent(null, worldPositionStays: false);
            }
            else
            {
                this._parent = parent;
                var parentUI = this._parent.GetComponent<UIElementBase>();
                if (parentUI is not null)
                {
                    parentUI.AddChild(this);
                }
                
                _myRectTransform.SetParent(parent.transform, worldPositionStays: false);
            }
        }

        private void AddChild(UIElementBase uiElementBase)
        {
            if (uiElementBase is null)
            {
                Debug.Log("ui is null");
                return;
            }

            if (_childElement.Contains(uiElementBase))
            {
                Debug.Log($"already contain ui {uiElementBase.name}");
                return;
            }

            _childElement.Add(uiElementBase);
        }

        private void RemoveChild(UIElementBase uiElementBase)
        {
            if (uiElementBase is null)
            {
                Debug.Log("ui is null");
                return;
            }

            if (_childElement.Contains(uiElementBase) == false)
            {
                return;
            }

            _childElement.Remove(uiElementBase);
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

        protected virtual void OnAwake() { }
        protected virtual void Enable() { }

        protected override void OnPostProcessBinding()
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
            UIElementManager.OnDestroy(this);
        }

        public string Key { private set; get; }
        private Action<string, UIElementBase> OnReturn { set; get; }
        
        public void SetReturnCallBack(string key, Action<string, UIElementBase> onReturn)
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