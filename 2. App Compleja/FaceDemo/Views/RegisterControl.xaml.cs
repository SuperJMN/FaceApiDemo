using System;
using Windows.UI.Xaml;
using FaceDemo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FaceDemo.Views
{
    public sealed partial class RegisterControl
    {
        public RegisterControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = (RegisterViewModel)DataContext;
            vm.LoadPeopleCommand.Execute().Subscribe();
        }        
    }
}
