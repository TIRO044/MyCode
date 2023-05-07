using System.IO;
using Script.GameSceneManager;
using UnityEngine;

namespace GBS.GameScene.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(SceneBase), editorForChildClasses: true)]
    public class SceneBaseEditor : Editor
    {
        private SceneBase _sceneBase;
        private bool _fold;

        void OnEnable()
        {
            _sceneBase = (SceneBase)target;
        }

        void OnDisable()
        {
            _sceneBase = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_sceneBase == null)
            {
                Debug.Log("scene base is null");
                return;
            }

            SetAssetPath();
            ShowPrefabPathTextList();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        const string removePath = "Assets/Resources/";
        private void SetAssetPath()
        {
            var save = false;

            if (_sceneBase.UIPathArrToLoadOnStart == null || (_sceneBase.UIPathArrToLoadOnStart.Length != _sceneBase.UIListToLoadOnStart.Count))
            {
                _sceneBase.UIPathArrToLoadOnStart = new string[_sceneBase.UIListToLoadOnStart.Count];
                save = true;
            }

            for (var index = 0; index < _sceneBase.UIListToLoadOnStart.Count; index++)
            {
                var uiPrefab = _sceneBase.UIListToLoadOnStart[index];
                if (uiPrefab == null)
                {
                    _sceneBase.UIPathArrToLoadOnStart[index] = string.Empty;
                    continue;
                }

                var path = AssetDatabase.GetAssetPath(uiPrefab);
                path = Path.ChangeExtension(path.Replace(removePath, ""), extension: null);
                if (_sceneBase.UIPathArrToLoadOnStart[index] == path)
                {
                    continue;
                }

                _sceneBase.UIPathArrToLoadOnStart[index] = path;
                save = true;
            }

            if (save)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void ShowPrefabPathTextList()
        {
            _fold = EditorGUILayout.Foldout(_fold, $"{nameof(_sceneBase.UIPathArrToLoadOnStart)}");
            EditorGUILayout.BeginVertical(SceneBaseEditorUtil.BoxStyle);
            if (_fold)
            {
                for (var index = 0; index < _sceneBase.UIPathArrToLoadOnStart.Length; index++)
                {
                    var str = _sceneBase.UIPathArrToLoadOnStart[index];
                    var uiDialog = _sceneBase.UIListToLoadOnStart[index];
                    if (uiDialog == null)
                    {
                        EditorGUILayout.LabelField("Empty");
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($" Element {index} Path", SceneBaseEditorUtil.ButtonStyle, GUILayout.Width(150), GUILayout.Height(20));
                        EditorGUILayout.LabelField($"[ {str} ]", SceneBaseEditorUtil.TextStyle, GUILayout.Height(20));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}