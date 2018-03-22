using System;
using System.Collections.Generic;
using System.Text;
using S2fx.Model.Metadata.Types;

namespace S2fx.Model.Annotations {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IdPropertyAttribute : AbstractPropertyAttribute {
        public override string PropertyTypeName => BuiltinPropertyTypeNames.IdTypeName;
    }
}
