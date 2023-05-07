using CoreScript.UIFramework.MVVM.View;
using CoreScript.UIFramework.MVVM.View.Applier;
using CoreScript.UIFramework.MVVM.ViewModel;

namespace MVVM.View.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEditor;
    using System.Reflection;

    [CustomEditor(typeof(CoreScript.UIFramework.MVVM.View.View), editorForChildClasses: true)]
    public class ViewEditor : Editor
    {
        private int _index;
        private string[] _viewModelTypeStr;
        private Assembly _asm;

        private CoreScript.UIFramework.MVVM.View.View _myView;

        void Awake()
        {
            _asm = Assembly.GetAssembly(typeof(ViewModel));
        }

        public override void OnInspectorGUI()
        {
            if (_myView == null)
            {
                _myView = target as CoreScript.UIFramework.MVVM.View.View;
               
                var vmCls = GetViewModelClass();
                for (var index = 0; index < vmCls.Length; index++)
                {
                    var vmType = vmCls[index].ToString();
                    if (vmType == _myView.ViewModelStr)
                    {
                        _index = index;
                        
                        Log($"My ViewModel Type : {_myView.MyViewModelType}");
                        Log($"My Index : {_index}");

                        break;
                    }
                }
            }

            UpdateViewModelInspector();
            Change();
        }

        private void Change()
        {
            if (GUI.changed || _change)
            {
                Debug.Log("SetDirty");
                EditorUtility.SetDirty(target);
                _change = false;
            }
        }

        private void UpdateViewModelInspector()
        {
            // 1. ViewModel이 변경되었을 때 프로퍼티를 다시 세팅합니다.
            SetViewModelTypeStringArray();

            // 2. ViewModel 변경
            ChangedViewModel();

            // 3. ViewModel프로퍼티 세팅
            SetProperties();
        }

        private void ChangedViewModel()
        {
            var selectedIndex = EditorGUILayout.Popup(_index, _viewModelTypeStr);
            if (selectedIndex != _index)
            {
                if (EditorUtility.DisplayDialog("주의"
                        , "ViewModel을 변경하면 세팅된 프로퍼티가 초기화됩니다. \n변경하시겠습니까?"
                        , "변경"))
                {
                    _index = selectedIndex;
                }
            }
        }

        private void SetViewModelTypeStringArray()
        {
            if (IsChanged())
            {
                _viewModelTypeStr = GetViewModelStringArray();
            }
        }

        private bool IsChanged()
        {
            var wh = GetViewModelStringArray();
            if (_viewModelTypeStr == null || wh.Length != _viewModelTypeStr.Length)
            {
                return true;
            }

            return false;
        }

        private Type[] GetViewModelClass()
        {
            var types = _asm.GetTypes();
            return types.Where(myType => 
                myType.IsClass && myType.IsSubclassOf(typeof(ViewModel))).ToArray();
        }

        private string[] GetViewModelStringArray()
        {
            var types = _asm.GetTypes();
            return types.Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(ViewModel))).Select(x => x.ToString()).ToArray();
        }

        private List<GameObject> _tempGobList = new();

        public void Init()
        {
            
        }
        
        private bool _change;
        private void SetProperties()
        {
            var vmCls = GetViewModelClass();
            var targetVm = vmCls[_index];
            var vmString = targetVm.ToString();

            if (_myView.ViewModelStr != vmString)
            { 
                _myView.SetViewModelType(targetVm);
                _myView.ViewModelStr = vmString;
                _myView.ViewModelProperties.Clear();
                _tempGobList.Clear();
                
                _change = true;
            }
            
            var targetViewModelProperties = targetVm.GetProperties();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"ViewModel Str : [ {_myView.ViewModelStr} ]");
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("ViewModel Properties");

            foreach (var p in targetViewModelProperties)
            {
                var name = p.Name;
                if (_myView.ViewModelProperties.ContainsKey(name) == false)
                {
                    _myView.ViewModelProperties.Add(name, null);
                }
            }

            var changedKv = new ValueTuple<string, GameObject>();

            foreach (var bridgeKv in _myView.ViewModelProperties)
            {
                var key = bridgeKv.Key;
                var gob = (GameObject)EditorGUILayout.ObjectField($"{key}", bridgeKv.Value, typeof(GameObject), true);
                if (gob != bridgeKv.Value)
                {
                    changedKv.Item1 = key;
                    changedKv.Item2 = gob;
                    break;
                }
            }

            if (string.IsNullOrEmpty(changedKv.Item1) == false && changedKv.Item2 != null)
            {
                _myView.ViewModelProperties[changedKv.Item1] = changedKv.Item2;
                Debug.Log($"Change Value key : {changedKv.Item1} _ value : {changedKv.Item2}");

                var applier = AddViewApplierComponent(changedKv.Item2);
                if (applier == null)
                {
                    Debug.LogWarning("Applier is null");
                }

                _change = true;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private ViewApplier AddViewApplierComponent(GameObject gob)
        {
            if (gob == null)
            {
                return null;
            }

            var pervApplier = gob.GetComponent<ViewApplier>();
            if (pervApplier != null)
            {
                Debug.Log($"Delete prevViewApplier: {gob.name}");
                DestroyImmediate(pervApplier);
            }

            var text = gob.GetComponent<Text>();
            if (text != null)
            {
                var textViewApplier = gob.AddComponent<TextViewApplier>();
                return textViewApplier;
            }

            var image = gob.GetComponent<Image>();
            if (image != null)
            {
                var imageViewApplier = gob.AddComponent<ImageViewApplier>();
                return imageViewApplier;
            }
            
            LogError("Applier Add Component Fail");
            return null;
        }

        private void Log(string log) => Debug.Log($"[{nameof(ViewEditor)}] _ {log}");
        private void LogWarning(string log) => Debug.LogWarning($"[{nameof(ViewEditor)}] _ {log}");
        private void LogError(string log) => Debug.LogError($"[{nameof(ViewEditor)}] _ {log}"); 
    }
}