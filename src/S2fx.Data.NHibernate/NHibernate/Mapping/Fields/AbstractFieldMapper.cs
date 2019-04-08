using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Impl;
using S2fx.Conventions;
using S2fx.Model.Metadata;

namespace S2fx.Data.NHibernate.Mapping.Fields {

    public abstract class AbstractFieldMapper : IFieldMapper {

        public abstract string FieldTypeName { get; }

        public abstract void MapField(
            ICustomizersHolder customizerHolder,
            IModelExplicitDeclarationsHolder modelExplicitDeclarationsHolder,
            PropertyPath currentFieldPath,
            MetaEntity enttiy,
            MetaField field);
    }

}
