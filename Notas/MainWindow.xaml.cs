using Notas.Entities;
using Notas.Interfaces;
using Notas.Repositories;
using Notas.Services;
using Notas.ViewModels;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Notas
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly Settings settings;
        private readonly ISettingsRepository settingsRepository;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel();
            InitializeComponent();
            DataContext = viewModel;
            viewModel.GridField = gridField;
            viewModel.SwitchMode = SwitchColor;

            settingsRepository = new SettingsRepository();
            settings = settingsRepository.Load();

            SwitchColor();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            titleNotas.ToolTip = $"Versão {version}";
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z && !settings.AutoAdd && viewModel.ScreenPostIt != null)
            {
                bool isTextFocused = viewModel.ScreenPostIt.PostItFields.Where(x => x.IsTextFocused).Count() > 0;
                if (isTextFocused == false)
                {
                    viewModel.AddPostIt(e.Key.ToString());
                }
            }

            base.OnPreviewKeyDown(e);
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                viewModel.ScreenPostIt = viewModel.GenerateNewScreenPostIt();
                viewModel.GridField.Children.Add(viewModel.ScreenPostIt);
            }
            else
            {
                viewModel.GridField.Children.RemoveAt(0);
            }
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SaveVisible)
                if (!DialogService.ShowWarning("Tem certeza de que deseja sair sem salvar?"))
                    return;

            viewModel.DelVisible = false;
            viewModel.SaveVisible = false;
            viewModel.SettingsVisible = true;

            Close();
        }

        public void SwitchColor()
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
