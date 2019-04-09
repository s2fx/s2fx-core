using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OrchardCore.Environment.Extensions.Features;
using S2fx.Model.Metadata.Types;

namespace S2fx.Model.Metadata {

    [DataContract]
    public class MetaEntity : AnyMetadata {

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        public Type ClrType { get; set; }

        public IEntityType EntityType { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public IEnumerable<string> Dependencies => DependentEntities;

        public ISet<string> DependentEntities { get; } = new HashSet<string>();

        public IFeatureInfo Feature { get; set; }

        [DataMember(Name = "Feature")]
        public string FeatureId => this.Feature.Id;

        [DataMember]
        public IDictionary<string, MetaField> Fields { get; } = new Dictionary<string, MetaField>();

        public string DbName { get; set; }

        public IList<AbstractMetaEntityAnnotation> Annotations { get; set; } = new List<AbstractMetaEntityAnnotation>();

        public AbstractMetaEntityAnnotation FindAnnotation(string name) => this.Annotations.FirstOrDefault(x => x.Name == name);

        public override void AcceptVisitor(IMetadataVisitor visitor) {
            visitor.VisitEntity(this);
            foreach (var field in this.Fields.Values) {
                visitor.VisitField(field);
            }
        }

        public override string ToString() => $"[{this.Name}]{this.DisplayName}";
    }

}
