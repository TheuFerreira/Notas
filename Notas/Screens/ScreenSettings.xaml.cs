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



        public ScreenSettings(bool isLight, FontFamily defaultFont)
        {
            InitializeComponent();

            cbMode.IsChecked = !isLight;
            cbMode.Click += CbMode_Click;

            this.defaultFont = defaultFont;
            cbFonts.Loaded += CbFonts_Loaded;
        }



        private void CbMode_Click(object sender, RoutedEventArgs e)
        {
            SwitchMode?.Invoke(!cbMode.IsChecked.Value, e);
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
            SwitchFont?.Invoke(defaultFont.ToString(), e);
        }



        public event RoutedEventHandler SwitchMode;
        public event RoutedEventHandler SwitchFont;
    }
}
