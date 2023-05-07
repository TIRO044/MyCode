using CoreScript.Singleton;
using Script.GameSceneManager;

namespace Script.Manager {
    public class GameManager : MonobehaviourSingleton<GameManager> {
        private GameSceneManager.GameSceneManager _gameSceneManager;

        public SceneBase Lobby;
        public SceneBase Main;

        void Awake()
        {
            _gameSceneManager = gameObject.AddComponent<GameSceneManager.GameSceneManager>();
            if (_gameSceneManager != null) {
                var lobby = Instantiate(Lobby);
                _gameSceneManager.StartScene(lobby);
            }
        }
    }
}