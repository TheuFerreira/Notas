using Notas.Entities;
using Notas.Interfaces;
using Notas.Repositories;
using Notas.Screens;
using Notas.Services;
using Notas.ViewModels;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

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

            IsVisibleChanged += MainWindow_IsVisibleChanged;

            settingsRepository = new SettingsRepository();
            settings = settingsRepository.Load();

            viewModel.SwitchColor();

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
                viewModel.ScreenPostIt = new ScreenPostIt();
                viewModel.ScreenPostIt.TextChanged += viewModel.ScreenPostIt_TextChanged;
                viewModel.ScreenPostIt.ToDelete += viewModel.ScreenPostIt_ToDelete;
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
    }
}
