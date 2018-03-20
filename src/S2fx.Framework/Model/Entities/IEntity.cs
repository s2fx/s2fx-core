using System;
using System.Collections.Generic;
using System.Text;

namespace S2fx.Model.Entities {

    public interface IEntity {

        long Id { get; set; }
        bool IsPersistent { get; }
    }

    public interface IAuditedEntity : IEntity {

        UserEntity CreatedBy { get; set; }
        UserEntity UpdatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
    }

    public interface IArchivableEntity<TEntity>
        where TEntity : class, IEntity {

        bool Archived { get; set; }
    }

    public interface IHierarchyEntity<TEntity>
        where TEntity : class, IEntity {

        TEntity Parent { get; set; }
        long RangeLeft { get; set; }
        long RangeRight { get; set; }
    }

    public interface IEntityWithVersion<TEntity>
        where TEntity : class, IEntity {

        long RecordVersion { get; set; }
    }

}
