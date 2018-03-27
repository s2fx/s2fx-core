using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using S2fx.Convention;
using S2fx.Data.Importing;
using S2fx.Data.Seeding;
using S2fx.Data.UnitOfWork;

namespace S2fx.Data {

    public static class ServiceCollectionExtensions {

        public static void AddS2fxData(this IServiceCollection services) {
            services.AddTransient<IDynamicEntityRepositoryResolver, DynamicEntityRepositoryResolver>();
            services.AddTransient<IUnitOfWorkManager, DefaultUnitOfWorkManager>();
            services.AddTransient<IDbNameConvention, S2DbNameConvention>();

            //seeding
            services.AddTransient<ISeedDataHarvester, SeedDataHarvester>();
            services.AddTransient<ISeedDataLoader, SeedDataLoader>();

            services.AddTransient<IDataImporter, DataImporter>();

            services.AddTransient<IDataSource, XmlDataSource>();
        }

    }

}
