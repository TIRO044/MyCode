using CoreScript.UIFramework.MVVM;
using CoreScript.UIFramework.UIElement;
using Script.UIScript.Element.Lobby.ViewModel;

namespace MVVM.View.Editor
{
    using UnityEngine;
    using UnityEditor;

    public static class ViewModelExtension
    {
        public static void ChangeLobbyTitle(this GameStartViewModel vm, string str)
        {
            vm.LobbyTitle = str;
        }
        
        public static void ChangeLobbyButtonImage(this GameStartViewModel vm, string str)
        {
            vm.LobbyButtonImage = str;
        }
    }

    public class MVVMTestWindow : EditorWindow
    {
        [MenuItem("MVVM/MVVMWindow")]
        static void Init()
        {
            MVVMTestWindow window = (MVVMTestWindow)EditorWindow.GetWindow(typeof(MVVMTestWindow));
            window.Show();
        }

        void OnGUI()
        {
            if (UIElementManager.OpenedList == null || UIElementManager.OpenedList.Count == 0)
                return;

            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 80;
            
            foreach (var openedUi in UIElementManager.OpenedList)
            {
                foreach (var kv in openedUi.ViewModelProperties)
                {
                    if (openedUi.MyViewModel is GameStartViewModel gameStartViewModel)
                    {
                        var propertyName = openedUi.ViewApplierDic[kv.Key].Name;
                        GUILayout.Label(propertyName);
                        if (propertyName == "LobbyTitle")
                        {
                            gameStartViewModel.LobbyTitle = GUILayout.TextField(gameStartViewModel.LobbyTitle);
                        } else if (propertyName == "LobbyButtonImage")
                        {
                            //Texture/OArielG/2DSimpleUIPack/Examples/Graphics/ui-small-buttons/ui-small-buttons_0
                            gameStartViewModel.LobbyButtonImage = GUILayout.TextField(gameStartViewModel.LobbyButtonImage);
                        }
                    }
                }
            }
        }
    }
}