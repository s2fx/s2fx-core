using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace S2fx.Model.Metadata.Types {

    public abstract class AbstractFieldType : IFieldType {

        public abstract string Name { get; }

        public abstract MetaField LoadClrProperty(PropertyInfo propertyInfo);

        public abstract bool TryParseFieldValue(
            MetaField property, string value, out object result, string format = null);
    }

}
