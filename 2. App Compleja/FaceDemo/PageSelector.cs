using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FaceDemo
{
    public class PageSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
            {
                return base.SelectTemplateCore(item, container);
            }

            var templateName = item.GetType().Name.Replace("ViewModel", "Template");
            return (DataTemplate) Application.Current.Resources[templateName];           
        }
    }
}