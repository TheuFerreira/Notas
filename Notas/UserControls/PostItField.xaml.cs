using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para PostItField.xam
    /// </summary>
    public partial class PostItField : UserControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PostItField), new PropertyMetadata(false));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PostItField), new PropertyMetadata(""));

        public long Id
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => textField.Text = value;
        }

        public event RoutedEventHandler Click;
        public event RoutedEventHandler TextFocus;
        public new event RoutedEventHandler LostFocus;

        public PostItField()
        {
            InitializeComponent();
        }

        public void FocusTextField()
        {
            textField.Dispatcher.BeginInvoke((Action)(() =>
            {
                textField.Focus();
                Keyboard.Focus(textField);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void BrSelect_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
            Click?.Invoke(sender, e);
        }

        private void TextField_LostFocus(object sender, RoutedEventArgs e)
            => LostFocus?.Invoke(this, e);

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
            => SetValue(TextProperty, textField.Text);

        private void TextField_GotFocus(object sender, RoutedEventArgs e)
            => TextFocus?.Invoke(this, e);
    }
}
