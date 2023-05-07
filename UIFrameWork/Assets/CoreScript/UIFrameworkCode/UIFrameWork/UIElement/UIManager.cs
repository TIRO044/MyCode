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
    }
}