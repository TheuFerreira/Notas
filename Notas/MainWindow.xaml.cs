using Notas.Entities;
using Notas.Interfaces;
using Notas.Repositories;
using Notas.Screens;
using Notas.Services;
using Notas.UserControls;
using System;
using System.Diagnostics;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        private ScreenPostIt screenPostIt;
        private readonly Settings settings;
        private readonly ISettingsRepository settingsRepository;

        public MainWindow()
        {
            InitializeComponent();

            IsVisibleChanged += MainWindow_IsVisibleChanged;

            settingsRepository = new SettingsRepository();
            settings = settingsRepository.Load();

            SwitchColor();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            titleNotas.ToolTip = $"Versão {version}";
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z && !settings.AutoAdd && screenPostIt != null)
            {
                bool isTextFocused = screenPostIt.PostItFields.Where(x => x.IsTextFocused).Count() > 0;
                if (isTextFocused == false)
                    BtnAdd_Click(e.Key.ToString(), null);
            }

            base.OnPreviewKeyDown(e);
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                screenPostIt = new ScreenPostIt();
                screenPostIt.TextChanged += ScreenPostIt_TextChanged;
                screenPostIt.ToDelete += ScreenPostIt_ToDelete;
                gridField.Children.Add(screenPostIt);
            }
            else
            {
                gridField.Children.RemoveAt(0);
            }
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.AddPostIt(sender.ToString());

            if (btnSave.Visibility == Visibility.Visible)
                screenPostIt.PostItFields[0].gdButtons.Visibility = Visibility.Collapsed;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.DelPostIt();
            btnDel.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            screenPostIt.SavePostIt();

            screenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Visible);

            gridField.Children.RemoveAt(0);
            screenPostIt = new ScreenPostIt();
            screenPostIt.TextChanged += ScreenPostIt_TextChanged;
            screenPostIt.ToDelete += ScreenPostIt_ToDelete;
            gridField.Children.Add(screenPostIt);

            btnSave.Visibility = Visibility.Collapsed;
            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Collapsed;

            ScreenSettings screenSettings = new ScreenSettings(settings.IsLight, settings.DefaultFont, settings.AutoAdd);
            screenSettings.SwitchMode += Settings_SwitchMode;
            screenSettings.SwitchFont += Settings_SwitchFont;
            screenSettings.SwitchAutoAdd += Settings_AutoAdd;
            screenSettings.RenderTransform = new TranslateTransform(300, 0);
            gridField.Children.Add(screenSettings);

            screenPostIt = gridField.Children[0] as ScreenPostIt;
            screenPostIt.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(250));
            animHide.Completed += (se, ev) => gridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(250));
            animShow.Completed += (se, ev) =>
            {
                screenPostIt = null;

                btnHelp.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Visible;
            };

            ((TranslateTransform)screenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            btnBack.Visibility = Visibility.Collapsed;
            btnHelp.Visibility = Visibility.Collapsed;

            screenPostIt = new ScreenPostIt();
            screenPostIt.TextChanged += ScreenPostIt_TextChanged;
            screenPostIt.ToDelete += ScreenPostIt_ToDelete;
            screenPostIt.RenderTransform = new TranslateTransform(300, 0);
            gridField.Children.Add(screenPostIt);

            ScreenSettings screenSettings = gridField.Children[0] as ScreenSettings;
            screenSettings.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(250));
            animHide.Completed += (se, ev) => gridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(250));
            animShow.Completed += (se, ev) =>
            {
                btnAdd.Visibility = Visibility.Visible;
                btnSettings.Visibility = Visibility.Visible;
            };

            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)screenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/TheuFerreira/Notas");
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
            => this.WindowState = WindowState.Minimized;

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.Visibility == Visibility.Visible)
                if (!DialogService.ShowWarning("Tem certeza de que deseja sair sem salvar?"))
                    return;

            btnDel.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;

            Close();
        }

        private void ScreenPostIt_TextChanged(object sender, RoutedEventArgs e)
        {
            if (screenPostIt == null)
                return;

            foreach (PostItField pf in screenPostIt.PostItFields)
            {
                if (pf.IsChanged)
                {
                    screenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Collapsed);

                    btnSave.Visibility = Visibility.Visible;
                    btnDel.Visibility = Visibility.Collapsed;
                    btnSettings.Visibility = Visibility.Collapsed;
                    return;
                }
            }

            screenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Visible);
            btnSave.Visibility = Visibility.Collapsed;
            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void ScreenPostIt_ToDelete(object sender, RoutedEventArgs e)
        {
            if (screenPostIt.IsSelected)
            {
                btnDel.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Collapsed;
                btnSettings.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnDel.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                btnSettings.Visibility = Visibility.Visible;
                btnAdd.Visibility = Visibility.Visible;
            }
        }

        private void Settings_SwitchMode(object sender, RoutedEventArgs e)
        {
            settings.IsLight = (bool)sender;
            settingsRepository.Save(settings);

            SwitchColor();
        }

        private void Settings_SwitchFont(object sender, RoutedEventArgs e)
        {
            settings.DefaultFont = new FontFamily(sender.ToString());
            Resources["DefaultFont"] = new FontFamily(settings.DefaultFont.ToString());

            settingsRepository.Save(settings);
        }

        private void Settings_AutoAdd(object sender, RoutedEventArgs e)
        {
            settings.AutoAdd = (bool)sender;
            settingsRepository.Save(settings);
        }

        private void SwitchColor()
        {
            Resources["DefaultFont"] = settings.DefaultFont;

            if (settings.IsLight)
            {
                Resources["TopBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                Resources["Text"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#000");
                Resources["FieldBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#F2F2F2");
                Resources["Selection"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#e3e3e3");
                Resources["PostItBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ScrollColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#595959");
                Resources["CheckboxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#ACACAC");
                Resources["CheckboxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#ACACAC");
                Resources["ComboboxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxSelection"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#808080");
            }
            else
            {
                Resources["TopBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#171717");
                Resources["Text"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["FieldBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#303030");
                Resources["Selection"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#3A3A3A");
                Resources["PostItBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#1A1A1A");
                Resources["ScrollColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#494949");
                Resources["CheckboxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                Resources["CheckboxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#272727");
                Resources["ComboboxBackground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxForeground"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#000");
                Resources["ComboboxSelection"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#CCCCCC");
            }
        }
    }
}
