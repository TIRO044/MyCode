using System.Collections.Generic;
using GBS.Scene;
using GBS.UI;
using UnityEngine;

namespace GBS.GameScene
{
    public class SceneBase : MonoBehaviour
    {
        private GameObject _uiCanvasGob;
        protected GameObject _uiCanvasGameObject
        {
            get
            {
                if (_uiCanvasGob == null)
                {
                    _uiCanvasGob = SceneLoader.Instance.FindGobFromScene(SceneNames.UIScene, "Canvas");
                }

                return _uiCanvasGob;
            }
        }

        [field: SerializeField] public List<UIBase> UIListToLoadOnStart { set; get; } = new ();
        [field: SerializeField, HideInInspector] public string[] UIPathArrToLoadOnStart { set; get; }
        [field: SerializeField] public GameScene.SceneType SceneType { protected set; get; }
        public GameScene.SceneDataState SceneState { protected set; get; }

        public virtual void Init() { }
        public virtual void LoadUI() { }
        public virtual void Finish() { }
        public virtual void CleanUp() { }
        public virtual void OnUpdate(float dt) { }
    }
}