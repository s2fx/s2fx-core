using System;
using System.Collections.Generic;
using System.Text;

namespace S2fx.View.Schemas {

    public interface IViewDefinition {
        string Feature { get; set; }
        string Name { get; }
    }

}
