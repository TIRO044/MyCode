
using CoreScript.UIFramework.UIElement;

namespace Script.GameSceneManager.SceneClass
{
    public class MainScene : SceneBase
    {
        public override void Init()
        {
            SceneState = GameScene.SceneDataState.Start;

            LoadUI();
        }

        public override void LoadUI()
        {
            SceneState = GameScene.SceneDataState.UILoad;

            foreach (var uiPath in UIPathArrToLoadOnStart)
            {
                UIElementManager.Open(_uiCanvasGameObject, uiPath);
            }
        }
    }
}
