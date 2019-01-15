using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NHibernate;
using NHibernate.Cfg;
using OrchardCore.Modules;
using S2fx.Data.NHibernate.Mapping;
using S2fx.Data.NHibernate.Mapping.Properties;
using S2fx.Data.NHibernate.UnitOfWork;
using S2fx.Data.UnitOfWork;
using S2fx.Utility;

namespace S2fx.Data.NHibernate {

    public class Startup : StartupBase {

        public override void ConfigureServices(IServiceCollection services) {
            //Unit of work
            //NH stuffs

            //Unit of work
            //services.AddTransient<ISessionFactory, NH.Cfg.Configuration>();
            services.AddScoped<IUnitOfWork, HibernateUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(DefaultHibernateRepository<>));

            //entity mapping
            services.AddTransient(typeof(EntityMappingClass<>), typeof(EntityMappingClass<>));
            services.AddTransient<IModelMapper, ModelMapper>();
            AddBuiltinPropertyMappers(services);

            //register nhibernate ISessionFactory 
            services.AddTransient<IHibernateConfigurationFactory, HibernateConfigurationFactory>();

            //register NH's Configuration to container
            services.AddSingleton<Configuration>(sp => sp.GetRequiredService<IHibernateConfigurationFactory>().Create());

            //register NH's ISessionFactory 
            services.AddSingleton<ISessionFactory>(sp => sp.GetRequiredService<Configuration>().BuildSessionFactory());

            services.AddScoped<ISession>(sp => sp.GetRequiredService<ISessionFactory>().OpenSession());

            //migrator
            services.AddTransient<IDatabaseMigrator, HibernateDatabaseMigrator>();
        }

        private void AddBuiltinPropertyMappers(IServiceCollection services) {
            var assembly = Assembly.GetExecutingAssembly();
            var mapperTypes = assembly.ExportedTypes.Where(t => t.IsPublic && t.IsClass && !t.IsAbstract && t.IsImplementsInterface<IPropertyMapper>());
            foreach (var mt in mapperTypes) {
                services.AddTransient(typeof(IPropertyMapper), mt);
            }
        }
    }
}