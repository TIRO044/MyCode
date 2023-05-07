namespace Script.UIScript.Element.Lobby.ViewModel{
    public class GameStartViewModel : CoreScript.UIFramework.MVVM.ViewModel.ViewModel
    {
        private string _lobbyTitle = "test";
        public string LobbyTitle
        {
            get => _lobbyTitle;
            set
            {
                if (_lobbyTitle == value)
                    return;
                
                _lobbyTitle = value;
                OnPropertyChanged();
            }
        }

        private string _lobbyButtonImage = "Texture/OArielG/2DSimpleUIPack/Examples/Graphics/ui-small-buttons/ui-small-buttons_0";
        public string LobbyButtonImage
        {
            get => _lobbyButtonImage;
            set
            {
                if (_lobbyButtonImage == value)
                    return;
                
                _lobbyButtonImage = value;
                OnPropertyChanged();
            }
        }
    }
}
