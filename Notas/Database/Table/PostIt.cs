using System.Windows.Media;

namespace Notas.Database.Table
{
    public class PostIt
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public SolidColorBrush Color { get; set; }
        public int Position { get; set; }


        public PostIt() 
        {
            Id = -1;
            Content = string.Empty;
        }

        public PostIt(long id)
        {
            Id = id;
        }

        public PostIt(long id, string content, SolidColorBrush color, int position)
        {
            Id = id;
            Content = content;
            Color = color;
            Position = position;
        }
    }
}
