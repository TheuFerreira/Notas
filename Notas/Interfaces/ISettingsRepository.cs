using Notas.Entities;

namespace Notas.Interfaces
{
    public interface ISettingsRepository
    {
        void Save(Settings settings);
        Settings Load();
    }
}
