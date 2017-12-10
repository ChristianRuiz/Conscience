using Conscience.Application.Services;
using Conscience.DataAccess.Repositories;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities
{
    public abstract class ConscienceMutation : ObjectGraphType<object>
    {
        protected void ModifyCollection<T, R>(BaseRepository<R> repository, LogEntryService logService, ICollection<T> collection, ICollection<T> dbCollection, Action<T, T> setProperties, Func<T, T, bool> additionalPredicate = null)
            where T : IdentityEntity, new()
            where R : class
        {
            var itemsToRemove = dbCollection.Where(dbItem => !collection.Any(item => dbItem.Id == item.Id || (additionalPredicate != null && additionalPredicate(item, dbItem)))).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                logService.Log(null, $"Removed {typeof(T).Name} '{itemToRemove.ToString()}'");
                dbCollection.Remove(itemToRemove);
                repository.DeleteChild(itemToRemove);
            }

            foreach (var item in collection)
            {
                var dbItem = dbCollection.FirstOrDefault(dbIt => (item.Id != 0 && dbIt.Id == item.Id) || (additionalPredicate != null && additionalPredicate(item, dbIt)));
                var needsToBeAdded = dbItem == null;
                if (needsToBeAdded)
                {
                    dbItem = new T();
                    dbCollection.Add(dbItem);
                }

                setProperties(item, dbItem);
                
                if (needsToBeAdded)
                    logService.Log(null, $"Added {typeof(T).Name} '{dbItem.ToString()}'");
            }
        }
    }
}
