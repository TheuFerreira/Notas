using Notas.Screens;

namespace Notas.Services
{
    public static class DialogService
    {
        public static bool ShowWarning(string message)
        {
            bool result = ScreenDialog.ShowDialog("Aviso", message);
            return result;
        }
    }
}
