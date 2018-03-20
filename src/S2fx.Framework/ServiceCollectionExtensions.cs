using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using S2fx.Data;
using S2fx.Data.Convention;
using S2fx.Data.UnitOfWork;
using S2fx.Environment.Extensions;
using S2fx.Environment.Extensions.Entity;
using S2fx.Model;
using S2fx.Model.Metadata.Loaders;
using S2fx.Utility;
using S2fx.Model.Metadata;
using S2fx.Model.Metadata.Types;

namespace Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionExtensions {
        public static void AddSlipStreamFramework(this IServiceCollection services) {
            //environment
            {
                services.AddTransient<IEntityHarvester, EntityHarvester>();
                services.AddTransient<IEntityMetadataProvider, ModuleEntityMetadataProvider>();
                services.AddTransient<IEntityMetadataProvider, BuiltInEntityMetadataProvider>();
            }

            //Data accessing
            {
                services.AddTransient<IDynamicEntityRepositoryResolver, DynamicEntityRepositoryResolver>();
                services.AddTransient<IUnitOfWorkManager, DefaultUnitOfWorkManager>();
                services.AddTransient<IDbNameConvention, S2DbNameConvention>();
            }

            //model
            {
                services.AddSingleton<IEntityManager, EntityManager>();
                services.AddTransient<IClrTypeEntityMetadataLoader, ClrTypeEntityMetadataLoader>();

                //meta data
                services.RegisterAllPropertyTypes();
            }
        }

        private static void RegisterAllPropertyTypes(this IServiceCollection services) {

            var assembly = Assembly.GetExecutingAssembly();
            var propertyTypes = assembly.ExportedTypes
                .Where(t => t.IsPublic && t.IsClass && !t.IsAbstract && t.IsImplementsInterface<IPropertyType>());
            foreach (var pt in propertyTypes) {
                services.AddSingleton(typeof(IPropertyType), pt);
            }
        }


    }
}
