using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer.Unity;


public interface IFooService
{
}

public class FooService : IFooService
{
}

public interface IBarService
{
    IFooService GetFoo();
}

public class BarService : IBarService
{
    private readonly IFooService _fooService;

    public BarService(IFooService fooService)
    {
        _fooService = fooService;
    }

    public IFooService GetFoo()
    {
        return _fooService;
    }
}

public class ViewModelTest : INotifyPropertyChanged
{
    public ViewModelTest()
    {
        Debug.Log("dd");
    }

    private string _test;
    public string Test
    {
        get => _test;
        set
        {
            _test = value;
            OnPropertyChanged();
        }
    }

    #region Notify
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    #endregion
}

public class ViewTest 
{
    private readonly ViewModelTest _vm;

    public ViewTest(ViewModelTest vm)
    {
        this._vm = vm;
    }

    public void Init()
    {
        Debug.Log("init");
    }

    public void OnPropertyChanged(object sender, PropertyChangedEventArgs arg)
    {
        switch (arg.PropertyName)
        {
            case nameof(ViewModelTest.Test):
                break;
        }
    }
}

public interface IRouteSearch
{

}

public class AStarRouteSearch : IRouteSearch
{

}

public class CharacterService
{
    private readonly IRouteSearch _routeSearch;

    public CharacterService(IRouteSearch routeSearch)
    {
        _routeSearch = routeSearch;
    }
}

public class ActorsView : MonoBehaviour
{
}

public class ActorPresenter : IStartable
{
    readonly CharacterService service;
    readonly ActorsView actorsView;

    public ActorPresenter(
        CharacterService service,
        ActorsView actorsView)
    {
        this.service = service;
        this.actorsView = actorsView;
    }

    void IStartable.Start()
    {
        // Scheduled at Start () on VContainer's own PlayerLoopSystem.
    }
}