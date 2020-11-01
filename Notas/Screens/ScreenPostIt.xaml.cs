using Notas.Database.Persistence;
using Notas.Database.Table;
using Notas.UserControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.Screens
{
    /// <summary>
    /// Interação lógica para ScreenPostIt.xam
    /// </summary>
    public partial class ScreenPostIt : UserControl
    {
        private readonly FontFamily fontFamily;

        public event RoutedEventHandler Select;
        public event RoutedEventHandler TextFocus;
        public event RoutedEventHandler TextChanged;

        public ScreenPostIt(FontFamily fontFamily)
        {
            InitializeComponent();

            this.fontFamily = fontFamily;

            int i = 1;
            List<PostIt> postIts = PersistencePostIt.GetAll();
            foreach (PostIt temp in postIts)
            {
                PostItField postIt = new PostItField();
                postIt.Click += PostItSelect_Click;
                /*postIt.ColorClick += PostItSelect_ColorClick;*/
                postIt.TextFocus += PostIt_TextFocus;
                postIt.TextChanged += PostIt_TextChanged;
                postIt.LostFocus += PostIt_LostFocus;
                postIt.DownClick += PostIt_DownClick;
                postIt.UpClick += PostIt_UpClick;
                postIt.FontFamily = fontFamily;
                postIt.Margin = new Thickness(0, 0, 0, 10);
                postIt.BackgroundColor = Resources["BackgroundColor"] as SolidColorBrush;
                postIt.Id = temp.Id;
                postIt.Text = temp.Content;
                postIt.Position = i;
                group.Children.Insert(0, postIt);

                i++;
            }
        }

        public void AddPostIt()
        {
            for (int i = 0; i < group.Children.Count; i++)
            {
                PostItField temp = (PostItField)group.Children[i];
                if (string.IsNullOrWhiteSpace(temp.Text))
                {
                    group.Children.Remove(temp);
                    i--;
                }
            }

            PostItField postIt = new PostItField();
            postIt.Click += PostItSelect_Click;
            //postIt.ColorClick += PostItSelect_ColorClick;
            postIt.TextFocus += PostIt_TextFocus;
            postIt.TextChanged += PostIt_TextChanged;
            postIt.LostFocus += PostIt_LostFocus;
            postIt.DownClick += PostIt_DownClick;
            postIt.UpClick += PostIt_UpClick;
            postIt.FontFamily = fontFamily;
            postIt.Margin = new Thickness(0, 0, 0, 10);
            postIt.Id = -1;
            postIt.BackgroundColor = Resources["BackgroundColor"] as SolidColorBrush;
            group.Children.Insert(0, postIt);
            postIt.FocusTextField();

            int pos = group.Children.Count;
            foreach (UIElement element in group.Children)
            {
                PostItField temp = (PostItField)element;
                temp.Position = pos;
                pos--;
            }
        }

        public void DelPostIt()
        {
            for (int i = 0; i < group.Children.Count; i++)
            {
                PostItField postIt = (PostItField)group.Children[i];
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
                        group.Children.Remove(postIt);
                        i--;
                    }
                }
            }

            int pos = 1;
            foreach (UIElement element in group.Children)
            {
                PostItField field = (PostItField)element;
                field.Position = pos;
                pos++;
            }

        }

        public void SavePostIt()
        {
            for (int i = group.Children.Count - 1; i >= 0; i--)
            {
                PostItField postIt = (PostItField)group.Children[i];
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
        }

        private void PostItSelect_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                PostItField post = (PostItField)sender;
                post.IsSelected = !post.IsSelected;
            }

            bool showDelete = false;
            foreach (UIElement element in group.Children)
            {
                PostItField postIt = (PostItField)element;
                if (postIt.IsSelected == true)
                    showDelete = true;
            }

            Select?.Invoke(showDelete, e);
        }

        private void PostIt_TextFocus(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in group.Children)
            {
                PostItField temp = (PostItField)element;
                if (temp.IsSelected == true)
                    temp.IsSelected = false;
            }

            TextFocus?.Invoke(sender, e);
        }

        private void PostIt_TextChanged(object sender, TextChangedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;

            if (postIt.TextFocused == false)
                return;

            bool show = false;
            foreach (UIElement element in group.Children)
            {
                PostItField temp = (PostItField)element;
                if (temp.OldText != temp.Text)
                {
                    show = true;
                    break;
                }
            }

            TextChanged?.Invoke(show, e);
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

                group.Children.Remove(postIt);
            }
        }

        private void PostIt_DownClick(object sender, RoutedEventArgs e)
        {
            PostItField field = (PostItField)sender;

            if (field.Position == 1)
                return;

            int total = group.Children.Count;

            PostItField old = (PostItField)group.Children[total - field.Position + 1];
            old.Position++;

            field.Position--;

            group.Children.Remove(field);
            group.Children.Insert(total - field.Position, field);

            PersistencePostIt.UpdatePosition(new PostIt(old.Id, old.Position));
            PersistencePostIt.UpdatePosition(new PostIt(field.Id, field.Position));
        }

        private void PostIt_UpClick(object sender, RoutedEventArgs e)
        {
            PostItField field = (PostItField)sender;
            int total = group.Children.Count;

            if (field.Position == total)
                return;

            PostItField old = (PostItField)group.Children[total - field.Position - 1];
            old.Position--;

            field.Position++;

            group.Children.Remove(field);
            group.Children.Insert(total - field.Position, field);

            PersistencePostIt.UpdatePosition(new PostIt(old.Id, old.Position));
            PersistencePostIt.UpdatePosition(new PostIt(field.Id, field.Position));
        }
    }
}
