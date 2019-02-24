using System;
using ReactiveUI;

namespace FaceDemo
{
    public class SectionViewModel
    {
        public string Name { get; set; }
        public ReactiveObject ViewModel { get; set; }
        public Uri Icon { get; set; }
    }
}