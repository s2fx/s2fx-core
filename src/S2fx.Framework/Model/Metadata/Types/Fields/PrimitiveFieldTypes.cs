using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace S2fx.Model.Metadata.Types {

    public class BooleanFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.BooleanTypeName;
        public override Type ClrType => typeof(bool);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = bool.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class Int32FieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.Int32TypeName;
        public override Type ClrType => typeof(int);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = int.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class Int64FieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.Int64TypeName;
        public override Type ClrType => typeof(long);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = long.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class FloatFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.FloatTypeName;
        public override Type ClrType => typeof(float);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = float.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class DoubleFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.DoubleTypeName;
        public override Type ClrType => typeof(double);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = double.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class StringFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.StringTypeName;
        public override Type ClrType => typeof(string);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            result = value;
            return true;
        }
    }

    public class ByteArrayFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.ByteArrayTypeName;
        public override bool SelectDefaultValue => false;
        public override bool LazyDefaultValue => true;
        public override Type ClrType => typeof(byte[]);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            throw new NotSupportedException();
        }
    }

    public class DecimalFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.DecimalTypeName;
        public override Type ClrType => typeof(decimal);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            var succeed = decimal.TryParse(value, out var typedValue);
            result = typedValue;
            return succeed;
        }
    }

    public class DateTimeFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.DateTimeTypeName;
        public override Type ClrType => typeof(DateTime);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            //TODO format
            throw new NotImplementedException();
        }
    }

    public class DateFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.DateTypeName;
        public override Type ClrType => typeof(DateTime);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            //TODO format
            throw new NotImplementedException();
        }
    }

    public class TimeFieldType : AbstractPrimitiveFieldType {
        public override string Name => BuiltinFieldTypeNames.TimeTypeName;
        public override Type ClrType => typeof(TimeSpan);

        public override bool TryParse(MetaField property, string value, out object result, string format = null) {
            //TODO format
            /*
            var succeed = LocalTime.(value, out var typedValue);
            result = typedValue;
            return succeed;
            */
            throw new NotImplementedException();
        }
    }

}
