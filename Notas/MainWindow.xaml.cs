using Notas.Database.Persistence;
using Notas.Database.Table;
using Notas.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notas
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PostItSelect_Click(null, null);

            List<PostIt> postIts = PersistencePostIt.GetAll();
            foreach (PostIt temp in postIts)
            {
                PostItField postIt = new PostItField();
                postIt.Click += PostItSelect_Click;
                postIt.TextFocus += PostIt_TextFocus;
                postIt.TextChanged += PostIt_TextChanged;
                postIt.LostFocus += PostIt_LostFocus;
                postIt.Margin = new Thickness(0, 0, 0, 15);
                postIt.Id = temp.Id;
                postIt.Text = temp.Content;
                groupPostIt.Children.Insert(0, postIt);
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
            postIt.TextFocus += PostIt_TextFocus;
            postIt.TextChanged += PostIt_TextChanged;
            postIt.LostFocus += PostIt_LostFocus;
            postIt.Margin = new Thickness(0, 0, 0, 15);
            postIt.Id = -1;
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
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach(UIElement element in groupPostIt.Children)
            {
                PostItField postIt = (PostItField) element;
                if (postIt.TextFocused)
                {
                    if (postIt.Id == -1)
                    {
                        PostIt temp = new PostIt(postIt.Text);
                        PersistencePostIt.Add(temp);
                    }
                    else
                    {
                        PostIt temp = new PostIt(postIt.Id, postIt.Text);
                        PersistencePostIt.Update(temp);
                    }

                    postIt.TextFocused = false;
                    postIt.OldText = postIt.Text;
                }
            }

            btnSave.Visibility = Visibility.Collapsed;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
            => this.WindowState = WindowState.Minimized;

        private void BtnClose_Click(object sender, RoutedEventArgs e)
            => Close();

        private void PostItSelect_Click(object sender, RoutedEventArgs e)
        {
            bool showDelete = false;
            foreach (UIElement element in groupPostIt.Children)
            {
                PostItField postIt = (PostItField)element;
                if (postIt.IsSelected == true)
                {
                    showDelete = true;
                }
            }

            btnDel.Visibility = showDelete ? Visibility.Visible : Visibility.Collapsed;
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
        }

        private void PostIt_TextChanged(object sender, TextChangedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;

            if (postIt.TextFocused == false)
                return;

            btnSave.Visibility = postIt.OldText != postIt.Text ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PostIt_LostFocus(object sender, RoutedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;
            if (string.IsNullOrWhiteSpace(postIt.Text))
                groupPostIt.Children.Remove(postIt);
        }
    }
}
