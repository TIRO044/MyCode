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

        public ViewModel.ViewModelBase ViewModelBase { private set; get; }

        public string Name
        {
            get
            {
                if(ViewModelBase == null) return string.Empty;

                return _vmPropertyInfo.Name;
            }
        }
        
        public virtual void RegisterViewObject(GameObject viewObject) { }
        public virtual void ApplyVmToView() { }

        public void SetPropertyInfo(PropertyInfo propertyInfo)
        {
            _vmPropertyInfo = propertyInfo;
        }

        public void SetVm(ViewModel.ViewModelBase viewModelBase)
        {
            ViewModelBase = viewModelBase;
        }
    }
}