using System.Diagnostics;
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

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/TheuFerreira/Notas");
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)
        {
            ClickSwitchMode?.Invoke(sender, e);
        }
    }
}
