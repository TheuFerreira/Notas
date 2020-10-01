using System.Windows.Media;

namespace Notas.Database.Table
{
    public class PostIt
    {
        public long Id { get; set; }
        public string Content { get; set; }

        public SolidColorBrush Color { get; set; }


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

        public PostIt(long id, string content, SolidColorBrush color)
        {
            Id = id;
            Content = content;
            Color = color;
        }

        public PostIt(string content, SolidColorBrush color)
        {
            Content = content;
            Color = color;
        }
    }
}
