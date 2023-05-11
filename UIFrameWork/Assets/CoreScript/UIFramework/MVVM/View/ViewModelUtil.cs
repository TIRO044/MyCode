using System;
using System.Linq;
using System.Reflection;

namespace CoreScript.UIFramework.MVVM.View
{
    public static class ViewModelUtil
    {
        private static Assembly _assembly;
        private static Type[] _vmTypes;

        private static Type[] VMTypes
        {
            get
            {
                if (_vmTypes == null)
                {
                    _assembly = Assembly.GetAssembly(typeof(ViewModel.ViewModelBase));
                    _vmTypes = _assembly.GetTypes().Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(ViewModel.ViewModelBase))).ToArray();
                }
                return _vmTypes;
            }
        }

        public static Type GetType(string typeStr)
        {
            var name = typeStr.Split('.').Last();
            foreach (var type in VMTypes)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }

            return null;
        }
    }
}