using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conscience.Application.Graph;

namespace System.Linq.Dynamic
{
    public static class OrderByExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, Type sourceType, Dictionary<string, string> mappings, string defaultOrdernig)
        {
            IQueryable<T> result = source;

            var allowedFilters = new List<string>();

            if (!string.IsNullOrEmpty(ordering))
            {
                string checkedOrdering = string.Empty;

                foreach (var part in ordering.Split(','))
                {
                    var partToAdd = part;

                    var field = part.Trim().Split(' ').First();

                    //We check that the source type actually has this field exposed
                    if (sourceType.HasProperty(field))
                    {
                        allowedFilters.Add(field);

                        //If we have a mapping, we replace the path to the field on the source type to the path in the destiny type
                        if (mappings.Any(m => m.Key.ToLowerInvariant() == field.ToLowerInvariant()))
                        {
                            var mapping = mappings.FirstOrDefault(m => m.Key.ToLowerInvariant() == field.ToLowerInvariant());
                            partToAdd = part.Replace(field, mapping.Value);
                        }

                        if (!string.IsNullOrEmpty(checkedOrdering))
                            checkedOrdering += ", ";
                        checkedOrdering += partToAdd;
                    }
                }

                result = result.OrderBy(checkedOrdering);
            }

            var defaultField = defaultOrdernig.Trim().Split(' ').First();
            if (!allowedFilters.Any(f => f.ToLowerInvariant() == defaultField.ToLowerInvariant()))
                result = result.OrderBy(defaultOrdernig);

            return result;
        }
    }
}
