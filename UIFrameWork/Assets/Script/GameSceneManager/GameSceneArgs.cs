namespace GBS.GameScene
{
    public class GameScene
    {
        public enum SceneType
        {
            None,
            Lobby,
            Main
        }

        public enum SceneDataState
        {
            None,
            Start,
            UILoad,
            Finish,
            CleanUp
        }

        public class SceneDataArgs
        {
            public SceneDataArgs(SceneDataState state)
            {
                State = state;
            }

            public SceneDataState State { private set; get; }
        }

        public delegate void GameSceneDataHandler(object sceneData, SceneDataArgs args);
    }
}