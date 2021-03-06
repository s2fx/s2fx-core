using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using S2fx.Data;
using S2fx.Data.Importing.Seeds;
using S2fx.Remoting;
using S2fx.Setup.Model;
using S2fx.Setup.Services;
using S2fx.View.Data;

namespace S2fx.Setup.RemoteService {

    [RemoteService(name: "Setup", Area = MvcControllerAreas.SystemArea)]
    public class SetupRemoteService {
        readonly ISetupService _setupService;
        readonly IServiceProvider _services;

        public SetupRemoteService(ISetupService setupService, IServiceProvider serviceProvider) {
            _setupService = setupService;
            _services = serviceProvider;
        }

        [RemoteServiceMethod(httpMethod: HttpMethod.Get, isRestful: true)]
        public IEnumerable<ShellSettings> GetAllTenants() {
            var shellHost = _services.GetRequiredService<IShellHost>();
            return shellHost.GetAllSettings();
        }


        [RemoteServiceMethod(httpMethod: HttpMethod.Post, isRestful: false)]
        public async Task StartSetupAsync(SetupOptions options) {

            var ctx = new SetupContext {
                RootPassword = options.AdminPassword,
                DbName = options.DbName,
                EnabledFeatures = options.EnabledFeatures,
                IsDemo = options.IsDemo,
            };

            await _setupService.SetupAsync(ctx);
        }

        [RemoteServiceMethod(httpMethod: HttpMethod.Get, isRestful: true)]
        public async Task InitDbAsync() {
            var dbMigrator = _services.GetRequiredService<IDbMigrator>();
            await dbMigrator.MigrateSchemaAsync();
        }

        [RemoteServiceMethod(httpMethod: HttpMethod.Get, isRestful: true)]
        public async Task LoadSeedsAsync() {
            var seedsLoader = _services.GetRequiredService<ISeedSynchronizer>();
            await seedsLoader.SynchronizeAllSeedsAsync();
            var viewLoader = _services.GetRequiredService<IViewDataSynchronizer>();
            await viewLoader.SynchronizeAllViewsAsync();
        }


        [RemoteServiceMethod(httpMethod: HttpMethod.Get, isRestful: true)]
        public Dictionary<string, object> Version() {
            return new Dictionary<string, object>() {
                { "Version", "0.1.0" }
            };
        }
    }

}
