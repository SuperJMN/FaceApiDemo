using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Zafiro.Designer
{
    public sealed class DesignerSurface : ItemsControl
    {
        public DesignerSurface()
        {
            this.DefaultStyleKey = typeof(DesignerSurface);
        }

        public BindingBase LeftBinding { get; set; }
        public BindingBase TopBinding { get; set; }
        public BindingBase HeightBinding { get; set; }
        public BindingBase WidthBinding { get; set; }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DesignerItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DesignerItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var di = (DesignerItem) element;

            di.SetBinding(DesignerItem.LeftProperty, LeftBinding);
            di.SetBinding(DesignerItem.TopProperty, TopBinding);
            di.SetBinding(WidthProperty, WidthBinding);
            di.SetBinding(HeightProperty, HeightBinding);

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
