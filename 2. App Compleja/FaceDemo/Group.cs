using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FaceDemo
{
    public class Group : IGrouping<string, BitmapImage>
    {
        public Group(string key, IList<BitmapImage> bitmaps)
        {
            Key = key;
            Bitmaps = bitmaps;
        }

        public IList<BitmapImage> Bitmaps { get; }

        public IEnumerator<BitmapImage> GetEnumerator()
        {
            return Bitmaps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Key { get; }
    }
}