using Microsoft.Win32;
using Notas.Database.Interfaces;
using Notas.Database.Repositories;
using Notas.Interfaces;
using Notas.Repositories;
using Notas.UserControls;
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
        public string HexColor { get; set; }

        public PostItField PostItField { get; set; }



        private readonly System.Windows.Media.SolidColorBrush oldBackgroundColor;
        private readonly System.Windows.Media.SolidColorBrush oldTextColor;
        private bool isLight;
        private bool isTextHex = false;
        private readonly bool isFont;
        private System.Windows.Media.FontFamily defaultFont;
        private readonly IPostItRepository postItRepository;



        public ScreenColorPalette(PostItField postItField, bool isFont = false)
        {
            InitializeComponent();

            LoadPreferences();
            SwitchColor();

            this.isFont = isFont;

            PostItField = postItField;
            oldBackgroundColor = PostItField.BackgroundColor;
            oldTextColor = PostItField.TextColor;

            Color color = isFont ? ColorTranslator.FromHtml(oldTextColor.ToString()) : ColorTranslator.FromHtml(oldBackgroundColor.ToString());
            slRed.Value = color.R;
            slGreen.Value = color.G;
            slBlue.Value = color.B;

            Slider_ValueChanged(null, null);

            topBar.MouseLeftButtonDown += TopBar_MouseDown;
            btnClose.Click += BtnClose_Click;

            slRed.ValueChanged += Slider_ValueChanged;
            slGreen.ValueChanged += Slider_ValueChanged;
            slBlue.ValueChanged += Slider_ValueChanged;

            tbHex.GotFocus += TbHex_GotFocus;
            tbHex.TextChanged += TbHex_TextChanged;
            tbHex.LostFocus += TbHex_LostFocus;

            btnConfirm.Click += BtnConfirm_Click;

            IDbRepository dbRepository = new DbRepository();
            postItRepository = new PostItRepository(dbRepository);
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            PostItField.BackgroundColor = oldBackgroundColor;
            PostItField.TextColor = oldTextColor;
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
                HexColor = ColorTranslator.ToHtml(color);

                tbHex.Text = HexColor;
            }

            if (isFont)
                PostItField.TextColor = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFrom(HexColor);
            else
                PostItField.BackgroundColor = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFrom(HexColor);
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (isFont)
                postItRepository.UpdateFontColor(PostItField.Id, HexColor);
            else
                postItRepository.UpdateColor(PostItField.Id, HexColor);

            Close();
        }



        private void TbHex_GotFocus(object sender, RoutedEventArgs e)
        {
            isTextHex = true;
        }

        private void TbHex_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (isTextHex)
            {
                try
                {
                    Color color = ColorTranslator.FromHtml(tbHex.Text);

                    HexColor = tbHex.Text;

                    slRed.Value = color.R;
                    slGreen.Value = color.G;
                    slBlue.Value = color.B;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void TbHex_LostFocus(object sender, RoutedEventArgs e)
        {
            isTextHex = false;
        }



        private void LoadPreferences()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ferreira\Notas");
            if (key != null)
            {
                isLight = bool.Parse(key.GetValue("Mode").ToString());

                object keyFont = key.GetValue("DefaultFont");
                defaultFont = new System.Windows.Media.FontFamily(keyFont != null ? keyFont.ToString() : "Segoe UI");
                key.Close();
            }
            else
            {
                isLight = true;
                defaultFont = new System.Windows.Media.FontFamily("Segoe UI");
            }
        }

        private void SwitchColor()
        {
            Resources["DefaultFont"] = defaultFont;

            if (isLight)
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
    }
}
