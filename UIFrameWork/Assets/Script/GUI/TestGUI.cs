using System.Collections.Generic;
using System.IO;
using System.Text;
using GBS.Scene;
using GBS.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGUI : MonoBehaviour
{
    private bool sceneScrollView = false;
    private bool uiScrollView = false;

    private readonly List<GameObject> _uiList = new ();
    
    public List<string> SceneNames { private set; get; } = new();

    void Start()
    {
        var prefabList = Resources.LoadAll("TempUIPrefabs");
        foreach (var prefab in prefabList)
        {
            var gob = prefab as GameObject;
            if (gob == null) continue;
            _uiList.Add(gob);
        }

        foreach (var scene in EditorBuildSettings.scenes)
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            SceneNames.Add(sceneName);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 800, 400));
        {
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Load Scene", GUILayout.Width(120), GUILayout.Height(30)))
                {
                    sceneScrollView = !sceneScrollView;
                }
                if (sceneScrollView)
                {
                    ShowSceneScrollView();
                }

                if (GUILayout.Button("Load UI", GUILayout.Width(120), GUILayout.Height(30)))
                {
                    uiScrollView = !uiScrollView;
                }

                if (uiScrollView)
                {
                    ShowUIScrollView();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    private readonly StringBuilder _sb = new ();
    Vector3 scrollPosition;
    void ShowSceneScrollView()
    {
        var style = new GUIStyle(GUI.skin.box);

        GUILayout.BeginVertical(style, GUILayout.Width(500), GUILayout.Height(100));
        {
            GUILayout.Label("Scene List", style);
            
            _sb.Clear();
            foreach (var cs in SceneLoader.Instance.ReadOnlyOpenedScenes)
            {
                _sb.Append(cs).AppendLine();
            }
            GUILayout.Label(text: $"{_sb}", style);

            using (var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(500), GUILayout.Height(100)))
            {
                foreach (var sceneName in SceneNames)
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button( $"�� �߰� => [ {sceneName} ]"))
                        {
                            SceneLoader.Instance.LoadScene(sceneName: sceneName, loadType: LoadSceneMode.Additive, onComplete: null);
                        }

                        if (GUILayout.Button(text: $"��ε� �� �� �ε� => [ {sceneName} ]"))
                        {
                            SceneLoader.Instance.UnloadAllOpenedScene(onComplete: () =>
                            {
                                SceneLoader.Instance.LoadScene(sceneName: sceneName, loadType: LoadSceneMode.Additive, onComplete: null);
                            });
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                scrollPosition = scrollViewScope.scrollPosition;
            }
        }
        GUILayout.EndVertical();
    }

    Vector3 scrollPosition1;
    void ShowUIScrollView()
    {
        var style = new GUIStyle(GUI.skin.box);
        GUILayout.BeginVertical(style, GUILayout.Width(200), GUILayout.Height(100));
        {
            GUILayout.Label("UI List", style);
            if (GUILayout.Button(text: $"������ ���� �ϳ��� �ݱ�"))
            {
                UIElementManager.CloseFromAbove();
            }

            using (var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition1, GUILayout.Width(190), GUILayout.Height(100)))
            {
                scrollPosition1 = scrollViewScope.scrollPosition;

                foreach (var uiPrefab in UIElementManager.OpenedList)
                {
                    if (GUILayout.Button(text: $"{uiPrefab.name}"))
                    {
                    }
                }
            }
        }
        GUILayout.EndVertical();
    }
}
