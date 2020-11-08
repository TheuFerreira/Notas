using Microsoft.Win32;
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
        private readonly bool mode;
        private FontFamily defaultFont;

        public event RoutedEventHandler SwitchMode;
        public event RoutedEventHandler SwitchFont;

        public ScreenSettings(bool mode)
        {
            InitializeComponent();

            this.mode = mode;

            cbMode.Loaded += CbMode_Loaded;
            cbMode.Click += CbMode_Click;

            cbFonts.Loaded += CbFonts_Loaded;
            cbFonts.SelectionChanged += CbFonts_SelectionChanged;
        }

        private void CbMode_Loaded(object sender, RoutedEventArgs e)
        {
            cbMode.IsChecked = !mode;
        }

        private void CbMode_Click(object sender, RoutedEventArgs e)
        {
            bool value = !cbMode.IsChecked.Value;
            SwitchMode?.Invoke(value, e);
        }



        private void CbFonts_Loaded(object sender, RoutedEventArgs e)
        {
            cbFonts.Items.Clear();
            InstalledFontCollection fontCollection = new InstalledFontCollection();
            foreach (System.Drawing.FontFamily font in fontCollection.Families)
                cbFonts.Items.Add(font.Name);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ferreira\Notas");
            if (key != null)
            {
                object keyFont = key.GetValue("DefaultFont");
                defaultFont = new FontFamily(keyFont != null ? keyFont.ToString() : "Segoe UI");
                key.Close();
            }
            else
            {
                defaultFont = new FontFamily("Segoe UI");
            }

            cbFonts.SelectedIndex = cbFonts.Items.Cast<string>().ToList().IndexOf(defaultFont.ToString());
        }

        private void CbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*defaultFont = new FontFamily(cbFonts.SelectedItem.ToString());
            Resources["DefaultFont"] = new FontFamily(defaultFont.ToString());

            SwitchFont?.Invoke(defaultFont.ToString(), e);*/
        }
    }
}
