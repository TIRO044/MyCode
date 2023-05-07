using System.Collections.Generic;
using GBS.Resource;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace GBS.UI
{
    using GameScene;
    
    public static class UIManager
    {
        private static readonly List<UIBase> _openList = new();
        public static IReadOnlyList<UIBase> OpenedList => _openList;
        public static UIBase Open(GameObject parent, GameScene.SceneType sceneType, string name)
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

        public static bool Close(UIBase ui)
        {
            if (_openList.Contains(ui) == false)
            {
                return false;
            }

            if (ui == null)
            {
                return false;
            }

            ui.Close();
            ui.Return();

            if (UIInstanceManager.Instance != null)
            {
                UIInstanceManager.Instance?.ReturnInstance(ui);
            }
            else
            {
                Debug.LogWarning($"[{nameof(CloseFromAbove)}] UIInstanceManager is null");
            }
            
            _openList.Remove(ui);
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

        public static void OnDestroy(UIBase ui)
        {
            if (UIInstanceManager.Instance != null)
            {
                UIInstanceManager.Instance?.RemoveInstance(ui);
            }
  
            if (_openList.Count == 0)
            {
                return;
            }
            _openList.Remove(ui);
        }

        public static void SendMessageToOpenList(IUIMessage message)
        {
            foreach (var openedUI in _openList)
            {
                if (openedUI == null)
                {
                    continue;
                }

                openedUI.ReceiveMessage(message);
            }
        }

        public static void SendMessageToChild(IUIMessage message, GameObject go)
        {
            var children = go.GetComponentsInChildren<UIBase>(includeInactive: true);
            foreach (var uiBase in children)
            {
                if (uiBase == null)
                {
                    // 여기는 차라리 null날 확률이 적다
                    continue;
                }

                uiBase.ReceiveMessage(message);
            }
        }

        public static void SendMessageToParents(IUIMessage message, GameObject go)
        {
            var children = go.GetComponentsInParent<UIBase>(includeInactive: true);
            foreach (var uiBase in children)
            {
                if (uiBase == null)
                {
                    // 여기는 차라리 null날 확률이 적다
                    continue;
                }

                uiBase.ReceiveMessage(message);
            }
        }
    }
}