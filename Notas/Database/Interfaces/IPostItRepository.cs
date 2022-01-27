using Notas.Database.Models;
using System.Collections.Generic;

namespace Notas.Database.Interfaces
{
    public interface IPostItRepository
    {
        void Insert(PostIt postIt);

        bool Delete(PostIt postIt);

        void Update(PostIt postIt);

        void UpdateColor(long id, string color);

        void UpdateFontColor(long id, string color);

        List<PostIt> GetAll();
    }
}
