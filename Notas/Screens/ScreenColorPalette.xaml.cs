using Notas.Interfaces;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Notas.Screens
{
    /// <summary>
    /// Lógica interna para ScreenColorPalette.xaml
    /// </summary>
    public partial class ScreenColorPalette : Window
    {
        private string hexColor;
        private bool isTextHex = false;

        public ScreenColorPalette(ISettingsRepository settingsRepository, System.Windows.Media.SolidColorBrush postItColor)
        {
            InitializeComponent();
            SwitchColor(settingsRepository);

            Color color = ColorTranslator.FromHtml(postItColor.ToString());
            slRed.Value = color.R;
            slGreen.Value = color.G;
            slBlue.Value = color.B;

            Slider_ValueChanged(null, null);

            slRed.ValueChanged += Slider_ValueChanged;
            slGreen.ValueChanged += Slider_ValueChanged;
            slBlue.ValueChanged += Slider_ValueChanged;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            OnCancel.Invoke(sender, e);
            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isTextHex)
            {
                int red = (int)slRed.Value;
                int green = (int)slGreen.Value;
                int blue = (int)slBlue.Value;

                Color color = Color.FromArgb(red, green, blue);
                hexColor = ColorTranslator.ToHtml(color);

                tbHex.Text = hexColor;
            }

            OnColorChanged?.Invoke(sender, hexColor);
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            OnConfirm.Invoke(sender, hexColor);
            Close();
        }

        private void TbHex_GotFocus(object sender, RoutedEventArgs e)
        {
            isTextHex = true;
        }

        private void TbHex_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!isTextHex)
                return;

            try
            {
                Color color = ColorTranslator.FromHtml(tbHex.Text);

                hexColor = tbHex.Text;

                slRed.Value = color.R;
                slGreen.Value = color.G;
                slBlue.Value = color.B;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TbHex_LostFocus(object sender, RoutedEventArgs e)
        {
            isTextHex = false;
        }

        private void SwitchColor(ISettingsRepository settingsRepository)
        {
            Entities.Settings settings = settingsRepository.Load();
            Resources["DefaultFont"] = settings.DefaultFont;

            if (settings.IsLight)
            {
                Resources["TopBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFF");
                Resources["Text"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#000");
                Resources["FieldBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#F2F2F2");
                Resources["Selection"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#e3e3e3");
                Resources["PostItBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF");
                Resources["ScrollColor"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#595959");
                Resources["CheckboxBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#ACACAC");
                Resources["CheckboxForeground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#ACACAC");
                Resources["ComboboxForeground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxSelection"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#808080");
                Resources["SliderBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#ACACAC");
            }
            else
            {
                Resources["TopBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#171717");
                Resources["Text"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF");
                Resources["FieldBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#303030");
                Resources["Selection"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#3A3A3A");
                Resources["PostItBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#1A1A1A");
                Resources["ScrollColor"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#494949");
                Resources["CheckboxBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFF");
                Resources["CheckboxForeground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#272727");
                Resources["ComboboxBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF");
                Resources["ComboboxForeground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#000");
                Resources["ComboboxSelection"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#CCCCCC");
                Resources["SliderBackground"] = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#ACACAC");
            }
        }

        public EventHandler<string> OnColorChanged;
        public EventHandler OnCancel;
        public EventHandler<string> OnConfirm;
    }
}
