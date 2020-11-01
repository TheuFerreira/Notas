using Microsoft.Win32;
using Notas.Screens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Notas
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    [SuppressMessage("Style", "IDE0017:Simplificar a inicialização de objeto", Justification = "<Pendente>")]
    public partial class MainWindow : Window
    {
        private bool mode;
        private ScreenPostIt screenPostIt;
        private ScreenSettings screenSettings;
        private FontFamily defaultFont;



        public MainWindow()
        {
            InitializeComponent();

            LoadPreferences();
            SwitchColor();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            titleNotas.ToolTip = $"Versão {version}";

            IsVisibleChanged += MainWindow_IsVisibleChanged;

            topBar.MouseDown += TopBar_MouseDown;
            btnAdd.Click += BtnAdd_Click;
            btnBack.Click += BtnBack_Click;
            btnSettings.Click += BtnSettings_Click;
            btnMinimize.Click += BtnMinimize_Click;
            btnClose.Click += BtnClose_Click;
        }



        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                if (btnBack.Visibility == Visibility.Visible)
                {
                    screenSettings = new ScreenSettings(mode);
                    screenSettings.SwitchMode += Settings_SwitchMode;
                    screenSettings.SwitchFont += Settings_SwitchFont;
                    gridField.Children.Add(screenSettings);
                }
                else
                {
                    screenPostIt = new ScreenPostIt(defaultFont);
                    screenPostIt.Select += PostItSelect_Click;
                    screenPostIt.TextFocus += PostIt_TextFocus;
                    screenPostIt.TextChanged += PostIt_TextChanged;
                    gridField.Children.Add(screenPostIt);
                }
            }
            else
            {
                gridField.Children.RemoveAt(0);
            }
        }



        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.AddPostIt();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.DelPostIt();

            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.SavePostIt();

            btnSave.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            btnBack.Visibility = Visibility.Collapsed;

            screenPostIt = new ScreenPostIt(defaultFont);
            screenPostIt.Select += PostItSelect_Click;
            screenPostIt.TextFocus += PostIt_TextFocus;
            screenPostIt.TextChanged += PostIt_TextChanged;
            screenPostIt.RenderTransform = new TranslateTransform(300, 0);
            gridField.Children.Add(screenPostIt);

            ScreenSettings screenSettings = gridField.Children[0] as ScreenSettings;
            screenSettings.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(500));
            animHide.Completed += (se, ev) => gridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(500));
            animShow.Completed += (se, ev) =>
            {
                btnAdd.Visibility = Visibility.Visible;
                btnSettings.Visibility = Visibility.Visible;

                btnBack.Visibility = Visibility.Collapsed;
            };

            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)screenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Collapsed;

            ScreenSettings screenSettings = new ScreenSettings(mode);
            screenSettings.SwitchMode += Settings_SwitchMode;
            screenSettings.SwitchFont += Settings_SwitchFont;
            screenSettings.RenderTransform = new TranslateTransform(300, 0);
            gridField.Children.Add(screenSettings);

            screenPostIt = gridField.Children[0] as ScreenPostIt;
            screenPostIt.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(500));
            animHide.Completed += (se, ev) => gridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(500));
            animShow.Completed += (se, ev) =>
            {
                screenPostIt = null;

                btnAdd.Visibility = Visibility.Collapsed;
                btnSettings.Visibility = Visibility.Collapsed;

                btnBack.Visibility = Visibility.Visible;
            };

            ((TranslateTransform)screenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
            => this.WindowState = WindowState.Minimized;

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.Visibility == Visibility.Visible)
                if (MessageBox.Show("Tem certeza de que deseja sair sem salvar?", "AVISO", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

            btnDel.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;

            Close();
        }



        private void PostItSelect_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.Visibility == Visibility.Visible)
                return;

            bool showDelete = (bool)sender;
            btnDel.Visibility = showDelete ? Visibility.Visible : Visibility.Collapsed;
            btnSettings.Visibility = showDelete ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PostItSelect_ColorClick(object sender, RoutedEventArgs e)
        {
            /*
            PostItField post = (PostItField)sender;
            tempPost = post;

            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField temp = (PostItField)element;
                if (post.Id != temp.Id)
                    temp.ColorFocused = false;
            }

            post.ColorFocused = !post.ColorFocused;

            groupBottom.Children.Clear();
            List<string> colors = EnumExtension.EnumColors();
            foreach (string color in colors)
            {
                ColorButton btn = new ColorButton();
                btn.Click += SelectColor_Click;
                btn.BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom(color);
                btn.IsSelected = btn.BackgroundColor.Color == post.BackgroundColor.Color;
                groupBottom.Children.Add(btn);
            }

            PostIt_TextFocus(null, null);
            gridField.RowDefinitions[2].Height = post.ColorFocused ? new GridLength(35) : new GridLength(0);
            */
        }

        private void PostIt_TextFocus(object sender, RoutedEventArgs e)
        {
            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = btnSave.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PostIt_TextChanged(object sender, RoutedEventArgs e)
        {
            bool show = (bool)sender;
            btnSave.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            btnSettings.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }



        private void Settings_SwitchMode(object sender, RoutedEventArgs e)
        {
            mode = (bool)sender;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ferreira\Notas");
            key.SetValue("Mode", mode);
            key.Close();

            SwitchColor();
        }

        private void Settings_SwitchFont(object sender, RoutedEventArgs e)
        {
            defaultFont = new FontFamily(sender.ToString());

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ferreira\Notas");
            key.SetValue("DefaultFont", defaultFont.ToString());
            key.Close();
        }



        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            /*
            ColorButton btnColor = (ColorButton)sender;

            foreach (UIElement element in groupBottom.Children)
            {
                ColorButton temp = (ColorButton)element;
                if (temp.BackgroundColor.Color != btnColor.BackgroundColor.Color)
                    temp.IsSelected = false;
            }

            btnColor.IsSelected = true;
            tempPost.BackgroundColor = btnColor.BackgroundColor;
            */
        }



        private void LoadPreferences()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ferreira\Notas");
            if (key != null)
            {
                mode = bool.Parse(key.GetValue("Mode").ToString());

                object keyFont = key.GetValue("DefaultFont");
                defaultFont = new FontFamily(keyFont != null ? keyFont.ToString() : "Segoe UI");
                key.Close();
            }
            else
            {
                mode = true;
                defaultFont = new FontFamily("Segoe UI");
            }
        }

        private void SwitchColor()
        {
            Resources["DefaultFont"] = defaultFont;

            if (mode)
            {
                Resources["BackgroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["SelectionColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE8E8E8");
                Resources["TextColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("Black");
                Resources["ScrollForegroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB9B9B9");
                Resources["CheckBoxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B");
                Resources["CheckBoxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
            }
            else
            {
                Resources["BackgroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#1B1B1B");
                Resources["SelectionColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B");
                Resources["TextColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ScrollForegroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#1B1B1B");
                Resources["CheckBoxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["CheckBoxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B");
            }
        }
    }
}
