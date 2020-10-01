using Notas.Database.Persistence;
using Notas.Database.Table;
using Notas.UserControls;
using System;
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

        public MainWindow()
        {
            InitializeComponent();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            titleNotas.ToolTip = $"Versão {version}";

            PostItSelect_Click(null, null);

            PreviewMouseLeftButtonDown += MainWindow_PreviewMouseLeftButtonDown;
            IsVisibleChanged += MainWindow_IsVisibleChanged;
        }

        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tempPost == null)
                return;

            Point mousePosition = e.GetPosition(this);
            Point panelPosition = panelColors.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

            if (mousePosition.Y < panelPosition.Y)
            {
                PersistencePostIt.UpdateColor(new PostIt(tempPost.Id, tempPost.BackgroundColor));
                gridField.RowDefinitions[2].Height = new GridLength(0);
                tempPost.ColorFocused = false;
                tempPost = null;
            }
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                List<PostIt> postIts = PersistencePostIt.GetAll();
                foreach (PostIt temp in postIts)
                {
                    PostItField postIt = new PostItField();
                    postIt.Click += PostItSelect_Click;
                    postIt.ColorClick += PostItSelect_ColorClick;
                    postIt.TextFocus += PostIt_TextFocus;
                    postIt.TextChanged += PostIt_TextChanged;
                    postIt.LostFocus += PostIt_LostFocus;
                    postIt.Margin = new Thickness(0, 0, 0, 10);
                    postIt.BackgroundColor = temp.Color;
                    postIt.Id = temp.Id;
                    postIt.Text = temp.Content;
                    groupPostIt.Children.Insert(0, postIt);
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
            postIt.Margin = new Thickness(0, 0, 0, 10);
            postIt.Id = -1;
            postIt.BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#1B1B1B");
            groupPostIt.Children.Insert(0, postIt);
            postIt.FocusTextField();
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

            btnDel.Visibility = Visibility.Collapsed;
            btnHelp.Visibility = Visibility.Visible;
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
                        PostIt temp = new PostIt(postIt.Text, postIt.BackgroundColor);
                        PersistencePostIt.Add(temp);
                    }
                    else
                    {
                        PostIt temp = new PostIt(postIt.Id, postIt.Text, postIt.BackgroundColor);
                        PersistencePostIt.Update(temp);
                    }

                    postIt.TextFocused = false;
                    postIt.OldText = postIt.Text;
                }
            }

            btnSave.Visibility = Visibility.Collapsed;
            btnHelp.Visibility = Visibility.Visible;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ainda não implementado!!!");
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
            btnHelp.Visibility = showDelete ? Visibility.Collapsed : Visibility.Visible;
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

            foreach (UIElement element in groupColors.Children)
            {
                ColorButton btnColor = (ColorButton)element;
                btnColor.IsSelected = btnColor.BackgroundColor.Color == post.BackgroundColor.Color;
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
            btnHelp.Visibility = btnSave.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
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
            btnHelp.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
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
    }
}
