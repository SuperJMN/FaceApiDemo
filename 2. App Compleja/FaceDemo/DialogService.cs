using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.ProjectOxford.Face;

namespace FaceDemo
{
    public class DialogService : IDialogService
    {
        public async Task<IUICommand> ShowException(Exception exception)
        {
            var exceptionMessage = exception.Message;
            if (exception is FaceAPIException apiException)
            {
                exceptionMessage = apiException.ErrorCode + ": " + apiException.ErrorMessage;
            }
            else
            {
                exceptionMessage = exception.Message;
            }

            var messageDialog = new MessageDialog(exceptionMessage, "Error");

            return await messageDialog.ShowAsync();
        }
    }
}