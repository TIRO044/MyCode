using Script.UIScript.Element.Lobby.ViewModel;

namespace CoreScript.UIFramework.MVVM
{
    // command가 필요할 때가 있다곤 하는데, 
// 생각보다 그렇게 많이 필요할 때가 없다.

    public interface ICommand
    {
        void Excute(string str);
    }

// Invoker
    public class EditorInvoker
    {
        public ICommand _lobbyTitleChangeCommand;
        public ICommand _lobbyImageChangeCommand;

        public void ChangeLobbyTitle(string str)
        {
            _lobbyTitleChangeCommand.Excute(str);
        }
    
        public void ChangeLobbyImage(string str)
        {
            _lobbyImageChangeCommand.Excute(str);
        }
    }

//Command
    public class GameStartViewModelCommand : ICommand
    {
        private GameStartViewModel _vm;
        public GameStartViewModelCommand(GameStartViewModel vm)
        {
            _vm = vm;
        }

        public void Excute(string str)
        {
            _vm.LobbyTitle = str;
        }
    }

    public class GameStartViewModelCommand1 : ICommand
    {      
        private GameStartViewModel _vm;
        public GameStartViewModelCommand1(GameStartViewModel vm)
        {
            _vm = vm;
        }
    
        public void Excute(string str)
        {
            _vm.LobbyButtonImage = str;
        }
    }
}