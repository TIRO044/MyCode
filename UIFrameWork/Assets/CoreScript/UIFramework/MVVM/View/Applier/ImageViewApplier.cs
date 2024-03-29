using System;
using CoreScript.InstanceManager;
using UnityEngine;
using UnityEngine.UI;

namespace CoreScript.UIFramework.MVVM.View.Applier
{
    [Serializable]
    public class ImageViewApplier : ViewApplier
    {
        [SerializeField] private Image _image;

        public override void RegisterViewObject(GameObject imageObject)
        {
            if (imageObject is null)
            {
                Debug.LogError("View object is null");
                return;
            }

            var image = imageObject.GetComponent<Image>();
            if (image is null)
            {
                Debug.LogError($"Text component is null {imageObject.name}");
                return;
            }

            MyObject = imageObject;
            _image = image;
        }

        public override void ApplyVmToView()
        {
            if (_image is null)
            {
                Debug.LogError("Not found viewObject");
                return;
            }

            if (_vmPropertyInfo is null)
            {
                Debug.LogError("Not found vmPropertyInfo");
                return;
            }
            
            if (ViewModelBase is null)
            {
                Debug.LogError("Not found ViewModel");
                return;
            }
            
            var value = _vmPropertyInfo.GetValue(ViewModelBase);
            if(value is null)
            {
                Debug.LogWarning("_vmPropertyInfo value is null");
                return;
            }
            
            var viewImageStr = value.ToString();
            var image = SpriteInstanceManager.Instance.GetInstance(viewImageStr, null);
            if (image is null)
            {
                Debug.LogError("Not found Image");
                return;
            }

            _image.sprite = image;
        }
    }
}