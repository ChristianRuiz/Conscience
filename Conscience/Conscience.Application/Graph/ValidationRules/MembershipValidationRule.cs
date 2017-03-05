using Conscience.DataAccess;
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
    public class MembershipValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var account = context.UserContext as ConscienceAccount;

            return new EnterLeaveListener(_ =>
            {
                if (account == null)
                {
                    var fieldsCount = _.GetFieldsCount<Field>();

                    var hasAnyAnynomousPermissions = false;

                    _.Match<Field>(fieldAst =>
                    {
                        fieldsCount.Count--;

                        var fieldDef = context.TypeInfo.GetFieldDef();

                        if (fieldDef != null && 
                            (fieldDef.GetType().FullName.StartsWith("GraphQL.Introspection") 
                            || fieldDef.HasPermission(RoleTypes.Anonymous)))
                            hasAnyAnynomousPermissions = true;

                        if (fieldsCount.Count == 0 && !hasAnyAnynomousPermissions)
                            context.ReportError(new ValidationError(
                                context.OriginalQuery,
                                "auth-required",
                                $"You are not authorized to run this query.",
                                fieldAst));
                    });
                }
            });
        }
    }
}
