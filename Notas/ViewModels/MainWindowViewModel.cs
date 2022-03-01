using Notas.Screens;
using Notas.UserControls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Notas.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand AddButton { get; set; }
        public ICommand DeleteButton { get; set; }
        public ICommand SaveButton { get; set; }
        public ICommand SettingsButton { get; set; }
        public ICommand BackButton { get; set; }
        public ICommand HelpButton { get; set; }

        private bool _addVisible = true;
        public bool AddVisible
        {
            get => _addVisible;
            set
            {
                _addVisible = value;
                OnPropertyChanged("AddVisible");
            }
        }

        private bool _delVisible = false;
        public bool DelVisible
        {
            get => _delVisible;
            set
            {
                _delVisible = value;
                OnPropertyChanged("DelVisible");
            }
        }

        private bool _saveVisible = false;
        public bool SaveVisible
        {
            get => _saveVisible;
            set
            {
                _saveVisible = value;
                OnPropertyChanged("SaveVisible");
            }
        }

        private bool _settingsVisible = true;
        public bool SettingsVisible
        {
            get => _settingsVisible;
            set
            {
                _settingsVisible = value;
                OnPropertyChanged("SettingsVisible");
            }
        }

        private bool _backVisible = false;
        public bool BackVisible
        {
            get => _backVisible;
            set
            {
                _backVisible = value;
                OnPropertyChanged("BackVisible");
            }
        }

        private bool _helpVisible = false;
        public bool HelpVisible
        {
            get => _helpVisible;
            set
            {
                _helpVisible = value;
                OnPropertyChanged("HelpVisible");
            }
        }

        public ScreenPostIt ScreenPostIt { get; set; }
        public Grid GridField { get; set; }
        public Action SwitchMode { get; set; }

        public MainWindowViewModel()
        {
            AddButton = new RelayCommand(o => AddPostIt(string.Empty));
            DeleteButton = new RelayCommand(o => DeletePostIts_Click());
            SaveButton = new RelayCommand(o => SavePostIts_Click());
            SettingsButton = new RelayCommand(o => Settings_Click());
            BackButton = new RelayCommand(o => Back_Click());
            HelpButton = new RelayCommand(o => Help_Click());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddPostIt(string text)
        {
            ScreenPostIt.AddPostIt(text);

            if (AddVisible)
                ScreenPostIt.PostItFields[0].gdButtons.Visibility = Visibility.Collapsed;
        }

        private void DeletePostIts_Click()
        {
            ScreenPostIt.DelPostIt();
            DelVisible = false;
            SaveVisible = false;
            SettingsVisible = true;
            AddVisible = true;
        }

        private void SavePostIts_Click()
        {
            ScreenPostIt.SavePostIt();
            ScreenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Visible);

            GridField.Children.RemoveAt(0);
            ScreenPostIt = GenerateNewScreenPostIt();
            GridField.Children.Add(ScreenPostIt);

            SaveVisible = false;
            DelVisible = false;
            SettingsVisible = true;
        }

        private void Settings_Click()
        {
            AddVisible = false;
            SettingsVisible = false;

            ScreenSettings screenSettings = new ScreenSettings();
            screenSettings.SwitchMode += Settings_SwitchMode;
            screenSettings.RenderTransform = new TranslateTransform(300, 0);
            GridField.Children.Add(screenSettings);

            ScreenPostIt = GridField.Children[0] as ScreenPostIt;
            ScreenPostIt.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(250));
            animHide.Completed += (se, ev) => GridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(250));
            animShow.Completed += (se, ev) =>
            {
                ScreenPostIt = null;

                HelpVisible = true;
                BackVisible = true;
            };

            ((TranslateTransform)ScreenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        private void Settings_SwitchMode(object sender, RoutedEventArgs e)
        {
            SwitchMode.Invoke();
        }

        private void Back_Click()
        {
            BackVisible = false;
            HelpVisible = false;

            ScreenPostIt = GenerateNewScreenPostIt();
            ScreenPostIt.RenderTransform = new TranslateTransform(300, 0);
            GridField.Children.Add(ScreenPostIt);

            ScreenSettings screenSettings = GridField.Children[0] as ScreenSettings;
            screenSettings.RenderTransform = new TranslateTransform();

            DoubleAnimation animHide = new DoubleAnimation(0d, -300, TimeSpan.FromMilliseconds(250));
            animHide.Completed += (se, ev) => GridField.Children.RemoveAt(0);
            DoubleAnimation animShow = new DoubleAnimation(300d, 0, TimeSpan.FromMilliseconds(250));
            animShow.Completed += (se, ev) =>
            {
                AddVisible = true;
                SettingsVisible = true;
            };

            ((TranslateTransform)screenSettings.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animHide);
            ((TranslateTransform)ScreenPostIt.RenderTransform).BeginAnimation(TranslateTransform.XProperty, animShow);
        }

        public ScreenPostIt GenerateNewScreenPostIt()
        {
            ScreenPostIt screenPostIt = new ScreenPostIt();
            screenPostIt.TextChanged += ScreenPostIt_TextChanged;
            screenPostIt.ToDelete += ScreenPostIt_ToDelete;

            return screenPostIt;
        }

        public void ScreenPostIt_TextChanged(object sender, RoutedEventArgs e)
        {
            if (ScreenPostIt == null)
                return;

            foreach (PostItField pf in ScreenPostIt.PostItFields)
            {
                if (pf.IsChanged)
                {
                    ScreenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Collapsed);

                    SaveVisible = true;
                    DelVisible = false;
                    SettingsVisible = false;
                    return;
                }
            }

            ScreenPostIt.PostItFields.ForEach(x => x.gdButtons.Visibility = Visibility.Visible);
            SaveVisible = false;
            DelVisible = false;
            SettingsVisible = true;
        }

        public void ScreenPostIt_ToDelete(object sender, RoutedEventArgs e)
        {
            if (ScreenPostIt.IsSelected)
            {
                DelVisible = true;
                SaveVisible = false;
                SettingsVisible = false;
                AddVisible = false;
            }
            else
            {
                DelVisible = false;
                SaveVisible = false;
                SettingsVisible = true;
                AddVisible = true;
            }
        }

        private void Help_Click()
        {
            Process.Start("https://github.com/TheuFerreira/Notas");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
