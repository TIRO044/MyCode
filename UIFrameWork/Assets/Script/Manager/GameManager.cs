namespace GBS.GameManager {

    using GameScene;

    public class GameManager : MonobehaviourSingleton<GameManager> {
        private GameSceneManager _gameSceneManager;

        public SceneBase Lobby;
        public SceneBase Main;

        void Awake()
        {
            _gameSceneManager = gameObject.AddComponent<GameSceneManager>();
            if (_gameSceneManager != null) {
                var lobby = Instantiate(Lobby);
                _gameSceneManager.StartScene(lobby);
            }
        }
    }
}