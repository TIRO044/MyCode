using System.Collections.Generic;
using Script.GameSceneManager;
using Script.Manager;
using UnityEngine;

namespace CoreScript.UIFramework.UIElement
{
    public static class UIElementManager
    {
        private static readonly List<UIElementBase> _openList = new();
        public static IReadOnlyList<UIElementBase> OpenedList => _openList;
        public static UIElementBase Open(GameObject parent, GameScene.SceneType sceneType, string name)
        {
            if (UIInstanceManager.Instance == null)
            {
                Debug.LogWarning($"UIInstanceManager is null");
                return null;
            }
            
            var ui = UIInstanceManager.Instance.GetInstance(name, onComplete: null);
            if (ui == null)
            {
                return null;
            }

            ui.SetParent(parent);
            ui.SetSceneType(sceneType);
            ui.Open();
            ui.SetAsLastSibling();

            _openList.Add(ui);
            return ui;
        }

        public static bool Close(UIElementBase uiElement)
        {
            if (_openList.Contains(uiElement) == false)
            {
                return false;
            }

            if (uiElement == null)
            {
                return false;
            }

            uiElement.Close();
            uiElement.Return();

            if (UIInstanceManager.Instance != null)
            {
                UIInstanceManager.Instance?.ReturnInstance(uiElement);
            }
            else
            {
                Debug.LogWarning($"[{nameof(CloseFromAbove)}] UIInstanceManager is null");
            }
            
            _openList.Remove(uiElement);
            return true;
        }

        public static void CloseFromAbove()
        {
            if (_openList.Count == 0)
            {
                return;
            }

            var closeTarget = _openList[^1];
            if (closeTarget == null)
            {
                _openList.RemoveAt(index: _openList.Count - 1);
                return;
            }
            
            closeTarget.Close();
            _openList.Remove(closeTarget);

            if (UIInstanceManager.Instance != null)
            {
                UIInstanceManager.Instance?.ReturnInstance(closeTarget);
            }
            else
            {
                Debug.LogWarning($"[{nameof(CloseFromAbove)}] UIInstanceManager is null");
            }
        }

        public static void OnDestroy(UIElementBase uiElement)
        {
            if (UIInstanceManager.Instance != null)
            {
                UIInstanceManager.Instance?.RemoveInstance(uiElement);
            }
  
            if (_openList.Count == 0)
            {
                return;
            }
            _openList.Remove(uiElement);
        }
    }
}