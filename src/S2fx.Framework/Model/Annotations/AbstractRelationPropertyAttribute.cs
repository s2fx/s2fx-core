using System;
using System.Collections.Generic;
using System.Text;

namespace S2fx.Model.Annotations {

    public abstract class AbstractRelationPropertyAttribute : AbstractPropertyAttribute {
        public CascadeMethod Cascade { get; set; } = CascadeMethod.All;
        public string RefEntity { get; set; }
        public string MappedBy { get; set; }

        public AbstractRelationPropertyAttribute(string refEntity, string mappedBy) {
            this.RefEntity = refEntity;
            this.MappedBy = mappedBy;
        }
    }
}
