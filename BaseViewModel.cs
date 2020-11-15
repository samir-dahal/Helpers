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
        public void Set<T>(string propName, bool value, T instance, [CallerMemberName] string commandProp = null)
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
        //xamarin push / pop page / alert
        public async Task Alert(string title, string message, string cancel = "OK")
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        public async Task PushPage(Page page)
        {
            await App.Current.MainPage.Navigation.PushAsync(page);
        }
        public async Task PopPage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
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
