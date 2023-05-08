using System;
using System.Linq;
using CoreScript.ObjectLoad;
using CoreScript.Singleton;
using UnityEngine;

namespace Script.Manager
{
    public class SpriteInstanceManager : MonobehaviourSingleton<SpriteInstanceManager>, IInstanceManager<Sprite>
    {  
        private static readonly string _name = $"[{nameof(UIInstanceManager)}]";

        //TODO: pool, factory DI
        private readonly IObjectLoader<Sprite> _imageInstanceLoader = new ObjectLoader<Sprite>(
            resourcePool: () => new ObjectPool<Sprite>(),
            objectFactory: (key, action) =>
            {
                var spriteName = key?.Split('/').Last();
                var spritePath = key?.Split(spriteName)?[0];
                
                var originalResourceTest = Resources.LoadAll(spritePath);
                if (originalResourceTest == null || originalResourceTest.Length == 0)
                {
                    Debug.LogError($"{_name} sprite load fail spritePath");
                    return null;
                }

                object targetSprite = null;
                foreach (var o in originalResourceTest)
                {
                    if (o.name == spriteName)
                    {
                        targetSprite = o;
                        break;
                    }
                }
                
                if (targetSprite is null)
                {
                    Debug.LogError($"{_name} Resource load fail _ {key}");
                    return null;
                }

                if (targetSprite is not Sprite sprite)
                {
                    Debug.LogError($"{_name} Not sprite type _ {key} _ {targetSprite.GetType()}");
                    return null;
                }

                var instance = Instantiate(sprite);
                return instance;
            });

        public Sprite GetInstance(string spriteName, Action onComplete)
        {
            return _imageInstanceLoader.Get(spriteName, onComplete);
        }

        public void ReturnInstance(Sprite sprite)
        {
            if (sprite == null)
                return;
            
            _imageInstanceLoader.Return(sprite.name, sprite);
        }

        public void RemoveInstance(Sprite sprite)
        {
            if (sprite == null)
                return;
            
            _imageInstanceLoader.Remove(sprite.name, sprite);
        }
    }
}