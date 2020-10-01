using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para ColorButton.xam
    /// </summary>
    public partial class ColorButton : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(ColorButton), new PropertyMetadata(null));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ColorButton), new PropertyMetadata(false));

        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public event RoutedEventHandler Click;

        public ColorButton()
        {
            InitializeComponent();
        }

        private void BtnClick_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
