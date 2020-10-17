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
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(TopButton));

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

        public SolidColorBrush TextColor
        {
            get => (SolidColorBrush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
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
