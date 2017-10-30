using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zafiro.Designer
{
    public class DesignerItem : ContentControl
    {
        public DesignerItem()
        {
            this.DefaultStyleKey = typeof(DesignerItem);
        }
        
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(
            "Left", typeof(double), typeof(DesignerItem), new PropertyMetadata(default(double)));

        public double Left
        {
            get { return (double) GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(
            "Top", typeof(double), typeof(DesignerItem), new PropertyMetadata(default(double)));

  
        public double Top
        {
            get { return (double) GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
    }
}