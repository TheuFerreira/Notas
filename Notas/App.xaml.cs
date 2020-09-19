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
            icon.Icon = main.Count > 0 ? GenerateIcon(iconStream) : new Icon(iconStream);
            icon.Click += (sender, ev) => main.Show();

            icon.ContextMenu = contextMenu;
            icon.Text = "Notas";
            icon.Visible = true;

            main.Show();

            base.OnStartup(e);
        }

        private Icon GenerateIcon(Stream iconStream)
        {
            Bitmap bitmap = new Bitmap(64, 64);

            Icon icon = new Icon(iconStream);
            Font drawFont = new Font(FontFamily.GenericSansSerif, 22, System.Drawing.FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            Graphics graphics = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Red);

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.DrawIcon(icon, 0, 0);
            graphics.DrawEllipse(pen, 39, 39, 25, 25);
            graphics.FillEllipse(Brushes.Red, 39, 39, 25, 25);

            Icon createdIcon = Icon.FromHandle(bitmap.GetHicon());

            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();

            return createdIcon;
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

                Stream iconStream = GetResourceStream(new Uri("Resources/ic_notes.ico", UriKind.Relative)).Stream;
                icon.Icon = main.Count > 0 ? GenerateIcon(iconStream) : new Icon(iconStream);
            }
        }
    }
}
