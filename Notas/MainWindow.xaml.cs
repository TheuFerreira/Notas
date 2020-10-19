using Microsoft.Win32;
using Notas.Database.Persistence;
using Notas.Database.Table;
using Notas.Extensions;
using Notas.UserControls;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notas
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private PostItField tempPost;
        private bool isSettings;
        private bool mode;


        public MainWindow()
        {
            InitializeComponent();

            LoadPreferences();
            SwitchColor();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            titleNotas.ToolTip = $"Versão {version}";

            PostItSelect_Click(null, null);

            PreviewMouseLeftButtonDown += MainWindow_PreviewMouseLeftButtonDown;
            IsVisibleChanged += MainWindow_IsVisibleChanged;
        }

        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            Point panelPosition = panelColors.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

            if (mousePosition.Y < panelPosition.Y)
            {
                if (tempPost != null)
                {
                    PersistencePostIt.UpdateColor(new PostIt(tempPost.Id, tempPost.BackgroundColor));
                    gridField.RowDefinitions[2].Height = new GridLength(0);
                    tempPost.ColorFocused = false;
                    tempPost = null;
                }
                else if (isSettings)
                {
                    gridField.RowDefinitions[2].Height = new GridLength(0);
                    isSettings = false;
                }
                groupColors.Children.Clear();
            }
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                int i = 1;
                List<PostIt> postIts = PersistencePostIt.GetAll();
                foreach (PostIt temp in postIts)
                {
                    PostItField postIt = new PostItField();
                    postIt.Click += PostItSelect_Click;
                    postIt.ColorClick += PostItSelect_ColorClick;
                    postIt.TextFocus += PostIt_TextFocus;
                    postIt.TextChanged += PostIt_TextChanged;
                    postIt.LostFocus += PostIt_LostFocus;
                    postIt.DownClick += PostIt_DownClick;
                    postIt.UpClick += PostIt_UpClick;
                    postIt.Margin = new Thickness(0, 0, 0, 10);
                    postIt.BackgroundColor = Resources["BackgroundColor"] as SolidColorBrush;
                    postIt.Id = temp.Id;
                    postIt.Text = temp.Content;
                    postIt.Position = i;
                    groupPostIt.Children.Insert(0, postIt);

                    i++;
                }
            }
            else
            {
                groupPostIt.Children.Clear();
            }
        }



        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < groupPostIt.Children.Count; i++)
            {
                PostItField temp = (PostItField)groupPostIt.Children[i];
                if (string.IsNullOrWhiteSpace(temp.Text))
                {
                    groupPostIt.Children.Remove(temp);
                    i--;
                }
            }

            PostItField postIt = new PostItField();
            postIt.Click += PostItSelect_Click;
            postIt.ColorClick += PostItSelect_ColorClick;
            postIt.TextFocus += PostIt_TextFocus;
            postIt.TextChanged += PostIt_TextChanged;
            postIt.LostFocus += PostIt_LostFocus;
            postIt.DownClick += PostIt_DownClick;
            postIt.UpClick += PostIt_UpClick;
            postIt.Margin = new Thickness(0, 0, 0, 10);
            postIt.Id = -1;
            postIt.BackgroundColor = Resources["BackgroundColor"] as SolidColorBrush;
            groupPostIt.Children.Insert(0, postIt);
            postIt.FocusTextField();

            int pos = groupPostIt.Children.Count;
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField temp = (PostItField)element;
                temp.Position = pos;
                pos--;
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < groupPostIt.Children.Count; i++)
            {
                PostItField postIt = (PostItField)groupPostIt.Children[i];
                if (postIt.IsSelected == true)
                {
                    bool del = true;
                    if (postIt.Id != -1)
                    {
                        PostIt temp = new PostIt(postIt.Id);
                        del = PersistencePostIt.Delete(temp);
                    }

                    if (del)
                    {
                        groupPostIt.Children.Remove(postIt);
                        i--;
                    }
                }
            }

            int pos = 1;
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField field = (PostItField)element;
                field.Position = pos;
                pos++;
            }

            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            for (int i = groupPostIt.Children.Count - 1; i >= 0; i--)
            {
                PostItField postIt = (PostItField)groupPostIt.Children[i];
                if (postIt.TextFocused)
                {
                    if (postIt.Id == -1)
                    {
                        PostIt temp = new PostIt(postIt.Text, postIt.BackgroundColor, postIt.Position);
                        PersistencePostIt.Add(temp);
                    }
                    else
                    {
                        PostIt temp = new PostIt(postIt.Id, postIt.Text, postIt.BackgroundColor, postIt.Position);
                        PersistencePostIt.Update(temp);
                    }

                    postIt.TextFocused = false;
                    postIt.OldText = postIt.Text;
                }
            }

            btnSave.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Visible;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            isSettings = !isSettings;

            groupColors.Children.Clear();
            GridSettings settings = new GridSettings(!mode);
            settings.ClickSwitchMode += Settings_ClickSwitchMode;
            groupColors.Children.Add(settings);
            gridField.RowDefinitions[2].Height = isSettings ? new GridLength(35) : new GridLength(0);
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

            if (sender != null)
            {
                PostItField post = (PostItField)sender;
                post.IsSelected = !post.IsSelected;
            }

            bool showDelete = false;
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField postIt = (PostItField)element;
                if (postIt.IsSelected == true)
                    showDelete = true;
            }

            btnDel.Visibility = showDelete ? Visibility.Visible : Visibility.Collapsed;
            btnSettings.Visibility = showDelete ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PostItSelect_ColorClick(object sender, RoutedEventArgs e)
        {
            PostItField post = (PostItField)sender;
            tempPost = post;

            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField temp = (PostItField)element;
                if (post.Id != temp.Id)
                    temp.ColorFocused = false;
            }

            post.ColorFocused = !post.ColorFocused;

            groupColors.Children.Clear();
            List<string> colors = EnumExtension.EnumColors();
            foreach (string color in colors)
            {
                ColorButton btn = new ColorButton();
                btn.Click += SelectColor_Click;
                btn.BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom(color);
                btn.IsSelected = btn.BackgroundColor.Color == post.BackgroundColor.Color;
                groupColors.Children.Add(btn);
            }

            PostIt_TextFocus(null, null);
            gridField.RowDefinitions[2].Height = post.ColorFocused ? new GridLength(35) : new GridLength(0);
        }

        private void PostIt_TextFocus(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField temp = (PostItField)element;
                if (temp.IsSelected == true)
                    temp.IsSelected = false;
            }

            btnDel.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = btnSave.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PostIt_TextChanged(object sender, TextChangedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;

            if (postIt.TextFocused == false)
                return;

            bool show = false;
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField temp = (PostItField)element;
                if (temp.OldText != temp.Text)
                {
                    show = true;
                    break;
                }
            }

            btnSave.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            btnSettings.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PostIt_LostFocus(object sender, RoutedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;
            if (string.IsNullOrWhiteSpace(postIt.Text))
            {
                if (postIt.Id != -1)
                {
                    PostIt temp = new PostIt(postIt.Id);
                    PersistencePostIt.Delete(temp);
                }

                groupPostIt.Children.Remove(postIt);
            }
        }



        private void Settings_ClickSwitchMode(object sender, RoutedEventArgs e)
        {
            mode = !mode;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ferreira\Notas");
            key.SetValue("Mode", mode);
            key.Close();

            BottomButton btn = (BottomButton)sender;
            btn.IsSecondText = !mode;

            SwitchColor();
        }



        private void PostIt_DownClick(object sender, RoutedEventArgs e)
        {
            PostItField field = (PostItField)sender;

            if (field.Position == 1)
                return;

            int total = groupPostIt.Children.Count;

            PostItField old = (PostItField)groupPostIt.Children[total - field.Position + 1];
            old.Position++;

            field.Position--;

            groupPostIt.Children.Remove(field);
            groupPostIt.Children.Insert(total - field.Position, field);

            PersistencePostIt.UpdatePosition(new PostIt(old.Id, old.Position));
            PersistencePostIt.UpdatePosition(new PostIt(field.Id, field.Position));
        }

        private void PostIt_UpClick(object sender, RoutedEventArgs e)
        {
            PostItField field = (PostItField)sender;
            int total = groupPostIt.Children.Count;

            if (field.Position == total)
                return;

            PostItField old = (PostItField)groupPostIt.Children[total - field.Position - 1];
            old.Position--;

            field.Position++;

            groupPostIt.Children.Remove(field);
            groupPostIt.Children.Insert(total - field.Position, field);

            PersistencePostIt.UpdatePosition(new PostIt(old.Id, old.Position));
            PersistencePostIt.UpdatePosition(new PostIt(field.Id, field.Position));
        }



        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            ColorButton btnColor = (ColorButton)sender;

            foreach (UIElement element in groupColors.Children)
            {
                ColorButton temp = (ColorButton)element;
                if (temp.BackgroundColor.Color != btnColor.BackgroundColor.Color)
                    temp.IsSelected = false;
            }

            btnColor.IsSelected = true;
            tempPost.BackgroundColor = btnColor.BackgroundColor;
        }
    


        private void LoadPreferences()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ferreira\Notas");
            if (key != null)
            {
                mode = bool.Parse(key.GetValue("Mode").ToString());
                key.Close();
            }
            else
            {
                mode = true;
            }
        }

        private void SwitchColor()
        {
            if (mode)
            {
                Resources["BackgroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["SelectionColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE8E8E8");
                Resources["TextColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("Black");
                Resources["ScrollForegroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB9B9B9");
            }
            else
            {
                Resources["BackgroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#1B1B1B");
                Resources["SelectionColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B");
                Resources["TextColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF");
                Resources["ScrollForegroundColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString("#1B1B1B");
            }
        }
    }
}
