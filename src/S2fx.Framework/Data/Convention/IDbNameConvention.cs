using System;
using System.Collections.Generic;
using System.Text;

namespace S2fx.Data.Convention {

    public interface IDbNameConvention {
        string EntityToTable(string entityFullName);
        string EntityPropertyToColumn(string propertyName);
        string MakeDbObjectFullName(string moduleName, string viewName);
    }
}
