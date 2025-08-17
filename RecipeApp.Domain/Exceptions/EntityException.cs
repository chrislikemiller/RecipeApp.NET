using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Domain.Exceptions;
public abstract class EntityException<T> : Exception
{
    public EntityException(string entityName, int entityId, string message) : base($"{GetDisplayName(typeof(T))} {entityName} (ID: {entityId}) {message}")
    {
    }

    private static string GetDisplayName(Type thisType)
    {
        return thisType.Name.Replace("DTO", "");
    }
}

