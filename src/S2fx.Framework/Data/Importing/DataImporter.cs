using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using S2fx.Data.Importing.Model;
using S2fx.Model;
using S2fx.Model.Metadata;
using OrchardCore.DeferredTasks;
using S2fx.Data.Transactions;
using OrchardCore.Environment.Shell;

namespace S2fx.Data.Importing {

    public class DataImporter : IDataImporter {

        readonly IDeferredTaskEngine _defferedTaskEngine;
        readonly ITransactionManager _transactionManager;
        readonly IEntityManager _entityManager;
        readonly IEnumerable<IDataSource> _dataSources;
        readonly IShellFeaturesManager _shellFeaturesManager;

        public event EventHandler<EntityRecordImportedEventArgs> EntityRecordImported;

        public DataImporter(
            IDeferredTaskEngine defferedTaskEngine,
            ITransactionManager transactionManager,
            IEntityManager entityManager,
            IShellFeaturesManager shellFeaturesManager,
            IEnumerable<IDataSource> dataSources) {
            _defferedTaskEngine = defferedTaskEngine;
            _transactionManager = transactionManager;
            _entityManager = entityManager;
            _shellFeaturesManager = shellFeaturesManager;
            _dataSources = dataSources;
        }

        public async Task ImportAsync(ImportingTaskDescriptor descriptor) {
            _defferedTaskEngine.AddTask(async defferedTaskContext => {
                using (var tx = _transactionManager.BeginTransaction()) {
                    var importTask = await this.CreateTaskAsync(descriptor);
                    var context = new ImportingTaskContext(defferedTaskContext.ServiceProvider);
                    await this.DoImportAsync(importTask, context);
                    await tx.CommitAsync();
                }
            });
            await Task.CompletedTask;
        }

        public async Task ImportAsync(IEnumerable<ImportingTaskDescriptor> sortedDescriptors) {
            _defferedTaskEngine.AddTask(async defferedTaskContext => {
                using (var tx = _transactionManager.BeginTransaction()) {
                    foreach (var descriptor in sortedDescriptors) {
                        var importTask = await this.CreateTaskAsync(descriptor);
                        var context = new ImportingTaskContext(defferedTaskContext.ServiceProvider);
                        await this.DoImportAsync(importTask, context);
                    }
                    await tx.CommitAsync();
                }
            });
            await Task.CompletedTask;
        }

        async Task DoImportAsync(ImportingTask importingTask, ImportingTaskContext context) {
            var dataSource = _dataSources.Single(x => x.Format == importingTask.Descriptor.DataSource);

            using (var stream = importingTask.Descriptor.ImportFileInfo.CreateReadStream()) {
                var recordFinderType = typeof(GenericRecordFinder<>).MakeGenericType(importingTask.Entity.ClrType);
                var recordFinder = context.ServiceProvider.GetRequiredService(recordFinderType) as IRecordFinder;

                var recordImporterType = typeof(GenericRecordImporter<>).MakeGenericType(importingTask.Entity.ClrType);
                var recordImporter = context.ServiceProvider.GetRequiredService(recordImporterType) as IRecordImporter;

                var reader = dataSource.Open(stream, importingTask.Descriptor.EntityMapping.Selector); //GetAllRows(stream, descriptor.EntityMapping.Selector);
                await reader.Initialize();
                while (await reader.ReadAsync()) {
                    await ImportSingleRecordAsync(importingTask, context, recordFinder, recordImporter, reader);
                }
            }
        }

        async Task ImportSingleRecordAsync(
            ImportingTask importingTask, ImportingTaskContext context, IRecordFinder recordFinder, IRecordImporter recordImporter, IDataSourceReader reader) {

            var propValues = new Dictionary<string, object>(importingTask.EntityBinding.PropertyMappings.Length);
            foreach (var propBind in importingTask.EntityBinding.PropertyMappings) {
                var propertyValueExpression = reader.GetField(propBind.SourceExpression).ToString();
                var metaProperty = importingTask.Entity.Fields[propBind.TargetProperty];
                if (metaProperty.Type.TryParse(metaProperty, propertyValueExpression, out var propertyValue, propBind.Format)) {
                    propValues.Add(metaProperty.Name, propertyValue);
                }
                else {
                    throw new DataImportingException(
                        $"Unable to parse the expression '{propertyValueExpression}' " +
                        $"for property '{metaProperty.Entity.Name}#{metaProperty.Name}'");
                }
            }

            var symbols = propValues
                .ToDictionary(x => x.Key, x => x.Value);

            var existedRecord = await recordFinder.FindExistedRecordOrDefaultAsync(importingTask, symbols);

            var needsImportRecord =
                (existedRecord == null)
                || (existedRecord != null && importingTask.EntityBinding.CanUpdate);

            if (!needsImportRecord) {
                return;
            }

            var record = existedRecord ?? Activator.CreateInstance(importingTask.Entity.ClrType);

            foreach (var propPair in propValues) {
                var metaProperty = importingTask.Entity.Fields[propPair.Key];
                metaProperty.ClrPropertyInfo.SetValue(record, propPair.Value);
            }

            await recordImporter.InsertOrUpdateEntityAsync(importingTask, record, importingTask.EntityBinding.CanUpdate);
            this.EntityRecordImported?.Invoke(this, new EntityRecordImportedEventArgs(importingTask.Entity, record));
        }

        async Task<ImportingTask> CreateTaskAsync(ImportingTaskDescriptor job) {
            var entity = _entityManager.GetEntity(job.Entity);
            var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
            var feature = features.Where(x => x.Id == job.Feature).Single();
            var importingTask = new ImportingTask(job, feature, entity, job.EntityMapping, null);
            //Populates property binders
            var ds = _dataSources.SingleOrDefault(x => x.Format == job.DataSource);
            if (ds == null) {
                var msg = string.Format("Unsupported format of seeding data: '{0}'", job.DataSource);
                throw new NotSupportedException(msg);
            }
            return importingTask;
        }

    }
}
