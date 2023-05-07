using CoreScript.UIFramework.UIElement;
using UnityEngine.UI;

namespace Script.UIScript.Element.Lobby.View
{
    public class GameStartView : UIElementBase
    {
        public Button StartButton;

        protected override void OnAwake()
        {
            StartButton = GetComponentInChildren<Button>(true);
            if (StartButton != null)
            {
                StartButton.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            UIElementManager.Close(this);
        }
    }
}