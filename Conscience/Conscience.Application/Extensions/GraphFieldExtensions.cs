using GraphQL;
using GraphQL.Builders;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public static class GraphFieldExtensions
    {
        public static readonly string PermissionsKey = "Permissions";

        public static bool HasSpecificPermissions(this IProvideMetadata type)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any();
        }
        
        public static bool HasPermission(this IProvideMetadata type, BeezyPermissions permission)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any(x => string.Equals(x, permission.ToString()));
        }

        public static IProvideMetadata AddPermissions(this IProvideMetadata type, params BeezyPermissions[] permissions)
        {
            foreach(var permission in permissions)
            {
                AddPermission(type, permission);
            }

            return type;
        }

        public static IProvideMetadata AddPermission(this IProvideMetadata type, BeezyPermissions permission)
        {
            var permissions = type.GetMetadata<List<string>>(PermissionsKey);

            if (permissions == null)
            {
                permissions = new List<string>();
                type.Metadata[PermissionsKey] = permissions;
            }

            permissions.Fill(permission.ToString());

            return type;
        }

        public static FieldBuilder<TSourceType, TReturnType> AddPermissions<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, params BeezyPermissions[] permissions)
        {
            builder.FieldType.AddPermissions(permissions);
            return builder;
        }

        public static FieldBuilder<TSourceType, TReturnType> AddPermission<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, BeezyPermissions permission)
        {
            builder.FieldType.AddPermission(permission);
            return builder;
        }
    }

    public enum BeezyPermissions
    {
        AllowAnonymous,
        BeezyAdmin,
        CommunityMember,
        CommunityOwner
    }
}
