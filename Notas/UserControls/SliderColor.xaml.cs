using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para SliderColor.xam
    /// </summary>
    public partial class SliderColor : UserControl
    {
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(SolidColorBrush), typeof(SliderColor));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(SliderColor));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(SliderColor));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SliderColor));



        public new SolidColorBrush Background
        {
            get => (SolidColorBrush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }



        private bool _isText;



        public SliderColor()
        {
            InitializeComponent();

            sd.ValueChanged += Sd_ValueChanged;

            tb.GotFocus += Tb_GotFocus;
            tb.TextChanged += Tb_TextChanged;
            tb.LostFocus += Tb_LostFocus;
        }

        private void Sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ValueChanged?.Invoke(sender, e);

            if (!_isText)
                tb.Text = e.NewValue.ToString("N0");
        }



        private void Tb_GotFocus(object sender, RoutedEventArgs e)
        {
            _isText = true;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(tb.Text, out double res) && _isText)
            {
                sd.Value = res;
            }
        }

        private void Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            _isText = false;
        }



        public event RoutedPropertyChangedEventHandler<double> ValueChanged;
    }
}
