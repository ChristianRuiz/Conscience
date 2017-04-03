using Conscience.Domain;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;
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
        public static readonly string RequiresMembershipKey = "RequiresMembership";

        public static bool DoesFieldRequiresMembership(this IProvideMetadata type)
        {
            return type.GetMetadata(RequiresMembershipKey, false);
        }

        public static void RequiresMembership(this IProvideMetadata type)
        {
            type.Metadata[RequiresMembershipKey] = true;
        }

        public static bool HasSpecificPermissions(this IProvideMetadata type)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any();
        }
        
        public static bool HasPermission(this IProvideMetadata type, RoleTypes permission)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any(x => string.Equals(x, permission.ToString()));
        }

        public static IEnumerable<string> GetAllPermission(this IProvideMetadata type)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions;
        }

        public static IProvideMetadata AddPermissions(this IProvideMetadata type, params RoleTypes[] permissions)
        {
            foreach(var permission in permissions)
            {
                AddPermission(type, permission);
            }

            return type;
        }

        public static IProvideMetadata AddPermission(this IProvideMetadata type, RoleTypes permission)
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
            this FieldBuilder<TSourceType, TReturnType> builder, params RoleTypes[] permissions)
        {
            builder.FieldType.AddPermissions(permissions);
            return builder;
        }

        public static FieldBuilder<TSourceType, TReturnType> AddPermission<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, RoleTypes permission)
        {
            builder.FieldType.AddPermission(permission);
            return builder;
        }

        public static IProvideMetadata AddAdminPermissions(this IProvideMetadata type)
        {
            return type.AddPermissions(RoleTypes.Admin, RoleTypes.CompanyAdmin);
        }

        public static IProvideMetadata AddQAPermissions(this IProvideMetadata type)
        {
            return type.AddAdminPermissions().AddPermission(RoleTypes.CompanyQA);
        }

        public static IProvideMetadata AddPlotPermissions(this IProvideMetadata type)
        {
            return type.AddQAPermissions().AddPermission(RoleTypes.CompanyPlot);
        }

        public static IProvideMetadata AddBehaviourPermissions(this IProvideMetadata type)
        {
            return type.AddQAPermissions().AddPermission(RoleTypes.CompanyBehaviour);
        }

        public static IProvideMetadata AddBehaviourAndPlotPermissions(this IProvideMetadata type)
        {
            return type.AddBehaviourPermissions().AddPermission(RoleTypes.CompanyPlot);
        }

        public static IProvideMetadata AddMaintenancePermissions(this IProvideMetadata type)
        {
            return type.AddBehaviourAndPlotPermissions()
                .AddPermission(RoleTypes.CompanyMaintenance);
        }

        public static IProvideMetadata AddHostPermissions(this IProvideMetadata type)
        {
            return type.AddPermissions(RoleTypes.Admin, RoleTypes.Host);
        }
    }
}
