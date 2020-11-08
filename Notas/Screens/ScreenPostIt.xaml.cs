using Notas.Database.Persistence;
using Notas.Database.Table;
using Notas.UserControls;
using System.Collections.Generic;
using System.Linq;
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
        public List<PostItField> PostItFields { get; set; }

        public bool IsSelected { get; set; }



        public ScreenPostIt()
        {
            InitializeComponent();

            Loaded += ScreenPostIt_Loaded;
        }

        public void AddPostIt()
        {
            NewPostItField(new PostIt());
        }

        public void SavePostIt()
        {
            int i = 1;
            foreach (PostItField pf in PostItFields)
            {
                PostIt postIt = new PostIt(pf.Id, pf.textField.Text, (SolidColorBrush)new BrushConverter().ConvertFrom("#1b1b1b"), i);
                if (pf.Id == -1)
                    PersistencePostIt.Add(postIt);
                else
                    PersistencePostIt.Update(postIt);
                i++;
            }
        }

        public void DelPostIt()
        {
            IsSelected = false;

            List<PostItField> toDelete = PostItFields.Where(x => x.IsFocused).Cast<PostItField>().ToList();
            foreach (PostItField pf in toDelete)
            {
                PostItFields.Remove(pf);
                group.Children.Remove(pf);

                PostIt postIt = new PostIt(pf.Id);
                PersistencePostIt.Delete(postIt);
            }
            PostItFields.ForEach(x => x.textField.IsEnabled = true);
            SavePostIt();
        }



        private void ScreenPostIt_Loaded(object sender, RoutedEventArgs e)
        {
            PostItFields = new List<PostItField>();
            List<PostIt> postIts = PersistencePostIt.GetAll();
            foreach (PostIt post in postIts)
            {
                NewPostItField(post);
            }
        }

        private void NewPostItField(PostIt post)
        {
            PostItField pf = new PostItField(post.Content);

            pf.SelectClick += Pf_SelectClick;
            pf.LostFocus += Pf_LostFocus;
            pf.TextChanged += Pf_TextChanged;
            pf.DownClick += Pf_DownClick;
            pf.UpClick += Pf_UpClick;

            pf.Id = post.Id;

            group.Children.Insert(0, pf);
            PostItFields.Insert(0, pf);

            if (post.Id == -1)
                pf.FocusTextField();
        }



        private void Pf_SelectClick(object sender, RoutedEventArgs e)
        {
            int id = PostItFields.Where(x => x.IsFocused).Count();
            IsSelected = id > 0;

            PostItFields.ForEach(x => x.textField.IsEnabled = id == 0);
            ToDelete?.Invoke(this, e);
        }

        private void Pf_LostFocus(object sender, RoutedEventArgs e)
        {
            PostItField pf = (PostItField)sender;

            if (pf.Id == -1 && string.IsNullOrWhiteSpace(pf.Text))
            {
                PostItFields.Remove(pf);
                group.Children.Remove(pf);

                TextChanged?.Invoke(this, e);
            }
        }

        private void Pf_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void Pf_DownClick(object sender, RoutedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;
            int index = PostItFields.IndexOf(postIt);

            if (index == PostItFields.Count - 1)
                return;

            PostItFields.RemoveAt(index);
            PostItFields.Insert(index + 1, postIt);

            group.Children.RemoveAt(index);
            group.Children.Insert(index + 1, postIt);

            SavePostIt();
        }

        private void Pf_UpClick(object sender, RoutedEventArgs e)
        {
            PostItField postIt = (PostItField)sender;
            int index = PostItFields.IndexOf(postIt);

            if (index == 0)
                return;

            PostItFields.RemoveAt(index);
            PostItFields.Insert(index - 1, postIt);

            group.Children.RemoveAt(index);
            group.Children.Insert(index - 1, postIt);

            SavePostIt();
        }



        public event RoutedEventHandler TextChanged;
        public event RoutedEventHandler ToDelete;
    }
}
