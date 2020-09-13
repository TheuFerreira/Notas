namespace Notas.Database.Table
{
    public class PostIt
    {
        public long Id { get; set; }
        public string Content { get; set; }

        public PostIt() { }

        public PostIt(long id)
        {
            Id = id;
        }

        public PostIt(long id, string content)
        {
            Id = id;
            Content = content;
        }

        public PostIt(string content)
        {
            Content = content;
        }
    }
}
