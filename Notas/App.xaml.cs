using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Notas
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        MainWindow main;
        NotifyIcon icon;
        bool exiting = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            MenuItem itemClose = new MenuItem();
            itemClose.Click += ItemClose_Click;
            itemClose.Index = 0;
            itemClose.Text = "E&xit";

            MenuItem itemShow = new MenuItem();
            itemShow.Click += (sender, ev) => main.Show();
            itemShow.Index = 1;
            itemShow.Text = "Mostrar Notas";

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.AddRange(new MenuItem[] { itemShow, itemClose });

            icon = new NotifyIcon();
            Stream iconStream = GetResourceStream(new Uri("Resources/ic_notes.ico", UriKind.Relative)).Stream;
            icon.Icon = new Icon(iconStream);

            icon.ContextMenu = contextMenu;
            icon.Text = "Notas";
            icon.Visible = true;

            main = new MainWindow();
            main.Closing += Main_Closing;
            main.Show();

            base.OnStartup(e);
        }

        private void ItemClose_Click(object sender, EventArgs e)
        {
            exiting = true;
            Current.MainWindow.Close();
        }

        private void Main_Closing(object sender, CancelEventArgs e)
        {
            if (!exiting)
            {
                e.Cancel = true;
                main.Hide();
            }
        }
    }
}
