using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreScript.UIFramework.UIElement.Controller.Tab.Sample
{
    public class TabSample : MonoBehaviour, ITabController<TabSample>
    {
        public ITabController<TabSample>.OnPointerClickHandler PointerClickHandler { get; set; }


        public void RegisterOnPointClick(ITabController<TabSample>.OnPointerClickHandler handler)
        {
            PointerClickHandler -= handler;
            PointerClickHandler += handler;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointClick ..");
            PointerClickHandler?.Invoke(this);

            //UIManager.SendMessageToParents(null, gameObject);
        }

        public void OnSelected(bool select)
        {

        }
    }
}
