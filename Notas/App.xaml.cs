using Notas.Database.Interfaces;
using Notas.Database.Repositories;
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
        private MainWindow main;
        private NotifyIcon icon;
        private bool exiting = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            IDbRepository dbRepository = new DbRepository();
            new UpdateDatabase(dbRepository).Execute();

            main = new MainWindow();
            main.Closing += Main_Closing;

            MenuItem itemClose = new MenuItem();
            itemClose.Click += ItemClose_Click;
            itemClose.Index = 0;
            itemClose.Text = "E&xit";

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.AddRange(new MenuItem[] { itemClose });

            icon = new NotifyIcon();
            Stream iconStream = GetResourceStream(new Uri("Resources/ic_notes.ico", UriKind.Relative)).Stream;
            icon.Icon = new Icon(iconStream);
            icon.MouseClick += Icon_MouseClick;

            icon.ContextMenu = contextMenu;
            icon.Text = "Notas";
            icon.Visible = true;

            main.Show();

            base.OnStartup(e);
        }

        private void Icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (main.Visibility == Visibility.Visible)
                main.Activate();
            else
                main.Show();
        }

        private void ItemClose_Click(object sender, EventArgs e)
        {
            exiting = true;
            Current.MainWindow.Close();
        }

        private void Main_Closing(object sender, CancelEventArgs e)
        {
            if (exiting)
                return;

            e.Cancel = true;
            main.Hide();
        }
    }
}
