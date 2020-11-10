using Notas.Database.Persistence;
using Notas.UserControls;
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

        public ScreenColorPalette(PostItField postItField)
        {
            InitializeComponent();

            PostItField = postItField;
            oldBackgroundColor = PostItField.BackgroundColor;

            Color color = ColorTranslator.FromHtml(oldBackgroundColor.ToString());
            slRed.Value = color.R;
            slGreen.Value = color.G;
            slBlue.Value = color.B;

            Slider_ValueChanged(null, null);

            topBar.MouseLeftButtonDown += TopBar_MouseDown;
            btnClose.Click += BtnClose_Click;

            slRed.ValueChanged += Slider_ValueChanged;
            slGreen.ValueChanged += Slider_ValueChanged;
            slBlue.ValueChanged += Slider_ValueChanged;

            btnConfirm.Click += BtnConfirm_Click;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            PostItField.BackgroundColor = oldBackgroundColor;
            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int red = (int)slRed.Value;
            int green = (int)slGreen.Value;
            int blue = (int)slBlue.Value;

            Color color = Color.FromArgb(red, green, blue);
            HexColor = ColorTranslator.ToHtml(color);

            PostItField.BackgroundColor = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFrom(HexColor);
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            PersistencePostIt.UpdateColor(PostItField.Id, HexColor);

            Close();
        }
    }
}
