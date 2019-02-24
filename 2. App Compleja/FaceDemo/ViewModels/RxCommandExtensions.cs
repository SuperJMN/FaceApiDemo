using System;
using ReactiveUI;

namespace FaceDemo.ViewModels
{
    public static class RxCommandExtensions 
    {
        public static IDisposable HandleErrors(this ReactiveCommand command, IDialogService dialogService)
        {
            return command.ThrownExceptions.Subscribe(x => dialogService.ShowException(x));
        }
    }
}