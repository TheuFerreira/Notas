﻿using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Notas.Database.Migrations;
using Notas.Database.Persistence;
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
            PersistencePostIt.TestConnection();
            UpdateDatabase();

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
            if (e.Button == MouseButtons.Left)
            {
                if (main.Visibility == Visibility.Visible)
                {
                    main.Activate();
                }
                else
                    main.Show();
            }
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
    

        private void UpdateDatabase()
        {
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                Update(scope.ServiceProvider);
            }
        }

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(PersistencePostIt.ConnectionString)
                    .ScanIn(typeof(AddColumnColor).Assembly)
                    .ScanIn(typeof(AddColumnPosition).Assembly)
                    .ScanIn(typeof(DropColumnColor).Assembly)
                    .ScanIn(typeof(DropTableTempPostIt).Assembly)
                    .ScanIn(typeof(AddColumnFontColor).Assembly)
                    .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void Update(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
