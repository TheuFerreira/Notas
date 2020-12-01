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
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(PostItField), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FFF")));
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(PostItField), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#000")));
        public new static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register("IsFocused", typeof(bool), typeof(PostItField), new PropertyMetadata(false));
        public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register("IsFixed", typeof(bool), typeof(PostItField), new PropertyMetadata(false));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PostItField), new PropertyMetadata(string.Empty));



        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public SolidColorBrush TextColor
        {
            get => (SolidColorBrush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public long Id { get; set; }

        public bool IsChanged { get; set; }

        public bool IsFixed
        {
            get => (bool)GetValue(IsFixedProperty);
            set => SetValue(IsFixedProperty, value);
        }

        public new bool IsFocused
        {
            get => (bool)GetValue(IsFocusedProperty);
            set => SetValue(IsFocusedProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsTextFocused { get; set; }



        public PostItField(string text)
        {
            InitializeComponent();

            Margin = new Thickness(0, 5, 0, 5);
            Text = text;
            bd.Height = textField.Height + 30;

            bdSelect.MouseLeftButtonDown += BdSelect_MouseLeftButtonDown;
            tbColor.MouseLeftButtonDown += TbColor_MouseLeftButtonDown;
            tbFontColor.MouseLeftButtonDown += TbFontColor_MouseLeftButtonDown;
            tbFixed.MouseLeftButtonDown += TbFixed_MouseLeftButtonDown;
            tbDown.MouseLeftButtonDown += TbDown_MouseLeftButtonDown;
            tbUp.MouseLeftButtonDown += TbUp_MouseLeftButtonDown;

            textField.GotFocus += TextField_GotFocus;
            textField.LostFocus += TextField_LostFocus;
            textField.TextChanged += TextField_TextChanged;
        }

        public void FocusTextField()
        {
            textField.Dispatcher.BeginInvoke((Action)(() =>
            {
                textField.Focus();
                Keyboard.Focus(textField);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }



        private void BdSelect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsFocused = !IsFocused;
            SelectClick?.Invoke(this, e);
        }

        private void TbColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ColorClick?.Invoke(this, e);
        }

        private void TbFontColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FontColorClick?.Invoke(this, e);
        }

        private void TbFixed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsFixed = !IsFixed;
            FixedClick?.Invoke(this, e);
        }

        private void TbDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DownClick?.Invoke(this, e);
        }

        private void TbUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpClick?.Invoke(this, e);
        }



        private void TextField_GotFocus(object sender, RoutedEventArgs e)
        {
            IsTextFocused = true;
        }

        private void TextField_LostFocus(object sender, RoutedEventArgs e)
        {
            IsTextFocused = false;
            LostFocus?.Invoke(this, e);
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsChanged = true;
            bd.Height = textField.Height + 30;
        
            TextChanged?.Invoke(this, e);
        }



        public event RoutedEventHandler SelectClick;
        public event RoutedEventHandler ColorClick;
        public event RoutedEventHandler FontColorClick;
        public event RoutedEventHandler FixedClick;
        public event RoutedEventHandler DownClick;
        public event RoutedEventHandler UpClick;
        public new event RoutedEventHandler LostFocus;
        public event TextChangedEventHandler TextChanged;
    }
}
