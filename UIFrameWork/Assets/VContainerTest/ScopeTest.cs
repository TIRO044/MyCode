using MVVM.View;
using MVVM.ViewModel;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Test
{
    public string Value { set; get; }
    public string Value1 { set; get; }
    public string Value2 { set; get; }

    public void Test1()
    {
        var propertyValues = GetType().GetProperties();

        var v = propertyValues[0].GetValue(this);
        Debug.Log(v);

        Value = "string Test!";

        var v1 = propertyValues[0].GetValue(this);
        var method = propertyValues[0].GetGetMethod();

        Debug.Log(v1);
        // 참조 형식이구만.. 

        Value = "GetGetMethod Test";

        var ob = method.Invoke(this, null);
        Debug.Log(ob);
    }
}
public class ViewModel1 : ViewModel
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public int Level { set; get; }
}

public class ViewModel2 : ViewModel
{
    public string Name { set; get; }
    public int Level { set; get; }
    public int Level2 { set; get; }
}


public class ScopeTest : LifetimeScope
{
    // Start is called before the first frame update
    protected override void Configure(IContainerBuilder builder)
    {
        //builder.Register<IFooService, FooService>(Lifetime.Scoped);
        //builder.Register<IBarService, BarService>(Lifetime.Scoped);

        //builder.Register<ViewTest>(Lifetime.Scoped);
        //builder.Register<ViewModelTest>(Lifetime.Scoped);

        // 이새낀 뭐지?
        //var t = builder.RegisterEntryPoint<ActorPresenter>(Lifetime.Scoped);
        //builder.Register<CharacterService>(Lifetime.Scoped);
        //builder.Register<IRouteSearch, AStarRouteSearch>(Lifetime.Scoped);
        //builder.Register<ActorsView>(Lifetime.Scoped);

        //var vt = Container.Resolve<ViewTest>();
        //vt.Init();
    }

    public void LoadLevel()
    {
       
    }
}

public class ViewTest1 : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<ViewModelTest>(Lifetime.Scoped);
    }
}

public class ViewModelContainer
{
    private ContainerBuilder _builder = new ContainerBuilder();
    private IObjectResolver _resolver;
    public ViewModelContainer()
    {
        _builder.Register<ViewModelTest>(Lifetime.Scoped);

        _resolver = _builder.Build();
    }
}

public class Test111
{
    [MenuItem("VContainer/Test")]
    public static void Testt()
    {
        var sco = GameObject.Find("ScopeTest");
        var test = sco.GetComponent<ScopeTest>();
        if (test != null)
        {
            test.Build();
        }
    }

    [MenuItem("VContainer/VmTest")]
    public static void Test1()
    {
        var builder = new ContainerBuilder();
        builder.Register<ViewTest>(Lifetime.Scoped);
        builder.Register<ViewModelTest>(Lifetime.Scoped);

        var resolver = builder.Build();
        var service = resolver.Resolve<ViewTest>();
        service.Init();
    }

    [MenuItem("VContainer/VmTest1")]
    public static void Test2()
    {
        var go = new GameObject();
        var viewTest1 =  go.AddComponent<ViewTest1>();
        viewTest1.Build();
    }

    [MenuItem("VContainer/ViewChange")]
    public static void Test23()
    {
        var testGob = GameObject.Find("test");
        var view = testGob.GetComponent<View>();

        foreach (var kv in view.ViewApplierDic)
        {
            Debug.Log($"{kv.Key}/{kv.Value.MyObject}");
        }

        var vm1 = view.MyViewModel as ViewModel1;
        vm1.Name = "test";
        vm1.Level = 12;
    }
}