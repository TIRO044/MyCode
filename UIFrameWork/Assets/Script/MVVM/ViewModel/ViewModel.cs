namespace MVVM.ViewModel
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    
    public class ViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void Bind(PropertyChangedEventHandler handler)
        {
            PropertyChanged -= handler;
            PropertyChanged += handler;
        }
    }
}