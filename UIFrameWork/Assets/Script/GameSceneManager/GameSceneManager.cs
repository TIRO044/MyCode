using System;
using UnityEngine;

namespace Script.GameSceneManager
{
    public class GameSceneManager : MonoBehaviour
    {
        // ��⿭ ó���� �ʿ��ұ�? Ȥ�� ���� ���� ���� ���� �� �ְ���?
        private SceneBase _currentScene;

        public void StartScene(SceneBase scene)
        {
            if(scene == null)
            {
                return;
            }

            if (_currentScene != null)
            {
                CleanUpScene();
            }

            switch (scene.SceneType)
            {
                case GameScene.SceneType.Lobby:
                    _currentScene = scene;
                    break;
                case GameScene.SceneType.Main:
                    break;
            }
        }

        public void CleanUpScene()
        {
            _currentScene.CleanUp();
            _currentScene = null;
        }

        private void UpdateCurrentScene()
        {
            if (_currentScene == null)
            {
                return;
            }

            if (_currentScene.SceneState is GameScene.SceneDataState.Finish or GameScene.SceneDataState.CleanUp)
            {
                return;
            }

            var dt = Time.deltaTime;
            _currentScene.OnUpdate(dt);

            switch (_currentScene.SceneState)
            {
                case GameScene.SceneDataState.None:
                    _currentScene.Init();
                    break;
                case GameScene.SceneDataState.Start:
                    break;
                case GameScene.SceneDataState.UILoad:
                    break;
                case GameScene.SceneDataState.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Update()
        {
            UpdateCurrentScene();
        }
    }
}