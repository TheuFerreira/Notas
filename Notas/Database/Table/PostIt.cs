using System.Windows.Media;

namespace Notas.Database.Table
{
    public class PostIt
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public SolidColorBrush Color { get; set; }
        public int Position { get; set; }


        public PostIt() { }

        public PostIt(long id)
        {
            Id = id;
        }

        public PostIt(long id, SolidColorBrush color)
        {
            Id = id;
            Color = color;
        }

        public PostIt(long id, int position)
        {
            Id = id;
            Position = position;
        }

        public PostIt(long id, string content, SolidColorBrush color, int position)
        {
            Id = id;
            Content = content;
            Color = color;
            Position = position;
        }

        public PostIt(string content, SolidColorBrush color, int position)
        {
            Content = content;
            Color = color;
            Position = position;
        }
    }
}
