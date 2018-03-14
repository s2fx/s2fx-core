using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using S2fx.Data.UnitOfWork;

namespace S2fx.Data.EFCore.UnitOfWork {

    public class EFCoreUnitOfWork : AbstractUnitOfWork {

        private readonly DbContext _dbContext;
        private WrappedEFCoreDbTransaction _wrappedTransaction;

        public DbContext DbContext { get; }

        public override IDbTransaction Transaction => _wrappedTransaction;

        public EFCoreUnitOfWork(DbContext dbContext) {
            _dbContext = dbContext;
        }

        protected override async Task BeginUowAsync() {
            if (this.IsTransactional) {
                try {
                    var dbtx = await _dbContext.Database.BeginTransactionAsync();
                    _wrappedTransaction = new WrappedEFCoreDbTransaction(dbtx);
                }
                catch {
                    _wrappedTransaction = null;
                }
            }
        }

        public override async Task CompleteAsync() {
            if (this.State != UnitOfWorkStatus.Started) {
                throw new InvalidOperationException();
            }

            try {
                await this.SaveChangesAsync();
                if (this.Transaction != null) {
                    this.Transaction.Commit();
                }
            }
            catch {
                this.State = UnitOfWorkStatus.Failed;
            }

            this.State = UnitOfWorkStatus.Succeed;
        }

        public override async Task SaveChangesAsync() {
            if (this.State != UnitOfWorkStatus.Started) {
                throw new InvalidOperationException();
            }

            await this.DbContext.SaveChangesAsync();
        }

        public override void Dispose() {
            if (this.State == UnitOfWorkStatus.Started) {
                //DO disposing
            }
        }

    }
}
