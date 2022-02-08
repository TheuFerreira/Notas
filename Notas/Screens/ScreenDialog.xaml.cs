using System.Windows;

namespace Notas.Screens
{
    /// <summary>
    /// Interaction logic for ScreenDialog.xaml
    /// </summary>
    public partial class ScreenDialog : Window
    {
        private bool result = false;

        public ScreenDialog()
        {
            InitializeComponent();
        }

        public static bool ShowDialog(string title, string message)
        {
            ScreenDialog screenDialog = new ScreenDialog();
            screenDialog.txtTitle.Text = title;
            screenDialog.txtMessage.Text = message;
            screenDialog.ShowDialog();

            return screenDialog.result;
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            Close();
        }
    }
}
