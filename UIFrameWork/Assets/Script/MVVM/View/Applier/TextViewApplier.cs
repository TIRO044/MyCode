namespace MVVM.View
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class TextViewApplier : ViewApplier
    {
        [SerializeField] private Text _text;

        public override void RegisterViewObject(GameObject viewObject)
        {
            if (viewObject == null)
            {
                Debug.LogError("View object is null");
                return;
            }

            var text = viewObject.GetComponent<Text>();
            if (text == null)
            {
                Debug.LogError($"Text component is null {viewObject.name}");
                return;
            }

            MyObject = viewObject;
            _text = text;
        }

        public override void ApplyVmToView()
        {
            if (_text == null)
            {
                Debug.LogError("Not found viewObject");
                return;
            }

            if (_vmPropertyInfo == null)
            {
                Debug.LogError("Not found vmPropertyInfo");
                return;
            }

            if (ViewModel == null)
            {
                Debug.LogError("ViewModel is null");
                return;
            }

            // ToString()?...
            var value = _vmPropertyInfo.GetValue(ViewModel);
            if (value == null)
            {
                Debug.LogError("vmPropertyInfo value is null");
                return;
            }

            _text.text = value.ToString();
        }
    }
}