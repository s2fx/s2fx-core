using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using S2fx.Model;
using S2fx.Model.Annotations;
using S2fx.View.Model.Model;

namespace S2fx.Security.Model {

    [Entity(EntityName), DisplayName("Role")]
    public class RoleEntity : AbstractAuditedEntity {
        public const string EntityName = "Core.Role";

        [Required, MaxLength(256)]
        public virtual string Name { get; set; }

        [MaxLength(256)]
        public virtual string DisplayName { get; set; }

        [ManyToManyProperty(mappedBy: "Roles", joinTable: "core_user_role")]
        public virtual ICollection<UserEntity> Users { get; set; }

        [ManyToManyProperty(mappedBy: "Roles", joinTable: "core_role_permission")]
        public virtual ICollection<PermissionEntity> Permissions { get; set; }

        [ManyToManyProperty(mappedBy: "Roles", joinTable: "core_role_menu")]
        public virtual ICollection<MenuEntryEntity> Menus { get; set; }
    }

}
