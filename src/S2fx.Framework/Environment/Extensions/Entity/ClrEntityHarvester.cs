using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Descriptor;
using OrchardCore.Modules;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Environment.Extensions;
using System.Reflection;
using S2fx.Model.Annotations;
using S2fx.Model.Metadata.Types;

namespace S2fx.Environment.Extensions.Entity {

    public class ClrEntityHarvester : AbstractClrEntityHarvester {

        public ClrEntityHarvester(IServiceProvider services)
            : base(services) {
        }

        public override async Task<IEnumerable<EntityInfo>> HarvestEntitiesAsync() {
            var extensions = this.Services.GetRequiredService<IExtensionManager>();
            var allFeatures = await extensions.LoadFeaturesAsync();
            var allEntities = allFeatures.SelectMany(x => this.HarvestClrEntityInFeature(x));
            return allEntities;
        }

    }

}
