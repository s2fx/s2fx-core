using System;
using System.Collections.Generic;
using System.Text;

namespace S2fx.Model.Metadata.Loaders {

    public interface IClrTypeEntityMetadataLoader {
        MetaEntity LoadEntityByClr(Type entityType);
    }
}
