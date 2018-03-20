using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace S2fx.Data.UnitOfWork {

    public abstract class AbstractUnitOfWork : IUnitOfWork {

        public IUnitOfWork Parent { get; set; }

        public Guid Id { get; }

        public TimeSpan Timeout { get; private set; }

        public bool IsTransactional { get; private set; }

        public abstract IDbConnection DbConnection { get; }

        public abstract IDbTransaction DbTransaction { get; }

        public UnitOfWorkStatus State { get; protected set; } = UnitOfWorkStatus.NotStarted;

        public AbstractUnitOfWork() {
            this.Id = Guid.NewGuid();
        }

        public virtual void Begin(UnitOfWorkOptions options) {
            if (this.State != UnitOfWorkStatus.NotStarted) {
                throw new InvalidOperationException("This UoW already started!");
            }

            this.Timeout = options.Timeout;
            this.IsTransactional = options.IsTransactional;
            this.BeginUow();
            this.State = UnitOfWorkStatus.Started;
        }

        public virtual void Begin() => this.Begin(UnitOfWorkOptions.DefaultOptions);

        public abstract Task CompleteAsync();

        public abstract Task SaveChangesAsync();

        public abstract void Dispose();

        protected abstract void BeginUow();
    }
}
