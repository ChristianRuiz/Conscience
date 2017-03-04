using Concience.DataAccess;
using Conscience.Application.Graph;
using Conscience.Domain;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.ValidationRules
{
    public class RolesValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var account = context.UserContext as ConscienceAccount;

            return new EnterLeaveListener(_ =>
            {
                if (account != null)
                {
                    _.Match<Field>(fieldAst =>
                    {
                        var fieldDef = context.TypeInfo.GetFieldDef();
                        
                        //TODO: Comprobar con el usuario
                        if (fieldDef != null)
                        {
                            var permissions = fieldDef.GetAllPermission();
                            var userRoles = account.Roles.Select(r => r.Name).ToList().Select(r => r.ToLowerInvariant());

                            if (permissions.Any(p => !userRoles.Contains(p)))
                            {
                                context.ReportError(new ValidationError(
                                    context.OriginalQuery,
                                    "auth-required",
                                    $"You are not authorized to run this query.",
                                    fieldAst));
                            }
                        }
                    });
                }
            });
        }
    }
}
