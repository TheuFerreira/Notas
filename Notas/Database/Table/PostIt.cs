using System.Windows.Media;

namespace Notas.Database.Table
{
    public class PostIt
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public SolidColorBrush Color { get; set; }
        public int Position { get; set; }
        public SolidColorBrush FontColor { get; set; }


        public PostIt() 
        {
            Id = -1;
            Content = string.Empty;
        }

        public PostIt(long id)
        {
            Id = id;
        }

        public PostIt(long id, string content, int position)
        {
            Id = id;
            Content = content;
            Position = position;
        }

        public PostIt(long id, string content, SolidColorBrush color, int position, SolidColorBrush fontColor)
        {
            Id = id;
            Content = content;
            Color = color;
            Position = position;
            FontColor = fontColor;
        }
    }
}
