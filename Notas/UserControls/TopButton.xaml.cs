using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para TopButton.xam
    /// </summary>
    public partial class TopButton : UserControl
    {
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(SolidColorBrush), typeof(TopButton));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(TopButton));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TopButton), new PropertyMetadata(""));

        public SolidColorBrush SelectionColor
        {
            get => (SolidColorBrush)GetValue(SelectionColorProperty);
            set => SetValue(SelectionColorProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public CornerRadius Radius
        {
            get => (CornerRadius)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public event RoutedEventHandler Click;

        public TopButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
