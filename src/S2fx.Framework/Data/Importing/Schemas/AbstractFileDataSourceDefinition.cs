using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace S2fx.Data.Importing.Schemas {
    public abstract class AbstractFileDataSourceDefinition : AbstractDataSourceDefinition, IFileDataSourceDescriptor {

        [Required]
        public string Path { get; set; }

    }
}
