using CoreScript.UIFramework.UIElement;
using VContainerTest;

namespace CoreScript.UIFramework.MVVM.View
{
    public class TempSlotView : UIElementBase
    {
        protected override void OnAwake()
        {
            if (MyViewModelBase is ViewModel1 vm1)
            {
                vm1.Name = "테스트얌";
                vm1.Level = 23324;
            }
        }

        public void Test_ChangeViewModelProperty(string name, int level)
        {
            if (MyViewModelBase is ViewModel1 vm1)
            {
                vm1.Name = name;
                vm1.Level = level;
            }
        }
    }
}
