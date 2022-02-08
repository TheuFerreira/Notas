using Notas.Database.Interfaces;
using Notas.Database.Models;
using Notas.Database.Repositories;
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

        private readonly IPostItRepository postItRepository;

        public ScreenPostIt()
        {
            InitializeComponent();

            PostItFields = new List<PostItField>();
            Loaded += ScreenPostIt_Loaded;

            IDbRepository dbRepository = new DbRepository();
            postItRepository = new PostItRepository(dbRepository);
        }

        public void AddPostIt(string key = "")
        {
            NewPostItField(new PostIt(), key);
        }

        public void SavePostIt()
        {
            int i = 1;
            foreach (PostItField pf in PostItFields)
            {
                int position = pf.IsFixed ? -1 : i;

                PostIt postIt = new PostIt(pf.Id, pf.textField.Text, position);

                if (pf.Id == -1)
                    postItRepository.Insert(postIt);
                else
                    postItRepository.Update(postIt);

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
                postItRepository.Delete(postIt);
            }
            PostItFields.ForEach(x => x.textField.IsEnabled = true);
            SavePostIt();
        }



        private void ScreenPostIt_Loaded(object sender, RoutedEventArgs e)
        {
            PostItFields = new List<PostItField>();
            List<PostIt> postIts = postItRepository.GetAll();
            foreach (PostIt post in postIts)
            {
                NewPostItField(post);
            }
        }

        private void NewPostItField(PostIt post, string key = "")
        {
            PostItField pf = new PostItField(post.Content);

            pf.SelectClick += Pf_SelectClick;
            pf.ColorClick += Pf_ColorClick;
            pf.FontColorClick += Pf_FontColorClick;
            pf.FixedClick += Pf_FixedClick;
            pf.LostFocus += Pf_LostFocus;
            pf.TextChanged += Pf_TextChanged;
            pf.DownClick += Pf_DownClick;
            pf.UpClick += Pf_UpClick;

            SolidColorBrush defaultColor = (SolidColorBrush)new BrushConverter().ConvertFrom(FindResource("PostItBackground").ToString());
            SolidColorBrush textColor = (SolidColorBrush)new BrushConverter().ConvertFrom(FindResource("Text").ToString());

            pf.Id = post.Id;
            pf.BackgroundColor = post.Color == null ? defaultColor : post.Color;
            pf.TextColor = post.FontColor == null ? textColor : post.FontColor;
            pf.IsFixed = post.Position == -1;

            int pos = PostItFields.Count > 0 && PostItFields[0].IsFixed ? 1 : 0;

            group.Children.Insert(pos, pf);
            PostItFields.Insert(pos, pf);

            if (key != "" && key.Length == 1)
            {
                pf.Text += key;
                pf.textField.SelectionStart = 1;
            }

            if (post.Id == -1)
                pf.FocusTextField();
        }



        private void Pf_FixedClick(object sender, RoutedEventArgs e)
        {
            PostItField pf = (PostItField)sender;
            if (pf.IsFixed && PostItFields.IndexOf(pf) != 0)
            {
                PostItFields[0].IsFixed = false;

                PostItFields.Remove(pf);
                PostItFields.Insert(0, pf);

                group.Children.Remove(pf);
                group.Children.Insert(0, pf);
            }

            SavePostIt();
        }

        private void Pf_ColorClick(object sender, RoutedEventArgs e)
        {
            ScreenColorPalette colorPalette = new ScreenColorPalette((PostItField)sender);
            colorPalette.ShowDialog();
        }

        private void Pf_FontColorClick(object sender, RoutedEventArgs e)
        {
            ScreenColorPalette colorPalette = new ScreenColorPalette((PostItField)sender, true);
            colorPalette.ShowDialog();
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

            if (index == PostItFields.Count - 1 || PostItFields[index].IsFixed)
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
            int minPosition = PostItFields.Count > 0 && PostItFields[0].IsFixed ? 1 : 0;

            if (index == minPosition || index == 0)
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
