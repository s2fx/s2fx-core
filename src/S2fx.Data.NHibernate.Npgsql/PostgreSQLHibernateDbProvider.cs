using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Driver;
using Npgsql;
using S2fx.Data.NHibernate.Npgsql.Types;
using S2fx.Data.NHibernate.Types;

namespace S2fx.Data.NHibernate.Npgsql {

    public class PostgreSQLHibernateDbProvider : IHibernateDbProvider {
        public const string DbProviderName = "PostgreSQL";

        public int Order => -4000;

        public string Name => DbProviderName;

        public string DisplayName => "PostgreSQL";

        public bool IsLoginNeeded => true;

        public Type JsonObjectType => typeof(TextBasedJsonObjectType<>);

        public Type XmlObjectType => typeof(TextBasedXmlObjectType<>);

        public void SetupConfiguration(Configuration cfg) {
            cfg.SetProperty(global::NHibernate.Cfg.Environment.ConnectionDriver, typeof(S2NpgsqlDriver).AssemblyQualifiedName);
            cfg.SetProperty(global::NHibernate.Cfg.Environment.Dialect, typeof(global::NHibernate.Dialect.PostgreSQL83Dialect).AssemblyQualifiedName);
        }

    }

}
