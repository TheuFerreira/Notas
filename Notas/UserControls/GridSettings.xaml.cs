using System.Windows;
using System.Windows.Controls;

namespace Notas.UserControls
{
    /// <summary>
    /// Interação lógica para GridSettings.xam
    /// </summary>
    public partial class GridSettings : UserControl
    {
        public event RoutedEventHandler ClickSwitchMode;

        public GridSettings(bool mode)
        {
            InitializeComponent();

            btnMode.IsSecondText = mode;
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)
        {
            ClickSwitchMode?.Invoke(sender, e);
        }
    }
}
