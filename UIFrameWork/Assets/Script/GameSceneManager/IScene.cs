using UnityEngine;

namespace Script.GameSceneManager
{
    /// <summary>
    /// Start -> Finish ~~ ...
    /// </summary>
    public interface IScene<T> where T : MonoBehaviour
    {
        //event GameScene.GameSceneDataHandler GameSceneDataHandler;
        GameScene.SceneDataState State { get; }
        void Start();
        void Finish();
        void Update(float dt);
    }
}