using LibraryDesktopWPF.Commands;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LibraryDesktopWPF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void SetProperty<T>(string propName, bool value, T instance, [CallerMemberName] string commandProp = null)
        {
            PropertyInfo statusProperty = GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo commandProperty = instance.GetType().GetProperty(commandProp, BindingFlags.Public | BindingFlags.Instance);
            if (statusProperty != null && statusProperty.CanWrite)
            {
                if (commandProperty != null && commandProperty.CanRead)
                {
                    statusProperty.SetValue(this, value);
                    ((Command)commandProperty.GetValue(instance)).ChangeCanExecute();
                }
            }
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
    }
}