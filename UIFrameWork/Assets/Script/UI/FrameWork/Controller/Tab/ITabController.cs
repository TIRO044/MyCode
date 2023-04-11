using UnityEngine;

namespace GBS.UI
{
    using UnityEngine.EventSystems;
    public interface ITabController<T> : IPointerClickHandler where T : MonoBehaviour
    {
        public delegate void OnPointerClickHandler(T monoBehaviour);

        OnPointerClickHandler PointerClickHandler { get; set; }

        void RegisterOnPointClick(OnPointerClickHandler handler);

        void OnSelected(bool select);
    }
}
