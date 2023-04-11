using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBS.Scene
{
    public static class SceneNames
    {
        public const string UIScene = "UIScene";
        public const string GameScene = "GameScene";
    }

    public class SceneLoader : MonobehaviourSingleton<SceneLoader>
    {
        public IReadOnlyList<string> ReadOnlyOpenedScenes => _openedScenes;
        private readonly List<string> _openedScenes = new();
        private readonly List<string> _loadingScene = new();
        private readonly List<string> _unloadingScene = new();

        public void LoadScene(string sceneName, LoadSceneMode loadType, Action onComplete)
        {
            if (_openedScenes.Contains(sceneName))
            {
                Debug.LogWarning($"already exist scene _ {sceneName}");
                return;
            }

            if (_loadingScene.Contains(sceneName))
            {
                Debug.LogWarning($"already loading _ {sceneName}");
                return;
            }

            // ��ε� ���� ����.. �ε��� �� ������ ó���Ѵ�. Ȥ�� Queueó���� �Ǿ�� �Ѵ�.
            if (_unloadingScene.Contains(sceneName))
            {
                Debug.LogWarning($"Already Loading _ {sceneName}");
                return;
            }

            void LoadComplete(AsyncOperation asyncOperation)
            {
                if (asyncOperation.isDone)
                {
                    _loadingScene.Remove(sceneName);
                    _openedScenes.Add(sceneName);
                    onComplete?.Invoke();
                    Debug.Log($"load complete _ {sceneName}");
                }
                else
                {
                    Debug.LogError($"load complete fail _ {sceneName}");
                }
            }

            _loadingScene.Add(sceneName);
            var async = SceneManager.LoadSceneAsync(sceneName, loadType);
            async.completed += LoadComplete;
        }

        public void UnloadScene(string sceneName, Action onComplete)
        {
            if (_openedScenes.Contains(sceneName) == false)
            {
                Debug.LogWarning($"not Exist Scene _ {sceneName}");
                return;
            }

            if (_unloadingScene.Contains(sceneName))
            {
                Debug.LogWarning($"already unloading _ {sceneName}");
                return;
            }

            void UnloadComplete(AsyncOperation asyncOperation)
            {
                _unloadingScene.Remove(sceneName);
                _openedScenes.Remove(sceneName);
                onComplete?.Invoke();
                Debug.Log($"unload complete _ {sceneName}");
            }

            _unloadingScene.Add(sceneName);
            var async = SceneManager.UnloadSceneAsync(sceneName);
            async.completed += UnloadComplete;
        }

        public void UnloadAllOpenedScene(Action onComplete)
        {
            void UnloadAllComplete()
            {
                if (_unloadingScene.Count == 0)
                {
                    onComplete?.Invoke();
                    Debug.Log("unload all complete");
                }
            }

            foreach (var openedScene in _openedScenes)
            {
                UnloadScene(openedScene, UnloadAllComplete);
            }
        }

        public GameObject FindGobFromScene(string sceneName, string gameObjName)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name != sceneName)
                {
                    continue;
                }

                var objs = scene.GetRootGameObjects();
                foreach (var obj in objs)
                {
                    if (obj.transform.name == gameObjName)
                    {
                        return obj.transform.gameObject;
                    }

                    var target = obj.transform.Find(gameObjName);
                    if (target == null)
                    {
                        continue;
                    }

                    return target.gameObject;
                }
            }

            return null;
        }
    }
}