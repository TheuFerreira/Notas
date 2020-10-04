using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para BottomButton.xam
    /// </summary>
    public partial class BottomButton : UserControl
    {
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(SolidColorBrush), typeof(BottomButton));
        public static readonly DependencyProperty IsSecondTextProperty = DependencyProperty.Register("IsSecondText", typeof(bool), typeof(BottomButton), new PropertyMetadata(false));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BottomButton));
        public static readonly DependencyProperty SecondTextProperty = DependencyProperty.Register("SecondText", typeof(string), typeof(BottomButton));


        public SolidColorBrush SelectionColor
        {
            get => (SolidColorBrush)GetValue(SelectionColorProperty);
            set => SetValue(SelectionColorProperty, value);
        }

        public bool IsSecondText
        {
            get => (bool)GetValue(IsSecondTextProperty);
            set => SetValue(IsSecondTextProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string SecondText
        {
            get => (string)GetValue(SecondTextProperty);
            set => SetValue(SecondTextProperty, value);
        }


        public BottomButton()
        {
            InitializeComponent();
        }



        public event RoutedEventHandler Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
