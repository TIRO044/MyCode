namespace MVVM.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using UnityEngine;
    using VContainer;
    using ViewModel;
    
    // mono, lifeTime 어떤 걸 상속받을지 고민 중
    public class View : MonoBehaviour
    {
        [SerializeField] public SerializableDictionary<string, GameObject> ViewModelProperties = new();
        [SerializeField] public string ViewModelStr;
        [ReadOnly(true)] public ViewModel MyViewModel { private set; get; }
       
        public Type MyViewModelType { private set; get; }

        public readonly Dictionary<string, ViewApplier> ViewApplierDic = new();

        private readonly ContainerBuilder _builder = new();
        private void BindViewModel()
        {
            if (string.IsNullOrEmpty(ViewModelStr))
            {
                Debug.LogError($"MyViewModelType is null");
                return;
            }

            var type = ViewModelUtil.GetType(ViewModelStr);
            if (type == null)
            {
                Debug.LogError($"ViewModel type is null");
                return;
            }

            MyViewModelType = type;

            _builder.Register(MyViewModelType, Lifetime.Scoped);
            var resolver = _builder.Build();
            var buildObj = resolver.Resolve(MyViewModelType);
            if (buildObj == null)
            {
                Debug.LogError($"resolving fail _ {ViewModelStr}");
                return;
            }
            
            MyViewModel = buildObj as ViewModel;
            if (MyViewModel == null)
            {
                Debug.LogError($"ViewModel casting fail");
                return;
            }

            // Bind
            MyViewModel.Bind(OnPropertyChanged);

            var propertyInfos = MyViewModel.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyName = propertyInfo.Name;
                if (ViewModelProperties.TryGetValue(propertyName, out var target))
                {
                    var applier = GetViewApplier(target);
                    if( applier == null )
                    {
                        Debug.Log($"not found applier _ {propertyName}");
                        continue;
                    }

                    applier.RegisterViewObject(target);
                    applier.SetPropertyInfo(propertyInfo);
                    applier.SetVm(MyViewModel);

                    ViewApplierDic[propertyName] = applier;
                }
            }
        }

        private ViewApplier GetViewApplier(GameObject gob)
        {
            if (gob == null)
            {
                return null;
            }

            var applier = gob.GetComponent<ViewApplier>();
            if (applier != null)
            {
                applier.MyObject = gob;
                return applier;
            }

            Debug.LogWarning($"Not found viewApplier _ [{gob.name}]");
            return null;
        }

        public void SetViewModelType(Type type)
        {
            MyViewModelType = type;
        }

        public ViewModel GetMyViewModel()
        {
            return MyViewModel;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ViewApplierDic.TryGetValue(e.PropertyName, out var viewApplier)) return;
            if (viewApplier == null)
            {
                Debug.LogError($"not found view applier");
                return;
            }

            viewApplier.ApplyVmToView();
        }

        private void Awake()
        {
            BindViewModel();
            BindAfter();
        }

        protected virtual void BindAfter() { }
    }
}