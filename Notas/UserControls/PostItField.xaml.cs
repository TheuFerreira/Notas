using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para PostItField.xam
    /// </summary>
    public partial class PostItField : UserControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PostItField), new PropertyMetadata(false));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PostItField), new PropertyMetadata(""));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(PostItField), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#1B1B1B")));
        public static readonly DependencyProperty ColorFocusedProperty = DependencyProperty.Register("ColorFocused", typeof(bool), typeof(PostItField), new PropertyMetadata(false));

        public long Id
        {
            get;
            set;
        }

        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public int Position { get; set; } = 1;

        public bool ColorFocused 
        {
            get => (bool)GetValue(ColorFocusedProperty);
            set => SetValue(ColorFocusedProperty, value);
        }

        public bool TextFocused { get; set; }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public string OldText { get; set; }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => textField.Text = value;
        }

        public event RoutedEventHandler ColorClick;
        public event RoutedEventHandler Click;
        public event RoutedEventHandler TextFocus;
        public event TextChangedEventHandler TextChanged;
        public new event RoutedEventHandler LostFocus;

        public event RoutedEventHandler DownClick;
        public event RoutedEventHandler UpClick;


        public PostItField()
        {
            InitializeComponent();

            Loaded += PostItField_Loaded;
        }

        private void PostItField_Loaded(object sender, RoutedEventArgs e)
        {
            OldText = Text;
        }

        public void FocusTextField()
        {
            textField.Dispatcher.BeginInvoke((Action)(() =>
            {
                textField.Focus();
                Keyboard.Focus(textField);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }


        private void BtnColor_Click(object sender, RoutedEventArgs e)
        {
            ColorClick?.Invoke(this, e);
        }

        private void BrSelect_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void TextField_LostFocus(object sender, RoutedEventArgs e)
        {
            OldText = "";

            LostFocus?.Invoke(this, e);
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(TextProperty, textField.Text);
            TextChanged?.Invoke(this, e);
            UpdateSize();
        }

        private void TextField_GotFocus(object sender, RoutedEventArgs e)
        {
            OldText = Text;
            TextFocused = true;

            TextFocus?.Invoke(this, e);
        }

        private void TextField_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            textField.Height = textField.LineCount >= 1 ? textField.LineCount * 30 : 30;
            border.Height = textField.Height + 30;
        }


        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            DownClick?.Invoke(this, e);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            UpClick?.Invoke(this, e);
        }
    }
}
