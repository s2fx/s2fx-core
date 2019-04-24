using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using S2fx.Model.Metadata;
using S2fx.Model.Metadata.Types;
using S2fx.Model.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using S2fx.Model.Environment;
using Microsoft.AspNetCore.Http;

namespace S2fx.Model {

    public class EntityManager : IEntityManager {

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IList<EntityEntry> _entityInfos = new List<EntityEntry>();
        readonly object InitializationLock = new object();
        IReadOnlyDictionary<string, MetaEntity> _entities = new Dictionary<string, MetaEntity>();
        bool _isLoaded = false;

        public ILogger Logger { get; }

        public EntityManager(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EntityManager> logger) {
            _httpContextAccessor = httpContextAccessor;
            this.Logger = logger;
        }


        public IReadOnlyDictionary<string, MetaEntity> GetEntities() {
            this.EnsureLoaded();
            return _entities;
        }

        public MetaEntity GetEntity(string fullName) {
            if (string.IsNullOrEmpty(fullName)) {
                throw new ArgumentNullException(nameof(fullName));
            }
            this.EnsureLoaded();
            return _entities[fullName];
        }

        public MetaEntity GetEntityByClrType(Type entityType) {
            this.EnsureLoaded();
            return this.GetEntity(_entities.Single(pair => pair.Value.ClrType == entityType).Key);
        }

        public Task<IEnumerable<EntityEntry>> LoadEntitiesAsync() {
            this.EnsureLoaded();
            return Task.FromResult(_entityInfos.AsEnumerable());
        }

        void EnsureLoaded() {
            if (_isLoaded) {
                return;
            }

            if (this.Logger.IsEnabled(LogLevel.Debug)) {
                Logger.LogDebug("Initializing EntityManager ...");
            }

            lock (this.InitializationLock) {
                var services = _httpContextAccessor.HttpContext.RequestServices;
                var harvesters = services.GetServices<IEntityHarvester>().OrderBy(x => x.Priority);
                var entityTypes = services.GetServices<IEntityType>();

                var entries = new List<EntityEntry>();
                foreach (var harvester in harvesters) {
                    var harvested = Task.Run(harvester.HarvestEntitiesAsync).Result;
                    entries.AddRange(harvested);
                }

                var context = new MetadataModelProviderContext(entries);

                var providers = services.GetServices<IMetadataModelProvider>().OrderBy(x => x.Order);
                foreach (var p in providers) {
                    p.OnProvidersExecuting(context);
                }

                //处理约定
                var conventionVisitor = services.GetRequiredService<ConventionMetadataVisitor>();
                context.Result.AcceptVisitor(conventionVisitor);

                foreach (var p in providers) {
                    p.OnProvidersExecuted(context);
                }

                _entities = context.Result.Entities.ToDictionary(x => x.Name);

                _isLoaded = true;
            } // lock
        }

    }

}
