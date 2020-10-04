using System.Windows;
using System.Windows.Controls;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para GridSettings.xam
    /// </summary>
    public partial class GridSettings : UserControl
    {
        public GridSettings()
        {
            InitializeComponent();
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)
        {
            BottomButton btn = (BottomButton)sender;
            btn.IsSecondText = !btn.IsSecondText;
        }
    }
}
