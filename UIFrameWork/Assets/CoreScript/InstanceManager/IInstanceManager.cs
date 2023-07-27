using System;

namespace CoreScript.InstanceManager
{
    // TODO : AsyncLoad
    public interface IInstanceManager<T>
    {
        public T GetInstance(string name, Action onComplete);
        public void ReturnInstance(T uiBase);
        public void RemoveInstance(T dialog);
    }
}