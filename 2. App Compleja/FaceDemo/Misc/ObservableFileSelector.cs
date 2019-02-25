using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using ReactiveUI;

namespace FaceDemo.Misc
{
    public class ObservableFileSelector
    {
        public ObservableFileSelector()
        {
            SelectFilesCommand = ReactiveCommand.CreateFromTask(SelectFiles);
        }
        private static async Task<IReadOnlyList<StorageFile>> SelectFiles()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.CommitButtonText = "Open";
            return await picker.PickMultipleFilesAsync();
        }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectFilesCommand { get; }
    }
}