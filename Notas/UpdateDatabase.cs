using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Notas.Database.Interfaces;
using Notas.Database.Migrations;
using System;

namespace Notas
{
    public class UpdateDatabase
    {
        private readonly IDbRepository _dbRepository;

        public UpdateDatabase(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public void Execute()
        {
            IServiceProvider serviceProvider = CreateServices();

            using (IServiceScope scope = serviceProvider.CreateScope())
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
                    .WithGlobalConnectionString(_dbRepository.ConnectionString)
                    .ScanIn(typeof(AddColumnColor).Assembly)
                    .ScanIn(typeof(AddColumnPosition).Assembly)
                    .ScanIn(typeof(DropColumnColor).Assembly)
                    .ScanIn(typeof(DropTableTempPostIt).Assembly)
                    .ScanIn(typeof(AddColumnFontColor).Assembly)
                    .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void Update(IServiceProvider serviceProvider)
        {
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
