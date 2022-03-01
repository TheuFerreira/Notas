using Notas.Entities;
using Notas.Interfaces;
using Notas.Repositories;
using System.Drawing.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notas.Screens
{
    /// <summary>
    /// Interação lógica para ScreenSettings.xam
    /// </summary>
    public partial class ScreenSettings : UserControl
    {
        private FontFamily defaultFont;
        private readonly ISettingsRepository _settingsRepository;
        private readonly Settings settings;

        public ScreenSettings()
        {
            InitializeComponent();

            _settingsRepository = new SettingsRepository();
            settings = _settingsRepository.Load();

            cbMode.IsChecked = !settings.IsLight;
            cbMode.Click += CbMode_Click;

            defaultFont = settings.DefaultFont;
            cbFonts.Loaded += CbFonts_Loaded;

            cbAutoAdd.IsChecked = !settings.AutoAdd;
            cbAutoAdd.Click += CbAutoAdd_Click;
        }

        private void CbMode_Click(object sender, RoutedEventArgs e)
        {
            bool value = !cbMode.IsChecked.Value;

            settings.IsLight = value;
            _settingsRepository.Save(settings);
            SwitchMode?.Invoke(sender, e);
        }

        private void CbFonts_Loaded(object sender, RoutedEventArgs e)
        {
            cbFonts.Items.Clear();
            InstalledFontCollection fontCollection = new InstalledFontCollection();
            foreach (System.Drawing.FontFamily font in fontCollection.Families)
                cbFonts.Items.Add(font.Name);

            cbFonts.SelectedIndex = cbFonts.Items.Cast<string>().ToList().IndexOf(defaultFont.ToString());
            cbFonts.SelectionChanged += CbFonts_SelectionChanged;
        }

        private void CbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultFont = new FontFamily(cbFonts.SelectedItem.ToString());

            settings.DefaultFont = new FontFamily(defaultFont.ToString());
            Resources["DefaultFont"] = new FontFamily(settings.DefaultFont.ToString());

            _settingsRepository.Save(settings);
        }

        private void CbAutoAdd_Click(object sender, RoutedEventArgs e)
        {
            bool value = !cbAutoAdd.IsChecked.Value;

            settings.AutoAdd = value;
            _settingsRepository.Save(settings);
        }

        public event RoutedEventHandler SwitchMode;
    }
}
