using Microsoft.Win32;
using Notas.Entities;
using Notas.Interfaces;
using System.Windows.Media;

namespace Notas.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        public Settings Load()
        {
            Settings settings = new Settings();

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ferreira\Notas");
            if (key != null)
            {
                settings.IsLight = bool.Parse(key.GetValue("Mode").ToString());

                object keyFont = key.GetValue("DefaultFont");
                settings.DefaultFont = new FontFamily(keyFont != null ? keyFont.ToString() : "Segoe UI");

                object keyAutoAdd = key.GetValue("autoAdd");
                settings.AutoAdd = keyAutoAdd != null && bool.Parse(keyAutoAdd.ToString());

                key.Close();
            }
            else
            {
                settings.IsLight = true;
                settings.DefaultFont = new FontFamily("Segoe UI");
                settings.AutoAdd = false;
            }

            return settings;
        }

        public void Save(Settings settings)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ferreira\Notas");
            key.SetValue("Mode", settings.IsLight);
            key.SetValue("DefaultFont", settings.DefaultFont.ToString());
            key.SetValue("AutoMode", settings.AutoAdd.ToString());
            key.Close();
        }
    }
}
