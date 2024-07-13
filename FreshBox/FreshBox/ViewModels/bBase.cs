using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FreshBox.ViewModels
{
    public class bBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
