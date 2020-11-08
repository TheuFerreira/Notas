using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para MenuButton.xam
    /// </summary>
    public partial class MenuButton : UserControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MenuButton), new PropertyMetadata(new CornerRadius(0)));

        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(MenuButton), new PropertyMetadata(12d));

        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(SolidColorBrush), typeof(MenuButton), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("Transparent")));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MenuButton), new PropertyMetadata(string.Empty));



        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

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



        public MenuButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }



        public event RoutedEventHandler Click;
    }
}
