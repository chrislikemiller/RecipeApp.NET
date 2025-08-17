using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Domain.Exceptions;

public class EntityAlreadyExistsException<T> : EntityException<T>
{
    private const string _message = "already exists";
    public EntityAlreadyExistsException(int entityId, string entityName = "") 
        : base(entityName, entityId, _message)
    {
    }
}

