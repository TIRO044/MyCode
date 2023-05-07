using System;
using System.Reflection;
using UnityEngine;

namespace CoreScript.UIFramework.MVVM.View
{
    [Serializable]
    public class ViewApplier : MonoBehaviour
    {
        protected PropertyInfo _vmPropertyInfo;
        [SerializeField] public GameObject MyObject;

        public ViewModel.ViewModel ViewModel { private set; get; }

        public string Name
        {
            get
            {
                if(ViewModel == null) return string.Empty;

                return _vmPropertyInfo.Name;
            }
        }
        
        public virtual void RegisterViewObject(GameObject viewObject) { }
        public virtual void ApplyVmToView() { }

        public void SetPropertyInfo(PropertyInfo propertyInfo)
        {
            _vmPropertyInfo = propertyInfo;
        }

        public void SetVm(ViewModel.ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}