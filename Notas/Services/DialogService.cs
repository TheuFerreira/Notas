using System.Windows;

namespace Notas.Services
{
    public static class DialogService
    {
        public static bool ShowQuestion(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "AVISO", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result != MessageBoxResult.No;
        }
    }
}
